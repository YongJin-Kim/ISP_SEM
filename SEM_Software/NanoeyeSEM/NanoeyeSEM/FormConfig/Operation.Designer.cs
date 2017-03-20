using System;
namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    partial class Operation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Operation));
            System.Windows.Forms.Label label19;
            System.Windows.Forms.Label label20;
            System.Windows.Forms.Label label21;
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.EmissionDisplayLabel = new System.Windows.Forms.Label();
            this.ProfileBtn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.DetectorDual = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.DetectorBSE = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.DetectorSE = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.CalibrationSettingsBtn = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.StartUpProgressBar = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.MainFormClose = new SEC.Nanoeye.Controls.BitmapRadioButton();
            this.m_ToolStartup = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LanguageButton3 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LanguageButton2 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LanguageButton1 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LowVac = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.HighVac = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.DetectorMerge = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.bitmapCheckBox1 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LanguageButton4 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.LanguageButton5 = new SEC.Nanoeye.Controls.BitmapCheckBox();
            label1 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            label20 = new System.Windows.Forms.Label();
            label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = System.Drawing.Color.White;
            label1.Name = "label1";
            label1.Tag = "RF1_Focus";
            // 
            // label19
            // 
            label19.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(label19, "label19");
            label19.ForeColor = System.Drawing.Color.White;
            label19.Name = "label19";
            label19.Tag = "RF1_Focus";
            // 
            // label20
            // 
            label20.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(label20, "label20");
            label20.ForeColor = System.Drawing.Color.White;
            label20.Name = "label20";
            label20.Tag = "RF1_Focus";
            // 
            // label21
            // 
            label21.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(label21, "label21");
            label21.ForeColor = System.Drawing.Color.White;
            label21.Name = "label21";
            label21.Tag = "RF1_Focus";
            // 
            // ultraLabel1
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            resources.ApplyResources(appearance8, "appearance8");
            resources.ApplyResources(appearance8.FontData, "appearance8.FontData");
            appearance8.ForceApplyResources = "FontData|";
            this.ultraLabel1.Appearance = appearance8;
            resources.ApplyResources(this.ultraLabel1, "ultraLabel1");
            appearance7.ForeColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(appearance7.FontData, "appearance7.FontData");
            resources.ApplyResources(appearance7, "appearance7");
            appearance7.ForceApplyResources = "FontData|";
            this.ultraLabel1.HotTrackAppearance = appearance7;
            this.ultraLabel1.Name = "ultraLabel1";
            // 
            // EmissionDisplayLabel
            // 
            this.EmissionDisplayLabel.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.EmissionDisplayLabel, "EmissionDisplayLabel");
            this.EmissionDisplayLabel.ForeColor = System.Drawing.Color.White;
            this.EmissionDisplayLabel.Name = "EmissionDisplayLabel";
            // 
            // ProfileBtn
            // 
            appearance38.BackColor = System.Drawing.Color.Transparent;
            appearance38.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance38.BorderColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(appearance38.FontData, "appearance38.FontData");
            appearance38.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            appearance38.ImageBackground = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_enable;
            appearance38.ImageBackgroundDisabled = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_enable;
            appearance38.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Client;
            appearance38.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance38.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance38.ImageVAlign = Infragistics.Win.VAlign.Middle;
            resources.ApplyResources(appearance38, "appearance38");
            appearance38.ForceApplyResources = "FontData|";
            this.ProfileBtn.Appearance = appearance38;
            resources.ApplyResources(this.ProfileBtn, "ProfileBtn");
            this.ProfileBtn.BackColor = System.Drawing.Color.Transparent;
            this.ProfileBtn.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.VisualStudio2005;
            this.ProfileBtn.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.ProfileBtn.DropDownListAlignment = Infragistics.Win.DropDownListAlignment.Center;
            this.ProfileBtn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ProfileBtn.Name = "ProfileBtn";
            this.ProfileBtn.OverflowIndicatorImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_drop1_enable;
            this.ProfileBtn.TabStop = false;
            this.ProfileBtn.SelectionChangeCommitted += new System.EventHandler(this.profileChange);
            // 
            // DetectorDual
            // 
            resources.ApplyResources(this.DetectorDual, "DetectorDual");
            this.DetectorDual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.DetectorDual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.DetectorDual.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_r;
            this.DetectorDual.Name = "DetectorDual";
            this.DetectorDual.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_r;
            this.DetectorDual.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.DetectorDual.UseCompatibleTextRendering = true;
            this.DetectorDual.Click += new System.EventHandler(this.DetectorChange);
            // 
            // DetectorBSE
            // 
            resources.ApplyResources(this.DetectorBSE, "DetectorBSE");
            this.DetectorBSE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.DetectorBSE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.DetectorBSE.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_c;
            this.DetectorBSE.Name = "DetectorBSE";
            this.DetectorBSE.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_c;
            this.DetectorBSE.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.DetectorBSE.UseCompatibleTextRendering = true;
            this.DetectorBSE.Click += new System.EventHandler(this.DetectorChange);
            // 
            // DetectorSE
            // 
            resources.ApplyResources(this.DetectorSE, "DetectorSE");
            this.DetectorSE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.DetectorSE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.DetectorSE.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_l;
            this.DetectorSE.Name = "DetectorSE";
            this.DetectorSE.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_l;
            this.DetectorSE.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.DetectorSE.UseCompatibleTextRendering = true;
            this.DetectorSE.Click += new System.EventHandler(this.DetectorChange);
            // 
            // CalibrationSettingsBtn
            // 
            resources.ApplyResources(this.CalibrationSettingsBtn, "CalibrationSettingsBtn");
            this.CalibrationSettingsBtn.BackColor = System.Drawing.Color.Transparent;
            this.CalibrationSettingsBtn.Checked = true;
            this.CalibrationSettingsBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CalibrationSettingsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.CalibrationSettingsBtn.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.Calibration_enable;
            this.CalibrationSettingsBtn.Name = "CalibrationSettingsBtn";
            this.CalibrationSettingsBtn.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.Calibration_disable;
            this.CalibrationSettingsBtn.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.CalibrationSettingsBtn.UseCompatibleTextRendering = true;
            this.CalibrationSettingsBtn.Click += new System.EventHandler(this.CalibrationSettingsBtn_Click);
            // 
            // StartUpProgressBar
            // 
            resources.ApplyResources(this.StartUpProgressBar, "StartUpProgressBar");
            this.StartUpProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.StartUpProgressBar.Checked = true;
            this.StartUpProgressBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StartUpProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.StartUpProgressBar.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_6;
            this.StartUpProgressBar.Name = "StartUpProgressBar";
            this.StartUpProgressBar.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_loading_6;
            this.StartUpProgressBar.UseCompatibleTextRendering = true;
            // 
            // MainFormClose
            // 
            resources.ApplyResources(this.MainFormClose, "MainFormClose");
            this.MainFormClose.BackColor = System.Drawing.Color.Transparent;
            this.MainFormClose.ForeColor = System.Drawing.Color.DarkRed;
            this.MainFormClose.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_bubble_close_e;
            this.MainFormClose.Name = "MainFormClose";
            this.MainFormClose.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_close_s;
            this.MainFormClose.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.MainFormClose.UseCompatibleTextRendering = true;
            this.MainFormClose.Click += new System.EventHandler(this.formCloseClick);
            // 
            // m_ToolStartup
            // 
            resources.ApplyResources(this.m_ToolStartup, "m_ToolStartup");
            this.m_ToolStartup.BackColor = System.Drawing.Color.Transparent;
            this.m_ToolStartup.Checked = true;
            this.m_ToolStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ToolStartup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.m_ToolStartup.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_start_enable1;
            this.m_ToolStartup.Name = "m_ToolStartup";
            this.m_ToolStartup.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.control_top_select1;
            this.m_ToolStartup.UseCompatibleTextRendering = true;
            this.m_ToolStartup.CheckedChanged += new System.EventHandler(this.ToolStartup_CheckedChangedEvent);
            // 
            // LanguageButton3
            // 
            resources.ApplyResources(this.LanguageButton3, "LanguageButton3");
            this.LanguageButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LanguageButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.LanguageButton3.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_r;
            this.LanguageButton3.Name = "LanguageButton3";
            this.LanguageButton3.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_r;
            this.LanguageButton3.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LanguageButton3.UseCompatibleTextRendering = true;
            this.LanguageButton3.Click += new System.EventHandler(this.LanguageChange);
            // 
            // LanguageButton2
            // 
            resources.ApplyResources(this.LanguageButton2, "LanguageButton2");
            this.LanguageButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LanguageButton2.Checked = true;
            this.LanguageButton2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LanguageButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.LanguageButton2.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_c;
            this.LanguageButton2.Name = "LanguageButton2";
            this.LanguageButton2.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_c;
            this.LanguageButton2.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LanguageButton2.UseCompatibleTextRendering = true;
            this.LanguageButton2.Click += new System.EventHandler(this.LanguageChange);
            // 
            // LanguageButton1
            // 
            resources.ApplyResources(this.LanguageButton1, "LanguageButton1");
            this.LanguageButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LanguageButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.LanguageButton1.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_l;
            this.LanguageButton1.Name = "LanguageButton1";
            this.LanguageButton1.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_l;
            this.LanguageButton1.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LanguageButton1.UseCompatibleTextRendering = true;
            this.LanguageButton1.Click += new System.EventHandler(this.LanguageChange);
            // 
            // LowVac
            // 
            resources.ApplyResources(this.LowVac, "LowVac");
            this.LowVac.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LowVac.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.LowVac.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle1_enable_r;
            this.LowVac.Name = "LowVac";
            this.LowVac.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle1_select_r;
            this.LowVac.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LowVac.UseCompatibleTextRendering = true;
            this.LowVac.Click += new System.EventHandler(this.VacuumStateToolStripMenuItem_Click);
            // 
            // HighVac
            // 
            resources.ApplyResources(this.HighVac, "HighVac");
            this.HighVac.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.HighVac.Checked = true;
            this.HighVac.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HighVac.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.HighVac.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle1_enable_l;
            this.HighVac.Name = "HighVac";
            this.HighVac.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle1_select_l;
            this.HighVac.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.HighVac.UseCompatibleTextRendering = true;
            this.HighVac.Click += new System.EventHandler(this.VacuumStateToolStripMenuItem_Click);
            // 
            // DetectorMerge
            // 
            resources.ApplyResources(this.DetectorMerge, "DetectorMerge");
            this.DetectorMerge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.DetectorMerge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.DetectorMerge.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_c;
            this.DetectorMerge.Name = "DetectorMerge";
            this.DetectorMerge.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_c;
            this.DetectorMerge.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.DetectorMerge.UseCompatibleTextRendering = true;
            this.DetectorMerge.Click += new System.EventHandler(this.DetectorChange);
            // 
            // bitmapCheckBox1
            // 
            resources.ApplyResources(this.bitmapCheckBox1, "bitmapCheckBox1");
            this.bitmapCheckBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.bitmapCheckBox1.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_c;
            this.bitmapCheckBox1.Name = "bitmapCheckBox1";
            this.bitmapCheckBox1.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_c;
            this.bitmapCheckBox1.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.bitmapCheckBox1.UseCompatibleTextRendering = true;
            this.bitmapCheckBox1.Click += new System.EventHandler(this.DetectorChange);
            // 
            // LanguageButton4
            // 
            resources.ApplyResources(this.LanguageButton4, "LanguageButton4");
            this.LanguageButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LanguageButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.LanguageButton4.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_l;
            this.LanguageButton4.Name = "LanguageButton4";
            this.LanguageButton4.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_l;
            this.LanguageButton4.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LanguageButton4.UseCompatibleTextRendering = true;
            this.LanguageButton4.Click += new System.EventHandler(this.LanguageChange);
            // 
            // LanguageButton5
            // 
            resources.ApplyResources(this.LanguageButton5, "LanguageButton5");
            this.LanguageButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.LanguageButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.LanguageButton5.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_enable_r;
            this.LanguageButton5.Name = "LanguageButton5";
            this.LanguageButton5.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_toggle2_select_r;
            this.LanguageButton5.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LanguageButton5.UseCompatibleTextRendering = true;
            this.LanguageButton5.Click += new System.EventHandler(this.LanguageChange);
            // 
            // Operation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_option_bg;
            this.Controls.Add(this.DetectorDual);
            this.Controls.Add(this.DetectorMerge);
            this.Controls.Add(this.DetectorBSE);
            this.Controls.Add(this.DetectorSE);
            this.Controls.Add(this.CalibrationSettingsBtn);
            this.Controls.Add(this.StartUpProgressBar);
            this.Controls.Add(this.ProfileBtn);
            this.Controls.Add(this.MainFormClose);
            this.Controls.Add(this.m_ToolStartup);
            this.Controls.Add(label21);
            this.Controls.Add(this.LanguageButton5);
            this.Controls.Add(this.LanguageButton3);
            this.Controls.Add(this.LanguageButton2);
            this.Controls.Add(this.LanguageButton4);
            this.Controls.Add(this.LanguageButton1);
            this.Controls.Add(label20);
            this.Controls.Add(this.LowVac);
            this.Controls.Add(this.HighVac);
            this.Controls.Add(label19);
            this.Controls.Add(label1);
            this.Controls.Add(this.EmissionDisplayLabel);
            this.Controls.Add(this.ultraLabel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Operation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Silver;
            this.Load += new System.EventHandler(this.Operation_Load);
            this.Shown += new System.EventHandler(this.OperationShown);
            this.LocationChanged += new System.EventHandler(this.formLocationChange);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_MainMenuStrip_MouseDoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.ProfileBtn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private SEC.Nanoeye.Controls.BitmapRadioButton MainFormClose;
        private System.Windows.Forms.Label EmissionDisplayLabel;
        private SEC.Nanoeye.Controls.BitmapCheckBox LowVac;
        private SEC.Nanoeye.Controls.BitmapCheckBox HighVac;
        private SEC.Nanoeye.Controls.BitmapCheckBox LanguageButton3;
        private SEC.Nanoeye.Controls.BitmapCheckBox LanguageButton2;
        private SEC.Nanoeye.Controls.BitmapCheckBox LanguageButton1;
        private SEC.Nanoeye.Controls.BitmapCheckBox m_ToolStartup;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ProfileBtn;
        private SEC.Nanoeye.Controls.BitmapCheckBox StartUpProgressBar;
        private SEC.Nanoeye.Controls.BitmapCheckBox CalibrationSettingsBtn;
        private SEC.Nanoeye.Controls.BitmapCheckBox DetectorDual;
        private SEC.Nanoeye.Controls.BitmapCheckBox DetectorBSE;
        private SEC.Nanoeye.Controls.BitmapCheckBox DetectorSE;
        private Controls.BitmapCheckBox DetectorMerge;
        private Controls.BitmapCheckBox bitmapCheckBox1;
        private Controls.BitmapCheckBox LanguageButton4;
        private Controls.BitmapCheckBox LanguageButton5;
    }
}