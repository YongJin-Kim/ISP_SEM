using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace SEC.GUIelement
{
	public partial class ButtonCone : System.Windows.Forms.Button
	{

		public enum ConeModeEnum
		{
			Up,
			Right,
			Down,
			Left
		}

		Bitmap normalBM = new Bitmap(10, 10);
		Bitmap hoverBM = new Bitmap(10, 10);
		Bitmap pushBM = new Bitmap(10, 10);
		Bitmap disableBM;

		ButtonStatesWithMouse bswm = ButtonStatesWithMouse.Normal;

		private ConeModeEnum _ConeMode = ConeModeEnum.Up;
		[DefaultValue(typeof(ConeModeEnum), "ConeModeEnum")]
		public ConeModeEnum ConeMode
		{
			get { return _ConeMode; }
			set
			{
				_ConeMode = value;
				GenerateBackgroundImg();
				this.Invalidate();
			}
		}

		private int _BoaderWidth = 2;
		[DefaultValue(2)]
		public int BoaderWidth
		{
			get { return _BoaderWidth; }
			set
			{
				if ( value < 0 )
				{
					throw new ArgumentException("Value must be positive.");
				}
				_BoaderWidth = value;
				GenerateBackgroundImg();
				this.Invalidate();
			}
		}

		private Color _ButtonColor = Color.Red;
		[DefaultValue(typeof(Color), "Red")]
		public Color ButtonColor
		{
			get { return _ButtonColor; }
			set
			{
				_ButtonColor = value;
				if (DesignMode) { this.Invalidate(); }
			}
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

		public ButtonCone()
		{
			SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
			SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserMouse, true);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pevent.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;

			if ( (bswm & ButtonStatesWithMouse.Disabled) == ButtonStatesWithMouse.Disabled )
			{
				if ( disableBM == null )
				{
					MakeDisableBM();

					lock ( disableBM )
					{
						pevent.Graphics.DrawImage(disableBM, new Point(0, 0));
					}
				}
			}
			else
			{
				switch ( bswm )
				{
				case ButtonStatesWithMouse.Normal:
					lock ( normalBM )
					{
						pevent.Graphics.DrawImage(normalBM, new Point(0, 0));
					}
					break;
				case ButtonStatesWithMouse.NormalHover:
					lock ( hoverBM )
					{
						pevent.Graphics.DrawImage(hoverBM, new Point(0, 0));
					}
					break;
				case ButtonStatesWithMouse.ButtonPush:
				case ButtonStatesWithMouse.PushHover:
					lock ( pushBM )
					{
						pevent.Graphics.DrawImage(pushBM, new Point(0, 0));
					}
					break;
				}
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			GenerateBackgroundImg();
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
			this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			bswm &= ~ButtonStatesWithMouse.ButtonPush;
			this.Invalidate();
		}

		double div = 1d / 4d;

		private void MakeDisableBM()
		{
			if ( (this.Width == 0) || (this.Height == 0) )
			{
				this.Region.MakeEmpty();
				return;
			}

			Region innerReg = GetReg(0);
			Region outterReg = GetReg(_BoaderWidth);


			Bitmap bmTmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			bmTmp.MakeTransparent();

			Graphics g = Graphics.FromImage(bmTmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			//System.Drawing.Region reg = outterReg.Clone();


			LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, ControlPaint.LightLight(_ButtonColor), ControlPaint.Dark(_ButtonColor), 45f);
			g.FillRegion(lgb, outterReg);
			//g.FillRegion(new SolidBrush(_BackColor), innerReg);


			if ( this.Image != null )
			{
				Rectangle drwRect;

				if ( this.Width < this.Height )
				{
					drwRect = new Rectangle(_BoaderWidth, this.Height - this.Width + _BoaderWidth, this.Width - _BoaderWidth - _BoaderWidth, this.Width - _BoaderWidth - _BoaderWidth);
				}
				else
				{
					drwRect = new Rectangle(this.Width / 2 - _BoaderWidth, _BoaderWidth, this.Height - _BoaderWidth - _BoaderWidth, this.Height - _BoaderWidth - _BoaderWidth);
				}

				//Rectangle rect = this.ClientRectangle;
				//rect.Inflate(-2 * _BoaderWidth,  (int)(this.ClientSize.Height * div / -2)/* - (int)(div * this.Height / 2d)*/);
				//rect.Offset(0, (int)(this.ClientSize.Height * div / 2));
				g.DrawImage(this.Image, drwRect);
			}

			g.Dispose();

			disableBM = bmTmp;

		}

		private void GenerateBackgroundImg()
		{
			if ( (this.Width == 0) || (this.Height == 0) )
			{
				this.Region.MakeEmpty();
				return;
			}

			Region innerReg = GetReg(_BoaderWidth + 2);
			Region outterReg = GetReg(2);
			//Region baseReg = GetReg(0);

			System.Drawing.Region reg = outterReg.Clone();

			this.Region = reg;

			#region Normal BM
			Bitmap bmTmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			//bmTmp.MakeTransparent();

			Graphics g = Graphics.FromImage(bmTmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(this.BackColor);

			LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, ControlPaint.Light(_ButtonColor), ControlPaint.Dark( _ButtonColor), 45f);
			g.FillRegion(new SolidBrush(_ButtonColor), outterReg);
			g.FillRegion(lgb, innerReg);

			DrawImg(g);

			g.Dispose();

			lock ( normalBM )
			{
				normalBM = bmTmp;
			}
			#endregion

			#region Hover BM
			bmTmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			//bmTmp.MakeTransparent();

			g = Graphics.FromImage(bmTmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(this.BackColor);

			g.FillRegion(new SolidBrush(_ButtonColor), outterReg);
			g.FillRegion(new SolidBrush(ControlPaint.LightLight(_ButtonColor)), innerReg);


			DrawImg(g);
			g.Dispose();

			lock ( hoverBM )
			{
				hoverBM = bmTmp;
			}
			#endregion

			#region Push BM
			bmTmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			//bmTmp.MakeTransparent();
			g = Graphics.FromImage(bmTmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(this.BackColor);

			//lgb = new LinearGradientBrush(this.ClientRectangle, ControlPaint.Dark(_ButtonColor), ControlPaint.Light(Color.FromArgb(164, _ButtonColor)), 45f);
			//g.FillRegion(new SolidBrush(Color.Black), baseReg);
			g.FillRegion(new SolidBrush(_ButtonColor), outterReg);
			g.FillRegion(new SolidBrush(ControlPaint.Dark(_ButtonColor)), innerReg);
			DrawImg(g);
			g.Dispose();

			lock ( pushBM )
			{
				pushBM = bmTmp;
			}
			#endregion
		}

		private void DrawImg(Graphics g)
		{
			if ( this.Image != null )
			{
				Rectangle drwRect;


				int size = 0;

				switch ( _ConeMode )
				{
				case ConeModeEnum.Up:
					if ( this.ClientSize.Width - _BoaderWidth * 2 > (this.ClientSize.Height - _BoaderWidth * 2) * (1 - div) )
					{
						size = (int)((this.ClientSize.Height - _BoaderWidth * 2) * (1 - div));
					}
					else
					{
						size = this.ClientSize.Width - _BoaderWidth * 2;
					}
					drwRect = new Rectangle((this.ClientSize.Width - size) / 2, this.ClientSize.Height - size - _BoaderWidth, size, size);

					break;
				case ConeModeEnum.Left:
					if ( (this.ClientSize.Width - _BoaderWidth * 2) * (1 - div) > this.ClientSize.Height - _BoaderWidth * 2 )
					{
						size = this.ClientSize.Height - _BoaderWidth * 2;
					}
					else
					{
						size = (int)((this.ClientSize.Width - _BoaderWidth * 2) * (1 - div));
					}
					drwRect = new Rectangle(this.ClientSize.Width - size - _BoaderWidth, (this.ClientSize.Height - size) / 2, size, size);
					break;
				case ConeModeEnum.Down:
					if ( this.ClientSize.Width - _BoaderWidth * 2 > (this.ClientSize.Height - _BoaderWidth * 2) * (1 - div) )
					{
						size = (int)((this.ClientSize.Height - _BoaderWidth * 2) * (1 - div));
					}
					else
					{
						size = this.ClientSize.Width - _BoaderWidth * 2;
					}
					drwRect = new Rectangle((this.ClientSize.Width - size) / 2, _BoaderWidth, size, size);
					break;
				case ConeModeEnum.Right:
					if ( (this.ClientSize.Width - _BoaderWidth * 2) * (1 - div) > this.ClientSize.Height - _BoaderWidth * 2 )
					{
						size = this.ClientSize.Height - _BoaderWidth * 2;
					}
					else
					{
						size = (int)((this.ClientSize.Width - _BoaderWidth * 2) * (1 - div));
					}
					drwRect = new Rectangle(_BoaderWidth, (this.ClientSize.Height - size) / 2, size, size);
					break;
				default:
					throw new Exception();
				}

				g.DrawImage(this.Image, drwRect);

			}
		}

		private Region GetReg(int boader)
		{
			Point[] pnts = new Point[5];
			GraphicsPath gp;
			switch ( _ConeMode )
			{
			case ConeModeEnum.Up:
				pnts[0] = new Point(this.ClientSize.Width / 2, boader);
				pnts[1] = new Point(boader, (int)((this.ClientSize.Height - boader - boader) * div) + boader);
				pnts[2] = new Point(boader, this.ClientSize.Height - boader);
				pnts[3] = new Point(this.ClientSize.Width - boader, this.ClientSize.Height - boader);
				pnts[4] = new Point(this.ClientSize.Width - boader, (int)((this.ClientSize.Height - boader - boader) * div) + boader);
				break;
			case ConeModeEnum.Right:
				pnts[0] = new Point(this.ClientSize.Width - boader, this.ClientSize.Height / 2);
				pnts[1] = new Point((int)((this.ClientSize.Width - boader - boader) * (1d - div) + boader), this.ClientSize.Height - boader);
				pnts[2] = new Point(boader, this.ClientSize.Height - boader);
				pnts[3] = new Point(boader, boader);
				pnts[4] = new Point((int)((this.ClientSize.Width - boader - boader) * (1d - div) + boader), boader);
				break;
			case ConeModeEnum.Down:
				pnts[0] = new Point(this.ClientSize.Width / 2, this.ClientSize.Height - boader);
				pnts[1] = new Point(boader, (int)((this.ClientSize.Height - boader - boader) * (1d - div) + boader));
				pnts[2] = new Point(boader, boader);
				pnts[3] = new Point(this.ClientSize.Width - boader, boader);
				pnts[4] = new Point(this.ClientSize.Width - boader, (int)((this.ClientSize.Height - boader - boader) * (1d - div) + boader));
				break;
			case ConeModeEnum.Left:
				pnts[0] = new Point(boader, this.ClientSize.Height / 2);
				pnts[1] = new Point((int)((this.ClientSize.Width - boader - boader) * div + boader), boader);
				pnts[2] = new Point(this.ClientSize.Width - boader, boader);
				pnts[3] = new Point(this.ClientSize.Width - boader, this.ClientSize.Height - boader);
				pnts[4] = new Point((int)((this.ClientSize.Width - boader - boader) * div + boader), this.ClientSize.Height - boader);
				break;
			}

			gp = new GraphicsPath();
			gp.AddPolygon(pnts);
			return new Region(gp);
		}
	}
}
