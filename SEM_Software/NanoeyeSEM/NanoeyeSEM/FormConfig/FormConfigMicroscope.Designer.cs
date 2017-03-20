namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	partial class FormConfigMicroscope
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			//AnodeLink(false);
			//FilamentLink(false);
			//GridLink(false);
			//CollectorLink(false);
			//AmplifierLink(false); 

			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.profileAppend = new System.Windows.Forms.Button();
			this.profileRemove = new System.Windows.Forms.Button();
			this.detectorbsedCheck = new System.Windows.Forms.CheckBox();
			this.vacuumlowCheck = new System.Windows.Forms.CheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.m_TabMicroscope = new System.Windows.Forms.TabControl();
			this.tpgEghv = new System.Windows.Forms.TabPage();
			this.avGridHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.avFilamentHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.avAnodeHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.label5 = new System.Windows.Forms.Label();
			this.EghvTextTB = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.gridImonLab = new System.Windows.Forms.Label();
			this.gridVmonLab = new System.Windows.Forms.Label();
			this.tipImonLab = new System.Windows.Forms.Label();
			this.tipVmonLab = new System.Windows.Forms.Label();
			this.anodeImonLab = new System.Windows.Forms.Label();
			this.anodeVmonLab = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.tpgAlias = new System.Windows.Forms.TabPage();
			this.AliasText = new System.Windows.Forms.Label();
			this.m_Alias = new System.Windows.Forms.TextBox();
			this.tpgCondensorLens = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.ss2CL2Nud = new System.Windows.Forms.NumericUpDown();
			this.label15 = new System.Windows.Forms.Label();
			this.ss2CL1Nud = new System.Windows.Forms.NumericUpDown();
			this.label16 = new System.Windows.Forms.Label();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.ss3CL2Nud = new System.Windows.Forms.NumericUpDown();
			this.label17 = new System.Windows.Forms.Label();
			this.ss3CL1Nud = new System.Windows.Forms.NumericUpDown();
			this.label18 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.ss1CL2Nud = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.ss1CL1Nud = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.cl2ReverseCbicvd = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt();
			this.lensCl2Hswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.cl1ReverseCbicvd = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt();
			this.cl1MaxNudicvd = new System.Windows.Forms.NumericUpDown();
			this.cl1MinNudicvd = new System.Windows.Forms.NumericUpDown();
			this.lensCl1Hswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.label21 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.tpgFocusLens = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.olReverseCbicvd = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt();
			this.olCoarseMaxNudicvd = new System.Windows.Forms.NumericUpDown();
			this.olCoarseMinNudicvd = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.focusCoarseHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.olFineMaxNudicvd = new System.Windows.Forms.NumericUpDown();
			this.olFineMinNudicvd = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.focusFineHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.tpgMagnificationOld = new System.Windows.Forms.TabPage();
			this.cbWDsame = new System.Windows.Forms.CheckBox();
			this.scanExtern = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.m_MagFeedback = new System.Windows.Forms.NumericUpDown();
			this.staticLabel3 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.m_MagLower = new System.Windows.Forms.Button();
			this.m_MagLevel = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.m_MagUpper = new System.Windows.Forms.Button();
			this.m_MagWidth = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.m_MagRemove = new System.Windows.Forms.Button();
			this.m_MagAdd = new System.Windows.Forms.Button();
			this.m_MagSelector = new System.Windows.Forms.ListBox();
			this.m_MagHeight = new System.Windows.Forms.NumericUpDown();
			this.tpgDetector = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.amplifierImonLab = new System.Windows.Forms.Label();
			this.detectorAmplifyHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.amplifierVmonLab = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.collectorImonLab = new System.Windows.Forms.Label();
			this.detectorCollectorHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.collectorVmonLab = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.tpgScanRotation = new System.Windows.Forms.TabPage();
			this.rotationGunHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.rotationBeamHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.rotationScanHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
			this.profilenameLab = new System.Windows.Forms.Label();
			this.btnRestore = new System.Windows.Forms.Button();
			this.fncWobbleBnt = new System.Windows.Forms.Button();
			this.fncHVlogBnt = new System.Windows.Forms.Button();
			this.fncMonitorBnt = new System.Windows.Forms.Button();
			this.m_TabMicroscope.SuspendLayout();
			this.tpgEghv.SuspendLayout();
			this.tpgAlias.SuspendLayout();
			this.tpgCondensorLens.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss2CL2Nud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ss2CL1Nud)).BeginInit();
			this.groupBox9.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss3CL2Nud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ss3CL1Nud)).BeginInit();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss1CL2Nud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ss1CL1Nud)).BeginInit();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cl1MaxNudicvd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cl1MinNudicvd)).BeginInit();
			this.tpgFocusLens.SuspendLayout();
			this.groupBox8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.olCoarseMaxNudicvd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.olCoarseMinNudicvd)).BeginInit();
			this.groupBox11.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.olFineMaxNudicvd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.olFineMinNudicvd)).BeginInit();
			this.tpgMagnificationOld.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_MagFeedback)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagLevel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagHeight)).BeginInit();
			this.tpgDetector.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tpgScanRotation.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(317, 482);
			this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(62, 24);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.SystemButton_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(250, 482);
			this.btnApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(61, 24);
			this.btnApply.TabIndex = 1;
			this.btnApply.Text = "&Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.SystemButton_Click);
			// 
			// profileAppend
			// 
			this.profileAppend.Location = new System.Drawing.Point(176, 48);
			this.profileAppend.Name = "profileAppend";
			this.profileAppend.Size = new System.Drawing.Size(75, 23);
			this.profileAppend.TabIndex = 6;
			this.profileAppend.Text = "Append";
			this.profileAppend.UseVisualStyleBackColor = true;
			this.profileAppend.Click += new System.EventHandler(this.profileCnt_Click);
			// 
			// profileRemove
			// 
			this.profileRemove.Location = new System.Drawing.Point(272, 48);
			this.profileRemove.Name = "profileRemove";
			this.profileRemove.Size = new System.Drawing.Size(75, 23);
			this.profileRemove.TabIndex = 7;
			this.profileRemove.Text = "Remove";
			this.profileRemove.UseVisualStyleBackColor = true;
			this.profileRemove.Click += new System.EventHandler(this.profileCnt_Click);
			// 
			// detectorbsedCheck
			// 
			this.detectorbsedCheck.AutoSize = true;
			this.detectorbsedCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.DetectorBSED;
			this.detectorbsedCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "DetectorBSED", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.detectorbsedCheck.Location = new System.Drawing.Point(12, 481);
			this.detectorbsedCheck.Name = "detectorbsedCheck";
			this.detectorbsedCheck.Size = new System.Drawing.Size(59, 19);
			this.detectorbsedCheck.TabIndex = 1;
			this.detectorbsedCheck.Text = "BSED";
			this.detectorbsedCheck.UseVisualStyleBackColor = true;
			// 
			// vacuumlowCheck
			// 
			this.vacuumlowCheck.AutoSize = true;
			this.vacuumlowCheck.Checked = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.VacuumLow;
			this.vacuumlowCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "VacuumLow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.vacuumlowCheck.Location = new System.Drawing.Point(12, 506);
			this.vacuumlowCheck.Name = "vacuumlowCheck";
			this.vacuumlowCheck.Size = new System.Drawing.Size(49, 19);
			this.vacuumlowCheck.TabIndex = 1;
			this.vacuumlowCheck.Text = "Low";
			this.vacuumlowCheck.UseVisualStyleBackColor = true;
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(16, 16);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(112, 24);
			this.label40.TabIndex = 11;
			this.label40.Text = "Microscope Profile";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_TabMicroscope
			// 
			this.m_TabMicroscope.Controls.Add(this.tpgEghv);
			this.m_TabMicroscope.Controls.Add(this.tpgAlias);
			this.m_TabMicroscope.Controls.Add(this.tpgCondensorLens);
			this.m_TabMicroscope.Controls.Add(this.tpgFocusLens);
			this.m_TabMicroscope.Controls.Add(this.tpgMagnificationOld);
			this.m_TabMicroscope.Controls.Add(this.tpgDetector);
			this.m_TabMicroscope.Controls.Add(this.tpgScanRotation);
			this.m_TabMicroscope.ItemSize = new System.Drawing.Size(80, 20);
			this.m_TabMicroscope.Location = new System.Drawing.Point(-1, 82);
			this.m_TabMicroscope.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.m_TabMicroscope.Multiline = true;
			this.m_TabMicroscope.Name = "m_TabMicroscope";
			this.m_TabMicroscope.SelectedIndex = 0;
			this.m_TabMicroscope.Size = new System.Drawing.Size(392, 392);
			this.m_TabMicroscope.TabIndex = 12;
			// 
			// tpgEghv
			// 
			this.tpgEghv.BackColor = System.Drawing.Color.Transparent;
			this.tpgEghv.Controls.Add(this.avGridHswd);
			this.tpgEghv.Controls.Add(this.avFilamentHswd);
			this.tpgEghv.Controls.Add(this.avAnodeHswd);
			this.tpgEghv.Controls.Add(this.label5);
			this.tpgEghv.Controls.Add(this.EghvTextTB);
			this.tpgEghv.Controls.Add(this.label30);
			this.tpgEghv.Controls.Add(this.label29);
			this.tpgEghv.Controls.Add(this.label22);
			this.tpgEghv.Controls.Add(this.label20);
			this.tpgEghv.Controls.Add(this.gridImonLab);
			this.tpgEghv.Controls.Add(this.gridVmonLab);
			this.tpgEghv.Controls.Add(this.tipImonLab);
			this.tpgEghv.Controls.Add(this.tipVmonLab);
			this.tpgEghv.Controls.Add(this.anodeImonLab);
			this.tpgEghv.Controls.Add(this.anodeVmonLab);
			this.tpgEghv.Controls.Add(this.label25);
			this.tpgEghv.Controls.Add(this.label19);
			this.tpgEghv.Location = new System.Drawing.Point(4, 44);
			this.tpgEghv.Name = "tpgEghv";
			this.tpgEghv.Padding = new System.Windows.Forms.Padding(3);
			this.tpgEghv.Size = new System.Drawing.Size(384, 344);
			this.tpgEghv.TabIndex = 2;
			this.tpgEghv.Text = "Accelerated Voltage";
			this.tpgEghv.UseVisualStyleBackColor = true;
			// 
			// avGridHswd
			// 
			this.avGridHswd.DisplayName = "Grid";
			this.avGridHswd.IsLimitedMode = false;
			this.avGridHswd.Location = new System.Drawing.Point(11, 169);
			this.avGridHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.avGridHswd.Maximum = 100;
			this.avGridHswd.Minimum = 0;
			this.avGridHswd.Name = "avGridHswd";
			this.avGridHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			this.avGridHswd.NameSize = new System.Drawing.Size(60, 11925);
			this.avGridHswd.Size = new System.Drawing.Size(354, 35);
			this.avGridHswd.TabIndex = 49;
			this.avGridHswd.Value = 0;
			this.avGridHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.avGridHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			// 
			// avFilamentHswd
			// 
			this.avFilamentHswd.DisplayName = "Filament";
			this.avFilamentHswd.IsLimitedMode = false;
			this.avFilamentHswd.Location = new System.Drawing.Point(11, 98);
			this.avFilamentHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.avFilamentHswd.Maximum = 100;
			this.avFilamentHswd.Minimum = 0;
			this.avFilamentHswd.Name = "avFilamentHswd";
			this.avFilamentHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			this.avFilamentHswd.NameSize = new System.Drawing.Size(60, 11925);
			this.avFilamentHswd.Size = new System.Drawing.Size(354, 30);
			this.avFilamentHswd.TabIndex = 48;
			this.avFilamentHswd.Value = 0;
			this.avFilamentHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.avFilamentHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			// 
			// avAnodeHswd
			// 
			this.avAnodeHswd.DisplayName = "Anode";
			this.avAnodeHswd.IsLimitedMode = false;
			this.avAnodeHswd.Location = new System.Drawing.Point(11, 18);
			this.avAnodeHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.avAnodeHswd.Maximum = 100;
			this.avAnodeHswd.Minimum = 0;
			this.avAnodeHswd.Name = "avAnodeHswd";
			this.avAnodeHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			this.avAnodeHswd.NameSize = new System.Drawing.Size(60, 11925);
			this.avAnodeHswd.Size = new System.Drawing.Size(354, 23);
			this.avAnodeHswd.TabIndex = 47;
			this.avAnodeHswd.Value = 0;
			this.avAnodeHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.avAnodeHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 256);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 15);
			this.label5.TabIndex = 38;
			this.label5.Text = "EGHV Text";
			// 
			// EghvTextTB
			// 
			this.EghvTextTB.Location = new System.Drawing.Point(78, 253);
			this.EghvTextTB.Name = "EghvTextTB";
			this.EghvTextTB.Size = new System.Drawing.Size(230, 21);
			this.EghvTextTB.TabIndex = 37;
			this.EghvTextTB.TextChanged += new System.EventHandler(this.EghvTextTB_TextChanged);
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(176, 204);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(48, 24);
			this.label30.TabIndex = 50;
			this.label30.Text = "Current";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(56, 204);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(56, 24);
			this.label29.TabIndex = 51;
			this.label29.Text = "Voltage";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(176, 132);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(48, 24);
			this.label22.TabIndex = 52;
			this.label22.Text = "Current";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(56, 132);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(56, 24);
			this.label20.TabIndex = 53;
			this.label20.Text = "Voltage";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridImonLab
			// 
			this.gridImonLab.BackColor = System.Drawing.Color.Black;
			this.gridImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridImonLab.ForeColor = System.Drawing.Color.White;
			this.gridImonLab.Location = new System.Drawing.Point(224, 208);
			this.gridImonLab.Name = "gridImonLab";
			this.gridImonLab.Size = new System.Drawing.Size(60, 24);
			this.gridImonLab.TabIndex = 28;
			this.gridImonLab.Text = "0";
			this.gridImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.gridImonLab.Click += new System.EventHandler(this.gridInfo_Click);
			// 
			// gridVmonLab
			// 
			this.gridVmonLab.BackColor = System.Drawing.Color.Black;
			this.gridVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridVmonLab.ForeColor = System.Drawing.Color.White;
			this.gridVmonLab.Location = new System.Drawing.Point(112, 208);
			this.gridVmonLab.Name = "gridVmonLab";
			this.gridVmonLab.Size = new System.Drawing.Size(60, 24);
			this.gridVmonLab.TabIndex = 27;
			this.gridVmonLab.Text = "0";
			this.gridVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.gridVmonLab.Click += new System.EventHandler(this.gridInfo_Click);
			// 
			// tipImonLab
			// 
			this.tipImonLab.BackColor = System.Drawing.Color.Black;
			this.tipImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tipImonLab.ForeColor = System.Drawing.Color.White;
			this.tipImonLab.Location = new System.Drawing.Point(224, 132);
			this.tipImonLab.Name = "tipImonLab";
			this.tipImonLab.Size = new System.Drawing.Size(60, 24);
			this.tipImonLab.TabIndex = 26;
			this.tipImonLab.Text = "0";
			this.tipImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tipImonLab.Click += new System.EventHandler(this.filamentInfo_Click);
			// 
			// tipVmonLab
			// 
			this.tipVmonLab.BackColor = System.Drawing.Color.Black;
			this.tipVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tipVmonLab.ForeColor = System.Drawing.Color.White;
			this.tipVmonLab.Location = new System.Drawing.Point(112, 132);
			this.tipVmonLab.Name = "tipVmonLab";
			this.tipVmonLab.Size = new System.Drawing.Size(60, 24);
			this.tipVmonLab.TabIndex = 25;
			this.tipVmonLab.Text = "0";
			this.tipVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.tipVmonLab.Click += new System.EventHandler(this.filamentInfo_Click);
			// 
			// anodeImonLab
			// 
			this.anodeImonLab.BackColor = System.Drawing.Color.Black;
			this.anodeImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.anodeImonLab.ForeColor = System.Drawing.Color.White;
			this.anodeImonLab.Location = new System.Drawing.Point(224, 45);
			this.anodeImonLab.Name = "anodeImonLab";
			this.anodeImonLab.Size = new System.Drawing.Size(60, 24);
			this.anodeImonLab.TabIndex = 13;
			this.anodeImonLab.Text = "0";
			this.anodeImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.anodeImonLab.Click += new System.EventHandler(this.anodeInfo_Click);
			// 
			// anodeVmonLab
			// 
			this.anodeVmonLab.BackColor = System.Drawing.Color.Black;
			this.anodeVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.anodeVmonLab.ForeColor = System.Drawing.Color.White;
			this.anodeVmonLab.Location = new System.Drawing.Point(112, 45);
			this.anodeVmonLab.Name = "anodeVmonLab";
			this.anodeVmonLab.Size = new System.Drawing.Size(60, 24);
			this.anodeVmonLab.TabIndex = 13;
			this.anodeVmonLab.Text = "0";
			this.anodeVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.anodeVmonLab.Click += new System.EventHandler(this.anodeInfo_Click);
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(176, 45);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(48, 24);
			this.label25.TabIndex = 54;
			this.label25.Text = "Current";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(56, 45);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(48, 24);
			this.label19.TabIndex = 55;
			this.label19.Text = "Voltage";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tpgAlias
			// 
			this.tpgAlias.Controls.Add(this.AliasText);
			this.tpgAlias.Controls.Add(this.m_Alias);
			this.tpgAlias.Location = new System.Drawing.Point(4, 24);
			this.tpgAlias.Name = "tpgAlias";
			this.tpgAlias.Size = new System.Drawing.Size(384, 364);
			this.tpgAlias.TabIndex = 5;
			this.tpgAlias.Text = "Alias";
			this.tpgAlias.UseVisualStyleBackColor = true;
			// 
			// AliasText
			// 
			this.AliasText.AutoSize = true;
			this.AliasText.Location = new System.Drawing.Point(18, 54);
			this.AliasText.Name = "AliasText";
			this.AliasText.Size = new System.Drawing.Size(34, 15);
			this.AliasText.TabIndex = 1;
			this.AliasText.Text = "Alias";
			// 
			// m_Alias
			// 
			this.m_Alias.Location = new System.Drawing.Point(58, 51);
			this.m_Alias.Name = "m_Alias";
			this.m_Alias.Size = new System.Drawing.Size(240, 21);
			this.m_Alias.TabIndex = 0;
			this.m_Alias.Leave += new System.EventHandler(this.m_Alias_Leave);
			// 
			// tpgCondensorLens
			// 
			this.tpgCondensorLens.BackColor = System.Drawing.Color.Transparent;
			this.tpgCondensorLens.Controls.Add(this.groupBox5);
			this.tpgCondensorLens.Controls.Add(this.groupBox9);
			this.tpgCondensorLens.Controls.Add(this.groupBox4);
			this.tpgCondensorLens.Controls.Add(this.groupBox7);
			this.tpgCondensorLens.Controls.Add(this.groupBox6);
			this.tpgCondensorLens.Location = new System.Drawing.Point(4, 24);
			this.tpgCondensorLens.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tpgCondensorLens.Name = "tpgCondensorLens";
			this.tpgCondensorLens.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tpgCondensorLens.Size = new System.Drawing.Size(384, 364);
			this.tpgCondensorLens.TabIndex = 1;
			this.tpgCondensorLens.Text = "Condenser Lens";
			this.tpgCondensorLens.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.ss2CL2Nud);
			this.groupBox5.Controls.Add(this.label15);
			this.groupBox5.Controls.Add(this.ss2CL1Nud);
			this.groupBox5.Controls.Add(this.label16);
			this.groupBox5.Location = new System.Drawing.Point(131, 223);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(116, 80);
			this.groupBox5.TabIndex = 11;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Spot Size 2";
			// 
			// ss2CL2Nud
			// 
			this.ss2CL2Nud.Location = new System.Drawing.Point(43, 46);
			this.ss2CL2Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss2CL2Nud.Name = "ss2CL2Nud";
			this.ss2CL2Nud.Size = new System.Drawing.Size(64, 21);
			this.ss2CL2Nud.TabIndex = 3;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(7, 46);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(30, 15);
			this.label15.TabIndex = 2;
			this.label15.Text = "CL2";
			// 
			// ss2CL1Nud
			// 
			this.ss2CL1Nud.Location = new System.Drawing.Point(43, 19);
			this.ss2CL1Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss2CL1Nud.Name = "ss2CL1Nud";
			this.ss2CL1Nud.Size = new System.Drawing.Size(64, 21);
			this.ss2CL1Nud.TabIndex = 1;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(7, 21);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(30, 15);
			this.label16.TabIndex = 0;
			this.label16.Text = "CL1";
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.ss3CL2Nud);
			this.groupBox9.Controls.Add(this.label17);
			this.groupBox9.Controls.Add(this.ss3CL1Nud);
			this.groupBox9.Controls.Add(this.label18);
			this.groupBox9.Location = new System.Drawing.Point(253, 223);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(116, 80);
			this.groupBox9.TabIndex = 10;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Spot Size 3";
			// 
			// ss3CL2Nud
			// 
			this.ss3CL2Nud.Location = new System.Drawing.Point(43, 46);
			this.ss3CL2Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss3CL2Nud.Name = "ss3CL2Nud";
			this.ss3CL2Nud.Size = new System.Drawing.Size(64, 21);
			this.ss3CL2Nud.TabIndex = 3;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(7, 46);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(30, 15);
			this.label17.TabIndex = 2;
			this.label17.Text = "CL2";
			// 
			// ss3CL1Nud
			// 
			this.ss3CL1Nud.Location = new System.Drawing.Point(43, 19);
			this.ss3CL1Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss3CL1Nud.Name = "ss3CL1Nud";
			this.ss3CL1Nud.Size = new System.Drawing.Size(64, 21);
			this.ss3CL1Nud.TabIndex = 1;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(7, 21);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(30, 15);
			this.label18.TabIndex = 0;
			this.label18.Text = "CL1";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.ss1CL2Nud);
			this.groupBox4.Controls.Add(this.label14);
			this.groupBox4.Controls.Add(this.ss1CL1Nud);
			this.groupBox4.Controls.Add(this.label13);
			this.groupBox4.Location = new System.Drawing.Point(8, 223);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(117, 80);
			this.groupBox4.TabIndex = 9;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Spot Size 1";
			// 
			// ss1CL2Nud
			// 
			this.ss1CL2Nud.Location = new System.Drawing.Point(43, 46);
			this.ss1CL2Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss1CL2Nud.Name = "ss1CL2Nud";
			this.ss1CL2Nud.Size = new System.Drawing.Size(64, 21);
			this.ss1CL2Nud.TabIndex = 3;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(7, 46);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(30, 15);
			this.label14.TabIndex = 2;
			this.label14.Text = "CL2";
			// 
			// ss1CL1Nud
			// 
			this.ss1CL1Nud.Location = new System.Drawing.Point(43, 19);
			this.ss1CL1Nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ss1CL1Nud.Name = "ss1CL1Nud";
			this.ss1CL1Nud.Size = new System.Drawing.Size(64, 21);
			this.ss1CL1Nud.TabIndex = 1;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(7, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(30, 15);
			this.label13.TabIndex = 0;
			this.label13.Text = "CL1";
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.cl2ReverseCbicvd);
			this.groupBox7.Controls.Add(this.lensCl2Hswd);
			this.groupBox7.Location = new System.Drawing.Point(8, 117);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(368, 100);
			this.groupBox7.TabIndex = 8;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Condenser Lens 2";
			// 
			// cl2ReverseCbicvd
			// 
			this.cl2ReverseCbicvd.AutoSize = true;
			this.cl2ReverseCbicvd.ControlValue = null;
			this.cl2ReverseCbicvd.Location = new System.Drawing.Point(290, 20);
			this.cl2ReverseCbicvd.Name = "cl2ReverseCbicvd";
			this.cl2ReverseCbicvd.Size = new System.Drawing.Size(72, 19);
			this.cl2ReverseCbicvd.TabIndex = 54;
			this.cl2ReverseCbicvd.Text = "Reverse";
			this.cl2ReverseCbicvd.UseVisualStyleBackColor = true;
			// 
			// lensCl2Hswd
			// 
			this.lensCl2Hswd.DisplayName = "Initial";
			this.lensCl2Hswd.IsLimitedMode = false;
			this.lensCl2Hswd.Location = new System.Drawing.Point(4, 46);
			this.lensCl2Hswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.lensCl2Hswd.Maximum = 100;
			this.lensCl2Hswd.Minimum = 0;
			this.lensCl2Hswd.Name = "lensCl2Hswd";
			this.lensCl2Hswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.lensCl2Hswd.NameSize = new System.Drawing.Size(100, 289);
			this.lensCl2Hswd.Size = new System.Drawing.Size(356, 50);
			this.lensCl2Hswd.TabIndex = 49;
			this.lensCl2Hswd.Value = 0;
			this.lensCl2Hswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.lensCl2Hswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.cl1ReverseCbicvd);
			this.groupBox6.Controls.Add(this.cl1MaxNudicvd);
			this.groupBox6.Controls.Add(this.cl1MinNudicvd);
			this.groupBox6.Controls.Add(this.lensCl1Hswd);
			this.groupBox6.Controls.Add(this.label21);
			this.groupBox6.Controls.Add(this.label27);
			this.groupBox6.Location = new System.Drawing.Point(8, 8);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(368, 101);
			this.groupBox6.TabIndex = 7;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Condenser Lens 1";
			// 
			// cl1ReverseCbicvd
			// 
			this.cl1ReverseCbicvd.AutoSize = true;
			this.cl1ReverseCbicvd.ControlValue = null;
			this.cl1ReverseCbicvd.Location = new System.Drawing.Point(276, 20);
			this.cl1ReverseCbicvd.Name = "cl1ReverseCbicvd";
			this.cl1ReverseCbicvd.Size = new System.Drawing.Size(72, 19);
			this.cl1ReverseCbicvd.TabIndex = 51;
			this.cl1ReverseCbicvd.Text = "Reverse";
			this.cl1ReverseCbicvd.UseVisualStyleBackColor = true;
			// 
			// cl1MaxNudicvd
			// 
			this.cl1MaxNudicvd.Location = new System.Drawing.Point(209, 19);
			this.cl1MaxNudicvd.Name = "cl1MaxNudicvd";
			this.cl1MaxNudicvd.Size = new System.Drawing.Size(61, 21);
			this.cl1MaxNudicvd.TabIndex = 50;
			this.cl1MaxNudicvd.ValueChanged += new System.EventHandler(this.clNudicvd_ValueChanged);
			// 
			// cl1MinNudicvd
			// 
			this.cl1MinNudicvd.Location = new System.Drawing.Point(72, 18);
			this.cl1MinNudicvd.Name = "cl1MinNudicvd";
			this.cl1MinNudicvd.Size = new System.Drawing.Size(61, 21);
			this.cl1MinNudicvd.TabIndex = 49;
			this.cl1MinNudicvd.ValueChanged += new System.EventHandler(this.clNudicvd_ValueChanged);
			// 
			// lensCl1Hswd
			// 
			this.lensCl1Hswd.DisplayName = "Initial";
			this.lensCl1Hswd.IsLimitedMode = false;
			this.lensCl1Hswd.Location = new System.Drawing.Point(6, 47);
			this.lensCl1Hswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.lensCl1Hswd.Maximum = 100;
			this.lensCl1Hswd.Minimum = 0;
			this.lensCl1Hswd.Name = "lensCl1Hswd";
			this.lensCl1Hswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.lensCl1Hswd.NameSize = new System.Drawing.Size(100, 289);
			this.lensCl1Hswd.Size = new System.Drawing.Size(354, 55);
			this.lensCl1Hswd.TabIndex = 48;
			this.lensCl1Hswd.Value = 0;
			this.lensCl1Hswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.lensCl1Hswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(139, 16);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(64, 24);
			this.label21.TabIndex = 14;
			this.label21.Text = "Maximum";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(7, 16);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(59, 24);
			this.label27.TabIndex = 16;
			this.label27.Text = "Minimum";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tpgFocusLens
			// 
			this.tpgFocusLens.BackColor = System.Drawing.Color.Transparent;
			this.tpgFocusLens.Controls.Add(this.groupBox8);
			this.tpgFocusLens.Controls.Add(this.groupBox11);
			this.tpgFocusLens.Location = new System.Drawing.Point(4, 24);
			this.tpgFocusLens.Name = "tpgFocusLens";
			this.tpgFocusLens.Padding = new System.Windows.Forms.Padding(3);
			this.tpgFocusLens.Size = new System.Drawing.Size(384, 364);
			this.tpgFocusLens.TabIndex = 4;
			this.tpgFocusLens.Text = "Focus";
			this.tpgFocusLens.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.olReverseCbicvd);
			this.groupBox8.Controls.Add(this.olCoarseMaxNudicvd);
			this.groupBox8.Controls.Add(this.olCoarseMinNudicvd);
			this.groupBox8.Controls.Add(this.label3);
			this.groupBox8.Controls.Add(this.label4);
			this.groupBox8.Controls.Add(this.focusCoarseHswd);
			this.groupBox8.Location = new System.Drawing.Point(8, 8);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(368, 128);
			this.groupBox8.TabIndex = 8;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Coarse";
			// 
			// olReverseCbicvd
			// 
			this.olReverseCbicvd.AutoSize = true;
			this.olReverseCbicvd.ControlValue = null;
			this.olReverseCbicvd.Location = new System.Drawing.Point(279, 21);
			this.olReverseCbicvd.Name = "olReverseCbicvd";
			this.olReverseCbicvd.Size = new System.Drawing.Size(72, 19);
			this.olReverseCbicvd.TabIndex = 56;
			this.olReverseCbicvd.Text = "Reverse";
			this.olReverseCbicvd.UseVisualStyleBackColor = true;
			// 
			// olCoarseMaxNudicvd
			// 
			this.olCoarseMaxNudicvd.Location = new System.Drawing.Point(212, 21);
			this.olCoarseMaxNudicvd.Name = "olCoarseMaxNudicvd";
			this.olCoarseMaxNudicvd.Size = new System.Drawing.Size(61, 21);
			this.olCoarseMaxNudicvd.TabIndex = 55;
			this.olCoarseMaxNudicvd.ValueChanged += new System.EventHandler(this.Focus_ValueChanged);
			// 
			// olCoarseMinNudicvd
			// 
			this.olCoarseMinNudicvd.Location = new System.Drawing.Point(75, 20);
			this.olCoarseMinNudicvd.Name = "olCoarseMinNudicvd";
			this.olCoarseMinNudicvd.Size = new System.Drawing.Size(61, 21);
			this.olCoarseMinNudicvd.TabIndex = 54;
			this.olCoarseMinNudicvd.ValueChanged += new System.EventHandler(this.Focus_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(142, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 24);
			this.label3.TabIndex = 52;
			this.label3.Text = "Maximum";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10, 18);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 24);
			this.label4.TabIndex = 53;
			this.label4.Text = "Minimum";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// focusCoarseHswd
			// 
			this.focusCoarseHswd.DisplayName = "Initial";
			this.focusCoarseHswd.IsLimitedMode = false;
			this.focusCoarseHswd.Location = new System.Drawing.Point(6, 61);
			this.focusCoarseHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.focusCoarseHswd.Maximum = 100;
			this.focusCoarseHswd.Minimum = 0;
			this.focusCoarseHswd.Name = "focusCoarseHswd";
			this.focusCoarseHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.focusCoarseHswd.NameSize = new System.Drawing.Size(100, 289);
			this.focusCoarseHswd.Size = new System.Drawing.Size(354, 60);
			this.focusCoarseHswd.TabIndex = 49;
			this.focusCoarseHswd.Value = 0;
			this.focusCoarseHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.focusCoarseHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.olFineMaxNudicvd);
			this.groupBox11.Controls.Add(this.olFineMinNudicvd);
			this.groupBox11.Controls.Add(this.label8);
			this.groupBox11.Controls.Add(this.label9);
			this.groupBox11.Controls.Add(this.focusFineHswd);
			this.groupBox11.Location = new System.Drawing.Point(8, 144);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(368, 128);
			this.groupBox11.TabIndex = 6;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Fine";
			// 
			// olFineMaxNudicvd
			// 
			this.olFineMaxNudicvd.Location = new System.Drawing.Point(212, 31);
			this.olFineMaxNudicvd.Name = "olFineMaxNudicvd";
			this.olFineMaxNudicvd.Size = new System.Drawing.Size(61, 21);
			this.olFineMaxNudicvd.TabIndex = 55;
			this.olFineMaxNudicvd.ValueChanged += new System.EventHandler(this.Focus_ValueChanged);
			// 
			// olFineMinNudicvd
			// 
			this.olFineMinNudicvd.Location = new System.Drawing.Point(75, 30);
			this.olFineMinNudicvd.Name = "olFineMinNudicvd";
			this.olFineMinNudicvd.Size = new System.Drawing.Size(61, 21);
			this.olFineMinNudicvd.TabIndex = 54;
			this.olFineMinNudicvd.ValueChanged += new System.EventHandler(this.Focus_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(142, 28);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 24);
			this.label8.TabIndex = 52;
			this.label8.Text = "Maximum";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(10, 28);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(59, 24);
			this.label9.TabIndex = 53;
			this.label9.Text = "Minimum";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// focusFineHswd
			// 
			this.focusFineHswd.DisplayName = "Initial";
			this.focusFineHswd.IsLimitedMode = false;
			this.focusFineHswd.Location = new System.Drawing.Point(6, 65);
			this.focusFineHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.focusFineHswd.Maximum = 100;
			this.focusFineHswd.Minimum = 0;
			this.focusFineHswd.Name = "focusFineHswd";
			this.focusFineHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.focusFineHswd.NameSize = new System.Drawing.Size(100, 289);
			this.focusFineHswd.Size = new System.Drawing.Size(354, 56);
			this.focusFineHswd.TabIndex = 50;
			this.focusFineHswd.Value = 0;
			this.focusFineHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.focusFineHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// tpgMagnificationOld
			// 
			this.tpgMagnificationOld.BackColor = System.Drawing.Color.Transparent;
			this.tpgMagnificationOld.Controls.Add(this.cbWDsame);
			this.tpgMagnificationOld.Controls.Add(this.scanExtern);
			this.tpgMagnificationOld.Controls.Add(this.groupBox1);
			this.tpgMagnificationOld.Location = new System.Drawing.Point(4, 44);
			this.tpgMagnificationOld.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tpgMagnificationOld.Name = "tpgMagnificationOld";
			this.tpgMagnificationOld.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tpgMagnificationOld.Size = new System.Drawing.Size(384, 344);
			this.tpgMagnificationOld.TabIndex = 0;
			this.tpgMagnificationOld.Text = "Magnification";
			this.tpgMagnificationOld.UseVisualStyleBackColor = true;
			// 
			// cbWDsame
			// 
			this.cbWDsame.AutoSize = true;
			this.cbWDsame.Checked = true;
			this.cbWDsame.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbWDsame.Location = new System.Drawing.Point(272, 312);
			this.cbWDsame.Name = "cbWDsame";
			this.cbWDsame.Size = new System.Drawing.Size(81, 19);
			this.cbWDsame.TabIndex = 4;
			this.cbWDsame.Text = "WD same";
			this.cbWDsame.UseVisualStyleBackColor = true;
			this.cbWDsame.CheckedChanged += new System.EventHandler(this.cbWDsame_CheckedChanged);
			// 
			// scanExtern
			// 
			this.scanExtern.Appearance = System.Windows.Forms.Appearance.Button;
			this.scanExtern.AutoSize = true;
			this.scanExtern.Location = new System.Drawing.Point(16, 312);
			this.scanExtern.Name = "scanExtern";
			this.scanExtern.Size = new System.Drawing.Size(92, 25);
			this.scanExtern.TabIndex = 3;
			this.scanExtern.Text = "External Scan";
			this.scanExtern.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.m_MagFeedback);
			this.groupBox1.Controls.Add(this.staticLabel3);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.m_MagLower);
			this.groupBox1.Controls.Add(this.m_MagLevel);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.m_MagUpper);
			this.groupBox1.Controls.Add(this.m_MagWidth);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.m_MagRemove);
			this.groupBox1.Controls.Add(this.m_MagAdd);
			this.groupBox1.Controls.Add(this.m_MagSelector);
			this.groupBox1.Controls.Add(this.m_MagHeight);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 288);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Magnification List";
			// 
			// m_MagFeedback
			// 
			this.m_MagFeedback.Location = new System.Drawing.Point(264, 248);
			this.m_MagFeedback.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_MagFeedback.Name = "m_MagFeedback";
			this.m_MagFeedback.Size = new System.Drawing.Size(80, 21);
			this.m_MagFeedback.TabIndex = 28;
			this.m_MagFeedback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.m_MagFeedback.ValueChanged += new System.EventHandler(this.m_MagFeedback_ValueChanged);
			// 
			// staticLabel3
			// 
			this.staticLabel3.Location = new System.Drawing.Point(224, 248);
			this.staticLabel3.Name = "staticLabel3";
			this.staticLabel3.Size = new System.Drawing.Size(40, 24);
			this.staticLabel3.TabIndex = 29;
			this.staticLabel3.Text = "F.B.";
			this.staticLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(224, 152);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(40, 24);
			this.label28.TabIndex = 30;
			this.label28.Text = "Mag.";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_MagLower
			// 
			this.m_MagLower.Location = new System.Drawing.Point(256, 120);
			this.m_MagLower.Name = "m_MagLower";
			this.m_MagLower.Size = new System.Drawing.Size(75, 23);
			this.m_MagLower.TabIndex = 18;
			this.m_MagLower.Text = "Down";
			this.m_MagLower.UseVisualStyleBackColor = true;
			this.m_MagLower.Click += new System.EventHandler(this.m_Mag_Click);
			// 
			// m_MagLevel
			// 
			this.m_MagLevel.Location = new System.Drawing.Point(264, 152);
			this.m_MagLevel.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.m_MagLevel.Name = "m_MagLevel";
			this.m_MagLevel.Size = new System.Drawing.Size(80, 21);
			this.m_MagLevel.TabIndex = 16;
			this.m_MagLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.m_MagLevel.ValueChanged += new System.EventHandler(this.m_MagLevel_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(224, 184);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 24);
			this.label1.TabIndex = 31;
			this.label1.Text = "Width";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_MagUpper
			// 
			this.m_MagUpper.Location = new System.Drawing.Point(256, 88);
			this.m_MagUpper.Name = "m_MagUpper";
			this.m_MagUpper.Size = new System.Drawing.Size(75, 23);
			this.m_MagUpper.TabIndex = 18;
			this.m_MagUpper.Text = "Up";
			this.m_MagUpper.UseVisualStyleBackColor = true;
			this.m_MagUpper.Click += new System.EventHandler(this.m_Mag_Click);
			// 
			// m_MagWidth
			// 
			this.m_MagWidth.DecimalPlaces = 6;
			this.m_MagWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this.m_MagWidth.Location = new System.Drawing.Point(264, 184);
			this.m_MagWidth.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_MagWidth.Name = "m_MagWidth";
			this.m_MagWidth.Size = new System.Drawing.Size(80, 21);
			this.m_MagWidth.TabIndex = 16;
			this.m_MagWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.m_MagWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_MagWidth.ValueChanged += new System.EventHandler(this.m_MagWidth_ValueChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(224, 216);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 24);
			this.label2.TabIndex = 32;
			this.label2.Text = "Height";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_MagRemove
			// 
			this.m_MagRemove.Location = new System.Drawing.Point(256, 56);
			this.m_MagRemove.Name = "m_MagRemove";
			this.m_MagRemove.Size = new System.Drawing.Size(75, 23);
			this.m_MagRemove.TabIndex = 18;
			this.m_MagRemove.Text = "Delete";
			this.m_MagRemove.UseVisualStyleBackColor = true;
			this.m_MagRemove.Click += new System.EventHandler(this.m_Mag_Click);
			// 
			// m_MagAdd
			// 
			this.m_MagAdd.Location = new System.Drawing.Point(256, 24);
			this.m_MagAdd.Name = "m_MagAdd";
			this.m_MagAdd.Size = new System.Drawing.Size(75, 23);
			this.m_MagAdd.TabIndex = 18;
			this.m_MagAdd.Text = "Append";
			this.m_MagAdd.UseVisualStyleBackColor = true;
			this.m_MagAdd.Click += new System.EventHandler(this.m_Mag_Click);
			// 
			// m_MagSelector
			// 
			this.m_MagSelector.FormattingEnabled = true;
			this.m_MagSelector.ItemHeight = 15;
			this.m_MagSelector.Location = new System.Drawing.Point(8, 24);
			this.m_MagSelector.Name = "m_MagSelector";
			this.m_MagSelector.ScrollAlwaysVisible = true;
			this.m_MagSelector.Size = new System.Drawing.Size(208, 244);
			this.m_MagSelector.TabIndex = 15;
			this.m_MagSelector.SelectedIndexChanged += new System.EventHandler(this.m_MagSelector_SelectedIndexChanged);
			// 
			// m_MagHeight
			// 
			this.m_MagHeight.DecimalPlaces = 6;
			this.m_MagHeight.Enabled = false;
			this.m_MagHeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this.m_MagHeight.Location = new System.Drawing.Point(264, 216);
			this.m_MagHeight.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_MagHeight.Name = "m_MagHeight";
			this.m_MagHeight.Size = new System.Drawing.Size(80, 21);
			this.m_MagHeight.TabIndex = 16;
			this.m_MagHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.m_MagHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_MagHeight.ValueChanged += new System.EventHandler(this.m_MagHeight_ValueChanged);
			// 
			// tpgDetector
			// 
			this.tpgDetector.Controls.Add(this.groupBox3);
			this.tpgDetector.Controls.Add(this.groupBox2);
			this.tpgDetector.Location = new System.Drawing.Point(4, 44);
			this.tpgDetector.Name = "tpgDetector";
			this.tpgDetector.Padding = new System.Windows.Forms.Padding(3);
			this.tpgDetector.Size = new System.Drawing.Size(384, 344);
			this.tpgDetector.TabIndex = 6;
			this.tpgDetector.Text = "Detector";
			this.tpgDetector.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.amplifierImonLab);
			this.groupBox3.Controls.Add(this.detectorAmplifyHswd);
			this.groupBox3.Controls.Add(this.amplifierVmonLab);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Location = new System.Drawing.Point(9, 143);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(367, 125);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Amplifier";
			// 
			// amplifierImonLab
			// 
			this.amplifierImonLab.BackColor = System.Drawing.Color.Black;
			this.amplifierImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.amplifierImonLab.ForeColor = System.Drawing.Color.White;
			this.amplifierImonLab.Location = new System.Drawing.Point(239, 93);
			this.amplifierImonLab.Name = "amplifierImonLab";
			this.amplifierImonLab.Size = new System.Drawing.Size(60, 24);
			this.amplifierImonLab.TabIndex = 61;
			this.amplifierImonLab.Text = "0";
			this.amplifierImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.amplifierImonLab.Click += new System.EventHandler(this.amplifierInfo_Click);
			// 
			// detectorAmplifyHswd
			// 
			this.detectorAmplifyHswd.DisplayName = "";
			this.detectorAmplifyHswd.IsLimitedMode = false;
			this.detectorAmplifyHswd.Location = new System.Drawing.Point(2, 28);
			this.detectorAmplifyHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.detectorAmplifyHswd.Maximum = 100;
			this.detectorAmplifyHswd.Minimum = 0;
			this.detectorAmplifyHswd.Name = "detectorAmplifyHswd";
			this.detectorAmplifyHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.detectorAmplifyHswd.NameSize = new System.Drawing.Size(60, 231);
			this.detectorAmplifyHswd.Size = new System.Drawing.Size(354, 48);
			this.detectorAmplifyHswd.TabIndex = 50;
			this.detectorAmplifyHswd.Value = 0;
			this.detectorAmplifyHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.detectorAmplifyHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// amplifierVmonLab
			// 
			this.amplifierVmonLab.BackColor = System.Drawing.Color.Black;
			this.amplifierVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.amplifierVmonLab.ForeColor = System.Drawing.Color.White;
			this.amplifierVmonLab.Location = new System.Drawing.Point(127, 93);
			this.amplifierVmonLab.Name = "amplifierVmonLab";
			this.amplifierVmonLab.Size = new System.Drawing.Size(60, 24);
			this.amplifierVmonLab.TabIndex = 60;
			this.amplifierVmonLab.Text = "0";
			this.amplifierVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.amplifierVmonLab.Click += new System.EventHandler(this.amplifierInfo_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(191, 93);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 24);
			this.label10.TabIndex = 62;
			this.label10.Text = "Current";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(71, 93);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 24);
			this.label11.TabIndex = 63;
			this.label11.Text = "Voltage";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.collectorImonLab);
			this.groupBox2.Controls.Add(this.detectorCollectorHswd);
			this.groupBox2.Controls.Add(this.collectorVmonLab);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(368, 129);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Collector";
			// 
			// collectorImonLab
			// 
			this.collectorImonLab.BackColor = System.Drawing.Color.Black;
			this.collectorImonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.collectorImonLab.ForeColor = System.Drawing.Color.White;
			this.collectorImonLab.Location = new System.Drawing.Point(240, 93);
			this.collectorImonLab.Name = "collectorImonLab";
			this.collectorImonLab.Size = new System.Drawing.Size(60, 24);
			this.collectorImonLab.TabIndex = 57;
			this.collectorImonLab.Text = "0";
			this.collectorImonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.collectorImonLab.Click += new System.EventHandler(this.collecotrInfo_Click);
			// 
			// detectorCollectorHswd
			// 
			this.detectorCollectorHswd.DisplayName = "";
			this.detectorCollectorHswd.IsLimitedMode = false;
			this.detectorCollectorHswd.Location = new System.Drawing.Point(2, 28);
			this.detectorCollectorHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.detectorCollectorHswd.Maximum = 100;
			this.detectorCollectorHswd.Minimum = 0;
			this.detectorCollectorHswd.Name = "detectorCollectorHswd";
			this.detectorCollectorHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.detectorCollectorHswd.NameSize = new System.Drawing.Size(60, 231);
			this.detectorCollectorHswd.Size = new System.Drawing.Size(354, 48);
			this.detectorCollectorHswd.TabIndex = 50;
			this.detectorCollectorHswd.Value = 0;
			this.detectorCollectorHswd.ValueDisplaySize = new System.Drawing.Size(60, 21);
			this.detectorCollectorHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// collectorVmonLab
			// 
			this.collectorVmonLab.BackColor = System.Drawing.Color.Black;
			this.collectorVmonLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.collectorVmonLab.ForeColor = System.Drawing.Color.White;
			this.collectorVmonLab.Location = new System.Drawing.Point(128, 93);
			this.collectorVmonLab.Name = "collectorVmonLab";
			this.collectorVmonLab.Size = new System.Drawing.Size(60, 24);
			this.collectorVmonLab.TabIndex = 56;
			this.collectorVmonLab.Text = "0";
			this.collectorVmonLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.collectorVmonLab.Click += new System.EventHandler(this.collecotrInfo_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(192, 93);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 24);
			this.label6.TabIndex = 58;
			this.label6.Text = "Current";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(72, 93);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 24);
			this.label7.TabIndex = 59;
			this.label7.Text = "Voltage";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tpgScanRotation
			// 
			this.tpgScanRotation.Controls.Add(this.rotationGunHswd);
			this.tpgScanRotation.Controls.Add(this.rotationBeamHswd);
			this.tpgScanRotation.Controls.Add(this.rotationScanHswd);
			this.tpgScanRotation.Location = new System.Drawing.Point(4, 44);
			this.tpgScanRotation.Name = "tpgScanRotation";
			this.tpgScanRotation.Size = new System.Drawing.Size(384, 344);
			this.tpgScanRotation.TabIndex = 11;
			this.tpgScanRotation.Text = "Rotation";
			this.tpgScanRotation.UseVisualStyleBackColor = true;
			// 
			// rotationGunHswd
			// 
			this.rotationGunHswd.DisplayName = "Gun Align";
			this.rotationGunHswd.IsLimitedMode = false;
			this.rotationGunHswd.Location = new System.Drawing.Point(10, 199);
			this.rotationGunHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rotationGunHswd.Maximum = 100;
			this.rotationGunHswd.Minimum = 0;
			this.rotationGunHswd.Name = "rotationGunHswd";
			this.rotationGunHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.rotationGunHswd.NameSize = new System.Drawing.Size(100, 20);
			this.rotationGunHswd.Size = new System.Drawing.Size(354, 51);
			this.rotationGunHswd.TabIndex = 52;
			this.rotationGunHswd.Value = 0;
			this.rotationGunHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
			this.rotationGunHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// rotationBeamHswd
			// 
			this.rotationBeamHswd.DisplayName = "Beam Shift";
			this.rotationBeamHswd.IsLimitedMode = false;
			this.rotationBeamHswd.Location = new System.Drawing.Point(10, 112);
			this.rotationBeamHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rotationBeamHswd.Maximum = 100;
			this.rotationBeamHswd.Minimum = 0;
			this.rotationBeamHswd.Name = "rotationBeamHswd";
			this.rotationBeamHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.rotationBeamHswd.NameSize = new System.Drawing.Size(100, 20);
			this.rotationBeamHswd.Size = new System.Drawing.Size(354, 51);
			this.rotationBeamHswd.TabIndex = 51;
			this.rotationBeamHswd.Value = 0;
			this.rotationBeamHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
			this.rotationBeamHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// rotationScanHswd
			// 
			this.rotationScanHswd.DisplayName = "Scan Rotation";
			this.rotationScanHswd.IsLimitedMode = false;
			this.rotationScanHswd.IsValueOperation = false;
			this.rotationScanHswd.Location = new System.Drawing.Point(10, 14);
			this.rotationScanHswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.rotationScanHswd.Maximum = 100;
			this.rotationScanHswd.Minimum = 0;
			this.rotationScanHswd.Name = "rotationScanHswd";
			this.rotationScanHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			this.rotationScanHswd.NameSize = new System.Drawing.Size(100, 20);
			this.rotationScanHswd.Size = new System.Drawing.Size(354, 54);
			this.rotationScanHswd.TabIndex = 50;
			this.rotationScanHswd.Value = 0;
			this.rotationScanHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
			this.rotationScanHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Top;
			// 
			// profilenameLab
			// 
			this.profilenameLab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.profilenameLab.Location = new System.Drawing.Point(139, 9);
			this.profilenameLab.Name = "profilenameLab";
			this.profilenameLab.Size = new System.Drawing.Size(240, 36);
			this.profilenameLab.TabIndex = 13;
			this.profilenameLab.Text = "Microscope Profile";
			this.profilenameLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnRestore
			// 
			this.btnRestore.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnRestore.Location = new System.Drawing.Point(250, 514);
			this.btnRestore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnRestore.Name = "btnRestore";
			this.btnRestore.Size = new System.Drawing.Size(61, 24);
			this.btnRestore.TabIndex = 15;
			this.btnRestore.Text = "&Restore";
			this.btnRestore.UseVisualStyleBackColor = true;
			this.btnRestore.Click += new System.EventHandler(this.SystemButton_Click);
			// 
			// fncWobbleBnt
			// 
			this.fncWobbleBnt.Location = new System.Drawing.Point(77, 482);
			this.fncWobbleBnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fncWobbleBnt.Name = "fncWobbleBnt";
			this.fncWobbleBnt.Size = new System.Drawing.Size(61, 24);
			this.fncWobbleBnt.TabIndex = 16;
			this.fncWobbleBnt.Text = "&Wobble";
			this.fncWobbleBnt.UseVisualStyleBackColor = true;
			this.fncWobbleBnt.Click += new System.EventHandler(this.Function_Click);
			// 
			// fncHVlogBnt
			// 
			this.fncHVlogBnt.Location = new System.Drawing.Point(77, 514);
			this.fncHVlogBnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fncHVlogBnt.Name = "fncHVlogBnt";
			this.fncHVlogBnt.Size = new System.Drawing.Size(61, 24);
			this.fncHVlogBnt.TabIndex = 17;
			this.fncHVlogBnt.Text = "&HVlog";
			this.fncHVlogBnt.UseVisualStyleBackColor = true;
			this.fncHVlogBnt.Click += new System.EventHandler(this.Function_Click);
			// 
			// fncMonitorBnt
			// 
			this.fncMonitorBnt.Location = new System.Drawing.Point(144, 482);
			this.fncMonitorBnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fncMonitorBnt.Name = "fncMonitorBnt";
			this.fncMonitorBnt.Size = new System.Drawing.Size(61, 24);
			this.fncMonitorBnt.TabIndex = 18;
			this.fncMonitorBnt.Text = "&Monitor";
			this.fncMonitorBnt.UseVisualStyleBackColor = true;
			this.fncMonitorBnt.Click += new System.EventHandler(this.Function_Click);
			// 
			// FormConfigMicroscope
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(391, 557);
			this.Controls.Add(this.fncMonitorBnt);
			this.Controls.Add(this.vacuumlowCheck);
			this.Controls.Add(this.detectorbsedCheck);
			this.Controls.Add(this.fncHVlogBnt);
			this.Controls.Add(this.fncWobbleBnt);
			this.Controls.Add(this.btnRestore);
			this.Controls.Add(this.profilenameLab);
			this.Controls.Add(this.m_TabMicroscope);
			this.Controls.Add(this.profileRemove);
			this.Controls.Add(this.profileAppend);
			this.Controls.Add(this.label40);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnApply);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormConfigMicroscope";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Microscope Setup";
			this.m_TabMicroscope.ResumeLayout(false);
			this.tpgEghv.ResumeLayout(false);
			this.tpgEghv.PerformLayout();
			this.tpgAlias.ResumeLayout(false);
			this.tpgAlias.PerformLayout();
			this.tpgCondensorLens.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss2CL2Nud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ss2CL1Nud)).EndInit();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss3CL2Nud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ss3CL1Nud)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ss1CL2Nud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ss1CL1Nud)).EndInit();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cl1MaxNudicvd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cl1MinNudicvd)).EndInit();
			this.tpgFocusLens.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.olCoarseMaxNudicvd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.olCoarseMinNudicvd)).EndInit();
			this.groupBox11.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.olFineMaxNudicvd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.olFineMinNudicvd)).EndInit();
			this.tpgMagnificationOld.ResumeLayout(false);
			this.tpgMagnificationOld.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_MagFeedback)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagLevel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_MagHeight)).EndInit();
			this.tpgDetector.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tpgScanRotation.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Button profileAppend;
		private System.Windows.Forms.Button profileRemove;
		private System.Windows.Forms.CheckBox detectorbsedCheck;
		private System.Windows.Forms.CheckBox vacuumlowCheck;
		private System.Windows.Forms.TabControl m_TabMicroscope;
		private System.Windows.Forms.TabPage tpgEghv;
		private SEC.Nanoeye.Support.Controls.HswdCvd avGridHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd avFilamentHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd avAnodeHswd;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox EghvTextTB;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label gridImonLab;
		private System.Windows.Forms.Label gridVmonLab;
		private System.Windows.Forms.Label tipImonLab;
		private System.Windows.Forms.Label tipVmonLab;
		private System.Windows.Forms.Label anodeImonLab;
		private System.Windows.Forms.Label anodeVmonLab;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TabPage tpgAlias;
		private System.Windows.Forms.Label AliasText;
		private System.Windows.Forms.TextBox m_Alias;
		private System.Windows.Forms.TabPage tpgCondensorLens;
		private System.Windows.Forms.GroupBox groupBox7;
		private SEC.Nanoeye.Support.Controls.HswdCvd lensCl2Hswd;
		private System.Windows.Forms.GroupBox groupBox6;
		private SEC.Nanoeye.Support.Controls.HswdCvd lensCl1Hswd;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.TabPage tpgFocusLens;
		private System.Windows.Forms.GroupBox groupBox8;
		private SEC.Nanoeye.Support.Controls.HswdCvd focusCoarseHswd;
		private System.Windows.Forms.GroupBox groupBox11;
		private SEC.Nanoeye.Support.Controls.HswdCvd focusFineHswd;
		private System.Windows.Forms.TabPage tpgMagnificationOld;
		private System.Windows.Forms.CheckBox cbWDsame;
		private System.Windows.Forms.CheckBox scanExtern;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown m_MagFeedback;
		private System.Windows.Forms.Label staticLabel3;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Button m_MagLower;
		private System.Windows.Forms.NumericUpDown m_MagLevel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button m_MagUpper;
		private System.Windows.Forms.NumericUpDown m_MagWidth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button m_MagRemove;
		private System.Windows.Forms.Button m_MagAdd;
		private System.Windows.Forms.ListBox m_MagSelector;
		private System.Windows.Forms.NumericUpDown m_MagHeight;
		private System.Windows.Forms.TabPage tpgDetector;
		private System.Windows.Forms.GroupBox groupBox3;
		private SEC.Nanoeye.Support.Controls.HswdCvd detectorAmplifyHswd;
		private System.Windows.Forms.GroupBox groupBox2;
		private SEC.Nanoeye.Support.Controls.HswdCvd detectorCollectorHswd;
		private System.Windows.Forms.TabPage tpgScanRotation;
		private SEC.Nanoeye.Support.Controls.HswdCvd rotationGunHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd rotationBeamHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd rotationScanHswd;
		private System.Windows.Forms.Label profilenameLab;
		private System.Windows.Forms.Button btnRestore;
		private System.Windows.Forms.Button fncWobbleBnt;
		private System.Windows.Forms.Label amplifierImonLab;
		private System.Windows.Forms.Label amplifierVmonLab;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label collectorImonLab;
		private System.Windows.Forms.Label collectorVmonLab;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button fncHVlogBnt;
		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt cl1ReverseCbicvd;
		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt cl2ReverseCbicvd;
		private System.Windows.Forms.NumericUpDown olCoarseMaxNudicvd;
		private System.Windows.Forms.NumericUpDown olCoarseMinNudicvd;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlInt olReverseCbicvd;
		private System.Windows.Forms.NumericUpDown olFineMaxNudicvd;
		private System.Windows.Forms.NumericUpDown olFineMinNudicvd;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown cl1MaxNudicvd;
		private System.Windows.Forms.NumericUpDown cl1MinNudicvd;
		private System.Windows.Forms.Button fncMonitorBnt;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.NumericUpDown ss2CL2Nud;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.NumericUpDown ss2CL1Nud;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.NumericUpDown ss3CL2Nud;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.NumericUpDown ss3CL1Nud;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.NumericUpDown ss1CL2Nud;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.NumericUpDown ss1CL1Nud;
		private System.Windows.Forms.Label label13;

	}
}