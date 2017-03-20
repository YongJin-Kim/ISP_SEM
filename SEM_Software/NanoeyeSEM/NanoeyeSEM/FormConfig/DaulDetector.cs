using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SEC.Nanoeye.Controls;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class DaulDetector : Form
    {
        MiniSEM mainform;
        private Template.ITemplate equip = null;
        private SEC.Nanoeye.NanoColumn.ISEMController _column = null;
        public SEC.Nanoeye.NanoColumn.ISEMController Column
        {
            get { return _column; }
            set
            {
                _column = value;
            }
        }

        public DaulDetector()
        {
            InitializeComponent();
        }

        public DaulDetector(MiniSEM minisem)
        {
            InitializeComponent();
            mainform = minisem;

            equip = SystemInfoBinder.Default.Equip;

            detectClt.ControlValue = equip.ColumnHVCLT;
            detectPmt.ControlValue = equip.ColumnHVPMT;

            
        }

        private void DaulDetector_Shown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.85), Cursor.Position.Y - (int)(this.Height + 10));
        }

        private Point mouseCurrentPoint = new Point(0, 0);
        private void DaulDetector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseCurrentPoint = e.Location;
            }
        }

        private void DaulDetector_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);
            }
        }

        private void MainFormClose_Click(object sender, EventArgs e)
        {
            mainform.DualDetectorEnable = false;
            //this.Hide();
        }

        public void Close()
        {
            this.Dispose();
            this.Close();
        }

        private void BSEChanChange(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            bcbTextChange(bcb);

            bcb.Checked = false;

        }




        public int BSEChan1Value = 1;
        public int BSEChan2Value = 4;
        public int BSEChan3Value = 16;
        public int BSEChan4Value = 64;
        private void bcbTextChange(BitmapCheckBox bcb)
        {
            switch (bcb.Name)
            {
                case "BSEChan1_1":
                case "BSEChan1":
                    if (bcb.Text == "+")
                    {
                        bcb.Text = "-";
                        BSEChan1Value = 2;
                    }
                    else if (bcb.Text == "-")
                    {
                        bcb.Text = "off";
                        BSEChan1Value = 0;
                    }
                    else
                    {
                        bcb.Text = "+";
                        BSEChan1Value = 1;

                    }
                    break;

                case "BSEChan2_1":
                case "BSEChan2":
                    if (bcb.Text == "+")
                    {
                        bcb.Text = "-";
                        BSEChan2Value = 8;
                    }
                    else if (bcb.Text == "-")
                    {
                        bcb.Text = "off";
                        BSEChan2Value = 0;
                    }
                    else
                    {
                        bcb.Text = "+";
                        BSEChan2Value = 4;

                    }
                    break;

                case "BSEChan3_1":
                case "BSEChan3":
                    if (bcb.Text == "+")
                    {
                        bcb.Text = "-";
                        BSEChan3Value = 32;
                    }
                    else if (bcb.Text == "-")
                    {
                        bcb.Text = "off";
                        BSEChan3Value = 0;
                    }
                    else
                    {
                        bcb.Text = "+";
                        BSEChan3Value = 16;

                    }
                    break;

                case "BSEChan4_1":
                case "BSEChan4":
                    if (bcb.Text == "+")
                    {
                        bcb.Text = "-";
                        BSEChan4Value = 128;
                    }
                    else if (bcb.Text == "-")
                    {
                        bcb.Text = "off";
                        BSEChan4Value = 0;
                    }
                    else
                    {
                        bcb.Text = "+";
                        BSEChan4Value = 64;

                    }
                    break;


            }


            BSEValueSUM();


        }

        private void BSEValueSUM()
        {
            int avg = BSEChan1Value + BSEChan2Value + BSEChan3Value + BSEChan4Value;

            ((SECtype.IControlInt)_column["BSE_Detector"]).Value = avg;
          
        }


        private void BSEValueChange(BitmapCheckBox bcb)
        {
            int _bseData = 0;


            if (BSEChan1.Text == "+")
            {
                _bseData += 1;
            }
            else if (BSEChan1.Text == "-")
            {
                _bseData += 2;
            }
            else
            {
                _bseData += 0;
            }




            if (BSEChan2.Text == "+")
            {
                _bseData += 4;
            }
            else if (BSEChan2.Text == "-")
            {
                _bseData += 8;
            }
            else
            {
                _bseData += 0;
            }



            if (BSEChan3.Text == "+")
            {
                _bseData += 16;
            }
            else if (BSEChan3.Text == "-")
            {
                _bseData += 32;
            }
            else
            {
                _bseData += 0;
            }


            if (BSEChan4.Text == "+")
            {
                _bseData += 64;
            }
            else if (BSEChan4.Text == "-")
            {
                _bseData += 128;
            }
            else
            {
                _bseData += 0;
            }


            ((SECtype.IControlInt)_column["BSE_Detector"]).Value = _bseData;
            

            //BSECheckSum();

        }

        private void BSEAmpChange(object sender, EventArgs e)
        {

            BitmapCheckBox bcb = sender as BitmapCheckBox;

            if (!bcb.Checked)
            {
                bcb.Checked = true;
                return;
            }


            int BSEAmpData = 0;

            switch (bcb.Name)
            {
                case "BSEAmp1":
                    BSEAmpData = 1;
                    BSEAmp1.Checked = true;
                    BSEAmp2.Checked = false;
                    BSEAmp3.Checked = false;
                    BSEAmp5.Checked = false;
                    BSEAmp8.Checked = false;

                    break;

                case "BSEAmp2":
                    BSEAmpData = 2;
                    BSEAmp1.Checked = false;
                    BSEAmp2.Checked = true;
                    BSEAmp3.Checked = false;
                    BSEAmp5.Checked = false;
                    BSEAmp8.Checked = false;


                    break;

                case "BSEAmp3":
                    BSEAmpData = 3;
                    BSEAmp1.Checked = false;
                    BSEAmp2.Checked = false;
                    BSEAmp3.Checked = true;
                    BSEAmp5.Checked = false;
                    BSEAmp8.Checked = false;

                    break;

                case "BSEAmp5":
                    BSEAmpData = 9;
                    BSEAmp1.Checked = false;
                    BSEAmp2.Checked = false;
                    BSEAmp3.Checked = false;
                    BSEAmp5.Checked = true;
                    BSEAmp8.Checked = false;
                    break;

                case "BSEAmp8":
                    BSEAmpData = 11;

                    BSEAmp1.Checked = false;
                    BSEAmp2.Checked = false;
                    BSEAmp3.Checked = false;
                    BSEAmp5.Checked = false;
                    BSEAmp8.Checked = true;

                    break;

                default:
                    break;

            }
 
            ((SECtype.IControlInt)_column["BSE_Amp"]).Value = BSEAmpData;

        }

        private void detectClt_ValueChanged(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            CltText.Location = new Point(left, top);


            CltText.Text = value.ToString();
        }

        private void detectPmt_ValueChanged(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            AmpText.Location = new Point(left, top);


            AmpText.Text = value.ToString();
        }
    }
}
