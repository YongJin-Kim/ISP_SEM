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
    [DesignerAttribute(typeof(BitmapButtonDesigner))]
    public class BitmapCheckBox : CheckBox
    {
        public event EventHandler SurfaceChanged;

        #region Private 멤버
        private Bitmap m_Surface;
        private Bitmap TrueImage;
        private Bitmap FalseImage;
        private Bitmap f_Surface;
        
        #endregion

        #region Public 속성
        protected override bool ShowFocusCues
        {
            get
            {
                return false;// base.ShowFocusCues;
            }
        }

        [Localizable(true)]
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

        [Localizable(false)]
        public Bitmap FSurface
        {
            get { return f_Surface; }
            set
            {
                f_Surface = value;
                this.OnSurfaceChanged(EventArgs.Empty);
            }
        }


      

        #endregion

        #region Public 메서드
        public BitmapCheckBox()
        {
            base.SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);
            //base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            base.Appearance = Appearance.Button;
            base.BackgroundImageLayout = ImageLayout.None;
            base.FlatStyle = FlatStyle.Flat;
            base.FlatAppearance.BorderSize = 0;
            //base.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //base.FlatAppearance.MouseOverBackColor = Color.Transparent;
            //base.FlatAppearance.CheckedBackColor = Color.Transparent;
            //base.BackColor = Color.Transparent;
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.TabStop = false;
            base.AutoSize = false;
            base.UseVisualStyleBackColor = false;
            base.UseCompatibleTextRendering = true;
            base.DoubleBuffered = true;
        }
        #endregion

        #region Protected 메서드
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.FlatAppearance.MouseDownBackColor = this.BackColor;
            base.FlatAppearance.MouseOverBackColor = this.BackColor;
            base.FlatAppearance.CheckedBackColor = this.BackColor;

            base.OnBackColorChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
       {
           if (this.Region != null)
           {
               e.Graphics.SetClip(this.Region, CombineMode.Intersect);
			   e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
               e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			   e.Graphics.InterpolationMode = InterpolationMode.High;
           }
           base.OnPaint(e);
       }

       protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            this.BackgroundImage = (this.Checked) ? this.TrueImage : this.FalseImage;
            if (this.BackgroundImage == this.TrueImage)
            {
                this.ForeColor = Color.FromArgb(148, 192, 57);
                
            }
            else
            {
                if (this.Text != "Start")
                {
                    this.ForeColor = Color.FromArgb(26, 45, 60);
                }
                
                
            }

            if (this.TrueImage == null)
            {
                if (this.Checked)
                {
                    this.ForeColor = Color.FromArgb(148, 192, 57);
                }
                else
                {
                    this.ForeColor = Color.FromArgb(26, 45, 60);
                }
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                //base.SetClientSizeCore(this.Surface.Width, this.Surface.Height / 2);
                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height);
            }

            base.OnSizeChanged(e);
        }

        protected virtual void OnSurfaceChanged(EventArgs e)
        {
            if (this.Surface != null)
            {
                this.TrueImage = Helper.GetSurfaceImage(this.Surface, true);
                this.FalseImage = Helper.GetSurfaceImage(this.FSurface, false);
                this.Region = Helper.GetBitmapRegion(this.FalseImage);

                this.BackgroundImage = (this.Checked) ? this.TrueImage : this.FalseImage;
                //if (this.BackgroundImage == this.TrueImage)
                //{
                //    this.ForeColor = Color.FromArgb(50, 235, 251);
                //}

                base.SetClientSizeCore(this.TrueImage.Width, this.TrueImage.Height);
            }

            if (this.SurfaceChanged != null)
            {
                this.SurfaceChanged(this, e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackgroundImage = this.TrueImage ;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BackgroundImage = (this.Checked) ? this.TrueImage : this.FalseImage;
            base.OnMouseUp(e);
        }
        #endregion
    }
}
