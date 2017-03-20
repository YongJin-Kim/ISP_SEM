namespace SettingConverter
{
	partial class SettingConverter
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
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.inputTb = new System.Windows.Forms.TextBox();
			this.inputBut = new System.Windows.Forms.Button();
			this.inputLb = new System.Windows.Forms.Label();
			this.outputBut = new System.Windows.Forms.Button();
			this.outputTb = new System.Windows.Forms.TextBox();
			this.convertBut = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.outputCb = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// inputTb
			// 
			this.inputTb.BackColor = System.Drawing.Color.White;
			this.inputTb.Location = new System.Drawing.Point(63, 12);
			this.inputTb.Name = "inputTb";
			this.inputTb.ReadOnly = true;
			this.inputTb.Size = new System.Drawing.Size(287, 21);
			this.inputTb.TabIndex = 0;
			// 
			// inputBut
			// 
			this.inputBut.Location = new System.Drawing.Point(350, 12);
			this.inputBut.Name = "inputBut";
			this.inputBut.Size = new System.Drawing.Size(32, 21);
			this.inputBut.TabIndex = 1;
			this.inputBut.Text = "...";
			this.inputBut.UseVisualStyleBackColor = true;
			this.inputBut.Click += new System.EventHandler(this.inputBut_Click);
			// 
			// inputLb
			// 
			this.inputLb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.inputLb.Location = new System.Drawing.Point(388, 12);
			this.inputLb.Name = "inputLb";
			this.inputLb.Size = new System.Drawing.Size(100, 21);
			this.inputLb.TabIndex = 2;
			this.inputLb.Text = "label1";
			this.inputLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// outputBut
			// 
			this.outputBut.Enabled = false;
			this.outputBut.Location = new System.Drawing.Point(350, 39);
			this.outputBut.Name = "outputBut";
			this.outputBut.Size = new System.Drawing.Size(32, 21);
			this.outputBut.TabIndex = 4;
			this.outputBut.Text = "...";
			this.outputBut.UseVisualStyleBackColor = true;
			this.outputBut.Click += new System.EventHandler(this.outputBut_Click);
			// 
			// outputTb
			// 
			this.outputTb.BackColor = System.Drawing.Color.White;
			this.outputTb.Location = new System.Drawing.Point(63, 39);
			this.outputTb.Name = "outputTb";
			this.outputTb.ReadOnly = true;
			this.outputTb.Size = new System.Drawing.Size(287, 21);
			this.outputTb.TabIndex = 3;
			// 
			// convertBut
			// 
			this.convertBut.Enabled = false;
			this.convertBut.Location = new System.Drawing.Point(388, 63);
			this.convertBut.Name = "convertBut";
			this.convertBut.Size = new System.Drawing.Size(100, 26);
			this.convertBut.TabIndex = 6;
			this.convertBut.Text = "Convert";
			this.convertBut.UseVisualStyleBackColor = true;
			this.convertBut.Click += new System.EventHandler(this.convertBut_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "Input";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "Output";
			// 
			// outputCb
			// 
			this.outputCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputCb.FormattingEnabled = true;
			this.outputCb.Items.AddRange(new object[] {
            "Mini-SEM",
            "Nanoeye",
            "Nanoeye_Mini-SEM"});
			this.outputCb.Location = new System.Drawing.Point(388, 39);
			this.outputCb.Name = "outputCb";
			this.outputCb.Size = new System.Drawing.Size(99, 20);
			this.outputCb.TabIndex = 9;
			this.outputCb.SelectedIndexChanged += new System.EventHandler(this.outputCb_SelectedIndexChanged);
			// 
			// SettingConverter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(499, 99);
			this.Controls.Add(this.outputCb);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.convertBut);
			this.Controls.Add(this.outputBut);
			this.Controls.Add(this.outputTb);
			this.Controls.Add(this.inputLb);
			this.Controls.Add(this.inputBut);
			this.Controls.Add(this.inputTb);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SettingConverter";
			this.Text = "Nanoeye - SettingConverter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inputTb;
		private System.Windows.Forms.Button inputBut;
		private System.Windows.Forms.Label inputLb;
		private System.Windows.Forms.Button outputBut;
		private System.Windows.Forms.TextBox outputTb;
		private System.Windows.Forms.Button convertBut;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox outputCb;
	}
}

