using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;  
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class SamplingTime : Form
    {
        MiniSEM mainForm;

        private int _SamplingTimeValue = 1;
        public int SamplingTimeValue
        {
            get
            {
                return _SamplingTimeValue;
            }
            set
            {
                _SamplingTimeValue = value;
            }
        }

        public SamplingTime()
        {
            InitializeComponent();
        }

        public SamplingTime(MiniSEM miniSEM)
        {
            mainForm = miniSEM;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();
        }

        


        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point((Cursor.Position.X - (int)(this.Width / 2))-4, (Cursor.Position.Y - (int)(this.Height))-10);
        }

        public void FormLocation()
        {
            this.Location = new Point((Cursor.Position.X - (int)(this.Width / 2)) - 4, (Cursor.Position.Y - (int)(this.Height)) - 10);
        }

        public void SamplingChecked()
        {
            switch (_SamplingTimeValue)
            {
                case 1:
                    SampleTime1.Checked = true;
                    break;

                case 2:
                    SampleTime2.Checked = true;
                    break;

                case 3:
                    SampleTime3.Checked = true;
                    break;

                case 4:
                    SampleTime4.Checked = true;
                    break;

                case 5:
                    SampleTime5.Checked = true;
                    break;

                case 6:
                    SampleTime6.Checked = true;
                    break;
            }
        }

        private void SamplingTimeValueChange(object sender, EventArgs e)
        {
            RadioButton bnt = sender as RadioButton;


            switch (bnt.Text)
            {
                case "1":
                    _SamplingTimeValue = 1;
                    break;

                case "2":
                    _SamplingTimeValue = 2;
                    break;

                case "3":
                    _SamplingTimeValue = 3;
                    break;

                case "4":
                    _SamplingTimeValue = 4;
                    break;

                case "5":
                    _SamplingTimeValue = 5;
                    break;

                case "6":
                    _SamplingTimeValue = 6;
                    break;
            }

            //_SamplingTimeValue = SamplingProgress.Value;
        }

        private void FormClose(object sender, EventArgs e)
        {
           
            mainForm.SamplingBtnClose();
            //this.Hide();
        }




    }
}
