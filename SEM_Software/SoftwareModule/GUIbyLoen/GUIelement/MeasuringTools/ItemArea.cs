//#define VerOri
#define VerFour

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	class ItemArea : ItemBase
	{
		private float m_Area = 0;
		RectangleF maxRect;

		public ItemArea()
			: this(true)
		{ }

		public override string ToString()
		{
			if (_DrawText)
			{
				return "Area";
			}
			else
			{
				return "ClosePath";
			}
		}

		public ItemArea(bool text)
		{
			this.MaxHandleCount = int.MaxValue;
			_DrawText = text;
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2)
			{
				path.AddLines(handles);
				path.CloseFigure();
			}

			if (handles.Length == this.MaxHandleCount)
			{
				Region region = new Region(path);
				RectangleF[] rectArray = region.GetRegionScans(new Matrix());

				float maxArea = 0;
				float area;

				m_Area = 0;
				foreach (RectangleF r in rectArray)
				{
					area = r.Width * r.Height;
					if (area > maxArea) {
						maxRect = new RectangleF(r.Location, r.Size);
					}
					m_Area += area;
				}

				Trace.WriteLine(m_Area.ToString());
			}
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			if ( !_DrawText ) { return false; }

			if (handles.Length == this.MaxHandleCount)
			{
				// Text 정보를 초기화 합니다.
				Font font = this.Parent.Font;
				float valuePerPixel = this.Parent.PixelLength;
	
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Alignment = StringAlignment.Center;

#if VerOri
				// 기존 방식
				RectangleF pathRect = path.GetBounds();

				//String text = m_Area.ToString("F" + decimalPlaces.ToString()) + unitString + "\x00B2";
                String text = ItemBase.GetMeasureString(m_Area * this.Parent.ValuePerPixel, 0) + "\x00B2"; 
                PointF pt = new PointF(pathRect.X + pathRect.Width / 2, pathRect.Y - font.Height);

				GraphicsPath textPath;
				textPath = new GraphicsPath();
				textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pt, format);


				path.AddPath(textPath, false);
#elif VerTwo
				// 가운데 점.
				RectangleF pathRect = path.GetBounds();

				//String text = m_Area.ToString("F" + decimalPlaces.ToString()) + unitString + "\x00B2";
				String text = ItemBase.GetMeasureString(m_Area * this.Parent.ValuePerPixel, 0) + "\x00B2";
				PointF pt = new PointF(pathRect.X + pathRect.Width / 2, pathRect.Y +pathRect.Height/2);

				GraphicsPath textPath;
				textPath = new GraphicsPath();
				textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, pt, format);


				path.AddPath(textPath, false);

#elif VerThree
				// 제일 큰 사각형

				//String text = m_Area.ToString("F" + decimalPlaces.ToString()) + unitString + "\x00B2";
				String text = ItemBase.GetMeasureString(m_Area * this.Parent.ValuePerPixel, 0) + "\x00B2";

				GraphicsPath textPath;
				textPath = new GraphicsPath();
				textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, maxRect, format);


				path.AddPath(textPath, false);

#elif VerFour
				// 가장자리 선.

				RectangleF pathRect = path.GetBounds();

				Point pt1, pt2, pt3;
				pt1 = handles[0];
				pt2 = handles[1];
				int i = 0;
				int j = 0;
				foreach (Point pt in handles) {
					if (pt.Y < pt1.Y) {
						pt1 = pt;
						j = i;
					}
					i++;
				}
				int l,r;
				if (j == handles.Length -1)
					r = 0;
				else 
					r = j+1;
				if (j == 0)
					l = handles.Length - 1;
				else
					l = j - 1;
				if (handles[l].Y < handles[r].Y) {
					pt2 = handles[l];
				}
				else {
					pt2 = handles[r];
				}

				if (pt1.X > pt2.X) {
					pt3 = pt1;
					pt1 = pt2;
					pt2 = pt3;
				}



				// 두점의 거리를 계산합니다.
				float length = GetLinearLength(pt1, pt2);
				// 시작점의 X축에서 끝점까지 시계방향의 각도를 구합니다.
				float angle = GetAngleByPoint(pt1, pt2);

				// 문자열이 표시될 중심점을 계산합니다.
				PointF point = new PointF((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
				// 표시될 측정 문자열을 구성합니다.

				//string[] areaStr = m_Area.ToString("E").Split(new char[] { 'E' });

				//double mantissa = Convert.ToDouble(areaStr[0]);
				//double exponent = Convert.ToDouble(areaStr[1]);

				//String text = ItemBase.GetMeasureString(m_Area * this.Parent.ValuePerPixel * this.Parent.ValuePerPixel, 0) + "\x00B2";
				//string text = ItemBase.GetMeasureStringArea(mantissa, exponent, valuePerPixel);
				//string text = SEC.GenericSupport.MathematicsSupport.NumberConverter.ToUnitString(m_Area * this.Parent.PixelLength * this.Parent.PixelLength, 6, false, 'm')x00B2";
				//string text = SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString(m_Area * this.Parent.PixelLength * this.Parent.PixelLength, 0, 6, false, 'm');

                string text;

                if (!this.Parent.textEnable)
                {
                    text = SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString(m_Area * this.Parent.PixelLength * this.Parent.PixelLength, 0, 6, false, 'm');
                    this.ItemText = text;
                }
                else
                {
                    text = this.ItemText;
                }

				GraphicsPath textPath;
				Matrix m = new Matrix();
				textPath = new GraphicsPath();
				textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, point, format);
				//textPath.AddRectangle(RectangleF.Inflate(textPath.GetBounds(), 2, 2));

				m.RotateAt(angle, point);
				m.Translate(0, -font.Size);
				textPath.Transform(m);
				//textPath.Flatten(null, 0.5F); ;
				path.AddPath(textPath, false);



#endif

				return true;
			}

			return false;
		}

	}
}
