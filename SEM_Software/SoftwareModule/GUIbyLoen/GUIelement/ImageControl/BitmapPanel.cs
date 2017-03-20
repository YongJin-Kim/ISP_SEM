using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace SEC.Nanoeye.Controls
{
    [DesignerAttribute(typeof(BitmapPanelDesigner))]
    public partial class BitmapPanel : Panel
    {
        public event EventHandler SurfaceChanged;

        #region Private 멤버
        private Bitmap m_Surface = null;
        #endregion
        
        public Bitmap Surface
        {
            get { return m_Surface; }
            set
            {
                if (m_Surface != value)
                {
                    m_Surface = value;

                    this.OnSurfaceChanged(EventArgs.Empty);
                }
            }
        }

        public BitmapPanel()
        {
            base.SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);
            //base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            base.BackgroundImageLayout = ImageLayout.None;
            //base.BackColor = Color.Transparent;
            base.AutoSize = false;
            base.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Region != null)
            {
                e.Graphics.SetClip(this.Region, CombineMode.Intersect);
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                e.Graphics.InterpolationMode = InterpolationMode.Low;
            }
            base.OnPaint(e);

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.Region != null)
            {
                e.Graphics.SetClip(this.Region, CombineMode.Intersect);
            }
            base.OnPaintBackground(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height);
            }

            base.OnSizeChanged(e);
        }

        protected virtual void OnSurfaceChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                this.Region = Helper.GetBitmapRegion(this.Surface);                
                //this.Region = new Region(new Rectangle(0, 0, 128, 128));
                base.BackgroundImage = this.Surface;

                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height);
            }

            if (this.SurfaceChanged != null)
            {
                this.SurfaceChanged(this, e);
            }
        }

        //protected override void OnPaint(PaintEventArgs pevent)
        //{
        //    Graphics g = pevent.Graphics;

        //    if (this.Surface != null)
        //    {
        //        if (this.Enabled)
        //        {
        //            g.DrawImage(this.Surface, 
        //                pevent.ClipRectangle.X,
        //                pevent.ClipRectangle.Y, 
        //                pevent.ClipRectangle,
        //                GraphicsUnit.Pixel);

        //            //string s = string.Format("{0}, {1}", this.Name, pevent.ClipRectangle);
        //            //Trace.WriteLine(s);
        //        }
        //        else
        //        {
        //            ControlPaint.DrawImageDisabled(g, this.Surface, 0, 0, this.BackColor);
        //        }
        //    }

        //    base.OnPaint(pevent);
        //}

        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    base.OnPaintBackground(pevent);
        //}
    }
}
