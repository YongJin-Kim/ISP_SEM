using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace SEC.GUIelement
{
	public class ButtonConvex : Button
	{
		Color _ButtonColor = Color.White;
		[DefaultValue(typeof(Color), "White")]
		public Color ButtonColor
		{
			get { return _ButtonColor; }
			set
			{
				_ButtonColor = value;
				this.Invalidate();
			}
		}

		ButtonStatesWithMouse bswm = ButtonStatesWithMouse.Normal;
		
		public ButtonConvex()
		{
			SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
			SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserMouse, true);

			this.FlatAppearance.BorderSize = 0;
			this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Region reg = new Region();
			reg.MakeEmpty();
			Region orireg = pevent.Graphics.Clip;
			pevent.Graphics.Clip = reg;
			base.OnPaint(pevent);
			pevent.Graphics.Clip = orireg;


			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height );
			Graphics g = pevent.Graphics;

			// 윤곽선.
			//g.FillEllipse(new Pen(Color.FromArgb(64, Color.Black), 1), rect);
			g.FillEllipse(new SolidBrush(Color.FromArgb(64, Color.Black)), rect);

			//테두리
			rect.Inflate(-2,-2);
			g.DrawEllipse(new Pen(Color.White, 2), rect);

			// 버튼
			rect.Inflate(-2, -2);
			//g.DrawEllipse(new Pen(Color.Gray, 1), rect);
			//rect.Inflate(-2, -2);

			GraphicsPath gp = new GraphicsPath();
			gp.AddRectangle(new Rectangle(rect.Left, this.Height * -1, rect.Width, this.Height * 3));
			gp.AddRectangle(rect);
			PathGradientBrush pgb = new PathGradientBrush(gp);
			gp.Dispose();
			//pgb.CenterPoint = pnts[1];

			Color da = ControlPaint.Dark(_ButtonColor);

			da = Color.Gray;

			pgb.FocusScales = new PointF(0.6F, 0.6F);
			//pgb.RotateTransform(5);
			pgb.CenterPoint = new PointF(this.Width / 2, this.Height / 2);

			

			switch ( bswm )
			{
			case ButtonStatesWithMouse.Normal:
				pgb.CenterColor = _ButtonColor;
				pgb.SurroundColors = new Color[] { da };
				break;
			case ButtonStatesWithMouse.NormalHover:
				pgb.CenterColor = ControlPaint.Light(_ButtonColor);
				pgb.SurroundColors = new Color[] {ControlPaint.Light( da )};
				break;
			case ButtonStatesWithMouse.ButtonPush:
			case ButtonStatesWithMouse.PushHover:
				pgb.CenterColor = ControlPaint.Dark(_ButtonColor);
				pgb.SurroundColors = new Color[] { ControlPaint.Dark(da) };
				break;
			case ButtonStatesWithMouse.Disabled:
				break;
			}

			g.FillEllipse(pgb, rect);

			if ( this.Image != null )
			{
				rect.Inflate(-2, -2);
				g.DrawImage(this.Image, rect);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(0, 0, this.Width, this.Height);
			this.Region = new Region(gp);
			gp.Dispose();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			if ( this.Enabled )
			{
				bswm &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				bswm |= ButtonStatesWithMouse.Disabled;
			}
			this.Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			bswm |= ButtonStatesWithMouse.MouseHover;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			bswm &= ~ButtonStatesWithMouse.MouseHover;
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			base.OnMouseDown(mevent);
			bswm |= ButtonStatesWithMouse.ButtonPush;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			bswm &= ~ButtonStatesWithMouse.ButtonPush;
			this.Invalidate();
		}



	}
}
