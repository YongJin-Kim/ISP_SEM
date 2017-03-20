using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace SEC.Nanoeye.NanoImage.ScanItem
{
    /// <summary>
    /// Scan Item : SampleComposite, LineAverage, Blur
    /// </summary>
    internal class SI_SC_LA_Blur : IScanItem
    {
        #region Image를 저장하는 버퍼를
        private int sampleThreshold;

        protected IntPtr _imagedata = IntPtr.Zero;
        public IntPtr ImageData
        {
            get { return _imagedata; }
        }

        protected IntPtr _imagedata2 = IntPtr.Zero;
        public IntPtr ImageData2
        {
            get { return _imagedata2; }
        }

        private long _imgDataByteCnt = 0;
        private long _imgDataByteCnt2 = 0;

        protected IntPtr averageBuffer = IntPtr.Zero;
        private long _avrBufferByteCnt = 0;

        protected IntPtr averageBuffer2 = IntPtr.Zero;
        private long _avrBufferByteCnt2 = 0;

        private bool _IsDisposed = false;
        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        public string Name
        {
            get { return _Setting.Name; }
        }

        private int _scanningRequeset;
        public int ScanningRequest
        {
            get { return _scanningRequeset; }
            set
            {
                _scanningRequeset = value;
            }
        }

        private int _scanningScanned;
        public int ScanningScanned
        {
            get { return _scanningScanned; }
        }

        private int _scanningPersentage = 0;
        public int ScanningPersentage
        {
            get { return _scanningPersentage; }
        }

        protected SettingScanner _Setting;
        public SettingScanner Setting
        {
            get { return _Setting; }
        }

        DataAcquation.IDaqData _DaqData;
        public DataAcquation.IDaqData DaqData
        {
            get { return _DaqData; }
            set { _DaqData = value; }
        }



        #endregion

        #region Event
        public event ScanDataUpdateDelegate ScanLineUpdated;
        public event ScanDataUpdateDelegate FrameUpdated;
        public event EventHandler SettingChangedEvent;
        public event EventHandler Disposed;
        public event EventHandler Disposing;

        protected virtual void OnScanLineUpdated(int start, int lines)
        {
            if (ScanLineUpdated != null)
            {
                ScanLineUpdated(this, _Setting.Name, start, lines);
            }
        }

        protected virtual void OnFramUpdated(int start, int lines)
        {
            if (FrameUpdated != null)
            {
                FrameUpdated(this, _Setting.Name, start, lines);
            }
        }

        protected virtual void OnSettingChanged()
        {
            if (SettingChangedEvent != null)
            {
                SettingChangedEvent(this, EventArgs.Empty);
            }
        }

        protected virtual void OnDisposed()
        {
            if (Disposed != null) { Disposed(this, EventArgs.Empty); }
        }

        protected virtual void OnDisposing()
        {
            if (Disposing != null) { Disposing(this, EventArgs.Empty); }
        }
        #endregion

        #region 생성자 & 소멸자 & Dispose
        public SI_SC_LA_Blur(SettingScanner set, DataAcquation.IDaqData daqData)
        {
            _Setting = (SettingScanner)set.Clone();
            _DaqData = daqData;
        }

        ~SI_SC_LA_Blur()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_imagedata != IntPtr.Zero) { FreeGH(ref _imagedata); }
            if (_imagedata2 != IntPtr.Zero) { FreeGH(ref _imagedata2); }

            if (averageBuffer != IntPtr.Zero) { FreeGH(ref averageBuffer); }
            if (averageBuffer2 != IntPtr.Zero) { FreeGH(ref averageBuffer2); }
        }

        public override string ToString()
        {
            if (_Setting != null) { return "ScanItem-" + _Setting.Name; }
            else { return base.ToString(); }
        }
        #endregion

        #region Scanning
        /// <summary>
        /// 이미지 획득을 준비 한다.
        /// </summary>
        public void Ready()
        {
            //_imagedata = AllocGH(_Setting.ImageHeight * _Setting.ImageWidth, typeof(short), out _imgDataByteCnt);

            //averageBuffer = AllocGH(_Setting.FrameWidth * _Setting.FrameHeight, typeof(int), out _avrBufferByteCnt);
            if (_imagedata != IntPtr.Zero)
            {
                FreeGH(ref _imagedata);
            }

            if (_imagedata2 != IntPtr.Zero)
            {
                FreeGH(ref _imagedata2);
            }

             _imagedata = AllocGH(_Setting.ImageHeight * _Setting.ImageWidth, typeof(short), out _imgDataByteCnt);

            averageBuffer = AllocGH(_Setting.FrameWidth * _Setting.FrameHeight, typeof(int), out _avrBufferByteCnt);


            if (_DaqData.DualEnable)
            {
                _imagedata2 = AllocGH(_Setting.ImageHeight * _Setting.ImageWidth, typeof(short), out _imgDataByteCnt2);

                averageBuffer2 = AllocGH(_Setting.FrameWidth * _Setting.FrameHeight, typeof(int), out _avrBufferByteCnt2);
            }
            

            sampleThreshold = CalSampleThreshold(_Setting.AoClock, _Setting.SampleComposite, _Setting.LineAverage, _Setting.FrameWidth, _Setting.FrameHeight, 0.3F);
            Debug.WriteLine("Sample Threshold - " + sampleThreshold.ToString(), this.ToString());

            OnSettingChanged();
        }

        /// <summary>
        /// 이미지를 획득 한다.
        /// </summary>
        /// <param name="worker"></param>
        public unsafe void Scanning(BackgroundWorker worker)
        {
            int frameWidth = _Setting.FrameWidth;
            int frameHeight = _Setting.FrameHeight;

            int imgRectHeight = _Setting.ImageHeight;
            int imgRectWidth = _Setting.ImageWidth;
            int imgRectY = _Setting.ImageTop;



            int availableLines, remainLines, readLines, readSampleCount;
            short[,] inputs;

            int scanLineIndex = 0;

            int dualcount = 1;
            if (_DaqData.DualEnable)
            {
                dualcount = 2;
            }


            while (scanLineIndex < frameHeight)
            {
                if (worker.CancellationPending) { return; }

                #region 수집 대기

                int readcount = 0;
                while (_DaqData.ReadAvailables < sampleThreshold)
                {
                    //Thread.Sleep(1);
                    //Trace.WriteLine("Waiting~~~~ : " + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString());
                    //Trace.WriteLine("Daq Samples Read Count " + _DaqData.ReadAvailables.ToString());
                    Thread.Sleep(10);
                    readcount++;
                    //Trace.WriteLine("Read Count !!!!!!!!!!!!!!!!!!!!! ----- count : " + readcount.ToString());
                    if (readcount > 50 * dualcount)
                    {
                        //Trace.WriteLine("DAQ Reset !!!!!!!!!!!!!!!!!!!!! ----- count : " + readcount.ToString());
                        readcount = 0;
                        _DaqData.Reset();
                    }
                    
                    
                    if (worker.CancellationPending) { return; }
                }
                #endregion
                readcount = 0;
                //Debug.WriteLine("RA : " + _DaqData.ReadAvailables.ToString(), "SI_SC_LA");

                #region 샘플 수집
                // Daq로 부터 읽어 들일 수 있는 scan line 수
                availableLines = _DaqData.ReadAvailables / (frameWidth * _Setting.SampleComposite * _Setting.LineAverage);
                //availableLines = 120000 / (frameWidth * _Setting.SampleComposite * _Setting.LineAverage);
                //availableLines = _DaqData.ReadAvailables / (frameWidth * sampleComposite * _Setting.LineAverage);

                // 한 프레임을 완성하기 위해 남은 scan line 수
                remainLines = frameHeight - scanLineIndex;

                // 실제 읽어 들일 Scan Line 수
                readLines = (availableLines > remainLines) ? remainLines : 5;

                // 읽어 들일 sample 수
                readSampleCount = readLines * frameWidth * _Setting.SampleComposite * _Setting.LineAverage;
                //readSampleCount = readLines * frameWidth * sampleComposite * _Setting.LineAverage;

                //Debug.WriteLine("Req : " + readSampleCount.ToString(), "SI_SC_LA");

                inputs = _DaqData.Read(readSampleCount);
                //Trace.WriteLine("input Length : " + inputs.Length.ToString());
                if (worker.CancellationPending) { return; }
                #endregion

                int avr = (int)Math.Log(_scanningScanned, 2);

                ScanUpdate(inputs, readLines, scanLineIndex, Math.Min(_scanningScanned, _Setting.AverageLevel));


                scanLineIndex += readLines;
            }

            OnFramUpdated(0, _Setting.ImageHeight);

            if (_scanningScanned != int.MaxValue)
            {
                _scanningScanned++;
            }
        }

        /// <summary>
        /// 수집한 데이터에 여러 image processing을 적용하여 이미지 데이터로 변경 한다.
        /// </summary>
        /// <param name="inputs">수집된 데이터</param>
        /// <param name="readLines">읽어 들인 라인수</param>
        /// <param name="scanLineIndex">읽기 시작한 라인</param>
        /// <param name="average">평균화 레벨</param>
        protected unsafe virtual void ScanUpdate(short[,] inputs, int readLines, int scanLineIndex, int average)
        {
            unchecked
            {
                int framewidth = _Setting.FrameWidth;
                int temp;
                int val;

                int pnt = 0;
                int pnt1 = 0;
                int[] avrTemp = new int[readLines * framewidth * _Setting.LineAverage];
                


                #region SE
                fixed (int* pntAT = avrTemp)
                {
                    int* pAT = pntAT;
                    int* pAB = (int*)averageBuffer.ToPointer();

                    pAB = &pAB[scanLineIndex * framewidth];

                    #region with LineAverage
                    // 수집한 샘플에 샘플 합성과 blur, line-average 를 적용 한다.
                    for (int line = 0; line < readLines; line++)
                    {

                        for (int sam = 0; sam < framewidth * _Setting.LineAverage; sam++)
                        {
                            *pAT = 0;

                            for (int j = 0; j < _Setting.SampleComposite; j++)
                            {
                                *pAT += inputs[0, pnt++];

                            }
                            *pAT++ /= _Setting.SampleComposite;

                        }

                        pAT = pntAT;

                        int tempPnt;
                        // Line Average & Frame Average
                        for (int avr = 0; avr < framewidth; avr++)
                        {
                            temp = 0;
                            tempPnt = avr;
                            for (int i = 0; i < _Setting.LineAverage; i++)
                            {
                                temp += pAT[tempPnt];
                                tempPnt += framewidth;

                                //temp += pAT[i * framewidth + avr];
                            }
                            temp /= _Setting.LineAverage;

                            val = *pAB;
                            // 프레임 평균화
                            // 산술 연산자가 shift 연산자보다 먼저 계산된다!!!
                            //*pAB++ = val - (val >> average) + (temp >> average);
                            //*pAB++ = ((val << average) - average + (temp << average)) >> average;
                            *pAB++ = val - (val >> average) + (temp << (16 - average));
                        }
                    #endregion
                        Bluring(line + scanLineIndex, average);
                    }


                }
                #endregion

                if (_DaqData.DualEnable)
                {
                    int[] avrTempB = new int[readLines * framewidth * _Setting.LineAverage];
                    #region BSE
                    pnt = 0;
                    fixed (int* pntAT = avrTempB)
                    {
                        int* pAT = pntAT;
                        int* pAB = (int*)averageBuffer2.ToPointer();

                        pAB = &pAB[scanLineIndex * framewidth];

                        #region with LineAverage
                        // 수집한 샘플에 샘플 합성과 blur, line-average 를 적용 한다.
                        for (int line = 0; line < readLines; line++)
                        {

                            for (int sam = 0; sam < framewidth * _Setting.LineAverage; sam++)
                            {
                                *pAT = 0;

                                for (int j = 0; j < _Setting.SampleComposite; j++)
                                {
                                    *pAT += inputs[1, pnt++];

                                }
                                *pAT++ /= _Setting.SampleComposite;

                            }
                            pAT = pntAT;

                            int tempPnt;
                            // Line Average & Frame Average
                            for (int avr = 0; avr < framewidth; avr++)
                            {
                                temp = 0;
                                tempPnt = avr;
                                for (int i = 0; i < _Setting.LineAverage; i++)
                                {
                                    temp += pAT[tempPnt];
                                    tempPnt += framewidth;

                                    //temp += pAT[i * framewidth + avr];
                                }
                                temp /= _Setting.LineAverage;

                                val = *pAB;
                                // 프레임 평균화
                                // 산술 연산자가 shift 연산자보다 먼저 계산된다!!!
                                //*pAB++ = val - (val >> average) + (temp >> average);
                                //*pAB++ = ((val << average) - average + (temp << average)) >> average;
                                *pAB++ = val - (val >> average) + (temp << (16 - average));
                            }

                        #endregion
                            Bluring(line + scanLineIndex, average);
                        }

                    }
                    #endregion
                }









                #region 기존 Dual
                //fixed (int* pntAT = avrTemp)
                //{
                //    int* pAT = pntAT;
                //    int* pAB = (int*)averageBuffer.ToPointer();

                //    pAB = &pAB[scanLineIndex * framewidth];

                //    #region without LineAverage
                //    // 수집한 샘플에 샘플 합성과 blur를 적용 한다.
                //    //for(int line = 0; line < readLines; line++)
                //    //{

                //    //    // 샘플 합성
                //    //    for(int sam = 0; sam < framewidth; sam++)
                //    //    {
                //    //        temp = 0;

                //    //        for(int j = 0; j < _Setting.SampleComposite; j++)
                //    //        {
                //    //            temp += inputs[0, pnt++];
                //    //        }
                //    //        temp /= _Setting.SampleComposite;

                //    //        val = *pAB;
                //    //        // 프레임 평균화
                //    //        // 산술 연산자가 shift 연산자보다 먼저 계산된다!!!
                //    //        *pAB++ = val - (val >> average) + (temp >> average);
                //    //    }

                //    //    Bluring(line + scanLineIndex, average);
                //    //}
                //    #endregion

                //    #region with LineAverage
                //    // 수집한 샘플에 샘플 합성과 blur, line-average 를 적용 한다.
                //    for (int line = 0; line < readLines; line++)
                //    {
                //        // 샘플 합성
                //        if (_DaqData.DualEnable)
                //        {
                //            int imggap = 120;

                //            for (int sam = 0; sam < (framewidth + imggap) * _Setting.LineAverage / 2; sam++)
                //            {
                //                *pAT = 0;

                //                for (int j = 0; j < _Setting.SampleComposite; j++)
                //                {
                //                    *pAT += inputs[0, pnt++];

                //                }
                //                *pAT++ /= _Setting.SampleComposite;

                //            }

                //            if (line == 0)
                //            {
                //                pnt = 0;
                //            }
                //            else
                //            {
                //                pnt += (framewidth * _Setting.LineAverage * _Setting.SampleComposite * line) - pnt;
                //            }



                //            pnt1 += _Setting.ImageLeft * _Setting.SampleComposite;
                //            for (int sam = 0; sam < framewidth * _Setting.LineAverage / 2; sam++)
                //            {
                //                *pAT = 0;




                //                for (int j = 0; j < _Setting.SampleComposite; j++)
                //                {
                //                    *pAT += inputs[1, pnt1++];

                //                }
                //                *pAT++ /= _Setting.SampleComposite;

                //            }

                //            pnt1 = pnt;

                //        }
                //        else
                //        {
                //            for (int sam = 0; sam < framewidth * _Setting.LineAverage; sam++)
                //            {
                //                *pAT = 0;

                //                for (int j = 0; j < _Setting.SampleComposite; j++)
                //                {
                //                    *pAT += inputs[0, pnt++];

                //                }
                //                *pAT++ /= _Setting.SampleComposite;

                //            }



                //        }

                //        pAT = pntAT;

                //        int tempPnt;
                //        // Line Average & Frame Average
                //        for (int avr = 0; avr < framewidth; avr++)
                //        {
                //            temp = 0;
                //            tempPnt = avr;
                //            for (int i = 0; i < _Setting.LineAverage; i++)
                //            {
                //                temp += pAT[tempPnt];
                //                tempPnt += framewidth;

                //                //temp += pAT[i * framewidth + avr];
                //            }
                //            temp /= _Setting.LineAverage;

                //            val = *pAB;
                //            // 프레임 평균화
                //            // 산술 연산자가 shift 연산자보다 먼저 계산된다!!!
                //            //*pAB++ = val - (val >> average) + (temp >> average);
                //            //*pAB++ = ((val << average) - average + (temp << average)) >> average;
                //            *pAB++ = val - (val >> average) + (temp << (16 - average));
                //        }



                //        Bluring(line + scanLineIndex, average);



                //    }
                //    #endregion
                //}
                #endregion

            }

            // 업데이트는 한줄 위에서 부터 시작 해서 마지막 라인은 뺀다.
            // 그 이유는 Bluring시 가장 마지막에 업데이트된 라인은 Bluring을 할 수 없어서 실제 이미지에는 업데이트가 안되었기 때문이다.

            int imgTop = _Setting.ImageTop;
            int imgBottom = _Setting.ImageHeight + _Setting.ImageTop;
            int imgLeft = _Setting.ImageLeft;
            int imgRight = _Setting.ImageLeft + _Setting.ImageWidth;
            int imgWidth = _Setting.ImageWidth;
            int imgHeight = _Setting.ImageHeight;

            // 실제적으로 이미지가 있는 라인 수를 계산
            int realReadLines = 0;
            int realStart = 0;

            if (scanLineIndex < imgTop)
            { // 이미지 위에서 시작 하는 경우
                if ((scanLineIndex + readLines) <= imgTop)
                { // 이미지 전 또는 이미지 첫 줄에 끝나는 경우.(첫줄은 bluring때문에 안됨.)
                }
                else if ((scanLineIndex + readLines) > (imgBottom))
                { // 이미지 밑에서 끝나는 경우

                    // 전체 이미지를 업데이트 한다.
                    OnScanLineUpdated(0, imgHeight);
                }
                else
                { // 이미지내 에서 끝나는 경우

                    // 가장 마지막 줄은 Bluring이 되지 않아 업데이트 되지 않았다.
                    realReadLines = readLines - (_Setting.ImageTop - scanLineIndex);

                    OnScanLineUpdated(0, realReadLines);
                }
            }
            else if (scanLineIndex > imgBottom)
            { // 이미지 밑에서 시작 하는 경우
                realReadLines = 0;
            }
            else
            { // 이미지 중간에서 시작 하는 경우

                realStart = scanLineIndex - imgTop;

                if ((scanLineIndex + readLines) >= (imgBottom))
                { // 이미지 밑에서 끝나는 경우
                    realReadLines = readLines - ((scanLineIndex + readLines) - imgBottom);
                }
                else
                {
                    // Bluring에 의해 마지막 라인은 업데이트 되지 않았다.
                    realReadLines = readLines - 1;
                }

                // Bluring에 의해 이전 업데이트에서 빠진 라인을 그리도록 알려 준다.
                if (realStart > 0)
                {
                    realStart--;
                    realReadLines++;
                }
                if (realReadLines > 0)
                {
                    OnScanLineUpdated(realStart, realReadLines);
                }
            }
        }

        // bluring를 수행 한다.
        protected unsafe virtual void Bluring(int line, int average)
        {
            int imgTop = _Setting.ImageTop;
            int imgBottom = _Setting.ImageHeight + _Setting.ImageTop;
            int imgLeft = _Setting.ImageLeft;
            int imgRight = _Setting.ImageLeft + _Setting.ImageWidth;
            int imgWidth = _Setting.ImageWidth;
            int imgHeight = _Setting.ImageHeight;

            // bluring은 현재 스캔된 것의 위에 줄에 적용한다.

            // bluring을 할 수 있는 이미지 영역 밖이다.
            if (line <= imgTop) { return; }
            if (line > imgBottom) { return; }

            // 첫번째 줄, 두번째 줄은 bluring을 할 수 없다.
            // 따라서 단순 복사만을 해 둔다.
            if (line < 2)
            {
                short* pID = (short*)_imagedata.ToPointer();
                int* pAB = (int*)averageBuffer.ToPointer();
                pID = &pID[(line - 1) * imgWidth];
                pAB = &pAB[(line - 1) * _Setting.FrameWidth + imgLeft];

                for (int x = 0; x < imgWidth; x++) { *pID++ = (short)(*pAB++); }

                return;
            }

            // 이미지 영역 밑으로 버리는 영역이 없다.
            // 따라서 마지막 라인을 단순 복사해 둔다.
            if (line == _Setting.FrameHeight - 1)
            {

                short* pID = (short*)_imagedata.ToPointer();

                int* pAB = (int*)averageBuffer.ToPointer();

                pID = &pID[line * imgWidth];

                pAB = &pAB[line * _Setting.FrameWidth + imgLeft];

                for (int x = 0; x < imgWidth; x++)
                {
                    *pID++ = (short)(*pAB++);
                }

                if (_DaqData.DualEnable)
                {
                    short* pID2 = (short*)_imagedata2.ToPointer();
                    int* pAB2 = (int*)averageBuffer2.ToPointer();
                    pID2 = &pID2[line * imgWidth];
                    pAB2 = &pAB2[line * _Setting.FrameWidth + imgLeft];

                    for (int x = 0; x < imgWidth; x++)
                    {
                        *pID2++ = (short)(*pAB2++);
                    }
                }

            }

            // bluring되는 영역이다.
            int start = imgLeft;
            int end = imgRight;
            // 이미지 왼쪽으로 버리는 영역이 없는 경우 이다.
            if (start == 0)
            {
                start = 1;

                //_imagedata[line - 1 - _imageRectangle.Top, 0] = (short)averageBuffer[line - 1, 0];

                short* pID = (short*)_imagedata.ToPointer();
                int* pAB = (int*)averageBuffer.ToPointer();
                pID[(line - 1 - imgTop) * imgWidth] = (short)(pAB[(line - 1) * _Setting.FrameWidth]);

                if (_DaqData.DualEnable)
                {
                    short* pID2 = (short*)_imagedata2.ToPointer();
                    int* pAB2 = (int*)averageBuffer2.ToPointer();
                    pID2[(line - 1 - imgTop) * imgWidth] = (short)(pAB2[(line - 1) * _Setting.FrameWidth]);
                }




            }

            // 이미지 오른쪽으로 버리는 영역이 없는 경우 이다.
            if ((imgRight) == _Setting.FrameWidth)
            {
                end -= 1; // 루프문에서는 작다로 계산하므로, 미리 1을 뺀다.

                short* pID = (short*)_imagedata.ToPointer();
                int* pAB = (int*)averageBuffer.ToPointer();

                pID[(line - 1 - _Setting.ImageTop) * imgWidth + end - imgLeft] = (short)(pAB[(line - 1) * _Setting.FrameWidth + end - imgLeft]);

                if (_DaqData.DualEnable)
                {
                    short* pID2 = (short*)_imagedata2.ToPointer();
                    int* pAB2 = (int*)averageBuffer2.ToPointer();
                    pID2[(line - 1 - _Setting.ImageTop) * imgWidth + end - imgLeft] = (short)(pAB2[(line - 1) * _Setting.FrameWidth + end - imgLeft]);
                }



            }

            {
                short* pID = (short*)_imagedata.ToPointer();
                int* pAB1 = (int*)averageBuffer.ToPointer();
                int* pAB2 = (int*)averageBuffer.ToPointer();
                int* pAB3 = (int*)averageBuffer.ToPointer();
                int* pAB4 = (int*)averageBuffer.ToPointer();
                int* pAB5 = (int*)averageBuffer.ToPointer();
                int* pAB6 = (int*)averageBuffer.ToPointer();
                int* pAB7 = (int*)averageBuffer.ToPointer();
                int* pAB8 = (int*)averageBuffer.ToPointer();
                int* pAB9 = (int*)averageBuffer.ToPointer();

                int frameWidth = _Setting.FrameWidth;
                int blur = _Setting.BlurLevel;

                pID = &pID[(line - 1 - imgTop) * imgWidth + start - imgLeft];
                pAB1 = &pAB1[(line - 2) * frameWidth + start - 1];
                pAB2 = &pAB2[(line - 2) * frameWidth + start];
                pAB3 = &pAB3[(line - 2) * frameWidth + start + 1];
                pAB4 = &pAB4[(line - 1) * frameWidth + start - 1];
                pAB5 = &pAB5[(line - 1) * frameWidth + start];
                pAB6 = &pAB6[(line - 1) * frameWidth + start + 1];
                pAB7 = &pAB7[(line) * frameWidth + start - 1];
                pAB8 = &pAB8[(line) * frameWidth + start];
                pAB9 = &pAB9[(line) * frameWidth + start + 1];


                long lTemp;

                // x좌표는 averageBuffer 기준 이다.
                for (int x = start; x < end; x++)
                {
                    //lTemp = Median(pAB1,pAB4,pAB7) * 32;	// 이곳이 실제 bluring이 적용 되는 곳이다.
                    //
                    //pAB5++;

                    lTemp = (long)(*pAB5++) * 32;

                    lTemp += (long)(*pAB1++) * blur;
                    lTemp += (long)(*pAB2++) * blur;
                    lTemp += (long)(*pAB3++) * blur;

                    lTemp += (long)(*pAB4++) * blur;
                    //lTemp += (long)(*pAB5++) * 32;	// 이곳이 실제 bluring이 적용 되는 곳이다.
                    lTemp += (long)(*pAB6++) * blur;

                    lTemp += (long)(*pAB7++) * blur;
                    lTemp += (long)(*pAB8++) * blur;
                    lTemp += (long)(*pAB9++) * blur;

                    *pID++ = (short)((lTemp / (32 + blur * 8)) >> 16);

                }

                if (_DaqData.DualEnable)
                {
                    short* pID2 = (short*)_imagedata2.ToPointer();
                    int* pAB12 = (int*)averageBuffer2.ToPointer();
                    int* pAB22 = (int*)averageBuffer2.ToPointer();
                    int* pAB32 = (int*)averageBuffer2.ToPointer();
                    int* pAB42 = (int*)averageBuffer2.ToPointer();
                    int* pAB52 = (int*)averageBuffer2.ToPointer();
                    int* pAB62 = (int*)averageBuffer2.ToPointer();
                    int* pAB72 = (int*)averageBuffer2.ToPointer();
                    int* pAB82 = (int*)averageBuffer2.ToPointer();
                    int* pAB92 = (int*)averageBuffer2.ToPointer();

                    pID2 = &pID2[(line - 1 - imgTop) * imgWidth + start - imgLeft];
                    pAB12 = &pAB12[(line - 2) * frameWidth + start - 1];
                    pAB22 = &pAB22[(line - 2) * frameWidth + start];
                    pAB32 = &pAB32[(line - 2) * frameWidth + start + 1];
                    pAB42 = &pAB42[(line - 1) * frameWidth + start - 1];
                    pAB52 = &pAB52[(line - 1) * frameWidth + start];
                    pAB62 = &pAB62[(line - 1) * frameWidth + start + 1];
                    pAB72 = &pAB72[(line) * frameWidth + start - 1];
                    pAB82 = &pAB82[(line) * frameWidth + start];
                    pAB92 = &pAB92[(line) * frameWidth + start + 1];

                    lTemp = 0;
                    for (int x = start; x < end; x++)
                    {
                        lTemp = (long)(*pAB52++) * 32;

                        lTemp += (long)(*pAB12++) * blur;
                        lTemp += (long)(*pAB22++) * blur;
                        lTemp += (long)(*pAB32++) * blur;

                        lTemp += (long)(*pAB42++) * blur;
                        //lTemp += (long)(*pAB5++) * 32;	// 이곳이 실제 bluring이 적용 되는 곳이다.
                        lTemp += (long)(*pAB62++) * blur;

                        lTemp += (long)(*pAB72++) * blur;
                        lTemp += (long)(*pAB82++) * blur;
                        lTemp += (long)(*pAB92++) * blur;

                        *pID2++ = (short)((lTemp / (32 + blur * 8)) >> 16);
                    }
                }







            }
        }

        private unsafe long Median(int* pAB1, int* pAB4, int* pAB7)
        {
            long[] value = new long[9];
            value[0] = *pAB1;
            value[1] = *(pAB1 + 1);
            value[2] = *(pAB1 + 2);
            value[3] = *pAB4;
            value[4] = *(pAB4 + 1);
            value[5] = *(pAB4 + 2);
            value[6] = *pAB7;
            value[7] = *(pAB7 + 1);
            value[8] = *(pAB7 + 2);

            Array.Sort(value);
            return value[4];
        }
        #endregion

        /// <summary>
        /// 최소 샘플 수집수
        /// </summary>
        /// <param name="sampleClock"></param>
        /// <param name="_sampleComposite"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="minimumPeriod">최소 시간(msec)</param>
        /// <returns></returns>
        private int CalSampleThreshold(double sampleClock, int _sampleComposite, int lineavr, int frameWidth, int frameHeight, float minimumPeriod)
        {
            int frameLength = frameWidth * frameHeight;

            //_sampleComposite = _sampleComposite * _SamplingTime;

            // 1 프레임을 갱신하는데 걸리는 시간
            double framePeriod = frameLength * _sampleComposite * lineavr / sampleClock;

            if (framePeriod < minimumPeriod)
            {	// 1 프레임 단위로 업데이트
                return frameLength * _sampleComposite * lineavr;
            }

            //int lines = (int)(framePeriod / minimumPeriod);
            int lines = (int)(minimumPeriod * frameHeight / framePeriod);
            if (lines < 1) { lines = 1; }
            return (int)(lines * frameWidth * lineavr * _sampleComposite);
        }

        public static void ValidateSetting(SettingScanner setting)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            System.Text.StringBuilder arg = new System.Text.StringBuilder();

            if (setting.AverageLevel > 7)
            {
                setting.AverageLevel = 0;
                msg.AppendLine("AverageLevel is large then 7.");
                arg.AppendLine("AverageLevel");
            }

            if (setting.BlurLevel > 31)
            {
                setting.BlurLevel = 0;
                msg.AppendLine("BlurLevel is large then 31.");
                arg.AppendLine("BlurLevel");
            }

            if (setting.FrameHeight < 1)
            {
                setting.FrameHeight = 240;
                msg.AppendLine("FrameHeight is small then 1.");
                arg.AppendLine("FrameHeight");
            }

            if (setting.FrameWidth < 1)
            {
                setting.FrameWidth = 320;
                msg.AppendLine("FrameWidth is small then 1.");
                arg.AppendLine("FrameWidth");
            }

            if ((setting.ImageHeight < 1) || (setting.ImageHeight > setting.FrameHeight))
            {
                setting.ImageHeight = setting.FrameHeight;
                msg.AppendLine("ImageHeight is invalid.");
                arg.AppendLine("ImageHeight");
            }

            if ((setting.ImageWidth < 1) || (setting.ImageWidth > setting.FrameWidth))
            {
                setting.ImageWidth = setting.FrameWidth;
                msg.AppendLine("ImageWidth is invalid.");
                arg.AppendLine("ImageWidth");
            }

            if ((setting.ImageTop + setting.ImageHeight) > setting.FrameHeight)
            {
                setting.ImageTop = 0;
                msg.AppendLine("ImageTop is invalid.");
                arg.AppendLine("ImageTop");
            }

            if ((setting.ImageLeft + setting.ImageWidth) > setting.FrameWidth)
            {
                setting.ImageLeft = 0;
                msg.AppendLine("ImageLeft is invalid.");
                arg.AppendLine("ImageLeft");
            }

            if (setting.LineAverage < 1)
            {
                setting.LineAverage = 1;
                msg.AppendLine("LineAverage is samller then 1.");
                arg.AppendLine("LineAverage");
            }

            if ((setting.PaintWidth < 0.1) || (setting.PaintWidth > 1))
            {
                setting.PaintWidth = 1;
                msg.AppendLine("PaintWidth must be bettwen from 0.1 to 1.");
                arg.AppendLine("PaintWidth");
            }

            if ((setting.PaintHeight < 0.1) || (setting.PaintHeight > 1))
            {
                setting.PaintHeight = 1;
                msg.AppendLine("PaintHeight must be bettwen from 0.1 to 1.");
                arg.AppendLine("PaintHeight");
            }

            if ((Math.Abs(setting.PaintX) + Math.Abs(setting.PaintWidth)) > 1)
            {
                setting.ShiftX = 0;
                msg.AppendLine("ShiftX is invalid number.");
                arg.AppendLine("ShiftX");
            }

            if ((Math.Abs(setting.PaintY) + Math.Abs(setting.PaintHeight)) > 1)
            {
                setting.PaintY = 0;
                msg.AppendLine("PaintY is invalid number.");
                arg.AppendLine("PaintY");
            }

            if (setting.PropergationDelay < 0)
            {
                setting.PropergationDelay = 0;
                msg.AppendLine("PropergationDelay is smaller then 0.");
                arg.AppendLine("PropergationDelay");
            }

            if (setting.SampleComposite < 1)
            {
                setting.SampleComposite = 1;
                msg.AppendLine("SampleComposite is smaller then 1.");
                arg.AppendLine("SampleComposite");
            }

            if (msg.Length > 0)
            {
                throw new ArgumentException(msg.ToString(), arg.ToString());
            }
        }

        #region Global Heap 관리
        private unsafe IntPtr AllocGH(int size, Type dataType, out long byteCnts)
        {
            Trace.Assert(size > 0);

            IntPtr result = IntPtr.Zero;

            if (dataType == typeof(byte)) { byteCnts = size * sizeof(byte); }
            else if (dataType == typeof(short)) { byteCnts = size * sizeof(short); }
            else if (dataType == typeof(int)) { byteCnts = size * sizeof(int); }
            else { throw new ArgumentException("지원 하지 않는 데이타 타입", "dataType"); }

            SEC.GenericSupport.GHeapManager.Alloc(ref result, (int)byteCnts, this.ToString());

            byte* p = (byte*)result.ToInt32();

            for (int i = 0; i < byteCnts; i++)
            {
                *p++ = 0;
            }

            return result;
        }

        private void FreeGH(ref IntPtr buffer)
        {
            Trace.Assert(buffer != IntPtr.Zero);
            //Trace.Assert(size > 0);

            
            SEC.GenericSupport.GHeapManager.Free(ref buffer, this.ToString());
            
        }
        #endregion
    }
}
