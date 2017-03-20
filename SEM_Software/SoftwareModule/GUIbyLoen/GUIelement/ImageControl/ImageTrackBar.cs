using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SEC.Nanoeye.Controls
{
    public class ImageTrackBar : TrackBar
    {
        public ImageTrackBar()
        {
            base.SuspendLayout();

            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            base.BackColor = Color.Transparent;

            //Panel panel = new Panel();
            //base.Controls.Add(panel);
            //panel.Dock = DockStyle.Fill;

            base.ResumeLayout(false);
        }
    }
}
