using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	class ItemLength : ItemBase
	{
		private float m_Length = 0;

		public ItemLength()
			: this(true)
		{ }

		public ItemLength(bool text)
		{
			this.MaxHandleCount = int.MaxValue;
			_DrawText = text;
		}

		public override string ToString()
		{
			if (_DrawText)
			{
				return "Length";
			}
			else
			{
				return "OpenPath";
			}
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2)
			{
				path.AddLines(handles);

				m_Length = 0;
				for (int i = 1; i < handles.Length; i++)
				{
					m_Length += GetLinearLength(handles[i - 1], handles[i]);
				}
			}
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			if ( !_DrawText ) { return false; }

			if (handles.Length >= 2)
			{
				// Text 정보를 초기화 합니다.
				Font font = this.Parent.Font;

				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Alignment = StringAlignment.Center;

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
				int l, r;
				if (j == handles.Length - 1)
					r = 0;
				else
					r = j + 1;
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
				//String text = ItemBase.GetMeasureString(m_Length * this.Parent.PixelLength, 0);
				//string text	 = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(m_Length * this.Parent.PixelLength, 0,3, false,'m');

                string text;

                if (!this.Parent.textEnable)
                {
                    text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(m_Length * this.Parent.PixelLength, 0, 3, false, 'm');
                    this.ItemText = text;
                }
                else
                {
                    text = this.ItemText;
                }

				// 시작점의 X축에서 끝점까지 시계방향의 각도를 구합니다.
				float angle = GetAngleByPoint(pt1, pt2);

				// 문자열이 표시될 중심점을 계산합니다.
				PointF point = new PointF((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
				// 표시될 측정 문자열을 구성합니다.

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

				return true;
			}
			return false;
		}
	}
}
