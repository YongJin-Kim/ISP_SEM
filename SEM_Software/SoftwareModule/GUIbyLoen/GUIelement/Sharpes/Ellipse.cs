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
	public partial class Ellipse : Control
	{
		private Region regLine;

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

		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				ChangeRegion();
				base.BackColor = value;
#if DEBUG
				if (DesignMode) { this.Invalidate(); }
#endif
			}
		}

		public Ellipse()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.UserPaint, true);

			ChangeRegion();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			ChangeRegion();
			base.OnSizeChanged(e);
		}

		private void ChangeRegion()
		{
			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

			int halfSW = (int)Math.Floor(_LineWidth / 2d);
			int halfLW = (int)Math.Ceiling(_LineWidth / 2d);

			gp.AddEllipse(halfSW, halfSW, this.Width - halfLW, this.Height - halfLW);
			gp.Widen(new Pen(_LineColor, _LineWidth));
			regLine = new Region(gp);

			if (BackColor == Color.Transparent)
			{
				this.Region = regLine;
			}
			else
			{
				gp = new System.Drawing.Drawing2D.GraphicsPath();
				gp.AddEllipse(0, 0, this.Width, this.Height);
				this.Region = new Region(gp);
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.FillRegion(new SolidBrush(_LineColor), regLine);
			base.OnPaint(pe);
		}
	}
}
