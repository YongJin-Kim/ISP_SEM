using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using System.Diagnostics;

namespace SEC.GUIelement
{
	public partial class FocusControl : Control
	{
		#region Property
		protected int _InnerMaximum = 100;
		[DefaultValue(100)]
		public int InnerMaximum
		{
			get { return _InnerMaximum; }
			set
			{
				_InnerMaximum = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		protected int _InnerMinimum = 0;
		[DefaultValue(0)]
		public int InnerMinimum
		{
			get { return _InnerMinimum; }
			set
			{
				_InnerMinimum = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		protected int _OutterMaximum = 100;
		[DefaultValue(100)]
		public int OutterMaximum
		{
			get { return _OutterMaximum; }
			set
			{
				_OutterMaximum = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		protected int _OutterMinimum = 0;
		[DefaultValue(0)]
		public int OutterMinimum
		{
			get { return _OutterMinimum; }
			set
			{
				_OutterMinimum = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		protected int _InnerValue = 0;
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual int InnerValue
		{
			get { return _InnerValue; }
			set
			{
				_InnerValue = value;
				if ( _InnerValue >= _InnerMaximum )
				{
					_InnerValue = _InnerMaximum;
				}
				if ( _InnerValue < _InnerMinimum )
				{
					_InnerValue = _InnerMinimum;
				}
				//GenerateBackground();
				//PerformLayout();
				OnInnerValueChanged();
				this.Invalidate();
			}

		}

		protected int _OutterValue = 0;
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual int OutterValue
		{
			get { return _OutterValue; }
			set
			{
				_OutterValue = value;
				if ( _OutterValue > _OutterMaximum )
				{
					_OutterValue = _OutterMaximum ;
				}
				if ( _OutterValue < _OutterMinimum )
				{
					_OutterValue = _OutterMinimum ;
				}
				//GenerateBackground();
				//PerformLayout();
				OnOutterValueChanged();
				this.Invalidate();
			}
		}

		private int _GageWidth = 12;
		[DefaultValue(12)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int GageWidth
		{
			get { return _GageWidth; }
			set
			{
				_GageWidth = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		private float _GageStartAngle = 135;
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(135F)]
		public float GageStartAngle
		{
			get { return _GageStartAngle; }
			set
			{
				_GageStartAngle = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		private float _GageSweepAngle = 270;
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(270F)]
		public float GageSweepAngle
		{
			get { return _GageSweepAngle; }
			set
			{
				_GageSweepAngle = value;
				GenerateBackground();
				//PerformLayout();
			}

		}

		private Color _InnerGageBackColor = Color.White;
		[DefaultValue(typeof(Color), "White")]
		public Color InnerGageBackColor
		{
			get { return _InnerGageBackColor; }
			set { _InnerGageBackColor = value; }
		}

		private Color _InnerGageForeColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color InnerGageForeColor
		{
			get { return _InnerGageForeColor; }
			set { _InnerGageForeColor = value; }
		}

		private Color _OutterGageBackColor = Color.White;
		[DefaultValue(typeof(Color), "White")]
		public Color OutterGageBackColor
		{
			get { return _OutterGageBackColor; }
			set { _OutterGageBackColor = value; }
		}

		private Color _OutterGageForeColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color OutterGageForeColor
		{
			get { return _OutterGageForeColor; }
			set { _OutterGageForeColor = value; }
		}

		private Color _CircleNormalColor = Color.BlueViolet;
		[DefaultValue(typeof(Color), "BlueViolet")]
		public Color CircleNormalColor
		{
			get { return _CircleNormalColor; }
			set
			{
				_CircleNormalColor = value;
				GenerateBackground();
				//PerformLayout();
			}
		}

		private Color _CircleHoverColor = Color.Aquamarine;
		[DefaultValue(typeof(Color), "Aquamarine")]
		public Color CircleHoverColor
		{
			get { return _CircleHoverColor; }
			set
			{
				_CircleHoverColor = value;
				GenerateBackground();
				//PerformLayout();
			}
		}

		private Color _CirclePushColor = Color.Red;
		[DefaultValue(typeof(Color), "Red")]
		public Color CirclePushColor
		{
			get { return _CirclePushColor; }
			set
			{
				_CirclePushColor = value;
				GenerateBackground();
				//PerformLayout();
			}
		}


		private int _SensitivityMode = 1;
		public int SensitivityMode
		{
			get { return _SensitivityMode; }
			set
			{
				int tmp;

				if ( value < 1 )
				{
					tmp = 0;
				}
				else if ( value > 2 )
				{
					tmp = 2;
				}
				else
				{
					tmp = value;
				}
				_SensitivityMode = tmp;
			}
		}

		private decimal _SensitivityInner1 = 0;
		public decimal SensitivityInner1
		{
			get { return _SensitivityInner1; }
			set { _SensitivityInner1 = value; }
		}

		private decimal _SensitivityInner2 = 0;
		public decimal SensitivityInner2
		{
			get { return _SensitivityInner2; }
			set { _SensitivityInner2 = value; }
		}
		private decimal _SensitivityInner3 = 0;
		public decimal SensitivityInner3
		{
			get { return _SensitivityInner3; }
			set { _SensitivityInner3 = value; }
		}

		private decimal _SensitivityOutter1 = 0;
		public decimal SensitivityOutter1
		{
			get { return _SensitivityOutter1; }
			set { _SensitivityOutter1 = value; }
		}
		private decimal _SensitivityOutter2 = 0;
		public decimal SensitivityOutter2
		{
			get { return _SensitivityOutter2; }
			set { _SensitivityOutter2 = value; }
		}
		private decimal _SensitivityOutter3 = 0;
		public decimal SensitivityOutter3
		{
			get { return _SensitivityOutter3; }
			set { _SensitivityOutter3 = value; }
		}

		/* ACTIVATION
		private Color _CircleActiveColor = Color.Yellow;
		[DefaultValue(typeof(Color), "Yellow")]
		public Color CircleActiveColor
		{
			get { return _CircleActiveColor; }
			set
			{
				_CircleActiveColor = value;
				GenerateBackground();
				//PerformLayout();
			}
		}

		private bool _Activation = false;
		[DefaultValue(false)]
		public bool Activation
		{
			get { return _Activation; }
			set
			{
				_Activation = value;
				ActivationChange();
			}
		}
		*/
		#endregion

		#region Event
		public event EventHandler InnerValueChanged;
		protected virtual void OnInnerValueChanged()
		{
			if  (InnerValueChanged != null) 
			{
				InnerValueChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler OutterValueChanged;
		protected virtual void OnOutterValueChanged()
		{
			if  (OutterValueChanged != null) 
			{
				OutterValueChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		StringFormat textSF = new StringFormat();

		public FocusControl()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			circleCol = _CircleNormalColor;
			//activeTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ActiveProc));

			textSF.Alignment = StringAlignment.Center;
			textSF.LineAlignment = StringAlignment.Center;

		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Brush circleBru;
			Color tarCol;
			/*if ( _Activation )
			{
				tarCol = circleCol;
			}
			else*/
			if ( mouseCapture )
			{
				tarCol = _CircleNormalColor;
			}
			else
			{
				if ( circleRect.Contains(this.PointToClient(Cursor.Position)) )
				{
					if ( (MouseButtons == MouseButtons.Left) || (MouseButtons == MouseButtons.Right) )
					{
						tarCol = _CirclePushColor;
					}
					else
					{
						tarCol = _CircleHoverColor;
					}
				}
				else
				{
					tarCol = _CircleNormalColor;
				}
			}
			CompositingQuality preCQ =pe.Graphics.CompositingQuality;
			InterpolationMode preIM = pe.Graphics.InterpolationMode;
			SmoothingMode preSM = pe.Graphics.SmoothingMode;
			pe.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
			pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if ( backBm != null )
			{
				pe.Graphics.DrawImage(backBm, this.ClientRectangle);
			}

			if ( (circleRect.Width != 0) && (circleRect.Height != 0) )
			{


				circleBru = new LinearGradientBrush(circleRect, ControlPaint.LightLight(tarCol), ControlPaint.Dark(tarCol), 45f);
				pe.Graphics.FillEllipse(circleBru, circleRect);
				circleBru.Dispose();

				PathGradientBrush pgb;
				GraphicsPath gp = new GraphicsPath();

				// 버튼을 채울 브러쉬를 초기화 합니다.
				// gp = new GraphicsPath();
				gp.AddEllipse(circleRect);
				pgb = new PathGradientBrush(gp);
				gp.Dispose();


				pgb.SurroundColors = new Color[] { ControlPaint.Dark(tarCol) };

				pgb.FocusScales = new PointF(0.1F, 0.1F);
				pgb.CenterPoint = new PointF(circleRect.Left + circleRect.Width / 4, circleRect.Top + circleRect.Height / 4);
				ControlPaint.Light(tarCol);
				pe.Graphics.FillEllipse(pgb, circleRect);
				pgb.Dispose();
				pe.Graphics.DrawEllipse(Pens.Black, circleRect);




				pe.Graphics.DrawArc(outterGagePen, outterGageRect, _GageStartAngle, _GageSweepAngle * (_OutterValue - _OutterMinimum) / (_OutterMaximum - _OutterMinimum));
				pe.Graphics.DrawArc(innerGagePen, innerGageRect, _GageStartAngle, _GageSweepAngle * (_InnerValue - _InnerMinimum) / (_InnerMaximum - _InnerMinimum));

				pe.Graphics.DrawString(_SensitivityMode.ToString(), this.Font, new SolidBrush(this.ForeColor), circleRect, textSF);
			}


			pe.Graphics.CompositingQuality = preCQ;
			pe.Graphics.InterpolationMode = preIM;
			pe.Graphics.SmoothingMode = preSM;
			base.OnPaint(pe);
		}

		bool mouseCapture = false;

		//protected override void OnMouseDown(MouseEventArgs e)
		//{
		//    base.OnMouseDown(e);
		//    if ( circleRect.Contains(e.Location) )
		//    {
		//        MouseCaptureSelect();
		//    }
		//}

		//protected override void OnMouseUp(MouseEventArgs e)
		//{
		//    base.OnMouseUp(e);
		//    if ( mouseCapture )
		//    {
		//        MouseCaptureSelect();
		//    }
		//}

		protected override void OnMouseClick(MouseEventArgs e)
		{
            

			base.OnMouseClick(e);

			if ( mouseCapture )
			{
				MouseCaptureSelect();
			}
			else
			{
				switch ( e.Button )
				{
				case MouseButtons.Left:
					capMode = 0;
					MouseCaptureSelect();
					break;
				case MouseButtons.Right:
					capMode = 1;
					MouseCaptureSelect();
					break;
				case MouseButtons.Middle:
					_SensitivityMode++;
					if ( _SensitivityMode > 2 )
					{
						_SensitivityMode = 0;
					}
					this.Invalidate();
					break;
				}
			}
		}

		int capMode = 0;
		int premouseposX;
		double moveAccum;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if ( mouseCapture )
			{
				switch ( capMode )
				{
				case 0:
					if ( e.X < outterGageRect.Left + 3 )
					{
						if ( _InnerValue > _InnerMinimum )
						{
							premouseposX = outterGageRect.Right - 4;
							Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
						}
					}
					else if ( e.X > outterGageRect.Right - 4 )
					{
						if ( _InnerValue <  _InnerMaximum)
						{
							premouseposX = outterGageRect.Left + 3;
							Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
						}
					}
					else
					{
						int truncateValue = 0;
						switch ( _SensitivityMode )
						{
						case 0:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityInner1);
							break;
						case 1:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityInner2);
							break;
						case 2:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityInner3);
							break;
						}
						truncateValue = (int)Math.Truncate(moveAccum);
						if ( truncateValue != 0 )
						{
							moveAccum -= truncateValue;
							this.InnerValue += truncateValue;
						}
						premouseposX = e.X;
					}
					break;
				case 1:
					if ( e.X < outterGageRect.Left + 3 )
					{
						if ( _OutterValue >_OutterMinimum+1  )
						{
							premouseposX = outterGageRect.Right - 4;
							Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
						}
					}
					else if ( e.X > outterGageRect.Right - 4 )
					{
						if ( _OutterValue < _OutterMaximum -1)
						{
							premouseposX = outterGageRect.Left + 3;
							Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
						}
					}
					else
					{
						int truncateValue = 0;
						switch ( _SensitivityMode )
						{
						case 0:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityOutter1);
							break;
						case 1:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityOutter2);
							break;
						case 2:
							moveAccum += (double)((e.X - premouseposX) / _SensitivityOutter3);
							break;
						}
						truncateValue = (int)Math.Truncate(moveAccum);
						if ( truncateValue != 0 )
						{
							moveAccum -= truncateValue;
							this.OutterValue += truncateValue;
						}
						premouseposX = e.X;
					}
					break;
				}
			}
			else
			{
				if ( circleRect.Contains(this.PointToClient(Cursor.Position)) )
				{
					if ( !mouseEnter )
					{
						mouseEnter = true;
						this.Invalidate();
					}
				}
				else
				{
					if ( mouseEnter )
					{
						mouseEnter = false;
						this.Invalidate();
					}
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if ( mouseEnter )
			{
				mouseEnter = false;
				this.Invalidate();

			}
			base.OnMouseLeave(e);
		}

		bool mouseEnter = false;

		Rectangle preClipRect;

		private void MouseCaptureSelect()
		{
			if ( mouseCapture )
			{
				mouseCapture = false;
				Cursor.Clip = preClipRect;
				Cursor = Cursors.Default;
				this.Capture = false;
			}
			else
			{
				this.Capture = true;
				mouseCapture = true;
				preClipRect = Cursor.Clip;
				Rectangle clipRect = outterGageRect;
				clipRect.Inflate(-2, -2);
				Cursor.Clip = this.RectangleToScreen(clipRect);
				Cursor = Cursors.NoMoveHoriz;
				moveAccum = 0;
				premouseposX = this.PointToClient(Cursor.Position).X;
			}

			this.Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			GenerateBackground();
		}

		/* Activation
		bool buttonDown = false;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ( (e.Button == MouseButtons.Left) && (circleRect.Contains(this.PointToClient(Cursor.Position))) )
			{
				this.Invalidate();
				buttonDown = true;
			}
			Capture = true;
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ( circleRect.Contains(this.PointToClient(Cursor.Position)) )
			{
				this.Invalidate();

				if ( buttonDown )
				{
					MouseHooking();
				}
				else
				{
					Capture = false;
				}
			}
			else
			{
				Capture = false;
			}
			buttonDown = false;
		}

		bool mouseEnter = false;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if ( circleRect.Contains(this.PointToClient(Cursor.Position)) )
			{
				if ( !mouseEnter )
				{
					mouseEnter = true;
					this.Invalidate();
				}
			}
			else if ( (mouseEnter) && (!buttonDown) )
			{
				mouseEnter = false;
				this.Invalidate();
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if ( !buttonDown )
			{
				mouseEnter = false;
				this.Invalidate();

			}
			base.OnMouseLeave(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			GenerateBackground();
		}

		//protected override void OnLayout(LayoutEventArgs levent)
		//{
		//    base.OnLayout(levent);

		//}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			GenerateBackground();
		}
		*/
		Bitmap backBm = null;
		Rectangle outterGageRect;
		Rectangle innerGageRect;
		Pen outterGagePen;
		Pen innerGagePen;
		private void GenerateBackground()
		{
			//if ( this.Container == null ) { return; }
			if ( this.Parent == null ) { return; }
			if ( this.Width == 0 ) { return; }
			// Controler의 영역을 결정 한다.
			//Rectangle regioRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
			//GraphicsPath gp = new GraphicsPath();
			//gp.AddEllipse(regioRect);
			//this.Region = new Region(gp);
			//gp.Dispose();

			// Background Image
			Bitmap bm = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			Graphics g = Graphics.FromImage(bm);

			CompositingQuality preCQ =g.CompositingQuality;
			InterpolationMode preIM = g.InterpolationMode;
			SmoothingMode preSM = g.SmoothingMode; ;
			g.CompositingQuality = CompositingQuality.GammaCorrected;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.SmoothingMode = SmoothingMode.AntiAlias;


			//g.Clear(Color.Transparent);
	
			// 배경 칠하기
			//Brush backSolid = new SolidBrush(this.BackColor);
			//g.FillEllipse(backSolid, regioRect);
			//g.Clear(this.ForeColor);
			g.Clear(Color.Transparent);
			//g.FillEllipse(new SolidBrush(Color.FromArgb(200, this.ForeColor)), this.ClientRectangle);


			Color backDark = ControlPaint.Dark(this.BackColor);
			Color backLight = ControlPaint.Light(this.BackColor);
			Rectangle innerRect = new Rectangle((bm.Width - bm.Height) / 2, 0, bm.Height, bm.Height);
			Brush innerSicle = new LinearGradientBrush(new PointF(innerRect.X, 0), new PointF(innerRect.Right, innerRect.Bottom), backDark, backLight);


			// 외곽 상자 그리기
			//Rectangle blankRect = new Rectangle(innerRect.X-14, this.ClientSize.Height / 2 - 12, 6, 24);
			//g.FillRectangle(new SolidBrush(Color.White), blankRect);
			//ControlPaint.DrawBorder(g, blankRect, Color.White, ButtonBorderStyle.Inset);

			//blankRect = new Rectangle(innerRect.Right+8, this.ClientSize.Height / 2 - 12, 6, 24);
			//g.FillRectangle(new SolidBrush(Color.White), blankRect);
			//ControlPaint.DrawBorder(g, blankRect, Color.White, ButtonBorderStyle.Outset);



			// 내부 엣지 효과
			g.FillEllipse(innerSicle, innerRect);

			// 내부 타원
			innerRect.Inflate(_GageWidth / -2 + 2, _GageWidth / -2 + 2);
			g.FillEllipse(Brushes.Gray, innerRect);
			
			
			innerRect.Inflate(_GageWidth / -2 - 2, _GageWidth / -2 - 2);

			Pen pbase = new Pen(_OutterGageBackColor, _GageWidth);
			outterGagePen = new Pen(_OutterGageForeColor, (int)(_GageWidth * 0.8));

			pbase.StartCap = LineCap.Round;
			pbase.EndCap = LineCap.Round;

			// 바깥 개이지 그리기
			g.DrawArc(pbase, innerRect, _GageStartAngle, _GageSweepAngle);
			outterGageRect = innerRect;
			//g.DrawArc( (pSweep, innerRect, _GageStartAngle, _GageSweepAngle * (_OutterValue - _OutterMinimum) / (_OutterMaximum - _OutterMinimum));
			pbase.Dispose();
			//pSweep.Dispose();


			innerRect.Inflate(_GageWidth * -1, _GageWidth * -1);
			pbase = new Pen(_InnerGageBackColor, _GageWidth);
			innerGagePen = new Pen(_InnerGageForeColor, (int)(_GageWidth * 0.8));

			pbase.StartCap = LineCap.Round;
			pbase.EndCap = LineCap.Round;

			// 안쪽 게이지
			g.DrawArc(pbase, innerRect, _GageStartAngle, _GageSweepAngle);
			innerGageRect = innerRect;
			//g.DrawArc(pSweep, innerRect, _GageStartAngle, _GageSweepAngle * (_InnderValue - _InnderMinimum) / (_InnderMaximum - _InnderMinimum));
			pbase.Dispose();
			//pSweep.Dispose();

			innerRect.Inflate(_GageWidth / -2, _GageWidth / -2);
			circleRect = innerRect;

			innerSicle.Dispose();
			//backSolid.Dispose();


			g.Dispose();

			backBm = bm;
			//if ( this.InvokeRequired )
			//{
			//    this.InvokePaint(this, new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
			//}
			//else
			//{
			//    this.Invalidate();
			//}
			//this.Refresh();
		}

		Rectangle circleRect;
		Color circleCol;

		#region Activation
		/*
		int actCnt = 0;
		System.Threading.Timer activeTimer;

		private void ActiveProc(object obj)
		{
			actCnt++;
			int drawCnt;
			if ( actCnt < 5 )
			{
				drawCnt = actCnt;
			}
			else
			{
				drawCnt = 10 - actCnt;
				if ( actCnt == 10 )
				{
					actCnt = 0;
				}
			}

			circleCol = Color.FromArgb(255,
				(_CircleNormalColor.R - _CircleActiveColor.R) * drawCnt / 5 + _CircleActiveColor.R,
				(_CircleNormalColor.G - _CircleActiveColor.G) * drawCnt / 5 + _CircleActiveColor.G,
				(_CircleNormalColor.B - _CircleActiveColor.B) * drawCnt / 5 + _CircleActiveColor.B);
			//this.Invalidate(circleRect);
			this.BeginInvoke(new Action<Rectangle>(this.Invalidate), new object[] { circleRect });
		}

		private void ActivationChange()
		{
			if ( _Activation )
			{
				activeTimer.Change(0, 150);
				actCnt = 0;
			}
			else
			{
				activeTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
				this.Invalidate();
			}
		}
		*/
		#endregion

		#region Mouse Hooking
		/*
		int mouserPreLocation = 0;

		public int hHook = 0;
		SEC.MSWindowsAPI.User32.HookMouseProc MouseHookProcedure;

		public static FocusControl ms;

		public static double mouseMoveAccum = 0;


		private void MouseHooking()
		{
			ms = this;
			mouserPreLocation = MousePosition.X;
			//this.Capture = true;

			MouseHookProcedure = new SEC.MSWindowsAPI.User32.HookMouseProc(MouseHookProc);

			RuntimeTypeHandle rt = Type.GetTypeHandle(this);

			//IntPtr hInstance = SEC.MSWindowsAPI.Kernel32.LoadLibrary("User32");
			IntPtr hInstance = SEC.MSWindowsAPI.Kernel32.LoadLibrary(AppDomain.CurrentDomain.FriendlyName);


			if ( hHook != 0 )
			{
				throw new Exception();
			}

			hHook = SEC.MSWindowsAPI.User32.SetWindowsHookEx(7,
						MouseHookProcedure,
						hInstance,
				//Process.GetCurrentProcess().Id);
				//System.Threading.Thread.CurrentThread.ManagedThreadId);
				//AppDomain.CurrentDomain.Id);
				0);
			if ( hHook == 0 )
			{

				//MessageBox.Show("SetWindowsHookEx Failed");

				int err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

				Debug.WriteLine("Hooking Error");
				return;
			}

			Debug.WriteLine("Mouse Hooked");

			//this.Capture = true;
			Cursor.Clip = this.TopLevelControl.RectangleToScreen(this.TopLevelControl.ClientRectangle);
			_Activation = true;
			ActivationChange();
		}

		private void UnHookMouseMove(string mode)
		{
			if ( hHook != 0 )
			{
				SEC.MSWindowsAPI.User32.UnhookWindowsHookEx(hHook);
				Cursor.Clip = new Rectangle();
				Capture = false;
				Debug.WriteLine("Mouse UnHook." + mode);
				hHook = 0;
			}

			_Activation = false;
			ActivationChange();

		}

		static bool hookprocessed = false;

		public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{

			if ( nCode < 0 )
			{
				return SEC.MSWindowsAPI.User32.CallNextHookEx(ms.hHook, nCode, wParam, lParam);
			}
			try
			{
				if ( !hookprocessed )
				{
					hookprocessed = true;
					double roundValue;
					SEC.MSWindowsAPI.User32.MouseHookStruct mhs;
					switch ( (int)wParam )
					{
					case SEC.MSWindowsAPI.WindowsMessage.WM_MOUSEMOVE:
						mhs = (SEC.MSWindowsAPI.User32.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(SEC.MSWindowsAPI.User32.MouseHookStruct));
						double divider = 1;

						switch ( MouseButtons )
						{
						case MouseButtons.Left:
							divider = 5;
							mouseMoveAccum += (double)(ms.mouserPreLocation - mhs.pt.x) / divider;
							roundValue = Math.Round(mouseMoveAccum);
							if ( roundValue == 0 ) { break; }
							mouseMoveAccum -= roundValue;
							ms.InnderValue -= (int)roundValue;
							break;
						case MouseButtons.Right:
							divider = 5;
							mouseMoveAccum += (double)(ms.mouserPreLocation - mhs.pt.x) / divider;
							roundValue = Math.Round(mouseMoveAccum);
							if ( roundValue == 0 ) { break; }
							mouseMoveAccum -= roundValue;
							//ms.Value -= (int)roundValue;

							if ( roundValue != 0 )
							{
								ms.OutterValue -= (int)roundValue;
							}
							break;
						}

						ms.mouserPreLocation = mhs.pt.x;
						break;
					case SEC.MSWindowsAPI.WindowsMessage.WM_MBUTTONUP:
						ms.UnHookMouseMove("WM_MBUTTONDBLCLK");
						break;
					//default:
					//    return SEC.MSWindowsAPI.User32.CallNextHookEx(ms.hHook, nCode, (IntPtr)wParam, (IntPtr)lParam);
					}
					hookprocessed = false;
				}
			}
			catch ( Exception  )
			{
				ms.UnHookMouseMove("MoouseHookProc Exception.");
			}
			return 1;
		}

		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			//if ( !Capture )
			//{
			//    UnHookMouseMove("OnMouseCaptureChanged");
			//}
			base.OnMouseCaptureChanged(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			UnHookMouseMove("OnLostFocus");
			base.OnLostFocus(e);
		}

		*/
		#endregion


	}

}
