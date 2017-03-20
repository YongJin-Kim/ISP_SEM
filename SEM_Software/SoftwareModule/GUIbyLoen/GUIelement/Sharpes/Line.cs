using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement.Sharpes
{
	public partial class Line : Control
	{
        private int _LineWidth = 3;
		[DefaultValue(3)]
		public int LineWidth
		{
			get { return _LineWidth; }
			set
			{
				if (_LineWidth != value)
				{
					_LineWidth = value;
#if DEBUG
					if (DesignMode) { this.Invalidate(); }
#endif
				}
			}
		}

		private Color _LineColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color LineColor
		{
			get { return _LineColor; }
			set
			{
				if (_LineColor != value)
				{
					_LineColor = value;
#if DEBUG
					if (DesignMode) { this.Invalidate(); }
#endif
				}
			}
		}

		private LineDirection _Direction = LineDirection.Vertical;
		public LineDirection Direction
		{
			get { return _Direction; }
			set
			{
				if (_Direction != value)
				{
					_Direction = value;
					ChangeRegion();
#if DEBUG
					if (DesignMode) { this.Invalidate(); }
#endif
				}
			}
		}

		public Line()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles.Opaque, true);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			ChangeRegion();
			base.OnSizeChanged(e);
		}

		private void ChangeRegion()
		{
			if ((this.Width == 0) && (this.Height == 0)) { return; }

			Point pntS,pntE;
			switch (_Direction)
			{
			case LineDirection.Vertical:
				pntS = new Point(this.Width / 2, 0);
				pntE = new Point(this.Width / 2, this.Height);
				break;
			case LineDirection.Horizontal:
				pntS = new Point(0, this.Height / 2);
				pntE = new Point(this.Width, this.Height / 2);
				break;
			case LineDirection.LeftbottomToRighttop:
				pntS = new Point(0, this.Height);
				pntE = new Point(this.Width, 0);
				break;
			case LineDirection.LefttopToRightbottom:
				pntS = new Point(0, 0);
				pntE = new Point(this.Width, this.Height);
				break;
			default:
				throw new ArgumentException();
			}


			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			gp.AddLine(pntS, pntE);
			gp.Widen(new Pen(_LineColor, _LineWidth));
			this.Region = new Region(gp);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.Clear(this._LineColor);
			base.OnPaint(pe);
		}

		public enum LineDirection
		{
			Horizontal,
			Vertical,
			LefttopToRightbottom,
			LeftbottomToRighttop
		}
	}
}
