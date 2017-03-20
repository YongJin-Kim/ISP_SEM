using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SEC.Nanoeye.Controls
{
    internal static class Helper
    {
        internal static Region GetBitmapRegion(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            GraphicsPath path = new GraphicsPath();
            Rectangle area = Rectangle.Empty;

            BitmapData bd =
                image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* p = (int*)bd.Scan0.ToInt32();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if ((*p & 0xFF000000) != 0)
                        {
                            if (area == Rectangle.Empty)
                            {
                                area = new Rectangle(x, y, 1, 1);
                            }
                            else
                            {
                                area.Width++;
                            }
                        }
                        else
                        {
                            if (area != Rectangle.Empty)
                            {
                                path.AddRectangle(area);
                                area = Rectangle.Empty;
                            }
                        }

                        p++;
                    }

                    if (area != Rectangle.Empty)
                    {
                        path.AddRectangle(area);
                        area = Rectangle.Empty;
                    }
                }                
            }

            image.UnlockBits(bd);

            path.CloseAllFigures();

            return new Region(path);
        }

        internal static Rectangle GetPaddingBounds(Rectangle bounds, Padding padding)
        {
            bounds.X += padding.Left; 
            bounds.Width -= (padding.Left + padding.Right);
            bounds.Y += padding.Top; 
            bounds.Height -= (padding.Top + padding.Bottom);

            return bounds;
        }

        internal static Bitmap GetSurfaceImage(
            Image surface,
            bool isPressed)
        {
            
            //Bitmap image = new Bitmap(surface.Width, surface.Height / 2);
            Bitmap image = new Bitmap(surface.Width, surface.Height);
            
            

            using (Graphics g = Graphics.FromImage(image))
            {
                Rectangle dstRect = new Rectangle(0, 0, image.Width, image.Height);
                Rectangle srcRect = dstRect;

               

                g.DrawImage(surface, dstRect, srcRect, GraphicsUnit.Pixel);
            }

            return image;
        }

        internal static void DrawControlImage(
            Graphics g, 
            Image image, 
            Rectangle clipBounds, 
            bool isPressed,
            Color backcolor, 
            bool enabled)
        {
            if (image == null)
            {
                return;
            }

            int x = clipBounds.Left;
            int y = clipBounds.Top;
 
            if (enabled)
            {
                if (isPressed)
                {
                    //clipBounds.Y += image.Height / 2;
                    clipBounds.Y += image.Height;
                    
                    
                }

                g.DrawImage(image, x, y, clipBounds, GraphicsUnit.Pixel);
            }
            else
            {
                ControlPaint.DrawImageDisabled(g, image, x, y, backcolor);
            } 
        }

        internal static void DrawImage(
            Graphics g, 
            Rectangle bounds, 
            Image image, 
            ImageLayout imageLayout, 
            bool enabled,
            Color backColor)
        {
            if (g == null || image == null)
            {
                return;
            }

            Rectangle dstBounds = new Rectangle();            

            switch (imageLayout)
            {
            case ImageLayout.Center:
                dstBounds = new Rectangle(
                    bounds.X + (bounds.Width - image.Width) / 2,
                    bounds.Y + (bounds.Height - image.Height) / 2,
                     
                    image.Width,
                    image.Height);
                break;
            case ImageLayout.None:
                dstBounds = new Rectangle(
                    bounds.X,
                    bounds.Y,
                    image.Width,
                    image.Height);
                break;
            case ImageLayout.Stretch:
                return;
            case ImageLayout.Tile:
                return;
            case ImageLayout.Zoom:
                return;
            }

            if (enabled)
            {
                g.DrawImage(image, dstBounds);
            }
            else
            {
                ControlPaint.DrawImageDisabled(g, image, dstBounds.X, dstBounds.Y, backColor);
            }
        }

        internal static void DrawText(
            Graphics g,
            Rectangle bounds,
            String text,
            ContentAlignment textAlign,
            Font font,
            bool enabled,
            Color foreColor,
            Color backColor)
        {
            if (g == null || text == null || font == null)
            {
                return;
            }

            StringFormat format = new StringFormat();

            switch (textAlign)
            {
            case ContentAlignment.TopLeft:
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Near;
                break;
            case ContentAlignment.TopCenter:
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Near;
                break;
            case ContentAlignment.TopRight:
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Near;
                break;
            case ContentAlignment.MiddleLeft:
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                break;
            case ContentAlignment.MiddleCenter:
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                break;
            case ContentAlignment.MiddleRight:
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;
                break;
            case ContentAlignment.BottomLeft:
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Far;
                break;
            case ContentAlignment.BottomCenter:
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Far;
                break;
            case ContentAlignment.BottomRight:
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Far;
                break;
            }

            if (enabled)
            {
                using (SolidBrush brush = new SolidBrush(foreColor))
                {
                    //SmoothingMode mode = g.SmoothingMode;
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    
                    g.DrawString(text, font, brush, bounds, format);

                    //g.SmoothingMode = mode;
                }
            }
            else
            {
                ControlPaint.DrawStringDisabled(g, text, font, backColor, bounds, format);
            }
        }
    }
}
