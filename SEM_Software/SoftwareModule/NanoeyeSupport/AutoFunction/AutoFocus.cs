using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SECtype = SEC.GenericSupport.DataType;

using SECcolumn = SEC.Nanoeye.NanoColumn;
using SECimage = SEC.Nanoeye.NanoImage;

using Kikwak.AutoFunctionCollection;

namespace SEC.Nanoeye.Support.AutoFunction
{
    public class AutoFocus : AutoFunctionBase
    {
        #region Property & Variables
        private SECimage.IScanItemEvent scanItem;
        private SECtype.ITable focusTable;
        private SECtype.IControlDouble focusValue;
        private SECcolumn.Lens.IWDSplineObjBase iwdsob;


        private int _FrameDiscard = 1;
        public int FrameDiscard
        {
            get { return _FrameDiscard; }
            set { _FrameDiscard = value; }
        }

        private int _FrameAverage = 3;
        public int FrameAverage
        {
            get { return _FrameAverage; }
            set { _FrameAverage = value; }
        }
        #endregion

        #region Event
        public event EventHandler ProcessStepChanged;
        protected virtual void OnProcessStepChanged()
        {
            if (ProcessStepChanged != null) { ProcessStepChanged(this, EventArgs.Empty); }
        }
        #endregion

        #region Override Method
        public override void Stop()
        {
            //scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
            //scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchRange_FrameUpdated);
            //scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchTable_FrameUpdated);
            scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchDynamic_FrameUpdated);
            OnProgressComplet();
        }

        public override void Cancel()
        {
            throw new NotSupportedException();
        }

        protected override void OnProgressComplet()
        {
            Debug.WriteLine("AutoFocus Complete");
            base.OnProgressComplet();
        }
        #endregion

        /// <summary>
        /// AutoFocus의 동작 모드
        /// </summary>
        public enum AutoFocusModeType
        {
            /// <summary>
            /// Table 탐색
            /// </summary>
            Table,
            /// <summary>
            /// Value의 전체 영역 탐색
            /// </summary>
            ValueRange,
            /// <summary>
            /// Value의 현재 값에서 가까운 영역 탐색
            /// </summary>
            ValueNear,
        }

        //public void SearchFocusValue(SECimage.IScanItemEvent isie,
        //                                SECtype.ITable ict,
        //                                SECtype.IControlDouble icd,
        //                                AutoFocusModeType autoFocusModeType,
        //                                int range)
        public void SearchFocusValue(SECimage.IScanItemEvent isie,
                                        SECcolumn.Lens.IWDSplineObjBase ict,
                                        SECtype.IControlDouble icd,
                                        AutoFocusModeType autoFocusModeType,
                                        int range)
        {
            //if (isie.Setting.ImageWidth != 256)
            //{
            //    throw new ArgumentException("Image Width must be 256.");
            //}
            //if (isie.Setting.ImageHeight != 256)
            //{
            //    throw new ArgumentException("Image Height must be 256.");
            //}

            _CancelVisiable = false;
            OnCancelVisiableChanged();
            _StopVisiable = true;
            OnStopVisiableChanged();


            scanItem = isie;
            //focusTable = ict;
            iwdsob = ict;
            focusValue = icd;

            freqSum = 0;

            switch (autoFocusModeType)
            {
                case AutoFocusModeType.ValueRange:
                    //SearchRange();
                    automodetype = AutoFocusModeType.ValueRange;
                    SearchDynamic();

                    break;
                case AutoFocusModeType.ValueNear:
                    //SearchNear(range);
                    automodetype = AutoFocusModeType.ValueNear;
                    SearchNear(range);
                    break;
                default:
                    throw new ArgumentException("Undefined AutoFocus Mode. " + autoFocusModeType.ToString(), "autoFocusModeType");
            }

            //SearchDynamic();
        }

