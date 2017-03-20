using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class MotorSpeedSetting : Form
    {
        public MotorSpeedSetting()
        {
            InitializeComponent();
        }

        public MotorSpeedSetting(MotorStage motorMain)
        {
            InitializeComponent();

            MotorStage = motorMain;

            SerialPortSearch();

            SettingsLoad();


        }

        private void SettingsLoad()
        {
            XLimitLeft.Text = Properties.Settings.Default.MotorStageXLeft.ToString();
            XLimitRight.Text = Properties.Settings.Default.MotorStageXRight.ToString();
            YLimitTop.Text = Properties.Settings.Default.MotorStageYTop.ToString();
            YLimitBottom.Text = Properties.Settings.Default.MotorStageYBottom.ToString();
            TLimitLeft.Text = Properties.Settings.Default.MotorStageTLeft.ToString();
            TLimitRight.Text = Properties.Settings.Default.MotorStageTRight.ToString();
            ZLimitTop.Text = Properties.Settings.Default.MotorStageZTop.ToString();
            ZLimitBottom.Text = Properties.Settings.Default.MotorStageZBottom.ToString();

            XPitch.Text = Properties.Settings.Default.MotorXPitch.ToString();
            YPitch.Text = Properties.Settings.Default.MotorYPitch.ToString();
            RPitch.Text = Properties.Settings.Default.MotorRPitch.ToString();
            TPitch.Text = Properties.Settings.Default.MotorTPitch.ToString();
            ZPitch.Text = Properties.Settings.Default.MotorZPitch.ToString();

            XHome.Text = Properties.Settings.Default.MotorXHome.ToString();
            YHome.Text = Properties.Settings.Default.MotorYHome.ToString();
            THome.Text = Properties.Settings.Default.MotorTHome.ToString();
            ZHome.Text = Properties.Settings.Default.MotorZHome.ToString();

            SpeedMin.Text = Properties.Settings.Default.MotorSpeedMin.ToString();
            SpeedMax.Text = Properties.Settings.Default.MotorSpeedMax.ToString();
            SpeedRtxt.Text = Properties.Settings.Default.MotorRspeed.ToString();
            SpeedTtxt.Text = Properties.Settings.Default.MotorTspeed.ToString();
            SpeedZtxt.Text = Properties.Settings.Default.MotorZspeed.ToString();


            SenserX.Checked = Properties.Settings.Default.MotorSenserX;
            SenserY.Checked = Properties.Settings.Default.MotorSenserY;
            SenserT.Checked = Properties.Settings.Default.MotorSenserT;
            SenserZ.Checked = Properties.Settings.Default.MotorSenserZ;

            DirectionX.Checked = Properties.Settings.Default.MotorXDirection;
            DirectionY.Checked = Properties.Settings.Default.MotorYDirection;
            DirectionR.Checked = Properties.Settings.Default.MotorRDirection;
            DirectionT.Checked = Properties.Settings.Default.MotorTDirection;
            DirectionZ.Checked = Properties.Settings.Default.MotorZDirection;

            

        }

        private void SerialPortSearch()
        {
            string[] port = System.IO.Ports.SerialPort.GetPortNames();

            int portcount = 0;
            int portSelect = 0;

            foreach (string portno in port)
            {
                StageSerialProtCombo.Items.Add(portno);
                
                if (Properties.Settings.Default.StagePortName == portno)
                {
                    portSelect = portcount;
                }
                portcount++;
            }

            if(portSelect == 0)
            {
                return;
            }

            StageSerialProtCombo.SelectedIndex = portSelect;




        }

        private MotorStage MotorStage = null;
         

        private void checkBox1_Click(object sender, EventArgs e)
        {

            //Properties.Settings.Default.Save();

            MotorStage.MSSEnable = false;
            MSCFormClose.Checked = false;

            SystemInfoBinder.Default.SetManager.StageSave("stage1");
            
            this.Close();
        }


       

        private void MotorSpeedSetting_Shown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.85), Cursor.Position.Y - (int)(this.Height + 10));
        }

        private Point mouseCurrentPoint = new Point(0, 0);
        private void MotorSpeedSetting_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseCurrentPoint = e.Location;
            }
        }

        private void MotorSpeedSetting_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);
            }
        }

        private void StageConnectBtn_Click(object sender, EventArgs e)
        {
            StageConnectBtn.Checked = false;

            MotorStage.StageLoad();
        }

        private void StageSerialProtCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StagePortName = StageSerialProtCombo.SelectedItem.ToString();
        }

        private void StageSettings_TextChange(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            switch (tb.Name)
            {
                case "XLimitLeft":
                    Properties.Settings.Default.MotorStageXLeft = Convert.ToInt32(XLimitLeft.Text);
                    break;

                case "XLimitRight":
                    Properties.Settings.Default.MotorStageXRight = Convert.ToInt32(XLimitRight.Text);
                    break;

                case "YLimitTop":
                    Properties.Settings.Default.MotorStageYTop = Convert.ToInt32(YLimitTop.Text);
                    break;

                case "YLimitBottom":
                    Properties.Settings.Default.MotorStageYBottom = Convert.ToInt32(YLimitBottom.Text);
                    break;

                case "TLimitLeft":
                    Properties.Settings.Default.MotorStageTLeft = Convert.ToInt32(TLimitLeft.Text);
                    break;

                case "TLimitRight":
                    Properties.Settings.Default.MotorStageTRight = Convert.ToInt32(TLimitRight.Text);
                    break;

                case "ZLimitTop":
                    Properties.Settings.Default.MotorStageZTop = Convert.ToInt32(ZLimitTop.Text);
                    break;

                case "ZLimitBottom":
                    Properties.Settings.Default.MotorStageZBottom = Convert.ToInt32(ZLimitBottom.Text);
                    break;

                case "XPitch":
                    Properties.Settings.Default.MotorXPitch = Convert.ToDouble(XPitch.Text);
                    break;

                case "YPitch":
                    Properties.Settings.Default.MotorYPitch = Convert.ToDouble(YPitch.Text);
                    break;

                case "RPitch":
                    Properties.Settings.Default.MotorRPitch = Convert.ToDouble(RPitch.Text);
                    break;

                case "TPitch":
                    Properties.Settings.Default.MotorTPitch = Convert.ToDouble(TPitch.Text);
                    break;

                case "ZPitch":
                    Properties.Settings.Default.MotorZPitch = Convert.ToDouble(ZPitch.Text);
                    break;

                case "XHome":
                    Properties.Settings.Default.MotorXHome = Convert.ToDouble(XHome.Text);
                    break;

                case "YHome":
                    Properties.Settings.Default.MotorYHome = Convert.ToDouble(YHome.Text);
                    break;

                case "THome":
                    Properties.Settings.Default.MotorTHome = Convert.ToDouble(THome.Text);
                    break;

                case "ZHome":
                    Properties.Settings.Default.MotorZHome = Convert.ToDouble(ZHome.Text);
                    break;

                case "SpeedMin":
                    Properties.Settings.Default.MotorSpeedMin = Convert.ToInt32(SpeedMin.Text);
                    break;

                case "SpeedMax":
                    Properties.Settings.Default.MotorSpeedMax = Convert.ToInt32(SpeedMax.Text);
                    break;

                case "SpeedRtxt":
                    Properties.Settings.Default.MotorRspeed = Convert.ToInt32(SpeedRtxt.Text);
                    break;

                case "SpeedTtxt":
                    Properties.Settings.Default.MotorTspeed = Convert.ToInt32(SpeedTtxt.Text);
                    break;
                    
                case "SpeedZtxt":
                    Properties.Settings.Default.MotorZspeed = Convert.ToInt32(SpeedZtxt.Text);
                    break;

                default:
                    break;


            }
        }

        private void StageSettings_CheckChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Controls.BitmapCheckBox bcb = sender as SEC.Nanoeye.Controls.BitmapCheckBox;

            switch (bcb.Name)
            {
                case "SenserX":
                    Properties.Settings.Default.MotorSenserX = bcb.Checked;
                    break;

                case "SenserY":
                    Properties.Settings.Default.MotorSenserY = bcb.Checked;
                    break;

                case "SenserT":
                    Properties.Settings.Default.MotorSenserT = bcb.Checked;
                    break;

                case "SenserZ":
                    Properties.Settings.Default.MotorSenserZ = bcb.Checked;
                    break;

                case "DirectionX":
                    Properties.Settings.Default.MotorXDirection = bcb.Checked;
                    break;

                case "DirectionY":
                    Properties.Settings.Default.MotorYDirection = bcb.Checked;
                    break;

                case "DirectionR":
                    Properties.Settings.Default.MotorRDirection = bcb.Checked;
                    break;

                case "DirectionT":
                    Properties.Settings.Default.MotorTDirection = bcb.Checked;
                    break;

                case "DirectionZ":
                    Properties.Settings.Default.MotorZDirection = bcb.Checked;
                    break;

                case "ZeroX":
                    MotorStage.StageMotorZero(1);
                    ZeroX.Checked = false;
                    break;

                case "ZeroY":
                    MotorStage.StageMotorZero(2);
                    ZeroY.Checked = false;
                    break;

                case "ZeroR":
                    MotorStage.StageMotorZero(3);
                    ZeroR.Checked = false;
                    break;

                case "ZeroT":
                    MotorStage.StageMotorZero(4);
                    ZeroT.Checked = false;
                    break;

                case "ZeroZ":
                    MotorStage.StageMotorZero(5);
                    ZeroZ.Checked = false;
                    break;

                default:
                    break;


            }


        }


        delegate void CalibrationDelegate();
        
        private void MSCalibration_CheckedChanged(object sender, EventArgs e)
        {
            

            MotorStage.CalibrationEnable = MSCalibrationBtn.Checked;
            if (MSCalibrationBtn.Checked)
            {

                CalibrationDelegate calDelay = new CalibrationDelegate(MotorStage.Calibration);
                IAsyncResult iftar = calDelay.BeginInvoke(null, null);
            }
            else
            {
                //MotorStage.
            }
            
        }



       

       

        


    }
}
