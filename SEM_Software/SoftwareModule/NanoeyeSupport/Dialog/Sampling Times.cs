using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.Support.Dialog
{
    public partial class Sampling_Times : Form
    {
        public Sampling_Times()
        {
            InitializeComponent();
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.85), Cursor.Position.Y - (int)(this.Height + 10));
        }
    }
}
