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
	class ItemEllipse : ItemBase
	{
		public ItemEllipse()
			: this(true)
		{ }

		public override string ToString()
		{
			if (_DrawText)
			{
				return "AreaEllipse";
			}
			else
			{
				return "Ellipse";
			}
		}

		public ItemEllipse(bool text)
		{
			this.MaxHandleCount = 2;
			_DrawText = text;
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

		protected override void UpdateShapePath(System.Drawing.Drawing2D.GraphicsPath path, System.Drawing.Point[] handles)
		{
			if (handles.Length == 2) {
				//path.StartFigure();
				int width = (handles[0].X - handles[1].X);
				int height = (handles[0].Y - handles[1].Y);
				int x1, y1;
				if (width < 0) {
					x1 = handles[0].X;
					width = -width;
				}
				else {
					x1 = handles[1].X;
//					width = width;
				}
				if (height < 0) {
					y1 = handles[0].Y;
					height = -height;
				}
				else {
					y1 = handles[1].Y;
//					height = height;
				}
				// 원호 그리기
				path.AddEllipse(x1,y1,width,height);

				if ( _DrawText ) {
					// 좌우 길이의 반 그리기
					path.AddLine(x1 + width / 2, y1 + height / 2, x1 + width, y1 + height / 2);

					// 위아래 길이의 반 그리기
					path.AddLine(x1 + width / 2, y1 + height / 2, x1 + width / 2, y1);
				}
				//path.CloseFigure();
			}
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
            if (!_DrawText) { return false; }

            //path.StartFigure();

            Font font = this.Parent.Font;
            float valuePerPixel = this.Parent.PixelLength;




            RectangleF pathRect = path.GetBounds();
            double halfPixel = this.Parent.PixelLength / 2d;
            double width = pathRect.Width * halfPixel;
            double height = pathRect.Height * halfPixel;

            // 좌우 길이의 반
            //string strR1 = "R1: " + SEC.MathematicsSupport.NumberConverter.ToUnitString(width, 3, false) + "m";
            //string strR1 = "R1: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(width, 0, 3, false, 'm');

            string strR1;

            if (!this.Parent.textEnable)
            {
                strR1 = "R1: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(width, 0, 3, false, 'm');
                this.itemTextR1 = strR1;
            }
            else
            {
                strR1 = this.itemTextR1;
            }
            PointF pntR1 = new PointF(pathRect.X + pathRect.Width * 3 / 4, pathRect.Y - Parent.Font.Height / 2);




            // 위아래 길이의 반
            //string strR2 = "R2: " + SEC.MathematicsSupport.NumberConverter.ToUnitString(height, 3, false) + "m";
            //string strR2 = "R2: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(height, 0, 3, false, 'm');
            string strR2;

            if (!this.Parent.textEnable)
            {
                strR2 = "R2: " + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(height, 0, 3, false, 'm');
                this.itemTextR2 = strR2;
            }
            else
            {
                strR2 = this.itemTextR2;
            }


            //PointF pntR1 = new PointF(pathRect.X + pathRect.Width * 3 / 4, pathRect.Y - Parent.Font.Height / 2);

            PointF pntR2 = new PointF(pathRect.X + pathRect.Width + Parent.Font.Size * strR1.Length / 2, pathRect.Y + pathRect.Height / 4);

            // 넓이
            //string strArea = "A: " + SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString(Math.PI * width * height, 0, 6, false, 'm');
            string strArea;

            if (!this.Parent.textEnable)
            {
                strArea = "A: " + SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString(Math.PI * width * height, 0, 6, false, 'm');
                this.itemTextArea = strArea;
            }
            else
            {
                strArea = this.itemTextArea;
            }


            PointF pntArea = new PointF(pathRect.X + pathRect.Width / 2, pathRect.Y - Parent.Font.Height);

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            GraphicsPath ellPath = new GraphicsPath();
            ellPath.AddString(strR1, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pntR1, format);
            ellPath.AddString(strR2, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pntR2, format);
            ellPath.AddString(strArea, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pntArea, format);

            path.AddPath(ellPath, false);

            ellPath.Dispose();
            format.Dispose();

            //path.CloseFigure();
            return true;
		}

		internal override void UpdateHandlePath(GraphicsPath path, Point[] handles)
		{
			if (this.IsSelected) {
				foreach (Point pt in handles) {
					path.AddRectangle(new RectangleF(pt.X - 4, pt.Y - 4, 8, 8));
					path.AddRectangle(new RectangleF(pt.X - 3, pt.Y - 3, 6, 6));
				}
			}
		}
	}
}
