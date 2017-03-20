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
	public partial class Rectangle : Control
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

		public Rectangle()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			//pe.Graphics.Clear(this.BackColor);
			pe.Graphics.DrawRectangle(new Pen(this._LineColor, this._LineWidth), (float)Math.Floor(_LineWidth / 2d), (float)Math.Floor(_LineWidth / 2d), this.Width - _LineWidth, this.Height - _LineWidth);
			base.OnPaint(pe);
		}
	}
}
