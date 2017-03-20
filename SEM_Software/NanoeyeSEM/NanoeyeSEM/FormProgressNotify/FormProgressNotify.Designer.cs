using System.Diagnostics;

namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class FormProgressNotify
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
			if ( Owner != null ) {
				Owner.Activate();
			}

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgressNotify));
            this.m_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.m_Message = new SEC.Nanoeye.Controls.StaticLabel();
            this.m_ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // m_ProgressBar
            // 
            this.m_ProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(83)))));
            this.m_ProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(192)))), ((int)(((byte)(57)))));
            resources.ApplyResources(this.m_ProgressBar, "m_ProgressBar");
            this.m_ProgressBar.Name = "m_ProgressBar";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(35)))), ((int)(((byte)(48)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(77)))), ((int)(((byte)(93)))));
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(202)))));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // m_Message
            // 
            resources.ApplyResources(this.m_Message, "m_Message");
            this.m_Message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(202)))));
            this.m_Message.Name = "m_Message";
            // 
            // FormProgressNotify
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.m_Message);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.m_ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgressNotify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProgress_FormClosing);
            this.Load += new System.EventHandler(this.FormProgress_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar m_ProgressBar;
		private System.Windows.Forms.Button btnCancel;
		private SEC.Nanoeye.Controls.StaticLabel m_Message;
        private System.Windows.Forms.Timer m_ProgressTimer;
	}
}