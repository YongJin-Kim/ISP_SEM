using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SEC.GUIelement.MeasuringTools
{
	/// <summary>
	/// Length from line
	/// </summary>
	[Serializable]
	internal class ItemLFL : ItemBase
	{
		public ItemLFL()
			: this(true)
		{
		}

		public ItemLFL(bool text)
		{
			this.MaxHandleCount = int.MaxValue;
			_DrawText = true;
		}

		public override string ToString()
		{
			return "Length from line";
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length < 2) { return; }

			path.AddLine(handles[0], handles[1]);

			double angle = (handles[1].Y - handles[0].Y) / (double)(handles[1].X - handles[0].X);

			int index = 2;
			while (handles.Length > index)
			{

			}
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
			throw new NotImplementedException();
		}
	}
}
