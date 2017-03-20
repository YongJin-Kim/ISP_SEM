using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using System.Windows.Forms;
using System.Collections;


namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	public class ItemTextbox : ItemBase
	{
		Font fnt;
		private string _Text = null;
		public string Text
		{
			get { return _Text; }
			set { _Text = value; }
		}

		Color colFont = Color.Black;
		int width;
		int height;
		int x1, y1;
		public ItemTextbox()
		{
			this.MaxHandleCount = 2;
			fnt = new Font(FontFamily.GenericMonospace, 12.0F, FontStyle.Regular, GraphicsUnit.Pixel);
		}

		public override string ToString()
		{
			return "TextBox";
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2) {
				//path.StartFigure();
				width = (handles[0].X - handles[1].X);
				height = (handles[0].Y - handles[1].Y);

				if (width < 0) {
					x1 = handles[0].X;
					width = -width;
				}
				else {
					x1 = handles[1].X;
					//width = width;
				}
				if (height < 0) {
					y1 = handles[0].Y;
					height = -height;
				}
				else {
					y1 = handles[1].Y;
					//height = height;
				}
				//path.AddRectangle(new Rectangle(x1, y1, width, height));
			}
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			if (_Text == null)
				return false;

			float valuePerPixel = this.Parent.PixelLength;
	
			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = StringAlignment.Center;

			RectangleF pathRect = new RectangleF(x1, y1, width, height);

			//GraphicsPath rectPath = new GraphicsPath();
			//rectPath.AddString( strText, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pt, format);
			path.AddString(_Text, fnt.FontFamily, (int)fnt.Style,  fnt.Size, pathRect , format);

			//if (rectPath.PointCount == 0)
			//    return;
			//path.AddPath(rectPath, true);

			//rectPath.Dispose();
			format.Dispose();

			return true;
		}

		internal override void UpdateHandlePath(GraphicsPath path, Point[] handles)
		{
			if (this.IsSelected) {	// 선택 된 경우
				foreach (Point pt in handles) {
					path.AddRectangle(new RectangleF(pt.X - 4, pt.Y - 4, 8, 8));
					path.AddRectangle(new RectangleF(pt.X - 3, pt.Y - 3, 6, 6));
				}
				path.AddRectangle(new Rectangle(x1, y1, width, height));
			}
		}

		internal void ChangeText()
		{
			TextInput ti = new TextInput();
			ti.str = _Text;
			ti.ft = fnt;
			ti.colFont = colFont;
			if (ti.ShowDialog() == DialogResult.OK) {
				fnt = ti.ft;
				_Text = ti.str;
				colFont = ti.colFont;
			}
			ti.Dispose();

		}

		internal override void Draw(Graphics g)
		{
			if (this.ItemPath != null) {
				SolidBrush b;

				if(colFont.GetBrightness() < 0.5)
					b = new SolidBrush(Color.White);
				else
					b = new SolidBrush(Color.Black);
				g.FillRegion(b, this.Region);
				b.Dispose();

				b = new SolidBrush(colFont);
				g.FillPath(b, this.ItemPath);
				b.Dispose();

			}
		}
	}
}
