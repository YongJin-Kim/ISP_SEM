namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class PasswordChange
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
			this.mtbOriginal = new System.Windows.Forms.MaskedTextBox();
			this.mtbNew = new System.Windows.Forms.MaskedTextBox();
			this.mtbNewRepeat = new System.Windows.Forms.MaskedTextBox();
			this.bApply = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.bCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mtbOriginal
			// 
			this.mtbOriginal.Location = new System.Drawing.Point(104, 16);
			this.mtbOriginal.Name = "mtbOriginal";
			this.mtbOriginal.PasswordChar = '*';
			this.mtbOriginal.Size = new System.Drawing.Size(136, 21);
			this.mtbOriginal.TabIndex = 0;
			// 
			// mtbNew
			// 
			this.mtbNew.Location = new System.Drawing.Point(104, 48);
			this.mtbNew.Name = "mtbNew";
			this.mtbNew.PasswordChar = '*';
			this.mtbNew.Size = new System.Drawing.Size(136, 21);
			this.mtbNew.TabIndex = 1;
			// 
			// mtbNewRepeat
			// 
			this.mtbNewRepeat.Location = new System.Drawing.Point(104, 80);
			this.mtbNewRepeat.Name = "mtbNewRepeat";
			this.mtbNewRepeat.PasswordChar = '*';
			this.mtbNewRepeat.Size = new System.Drawing.Size(136, 21);
			this.mtbNewRepeat.TabIndex = 2;
			// 
			// bApply
			// 
			this.bApply.Location = new System.Drawing.Point(48, 112);
			this.bApply.Name = "bApply";
			this.bApply.Size = new System.Drawing.Size(75, 23);
			this.bApply.TabIndex = 3;
			this.bApply.Text = "Apply";
			this.bApply.UseVisualStyleBackColor = true;
			this.bApply.Click += new System.EventHandler(this.bApply_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "Original";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(64, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "New";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 12);
			this.label3.TabIndex = 6;
			this.label3.Text = "New Repeat";
			// 
			// bCancel
			// 
			this.bCancel.Location = new System.Drawing.Point(136, 112);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 7;
			this.bCancel.Text = "Cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// PasswordChange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(260, 146);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bApply);
			this.Controls.Add(this.mtbNewRepeat);
			this.Controls.Add(this.mtbNew);
			this.Controls.Add(this.mtbOriginal);
			this.Name = "PasswordChange";
			this.Text = "Password Change";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MaskedTextBox mtbOriginal;
		private System.Windows.Forms.MaskedTextBox mtbNew;
		private System.Windows.Forms.MaskedTextBox mtbNewRepeat;
		private System.Windows.Forms.Button bApply;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bCancel;
	}
}