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
	public partial class Triangle : Control
	{
		private SEC.GUIelement.Sharpes.Triangle.Orientation _Angle = SEC.GUIelement.Sharpes.Triangle.Orientation.Angle0;
		[DefaultValue(typeof(SEC.GUIelement.Sharpes.Triangle.Orientation), "Angle0")]
		public SEC.GUIelement.Sharpes.Triangle.Orientation Angle
		{
			get { return _Angle; }
			set
			{
				if (_Angle != value)
				{
					_Angle = value;
#if DEBUG
					if (DesignMode)
					{
						OutlineChange();
						this.Invalidate();
					}
#endif
				}
			}
		}

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
		[DefaultValue(typeof(Color),"Black")]
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

		private Color _FillColor = Color.Red;
		[DefaultValue(typeof(Color), "Red")]
		public Color FillColor
		{
			get { return _FillColor; }
			set
			{
				if(_FillColor != value)
				{
					_FillColor = value;
					this.Invalidate();
				}
			}
		}

		System.Drawing.Drawing2D.GraphicsPath polygon;

		public Triangle()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.UserPaint, true);


			this.MinimumSize = new Size(10, 10);
			Point[] outline;

			outline = new Point[3];
			outline[0] = new Point(this.Width / 2, 0);
			outline[1] = new Point(this.Width, this.Height);
			outline[2] = new Point(0, this.Height);

			polygon = new System.Drawing.Drawing2D.GraphicsPath();
			polygon.AddPolygon(outline);
		}

		public override Size MinimumSize
		{
			get { return base.MinimumSize; }
			set
			{
				int w = value.Width;
				int h = value.Height;
				if (w < 10) { w = 10; }
				if (h < 10) { h = 10; }
				System.Drawing.Size s = new Size(w, h);
				base.MinimumSize = s;
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			OutlineChange();
			base.OnSizeChanged(e);
		}

		private void OutlineChange()
		{
			Point[] pnts = new Point[3];

			int halfW = (int)Math.Ceiling(_LineWidth / 2d);

			switch (_Angle)
			{
			case Orientation.Angle0:
				pnts[0] = new Point(this.Width / 2, 0);
				pnts[1] = new Point(this.Width, this.Height);
				pnts[2] = new Point(0, this.Height);
				break;
			case Orientation.Angle90:
				pnts[0] = new Point(this.Width, this.Height / 2);
				pnts[1] = new Point(0, this.Height);
				pnts[2] = new Point(0, 0);
				break;
			case Orientation.Angle180:
				pnts[0] = new Point(this.Width / 2, this.Height);
				pnts[1] = new Point(0, 0);
				pnts[2] = new Point(this.Width, 0);
				break;
			case Orientation.Angle270:
				pnts[0] = new Point(0, this.Height / 2);
				pnts[1] = new Point(this.Width, 0);
				pnts[2] = new Point(this.Width, this.Height);
				break;
			default:
				throw new ArgumentException();
			}

			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			gp.AddPolygon(pnts);

			polygon = gp;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.FillPath(new SolidBrush(_FillColor), polygon);
			pe.Graphics.DrawPath(new Pen(_LineColor, _LineWidth), polygon);

			base.OnPaint(pe);
		}

		public enum Orientation
		{
			Angle0,
			Angle90,
			Angle180,
			Angle270
		}
	}
}
