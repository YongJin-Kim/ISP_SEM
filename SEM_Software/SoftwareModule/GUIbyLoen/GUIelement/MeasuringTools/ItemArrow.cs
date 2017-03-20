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
	internal class ItemArrow : ItemBase
	{
		public ItemArrow()
		{
			this.MaxHandleCount = 2;
		}

		public override string ToString()
		{
			return "Arrow";
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length >= 2)
			{
				path.StartFigure();
				path.AddLines(handles);
			}
		}


		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			return false;
		}

		internal override void UpdateHandlePath(GraphicsPath path, Point[] handles)
		{
			if (this.IsSelected) {	// 선택 된 경우
				foreach (Point pt in handles) {
					path.AddRectangle(new RectangleF(pt.X - 4, pt.Y - 4, 8, 8));
					path.AddRectangle(new RectangleF(pt.X - 3, pt.Y - 3, 6, 6));
					//path.AddRectangle(new RectangleF(pt.X - 2, pt.Y - 2, 4, 4));
					//path.AddRectangle(new RectangleF(pt.X - 1, pt.Y - 1, 2, 2));
				}
			}
			if ( handles.Length == 2 )
			{
				float angle = GetAngleByPoint(handles[0], handles[1]);

				GraphicsPath arrowPath = new GraphicsPath();
				Matrix m = new Matrix();
				arrowPath.AddLine(handles[1], new Point(handles[1].X - 10, handles[1].Y - 5));
				arrowPath.AddLine(handles[1], new Point(handles[1].X - 5, handles[1].Y - 10));

				m.RotateAt(angle - 45, handles[1]);
				arrowPath.Transform(m);

				path.AddPath(arrowPath, false);
			}
		}

	}
}
