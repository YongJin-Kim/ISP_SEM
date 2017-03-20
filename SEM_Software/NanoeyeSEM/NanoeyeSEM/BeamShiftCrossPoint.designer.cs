namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class BeamShiftCrossPoint
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
            this.bsnY = new System.Windows.Forms.NumericUpDown();
            this.bsnX = new System.Windows.Forms.NumericUpDown();
            this.CrossPointBox = new System.Windows.Forms.PictureBox();
            this.CenterButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MainFormClose = new SEC.Nanoeye.Controls.BitmapRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.bsnY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsnX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPointBox)).BeginInit();
            this.SuspendLayout();
            // 
            // bsnY
            // 
            this.bsnY.BackColor = System.Drawing.Color.Silver;
            this.bsnY.Location = new System.Drawing.Point(240, 187);
            this.bsnY.Maximum = new decimal(new int[] {
            2047,
            0,
            0,
            0});
            this.bsnY.Minimum = new decimal(new int[] {
            2048,
            0,
            0,
            -2147483648});
            this.bsnY.Name = "bsnY";
            this.bsnY.Size = new System.Drawing.Size(60, 21);
            this.bsnY.TabIndex = 20;
            this.bsnY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bsnY.Leave += new System.EventHandler(this.bsnX_Leave);
            // 
            // bsnX
            // 
            this.bsnX.BackColor = System.Drawing.Color.Silver;
            this.bsnX.Location = new System.Drawing.Point(240, 160);
            this.bsnX.Maximum = new decimal(new int[] {
            2047,
            0,
            0,
            0});
            this.bsnX.Minimum = new decimal(new int[] {
            2048,
            0,
            0,
            -2147483648});
            this.bsnX.Name = "bsnX";
            this.bsnX.Size = new System.Drawing.Size(60, 21);
            this.bsnX.TabIndex = 19;
            this.bsnX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bsnX.Leave += new System.EventHandler(this.bsnX_Leave);
            // 
            // CrossPointBox
            // 
            this.CrossPointBox.BackColor = System.Drawing.Color.White;
            this.CrossPointBox.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_map_box;
            this.CrossPointBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CrossPointBox.Location = new System.Drawing.Point(12, 60);
            this.CrossPointBox.Name = "CrossPointBox";
            this.CrossPointBox.Size = new System.Drawing.Size(203, 203);
            this.CrossPointBox.TabIndex = 16;
            this.CrossPointBox.TabStop = false;
            this.CrossPointBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CrossPointBox_MouseMove);
            this.CrossPointBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CrossPointBox_MouseDown);
            this.CrossPointBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CrossPointBox_Paint);
            this.CrossPointBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CrossPointBox_MouseUp);
            // 
            // CenterButton
            // 
            this.CenterButton.BackColor = System.Drawing.Color.Transparent;
            this.CenterButton.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_map_btn_enable;
            this.CenterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CenterButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CenterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CenterButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.CenterButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.CenterButton.Location = new System.Drawing.Point(240, 214);
            this.CenterButton.Name = "CenterButton";
            this.CenterButton.Size = new System.Drawing.Size(58, 27);
            this.CenterButton.TabIndex = 15;
            this.CenterButton.Tag = "BSW_Center";
            this.CenterButton.Text = "BSW_Center";
            this.CenterButton.UseVisualStyleBackColor = false;
            this.CenterButton.Click += new System.EventHandler(this.CenterButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.label1.Location = new System.Drawing.Point(223, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.label2.Location = new System.Drawing.Point(223, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "Map View";
            // 
            // MainFormClose
            // 
            this.MainFormClose.Appearance = System.Windows.Forms.Appearance.Button;
            this.MainFormClose.BackColor = System.Drawing.Color.Transparent;
            this.MainFormClose.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MainFormClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.MainFormClose.ForeColor = System.Drawing.Color.DarkRed;
            this.MainFormClose.FSurface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_close_s;
            this.MainFormClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MainFormClose.Location = new System.Drawing.Point(282, 19);
            this.MainFormClose.Name = "MainFormClose";
            this.MainFormClose.Size = new System.Drawing.Size(23, 23);
            this.MainFormClose.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_bubble_close_e;
            this.MainFormClose.TabIndex = 178;
            this.MainFormClose.Tag = "";
            this.MainFormClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MainFormClose.UseCompatibleTextRendering = true;
            this.MainFormClose.Click += new System.EventHandler(this.FormClose);
            // 
            // BeamShiftCrossPoint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.popup_map_t_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(318, 275);
            this.Controls.Add(this.MainFormClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bsnY);
            this.Controls.Add(this.bsnX);
            this.Controls.Add(this.CrossPointBox);
            this.Controls.Add(this.CenterButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BeamShiftCrossPoint";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Tag = "BSW_Title";
            this.Text = "BSW_Title";
            this.Shown += new System.EventHandler(this.FormWhown);
            ((System.ComponentModel.ISupportInitialize)(this.bsnY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsnX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPointBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown bsnY;
		private System.Windows.Forms.NumericUpDown bsnX;
		private System.Windows.Forms.PictureBox CrossPointBox;
		private System.Windows.Forms.Button CenterButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private SEC.Nanoeye.Controls.BitmapRadioButton MainFormClose;
	}
}

