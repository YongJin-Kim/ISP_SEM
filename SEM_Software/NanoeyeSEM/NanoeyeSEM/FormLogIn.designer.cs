namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class FormLogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogIn));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.DialogOkButton = new System.Windows.Forms.Button();
            this.bChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // DialogOkButton
            // 
            resources.ApplyResources(this.DialogOkButton, "DialogOkButton");
            this.DialogOkButton.Name = "DialogOkButton";
            this.DialogOkButton.UseVisualStyleBackColor = true;
            this.DialogOkButton.Click += new System.EventHandler(this.DialogOkButton_Click);
            // 
            // bChange
            // 
            resources.ApplyResources(this.bChange, "bChange");
            this.bChange.Name = "bChange";
            this.bChange.UseVisualStyleBackColor = true;
            this.bChange.Click += new System.EventHandler(this.bChange_Click);
            // 
            // FormLogIn
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bChange);
            this.Controls.Add(this.DialogOkButton);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogIn";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button DialogOkButton;
		private System.Windows.Forms.Button bChange;
	}
}