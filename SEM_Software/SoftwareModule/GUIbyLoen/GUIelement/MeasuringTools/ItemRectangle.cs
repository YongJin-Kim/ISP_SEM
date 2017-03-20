using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;

namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	class ItemRectangle : ItemBase
	{
		public ItemRectangle()
			: this(true)
		{ }

		public ItemRectangle(bool text)
		{
			this.MaxHandleCount = 2;
			_DrawText = text;
		}

		public override string ToString()
		{
			if (_DrawText)
			{
				return "AreaSquare";
			}
			else
			{
				return "Rectangle";
			}
		}

		internal override void HandleAdd(Point pnt)
		{
			base.HandleAdd(pnt);

			if (Parent.IsSymetric && (HandleCount == 2)) { MakeSymetric(); }
		}

		internal override void UpdateHandle(Point location)
		{
			base.UpdateHandle(location);

			if (Parent.IsSymetric && (HandleCount == 2)) { MakeSymetric(); }
		}

		private void MakeSymetric()
		{
			int index = Math.Abs(HandleIndex - 1);

			int gapX, gapY;
			Point pnt = Handles[HandleIndex];
			gapX = Handles[index].X - pnt.X;
			gapY = Handles[index].Y - pnt.Y;

			if (Math.Abs(gapX) < Math.Abs(gapY))
			{
				if (gapX < 0) { gapX = Math.Abs(gapY) * -1; }
				else { gapX = Math.Abs(gapY); }
			}
			else
			{
				if (gapY < 0) { gapY = Math.Abs(gapX) * -1; }
				else { gapY = Math.Abs(gapX); }
			}


			pnt.X = Handles[index].X - gapX;
			pnt.Y = Handles[index].Y - gapY;
			Handles[HandleIndex] = pnt;

			Update();
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2)
			{
				path.StartFigure();
				path.AddLine(handles[0],new Point(handles[0].X,handles[1].Y ));
				path.AddLine(handles[0], new Point(handles[1].X, handles[0].Y));
				path.AddLine(handles[1], new Point(handles[0].X, handles[1].Y));
				path.AddLine(handles[1], new Point(handles[1].X, handles[0].Y));
			}
		}


		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
            if (!_DrawText) { return false; }

            Font font = this.Parent.Font;
            float valuePerPixel = this.Parent.PixelLength;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            RectangleF pathRect = path.GetBounds();

            //string strWidth = "W: "+ItemBase.GetMeasureString(pathRect.Width * this.Parent.PixelLength, 0);
            //string strWidth = "W: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(pathRect.Width * this.Parent.PixelLength, 0, 3, false, 'm');

            string strWidth;

            if (!this.Parent.textEnable)
            {
                strWidth = "W: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(pathRect.Width * this.Parent.PixelLength, 0, 3, false, 'm');
                this.itemTextwidh = strWidth;
            }
            else
            {
                strWidth = this.itemTextwidh;
            }

            PointF ptWidth = new PointF(pathRect.X + pathRect.Width / 2, pathRect.Y + pathRect.Height + font.Height / 2);

            //string strHeight = "H: "+ItemBase.GetMeasureString(pathRect.Height * this.Parent.PixelLength, 0);
            //string strHeight = "H: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(pathRect.Height * this.Parent.PixelLength, 0, 3, false, 'm');

            string strHeight;

            if (!this.Parent.textEnable)
            {
                strHeight = "H: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(pathRect.Height * this.Parent.PixelLength, 0, 3, false, 'm');
                this.itemTexthight = strHeight;
            }
            else
            {
                strHeight = this.itemTexthight;
            }


            PointF ptHeight = new PointF(pathRect.X - (font.Size * strHeight.Length / 2), pathRect.Y + pathRect.Height / 2);

            string strArea;

            if (!this.Parent.textEnable)
            {
                strArea = "A: " + SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString(pathRect.Height * pathRect.Width * this.Parent.PixelLength * this.Parent.PixelLength, 0, 6, false, 'm');
                this.itemTextArea = strArea;
            }
            else
            {
                strArea = this.itemTextArea;
            }


            PointF ptArea = new PointF(pathRect.X + pathRect.Width / 2, pathRect.Y - font.Height / 2);

            GraphicsPath rectPath = new GraphicsPath();
            rectPath.AddString(strWidth, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, ptWidth, format);
            rectPath.AddString(strHeight, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, ptHeight, format);
            rectPath.AddString(strArea, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, ptArea, format);

            path.AddPath(rectPath, false);

            rectPath.Dispose();
            format.Dispose();

            return true;
		}

		internal override void UpdateHandlePath(GraphicsPath path, Point[] handles)
		{
			if (this.IsSelected) {
				foreach (Point pt in handles) {
					path.AddRectangle(new RectangleF(pt.X - 4, pt.Y - 4, 8, 8));
					path.AddRectangle(new RectangleF(pt.X - 3, pt.Y - 3, 6, 6));
					//path.AddRectangle(new RectangleF(pt.X - 2, pt.Y - 2, 4, 4));
					//path.AddRectangle(new RectangleF(pt.X - 1, pt.Y - 1, 2, 2));
				}
			}
		}
	}
}
