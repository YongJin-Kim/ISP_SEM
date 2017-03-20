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
    [DesignerAttribute(typeof(ImageLabelDesigner))]
    public class BitmapLabel : Label
    {
        #region Private 멤버
        private Image m_Surface;
        private Control OldParent = null;
        #endregion

        #region Public 속성
        [Localizable(true)]
        public Image Surface
        {
            get { return m_Surface; }
            set
            {
                if (value != null)
                {
                    base.SetClientSizeCore(value.Width, value.Height / 2);
                }

                if (m_Surface != value)
                {
                    m_Surface = value;
                    this.ParentInvalidate(this.Bounds);
                }
            }
        }
        #endregion

        #region Public 메서드
        public BitmapLabel()
        {
        }
        #endregion

        #region Protected 메서드
        protected override void  OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            if (this.DesignMode)
            {
                this.Region = new Region(new Rectangle(0, 0, 8, 8));
            }
            else
            {
                this.Region = new Region(Rectangle.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                e.Graphics.Clear(this.ForeColor);
            }

            base.OnPaint(e);
        }

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_Surface != null)
            {
                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height / 2);
            }

            base.OnSizeChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.OldParent != null)
            {
                this.OldParent.Paint -= new PaintEventHandler(Parent_Paint);
                this.OldParent.MouseDown -= new MouseEventHandler(Parent_MouseDown);
                this.OldParent.MouseUp -= new MouseEventHandler(Parent_MouseUp);
            }

            if (this.Parent != null)
            {
                this.Parent.Paint += new PaintEventHandler(Parent_Paint);
                this.Parent.MouseDown += new MouseEventHandler(Parent_MouseDown);
                this.Parent.MouseUp += new MouseEventHandler(Parent_MouseUp);
            }

            this.OldParent = this.Parent;

            base.OnParentChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.ParentInvalidate(this.Bounds);
        }
        #endregion

        #region private 메서드
        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.Bounds.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                this.ParentInvalidate(this.Bounds);
            }
        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Bounds.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                this.ParentInvalidate(this.Bounds);
            }
        }

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            if (this.Visible)
            {
                Graphics g = e.Graphics;

                Helper.DrawControlImage(
                    g,
                    this.Surface,
                    this.Bounds,
                    true,
                    this.BackColor,
                    this.Enabled);

                Rectangle bounds = Helper.GetPaddingBounds(this.Bounds, this.Padding);

                Helper.DrawImage(
                    g,
                    bounds,
                    this.BackgroundImage,
                    this.BackgroundImageLayout,
                    this.Enabled,
                    this.BackColor);

                Helper.DrawText(
                    g,
                    bounds,
                    this.Text,
                    this.TextAlign,
                    this.Font,
                    this.Enabled,
                    this.ForeColor,
                    this.BackColor);
            }
        }

        private void ParentInvalidate(Rectangle clipRect)
        {
            if (this.Parent != null)
            {
                this.Parent.Invalidate(clipRect);
            }
        }
        #endregion
    }
}