        void autoVideo_ProgressChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void autoVideo_ProgressComplet(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region 탐색
        int frameCount;
        int[] freqFilter;
        short[][] frameBuffer;
        double freqSum = 0.0;

        private AutoFocusModeType automodetype;

        int dynamicIndex = 0;
        private int[] dynamicDivider = { 16, 6 };
        public int[] DynamicDivider
        {
            get { return dynamicDivider; }
            set { dynamicDivider = value; }
        }

        private double _NextGapUp = 1d;
        public double NextGapUp
        {
            get { return _NextGapUp; }
            set { _NextGapUp = value; }
        }

        private double _NextGapDown = 1d;
        public double NextGapDown
        {
            get { return _NextGapDown; }
            set { _NextGapDown = value; }
        }

        double dynamicModifier;

        SortedList<double, double> searchResult;

        int imgIndex;

        double focusMin;
        double focusMax;

        int totalProcess;
        object[,] WDtable;

        private void SearchDynamic()
        {
            System.Diagnostics.Trace.WriteLine("SearchDynamic Start - FA " + _FrameAverage.ToString(), "AutoFocus");

            dynamicIndex = 0;

            //freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);
            freqFilter = AutoFocusHelper.InitializeFreqFilter5(256, 256, 1f);
            //freqFilter = AutoFocusHelper.InitializeFreqFilter(256, 256, 0.1f, 0.7f);

            totalProcess = 0;
            foreach (int i in dynamicDivider)
            {
                totalProcess += i * (_FrameAverage + _FrameDiscard);
        }


            double max = 0;
            double min = 0;

            WDtable = iwdsob.TableGet();

            if (WDtable.Length < 6)
            {
                min = focusValue.Minimum;
                max = focusValue.Maximum;
            }
            else
            {
                min = ((int)(WDtable[1, 1]) - (int)(WDtable[0, 1])) / 10.0;
                max = ((int)(WDtable[4, 1]) - (int)(WDtable[3, 1])) / 5.0;



                min = ((int)(WDtable[0, 1]) - min) / 4095;
                max = ((int)(WDtable[4, 1]) + max) / 4095;
            }
            

            //SearchDynamicInner(focusValue.Maximum, focusValue.Minimum);
            SearchDynamicInner(max, min);
        }

        private void SearchNear(int range)
        {
            dynamicIndex = 0;

            //freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);
            freqFilter = AutoFocusHelper.InitializeFreqFilter5(256, 256, 1f);
            //freqFilter = AutoFocusHelper.InitializeFreqFilter(256, 256, 0.1f, 0.7f);

            totalProcess = 0;
            foreach (int i in dynamicDivider)
            {
                totalProcess += i * (_FrameAverage + _FrameDiscard);
            }



            double min = focusValue.Minimum;
            double max = focusValue.Maximum;

            SearchDynamicInner(max, min);
        }

        private void SearchDynamicInner(double max, double min)
        {
            if (dynamicIndex == dynamicDivider.Length)
            {
                Stop();
                return;
            }

            OnProcessStepChanged();

            max = Math.Min(max, focusValue.Maximum);
            min = Math.Max(min, focusValue.Minimum);
            focusMin = min;
            focusMax = max;

            dynamicModifier = (max - min) / dynamicDivider[dynamicIndex];
            dynamicIndex++;

            frameCount = 0;
            imgIndex = 0;

            searchResult = new SortedList<double, double>();

            frameBuffer = new short[_FrameAverage][];

            //DebugginLog.Instance.Show(); 

            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchDynamic_FrameUpdated);
        }

        void SearchDynamic_FrameUpdated(object sender, string name, int startline, int lines)
        {
            frameCount++;

            _Progress = 0;
            for (int i = 0; i < dynamicIndex - 1; i++)
            {
                _Progress += dynamicDivider[i] * (_FrameAverage + _FrameDiscard);
            }

            _Progress += imgIndex * (_FrameAverage + _FrameDiscard);
            _Progress += frameCount;
            _Progress = _Progress * 100 / totalProcess;

            _Progress = Math.Min(100, _Progress);

            OnProgressChanged();

            // 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
            if (frameCount < _FrameDiscard + 1) { return; }
            if (frameCount > _FrameAverage + _FrameDiscard) { return; }

            NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
            frameBuffer[frameCount - _FrameDiscard - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

            System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - _FrameDiscard - 1], 0, frameBuffer[frameCount - _FrameDiscard - 1].Length);

            // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
            if (frameCount == (_FrameAverage + _FrameDiscard))
            {



                double focusTarget;


                // 제곱 평균을 구한다.
                short[] img = AutoFocusHelper.MakeAvrImage(frameBuffer, _FrameAverage);

                img = AutoFocusHelper.HistogramEqual(img);
                float freqValue = AutoFocusHelper.GetFreqAmplValue2(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);
                //DebugginLog.Instance.AddString(string.Format("{0} - V {1} , F {2}", imgIndex, focusValue.Value, freqValue), "AutoFocus-Dynamic");
                Debug.WriteLine(string.Format("{0} - V {1} , F {2}", imgIndex, focusValue.Value, freqValue), "AutoFocus-Dynamic");

                freqSum += freqValue;



                try
                {
                    searchResult.Add(focusValue.Value, freqValue);

                }
                catch (Exception ex)
                {
                    //SearchDynamicBreak(isie);
                    SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                }

                imgIndex++;


                switch (imgIndex)
                {
                    case 1:	// 첫번째 프레임은 현재 값이 들어 있음.
                        focusTarget = focusMax;
                        break;
                    case 2:
                    // 초점값이 최대 일때 fft 값이 큰 경우는 무시. focus 범위에 따라 값이 잘못 나올 수 있음.
                    case 3:
                    case 4:
                        // 이미지 수집을 계속 한다.

                        focusTarget = focusValue.Value - dynamicModifier;
                        break;
                    case 0:
                        throw new ArgumentException("imgIndex가 0일 수 없다.");
                    default:
                        if (imgIndex > dynamicDivider[dynamicIndex - 1])
                        {
                            // 전체 범위 탐색 완료
                            SearchDynamicBreak(isie);
                            return;
                        }
                        //else
                        //{
                        //    // 탐색 중인 경우.
                        //    if (imgIndex > dynamicDivider[dynamicIndex - 1] / 4)
                        //    {
                        //        bool br = true;
                        //        for (int i = 0; i < dynamicDivider[dynamicIndex - 1] / 4; i++)
                        //        {
                        //            // 기울기가 바뀌었는지를 확인하여 탐색을 중단 시킨다.
                        //            if (searchResult.Values[i] > searchResult.Values[i + 1])
                        //            {
                        //                br = false;
                        //                break;
                        //            }
                        //        }

                        //        if (br)
                        //        {
                        //            SearchDynamicBreak(isie);
                        //            return;
                        //        }
                        //    }
                        //}




                        focusTarget = focusValue.Value - dynamicModifier;



                        break;
                }

                focusValue.Value = Math.Max(focusValue.Minimum, focusTarget);
                focusValue.Value = Math.Min(focusValue.Maximum, focusTarget);
                // 이미지 수집을 다시 시작한다.
                frameCount = 0;
            }
        }




        private void SearchDynamicBreak(NanoImage.IScanItemEvent isie)
        {
            isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchDynamic_FrameUpdated);

            double focus = 0.0;
           
        
            double fft = searchResult.Values[0];

            foreach (KeyValuePair<double, double> kvp in searchResult)
            {


                if (kvp.Value < fft)
                {
                    focus = kvp.Key;
                    fft = kvp.Value;
                }


            }




            if (focus == 0)
            {
                focus = searchResult.Keys[0];
            }


            if (focus == 1)
            {
                focus = searchResult.Keys[0];
            }


            System.Diagnostics.Debug.WriteLine("Result - " + focus.ToString(), "AutoFocus - Dynamic");
            focusValue.Value = focus;
            freqSum = 0;
            SearchDynamicInner(focus + dynamicModifier * _NextGapUp, focus - dynamicModifier * _NextGapDown);
            
        }



        //        #region Near 탐색
        //        SortedList<double, double> nearTable;
        //        int nearRange;
        //        #region Old
        ////        private void SearchNear(int range)
        ////        {


        ////            frameCount = 0;
        ////            frameBuffer = new short[_FrameAverage][];

        ////            nearTable = new SortedList<double, double>();

        ////            freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

        ////            nearRange = (int)Math.Min(range, (focusValue.Maximum - focusValue.Minimum) / focusValue.Precision / 4);

        ////            focusValue.Value = Math.Min(focusValue.Value + nearRange * focusValue.Precision * 1.5, focusValue.Maximum);

        ////            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
        ////        }

        ////        void SearchNear_FrameUpdated(object sender, string name, int startline, int lines)
        ////        {
        ////#if DEBUG
        ////            try
        ////            {
        ////#endif
        ////                frameCount++;
        ////                // 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
        ////                if (frameCount < _FrameDiscard + 1) { return; }
        ////                if (frameCount > _FrameAverage + _FrameDiscard) { return; }

        ////                NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
        ////                frameBuffer[frameCount - _FrameDiscard - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

        ////                System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - _FrameDiscard - 1], 0, frameBuffer[frameCount - _FrameDiscard - 1].Length);

        ////                // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
        ////                if (frameCount == _FrameAverage + _FrameDiscard)
        ////                {
        ////                    //int focus = (int)(focusValue.Value / focusValue.Precision);

        ////                    double focus	 = focusValue.Value;

        ////                    // 제곱 평균을 구한다.
        ////                    short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, _FrameAverage);

        ////                    float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

        ////                    Debug.WriteLine(string.Format("{0} index - {1} freqValue", focus, freqValue), "AutoFocus-Range");

        ////                    nearTable.Add(focusValue.Value, freqValue);

        ////                    frameCount = 0;

        ////                    double focusCen = (focusValue.Maximum + focusValue.Minimum) / 2;

        ////                    switch (nearTable.Count)
        ////                    {
        ////                    case 1:
        ////                    case 2:
        ////                    case 3:
        ////                        focusValue.Value -= nearRange * focusValue.Precision;
        ////                        break;
        ////                    default:
        ////                        {
        ////                            scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);

        ////                            int maxIndex = FindMaxValue(nearTable);
        ////                            //if (maxIndex == 0)
        ////                            //{
        ////                            //    double fVal = ((nearTable.Keys[maxIndex] + focusValue.Minimum) / 2);

        ////                            //    if (fVal == nearTable.Keys[maxIndex])
        ////                            //    {
        ////                            //        scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
        ////                            //        Stop();
        ////                            //        return;
        ////                            //    }
        ////                            //    else
        ////                            //    {
        ////                            //        focusValue.Value = fVal;


        ////                            //    }
        ////                            //}
        ////                            //else if (maxIndex == nearTable.Count - 1)
        ////                            //{
        ////                            //    double fVal = ((nearTable.Keys[maxIndex] + focusValue.Maximum) / 2);

        ////                            //    if (fVal == nearTable.Keys[maxIndex])
        ////                            //    {
        ////                            //        scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
        ////                            //        Stop();
        ////                            //        return;
        ////                            //    }
        ////                            //    else { focusValue.Value = fVal; }
        ////                            //}
        ////                            //else
        ////                            //{
        ////                            #region 여러번 탐색
        ////                                /*
        ////                                int secIndex = FindSecondValue(nearTable);

        ////                                float deltaA = (nearTable[maxIndex] - nearTable[maxIndex - 1]) / (nearTable.Keys[maxIndex] - nearTable.Keys[maxIndex - 1]);
        ////                                float deltaB = (nearTable[maxIndex + 1] - nearTable[maxIndex]) / (nearTable.Keys[maxIndex + 1] - nearTable.Keys[maxIndex]);

        ////                                if (Math.Sign(deltaA) != Math.Sign(deltaB))
        ////                                {
        ////                                    if (maxIndex > secIndex)
        ////                                    {
        ////                                        focusValue.Value = (nearTable.Keys[maxIndex] + nearTable.Keys[maxIndex+1]) * focusValue.Precision / 2;
        ////                                    }
        ////                                    else
        ////                                    {
        ////                                        focusValue.Value = (nearTable.Keys[maxIndex] + nearTable.Keys[maxIndex - 1]) * focusValue.Precision / 2;
        ////                                    }
        ////                                }
        ////                                else
        ////                                {
        ////                                    focusValue.Value = (nearTable.Keys[maxIndex] + nearTable.Keys[secIndex]) * focusValue.Precision / 2;
        ////                                }
        ////                                */
        ////                                #endregion

        ////                            #region Spline을 이용한 한번만 탐색

        ////                                //double fftVal = nearTable.Values[0];

        ////                                //double foR = nearTable.Keys[0];

        ////                                //for (double index = nearTable.Keys[0]; index < nearTable.Keys.Last(); index += focusValue.Precision)
        ////                                //{
        ////                                //    double calFFT = SEC.GenericSupport.Mathematics.Interpolation.Spline(nearTable, index);

        ////                                //    if (calFFT > fftVal)
        ////                                //    {
        ////                                //        foR = index;
        ////                                //    }
        ////                                //}

        ////                                //focusValue.Value = foR;

        ////                                //Stop();
        ////                                //return;
        ////                            #endregion

        ////                            #region Bezier을 이용한 한번만 탐색

        ////                            double fftVal = nearTable.Values[0];

        ////                            double foR = nearTable.Keys[0];

        ////                            for(double index = nearTable.Keys[0]; index < nearTable.Keys.Last(); index += focusValue.Precision)
        ////                            {
        ////                                double calFFT = SEC.GenericSupport.Mathematics.Interpolation.Bezier(nearTable, index);

        ////                                if(calFFT > fftVal)
        ////                                {
        ////                                    foR = index;
        ////                                    fftVal = calFFT;
        ////                                }
        ////                            }

        ////                            System.Diagnostics.Debug.WriteLine("Result - " + foR.ToString(), "AutoFocus - Range");
        ////                            focusValue.Value = foR;

        ////                            Stop();
        ////                            return;
        ////                            #endregion
        ////                            //}
        ////                        }
        ////                    }
        ////                    _Progress += 1;
        ////                    _Progress %= 2;
        ////                    _Progress *= 100;
        ////                    OnProgressChanged();
        ////                }
        ////#if DEBUG
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                throw new Exception("AutoFocus-Range Error", ex);
        ////            }
        ////#endif
        ////        }

        ////        private int FindMaxValue(SortedList<double, double> nearTable)
        ////        {
        ////            int result = 0;
        ////            double val = nearTable.Values[result];

        ////            foreach (KeyValuePair<double, double> kvp in nearTable)
        ////            {
        ////                if (kvp.Value > val)
        ////                {
        ////                    result = nearTable.IndexOfKey(kvp.Key);
        ////                    val = kvp.Value;
        ////                }
        ////            }

        ////            return result;
        ////        }
        //        #endregion

        //        private void SearchNear(int range)
        //        {
        //            frameCount = 0;
        //            frameBuffer = new short[_FrameAverage][];

        //            nearTable = new SortedList<double, double>();

        //            freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

        //            nearRange = (int)Math.Min(range, (focusValue.Maximum - focusValue.Minimum) / focusValue.Precision / 4);

        //            //focusValue.Value = Math.Min(focusValue.Value + nearRange * focusValue.Precision * 1.5, focusValue.Maximum);

        //            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
        //        }

        //        void SearchNear_FrameUpdated(object sender, string name, int startline, int lines)
        //        {
        //#if DEBUG
        //            try
        //            {
        //#endif
        //                frameCount++;
        //                // 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
        //                if (frameCount < _FrameDiscard + 1) { return; }
        //                if (frameCount > _FrameAverage + _FrameDiscard) { return; }

        //                NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
        //                frameBuffer[frameCount - _FrameDiscard - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

        //                System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - _FrameDiscard - 1], 0, frameBuffer[frameCount - _FrameDiscard - 1].Length);

        //                // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
        //                if (frameCount == _FrameAverage + _FrameDiscard)
        //                {
        //                    //int focus = (int)(focusValue.Value / focusValue.Precision);

        //                    double focus	 = focusValue.Value;

        //                    // 제곱 평균을 구한다.
        //                    short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, _FrameAverage);

        //                    float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

        //                    Trace.WriteLine(string.Format("{0} index - {1} freqValue", focus, freqValue), "AutoFocus-Range");

        //                    nearTable.Add(focus, freqValue);

        //                    frameCount = 0;

        //                    //double focusCen = (focusValue.Maximum + focusValue.Minimum) / 2;

        //                    switch (nearTable.Count)
        //                    {
        //                    case 1:
        //                        if (focusValue.Value >= focusValue.Maximum)
        //                        {
        //                            double val	 = focusValue.Value - nearRange * focusValue.Precision / 5;
        //                        }
        //                        else
        //                        {
        //                            double val = focusValue.Value + (nearRange * focusValue.Precision) / 2;
        //                            focusValue.Value = Math.Min(focusValue.Maximum, val);
        //                        }
        //                        break;
        //                    case 2:
        //                    case 3:
        //                    case 4:
        //                        focusValue.Value = Math.Max(focusValue.Minimum, focusValue.Value - nearRange * focusValue.Precision/4);
        //                        break;
        //                    default:
        //                        {

        //                            scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);

        //                            int maxIndex = FindMaxValue(nearTable);

        //                            #region Spline을 이용한 한번만 탐색

        //                            double fftVal = nearTable.Values[0];

        //                            double foR = nearTable.Keys[0];

        //                            for (double index = nearTable.Keys[0]; index < nearTable.Keys.Last(); index += focusValue.Precision)
        //                            {
        //                                double calFFT = SEC.GenericSupport.Mathematics.Interpolation.Bezier(nearTable, index);

        //                                System.Diagnostics.Trace.WriteLine("FFT - index :" + index.ToString() + ", val : " + calFFT.ToString(), "AutoFocus - Range");

        //                                if (calFFT > fftVal)
        //                                {
        //                                    foR = index;
        //                                    fftVal = calFFT;
        //                                    System.Diagnostics.Trace.WriteLine("Selected.", "AutoFocus - Range");
        //                                }
        //                            }

        //                            focusValue.Value = foR;

        //                            Stop();
        //                            return;
        //                            #endregion
        //                        }
        //                    }
        //                    _Progress = nearTable.Count * 20;

        //                    _Progress = Math.Min(99, _Progress);
        //                    OnProgressChanged();
        //                }
        //#if DEBUG
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("AutoFocus-Range Error", ex);
        //            }
        //#endif
        //        }

        //        private int FindMaxValue(SortedList<double, double> nearTable)
        //        {
        //            int result = 0;
        //            double val = nearTable.Values[result];

        //            foreach (KeyValuePair<double, double> kvp in nearTable)
        //            {
        //                if (kvp.Value > val)
        //                {
        //                    result = nearTable.IndexOfKey(kvp.Key);
        //                    val = kvp.Value;
        //                }
        //            }

        //            return result;
        //        }
        //    #endregion

        //        #region 범위 탐색
        //        int rangeCount;
        //        const int searchCountFirst = 9;
        //        const int searchCountSecond = 5;
        //        int rangeFrameCountMax;
        //        int rangeFrameCountNow;

        //        private void TSearchRange()
        //        {
        //            System.Diagnostics.Debug.WriteLine("SearchRange First Start", "AutoFocus");
        //            rangeCount = 0;
        //            frameCount = 0;
        //            frameBuffer = new short[_FrameAverage][];


        //            rangeFrameCountMax = (_FrameAverage + 3) * (searchCountFirst + searchCountSecond);
        //            rangeFrameCountNow = 0;

        //            freqAtObjectLL = new LinkedList<DoubleFloatStruct>();

        //            double coarseRange = focusValue.Maximum - focusValue.Minimum;
        //            for(int i  = searchCountFirst; i > 0; i--)
        //            {
        //                freqAtObjectLL.AddLast(new LinkedListNode<DoubleFloatStruct>(new DoubleFloatStruct(coarseRange * i / searchCountFirst + focusValue.Minimum, 0)));
        //            }

        //            freqAtObjectNode = freqAtObjectLL.First;

        //            focusValue.Value = freqAtObjectNode.Value.dValue;

        //            freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

        //            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(TSearchRange_FrameUpdated);
        //        }

        //        private void TSearchSecondRange()
        //        {
        //            System.Diagnostics.Debug.WriteLine("SearchRange Second Start", "AutoFocus");
        //            LinkedListNode<DoubleFloatStruct> dfsMaxNode = freqAtObjectLL.First;

        //            for(LinkedListNode<DoubleFloatStruct> node = freqAtObjectLL.First; node != null; node = node.Next)
        //            {
        //                if(node.Value.fValue > dfsMaxNode.Value.fValue)
        //                {
        //                    dfsMaxNode = node;
        //                }
        //            }

        //            System.Diagnostics.Debug.WriteLine(string.Format("SearchRange Second start Pnt {0} - focus, {1} - freq", dfsMaxNode.Value.dValue, dfsMaxNode.Value.fValue), "AutoFocus");

        //            double fMax, fMin;
        //            if(dfsMaxNode.Next == null) { fMin = dfsMaxNode.Value.dValue; }
        //            else { fMin = dfsMaxNode.Next.Value.dValue; }
        //            if(dfsMaxNode.Previous == null) { fMax = dfsMaxNode.Value.dValue; }
        //            else { fMax = dfsMaxNode.Previous.Value.dValue; }

        //            rangeCount = 1;
        //            frameCount = 0;
        //            frameBuffer = new short[_FrameAverage][];

        //            freqAtObjectLL = new LinkedList<DoubleFloatStruct>();

        //            double coarseRange = fMax - fMin;
        //            for (int i  = searchCountSecond ; i >= 0; i--)
        //            {
        //                freqAtObjectLL.AddLast(new LinkedListNode<DoubleFloatStruct>(new DoubleFloatStruct(coarseRange * i / searchCountSecond + fMin, 0)));
        //            }

        //            freqAtObjectNode = freqAtObjectLL.First;

        //            focusValue.Value = freqAtObjectNode.Value.dValue;

        //            //freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

        //            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchRange_FrameUpdated);
        //        }


        //        void TSearchRange_FrameUpdated(object sender, string name, int startline, int lines)
        //        {
        //            // 버리는 프레임 수.
        ////#if DEBUG
        ////            try
        ////            {
        ////#endif
        //                frameCount++;

        //                _Progress = (rangeFrameCountNow++ + 1) * 100 / rangeFrameCountMax;
        //                if (_Progress >= 100)
        //                {
        //                    _Progress = 99;
        //                }
        //                OnProgressChanged();

        //                // 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
        //                if (frameCount < _FrameDiscard + 1) { return; }
        //                if (frameCount > _FrameAverage + _FrameDiscard) { return; }

        //                NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
        //                frameBuffer[frameCount - _FrameDiscard - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

        //                System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - _FrameDiscard - 1], 0, frameBuffer[frameCount - _FrameDiscard - 1].Length);

        //                // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
        //                if (frameCount == (_FrameAverage + _FrameDiscard))
        //                {
        //                    LinkedListNode<DoubleFloatStruct> preNode = freqAtObjectNode;
        //                    freqAtObjectNode = freqAtObjectNode.Next;
        //                    if (freqAtObjectNode != null)
        //                    {
        //                        focusValue.Value = freqAtObjectNode.Value.dValue;
        //                    }

        //                    // 제곱 평균을 구한다.
        //                    short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, _FrameAverage);

        //                    float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

        //                    freqAtObjectLL.AddAfter(preNode, new DoubleFloatStruct(preNode.Value.dValue, freqValue));
        //                    freqAtObjectLL.Remove(preNode);

        //                    Debug.WriteLine(string.Format("{0} index - {1} freqvalue", preNode.Value.dValue, freqValue), "AutoFocus-Range");

        //                    // 모든 WD에 대한 검사를 마침.
        //                    if (freqAtObjectNode == null)
        //                    {
        //                        isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchRange_FrameUpdated);

        //                        //DoubleFloatStruct dfs = freqAtObjectLL.First.Value;

        //                        if(rangeCount == 0)
        //                        {
        //                            SearchSecondRange();
        //                        }
        //                        else
        //                        {
        //                            SortedList<double, double> focusDic = new SortedList<double, double>();
        //                            foreach(DoubleFloatStruct dfsNode in freqAtObjectLL)
        //                            {
        //                                focusDic.Add((int)(dfsNode.dValue / focusValue.Precision), dfsNode.fValue);
        //                            }

        //                            double focus = focusDic.Keys.First();
        //                            double fft= focusDic.Values.First();

        //                            int last = (int)focusDic.Keys.Last();

        //                            for(int i = (int)focus; i < last; i++)
        //                            {
        //                                double fftNew = SEC.GenericSupport.Mathematics.Interpolation.Bezier(focusDic, i);
        //                                if(fftNew > fft)
        //                                {
        //                                    fft = fftNew;
        //                                    focus = i;
        //                                }
        //                            }

        //                            focusValue.Value = focus * focusValue.Precision;

        //                            Stop();
        //                        }

        //                    }

        //                    frameCount = 0;
        //                }
        ////#if DEBUG
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                throw new Exception("AutoFocus-Range Error", ex);
        ////            }
        ////#endif
        //        }
        //        #endregion

        //        #region Table 탐색
        //        private void SearchTable()
        //        {


        //            frameCount = 0;
        //            focusTable.SelectedIndex = focusTable.IndexMinimum;
        //            frameBuffer = new short[_FrameAverage][];

        //            freqAtObjectLL = new LinkedList<DoubleFloatStruct>();

        //            freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

        //            scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchTable_FrameUpdated);
        //        }

        //        void SearchTable_FrameUpdated(object sender, string name, int startline, int lines)
        //        {
        //#if DEBUG
        //            try
        //            {
        //#endif
        //                frameCount++;
        //                NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;

        //                // 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
        //                if (frameCount < _FrameDiscard + 1) { return; }
        //                // 현재 계산중임.
        //                if (frameCount > _FrameAverage + _FrameDiscard) { return; }

        //                frameBuffer[frameCount - _FrameDiscard - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

        //                System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - _FrameDiscard - 1], 0, frameBuffer[frameCount - _FrameDiscard - 1].Length);

        //                // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
        //                if (frameCount == _FrameAverage + _FrameDiscard)
        //                {
        //                    int tableIndex = focusTable.SelectedIndex;

        //                    focusTable.SelectedIndex++;

        //                    // 제곱 평균을 구한다.
        //                    short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, _FrameAverage);

        //                    float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

        //                    freqAtObjectLL.AddLast(new DoubleFloatStruct(tableIndex, freqValue));

        //                    Debug.WriteLine(string.Format("{0} index - {1} freqvalue", tableIndex, freqValue), "AutoFocus-Table");

        //                    // 모든 WD에 대한 검사를 마침.
        //                    if (tableIndex == focusTable.SelectedIndex)
        //                    {
        //                        scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchTable_FrameUpdated);

        //                        DoubleFloatStruct dfs = freqAtObjectLL.First.Value;

        //                        foreach (DoubleFloatStruct dfsNode in freqAtObjectLL)
        //                        {
        //                            if (dfsNode.fValue > dfs.fValue)
        //                            {
        //                                dfs = dfsNode;
        //                            }
        //                        }

        //                        focusTable.SelectedIndex = (int)dfs.dValue;

        //                        Stop();
        //                    }

        //                    frameCount = 0;
        //                }
        //#if DEBUG
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("AutoFocus-Table Error", ex);
        //            }
        //#endif
        //        }
        //        #endregion

        #endregion
    }
}
