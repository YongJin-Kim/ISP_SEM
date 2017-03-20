using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class CameraCBcontrol : Form
    {
        //private MiniSEM minisem;

        public CameraCBcontrol()
        {

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();
        }

        public bool CameraEnbale = false;
        

        //public CameraCBcontrol(MiniSEM MainForm)
        //{
        //    InitializeComponent();

        //    minisem = MainForm;
        //}

        private void CCcontrastValueChange(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraContrast = m_ContrastDisp.Value;
        }

        private void CCbrightnessValueChange(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraBirghtness = m_BrightnessDisp.Value;
        }

        private void FormClose(object sender, EventArgs e)
        {
            CameraEnbale = false;
            this.Dispose();
            this.Close();
        }

        private void FormShown(object sender, EventArgs e)
        {
            CameraEnbale = true;
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.55), Cursor.Position.Y - (int)(this.Height + 10));
            
        }

        //private void CameraLeft_TextChanged(object sender, EventArgs e)
        //{
        //    //int a = Convert.ToInt32(CameraLeft.Text);
        //    Properties.Settings.Default.CameraLeft = Convert.ToInt32(CameraLeft.Text);
        //}

        //private void CameraRight_TextChanged(object sender, EventArgs e)
        //{
        //    Properties.Settings.Default.CameraRight = Convert.ToInt32(CameraRight.Text);
        //}

        //private void CameraWidth_TextChanged(object sender, EventArgs e)
        //{
        //    Properties.Settings.Default.CameraWidth = Convert.ToInt32(CameraWidth.Text);
        //}

        //private void CameraHight_TextChanged(object sender, EventArgs e)
        //{
        //    Properties.Settings.Default.CameraHight = Convert.ToInt32(CameraHight.Text);
        //}



        


    }
}
