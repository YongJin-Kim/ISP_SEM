using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using SEC.Nanoeye.Controls;
using System.Globalization;
using System.Threading;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class Operation : Form
    {
        MiniSEM miniSEM;


        private Point _formLocation;
        public Point FormLocation
        {
            get { return _formLocation; }
            set
            {
                _formLocation = value;
                InitDisplay();
            }
        }

        private bool _LowVacEnable = false;
        public bool LowVacEnable
        {
            get { return _LowVacEnable; }
            set { _LowVacEnable = value; }
        }

        private string _profile = null;
        public string Profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
                InitDisplay();
            }
        }

        private bool _DetectorMode = false;
        public bool DetectorMode
        {
            get { return _DetectorMode; }
            set
            {
                _DetectorMode = value;
                InitDisplay();
            }
        }

        private bool _VacuumMode = false;
        public bool VacuumMode
        {
            get { return _VacuumMode; }
            set
            {
                _VacuumMode = value;
                InitDisplay();
            }
        }

        private string[] language = new string[3];
        public string[] Langualge
        {
            get { return language; }
            set
            {
                language = value;
                InitDisplay();
            }
        }

        private string _Settinglan = null;
        public string SettingLan
        {
            get { return _Settinglan; }
            set
            {
                _Settinglan = value;
                InitDisplay();
            }
        }

        private bool _StartupEnable = false;
        public bool StartupEnable
        {
            get { return _StartupEnable; }
            set
            {
                _StartupEnable = value;
                m_ToolStartup.Checked = _StartupEnable;
            }
        }

        private bool startEnable = false;
        public bool StartEnable
        {
            get { return startEnable; }
            set
            {
                startEnable = value;
                m_ToolStartup.Enabled = startEnable;

            }
        }

        private bool detectorEnable = true;
        public bool DetectorEnable
        {
            get { return detectorEnable; }
            set
            {
                detectorEnable = value;

                DetectorSE.Enabled = detectorEnable;
            }
        }

        private bool detectorBSEEnable = false;
        public bool DetectorBSEEnable
        {
            get { return detectorBSEEnable; }
            set
            {
                detectorBSEEnable = value;
                DetectorBSE.Enabled = detectorBSEEnable;
            }
        }

        private bool _calibrationEnable = false;
        public bool CalibrationEnable
        {
            get { return _calibrationEnable; }
            set
            {
                _calibrationEnable = value;
                

            }
        }

        private bool _SEEnable = true;
        public bool SEEnable
        {
            get { return _SEEnable; }
            set
            {
                _SEEnable = value;
                //InitDisplay();
            }
        }



        public Operation()
        {
            InitializeComponent();


            if (_DetectorMode)
            {
                DetectorSE.Checked = true;
                DetectorBSE.Checked = false;
            }
            else
            {
                DetectorSE.Checked = false;
                DetectorBSE.Checked = true;
            }

            if (_VacuumMode)
            {
                HighVac.Checked = true;
                LowVac.Checked = false;
            }
            else
            {
                HighVac.Checked = false;
                LowVac.Checked = true;
            }

            if (_StartupEnable)
            {
                m_ToolStartup.Checked = true;
            }


        }
        public Operation(MiniSEM minisem)
        {
            this.miniSEM = minisem;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();

           

           

        }

        public void InitDisplay()
        {
            if (m_ToolStartup.Checked)
            {
                ProfileBtn.Enabled = false;
                LanguageButton1.Enabled = false;
                LanguageButton2.Enabled = false;
                LanguageButton3.Enabled = false;
                LanguageButton4.Enabled = false;
                LanguageButton5.Enabled = false;
            }
            else
            {
                ProfileBtn.Enabled = true;
                LanguageButton1.Enabled = true;
                LanguageButton2.Enabled = true;
                LanguageButton3.Enabled = true;
                LanguageButton4.Enabled = true;
                LanguageButton5.Enabled = true;
            }

            //DetectorSE.Enabled = detectorEnable;

            if (miniSEM.DualDisplay)
            {
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = true;
                DetectorMerge.Checked = false;

                HighVac.Enabled = true;
                LowVac.Enabled = false;
            }
            else if(miniSEM.MergeDisplay)
            {
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = true;

                HighVac.Enabled = true;
                LowVac.Enabled = false;

            }
            else
            {
                if (_DetectorMode)
                {
                    DetectorSE.Checked = false;
                    DetectorBSE.Checked = true;

                    DetectorSE.Enabled = false;
                    DetectorDual.Enabled = false;
                    DetectorMerge.Enabled = false;
                    HighVac.Enabled = true;

                    if (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3000MB || SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SH3500MB)
                    {
                        DetectorSE.Enabled = false;
                        DetectorDual.Enabled = false;
                        DetectorMerge.Enabled = false;
                    }

                    //if(miniSEM.ultraLabel4
                    LowVac.Enabled = _LowVacEnable;
                }
                else
                {
                    DetectorSE.Checked = true;
                    DetectorBSE.Checked = false;
                    DetectorDual.Checked = false;
                    DetectorMerge.Checked = false;

                    DetectorDual.Enabled = DetectorBSE.Enabled;
                    DetectorMerge.Enabled = DetectorBSE.Enabled;

                    HighVac.Enabled = true;
                    LowVac.Enabled = false;
                }
            }

           
            

            if (_VacuumMode)
            {
                HighVac.Checked = false;
                LowVac.Checked = true;
            }
            else
            {
                HighVac.Checked = true;
                LowVac.Checked = false;
                if (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3000MB || SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SH3500MB)
                {
                    DetectorSE.Enabled = false;
                    DetectorDual.Enabled = false;
                    DetectorMerge.Checked = false;
                }
                else
                {
                    DetectorSE.Enabled = true;
                    DetectorDual.Enabled = DetectorBSE.Enabled;
                    DetectorMerge.Enabled = DetectorBSE.Enabled;
                }

                
            }


            if (_StartupEnable)
            {
                m_ToolStartup.Checked = true;
            }

            


            CalibrationSettingsBtn.Checked = _calibrationEnable;
           
            //DetectorSE.Enabled = SEEnable;

            m_ToolStartup.Enabled = startEnable;



        }
        //public void formSettingsProc()
        //{

        //}

        private void formLocationChange(object sender, EventArgs e)
        {
            this.Location = new Point(_formLocation.X, _formLocation.Y);

        }

        private string _EmissionStr = null;
        public string EmissionStr
        {
            get { return _EmissionStr; }
            set
            {
                _EmissionStr = value;
                EmissionDisplayLabelChange();
            }
        }

        public void EmissionDisplayLabelChange()
        {
            EmissionDisplayLabel.Text = _EmissionStr;

            //this.
        }

        public void ProfileChange(string[] str)
        {

            ProfileBtn.Items.Clear();

            foreach (string colist in str)
            {
                ProfileBtn.Items.Add(colist);
            }

            ProfileBtn.Text = _profile;

            //DetectorSE.Focus();

        }



        private void formCloseClick(object sender, EventArgs e)
        {
            //miniSEM.operationFormClose();
            formClose();

            //MiniSEM.ActiveForm.Activate();
            //this.de
            //miniSEM.OperChecked(false);

        }

        public void formClose()
        {
            this.Hide();
        }

        public void Close()
        {
            this.Close();
            this.Dispose();
        }

        private void profileChange(object sender, EventArgs e)
        {
            miniSEM.profileChange(sender, e);
        }

        private void VacuumStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            if (bcb == HighVac)
            {

                if (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3000MB)
                {
                    DetectorSE.Enabled = false;
                    DetectorDual.Enabled = false;

                }
                else
                {
                    DetectorSE.Enabled = true;
                    DetectorDual.Enabled = true;
                    DetectorMerge.Enabled = true;

                }

                HighVac.Checked = true;
                LowVac.Checked = false;
                
            }
            else
            {

                DetectorSE.Enabled = false;
                DetectorDual.Enabled = false;
                DetectorMerge.Enabled = false;

                HighVac.Checked = false;
                LowVac.Checked = true;
                

            }


            miniSEM.VacuumStateToolStripMenuItem_Click(sender, e);
        }

        private void LanguageChange(object sender, EventArgs e)
        {
            miniSEM.LanguageChange(sender, e);
        }

        private void OperationShown(object sender, EventArgs e)
        {



            switch (Properties.Settings.Default.Language)
            {
                case "ko-KR":

                    LanguageButton1.Checked = true;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;
                    break;

                case "zh-CN":
                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = true;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;
                    break;

                case "fr":
                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = true;
                    LanguageButton5.Checked = false;
                    break;

                case "ru-RU":
                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = true;
                    break;

                case "en-US":
                default:
                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = true;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;
                    break;
            }


        }

        private void ToolStartup_CheckedChangedEvent(object sender, EventArgs e)
        {
            if (m_ToolStartup.Checked)
            {
                m_ToolStartup.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_cancel_select;
                m_ToolStartup.Checked = true;
                ProfileBtn.Enabled = false;
                DetectorSE.Enabled = false;
                DetectorDual.Enabled = false;
                DetectorMerge.Enabled = false;

                DetectorBSE.Enabled = false;
                LanguageButton1.Enabled = false;
                LanguageButton2.Enabled = false;
                LanguageButton3.Enabled = false;
                LanguageButton4.Enabled = false;
                LanguageButton5.Enabled = false;
                miniSEM.startEnable(true);
                //miniSEM.ToolStartup_CheckedChangedEvent(sender, e);
            }
            else
            {
                m_ToolStartup.Checked = false;
                miniSEM.startEnable(false);
                StartUpProgressBar.Visible = false;
                ProfileBtn.Enabled = true;
                LanguageButton1.Enabled = true;
                LanguageButton2.Enabled = true;
                LanguageButton3.Enabled = true;
                LanguageButton4.Enabled = true;
                LanguageButton5.Enabled = true;
                miniSEM.ToolStartup_CheckedChangedEvent(sender, e);
            }



        }

        public void ToolStartupCheckedEnable(bool enable)
        {
            m_ToolStartup.Checked = enable;
            m_ToolStartup.Enabled = enable;
        }

        private void CalibrationSettingsBtn_Click(object sender, EventArgs e)
        {
            if (!CalibrationSettingsBtn.Checked)
            {
                //CalibrationSettingsBtn.Visible = false;
                miniSEM.CalibrationEnableChange(false);
            }
            else
            {
                miniSEM.CalibrationEnableChange(true);
            }
        }


        internal void Change(int p)
        {
            StartUpProgressBar.Visible = true;



            switch (p)
            {
                case 0:
                    m_ToolStartup.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_cancel_select;
                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_1;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_1;


                    break;

                case 1:

                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_2;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_2;
                    break;

                case 2:

                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_3;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_3;
                    break;

                case 3:

                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_4;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_4;
                    break;

                case 4:

                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_5;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_5;
                    break;

                case 5:
                    m_ToolStartup.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_top_select1;
                    StartUpProgressBar.FSurface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_6;
                    StartUpProgressBar.Surface = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_6;

                    DetectorSE.Enabled = true;
                    DetectorBSE.Enabled = true;
                    break;

                default:
                    StartUpProgressBar.Visible = false;
                    break;
            }
        }

        private void m_MainMenuStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            miniSEM.m_MainMenuStrip_MouseDoubleClick(sender, e);
        }

        public void m_ToolsStartupEnable(bool enable)
        {
            m_ToolStartup.Enabled = enable;
        }

        private void DetectorChange(object sender, EventArgs e)
        {
            BitmapCheckBox btn = sender as BitmapCheckBox;

            if (btn.Name == "DetectorSE")
            {
                if (!btn.Checked)
                {
                    DetectorSE.Checked = true;
                    return;
                }

                miniSEM.SEChecked = btn.Checked;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = false;
                LowVac.Enabled = false;

            }

            if (btn.Name == "DetectorBSE")
            {
                if (!btn.Checked)
                {
                    DetectorBSE.Checked = true;
                    return;
                }

                miniSEM.BSEChecked = btn.Checked;
                DetectorSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = false;
                LowVac.Enabled = Properties.Settings.Default.VacuumLow;
            }

            if (btn.Name == "DetectorDual")
            {
                if (!btn.Checked)
                {
                    DetectorDual.Checked = true;
                    return;
                }

                miniSEM.DualChecked = btn.Checked;
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorMerge.Checked = false;
                LowVac.Enabled = false;
            }

            if (btn.Name == "DetectorMerge")
            {
                if (!btn.Checked)
                {
                    DetectorMerge.Checked = true;
                    return;
                }

                miniSEM.MergeChecked = btn.Checked;
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;
                LowVac.Enabled = false;
            }


        }

        private void Operation_Load(object sender, EventArgs e)
        {

        }

        private void DetectorDual_CheckedChanged(object sender, EventArgs e)
        {

        }

       
    }
}
