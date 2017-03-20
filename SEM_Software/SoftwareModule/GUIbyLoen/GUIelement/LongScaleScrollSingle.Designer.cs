namespace SEC.GUIelement
{
	partial class LongScaleScrollSingle
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
			this.ValuePanel = new System.Windows.Forms.Panel();
			this.RightBe = new SEC.GUIelement.ButtonEllipse();
			this.LeftBe = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// ValuePanel
			// 
			this.ValuePanel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ValuePanel.Location = new System.Drawing.Point(73, 3);
			this.ValuePanel.Name = "ValuePanel";
			this.ValuePanel.Size = new System.Drawing.Size(148, 25);
			this.ValuePanel.TabIndex = 2;
			this.ValuePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ValuePanel_Paint);
			this.ValuePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ValuePanel_MouseDown);
			this.ValuePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValuePanel_MouseUp);
			// 
			// RightBe
			// 
			this.RightBe.Activation = false;
			this.RightBe.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.RightBe.ImageOffset = new System.Drawing.Point(2, 2);
			this.RightBe.Location = new System.Drawing.Point(34, 3);
			this.RightBe.Margin = new System.Windows.Forms.Padding(0);
			this.RightBe.Name = "RightBe";
			this.RightBe.PaintRation = 80;
			this.RightBe.RepeatPush = true;
			this.RightBe.Size = new System.Drawing.Size(36, 25);
			this.RightBe.TabIndex = 1;
			this.RightBe.Click += new System.EventHandler(this.RightBe_Click);
			// 
			// LeftBe
			// 
			this.LeftBe.Activation = false;
			this.LeftBe.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LeftBe.ImageOffset = new System.Drawing.Point(1, 1);
			this.LeftBe.Location = new System.Drawing.Point(0, 3);
			this.LeftBe.Margin = new System.Windows.Forms.Padding(0);
			this.LeftBe.Name = "LeftBe";
			this.LeftBe.PaintRation = 80;
			this.LeftBe.RepeatPush = true;
			this.LeftBe.Size = new System.Drawing.Size(34, 25);
			this.LeftBe.TabIndex = 0;
			this.LeftBe.Click += new System.EventHandler(this.LeftBe_Click);
			// 
			// LongScaleScrollSingle
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ValuePanel);
			this.Controls.Add(this.RightBe);
			this.Controls.Add(this.LeftBe);
			this.Name = "LongScaleScrollSingle";
			this.Size = new System.Drawing.Size(225, 31);
			this.ResumeLayout(false);

		}

		#endregion

		private ButtonEllipse LeftBe;
		private ButtonEllipse RightBe;
		private System.Windows.Forms.Panel ValuePanel;
	}
}
