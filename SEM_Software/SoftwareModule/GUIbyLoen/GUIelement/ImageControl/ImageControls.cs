using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SEC.Nanoeye.Controls
{
    public class ImageControl
    {
        internal static void DrawControlImage(Graphics g, Image image, Rectangle bounds, int x, int y, Color backcolor, bool enabled)
        {
            if (enabled)
            {
                g.DrawImage(image, bounds, new Rectangle(x, y, bounds.Width, bounds.Height), GraphicsUnit.Pixel);
            }
            else
            {
                g.SetClip(bounds);
                ControlPaint.DrawImageDisabled(g, image, x, y, backcolor);
                g.ResetClip();
            }
        }

        internal static Region GetBitmapRegion(Bitmap bitmap)
        {
            Region region = new Region(Rectangle.Empty);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    if (bitmap.GetPixel(x, y).A > 0x00)
                    {
                        region.Union(new Rectangle(x, y, 1, 1));
                    }
                }
            }

            return region;
        }

        internal static Region GetExpandedRegion(Control control, Region region, float width, float height)
        {
            SizeF size;

            using (Graphics g = control.CreateGraphics())
            {
                size = region.GetBounds(g).Size;                
            }

            Region r = new Region(region.GetRegionData());
            Matrix m = new Matrix();
            m.Scale((size.Width + width) / size.Width, (size.Height + height) / size.Height);
            m.Translate(-width / 2, -height / 2);
            r.Transform(m);

            return r;
        }
    }
}
