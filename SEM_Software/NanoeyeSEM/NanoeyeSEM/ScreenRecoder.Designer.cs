namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class ScreenRecorder
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
			if ( disposing && (components != null) ) {
				components.Dispose();
			}

			if ( recoder != null ) {
				recoder.Stop();
				recoder = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenRecorder));
            this.filename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.file = new System.Windows.Forms.Button();
            this.run = new System.Windows.Forms.CheckBox();
            this.fpsValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.areaImage = new System.Windows.Forms.RadioButton();
            this.areaMain = new System.Windows.Forms.RadioButton();
            this.areaScreen = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusTSSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.runtimeTSSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.rotateCb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MainFormClose = new SEC.Nanoeye.Controls.BitmapRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.fpsValue)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.BackColor = System.Drawing.Color.Gray;
            this.filename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.filename, "filename");
            this.filename.ForeColor = System.Drawing.Color.White;
            this.filename.Name = "filename";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            this.label1.Tag = "RECODER_FilePath";
            // 
            // file
            // 
            this.file.BackColor = System.Drawing.Color.Transparent;
            this.file.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_name_drop2_m_enable;
            resources.ApplyResources(this.file, "file");
            this.file.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.file.Name = "file";
            this.file.UseVisualStyleBackColor = false;
            this.file.Click += new System.EventHandler(this.file_Click);
            // 
            // run
            // 
            resources.ApplyResources(this.run, "run");
            this.run.AutoCheck = false;
            this.run.BackColor = System.Drawing.Color.Transparent;
            this.run.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.btn_record_enable;
            this.run.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.run.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.run.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.run.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.run.ForeColor = System.Drawing.Color.DimGray;
            this.run.Name = "run";
            this.run.UseVisualStyleBackColor = false;
            this.run.Click += new System.EventHandler(this.run_Click);
            // 
            // fpsValue
            // 
            this.fpsValue.DecimalPlaces = 1;
            this.fpsValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.fpsValue, "fpsValue");
            this.fpsValue.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fpsValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.fpsValue.Name = "fpsValue";
            this.fpsValue.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            this.label2.Tag = "RECODER_Fps";
            // 
            // areaImage
            // 
            this.areaImage.BackColor = System.Drawing.Color.Transparent;
            this.areaImage.Checked = true;
            resources.ApplyResources(this.areaImage, "areaImage");
            this.areaImage.ForeColor = System.Drawing.Color.White;
            this.areaImage.Name = "areaImage";
            this.areaImage.TabStop = true;
            this.areaImage.Tag = "RECODER_Image";
            this.areaImage.UseVisualStyleBackColor = false;
            // 
            // areaMain
            // 
            this.areaMain.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.areaMain, "areaMain");
            this.areaMain.ForeColor = System.Drawing.Color.White;
            this.areaMain.Name = "areaMain";
            this.areaMain.Tag = "RECODER_Program";
            this.areaMain.UseVisualStyleBackColor = false;
            // 
            // areaScreen
            // 
            this.areaScreen.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.areaScreen, "areaScreen");
            this.areaScreen.ForeColor = System.Drawing.Color.White;
            this.areaScreen.Name = "areaScreen";
            this.areaScreen.Tag = "RECODER_Screen";
            this.areaScreen.UseVisualStyleBackColor = false;
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusTSSL,
            this.runtimeTSSL});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // statusTSSL
            // 
            resources.ApplyResources(this.statusTSSL, "statusTSSL");
            this.statusTSSL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.statusTSSL.Name = "statusTSSL";
            // 
            // runtimeTSSL
            // 
            this.runtimeTSSL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.runtimeTSSL.Name = "runtimeTSSL";
            resources.ApplyResources(this.runtimeTSSL, "runtimeTSSL");
            // 
            // rotateCb
            // 
            this.rotateCb.BackColor = System.Drawing.Color.DimGray;
            this.rotateCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rotateCb.FormattingEnabled = true;
            resources.ApplyResources(this.rotateCb, "rotateCb");
            this.rotateCb.Name = "rotateCb";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            this.label3.Tag = "RECODER_Rotation";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label4.Name = "label4";
            // 
            // MainFormClose
            // 
            resources.ApplyResources(this.MainFormClose, "MainFormClose");
            this.MainFormClose.BackColor = System.Drawing.Color.Transparent;
            this.MainFormClose.ForeColor = System.Drawing.Color.DarkRed;
            this.MainFormClose.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_close_s;
            this.MainFormClose.Name = "MainFormClose";
            this.MainFormClose.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_bubble_close_e;
            this.MainFormClose.Tag = "";
            this.MainFormClose.UseCompatibleTextRendering = true;
            this.MainFormClose.Click += new System.EventHandler(this.FormClose);
            // 
            // ScreenRecorder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_record_bg;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.MainFormClose);
            this.Controls.Add(this.run);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rotateCb);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.areaScreen);
            this.Controls.Add(this.areaMain);
            this.Controls.Add(this.areaImage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fpsValue);
            this.Controls.Add(this.file);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filename);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenRecorder";
            this.ShowInTaskbar = false;
            this.Tag = "RECODER_Title";
            this.Shown += new System.EventHandler(this.FormShown);
            ((System.ComponentModel.ISupportInitialize)(this.fpsValue)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox filename;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button file;
		private System.Windows.Forms.CheckBox run;
		private System.Windows.Forms.NumericUpDown fpsValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton areaImage;
		private System.Windows.Forms.RadioButton areaMain;
		private System.Windows.Forms.RadioButton areaScreen;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusTSSL;
		private System.Windows.Forms.ToolStripStatusLabel runtimeTSSL;
		private System.Windows.Forms.ComboBox rotateCb;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private SEC.Nanoeye.Controls.BitmapRadioButton MainFormClose;
	}
}