using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SEC.GUIelement
{
    /*
     * 1배일때의 이미지의 물리적 넓이는 받음.
     * 배율에서 따라 이미지의 물리적 넓이를 계산.
     * 계산한 이미지의 물리적 넓이를 이용하여 이미지 한 픽셀의 물리적 크기를 결정.
     * 픽셀의 크기를 입력 받을 경우, 이미지의 픽셀 수에 따라 배율이 변경되므로 불가.
     */

    /// <summary>
    /// 이미지를 표시 하고 측정 도구를 사용한다.
    /// 배율 및 측정 도구의 경우,
    /// 1배일때의 이미지의 물리적 넓이를 이용하여, 배율에 따른 이미지의 물리적 넓이를 계산한다.
    /// 계산한 이미지의 물리적 넓이를 이용하여 이미지 한 픽셀의 물리적 크기를 결정한다.
    /// </summary>
    public partial class ImagePanel : Control, ISupportInitialize
    {
        #region Property & Vaiables
        protected bool _IsRepaintWhenVideoChanged = true;
        /// <summary>
        /// Video값이 바뀔경우 ImagePricture를 다시 생성 및 다시 그릴지를 결정한다.
        /// </summary>
        [DefaultValue(true)]
        public virtual bool IsRepaintWhenVideoChanged
        {
            get { return _IsRepaintWhenVideoChanged; }
            set { _IsRepaintWhenVideoChanged = value; }
        }

        private Bitmap _ImageError = null;
        /// <summary>
        /// 에러 상태에서 표시되는 이미지
        /// </summary>
        [DefaultValue(null)]
        public Bitmap ImageError
        {
            get { return _ImageError; }
            set { _ImageError = value; }
        }

        MouseEventArgs mousemoveEvent = null;

        Point dzCalPnt;

        #region Image Data
        /// <summary>
        /// Unscaled, Uncliped Imageb
        /// </summary>
        protected Bitmap imagePicture = null;

        /// <summary>
        /// 원본 데이터
        /// </summary>
        protected IntPtr imageData = IntPtr.Zero;

        protected IntPtr imageData2 = IntPtr.Zero;


        /// <summary>
        /// 이미지의 픽셀 수.
        /// </summary>
        public Size ImageSize
        {
            get { return imageSize; }
        }
        protected Size imageSize = Size.Empty;

        // Double Buffer
        protected BufferedGraphicsContext dfContext;
        protected BufferedGraphics dfGraphics;

        #endregion

        #region Setup
        #region Micron Bar
        private bool _MicronEnable = true;
        /// <summary>
        /// 미크론바 표시 여부를 결정 한다.
        /// </summary>
        [DefaultValue(true)]
        public bool MicronEnable
        {
            get { return _MicronEnable; }
            set
            {
                if (_MicronEnable == value) { return; }

                lock (dfGraphics) { dfGraphics.Graphics.Clear(BackColor); }

                MicronMake();
                _MicronEnable = value;
                ChangeImageProcess(1);
                this.Invalidate();
            }
        }

        private bool _MicronVoltage = false;
        [DefaultValue(false)]
        public bool MicronVoltage
        {
            get { return _MicronVoltage; }
            set
            {
                if (_MicronVoltage == value) { return; }

                _MicronVoltage = value;
                MicronMake();
            }
        }

        private bool _MicronMag = false;
        [DefaultValue(false)]
        public bool MicronMag
        {
            get { return _MicronMag; }
            set
            {
                if (_MicronMag == value) { return; }

                _MicronMag = value;
                MicronMake();
            }
        }

        private string _MicronDescriptor = "SEC";
        /// <summary>
        /// 설명자 내용
        /// </summary>
        public string MicronDescriptor
        {
            get { return _MicronDescriptor; }
            set
            {
                if (_MicronDescriptor == value) { return; }

                _MicronDescriptor = value;
                MicronMake();
            }
        }

        private bool _MicronCompany = false;
        [DefaultValue(false)]
        public bool MicronCompany
        {
            get { return _MicronCompany; }
            set
            {
                if (_MicronCompany == value) { return; }

                _MicronCompany = value;
                MicronMake();
            }
        }


        private string _Company = "SEC";
        public string Company
        {
            get { return _Company; }
            set
            {
                if (_Company == value) { return; }

                _Company = value;
                MicronMake();

            }
        }

        private string _MicronEghv = "0Kv";
        /// <summary>
        /// 고압 표시 문자
        /// </summary>
        public string MicronEghv
        {
            get { return _MicronEghv; }
            set
            {
                if (_MicronEghv == value) { return; }

                _MicronEghv = value;
                MicronMake();
            }
        }

        private string _MicronMagnification = "x1";
        /// <summary>
        /// Dpi를 이용한 표시 배율 정보.
        /// </summary>
        public string MicronMagnification
        {
            get { return _MicronMagnification; }
        }


        private bool _MicronDetector = false;
        [DefaultValue(false)]
        public bool MicronDetector
        {
            get { return _MicronDetector; }
            set
            {
                if (_MicronDetector == value) { return; }

                _MicronDetector = value;
                MicronMake();
            }
        }

        private string _MicronEtcString = null;
        [DefaultValue(null)]
        public string MicronEtcString
        {
            get { return _MicronEtcString; }
            set
            {
                _MicronEtcString = value;
                MicronMake();
            }
        }

        private bool _MicronVacuum = false;
        [DefaultValue(false)]
        public bool MicronVacuum
        {
            get { return _MicronVacuum; }
            set
            {
                if (_MicronVacuum == value) { return; }

                _MicronVacuum = value;
                MicronMake();
            }
        }

        private string _VacEtcString = null;
        [DefaultValue(null)]
        public string VacEtcString
        {
            get { return _VacEtcString; }
            set
            {
                _VacEtcString = value;
                MicronMake();
            }
        }

        private bool _MicronDate = false;
        [DefaultValue(false)]
        public bool MicronDate
        {
            get { return _MicronDate; }
            set
            {
                if (_MicronDate == value) { return; }

                _MicronDate = value;
                MicronMake();
            }
        }

        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (base.BackColor != value)
                {
                    base.BackColor = value;
                    MicronMake();
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (base.ForeColor != value)
                {
                    base.ForeColor = value;
                    MicronMake();
                }
            }
        }

        /// <summary>
        /// MicronBar에 표시되는 문자의 Edge 색.
        /// </summary>
        private Color _EdgeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black")]
        public Color EdgeColor
        {
            get { return _EdgeColor; }
            set
            {
                if (_EdgeColor == value) { return; }

                _EdgeColor = value;
                MicronMake();
            }
        }

        private string _date = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        public string Date
        {
            get { return _date; }
            set
            {
                if (_date == value) { return; }

                _date = value;
                MicronMake();

            }
        }

        private Image _dot = null;
        public Image Dot
        {
            get { return _dot; }
            set
            {
                if (_dot == value) { return; }
                _dot = value;
                MicronMake();
            }
        }

        private Image _scalebar = null;
        public Image ScaleBar
        {
            get { return _scalebar; }
            set
            {
                _scalebar = value;
                MicronMake();
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (value != base.Font)
                {
                    if (value == null) { base.Font = new Font("Arial", 6, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0))); }
                    else { base.Font = value; }
                    MicronMake();
                }
            }
        }

        private ScalebarDrawer.TickStyle _MicronType = ScalebarDrawer.TickStyle.Ellipse;
        [DefaultValue(typeof(ScalebarDrawer.TickStyle), "Ellipse")]
        public ScalebarDrawer.TickStyle MicronType
        {
            get { return _MicronType; }
            set
            {
                if (_MicronType != value)
                {
                    _MicronType = value;
                    MicronMake();
                }
            }
        }
        #endregion

        #region Magnification
        /// <summary>
        /// 측정한 이미지의 한 픽셀의 길(Meter)
        /// </summary>
        protected double _LengthPerPixel = 0.0001d;
        /// <summary>
        /// 측정한 이미지의 한 픽셀의 길(Meter)
        /// </summary>
        public double LengthPerPixel
        {
            get { return _LengthPerPixel; }
        }

        /// <summary>
        /// 1배에서 이미지의 물리적 넓이. (Meter)
        /// </summary>
        protected double _ImageWidthAt_x1 = 0.128d;
        //protected double _ImageWidthAt_x1 = 0.064d;

        /// <summary>
        /// 1배에서 이미지의 물리적 넓이. (Meter)
        /// </summary>
        [DefaultValue(0.128d)]
        public double ImageWidthAt_x1
        {
            get { return _ImageWidthAt_x1; }
            set
            {
                if (_ImageWidthAt_x1 != value)
                {
                    _ImageWidthAt_x1 = value;
                    ChangeMagnification();
                    MicronMake();
                }
            }
        }

        /// <summary>
        /// 모니터의 픽셀 크기에 의한 배율 조정 계수.
        /// </summary>
        private double pixelMag = 3.0d;
        /// <summary>
        /// 모니터 상에서 640pixel의 길이.cm 단위
        /// 측정 배율이 아닌 표시배율로 배율 값을 표시 할때 사용 된다.
        /// </summary>
        [DefaultValue(19.2d)]
        public decimal LenthOf640Pixel
        {
            get { return (decimal)(pixelMag * 6.4); }
            set
            {
                if (pixelMag == (double)((double)value / 6.4)) { return; }

                pixelMag = (double)((double)value / 6.4);
                ChangeMagnification();
                MicronMake();
            }
        }

        private int _Magnification = 1;
        /// <summary>
        /// 배율
        /// </summary>
        [DefaultValue(1)]
        public int Magnification
        {
            get { return _Magnification; }
            set
            {
                if (_Magnification == value) { return; }

                _Magnification = value;
                ChangeMagnification();
                MicronMake();
            }
        }

        /// <summary>
        /// 측정 배율로 표시 여부.
        /// false일 경우 Digital Zoom과 모니터의 물리적 픽셀 크기를 적용한 배율이 표시 된다.
        /// </summary>
        private bool _AcquiredMagDisplay = false;
        /// <summary>
        /// 측정 배율로 표시 여부.
        /// false일 경우 Digital Zoom과 모니터의 물리적 픽셀 크기를 적용한 배율이 표시 된다.
        /// </summary>
        [DefaultValue(false)]
        public bool AcquiredMagDisplay
        {
            get { return _AcquiredMagDisplay; }
            set
            {
                if (_AcquiredMagDisplay == value) { return; }

                _AcquiredMagDisplay = value;
                ChangeMagnification();
                MicronMake();
            }
        }

        /// <summary>
        /// 배율 표현을 정규화 할지 여부.
        /// </summary>
        private bool _MagnificationRegular = true;
        /// <summary>
        /// 배율 표현을 정규화 할지 여부.
        /// </summary>
        [DefaultValue(true)]
        public bool MagnificationRegular
        {
            get { return _MagnificationRegular; }
            set
            {
                _MagnificationRegular = value;
                ChangeMagnification();
                MicronMake();
            }
        }
        #endregion

        #region Digital Zoom
        [ComVisible(false)]
        public enum EDigitalZoomMode : int
        {
            Fill = 0,
            Quarter = 1,
            Half = 2,
            Original = 3,
            Double = 4,
            Quadruple = 5
        }

        protected int _DigitalZoomMode = 0;
        /// <summary>
        /// 픽셀 확장 및 압축 여부를 결정. 1일 경우 샘플링 결과와 픽셀을 1:1로 매치 시킨다.
        /// </summary>
        [DefaultValue(0)]
        public int DigitalZoomMode
        {
            get { return _DigitalZoomMode; }
            set
            {
                if (_DigitalZoomMode == value) { return; }
                try
                {
                    lock (dfGraphics) { dfGraphics.Graphics.Clear(BackColor); }

                    if (value < 0) { _DigitalZoomMode = 0; }
                    else { _DigitalZoomMode = value; }

                    ChangeMagnification();
                    MicronMake();
                    ChangeMToolsOrigion();
                    MTools.Validate();

                    if (imageSize.Height >= 240) { ChangeImageProcess(1); }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected double _DigitalZoomRunning = 1.0d;
        /// <summary>
        /// Digital Zoom에 의한 배율
        /// </summary>
        [DefaultValue(1.0d)]
        public double DigitalZoomRunnging
        {
            get { return _DigitalZoomRunning; }
        }

        protected Point _DigitalZoomPoint = new Point(0, 0);
        public Point DigitalZoomPoint
        {
            get { return _DigitalZoomPoint; }
            set
            {
                if (_DigitalZoomPoint == value) { return; }
                lock (dfGraphics) { dfGraphics.Graphics.Clear(BackColor); }

                if (value == null) { _DigitalZoomPoint = new Point(0, 0); }
                else { _DigitalZoomPoint = value; }
                ChangeMToolsOrigion();
                MTools.Validate();
                ChangeImageProcess(1);
            }
        }

        /// <summary>
        /// 현재 이미지가 표시되고 있는 이미지의 위치
        /// </summary>
        protected Rectangle _ImageBounds = new Rectangle();
        /// <summary>
        /// 화면상에 표시되는 이미지의 실제 이미지에서의 위치.
        /// </summary>
        public Rectangle ImageBounds
        {
            get { return _ImageBounds; }
        }
        #endregion


        #region Video
        protected volatile uint[] videoLUT;
        protected volatile uint[] videoLUT2;

        protected bool videoChanging = false;
        protected bool videoRechange = false;

        protected double _Contrast = 1.0d;
        /// <summary>
        /// Contrast
        /// </summary>
        public int Contrast
        {
            get { return (int)(Math.Log10(_Contrast) * 100); }
            set
            {
                double temp = Math.Pow(10d, value / 100d);
                if (_Contrast == temp) { return; }

                _Contrast = temp;

                if (_VideoContorlMode == VideoControlModeEnum.ContrastBrightness) { ChangeVideoLUT(); }

            }
        }

        protected int _Brightness = 0;
        /// <summary>
        /// Brightness
        /// </summary>
        public int Brightness
        {
            get { return _Brightness; }
            set
            {
                if (_Brightness == value) { return; }

                _Brightness = value;

                if (_VideoContorlMode == VideoControlModeEnum.ContrastBrightness) { ChangeVideoLUT(); }
            }

        }

        protected double _Contrast2 = 1.0d;
        /// <summary>
        /// Contrast
        /// </summary>
        public int Contrast2
        {
            get { return (int)(Math.Log10(_Contrast2) * 100); }
            set
            {
                double temp = Math.Pow(10d, value / 100d);
                if (_Contrast2 == temp) { return; }

                _Contrast2 = temp;

                if (_VideoContorlMode == VideoControlModeEnum.ContrastBrightness) { ChangeVideoLUT(); }

            }
        }

        protected int _Brightness2 = 0;
        /// <summary>
        /// Brightness
        /// </summary>
        public int Brightness2
        {
            get { return _Brightness2; }
            set
            {
                if (_Brightness2 == value) { return; }

                _Brightness2 = value;

                if (_VideoContorlMode == VideoControlModeEnum.ContrastBrightness) { ChangeVideoLUT(); }
            }

        }
        protected bool _Dual = false;
        public bool Dual
        {
            get { return _Dual; }
            set
            {
                _Dual = value;
            }
        }

        protected bool _Merge = false;
        public bool Merge
        {
            get { return _Merge; }
            set
            {
                _Merge = value;
            }
        }


        Point origin = new Point(0, 0);

        protected short _HistogramMinimum = short.MinValue;
        [DefaultValue(short.MinValue)]
        public short HistogramMinimum
        {
            get { return _HistogramMinimum; }
            set
            {
                if (_HistogramMinimum == value) { return; }
                _HistogramMinimum = value;
                if (_VideoContorlMode == VideoControlModeEnum.Histogram) { ChangeVideoLUT(); }
            }
        }

        protected short _HistogramMaximum = short.MaxValue;
        [DefaultValue(short.MaxValue)]
        public short HistogramMaximum
        {
            get { return _HistogramMaximum; }
            set
            {
                if (_HistogramMaximum == value) { return; }
                _HistogramMaximum = value;
                if (_VideoContorlMode == VideoControlModeEnum.Histogram) { ChangeVideoLUT(); }
            }
        }

        public enum VideoControlModeEnum
        {
            ContrastBrightness,
            ContrastBrightness2,
            Histogram
        }

        protected VideoControlModeEnum _VideoContorlMode = VideoControlModeEnum.ContrastBrightness;
        [DefaultValue(typeof(VideoControlModeEnum), "ContrastBrightness")]
        public VideoControlModeEnum VideoControlMode
        {
            get { return _VideoContorlMode; }
            set
            {
                _VideoContorlMode = value;
                ChangeVideoLUT();
            }
        }
        #endregion
        #endregion

        #region Overlay
        //private bool _CrossHair = false;
        //[DefaultValue(false)]
        //public bool CrossHair
        //{
        //    get { return _CrossHair; }
        //    set
        //    {
        //        if (_CrossHair != value)
        //        {
        //            _CrossHair = value;
        //            this.Invalidate();
        //        }
        //    }
        //}

        private bool _OverlayDraw = false;
        [DefaultValue(false)]
        public bool OverlayDraw
        {
            get { return _OverlayDraw; }
            set
            {
                if (_OverlayDraw != value)
                {
                    _OverlayDraw = value;
                    this.Invalidate();
                }
            }
        }

        private Image _OverlayImage = null;
        [DefaultValue(null)]
        public Image OverlayImage
        {
            get { return _OverlayImage; }
            set
            {
                if (_OverlayImage != value)
                {
                    if (_OverlayImage != null)
                    {
                        lock (_OverlayImage) { _OverlayImage = value; }
                    }
                    else
                    {
                        _OverlayImage = value;
                    }
                    this.Invalidate();
                }
            }
        }
        #endregion


        protected MeasuringTools.MToolsManager _MTools = null;
        /// <summary>
        /// Measruing Tool의 관리자
        /// </summary>
        [Browsable(false),
        DefaultValue(null)]
        public MeasuringTools.MToolsManager MTools
        {
            get { return _MTools; }
            set
            {
                _MTools = value;
                _MTools.Canvas = this;

                ChangeMToolsOrigion();
                _MTools.Zoom = _DigitalZoomRunning;
            }
        }

        private int _MicronBarLocation = 0;
        public int MicronBarLocation
        {
            get { return _MicronBarLocation; }
            set
            {
                _MicronBarLocation = value;
            }
        }

        //ArchivesTabEnable
        private bool _ArchivesTabEnable = false;
        public bool ArchivesTabEnable
        {
            get { return _ArchivesTabEnable; }
            set
            {
                _ArchivesTabEnable = value;
            }
        }




        /// <summary>
        /// MTools에 바로 mouse move 이벤트를 날리면 오버헤드가 너무 크므로 특정 주기마다 날리도록 함.
        /// </summary>
        System.Windows.Forms.Timer mtoolMouseMoveTimer;

        /// <summary>
        /// 초기화 중인지 여부.
        /// </summary>
        protected bool initing = false;
        #endregion

        #region 생성자 & 소멸자 & 초기화
        public ImagePanel()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);


            _MTools = new MeasuringTools.MToolsManager();
            _MTools.Canvas = this;

            mousemoveEvent = null;
            mtoolMouseMoveTimer = new System.Windows.Forms.Timer();
            mtoolMouseMoveTimer.Interval = 200;
            mtoolMouseMoveTimer.Tick += new EventHandler(mtoolMouseMoveTimer_Tick);
            mtoolMouseMoveTimer.Start();

            dfContext = BufferedGraphicsManager.Current;
            dfContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            dfGraphics = dfContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            videoLutCBmodeThread = new System.Threading.Thread(VideoLutCBmodeProc);

            videoLutCBmodeThread2 = new System.Threading.Thread(VideoLutCBmodeProc2);

            videoLutHismodeThread = new System.Threading.Thread(VideoLutHistogrammodeProc);

            ChangeVideoLUT();
        }

        private Size ImageChangeSize = new Size(0, 0);

        //public void FormImageSizeChange(Size size)
        //{
        //    //int SizeY = size.Height - imageSize.Height;

        //    //ImageChangeSize.Height = SizeY /2;

        //    MicronMake();
        //}

        public void BeginInit()
        {
            initing = true;
        }

        public void EndInit()
        {
            initing = false;

            ChangeVideoLUT();
            ChangeMagnification();
            ChangeMToolsOrigion();
            if (imageData != IntPtr.Zero)
            //if (imageData2 != IntPtr.Zero)
            {
                ChangeImageProcess(1);
            }
            MicronMake();
            MTools.Validate();
            this.Invalidate();
        }

        private void DisposeInner()
        {

            if (imageData != System.IntPtr.Zero)
            //if (imageData2 != System.IntPtr.Zero)
            {
                SEC.GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
                //SEC.GenericSupport.GHeapManager.Free(ref imageData2, this.ToString());
            }

            dfGraphics.Dispose();
            dfContext.Invalidate();
            imagePicture.Dispose();
            _ImageError = null;
            _MTools = null;
            imagePicture = null;
        }
        #endregion

        #region override
        protected override void OnSizeChanged(EventArgs e)
        {
            // 사이즈가 너무 작은 경우 회색 화면만 표시 하도록 함.
            if ((this.Width < 320) || (this.Height < 240))
            {
                //imageRepaint = new Bitmap(this.Width, this.Height);
                this.BackgroundImage = _ImageError;
                this.Invalidate();
                return;
            }


            this.BackgroundImage = null;

            if (DesignMode) { return; }

            ChangeMagnification();
            ChangeMToolsOrigion();
            MicronMake();
            MTools.Validate();

            dfContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            dfGraphics = dfContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            if (imageData != IntPtr.Zero)
            //if (imageData2 != IntPtr.Zero)
            {
                ChangeImageProcess(1);
            }

            Debug.WriteLine(this.ClientRectangle.ToString(), "Paint Panel - " + this.Name);

            base.OnSizeChanged(e);

        }

        public override string ToString()
        {
            return "Scan paint - " + this.Name;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;

            lock (dfGraphics) { dfGraphics.Render(g); }

            _MTools.Draw(g);

            //if (_MicronEnable)
            //{


            if (_MicronEnable)
            {
                if (this.ClientSize.Height < 700)
                {
                    g.DrawImage(micronBM,
                   new RectangleF(
                   (float)origin.X,
                   (float)((origin.Y + (imageSize.Height * 13 * _DigitalZoomRunning / 14)) - 10),
                   (float)(imageSize.Width * _DigitalZoomRunning),
                   (float)(imageSize.Height * _DigitalZoomRunning / 15)));
                }
                else
                {
                    g.DrawImage(micronBM,
                   new RectangleF(
                   (float)origin.X,
                   (float)((origin.Y + (imageSize.Height * 13 * _DigitalZoomRunning / 14)) + 5),
                   (float)(imageSize.Width * _DigitalZoomRunning),
                   (float)(imageSize.Height * _DigitalZoomRunning / 15)));
                    //g.DrawImage(micronBM,
                    //new RectangleF(
                    //(float)origin.X,
                    //(float)(_MicronBarLocation - (imageSize.Height * _DigitalZoomRunning / 15)),
                    //(float)(imageSize.Width * _DigitalZoomRunning),
                    //(float)(imageSize.Height * _DigitalZoomRunning / 15)));
                }


            }


            //if (_MicronEnable)
            //{
            //    g.DrawImage(micronBM,
            //        new RectangleF(
            //        (float)origin.X,
            //        (float)(origin.Y + (imageSize.Height * 14 * _DigitalZoomRunning / 15)),
            //        (float)(imageSize.Width * _DigitalZoomRunning),
            //        (float)(imageSize.Height * _DigitalZoomRunning / 15)));
            //}

            //if (_CrossHair)
            //{
            //    g.DrawLine(Pens.Blue, (int)(origin.X + origin.X + imageSize.Width * _DigitalZoomRunning) / 2, (int)(origin.Y), (int)(origin.X + origin.X + imageSize.Width * _DigitalZoomRunning) / 2, (int)((origin.Y + imageSize.Height * _DigitalZoomRunning)));
            //    g.DrawLine(Pens.Blue, origin.X, (int)((origin.Y + origin.Y + imageSize.Height * _DigitalZoomRunning) / 2), (int)(origin.X + imageSize.Width * _DigitalZoomRunning), (int)((origin.Y + origin.Y + imageSize.Height * _DigitalZoomRunning) / 2));
            //}

            if (_OverlayDraw)
            {
                if (_OverlayImage != null)
                {
                    lock (_OverlayImage)
                    {
                        //g.DrawImage(_OverlayImage, new Rectangle(origin.X +(imageSize.Width / 4), origin.Y + (imageSize.Height /4), (int)((imageSize.Width * _DigitalZoomRunning) /2), (int)((imageSize.Height * _DigitalZoomRunning) /2)));
                        //g.DrawImage(_OverlayImage, new Rectangle(origin.X + (this.ClientSize.Width / 2) - (int)((imageSize.Width * _DigitalZoomRunning) / 4), origin.Y + (this.ClientSize.Height / 2) - (int)((imageSize.Height * _DigitalZoomRunning) / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));

                        if (this.ClientSize.Height < 700)
                        {
                            //g.DrawImage(_OverlayImage, new Rectangle(origin.X + (this.ClientSize.Width / 4), origin.Y + (this.ClientSize.Height / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));
                            //g.DrawImage(_OverlayImage, new Rectangle(origin.X, origin.Y, (int)(imageSize.Width * _DigitalZoomRunning), (int)((imageSize.Height * _DigitalZoomRunning) - (imageSize.Height * _DigitalZoomRunning / 10))));
                            g.DrawImage(_OverlayImage, new Rectangle(0 + (this.ClientSize.Width / 2) - (int)((imageSize.Width * _DigitalZoomRunning) / 4), (-45) + (this.ClientSize.Height / 2) - (int)((imageSize.Height * _DigitalZoomRunning) / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));
                        }
                        else
                        {
                            //g.DrawImage(_OverlayImage, new Rectangle(origin.X + (imageSize.Width / 4), origin.Y + (imageSize.Height / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));
                            //g.DrawImage(_OverlayImage, new Rectangle(0, 0, (int)(imageSize.Width * _DigitalZoomRunning), (int)((imageSize.Height * _DigitalZoomRunning - (imageSize.Height * _DigitalZoomRunning / 15)))));
                            //g.DrawImage(_OverlayImage, new Rectangle(0 + (this.ClientSize.Width / 2) - (int)((imageSize.Width * _DigitalZoomRunning) / 4), (-71) + (this.ClientSize.Height / 2) - (int)((imageSize.Height * _DigitalZoomRunning) / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));
                            g.DrawImage(_OverlayImage, new Rectangle(0 + (this.ClientSize.Width / 2) - (int)((imageSize.Width * _DigitalZoomRunning) / 4), (this.ClientSize.Height / 2) - (int)((imageSize.Height * _DigitalZoomRunning) / 4) - ScaleBar.Size.Height, (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));

                        }
                    }
                }
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
            {
                dzCalPnt = e.Location;
            }
            else
            {
                if (_MTools.Visiable) { _MTools.MouseDown(e.Location, e.Button); }
            }
        }

        void mtoolMouseMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mousemoveEvent != null)
            {
                if (_MTools.Visiable) { _MTools.MouseMove(mousemoveEvent.Location, mousemoveEvent.Button); }

                mousemoveEvent = null;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_MTools.Visiable) { _MTools.MouseUp(e.Location, e.Button); }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                //DigitalZoomPoint = new Point(_DigitalZoomPoint.X + (e.X - dzCalPnt.X), _DigitalZoomPoint.Y + (e.Y - dzCalPnt.Y));
                //dzCalPnt = e.Location;
            }
            else
            {
                if (_MTools.Visiable) { mousemoveEvent = e; }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (e.Button == MouseButtons.Middle)
            {
                DigitalZoomPoint = new Point(0, 0);
            }
        }
        #endregion

        #region 설정 변경에 따른 변수 수정용 Method
        protected System.Threading.Thread videoLutCBmodeThread;
        protected System.Threading.Thread videoLutCBmodeThread2;
        protected System.Threading.Thread videoLutHismodeThread;

        protected unsafe void VideoLutHistogrammodeProc(object data)
        {
            uint[] lut = new uint[65536];
            int temp;

            System.Threading.Thread thr = (System.Threading.Thread)(((object[])data)[0]);
            double hisMax = (double)(((object[])data)[1]);
            double hisMin = (double)(((object[])data)[2]);

            fixed (uint* ptr = lut)
            {
                uint* pLut = ptr;
                int index;

                for (index = short.MinValue; index < hisMin; index++)
                {
                    *pLut++ = (uint)((uint)255 << 24);
                }

                double modifier = 256d / (hisMax - hisMin);

                for (; index < hisMax; index++)
                {
                    temp = (int)((index - hisMin) * modifier);
                    temp = (temp > 255) ? 255 : temp;
                    temp = (temp < 0) ? 0 : temp;
                    *pLut++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
                }
                for (; index < short.MaxValue; index++)
                {
                    *pLut++ = (uint)(0xffffffff);
                }
            }

            videoLUT = lut;

            if (_IsRepaintWhenVideoChanged)
            {
                if (imageSize.Width >= 320)
                {
                    ChangeImageProcess(0);
                }
            }
        }

        protected unsafe void VideoLutCBmodeProc(object data)
        {
            uint[] lut = new uint[65536];

            System.Threading.Thread thr = (System.Threading.Thread)(((object[])data)[0]);
            double bri = (double)(((object[])data)[1]);
            double con = (double)(((object[])data)[2]);

            fixed (uint* ptr = lut)
            {
                uint* pLut = ptr;

                System.Diagnostics.Debug.WriteLine(con.ToString() + " - con, " + bri.ToString() + " - bri", this.ToString());

                int length = (short.MaxValue - short.MinValue + 1) / Environment.ProcessorCount;
                int cnt = Environment.ProcessorCount;
                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    int t = short.MinValue + (length * i);
                    uint* pLutTemp = &(pLut[length * i]);

                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        int temp;
                        double tem1 = t * con + bri;
                        temp = (tem1 > 255) ? 255 : (int)tem1;
                        temp = (temp < 0) ? 0 : temp;
                        *pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
                        for (int k = t; k < t + length; k++)
                        {
                            tem1 += con;
                            temp = (tem1 > 255) ? 255 : (int)tem1;
                            temp = (temp < 0) ? 0 : temp;
                            *pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
                        }
                        cnt--;
                    });
                }

                while (cnt > 0)
                {
                    System.Threading.Thread.Sleep(1);
                    if (thr.ThreadState == System.Threading.ThreadState.AbortRequested) { return; }
                }

            }
            if (thr.ThreadState == System.Threading.ThreadState.AbortRequested)
            {
                return;
            }
            videoLUT = lut;

            System.Diagnostics.Debug.WriteLine("ChangeImg", this.ToString());
            if (_IsRepaintWhenVideoChanged)
            {
                if (imageSize.Width >= 320)
                {
                    
                    ChangeImageProcess(0);
                }
            }
        }


        
        protected unsafe void VideoLutCBmodeProc2(object data)
        {
            uint[] lut = new uint[65536];

            System.Threading.Thread thr = (System.Threading.Thread)(((object[])data)[0]);
            double bri = (double)(((object[])data)[1]);
            double con = (double)(((object[])data)[2]);

            fixed (uint* ptr = lut)
            {
                uint* pLut = ptr;

                System.Diagnostics.Debug.WriteLine(con.ToString() + " - con, " + bri.ToString() + " - bri", this.ToString());

                int length = (short.MaxValue - short.MinValue + 1) / Environment.ProcessorCount;
                int cnt = Environment.ProcessorCount;
                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    int t = short.MinValue + (length * i);
                    uint* pLutTemp = &(pLut[length * i]);

                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        int temp;
                        double tem1 = t * con + bri;
                        temp = (tem1 > 255) ? 255 : (int)tem1;
                        temp = (temp < 0) ? 0 : temp;
                        *pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
                        for (int k = t; k < t + length; k++)
                        {
                            tem1 += con;
                            temp = (tem1 > 255) ? 255 : (int)tem1;
                            temp = (temp < 0) ? 0 : temp;
                            *pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
                        }
                        cnt--;
                    });
                }

                while (cnt > 0)
                {
                    System.Threading.Thread.Sleep(1);
                    if (thr.ThreadState == System.Threading.ThreadState.AbortRequested) { return; }
                }

            }
            if (thr.ThreadState == System.Threading.ThreadState.AbortRequested)
            {
                return;
            }
            videoLUT2 = lut;

            System.Diagnostics.Debug.WriteLine("ChangeImg", this.ToString());
            if (_IsRepaintWhenVideoChanged)
            {
                if (imageSize.Width >= 320)
                {
                   
                    ChangeImageProcess(0);
                }
            }
        }

        /// <summary>
        /// Contrast 및 Brightness가 바뀐 경우.
        /// </summary>
        protected virtual void ChangeVideoLUT()
        {
            if (initing) { return; }

            //while (videoLutCBmodeThread.IsAlive)
            //{
            //    videoLutCBmodeThread.Abort();
            //    System.Threading.Thread.Sleep(1);
            //}

            //while (videoLutHismodeThread.IsAlive)
            //{
            //    videoLutHismodeThread.Abort();
            //    System.Threading.Thread.Sleep(1);
            //}

            if (videoLutCBmodeThread.ThreadState == System.Threading.ThreadState.Running)
            {
                videoLutCBmodeThread.Abort();
                System.Threading.Thread.Sleep(1);
            }


            if (videoLutCBmodeThread2.ThreadState == System.Threading.ThreadState.Running)
            {
                videoLutCBmodeThread2.Abort();
                System.Threading.Thread.Sleep(1);
            }


            if (videoLutHismodeThread.ThreadState == System.Threading.ThreadState.Running)
            {
                videoLutHismodeThread.Abort();
                System.Threading.Thread.Sleep(1);
            }

            switch (_VideoContorlMode)
            {
                case VideoControlModeEnum.ContrastBrightness:
                default:
                    videoLutCBmodeThread = new System.Threading.Thread(VideoLutCBmodeProc);
                    videoLutCBmodeThread.Start((object)(new object[] { videoLutCBmodeThread, (double)_Brightness, (double)_Contrast }));
                    //    break;

                    //case VideoControlModeEnum.ContrastBrightness2:
                    //if (_Dual)
                    //{
                        videoLutCBmodeThread2 = new System.Threading.Thread(VideoLutCBmodeProc2);
                        videoLutCBmodeThread2.Start((object)(new object[] { videoLutCBmodeThread2, (double)_Brightness2, (double)_Contrast2 }));
                    //}


                    break;
                case VideoControlModeEnum.Histogram:
                    videoLutHismodeThread = new System.Threading.Thread(VideoLutHistogrammodeProc);
                    videoLutHismodeThread.Start((object)(new object[] { videoLutHismodeThread, (double)_HistogramMaximum, (double)_HistogramMinimum }));
                    break;
            }
        }

        protected void ChangeMagnification()
        {
            if (initing) { return; }

            if (_DigitalZoomMode == 0)
            {
                int disH, disW;
                if (this.ClientSize.Height < this.ClientSize.Width * 0.75d) // 표시 높이보다 화면의 높이가 낮은 경우.
                //if (this.ClientSize.Height < this.ClientSize.Width * 0.5625d) // 표시 높이보다 화면의 높이가 낮은 경우.
                {
                    //disH = this.ClientSize.Height;
                    //disW = (int)(this.ClientSize.Height * 16 / 9);

                    disH = this.ClientSize.Width * 4 / 3;
                    disW = (int)(this.ClientSize.Width);


                    //disH = this.ClientSize.Height;
                    //disW = (int)(this.ClientSize.Height * 16 / 10);
                }
                else
                {
                    //disW = this.ClientSize.Width;
                    //disH = (int)(disW * 9 / 16);
                    //disW = this.ClientSize.Width;
                    //disH = (int)(disW * 10 / 16);
                    disH = this.ClientSize.Width * 3 / 4;
                    disW = (int)(this.ClientSize.Width);
                }

                if (imagePicture == null) { _DigitalZoomRunning = 1; }
                else { _DigitalZoomRunning = disW / (float)imageSize.Width; }
            }
            else
            {
                _DigitalZoomRunning = Math.Pow(2, _DigitalZoomMode - 3);
            }

            //lengthPerPixel = _PixelDefaultSize / _Magnification / _DigitalZoomRunning;
            _LengthPerPixel = _ImageWidthAt_x1 / _Magnification / imageSize.Width / _DigitalZoomRunning;

            #region Magnification
            int mag;

            if (_AcquiredMagDisplay) { mag = (int)_Magnification; }
            else { mag = (int)(_Magnification * _DigitalZoomRunning); }

            if (mag < 1) { mag = 1; }

            _MicronMagnification = MakeMagString(mag);
            #endregion

            if (_MTools != null)
            {
                _MTools.PixelLength = (float)_LengthPerPixel;
                //Debug.WriteLine(_MTools.PixelLength.ToString(), "Pixel Length");
                _MTools.Zoom = _DigitalZoomRunning;
            }
        }

        protected void ChangeMToolsOrigion()
        {
            if (initing) { return; }
            if (imagePicture == null) { return; }
            if (_MTools != null)
            {
                origin.X = (int)(this.ClientSize.Width / 2f + (imageSize.Width / -2f + _DigitalZoomPoint.X) * _DigitalZoomRunning);
                origin.Y = (int)(this.ClientSize.Height / 2f + (imageSize.Height / -2f + _DigitalZoomPoint.Y) * _DigitalZoomRunning);
                _MTools.Origin = origin;

                Debug.WriteLine(origin, "MTools Origin");
            }

            _ImageBounds = new Rectangle((int)(this.ClientSize.Width / 2f + (imageSize.Width / -2f + _DigitalZoomPoint.X) * _DigitalZoomRunning),
                (int)(this.ClientSize.Height / 2f + (imageSize.Height / -2f + _DigitalZoomPoint.Y) * _DigitalZoomRunning),
                (int)(imageSize.Width * _DigitalZoomRunning),
                (int)(imageSize.Height * _DigitalZoomRunning));
        }

        protected string MakeMagString(int mag)
        {
            if (_MagnificationRegular) { mag = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower(mag, 2, new int[] { 10, 13, 15, 20, 30, 40, 50, 70 }); }

            string result = "x" + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(mag, 0, 2, false, (char)0);
            return result;
        }

        #endregion

        #region Image 수정
        /// <summary>
        /// 이미지를 변경한다.
        /// </summary>
        /// <param name="startline">이미지의 시작점</param>
        /// <param name="lineLength">이미지의 높이</param>
        /// <param name="level">변경 시작점. 0이면 data 부터. 1이면 imagePicture 부터</param>
        /// <param name="doNextLevel">다음 레벨까지 순차 진행 할지 여부</param>
        protected virtual void ChangeImageProcess(int level)
        {
            switch (level)
            {
                case 0:
                    DataToImage();
                    goto case 1;
                case 1:
                    ChangeRepaintimage();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Digital Zoom. DZ pnt, ClientSize, imagePicture가 바뀐 경우.
        /// 직접 사용 하지 말고 ChangeImageProcess 함수를 이용 할 것!!! (PaintPanel을 위해)
        /// </summary>
        protected virtual void ChangeRepaintimage()
        {
            if (initing) { return; }

            if (imagePicture == null) { return; }


            RectangleF repaintRect = new RectangleF((float)(this.ClientSize.Width / 2f + (imageSize.Width / -2f + _DigitalZoomPoint.X) * _DigitalZoomRunning),
                                (float)(this.ClientSize.Height / 2f + (imageSize.Height / -2f + _DigitalZoomPoint.Y) * _DigitalZoomRunning),
                                (float)(imageSize.Width * _DigitalZoomRunning),
                                (float)(imageSize.Height * _DigitalZoomRunning));

            lock (dfGraphics)
            {
                Graphics g = dfGraphics.Graphics;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(this.BackColor);
                lock (imagePicture)
                {
                    g.DrawImage(imagePicture, Rectangle.Round(repaintRect));
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Data를 Image에 쓴다.
        /// 직접 사용 하지 말고 ChangeImageProcess 함수를 이용 할 것!!! (PaintPanel을 위해)
        /// </summary>
        /// <param name="data">원본 데이타의 포인터</param>
        /// <param name="bm">그려질 </param>
        /// <param name="width"></param>
        /// <param name="startLine"></param>
        /// <param name="lineLength"></param>
        /// <param name="videoLUT"></param>
        protected virtual unsafe void DataToImage()
        {
            Bitmap bm = new Bitmap(imageSize.Width, imageSize.Height);

            //short* pData;
            short* pData = (short*)imageData.ToInt32();
            //short* pData = (short*)imageData2.ToInt32();

            


            int paintLength = imageSize.Width * imageSize.Height;

            BitmapData bd = bm.LockBits(new Rectangle(0, 0, imageSize.Width, imageSize.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            uint* bmPnt = (uint*)(bd.Scan0.ToInt32());

            unchecked
            {
                // Single Thread
                DataToImageInnderSingleThread(pData, paintLength, bmPnt);

                // Multi Thread
                //DataToImageInnerMultiThread(pData, paintLength, bmPnt);
            }

            bm.UnlockBits(bd);

            Bitmap temp = imagePicture;
            imagePicture = bm;
            if (temp != null) { temp.Dispose(); }
        }

        unsafe private void DataToImageInnderSingleThread(short* pData, int paintLength, uint* bmPnt)
        {
            for (int len = 0; len < paintLength - 1; len++)
            {
                *bmPnt++ = videoLUT[*pData++ + 32768];
            }
            *bmPnt = videoLUT[*pData + 32768];
        }

        unsafe private void DataToImageInnerMultiThread(short* pData, int paintLength, uint* bmPnt)
        {
            int procCnt = Environment.ProcessorCount;

            int runThread = procCnt;
            System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);

            int oneThreadLength = paintLength / procCnt;
            int lastThreadLength = paintLength - (oneThreadLength * (procCnt - 1));
            for (int i = 0; i < procCnt - 1; i++)
            {
                int t = i;

                System.Threading.ThreadPool.QueueUserWorkItem(delegate
                {
                    uint* dest = &bmPnt[oneThreadLength * t];
                    short* src = &pData[oneThreadLength * t];
                    for (int len = 0; len < oneThreadLength; len++)
                    {
                        *dest++ = videoLUT[*src++ + 32768];
                    }
                    runThread--;
                    if (runThread < 1) { mre.Set(); }
                });
            }
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                uint* dest = &bmPnt[oneThreadLength * (procCnt - 1)];
                short* src = &pData[oneThreadLength * (procCnt - 1)];
                for (int len = 0; len < lastThreadLength; len++)
                {
                    *dest++ = videoLUT[*src++ + 32768];
                }
                runThread--;
                if (runThread < 1) { mre.Set(); }
            });

            mre.WaitOne();
        }
        #endregion

        #region MicronBar
        private Size micronBarSize = new Size(0, 0);
        public Size MicronBarSize
        {
            get { return micronBarSize; }
            set
            {
                micronBarSize = value;
            }
        }

        private Bitmap micronBM = null;

        /// <summary>
        /// 미크론 바 이미지를 생성한다.
        /// </summary>
        private void MicronMake()
        {
            int ImageSizeWidth = this.ClientSize.Width * 2;
            int ImageSizeHeight = this.ClientSize.Height * 2;

            if (initing) { return; }
            if (imagePicture == null) { return; }

            RectangleF scalebarRect = new RectangleF(0, 0, 0, 0);
            RectangleF desciptorRect = new RectangleF(0, 0, 0, 0);
            RectangleF hvRect = new RectangleF(0, 0, 0, 0);
            RectangleF magRect = new RectangleF(0, 0, 0, 0);

            //micronArea = new Rectangle(0, this.Height * 14 / 15, this.Width, this.Height / 15);


            //micronBM = ScalebarDrawer.MakeMicronbar(new Size(ImageSizeWidth, (ImageSizeHeight / 19) - 1),
            //                                        this.BackColor,
            //                                        this.ForeColor,
            //                                        this.EdgeColor,
            //                                        this.Font,
            //                                        _Company,
            //                                        _MicronDescriptor,
            //                                        _MicronVoltage,
            //                                        _MicronEghv,
            //                                        _MicronMag,
            //                                        _MicronMagnification,
            //                                        _LengthPerPixel * _DigitalZoomRunning * imageSize.Width / ImageSizeWidth,
            //                                        _Magnification * _DigitalZoomRunning * imageSize.Width / ImageSizeWidth,
            //                                        new Padding(0, 0, 0, 0),
            //                                        _MicronType,
            //                                        _MicronEtcString,
            //                                        _VacEtcString,
            //                                        _MicronDetector,
            //                                        _MicronVacuum,
            //                                        _MicronDate,
            //                                        _MicronCompany,
            //                                        _date,
            //                                        _dot,
            //                                        _scalebar);
            //micronBM = ScalebarDrawer.MakeMicronbar(new Size(1280, (960 / 19) - 1),
            micronBM = ScalebarDrawer.MakeMicronbar(new Size(1280, (960 / 15) - 1),
                                                    this.BackColor,
                                                    this.ForeColor,
                                                    this.EdgeColor,
                                                    this.Font,
                                                    _Company,
                                                    _MicronDescriptor,
                                                    _MicronVoltage,
                                                    _MicronEghv,
                                                    _MicronMag,
                                                    _MicronMagnification,
                                                    _LengthPerPixel * _DigitalZoomRunning * imageSize.Width / 1280,
                                                    _Magnification * _DigitalZoomRunning * imageSize.Width / 1280,
                                                    new Padding(0, 0, 0, 0),
                                                    _MicronType,
                                                    _MicronEtcString,
                                                    _VacEtcString,
                                                    _MicronDetector,
                                                    _MicronVacuum,
                                                    _MicronDate,
                                                    _MicronCompany,
                                                    _date,
                                                    _dot,
                                                    _scalebar);
            //this.Invalidate(micronArea);

            micronBarSize.Width = micronBM.Width;
            micronBarSize.Height = micronBM.Height;

            this.Invalidate();
        }
        #endregion

        #region Import & Export
        public virtual Bitmap ExportPicture()
        {

            return ExportPicture(true);
            //return ExportPicture(false);
        }

        public virtual Bitmap ExportPicture(bool withMicron)
        {
            Bitmap bm;
            lock (imagePicture) { bm = new Bitmap(imagePicture); }
            if (withMicron)
            {
                bm = MakeExportImage(bm);
            }

            MTools.textEnable = true;

            Graphics g = Graphics.FromImage(bm);
            Point preOrigion = MTools.Origin;
            double preZoom = MTools.Zoom;

            //이미지 전체를 저장하므로 Digital Zoom 관련 정보를 초기화 해야 한다.
            MTools.Origin = new Point(0, 0);
            MTools.Zoom = 1;
            MTools.PixelLength = (float)(_ImageWidthAt_x1 / _Magnification / imageSize.Width);

            MTools.Validate();
            MTools.Draw(g);
            g.Flush();
            g.Dispose();

            MTools.Origin = preOrigion;
            MTools.Zoom = preZoom;
            MTools.PixelLength = (float)_LengthPerPixel;
            MTools.Validate();

            MTools.textEnable = false;

            return bm;
        }

        protected Bitmap MakeExportImage(Bitmap bm)
        {
            if (_MicronEnable)
            {
                int mag = (int)_Magnification;

                //mag = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower(mag, 2, new int[] { 10, 13, 15, 20, 30, 40, 50, 70 });

                //string strMag = "x" + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(mag, 0, 3, false, (char)0);

                string strMag = MakeMagString(mag);
                //1280,960
                Bitmap micron = ScalebarDrawer.MakeMicronbar(new Size(1280, 960 / 15),
                                                this.BackColor,
                                                this.ForeColor,
                                                this.EdgeColor,
                                                this.Font,
                                                _Company,
                                                _MicronDescriptor,
                                                _MicronVoltage,
                                                _MicronEghv,
                                                _MicronMag,
                                                strMag,
                                                (_ImageWidthAt_x1 / _Magnification / imageSize.Width) * imageSize.Width / 1280,
                                                _Magnification,
                                                new Padding(0, 0, 0, 0),
                                                 ScalebarDrawer.TickStyle.Rectangle,
                                                 _MicronEtcString,
                                                 _VacEtcString,
                                                 _MicronDetector,
                                                 _MicronVacuum,
                                                 _MicronDate,
                                                 _MicronCompany,
                                                 _date,
                                                 this.Dot,
                                                 this.ScaleBar);
                Graphics g = Graphics.FromImage(bm);
                g.DrawImage(micron, new Rectangle(0, bm.Height * 14 / 15, bm.Width, bm.Height / 15));
                g.Dispose();

            }
            return bm;
        }

        /// <summary>
        /// Image data(원본)를 가져온다.
        /// </summary>
        /// <returns></returns>
        public virtual unsafe short[,] ExportData()
        {
            short[,] result = new short[imageSize.Height, imageSize.Width];

            fixed (short* pntResult = result)
            {
                short* pResult = pntResult;
                short* pData = (short*)imageData.ToInt32();
                //short* pData = (short*)imageData2.ToInt32();

                int length = imageSize.Width * imageSize.Height;

                while (length > 0)
                {
                    *pResult++ = *pData++;
                    length--;
                }
            }

            return result;
        }

        /// <summary>
        /// Image bitmap을 설정한다.
        /// </summary>
        /// <param name="bm"></param>
        public virtual unsafe void ImportPicture(Bitmap bm)
        {
            if (imageData != IntPtr.Zero)
            //if (imageData2 != IntPtr.Zero)
            {
                SEC.GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
                //SEC.GenericSupport.GHeapManager.Free(ref imageData2, this.ToString());
            }

            Bitmap tmp = new Bitmap(bm);

            imageSize = new Size(tmp.Width, tmp.Height);
            //imageSize = new Size(ClientSize.Width, ClientSize.Height);



            SEC.GenericSupport.GHeapManager.Alloc(ref imageData, tmp.Width * tmp.Height * 2, this.ToString());
            //SEC.GenericSupport.GHeapManager.Alloc(ref imageData2, tmp.Width * tmp.Height * 2, this.ToString());

            BitmapData bd = tmp.LockBits(new Rectangle(0, 0, tmp.Width, tmp.Height), ImageLockMode.ReadOnly, tmp.PixelFormat);

            short* pntDataOri = (short*)imageData.ToInt32();
            //short* pntDataOri = (short*)imageData2.ToInt32();

            int length = tmp.Width * tmp.Height;

            switch (tmp.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    uint* bmPnt = (uint*)(bd.Scan0.ToInt32());
                    for (int i = 0; i < length; i++)
                    {
                        *pntDataOri++ = (short)((*bmPnt++) & 0xff);
                    }
                    break;
                case PixelFormat.Format8bppIndexed:
                    byte* bmPntByte = (byte*)(bd.Scan0.ToInt32());
                    for (int i = 0; i < length; i++)
                    {
                        *pntDataOri++ = (short)((*bmPntByte++) & 0xff);
                    }
                    break;
            }
            tmp.UnlockBits(bd);

            imagePicture = tmp;

            //imagePicture.Save("c:\\test3.bmp");

            ChangeImageProcess(1);
        }

        /// <summary>
        /// 이미지 데이터를 설정한다.
        /// </summary>
        /// <param name="datas"></param>
        public virtual unsafe void ImportData(short[,] datas)
        {
            int length = datas.Length;

            if (imageData != IntPtr.Zero)
            //if (imageData2 != IntPtr.Zero)
            {
                SEC.GenericSupport.GHeapManager.Free(ref imageData, this.ToString());
                //SEC.GenericSupport.GHeapManager.Free(ref imageData2, this.ToString());
            }
            SEC.GenericSupport.GHeapManager.Alloc(ref imageData, length * 2, this.ToString());
            //SEC.GenericSupport.GHeapManager.Alloc(ref imageData2, length * 2, this.ToString());

            fixed (short* pntData = datas)
            {
                short* pData = pntData;
                short* pImg = (short*)imageData.ToInt32();
                //short* pImg = (short*)imageData2.ToInt32();

                while (length > 0)
                {
                    *pImg++ = *pData++;
                    length--;
                }
            }

            

            imageSize = new Size(datas.GetLength(1), datas.GetLength(0));

            ChangeImageProcess(0);
        }
        #endregion

        #region 기타
        /// <summary>
        /// Control 좌표의 Point를 이미지 좌표로 변환 한다.
        /// </summary>
        /// <param name="pnt">Control 기준 Point</param>
        /// <returns>Image 기준 Point</returns>
        public Point PointToImage(Point pnt)
        {
            int x = (int)((pnt.X - origin.X) / DigitalZoomRunnging);
            int y = (int)((pnt.Y - origin.Y) / DigitalZoomRunnging);
            return new Point(x, y);
        }

        /// <summary>
        /// 화면상에서 이미지가 표시되고 있는 위치를 가져 온다.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetDisplayRect()
        {
            Rectangle result = new Rectangle();
            result.Location = origin;

            result.Width = (int)(imageSize.Width * _DigitalZoomRunning);
            result.Height = (int)(imageSize.Height * _DigitalZoomRunning);

            return result;
        }
        #endregion

        #region Basic Overlay

        public enum BasicOverlayType
        {
            Cross,
            CrossWithCircle
        }

        /// <summary>
        /// 기론 Overlay Image를 생성한다.
        /// </summary>
        /// <param name="bot">Overlay type</param>
        /// <param name="imgSize">이미지 크기</param>
        /// <returns></returns>
        public static Image BasicOverlay(BasicOverlayType bot, Size imgSize)
        {
            Bitmap bm = new Bitmap(imgSize.Width, imgSize.Height);

            Graphics g = Graphics.FromImage(bm);
            Pen p = new Pen(Color.Blue);
            p.Width = 3;

            g.DrawLine(p, 0, imgSize.Height / 2, imgSize.Width, imgSize.Height / 2);
            g.DrawLine(p, imgSize.Width / 2, 0, imgSize.Width / 2, imgSize.Height);

            if (bot == BasicOverlayType.CrossWithCircle)
            {
                int s = Math.Min(imgSize.Width, imgSize.Height) / 2;

                p.Color = Color.Red;

                g.DrawEllipse(p, (imgSize.Width - s) / 2, (imgSize.Height - s) / 2, s, s);
            }

            p.Dispose();
            g.Dispose();
            return bm;
        }
        #endregion
    }
}
