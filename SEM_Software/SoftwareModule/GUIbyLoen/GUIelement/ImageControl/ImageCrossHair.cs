using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Design;


namespace SEC.Nanoeye.Controls
{
    public class ImageCrossHair : Control
    {
        private Bitmap m_PatternImage = null;

		public ImageCrossHair()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

        [Localizable(true)]
        public Bitmap Pattern
        {
            set
            {
                if (m_PatternImage != value)
                {
                    m_PatternImage = value;

                    if (m_PatternImage != null)
                    {
                        this.SetPattern(value);
                    }
                }
            }

            get
            {
                return m_PatternImage;
            }
        }

        public void SetPattern(Bitmap bitmap)
        {
            this.Region = ImageCrossHair.BitmapToRegion(bitmap);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (m_PatternImage != null)
            {
                Graphics g = e.Graphics;

                g.DrawImage(m_PatternImage, 0, 0);
            }
        }

        private static Region BitmapToRegion(Bitmap bitmap)
        {            
            BitmapData bd = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            int width = bitmap.Width;
            int height = bitmap.Height;
            Region region = new Region(Rectangle.Empty);

            unsafe
            {
                uint* pixel = (uint*)bd.Scan0.ToInt32();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if ((*pixel & 0xFF000000) > 0)
                        {
                            region.Union(new Rectangle(x, y, 1, 1));
                        }

                        pixel++;
                    }
                }
            }

            bitmap.UnlockBits(bd);

            return region;
        }
    }
}
