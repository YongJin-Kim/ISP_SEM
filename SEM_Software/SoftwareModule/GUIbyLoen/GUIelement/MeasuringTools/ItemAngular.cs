	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Diagnostics;

namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	internal class ItemAngular : ItemBase
	{
		private float m_AngleA = 0;
		private float m_AngleB = 0;
		private float m_AngleDelta = 0;

		public ItemAngular()
		{
			this.MaxHandleCount = 3;
		}

		public override string ToString()
		{
			return "Angular";
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2)
			{
				path.AddLines(handles);
			}

			if (handles.Length == this.MaxHandleCount)
			{
				path.StartFigure();

				m_AngleA = GetAngleByPoint(handles[1], handles[0]);
				m_AngleB = GetAngleByPoint(handles[1], handles[2]);
				Rectangle rect = new Rectangle(handles[1].X - 12, handles[1].Y - 12, 24, 24);

				if (m_AngleA > m_AngleB)
				{
					m_AngleDelta = 360F - (m_AngleA - m_AngleB);
				}
				else
				{
					m_AngleDelta = m_AngleB - m_AngleA;
				}

				path.AddArc(rect, m_AngleA, m_AngleDelta);
			}

		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length == this.MaxHandleCount)
			{
				// Text 정보를 초기화 합니다.
				Font font = this.Parent.Font;
				float valuePerPixel = this.Parent.PixelLength;
				int anglePlaces = this.Parent.AnglePlaces;

				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Alignment = StringAlignment.Near;

				// 문자열이 표시될 중심점을 계산합니다.
				PointF point = new PointF(handles[1].X + 16, handles[1].Y - font.Height * 0.6F);
				// 표시될 측정 문자열을 구성합니다.				
                //String text = m_AngleDelta.ToString("F" + anglePlaces.ToString()) + "\x00B0";

                String text;

                if (!this.Parent.textEnable)
                {
                    text = m_AngleDelta.ToString("F" + anglePlaces.ToString()) + "\x00B0";
                    this.ItemText = text;
                }
                else
                {
                    text = this.ItemText;
                }
				// 문자열이 회전될 각도를 계산합니다.
				float angle = m_AngleA + m_AngleDelta / 2F;
				if (m_AngleDelta < 33)
				{
					angle += 180;
					point.X -= 8;
				}

				GraphicsPath textPath;
				Matrix m = new Matrix();
				textPath = new GraphicsPath();
				textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, point, format);

				m.RotateAt(angle, handles[1]);
				m.Translate(0, font.Size);
				textPath.Transform(m);
				path.AddPath(textPath, false);
			}

			return true;
		}
	}
}
