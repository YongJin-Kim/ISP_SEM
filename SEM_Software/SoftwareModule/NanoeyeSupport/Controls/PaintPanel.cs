using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using System.Diagnostics;

namespace SEC.Nanoeye.Support.Controls
{
    public partial class PaintPanel : GUIelement.ImagePanel
    {
        #region Property & Vaiables
        protected object imageOriginalSync = new object();

        /// <summary>
        /// 이미지 업데이트 쓰레드가 살아 있어야 하는지 여부를 결정한다.
        /// </summary>
        private bool scanupdateRun = true;

        private SEC.Nanoeye.NanoImage.IScanItemEvent _SiEvent;
        public SEC.Nanoeye.NanoImage.IScanItemEvent SiEvent
        {
            get { return _SiEvent; }
        }

        private string SiName;

        private int scannedHeight = 0;

        Queue<ScanUpdateType> UpdateEvent = new Queue<ScanUpdateType>();

        RectangleF paintRect = RectangleF.Empty;

        #region Scan Update
        /// <summary>
        /// Scan Item에서 들어온 데이터를 캐싱하고, 이미지화 함.
        /// </summary>
        private Thread scanUpdateThread;

        /// <summary>
        /// Scan Item으로 부터 데이터가 들어왔음을 알림.
        /// </summary>
        private ManualResetEvent mreScanUpdateRequest;

        /// <summary>
        /// DataoriToImageori 함수 호출중 dataOri를 릴리즈 하지 못하도록 하기 위함.
        /// </summary>
        private object dataOriLockObj = new object();
        #endregion

        private bool _AutoRelease = false;
        /// <summary>
        /// Frame이 완성되면 자동으로 링크를 해제 할지를 결정한다.
        /// </summary>
        public bool AutoRelease
        {
            get { return _AutoRelease; }
            set { _AutoRelease = value; }
        }

        public override bool IsRepaintWhenVideoChanged
        {
            get
            {
                return base.IsRepaintWhenVideoChanged;
            }
            set
            {

            }
        }

        protected Bitmap imageOriginal;

        protected Size imageOriSize;

        /// <summary>
        /// 원본 이미지의 1배에서 이미지의 물리적 넓이를 가져 온다.
        /// 이미지 수집 장치가 이미지의 전체를 작성 하지 않고, 일부분만 작성 할 수 있어서 이다.
        /// </summary>
        public double ImageWidthAt_x1_Original
        {
            get { return _ImageWidthAt_x1 * paintRect.Width; }
        }
        #endregion

        public PaintPanel()
        {
            _IsRepaintWhenVideoChanged = false;

            scanUpdateThread = new Thread(new ThreadStart(ScanUpdateProc));
            mreScanUpdateRequest = new ManualResetEvent(false);
            scanUpdateThread.Name = this.ToString();
            scanUpdateThread.Start();

            SEC.GenericSupport.GHeapManager.Alloc(ref imageData, 1, this.ToString());
            SEC.GenericSupport.GHeapManager.Alloc(ref imageData2, 1, this.ToString());
            imageOriginal = new Bitmap(1, 1);
            imageOriSize = new Size(1, 1);


            //int ImageSizeWidth = this.imageSize.Width;
            //int ImageSizeHeight = this.Height * 2;

            imagePicture = new Bitmap(1280, 960, PixelFormat.Format32bppArgb);
            imageSize = new Size(1280, 960);

            //imagePicture = new Bitmap(ImageSizeWidth, ImageSizeHeight, PixelFormat.Format32bppArgb);
            //imageSize = new Size(ImageSizeWidth, ImageSizeHeight);
        }

        public void FormImageSizeChange(int width, int height)
        {
            imagePicture = new Bitmap(width * 2, height * 2, PixelFormat.Format32bppArgb);
            imageSize = new Size(width * 2, height * 2);

            ChangeImageProcess(1);

            //FormImageSizeChange(imageSize);


            //this.Invalidate();
        }

