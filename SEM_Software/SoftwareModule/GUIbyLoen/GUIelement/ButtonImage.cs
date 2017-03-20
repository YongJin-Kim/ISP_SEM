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
	[DesignerAttribute(typeof(SEC.Nanoeye.Controls.BitmapButtonDesigner))]
	public partial class ButtonImage : Button
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
				if ( _Surface == null )
				{
					divideImg = null;
				}
				else
				{
					divideImg = Helper.ImageHelper.ImageDivider(_Surface, _SurfaceOrientation, 4);
					this.Size = divideImg[0].Size;

					this.Region = SEC.Nanoeye.Controls.Helper.GetBitmapRegion(divideImg[0]);

				}
				this.Invalidate();
			}
		}

		public ButtonImage()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if ( divideImg != null )
			{
				this.Size = divideImg[0].Size;
			}

			base.OnSizeChanged(e);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			
			if ( divideImg == null )
			{
				pevent.Graphics.DrawString(this.Name, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
				sf.Dispose();
			}
			else
			{
				int index;

				if ( (bsw & ButtonStatesWithMouse.Disabled) == ButtonStatesWithMouse.Disabled )
				{
					index = 3;
				}
				else
				{
					switch ( bsw )
					{
					case ButtonStatesWithMouse.Normal: index = 0; break;
					case ButtonStatesWithMouse.NormalHover: index = 2; break;
					case ButtonStatesWithMouse.ButtonPush: index = 1; break;
					case ButtonStatesWithMouse.PushHover: index = 1; break;
					default: throw new ArgumentException();
					}
				}
				pevent.Graphics.DrawImage(divideImg[index], new Point(0, 0));
			}

			pevent.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			bsw |= ButtonStatesWithMouse.MouseHover;

			Invalidate();

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			bsw &= ~ButtonStatesWithMouse.MouseHover;

			Invalidate();

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			bsw |= ButtonStatesWithMouse.ButtonPush;

			Invalidate();
			
			base.OnMouseDown(mevent);
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			bsw &= ~ButtonStatesWithMouse.ButtonPush;

			Invalidate();
			
			base.OnMouseUp(mevent);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			if ( this.Enabled )
			{
				bsw &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				bsw |= ButtonStatesWithMouse.Disabled;
			}

			Invalidate();


			base.OnEnabledChanged(e);
		}

	}
}
