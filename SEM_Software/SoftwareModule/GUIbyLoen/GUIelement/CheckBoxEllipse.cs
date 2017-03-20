using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace SEC.GUIelement
{
	public partial class CheckBoxEllipse : CheckBox
	{
        private Bitmap[] divideImg = null;

        private Bitmap _Surface = null;
        public Bitmap Surface
        {
            get { return _Surface; }
            set
            {
                _Surface = value;
                GenerateImg();
            }
        }

        private Bitmap _FSurface = null;
        public Bitmap FSurface
        {
            get { return _FSurface; }
            set
            {
                _FSurface = value;
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

		public override Color ForeColor
		{
			get { return _Style.ForeColor; }
			set
			{
				_Style.ForeColor = value;
				base.ForeColor = value;
			}
		}

		public override Font Font
		{
			get { return _Style.Font; }
			set
			{
				_Style.Font = value;
				base.Font = value;
			}
		}

		private EllipseButtonStyle _Style = new EllipseButtonStyle();
		public EllipseButtonStyle Style
		{
			get { return _Style; }
			set {
				if (value == null) { _Style = new EllipseButtonStyle(); }
				else { _Style = value; }
			}
		}

		public CheckBoxEllipse()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		ButtonStatesWithMouse bst  = ButtonStatesWithMouse.Normal;

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(this.ClientRectangle);

			this.Region = new Region(gp);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			// 기본 drawing 메서드로는 이미지를 그리지 못하게 함.
			base.OnPaint(pevent);



			Graphics g = pevent.Graphics;

			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.HighQuality;

			g.Clear(this.BackColor);

			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

			ButtonStatesWithMouse bswm = bst;

			if ( this.ThreeState )
			{
				if ( this.CheckState == CheckState.Checked )
				{
					bswm |= ButtonStatesWithMouse.ButtonPush;
				}
			}
			else
			{
				if ( this.Checked )
				{
					bswm |= ButtonStatesWithMouse.ButtonPush;
				}
			}

			Helper.GuiPainter.DrawButtonEllipse(g, rect, _Style.BackColor, _Style.ColorStart, _Style.ColorCenter, bswm);
            //rect = g.DrawImage(_Surface, rect);


			if ( this.BackgroundImage != null )
			{
				//g.DrawImage(this.BackgroundImage, rect);
                g.DrawImage(this.Surface, rect);
			}

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			g.DrawString(this.Text, _Style.Font, new SolidBrush(_Style.ForeColor), rect, sf);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			bst |= ButtonStatesWithMouse.ButtonPush;

			this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			bst &= ~ButtonStatesWithMouse.ButtonPush;

			this.Invalidate();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			if ( this.Enabled )
			{
				bst &= ~ButtonStatesWithMouse.Disabled;
			}
			else
			{
				bst |= ButtonStatesWithMouse.Disabled;
			}

			this.Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			bst |= ButtonStatesWithMouse.MouseHover;

			this.Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			bst &= ~ButtonStatesWithMouse.MouseHover;

			this.Invalidate();
		}

        private void GenerateImg()
        {
            if (_Surface == null)
            {
                divideImg = null;
            }
            else
            {
                if (_Surface == null)
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

	}
}

