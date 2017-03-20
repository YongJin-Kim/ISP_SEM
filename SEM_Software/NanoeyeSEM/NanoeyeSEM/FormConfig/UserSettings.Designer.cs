namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    partial class UserSettings
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.SECheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BSECheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LowVacCheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CameraCheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Focus_BarTypeCheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.NewMagCheck = new SEC.Nanoeye.Controls.BitmapCheckBox();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Font = new System.Drawing.Font("Arial Unicode MS", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            appearance2.ForeColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.HotTrackAppearance = appearance2;
            this.ultraLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ultraLabel1.Location = new System.Drawing.Point(21, 18);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(101, 23);
            this.ultraLabel1.TabIndex = 174;
            this.ultraLabel1.Text = "User Settings";
            // 
            // SECheck
            // 
            this.SECheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.SECheck.BackColor = System.Drawing.Color.Transparent;
            this.SECheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.SECheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SECheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SECheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.SECheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.SECheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.SECheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SECheck.Location = new System.Drawing.Point(25, 62);
            this.SECheck.Name = "SECheck";
            this.SECheck.Size = new System.Drawing.Size(20, 19);
            this.SECheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.SECheck.TabIndex = 193;
            this.SECheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.SECheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SECheck.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(46, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 15);
            this.label4.TabIndex = 192;
            this.label4.Text = "SE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(104, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 192;
            this.label1.Text = "BSE";
            // 
            // BSECheck
            // 
            this.BSECheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.BSECheck.BackColor = System.Drawing.Color.Transparent;
            this.BSECheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.BSECheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BSECheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.BSECheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.BSECheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.BSECheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.BSECheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BSECheck.Location = new System.Drawing.Point(83, 62);
            this.BSECheck.Name = "BSECheck";
            this.BSECheck.Size = new System.Drawing.Size(20, 19);
            this.BSECheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.BSECheck.TabIndex = 193;
            this.BSECheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.BSECheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BSECheck.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(174, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 192;
            this.label2.Text = "Low Vacuum";
            // 
            // LowVacCheck
            // 
            this.LowVacCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.LowVacCheck.BackColor = System.Drawing.Color.Transparent;
            this.LowVacCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.LowVacCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LowVacCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LowVacCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.LowVacCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.LowVacCheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.LowVacCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LowVacCheck.Location = new System.Drawing.Point(153, 62);
            this.LowVacCheck.Name = "LowVacCheck";
            this.LowVacCheck.Size = new System.Drawing.Size(20, 19);
            this.LowVacCheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.LowVacCheck.TabIndex = 193;
            this.LowVacCheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.LowVacCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LowVacCheck.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(289, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 192;
            this.label3.Text = "Camera";
            // 
            // CameraCheck
            // 
            this.CameraCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.CameraCheck.BackColor = System.Drawing.Color.Transparent;
            this.CameraCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.CameraCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CameraCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CameraCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.CameraCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.CameraCheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.CameraCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CameraCheck.Location = new System.Drawing.Point(263, 62);
            this.CameraCheck.Name = "CameraCheck";
            this.CameraCheck.Size = new System.Drawing.Size(20, 19);
            this.CameraCheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.CameraCheck.TabIndex = 193;
            this.CameraCheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.CameraCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CameraCheck.UseCompatibleTextRendering = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(45, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 15);
            this.label5.TabIndex = 192;
            this.label5.Text = "Focus_Bar Type";
            // 
            // Focus_BarTypeCheck
            // 
            this.Focus_BarTypeCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.Focus_BarTypeCheck.BackColor = System.Drawing.Color.Transparent;
            this.Focus_BarTypeCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.Focus_BarTypeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Focus_BarTypeCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Focus_BarTypeCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Focus_BarTypeCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.Focus_BarTypeCheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.Focus_BarTypeCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Focus_BarTypeCheck.Location = new System.Drawing.Point(25, 97);
            this.Focus_BarTypeCheck.Name = "Focus_BarTypeCheck";
            this.Focus_BarTypeCheck.Size = new System.Drawing.Size(20, 19);
            this.Focus_BarTypeCheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.Focus_BarTypeCheck.TabIndex = 193;
            this.Focus_BarTypeCheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.Focus_BarTypeCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Focus_BarTypeCheck.UseCompatibleTextRendering = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial Unicode MS", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(172, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 15);
            this.label6.TabIndex = 192;
            this.label6.Text = "New Mag";
            // 
            // NewMagCheck
            // 
            this.NewMagCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.NewMagCheck.BackColor = System.Drawing.Color.Transparent;
            this.NewMagCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.MicronVoltage;
            this.NewMagCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewMagCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "MicronVoltage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NewMagCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.NewMagCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(235)))), ((int)(((byte)(251)))));
            this.NewMagCheck.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_checkbox;
            this.NewMagCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NewMagCheck.Location = new System.Drawing.Point(152, 97);
            this.NewMagCheck.Name = "NewMagCheck";
            this.NewMagCheck.Size = new System.Drawing.Size(20, 19);
            this.NewMagCheck.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_check_sel;
            this.NewMagCheck.TabIndex = 193;
            this.NewMagCheck.Tag = global::SEC.Nanoeye.NanoeyeSEM.LanguageResources.MultiLanguage.ProgramReStart;
            this.NewMagCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NewMagCheck.UseCompatibleTextRendering = true;
            // 
            // UserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_status_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(420, 303);
            this.Controls.Add(this.NewMagCheck);
            this.Controls.Add(this.Focus_BarTypeCheck);
            this.Controls.Add(this.CameraCheck);
            this.Controls.Add(this.LowVacCheck);
            this.Controls.Add(this.BSECheck);
            this.Controls.Add(this.SECheck);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ultraLabel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserSettings";
            this.Text = "UserSettings";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private SEC.Nanoeye.Controls.BitmapCheckBox SECheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private SEC.Nanoeye.Controls.BitmapCheckBox BSECheck;
        private System.Windows.Forms.Label label2;
        private SEC.Nanoeye.Controls.BitmapCheckBox LowVacCheck;
        private System.Windows.Forms.Label label3;
        private SEC.Nanoeye.Controls.BitmapCheckBox CameraCheck;
        private System.Windows.Forms.Label label5;
        private SEC.Nanoeye.Controls.BitmapCheckBox Focus_BarTypeCheck;
        private System.Windows.Forms.Label label6;
        private SEC.Nanoeye.Controls.BitmapCheckBox NewMagCheck;
    }
}