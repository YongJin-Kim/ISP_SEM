using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.Controls
{
    [DesignerAttribute(typeof(StaticLabelDesigner))]
    public partial class StaticLabel : Control
    {
        private event EventHandler TextAlignChanged;
        private StringFormat m_StringFormat = new StringFormat();
        private ContentAlignment m_TextAlign = ContentAlignment.TopLeft;
        private Control m_Parent = null;

        [Localizable(true)]
        [DefaultValue((int)ContentAlignment.TopLeft)]
        public ContentAlignment TextAlign
        {
            get { return m_TextAlign; }
            set
            {
                if (m_TextAlign != value)
                {
                    m_TextAlign = value;

                    m_StringFormat.Alignment =
                        StaticLabel.GetHorizontalAlignment(m_TextAlign);
                    m_StringFormat.LineAlignment =
                        StaticLabel.GetVerticalAlignment(m_TextAlign);

                    this.OnTextAlignChnged(EventArgs.Empty);
                }
            }
        }

        public StaticLabel()
        {
            InitializeComponent();

            this.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            if (this.Visible)
            {
                Graphics g = e.Graphics;

                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    if (this.Enabled)
                    {
                        g.DrawString(
                            this.Text,
                            this.Font, 
                            brush, 
                            this.Bounds, 
                            m_StringFormat);
                    }
                    else
                    {
                        ControlPaint.DrawStringDisabled(
                            g, 
                            this.Text, 
                            this.Font, 
                            this.BackColor, 
                            this.Bounds, 
                            m_StringFormat);
                    }
                }

                g.Flush();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                Graphics g = e.Graphics;

                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    g.FillRegion(brush, this.Region);
                }

                using (Pen pen = new Pen(this.ForeColor))
                {
                    g.DrawRectangle(pen, 0, 0, 7, 7);
                }
            }

            base.OnPaint(e);
        }

        protected virtual void OnTextAlignChnged(EventArgs e)
        {
            if (TextAlignChanged != null)
            {
                TextAlignChanged(this, e);
            }

            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (base.DesignMode)
            {
                this.Region = new Region(new RectangleF(0, 0, 8, 8));
            }
            else
            {
                this.Region = new Region(Rectangle.Empty);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (this.Parent != m_Parent)
            {
                if (m_Parent != null)
                {
					m_Parent.Paint -= new PaintEventHandler(Parent_Paint);
                }

                if (this.Parent != null)
                {
                    this.Parent.Paint += new PaintEventHandler(Parent_Paint);
                }

                m_Parent = this.Parent;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.ParentInvalidate(this.Bounds);
        }

        private void ParentInvalidate(Rectangle bounds)
        {
            if (m_Parent != null)
            {
                m_Parent.Invalidate(bounds);
            }
        }


        private static StringAlignment GetHorizontalAlignment(ContentAlignment align)
        {
            if (((int)align & (int)AlignmentHorizontal.Near) > 0)
            {
                return StringAlignment.Near;
            }
            else if (((int)align & (int)AlignmentHorizontal.Center) > 0)
            {
                return StringAlignment.Center;
            }
            else if (((int)align & (int)AlignmentHorizontal.Far) > 0)
            {
                return StringAlignment.Far;
            }

            throw new InvalidEnumArgumentException();
        }

        private static StringAlignment GetVerticalAlignment(ContentAlignment align)
        {
            if (((int)align & (int)AlignmentVertical.Near) > 0)
            {
                return StringAlignment.Near;
            }
            else if (((int)align & (int)AlignmentVertical.Center) > 0)
            {
                return StringAlignment.Center;
            }
            else if (((int)align & (int)AlignmentVertical.Far) > 0)
            {
                return StringAlignment.Far;
            }

            throw new InvalidEnumArgumentException();
        }
    }
}
