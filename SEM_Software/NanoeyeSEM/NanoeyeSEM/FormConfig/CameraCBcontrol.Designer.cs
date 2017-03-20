namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    partial class CameraCBcontrol
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraCBcontrol));
            this.label1 = new System.Windows.Forms.Label();
            this.videoContrastLab = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_BrightnessDisp = new SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle();
            this.m_ContrastDisp = new SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle();
            this.MainFormClose = new SEC.Nanoeye.Controls.BitmapRadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label1.Name = "label1";
            // 
            // videoContrastLab
            // 
            resources.ApplyResources(this.videoContrastLab, "videoContrastLab");
            this.videoContrastLab.BackColor = System.Drawing.Color.Transparent;
            this.videoContrastLab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
            this.videoContrastLab.Name = "videoContrastLab";
            this.videoContrastLab.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
            this.label2.Name = "label2";
            this.label2.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            // 
            // m_BrightnessDisp
            // 
            resources.ApplyResources(this.m_BrightnessDisp, "m_BrightnessDisp");
            this.m_BrightnessDisp.BackColor = System.Drawing.Color.Transparent;
            this.m_BrightnessDisp.ButtonSize = new System.Drawing.Size(27, 27);
            this.m_BrightnessDisp.ButtonSizeAuto = false;
            this.m_BrightnessDisp.DividInner = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.m_BrightnessDisp.DividOutter = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_BrightnessDisp.LeftIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_brightness1_enable;
            this.m_BrightnessDisp.Maximum = 200;
            this.m_BrightnessDisp.Minimum = 0;
            this.m_BrightnessDisp.Name = "m_BrightnessDisp";
            this.m_BrightnessDisp.Orientation = false;
            this.m_BrightnessDisp.PanelColor = System.Drawing.Color.Transparent;
            this.m_BrightnessDisp.Position = new System.Drawing.Rectangle(120, -2, 14, 27);
            this.m_BrightnessDisp.reBruIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_dot;
            this.m_BrightnessDisp.RightIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_brightness2_enable;
            this.m_BrightnessDisp.TextIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bar_info;
            this.m_BrightnessDisp.TrackBarIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bg;
            this.m_BrightnessDisp.Value = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.CameraBirghtness;
            this.m_BrightnessDisp.ValueChanged += new System.EventHandler(this.CCbrightnessValueChange);
            // 
            // m_ContrastDisp
            // 
            resources.ApplyResources(this.m_ContrastDisp, "m_ContrastDisp");
            this.m_ContrastDisp.BackColor = System.Drawing.Color.Transparent;
            this.m_ContrastDisp.ButtonSize = new System.Drawing.Size(27, 27);
            this.m_ContrastDisp.ButtonSizeAuto = false;
            this.m_ContrastDisp.DividInner = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.m_ContrastDisp.DividOutter = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_ContrastDisp.LeftIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_contrast1_enable;
            this.m_ContrastDisp.Maximum = 200;
            this.m_ContrastDisp.Minimum = 0;
            this.m_ContrastDisp.Name = "m_ContrastDisp";
            this.m_ContrastDisp.Orientation = false;
            this.m_ContrastDisp.PanelColor = System.Drawing.Color.Transparent;
            this.m_ContrastDisp.Position = new System.Drawing.Rectangle(120, -2, 14, 27);
            this.m_ContrastDisp.reBruIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_dot;
            this.m_ContrastDisp.RightIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_contrast2_enable;
            this.m_ContrastDisp.TextIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bar_info;
            this.m_ContrastDisp.TrackBarIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bg;
            this.m_ContrastDisp.Value = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.CameraContrast;
            this.m_ContrastDisp.ValueChanged += new System.EventHandler(this.CCcontrastValueChange);
            // 
            // MainFormClose
            // 
            resources.ApplyResources(this.MainFormClose, "MainFormClose");
            this.MainFormClose.BackColor = System.Drawing.Color.Transparent;
            this.MainFormClose.ForeColor = System.Drawing.Color.DarkRed;
            this.MainFormClose.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_close_s;
            this.MainFormClose.Name = "MainFormClose";
            this.MainFormClose.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_close_s;
            this.MainFormClose.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.MainFormClose.UseCompatibleTextRendering = true;
            this.MainFormClose.Click += new System.EventHandler(this.FormClose);
            // 
            // CameraCBcontrol
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_sample_bg;
            this.Controls.Add(this.m_BrightnessDisp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.videoContrastLab);
            this.Controls.Add(this.m_ContrastDisp);
            this.Controls.Add(this.MainFormClose);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CameraCBcontrol";
            this.Shown += new System.EventHandler(this.FormShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private SEC.Nanoeye.Controls.BitmapRadioButton MainFormClose;
        private SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle m_ContrastDisp;
        private System.Windows.Forms.Label videoContrastLab;
        private SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle m_BrightnessDisp;
        private System.Windows.Forms.Label label2;
    }
}