namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    partial class ArchivesTabCB
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
            System.Windows.Forms.Label label22;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchivesTabCB));
            System.Windows.Forms.Label label2;
            this.ArchivesImageCB = new System.Windows.Forms.Panel();
            this.ArchivesBrightness = new SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle();
            this.ArchivesContrast = new SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle();
            this.MainFormClose = new SEC.Nanoeye.Controls.BitmapRadioButton();
            this.label1 = new System.Windows.Forms.Label();
            label22 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.ArchivesImageCB.SuspendLayout();
            this.SuspendLayout();
            // 
            // label22
            // 
            resources.ApplyResources(label22, "label22");
            label22.BackColor = System.Drawing.Color.Transparent;
            label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
            label22.Name = "label22";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
            label2.Name = "label2";
            // 
            // ArchivesImageCB
            // 
            resources.ApplyResources(this.ArchivesImageCB, "ArchivesImageCB");
            this.ArchivesImageCB.BackColor = System.Drawing.Color.DimGray;
            this.ArchivesImageCB.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_sample_bg;
            this.ArchivesImageCB.Controls.Add(label2);
            this.ArchivesImageCB.Controls.Add(label22);
            this.ArchivesImageCB.Controls.Add(this.ArchivesBrightness);
            this.ArchivesImageCB.Controls.Add(this.ArchivesContrast);
            this.ArchivesImageCB.Controls.Add(this.MainFormClose);
            this.ArchivesImageCB.Controls.Add(this.label1);
            this.ArchivesImageCB.Name = "ArchivesImageCB";
            // 
            // ArchivesBrightness
            // 
            resources.ApplyResources(this.ArchivesBrightness, "ArchivesBrightness");
            this.ArchivesBrightness.BackColor = System.Drawing.Color.Transparent;
            this.ArchivesBrightness.ButtonSize = new System.Drawing.Size(27, 27);
            this.ArchivesBrightness.ButtonSizeAuto = false;
            this.ArchivesBrightness.DividInner = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ArchivesBrightness.DividOutter = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.ArchivesBrightness.LeftIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_brightness1_enable;
            this.ArchivesBrightness.Maximum = 1024;
            this.ArchivesBrightness.Minimum = -1024;
            this.ArchivesBrightness.Name = "ArchivesBrightness";
            this.ArchivesBrightness.Orientation = false;
            this.ArchivesBrightness.PanelColor = System.Drawing.Color.Transparent;
            this.ArchivesBrightness.Position = new System.Drawing.Rectangle(87, -2, 14, 27);
            this.ArchivesBrightness.reBruIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_dot;
            this.ArchivesBrightness.RightIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_brightness2_enable;
            this.ArchivesBrightness.TextIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bar_info;
            this.ArchivesBrightness.TrackBarIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bg;
            this.ArchivesBrightness.Value = 0;
            this.ArchivesBrightness.ValueChanged += new System.EventHandler(this.ArchivesBrightnessValueChange);
            // 
            // ArchivesContrast
            // 
            resources.ApplyResources(this.ArchivesContrast, "ArchivesContrast");
            this.ArchivesContrast.BackColor = System.Drawing.Color.Transparent;
            this.ArchivesContrast.ButtonSize = new System.Drawing.Size(27, 27);
            this.ArchivesContrast.ButtonSizeAuto = false;
            this.ArchivesContrast.DividInner = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ArchivesContrast.DividOutter = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.ArchivesContrast.LeftIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_contrast1_enable;
            this.ArchivesContrast.Maximum = 256;
            this.ArchivesContrast.Minimum = -256;
            this.ArchivesContrast.Name = "ArchivesContrast";
            this.ArchivesContrast.Orientation = false;
            this.ArchivesContrast.PanelColor = System.Drawing.Color.Transparent;
            this.ArchivesContrast.Position = new System.Drawing.Rectangle(86, -2, 14, 27);
            this.ArchivesContrast.reBruIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_dot;
            this.ArchivesContrast.RightIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_contrast2_enable;
            this.ArchivesContrast.TextIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bar_info;
            this.ArchivesContrast.TrackBarIcon = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_progress_bg;
            this.ArchivesContrast.Value = 0;
            this.ArchivesContrast.ValueChanged += new System.EventHandler(this.ArchivesCantrastValueChange);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label1.Name = "label1";
            // 
            // ArchivesTabCB
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ArchivesImageCB);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ArchivesTabCB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FormShown);
            this.ArchivesImageCB.ResumeLayout(false);
            this.ArchivesImageCB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ArchivesImageCB;
        private SEC.Nanoeye.Controls.BitmapRadioButton MainFormClose;
        private System.Windows.Forms.Label label1;
        private SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle ArchivesContrast;
        private SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle ArchivesBrightness;
    }
}