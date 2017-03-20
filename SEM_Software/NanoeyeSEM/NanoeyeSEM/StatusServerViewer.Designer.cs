namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class StatusServerViewer
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
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbIP1 = new System.Windows.Forms.TextBox();
            this.tbIP2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIP3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbIP4 = new System.Windows.Forms.TextBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ServerOnOff = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.LoopBtn = new System.Windows.Forms.RadioButton();
            this.NetWorkBtn = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tbIP1
            // 
            this.tbIP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.tbIP1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIP1.ForeColor = System.Drawing.Color.Black;
            this.tbIP1.Location = new System.Drawing.Point(20, 113);
            this.tbIP1.Name = "tbIP1";
            this.tbIP1.ReadOnly = true;
            this.tbIP1.Size = new System.Drawing.Size(51, 21);
            this.tbIP1.TabIndex = 2;
            // 
            // tbIP2
            // 
            this.tbIP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.tbIP2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIP2.ForeColor = System.Drawing.Color.Black;
            this.tbIP2.Location = new System.Drawing.Point(86, 114);
            this.tbIP2.Name = "tbIP2";
            this.tbIP2.ReadOnly = true;
            this.tbIP2.Size = new System.Drawing.Size(51, 21);
            this.tbIP2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(74, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(140, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = ".";
            // 
            // tbIP3
            // 
            this.tbIP3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.tbIP3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIP3.ForeColor = System.Drawing.Color.Black;
            this.tbIP3.Location = new System.Drawing.Point(150, 114);
            this.tbIP3.Name = "tbIP3";
            this.tbIP3.ReadOnly = true;
            this.tbIP3.Size = new System.Drawing.Size(51, 21);
            this.tbIP3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(203, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = ".";
            // 
            // tbIP4
            // 
            this.tbIP4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.tbIP4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIP4.ForeColor = System.Drawing.Color.Black;
            this.tbIP4.Location = new System.Drawing.Point(216, 114);
            this.tbIP4.Name = "tbIP4";
            this.tbIP4.ReadOnly = true;
            this.tbIP4.Size = new System.Drawing.Size(51, 21);
            this.tbIP4.TabIndex = 7;
            // 
            // nudPort
            // 
            this.nudPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.nudPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default, "StatusServerPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudPort.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudPort.Location = new System.Drawing.Point(273, 114);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            49152,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(88, 21);
            this.nudPort.TabIndex = 1;
            this.nudPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPort.Value = global::SEC.Nanoeye.NanoeyeSEM.Properties.Settings.Default.StatusServerPort;
            this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.label4.Location = new System.Drawing.Point(18, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Activate";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerOnOff
            // 
            this.ServerOnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.ServerOnOff.AutoCheck = false;
            this.ServerOnOff.BackColor = System.Drawing.Color.Transparent;
            this.ServerOnOff.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.status_off;
            this.ServerOnOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ServerOnOff.FlatAppearance.BorderSize = 0;
            this.ServerOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ServerOnOff.ForeColor = System.Drawing.Color.Transparent;
            this.ServerOnOff.Location = new System.Drawing.Point(334, 72);
            this.ServerOnOff.Name = "ServerOnOff";
            this.ServerOnOff.Size = new System.Drawing.Size(27, 27);
            this.ServerOnOff.TabIndex = 0;
            this.ServerOnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ServerOnOff.UseVisualStyleBackColor = false;
            this.ServerOnOff.Click += new System.EventHandler(this.ServerOnOff_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label5.Location = new System.Drawing.Point(15, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "Status Server";
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.AutoCheck = false;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_btn_nor_enable;
            this.checkBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBox1.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.checkBox1.FlatAppearance.BorderSize = 0;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.checkBox1.Location = new System.Drawing.Point(142, 172);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 30);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "OK";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.Click += new System.EventHandler(this.ServerOnOff_Click);
            // 
            // LoopBtn
            // 
            this.LoopBtn.AutoSize = true;
            this.LoopBtn.BackColor = System.Drawing.Color.Transparent;
            this.LoopBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.LoopBtn.Location = new System.Drawing.Point(164, 77);
            this.LoopBtn.Name = "LoopBtn";
            this.LoopBtn.Size = new System.Drawing.Size(78, 16);
            this.LoopBtn.TabIndex = 13;
            this.LoopBtn.Text = "Loopback";
            this.LoopBtn.UseVisualStyleBackColor = false;
            this.LoopBtn.Click += new System.EventHandler(this.NetWorkBtn_Click);
            // 
            // NetWorkBtn
            // 
            this.NetWorkBtn.AutoSize = true;
            this.NetWorkBtn.BackColor = System.Drawing.Color.Transparent;
            this.NetWorkBtn.Checked = true;
            this.NetWorkBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.NetWorkBtn.Location = new System.Drawing.Point(248, 77);
            this.NetWorkBtn.Name = "NetWorkBtn";
            this.NetWorkBtn.Size = new System.Drawing.Size(69, 16);
            this.NetWorkBtn.TabIndex = 13;
            this.NetWorkBtn.TabStop = true;
            this.NetWorkBtn.Text = "NetWork";
            this.NetWorkBtn.UseVisualStyleBackColor = false;
            this.NetWorkBtn.Click += new System.EventHandler(this.NetWorkBtn_Click);
            // 
            // StatusServerViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_status_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(377, 214);
            this.Controls.Add(this.NetWorkBtn);
            this.Controls.Add(this.LoopBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbIP4);
            this.Controls.Add(this.tbIP3);
            this.Controls.Add(this.tbIP2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIP1);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.ServerOnOff);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "StatusServerViewer";
            this.Text = "Status Server";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Shown += new System.EventHandler(this.FromShown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox ServerOnOff;
		private System.Windows.Forms.NumericUpDown nudPort;
		private System.Windows.Forms.TextBox tbIP1;
		private System.Windows.Forms.TextBox tbIP2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbIP3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbIP4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton LoopBtn;
        private System.Windows.Forms.RadioButton NetWorkBtn;
	}
}