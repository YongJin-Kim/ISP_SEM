using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel.Design;

namespace SEC.GUIelement
{
	public class NamedPanel : System.Windows.Forms.Panel
	{
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Invalidate();
			}
		}

		private int _TextWidth = 30;
		[DefaultValue(30)]
		public int TextWidht
		{
			get { return _TextWidth; }
			set
			{
				_TextWidth = value;
				this.Invalidate();
			}
		}

		private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;
		[DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
		public ContentAlignment TextAlign
		{
			get { return _TextAlign; }
			set
			{
				_TextAlign = value;
				this.Invalidate();
			}
		}

		public NamedPanel()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			StringFormat sf = new StringFormat();
			switch ( _TextAlign )
			{
			case ContentAlignment.BottomCenter:
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Far;
				break;
			case ContentAlignment.BottomLeft:
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Far;
				break;
			case ContentAlignment.BottomRight:
				sf.Alignment = StringAlignment.Far;
				sf.LineAlignment = StringAlignment.Far;
				break;
			case ContentAlignment.MiddleCenter:
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				break;
			case ContentAlignment.MiddleLeft:
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				break;
			case ContentAlignment.MiddleRight:
				sf.Alignment = StringAlignment.Far;
				sf.LineAlignment = StringAlignment.Center;
				break;
			case ContentAlignment.TopCenter:
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Near;
				break;
			case ContentAlignment.TopLeft:
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				break;
			case ContentAlignment.TopRight:
				sf.Alignment = StringAlignment.Far;
				sf.LineAlignment = StringAlignment.Near;
				break;
			}
			e.Graphics.DrawString(Text, Font, new SolidBrush(this.ForeColor), new RectangleF(_TextWidth / 10, 0, _TextWidth, this.ClientSize.Height), sf);
		}
	}
}
