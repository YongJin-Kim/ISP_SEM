using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class CheckBoxImage : CheckBox
	{
		private Bitmap[] divideImg = null;

		ButtonStatesWithMouse bsw = ButtonStatesWithMouse.Normal;

		private Bitmap _Surface = null;
		[DefaultValue(null)]
		public Bitmap Surface
		{
			get { return _Surface; }
			set
			{
				_Surface = value;
				GenerateImg();
			}
		}

		private Orientation _SurfaceOrientation = Orientation.Horizontal;
		[DefaultValue(typeof(Orientation), "Horizontal")]
		public Orientation SurfaceOrientation
		{
			get { return _SurfaceOrientation; }
			set
			{
				_SurfaceOrientation = value;
				GenerateImg();
			}
		}

		private void GenerateImg()
		{
			if ( _Surface == null )
			{
				divideImg = null;
			}
			else
			{
				divideImg = Helper.ImageHelper.ImageDivider(_Surface, _SurfaceOrientation, 5);
				this.Size = divideImg[0].Size;
			}

			this.Invalidate();
		}

		public CheckBoxImage()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			threstateTimer = new Timer();
			threstateTimer.Interval = 300;
			threstateTimer.Tick += new EventHandler(threstateTimer_Tick);
		}

		#region overrride
		protected override void OnCheckedChanged(EventArgs e)
		{
			threstateTimer.Stop();
			if (this.Checked)
			{
				bsw |= ButtonStatesWithMouse.ButtonPush;
			}
			else
			{
				bsw &= ~ButtonStatesWithMouse.ButtonPush;
			}
			base.OnCheckedChanged(e);
		}

		protected override void OnCheckStateChanged(EventArgs e)
		{
			switch (CheckState)
			{
			case CheckState.Checked:
				threstateTimer.Stop();
				bsw |= ButtonStatesWithMouse.ButtonPush;
				break;
			case CheckState.Indeterminate:
				threstateCnt = 0;
				threstateTimer.Start();
				break;
			case CheckState.Unchecked:
				threstateTimer.Stop();
				bsw &= ~ButtonStatesWithMouse.ButtonPush;
				break;
			}

			base.OnCheckStateChanged(e);
		}

		protected override void OnMouseEnter(EventArgs eventargs)
		{
			bsw |= ButtonStatesWithMouse.MouseHover;

			base.OnMouseEnter(eventargs);
		}

		protected override void OnMouseLeave(EventArgs eventargs)
		{
			bsw &= ~ButtonStatesWithMouse.MouseHover;

			base.OnMouseLeave(eventargs);
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			bsw |= ButtonStatesWithMouse.ButtonPush;

			base.OnMouseDown(mevent);
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if ((!this.Checked) || (this.CheckState != CheckState.Checked))
			{
				bsw &= ~ButtonStatesWithMouse.ButtonPush;
			}

			base.OnMouseUp(mevent);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			if (this.Enabled)
			{
				bsw &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				threstateTimer.Stop();
				bsw |= ButtonStatesWithMouse.Disabled;
			}
			
			base.OnEnabledChanged(e);
		}

		protected override void SetClientSizeCore(int x, int y)
		{
			if (divideImg != null)
			{
				base.SetClientSizeCore(divideImg[0].Size.Width, divideImg[0].Size.Height);
			}
			else
			{
				base.SetClientSizeCore(x, y);
			}
		}

		//protected override void OnSizeChanged(EventArgs e)
		//{
		//    if ( divideImg != null )
		//    {
		//        //this.Size = divideImg[0].Size;
		//        this.SetClientSizeCore(divideImg[0].Size.Width, divideImg[0].Size.Height);
		//    }

		//    base.OnSizeChanged(e);
		//}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//Region pre = pevent.Graphics.Clip;

			//Region newreg = new Region();
			//newreg.MakeEmpty();
			//pevent.Graphics.Clip = newreg;
			base.OnPaint(pevent);
			//pevent.Graphics.Clip = pre;

			pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			if (divideImg == null)
			{
				pevent.Graphics.DrawString(this.Name, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
				sf.Dispose();
				return;
			}

			int index;

			if (ThreeState && (CheckState == CheckState.Indeterminate))
			{
				switch (checkInterBsw)
				{
				case ButtonStatesWithMouse.Normal:
					index = 0;
					break;
				case ButtonStatesWithMouse.NormalHover:
					index = 2;
					break;
				case ButtonStatesWithMouse.ButtonPush:
					index = 1;
					break;
				case ButtonStatesWithMouse.PushHover:
					index = 4;
					break;
				default:
					throw new ArgumentException();
				}
			}
			else
			{
				if ((bsw & ButtonStatesWithMouse.Disabled) == ButtonStatesWithMouse.Disabled)
				{
					index = 3;
				}
				else
				{
					switch (bsw)
					{
					case ButtonStatesWithMouse.Normal:
						index = 0;
						break;
					case ButtonStatesWithMouse.NormalHover:
						index = 2;
						break;
					case ButtonStatesWithMouse.ButtonPush:
						index = 1;
						break;
					case ButtonStatesWithMouse.PushHover:
						index = 4;
						break;
					default:
						throw new ArgumentException();
					}
				}
			}
			pevent.Graphics.DrawImage(divideImg[index], new Point(0, 0));


			if (this.Image != null)
			{
				int height;
				int width;
				if ((this.ClientSize.Width / (double)this.Image.Width) > (this.ClientSize.Height / (double)this.Image.Height))
				{
					height = (int)(this.ClientSize.Height * 0.8d);
					width = this.ClientSize.Width * height / this.Image.Height;
				}
				else
				{
					width = (int)(this.ClientSize.Width * 0.8d);
					height = (int)(this.ClientSize.Height * width / this.Image.Width);
				}
				pevent.Graphics.DrawImage(this.Image, new Rectangle((this.ClientSize.Width - width) / 2,
																	(this.ClientSize.Height - height) / 2,
																	width,
																	height));
			}

			pevent.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
		}
		#endregion

		int threstateCnt;
		Timer threstateTimer = null;
		ButtonStatesWithMouse checkInterBsw = ButtonStatesWithMouse.Normal;

		void threstateTimer_Tick(object sender, EventArgs e)
		{
			switch ( threstateCnt )
			{
			case 0:
				threstateCnt = 1;
				checkInterBsw = ButtonStatesWithMouse.PushHover;
				break;
			case 1:
				threstateCnt = 2;
				checkInterBsw = ButtonStatesWithMouse.ButtonPush;
				break;
			case 2:
				threstateCnt = 3;
				checkInterBsw = ButtonStatesWithMouse.PushHover;
				break;
			case 3:
				threstateCnt = 4;
				checkInterBsw = ButtonStatesWithMouse.NormalHover;
				break;
			case 4:
				threstateCnt = 5;
				checkInterBsw = ButtonStatesWithMouse.Normal;
				break;
			case 5:
				threstateCnt = 0;
				checkInterBsw = ButtonStatesWithMouse.NormalHover;
				break;
			}
			this.Invalidate();
		}
	}
}
