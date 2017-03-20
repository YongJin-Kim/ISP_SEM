using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Diagnostics;

namespace SEC.Nanoeye.Controls
{    
    public class BitmapLamp : Control
    {
        public event EventHandler SurfaceChanged;
        public event EventHandler CheckedChanged;

        #region Private 멤버
        private Bitmap _surface;
        private Bitmap TrueImage;
        private Bitmap FalseImage;
        private bool _checked = false;
        #endregion

        #region Public 속성
        protected override bool ShowFocusCues
        {
            get
            {
                return false;// base.ShowFocusCues;
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    this.OnCheckedChanged(EventArgs.Empty);
                }
            }
        }

        public Bitmap Surface
        {
            get { return _surface; }
            set
            {
                if (_surface != value)
                {
                    _surface = value;
                    this.OnSurfaceChanged(EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Public 메서드
        public BitmapLamp()
        {
            base.SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);
            //base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            base.BackgroundImageLayout = ImageLayout.None;
            //base.FlatStyle = FlatStyle.Flat;
            //base.FlatAppearance.BorderSize = 0;
            //base.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //base.FlatAppearance.MouseOverBackColor = Color.Transparent;
            //base.FlatAppearance.CheckedBackColor = Color.Transparent;
            //base.BackColor = Color.Transparent;
            //base.UseVisualStyleBackColor = false;
            //base.UseCompatibleTextRendering = true;
            base.TabStop = false;
            base.DoubleBuffered = true;
        }
        #endregion

        #region Protected 메서드
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

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height / 2);
            }

            base.OnSizeChanged(e);
        }

        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);
            this.AutoSize = false;
        }

        protected virtual void OnSurfaceChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                this.TrueImage = Helper.GetSurfaceImage(this.Surface, true);
                this.FalseImage = Helper.GetSurfaceImage(this.Surface, false);
                this.Region = Helper.GetBitmapRegion(this.FalseImage);

                this.BackgroundImage = (this.Checked) ? this.TrueImage : this.FalseImage;

                base.SetClientSizeCore(this.TrueImage.Width, this.TrueImage.Height);
            }

            if (this.SurfaceChanged != null)
            {
                this.SurfaceChanged(this, e);
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (this.CheckedChanged != null)
            {
                this.CheckedChanged(this, e);
            }

            this.BackgroundImage = (this.Checked) ? this.TrueImage : this.FalseImage;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.BackgroundImage = this.TrueImage;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.BackgroundImage = this.FalseImage;
            base.OnMouseUp(e);
        }
        #endregion
    }
}
