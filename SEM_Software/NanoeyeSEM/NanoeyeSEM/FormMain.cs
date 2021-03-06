using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using SEC.Nanoeye.Controls;
using SEC.Nanoeye.NanoeyeSEM.Settings;
using SECcolumn = SEC.Nanoeye.NanoColumn;
using SECimage = SEC.Nanoeye.NanoImage;
using SECtype = SEC.GenericSupport.DataType;
using Infragistics.Win.UltraWinEditors;
using SEC.Nanoeye.NanoeyeSEM.Settings.SNE4000M;
using SEC.Nanoeye.Support.Controls;
using OpenCvSharp;
using System.Threading;
using System.Resources;
using Windows7.Multitouch;
using Windows7.Multitouch.WinForms;



namespace SEC.Nanoeye.NanoeyeSEM
{
    public partial class MiniSEM : Form
    {
        #region Variablese
        private SEC.Nanoeye.NanoImage.IActiveScan scanner = null;
        private SEC.Nanoeye.NanoColumn.ISEMController column = null;
        private Settings.ISettingManager setManager = null;
        private Template.ITemplate equip = null;
        private Manager4000M manager = new Manager4000M();

        private StatusServerViewer statuServer;

        SEC.Nanoeye.Support.Dialog.LensWobbleForm lwf = new SEC.Nanoeye.Support.Dialog.LensWobbleForm();


        CvCapture capture;
        System.Windows.Forms.Timer systemCheckTimer;
        DateTime communicationErrorLastTime;
        int communicationErrorCount = 0;
        bool communicationErrorOccured = false;
        bool MagBtnEnable = false;

        bool FilamentCheckedStartEnable = false;

        const int Frequency = 6;

        //private int SamplingTimes = 1;
        //System.Windows.Forms.

        SEC.GenericSupport.SingleForm sfOption = new SEC.GenericSupport.SingleForm(typeof(FormConfig.FormConfigOption));
        //SEC.GenericSupport.SingleForm sfScreenRecoder = new SEC.GenericSupport.SingleForm(typeof(ScreenRecorder));
        //SEC.GenericSupport.SingleForm sfMicronModify = new SEC.GenericSupport.SingleForm(typeof(InfoZone));
        SEC.GenericSupport.SingleForm sfScanner = new SEC.GenericSupport.SingleForm(typeof(FormConfig.Scanner));
        //SEC.GenericSupport.SingleForm sfArchivesCB = new SEC.GenericSupport.SingleForm(typeof(FormConfig.ArchivesTabCB));

        SEC.Nanoeye.NanoeyeSEM.FormConfig.ArchivesTabCB sfArchivesCB = null;

        SEC.GenericSupport.SingleForm sfMicroscope;
        //SEC.GenericSupport.SingleForm sfBeamshiftWindow = new SEC.GenericSupport.SingleForm(typeof(BeamShiftCrossPoint));
        
        //SEC.GenericSupport.SingleForm sfMTools = new SEC.GenericSupport.SingleForm(typeof(FormConfig.Measurement_Tools));
        //SEC.Nanoeye.NanoeyeSEM.FormConfig.Measurement_Tools sfMTools = new SEC.Nanoeye.NanoeyeSEM.FormConfig.Measurement_Tools();
        SEC.Nanoeye.NanoeyeSEM.FormConfig.Measurement_Tools sfMTools = null;
        //SEC.Nanoeye.NanoeyeSEM.FormConfig.Operation operation = new SEC.Nanoeye.NanoeyeSEM.FormConfig.Operation();
        SEC.Nanoeye.NanoeyeSEM.FormConfig.Operation operation = null;
        SEC.Nanoeye.NanoeyeSEM.InfoZone sfMicronModify = null;
        //SEC.Nanoeye.NanoeyeSEM.ScreenRecorder sfScreenRecoder = null;
        SEC.Nanoeye.NanoeyeSEM.BeamShiftCrossPoint sfBeamshiftWindow = null;

        SEC.Nanoeye.NanoeyeSEM.FormConfig.DaulDetector DualDetector;




        //SEC.Nanoeye.NanoeyeSEM.FormConfig.SamplingTime samplingtime = new SEC.Nanoeye.NanoeyeSEM.FormConfig.SamplingTime();
        SEC.Nanoeye.NanoeyeSEM.FormConfig.SamplingTime samplingtime = null;
        
        private Size FormSizeGap = new Size(0, 0);

        private TouchHandler _touchHandler;

        System.Threading.ManualResetEvent mreRunningTime = new System.Threading.ManualResetEvent(false);

        SEC.Nanoeye.NanoeyeSEM.FormConfig.MotorStage MotorStage = new SEC.Nanoeye.NanoeyeSEM.FormConfig.MotorStage();
        SEC.Nanoeye.NanoeyeSEM.FormConfig.MotorSpeedSetting MotorSettings;
        #endregion


        //private SEC.Nanoeye.Support.Controls.PaintPanel ppSingle;
        public SEC.Nanoeye.Support.Controls.PaintPanel PPSingle
        {
            get { return ppSingle; }
            set { ppSingle = value; }
        }


        #region 생성자 & 초기화 & 소멸자
        public MiniSEM()
            : this(AppDeviceEnum.AutoDetect, AppSellerEnum.AutoDetect, AppModeEnum.Run)
        {
        }

        public MiniSEM(AppDeviceEnum device, AppSellerEnum seller, AppModeEnum mode)
        {
            this.Opacity = 0;
            //Initialize.Splash.Default = new Kikwak.ProjectSEM.Initialize.Splash();
            //Initialize.Splash.Default.Show(this);
            //Initialize.Splash.Default.UpdateInfo(device, seller, mode);


            communicationErrorLastTime = DateTime.Now;

            SystemInfoBinder.Default.AppDevice = device;
            SystemInfoBinder.Default.AppSeller = seller;
            SystemInfoBinder.Default.AppMode = mode;

            int cultureCode = 0;


            Trace.WriteLine("Properties.Settings.Default.Language : " + Properties.Settings.Default.Language.ToString());
                            

           
            //LanguageResources.MultiLanguage.Culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Language);
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);
            

            InitializeComponent();


            this.Load += new EventHandler(frMainMagWheel);
           


            //m_MenuConfigScanning.Visible = m_MenuConfigMicroscope.Visible = (mode != AppModeEnum.Run);
            m_ToolStartup.Enabled = (mode == AppModeEnum.Debug);

            // Disable Ctrl+Tab in ultratabw
            InfraDisableUltratabKeymapping(frameMain.KeyActionMappings);
            //InfraDisableUltratabKeymapping(frontPanelUtc.KeyActionMappings);


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


            ppSingle.MTools.ItemListChanged += new EventHandler(MTools_ItemChanged);
            ppSingle.MTools.SelectedItemChanged += new EventHandler(MTools_SelectedItemChanged);

            SystemInfoBinder.Default.MainForm = this;
            SystemInfoBinder.Default.PropertyChanged += new PropertyChangedEventHandler(SystemInfoBinder_PropertyChanged);
            UIsetBinder.Default.PropertyChanged += new PropertyChangedEventHandler(UIsetBinder_PropertyChanged);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            #region UI Tag 입력
            frMainScanPauseBb.Tag = ScanModeEnum.ScanPause;
            frMainScanPfBb.Tag = ScanModeEnum.FastPhoto;
            frMainScanPsBb.Tag = ScanModeEnum.SlowPhoto;
            frMainScanPsBb2.Tag = ScanModeEnum.SlowPhoto2;
            frMainScanSfBb.Tag = ScanModeEnum.FastScan;
            frMainScanSsBb.Tag = ScanModeEnum.SlowScan;
            #endregion

            #region 가속전압 Tag
            //btmProfile.Tag = Profile.profile1;
            //btmProfile2.Tag = Profile.profile2;
            //btmProfile3.Tag = Profile.profile3;
            //btmProfile4.Tag = Profile.profile4;
            //btmProfile5.Tag = Profile.profile5;
            //btmProfile6.Tag = Profile.profile6;
            //btmProfile7.Tag = Profile.profile7;
            #endregion


            #region Data Bindings
            ppSingle.DataBindings.Add(new Binding("OverlayDraw", toolCrosshair, "Checked"));
            //toolCrosshair.DataBindings.Add(new Binding("Checked", tsmnuCrossHair, "Checked", false, DataSourceUpdateMode.OnPropertyChanged));
            //toolMeasuring.DataBindings.Add(new Binding("Checked", m_MenuMeasureTools, "Checked", false, DataSourceUpdateMode.OnPropertyChanged));
            //toolAlign.DataBindings.Add(new Binding("Checked", tsmnuBeamAligns, "Checked", false, DataSourceUpdateMode.OnPropertyChanged));

            //videoAuto.DataBindings.Add("Enabled", m_ToolStartup, "Checked");
            //frMainFocusAutoBb.DataBindings.Add("Enabled", m_ToolStartup, "Checked");
            #endregion

            MakePainterOverlay();

        }

        private void MakePainterOverlay()
        {
            ppSingle.Dot = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.info_dot;
            //ppSingle.OverlayImage = SEC.GUIelement.ImagePanel.BasicOverlay(SEC.GUIelement.ImagePanel.BasicOverlayType.CrossWithCircle, ppSingle.ImageSize);
            ppSingle.OverlayImage = SEC.GUIelement.ImagePanel.BasicOverlay(SEC.GUIelement.ImagePanel.BasicOverlayType.Cross, ppSingle.ImageSize);
            ppSingle.Company = SystemInfoBinder.Default.AppSeller.ToString();
            ppSingle.ScaleBar = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.info_graph3;


        }

        private void InfraDisableUltratabKeymapping(Infragistics.Win.UltraWinTabControl.UltraTabControlKeyActionMappingsCollection utkamc)
        {
            System.Collections.Generic.List<Infragistics.Win.KeyActionMappingBase> mappingsToRemove = new List<Infragistics.Win.KeyActionMappingBase>();

            foreach (Infragistics.Win.KeyActionMappingBase mapping in utkamc)
            {
                if (mapping.KeyCode == Keys.Tab)
                {
                    //Infragistics.Win.UltraWinTabControl.UltraTabControlAction.n
                    if ((mapping.SpecialKeysRequired == Infragistics.Win.SpecialKeys.Ctrl) ||
                        (mapping.SpecialKeysRequired == Infragistics.Win.SpecialKeys.ShiftCtrl))
                    {
                        mappingsToRemove.Add(mapping);
                    }
                }
            }

            foreach (Infragistics.Win.KeyActionMappingBase mapping in mappingsToRemove)
            {
                utkamc.Remove(mapping);
            }
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(e.Exception);
        }

        /// <summary>
        /// 주 프로그램 초기화 이벤트 입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                Initialize.Initializer.Initialize(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Initialize.Splash.Default, ex.Message + "\r\n" + TextManager.Instance.GetString("Message_ProgramClose"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Initialize.Splash.Default.Dispose();
                Initialize.Splash.Default = null;

