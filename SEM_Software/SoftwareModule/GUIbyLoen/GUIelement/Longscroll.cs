using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SEC.GUIelement
{
	/// <summary>
	/// 넓은 범위를 제어함. 
	/// User control로 제작시 designer상에서 Property가 바뀌는 현상이 있어 
	/// 직접 구현 함.
	/// </summary>
	public partial class Longscroll : Control
	{
		#region Property & Variables
		private Rectangle leftRect;
		private Rectangle rightRect;
		private Rectangle panelRect;

		private ButtonStatesWithMouse leftBswm = ButtonStatesWithMouse.Normal;
		private ButtonStatesWithMouse rightBswm = ButtonStatesWithMouse.Normal;
		private ButtonStatesWithMouse panelBswm = ButtonStatesWithMouse.Normal;

		//// Double Buffer
		//protected BufferedGraphicsContext dfContext;
		//protected BufferedGraphics dfGraphics;

		/// <summary>
		/// Panel이나 Button에 의해 바뀌도록 요청된 값을 실제 값에 적용 하는 타이머
		/// Panel의 경우 Mouse Move 이벤트를 사용 하는데 이 이벤트가 너무 자주 발생 하므로 이를 이용 하여 막음.
		/// </summary>
		private Timer updateTimer;

		/// <summary>
		/// updateTimer에어서 실제 값에 적용될 값.
		/// </summary>
		protected int targetValue;

		private Timer repeatTimer;
		private int repetAccel;

		private Image _LeftIcon = null;
		[DefaultValue(null)]
		public Image LeftIcon
		{
			get { return _LeftIcon; }
			set
			{
				_LeftIcon = value;
				this.Invalidate(leftRect);
			}
		}

		private Image _RightIcon = null;
		[DefaultValue(null)]
		public Image RightIcon
		{
			get { return _RightIcon; }
			set
			{
				_RightIcon = value;
				this.Invalidate(rightRect);
			}
		}


		/// <summary>
		/// Panel 선택 모드
		/// </summary>
		private PanelSelectionEnum panelMode= PanelSelectionEnum.Non;

		private Color revBru = Color.White;
		private Color _PanelColor = Color.Black;
		/// <summary>
		/// 스크롤 영역의 배경 색. 인디케이터의 색은 배경색의 반전석으로 결정 된다.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		public Color PanelColor
		{
			get { return _PanelColor; }
			set
			{
				_PanelColor = value;
				revBru = Color.FromArgb(value.ToArgb() ^ 0x00ffffff);
				DrawBufferImage();
			}
		}

		protected int _Value = -1;
		[DefaultValue(-1)]
		public int Value
		{
			get { return _Value; }
			set
			{

				int tmp;
				if(value > _Maximum) { tmp = _Maximum; }
				else if(value < _Minimum) { tmp = _Minimum; }
				else { tmp = value; }

				if(_Value != tmp)
				{
					_Value = tmp;
					//targetValue = _Value;


					OnValueChanged();
				}
				if(_Value == _Minimum) { leftBswm |= ButtonStatesWithMouse.Disabled; }
				else { leftBswm &= ~ButtonStatesWithMouse.Disabled; }

				if(_Value == _Maximum) { rightBswm |= ButtonStatesWithMouse.Disabled; }
				else { rightBswm &= ~ButtonStatesWithMouse.Disabled; }

				DrawBufferImage();
			}
		}

		protected int _Maximum = 100;
		[DefaultValue(100)]
		public virtual int Maximum
		{
			get { return _Maximum; }
			set { _Maximum = value; }
		}

		protected int _Minimum = 0;
		[DefaultValue(100)]
		public virtual int Minimum
		{
			get { return _Minimum; }
			set { _Minimum = value; }
		}

		protected decimal _DividInner = 5M;
		[DefaultValue(5)]
		public decimal DividInner
		{
			get { return _DividInner; }
			set { _DividInner = value; }
		}

		protected decimal _DividOutter = 0.5M;
		[DefaultValue(0.5)]
		public decimal DividOutter
		{
			get { return _DividOutter; }
			set { _DividOutter = value; }
		}

		protected ValueViewMode _ValueView = ValueViewMode.Non;
		[DefaultValue(typeof(ValueViewMode), "Non")]
		public ValueViewMode ValueView
		{
			get { return _ValueView; }
			set
			{
				_ValueView = value;

				DrawBufferImage();
			}
		}

		protected int _DecimalPlace = 0;
		[DefaultValue(0)]
		public int DecimalPlace
		{
			get { return _DecimalPlace; }
			set { _DecimalPlace = value; }
		}

		private EllipseButtonStyle _ButtonLeftStyle = new EllipseButtonStyle();
		public EllipseButtonStyle ButtonLeftStyle
		{
			get { return _ButtonLeftStyle; }
			set
			{
				if(value == null) { _ButtonLeftStyle = new EllipseButtonStyle(); }
				else { _ButtonLeftStyle = value; }
			}
		}

		private EllipseButtonStyle _ButtonRightStyle = new EllipseButtonStyle();
		public EllipseButtonStyle ButtonRightStyle
		{
			get { return _ButtonRightStyle; }
			set
			{
				if(value == null) { _ButtonRightStyle = new EllipseButtonStyle(); }
				else { _ButtonRightStyle = value; }
			}
		}

		private bool _IndicatorVisiable = true;
		[DefaultValue(true)]
		public bool IndicatorVisiable
		{
			get { return _IndicatorVisiable; }
			set
			{
				_IndicatorVisiable = value;
				DrawBufferImage();
			}
		}

		protected string _ValueSymbol = "";
		[DefaultValue("")]
		public string ValueSymbol
		{
			get { return _ValueSymbol; }
			set { _ValueSymbol = value; }
		}

		protected bool _ValueSymbolIsBack = true;
		[DefaultValue(true)]
		public bool ValueSymbolIsBack
		{
			get { return _ValueSymbolIsBack; }
			set
			{
				_ValueSymbolIsBack = value;
				DrawBufferImage();
			}
		}

		private bool _ButtonSizeAuto = true;
		[DefaultValue(true)]
		public bool ButtonSizeAuto
		{
			get { return _ButtonSizeAuto; }
			set
			{
				_ButtonSizeAuto = value;
				CalControlBounce();
			}
		}

		private Size _ButtonSize = new Size(32, 32);
		public Size ButtonSize
		{
			get { return _ButtonSize; }
			set
			{
				if((value.Width < 1) || (value.Height < 1))
				{
					_ButtonSize = new Size(32, 32);
				}
				else
				{
					_ButtonSize = value;
				}
				CalControlBounce();
			}
		}

		/// <summary>
		/// Value를 사용자가 특정 스텝으로 바꿀 경우 사용 한다.
		/// arg1 - 목표값.
		/// arg2 - 현재값,
		/// arg3 - Maximum,
		/// arg4 - Minimum,
		/// return - 결과 값. 이 값에는 arg1이 포함 되어 있지 않아야 한다.
		/// </summary>
		public Func<int, int, int, int, int> CustomStep;

		/// <summary>
		/// 표시할 문자열을 사용자가 지정 한다.
		/// </summary>
		public Func<Longscroll,string> UserValueString;

		protected bool _UseCustomStep=false;

		/// <summary>
		/// Value의 변화가 필요할 경우 CustomStep을 호출하여 변화 시킨다.
		/// </summary>
		[DefaultValue(false)]
		public bool UseCustomStep
		{
			get { return _UseCustomStep; }
			set { _UseCustomStep = value; }
		}
		#endregion

		#region Event
		public event EventHandler ValueChanged;
		protected virtual void OnValueChanged()
		{
			if(ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		public Longscroll()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			//SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.StandardClick, true);
			//SetStyle(ControlStyles.StandardDoubleClick, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.UserMouse, true);

			repeatTimer = new Timer();
			repeatTimer.Interval = 200;
			repeatTimer.Tick += new EventHandler(repeatTimer_Tick);

			updateTimer = new Timer();
			updateTimer.Interval = 50;
			updateTimer.Tick += new EventHandler(updateTimer_Tick);

			//dfContext = BufferedGraphicsManager.Current;
			//dfContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			//dfGraphics = dfContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
		}

		protected virtual void updateTimer_Tick(object sender, EventArgs e)
		{
			if(targetValue != _Value)
			{
				if(_UseCustomStep)
				{
					targetValue = CustomStep(targetValue, _Value, _Maximum, _Minimum);
				}
				Value = targetValue;
			}
		}

		protected virtual void repeatTimer_Tick(object sender, EventArgs e)
		{
			int add;

			if(repetAccel < 10)
			{
				add = 1;
				repetAccel++;
			}
			else if(repetAccel == 10)	// Interval을 바꾸기 위해 존재.
			{
				repeatTimer.Interval = 100;
				add = 2;
				repetAccel++;
			}
			else
			{
				add = 2;
			}


			if((leftBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
			{
				if((leftBswm & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
				{
					targetValue = _Value - add;
				}
			}

			if((rightBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
			{
				if((rightBswm & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
				{
					targetValue = _Value + add;
				}
			}
		}

		#region Override
		protected override void OnPaint(PaintEventArgs e)
		{
			//Region clip = e.Graphics.Clip;
			//Region reg = new Region();
			//reg.MakeEmpty();
			//e.Graphics.Clip = reg;
			//e.Graphics.Clip = clip;

			//e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
			//e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			//e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			//e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//e.Graphics.Clear(this.BackColor);
			//// 버튼 그리기
			//lock (dfGraphics)
			//{
			//    dfGraphics.Render(e.Graphics);
			//}



			Graphics g = e.Graphics;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			Helper.GuiPainter.DrawButtonEllipse(g, leftRect, _ButtonLeftStyle.BackColor, _ButtonLeftStyle.ColorStart, _ButtonLeftStyle.ColorCenter, leftBswm);
			Helper.GuiPainter.DrawButtonEllipse(g, rightRect, _ButtonRightStyle.BackColor, _ButtonRightStyle.ColorStart, _ButtonRightStyle.ColorCenter, rightBswm);


			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

			// 조절 영역 그리기
			g.FillRectangle(new SolidBrush(_PanelColor), panelRect);
			ControlPaint.DrawBorder(g, panelRect, ControlPaint.LightLight(_PanelColor), ButtonBorderStyle.Outset);

			// 버튼 위에 이미지 얹기
			if (_LeftIcon != null)
			{
				Rectangle lRect = leftRect;
				lRect.Inflate((int)(leftRect.Width * (_ButtonLeftStyle.PaintRatio / 100d - 1d)), (int)(leftRect.Height * (_ButtonLeftStyle.PaintRatio / 100d - 1d)));
				lRect.Offset(_ButtonLeftStyle.ImageOffset);
				g.DrawImage(_LeftIcon, lRect);
			}
			if (_RightIcon != null)
			{
				Rectangle rRect = rightRect;
				rRect.Inflate((int)(rightRect.Width * (_ButtonRightStyle.PaintRatio / 100d - 1d)), (int)(rightRect.Height * (_ButtonRightStyle.PaintRatio / 100d - 1d)));
				rRect.Offset(_ButtonRightStyle.ImageOffset);
				g.DrawImage(_RightIcon, rRect);
			}

			int gapMaxMain = _Maximum - _Minimum;
			int panelWidthHalf = panelRect.Width / 2;

			Rectangle valueRect;
			if (_IndicatorVisiable)
			{
				// Indicator 그리기
				try
				{
					valueRect = new Rectangle(4 + (_Value - _Minimum) * (panelRect.Width - 8) / gapMaxMain + panelRect.Left - 2, 4, 4, this.Height - 8);
				}
				catch (DivideByZeroException)
				{
					valueRect = new Rectangle(panelRect.Left + panelWidthHalf - 2, panelRect.Top, 4, 4);
				}

				g.FillRectangle(new SolidBrush(revBru), valueRect);
				ControlPaint.DrawBorder(g, valueRect, ControlPaint.LightLight(revBru), ButtonBorderStyle.Inset);


				// 문자열 위치 잡기
				if (_Value < (gapMaxMain) / 2 + _Minimum)
				{
					valueRect = new Rectangle(panelRect.X + panelWidthHalf, panelRect.Y, panelWidthHalf, panelRect.Height);
				}
				else
				{
					valueRect = new Rectangle(panelRect.X, panelRect.Y, panelWidthHalf, panelRect.Height);
				}
			}
			else
			{
				valueRect = panelRect;
			}

			// 값 표시 문자열
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			string str = GetDisplayString();
			g.DrawString(str, this.Font, new SolidBrush(this.ForeColor), valueRect, sf);

			if (Focused) { ControlPaint.DrawFocusRectangle(g, this.ClientRectangle); }

			base.OnPaint(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Invalidate();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			CalControlBounce();

			//lock (dfGraphics)
			//{
			//    dfContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			//    dfGraphics = dfContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
			//}

			//System.Diagnostics.Debug.WriteLine(leftRect.ToString(), "leftRect");
			//System.Diagnostics.Debug.WriteLine(rightRect.ToString(), "rightRect");
			//System.Diagnostics.Debug.WriteLine(panelRect.ToString(), "panelRect");
		}

		protected void DrawBufferImage()
		{

			//lock (dfGraphics)
			//{
			//    Graphics g = dfGraphics.Graphics;
			//    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			//    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			//    //g.Clear(this.BackColor);

			//    Helper.GuiPainter.DrawButtonEllipse(g, leftRect, _ButtonLeftStyle.BackColor, _ButtonLeftStyle.ColorStart, _ButtonLeftStyle.ColorCenter, leftBswm);
			//    Helper.GuiPainter.DrawButtonEllipse(g, rightRect, _ButtonRightStyle.BackColor, _ButtonRightStyle.ColorStart, _ButtonRightStyle.ColorCenter, rightBswm);


			//    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
			//    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			//    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

			//    // 조절 영역 그리기
			//    g.FillRectangle(new SolidBrush(_PanelColor), panelRect);
			//    ControlPaint.DrawBorder(g, panelRect, ControlPaint.LightLight(_PanelColor), ButtonBorderStyle.Outset);

			//    // 버튼 위에 이미지 얹기
			//    if (_LeftIcon != null)
			//    {
			//        Rectangle lRect = leftRect;
			//        lRect.Inflate((int)(leftRect.Width * (_ButtonLeftStyle.PaintRatio / 100d - 1d)), (int)(leftRect.Height * (_ButtonLeftStyle.PaintRatio / 100d - 1d)));
			//        lRect.Offset(_ButtonLeftStyle.ImageOffset);
			//        g.DrawImage(_LeftIcon, lRect);
			//    }
			//    if (_RightIcon != null)
			//    {
			//        Rectangle rRect = rightRect;
			//        rRect.Inflate((int)(rightRect.Width * (_ButtonRightStyle.PaintRatio / 100d - 1d)), (int)(rightRect.Height * (_ButtonRightStyle.PaintRatio / 100d - 1d)));
			//        rRect.Offset(_ButtonRightStyle.ImageOffset);
			//        g.DrawImage(_RightIcon, rRect);
			//    }

			//    int gapMaxMain = _Maximum - _Minimum;
			//    int panelWidthHalf = panelRect.Width / 2;

			//    // 
			//    Rectangle valueRect;
			//    if (_IndicatorVisiable)
			//    {
			//        try
			//        {
			//            valueRect = new Rectangle(4 + (_Value - _Minimum) * (panelRect.Width - 8) / gapMaxMain + panelRect.Left - 2, 4, 4, this.Height - 8);
			//        }

			//        catch (DivideByZeroException)
			//        {
			//            valueRect = new Rectangle(panelRect.Left + panelWidthHalf - 2, panelRect.Top, 4, 4);
			//        }

			//        g.FillRectangle(new SolidBrush(revBru), valueRect);
			//        ControlPaint.DrawBorder(g, valueRect, ControlPaint.LightLight(revBru), ButtonBorderStyle.Inset);
			//    }

			//    // 값 표시 문자열
			//    StringFormat sf = new StringFormat();
			//    sf.Alignment = StringAlignment.Center;
			//    sf.LineAlignment = StringAlignment.Center;

			//    if (_IndicatorVisiable)
			//    {
			//        if (_Value < (gapMaxMain) / 2 + _Minimum)
			//        {
			//            valueRect = new Rectangle(panelRect.X + panelWidthHalf, panelRect.Y, panelWidthHalf, panelRect.Height);
			//        }
			//        else
			//        {
			//            valueRect = new Rectangle(panelRect.X, panelRect.Y, panelWidthHalf, panelRect.Height);
			//        }
			//    }
			//    else
			//    {
			//        valueRect = panelRect;
			//    }

			//    string str = GetDisplayString();
			//    g.DrawString(str, this.Font, new SolidBrush(this.ForeColor), valueRect, sf);
			//}

			this.Invalidate();


		}

		private void CalControlBounce()
		{
			if (_ButtonSizeAuto)
			{
				leftRect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Height - 1, this.ClientRectangle.Height - 1);
				rightRect = new Rectangle(leftRect.Right + Padding.Horizontal, this.ClientRectangle.Top, this.ClientRectangle.Height - 1, this.ClientRectangle.Height - 1);
				panelRect = new Rectangle(rightRect.Right + Padding.Horizontal, this.ClientRectangle.Top, this.ClientRectangle.Width - rightRect.Right - this.Padding.Horizontal, this.ClientRectangle.Height);
			}
			else
			{
				leftRect = new Rectangle(this.ClientRectangle.Left, (this.ClientRectangle.Height - _ButtonSize.Height) / 2, this._ButtonSize.Width - 1, this._ButtonSize.Height - 1);
				rightRect = new Rectangle(leftRect.Right + Padding.Horizontal, leftRect.Top, this._ButtonSize.Width - 1, this._ButtonSize.Height - 1);
				panelRect = new Rectangle(rightRect.Right + Padding.Horizontal, this.ClientRectangle.Top, this.ClientRectangle.Width - rightRect.Right - this.Padding.Horizontal, this.ClientRectangle.Height);
			}

			//System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			//gp.AddEllipse(leftRect);
			//gp.StartFigure();
			//gp.AddEllipse(rightRect);
			//gp.StartFigure();
			//gp.AddRectangle(panelRect);
			//gp.CloseAllFigures();
			//this.Region = new Region(gp);


		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(leftRect.Contains(e.Location))
			{
				leftBswm |= ButtonStatesWithMouse.ButtonPush;
				repeatTimer.Interval = 200;
				repetAccel = 0;
				repeatTimer.Start();
				updateTimer.Start();
			}
			else if(rightRect.Contains(e.Location))
			{
				rightBswm |= ButtonStatesWithMouse.ButtonPush;
				repeatTimer.Interval = 200;
				repetAccel = 0;
				repeatTimer.Start();
				updateTimer.Start();
			}
			else if(panelRect.Contains(e.Location))
			{
				panelBswm |= ButtonStatesWithMouse.ButtonPush;
				targetValue = _Value;
			}

			repeatTimer_Tick(this, EventArgs.Empty);

			DrawBufferImage();

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{

			repeatTimer.Stop();
			updateTimer.Stop();

			// panel이 눌린 상태 라면.
			if((panelBswm & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
			{
				if((panelBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
				{
					// 기존에 panel모든가 선택 되지 않았다면 모드 선택 함.
					if(panelMode == PanelSelectionEnum.Non)
					{
						if(panelRect.Contains(e.Location))
						{
							switch(e.Button)
							{
							case MouseButtons.Left:
								panelMode = PanelSelectionEnum.Left;
								MouseClip();
								break;
							case MouseButtons.Right:
								panelMode = PanelSelectionEnum.Right;
								MouseClip();
								break;
							}
						}
					}
					// 기존에 panel 모드가 선택 되었다면 해제 함.
					else
					{
						panelMode = PanelSelectionEnum.Non;
						MouseRelease();
					}
				}
			}

			leftBswm &= ~ButtonStatesWithMouse.ButtonPush;
			rightBswm &= ~ButtonStatesWithMouse.ButtonPush;
			panelBswm &= ~ButtonStatesWithMouse.ButtonPush;

			if(leftRect.Contains(e.Location))
			{
				leftBswm |= ButtonStatesWithMouse.MouseHover;
			}
			else if(rightRect.Contains(e.Location))
			{
				rightBswm |= ButtonStatesWithMouse.MouseHover;
			}
			else if(panelRect.Contains(e.Location))
			{
				panelBswm |= ButtonStatesWithMouse.MouseHover;
			}
			// control 밖의 영역 일 수 있음.
			//else
			//{
			//    throw new ArgumentException("정의 되지 않은 지점.");
			//}

			DrawBufferImage();

			if(Parent != null)
			{
				if(Parent.ContextMenuStrip != null)
				{
					if(Parent.ContextMenuStrip.Visible)
					{
						Parent.ContextMenuStrip.Visible = false;
					}
				}
			}

			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// 기존에 눌린 버튼이 있음.
			if(((leftBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
				|| ((rightBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
				|| ((panelBswm & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush))
			{

				return;
			}
			// panel 모드가 동작 중.
			else if(panelMode != PanelSelectionEnum.Non)
			{
				PanelMoveOper(e);
				return;
			}
			else
			{
				leftBswm &= ~ButtonStatesWithMouse.MouseHover;
				rightBswm &= ~ButtonStatesWithMouse.MouseHover;
				panelBswm &= ~ButtonStatesWithMouse.MouseHover;

				if(leftRect.Contains(e.Location))
				{
					leftBswm |= ButtonStatesWithMouse.MouseHover;
				}
				else if(rightRect.Contains(e.Location))
				{
					rightBswm |= ButtonStatesWithMouse.MouseHover;
				}
				else if(panelRect.Contains(e.Location))
				{
					panelBswm |= ButtonStatesWithMouse.MouseHover;
				}
			}
			//Refresh();

			this.Invalidate();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			if(this.Enabled)
			{
				if(_Value == _Minimum) { leftBswm |= ButtonStatesWithMouse.Disabled; }
				else { leftBswm &= ~ButtonStatesWithMouse.Disabled; }

				if(_Value == _Maximum) { rightBswm |= ButtonStatesWithMouse.Disabled; }
				else { rightBswm &= ~ButtonStatesWithMouse.Disabled; }

				panelBswm &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				leftBswm |= ButtonStatesWithMouse.Disabled;
				rightBswm |= ButtonStatesWithMouse.Disabled;
				panelBswm |= ButtonStatesWithMouse.Disabled;
			}

			base.OnEnabledChanged(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

            if (panelMode == PanelSelectionEnum.Left)
            {
                return;
            }


			leftBswm &= ~ButtonStatesWithMouse.MouseHover;
			rightBswm &= ~ButtonStatesWithMouse.MouseHover;
			panelBswm &= ~ButtonStatesWithMouse.MouseHover;

			if(panelMode != PanelSelectionEnum.Non)
			{
				MouseRelease();
				panelMode = PanelSelectionEnum.Non;
			}

			DrawBufferImage();
		}
		#endregion

		protected virtual string GetDisplayString()
		{
			switch(_ValueView)
			{
			default:
				throw new NotSupportedException();
			case ValueViewMode.Non:	// 표시 하는것 없음.
				return "";
			case ValueViewMode.Number:
				if(_ValueSymbolIsBack)
				{
					return _Value.ToString("F" + _DecimalPlace.ToString()) + _ValueSymbol;
				}
				else
				{
					return _ValueSymbol + _Value.ToString("F" + _DecimalPlace.ToString());
				}
			case ValueViewMode.Percent:
				return ((_Value * 100f / _Maximum).ToString("F2") + "%");
			case ValueViewMode.AbsolutePerccent:
				return (((_Value - _Minimum) * 100f / (_Maximum - _Minimum)).ToString("F2") + "%");
			case ValueViewMode.User:
				if (UserValueString != null)
				{
					string str = UserValueString(this);
					if (_ValueSymbolIsBack) { str += _ValueSymbol; }
					else { str = _ValueSymbol + str; }
					return str;
				}
				else
				{
					return "";
				}
			}
		}

		#region Panel Mouse Move
		Rectangle preClip;
		Rectangle clipRect;
		Point preMousePnt;
		float mouseAccum;

		private void PanelMoveOper(MouseEventArgs e)
		{
			if(e.X < clipRect.X + 2)
			{
				if((leftBswm & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
				{
					Cursor.Position = this.PointToScreen(new Point(clipRect.Right - 3, e.Y));
				}
			}
			else if(e.X >= clipRect.Right - 2)
			{
				if((rightBswm & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
				{
					Cursor.Position = this.PointToScreen(new Point(clipRect.X + 3, e.Y));
				}
			}
			else
			{
				switch(panelMode)
				{
				case PanelSelectionEnum.Left:
					mouseAccum += (float)((e.X - preMousePnt.X) / (double)_DividInner);
					break;
				case PanelSelectionEnum.Right:
					mouseAccum += (float)((e.X - preMousePnt.X) / (double)_DividOutter);
					break;
				}

				int val = (int)Math.Truncate(mouseAccum);
				mouseAccum -= val;

				int tmp = targetValue + val;

				if(tmp < _Minimum)
				{
					targetValue = _Minimum;
				}
				else if(tmp > _Maximum)
				{
					targetValue = _Maximum;
				}
				else
				{
					targetValue = tmp;
				}
			}

			preMousePnt = this.PointToClient(Cursor.Position);
		}

		private void MouseRelease()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Cursor.Clip = preClip;


			//System.Diagnostics.Debug.WriteLine(preClip.ToString(), "MouseRelease");

			this.Capture = false;

			updateTimer.Stop();


			this.BeginInvoke(actCursorChange, this, Cursors.Default);
			sw.Stop();
			Debug.WriteLine("MouseRelease " + sw.ElapsedTicks.ToString(), "Longscroll");
		}

		Action<Control, Cursor> actCursorChange = (con, cur) =>
		{
			con.Cursor = cur;
		};

		private void MouseClip()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			mouseAccum = 0;

			this.Capture = true;

			clipRect = panelRect;
			clipRect.Inflate(0, (8 - panelRect.Height) / 2);
			preClip = Cursor.Clip;

			//System.Diagnostics.Debug.WriteLine(preClip.ToString(), "MouseClip");

			Cursor.Clip = this.RectangleToScreen(clipRect);

			

			preMousePnt = this.PointToClient(Cursor.Position);

			updateTimer.Start();


			this.BeginInvoke(actCursorChange,this,Cursors.NoMoveHoriz);

			sw.Stop();
			Debug.WriteLine("MouseClip " + sw.ElapsedTicks.ToString(), "Longscroll");

		}
		#endregion

		#region 증감
		public void PerformIncrease()
		{
			if(!updateTimer.Enabled)
			{
				targetValue = _Value + 1;
				updateTimer_Tick(updateTimer, EventArgs.Empty);
			}
		}

		public void PerformDecrease()
		{
			if(!updateTimer.Enabled)
			{
				targetValue = _Value - 1;
				updateTimer_Tick(updateTimer, EventArgs.Empty);
			}
		}
		#endregion

		#region 각종 Enum
		enum PanelSelectionEnum
		{
			Non,
			Left,
			Right
		}

		public enum ValueViewMode
		{
			Non,
			Number,
			Percent,
			AbsolutePerccent,
			User
		}

		/// <summary>
		/// 값을 증감 시키는 방법을 선택 한다.
		/// </summary>
		public enum ValueSelectMode
		{
			/// <summary>
			/// 기본적인 증감점. 
			/// 1씩 증감 한다.
			/// </summary>
			Default,
			/// <summary>
			/// 사용자 정의 방법.
			/// "Func... CustomStep"을 호출하여 증감 한다.
			/// </summary>
			CustumStep
		}
		#endregion
	}
}