        public void FormClientSizeChange(Size size)
        {
            ClientSize = new Size(size.Width, size.Height);
            imagePicture = new Bitmap(1280, 960, PixelFormat.Format32bppArgb);
            imageSize = new Size(1280, 960);
            //imagePicture = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            //imageSize = new Size(size.Width, size.Height);

            ChangeImageProcess(1);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        #region Scan Item event 처리

        /// <summary>
        /// Scan Item의 이벤트에 연결 한다.
        /// </summary>
        /// <param name="eventSI"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EventLink(SEC.Nanoeye.NanoImage.IScanItemEvent eventSI, string name)
        {
            int ImageSizeWidth = this.ClientSize.Width * 2;
            int ImageSizeHeight = this.ClientSize.Height * 2;


            imageSize = new Size(1280, 960);
            //imageSize = new Size(ImageSizeWidth, ImageSizeHeight);
            //imageSize = new Size(320, 320);

            if ((imageData != IntPtr.Zero) && (_SiEvent == null))
            //if ((imageData2 != IntPtr.Zero) && (_SiEvent == null))
            {
                GenericSupport.GHeapManager.Free(ref imageData, this.ToString());

                GenericSupport.GHeapManager.Free(ref imageData2, this.ToString());


            }

            EventRelease(false);
            scannedHeight = 0;
            SiName = name;
            _SiEvent = eventSI;

            SettingChanged();

            _SiEvent.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_FrameUpdated);
            _SiEvent.ScanLineUpdated += new Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_ScanLineUpdated);

            _SiEvent.Disposing += new EventHandler(SiEvent_Disposing);
            _MTools.Visiable = false;

            this.Invalidate();

            return true;
        }

        void SiEvent_Disposing(object sender, EventArgs e)
        {
            EventRelease(true);
        }

        void SiEvent_FrameUpdated(object sender, string name, int startline, int lines)
        {
            if (_AutoRelease)
            {
                EventRelease(true);
            }
        }

        void SiEvent_SettingChangedEvent(object sender, EventArgs e)
        {
            SettingChanged();
        }

        private void SettingChanged()
        {
            lock (UpdateEvent)
            {
                UpdateEvent.Clear();
            }

            imageData = _SiEvent.ImageData;

            if (_Dual || _Merge)
            {
                imageData2 = _SiEvent.ImageData2;
            }

            imageOriSize = new Size(_SiEvent.Setting.ImageWidth, _SiEvent.Setting.ImageHeight);
            paintRect = new RectangleF((float)_SiEvent.Setting.PaintX, (float)_SiEvent.Setting.PaintY, (float)_SiEvent.Setting.PaintWidth, (float)_SiEvent.Setting.PaintHeight);

            lock (imageOriginalSync)
            {
                imageOriginal = new Bitmap(imageOriSize.Width, imageOriSize.Height, PixelFormat.Format32bppArgb);
            }
        }

        /// <summary>
        /// Scan Item의 이벤트 연결을 해제한다.
        /// </summary>
        /// <returns></returns>
        public bool EventRelease()
        {
            return EventRelease(true);
        }

