namespace SEC.Nanoeye.Support.Dialog
{
	partial class HVmoniter
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

			if (_Column != null)
			{
				((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvElectronGun"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvElectronGun_RepeatUpdated);
				((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvFilament_RepeatUpdated);
				((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvGrid"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvGrid_RepeatUpdated);
				((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvCollector"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvClt_RepeatUpdated);
				((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvPmt"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvPmt_RepeatUpdated);
				_Column = null;
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.cltNud = new SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble();
			this.pmtNud = new SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble();
			this.gridNud = new SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble();
			this.tipNud = new SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble();
			this.anodeNud = new SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.hvenableCbicb = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool();
			this.pmtVmonLab = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cltCmonLab = new System.Windows.Forms.Label();
			this.cltVmonLab = new System.Windows.Forms.Label();
			this.pmtCmonLab = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.periodNud = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.runCb = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.filenameBut = new System.Windows.Forms.Button();
			this.filenameTb = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.gridCmonLab = new System.Windows.Forms.Label();
			this.gridVmonLab = new System.Windows.Forms.Label();
			this.tipCmonLab = new System.Windows.Forms.Label();
			this.tipVmonLab = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.anodeImonLab = new System.Windows.Forms.Label();
			this.anodeVmonLab = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cltNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pmtNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tipNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.anodeNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.periodNud)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.cltNud);
			this.splitContainer1.Panel1.Controls.Add(this.pmtNud);
			this.splitContainer1.Panel1.Controls.Add(this.gridNud);
			this.splitContainer1.Panel1.Controls.Add(this.tipNud);
			this.splitContainer1.Panel1.Controls.Add(this.anodeNud);
			this.splitContainer1.Panel1.Controls.Add(this.label13);
			this.splitContainer1.Panel1.Controls.Add(this.label12);
			this.splitContainer1.Panel1.Controls.Add(this.label11);
			this.splitContainer1.Panel1.Controls.Add(this.label10);
			this.splitContainer1.Panel1.Controls.Add(this.label9);
			this.splitContainer1.Panel1.Controls.Add(this.hvenableCbicb);
			this.splitContainer1.Panel1.Controls.Add(this.pmtVmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.cltCmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.cltVmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.pmtCmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.periodNud);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.runCb);
			this.splitContainer1.Panel1.Controls.Add(this.label8);
			this.splitContainer1.Panel1.Controls.Add(this.filenameBut);
			this.splitContainer1.Panel1.Controls.Add(this.filenameTb);
			this.splitContainer1.Panel1.Controls.Add(this.label7);
			this.splitContainer1.Panel1.Controls.Add(this.label6);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.gridCmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.gridVmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.tipCmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.tipVmonLab);
			this.splitContainer1.Panel1.Controls.Add(this.label25);
			this.splitContainer1.Panel1.Controls.Add(this.label19);
			this.splitContainer1.Panel1.Controls.Add(this.anodeImonLab);
			this.splitContainer1.Panel1.Controls.Add(this.anodeVmonLab);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Size = new System.Drawing.Size(572, 504);
			this.splitContainer1.SplitterDistance = 212;
			this.splitContainer1.TabIndex = 0;
			// 
			// cltNud
			// 
			this.cltNud.Location = new System.Drawing.Point(75, 471);
			this.cltNud.Name = "cltNud";
			this.cltNud.Size = new System.Drawing.Size(116, 21);
			this.cltNud.TabIndex = 106;
			// 
			// pmtNud
			// 
			this.pmtNud.Location = new System.Drawing.Point(75, 444);
			this.pmtNud.Name = "pmtNud";
			this.pmtNud.Size = new System.Drawing.Size(116, 21);
			this.pmtNud.TabIndex = 105;
			// 
			// gridNud
			// 
			this.gridNud.Location = new System.Drawing.Point(75, 417);
			this.gridNud.Name = "gridNud";
			this.gridNud.Size = new System.Drawing.Size(116, 21);
			this.gridNud.TabIndex = 104;
			// 
			// tipNud
			// 
			this.tipNud.Location = new System.Drawing.Point(75, 390);
			this.tipNud.Name = "tipNud";
			this.tipNud.Size = new System.Drawing.Size(116, 21);
			this.tipNud.TabIndex = 103;
			// 
			// anodeNud
			// 
			this.anodeNud.Location = new System.Drawing.Point(75, 363);
			this.anodeNud.Name = "anodeNud";
			this.anodeNud.Size = new System.Drawing.Size(116, 21);
			this.anodeNud.TabIndex = 102;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(28, 473);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(20, 12);
			this.label13.TabIndex = 101;
			this.label13.Text = "Clt";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(28, 446);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(27, 12);
			this.label12.TabIndex = 100;
			this.label12.Text = "Pmt";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(28, 419);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(28, 12);
			this.label11.TabIndex = 99;
			this.label11.Text = "Grid";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(28, 392);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(23, 12);
			this.label10.TabIndex = 98;
			this.label10.Text = "Tip";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(28, 365);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(41, 12);
			this.label9.TabIndex = 97;
			this.label9.Text = "Anode";
			// 
			// hvenableCbicb
			// 
			this.hvenableCbicb.AutoSize = true;
			this.hvenableCbicb.ControlValue = null;
			this.hvenableCbicb.Location = new System.Drawing.Point(15, 169);
			this.hvenableCbicb.Name = "hvenableCbicb";
			this.hvenableCbicb.Size = new System.Drawing.Size(86, 16);
			this.hvenableCbicb.TabIndex = 81;
			this.hvenableCbicb.Text = "HVEnabled";
			this.hvenableCbicb.UseVisualStyleBackColor = true;
			// 
			// pmtVmonLab
			// 
			this.pmtVmonLab.BackColor = System.Drawing.Color.Black;
			this.pmtVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pmtVmonLab.ForeColor = System.Drawing.Color.White;
			this.pmtVmonLab.Location = new System.Drawing.Point(75, 105);
			this.pmtVmonLab.Name = "pmtVmonLab";
			this.pmtVmonLab.Size = new System.Drawing.Size(60, 24);
			this.pmtVmonLab.TabIndex = 80;
			this.pmtVmonLab.Text = "0";
			this.pmtVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 129);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 24);
			this.label3.TabIndex = 79;
			this.label3.Text = "Collector";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 105);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 24);
			this.label4.TabIndex = 78;
			this.label4.Text = "Pmt";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cltCmonLab
			// 
			this.cltCmonLab.BackColor = System.Drawing.Color.Black;
			this.cltCmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.cltCmonLab.ForeColor = System.Drawing.Color.White;
			this.cltCmonLab.Location = new System.Drawing.Point(141, 129);
			this.cltCmonLab.Name = "cltCmonLab";
			this.cltCmonLab.Size = new System.Drawing.Size(60, 24);
			this.cltCmonLab.TabIndex = 77;
			this.cltCmonLab.Text = "0";
			this.cltCmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cltVmonLab
			// 
			this.cltVmonLab.BackColor = System.Drawing.Color.Black;
			this.cltVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.cltVmonLab.ForeColor = System.Drawing.Color.White;
			this.cltVmonLab.Location = new System.Drawing.Point(75, 129);
			this.cltVmonLab.Name = "cltVmonLab";
			this.cltVmonLab.Size = new System.Drawing.Size(60, 24);
			this.cltVmonLab.TabIndex = 76;
			this.cltVmonLab.Text = "0";
			this.cltVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pmtCmonLab
			// 
			this.pmtCmonLab.BackColor = System.Drawing.Color.Black;
			this.pmtCmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pmtCmonLab.ForeColor = System.Drawing.Color.White;
			this.pmtCmonLab.Location = new System.Drawing.Point(141, 105);
			this.pmtCmonLab.Name = "pmtCmonLab";
			this.pmtCmonLab.Size = new System.Drawing.Size(60, 24);
			this.pmtCmonLab.TabIndex = 75;
			this.pmtCmonLab.Text = "0";
			this.pmtCmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(156, 275);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 23);
			this.label2.TabIndex = 73;
			this.label2.Text = "sec";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// periodNud
			// 
			this.periodNud.DecimalPlaces = 1;
			this.periodNud.Location = new System.Drawing.Point(75, 278);
			this.periodNud.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
			this.periodNud.Name = "periodNud";
			this.periodNud.Size = new System.Drawing.Size(75, 21);
			this.periodNud.TabIndex = 72;
			this.periodNud.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.periodNud.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.periodNud.ValueChanged += new System.EventHandler(this.periodNud_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 275);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 71;
			this.label1.Text = "Period";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// runCb
			// 
			this.runCb.Appearance = System.Windows.Forms.Appearance.Button;
			this.runCb.Location = new System.Drawing.Point(54, 305);
			this.runCb.Name = "runCb";
			this.runCb.Size = new System.Drawing.Size(104, 52);
			this.runCb.TabIndex = 70;
			this.runCb.Text = "Start";
			this.runCb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.runCb.UseVisualStyleBackColor = true;
			this.runCb.CheckedChanged += new System.EventHandler(this.runCb_CheckedChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(13, 216);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 23);
			this.label8.TabIndex = 69;
			this.label8.Text = "File Name";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// filenameBut
			// 
			this.filenameBut.Location = new System.Drawing.Point(168, 216);
			this.filenameBut.Name = "filenameBut";
			this.filenameBut.Size = new System.Drawing.Size(33, 23);
			this.filenameBut.TabIndex = 68;
			this.filenameBut.Text = "...";
			this.filenameBut.UseVisualStyleBackColor = true;
			this.filenameBut.Click += new System.EventHandler(this.filenameBut_Click);
			// 
			// filenameTb
			// 
			this.filenameTb.BackColor = System.Drawing.Color.White;
			this.filenameTb.Location = new System.Drawing.Point(13, 245);
			this.filenameTb.Name = "filenameTb";
			this.filenameTb.ReadOnly = true;
			this.filenameTb.Size = new System.Drawing.Size(188, 21);
			this.filenameTb.TabIndex = 67;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(7, 81);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(62, 24);
			this.label7.TabIndex = 66;
			this.label7.Text = "Grid";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(7, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(62, 24);
			this.label6.TabIndex = 65;
			this.label6.Text = "Tip";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 33);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(62, 24);
			this.label5.TabIndex = 64;
			this.label5.Text = "Anode";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gridCmonLab
			// 
			this.gridCmonLab.BackColor = System.Drawing.Color.Black;
			this.gridCmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridCmonLab.ForeColor = System.Drawing.Color.White;
			this.gridCmonLab.Location = new System.Drawing.Point(141, 81);
			this.gridCmonLab.Name = "gridCmonLab";
			this.gridCmonLab.Size = new System.Drawing.Size(60, 24);
			this.gridCmonLab.TabIndex = 63;
			this.gridCmonLab.Text = "0";
			this.gridCmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gridVmonLab
			// 
			this.gridVmonLab.BackColor = System.Drawing.Color.Black;
			this.gridVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridVmonLab.ForeColor = System.Drawing.Color.White;
			this.gridVmonLab.Location = new System.Drawing.Point(75, 81);
			this.gridVmonLab.Name = "gridVmonLab";
			this.gridVmonLab.Size = new System.Drawing.Size(60, 24);
			this.gridVmonLab.TabIndex = 62;
			this.gridVmonLab.Text = "0";
			this.gridVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tipCmonLab
			// 
			this.tipCmonLab.BackColor = System.Drawing.Color.Black;
			this.tipCmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tipCmonLab.ForeColor = System.Drawing.Color.White;
			this.tipCmonLab.Location = new System.Drawing.Point(141, 57);
			this.tipCmonLab.Name = "tipCmonLab";
			this.tipCmonLab.Size = new System.Drawing.Size(60, 24);
			this.tipCmonLab.TabIndex = 59;
			this.tipCmonLab.Text = "0";
			this.tipCmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tipVmonLab
			// 
			this.tipVmonLab.BackColor = System.Drawing.Color.Black;
			this.tipVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tipVmonLab.ForeColor = System.Drawing.Color.White;
			this.tipVmonLab.Location = new System.Drawing.Point(75, 57);
			this.tipVmonLab.Name = "tipVmonLab";
			this.tipVmonLab.Size = new System.Drawing.Size(60, 24);
			this.tipVmonLab.TabIndex = 58;
			this.tipVmonLab.Text = "0";
			this.tipVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(139, 9);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(62, 24);
			this.label25.TabIndex = 60;
			this.label25.Text = "Current";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(73, 9);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(62, 24);
			this.label19.TabIndex = 61;
			this.label19.Text = "Voltage";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// anodeImonLab
			// 
			this.anodeImonLab.BackColor = System.Drawing.Color.Black;
			this.anodeImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.anodeImonLab.ForeColor = System.Drawing.Color.White;
			this.anodeImonLab.Location = new System.Drawing.Point(141, 33);
			this.anodeImonLab.Name = "anodeImonLab";
			this.anodeImonLab.Size = new System.Drawing.Size(60, 24);
			this.anodeImonLab.TabIndex = 57;
			this.anodeImonLab.Text = "0";
			this.anodeImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// anodeVmonLab
			// 
			this.anodeVmonLab.BackColor = System.Drawing.Color.Black;
			this.anodeVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.anodeVmonLab.ForeColor = System.Drawing.Color.White;
			this.anodeVmonLab.Location = new System.Drawing.Point(75, 33);
			this.anodeVmonLab.Name = "anodeVmonLab";
			this.anodeVmonLab.Size = new System.Drawing.Size(60, 24);
			this.anodeVmonLab.TabIndex = 56;
			this.anodeVmonLab.Text = "0";
			this.anodeVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(356, 504);
			this.textBox1.TabIndex = 0;
			this.textBox1.WordWrap = false;
			// 
			// updateTimer
			// 
			this.updateTimer.Interval = 60000;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// HVmoniter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(572, 504);
			this.Controls.Add(this.splitContainer1);
			this.Name = "HVmoniter";
			this.Text = "HVmoniter";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cltNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pmtNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tipNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.anodeNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.periodNud)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.SplitContainer splitContainer1;
		protected System.Windows.Forms.TextBox textBox1;
		protected System.Windows.Forms.Label tipCmonLab;
		protected System.Windows.Forms.Label tipVmonLab;
		protected System.Windows.Forms.Label label25;
		protected System.Windows.Forms.Label label19;
		protected System.Windows.Forms.Label anodeImonLab;
		protected System.Windows.Forms.Label anodeVmonLab;
		protected System.Windows.Forms.CheckBox runCb;
		protected System.Windows.Forms.Label label8;
		protected System.Windows.Forms.Button filenameBut;
		protected System.Windows.Forms.TextBox filenameTb;
		protected System.Windows.Forms.Label label7;
		protected System.Windows.Forms.Label label6;
		protected System.Windows.Forms.Label label5;
		protected System.Windows.Forms.Label gridCmonLab;
		protected System.Windows.Forms.Label gridVmonLab;
		protected System.Windows.Forms.Label label2;
		protected System.Windows.Forms.NumericUpDown periodNud;
		protected System.Windows.Forms.Label label1;
		protected System.Windows.Forms.Timer updateTimer;
		protected System.Windows.Forms.Label label3;
		protected System.Windows.Forms.Label label4;
		protected System.Windows.Forms.Label cltCmonLab;
		protected System.Windows.Forms.Label cltVmonLab;
		protected System.Windows.Forms.Label pmtCmonLab;
		protected System.Windows.Forms.Label pmtVmonLab;
		protected SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool hvenableCbicb;
		protected SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble cltNud;
		protected SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble pmtNud;
		protected SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble gridNud;
		protected SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble tipNud;
		protected SEC.Nanoeye.Support.Controls.NumericUpDownIControlDouble anodeNud;
		protected System.Windows.Forms.Label label13;
		protected System.Windows.Forms.Label label12;
		protected System.Windows.Forms.Label label11;
		protected System.Windows.Forms.Label label10;
		protected System.Windows.Forms.Label label9;

	}
}