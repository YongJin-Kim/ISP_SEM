using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
    public partial class EllipseKnob : Control, ISupportInitialize
    {
        #region Property & Variables
        /// <summary>
        /// Control의 기본 이미지
        /// </summary>
        private Bitmap imageBase = new Bitmap(10, 10);
        /// <summary>
        /// imageBase 위에 마우스 상태 및 값에 따른 정보를 표시한 이미지
        /// </summary>
        private Bitmap imagePaint = new Bitmap(10, 10);

        private ButtonStatesWithMouse bswm = ButtonStatesWithMouse.Normal;

        private int imageValue = 0;
        private int targetValue = 0;

        private Timer rePaintTimer;

        #region Mouse Caputre 관련
        /// <summary>
        /// 0 - Non Capture
        /// 1 - Left Capture
        /// 2 - Right Capture
        /// </summary>
        public int mouseCaptureMode = 0;

        private int premouseposX = 0;
        private double moveAccum = 0;
        private Rectangle preClipRect;
        #endregion

        private bool _IsInited = true;
        public bool IsInited
        {
            get { return _IsInited; }
        }

        #region Value
        private int _Minimum = 0;
        [DefaultValue(0)]
        public virtual int Minimum
        {
            get { return _Minimum; }
            set
            {

                if ((_IsInited) && (value > _Value)) { throw new ArgumentException("Minimum is larger then Value."); }
                _Minimum = value;
            }
        }

        private int _Maximum = 100;
        [DefaultValue(0)]
        public virtual int Maximum
        {
            get { return _Maximum; }
            set
            {
                if ((_IsInited) && (value < _Value)) { throw new ArgumentException("Maximum is smaller then Value."); }
                _Maximum = value;
            }
        }

        private int _Value = 0;
        [DefaultValue(0)]
        public virtual int Value
        {
            get { return _Value; }
            set
            {
                if (_Value == value) { return; }

                if (value < _Minimum) { _Value = _Minimum; }
                else if (value > _Maximum) { _Value = _Maximum; }
                else { _Value = value; }
                targetValue = _Value;
            }
        }
        #endregion

        #region Sensitive
        private decimal _SensitiveRight = 0.5M;
        [DefaultValue(0.5)]
        public decimal SensitiveRight
        {
            get { return _SensitiveRight; }
            set { _SensitiveRight = value; }
        }

        private decimal _SensitiveLeft = 5;
        [DefaultValue(5)]
        public decimal SensitiveLeft
        {
            get { return _SensitiveLeft; }
            set { _SensitiveLeft = value; }
        }
        #endregion

        #region Color
        private Color _ColorBase = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color ColorBase
        {
            get { return _ColorBase; }
            set
            {
                _ColorBase = value;
                GenerateBackground();
            }
        }

        private Color _ColorGauge = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color ColorGauge
        {
            get { return _ColorGauge; }
            set
            {
                _ColorGauge = value;
                GenerateBackground();
            }
        }

        private Color _ColorIndicator = Color.Blue;
        [DefaultValue(typeof(Color), "Blue")]
        public Color ColorIndicator
        {
            get { return _ColorIndicator; }
            set
            {
                _ColorIndicator = value;
            }
        }

        private Color _ColorCircleNormal = Color.Silver;
        [DefaultValue(typeof(Color), "Silver")]
        public Color ColorCircleNormal
        {
            get { return _ColorCircleNormal; }
            set
            {
                _ColorCircleNormal = value;
            }
        }

        private Color _ColorCircleHover = Color.Aquamarine;
        [DefaultValue(typeof(Color), "Aquamarine")]
        public Color ColorCircleHover
        {
            get { return _ColorCircleHover; }
            set
            {
                _ColorCircleHover = value;
            }
        }

        private Color _ColorCirclePush = Color.Red;
        [DefaultValue(typeof(Color), "Red")]
        public Color ColorCirclePush
        {
            get { return _ColorCirclePush; }
            set
            {
                _ColorCirclePush = value;
            }
        }

        private Image _centerIcon = null;
        public Image Centericon
        {
            get { return _centerIcon; }
            set
            {
                _centerIcon = value;
                this.Invalidate(preClipRect);
            }
        }

        private Image _defaultIcon = null;
        public Image DefaultIcon
        {
            get { return _defaultIcon; }
            set
            {
                _defaultIcon = value;
                this.Invalidate(preClipRect);
            }
        }
        #endregion
        #endregion

        #region Event
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler Turn;
        protected virtual void OnTurn()
        {
            if (Turn != null)
            {
                Turn(this, EventArgs.Empty);
            }
        }
        #endregion

        public EllipseKnob()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            this.MinimumSize = new Size(30, 30);

            rePaintTimer = new Timer();
            rePaintTimer.Tick += new EventHandler(rePaintTimer_Tick);
            rePaintTimer.Interval = 200;
            rePaintTimer.Start();

        }

        bool stateChagned = false;

        void rePaintTimer_Tick(object sender, EventArgs e)
        {
            if (_Value != targetValue)
            {
                Value = targetValue;

                if (_IsInited) { OnValueChanged(); }
            }

            if (this.Visible && ((_Value != imageValue) || stateChagned))
            {
                GeneratePaintimage();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            lock (imagePaint)
            {
                pe.Graphics.DrawImage(imagePaint, this.ClientRectangle);
            }
            base.OnPaint(pe);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseCaptureMode == 0) { return; }

            if (e.X < this.ClientRectangle.Left + 3)
            {
                if (targetValue > _Minimum)
                {
                    premouseposX = this.ClientRectangle.Right - 4;
                    Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
                }
            }
            else if (e.X > this.ClientRectangle.Right - 4)
            {
                if (targetValue < _Maximum)
                {
                    premouseposX = this.ClientRectangle.Left + 3;
                    Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
                }
            }
            else
            {
                int truncateValue = 0;
                switch (mouseCaptureMode)
                {
                    case 1:
                        moveAccum += (double)((e.X - premouseposX) / _SensitiveLeft);
                        break;
                    case 2:
                        moveAccum += (double)((e.X - premouseposX) / _SensitiveRight);
                        break;
                    default:
                        throw new InvalidOperationException("Undefined Mode.");
                }
                truncateValue = (int)Math.Truncate(moveAccum);
                if (truncateValue != 0)
                {
                    moveAccum -= truncateValue;
                    targetValue += truncateValue;

                    if (targetValue > _Maximum)
                    {
                        targetValue = _Maximum;
                    }
                    else if (targetValue < _Minimum)
                    {
                        targetValue = _Minimum;
                    }

                }
                premouseposX = e.X;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            {
                GenerateBackground();
            }
        }

        #region 마우스 상태
        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    bswm |= ButtonStatesWithMouse.MouseHover;
        //    stateChagned = true;
        //    base.OnMouseEnter(e);
        //}

        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    bswm &= ~ButtonStatesWithMouse.MouseHover;
        //    stateChagned = true;
        //    base.OnMouseLeave(e);
        //}

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    return;

        //    bswm |= ButtonStatesWithMouse.ButtonPush;
        //    stateChagned = true;
        //    base.OnMouseDown(e);
        //}

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
           
        //    bswm &= ~ButtonStatesWithMouse.ButtonPush;
        //    if (mouseCaptureMode == 0)
        //    {
        //        switch (e.Button)
        //        {
        //            case MouseButtons.Left:
        //                mouseCaptureMode = 1;
        //                break;
        //            case MouseButtons.Right:
        //                mouseCaptureMode = 2;
        //                break;
        //        }

        //        premouseposX = e.Location.X;
        //        moveAccum = 0;
        //    }
        //    else
        //    {
        //        mouseCaptureMode = 0;
        //    }

        //    switch (mouseCaptureMode)
        //    {
        //        case 0:
        //            rePaintTimer.Interval = 200;
        //            Cursor.Clip = preClipRect;

        //            //this.BackgroundImage = _defaultIcon;
        //            Cursor = Cursors.Default;
        //            this.Capture = false;
        //            break;
        //        case 1:
        //        case 2:
        //            this.Capture = true;

        //            //this.BackgroundImage = _centerIcon;
        //            preClipRect = Cursor.Clip;
        //            //Rectangle clipRect = ClientRectangle;
        //            Rectangle clipRect = new Rectangle(ClientRectangle.X, ClientRectangle.Top +(int)( ClientRectangle.Height * 0.4), ClientRectangle.Width, (int)(ClientRectangle.Height * 0.2));
        //            clipRect.Inflate(-2, -2);
        //            Cursor.Clip = this.RectangleToScreen(clipRect);
        //            Cursor = Cursors.NoMoveHoriz;
        //            moveAccum = 0;
        //            premouseposX = this.PointToClient(Cursor.Position).X;
        //            rePaintTimer.Interval = 80;
        //            break;
        //        default:
        //            throw new InvalidOperationException("Undefined Mode.");
        //    }
        //    stateChagned = true;
        //    this.Invalidate();
        //    base.OnMouseUp(e);
        //}

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    if (Enabled)
        //    {
        //        bswm &= ~ButtonStatesWithMouse.Disabled;
        //    }
        //    else
        //    {
        //        bswm |= ButtonStatesWithMouse.Disabled;
        //    }
        //    base.OnEnabledChanged(e);
        //}
        #endregion

        public void BeginInit()
        {
            _IsInited = false;
        }

        public void EndInit()
        {
            _IsInited = true;
            ValidateValue(_Value);
            GenerateBackground();
            OnValueChanged();
        }

        private void ValidateValue(int value)
        {
            if (value < _Minimum)
            {
                //throw new ArgumentException("Value is smaller then Minimum."); }
                value = _Minimum;
            }
            if (value > _Maximum)
            {
                //throw new ArgumentException("Value is larger then Maximum."); }
                value = _Maximum;
            }
        }

        private void GenerateBackground()
        {
            try
            {
                if (!_IsInited) { return; }

                //GraphicsPath gp = new GraphicsPath();
                //gp.AddEllipse(this.ClientRectangle);
                //this.Region = new Region(gp);

                this.Region = new Region(this.ClientRectangle);
                Rectangle bounds = this.ClientRectangle;
                bounds.Inflate(-1, -1);

                Bitmap bm = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(bm);
               
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //Rectangle BackImage = new Rectangle(this.Left, this.Top, _centerIcon.Width, _centerIcon.Height);
                //g.DrawImage(_centerIcon, BackImage);

                DrawFrame(g, bounds, 5);
                
                bounds.Inflate(-5, -5);
                DrawGage(g, bounds, 12, 135, 270);
                g.Dispose();

                lock (imageBase)
                {
                    imageBase = bm;
                }

                GeneratePaintimage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GeneratePaintimage()
        {
            if (!_IsInited) { return; }

            stateChagned = false;

            Bitmap bm;

            imageValue = _Value;

            float angle = GetAngleValue();

            Rectangle bounds = new Rectangle(0, 0, imageBase.Width, imageBase.Height);
            
            lock (imageBase)
            {
                bm = imageBase.Clone(bounds, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }

            Graphics g = Graphics.FromImage(bm);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 프레임 영역 제거
            bounds.Inflate(-5, -5);

            // Gauge 베이스 영역 제거
            bounds.Inflate(-7, -7);

            DrawIndigator(g, bounds, angle);

            // 게이지 영역 제거
            bounds.Inflate(-6, -6);

            //g.DrawImage(_centerIcon, bounds);
            DrawInnerCircle(g, bounds);
            //g.DrawImage(_centerIcon, bounds);
            RectangleF markBounds = GetMarkBounds(bounds, angle + 135);
            //DrawMark(g, markBounds);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(Text, Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);

            g.Dispose();

            lock (imagePaint)
            {
                imagePaint = bm;
            }

            this.Invalidate();
        }

        private void DrawMark(Graphics g, RectangleF bounds)
        {
            LinearGradientBrush lgb;

            lgb = new LinearGradientBrush(
                bounds,
                ControlPaint.Dark(this.BackColor),
                Color.White,
                LinearGradientMode.ForwardDiagonal);
            //g.FillEllipse(lgb, bounds);
            //g.DrawImage(_centerIcon, bounds);

            lgb.Dispose();
        }

        private void DrawIndigator(Graphics g, Rectangle bounds, float angel)
        {
            Pen pRotate;

            pRotate = new Pen(this.ColorIndicator, 6);

            g.DrawArc(pRotate, bounds, 135f, angel);

            pRotate.Dispose();
        }

        private void DrawInnerCircle(Graphics g, Rectangle bounds)
        {
            PathGradientBrush pgb;
            GraphicsPath gp = new GraphicsPath();

            // 버튼을 채울 브러쉬를 초기화 합니다.
            // gp = new GraphicsPath();
            gp.AddEllipse(bounds);
            pgb = new PathGradientBrush(gp);
            gp.Dispose();

            switch (bswm)
            {
                case ButtonStatesWithMouse.Normal:
                    pgb.SurroundColors = new Color[] { ColorCircleNormal };
                    break;
                case ButtonStatesWithMouse.NormalHover:
                    pgb.SurroundColors = new Color[] { ColorCircleHover };
                    break;
                case ButtonStatesWithMouse.PushHover:
                case ButtonStatesWithMouse.ButtonPush:
                    pgb.SurroundColors = new Color[] { ColorCirclePush };
                    break;
                case ButtonStatesWithMouse.Disabled:
                    break;
            }

            pgb.FocusScales = new PointF(0.1F, 0.1F);
            pgb.CenterPoint = new PointF(bounds.Left + bounds.Width / 4, bounds.Top + bounds.Height / 4);
            pgb.CenterColor = Color.Transparent;// ControlPaint.Light(this.BackColor);
            g.FillEllipse(pgb, bounds);
            pgb.Dispose();
            g.DrawEllipse(Pens.Transparent, bounds);
            //g.DrawImage(_centerIcon, bounds);

        }

        private void DrawFrame(Graphics g, Rectangle bounds, Int32 frameWidth)
        {
            //LinearGradientBrush lgb;
            //Rectangle frameBounds = bounds;
            //lgb = new LinearGradientBrush(frameBounds, Color.Empty, Color.Empty, LinearGradientMode.ForwardDiagonal);

            //ColorBlend blend = new ColorBlend(3);
            //blend.Colors[0] = ControlPaint.Dark(this.ColorBase);
            //blend.Colors[1] = this.ColorBase;
            //blend.Colors[2] = Color.White;
            //blend.Positions[0] = 0;
            //blend.Positions[2] = 1;

            //Pen p;
            //for (int i = 0; i < frameWidth; i++)
            //{
            //    blend.Positions[1] = 0.7F - (0.3F * i) / frameWidth;
            //    lgb.InterpolationColors = blend;

            //    p = new Pen(lgb, 2F);
            //    g.DrawEllipse(p, frameBounds);
            //    frameBounds.Inflate(-1, -1);
            //    p.Dispose();
            //}

            //lgb.Dispose();
        }

        private void DrawGage(Graphics g, Rectangle bounds, int width, float gageStart, float gageSweep)
        {
            Pen pBack = new Pen(this.ColorBase, width);
            Pen pSweep = new Pen(this.ColorGauge, width - 4);

            bounds.Inflate(-width / 2, -width / 2);
            g.DrawEllipse(pBack, bounds);

            pSweep.StartCap = LineCap.Round;
            pSweep.EndCap = LineCap.Round;
            g.DrawArc(pSweep, bounds, gageStart, gageSweep);


            pBack.Dispose();
            pSweep.Dispose();
        }

        private float GetAngleValue()
        {
            float angle;
            try { angle = ((270 * (_Value - _Minimum)) / (_Maximum - _Minimum)); }
            catch (DivideByZeroException) { angle = 0; }
            return angle;
        }

        RectangleF GetMarkBounds(Rectangle bounds, float angle)
        {
            // 마크 표시 크기를 계산합니다. (1 / 4)
            Size markSize = new Size(bounds.Width / 4, bounds.Height / 4);

            // 각도 0인 기준점을 지정하고 angle만큼 시계방향으로 회전합니다. 
            PointF pt = new PointF(bounds.Right - bounds.Width /2, bounds.Bottom - bounds.Height/2);
            //pt = GetRotatePoint(bounds, pt, angle);
            //pt.Offset(-markSize.Width / 2F, -markSize.Height / 2F);
            //pt.X -= markSize.Width / 2F;
            //pt.Y -= markSize.Height / 2F;

            return new RectangleF(pt, markSize);
        }

        private static PointF GetRotatePoint(Rectangle bounds, PointF location, float degree)
        {
            PointF basis =
                new PointF(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height / 2F);

            float radian = (float)(degree * Math.PI / 180);
            float sin = (float)Math.Sin(radian);
            float cos = (float)Math.Cos(radian);

            PointF p = new PointF(
                cos * (location.X - basis.X) + sin * (location.Y - basis.Y) + basis.X,
                sin * (location.X - basis.X) - cos * (location.Y - basis.Y) + basis.Y);

            return p;
        }
    }
}
