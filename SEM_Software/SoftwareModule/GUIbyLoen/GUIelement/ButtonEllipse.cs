using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class ButtonEllipse : Button
	{
		#region Property & Variables
		int actCnt = 0;
		int accCnt = 0;

		private int _AccelState = 0;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int AccelState
		{
			get { return _AccelState; }
		}

		System.Threading.Timer activeTimer;
		System.Threading.Timer repeatTimer;

		private Color _BackColorCache;

		private bool _Activation = false;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Activation
		{
			get { return _Activation; }
			set
			{
				_Activation = value;
				actCnt = 0;
				if (_Activation)
				{
					 _BackColorCache = BackColor;
					activeTimer.Change(0, 150);
				}
				else
				{
					activeTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
					BackColor = _BackColorCache;
				}
			}

		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get { return _Style.Font; }
			set
			{
				if (_Style.Font != value)
				{
					_Style.Font = value;
					base.Font = _Style.Font;
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get { return _Style.ForeColor; }
			set
			{
				_Style.ForeColor = value;
				base.ForeColor = _Style.ForeColor;
			}
		}

		private EllipseButtonStyle _Style = new EllipseButtonStyle();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public EllipseButtonStyle Style
		{
			get { return _Style; }
			set
			{
				if (value == null) { _Style = new EllipseButtonStyle(); }
				else { _Style = value; }
			}
		}
		#endregion

		public ButtonEllipse()
			: base()
		{
			base.SetStyle(
				ControlStyles.DoubleBuffer |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.SupportsTransparentBackColor|
				ControlStyles.StandardClick,
				true);

			_Style = new EllipseButtonStyle();

			base.Font = _Style.Font;
			base.ForeColor = _Style.ForeColor;



			InitializeComponent();

			activeTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ActiveTimerProc));
			repeatTimer = new System.Threading.Timer(new System.Threading.TimerCallback(RepeatTimerProc));
		}

		void ActiveTimerProc(object obj)
		{
			int conv = 0;
			if (actCnt < 10) // Active Color로 바꾸는 중.
			{
				conv = actCnt++;
			}
			else // TextLabelBackColor로 바꾸는 중.
			{
				conv = 20 - actCnt;
				actCnt++;
				if (actCnt == 20)
				{
					actCnt = 0;
				}

			}

			Color col = Color.FromArgb(255,
				(_Style.ActiveColor.R - _BackColorCache.R) * conv / 10 + _BackColorCache.R,
				(_Style.ActiveColor.G - _BackColorCache.G) * conv / 10 + _BackColorCache.G,
				(_Style.ActiveColor.B - _BackColorCache.B) * conv / 10 + _BackColorCache.B
				);
			base.BackColor = col;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
	
		{
			Graphics g = pe.Graphics;
			//g.Clear(this.BackColor);
			//g.SetClip(new Rectangle());
			base.OnPaint(pe);
			//g.SetClip(pe.ClipRectangle);

			////int gapX = (int)(this.Width / 8);
			////int gapY = (int)(this.Height / 8);
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.HighQuality;

			Rectangle drawRect = ClientRectangle;
			drawRect.Width -= 1;
			drawRect.Height -= 1;
			g.Clear(this.BackColor);
			Helper.GuiPainter.DrawButtonEllipse(g, drawRect, _Style.BackColor, _Style.ColorStart, _Style.ColorCenter, bst);

			if (this.BackgroundImage != null)
			{
				RectangleF imgRect = drawRect;
				imgRect.Inflate((int)(imgRect.Width * (_Style.PaintRatio / 100d - 1d)), (int)(imgRect.Height * (_Style.PaintRatio / 100d - 1d)));
				imgRect.Offset(_Style.ImageOffset);
				//g.DrawImage(this.BackgroundImage, new Point(0, 0));
				g.DrawImage(this.BackgroundImage, imgRect);
			}

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			g.DrawString(this.Text, Font, new SolidBrush(this.ForeColor), drawRect, sf);

		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			GraphicsPath gp = new GraphicsPath();
			Rectangle rect = this.ClientRectangle;
			rect.Width -= 1;
			rect.Height -= 1;
			gp.AddEllipse(rect);

			this.Region = new Region(gp);
		}

		ButtonStatesWithMouse bst  = ButtonStatesWithMouse.Normal;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			bst |= ButtonStatesWithMouse.ButtonPush;

			if (_Style.RepeatPush)
			{
				_AccelState = 1;
				accCnt = 0;
				repeatTimer.Change(500, 200);
			}
			Refresh();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			bst &= ~ButtonStatesWithMouse.ButtonPush;

			if (_Style.RepeatPush)
			{
				_AccelState = 1;
				accCnt = 0;
				repeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
			}
			Refresh();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			if (this.Enabled)
			{
				bst &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				bst = ButtonStatesWithMouse.Disabled;
			}

			repeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

			Refresh();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			bst |= ButtonStatesWithMouse.MouseHover;

			Refresh();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			bst &= ~ButtonStatesWithMouse.MouseHover;

			Refresh();
		}

		void RepeatTimerProc(object obj)
		{
			accCnt++;
			if (accCnt > _AccelState * 10)
			{
				_AccelState++;
			}
			OnClick(EventArgs.Empty);
		}
	}
}