                if (SystemInfoBinder.Default.AppMode == AppModeEnum.Run)
                {
                    SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                    this.Dispose();
                    Application.ExitThread();
                    Application.Exit();
                    return;
                }
                else
                {
                    throw new Exception("초기화 실패", ex);
                }
            }

            Initialize.Splash.Default.Dispose();

            systemCheckTimer = new System.Windows.Forms.Timer();
            systemCheckTimer.Interval = 1000;
            systemCheckTimer.Tick += new EventHandler(SystemCheckTimer_Tick);
            systemCheckTimer.Start();

            //runTime = new FilamentRunningTime();
            //runTime.FilamentTime += new EventHandler(FilamentRunningTimeLabel);
        }

        //FilamentRunningTime runTime;

        private void MiniSEMClose(object sender, EventArgs e)
        {
            
            if (capture != null)
            {
                capture.Dispose();
                //capture.b
            }

            
            Finalizer();

            
            this.Dispose();
            this.Close();
            
            
        }

        public void mainReset()
        {

            //this.Close();
            Finalizer();
            this.Dispose();
            Application.Restart();
        }


        /// <summary>
        /// Dispose 함수에서 호출 됨.
        /// </summary>
        private void Finalizer()
        { 
            if (m_ToolStartup.Checked)
            {
                //Trace.WriteLine("m_ToolStartup Stop---" + DateTime.Now.Second);
                m_ToolStartup.Checked = false;
            }

            if (SpotModeBtn.Checked)
            {
                //Trace.WriteLine("Spot Mode Stop---" + DateTime.Now.Second);
                SpotModeBtn.Checked = false;
                spotForm.Dispose();
                SpotFormClose();
            }

            if (sfMicroscope != null)
            {
                sfMicroscope.Close();
            }

            if (MotorStage != null)
            {
                MotorStage.MotorClose();
            }

            if (Properties.Settings.Default.Camera)
            {

                StopCamera();

            }

            setManager.StageSave("stage1");

            if (StatusServer.Default.ServerEnable) { StatusServer.Default.ServerOff(); }

           
            Properties.Settings.Default.Save();

            StatusServer.Default.Dispose();
            StatusServer.Default.ScanStealed -= new EventHandler(statusServer_ScanStealed);
            StatusServer.Default.ScanFree -= new EventHandler(statusServer_ScanFree);

            if (SystemInfoBinder.Default.SetManager != null)
            {
              
                SystemInfoBinder.Default.SetManager.ColumnSave(column, ColumnOnevalueMode.Run);
                SystemInfoBinder.Default.SetManager.Save(SystemInfoBinder.Default.SettingFileName);

                if (column != null)
                {
                    ((SECtype.IControlBool)column["HvEnable"]).Value = false;

                    if (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE4500M && SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000MB && SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3200M)
                    {
                        ((SECtype.IControlInt)column["VacuumMode"]).Value = 0;

                    }
                    Delay(1000);
                    
                    //column.Dispose();
                    column = null;
                }

                SystemInfoBinder.Default.Nanoeye.Scanner.Dispose();
                ppSingle.Dispose();
                SystemInfoBinder.Default.SetManager = null;

               
                
            }
        }

        // Initializer에 의해 호출된다.
        internal void InitializedDevice()
        {



            //SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE4000M;
            switch (SystemInfoBinder.Default.AppDevice)
            {
                case AppDeviceEnum.SNE1500M:
                    sfMicroscope = new SEC.GenericSupport.SingleForm(typeof(FormConfig.FormConfigMicroscope));
                    SystemInfoBinder.Default.Equip = new Template.MiniSEM();
                    break;
                case AppDeviceEnum.SNE3000M:
                case AppDeviceEnum.SNE4000M:
                case AppDeviceEnum.SNE4500M:
                case AppDeviceEnum.SNE3200M:
                case AppDeviceEnum.SNE3000MS:
                case AppDeviceEnum.SNE3000MB:
                case AppDeviceEnum.SH4000M:
                case AppDeviceEnum.SH3500MB:
                case AppDeviceEnum.SH5000M:
                    sfMicroscope = new SEC.GenericSupport.SingleForm(typeof(FormConfig.Microscope));
                    SystemInfoBinder.Default.Equip = new Template.SNE4000M();
                    //SystemInfoBinder.Default.Equip = new Template.MiniSEM();
                    break;
                case AppDeviceEnum.SNE4500P:
                    sfMicroscope = new SEC.GenericSupport.SingleForm(typeof(FormConfig.Microscope));
                    SystemInfoBinder.Default.Equip = new Template.SNE4500P();
                    break;
                default:
                    throw new NotSupportedException();
            }                 

            column = SystemInfoBinder.Default.Nanoeye.Controller;
            scanner = SystemInfoBinder.Default.Nanoeye.Scanner;
            setManager = SystemInfoBinder.Default.SetManager;
            equip = SystemInfoBinder.Default.Equip;


            ColumnLink();

            ColumnListChange();

            (equip.ColumnHVGun as SECcolumn.IColumnValue).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(HvElectronGun_RepeatUpdated);
            (equip.ColumnVacuumState as SECcolumn.IColumnValue).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(VacuumState_RepeatUpdated);

            switch (SystemInfoBinder.Default.AppDevice)
            {
                case AppDeviceEnum.SNE1500M:
                    (equip.ColumnVacuumMode as SECcolumn.IColumnValue).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(VacuumMode_RepeatUpdated);
                    break;
                case AppDeviceEnum.SNE3000M:
                case AppDeviceEnum.SNE4000M:
                case AppDeviceEnum.SNE4500M:
                case AppDeviceEnum.SNE4500P:
                case AppDeviceEnum.SNE3200M:
                case AppDeviceEnum.SNE3000MS:
                case AppDeviceEnum.SNE3000MB:
                case AppDeviceEnum.SH4000M:
                case AppDeviceEnum.SH3500MB:
                case AppDeviceEnum.SH5000M:
                    break;
                case AppDeviceEnum.AutoDetect:
                default:
                    throw new NotSupportedException("장치가 정의 되지 않음");
            }



            column.CommunicationErrorOccured += new EventHandler<SECtype.CommunicationErrorOccuredEventArgs>(m_NanoView_CommunicationErrorOccured);

        }

        //private void ColumnListChange()
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Initializer에 의해 호출 된다.
        /// UI를 정의 한다.
        /// </summary>
        internal void InitiDisplay()
        {
            // 배경 정의
            switch (SystemInfoBinder.Default.AppSeller)
            {
                case AppSellerEnum.SEC:
                case AppSellerEnum.Evex:
                case AppSellerEnum.Hirox:
                    switch (SystemInfoBinder.Default.AppDevice)
                    {
                        case AppDeviceEnum.SNE1500M:
                       
                            this.BackgroundImage = Properties.Resources.MainFrame;
                            //this.Controls.Remove(DeviceNameUl);
                            break;
                        case AppDeviceEnum.SNE3000M:
                        case AppDeviceEnum.SNE4000M:
                            //this.BackgroundImage = Properties.Resources.MainFrame_BackColor;
                            break;
                    }
                    break;
                case AppSellerEnum.Nikkiso:
                    this.BackgroundImage = Properties.Resources.MainFrame_SEMTRAC;
                    //this.Controls.Remove(DeviceNameUl);
                    break;
            }

            #region 언어 설정
            //ToolStripMenuItem firstTsmi = null;
            //ToolStripMenuItem checkedTsmi = null;

            //string[] lans = TextManager.Instance.LanguageList;


            //switch (Properties.Settings.Default.Language)
            //{
            //    case "Korean":
            //    case "한국어":
            //        Properties.Settings.Default.Language = "한국어";
            //        break;

            //    case "Japanese":
            //    case "日本語":
            //        Properties.Settings.Default.Language = "日本語";
            //        break;

            //    case "English":
            //    default:
            //        Properties.Settings.Default.Language = "English";
            //        break;
            //}

           

            //if (lans.Length > 0)
            //{
            //    foreach (string str in lans)
            //    {
            //        ToolStripMenuItem tsmi = new ToolStripMenuItem(str);
            //        languageToolStripMenuItem.DropDownItems.Add(tsmi);


            //        tsmi.Click += new EventHandler(Language_Click);
            //        if (firstTsmi == null) { firstTsmi = tsmi; }

            //        Trace.WriteLine("language : " + Properties.Settings.Default.Language.ToString());

            //        if (Properties.Settings.Default.Language == str)
            //        {
            //            tsmi.Checked = true;

            //            checkedTsmi = tsmi;
            //        }
            //    }
            

            //    LanguageButton1.Text = "Korean";
            //    LanguageButton2.Text = "English";
            //    LanguageButton3.Text = "Japanese";



            //    if (checkedTsmi == null)
            //    {
            //        checkedTsmi = firstTsmi;
            //        checkedTsmi.Checked = true;
            //    }

            //    TextManager.Instance.Language = checkedTsmi.Text;

            //    TextManager.Instance.DefineText(this);

            //    //switch(TextManager.Instance.Language


            //}
            //// 버튼 글자 다시 불러 오기.
            ////frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[1];
            ////frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[0];
            #endregion




            // 배경에 따른 글자 색 정의 및 사용 하지 않는 control 제거
            // Ultra lable은 바인딩이 되지 않아 여기서  함.
            Infragistics.Win.Appearance appeaLab = new Infragistics.Win.Appearance();
            appeaLab.TextHAlign = Infragistics.Win.HAlign.Center;
            appeaLab.TextVAlign = Infragistics.Win.VAlign.Middle;
            appeaLab.BackColor = Color.SlateGray;

            //rfStigAlignXLab.Appearance = appeaLab;
            //rfStigAlignYLab.Appearance = appeaLab;
            //rfGunAlignUl.Appearance = appeaLab;
            //rfGunSettingLab.Appearance = appeaLab;

            Infragistics.Win.Appearance appeaControl = new Infragistics.Win.Appearance();
            appeaControl.TextHAlign = Infragistics.Win.HAlign.Left;
            appeaControl.TextVAlign = Infragistics.Win.VAlign.Middle;
            appeaControl.BackColor = Color.SlateGray;

            //rfStigAlignXControl.Appearance = appeaControl;
            //rfStigAlignYControl.Appearance = appeaControl;
            //rfGunSettingControl.Appearance = appeaControl;
            //rfGunControlUl.Appearance = appeaControl;

            fpList = new List<Infragistics.Win.UltraWinTabControl.UltraTab>();
            switch (SystemInfoBinder.Default.AppDevice)
            {
                case AppDeviceEnum.SNE1500M:
                    UIsetBinder.Default.LabelForeColor = Color.DarkRed;
                    UIsetBinder.Default.ButtonForeColor = Color.DarkRed; 
                    appeaLab.ForeColor = Color.DarkRed;
                    appeaControl.ForeColor = Color.DarkRed;

                    break;
                case AppDeviceEnum.SNE3000M:
                    ModelLab.Text = "SNE-3000M";

                   

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = Properties.Settings.Default.VacuumLow;


                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    focusmodeDl.SelectedIndex = 0;
                    frMainFocusWobbleCbewicb.Visible = false;

                    Properties.Settings.Default.Camera = Properties.Settings.Default.Camera;


                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }

                    scanner.Revers(false);
                    break;

                case AppDeviceEnum.SNE4000M:

                    ModelLab.Text = "SNE-4000M";

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;


                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                        frMainFocusWobbleCbewicb_S.Visible = true;
                        WDLabel.Visible = true;


                        MSTCoarseText.Text = "W/D";


                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }

                    break;

                case AppDeviceEnum.SNE4500M:
                    ModelLab.Text = "SS-150";

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;


                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                        frMainFocusWobbleCbewicb_S.Visible = true;
                        WDLabel.Visible = true;

                        MSTCoarseText.Text = "W/D";

                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }
                    frMainFocusAWBb.Text = "AW";

                    break;

                case AppDeviceEnum.SNE4500P:
                    ModelLab.Text = "SNE-4500P";

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;


                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                        frMainFocusWobbleCbewicb_S.Visible = true;
                        WDLabel.Visible = true;

                        MSTCoarseText.Text = "W/D";

                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }
                    break;

                case AppDeviceEnum.SNE3000MB:
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED = false;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED = true;
                    //SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow = true;


                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;

                    DetectorSE.Checked = false;
                    DetectorBSE.Checked = true;
                    DetectorDual.Enabled = false;
                    
                    SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.BSED;

                    ModelLab.Text = "SNE-3000MB";
                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    scanner.ScanMode(true);
                    focusmodeDl.SelectedIndex = 0;


                    BSEControl.SelectedTab = BSEControl.Tabs["BSEControl"];

                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }

                    frMainFocusAWBb.Text = "AC";


                    break;
                case AppDeviceEnum.SNE3200M:
                    ModelLab.Text = "SS-60";

                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED = true;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED = true;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow = true;

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = false;


                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    focusmodeDl.SelectedIndex = 0;
                    frMainFocusWobbleCbewicb.Visible = false;
                    Properties.Settings.Default.Camera = true;
                    

                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }
                    
                    scanner.Revers(false);
                    frMainFocusAWBb.Text = "AC";

                    break;
                case AppDeviceEnum.SNE3000MS:
                    ModelLab.Text = "SNE-3000MS";

                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED = true;
                    //SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED = true;
                    //SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow = true;

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = false;


                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    focusmodeDl.SelectedIndex = 0;
                    frMainFocusWobbleCbewicb.Visible = false;
                    Properties.Settings.Default.Camera = false;


                    if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["BarType"];
                    }
                    else
                    {
                        FocusTypeTab.SelectedTab = FocusTypeTab.Tabs["OriType"];
                    }

                    scanner.Revers(false);
                    frMainFocusAWBb.Text = "AC";

                    break;

                case AppDeviceEnum.SH3500MB:


                    ModelLab.Text = "SH-3500M";

                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED = false;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED = true;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow = true;


                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;

                    DetectorSE.Checked = false;
                    DetectorBSE.Checked = true;
                    frMainFocusWobbleCbewicb.Visible = false;

                    SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.BSED;
                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    scanner.ScanMode(true);
                    focusmodeDl.SelectedIndex = 0;


                    BSEControl.SelectedTab = BSEControl.Tabs["BSEControl"];



                    break;

                case AppDeviceEnum.SH4000M:
                    ModelLab.Text = "SH-4000M";

                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED = true;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED = true;
                    SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow = true;

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = Properties.Settings.Default.DetectorBSED;


                    frMainFocusWdLswis.Visible = false;
                    focusmodeDl.Visible = true;
                    focusmodeDl.SelectedIndex = 0;
                    //CamarBtn.Visible = true;
                    frMainFocusWobbleCbewicb.Visible = false;
                    Properties.Settings.Default.Camera = true;

                    //SpotModeBtn.Visible = false;

                    scanner.Revers(true);
                    break;

                case AppDeviceEnum.SH5000M:
                    ModelLab.Text = "SH-5000M";

                    DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                    DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;

                    LowVac.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;

                    //SpotModeBtn.Visible = false;
                    break;


                default:
                    Trace.WriteLine("Invalid Device in InitializeOptionUI.", "Error");
                    throw new NotSupportedException();
            }



            // Binding에 사용되는 데이터 정의
            sicBs.DataSource = SystemInfoBinder.Default;
            uIsetBinderBindingSource.DataSource = UIsetBinder.Default;



            // 카메라는 항상 표시 하지 않음.
            //tsmiCamera.Visible = false;e



            // 프로그램 시작 시 ScanPause 상태로 진입 하게 함.
            frMainScanPauseBb.Checked = true;



            // 진공 모드 및 detector 모드를 읽어서 표시 함.
            ChangeDetectAndVacuumStateDisplay();



            // Status server랑 연동.
            StatusServer.Default.ListenerpPort = (int)Properties.Settings.Default.StatusServerPort;
            StatusServer.Default.GetDeviceValue = new StatusServer.GetDeviceValueDelegate(SendStatusServer);
            StatusServer.Default.ScanStealed += new EventHandler(statusServer_ScanStealed);
            StatusServer.Default.ScanFree += new EventHandler(statusServer_ScanFree);
            
            if (Properties.Settings.Default.AutoConnectStatusServer) { StatusServer.Default.ServerOn(); }



            // 기타 UI 상태 정의
            SystemInfoBinder.Default.ImageExportable = false;
            toolMeasuring.Enabled = true;

            //string exportMode = toolFilePrint.Tag as string;
            //if (exportMode == "File") { toolFilePrint.Image = Properties.Resources.HardDrive; }
            //else { toolFilePrint.Image = Properties.Resources.Printer; }


            // Wizard 읽어 오기 wizardToolStripMenuItem
            LoadWizard();

            //m_MenuConfigMicroscope.Visible = false;
            //m_MenuConfigScanning.Visible = false;

            StartUpProgressBar.Visible = false;

            //HighVac.Enabled = false;
            //LowVac.Enabled = false;

            //Size WorkingArea = Screen.PrimaryScreen.WorkingArea.Size;
            FormSizeGap = this.Size;

            //this.Size = WorkingArea;
            Infomation();

            if (Properties.Settings.Default.Camera)
            {
                CamarBtn.Visible = true;
                StartCameras();
            }
            else
            {
                CamarBtn.Visible = false;
            }

            //Size Bounds = new Size(1366, 738);
            //this.Size = Bounds;


            //this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Size.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Size.Height / 2);
           

            //FormSizeChange();

            //this.MainFormMaxBox.Checked = true;

           
            ImageTab.Visible = false;

            this.Opacity = 1;

            //ProfileBtn.Appearance.ImageBackground = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_enable;

            initArchivesAddress();
            
            statusServerChange();

            ControlDefaultSetting();
            
           


            m4000FocusWF.Value = 6;

            AutoWobble();

            if (WobbleCheckedBox.Checked)
            {
                m4000FocusWF.Enabled = false;
                m4000FocusWA.Enabled = false;
            }

            ImageTabBtn.Checked = true;

            SpotModeBtn.Visible = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.SpotMode;
            if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.SpotMode)
            {
                SpotModeBtn.Enabled = false;
            }

            
            if (Properties.Settings.Default.MotorStageEnable)
            {
                StageSettingsLoad();

                MotorStage.StageLoad();
                //MotorStage.Show();

                if (MotorStage.portsArray != null)
                {
                    foreach (string portnumber in MotorStage.portsArray)
                    {
                        comboBox1.Items.Add(portnumber);
                    }
                }

                

                //MotorStage.Show();

                MotorStage.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(MotorValue_Change);
                
            }

            sfMTools = new SEC.Nanoeye.NanoeyeSEM.FormConfig.Measurement_Tools(this);

            if (Properties.Settings.Default.MicronEnable == false)
            {
                Properties.Settings.Default.MicronEnable = true;
            }

            if (Properties.Settings.Default.NewMag)
            {
                //ppSingle.ImageWidthAt_x1 = 0.32;          
                ppSingle.ImageWidthAt_x1 = 0.168;          
            }
            else if (Properties.Settings.Default.FEIMag)
            {
                ppSingle.ImageWidthAt_x1 = 0.32;
            }
            else
            {
                ppSingle.ImageWidthAt_x1 = 0.128;
            }

            if (Properties.Settings.Default.DetectorSED == true && Properties.Settings.Default.DetectorBSED == true)
            {
                DetectorDual.Enabled = true;
                DetectorMerge.Enabled = true;
            }
            else
            {
                DetectorDual.Enabled = false;
                DetectorMerge.Enabled = false;
            }



            DetectorComboBtn.SelectedIndex = 0;
            VacuumComboBtn.SelectedIndex = 0;

            ProfileBtn.Appearance.ImageBackground = Properties.Resources.icon02_01;
            DetectorComboBtn.Appearance.ImageBackground = Properties.Resources.icon02_01;
            VacuumComboBtn.Appearance.ImageBackground = Properties.Resources.icon02_01;



            FunctionChange(FCTSpotSizeBtn, EventArgs.Empty);

            EmissionDisplayLabel.Focus();


            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            loadImage = Cv.LoadImage(strPath + "CameraBackground.jpg");

            string result = null;

            result += Properties.Settings.Default.FilamentRunningTime.Days.ToString() + "days ";
            result += Properties.Settings.Default.FilamentRunningTime.Hours.ToString().PadLeft(2, '0') + ":";
            result += Properties.Settings.Default.FilamentRunningTime.Minutes.ToString().PadLeft(2, '0');
            FilamentRunTimeLabel.Text = result;

            MotorStageSpeedChange();

        }

        private void StageSettingsLoad()
        {
            setManager.StageLoad("stage1");
        }



        private bool MotorXTextEnable = false;
        private bool MotorYTextEnable = false;
        private bool MotorRTextEnable = false;
        private bool MotorTTextEnable = false;
        private bool MotorZTextEnable = false;
        private void MotorValue_Change(object sender, EventArgs e)
        {
            
            //MotorXtxt.Text = MotorStage.MotorXvalue[1].ToString();

            if (!MotorXTextEnable)
            {
                setMotorvalue(MotorXtxt, MotorStage.MotorXvalue[1].ToString());
                if (MotorStage.MotorXvalue[3] == "L" )
                {
                    MotorXtxt.BackColor = Color.Yellow;
                }
                else
                {
                    MotorXtxt.BackColor = Color.FromArgb(54, 70, 83);
                }

            }

            if (!MotorYTextEnable)
            {
                setMotorvalue(MotorYtxt, MotorStage.MotorYvalue[1].ToString());
                if (MotorStage.MotorYvalue[3] == "L")
                {
                    MotorYtxt.BackColor = Color.Yellow;
                }
                else
                {
                    MotorYtxt.BackColor = Color.FromArgb(54, 70, 83);
                }
            }

            if (!MotorRTextEnable)
            {
                setMotorvalue(MotorRtxt, MotorStage.MotorRvalue[1].ToString());
            }

            if (!MotorTTextEnable)
            {
                setMotorvalue(MotorTtxt, MotorStage.MotorTvalue[1].ToString());
                if (MotorStage.MotorTvalue[3] == "L")
                {
                    MotorTtxt.BackColor = Color.Yellow;
                }
                else
                {
                    MotorTtxt.BackColor = Color.FromArgb(54, 70, 83);
                }
            }


            if (!MotorZTextEnable)
            {
                setMotorvalue(MotorZtxt, MotorStage.MotorZvalue[1].ToString());

                if (MotorStage.MotorZvalue[3] == "L")
                {
                    MotorZtxt.BackColor = Color.Yellow;
                }
                else
                {
                    MotorZtxt.BackColor = Color.FromArgb(54, 70, 83);
                }
            }


            picturePoint();
            
        }


        //private IplImage imgSrc;
        private IplImage loadImage;
        private void picturePoint()
        {

            //imgSrc = new IplImage();

            imgSrc = new IplImage(400, 300, BitDepth.U8, 3);
            //imgSrc.back


            //imgSrc = Cv.LoadImage(Properties.Resources.icon05_01_01);
            imgSrc = Cv.CloneImage(loadImage);
            //imgSrc.image

            //imgSrc = loadImage;
            double x = Convert.ToDouble(MotorStage.MotorXvalue[1]);
            double y = Convert.ToDouble(MotorStage.MotorYvalue[1]);

            x = pictureBoxIpl1.Size.Width / 2 + (x * (200 / 20));
            y = pictureBoxIpl1.Size.Height / 2 - (y * (150 / 20));


            CvPoint centerPoint = new CvPoint((int)x, (int)y);



            //imgSrc.DrawMarker(centerPoint.X, centerPoint.Y, Cv.RGB(50, 235, 251), MarkerStyle.Cross, 250, LineType.AntiAlias, 2);
            //imgSrc.DrawCircle(new CvPoint(pictureBoxIpl1.Size.Width /2, pictureBoxIpl1.Size.Height /2)  , 5, Cv.RGB(255, 0, 0));

            imgSrc.DrawCircle(centerPoint, 30, Cv.RGB(148, 192, 57), 3);
            imgSrc.DrawCircle(centerPoint, 40, Cv.RGB(148, 192, 57), 1);

            //imgSrc.DrawCircle(new CvPoint(pictureBoxIpl1.Size.Width / 2, pictureBoxIpl1.Size.Height / 2), 0, Cv.RGB(255, 0, 0), 8);



            pictureBoxIpl1.ImageIpl = imgSrc;
            //pictureBoxIpl1.SizeMode = PictureBoxSizeMode.CenterImage;
            //pictureBoxIpl1.BackgroundImage = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.Camera_CrossPoint;
        }

        private delegate void setMotorValueDelegatee(TextBox txb, string str);
        public void setMotorvalue(TextBox txb, string str)
        {
            if (txb.InvokeRequired)
            {
                setMotorValueDelegatee st = new setMotorValueDelegatee(setMotorvalue);
                txb.Invoke(st, txb, str);
            }
            else
            {

                txb.Text = str;
                
            }
        }

        private void ControlDefaultSetting()
        {

               
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);
                object table = iwdsob.TableGet();

                Array tableCoares = (Array)table;

                if (tableCoares.Length == 0) { return; }
                
                int Coares = 0;

                
                Coares = (int)tableCoares.GetValue(tableCoares.Length / 5 -1, 1);


                frMainFocusWdLswis.Value = Coares;
                

               
                //int Coarse = (Int32)table[4, 1];

               
                
                setManager.ColumnOneLoad(equip.ColumnLensOLF, ColumnOnevalueMode.Factory);
           
                
          
                //setManager.ColumnOneLoad(equip.ColumnStigXAB, ColumnOnevalueMode.Factory);
                //setManager.ColumnOneLoad(equip.ColumnStigXCD, ColumnOnevalueMode.Factory);
        
                //setManager.ColumnOneLoad(equip.ColumnStigYAB, ColumnOnevalueMode.Factory);
                //setManager.ColumnOneLoad(equip.ColumnStigYCD, ColumnOnevalueMode.Factory);
         
                setManager.ColumnOneLoad(equip.ColumnHVFilament, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVGrid, ColumnOnevalueMode.Factory);
                //alignHVR.Checked = false;
       
                //setManager.ColumnOneLoad(equip.ColumnGAX, ColumnOnevalueMode.Factory);
                //setManager.ColumnOneLoad(equip.ColumnGAY, ColumnOnevalueMode.Factory);
                //alignGunR.Checked = false;
        
                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);
                //detectorReset.Checked = false;
    
                setManager.ColumnOneLoad(equip.ColumnBSX, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnBSY, ColumnOnevalueMode.Factory);
         
                setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.Factory);
                //ss3Reset.Checked = false;    
            //bitmapMiddle.Checked = true;
                //SpotSizeChange(sender, e);
        
                //setManager.ColumnOneLoad(equip.ColumnStigXV, ColumnOnevalueMode.Factory);
                //setManager.ColumnOneLoad(equip.ColumnStigYV, ColumnOnevalueMode.Factory);
        
       
                //setManager.ColumnOneLoad(equip.ColumnDynamicFocus, ColumnOnevalueMode.Factory);
    
                (equip.ColumnScanRotation as SECtype.IControlDouble).Value = 0;
                MagCount = 0;
                frMainRotateDisLab.Text = ((int)(MagCount)).ToString() + "\x00B0";
                frMainRotateReset.Checked = false;

            //setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
            //setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

            //detectClt.ControlValue = equip.ColumnHVCLT;
            //detectPmt.ControlValue = equip.ColumnHVPMT;
        }

        private void initArchivesAddress()
        {
            try
            {
                ArchivesAdress.Text = Properties.Settings.Default.ArchAdress;
            }
            catch
            {
                switch (SystemInfoBinder.Default.AppSeller)
                {
                    case AppSellerEnum.Hirox:
                        ArchivesAdress.Text = "C:\\";
                        break;

                    case AppSellerEnum.SEC:
                        ArchivesAdress.Text = "C:\\";
                        break;

                    default:
                        break;

                }
               
            }
            
        }

        private void Infomation()
        {
            //AboutBox1 ab = new AboutBox1();

            switch (SystemInfoBinder.Default.AppSeller)
            {
                case AppSellerEnum.Hirox:
                case AppSellerEnum.SEC:
                    InfomationLab.Text = "MiniSEM";
                    InfoVersionLab.Text = String.Format("Version {0}", System.Reflection.Assembly.LoadFrom(Application.ExecutablePath).GetName().Version.ToString());
                    InfoCompanyLab.Text = "Copyright © 2013 SEC";
                    InfoComLab.Text = "SEC";

                    InfotextDescriptionLab.Text = "SEC Co.,Ltd";
                    InfoPhoneLab.Text = "82-31-215-7341";
                    InfoAddrLab.Text = "www.seceng.co.kr";
                    break;

                case AppSellerEnum.Evex:
                    InfomationLab.Text = "Evex MiniSEM";
                    InfoVersionLab.Text = String.Format("Version {0}", System.Reflection.Assembly.LoadFrom(Application.ExecutablePath).GetName().Version.ToString());
                    InfoCompanyLab.Text = "Copyright © 2013 Evex";
                    InfoComLab.Text = "Evex";

                    InfotextDescriptionLab.Text = "Evex Inc.";
                    InfoPhoneLab.Text = "609-252-9192";
                    InfoAddrLab.Text = "www.evex.com";


                    break;

                case AppSellerEnum.Nikkiso:
                    InfomationLab.Text = "SEMTRAC";
                    InfoVersionLab.Text = String.Format("Version {0}", System.Reflection.Assembly.LoadFrom(Application.ExecutablePath).GetName().Version.ToString());
                    InfoCompanyLab.Text = "Copyright © 2012 NIKKISO";
                    InfoComLab.Text = "NIKKISO";

                    InfotextDescriptionLab.Text = "NIKKISO Co.,Ltd";
                    InfoPhoneLab.Text = "+81-3-3443-3732";
                    InfoAddrLab.Text = "www.nikkiso.co.jp";

                    break;
            }
        }


        /// <summary>
        /// 파일 system에서 wizard를 읽어 온다.
        /// </summary>
        private void LoadWizard()
        {
            List<string[]> wizardList = new List<string[]>();

            string[] files = System.IO.Directory.GetFiles(@".\Wizard\");
            foreach (string path in files)
            {
                // xml 파일만 읽을 수 있다.
                if (System.IO.Path.GetExtension(path) != ".xml") { continue; }

                System.Xml.XmlReader xtr = System.Xml.XmlReader.Create(path);

                // wizard 선언을 찾음. 없으면 사용 불가.
                if (!xtr.ReadToFollowing("Wizard"))
                {
                    xtr.Close();
                    continue;
                }


                // priority 순서대로 정리 하여야 하나, 일단은 이름 순으로 정렬 함.
                //string name;

                //if (xtr.MoveToAttribute("Name"))// 이름이 있는 경우
                //{
                //    name = xtr.ReadContentAsString();
                //}
                //else // 이름이 없는 경우
                //{
                //    name = System.IO.Path.GetFileName(path);
                //}

                //xtr.MoveToFirstAttribute();


                //string prio;
                //if (xtr.MoveToAttribute("Priority")) // 우선 순위가 있는 경우
                //{
                //    prio = xtr.ReadContentAsString();
                //}
                //else // 우선 순위가 없는 경우
                //{
                //    prio = "-1";
                //}


                //wizardList.Add(new string[] { prio, name, path });


                // Equipment 읽어서 안되는 것은 제거 해야 함.
                // SNE-1500M,3000M의 경우 Apertual align은 없어야 함.

                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Tag = path;

                if (xtr.MoveToAttribute("Name"))// 이름이 있는 경우
                {
                    tsmi.Text = xtr.ReadContentAsString();
                }
                else // 이름이 없는 경우
                {
                    tsmi.Text = System.IO.Path.GetFileName(path);
                }

                tsmi.Click += new EventHandler(Wizard_Click);
                //wizardToolStripMenuItem.DropDownItems.Add(tsmi);

                xtr.Close();
            }
        }

        private void ColumnLink()
        {
            SystemInfoBinder.Default.Nanoeye.Controller.CommunicationErrorOccured += new EventHandler<SEC.GenericSupport.DataType.CommunicationErrorOccuredEventArgs>(Controller_CommunicationErrorOccured);

           
            detectClt.ControlValue = equip.ColumnHVCLT;
            detectPmt.ControlValue = equip.ColumnHVPMT;

            //bsX.ControlValue = equip.ColumnBSX;
            //bsX.ControlValue = equip.ColumnBSX;
            //bsY.ControlValue = equip.ColumnBSY;

            BeamShiftXSCB.ControlValue = equip.ColumnBSX;
            BeamShiftYSCB.ControlValue = equip.ColumnBSY;

            //tcDynamicFocus.ControlValue = equip.ColumnDynamicFocus;

            ss2CL1.ControlValue = equip.ColumnLensCL1;
            ss2CL2.ControlValue = equip.ColumnLensCL2;

            ss3SpotSize.ControlValue = equip.ColumnLensCL1;
            //SpotValueLocationChange(ss3SpotSize);

            (equip.ColumnLensCL1 as SECtype.IControlDouble).ValueChanged += new EventHandler(LensCL1_ValueChanged);
            (equip.ColumnLensCL2 as SECtype.IControlDouble).ValueChanged += new EventHandler(LensCL2_ValueChanged);

            stigX.ControlValue = equip.ColumnStigXV;
            stigY.ControlValue = equip.ColumnStigYV;

            (equip.ColumnStigXV as SECtype.IControlDouble).ValueChanged += new EventHandler(StigX_ValueChanged);
            (equip.ColumnStigYV as SECtype.IControlDouble).ValueChanged += new EventHandler(StigY_ValueChanged);
            //StigXString();
            //StigYString();

            alignGunX.ControlValue = equip.ColumnGAX;
            alignGunY.ControlValue = equip.ColumnGAY;
            alignHVF.ControlValue = equip.ColumnHVFilament;
            alignHVG.ControlValue = equip.ColumnHVGrid;

            //BSEAmpScrollC.ControlValue = equip.ColumnBSEAmpC;
            //BSEAmpScrollD.ControlValue = equip.ColumnBSEAmpD;





            

            //frMainFocusKnobEkwicvd.ControlValue = equip.ColumnLensOLF as SECtype.IControlDouble;
            //frMainFocusKnobEkwicvd.ControlValue = equip.ColumnLensOLC as SECtype.IControlDouble;

            frMainFocusKnobEkwicvd.ControlValue = equip.ColumnLensOLC;
            frMainFocusWdLswis.ControlValue = equip.ColumnLensOLC;
            FunctionEllipseContorlB.ControlValue = equip.ColumnLensOLF;

            (equip.ColumnLensOLF as SECtype.IControlDouble).ValueChanged += new EventHandler(LensOlFine_ValueChanged);

            //setManager.ColumnOneLoad(equip.ColumnLensOLC, ColumnOnevalueMode.Factory);

            //TrackBarCoarse.ControlValue = equip.ColumnLensOLC;
            //TrackBarFine.ControlValue = equip.ColumnLensOLF as SECtype.IControlDouble;




            frMainFocusWdLswis.UserValueString = new Func<SEC.GUIelement.ImageForcusBar, string>(WDString);

            //ArchivesAdress.Text = Properties.Settings.Default.ArchAdress;

            (equip.ColumnLensOLC as SECtype.IControlDouble).ValueChanged += new EventHandler(LensOlCoarse_ValueChanged);

            m_ContrastDisp.ValueChanged +=new EventHandler(Contrast_ValueChanged);
            m_BrightnessDisp.ValueChanged +=new EventHandler(Brightness_ValueChanged);

            


            uint val;
            try
            {
                val = (uint)((equip.ColumnVacuumLastError as SECtype.IControlInt).Value);
                Trace.WriteLine("Vacuum last error - " + val.ToString("X"), "Info");
            }
            catch (Exception)
            {
                Trace.WriteLine("Controller is not support to get Vacuum_Last_Error.", "Warring");
            }

            try
            {
                val = (uint)((equip.ColumnVacuumResetCode as SECtype.IControlInt).Value);
                Trace.WriteLine("Vacuum reset code - " + val.ToString("X"), "Info");
            }
            catch (Exception)
            {
                Trace.WriteLine("Controller is not support to get Vacuum_Reset_Code.", "Warring");
            }
        }

        /// <summary>
        /// OL Coarse의 값이 바뀔 경우 Fine의 값을 중간 값으로 변경한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LensOlCoarse_ValueChanged(object sender, EventArgs e)
        {
            SECtype.IControlDouble icd = equip.ColumnLensOLF as SECtype.IControlDouble;
            icd.Value = (icd.Maximum + icd.Minimum) / 2;

            UIsetBinder.Default.MagMaximum = equip.MagLenghtGet();
            equip.MagChange(UIsetBinder.Default.MagIndex);

            MagStingChange();

            WDString();

        }

        private void WDString()
        {
            SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);
            int wd = iwdsob.WorkingDistance;


            string result = wd.ToString();
            //if (iwdsob.IsNegativeOverflow)
            //{
            //    if (iwdsob.IsPositiveOverflow) { result = "???"; }
            //    else { result = "< " + result; }
            //    frMainFocusWdLswis.ForeColor = Color.Red;
            //}
            //else if (iwdsob.IsPositiveOverflow)
            //{
            //    result = "> " + result;
            //    frMainFocusWdLswis.ForeColor = Color.Red;
            //}
            //else
            //{
            //    frMainFocusWdLswis.ForeColor = Color.White;
            //}

            
            //WDLabel.Text = result + " mm";
            //if (FCTFocusBtn.Checked)
            //{
                //FCTConValueLab.Text = result + " mm";
                SetControlLab(FCTConValueLab, result + " mm");
            //}
        }

        void LensOlFine_ValueChanged(object sender, EventArgs e)
        {
            FineString();
        }

        private void FineString()
        {
            //if (FCTFocusBtn.Checked)
            //{
                SECtype.IControlDouble icd = equip.ColumnLensOLF as SECtype.IControlDouble;

                //(int)((ewc.Value - ewc.Minimum) * 39 / (ewc.Maximum - ewc.Minimum));

                int value = (int)((icd.Value - icd.Minimum) * 100 / (icd.Maximum - icd.Minimum));

                //FCTConValueLab2.Text = value.ToString() + "%";
                SetControlLab(FCTConValueLab2, value.ToString() + "%");

            //}
        }

        delegate void SetControlLabCallBack(Label lab, string text);

        private void SetControlLab(Label lab, string text)
        {
            if (this.InvokeRequired)
            {
                SetControlLabCallBack d = new SetControlLabCallBack(SetControlLab);
                this.Invoke(d, new object[] { lab, text });
            }
            else
            {
                lab.Text = text;
            }
            //FCTConValueLab2.Text = text;
        }

       

        private void LensCL1String()
        {
            if (FCTSpotSizeBtn.Checked)
            {

                SECtype.IControlDouble icd = equip.ColumnLensCL1 as SECtype.IControlDouble;

                int value = (int)((icd.Value - icd.Minimum) * 100 / (icd.Maximum - icd.Minimum));



                SpotCL1Lab.Text = value.ToString() + "%";
            }
        }

        void LensCL1_ValueChanged(object sender, EventArgs e)
        {
            LensCL1String();
        }

        void LensCL2_ValueChanged(object sender, EventArgs e)
        {

            LensCL2String();
        }

        private void LensCL2String()
        {
            if (FCTSpotSizeBtn.Checked)
            {

                SECtype.IControlDouble icd = equip.ColumnLensCL2 as SECtype.IControlDouble;

                int value = (int)((icd.Value - icd.Minimum) * 100 / (icd.Maximum - icd.Minimum));

                SpotCL2Lab.Text = value.ToString() + "%";
            }
        }

        void StigX_ValueChanged(object sender, EventArgs e)
        {
            StigXString();
        }

        private void StigXString()
        {
            if (FCTStingBtn.Checked)
            {

                SECtype.IControlDouble icd = equip.ColumnStigXV as SECtype.IControlDouble;

                //int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;

                int value = (int)(((icd.Value - icd.Minimum) * 100 / (icd.Maximum - icd.Minimum)) - 50);

                StigXValueLab.Text = value.ToString() + "%";
            }
        }

        void StigY_ValueChanged(object sender, EventArgs e)
        {
            StigYString();
        }

        private void StigYString()
        {
            if (FCTStingBtn.Checked)
            {

                SECtype.IControlDouble icd = equip.ColumnStigYV as SECtype.IControlDouble;

                int value = (int)(((icd.Value - icd.Minimum) * 100 / (icd.Maximum - icd.Minimum)) - 50);

                StigYValueLab.Text = value.ToString() + "%";
            }
        }

        string WDString(SEC.GUIelement.ImageForcusBar control)
        {
            SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);
            int wd = iwdsob.WorkingDistance;


            string result = wd.ToString();
            if (iwdsob.IsNegativeOverflow)
            {
                if (iwdsob.IsPositiveOverflow) { result = "???"; }
                else { result = "< " + result; }
                frMainFocusWdLswis.ForeColor = Color.Red;
            }
            else if (iwdsob.IsPositiveOverflow)
            {
                result = "> " + result;
                frMainFocusWdLswis.ForeColor = Color.Red;
            }
            else
            {
                frMainFocusWdLswis.ForeColor = Color.White;
            }

            //WDLabel.Text = result.ToString();

            return result + " ";


        }

        void Contrast_ValueChanged(object sender, EventArgs e)
        {
            //ContrastString();
            int value = (int)((m_ContrastDisp.Value - m_ContrastDisp.Minimum) * 100 / (m_ContrastDisp.Maximum - m_ContrastDisp.Minimum));

            //ContrastLab.Text = value.ToString() + "%";

            SetVideotLab(ContrastLab, value.ToString() + "%");

        }

        delegate void VideoLabCallBack(Label lab, string text);

        private void SetVideotLab(Label lab, string text)
        {
            if (this.InvokeRequired)
            {
                VideoLabCallBack d = new VideoLabCallBack(SetVideotLab);
                this.Invoke(d, new object[] { lab, text });
            }
            else
            {
                lab.Text = text;
            }
        }

       

        void Brightness_ValueChanged(object sender, EventArgs e)
        {
            //BrightnessString();
            int value = (int)((m_BrightnessDisp.Value - m_BrightnessDisp.Minimum) * 100 / (m_BrightnessDisp.Maximum - m_BrightnessDisp.Minimum));

            //BrightnessLab.Text = value.ToString() + "%";
            SetVideotLab(BrightnessLab, value.ToString() + "%");
        }


        #endregion

        #region 키보드 처리
        private string[] HotkeyList = { "Coarse", "Fine", "StigX", "StigY", "CL1", "CL2", "Contrast", "Brightness" };
        private int HotkeyIndex = 0;


        #region 키보드 처리

        System.Windows.Forms.Timer SuttleTimer;
        bool suttleDirection = false;
        private void suttleControl(object sender, EventArgs e)
        {
           
            if (suttleDirection)
            {
               

                //if (FCTFocusBtn.Checked || FCTSpotSizeBtn.Checked)
                //{
                    frMainFocusKnobEkwicvd.Value++;
                    frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.Value * frMainFocusKnobEkwicvd.ControlValue.Precision;
                //}
                //else
                //{
                //    frMainFocusKnobEkwicvd.Value += 20;
                //    //frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.Value;
                //    //m_ContrastDisp.Value += 20;
                //}
                
                
            }
            else
            {
                
                //if (FCTFocusBtn.Checked || FCTSpotSizeBtn.Checked)
                //{
                    frMainFocusKnobEkwicvd.Value--;
                    frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.Value * frMainFocusKnobEkwicvd.ControlValue.Precision;
                //}
                //else
                //{
                //    frMainFocusKnobEkwicvd.Value -= 20;
                //    //frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.Value;
                //}
               
            }
          
           

        }
        
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            //ImageTabSpot.Checked = false;
            //ImageTabBeamShift.Checked = false;
            //ImageTabSig.Checked = false;

            //ppSingle.Focus();

            Keys key = keyData & ~(Keys.Shift | Keys.Control);


            switch (key)
            {

                case Keys.LButton | Keys.MButton | Keys.Space:
                    ppSingle.Focus();

                    //this.Focus();

                    MotorStage.StageTextChange(1, Properties.Settings.Default.MotorStageXLeft);
                    
                    //if (Properties.Settings.Default.MotorXDirection)
                    //{
                    //    MotorStage.StageMoveChange(1, -0.002);
                    //}
                    //else
                    //{
                    //    MotorStage.StageMoveChange(1, 0.002);
                    //}

                    break;

                case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space:

                    ppSingle.Focus();
                    //this.Focus();

                    MotorStage.StageTextChange(1, Properties.Settings.Default.MotorStageXRight);
                    //if (Properties.Settings.Default.MotorXDirection)
                    //{
                    //    MotorStage.StageMoveChange(1, 0.002);
                    //}
                    //else
                    //{
                    //    MotorStage.StageMoveChange(1, -0.002);
                    //}

                    break;

                case Keys.RButton | Keys.MButton | Keys.Space:

                    ppSingle.Focus();
                    //this.Focus();

                    MotorStage.StageTextChange(2, Properties.Settings.Default.MotorStageYTop);
                    //if (Properties.Settings.Default.MotorYDirection)
                    //{
                    //    MotorStage.StageMoveChange(2, -0.002);
                    //}
                    //else
                    //{
                    //    MotorStage.StageMoveChange(2, 0.002);
                    //}


                    break;

                case Keys.Back | Keys.Space:

                    ppSingle.Focus();
                    //this.Focus();

                    MotorStage.StageTextChange(2, Properties.Settings.Default.MotorStageYBottom);

                    //if (Properties.Settings.Default.MotorYDirection)
                    //{
                    //    MotorStage.StageMoveChange(2, 0.002);
                    //}
                    //else
                    //{
                    //    MotorStage.StageMoveChange(2, -0.002);
                    //}

                    break;

                case Keys.Add:

                    ppSingle.Focus();
                    //if (FCTFocusBtn.Checked || FCTSpotSizeBtn.Checked)
                    //{
                        FunctionEllipseContorlB.Value++;
                        FunctionEllipseContorlB.ControlValue.Value = FunctionEllipseContorlB.Value * FunctionEllipseContorlB.ControlValue.Precision;
                    //}
                    //else
                    //{
                    //    FunctionEllipseContorlB.Value += 20;
                    //    //FunctionEllipseContorlB.ControlValue.Value = FunctionEllipseContorlB.Value * FunctionEllipseContorlB.ControlValue.Precision;
                    //}


                    break;

                case Keys.Subtract:
                    ppSingle.Focus();
                    FunctionEllipseContorlB.Value--;
                    //if (FCTFocusBtn.Checked || FCTSpotSizeBtn.Checked)
                    //{
                        FunctionEllipseContorlB.ControlValue.Value = FunctionEllipseContorlB.Value * FunctionEllipseContorlB.ControlValue.Precision;
                    //}
                    //else
                    //{
                    //    FunctionEllipseContorlB.Value -= 20;
                    //    //FunctionEllipseContorlB.ControlValue.Value = FunctionEllipseContorlB.Value * FunctionEllipseContorlB.ControlValue.Precision;
                    //}
                    break;

                case Keys.Divide:
                    ppSingle.Focus();
                    if (SuttleTimer == null)
                    {
                        SuttleTimer = new System.Windows.Forms.Timer();
                        SuttleTimer.Tick += new EventHandler(suttleControl);
                        SuttleTimer.Interval = 10;
                        suttleDirection = false;
                        SuttleTimer.Start();
                    }



                    break;

                case Keys.Multiply:
                    ppSingle.Focus();
                    if (SuttleTimer == null)
                    {
                        SuttleTimer = new System.Windows.Forms.Timer();
                        SuttleTimer.Tick += new EventHandler(suttleControl);
                        SuttleTimer.Interval = 10;
                        suttleDirection = true;
                        SuttleTimer.Start();
                    }

                    break;

                case Keys.Decimal:
                    ppSingle.Focus();
                    SuttleTimer.Tick -= new EventHandler(suttleControl);
                    SuttleTimer.Stop();
                    SuttleTimer = null;
                    break;

                default:
                    break;
            }

           

            //switch (key)
            //{

            //    case Keys.Q:

            //         string scanName = "FaseScan"; 
            //        if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.Merge || SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.DualSEBSE)
            //        {
            //            scanName = scanner.ItemsRunning[0].Name;

            //            frMainScanSfBb.PerformClick();
            //            Delay(500);
            //        }

            //        new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());

            //        if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.Merge || SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.DualSEBSE)
            //        {
            //            switch (scanName)
            //            {
            //                case "Fast Scan":
            //                    frMainScanSfBb.PerformClick();
            //                    break;

            //                case "Slow Scan":
            //                    frMainScanSsBb.PerformClick();
            //                    break;

            //                case "Fast Photo":
            //                    frMainScanPfBb.PerformClick();
            //                    break;

            //                case "Slow Photo":
            //                    frMainScanPsBb.PerformClick();
            //                    break;

            //                case "Slow Photo2":
            //                    frMainScanPsBb2.PerformClick();
            //                    break;
            //            }

                        
            //        }

            //        //new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
            //        break;

            //    case Keys.W:
            //        m_ContrastDisp.Value--;
            //        break;

            //    case Keys.E:
            //        m_ContrastDisp.Value++;
            //        break;

            //    case Keys.R:
            //        m_BrightnessDisp.Value--;
            //        break;

            //    case Keys.T:
            //        m_BrightnessDisp.Value++;
            //        break;

            //    case Keys.A:
            //        frMainFocusWdLswis.Value--;
            //        if (focusmodeDl.Visible)
            //        {
            //            focusmodeDl.SelectedIndex = 0;
            //        }


            //        break;

            //    case Keys.S:
            //        frMainFocusWdLswis.Value++;
            //        if (focusmodeDl.Visible)
            //        {
            //            focusmodeDl.SelectedIndex = 0;
            //        }
            //        break;

            //    case Keys.D:
            //        frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.ControlValue.Value - frMainFocusKnobEkwicvd.ControlValue.Precision;
            //        focusmodeDl.SelectedIndex = 1;
            //        break;

            //    case Keys.F:
            //        frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.ControlValue.Value + frMainFocusKnobEkwicvd.ControlValue.Precision;
            //        focusmodeDl.SelectedIndex = 1;
            //        break;

            //    case Keys.OemOpenBrackets:
            //        ss3SpotSize.Value--;
            //        ImageTabSpot.Checked = true;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = false;
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];
            //        break;

            //    case Keys.OemCloseBrackets:
            //        ss3SpotSize.Value++;
            //        ImageTabSpot.Checked = true;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = false;
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];
            //        break;

            //    case Keys.I:
            //        ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.3) + ss3SpotSize.Minimum);
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];
            //        ImageTabSpot.Checked = true;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.O:
            //        ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.5) + ss3SpotSize.Minimum);
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];
            //        ImageTabSpot.Checked = true;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.P:
            //        ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.7) + ss3SpotSize.Minimum);
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];
            //        ImageTabSpot.Checked = true;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.Z:
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imStig"];
            //        stigX.Value -= 5;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = true;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.X:
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imStig"];
            //        stigX.Value += 5;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = true;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.C:
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imStig"];
            //        stigY.Value -= 5;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = true;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.V:
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imStig"];
            //        stigY.Value += 5;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = true;
            //        ImageTabBeamShift.Checked = false;
            //        break;

            //    case Keys.LButton | Keys.MButton | Keys.Space:

            //        //MotorStage.MotorXLMove();

            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imBshift"];
            //        //bsX.Value--;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = true;
            //        break;

            //    case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space:

            //        //MotorStage.MotorXRMove();
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imBshift"];
            //        //bsX.Value++;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = true;
            //        break;

            //    case Keys.RButton | Keys.MButton | Keys.Space:

            //        //MotorStage.MotorYTMove();
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imBshift"];
            //        //bsY.Value++;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = true;
            //        break;

            //    case Keys.Back | Keys.Space:
            //        //MotorStage.MotorYBMove();
            //        Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imBshift"];
            //        //bsY.Value--;
            //        ImageTabSpot.Checked = false;
            //        ImageTabSig.Checked = false;
            //        ImageTabBeamShift.Checked = true;
            //        break;

            //    case Keys.LButton | Keys.ShiftKey | Keys.Space:
            //        frMainScanSfBb.PerformClick();
            //        break;

            //    case Keys.RButton | Keys.ShiftKey | Keys.Space:
            //        frMainScanSsBb.PerformClick();
            //        break;

            //    case Keys.LButton | Keys.RButton | Keys.ShiftKey | Keys.Space:
            //        frMainScanPfBb.PerformClick();
            //        break;

            //    case Keys.MButton | Keys.ShiftKey | Keys.Space:
            //        frMainScanPsBb.PerformClick();
            //        break;

            //    case Keys.LButton | Keys.MButton | Keys.ShiftKey | Keys.Space:
            //        frMainScanPauseBb.PerformClick();
            //        break;

            //    case Keys.RButton | Keys.MButton | Keys.ShiftKey | Keys.Space:

            //        ImageExportManager.ImageExportModeEnum iee = ImageExportManager.ImageExportModeEnum.File;

            //        bool scanning;

            //        if (scanner.ItemsRunning == null)
            //        {
            //            scanning = false;
            //        }

            //        else
            //        {
            //            if (scanner.ItemsRunning[0].Setting.Name == "Scan Pause") { scanning = false; }
            //            else { scanning = true; }
            //        }

            //         string ImageSaveFileName = null;

            //        ImageSaveFileName = ImageAutoName(ImageSaveFileName);

            //        ImageExportManager.ImageExport(ppSingle, iee, !scanning, ImageSaveFileName);

            //        string filename = ImageExportManager.SystemInfoFileName;
            //        WriteSystemInfo(filename);

            //        break;

            //    case Keys.Add:
            //    case Keys.RButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17:
            //        MagTrakBar.Value++;
            //        break;

            //    case Keys.Subtract:
            //    case Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17:
            //        MagTrakBar.Value--;
            //        break;

            //    case Keys.ShiftKey | Keys.Space:
            //        if (m_ToolStartup.Checked)
            //        {
            //            m_ToolStartup.Checked = false;
            //        }
            //        else
            //        {
            //            m_ToolStartup.Checked = true;
            //        }
            //        break;

            //    default:
            //        break;


            //}

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

       


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey) { ppSingle.MTools.IsSymetric = true; }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey) { ppSingle.MTools.IsSymetric = false; }

            base.OnKeyUp(e);
        }
        #endregion

        #region Binding 이벤트 처리
        void SystemInfoBinder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DetectorMode":
                case "VacuumMode":

                    if ((SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.SED) && (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum))
                    {
                        column["HvPmt"].Enable = true;
                        column["HvCollector"].Enable = true;
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvPmt"], ColumnOnevalueMode.Run);
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvCollector"], ColumnOnevalueMode.Run);
                    }
                    else if ((SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.DualSEBSE) && (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum))
                    {
                        column["HvPmt"].Enable = true;
                        column["HvCollector"].Enable = true;
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvPmt"], ColumnOnevalueMode.Run);
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvCollector"], ColumnOnevalueMode.Run);
                    }
                    else if ((SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.Merge) && (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum))
                    {
                        column["HvPmt"].Enable = true;
                        column["HvCollector"].Enable = true;
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvPmt"], ColumnOnevalueMode.Run);
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvCollector"], ColumnOnevalueMode.Run);
                    }
                    else
                    {
                        ((SECtype.IControlDouble)column["HvPmt"]).Value = 0;
                        ((SECtype.IControlDouble)column["HvCollector"]).Value = 0;
                        column["HvPmt"].Enable = false;
                        column["HvCollector"].Enable = false;
                    }
                    ChangeDetectAndVacuumStateDisplay();
                    break;
                case "ImageExportable": // ToolStipMenuItem은 DataBinding이 되지 않는다.
                    //exportFileTsmi.Enabled = SystemInfoBinder.Default.ImageExportable;
                    //exportPrintTsmi.Enabled = SystemInfoBinder.Default.ImageExportable;
                    //exportManagerTsmi.Enabled = SystemInfoBinder.Default.ImageExportable;
                    break;
                case "ScanSource":
                    {
                        string normalMode;
                        string externMode = "fpSpotMode2";

                        switch (SystemInfoBinder.Default.AppDevice)
                        {
                            case AppDeviceEnum.SNE1500M:
                            case AppDeviceEnum.SNE3000M:
                                normalMode = "fpSpotMode1";
                                break;
                            case AppDeviceEnum.SNE4000M:
                                normalMode = "fpSpotMode3";
                                break;
                            default:
                                Trace.WriteLine("Invalid Device in InitializeOptionUI.", "Error");
                                throw new NotSupportedException();
                        }

                        fpList.Remove(frontPanelUtc.Tabs["fpSpotMode1"]);
                        fpList.Remove(frontPanelUtc.Tabs["fpSpotMode2"]);
                        fpList.Remove(frontPanelUtc.Tabs["fpSpotMode3"]);

                        int index = fpList.IndexOf(frontPanelUtc.Tabs["fpStig"]);

                        switch (SystemInfoBinder.Default.ScanSource)
                        {
                            case 0:
                                setManager.ColumnOneSave(equip.ColumnLensCL1, ColumnOnevalueMode.External);
                                setManager.ColumnOneSave(equip.ColumnLensCL2, ColumnOnevalueMode.External);
                                setManager.ColumnOneSave(equip.ColumnHVPMT, ColumnOnevalueMode.External);
                                setManager.ColumnOneSave(equip.ColumnHVCLT, ColumnOnevalueMode.External);
                                fpList.Insert(index, frontPanelUtc.Tabs[normalMode]);
                                setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.Run);
                                setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.Run);
                                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Run);
                                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Run);
                                break;
                            case 1:
                                setManager.ColumnOneSave(equip.ColumnLensCL1, ColumnOnevalueMode.Run);
                                setManager.ColumnOneSave(equip.ColumnHVPMT, ColumnOnevalueMode.Run);
                                setManager.ColumnOneSave(equip.ColumnHVCLT, ColumnOnevalueMode.Run);
                                fpList.Insert(index, frontPanelUtc.Tabs[externMode]);
                                setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.External);
                                setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.External);
                                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.External);
                                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.External);
                                break;
                        }

                        if (frontPanelUtc.SelectedTab.Text == "Spot Size")
                        {
                            frontPanelUtc.SelectedTab = fpList[index];
                        }
                    }
                    break;

                case "ImageContrast2":
                    ppSingle.Contrast2 = SystemInfoBinder.Default.Contrast2;
                    break;

                case "ImageBrightness2":
                    ppSingle.Brightness2 = SystemInfoBinder.Default.Brightness2;
                    break;
            }
        }

        void UIsetBinder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MagIndex":
                    equip.MagChange(UIsetBinder.Default.MagIndex);

                    MagStingChange();
                    break;
            }
        }
        #endregion

        #region Wizard 지원
        List<SEC.GUIelement.Emphasis> emphasisAreaList = new List<SEC.GUIelement.Emphasis>();

        #region Emphasis
        internal void WizardEmphasis(string[][] emphasisList)
        {
            WizardEmphasisClear();

            if (emphasisList == null) { return; }

            List<SEC.GUIelement.Emphasis> eal = new List<SEC.GUIelement.Emphasis>();
            foreach (string[] emphasis in emphasisList)
            {
                switch (emphasis[0])
                {
                    case "Control":
                        Control con = FindControl(emphasis[1], this.Controls);

                        if (con != null)
                        {
                            SEC.GUIelement.Emphasis e = new SEC.GUIelement.Emphasis();
                            con.Parent.Controls.Add(e);

                            e.EmphasisMode = SEC.GUIelement.Emphasis.EmphasisModeType.Control;
                            e.Control = con;

                            con.Parent.Controls.SetChildIndex(e, 0);

                            eal.Add(e);
                        }
                        else { Trace.WriteLine("Can't fine emphasis control - name : " + emphasis[0] + " target : " + emphasis[1], "Wizard-FormMain"); }
                        break;
                    case "Area":
                        WizardEmphasisAreaSet(emphasis, eal);
                        break;
                    default:
                        Trace.WriteLine("Undefine Emphasis - name : " + emphasis[0], "Wizard-FormMain");
                        break;
                }
            }

            emphasisAreaList = eal;
            this.Invalidate();
        }

        private void WizardEmphasisClear()
        {
            lock (emphasisAreaList)
            {
                foreach (Control con in emphasisAreaList)
                {
                    try { con.Parent.Controls.Remove(con); }
                    catch (Exception) { }
                    con.Dispose();
                }
                emphasisAreaList.Clear();
            }
        }

        private void WizardEmphasisAreaSet(string[] emphasis, List<SEC.GUIelement.Emphasis> eal)
        {
            if (emphasis.Length != 6)
            {
                Trace.WriteLine("Invalid empahsis area data lenght - " + emphasis.Length.ToString() + ", name - " + emphasis[0], "Wizard-FormMain");
                return;
            }

            SEC.GUIelement.Emphasis e = new SEC.GUIelement.Emphasis();
            e.EmphasisMode = SEC.GUIelement.Emphasis.EmphasisModeType.Area;

            Rectangle rect;
            try
            {
                rect = new Rectangle(int.Parse(emphasis[1]), int.Parse(emphasis[2]), int.Parse(emphasis[3]), int.Parse(emphasis[4]));
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Invalid empasis area location - " + emphasis[0], "Wizard-FormMain");
                SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
                return;
            }

            Control con = FindControl(emphasis[5], this.Controls);

            if (con == null)
            {
                Trace.WriteLine("Can't find base control - " + emphasis[5] + ", name - " + emphasis[0], "Wizard-FormMain");
                return;
            }
            e.Area = rect;
            con.Controls.Add(e);

            try
            {
                e.Parent.Controls.SetChildIndex(e, 0);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Fail to set top control - " + emphasis[0], "Wizard-FormMain");
                SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
            }
            eal.Add(e);
        }
        #endregion

        #region Condition
        List<Control> wizardConditionCache = new List<Control>();
        internal bool WizardConditionCheckUI(string name, bool type, string target, string value, bool log)
        {
            Component con = null;
            switch (name)
            {
                case "focusWobbleUpcc":
                    con = focusWobbleUpcc;
                    break;
                default:
                    lock (wizardConditionCache)
                    {
                        foreach (Control conIn in wizardConditionCache)
                        {
                            if (conIn.Name == name)
                            {
                                con = conIn;
                                break;
                            }
                        }
                    }
                    break;
            }

            // 캐쉬에 없는 경우... 시간 오래 걸릴듯..
            if (con == null)
            {
                con = FindControl(name, this.Controls);

                if (con == null)
                {
                    if (log) { Trace.WriteLine("Can't find control - " + name, "Wizard-FormMain"); }
                    return true;
                }

                lock (wizardConditionCache) { wizardConditionCache.Add(con as Control); }
            }

            if (con is Control)	// component 는 visible이 없다.
            {
                // 화면 상에 보여 지고 있는지를 확인 한다.
                if (!CheckVisable(con as Control)) { return false; }
            }

            System.Reflection.MemberInfo[] mis = con.GetType().GetMembers();

            foreach (System.Reflection.MemberInfo mi in mis)
            {
                if (mi.Name == target)
                {
                    switch (mi.MemberType)
                    {
                        case System.Reflection.MemberTypes.Constructor:
                        case System.Reflection.MemberTypes.Custom:
                        case System.Reflection.MemberTypes.Event:
                        case System.Reflection.MemberTypes.Field:
                        case System.Reflection.MemberTypes.Method:
                        case System.Reflection.MemberTypes.NestedType:
                        case System.Reflection.MemberTypes.TypeInfo:
                        default:
                            if (log) { Trace.WriteLine("Undefined memberType. - name : " + name + ", target : " + target, "Wizard-FormMain"); }
                            return true;
                        case System.Reflection.MemberTypes.Property:
                            return WizardCheckProperty(con, mi as System.Reflection.PropertyInfo, type, value);
                    }
                }
            }

            if (log) { Trace.WriteLine("Can't find farget. - name : " + name + ", target : " + target, "Wizard-FormMain"); }
            return true;
        }

        private bool WizardCheckProperty(Component con, System.Reflection.PropertyInfo propertyInfo, bool type, string value)
        {
            object pValue = propertyInfo.GetValue(con, null).ToString();
            string sValue = pValue.ToString();
            return (type == (sValue == value));
        }
        #endregion

        private bool CheckVisable(Control con)
        {
            if (!con.Visible) { return false; }

            if (con.Parent == null) { return true; }

            return CheckVisable(con.Parent);
        }

        private Control FindControl(string name, Control.ControlCollection controlCollection)
        {
            foreach (Control con in controlCollection)
            {
                if (con.Name == name) { return con; }

                Control conT = FindControl(name, con.Controls);

                if (conT != null) { return conT; }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 프로그램 상태 점검용.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void SystemCheckTimer_Tick(object sender, EventArgs e)
        {
            ppSingle.Date = DateTime.Now.ToString("yyyy.MM.dd HH:mm");
        }

        public override string ToString()
        {
            return "FromMain";
        }

        #region 주사 관련
        #region Scanner

        SECimage.SettingScanner beforScanner;
        private void ApplyScanningProfile(ScanModeEnum name, bool canvasPaint, bool useBeforeProfile, bool bse)
        {
            if (beforScanner != null)
            {
                if (beforScanner.Name == "Slow Photo2" && name == ScanModeEnum.SlowPhoto2)
                {
                    return;
                }
            }
            

            SECimage.SettingScanner ss;
            ss = SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)name]);
            beforScanner = ss;

            if ((name == ScanModeEnum.FastScan) && (FastScanScanningArea != 5))
            {
                AreaScanProfile(ss);
            }

            if (bse)
            {
                ss.AiChannel += 4;
            }

            if (samplingtime == null)
            {
                ss.SampleComposite = ss.SampleComposite;
            }
            else if (samplingtime.SamplingTimeValue == 1)
            {
                ss.SampleComposite = ss.SampleComposite;
            }
            else
            {
                ss.SampleComposite = ss.SampleComposite + (samplingtime.SamplingTimeValue * 2);
            }
            

            try
            {
                //scanner.Stop(true);
                scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);
            }
            catch
            {
                MessageBox.Show("NI DAQ, check out the power supply and cables");
                return;
            }

            
          


           
            SECimage.IScanItemEvent isie = scanner.ItemsReady[0];

            if (canvasPaint)
            {
                ppSingle.EventLink(isie, isie.Name);
                //videoMeanlevelDisp.Text = isie.Setting.AverageLevel.ToString();
            }
            else { ppSingle.EventRelease(); }

            //scanner.OnePoint(SpotModePntX, SpotModePntX);


            if (name == ScanModeEnum.SpotMode)
            {
                scanner.OnePoint(SpotModePntX, SpotModePntY);
            }
            else if (name == ScanModeEnum.ScanPause)
            {
                if (framScanEnable)
                {
                    scanner.scanFrameStop();
                    framScanEnable = false;
                }
                else
                {
                    //scanner.Stop();
                   
                    scanner.OnePoint(0, 0);
                }
                
               
            }
            else
            {
                scanner.Change();
            }
           
        
            

           
               
            
        }

        private void AreaScanProfile(SEC.Nanoeye.NanoImage.SettingScanner ss)
        {
            switch (FastScanScanningArea)
            {
                case 1:
                    ss.PaintX -= 0.25f;
                    ss.PaintY -= 0.1875f;
                    ss.ShiftX -= ss.AreaShiftX;
                    ss.ShiftY -= ss.AreaShiftY;
                    ss.Name = "AS TL";
                    break;
                case 2:
                    //ss.PaintX -= 0.25f;
                    ss.PaintY -= 0.1875f;
                    //ss.ShiftX -= ss.AreaShiftX;
                    ss.ShiftY -= ss.AreaShiftY;
                    ss.Name = "AS TC";
                    break;
                case 3:
                    ss.PaintX += 0.25f;
                    ss.PaintY -= 0.1875f;
                    ss.ShiftX += ss.AreaShiftX;
                    ss.ShiftY -= ss.AreaShiftY;
                    ss.Name = "AS TR";
                    break;
                case 4:
                    ss.PaintX -= 0.25f;
                    //ss.PaintY -= 0.125f;
                    ss.ShiftX -= ss.AreaShiftX;
                    //ss.ShiftY -= ss.AreaShiftY;
                    ss.Name = "AS ML";
                    break;
                case 6:
                    ss.PaintX += 0.25f;
                    //ss.PaintY -= 0.125f;
                    ss.ShiftX += ss.AreaShiftX;
                    //ss.ShiftY -= ss.AreaShiftY;
                    ss.Name = "AS MR";
                    break;
                case 7:
                    ss.PaintX -= 0.25f;
                    ss.PaintY += 0.1875f;
                    ss.ShiftX -= ss.AreaShiftX;
                    ss.ShiftY += ss.AreaShiftY;
                    ss.Name = "AS BL";
                    break;
                case 8:
                    //ss.PaintX -= 0.25f;
                    ss.PaintY += 0.1875f;
                    //ss.ShiftX -= ss.AreaShiftX;
                    ss.ShiftY += ss.AreaShiftY;
                    ss.Name = "AS BC";
                    break;
                case 9:
                    ss.PaintX += 0.25f;
                    ss.PaintY += 0.1875f;
                    ss.ShiftX += ss.AreaShiftX;
                    ss.ShiftY += ss.AreaShiftY;
                    ss.Name = "AS BR";
                    break;
                default:
                    throw new InvalidOperationException("Undefined Area Scan.");
            }
        }

        private void ScanSelect_Click(object sender, EventArgs e)
        {
            SEC.Nanoeye.Controls.BitmapRadioButton brb = sender as SEC.Nanoeye.Controls.BitmapRadioButton;
            ScanModeEnum sme = (ScanModeEnum)brb.Tag;

            
            ScanModeChange(sme);
            

            
        }

        private int FastScanScanningArea = 5;
        FastScanAreaSelect fsas;

        void fsas_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Activate();
            if (fsas.ScanningArea != FastScanScanningArea)
            {
                FastScanScanningArea = fsas.ScanningArea;
                if (frMainScanPauseBb.Checked)
                {
                    ApplyScanningProfile(ScanModeEnum.FastScan, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                }
            }
            fsas.Dispose();
            fsas = null;
        }

        
        private void ScanModeChange(ScanModeEnum sme)
        {

                         
            frMainScanPauseBb.Checked = false;
            frMainScanPfBb.Checked = false;
            frMainScanPsBb.Checked = false;
            frMainScanPsBb2.Checked = false;
            frMainScanSfBb.Checked = false;
            frMainScanSsBb.Checked = false;

            //if (scanFrameUpdate)
            //{
            //    ppSingle.SiEvent.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(scanFramUpdate);
            //    scanFrameUpdate = false;
            //}

            
           

            try
            {
                switch (sme)
                {
                    case ScanModeEnum.ScanPause:
                        //if (m_ToolStartup.Checked)
                        //{
                       
                        ApplyScanningProfile(ScanModeEnum.ScanPause, false, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                        //}
                        //else
                        //{
                        //    ppSingle.EventRelease();
                        //    scanner.Stop(true);
                        //}

                        SystemInfoBinder.Default.ImageExportable = true;
                        //toolMeasuring.Enabled = m_MenuMeasureTools.Enabled = true;
                        //tsmiSpotMode.Enabled = false;

                        frMainScanPauseBb.Checked = true;

                        SpotModeBtn.Enabled = false;

                        Debug.WriteLine("ScanPause", this.ToString());
                        break;

                    case ScanModeEnum.FastScan:
                        SystemInfoBinder.Default.ImageExportable = false;
                        //toolMeasuring.Enabled = m_MenuMeasureTools.Enabled = false;
                        //tsmiSpotMode.Enabled = false;

                        frMainScanSfBb.Checked = true;


                        ApplyScanningProfile(ScanModeEnum.FastScan, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);

                        SpotModeBtn.Enabled = false;

                        Debug.WriteLine("Fast Scan Mode", this.ToString());
                     
                        break;

                    case ScanModeEnum.SlowScan:
                        SystemInfoBinder.Default.ImageExportable = true;
                        //toolMeasuring.Enabled = m_MenuMeasureTools.Enabled = false;
                        //tsmiSpotMode.Enabled = true;

                        frMainScanSsBb.Checked = true;


                        ApplyScanningProfile(ScanModeEnum.SlowScan, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                        
                        //ppSingle.SiEvent.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(scanFramUpdate);
                       

                        SpotModeBtn.Enabled = true;

                        Debug.WriteLine("Slow Scan Mode", this.ToString());
                        break;

                    case ScanModeEnum.FastPhoto:
                        SystemInfoBinder.Default.ImageExportable = true;
                        //toolMeasuring.Enabled = m_MenuMeasureTools.Enabled = false;
                        //tsmiSpotMode.Enabled = true;

                        frMainScanPfBb.Checked = true;
                        //ppSingle.AutoRelease = true;

                        ApplyScanningProfile(ScanModeEnum.FastPhoto, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                        //ppSingle.SiEvent.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(scanFramUpdate);


                        SpotModeBtn.Enabled = true;

                        

                        Debug.WriteLine("Fast Photo Mode", this.ToString());
                        break;

                    case ScanModeEnum.SlowPhoto:
                        SystemInfoBinder.Default.ImageExportable = true;
                        //toolMeasuring.Enabled = m_MenuMeasureTools.Enabled = false;
                        //tsmiSpotMode.Enabled = true;
                        //ppSingle.AutoRelease = true;
                       

                        frMainScanPsBb.Checked = true;
                        ApplyScanningProfile(ScanModeEnum.SlowPhoto, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                        SpotModeBtn.Enabled = true;

                        Debug.WriteLine("Slow Photo Mode", this.ToString());
                        break;

                    case ScanModeEnum.SlowPhoto2:
                        SystemInfoBinder.Default.ImageExportable = true;
                      
                        frMainScanPsBb2.Checked = true;
                        ApplyScanningProfile(ScanModeEnum.SlowPhoto2, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
                        //ppSingle.SiEvent.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(scanFramUpdate);
                        SpotModeBtn.Enabled = true;


                        Debug.WriteLine("Slow Photo Mode2", this.ToString());
                        break;
                }
            }
            catch (IOException ioe)
            {
                SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ioe);
                frMainScanPauseBb.PerformClick();
            }
            catch (Exception ex)
            {
                SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
                Trace.Fail(ex.Message, ex.StackTrace);
            }
        }


        bool framScanEnable = false;
        bool scanFrameUpdate = false;
        void scanFramUpdate(object sender, string name, int startline, int lines)
        {
            //scanFrameUpdate = true;


            
            ppSingle.SiEvent.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(scanFramUpdate);

            
            scanner.Stop(false);
                
            ApplyScanningProfile(ScanModeEnum.ScanPause, false, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);
            
            
            

            //ApplyScanningProfile(ScanModeEnum.SlowPhoto2, true, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);

            //if (frMainScanPfBb.Checked)
            //{
            //    setEnable_Control(frMainScanPfBb, false);
            //}

            //if (frMainScanPsBb.Checked)
            //{
            //    setEnable_Control(frMainScanPsBb, false);
            //}

            //setEnable_Control(frMainScanPauseBb, true);

            

            //GC.Collect();
         
            
        }


        delegate void Ctr_Involk(BitmapRadioButton ctr, bool enable);

        public void setEnable_Control(BitmapRadioButton ctr, bool enable)
        {
            if (ctr.InvokeRequired)
            {
                Ctr_Involk CI = new Ctr_Involk(setEnable_Control);
                ctr.Invoke(CI, ctr, enable);
            }
            else
            {
                ctr.Checked = enable;
            }
        }


       

        #endregion

        #region Video
        private void ContrastChange_Click(object sender, EventArgs e)
        {
            BitmapButton bb = sender as BitmapButton;

            if ((string)bb.Tag == "Dec") { SystemInfoBinder.Default.Contrast--; }
            else if ((string)bb.Tag == "Inc") { SystemInfoBinder.Default.Contrast++; }
            else { throw new ArgumentException(); }
        }

        private void BrightnessChange_Click(object sender, EventArgs e)
        {
            BitmapButton bb = sender as BitmapButton;

            if ((string)bb.Tag == "Dec") { SystemInfoBinder.Default.Brightness--; }
            else if ((string)bb.Tag == "Inc") { SystemInfoBinder.Default.Brightness++; }
            else { throw new ArgumentException(); }
        }

        private void VideoMeanlevenChange_Click(object sender, EventArgs e)
        {
            if (scanner.ItemsRunning == null) { return; }

            SECimage.IScanItemEvent isie = scanner.ItemsRunning[0];

            BitmapButton bb = sender as BitmapButton;

            if (isie != null)
            {
                if ((string)bb.Tag == "Dec")
                {
                    if (isie.Setting.AverageLevel > 0) { isie.Setting.AverageLevel--; }
                }
                else if ((string)bb.Tag == "Inc")
                {
                    if (isie.Setting.AverageLevel < 8) { isie.Setting.AverageLevel++; }
                }
                else
                {
                    throw new ArgumentException();
                }
                //videoMeanlevelDisp.Text = isie.Setting.AverageLevel.ToString();
            }
        }
        #endregion

        #region Spot Mode
        Form spotForm;
        Point spotPnt = new Point(-1, -1);
        RadioButton beforeSpotRB;
        private void SpecialModeSpot()
        {
            //tsmiDZcenter.PerformClick();
            //tsmiDZori.PerformClick();
            if (frMainScanSsBb.Checked) { beforeSpotRB = frMainScanSsBb; }
            else if (frMainScanPfBb.Checked) { beforeSpotRB = frMainScanPfBb; }
            else if (frMainScanSsBb.Checked) { beforeSpotRB = frMainScanSsBb; }
            else { throw new Exception("정의 되지 않은 버튼"); }

            //frMainScanPauseBb.Checked = true;
            ScanModeChange(ScanModeEnum.ScanPause);

            spotForm = new Form();
            spotForm.ControlBox = false;
            spotForm.Size = ppSingle.Size;
            spotForm.BackColor = Color.Black;
            spotForm.Opacity = 0.5d;
            //spotForm.LocationChanged += new EventHandler(spotForm_LocationChanged);
            spotForm.Disposed += new EventHandler(spotForm_Disposed);
            spotForm.StartPosition = FormStartPosition.Manual;
            spotForm.Location = new Point(this.Left + 9, this.Top + 50);
            spotForm.ShowInTaskbar = false;

            this.LocationChanged += new EventHandler(FormMain_LocationChanged);

            Panel imgPanel = new Panel();
            imgPanel.Location = new Point(ppSingle.Location.X , ppSingle.Location.Y - 50);
            imgPanel.Size = ppSingle.Size;
            imgPanel.BackColor = Color.SteelBlue;
            imgPanel.MouseClick += new MouseEventHandler(imgPanel_MouseClick);
            imgPanel.Paint += new PaintEventHandler(imgPanel_Paint);
            imgPanel.Cursor = Cursors.Cross;

            Button spotBut = new Button();
            spotBut.Location = new Point(732, 564);
            spotBut.Size = new Size(224, 112);
            spotBut.MouseClick += new MouseEventHandler(spotBut_MouseClick);
            spotBut.Cursor = Cursors.Arrow;
            spotBut.UseVisualStyleBackColor = true;
            //spotBut.BackColor = Color.Black;

            spotForm.Controls.Add(imgPanel);
            spotForm.Controls.Add(spotBut);
            spotForm.Cursor = Cursors.No;
            spotForm.Show(this);
        }

        void spotForm_Disposed(object sender, EventArgs e)
        {
            SpotFormClose();

        }

        void FormMain_LocationChanged(object sender, EventArgs e)
        {
            if (spotForm.WindowState != FormWindowState.Normal) { spotForm.WindowState = FormWindowState.Normal; }
        }

        void imgPanel_Paint(object sender, PaintEventArgs e)
        {
            if (spotPnt.X >= 0)
            {
                //e.Graphics.DrawEllipse(Pens.Red, new Rectangle(spotPnt, new Size(3, 3)));
                e.Graphics.FillEllipse(Brushes.Red, new Rectangle(new Point(spotPnt.X - 2, spotPnt.Y - 2), new Size(5, 5)));
            }
        }

        void SpotFormClose()
        {
            this.LocationChanged -= new EventHandler(FormMain_LocationChanged);
            this.Activate();
            //m_ScanPause.Checked = true;
            //			m_ScanSelect_CheckedChanged(m_ScanPause, EventArgs.Empty);
            spotPnt = new Point(-1, -1);
            beforeSpotRB.Checked = true;

            if (frMainScanSsBb.Checked)
            {
                ScanModeChange(ScanModeEnum.SlowScan);
            }
            else if(frMainScanPfBb.Checked)
            {
                ScanModeChange(ScanModeEnum.FastPhoto);
            }
            else if (frMainScanPsBb.Checked)
            {
                ScanModeChange(ScanModeEnum.SlowPhoto);
            }
            
        }

        void spotBut_MouseClick(object sender, MouseEventArgs e)
        {
            spotForm.Dispose();
            SpotFormClose();
        }


        //private SEC.Nanoeye.NanoeyeSEM.Settings.MiniSEM.ScanningProfile beforeProfileScanning;
        System.Threading.ManualResetEvent mreSpotMode = new System.Threading.ManualResetEvent(true);

        double SpotModePntX;
        double SpotModePntY;

        void imgPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (mreSpotMode.WaitOne(0, false) == false)
            {
                return;
            }
            mreSpotMode.Reset();

            scanner.Stop(true);

            Debug.WriteLine("Scan enable : " + scanner.IsRun);

            spotPnt = e.Location;
            spotForm.Controls[0].Invalidate();
            Debug.WriteLine(e.Location, "Spot Point");


            SECimage.SettingScanner ss;
            ss = SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)ScanModeEnum.SpotMode]);

            double ratiogap = (double)(ss.ImageWidth + ss.ImageLeft * 2.25) / ppSingle.Width * ss.RatioX;

            int left = (int)(-32767 * ratiogap + 65534 * ss.ShiftX);
            int top = (int)(-32767 * ss.RatioY + 65534 * ss.ShiftY);
            int right = (int)(32767 * ratiogap + 65534 * ss.ShiftX);
            int bottom = (int)(32767 * ss.RatioY + 65534 * ss.ShiftY);

            Rectangle bounds = Rectangle.FromLTRB(left, top, right, bottom);


            double xgap = (double)ss.ImageWidth / ppSingle.Width;
            double ygap = (double)ppSingle.Height / ss.ImageTop;


            SpotModePntX = (double)bounds.Width / ppSingle.Width * (spotPnt.X + ss.ImageLeft) + bounds.X;

            SpotModePntY = (double)(spotPnt.Y + (ss.ImageTop * 6)) * bounds.Height / ppSingle.Height + bounds.Y;


            Debug.WriteLine("Point : " + SpotModePntX.ToString() + "," + SpotModePntY.ToString(), "Spot Point");



            ApplyScanningProfile(ScanModeEnum.SpotMode, false, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);


            mreSpotMode.Set();

            Debug.WriteLine("Scan enable : " + scanner.IsRun);
        }

        void spotForm_LocationChanged(object sender, EventArgs e)
        {
            this.Location = spotForm.Location;
            //spotForm.Location = ppSingle.Location;
        }
        #endregion
        #endregion

        #region Main UI
        private void MainUI_Click(object sender, EventArgs e)
        {
            bool scanning;

            if (scanner.ItemsRunning == null)
            {
                scanning = false;
            }
            else
            {
                if (scanner.ItemsRunning[0].Setting.Name == "Scan Pause") { scanning = false; }
                else { scanning = true; }
            }

            //#region Tools
            //if ((sender == exportManagerTsmi) || (sender == toolManager))
            //{
            //    #region Image Open
            //    /*
            //    using (OpenFileDialog dialog = new OpenFileDialog())
            //    {
            //        dialog.Filter = "Bitmap File(*.BMP)|*.BMP|JPEG File(*.JPG)|*.JPG|PNG File(*.PNG)|*.PNG|GIF File(*.GIF)|*.GIF";
            //        dialog.AddExtension = true;
            //        dialog.CheckFileExists = false;
            //        dialog.DefaultExt = "*.BMP";

            //        if (dialog.ShowDialog(this) == DialogResult.OK)
            //        {
            //            try
            //            {
            //                FormImageOpen imageOpen = new FormImageOpen();
            //                imageOpen.Text = dialog.FileName;
            //                imageOpen.ImagePanel.BackgroundImage = Bitmap.FromFile(dialog.FileName);
            //                imageOpen.Owner = this;

            //                imageOpen.Show();
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message, "Image Viewer");
            //            }
            //        }
            //    }
            //     */
            //    #endregion

            //    ImageExportManager.ImageExport(ppSingle, ImageExportManager.ImageExportModeEnum.Manager, scanning);
            //}
            //else if (sender == screenRecoderToolStripMenuItem)
            //{
            //    (sfScreenRecoder.FormInstance as ScreenRecorder).ImageWindow = ppSingle;
            //    sfScreenRecoder.Show(this);
            //}
            //            else if (sender == WobbleMenuItem)
            //            {
            //                lwf = new SEC.Nanoeye.Support.Dialog.LensWobbleForm();
            //                lwf.Column = column;
            //                lwf.OL_Amplitude = OL_Amplitude_Value();
            //                lwf.OL_Frequency = Frequency;
            //                lwf.Show(this);


            ////				equip.Magnification

            //           }

            //#endregion

            #region Mode
            // if (sender == tsmiExternalAnalyzer)
            //{
            //    if (tsmiExternalAnalyzer.Checked)
            //    {
            //        tsmiExternalAnalyzer.Checked = false;
            //        SystemInfoBinder.Default.ScanSource = 0;
            //    }
            //    else
            //    {
            //        tsmiExternalAnalyzer.Checked = true;
            //        SystemInfoBinder.Default.ScanSource = 1;
            //    }
            //}
            //else if (sender == tsmiSpotMode)
            //{
            //    SpecialModeSpot();
            //}
            //else if (sender == tsmiCamera)
            //{
            //    tsmiCamera.Checked = !(tsmiCamera.Checked);
            //    CameraStateChange(tsmiCamera.Checked);
            //}
            #endregion

            //#region Adjustment
            //else if (sender == m_MenuDescriptor) { sfMicronModify.Show(); }
            //else if (sender == optionToolStripMenuItem)
            //{
            //    sfOption.Show(this);


            //}
            //else if (sender == m_MenuConfigMicroscope)
            //{
            //    if (!sfMicroscope.IsCreated)
            //    {
            //        sfMicroscope.Create();
            //        (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).HVtextChanged += new EventHandler(ColumnConfig_HVtextChanged);
            //        (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).ProfileListChanged += new EventHandler(ColumnConfig_ProfileListChanged);
            //    }
            //    sfMicroscope.Show(this);
            //}
            //else if (sender == m_MenuConfigScanning)
            //{
            //    sfScanner.Show(this);
            //}
            //else if (sender == FRTtoolStripMenuItem) { FilamentRuntimeChecker.ShowForm(this); }
            //#endregion

            //#region About
            //else if (sender == infomationToolStripMenuItem)
            //{
            //    AboutBox1 ab = new AboutBox1();

            //    ab.ShowDialog();
            //}
            //#endregion

            //#region Digital Zoom
            //else if (sender == tsmiDZori)
            //{

            //    tsmiDZori.Checked = true;
            //    tsmiDZ2.Checked = false;
            //    tsmiDZ4.Checked = false;

            //    ppSingle.DigitalZoomMode = 2;

            //    //InfoUpdate();
            //}
            //else if (sender == tsmiDZ2)
            //{
            //    tsmiDZori.Checked = false;
            //    tsmiDZ2.Checked = true;
            //    tsmiDZ4.Checked = false;

            //    ppSingle.DigitalZoomMode = 3;

            //    //InfoUpdate();
            //}
            //else if (sender == tsmiDZ4)
            //{
            //    tsmiDZori.Checked = false;
            //    tsmiDZ2.Checked = false;
            //    tsmiDZ4.Checked = true;

            //    ppSingle.DigitalZoomMode = 4;

            //    //InfoUpdate();
            //}
            //else if (sender == tsmiDZcenter)
            //{
            //    ppSingle.DigitalZoomPoint = new Point(0, 0);
            //}
            //#endregion

            //else { MessageBox.Show(this, "Undefined Menu."); }
        }

        void Language_Click(object sender, EventArgs e)
        {
            //ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            BitmapCheckBox lag = sender as BitmapCheckBox;

            switch (lag.Text)
            {
                case "Korean":
                case "한국어":
                case "朝鲜的":
                case "Корейский":
                case "Coréen":
                    Properties.Settings.Default.Language = "ko-KR";
                    //cultureCode = 0x0412;	//ko-KR\

                    //cultureCode = "ko-KR";	//ko-KR
                    //LanguageButton1.Click += new EventHandler(LanguageChange);

                    break;
                case "Chinese":
                case "中国":
                case "중국어":
                case "Китайский":
                case "Chinois":
                    Properties.Settings.Default.Language = "zh-CN";
                    //cultureCode = 0x0411;	//ja-JP
                    //cultureCode = "ja-JP";	//ja-JP
                    //LanguageButton3.Click += new EventHandler(LanguageChange);
                    break;

                case "French":
                case "法国":
                case "프랑스어":
                case "Франция":
                    Properties.Settings.Default.Language = "fr";
                    //cultureCode = 0x0411;	//ja-JP
                    //cultureCode = "ja-JP";	//ja-JP
                    //LanguageButton3.Click += new EventHandler(LanguageChange);
                    break;

                case "Russia":
                case "Russe":
                case "Россия":
                case "러시아어":
                case "俄罗斯":
                    Properties.Settings.Default.Language = "ru-RU";
                    break;


                case "English":
                case "英语":
                case "영어":
                case "английский":
                case "Anglais":
                default:
                    Properties.Settings.Default.Language = "en-US";
                    //cultureCode = 0x0409;	//en-US
                    //cultureCode = "en-US";	//en-US
                    //LanguageButton2.Click += new EventHandler(LanguageChange);
                    break;

            }

            //Properties.Settings.Default.Language = lag.Text;
            Properties.Settings.Default.Save();

            // 여기는 TextManager에서 불러 오지 않고, 반듯이 영어로 표시 한다.
            MessageBox.Show(this, LanguageResources.MultiLanguage.ProgramReStart, LanguageResources.MultiLanguage.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
            this.Dispose();
            Application.Restart();
        }

        void Wizard_Click(object sender, EventArgs e)
        {
            if (!Wizard.WizardViewer.IsCreated)
            {
                Wizard.WizardViewer.Default.Disposed += new EventHandler(WizardViewer_Disposed);
            }

            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            this.Location = new Point(0, this.Location.Y);
            Wizard.WizardViewer.Default.WizardPath = (string)tsmi.Tag;
            Wizard.WizardViewer.Default.Location = new Point(this.Bounds.Right, this.Bounds.Top);

            int screenWidth = Screen.FromControl(this).WorkingArea.Width;
            int wvWidth;

            if (screenWidth > this.Bounds.Width + 200)
            {
                wvWidth = Screen.FromControl(this).WorkingArea.Width - this.Bounds.Width;
            }
            else
            {
                wvWidth = Wizard.WizardViewer.Default.Size.Width;
            }

            lock (wizardConditionCache)
            {
                wizardConditionCache.Clear();
            }

            Wizard.WizardViewer.Default.Size = new Size(wvWidth, this.Height);
            if (!Wizard.WizardViewer.Default.Visible) { Wizard.WizardViewer.Default.Show(this); }
        }

        void WizardViewer_Disposed(object sender, EventArgs e)
        {
            WizardEmphasisClear();
        }

        public void m_MainMenuStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ModifierKeys & Keys.Shift) == Keys.Shift) &&
                ((ModifierKeys & Keys.Alt) == Keys.Alt))
            {


                m_MenuConfigMicroscope.Visible = true;
                m_MenuConfigScanning.Visible = true;
                ImageTab.Visible = true;
                

                //this.Invalidate();
            }
        }

        void ColumnConfig_ProfileListChanged(object sender, EventArgs e)
        {
            (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).SettingChanged();
            ColumnListChange();
        }

        void ColumnConfig_HVtextChanged(object sender, EventArgs e)
        {
            ppSingle.MicronEghv = column.HVtext;
        }

        private void CameraStateChange(bool state)
        {
            if (state)
            {
                ((SECtype.IControlBool)column["CameraPower"]).Value = true;

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.Camera;
            }
            else
            {
                ((SECtype.IControlBool)column["CameraPower"]).Value = false;

                
            }
        }

        private void toolFilePrint_MouseUp(object sender, MouseEventArgs e)
        {
           
            //if (e.Button == MouseButtons.Right)
            //{
            //    string mode = toolFilePrint.Tag as string;
            //    if (mode == "File")
            //    {
            //        global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.ExportMode = "Print";
            //        toolFilePrint.Image = Properties.Resources.Printer;
            //    }
            //    else
            //    {
            //        global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.ExportMode = "File";
            //        toolFilePrint.Image = Properties.Resources.HardDrive;
            //    }
            //}
        }

        private string[] SystemInfoData = null;

        
        private void Export_Click(object sender, EventArgs e)
        {

            
            
            string mode;

            if (sender is Control)
            {
                Control bb = sender as Control;
                mode = bb.Tag as string;
            }
            else if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
                mode = tsmi.Tag as string;
                switch (mode)
                {
                    case "MENU_TOOLS_Manager":
                        mode = "Manager";
                        break;
                    case "MENU_TOOLS_Print":
                        mode = "Print";
                        break;
                    case "MENU_TOOLS_File":
                        mode = "File";
                        break;
                    default:
                        if (SystemInfoBinder.Default.AppMode == AppModeEnum.Debug)
                        {
                            throw new ArgumentException("정의 되지 않은 Export mode");
                        }
                        else
                        {
                            mode = "File";	// 발생 하면 안되지만, 만약을 위해서.
                            break;
                        }
                }
            }
            else
            {
                if (SystemInfoBinder.Default.AppMode == AppModeEnum.Debug)
                {
                    throw new ArgumentException("정의 되지 않은 UI type");
                }
                else
                {
                    mode = "File";	// 발생 하면 안되지만, 만약을 위해서.
                }
            }
            ImageExportManager.ImageExportModeEnum iee;

            switch (mode)
            {
                case "Print":
                    iee = ImageExportManager.ImageExportModeEnum.Print;
                    break;
                case "File":
                    iee = ImageExportManager.ImageExportModeEnum.File;
                    break;
                case "Manager":
                    iee = ImageExportManager.ImageExportModeEnum.Manager;
                    break;
                default:
                    throw new ArgumentException("정의 되지 않은 Export mode");
            }

            bool scanning;

            if (scanner.ItemsRunning == null)
            {
                scanning = false;
            }
            else
            {
                if (scanner.ItemsRunning[0].Setting.Name == "Scan Pause") { scanning = false; }
                else { scanning = true; }
            }



            string ImageSaveFileName = null;

            ImageSaveFileName = ImageAutoName(ImageSaveFileName);


            
            ImageExportManager.ImageExport(ppSingle, iee, !scanning, ImageSaveFileName);

            
            
            string filename = ImageExportManager.SystemInfoFileName;

            if (filename != null)
            {
                WriteSystemInfo(filename);
                ArchivesListView.Clear();
            }
            

            toolFilePrint.Checked = false;
            ArchivesCapture.Checked = false;


            //Properties.Settings.Default.ArchAdress = ArchivesAdress.Text;

            ArchivesAdressChange(sender, e);
              
            
        }

        private string ImageAutoName(string ATfileName)
        {
            if (Properties.Settings.Default.MicronCompany)
            {
                ATfileName += Properties.Settings.Default.CompanyText;
                ATfileName += "_";
            }

            if (Properties.Settings.Default.MicronVoltage)
            {
                ATfileName += column.HVtext;
                ATfileName += "_";
            }

            if (Properties.Settings.Default.MicronDetector)
            {
                if(DetectorSE.Checked)
                {
                    ATfileName += "SE";
                }
                else if (DetectorBSE.Checked)
                {
                    ATfileName += "BSE";
                }
                else if (DetectorDual.Checked)
                {
                    ATfileName += "Dual";
                }
                else
                {
                    ATfileName += "Merge";
                }

                ATfileName += "_";
                
            }

            if (Properties.Settings.Default.MicronVacuum)
            {
                if (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum)
                {
                    ATfileName += "HighVac";
                    
                }
                else
                {

                    ATfileName += "LowVac";
                }

                ATfileName += "_";


            }

            if (Properties.Settings.Default.MicronMag)
            {
                ATfileName += frMainMagDisLab.Text;
                ATfileName += "_";
            }

            //if (Properties.Settings.Default.MicronDate)
            //{
            //    ATfileName += ppSingle.Date;
            //    ATfileName += "_";
            //}

            if (Properties.Settings.Default.DescriptorText != "")
            {
                ATfileName += Properties.Settings.Default.DescriptorText;
                //ATfileName += "_";
            }



            return ATfileName;
        }

        public void WriteSystemInfo(string fileName)
        {
            using (StreamWriter sw = File.CreateText(fileName + ".txt"))
            {
                sw.WriteLine("[Image Info]");
                sw.WriteLine("Model : " + ModelLab.Text);
                sw.WriteLine("Date : " + DateTime.Now.ToString());
                sw.WriteLine("Time : " + DateTime.Now.TimeOfDay.ToString());
                sw.WriteLine("File Name : " + fileName);
                sw.WriteLine("Discription : " + /*m_InfoDesc.Text);*/InfomationLab.Text);
                //sw.WriteLine("Image Size : " + ppSingle.Size.ToString());
                sw.WriteLine("Acceleated Voltage : " + /*m_InfoEghv.Text);*/ppSingle.MicronEghv);
                sw.WriteLine("Magnification : " + frMainMagDisLab.Text);
                sw.WriteLine("Emission Current : " + EmissionDisplayLabel.Text);
                switch (SystemInfoBinder.Default.DetectorMode)
                {
                    case SystemInfoBinder.ImageSourceEnum.BSED:
                        sw.WriteLine("Detector Mode : BSED");
                        break;
                    case SystemInfoBinder.ImageSourceEnum.SED:
                        sw.WriteLine("Detector Mode : SED");
                        break;
                }
                switch (SystemInfoBinder.Default.VacuumMode)
                {
                    case SystemInfoBinder.VacuumModeEnum.HighVacuum:
                        sw.WriteLine("Vacuum Mode : High Vacuum");
                        break;
                    case SystemInfoBinder.VacuumModeEnum.LowVacuum:
                        sw.WriteLine("Vacuum Mode : Low Vacuum");
                        break;
                }
                if (frMainScanPauseBb.Checked)
                {
                    sw.WriteLine("Scan Speed : ");
                }
                else if (frMainScanSfBb.Checked)
                {
                    sw.WriteLine("Scan Speed : Fast Scan");
                }
                else if (frMainScanSsBb.Checked)
                {
                    sw.WriteLine("Scan Speed : Slow Scan");
                }
                else if (frMainScanPfBb.Checked)
                {
                    sw.WriteLine("Scan Speed : Fast Photo");
                }
                else if (frMainScanPsBb.Checked)
                {
                    sw.WriteLine("Scan Speed : Slow Photo");
                }

                sw.WriteLine("[Running Info]");
                sw.WriteLine("Focus : " + frMainFocusKnobEkwicvd.Value.ToString());
                sw.WriteLine("Magnification : " + frMainMagDisLab.Text);
                sw.WriteLine("Scan Rotaion : " + frMainRotateDisLab.Text);
                sw.WriteLine("[   - Image Process]");
                sw.WriteLine("Brightness : " + SystemInfoBinder.Default.Brightness.ToString());
                sw.WriteLine("Contrast : " + SystemInfoBinder.Default.Contrast.ToString());
                //sw.WriteLine("Frame Average : " + m_MicroscopeProfile.FilterCnt.ToString());
                sw.WriteLine("[   - Detector]");
                sw.WriteLine("Detector : " + detectPmt.Value.ToString());
                sw.WriteLine("Collector : " + detectClt.Value.ToString());
                sw.WriteLine("[   - Spot Size]");
                sw.WriteLine("Spot Size : " + ss2CL1.Value.ToString());
                sw.WriteLine("[   - Working Distance]");
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);
                
                int defaultWD =0;
                switch(SystemInfoBinder.Default.AppDevice)
                {
                    case AppDeviceEnum.SNE4500M:
                    case AppDeviceEnum.SNE4000M:
                        defaultWD = 3;
                        break;
                    case AppDeviceEnum.SNE3200M:
                    case AppDeviceEnum.SNE3000M:
                    case AppDeviceEnum.SNE3000MB:
                    case AppDeviceEnum.SNE3000MS:
                        defaultWD = 8;
                        break;
                }

                sw.WriteLine("Default Working Distance : " + defaultWD.ToString() + "mm");
                sw.WriteLine("Working Distance : " + iwdsob.WorkingDistance.ToString() + "mm");
                sw.WriteLine("[   - Stig]");
                sw.WriteLine("Stig X : " + StgXValue.Text);
                sw.WriteLine("Stig Y : " + SigYValue.Text);
                sw.WriteLine("[   - Gun Align]");
                sw.WriteLine("GA X : " + GunX.Text);
                sw.WriteLine("GA Y : " + GunX.Text);
                sw.WriteLine("[   - Beam Shift]");
                sw.WriteLine("BS X : " + bsXValue.Text);
                sw.WriteLine("BS Y : " + bsXValue.Text);

                sw.WriteLine("[Config Info]");
                sw.WriteLine("Alias : " + ppSingle.MicronEghv);
                sw.WriteLine("[   - Accelerated Voltage]");
                int gunValue = (int)(equip.ColumnHVGun.Value * 255);
                sw.WriteLine("Anode : " + gunValue.ToString());
                sw.WriteLine("Filament : " + alignHVF.Value.ToString());

                object[] Tipcu = ((SECcolumn.IColumnValue)column["HvFilament"]).Read;

                if (Tipcu[0] == null)
                {
                    Tipcu[0] = 0;
                }

                sw.WriteLine("Filament Current: " + Tipcu[0].ToString());
                sw.WriteLine("Grid : " + alignHVG.Value.ToString());

                sw.WriteLine("[   - CL]");
                sw.WriteLine("CL1 Val : " + ss2CL1.Value.ToString());
                sw.WriteLine("CL1 Max : " + ss2CL1.Maximum.ToString());
                sw.WriteLine("CL1 Min : " + ss2CL1.Minimum.ToString());
                //sw.WriteLine("CL1 Inv : " + m_MicroscopeProfile.Con1Invert.ToString());
                sw.WriteLine("CL2 Val : " + ss2CL2.Value.ToString());
                sw.WriteLine("CL2 Max : " + ss2CL2.Maximum.ToString());
                sw.WriteLine("CL2 Min : " + ss2CL2.Minimum.ToString());
                //sw.WriteLine("CL2 Inv : " + m_MicroscopeProfile.Con2Invert.ToString());

                sw.WriteLine("[   - Focus]");
                sw.WriteLine("OL1 Val : " + frMainFocusKnobEkwicvd.Value.ToString());
                sw.WriteLine("OL1 Max : " + frMainFocusKnobEkwicvd.Maximum.ToString());
                sw.WriteLine("OL1 Min : " + frMainFocusKnobEkwicvd.Minimum.ToString());
                sw.WriteLine("OL2 Val : " + frMainFocusWdLswis.Value.ToString());
                sw.WriteLine("OL2 Max : " + frMainFocusWdLswis.Maximum.ToString());
                sw.WriteLine("OL2 Min : " + frMainFocusWdLswis.Minimum.ToString());
                //sw.WriteLine("OL Inv : " + m_MicroscopeProfile.Obj1Invert.ToString());

                sw.WriteLine("[   - Stig]");

                //SystemInfoBinder.Default.Equip.
                int stigxab = (int)(equip.ColumnStigXAB.Value * 5000);
                sw.WriteLine("XAB : " + stigxab.ToString());

                int stigxcd = (int)(equip.ColumnStigXCD.Value * 5000);
                sw.WriteLine("XCD : " + stigxcd.ToString());

                sw.WriteLine("XFreq : " + equip.ColumnStigXWF.Value.ToString());
                sw.WriteLine("XAmpl : " + equip.ColumnStigXWA.Value.ToString());
                sw.WriteLine("XWobble : " + equip.ColumnStigXWE.Value.ToString());

                int stigYAB = (int)(equip.ColumnStigYAB.Value * 5000);
                sw.WriteLine("YAB : " + stigYAB.ToString());

                int stigYCD = (int)(equip.ColumnStigYCD.Value * 5000);
                sw.WriteLine("YCD : " + stigYCD.ToString());

                sw.WriteLine("YFreq : " + equip.ColumnStigYWF.Value.ToString());
                sw.WriteLine("YAmpl : " + equip.ColumnStigYWA.Value.ToString());
                sw.WriteLine("YWobble : " + equip.ColumnStigYWE.Value.ToString());

                //sw.WriteLine("[   - Detector]");
                //sw.WriteLine("EgpsPmtDefault : " + equip.ColumnHVPMT.Value.ToString());
                //sw.WriteLine("EgpsCltDefault : " + equip.ColumnHVCLT.Value.ToString());

                sw.WriteLine("[   - Rotation]");
                object[,] WDTable = iwdsob.TableGet();
                int num = 0;

                if (WDTable.Length > 1)
                {
                    switch (iwdsob.WorkingDistance)
                    {
                        case 0:
                            num = 4;
                            break;

                        case 5:
                            num = 3;
                            break;

                        case 10:
                            num = 2;
                            break;

                        case 20:
                            num = 1;
                            break;

                        default:
                            num = 0;
                            break;
                    }
                }
                else
                {
                    num = 0;
                }

                if (WDTable.Length > 5)
                {
                    sw.WriteLine("Scan Rotation : " + WDTable[num, 3]);
                    sw.WriteLine("BeamShift Rotation : " + WDTable[num, 4]);
                }
               
                //sw.WriteLine("GunAlign Rotation : " + m_MicroscopeProfile.GunAlignRotation.ToString());

                //double sizegap = (double)1280 / ppSingle.Size.Width;
                //double pixelsize = ppSingle.LengthPerPixel * 1000000 / sizegap;
               

                SEC.Nanoeye.NanoImage.IScanItemEvent[] runList = SystemInfoBinder.Default.Nanoeye.Scanner.ItemsReady;

                if (runList == null)
                {
                    runList = SystemInfoBinder.Default.Nanoeye.Scanner.ItemsRunning;
                }

                if (runList != null)
                {

                    SEC.Nanoeye.NanoImage.IScanItemEvent isie = runList[0];

                    sw.WriteLine("[Active Scan]");
                    sw.WriteLine("Channel : " + isie.Setting.AiChannel.ToString());
                    sw.WriteLine("Bipolra : " + isie.Setting.AiDifferential.ToString());
                    sw.WriteLine("Sampling Speed : " + isie.Setting.FramePerSecond.ToString());
                    sw.WriteLine("Image Level : " + isie.Setting.AiMaximum.ToString() + "," + isie.Setting.AiMinimum.ToString());
                    sw.WriteLine("Sample Average : " + isie.Setting.SampleComposite.ToString());
                    sw.WriteLine("Scan Signal Level : " + isie.Setting.AoMaximum.ToString() + "," + isie.Setting.AoMinimum.ToString());
                }

                if (runList != null)
                {
                    SEC.Nanoeye.NanoImage.IScanItemEvent isie = runList[0];

                    sw.WriteLine("Frame Average : " + isie.Setting.LineAverage.ToString());
                    sw.WriteLine("Blur Level : " + isie.Setting.BlurLevel.ToString());
                    sw.WriteLine("Image Size : " + isie.Setting.ImageWidth.ToString() + "," + isie.Setting.ImageHeight.ToString());
                    sw.WriteLine("Paint Area : " + isie.Setting.PaintWidth.ToString() + "," + isie.Setting.PaintHeight.ToString());
                    sw.WriteLine("ImmediatePaint : " + isie.Setting.IsModifialble.ToString());
                    sw.WriteLine("Frame Size : " + isie.Setting.FrameWidth.ToString() + "," +isie.Setting.FrameHeight.ToString());
                    sw.WriteLine("Image Area : " + isie.Setting.PaintX.ToString() + "," + isie.Setting.PaintY.ToString() + "," + isie.Setting.PaintWidth.ToString() + "," + isie.Setting.PaintHeight.ToString());


                    double sizegap = (double)isie.Setting.ImageWidth / ppSingle.Size.Width;
                    double pixelsize = ppSingle.LengthPerPixel * 1000000 / sizegap;
                    sw.WriteLine("Pixel Size : " + pixelsize.ToString());


                }


                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

            

            
            
        }


        private void frMainFocusAutoBb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Control con = sender as Control;

                switch ((string)con.Tag)
                {
                    case "AF-Fine":
                        con.Text = "Auto";
                        con.ForeColor = Color.FromArgb(188, 188, 188);
                        con.Tag = "AF-WD";
                        //con.Text.color
                        break;
                    default:
                    case "AF-WD":
                        con.Text = "Fine";
                        con.ForeColor = Color.FromArgb(188, 188, 188);
                        con.Tag = "AF-Fine";
                        break;
                }
            }
        }

        private void AutoFunction_Click(object sender, EventArgs e)
        {

            
           
            //Control con = sender as Control;
            BitmapCheckBox con = sender as BitmapCheckBox;

            SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

            if (con.Name == "frMainFocusAFBb")
            {
                con.Tag = "AF-Fine";
            }
            

            switch ((string)con.Tag)
            {
                case "Video":

                   
                    if (frMainScanPauseBb.Checked)
                    {
                        videoAuto.Checked = false;
                        return;
                    }

                    //scanner.ItemsReady(
                    //ScanModeChange(ScanModeEnum.FastScan); 
                    string scanName = "FaseScan"; 
                    if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.Merge || SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.DualSEBSE)
                    {
                        scanName = scanner.ItemsRunning[0].Name;

                        frMainScanSfBb.PerformClick();
                        Delay(500);
                    }

                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

                    if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.Merge || SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.DualSEBSE)
                    {
                        switch (scanName)
                        {
                            case "Fast Scan":
                                frMainScanSfBb.PerformClick();
                                break;

                            case "Slow Scan":
                                frMainScanSsBb.PerformClick();
                                break;

                            case "Fast Photo":
                                frMainScanPfBb.PerformClick();
                                break;

                            case "Slow Photo":
                                frMainScanPsBb.PerformClick();
                                break;

                            case "Slow Photo2":
                                frMainScanPsBb2.PerformClick();
                                break;
                        }

                        
                    }

                    
                    //videoAuto.Checked = false;
                    videoAuto.Checked = false;
                    break;
                case "AF-Fine":

                    if (frMainScanPauseBb.Checked)
                    {
                        frMainFocusAFBb.Checked = false;
                        return;
                    }

                    if (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3200M && SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3000MS)
                    {
                        ((SECtype.IControlDouble)column["LensObjectCoarse"]).Value = ((((double)equip.ColumnLensOLC.Maximum - (double)equip.ColumnLensOLC.Minimum) / 2) + (double)equip.ColumnLensOLC.Minimum);
                    }


                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoFocusFine, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());
                    frMainFocusAFBb.Checked = false;

                    break;
                case "AF-WD":
                    if (frMainScanPauseBb.Checked)
                    {
                        frMainFocusAWBb.Checked = false;
                        return;
                    }

                    if (!FocusAutoBtn.Checked)
                    {
                        return;
                    }


                    int mag = UIsetBinder.Default.MagIndex;
                    bool enable = true;

                    while (enable)
                    {
                        if (equip.Magnification < 1000)
                        {
                            UIsetBinder.Default.MagIndex++;
                        }
                        else if (equip.Magnification > 1000)
                        {
                            UIsetBinder.Default.MagIndex--;
                        }
                        else
                        {
                            enable = false;
                        }
                    }


                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoFocusCoarse, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());
                    frMainFocusAWBb.Checked = false;
                    Delay(1000);
                    enable = true;
                    while (enable)
                    {
                        if (equip.Magnification < 10000)
                        {
                            UIsetBinder.Default.MagIndex++;
                        }
                        else if (equip.Magnification > 10000)
                        {
                            UIsetBinder.Default.MagIndex--;
                        }
                        else
                        {
                            enable = false;
                        }
                    }


                    if (frMainScanPauseBb.Checked)
                    {
                        frMainFocusAFBb.Checked = false;
                        return;
                    }


                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoFocusFine, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());
                    FocusAutoBtn.Checked = false;

                    UIsetBinder.Default.MagIndex = mag;


                    break;
                default:
                    //new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle);
                    
                    MessageBox.Show("정의되지 않은 자동화 기능");
                    break;
            }
        }



        private void ResetFunction_Click(object sender, EventArgs e)
        {
            BitmapCheckBox con = sender as BitmapCheckBox;

            if (!con.Checked)
            {
                return;
            }

            switch ((string)con.Tag)
            {
                case "Focus":

                    if (focusmodeDl.Visible)
                    {
                        //equip.ColumnLensOLC
                        //((SECtype.IControlDouble)column["LensObjectCoarse"]).Value = ((double)equip.ColumnLensCL1.Value);
                        ((SECtype.IControlDouble)column["LensObjectCoarse"]).Value = ((((double)equip.ColumnLensOLC.Maximum - (double)equip.ColumnLensOLC.Minimum) / 2) + (double)equip.ColumnLensOLC.Minimum);
                        
                    }
                    else
                    {


                        setManager.ColumnOneLoad(equip.ColumnLensOLF, ColumnOnevalueMode.Factory);
                    }

                    
                    break;
                case "StigAlign-X":
                    setManager.ColumnOneLoad(equip.ColumnStigXAB, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnStigXCD, ColumnOnevalueMode.Factory);
                    break;
                case "StigAlign-Y":
                    setManager.ColumnOneLoad(equip.ColumnStigYAB, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnStigYCD, ColumnOnevalueMode.Factory);
                    break;
                case "GunSetting":
                    setManager.ColumnOneLoad(equip.ColumnHVFilament, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnHVGrid, ColumnOnevalueMode.Factory);
                    alignHVR.Checked = false;
                    break;
                case "GunAlign":
                    setManager.ColumnOneLoad(equip.ColumnGAX, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnGAY, ColumnOnevalueMode.Factory);
                    alignGunR.Checked = false;
                    break;
                case "Detector":
                    setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);
                    detectorReset.Checked = false;
                    break;
                case "BeamShift":
                    setManager.ColumnOneLoad(equip.ColumnBSX, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnBSY, ColumnOnevalueMode.Factory);
                    bsReset.Checked = false;
                    break;
                case "CL-Ext":
                    setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.Factory);
                    ss3Reset.Checked = false;
                    //FocusAutoBtn.Checked = false;
                //bitmapMiddle.Checked = true;
                    //SpotSizeChange(sender, e);
                    break;
                case "Stig":
                    setManager.ColumnOneLoad(equip.ColumnStigXV, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnStigYV, ColumnOnevalueMode.Factory);
                    stigReset.Checked = false;
                    //FocusAutoBtn.Checked = false;
                    break;
                case "TiltCorrection":
                    setManager.ColumnOneLoad(equip.ColumnDynamicFocus, ColumnOnevalueMode.Factory);
                    break;

                case "Rotate":
                    (equip.ColumnScanRotation as SECtype.IControlDouble).Value = 0;
                    MagCount = 0;
                    frMainRotateDisLab.Text = ((int)(MagCount)).ToString() + "\x00B0";
                    frMainRotateReset.Checked = false;
                    break;
                default:
                    Trace.WriteLine("Undefined reset target. - " + con.Tag + "(" + con.Name + ")", "Error");
                    //MessageBox.Show("Undefined reset target.");
                    break;
            }
            
        }
        //#endregion

        #region Column, 고압  및 진공 관련
        #region Column Event
        void Controller_CommunicationErrorOccured(object sender, SEC.GenericSupport.DataType.CommunicationErrorOccuredEventArgs e)
        {
            communicationErrorCount++;
            Trace.WriteLine("Coomunication Error Count - " + communicationErrorCount.ToString(), "Warring");
        }

        string preVacuumMode = "None";
        void VacuumMode_RepeatUpdated(object sender, object[] value)
        {
            string mode = value[0] as string;
            if (preVacuumMode != mode)
            {
                Trace.WriteLine("VacuumMode changed from " + preVacuumMode + " to " + mode, "Info");

                switch (mode)
                {
                    case "HighVacuum":
                        if (SystemInfoBinder.Default.VacuumMode != SystemInfoBinder.VacuumModeEnum.HighVacuum)
                        {
                            //lowVacuumToolStripMenuItem.Checked = false;
                            //hightVacuumToolStripMenuItem.Checked = true;
                            SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum;

                            MessageBox.Show(this, TextManager.Instance.GetString("Message_HighVacuum").Text);
                        }
                        break;
                    case "LowVacuum":
                        if (SystemInfoBinder.Default.VacuumMode != SystemInfoBinder.VacuumModeEnum.LowVacuum)
                        {
                            //lowVacuumToolStripMenuItem.Checked = true;
                            //hightVacuumToolStripMenuItem.Checked = false;
                            SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.LowVacuum;

                            MessageBox.Show(this, TextManager.Instance.GetString("Message_LowVacuum").Text);
                        }
                        break;
                    default:
                        MessageBox.Show(this, "Undefine Vacuum Mode");
                        break;
                }

                preVacuumMode = mode;
            }
        }

        void m_NanoView_CommunicationErrorOccured(object sender, SECtype.CommunicationErrorOccuredEventArgs e)
        {
            //MessageBox.Show(this, "Communication Error Occured." + e.ErrorValue.Name);
            Trace.WriteLine("Communication Error Occured at " + e.ErrorValue.Name, "Communication Error");
        }

        string preEmissionStr = "000uA";
        int count = 0;
        void HvElectronGun_RepeatUpdated(object sender, object[] value)
        {
            double emmition = (double)value[0];
            
            string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
            strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 2, false, 'A');
            //Trace.WriteLine("Emission default : " + emmition.ToString());
            
           
         

            string result = "";

            if (preEmissionStr != strNew || StSystemInfoBtn.Checked)
            {
                Action act = () =>
                {
                    //if (StSystemInfoBtn.Checked)
                    //{
                    result += Properties.Settings.Default.FilamentRunningTime.Days.ToString() + "days ";
                    result += Properties.Settings.Default.FilamentRunningTime.Hours.ToString().PadLeft(2, '0') + ":";
                    result += Properties.Settings.Default.FilamentRunningTime.Minutes.ToString().PadLeft(2, '0');
                    FilamentRunTimeLabel.Text = result;
                    //FilamentRunTimeLabel.Invalidate();

                    //}

                   

                    //Trace.WriteLine(emmition.ToString(), "Emmision");
                    Trace.WriteLine(string.Format("Emission - {0}", strNew), "Info");
                    EmissionDisplayLabel.Text = strNew;


                    //FilamentRunTimeLabel.Text = Properties.Settings.Default.FilamentRunningTime.ToString();
                    if (operation != null)
                    {
                        operation.EmissionStr = strNew;
                    }

                    preEmissionStr = strNew;

                    //object[] FilamntNew = ((SECcolumn.IColumnValue)column["HvFilament"]).Read;
                    //double defaultFilament = 4000;

                    //Trace.WriteLine(FilamntNew[0].ToString(), "FilamentNew[0]");
                    //Trace.WriteLine(FilamntNew[1].ToString(), "FilamentNew[1]");


                    //if ((double)FilamntNew[1] <= defaultFilament && FilamentCheckedStartEnable)
                    //{
                    //    MessageBox.Show("Filament disconnection");
                    //}


                    if (emmition <= 80 && FilamentCheckedStartEnable && m_ToolStartup.Checked == true)
                    {
                        FilamentCheck();
                    }
                    //SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

                    //((SECcolumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Tip_CheckUpdated);


                    
                };

               

                this.BeginInvoke(act);
            }
        }


        void FilamentCheck()
        {
           

            object[] FilamentNew = ((SECcolumn.IColumnValue)column["HvFilament"]).Read;

            Trace.WriteLine(FilamentNew[0].ToString(), "FilamentNew[0]");
            Trace.WriteLine(FilamentNew[1].ToString(), "FilamentNew[1]");

            if ((double)FilamentNew[1] < 40000 && FilamentCheckedStartEnable)
            {
                FilamentCheckedStartEnable = false;
                MessageBox.Show("Filament disconnection");
            }

        }
        
        void Tip_CheckUpdated(object sender, object[] value)
        {
            double data = (double)value[1];
    
            if (data <= 600)
            {
                MessageBox.Show("Filament has been disconnected.");
            }

        }

        delegate void ToolStartupTimeHandler();
        void StrtNewThread()
        {
            while (true)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ToolStartupTimeHandler(ToolStartup));
                    break;
                }
                else
                {
                    ToolStartup();

                }
                System.Threading.Thread.Sleep(5000);

            }
        }

        void ToolStartup()
        {
            m_ToolStartup.Enabled = true;


        }



        //delegate void ct_test();

        string preVacuumState = "None";
        bool enable = false;

        void VacuumState_RepeatUpdated(object sender, object[] value)
        {
            //Trace.WriteLine("State chage: " + ((SECtype.IControlInt)column["VacuumMode"]).Value.ToString());

            string state = (string)(value[0]);
            //Trace.WriteLine("state - " + state, "Info");

            


            if (preVacuumState != state)
            {
                if (SystemInfoBinder.Default.AppMode == AppModeEnum.Run)
                {
                    Trace.WriteLine("VacuumState changed from " + preVacuumState + " to " + state, "Info");

                    //if (state =)
                    //{
                    //    //m_ToolStartup.Checked = false;

                    //    return;
                    //}

                    //state = "Ready";
                    if (state == "Ready")
                    {
                        enable = true;
                        if (operation == null)
                        {
                            StrtNewThread();

                        }
                        else
                        {
                            StrtEnableThread();
                            //operation.m_ToolsStartupEnable(true);
                        }



                        //ct_test t = delegate() { m_ToolStartup.Enabled = true; };
                        //m_ToolStartup.Enabled = true;
                    }
                    else
                    {
                        if (m_ToolStartup.Checked)
                        {
                            //operation.Close();

                            //ScanModeChange(ScanModeEnum.ScanPause);
                            StrtCheckedEnableThread();
                            //m_ToolStartup.Checked = false;



                            uint val = 0xffffffff;
                            try
                            {
                                val = (uint)((equip.ColumnVacuumLastError as SECtype.IControlInt).Value);
                                Trace.WriteLine("Vacuum last error - 0x" + val.ToString("X"), "Info");
                            }
                            catch (Exception)
                            {
                                Trace.WriteLine("Controller is not support to get Vacuum_Last_Error.", "Warring");
                            }

                            MessageBox.Show(this, "Venting - " + val.ToString("X"), "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            try
                            {
                                val = (uint)((equip.ColumnVacuumResetCode as SECtype.IControlInt).Value);
                                Trace.WriteLine("Vacuum reset code - 0x" + val.ToString("X"), "Info");
                            }
                            catch (Exception)
                            {
                                Trace.WriteLine("Controller is not support to get Vacuum_Reset_Code.", "Warring");
                            }


                        }
                        else
                        {
                            enable = false;
                            StrtEnableThread();
                            //StartupEnable(false);
                        }




                        //m_ToolStartup.Enabled = false;
                        //operation.m_ToolsStartupEnable(false);

                    }
                }
                else
                {
                    m_ToolStartup.Enabled = true;
                }

                //m_ToolStartup.Enabled = true;
                preVacuumState = state;
            }
        }

        delegate void StartupEnableTimeHandler();
        void StrtEnableThread()
        {
            while (true)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new StartupEnableTimeHandler(StartupEnable));
                    break;
                }
                else
                {
                    StartupEnable();

                }
                System.Threading.Thread.Sleep(5000);

            }
        }

        void StartupEnable()
        {
            if (operation == null) { return; }

            if (enable)
            {
                m_ToolStartup.Enabled = true;
                operation.m_ToolsStartupEnable(true);
            }
            else
            {
                m_ToolStartup.Enabled = false;
                operation.m_ToolsStartupEnable(false);
            }

        }

        delegate void StartupCheckedEnableTimeHandler();
        void StrtCheckedEnableThread()
        {
            while (true)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new StartupCheckedEnableTimeHandler(StartupChecekEnable));
                    break;
                }
                else
                {
                    StartupChecekEnable();

                }
                System.Threading.Thread.Sleep(5000);

            }
        }

        void StartupChecekEnable()
        {
            if (operation == null)
            {
                m_ToolStartup.Checked = false;
                m_ToolStartup.Enabled = false;
            }
            else
            {
                ScanModeChange(ScanModeEnum.ScanPause);
                operation.ToolStartupCheckedEnable(false);
            }

            //m_ToolStartup.Checked = false;

        }
        #endregion

        #region Column





        void m_FormConfigMicroscope_ProfileListChanged(object sender, EventArgs e)
        {
            ColumnListChange();
        }



        private void ColumnListChange()
        {

            ProfileBtn.Items.Clear();
            this.tsmnuSetup.DropDownItems.Clear();
            string[] colList = SystemInfoBinder.Default.SetManager.ColumnList();

            foreach (string str in colList)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();

                tsmi.Text = str;
                tsmi.Click += new EventHandler(MicroscopeProfileMenu_Click);


                ProfileBtn.Items.Add(str);


                //operation.ProfileChange(str);

                //ProfileBtn.SelectionChangeCommitted += new EventHandler(MicroscopeProfileMenu_Click);

                //ProfileBtn.Value = 
                //ProfileBtn.Click += new EventHandler(MicroscopeProfileMenu_Click);
                tsmnuSetup.DropDownItems.Add(tsmi);
            }
            if ((int)Properties.Settings.Default.SelectedProfile >= colList.Length)
            {
                Properties.Settings.Default.SelectedProfile = colList.Length - 1;
            }
            Trace.WriteLine("Profile Number" + Properties.Settings.Default.SelectedProfile.ToString());

            if (Properties.Settings.Default.SelectedProfile < 0)
            {
                Properties.Settings.Default.SelectedProfile = 0;
            }


            //ProfileBtn.Items[(int)Properties.Settings.Default.SelectedProfile].;
            //this.t
            ProfileBtn.Text = this.tsmnuSetup.DropDownItems[(int)Properties.Settings.Default.SelectedProfile].Text;

            if (ProfileBtn.Text == "NA") { return; }

            this.tsmnuSetup.DropDownItems[(int)Properties.Settings.Default.SelectedProfile].PerformClick();



            //ProfileBnt.Text = profileList1.Text;
            //ProfileBnt.ForeColor = Color.FromArgb(50, 235, 251);


        }

        public void profileChange(object sender, EventArgs e)
        {
            UltraComboEditor clickitem = sender as UltraComboEditor;
            if (clickitem.SelectedIndex < 0)
            {
                return;
            }

            this.tsmnuSetup.DropDownItems[clickitem.SelectedIndex].PerformClick();

            //ColumnListChange();
            ProfileBtn.Text = this.tsmnuSetup.DropDownItems[clickitem.SelectedIndex].Text;
            ProfileBtn.Appearance.ImageBackground = Properties.Resources.icon02_01;
            EmissionDisplayLabel.Focus(); 

            ControlDefaultSetting();
            //DetectorSE.Focus();
            
        }


        /// <summary>
        /// 현미경 설정 메뉴 이벤트 처리기입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void MicroscopeProfileMenu_Click(object sender, EventArgs e)
        {
            // 고압이 켜진 상태에서는 설정을 변경 할 수 없다.
            if (m_ToolStartup.Checked) { return; }

            // 프로그램 시작시에는 column의 값이 불러와 있지 않다.
            if (column.Name != null)
            {
                setManager.ColumnSave(column, ColumnOnevalueMode.Run);
            }


            //Infragistics.Win.UltraWinEditors.UltraComboEditor profile = sender as Infragistics.Win.UltraWinEditors.UltraComboEditor;
            //column.Name = profile.SelectedText;
            string prePofileName = column.Name;


            // 설정 창이 보여지고 있는 상태에서는 설정을 저장 해야 한다.
            if (sfMicroscope.IsCreated)
            {
                (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).SettingSave();
            }


            //Infragistics.Win.UltraWinEditors.UltraComboEditor clickedItem = sender as Infragistics.Win.UltraWinEditors.UltraComboEditor;
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            //Profile profile

            //Profile profile = (Profile)brb.Text;

            int i = 0;




            foreach (ToolStripMenuItem item in this.tsmnuSetup.DropDownItems)
            //foreach (Infragistics.Win.UltraWinEditors.UltraComboEditor item in this.tsmnuSetup.DropDownItems)
            {
                if (clickedItem == item)
                {
                    //item.CheckState = CheckState.Checked;

                    SystemInfoBinder.Default.SetManager.ColumnLoad(item.Text, column, ColumnOnevalueMode.Run);

                    //InfoUpdate();

                    Properties.Settings.Default.SelectedProfile = i;
                }
                else
                {
                    item.CheckState = CheckState.Unchecked;
                }
                i++;
            }

            ppSingle.MicronEghv = column.HVtext;

            if (sfMicroscope.IsCreated)
            {
                (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).SettingChanged();
            }

            //SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE4500M;
            switch (SystemInfoBinder.Default.AppDevice)
            {
                case AppDeviceEnum.SNE1500M:
                case AppDeviceEnum.SNE3000M:
                    //ss1Spot1.Checked = false;
                    //ss1Spot1.Checked = true;
                    break;
                case AppDeviceEnum.SNE4000M:
                case AppDeviceEnum.SNE4500P:
                    break;
                case AppDeviceEnum.AutoDetect:
                    throw new InvalidOperationException();
            }

            UIsetBinder.Default.MagMaximum = equip.MagLenghtGet();
            MagTrakBar.Maximum = UIsetBinder.Default.MagMaximum;
            UIsetBinder.Default.MagIndex = 1;
            MagTrakBar.Value = UIsetBinder.Default.MagIndex;
            UIsetBinder.Default.MagIndex = 0;
            MagTrakBar.Minimum = UIsetBinder.Default.MagIndex;

            Trace.WriteLine("Profile changed from + " + prePofileName + " to " + column.Name, "Info");
        }
        #endregion

        #region Rotation
        private void RotateLevel_ValueChanged(object sender, EventArgs e)
        {

        }

        private double MagCount = 0;

        private void RotateLevel_Click(object sender, EventArgs e)
        {
            

            if (sender == frMainRotateIncBb)
            {
                double val = (equip.ColumnScanRotation as SECtype.IControlDouble).Value;
                val++;
                MagCount++;
                if (val > 180) { val += 360; }

                if (MagCount > 360) { MagCount = 1; }
                

                (equip.ColumnScanRotation as SECtype.IControlDouble).Value = (val);
                frMainRotateDisLab.Text = ((int)(MagCount)).ToString() + "\x00B0";

            }
            else if (sender == frMainRotateDecBb)
            {
                double val = (equip.ColumnScanRotation as SECtype.IControlDouble).Value;
                val--;
                MagCount--;
                if (val < -180) { val -= 360; }

                if (MagCount < 0) { MagCount = 359; }

                (equip.ColumnScanRotation as SECtype.IControlDouble).Value = (val);

                frMainRotateDisLab.Text = ((int)(MagCount)).ToString() + "\x00B0";
            }
            else
            {
                throw new ArgumentException();
            }
        }
        #endregion

        #region VacuumMode & Detector Mode
        public void VacuumStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            BitmapCheckBox tsmi = sender as BitmapCheckBox;
            if (!tsmi.Checked)
            {
                //tsmi.
                return;
            }


           
            switch (tsmi.Name)
            {
                case "LowVac":

                    //저진공 모드에서는 BSED만 사용 가능.
                    //if (!backScatteredElectronToolStripMenuItem.Checked)
                    //{
                    //    backScatteredElectronToolStripMenuItem.PerformClick();
                    //}
                    //secondaryElectronToolStripMenuItem.Enabled = false;

                    if (DetectorSE.Checked) 
                    {
                        tsmi.Checked = false;
                        return; 
                    }
                    LowVac.Checked = true;
                    SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.LowVacuum;
                    //hightVacuumToolStripMenuItem.Checked = false;
                    //lowVacuumToolStripMenuItem.Checked = true;

                    // 저진공 모드에서는 벨브 상태 요청.
                    //if ((SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE1500M) || (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3200M))
                    //{
                    ((SECtype.IControlInt)column["VacuumMode"]).Value = 1;
                    //}
                    HighVac.Checked = false;
                    DetectorSE.Enabled = false;
                    DetectorDual.Enabled = false;
                    DetectorMerge.Enabled = false;

                    break;
                case "HighVac":
                    //hightVacuumToolStripMenuItem.Checked = true;
                    //lowVacuumToolStripMenuItem.Checked = false;
                    //secondaryElectronToolStripMenuItem.Enabled = true;

                    if (ModelLab.Text == "SNE-3000MB")
                    {
                        DetectorSE.Enabled = false;
                        DetectorDual.Enabled = false;
                        DetectorMerge.Enabled = false;
                    }
                    else
                    {
                        DetectorSE.Enabled = true;
                        DetectorDual.Enabled = true;
                        DetectorMerge.Enabled = true;
                    }
                   

                    

                    if (m_ToolStartup.Checked)
                    {
                        // 고압이 켜진 상태에서는 가속전압등을 낮췄다 높이는 작업을 함.

                        highvacuumTimer = new System.Threading.Timer(new System.Threading.TimerCallback(HighVacuumTimerExpiered));
                        highVacTimer = 0;
                        highvacuumTimer.Change(0, 1000);

                        highVacForm = new Form();
                        highVacForm.StartPosition = FormStartPosition.CenterParent;
                        highVacForm.Owner = this;
                        highVacLable = new Label();
                        highVacLable.Dock = DockStyle.Fill;
                        highVacLable.TextAlign = ContentAlignment.MiddleCenter;
                        highVacLable.Location = new Point(ppSingle.Left + ppSingle.Width / 2, ppSingle.Top + ppSingle.Height / 2);
                        highVacForm.Controls.Add(highVacLable);
                        highVacForm.FormBorderStyle = FormBorderStyle.None;
                        highVacForm.Size = new Size(300, 50);
                        
                        highVacForm.ShowDialog();
                    }
                    else
                    {
                        // 고압이 켜지지 않은 상태에서는 바로 진공 모드를 바꿈.
                        ((SECtype.IControlInt)column["VacuumMode"]).Value = 0;
                        SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum;
                    }

                    //DetectorSE.Enabled = false;
                    LowVac.Checked = false;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        System.Threading.Timer highvacuumTimer;
        int highVacTimer = 0;
        Form highVacForm;
        Label highVacLable;
        //highVacLable.
        void HighVacuumTimerExpiered(object obj)
        {
            //highVacLable.Location = new Point(ppSingle.Width /2, ppSingle.Height /2 );

            FilamentCheckedStartEnable = false;
            Action vacChangeAct = () =>
            {
                switch (highVacTimer)
                {
                    case 0:
                        operation.Hide();
                        highVacLable.Text = "20sec...\r\nEmmision Down.";
                        ((SECtype.IControlDouble)column["HvGrid"]).Value = 0;
                        ((SECtype.IControlDouble)column["HvFilament"]).Value = 0;
                        break;
                   
                    case 1:
                        highVacLable.Text = "19sec...\r\nEmmision Down.";

                        break;
                   
                    case 2:
                        highVacLable.Text = "18sec...\r\nHV off.";
                        ((SECtype.IControlDouble)column["HvElectronGun"]).Value = 0;
                        break;
                   
                    case 3:
                        highVacLable.Text = "17sec...\r\nHV off.";
                        break;
                    
                    case 4:
                        highVacLable.Text = "16sec...\r\nHV off.";
                        break;
                    
                    case 5:
                        highVacLable.Text = "15sec...\r\nValve Changing.";
                        ((SECtype.IControlInt)column["VacuumMode"]).Value = 0;
                        break;
                   
                    case 6:
                        highVacLable.Text = "14sec...\r\nValve Changing.";
                        break;
                   
                    case 7:
                        highVacLable.Text = "13sec...\r\nAccelated Voltage On.";
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvElectronGun"], ColumnOnevalueMode.Factory);
                        break;
                    
                    case 8:
                        highVacLable.Text = "12sec...\r\nAccelated Voltage On.";
                        break;
                    
                    case 9:
                        highVacLable.Text = "11sec...\r\nAccelated Voltage On.";
                        break;

                    case 10:
                        highVacLable.Text = "10sec...\r\nAccelated Voltage On.";
                        break;

                    case 11:
                        highVacLable.Text = "9sec...\r\nAccelated Voltage On.";
                        break;

                    case 12:
                        highVacLable.Text = "8sec...\r\nAccelated Voltage On.";
                        break;

                    case 13:
                        highVacLable.Text = "7sec...\r\nAccelated Voltage On.";
                        break;

                    case 14:
                        highVacLable.Text = "6sec...\r\nAccelated Voltage On.";
                        break;

                    case 15:
                        highVacLable.Text = "5sec...\r\nAccelated Voltage On.";
                        break;

                    case 16:
                        highVacLable.Text = "4sec...\r\nAccelated Voltage On.";
                        break;

                    case 17:
                        highVacLable.Text = "3sec...\r\nAccelated Voltage On.";
                        break;

                    case 18:
                        highVacLable.Text = "2sec...\r\nAccelated Voltage On.";
                        break;

                    case 19:
                        highVacLable.Text = "1sec...\r\nBeam current On.";

                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvGrid"], ColumnOnevalueMode.Run);
                        SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvFilament"], ColumnOnevalueMode.Run);
                        operation.InitDisplay();
                        //operation.Show();
                        break;
                   
                    case 20:
                        highVacForm.Close();
                        highVacForm.Dispose();

                        highvacuumTimer.Dispose();

                        SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum;
                        FilamentCheckedStartEnable = true;

                        break;
                }
                highVacLable.Invalidate();
                highVacTimer++;
            };
            this.Invoke(vacChangeAct);
        }

        private void DetectorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ChangeDetectAndVacuumStateDisplay()
        {
            string infoStr = "";
            switch (SystemInfoBinder.Default.DetectorMode)
            {
                case SystemInfoBinder.ImageSourceEnum.SED:
                    //m_InfomationPanel.Detector = InfoPanel.DetectorMode.SED;
                    infoStr += DetectorSE.Text;
                    break;
                case SystemInfoBinder.ImageSourceEnum.BSED:
                    //m_InfomationPanel.Detector = InfoPanel.DetectorMode.BSED;
                    infoStr += DetectorBSE.Text;
                    break;
                case SystemInfoBinder.ImageSourceEnum.DualSEBSE:
                    //m_InfomationPanel.Detector = InfoPanel.DetectorMode.BSED;
                    infoStr += "Dual";
                    break;

                case SystemInfoBinder.ImageSourceEnum.Merge:
                    //m_InfomationPanel.Detector = InfoPanel.DetectorMode.BSED;
                    infoStr += "Merge";
                    break;

                case SystemInfoBinder.ImageSourceEnum.Camera:
                    infoStr += "Camera";
                    break;
            }

            //infoStr += "\r\n";

            string vacStr = "";
            switch (SystemInfoBinder.Default.VacuumMode)
            {
                case SystemInfoBinder.VacuumModeEnum.HighVacuum:
                    //m_InfomationPanel.Vacuum = InfoPanel.VacuumMode.HighVacuum;
                    vacStr += HighVac.Text;
                    break;
                case SystemInfoBinder.VacuumModeEnum.LowVacuum:
                    //m_InfomationPanel.Vacuum = InfoPanel.VacuumMode.LowVacuum;
                    vacStr += LowVac.Text;
                    break;
            }

            //detectVacuumLab.Text = infoStr;
            ppSingle.MicronEtcString = infoStr;
            ppSingle.VacEtcString = vacStr;

            //ppSingle.MicronEtcString = infoStr;


            //detectVacuumLab.Visible = true;

        }
        #endregion

        #region Focus Wobble
        private void frMainFocusWobbleCbewicb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.FocusType)
                {
                    focusWobbleUpcc.Show(this, WobbleBtn.PointToScreen(e.Location));
                }
                else
                {
                    focusWobbleUpcc.Show(this, WobbleBtn.PointToScreen(e.Location));
                }

                
            }
        }

        #endregion

        #region Stig Wobble
        private void AlignSWReset_Click(object sender, EventArgs e)
        {
            //alignStigWF.Value = 0;
            //alignStigWF.Value = 0;
        }

        #endregion

        #region Magnification


        private bool framChange = false;
        private void frMainMagWheel(object sender, EventArgs e)
        {
            
            this.MouseWheel += new MouseEventHandler(frMainMagWheelControl);
        }

        private void frMainMagWheelControl(object sender, MouseEventArgs e)
        {

            if (!framChange)
            {
                if (e.Delta > 0)
                {
                    if (UIsetBinder.Default.MagIndex < UIsetBinder.Default.MagMaximum)
                    {
                        //UIsetBinder.Default.MagIndex++;
                        if (!MagBtnEnable)
                        {
                            while (true)
                            {
                                MagTrakBar.Value++;

                                if (UIsetBinder.Default.MagIndex == UIsetBinder.Default.MagMaximum)
                                {
                                    break;
                                }

                                if (equip.Magnification < 10)
                                {
                                    if (equip.Magnification % 5 == 0)
                                    {
                                        break;
                                    }

                                }
                                else if (equip.Magnification < 100)
                                {
                                    if (equip.Magnification % 5 == 0)
                                    {
                                        break;
                                    }

                                }
                                else if (equip.Magnification < 1000)
                                {
                                    if (equip.Magnification % 50 == 0)
                                    {
                                        break;
                                    }
                                }
                                else if (equip.Magnification < 10000)
                                {
                                    if (equip.Magnification % 500 == 0)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (equip.Magnification % 5000 == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            

                            //MagTrakBar.Value = MagTrakBar.Value + 10;
                        }
                        else
                        {
                            MagTrakBar.Value++;
                        }
                        
                    }

                    if (WobbleCheckedBox.Checked)
                    {
                        m4000FocusWF.Value = 6;

                        AutoWobble();

                    }

                    

                    


                }
                else
                {

                    if (UIsetBinder.Default.MagIndex > UIsetBinder.Default.MagMinimum)
                    {
                        
                        if (!MagBtnEnable)
                        {
                            while (true)
                            {
                                MagTrakBar.Value--;

                                if (UIsetBinder.Default.MagIndex == UIsetBinder.Default.MagMinimum)
                                {
                                    break;
                                }

                                if (equip.Magnification < 10)
                                {
                                    if (equip.Magnification % 5 == 0)
                                    {
                                        break;
                                    }

                                }
                                else if (equip.Magnification < 100)
                                {
                                    if (equip.Magnification % 5 == 0)
                                    {
                                        break;
                                    }

                                }
                                else if (equip.Magnification < 1000)
                                {
                                    if (equip.Magnification % 50 == 0)
                                    {
                                        break;
                                    }
                                }
                                else if (equip.Magnification < 10000)
                                {
                                    if (equip.Magnification % 500 == 0)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (equip.Magnification % 5000 == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            //MagTrakBar.Value = MagTrakBar.Value - 10;
                        }
                        else
                        {
                            MagTrakBar.Value--;
                        }
                        
                    }

                    if (WobbleCheckedBox.Checked)
                    {
                        m4000FocusWF.Value = 6;

                        AutoWobble();

                    }


                }


                MotorStageSpeedChange();

            }
            else
            {
                if (e.Delta > 0)
                {
                    if (DZTrackBar.Value < DZTrackBar.Maximum)
                    {
                        DZTrackBar.Value++;

                        archTrackBqr = DZTrackBar.Value;
                        //DZLabel.Text = "x" + DZTrackBar.Value.ToString();
                        //ArchivesImageSizeChange(true);

                    }
                }
                else
                {
                    if (DZTrackBar.Value > DZTrackBar.Minimum)
                    {
                        DZTrackBar.Value--;
                        archTrackBqr = DZTrackBar.Value;
                        //DZLabel.Text = "x" + DZTrackBar.Value.ToString();
                        //ArchivesImageSizeChange(false);
                    }
                }
            }


            //if (Properties.Settings.Default.MotorStageEnable)
            //{
            //        MotorStage.SpeedChange();
                
                
            //}
            //MotorStageSpeedChange();
            

           

        }

        private void MotorStageSpeedChange()
        {
            double speedmin = Properties.Settings.Default.MotorSpeedMin;
            double speedmax = Properties.Settings.Default.MotorSpeedMax;
            double nowmag = UIsetBinder.Default.MagIndex;
            double maxmag = UIsetBinder.Default.MagMaximum;
            double minmag = UIsetBinder.Default.MagMinimum;

            double magNow = (speedmin - speedmax) / maxmag * nowmag;
            double speedvalue = magNow - ((speedmin - speedmax) / maxmag * minmag);
            Properties.Settings.Default.MotorSpeed = (int)(speedvalue + speedmax);
        }


        private void AutoWobble()
        {
            if (equip.Magnification <= 1000)
            {
                m4000FocusWA.Value = 20;
            }
            else if (equip.Magnification <= 5000)
            {
                m4000FocusWA.Value = 15;
            }
            else if (equip.Magnification <= 7000)
            {
                 m4000FocusWA.Value = 10;
            }
            else if (equip.Magnification <= 10000)
            {
                m4000FocusWA.Value = 5;
            }
            else
            {
                m4000FocusWA.Value = 1;
            }




        }


        int archTrackBqr = 0;

        private void ArchivesDZTrackBarValueChange(object sender, EventArgs e)
        {
            ImageTrackBarWithSingle itb = sender as ImageTrackBarWithSingle;

            if (archTrackBqr > DZTrackBar.Maximum) { return; }
            if (archTrackBqr < DZTrackBar.Minimum) { return; }

            if (archTrackBqr < itb.Value)
            {
                archTrackBqr++;
                DZLabel.Text = "x" + DZTrackBar.Value.ToString();
                ArchivesImageSizeChange(true);
            }
            else
            {
                archTrackBqr--;
                DZLabel.Text = "x" + DZTrackBar.Value.ToString();
                ArchivesImageSizeChange(false);
            }
        }

        //private System.Drawing.Imaging.Metafile mt;

        private void ArchivesImageSizeChange(bool enable)
        {
            //Bitmap bmp = (Bitmap)ArchivesPictureBox.Image;
            
            //if (bmp == null) { return; }

            int left = ArchivesPictureBox.Left;
            int height = ArchivesPictureBox.Top;

            if (enable)
            {
                ArchivesPictureBox.Width += ArchivesPictureBox.Width;
                ArchivesPictureBox.Height += ArchivesPictureBox.Height;

                ArchivesPictureBox.Left -= ArchivesPictureBox.Width / 4;
                ArchivesPictureBox.Top -= ArchivesPictureBox.Height / 4;
            }
            else
            {
                ArchivesPictureBox.Width = ArchivesPictureBox.Width / 2;
                ArchivesPictureBox.Height = ArchivesPictureBox.Height / 2;

                ArchivesPictureBox.Left += ArchivesPictureBox.Width / 2;
                ArchivesPictureBox.Top += ArchivesPictureBox.Height / 2;
            }
           

        }

        private void frMainMagDecBb_Click(object sender, EventArgs e)
        {
            if (UIsetBinder.Default.MagIndex > UIsetBinder.Default.MagMinimum) { UIsetBinder.Default.MagIndex--; }
            //if (lwf != null) { lwf.OL_Amplitude = OL_Amplitude_Value(); }

        }

        private void frMainMagMiniNum_Click(object sender, EventArgs e)
        {
            UIsetBinder.Default.MagIndex = UIsetBinder.Default.MagMinimum;
            MagTrakBar.Value = 0;
        }

        private void frMainMagrise_Click(object sender, EventArgs e)
        {
            UIsetBinder.Default.MagIndex++;

        }

        private void frMainMagIncBb_Click(object sender, EventArgs e)
        {
            if (UIsetBinder.Default.MagIndex < UIsetBinder.Default.MagMaximum) { UIsetBinder.Default.MagIndex++; }
            //if (lwf != null) { lwf.OL_Amplitude = OL_Amplitude_Value(); }
        }

        private void MagStingChange()
        {
            UIsetBinder.Default.MagString = "x" + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(equip.Magnification, 0, 2, false, ' ');

            ppSingle.Magnification = equip.Magnification;
        }
        #endregion

        //#endregion

        #region StatusServer
        //StatusServer statusServer = null;
        //private void statusServerToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    new StatusServerViewer().Show(this);
        //}

        Button scanStealBut = null;

        void statusServer_ScanFree(object sender, EventArgs e)
        {
            scanStealBut_MouseClick(null, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));

        }
        #endregion

        private delegate void StatusServerScanChangeDelegate();

        void ScanStealed()
        {
            //if (m_FastScan.Checked) { bfScanSteal = m_FastScan; }
            //else if (m_SlowScan.Checked) { bfScanSteal = m_SlowScan; }
            //else if (m_FastPhoto.Checked) { bfScanSteal = m_FastPhoto; }
            //else if (m_SlowPhoto.Checked) { bfScanSteal = m_SlowPhoto; }
            //else if (m_ScanPause.Checked) { bfScanSteal = m_ScanPause; }
            //m_ScanPause.Checked = true;
            //Graphics g = Graphics.FromImage(m_RepaintImage);
            //g.Clear(Color.Black);
            //g.DrawString("Scan Stealed", new Font(Font.FontFamily, 32), Brushes.White, new Point(0, 0));
            //g.Dispose();
            ////m_ScreenWindow.Invalidate();

            //scanStealBut = new Button();
            //scanStealBut.Location = new Point(736, 576);
            //scanStealBut.Size = new Size(224, 112);
            //scanStealBut.MouseClick += new MouseEventHandler(scanStealBut_MouseClick);
            //scanStealBut.Cursor = Cursors.Arrow;
            //scanStealBut.UseVisualStyleBackColor = true;
            //scanStealBut.Text = "Scan Stealed.";
            //this.Controls.Add(scanStealBut);
            //scanStealBut.BringToFront();
        }

        //RadioButton bfScanSteal;
        void statusServer_ScanStealed(object sender, EventArgs e)
        {
            if (scanStealBut == null)
            {
                this.BeginInvoke(new StatusServerScanChangeDelegate(ScanStealed));
            }
        }

        void scanStealBut_MouseClick(object sender, MouseEventArgs e)
        {
            this.Invoke(new StatusServerScanChangeDelegate(ScanFree));
        }

        void ScanFree()
        {
            //if (scanStealBut != null)
            //{
            //    this.Controls.Remove(scanStealBut);
            //    scanStealBut.Dispose();
            //    scanStealBut = null;
            //    bfScanSteal.Checked = true;
            //}
        }

        string SendStatusServer(StatusServer.DeviceValueEnum device)
        {
            switch (device)
            {
                case StatusServer.DeviceValueEnum.AccelatedVoltage:
                    //return m_InfoEghv.Text;
                    //return m_InfomationPanel.Eghv;
                    return ppSingle.MicronEghv;
                case StatusServer.DeviceValueEnum.Magnification:
                    //return m_InfoMag.Text;
                    //return m_InfomationPanel.Magnification;
                    return ppSingle.MicronMagnification;
            }
            return "";
        }
        #endregion



        #region Front Panel
        List<Infragistics.Win.UltraWinTabControl.UltraTab> fpList = null;
        FrontPanelList fpFpl = null;

        private void fpSelectListBut_Click(object sender, EventArgs e)
        {
            if ((fpFpl == null) || (fpFpl.IsDisposed))
            {
                fpFpl = new FrontPanelList();
                fpFpl.PanelSeleted = PanelSeleted;
                fpFpl.SetItem(fpList);
                fpFpl.Show(this);
            }
            else
            {
                fpFpl.Visible = true;
            }
        }

        void PanelSeleted(string name)
        {
            if (Properties.Settings.Default.AutoHideFPL) { fpFpl.Hide(); }
            //frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[name];
        }

        private void fpSelectUpDownBb_Click(object sender, EventArgs e)
        {
            //int index = fpList.IndexOf(frontPanelUtc.SelectedTab);

            //if (sender == fpSelectDownBb)
            //{
            //    index++;
            //}
            //else if (sender == fpSelectUpBb)
            //{
            //    index--;
            //}
            //else
            //{
            //    throw new ArgumentException();
            //}

            //if (index < 0) { index = fpList.Count - 1; }
            //index %= fpList.Count;

            //frontPanelUtc.SelectedTab = fpList[index];
        }

        private void frontPanelUtc_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //fpSelectListBut.Text = frontPanelUtc.SelectedTab.Text;
        }
        #endregion
        //#endregion

        #region Measuring Tools
        public void MTools_Click(Control con)
        {
            //Control con  = sender as Control;

            switch (con.Name)
            {
                case "mtColor":
                    SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
                    if (ib != null)
                    {
                        ColorDialog cd = new ColorDialog();
                        cd.Color = ib.ItemColor;
                        if (cd.ShowDialog() == DialogResult.OK)
                        {
                            ib.ItemColor = cd.Color;
                        }
                    }
                    break;
                case "mtDeleteAll":
                    ppSingle.MTools.DeleteItemAll();
                    break;
                case "mtDeleteOne":
                    ppSingle.MTools.DeleteItem();
                    break;
                case "mtIAngular":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Angle);
                    break;
                case "mtIArea":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.ClosePath, mtText.Checked);
                    break;
                case "mtIArrow":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Arrow);
                    break;
                case "mtICancel":
                    ppSingle.MTools.SelectNull();
                    break;
                case "mtIEllipse":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Ellipse, mtText.Checked);
                    break;
                case "mtILength":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.OpenPath, mtText.Checked);
                    break;
                case "mtILinear":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Line, mtText.Checked);
                    break;
                case "mtIRectangle":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Rectangle, mtText.Checked);
                    break;
                case "mtIText":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.TextBox);
                    break;
                case "mtIMarquios":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.MarquiosScale);
                    break;
                case "mtIPoint":
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Point, mtText.Checked);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public void mtList_SelectedIndexChanged()
        {
            ppSingle.MTools.SetSelectItem(mtList.SelectedItem as SEC.GUIelement.MeasuringTools.ItemBase);
        }

        void MTools_SelectedItemChanged(object sender, EventArgs e)
        {
            SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
            mtList.SelectedItem = ib;
            if (ib != null)
            {
                mtText.Checked = ib.DrawText;
            }
        }

        void MTools_ItemChanged(object sender, EventArgs e)
        {
            sfMTools.MTList.Items.Clear();
            foreach (SEC.GUIelement.MeasuringTools.ItemBase ib in ppSingle.MTools)
            {
                sfMTools.MTList.Items.Add(ib);
            }
            sfMTools.PpSingle = ppSingle;
            sfMTools.MTList.SelectedItem = ppSingle.MTools.GetSelectItem();
        }
        private void MToolsText_CheckedChanged(object sender, EventArgs e)
        {
            SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
            if (ib != null)
            {
                ib.DrawText = mtText.Checked;
                ppSingle.MTools.SelectNull();
                ppSingle.MTools.SetSelectItem(ib);
                ppSingle.Invalidate();

                MTools_ItemChanged(ppSingle.MTools, EventArgs.Empty);
            }
        }
        #endregion

        #region Foront Panel 추가 기능
        private void SpotsizeMode_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                foreach (Control con in cb.Parent.Controls)
                {
                    if (con is CheckBox)
                    {
                        if (con != cb)
                        {
                            CheckBox cbn = con as CheckBox;
                            cbn.Checked = false;
                        }
                    }
                }

                Settings.MiniSEM.ManagerMiniSEM manager = SystemInfoBinder.Default.SetManager as Settings.MiniSEM.ManagerMiniSEM;

                int mode = -1;

                //if (cb == ss1Spot1) { mode = 0; }
                //else if (cb == ss1Spot2) { mode = 1; }
                //else if (cb == ss1Spot3) { mode = 2; }
                //else { throw new ArgumentException(); }

                int cl1, cl2;

                manager.GetSizesizeMode(mode, out cl1, out cl2);

                (equip.ColumnLensCL1 as SECtype.IControlDouble).Value = cl1 * (equip.ColumnLensCL1 as SECtype.IControlDouble).Precision;
                (equip.ColumnLensCL2 as SECtype.IControlDouble).Value = cl2 * (equip.ColumnLensCL2 as SECtype.IControlDouble).Precision;
            }
        }



        //private SEC.Nanoeye.NanoColumn.ISEMController column = null;
        public SEC.Nanoeye.NanoColumn.ISEMController Column
        {
            set
            {
                //bsX.ControlValue = equip.ColumnBSX;
                //bsY.ControlValue = equip.ColumnBSY;
            }
        }


        public void bsWindows_Close()
        {
            bsWindow.Checked = false;
        }


        private void bsWindow_Click(object sender, EventArgs e)
        {
            if (bsWindow.Checked)
            {
                sfBeamshiftWindow = new BeamShiftCrossPoint(this);

                //sfBeamshiftWindow.Show(this);
                sfBeamshiftWindow.Show();
                //sfBeamshiftWindow.BeamShiftLocation();
            }
            else
            {
                sfBeamshiftWindow.Hide();
            }

            
        }

        private void stigSync_CheckStateChanged(object sender, EventArgs e)
        {
            //(equip.ColumnStigSyncX as SECtype.IControlBool).Value = stigSync.Checked;
            //(equip.ColumnStigSyncY as SECtype.IControlBool).Value = stigSync.Checked;
        }
        #endregion

        #region StartUp
        private int m_AutoStartupIndex;
        FormProgressNotify formProgress;
        public void startEnable(bool enable)
        {
            m_ToolStartup.Checked = enable;
        }

        private System.Windows.Forms.Timer m_Stopwatch;
        public void ToolStartup_CheckedChangedEvent(object sender, EventArgs e)
        {

            //m_ToolStartup.Checked
            //if (operation != null)
            //{
            //    m_ToolStartup.Checked = startEnable;
            //}

           
            if (m_ToolStartup.Checked)
            {
                formProgress = new FormProgressNotify("Auto Startup");

                formProgress.NotifyMessage = "Progress";
                formProgress.Owner = this;
                formProgress.ProgressChecking += new EventHandler<ProgressCheckingEventArgs>(formProgress_ProgressTesting_AutoStartup);
                
                //ProfileBtn.Enabled = false;
                //ProfileBtn.Appearance.ImageBackground = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_disable;
                //DetectorSE.Enabled = false;
                //DetectorBSE.Enabled = false;
                //DetectorDual.Enabled = false;
                //DetectorMerge.Enabled = false;

                //HighVac.Enabled = false;
                //LowVac.Enabled = false;
                //LanguageButton1.Enabled = false;
                //LanguageButton2.Enabled = false;
                //LanguageButton3.Enabled = false;
                //LanguageButton4.Enabled = false;
                //LanguageButton5.Enabled = false;

                m_ToolStartup.Text = "Stop";

                //m_Stopwatch = new System.Windows.Forms.Timer();
                //m_Stopwatch.Interval = 1000;
                //m_Stopwatch.Tick += new EventHandler(formProgress_ProgressTesting_AutoStartup);
               

                

                formProgress.TimerInterval = 1000;


                FilamentRuntimeChecker.Start();
                m_AutoStartupIndex = 0;


                formProgress.TimerEnabled = true;

                DialogResult result = formProgress.ShowDialog(this);


                //m_Stopwatch.Start();
                //formProgress.FormStart();

                //formProgress_ProgressTesting_AutoStartup();





                if (!m_ToolStartup.Checked)
                {
                    //m_Stopwatch.Stop();
                    m_ToolStartup.Checked = false;
                    if (operation != null)
                    {
                        operation.StartupEnable = false;

                    }


                    MessageBox.Show(
                        TextManager.Instance.GetString("Message_StartCancel").Text,
                        this.Text.PadRight(48),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (formProgress.TimerEnabled)
                    {
                        try
                        {
                            m_ToolStartup.Checked = false;
                        }
                        catch
                        {
                            return;
                        }
                        
                        //return;
                    }
                }
                

                

                //else
                //{
                //systemCheckTimer.Start();
                //}
                //StartMainButton.Checked = true;

                //if
                //frameMain.SelectedTab = frameMain.Tabs["frNewMain"];
                //setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                //setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);
                //operation.StartupEnable = true;
                if (m_ToolStartup.Checked)
                {
                    formProgress.Close();
                    formProgress.Dispose();

               
                    ((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
                    ((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
                    ((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
                }
               
            }
            else
            {
                ((SECtype.IControlBool)column["HvEnable"]).Value = false;

                

                ScanModeChange(ScanModeEnum.ScanPause);

                // emission 표시 초기화
                EmissionDisplayLabel.Text = "000uA";
                //systemCheckTimer.Stop();

                if (m_Stopwatch != null)
                {
                    m_Stopwatch.Stop();
                }

                 
                FilamentRuntimeChecker.Stop();
                StartUpProgressBar.Visible = false;
                if (operation != null)
                {
                    operation.StartupEnable = false;
                }

                ProfileBtn.Enabled = true;
                //ProfileBtn.Appearance.ImageBackground = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_enable;
                //DetectorSE.Enabled = true;
                //DetectorBSE.Enabled = true;
                //HighVac.Enabled = true;
                //LowVac.Enabled = true;
                LanguageButton1.Enabled = true;
                LanguageButton2.Enabled = true;
                LanguageButton3.Enabled = true;
                LanguageButton4.Enabled = true;
                LanguageButton5.Enabled = true;

                FilamentCheckedStartEnable = false;

                switch (SystemInfoBinder.Default.AppDevice)
                {
                    case AppDeviceEnum.SNE3000MB:
                        DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                        DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorDual.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorMerge.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        HighVac.Enabled = true;
                        LowVac.Enabled = true;

                        detectClt.Value = 0;
                        detectPmt.Value = 0;

                        break;

                    case AppDeviceEnum.SNE3200M:
                        DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                        DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorDual.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorMerge.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        HighVac.Enabled = false;
                        LowVac.Enabled = false;

                        if (DetectorSE.Checked)
                        {
                            detectClt.Value = detectClt.Maximum;
                            detectPmt.Value = detectPmt.Maximum;
                        }
                        else
                        {
                            detectClt.Value = 0;
                            detectPmt.Value = 0;
                        }

                        break;

                    case AppDeviceEnum.SNE4500M:
                        DetectorSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorSED;
                        DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorDual.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        DetectorMerge.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                        HighVac.Enabled = false;
                        LowVac.Enabled = false;

                        if (operation != null)
                        {
                            operation.DetectorEnable = DetectorSE.Enabled;
                            operation.DetectorBSEEnable = DetectorBSE.Enabled;
                            
                        }

                        if (DetectorSE.Checked)
                        {
                            detectClt.Value = detectClt.Maximum;
                            detectPmt.Value = detectPmt.Maximum;
                        }
                        else
                        {
                            detectClt.Value = 0;
                            detectPmt.Value = 0;
                        }
                        break;
                }

                ((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
                ((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
                ((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);

                m_ToolStartup.ForeColor = Color.FromArgb(201, 201, 202);
                m_ToolStartup.Text = "Start";
               
                

            }


            

            //m_ToolStartup.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_top_select;

            //if (m_ToolStartup.Checked)
            //{
            //    startEnable = true;
            //}
            //else
            //{
            //    startEnable = false;
            //}


           
        }

        Action<Label, string> TextSetAct = (lab, str) => { lab.Text = str; };
        void Eghv_RepeatUpdated(object sender, object[] value)
        {
            if (InvokeRequired)
            {
                EghvCurrentLab.BeginInvoke(TextSetAct, new object[] { EghvCurrentLab, string.Format("{0:0.0}",(double)(value[0])) });
                //hvEghvcurLab.BeginInvoke(TextSetAct, new object[] { hvEghvcurLab, ((double)(value[0])).ToString() });
            }
        }

        void Tip_RepeatUpdated(object sender, object[] value)
        {
            if (InvokeRequired)
            {
                TipCurrentLab.BeginInvoke(TextSetAct, new object[] { TipCurrentLab, string.Format("{0:0.0}",(double)(value[0])) });
                //hvTipcurLab.BeginInvoke(TextSetAct, new object[] { hvTipcurLab, ((double)(value[0])).ToString() });
            }
        }

        void Grid_RepeatUpdated(object sender, object[] value)
        {
            if (InvokeRequired)
            {
                GridCurrentLab.BeginInvoke(TextSetAct, new object[] { GridCurrentLab, string.Format("{0:0.0}", (double)(value[0])) });
                //hvGridcurLab.BeginInvoke(TextSetAct, new object[] { hvGridcurLab, ((double)(value[0])).ToString() });
            }
        }

        private void formProgress_ProgressTesting_AutoStartup(object sender, ProgressCheckingEventArgs e)
        //private void formProgress_ProgressTesting_AutoStartup(object sender, EventArgs e)
        {
            e.Progress = (m_AutoStartupIndex * 100) / 15;

            StartUpProgressBar.Visible = true;

            if (!m_ToolStartup.Checked)
            {
                m_AutoStartupIndex = 20;
            }


            Trace.WriteLine("m_AutoStartupIndex : " + m_AutoStartupIndex.ToString());



            switch (m_AutoStartupIndex++)
            {
                case 0:	// EGPS ON
                    ((SECtype.IControlBool)column["HvEnable"]).Value = true;
                    equip.MagChange(1);
                    MagTrakBar.Value = MagTrakBar.Minimum;

                    setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.Factory);

                    

                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_1;
                    if (operation != null)
                    {
                        operation.Change(0);
                    }
                    break;
                case 1:
                    if (focusmodeDl.Visible)
                    {
                        ((SECtype.IControlDouble)column["LensObjectCoarse"]).Value = ((((double)equip.ColumnLensOLC.Maximum - (double)equip.ColumnLensOLC.Minimum) / 2) + (double)equip.ColumnLensOLC.Minimum);
                    }
                    else
                    {
                        setManager.ColumnOneLoad(equip.ColumnLensOLF, ColumnOnevalueMode.Factory);
                    }
                    break;

                case 2:
                    setManager.ColumnOneLoad(equip.ColumnStigXAB, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnStigXCD, ColumnOnevalueMode.Factory);
                
                    setManager.ColumnOneLoad(equip.ColumnStigYAB, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnStigYCD, ColumnOnevalueMode.Factory);
                    break;
                case 3:
                    equip.MagChange(0);
                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvElectronGun"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
                    if (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum)
                    {
                        ((SECtype.IControlDouble)column["HvCollector"]).Value /= 2;
                        ((SECtype.IControlDouble)column["HvPmt"]).Value /= 2;
                    }
                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_2;
                    if (operation != null)
                    {
                        operation.Change(1);
                    }

                    break;
                case 4:

                      setManager.ColumnOneLoad(equip.ColumnGAX, ColumnOnevalueMode.Factory);
                      setManager.ColumnOneLoad(equip.ColumnGAY, ColumnOnevalueMode.Factory);
                    break;
                case 5:
                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvFilament"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_3;
                    if (operation != null)
                    {
                        operation.Change(2);
                    }
                    break;
                case 6:
                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvGrid"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
                    switch (SystemInfoBinder.Default.VacuumMode)
                    {
                        case SystemInfoBinder.VacuumModeEnum.HighVacuum:
                            switch (SystemInfoBinder.Default.ScanSource)
                            {
                                case 0:
                                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvCollector"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Run);
                                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvPmt"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Run);

                                    setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                                    setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                                    break;
                                case 1:
                                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvCollector"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.External);
                                    SystemInfoBinder.Default.SetManager.ColumnOneLoad(column["HvPmt"], SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.External);
                                    break;
                            }
                            break;
                        case SystemInfoBinder.VacuumModeEnum.LowVacuum:
                            ((SECtype.IControlDouble)column["HvCollector"]).Value = 0;
                            ((SECtype.IControlDouble)column["HvPmt"]).Value = 0;
                            break;
                    }
                    break;

                    
                case 7:
                case 8:
                case 9:
                    //StartButtonProgressBar.Checked = true;
                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_4;
                    if (operation != null)
                    {
                        operation.Change(3);
                    }
                    break;
                case 10:
                    //formProgress.TimerEnabled = false;
                    ScanModeChange(ScanModeEnum.FastScan);
                    //formProgress.TimerEnabled = true;
                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_5;
                    if (operation != null)
                    {
                        operation.Change(4);
                    }
                    break;
                case 11:
                case 12:
                case 13:
                    
                    break;
                case 14:
                    //formProgress.TimerEnabled = false;
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                    //formProgress.TimerEnabled = true;
                    //StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_6;
                    if (operation != null)
                    {
                        operation.Change(5);
                    }
                    break;
                case 15:
                    //frameMain.SelectedTab = frameMain.Tabs["frNewMain"];
                   
                    //DetectorSE.Enabled = true;
                    //DetectorBSE.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                    //DetectorDual.Enabled = SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
                    //m_Stopwatch.Stop();
                    //operation.Hide();
                   
                    FilamentCheckedStartEnable = true;
                    formProgress.TimerEnabled = false;
                    e.Cancel = true;

                    //m_Stopwatch.
                    break;
                default:
                    m_ToolStartup.Checked = false;
                    FilamentCheckedStartEnable = false;
                    if (operation != null)
                    {
                        operation.Change(6);
                    }
                    
                    e.Cancel = true;
                    break;
            }

        }


        #endregion

        private void frPageChange(object sender, EventArgs e)
        {
            frameMain.SelectedTab = frameMain.Tabs["frNewMain"];
        }

        private void SpotSizeChange(object sender, EventArgs e)
        {


            CheckBox cb = sender as CheckBox;
            if (cb == null)
            {
                //bitmapMiddle.Checked = true;

                //bitmapSmall.Checked = false;
                //bitmapLarge.Checked = false;
                //ss3SpotSize.Value = (int)((ss3SpotSize.Minimum + ss3SpotSize.Maximum) * 0.5);
                return;

            }
            else if (!cb.Checked)
            {

                if (cb.Name == EDSButton.Name)
                {
                    SpotSizePage.SelectedTab = SpotSizePage.Tabs["frSpot1"];

                    setManager.ColumnOneLoad(equip.ColumnLensCL1, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnLensCL2, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                    setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);
                    
                }
               
               



            }
            else if (cb.Checked)
            {
                if (cb.Name == EDSButton.Name)
                {
                    SpotSizePage.SelectedTab = SpotSizePage.Tabs["frSpot2"];

                    if (Properties.Settings.Default.EDSHighSECL1 != 0)
                    {
                        EDSChange();
                    }
                   
                }

                if (cb == bitmapSmall)
                {

                    //ss3SpotSize.Value = (int)((ss3SpotSize.Minimum + ss3SpotSize.Maximum) * 0.3);
                    ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.3) + ss3SpotSize.Minimum);
                    //Trace.WriteLine("ss3SpotSize.ControlValue : " + ss3SpotSize.ControlValue.ToString());

                    bitmapSmall.Checked = true;
                    bitmapMiddle.Checked = false;
                    bitmapLarge.Checked = false;

                }
                else if (cb == bitmapMiddle)
                {
                    ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.5) + ss3SpotSize.Minimum);
                    bitmapSmall.Checked = false;
                    bitmapMiddle.Checked = true;
                    bitmapLarge.Checked = false;
                }
                else if (cb == bitmapLarge)
                {
                    ss3SpotSize.Value = (int)(((ss3SpotSize.Maximum - ss3SpotSize.Minimum) * 0.7) + ss3SpotSize.Minimum);
                    bitmapMiddle.Checked = false;
                    bitmapSmall.Checked = false;
                    bitmapLarge.Checked = true;
                }
            }

            

            //cb.Checked = false;
        }

        private void ss3SpotSizeChange(object sender, EventArgs e)
        {

            //if (ss3SpotSize.Value <= (int)((ss3SpotSize.Minimum + ss3SpotSize.Maximum) * 0.3))
            //{
            //    bitmapSmall.Checked = true;
            //    bitmapMiddle.Checked = false;
            //    bitmapLarge.Checked = false;
            //}
            //else if (ss3SpotSize.Value >= (int)((ss3SpotSize.Minimum + ss3SpotSize.Maximum) * 0.7))
            //{
            //    bitmapSmall.Checked = false;
            //    bitmapMiddle.Checked = false;
            //    bitmapLarge.Checked = true;
            //}
            //else
            //{
            //    bitmapSmall.Checked = false;
            //    bitmapMiddle.Checked = true;
            //    bitmapLarge.Checked = false;
            //}

        }

        private bool OperEnable = false;

        private void PageChange(object sender, EventArgs e)
        {

            frameMain.SuspendLayout();

        }

        public bool SEChecked
        {
            get { return DetectorSE.Checked; }
            set
            {
                DetectorSE.Checked = value;
                DetectorBSE.Checked = false;
                DetectorChange(DetectorSE, EventArgs.Empty);


            }
        }



        public void DetectorSEChange(object sender, EventArgs e)
        {
            

            if (DetectorSE.Checked)
            {
                DetectorSE.Checked = true;
                DetectorBSE.Checked = false;

                //HighVac.Enabled = false;
                LowVac.Enabled = false;

               
                scanner.Revers(true);

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.SED;

                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);


                //detectClt.ControlValue = equip.ColumnHVCLT;
                //detectPmt.ControlValue = equip.ColumnHVPMT;



                scanner.ScanMode(false);

            }
            else
            {
                DetectorSE.Checked = true;

                return;
            }

            if (m_ToolStartup.Checked)
            {
                //scanner.ScanMode(false);
                // 고압이 켜진상태에서는 FastScan을 시작/재시작 시킴.
                                 
                
                if (frMainScanSfBb.Checked) 
                {
                    ScanModeChange(ScanModeEnum.ScanPause); 
                    ScanModeChange(ScanModeEnum.FastScan);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else if (frMainScanSsBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.SlowScan);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else if (frMainScanPfBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.FastPhoto);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else if (frMainScanPsBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.SlowPhoto);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                }
  
            }

            //frameMain.SelectedTab = frameMain.Tabs["frNewMain"];
            BSEControl.SelectedTab = BSEControl.Tabs["DetectorMode"];

            //ChangeDetectAndVacuumStateDisplay();

        }

        public bool BSEChecked
        {
            get
            {
                return DetectorBSE.Checked;
            }
            set
            {
                DetectorBSE.Checked = value;
                DetectorSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = false;
                DetectorChange(DetectorBSE, EventArgs.Empty);
            }
        }

        public bool DualChecked
        {
            get { return DetectorDual.Checked; }
            set
            {

                DetectorDual.Checked = value;
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorMerge.Checked = false;

                DetectorChange(DetectorDual, EventArgs.Empty);
            }
        }

        public bool MergeChecked
        {
            get { return DetectorMerge.Checked; }
            set
            {

                DetectorMerge.Checked = value;
                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;

                DetectorChange(DetectorMerge, EventArgs.Empty);
            }
        }



       
        public void DetectorBSEChange(object sender, EventArgs e)
        {
            if (DetectorBSE.Checked)
            {
                DetectorBSE.Checked = true;
                DetectorSE.Checked = false;
                
                HighVac.Enabled = true;

                //((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;

                if (CamarBtn.Checked)
                {
                    CamarBtn.Checked = false;
                    pictureBoxIpl1.Visible = false;
                    ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;
                    
                }
                
                
                

                
                //StopCamera();

                if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow)
                {
                    LowVac.Enabled = true;
                }
                else
                {
                    LowVac.Enabled = false;
                }

                //LowVac.Enabled = true;

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.BSED;
                
               
                
                scanner.Revers(false);
                //scanner.ScanMode(true);


            }
            else
            {
                DetectorBSE.Checked = true;
                return;
            }

            if (m_ToolStartup.Checked)
            {
               

                scanner.ScanMode(true);
                // 고압이 켜진상태에서는 FastScan을 시작/재시작 시킴.


              
                
           
                if (frMainScanSfBb.Checked) 
                {
                    
                    ScanModeChange(ScanModeEnum.ScanPause); 
                    ScanModeChange(ScanModeEnum.FastScan);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());

                           
                }
                else if (frMainScanSsBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.SlowScan);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else if (frMainScanPfBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.FastPhoto);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else if (frMainScanPsBb.Checked)
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                    ScanModeChange(ScanModeEnum.SlowPhoto);
                    Delay(1000);
                    new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, null, SystemInfoBinder.Default.DetectorMode.ToString());
                }
                else
                {
                    ScanModeChange(ScanModeEnum.ScanPause);
                }
                //ScanModeChange(ScanModeEnum.ScanPause);
                //ScanModeChange(ScanModeEnum.FastScan);
               
              
          
            }

            BSEControl.SelectedTab = BSEControl.Tabs["BSEControl"];
            //BSECheckSum();

            //FilterListAdd();
            //FilterSum();

            //ChangeDetectAndVacuumStateDisplay();
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now; 
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS); 
            DateTime AfterWards = ThisMoment.Add(duration); 
            
            while (AfterWards >= ThisMoment) 
            {
                System.Windows.Forms.Application.DoEvents(); 
                ThisMoment = DateTime.Now; 
            } 
            
            return DateTime.Now;
        }
      

        private void HogjVacChange(object sender, EventArgs e)
        {

            if (HighVac.Checked)
            {
                HighVac.Checked = true;
                LowVac.Checked = false;
                SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum;
                //MessageBox.Show(this, TextManager.Instance.GetString("Message_HighVacuum").Text);
            }
            else
            {
                return;
            }

            preVacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum.ToString();

        }

        private void LowVacChange(object sender, EventArgs e)
        {
            if (LowVac.Checked)
            {
                HighVac.Checked = false;
                LowVac.Checked = true;
                SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.LowVacuum;
                //MessageBox.Show(this, TextManager.Instance.GetString("Message_HighVacuum").Text);
            }
            else
            {
                return;
            }

            preVacuumMode = SystemInfoBinder.VacuumModeEnum.LowVacuum.ToString();

        }

        public void LanguageChange(object sender, EventArgs e)
        {
            BitmapCheckBox lag = sender as BitmapCheckBox;



            switch (lag.Name)
            {
                case "LanguageButton1":

                    //if(LanguageButton1.Checked)
                    //{
                    //    return;

                    //}

                    LanguageButton1.Checked = true;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;



                    //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);



                    TextManager.Instance.Language = LanguageButton1.Text;
                    TextManager.Instance.DefineText(this);

                    Language_Click(sender, e);

                    //frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[1];
                    //frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[0];

                    break;

                case "LanguageButton2":

                    //if (LanguageButton2.Checked)
                    //{
                    //    return;

                    //}

                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = true;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;




                    TextManager.Instance.Language = LanguageButton2.Text;
                    TextManager.Instance.DefineText(this);

                    Language_Click(sender, e);

                    //frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[1];
                    //frontPanelUtc.SelectedTab = frontPanelUtc.Tabs[0];
                    break;

                case "LanguageButton3":

                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = true;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = false;

                    TextManager.Instance.Language = LanguageButton3.Text;
                    TextManager.Instance.DefineText(this);

                    Language_Click(sender, e);

                    
                    break;

                case "LanguageButton4":

                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = true;
                    LanguageButton5.Checked = false;

                    TextManager.Instance.Language = LanguageButton4.Text;
                    TextManager.Instance.DefineText(this);

                    Language_Click(sender, e);


                    break;

                case "LanguageButton5":

                    LanguageButton1.Checked = false;
                    LanguageButton2.Checked = false;
                    LanguageButton3.Checked = false;
                    LanguageButton4.Checked = false;
                    LanguageButton5.Checked = true;

                    TextManager.Instance.Language = LanguageButton5.Text;
                    TextManager.Instance.DefineText(this);

                    Language_Click(sender, e);


                    break;



            }
            //BitmapButton LagBTN = 

            //switch (LagBTN.Text)
            //{

            //}
        }

        public void MToolsClose()
        {
            toolMeasuring.Checked = false;
            ArchivesMTools.Checked = false;
        }

        private void MToolsButtonClick(object sender, EventArgs e)
        {
            if (toolMeasuring.Checked || ArchivesMTools.Checked)
            {
                

                //sfMTools =  new SEC.Nanoeye.NanoeyeSEM.FormConfig.Measurement_Tools(this);
                sfMTools.PpSingle = ppSingle;
                sfMTools.FormLcation();
                sfMTools.Show();

                //sfMTools.


            }
            else
            {
                sfMTools.Hide();
            }




        }

        private void MainTabChange(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }

        private void ArchivesOpen_Click(object sender, EventArgs e)
        {

            //bitmapCheckBox1.Checked = false;

            string PathName = ArchivesAdress.Text;
            //if(!Directory.Exists(PathName))
            //{
            //    Directory.CreateDirectory(PathName);
            //}

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = PathName;
            folderDialog.ShowDialog();

            ArchivesAdress.Text = folderDialog.SelectedPath;
            Properties.Settings.Default.ArchAdress = ArchivesAdress.Text;

            ArchivesAdressChange(sender, e);


        }


        
        private void ArchivesAdressChange(object sender, EventArgs e)
        {
            if(ImageListLageBtn.Checked)
            {
                ImageListItemSizeChange(ImageListLageBtn, EventArgs.Empty);
            }
            else if (ImageListMiddleBtn.Checked)
            {
                ImageListItemSizeChange(ImageListMiddleBtn, EventArgs.Empty);
            }
            else
            {
                ImageListItemSizeChange(ImageListSmallBtn, EventArgs.Empty);
            }

            

        }

        private void ImageListChange()
        {
            string currDir = Properties.Settings.Default.ArchAdress;
            if (currDir == "")
            {
                currDir = Properties.Settings.Default.ArchAdress;

                return;
            }

            frSamllListView.Clear();


            GC.Collect();



            DirectoryInfo di = new DirectoryInfo(currDir);
            if (!di.Exists)
            {
                di.Create();
            }


            FileInfo[] files = di.GetFiles();
            PictureBox img;
            Bitmap[] bmp = new Bitmap[files.Length];


            Point point = new Point(listBox1.Left, listBox1.Top);

            int count = 0;
            int id = 0;

            if (imageList1.Images.Count != 0)
            {
                imageList1.Images.Clear();

            }

            if (imageList1.Images.Keys.Count != 0)
            {
                imageList1.Images.Clear();
            }



            foreach (var fi in files)
            {
                GC.Collect();
                if (System.Text.RegularExpressions.Regex.IsMatch(fi.Extension, @"(jpg|png)", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string path = fi.FullName;
                    Bitmap bmp1 = new Bitmap(path);

                    this.imageList1.Images.Add(bmp1);
                    int idx = this.imageList1.Images.Keys.Count - 1;
                    this.imageList1.Images.SetKeyName(idx, "");

                    frSamllListView.Items.Add(fi.Name, idx);

                    frSamllListView.EnsureVisible(frSamllListView.Items.Count - 1);

                    count++;


                }





            }


            if (frSamllListView.Items.Count == 0) { return; }

            frSamllListView.EnsureVisible(frSamllListView.Items.Count - 1);

            

            
        }

      
        


        private void StartProgress(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();

            Brush brush = Brushes.Red;
            g.FillRectangle(brush, m_ToolStartup.Left, m_ToolStartup.Top, 100, 100);
            g.FillEllipse(brush, m_ToolStartup.Left, m_ToolStartup.Top, 100, 100);

            g.Dispose();
        }

        private void ImageMiddleTabChange(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (!cb.Checked)
            {
                cb.Checked = true;
                return;
            }



            if (cb == ImageTabSpot)
            {
                ImageTabSig.Checked = false;
                ImageTabBeamShift.Checked = false;
                Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imSpot"];

            }
            else if (cb == ImageTabSig)
            {
                ImageTabSpot.Checked = false;
                ImageTabBeamShift.Checked = false;

                Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imStig"];
            }
            else if (cb == ImageTabBeamShift)
            {
                ImageTabSpot.Checked = false;
                ImageTabSig.Checked = false;

                Img_MiddleTab.SelectedTab = Img_MiddleTab.Tabs["imBshift"];
            }




        }


        private void ImageListView_Click(object sender, EventArgs e)
        {
            ////this.frMainMagDisLab.
            //if (MainImageListBtn.Checked)
            //{

            //    this.frSamllListView.Location = new Point(this.ppSingle.Right - (frSamllListView.Width), this.ppSingle.Top);
            //    this.frSamllListView.Size = new Size(this.frSamllListView.Width, (this.ppSingle.Height - 70));
            //    MainImageListBtn.Location = new Point(frSamllListView.Left - MainImageListBtn.Width - 10, MainImageListBtn.Location.Y);
            //    frSamllListView.Visible = true;
            //    SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            //    frSamllListView.Show();
              
            //    ArchTabmag.Location = new Point(frSamllListView.Left - ArchTabmag.Width, ArchTabmag.Location.Y);
               

            //    frSamllListView.Visible = true;
            //    this.ppSingle.Invalidate();
                

            //    frSamllListView.Scrollable = true;




            //    ArchivesAdressChange(sender, e);



            //}
            //else
            //{
            //    MainImageListBtn.Location = new Point(ppSingle.Right - MainImageListBtn.Width - 10, MainImageListBtn.Location.Y);
            //    ArchTabmag.Location = new Point(ppSingle.Right - ArchTabmag.Width - 10, ArchTabmag.Location.Y);
              
            //    frSamllListView.Visible = false;

            //}

            //this.Invalidate();





        }

        private void frTabPageChange(object sender, EventArgs e)
        {
          
            
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            GC.Collect();

            if (!bcb.Checked)
            {
                bcb.Checked = true;
                return;
            }

            
            if (sender == ImageTabBtn)
            {
                ArchivesTabBtn.Checked = false;
                SettingTabBtn.Checked = false;
                ImageTabBtn.Checked = true;

                ppSingle.ArchivesTabEnable = false;
                this.ppSingle.MicronEnable = true;


                //ArchivesListView.Clear();
                
                //GC.Collect();

                //ScanModeChange(ScanModeEnum.FastScan);
               

                //if (ArchivesPictureBox == null)
                //{

                //}
                //else if (ArchivesPictureBox.Enabled)
                //{
                //    ArchivesPictureBox.Hide();
                //}


                //ScanModeChange(ScanModeEnum.FastScan);
                //SystemInfoBinder.Default.in

               
                ppSingle.MicronEnable = true;

                UIsetBinder.Default.MagIndex++;
                M_Settings.SelectedTab = M_Settings.Tabs["frImage"];
                ArchTabmag.SelectedTab = ArchTabmag.Tabs["ImageTab"];
                framChange = false;

                

                if (ArchivesTabCB.Checked)
                {
                    ArchivesTabCB.Checked = false;
                    sfArchivesCB.Hide();
                }

                if (ArchivesMTools.Checked)
                {
                    ArchivesMTools.Checked = false;
                    sfMTools.Hide();
                }

                
                UIsetBinder.Default.MagIndex--;
            }
            else if (sender == ArchivesTabBtn)
            {

                ScanModeChange(ScanModeEnum.ScanPause);

                ppSingle.ArchivesTabEnable = true;

                ArchivesTabBtn.Checked = true;
                ImageTabBtn.Checked = false;
                SettingTabBtn.Checked = false;
                framChange = false;

                frSamplingBtn.Checked = false;
                toolMeasuring.Checked = false;
                imageTabRecordBtn.Checked = false;
                bsWindow.Checked = false;

                //this.Load += new EventHandler(frMainMagWheel);

                M_Settings.SelectedTab = M_Settings.Tabs["frArchives"];
                ArchTabmag.SelectedTab = ArchTabmag.Tabs["ImageTab"];

                if (ArchivesListView.Items.Count == 0)
                {
                    ArchivesAdressChange(sender, e);
                
                }

               
               
                

                

            }
            else
            {

                ppSingle.ArchivesTabEnable = false;

                ImageTabBtn.Checked = false;
                ArchivesTabBtn.Checked = false;
                SettingTabBtn.Checked = true;
                framChange = false;

                frSamplingBtn.Checked = false;
                toolMeasuring.Checked = false;
                imageTabRecordBtn.Checked = false;
                bsWindow.Checked = false;

                this.ppSingle.MicronEnable = true;

                M_Settings.SelectedTab = M_Settings.Tabs["frSettings"];
                ArchTabmag.SelectedTab = ArchTabmag.Tabs["ImageTab"];

                 if (ArchivesTabCB.Checked)
                {
                    ArchivesTabCB.Checked = false;
                    sfArchivesCB.Hide();
                }

                if (ArchivesMTools.Checked)
                {
                    ArchivesMTools.Checked = false;
                    sfMTools.Hide();
                }

                
            }

        }




        private void SettingTabChange(object sender, EventArgs e)
        {

            if (sender == StAdjustmentBtn)
            {

                if (StAdjustmentBtn.Checked)
                {
                    StSystemInfoBtn.Checked = false;
                    //StAdjustmentBtn.Checked = true;
                }
                else
                {
                    StAdjustmentBtn.Checked = true;
                    return;
                }


                StSysteminfoTab.SelectedTab = StSysteminfoTab.Tabs["SettingTabAd"];

            }
            else
            {


                if (StSystemInfoBtn.Checked)
                {

                    StAdjustmentBtn.Checked = false;
                }
                else
                {
                    StSystemInfoBtn.Checked = true;
                    return;
                }



                StSysteminfoTab.SelectedTab = StSysteminfoTab.Tabs["SettingTabSI"];
                //FilamentRuntimeChecker filamentRunningTime;
                //filamentRunningTime.










            }
        }



        private void SpotValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            int left = tb.Left;
            int top = tb.Top - 25;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum));


            left = ((tb.Left + 34) -21) + (value * 2);


            ss3SpotValue.Location = new Point(left, top);
            ss3SpotValue.Text = value.ToString();

            
            //SpotValueLocationChange(tb);

        }

       

        private void resetMouseUp(object sender, MouseEventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            if (bcb.Checked)
            {
                bcb.Checked = false;
            }
        }

        private PictureBox ArchivesPictureBox = new PictureBox();

        private ListView list;

        private Bitmap _Bmp = null;
        public Bitmap BmP
        {
            get { return _Bmp; }
            set { _Bmp = value; }
        }

        private byte[] _ArchivesImageData = null;
        public byte[] ArchivesImageData
        {
            get { return _ArchivesImageData; }
            set
            {
                _ArchivesImageData = value;
            }
        }

        string path = null;

        private void ArchivesItemChange()
        {
            if (!frMainScanPauseBb.Checked)
            {
                ScanModeChange(ScanModeEnum.ScanPause);
            }

            if (list == null)
            {
                return;
            }

            Properties.Settings.Default.MicronEnable = false;



            if (sfArchivesCB != null)
            {
                sfArchivesCB.ArchivesTabEnable = false;
            }



            _Bmp = new Bitmap(path);
            

            int scanmode = 1;


            switch (_Bmp.Width)
            {
                case 320:
                    scanmode = 0;
                    OpenImageTimer = 1;
                    break;

                case 640:
                    scanmode = 1;
                    OpenImageTimer = 2;
                    break;

                case 1280:
                    scanmode = 2;
                    OpenImageTimer = 4;
                    break;

                case 2560:
                    scanmode = 3;
                    OpenImageTimer = 8;
                    break;

                case 5120:
                    scanmode = 10;
                    OpenImageTimer = 16;
                    break;

                default:
                    break;
            }


            SECimage.SettingScanner ss;
            ss = SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[scanmode]);
            ss.SampleComposite = 1;
            scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);
            SECimage.IScanItemEvent isie = scanner.ItemsReady[0];
            ppSingle.EventLink(isie, isie.Name);
            scanner.Change();

            OpenImageChangeTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ArchivesImageChangeTimerExpiered));
            OpenImageChangeTimer.Change(0, 1000);

            OpenImageForm = new Form();
            OpenImageForm.StartPosition = FormStartPosition.CenterParent;
            OpenImageForm.Owner = this;
            OpenImageLable = new Label();
            OpenImageLable.Dock = DockStyle.Fill;
            OpenImageLable.TextAlign = ContentAlignment.MiddleCenter;
            OpenImageLable.Location = new Point(ppSingle.Left + ppSingle.Width / 2, ppSingle.Top + ppSingle.Height / 2);
            OpenImageForm.Controls.Add(OpenImageLable);
            OpenImageForm.FormBorderStyle = FormBorderStyle.None;
            OpenImageForm.Size = new Size(300, 50);
            OpenImageForm.TopMost = true;

            OpenImageForm.ShowDialog();
            ScanModeChange(ScanModeEnum.ScanPause);

            Properties.Settings.Default.MicronEnable = false;


         

            ppSingle.BeginInit();
            ppSingle.ImportPicture(_Bmp);
            ppSingle.EndInit();

            _Bmp.Dispose();
            ArchivesImageData = SEC.GenericSupport.Converter.ArrayToBytearray(ppSingle.ExportData());

            this.TopMost = true;
            this.Invalidate();

            this.TopMost = false;
            this.Invalidate();

            
        }

        private void ArchivesListView_Change(object sender, EventArgs e)
        {
           
            list = sender as ListView;
            string name;

            name = list.FocusedItem.Text;

            if (path == ArchivesAdress.Text + "\\" + name)
            {
                return;
            }

            path = ArchivesAdress.Text + "\\" + name;

            ArchivesItemChange();

        }

        System.Threading.Timer OpenImageChangeTimer;
        Form OpenImageForm;
        Label OpenImageLable;
        int OpenImageTimer = 0;

        void ArchivesImageChangeTimerExpiered(object obj)
        {
            //highVacLable.Location = new Point(ppSingle.Width /2, ppSingle.Height /2 );
            Action vacChangeAct = () =>
            {
                switch (OpenImageTimer)
                {
                    case 0:
                        OpenImageForm.Close();
                        OpenImageForm.Dispose();
                        OpenImageChangeTimer.Dispose();
                        break;

                    case 1:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 2:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 3:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 4:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 5:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 6:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    case 7:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                    
                    case 8:
                    default:
                        OpenImageLable.Text = OpenImageTimer.ToString() + "\r\nOf image conversion";
                        break;

                }
                OpenImageLable.Invalidate();
                OpenImageTimer--;
            };

            this.Invoke(vacChangeAct);
        }


        private void ArchivesPictureBox_Click(object sender, EventArgs e)
        {
            ArchivesPictureBox.Focus(); 
        }

        private void MicroscopeSettingBtn(object sender, EventArgs e)
        {
           

            using (FormLogIn login = new FormLogIn())
            {
                this.Invalidate();
                if (login.ShowDialog() == DialogResult.Cancel)
                {
                    m_MenuConfigMicroscope.Checked = false;
                    m_MenuConfigMicroscope.ForeColor = Color.FromArgb(26, 45, 60);
                    return;
                }
            }


            if (!sfMicroscope.IsCreated)
            {
                sfMicroscope.Create();
                (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).HVtextChanged += new EventHandler(ColumnConfig_HVtextChanged);
                (sfMicroscope.FormInstance as FormConfig.IMicroscopeSetupWindow).ProfileListChanged += new EventHandler(ColumnConfig_ProfileListChanged);
            }

            sfMicroscope.Show(this);

            m_MenuConfigMicroscope.Checked = false;
            m_MenuConfigMicroscope.ForeColor = Color.FromArgb(26, 45, 60);


        }

        private void ScanningSettingBtn(object sender, EventArgs e)
        {
            

            using (FormLogIn login = new FormLogIn())
            {
                if (login.ShowDialog() == DialogResult.Cancel)
                {
                    m_MenuConfigScanning.Checked = false;
                    m_MenuConfigScanning.ForeColor = Color.FromArgb(26, 45, 60);
                    return;
                }
            }


            sfScanner.Show(this);
            m_MenuConfigScanning.Checked = false;
            m_MenuConfigScanning.ForeColor = Color.FromArgb(26, 45, 60);
        }

        private void focusmodeDl_SeletedIndexChanged(object sender, EventArgs e)
        {
            switch (focusmodeDl.SelectedIndex)
            {
                case 0:
                    frMainFocusKnobEkwicvd.ControlValue = (SECtype.IControlDouble)column["LensObjectCoarse"];
                    frMainFocusKnobEkwicvd.DataBindings.Clear();
                    frMainFocusKnobEkwicvd.DataBindings.Add(new System.Windows.Forms.Binding("SensitiveLeft", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "SenseFocusCoarseLeft", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                    frMainFocusKnobEkwicvd.DataBindings.Add(new System.Windows.Forms.Binding("SensitiveRight", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "SenseFocusCoarseRight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                    frMainFocusAFBb.Tag = "AF-WD";
                    frMainFocusAFBb.ForeColor = Color.FromArgb(188, 188, 188);

                    break;
                case 1:
                    frMainFocusKnobEkwicvd.ControlValue = (SECtype.IControlDouble)column["LensObjectFine"];
                    frMainFocusKnobEkwicvd.DataBindings.Clear();
                    frMainFocusKnobEkwicvd.DataBindings.Add(new System.Windows.Forms.Binding("SensitiveLeft", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "SenseFocusFineLeft", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                    frMainFocusKnobEkwicvd.DataBindings.Add(new System.Windows.Forms.Binding("SensitiveRight", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "SenseFocusFineRight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                    frMainFocusAFBb.Tag = "AF-Fine";
                    frMainFocusAFBb.ForeColor = Color.FromArgb(188, 188, 188);

                    break;
                default:
                    Trace.Fail("Undefined focus mode.");
                    break;
            }


        }


        private void OPTION_IE_SetImport_but_Click(object sender, EventArgs e)
        {


            string fileName = null;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "NanoeyeSEM.bin(*.bin)|*.bin|NanoeyeSEM.config(*.config)|*.config";
                dialog.AddExtension = true;
                dialog.CheckFileExists = false;
                dialog.DefaultExt = "*.bin";
                dialog.FileName = "NanoeyeSEM.bin";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = dialog.FileName;

                    MiniSEM.ActiveForm.Dispose();
                    manager.Load(fileName);
                    manager.Save(SystemInfoBinder.Default.SettingFileName);
                    Application.Restart();

                }


            }

            ImportSettingBtn.Checked = false;

        }

        private void OPTION_IE_SetExport_but_Click(object sender, EventArgs e)
        {


            string fileName = null;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "bin";
            dialog.Filter = "bin files (*.bin)|*.bin";
            dialog.FileName = "NanoeyeSEM.bin";
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                dialog.Dispose();
                ExportSettingBtn.Checked = false;
                return;
            }



            fileName = dialog.FileName;
            SystemInfoBinder.Default.SetManager.Save(fileName);

            ExportSettingBtn.Checked = false;
        }

        private void OPTION_IE_LogExport_But_Click(object sender, EventArgs e)
        {

            string fileName = null;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.AddExtension = true;
            OFD.DefaultExt = "log";
            OFD.Filter = "log files (*.log)|*.log";

            string logPath = Application.CommonAppDataPath + @".\Log";
            OFD.InitialDirectory = logPath;
            if (OFD.ShowDialog(this) != DialogResult.OK)
            {
                OFD.Dispose();
                ExportLogBtn.Checked = false;
                return;
            }


            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "log";
            dialog.Filter = "log files (*.log)|*.log";



            //string logPath = Application.CommonAppDataPath + @".\Log";

            //logPath += "\\";
            //logPath += DateTime.Now.Year.ToString("00") + "-";
            //logPath += DateTime.Now.Month.ToString("00") + "-";
            //logPath += DateTime.Now.Day.ToString("00");
            //logPath += ".log";

            //fileName = logPath;

            fileName += DateTime.Now.Year.ToString("00") + "-";
            fileName += DateTime.Now.Month.ToString("00") + "-";
            fileName += DateTime.Now.Day.ToString("00");
            fileName += ".log";

            //string SavePath = Application.StartupPath;


            dialog.InitialDirectory = Application.StartupPath;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                dialog.Dispose();
                ExportLogBtn.Checked = false;
                return;
            }

            File.Copy(OFD.FileName, dialog.FileName);

            this.Close();

            ExportLogBtn.Checked = false;
        }

       
       



        private void BSEChChange(object sender, EventArgs e)
        {
            if (sender == BSEChan1)
            {

            }
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

            ((SECtype.IControlInt)column["BSE_Detector"]).Value = avg;
            Trace.WriteLine("BSE Value : " + avg.ToString());
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


            ((SECtype.IControlInt)column["BSE_Detector"]).Value = _bseData;
            Trace.WriteLine("BSE Value : " + _bseData.ToString());

            //BSECheckSum();

        }




        private void Cl1ValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            CL1Text.Location = new Point(left, top);

            CL1Text.Text = value.ToString();
            
        }


        private void Cl2ValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            CL2Text.Location = new Point(left, top);

            CL2Text.Text = value.ToString();
            
        }

        private void StigXValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            StgXValue.Location = new Point(left, top);


            StgXValue.Text = value.ToString();

            
        }

        private void StigYValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }


            
            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            SigYValue.Location = new Point(left, top);

            SigYValue.Text = value.ToString();

            
        }

        private void BSXValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }
            
            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            //left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            //bsXValue.Location = new Point(left, top);

            BeamShiftXLab.Text = value.ToString();
            
        }

        private void BSYValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            //left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            //bsYValue.Location = new Point(left, top);

            BeamShiftYLab.Text = value.ToString();
            
        }

        private void GunValueChange(object sender, EventArgs e)
        {

            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            //int left = tb.Left;
            //int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            //left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            //GunX.Location = new Point(left, top);


            GunAlignXLab.Text = value.ToString();

        }

        private void GunYValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            //int left = tb.Left;
            //int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            //left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            //GunY.Location = new Point(left, top);

            GunAlignYLab.Text = value.ToString();

        }

        private void CltValueChange(object sender, EventArgs e)
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

        private void AmpTextChange(object sender, EventArgs e)
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

        private void FilamentChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            FilamentText.Location = new Point(left, top);


            FilamentText.Text = value.ToString();

        }

        private void GridChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            GridText.Location = new Point(left, top);

            GridText.Text = value.ToString();


        }

        private void statusServerClick(object sender, EventArgs e)
        {
            if (StatusServerBtn.Checked)
            {
                statuServer = new StatusServerViewer();
                //statuServer.statusAdressChange += new EventHandler(statusIPAdress);
                statuServer.Show(this);
                }
            else
            {
                statusServerChange();
                statuServer.Hide();
            }
            
        }

        private void statusServerChange()
        {
            byte[] addr = StatusServer.Default.ClientAddress.Address.GetAddressBytes();

            IpAddress.Text = addr[0] + "." + addr[1] + "." + addr[2] + "." + addr[3];
        }


        private void WobbleChage(object sender, EventArgs e)
        {

            lwf.Column = column;
            lwf.OL_Amplitude = m4000FocusWA.Value;
            lwf.OL_Frequency = m4000FocusWF.Value;

            
            if (WobbleBtn.Checked)
            {
                lwf.OL_Wobble = WobbleBtn.Checked;
            }
            else
            {
                lwf.OL_Wobble = WobbleBtn.Checked;
            }


        }

        public void InfoFormClose()
        {
            //InfoZoneBtn.Checked = false;
            //sfMicronModify.Hide();
            
        }

        public void InfoZonechecked(object sender, EventArgs e)
        {
            //if (InfoZoneBtn.Checked)
            //{
            //    sfMicronModify = new SEC.Nanoeye.NanoeyeSEM.InfoZone(this);
            //    sfMicronModify.Show();
            //}
            //else
            //{
            //    sfMicronModify.Hide();
            //}
        }

        private void MagValueChange(object sender, EventArgs e)
        {
            ImageTrackBarWithSingle itb = sender as ImageTrackBarWithSingle;


            UIsetBinder.Default.MagIndex = itb.Value;

        }

        
        


        private Point mouseCurrentPoint = new Point(0, 0);
        private void mainFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                

                if (samplingtime != null)
                {
                    samplingtime.Hide();
                    frSamplingBtn.Checked = false;
                }

                if (sfMTools != null)
                {
                    sfMTools.Hide();
                    toolMeasuring.Checked = false;
                }
                /*
                if (sfScreenRecoder != null)
                {
                    sfScreenRecoder.Hide();
                    imageTabRecordBtn.Checked = false;
                }
                */
                mouseCurrentPoint = e.Location;
            }
        }

        private void mainFormMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                


                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);

                if (SpotModeBtn.Checked)
                {
                    spotForm.Location = this.Location;
                }

                if (Properties.Settings.Default.MotorStageEnable)
                {
                    MotorStage.MotorLocationChange(this.Right, this.Top);
                }

            }
        }

        private bool _RecordingEnable = false;
        public bool RecordingEnable
        {
            get { return _RecordingEnable; }
            set { _RecordingEnable = value; }
        }

        public void RecordBtn_Close()
        {
            imageTabRecordBtn.Checked = false;
        }

        private void RecordBtn_Click(object sender, EventArgs e)
        {
            /*
            if (imageTabRecordBtn.Checked)
            {
                if (RecordingEnable)
                {
                    sfScreenRecoder.Show(this);
                }
                else
                {
                    sfScreenRecoder = new SEC.Nanoeye.NanoeyeSEM.ScreenRecorder(this);
                    //(sfScreenRecoder.FormInstance as ScreenRecorder).ImageWindow = ppSingle;

                    sfScreenRecoder.ImageWindow = ppSingle;

                    //sfScreenRecoder.FormLocation();
                    //sfScreenRecoder.Show(this);
                    sfScreenRecoder.Show(this);
                }
                
            }
            else
            {
                //RecordingEnable = sfScreenRecoder.RecordEnable;
                if (RecordingEnable)
                {
                    //RecordingEnable = (ScreenRecorder as sfScreenRecoder.RecordEnable).;
                    imageTabRecordBtn.Image = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.icon_rec_dot;
                    imageTabRecordBtn.Invalidate();
                }
                else
                {
                    imageTabRecordBtn.Image = null;
                    imageTabRecordBtn.Invalidate();
                }

                sfScreenRecoder.Hide();
            }
            */
        }

        private void FocusLeftValue(object sender, EventArgs e)
        {
            if (frMainFocusKnobEkwicvd.mouseCaptureMode == 0)
            {
                if (frMainFocusKnobEkwicvd.Value > frMainFocusKnobEkwicvd.Minimum)
                {
                    frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.ControlValue.Value - frMainFocusKnobEkwicvd.ControlValue.Precision;
                }

            }
            FocusControlLeft.Checked = false;

           
        }

        private void FocusRightValue(object sender, EventArgs e)
        {
            if (frMainFocusKnobEkwicvd.mouseCaptureMode == 0)
            {
                if (frMainFocusKnobEkwicvd.Value < frMainFocusKnobEkwicvd.Maximum)
                {
                    frMainFocusKnobEkwicvd.ControlValue.Value = frMainFocusKnobEkwicvd.ControlValue.Value + frMainFocusKnobEkwicvd.ControlValue.Precision;
                }
            }

           
            FocusControlRight.Checked = false;
        }

        private void FocusMouseUp(object sender, MouseEventArgs e)
        {
            if (frMainFocusKnobEkwicvd.mouseCaptureMode > 0)
            {
                frMainFocusKnobEkwicvd.MouseEventChange(e);
            }
        }

        

        private void MainFormMinize(object sender, EventArgs e)
        {
           

            if (sfMTools != null)
            {
                MToolsClose();
                sfMTools.Hide();
            }
            /*
            if (sfScreenRecoder != null)
            {
                RecordBtn_Close();
                sfScreenRecoder.Hide();
            }
           */


            this.WindowState = FormWindowState.Minimized;
            mainMinimizeButton.Checked = false;
        }




        System.Windows.Forms.Timer MouseTimer;
        private bool RotateMode = false;

        private void RotateMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) { return; }

            BitmapCheckBox bcb = sender as BitmapCheckBox;

            if (bcb.Name == "frMainRotateDecBb")
            {
                RotateMode = false;
            }
            else
            {
                RotateMode = true;
            }


            MouseTimer = new System.Windows.Forms.Timer();
            MouseTimer.Tick += new EventHandler(RotateLevelChange);
            MouseTimer.Interval = 100;
           

            MouseTimer.Start();

        }

        private void RotateLevelChange(object sender, EventArgs e)
        {
            double val = (equip.ColumnScanRotation as SECtype.IControlDouble).Value;

            if (!RotateMode)
            {
                val++;
                MagCount--;
            }
            else
            {
             
                val--;
                MagCount++;
                
            }
           
            if (val > 180) { val -= 360; }

            if (val < -180) { val = 180; }

            if (MagCount < 0) { MagCount = 360; }
            if (MagCount > 360) { MagCount = 0; }

            (equip.ColumnScanRotation as SECtype.IControlDouble).Value = (val);

            frMainRotateDisLab.Text = ((int)(MagCount)).ToString() + "\x00B0";

        }

        private void RotateMouseUp(object sender, MouseEventArgs e)
        {
            frMainRotateDecBb.Checked = false;
            frMainRotateIncBb.Checked = false;
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            MouseTimer.Stop();
            
        }

        private void ArchivesOriginal(object sender, EventArgs e)
        {
            ArchivesPictureBox.Width = ppSingle.Width;
            ArchivesPictureBox.Height = ppSingle.Height;

            ArchivesPictureBox.Left = ppSingle.Left;
            ArchivesPictureBox.Top = ppSingle.Top;
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


            Trace.WriteLine("BSEAmpValue : " + BSEAmpData.ToString());
            ((SECtype.IControlInt)column["BSE_Amp"]).Value = BSEAmpData;

        }

        private void OnFilamentTimerReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FilamentRunningTime = TimeSpan.Zero;

            string result = null;

            result += Properties.Settings.Default.FilamentRunningTime.Days.ToString() + "days ";
            result += Properties.Settings.Default.FilamentRunningTime.Hours.ToString().PadLeft(2, '0') + ":";
            result += Properties.Settings.Default.FilamentRunningTime.Minutes.ToString().PadLeft(2, '0');
            FilamentRunTimeLabel.Text = result;
            FilamentRunTimeLabel.Invalidate();
            

            FilamentRuntimeResetBtn.Checked = false;

        }

        private void WobbleCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            if (WobbleCheckedBox.Checked)
            {
                m4000FocusWF.Enabled = false;
                m4000FocusWA.Enabled = false;
            }
            else
            {
                m4000FocusWF.Enabled = true;
                m4000FocusWA.Enabled = true;
            }


        }

        private void BSEFilter_Click(object sender, EventArgs e)
        {
             
             CheckBox cbb = sender as CheckBox;
             int value = 0;


             if (BSE_Filter1.Checked)
             {
                 value += 1;
             }

             if (BSE_Filter2.Checked)
             {
                 value += 2;
             }

             if (BSE_Filter4.Checked)
             {
                 value += 4;
             }

             ((SECtype.IControlInt)column["BSE_Filter"]).Value = value;

        }

        private void BSEAmpCValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int valueLocation = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum))); ;
            int value = (int)(tb.Value);


            left = (((tb.Left + 34) - 21) + ((valueLocation - 50) * 2)) + 100;

            BSEAmpC.Location = new Point(left, top);


            BSEAmpC.Text = value.ToString();
        }

        private void BSEAmpDValueChange(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int valueLocation = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum))); ;
            int value = (int)(tb.Value);

            left = (((tb.Left + 34) - 21) + ((valueLocation - 50) * 2)) + 100;

            BSEAmpD.Location = new Point(left, top);


            BSEAmpD.Text = value.ToString();
        }

        public void ArchivesFormClose()
        {
            ArchivesTabCB.Checked = false;
        }


        private void ArchivesTabCB_Click(object sender, EventArgs e)
        {
            if (BmP == null)
            {
                ArchivesTabCB.Checked = false;
                return;
            }

            if (ArchivesTabCB.Checked)
            {

                sfArchivesCB = new SEC.Nanoeye.NanoeyeSEM.FormConfig.ArchivesTabCB(this);
                //(sfScreenRecoder.FormInstance as ScreenRecorder).ImageWindow = ppSingle;


                sfArchivesCB.FormLocation();

                sfArchivesCB.PpSingle = ppSingle;
                sfArchivesCB.ImageSize = new Size(BmP.Width, BmP.Height);
                

                //sfArchivesCB = new SEC.GenericSupport.SingleForm();
                //sfArchivesCB.Location();
                //sfArchivesCB.FormLocation();
                sfArchivesCB.Show();
            }
            else
            {
                sfArchivesCB.Hide();
            }
        }

        private void CalibrationBtn_Click(object sender, EventArgs e)
        {
            if (CalibrationBtn.Checked)
            {
                m_MenuConfigMicroscope.Visible = true;
                m_MenuConfigScanning.Visible = true;
                ImageTab.Visible = true;
                

            }
            else
            {
                m_MenuConfigMicroscope.Visible = false;
                m_MenuConfigScanning.Visible = false;
                ImageTab.Visible = false;
                
            }
           
        }

        public void CalibrationEnableChange(bool enable)
        {
            if (enable)
            {
                m_MenuConfigMicroscope.Visible = true;
                m_MenuConfigScanning.Visible = true;
                ImageTab.Visible = true;
                //Operation.Visible = false;
                CalibrationBtn.Checked = true;
            }
            else
            {
                m_MenuConfigMicroscope.Visible = false;
                m_MenuConfigScanning.Visible = false;
                ImageTab.Visible = false;
                //Operation.Visible = false;
                CalibrationBtn.Checked = false;
            }

           

        }

        private void SpotModeBtn_Click(object sender, EventArgs e)
        {
            if(SpotModeBtn.Checked)
            {
                SpecialModeSpot();
            }
            else
            {
                spotForm.Dispose();
                SpotFormClose();
            }


        }

        //AForge.Video.DirectShow.FilterInfoCollection videoDevices;
        //
        //AForge.Controls.VideoSourcePlayer videoSourcePlayer2;


        private System.Windows.Forms.Timer CameraTimer;
        
        private IplImage imgSrc;
        

        
        
        private void StartCameras(object sender, EventArgs e)
        {

            if (CamarBtn.Checked)
            {
                try
                {
                    pictureBoxIpl1.Visible = true;

                    

                    ((SECtype.IControlInt)column["VacuumCamera"]).Value = 1;

                    
                    
                    //StartCameras();
                }
                catch
                {
                    MessageBox.Show("The camera is not connected.");
                }
               
            }
            else
            {
                pictureBoxIpl1.Visible = false;
                //videoSourcePlayer1.Dispose();
                //videoSourcePlayer1.SignalToStop();
                //videoSourcePlayer1.WaitForStop();B

                ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;

               
                
                //StopCamera();

            }

        }

        private void initCamera()
        {
           
            capture = CvCapture.FromCamera(CaptureDevice.DShow, 0);
            capture.SetCaptureProperty(CaptureProperty.FrameWidth, 320);
            capture.SetCaptureProperty(CaptureProperty.FrameHeight, 240);
            

            imgSrc = new IplImage(320, 240, BitDepth.U8, 3);

            CameraTimer = new System.Windows.Forms.Timer();
            CameraTimer.Interval = 33;
            CameraTimer.Tick += new EventHandler(timer1_tick);

            
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            capture.Brightness = Properties.Settings.Default.CameraBirghtness;
            capture.Contrast = Properties.Settings.Default.CameraContrast;

            imgSrc = capture.QueryFrame();


            if (Properties.Settings.Default.CameraTiltEnable)
            {
                int left = Properties.Settings.Default.CameraLeft;
                int right = Properties.Settings.Default.CameraRight;
                int width = Properties.Settings.Default.CameraWidth;
                int height = Properties.Settings.Default.CameraHight;

                for (int a = 0; a < 5; a++)
                {
                    imgSrc.DrawLine(Cv.Point(left, height + (a * width)), Cv.Point(right, height + (a * width)), Cv.RGB(50, 235, 251));
                    //imgSrc.AddText(Convert.ToString(a + 10), Cv.Point(right + 10, height + (a * width)) , CvFont font);
                    imgSrc.PutText(Convert.ToString((a * 10 - 40) * (-1)), Cv.Point(right + 10, height + (a * width) + 3), new CvFont(FontFace.HersheyComplex, 0.2, 0.2), new CvColor(50, 235, 251));

                }
            }

            

           

            //CvFont font1 = new CvFont(FontFace.
            
           

            


          
            

            pictureBoxIpl1.ImageIpl = imgSrc;
            
            //g.DrawImage2+hye(_OverlayImage, new Rectangle(0 + (this.ClientSize.Width / 2) - (int)((imageSize.Width * _DigitalZoomRunning) / 4), (-45) + (this.ClientSize.Height / 2) - (int)((imageSize.Height * _DigitalZoomRunning) / 4), (int)((imageSize.Width * _DigitalZoomRunning) / 2), (int)((imageSize.Height * _DigitalZoomRunning) / 2)));
           
            
        }

        private void StopCamera()
        {

            //Trace.WriteLine("Camera Stop Start---" + DateTime.Now.Second);
            if (CameraTimer != null)
            {
                pictureBoxIpl1.Visible = false;
                CameraTimer.Enabled = false;
                //Trace.WriteLine("Camera Timer Stop---" + DateTime.Now.Second);
            }

            //if (capture != null)
            //{
            //    //Cv.ReleaseCapture(capture);
               
            //    System.Diagnostics.Debug.WriteLine("Cameara Stop Start" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());
            //    //capture.Dispose();
            //    System.Diagnostics.Debug.WriteLine("Cameara Stop end" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());
                
            //    //capture.
            //}

            //Trace.WriteLine("Camera imgsrc Dispose Start---" + DateTime.Now.Second);
            if (imgSrc != null)
            {
                imgSrc.Dispose();
            }
            //Trace.WriteLine("Camera imgsrc Dispose Stop---" + DateTime.Now.Second);


            //Trace.WriteLine("Camera power Off Start---" + DateTime.Now.Second);
            if (CamarBtn.Checked)

            {
                
                
                ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;
                //Trace.WriteLine("Camera power Off Stop---" + DateTime.Now.Second);
            }
           
         
        }

        private void StartCameras()
        {
            try
            {
                initCamera();
            }
            catch
            {
                //MessageBox.Show("The camera is not connected.");
                return;
            }

            CameraTimer.Interval = 33;
            CameraTimer.Enabled = true;

        }


        private void CameraCBcontrol(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //SEC.Nanoeye.NanoeyeSEM.FormConfig.CameraCBcontrol CCBcontrol = new SEC.Nanoeye.NanoeyeSEM.FormConfig.CameraCBcontrol(this);
                SEC.Nanoeye.NanoeyeSEM.FormConfig.CameraCBcontrol CCBcontrol = new SEC.Nanoeye.NanoeyeSEM.FormConfig.CameraCBcontrol();

                if (CCBcontrol.CameraEnbale == false)
                {
                    CCBcontrol.Show();
                }
                

            }
        }

        private int _CameraContrast = Properties.Settings.Default.CameraContrast;
        public int CameraContrast
        {
            get { return _CameraContrast; }
            set
            {
                _CameraContrast = value;
                //CameraControlChange();
            }
        }

        private int _CameraBrightness = Properties.Settings.Default.CameraBirghtness;
        public int CameraBrightness
        {
            get { return _CameraBrightness; }
            set
            {
                _CameraBrightness = value;
                //CameraControlChange();
            }
        }

        private void MagBtn_Click(object sender, EventArgs e)
        {
            if (!MagBtnEnable)
            {
                MagBtnEnable = true;
                MagBtn.Text = "Mag-";
            }
            else
            {
                MagBtnEnable = false;
                MagBtn.Text = "Mag+";

            }
        }



        private void EDSChange()
        {
            if (SystemInfoBinder.Default.VacuumMode == SystemInfoBinder.VacuumModeEnum.HighVacuum)
            {
                if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.SED)
                {
                    ss2CL1.Value = Properties.Settings.Default.EDSHighSECL1;
                    ss2CL2.Value = Properties.Settings.Default.EDSHighSECL2;
                    detectClt.Value = Properties.Settings.Default.EDSHighSEClt;
                    detectPmt.Value = Properties.Settings.Default.EDSHighSEPmt;
                }
                else
                {
                    ss2CL1.Value = Properties.Settings.Default.EDSHighBSECL1;
                    ss2CL2.Value = Properties.Settings.Default.EDSHighBSECL2;

                    BSEAmp1.Checked = false;
                    BSEAmp2.Checked = false;
                    BSEAmp3.Checked = false;
                    BSEAmp5.Checked = false;
                    BSEAmp8.Checked = false;
                    

                    switch (Properties.Settings.Default.EDSHighBSEAmp)
                    {
                        case 1:
                            BSEAmp8.Checked = true;
                            break;

                        case 2:
                            BSEAmp5.Checked = true;
                            break;

                        case 3:
                            BSEAmp3.Checked = true;
                            break;

                        case 4:
                            BSEAmp2.Checked = true;
                            break;

                        case 5:
                            BSEAmp1.Checked = true;
                            break;
                    }
                }

            }
            else
            {
                ss2CL1.Value = Properties.Settings.Default.EDSLowBSECL1;
                ss2CL2.Value = Properties.Settings.Default.EDSLowBSECL2;

                BSEAmp1.Checked = false;
                BSEAmp2.Checked = false;
                BSEAmp3.Checked = false;
                BSEAmp5.Checked = false;
                BSEAmp8.Checked = false;

                switch (Properties.Settings.Default.EDSLowBSEAmp)
                {
                    case 1:
                        BSEAmp8.Checked = true;
                        break;

                    case 2:
                        BSEAmp5.Checked = true;
                        break;

                    case 3:
                        BSEAmp3.Checked = true;
                        break;

                    case 4:
                        BSEAmp2.Checked = true;
                        break;

                    case 5:
                        BSEAmp1.Checked = true;
                        break;
                }

            }
        }

        double bsx = 0;
        double bsy = 0;

        private void ppSingle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Properties.Settings.Default.MotorStageEnable)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Point MainPoint = new Point(ppSingle.Size.Width / 2, (ppSingle.Size.Height - 81) / 2);
            Point MovePoint = new Point(e.X, e.Y);

            double ChangePointX = MovePoint.X - MainPoint.X;
            double ChangePointY = MovePoint.Y - MainPoint.Y;


            double pixelSize = ppSingle.LengthPerPixel * 1000;


            double Radian = Math.PI / 180 * MagCount;

            double tX = ChangePointX * Math.Cos(Radian) + ChangePointY * Math.Sin(Radian);
            double tY = ChangePointX * -1 * Math.Sin(Radian) + ChangePointY * Math.Cos(Radian);

            if (equip.Magnification < 5000)
            {

                ChangePointX = tX * pixelSize;
                ChangePointY = tY * pixelSize;

                //MotorStage.StageMoveChange(ChangePointX, ChangePointY);

                double Motorx = Properties.Settings.Default.MotorStageXvalue;
                double Motory = Properties.Settings.Default.MotorStageYvalue;

                Motorx = Motorx - ChangePointX;
                Motory = Motory + ChangePointY;


                MotorStage.StageTextChange(1, Motorx);
                Delay(50);
                MotorStage.StageTextChange(2, Motory);
            }
            else
            {

                pixelSize = pixelSize * 10000;

                if (Properties.Settings.Default.MBeamShiftX)
                {
                    bsx = (BeamShiftXSCB.Value + ((tX * pixelSize)));
                }
                else
                {
                    bsx = (BeamShiftXSCB.Value + ((tX * pixelSize) * (-1)));
                }

                if (Properties.Settings.Default.MBeamShiftY)
                {
                    bsy = (BeamShiftYSCB.Value + ((tY * pixelSize) * (-1)));
                }
                else
                {
                    bsy = (BeamShiftYSCB.Value + ((tY * pixelSize)));
                }
                
                (SystemInfoBinder.Default.Equip.ColumnBSX as SEC.GenericSupport.DataType.IControlDouble).Value = bsx * (SystemInfoBinder.Default.Equip.ColumnBSX as SEC.GenericSupport.DataType.IControlDouble).Precision;
                (SystemInfoBinder.Default.Equip.ColumnBSY as SEC.GenericSupport.DataType.IControlDouble).Value = bsy * (SystemInfoBinder.Default.Equip.ColumnBSY as SEC.GenericSupport.DataType.IControlDouble).Precision;
            }

        }

        private void MotorBtn_Click(object sender, EventArgs e)
        {
            if (MotorBtn.Checked)
            {
                Properties.Settings.Default.StagePortName = comboBox1.SelectedItem.ToString();

                MotorStage.MotorConnet(comboBox1.SelectedItem.ToString());
                Properties.Settings.Default.MotorStageEnable = true;
            }
            else
            {
                MotorStage.ProtClose();
            }
        }

        private void UserSettingsBtn_Click(object sender, EventArgs e)
        {

        }

        private bool _dualDisplay = false;
        public bool DualDisplay
        {
            get { return _dualDisplay; }
            set { _dualDisplay = value; }
        }

        private bool _MergeDisplay = false;
        public bool MergeDisplay
        {
            get { return _MergeDisplay; }
            set { _MergeDisplay = value; }
        }

        bool Multichannel = false;

        public void DetectorChange(object sender, EventArgs e)
        {

            BitmapCheckBox bcb = sender as BitmapCheckBox;

            ApplyScanningProfile(ScanModeEnum.ScanPause, false, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);

           

            if (bcb.Name == "DetectorSE")
            {
                if (!bcb.Checked)
                {
                    DetectorSE.Checked = true;
                    return;
                }

                
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = false;

                _dualDisplay = false;
                _MergeDisplay = false;
                LowVac.Enabled = false;


                scanner.Revers(true);
                scanner.DualMode(false);
                ppSingle.Dual = false;
                ppSingle.Merge = false;
               

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.SED;

                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);
                Multichannel = false;

                //BSEControl.SelectedTab = BSEControl.Tabs["DetectorMode"];

            }
            else if (bcb.Name == "DetectorBSE")
            {
                if (!bcb.Checked)
                {
                    DetectorBSE.Checked = true;
                    return;
                }

                
                DetectorSE.Checked = false;
                DetectorDual.Checked = false;
                DetectorMerge.Checked = false;


                _dualDisplay = false;
                _MergeDisplay = false;
                HighVac.Enabled = true;

                scanner.DualMode(false);
                ppSingle.Dual = false;
                ppSingle.Merge = false;
               

                if (CamarBtn.Checked)
                {
                    CamarBtn.Checked = false;
                    pictureBoxIpl1.Visible = false;
                    ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;

                }

                if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow)
                {
                    LowVac.Enabled = true;
                }
                else
                {
                    LowVac.Enabled = false;
                }

                if (SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow)
                {
                    LowVac.Enabled = true;
                }
                else
                {
                    LowVac.Enabled = false;
                }

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.BSED;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.Revers(false);

                Multichannel = false;
                //BSEControl.SelectedTab = BSEControl.Tabs["BSEControl"];
            }
            else if (bcb.Name == "DetectorDual")
            {

                if (!bcb.Checked)
                {
                    DetectorDual.Checked = true;
                    return;
                }

                _dualDisplay = true;
                _MergeDisplay = false;

                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorMerge.Checked = false;

               

                LowVac.Enabled = false;

                scanner.DualMode(true);

                scanner.Revers(true);
                ppSingle.Dual = true;
                ppSingle.Merge = false;

                if (CamarBtn.Checked)
                {
                    CamarBtn.Checked = false;
                    pictureBoxIpl1.Visible = false;
                    ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;

                }

               
                LowVac.Enabled = false;
                

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.DualSEBSE;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);
                Multichannel = true;

            }
            else if (bcb.Name == "DetectorMerge")
            {
                if (!bcb.Checked)
                {
                    DetectorMerge.Checked = true;
                    return;
                }

                _dualDisplay = false;
                _MergeDisplay = true;

                DetectorSE.Checked = false;
                DetectorBSE.Checked = false;
                DetectorDual.Checked = false;



                LowVac.Enabled = false;

                scanner.DualMode(true);

                scanner.Revers(true);
                ppSingle.Merge = true;
                ppSingle.Dual = false;
                

                if (CamarBtn.Checked)
                {
                    CamarBtn.Checked = false;
                    pictureBoxIpl1.Visible = false;
                    ((SECtype.IControlInt)column["VacuumCamera"]).Value = 0;

                }


                LowVac.Enabled = false;


                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.Merge;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);

                Multichannel = true;

            }



            if (frMainScanSfBb.Checked)
            {

                ScanModeChange(ScanModeEnum.FastScan);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanSsBb.Checked)
            {

                ScanModeChange(ScanModeEnum.SlowScan);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanPfBb.Checked)
            {

                ScanModeChange(ScanModeEnum.FastPhoto);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanPsBb.Checked)
            {

                ScanModeChange(ScanModeEnum.SlowPhoto);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else
            {
                ScanModeChange(ScanModeEnum.ScanPause);
            }


        }


       
        public bool DualDetectorEnable
        {
            get { return DualDetectorBtn.Checked; }
            set
            {
                DualDetectorBtn.Checked = value;
            }
        }

        private void DualDetectorBtn_CheckedChanged(object sender, EventArgs e)
        {

            BitmapCheckBox bcb = sender as BitmapCheckBox;

            if (bcb.Checked)
            {
                if (DualDetector == null)
                {
                    DualDetector = new SEC.Nanoeye.NanoeyeSEM.FormConfig.DaulDetector(this);
                    DualDetector.Column = column;
                }
                
                DualDetector.Show();
            }
            else
            {
                DualDetector.Hide();
            }
        }

        private void digitalZoomX2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ppSingle.DigitalZoomMode = 4;
        }

        private void oriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ppSingle.DigitalZoomMode = 0;
        }

        private void ppSingle_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        public void SamplingBtnClose()
        {
            frSamplingBtn.Checked = false;
        }

        public void SamplingBtnClick(object sender, EventArgs e)
        {
            if (frSamplingBtn.Checked)
            {
                //samplingtime.Shown
                //samplingtime.Click += new EventArgs(samplingtime.Shown);

                //frSamplingBtn.Checked = true;

                if (samplingtime == null)
                {
                    samplingtime = new SEC.Nanoeye.NanoeyeSEM.FormConfig.SamplingTime(this);
                    samplingtime.FormLocation();
                    samplingtime.Show();
                }
                else
                {
                    //samplingtime.FormLocation();
                    samplingtime.SamplingChecked();
                    samplingtime.Show();
                }


            }
            else
            {

                //frSamplingBtn.Checked = false;

                samplingtime.Hide();


            }

        }

        private void MotorXtxt_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox tb = sender as TextBox;

            switch (tb.Name)
            {
                case "MotorXtxt":
                    MotorXTextEnable = true;
                    break;

                case "MotorYtxt":
                    MotorYTextEnable = true;
                    break;

                case "MotorRtxt":
                    MotorRTextEnable = true;
                    break;

                case "MotorTtxt":
                    MotorTTextEnable = true;
                    break;

                case "MotorZtxt":
                    MotorZTextEnable = true;
                    break;

                default:
                    break;
            }
        }

        private void MotorXtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                TextBox tb = sender as TextBox;

                switch (tb.Name)
                {
                    case "MotorXtxt":
                        MotorStage.StageTextChange(1, Convert.ToDouble(tb.Text));
                        MotorXTextEnable = false;
                        break;

                    case "MotorYtxt":
                        MotorStage.StageTextChange(2, Convert.ToDouble(tb.Text));
                        MotorYTextEnable = false;
                        break;

                    case "MotorRtxt":
                        MotorStage.StageTextChange(3, Convert.ToDouble(tb.Text));
                        MotorRTextEnable = false;
                        break;

                    case "MotorTtxt":
                        MotorStage.StageTextChange(4, Convert.ToDouble(tb.Text));
                        MotorTTextEnable = false;
                        break;

                    case "MotorZtxt":
                        MotorStage.StageTextChange(5, Convert.ToDouble(tb.Text));
                        MotorZTextEnable = false;
                        break;

                    default:
                        break;
                }
            }
        }

        private void MotorAllStop(object sender, EventArgs e)
        {
            MotorStage.StageStop(6);
            MotorStopBtn.Checked = false;
        }



        
        private void MTools_Click(object sender, EventArgs e)
        {

            MtoolsColorBcb.Checked = false;
            MtoolsDeleteAllBcb.Checked = false;
            MtoolsDeleteOneBcb.Checked = false;
            MtoolsAreaBcb.Checked = false;
            MtoolsArrowBcb.Checked = false;
            MtoolsEllipseBcb.Checked = false;
            MtoolsLengthBcb.Checked = false;
            MtoolsLinearBcb.Checked = false;
            MtoolsRectangleBcb.Checked = false;
            MtoolsTextBcb.Checked = false;
            MtoolsMarquiosBcb.Checked = false;
            MtoolsPointBcb.Checked = false;


            BitmapCheckBox con = sender as BitmapCheckBox;
            con.Checked = true;

            
            switch (con.Name)
            {
                case "MtoolsColorBcb":
                    SEC.GUIelement.MeasuringTools.ItemBase ib = ppSingle.MTools.GetSelectItem();
                    if (ib != null)
                    {
                        ColorDialog cd = new ColorDialog();
                        cd.Color = ib.ItemColor;
                        if (cd.ShowDialog() == DialogResult.OK)
                        {
                            ib.ItemColor = cd.Color;
                            Properties.Settings.Default.MtoolsColor = cd.Color;
                        }
                    }
                    break;
                case "MtoolsDeleteAllBcb":
                    ppSingle.MTools.DeleteItemAll();
                    break;
                case "MtoolsDeleteOneBcb":
                    ppSingle.MTools.DeleteItem();
                    break;
                case "MtoolsAngularBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Angle);
                    break;
                case "MtoolsAreaBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.ClosePath, mtText.Checked);
                    break;
                case "MtoolsArrowBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Arrow);
                    break;
                case "mtICancel":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.SelectNull();
                    break;
                case "MtoolsEllipseBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Ellipse, mtText.Checked);
                    break;
                case "MtoolsLengthBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.OpenPath, mtText.Checked);
                    break;
                case "MtoolsLinearBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Line, mtText.Checked);
                    break;
                case "MtoolsRectangleBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Rectangle, mtText.Checked);
                    break;
                case "MtoolsTextBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.TextBox);
                    break;
                case "MtoolsMarquiosBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.MarquiosScale);
                    break;
                case "MtoolsPointBcb":
                    ppSingle.MTools.Color = Properties.Settings.Default.MtoolsColor;
                    ppSingle.MTools.AddItem(SEC.GUIelement.MeasuringTools.ItemStyle.Point, mtText.Checked);
                    break;

                case "MtoolsFontBcb":
                    FontDialog MtFont = new FontDialog();

                    MtFont.Font = ppSingle.MTools.Font;

                    if (MtFont.ShowDialog() == DialogResult.OK)
                    {
                        ppSingle.MTools.Font = MtFont.Font;

                        
                    }
                    MtoolsFontBcb.Checked = false;
                    break;
                     
                default:
                    throw new ArgumentException();
            }

        }


        private int FunctionContorlValue = 0;

        private void FCTControlValueChange(object sender, EventArgs e)
        {
            EllipseKnobWithICVD ekwc = sender as EllipseKnobWithICVD;

            SetFCTControlValue(ekwc);
        }

        delegate void SetFCTControlCallBack(EllipseKnobWithICVD ekwc);

        private void SetFCTControlValue(EllipseKnobWithICVD ekwc)
        {
            if (this.InvokeRequired)
            {
                SetFCTControlCallBack d = new SetFCTControlCallBack(SetFCTControlValue);
                this.Invoke(d, new object[] { ekwc });
            }
            else
            {
                FCTControlValueChange(ekwc);
            }
        }

        private void FCTControlValueChange(EllipseKnobWithICVD sender)
        {
            EllipseKnobWithICVD ewc = sender as EllipseKnobWithICVD;
                
            //double value = ewc.Maximum - ewc.Minimum;

            if (FunctionContorlValue != (int)((FunctionEllipseContorlB.Value - FunctionEllipseContorlB.Minimum) * 39 / (FunctionEllipseContorlB.Maximum - FunctionEllipseContorlB.Minimum)))
            {
                FunctionContorlValue = (int)((FunctionEllipseContorlB.Value - FunctionEllipseContorlB.Minimum) * 39 / (FunctionEllipseContorlB.Maximum - FunctionEllipseContorlB.Minimum));


                switch (FunctionContorlValue)
                {
                    case 0:
                        ControlProgressS1.Visible = false;
                        ControlProgressS2.Visible = false;
                        ControlProgressS3.Visible = false;
                        ControlProgressS4.Visible = false;
                        ControlProgressS5.Visible = false;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 1:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = false;
                        ControlProgressS3.Visible = false;
                        ControlProgressS4.Visible = false;
                        ControlProgressS5.Visible = false;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 2:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = false;
                        ControlProgressS4.Visible = false;
                        ControlProgressS5.Visible = false;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 3:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = false;
                        ControlProgressS5.Visible = false;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 4:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = false;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 5:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = false;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 6:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = false;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 7:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = false;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 8:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = false;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 9:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = false;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 10:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = false;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 11:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = false;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 12:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = false;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 13:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = false;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 14:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = false;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 15:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = false;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 16:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = false;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 17:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = false;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 18:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = false;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 19:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = false;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 20:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = false;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 21:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = false;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 22:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = false;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 23:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = false;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 24:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = false;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 25:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = false;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 26:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = false;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 27:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = false;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 28:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = false;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 29:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = false;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 30:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = false;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 31:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = false;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 32:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = false;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 33:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = false;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 34:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = false;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 35:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = true;
                        ControlProgressS36.Visible = false;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 36:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = true;
                        ControlProgressS36.Visible = true;
                        ControlProgressS37.Visible = false;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 37:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = true;
                        ControlProgressS36.Visible = true;
                        ControlProgressS37.Visible = true;
                        ControlProgressS38.Visible = false;
                        ControlProgressS39.Visible = false;
                        break;

                    case 38:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = true;
                        ControlProgressS36.Visible = true;
                        ControlProgressS37.Visible = true;
                        ControlProgressS38.Visible = true;
                        ControlProgressS39.Visible = false;
                        break;

                    case 39:
                        ControlProgressS1.Visible = true;
                        ControlProgressS2.Visible = true;
                        ControlProgressS3.Visible = true;
                        ControlProgressS4.Visible = true;
                        ControlProgressS5.Visible = true;
                        ControlProgressS6.Visible = true;
                        ControlProgressS7.Visible = true;
                        ControlProgressS8.Visible = true;
                        ControlProgressS9.Visible = true;
                        ControlProgressS10.Visible = true;
                        ControlProgressS11.Visible = true;
                        ControlProgressS12.Visible = true;
                        ControlProgressS13.Visible = true;
                        ControlProgressS14.Visible = true;
                        ControlProgressS15.Visible = true;
                        ControlProgressS16.Visible = true;
                        ControlProgressS17.Visible = true;
                        ControlProgressS18.Visible = true;
                        ControlProgressS19.Visible = true;
                        ControlProgressS20.Visible = true;
                        ControlProgressS21.Visible = true;
                        ControlProgressS22.Visible = true;
                        ControlProgressS23.Visible = true;
                        ControlProgressS24.Visible = true;
                        ControlProgressS25.Visible = true;
                        ControlProgressS26.Visible = true;
                        ControlProgressS27.Visible = true;
                        ControlProgressS28.Visible = true;
                        ControlProgressS29.Visible = true;
                        ControlProgressS30.Visible = true;
                        ControlProgressS31.Visible = true;
                        ControlProgressS32.Visible = true;
                        ControlProgressS33.Visible = true;
                        ControlProgressS34.Visible = true;
                        ControlProgressS35.Visible = true;
                        ControlProgressS36.Visible = true;
                        ControlProgressS37.Visible = true;
                        ControlProgressS38.Visible = true;
                        ControlProgressS39.Visible = true;
                        break;

                    default:
                        break;

                }


            }
            else
            {

            }


            


        }




        private void FCTControlValueChange2(object sender, EventArgs e)
        {
            EllipseKnobWithICVD ekwc = sender as EllipseKnobWithICVD;

            FCTControlValueChange2(ekwc);
        }

        private void FCTControlValueChange2(EllipseKnobWithICVD sender)
        {
            EllipseKnobWithICVD ewc = sender as EllipseKnobWithICVD;

            //double value = ewc.Maximum - ewc.Minimum;

            if (FunctionContorlValue != (int)((frMainFocusKnobEkwicvd.Value - frMainFocusKnobEkwicvd.Minimum) * 39 / (frMainFocusKnobEkwicvd.Maximum - frMainFocusKnobEkwicvd.Minimum)))
            {
                FunctionContorlValue = (int)((frMainFocusKnobEkwicvd.Value - frMainFocusKnobEkwicvd.Minimum) * 39 / (frMainFocusKnobEkwicvd.Maximum - frMainFocusKnobEkwicvd.Minimum));


                switch (FunctionContorlValue)
                {
                    case 0:
                        ControlProgress1.Visible = false;
                        ControlProgress2.Visible = false;
                        ControlProgress3.Visible = false;
                        ControlProgress4.Visible = false;
                        ControlProgress5.Visible = false;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 1:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = false;
                        ControlProgress3.Visible = false;
                        ControlProgress4.Visible = false;
                        ControlProgress5.Visible = false;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 2:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = false;
                        ControlProgress4.Visible = false;
                        ControlProgress5.Visible = false;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 3:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = false;
                        ControlProgress5.Visible = false;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 4:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = false;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 5:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = false;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 6:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = false;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 7:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = false;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 8:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = false;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 9:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = false;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 10:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = false;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 11:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = false;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 12:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = false;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 13:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = false;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 14:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = false;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 15:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = false;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 16:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = false;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 17:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = false;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 18:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = false;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 19:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = false;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 20:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = false;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 21:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = false;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 22:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = false;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 23:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = false;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 24:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = false;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 25:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = false;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 26:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = false;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 27:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = false;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 28:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = false;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 29:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = false;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 30:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = false;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 31:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = false;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 32:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = false;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 33:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = false;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 34:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = false;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 35:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = true;
                        ControlProgress36.Visible = false;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 36:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = true;
                        ControlProgress36.Visible = true;
                        ControlProgress37.Visible = false;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 37:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = true;
                        ControlProgress36.Visible = true;
                        ControlProgress37.Visible = true;
                        ControlProgress38.Visible = false;
                        ControlProgress39.Visible = false;
                        break;

                    case 38:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = true;
                        ControlProgress36.Visible = true;
                        ControlProgress37.Visible = true;
                        ControlProgress38.Visible = true;
                        ControlProgress39.Visible = false;
                        break;

                    case 39:
                        ControlProgress1.Visible = true;
                        ControlProgress2.Visible = true;
                        ControlProgress3.Visible = true;
                        ControlProgress4.Visible = true;
                        ControlProgress5.Visible = true;
                        ControlProgress6.Visible = true;
                        ControlProgress7.Visible = true;
                        ControlProgress8.Visible = true;
                        ControlProgress9.Visible = true;
                        ControlProgress10.Visible = true;
                        ControlProgress11.Visible = true;
                        ControlProgress12.Visible = true;
                        ControlProgress13.Visible = true;
                        ControlProgress14.Visible = true;
                        ControlProgress15.Visible = true;
                        ControlProgress16.Visible = true;
                        ControlProgress17.Visible = true;
                        ControlProgress18.Visible = true;
                        ControlProgress19.Visible = true;
                        ControlProgress20.Visible = true;
                        ControlProgress21.Visible = true;
                        ControlProgress22.Visible = true;
                        ControlProgress23.Visible = true;
                        ControlProgress24.Visible = true;
                        ControlProgress25.Visible = true;
                        ControlProgress26.Visible = true;
                        ControlProgress27.Visible = true;
                        ControlProgress28.Visible = true;
                        ControlProgress29.Visible = true;
                        ControlProgress30.Visible = true;
                        ControlProgress31.Visible = true;
                        ControlProgress32.Visible = true;
                        ControlProgress33.Visible = true;
                        ControlProgress34.Visible = true;
                        ControlProgress35.Visible = true;
                        ControlProgress36.Visible = true;
                        ControlProgress37.Visible = true;
                        ControlProgress38.Visible = true;
                        ControlProgress39.Visible = true;
                        break;

                    default:
                        break;

                }


            }
            else
            {

            }
        }

       

        delegate void SetFCTControlCallBack2(EllipseKnobWithICVD ekwc);

        private void SetFCTControlValue2(EllipseKnobWithICVD ekwc)
        {
            if (this.InvokeRequired)
            {
                SetFCTControlCallBack2 d = new SetFCTControlCallBack2(SetFCTControlValue2);
                this.Invoke(d, new object[] { ekwc });
            }
            else
            {
                FCTControlValueChange2(ekwc);
            }
        }

        private void FunctionChange(object sender, EventArgs e)
        {
            //FCTFocusBtn.Checked = false;
            //FCTFocusLab.ForeColor = Color.FromArgb(0, 9, 19);
            
            //FCTMagnificationBtn.Checked = false;
            //FCTMagnificationLab.ForeColor = Color.FromArgb(0, 9, 19);

            FCTSpotSizeBtn.Checked = false;
            FCTSpotSizeLab.ForeColor = Color.FromArgb(0, 9, 19);

            FCTStingBtn.Checked = false;
            FCTStigLab.ForeColor = Color.FromArgb(0, 9, 19);

            FCTBeamShiftBtn.Checked = false;
            FCTBeamShiftLab.ForeColor = Color.FromArgb(0, 9, 19);

            //FCTCBBtn.Checked = false;
            //FCTCBLab.ForeColor = Color.FromArgb(0, 9, 19);

            FCTSettingsBtn.Checked = false;
            FCTSettingsLab.ForeColor = Color.FromArgb(0, 9, 19);

            //MainControlPanel.Visible = false;
            //SecControlPanel.Visible = false;


            //frMainFocusKnobEkwicvd.DataBindings.Clear();
            //FunctionEllipseContorlB.DataBindings.Clear();

            //frMainFocusKnobEkwicvd.ValueChanged -= new EventHandler(Contrast_ValueChanged);

            //FunctionEllipseContorlB.ValueChanged -= new EventHandler(Brightness_ValueChanged);

            
            

            BitmapCheckBox bcb = sender as BitmapCheckBox;
            if (bcb.Checked)
            {
                return;
            }
            else
            {
                bcb.Checked = true;
                //bcb.ForeColor = Color.FromArgb(148,192,57);
            }


            
            
           

            switch (bcb.Name)
            {
                case "FCTFocusBtn":
                    //FCTFocusLab.ForeColor = Color.FromArgb(148, 192, 57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Focus"];

                    FocusAutoBtn.Click -= new EventHandler(ResetFunction_Click);

                    FCTConTextLab.Text = "WD";
                    FCTConTextLab2.Text = "Focus";

                    frMainFocusKnobEkwicvd.ControlValue = equip.ColumnLensOLC;
                    FunctionEllipseContorlB.ControlValue = equip.ColumnLensOLF;

                    WDString();
                    FineString();

                    WobbleBtn.Visible = true;

                    FocusAutoBtn.Text = "Auto";
                    FocusAutoBtn.Tag = "AF-WD";
                    FocusAutoBtn.Click += new EventHandler(AutoFunction_Click);

                    FocusAutoBtn.Checked = false;

                    break;

                case "FCTMagnificationBtn":
                    //FCTMagnificationLab.ForeColor = Color.FromArgb(148, 192, 57);
                    //FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Magnification"];

                    //WobbleBtn.Visible = false;
                    break;

                case "FCTSpotSizeBtn":
                    FCTSpotSizeLab.ForeColor = Color.FromArgb(148, 192, 57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["SpotSize"];

                    //FocusAutoBtn.Click -= new EventHandler(AutoFunction_Click);

                    //FCTConTextLab.Text = "CL1";
                    //FCTConTextLab2.Text = "CL2";

                    //frMainFocusKnobEkwicvd.ControlValue = equip.ColumnLensCL1;
                    //FunctionEllipseContorlB.ControlValue = equip.ColumnLensCL2;


                    LensCL1String();
                    LensCL2String();

                    //WobbleBtn.Visible = false;

                    //FocusAutoBtn.Text = "Default";
                    //FocusAutoBtn.Tag = "CL-Ext";
                    //FocusAutoBtn.Click += new EventHandler(ResetFunction_Click);

                    //FocusAutoBtn.Checked = false;

                    break;

                case "FCTStingBtn":
                    FCTStigLab.ForeColor = Color.FromArgb(148,192,57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Stigmation"];

                    //FocusAutoBtn.Click -= new EventHandler(AutoFunction_Click);

                    //FCTConTextLab.Text = "Stig X";
                    //FCTConTextLab2.Text = "Stig Y";

                    //frMainFocusKnobEkwicvd.ControlValue = equip.ColumnStigXV;
                    //FunctionEllipseContorlB.ControlValue = equip.ColumnStigYV;

                    StigXString();
                    StigYString();

                    //WobbleBtn.Visible = false;
                    //FocusAutoBtn.Text = "Default";
                    //FocusAutoBtn.Tag = "Stig";
                    //FocusAutoBtn.Click += new EventHandler(ResetFunction_Click);

                    //FocusAutoBtn.Checked = false;
                    break;

                case "FCTBeamShiftBtn":
                    FCTBeamShiftLab.ForeColor = Color.FromArgb(148, 192, 57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Beamshift"];
                    break;

                case "FCTCBBtn":
                    //FCTCBLab.ForeColor = Color.FromArgb(148, 192, 57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Con/Bri"];
                    
                    
                    break;

                case "FCTSettingsBtn":
                    FCTSettingsLab.ForeColor = Color.FromArgb(148, 192, 57);
                    FunctionControlsTab.SelectedTab = FunctionControlsTab.Tabs["Settings"];

                    break;

            }

            


        }

        private void MotorAllHome(object sender, EventArgs e)
        {
            MotorStage.StageAllHome();
            MotorHomeBtn.Checked = false;
        }

        private void ProfileBtn_MouseDown(object sender, MouseEventArgs e)
        {
            ProfileBtn.Appearance.ImageBackground = Properties.Resources.icon02_02;
        }

        private void DetectorComboBtn_MouseDown(object sender, MouseEventArgs e)
        {
            DetectorComboBtn.Appearance.ImageBackground = Properties.Resources.icon02_02;
        }

        private void DetectorChanged(object sender, EventArgs e)
        {
            UltraComboEditor clickitem = sender as UltraComboEditor;
            if (clickitem.SelectedIndex < 0)
            {
                return;
            }


            DetectorComboBtn.Text = clickitem.SelectedItem.DisplayText;
            EmissionDisplayLabel.Focus();
            

            ApplyScanningProfile(ScanModeEnum.ScanPause, false, true, SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED ? true : false);



            if (clickitem.SelectedItem.DataValue == "SE")
            {
                
                scanner.Revers(true);
                scanner.DualMode(false);
                ppSingle.Dual = false;
                ppSingle.Merge = false;


                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.SED;

                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);
                Multichannel = false;

                //BSEControl.SelectedTab = BSEControl.Tabs["DetectorMode"];

            }
            else if (clickitem.SelectedItem.DataValue == "BSE")
            {
               

                scanner.DualMode(false);
                ppSingle.Dual = false;
                ppSingle.Merge = false;


                              

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.BSED;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.Revers(false);

                Multichannel = false;
                //BSEControl.SelectedTab = BSEControl.Tabs["BSEControl"];
            }
            else if (clickitem.SelectedItem.DataValue == "Dual")
            {

                scanner.DualMode(true);

                scanner.Revers(true);
                ppSingle.Dual = true;
                ppSingle.Merge = false;

               

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.DualSEBSE;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);
                Multichannel = true;

            }
            else if (clickitem.SelectedItem.DataValue == "Merge")
            {


                LowVac.Enabled = false;

                scanner.DualMode(true);

                scanner.Revers(true);
                ppSingle.Merge = true;
                ppSingle.Dual = false;


               

                SystemInfoBinder.Default.DetectorMode = SystemInfoBinder.ImageSourceEnum.Merge;


                setManager.ColumnOneLoad(equip.ColumnHVCLT, ColumnOnevalueMode.Factory);
                setManager.ColumnOneLoad(equip.ColumnHVPMT, ColumnOnevalueMode.Factory);

                scanner.ScanMode(false);

                Multichannel = true;

            }



            if (frMainScanSfBb.Checked)
            {

                ScanModeChange(ScanModeEnum.FastScan);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanSsBb.Checked)
            {

                ScanModeChange(ScanModeEnum.SlowScan);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanPfBb.Checked)
            {

                ScanModeChange(ScanModeEnum.FastPhoto);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else if (frMainScanPsBb.Checked)
            {

                ScanModeChange(ScanModeEnum.SlowPhoto);
                Delay(1000);
                SECcolumn.Lens.IWDSplineObjBase iwdsob = (equip.ColumnWD as SECcolumn.Lens.IWDSplineObjBase);

                new AutoFunctionManager().RunAutoFunction(AutoFunctionManager.AutoFunctionType.AutoVideo, ppSingle, iwdsob, SystemInfoBinder.Default.DetectorMode.ToString());

            }
            else
            {
                ScanModeChange(ScanModeEnum.ScanPause);
            }

            DetectorComboBtn.Appearance.ImageBackground = Properties.Resources.icon02_01;


        }

        private void VacuumChange(object sender, EventArgs e)
        {
            UltraComboEditor clickitem = sender as UltraComboEditor;
            if (clickitem.SelectedIndex < 0)
            {
                return;
            }


            if (clickitem.SelectedItem.DataValue == "LowVac")
            {
                
                SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.LowVacuum;

                ((SECtype.IControlInt)column["VacuumMode"]).Value = 1;
                
            }
            else
            {

                if (m_ToolStartup.Checked)
                {
                    // 고압이 켜진 상태에서는 가속전압등을 낮췄다 높이는 작업을 함.

                    highvacuumTimer = new System.Threading.Timer(new System.Threading.TimerCallback(HighVacuumTimerExpiered));
                    highVacTimer = 0;
                    highvacuumTimer.Change(0, 1000);

                    highVacForm = new Form();
                    highVacForm.StartPosition = FormStartPosition.CenterParent;
                    highVacForm.Owner = this;
                    highVacLable = new Label();
                    highVacLable.Dock = DockStyle.Fill;
                    highVacLable.TextAlign = ContentAlignment.MiddleCenter;
                    highVacLable.Location = new Point(ppSingle.Left + ppSingle.Width / 2, ppSingle.Top + ppSingle.Height / 2);
                    highVacForm.Controls.Add(highVacLable);
                    highVacForm.FormBorderStyle = FormBorderStyle.None;
                    highVacForm.Size = new Size(300, 50);

                    highVacForm.ShowDialog();
                }
                else
                {
                    // 고압이 켜지지 않은 상태에서는 바로 진공 모드를 바꿈.
                    ((SECtype.IControlInt)column["VacuumMode"]).Value = 0;
                    SystemInfoBinder.Default.VacuumMode = SystemInfoBinder.VacuumModeEnum.HighVacuum;
                }

               
            }


           

        }

        private void FunctionPageChange(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;
            bcb.Checked = false;

            if (FunctionIconTab.SelectedTab.Index == 0)
            {
                FunctionIconTab.SelectedTab = FunctionIconTab.Tabs[1];
            }
            else
            {
                FunctionIconTab.SelectedTab = FunctionIconTab.Tabs[0];
            }

            
        }

        private void StageSettingsChange(object sender, EventArgs e)
        {


            if (MotorSettings == null)
            {
              
                MotorSettings = new FormConfig.MotorSpeedSetting(MotorStage);
                MotorSettings.Show();
                
            }

            if (MotorSettings.IsDisposed)
            {
                MotorSettings = new FormConfig.MotorSpeedSetting(MotorStage);
                MotorSettings.Show();
            }
            

            
            FCTStageSettingsBtn.Checked = false;
            FCTStageSettingsBtn.ForeColor = Color.FromArgb(26, 45, 60);

        }



        private void MiniSEM_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    MotorStage.StageStop(1);
                    Thread.Sleep(20);
                    MotorStage.StageStop(1);
                    
                    break;

                case Keys.Right:
                    MotorStage.StageStop(1);
                    Thread.Sleep(20);
                    MotorStage.StageStop(1);
                    break;

                case Keys.Up:
                    MotorStage.StageStop(2);
                    Thread.Sleep(20);
                    MotorStage.StageStop(2);
                    break;

                case Keys.Down:
                    MotorStage.StageStop(2);
                    Thread.Sleep(20);
                    MotorStage.StageStop(2);
                    break;
            }
        }

        private bool MainScrollBarEnable = false;
        private bool SeScrollBarEnable = false;
        private void FCTConValueLab_Click(object sender, EventArgs e)
        {
            if (MainScrollBarEnable)
            {
                MainControlScrollBar.DataBindings.Clear();
                MainScrollBarEnable = false;
                MainControlPanel.Visible = false;
                
            }
            else
            {
                

                MainScrollBarEnable = true;
                MainControlPanel.Visible = true;

                MainControlScrollBar.DataBindings.Clear();
                MainControlScrollBar.ControlValue = null;

                //if (FCTFocusBtn.Checked)
                //{
                    
                    MainControlScrollBar.ControlValue = equip.ColumnLensOLC;
                    
                //}
                //else if (FCTSpotSizeBtn.Checked)
                //{
                //    MainControlScrollBar.ControlValue = equip.ColumnLensCL1;
                //}
                //else if (FCTStingBtn.Checked)
                //{
                //    MainControlScrollBar.ControlValue = equip.ColumnStigXV;
                //}
                //else if (FCTCBBtn.Checked)
                //{
                //    MainControlScrollBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.sicBs, "Contrast", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));

                //    MainControlScrollBar.Maximum = 511;
                //    MainControlScrollBar.Minimum = -512;


                //    MainControlScrollBar.ValueChanged += new EventHandler(FCTControlValueChange2);

                //}
                //else
                //{

                //}
            }
        }


        private void FCTConValueLab2_Click(object sender, EventArgs e)
        {
            if (SeScrollBarEnable)
            {
                SecControlBar.DataBindings.Clear();
                SeScrollBarEnable = false;
                SecControlPanel.Visible = false;
            }
            else
            {
                
                SeScrollBarEnable = true;
                SecControlPanel.Visible = true;

                SecControlBar.DataBindings.Clear();
                SecControlBar.ControlValue = null;

                //if (FCTFocusBtn.Checked)
                //{
                    SecControlBar.ControlValue = equip.ColumnLensOLF as SECtype.IControlDouble;
                //}
                //else if (FCTSpotSizeBtn.Checked)
                //{
                //    SecControlBar.ControlValue = equip.ColumnLensCL2;
                //}
                //else if (FCTStingBtn.Checked)
                //{
                //    SecControlBar.ControlValue = equip.ColumnStigYV;
                //}
                //else if (FCTCBBtn.Checked)
                //{
                //    //SecControlBar.ControlValue = FunctionEllipseContorlB.ControlValue;

                //    SecControlBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.sicBs, "Brightness", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));

                //    SecControlBar.Maximum = 511;
                //    SecControlBar.Minimum = -512;

                //    SecControlBar.ValueChanged += new EventHandler(FCTControlValueChange);
                //}
                //else
                //{

                //}

            }
        }

        private void MagButtonChange(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;
            int mag = 0;

            switch (bcb.Name)
            {

                case "Magx1000Btn":
                    mag = equip.Magnification + 1000;


                    while (true)
                    {
                        if (mag > equip.Magnification)
                        {
                            MagTrakBar.Value++;
                            if (equip.Magnification >= mag)
                            {
                                break;
                            }
                            else if (UIsetBinder.Default.MagIndex >= UIsetBinder.Default.MagMaximum)
                            {
                                break;
                            }
                        }
                    }

                    
                    break;

                case "Magx10000Btn":
                     mag = equip.Magnification + 10000;


                    while (true)
                    {
                        if (mag > equip.Magnification)
                        {
                            MagTrakBar.Value++;
                            if (equip.Magnification >= mag)
                            {
                                break;
                            }
                            else if (UIsetBinder.Default.MagIndex >= UIsetBinder.Default.MagMaximum)
                            {
                                break;
                            }
                        }
                    }
                    break;

                case "MagDefaultBtn":
                    MagTrakBar.Value = 0;
                    break;

            }

            bcb.Checked = false;
        }

        private void ImageListItemSizeChange(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            ImageListLageBtn.Checked = false;
            ImageListMiddleBtn.Checked = false;
            ImageListSmallBtn.Checked = false;

            switch (bcb.Name)
            {
                case "ImageListLageBtn":
                    frSamllListView.View = View.LargeIcon;
                    
                    frSamllListView.LargeImageList.ImageSize = new Size(256, 191);

                    ImageListChange();
                    //ArchivesAdressChange(sender, e);
                    frSamllListView.Sort();
                    frSamllListView.Invalidate();
                    
                    break;

                case "ImageListMiddleBtn":

                    frSamllListView.View = View.SmallIcon;
                    frSamllListView.SmallImageList.ImageSize = new Size(128, 95);
                    //ArchivesAdressChange(sender, e);
                    ImageListChange();
                    frSamllListView.SmallImageList = imageList1;
                    frSamllListView.Invalidate();
                    break;

                case "ImageListSmallBtn":
                    frSamllListView.View = View.Tile;
                    frSamllListView.TileSize = new Size(128, 95);
                    ImageListChange();    
                    //ArchivesAdressChange(sender, e);
                    //frSamllListView.
                    frSamllListView.Invalidate();
                    break;
            }

            bcb.Checked = true;


        }





        




       
    }


}
        #endregion

