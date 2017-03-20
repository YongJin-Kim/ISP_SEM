using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

using System.Windows.Forms;

using System.Diagnostics;

namespace SEC.GUIelement.Helper
{
	internal class ImageHelper
	{
		public static Bitmap[] ImageDivider(Bitmap img, Orientation oriental, int count)
		{
			Debug.Assert(img != null);
			Debug.Assert(count > 0);

			Bitmap[] result = new Bitmap[count];

			int width, height;
			if ( oriental == Orientation.Horizontal )
			{
				width = img.Width / count;
				height = img.Height;

				for ( int i = 0 ; i < count ; i++ )
				{
					result[i] = new Bitmap(width, height);

					Graphics g = Graphics.FromImage(result[i]);
					g.DrawImage(img, new Rectangle(0, 0, width, height), i * width, 0, width, height, GraphicsUnit.Pixel);
				}

			}
			else
			{
				width = img.Width;
				height = img.Height / count;

				for ( int i = 0 ; i < count ; i++ )
				{
					result[i] = new Bitmap(width, height);

					Graphics g = Graphics.FromImage(result[i]);
					g.DrawImage(img, new Rectangle(0, 0, width, height), 0, i * height, width, height, GraphicsUnit.Pixel);
				}
			}

			return result;
		}
	}
}