        private unsafe bool EventRelease(bool copy)
        {
            _MTools.Visiable = true;
            //this.Invalidate();
            if (_SiEvent == null) { return false; }

            _SiEvent.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_FrameUpdated);
            _SiEvent.ScanLineUpdated -= new Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_ScanLineUpdated);

            _SiEvent.Disposing -= new EventHandler(SiEvent_Disposing);
            if (copy)
            {
                lock (dataOriLockObj)
                {
                    try
                    {
                        imageData = IntPtr.Zero;

                        imageData2 = IntPtr.Zero;



                        int length = _SiEvent.Setting.ImageWidth * _SiEvent.Setting.ImageHeight;

                        GenericSupport.GHeapManager.Alloc(ref imageData, length * 2, this.ToString());

                       
                        GenericSupport.GHeapManager.Alloc(ref imageData2, length * 2, this.ToString());
                       
                        



                        short* pntSource = (short*)_SiEvent.ImageData;
                        short* pntDest = (short*)imageData;

                        while (length > 0)
                        {
                            *pntDest++ = *pntSource++;



                            length--;
                        }

                        if (_Dual || _Merge)
                        {
                            short* pntSource2 = (short*)_SiEvent.ImageData2;
                            short* pntDest2 = (short*)imageData2;

                            while (length > 0)
                            {
                                *pntDest2++ = *pntSource2;
                                length--;
                            }


                        }

                        








                        ChangeImageProcess(2);
                    }
                    catch (Exception ex)
                    {
                        SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                        GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
                    }
                }
            }

            _SiEvent = null;
            return true;
        }

        /// <summary>
        /// 들어온 이미지 데이터를 처리. Thread 작업 임.
        /// </summary>
        private unsafe void ScanUpdateProc()
        {
            Debug.WriteLine(this.ToString() + " paint thread start", this.ToString());

            while (scanupdateRun)
            {
                #region 이미지 업데이트 정보 기다림
                if (scanUpdateThread.ThreadState == System.Threading.ThreadState.AbortRequested)
                {
                    return;
                }

                if (UpdateEvent.Count == 0)
                {
                    mreScanUpdateRequest.Reset();
                    mreScanUpdateRequest.WaitOne();

                    if (!scanupdateRun) { return; }

                    if (scanUpdateThread.ThreadState == System.Threading.ThreadState.AbortRequested) { return; }
                }

                #endregion

                int start;
                int lineLength;
                ScanUpdateType sutDat;

                lock (UpdateEvent)
                {
                    // TODO : Queue에 값이 없는 상태에서 진입 하는 경우가 있다.
                    // 버그이므로 추후 수정 해야 함.
                    // 동기화 오류로 추정
                    try { sutDat = UpdateEvent.Dequeue(); }
                    catch (Exception) { continue; }
                }

                start = sutDat.startLine;
                lineLength = sutDat.lines;

                // Scan 중 Scan이 중지 될 경우 중지된 영역까지만 다시 그리도록 하기 위해.
                if (scannedHeight < start + lineLength) { scannedHeight = start + lineLength; }

                #region 업데이트할 영역 결정.
                while (true)
                {
                    if (UpdateEvent.Count == 0) { break; }

                    ScanUpdateType sut = UpdateEvent.Peek();

                    // 다음번 획득 이미지 데이터가 기존 이미지 보다 위 부분을 스캔한 경우.
                    // 기존 이미지만 그리도록 한다.
                    if (sut.startLine < start + imageOriSize.Height) { break; }

                    // 다음번 획득 이미지 데이터가 기존 이미지 바로 아래 있는 경우.
                    // 기존 이미지와 다음 이미지를 함께 그리도록 한다.
                    else if (sut.startLine == start + imageOriSize.Height)
                    {
                        sut = UpdateEvent.Dequeue();
                        lineLength += sut.lines;
                    }
                    // 다은번 획득 이미지 데이터가 기존 이미지 보다 아래 있으나 그 사이에 공간이 있는 경우 
                    // 기존 이미지, 다음번 이미지 밑 그 사이 이미지 까지 그리도록 한다. (발생 해서는 안됨.)
                    else
                    {
                        throw new Exception();
                        //sut = UpdateEvent.Dequeue();
                        //imageHeight = (sut.startLine + sut.lines) - start;
                    }

                }
                #endregion

                try
                {
                    DataToImage(start, lineLength);
                    ImageChange(start, lineLength);	// 10%
                    ChangeRepaintimage(start, lineLength);
                }
                catch (Exception ex)
                {
                    SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                }
            }
        }

        private unsafe void SiEvent_ScanLineUpdated(object sender, string name, int StartLine, int Lines)
        {
            Debug.Assert(Lines > 0);
            Debug.Assert(StartLine >= 0);

            UpdateEvent.Enqueue(new ScanUpdateType(name, StartLine, Lines));
            mreScanUpdateRequest.Set();
        }
        #endregion

        #region Image 수정
        protected override void ChangeImageProcess(int level)
        {
            switch (level)
            {
                case 2:
                    ImageChange(0, scannedHeight);
                    ChangeRepaintimage(0, imageOriSize.Height);
                    break;
                default:
                    base.ChangeImageProcess(level);
                    break;
            }
        }

        protected override unsafe void DataToImage()
        {
            Bitmap bm = new Bitmap(imageOriSize.Width, imageOriSize.Height);

            short* pData = (short*)imageData.ToInt32();
            //short* pData = (short*)imageData2.ToInt32();
            int paintLength = imageOriSize.Width * imageOriSize.Height;

            BitmapData bd = bm.LockBits(new Rectangle(0, 0, imageOriSize.Width, imageOriSize.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            uint* bmPnt = (uint*)(bd.Scan0.ToInt32());

            unchecked
            {
                for (int len = 0; len < paintLength; len++)
                {
                    *bmPnt++ = videoLUT[*pData++ + 32768];
                }
            }

            bm.UnlockBits(bd);

            lock (imageOriginalSync) { imageOriginal = bm; }

            ImageChange(0, scannedHeight);
        }

        private unsafe void DataToImage(int start, int lineLength)
        {
            short* pData = (short*)imageData.ToInt32();
            short* pData2 = (short*)imageData2.ToInt32();
            pData += start * imageOriSize.Width;
            pData2 += start * imageOriSize.Width;
            int paintLength = imageOriSize.Width * lineLength;

            lock (imageOriginalSync)
            {
                BitmapData bd = imageOriginal.LockBits(new Rectangle(0, start, imageOriSize.Width, lineLength), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                uint* bmPnt = (uint*)(bd.Scan0.ToInt32());

                unchecked
                {
                    if (_Dual)
                    {
                        pData += imageOriSize.Width / 4;
                        pData2 += imageOriSize.Width / 4;
                        for (int len = 0; len < lineLength; len++)
                        {
                            for (int i = 0; i < imageOriSize.Width; i++)
                            {
                                if (i < imageOriSize.Width / 2)
                                {
                                    *bmPnt = videoLUT[*pData++ + 32768];

                                }
                                else
                                {
                                    *bmPnt = videoLUT2[*pData2++ + 32768];
                                    pData++;
                                }


                                bmPnt++;
                            }

                            for (int j = 0; j < imageOriSize.Width / 2; j++)
                            {
                                pData2++;
                            }
                        }
                    }
                    else if (_Merge)
                    {
                        for (int len = 0; len < paintLength; len++)
                        {

                            short sepmt = *pData++;
                            short bsepmt = *pData2++;
                            short mergepnt;

                            if (sepmt > bsepmt)
                            {
                                mergepnt = sepmt;
                            }
                            else
                            {
                                mergepnt = bsepmt;
                            }

                           
                            *bmPnt = videoLUT[mergepnt + 32768];

                            bmPnt++;
                           

                            
                        }
                    }
                    else
                    {
                        for (int len = 0; len < paintLength; len++)
                        {
                            *bmPnt = videoLUT[*pData++ + 32768];
                            bmPnt++;
                        }
                    }


                }

                imageOriginal.UnlockBits(bd);
            }
        }

        /// <summary>
        /// DoubelBuffer에 이미지 그리기.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void ChangeRepaintimage(int start, int length)
        {
            float ImageSizeWidth = this.ClientSize.Width * 2f;
            float ImageSizeHeight = this.ClientSize.Height * 2f;


            RectangleF picRect = new RectangleF(0,
                        1280f * (paintRect.Y + paintRect.Height * start / (float)imageOriSize.Height),
                        1280f,
                        1280f * paintRect.Height * (float)length / (float)imageOriSize.Height);
            //RectangleF picRect = new RectangleF(0,
            //            ImageSizeWidth * (paintRect.Y + paintRect.Height * start / (float)imageOriSize.Height),
            //            ImageSizeWidth,
            //            ImageSizeWidth * paintRect.Height * (float)length / (float)imageOriSize.Height);

            // 줄이 가는 현상 제거를 위해
            // Horizontal은 전체를 그리므로, X로 확장할 필요가 없음.
            picRect.Inflate(0, 8);

            RectangleF repaintRect = new RectangleF((float)Math.Floor(this.ClientSize.Width / 2f + (-1280f / 2f + _DigitalZoomPoint.X + picRect.X) * _DigitalZoomRunning),
                                                       (float)Math.Floor(this.ClientSize.Height / 2f + (-960f / 2f + _DigitalZoomPoint.Y + picRect.Y) * _DigitalZoomRunning),
                                                       (float)Math.Ceiling(picRect.Width * _DigitalZoomRunning),
                                                       (float)Math.Ceiling(picRect.Height * _DigitalZoomRunning));

            //RectangleF repaintRect = new RectangleF((float)Math.Floor(this.ClientSize.Width / 2f + ((ImageSizeWidth * -1) / 2f + _DigitalZoomPoint.X + picRect.X) * _DigitalZoomRunning),
            //                                           (float)Math.Floor(this.ClientSize.Height / 2f + ((-1 * ImageSizeHeight) / 2f + _DigitalZoomPoint.Y + picRect.Y) * _DigitalZoomRunning),
            //                                           (float)Math.Ceiling(picRect.Width * _DigitalZoomRunning),
            //                                           (float)Math.Ceiling(picRect.Height * _DigitalZoomRunning));

            Graphics g = dfGraphics.Graphics;
            lock (dfGraphics)
            {
                //ImageAttributes ia = new ImageAttributes();
                //ia.SetGamma(1.5f);

                lock (imagePicture)
                {
                    g.DrawImage(imagePicture, repaintRect, picRect, GraphicsUnit.Pixel);
                }
            }

            if (this.Visible) { this.Invalidate(Rectangle.Ceiling(repaintRect)); }

            //System.Diagnostics.Debug.WriteLine("S:" + start.ToString() + ", L:" + length.ToString() + ", R:" + repaintRect.ToString() + " P:" + picRect.ToString() + " PA:" + paintRect.ToString());
        }

        /// <summary>
        /// Original 이미지를 ImagePicture에 그림.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void ImageChange(int start, int length)
        {

            float ImageSizeWidth = this.ClientSize.Width * 2f;
            float ImageSizeHeight = this.ClientSize.Height * 2f;

            // 줄이 가는 현상 제거와 주사 간격 간의 찌그러짐 방지를 위해 위에 8줄을 추가로 그린다.
            if (start > 8) { start -= 8; length += 8; }
            else { length += start; start = 0; }

            RectangleF oriRect = new RectangleF(0, start, imageOriSize.Width, length);

            RectangleF picRect = new RectangleF((float)Math.Floor(1280 * paintRect.X),
                                                (float)Math.Floor(1280 * (paintRect.Y + (float)start * paintRect.Height / (float)imageOriSize.Height)),
                                                (float)Math.Ceiling(1280 * paintRect.Width),
                                                (float)Math.Ceiling(1280 * length * paintRect.Height / imageOriSize.Height));
            //RectangleF picRect = new RectangleF((float)Math.Floor(ImageSizeWidth * paintRect.X),
            //                                    (float)Math.Floor(ImageSizeWidth * (paintRect.Y + (float)start * paintRect.Height / (float)imageOriSize.Height)),
            //                                    (float)Math.Ceiling(ImageSizeWidth * paintRect.Width),
            //                                    (float)Math.Ceiling(ImageSizeWidth * length * paintRect.Height / imageOriSize.Height));

            lock (imagePicture)
            {
                using (Graphics g = Graphics.FromImage(imagePicture))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    lock (imageOriginalSync)
                    {
                        g.DrawImage(imageOriginal, picRect, oriRect, GraphicsUnit.Pixel);
                    }
                    g.Dispose();
                }
            }
        }
        #endregion

        #region Import & Export
        public Bitmap ExportOriginal()
        {
            Bitmap bm;
            lock (imageOriginalSync) 
            { 
                bm = new Bitmap(imageOriginal);
                //imageOriginal.Dispose();
            }
            return MakeExportImage(bm);
        }



        public bool ThumbnailCallback()
        {
            return false;
        }

        private byte[] _ArchivesImageData = null;
        public byte[] ArchivesImageData
        {
            get { return _ArchivesImageData; }
            set
            {
                _ArchivesImageData = value;
            }
        }

        public override unsafe void ImportPicture(Bitmap bm)
        {

            //if (imageData != IntPtr.Zero)
            //{
            //    SEC.GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
            //}

            //Bitmap tmp = new Bitmap(bm);

            //imageSize = new Size(tmp.Width, tmp.Height);

            //SEC.GenericSupport.GHeapManager.Alloc(ref imageData, tmp.Width * tmp.Height * 2, this.ToString());

            //BitmapData bd = tmp.LockBits(new Rectangle(0, 0, tmp.Width, tmp.Height), ImageLockMode.ReadOnly, tmp.PixelFormat);

            //short* pntDataOri = (short*)imageData.ToInt32();

            //int length = tmp.Width * tmp.Height;

            //switch (tmp.PixelFormat)
            //{
            //    case PixelFormat.Format32bppArgb:
            //        uint* bmPnt = (uint*)(bd.Scan0.ToInt32());
            //        for (int i = 0; i < length; i++)
            //        {
            //            *pntDataOri++ = (short)((*bmPnt++) & 0xff);
            //        }
            //        break;
            //    case PixelFormat.Format8bppIndexed:
            //        byte* bmPntByte = (byte*)(bd.Scan0.ToInt32());
            //        for (int i = 0; i < length; i++)
            //        {
            //            *pntDataOri++ = (short)((*bmPntByte++) & 0xff);
            //        }
            //        break;
            //}
            //tmp.UnlockBits(bd);

            //imagePicture = tmp;

            ////imagePicture.Save("c:\\test3.bmp");

            //ChangeImageProcess(1);

            //if (imageData != IntPtr.Zero)
            //{
            //    SEC.GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
            //}



            //base.BeginInit();
            base.ImportPicture(bm);

            //FormClientSizeChange(bm.Size);
            //base.EndInit();



            //ArchivesImageData = SEC.GenericSupport.Converter.ArrayToBytearray(ExportData());


            //base.ImportData(SEC.GenericSupport.Converter.ArrayFromBytearray(_ArchivesImageData, base.ImageSize.Width, base.ImageSize.Height));


            //base.EndInit();

            //throw new NotSupportedException("수집된 이미지 표시 장치이다. Import이 지원되지 않는다.");
        }

        //public override unsafe void Archives

        public override unsafe short[,] ExportData()
        {
            //short[,] result = new short[imageSize.Height, imageSize.Width];

            //result = base.ExportData();
            short[,] result = new short[imageOriSize.Height, imageOriSize.Width];

            fixed (short* pntresult = result)
            {
                short* presult = pntresult;
                short* pdata = (short*)imageData.ToInt32();
                if (_Dual)
                {
                    short* pdata2 = (short*)imageData2.ToInt32();
                }


                int length = imageOriSize.Width * imageOriSize.Height;

                while (length > 0)
                {
                    *presult++ = *pdata++;

                    length--;
                }
            }
            

            return result;
        }
        #endregion

        struct ScanUpdateType
        {
            public readonly string name;
            public readonly int startLine;
            public readonly int lines;
            public ScanUpdateType(string Name, int StartLine, int Lines)
            {
                this.name = Name;
                this.startLine = StartLine;
                this.lines = Lines;
            }
        }
    }
}
