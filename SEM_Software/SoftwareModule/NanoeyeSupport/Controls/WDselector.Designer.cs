namespace SEC.Nanoeye.Support.Controls
{
	partial class WDselector
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
			this.rightBe = new SEC.GUIelement.ButtonEllipse();
			this.leftBe = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Black;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(51, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// rightBe
			// 
			this.rightBe.Activation = false;
			this.rightBe.BackColor = System.Drawing.Color.White;
			this.rightBe.Location = new System.Drawing.Point(157, 25);
			this.rightBe.Name = "rightBe";
			this.rightBe.Size = new System.Drawing.Size(24, 24);
			this.rightBe.TabIndex = 1;
			this.rightBe.Click += new System.EventHandler(this.rightBe_Click);
			this.rightBe.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rightBe_MouseDoubleClick);
			// 
			// leftBe
			// 
			this.leftBe.Activation = false;
			this.leftBe.BackColor = System.Drawing.Color.White;
			this.leftBe.Location = new System.Drawing.Point(21, 25);
			this.leftBe.Name = "leftBe";
			this.leftBe.Size = new System.Drawing.Size(24, 24);
			this.leftBe.TabIndex = 0;
			this.leftBe.Click += new System.EventHandler(this.leftBe_Click);
			this.leftBe.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.leftBe_MouseDoubleClick);
			// 
			// WDselector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rightBe);
			this.Controls.Add(this.leftBe);
			this.Name = "WDselector";
			this.Size = new System.Drawing.Size(631, 367);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private SEC.GUIelement.ButtonEllipse leftBe;
		private SEC.GUIelement.ButtonEllipse rightBe;
	}
}
