namespace GenericSupportTester
{
	partial class Form1
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
			this.unitTb = new System.Windows.Forms.TextBox();
			this.areaTb = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.argNud = new System.Windows.Forms.NumericUpDown();
			this.digitNud = new System.Windows.Forms.NumericUpDown();
			this.expNud = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.splineResult = new System.Windows.Forms.ListBox();
			this.testSpline = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.argNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.digitNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.expNud)).BeginInit();
			this.SuspendLayout();
			// 
			// unitTb
			// 
			this.unitTb.Location = new System.Drawing.Point(155, 24);
			this.unitTb.Name = "unitTb";
			this.unitTb.Size = new System.Drawing.Size(200, 21);
			this.unitTb.TabIndex = 0;
			// 
			// areaTb
			// 
			this.areaTb.Location = new System.Drawing.Point(155, 52);
			this.areaTb.Name = "areaTb";
			this.areaTb.Size = new System.Drawing.Size(200, 21);
			this.areaTb.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(60, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "Unit";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(60, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "Area";
			// 
			// argNud
			// 
			this.argNud.DecimalPlaces = 12;
			this.argNud.Location = new System.Drawing.Point(155, 95);
			this.argNud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.argNud.Name = "argNud";
			this.argNud.Size = new System.Drawing.Size(120, 21);
			this.argNud.TabIndex = 4;
			this.argNud.ValueChanged += new System.EventHandler(this.argNud_ValueChanged);
			// 
			// digitNud
			// 
			this.digitNud.Location = new System.Drawing.Point(155, 123);
			this.digitNud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.digitNud.Name = "digitNud";
			this.digitNud.Size = new System.Drawing.Size(120, 21);
			this.digitNud.TabIndex = 5;
			this.digitNud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.digitNud.ValueChanged += new System.EventHandler(this.argNud_ValueChanged);
			// 
			// expNud
			// 
			this.expNud.Location = new System.Drawing.Point(155, 151);
			this.expNud.Name = "expNud";
			this.expNud.Size = new System.Drawing.Size(120, 21);
			this.expNud.TabIndex = 6;
			this.expNud.ValueChanged += new System.EventHandler(this.argNud_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(60, 97);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "Argument";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(60, 125);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "Digit";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(60, 153);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(58, 12);
			this.label5.TabIndex = 9;
			this.label5.Text = "Exponent";
			// 
			// splineResult
			// 
			this.splineResult.FormattingEnabled = true;
			this.splineResult.ItemHeight = 12;
			this.splineResult.Location = new System.Drawing.Point(92, 193);
			this.splineResult.Name = "splineResult";
			this.splineResult.Size = new System.Drawing.Size(342, 88);
			this.splineResult.TabIndex = 10;
			// 
			// testSpline
			// 
			this.testSpline.Location = new System.Drawing.Point(11, 225);
			this.testSpline.Name = "testSpline";
			this.testSpline.Size = new System.Drawing.Size(75, 23);
			this.testSpline.TabIndex = 11;
			this.testSpline.Text = "Spline";
			this.testSpline.UseVisualStyleBackColor = true;
			this.testSpline.Click += new System.EventHandler(this.testSpline_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(446, 303);
			this.Controls.Add(this.testSpline);
			this.Controls.Add(this.splineResult);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.expNud);
			this.Controls.Add(this.digitNud);
			this.Controls.Add(this.argNud);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.areaTb);
			this.Controls.Add(this.unitTb);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.argNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.digitNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.expNud)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox unitTb;
		private System.Windows.Forms.TextBox areaTb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown argNud;
		private System.Windows.Forms.NumericUpDown digitNud;
		private System.Windows.Forms.NumericUpDown expNud;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox splineResult;
		private System.Windows.Forms.Button testSpline;
	}
}

