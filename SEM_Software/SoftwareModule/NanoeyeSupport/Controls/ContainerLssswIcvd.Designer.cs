namespace SEC.Nanoeye.Support.Controls
{
	partial class ContainerLssswIcvd
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
			if ( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.autoBB = new SEC.GUIelement.ButtonEllipse();
			this.resetBB = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 65);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// autoBB
			// 
			this.autoBB.Activation = false;
			this.autoBB.Location = new System.Drawing.Point(213, 4);
			this.autoBB.Name = "autoBB";
			this.autoBB.Size = new System.Drawing.Size(24, 24);
			this.autoBB.TabIndex = 3;
			this.autoBB.Click += new System.EventHandler(this.autoBB_Click);
			// 
			// resetBB
			// 
			this.resetBB.Activation = false;
			this.resetBB.Location = new System.Drawing.Point(213, 34);
			this.resetBB.Name = "resetBB";
			this.resetBB.Size = new System.Drawing.Size(24, 24);
			this.resetBB.TabIndex = 4;
			this.resetBB.Click += new System.EventHandler(this.resetBB_Click);
			// 
			// ContainerLssswIcvd
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.resetBB);
			this.Controls.Add(this.autoBB);
			this.Controls.Add(this.label1);
			this.Name = "ContainerLssswIcvd";
			this.Size = new System.Drawing.Size(264, 65);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Label label1;
		protected SEC.GUIelement.ButtonEllipse autoBB;
		protected SEC.GUIelement.ButtonEllipse resetBB;

	}
}
