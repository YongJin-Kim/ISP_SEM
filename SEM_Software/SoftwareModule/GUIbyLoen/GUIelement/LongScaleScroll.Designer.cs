namespace SEC.GUIelement
{
	partial class LongScaleScroll
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
			this.OutterBEright = new SEC.GUIelement.ButtonEllipse();
			this.InnerBERight = new SEC.GUIelement.ButtonEllipse();
			this.OutterBEleft = new SEC.GUIelement.ButtonEllipse();
			this.InnerBEleft = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// OutterBEright
			// 
			this.OutterBEright.Activation = false;
			this.OutterBEright.BackColor = System.Drawing.Color.Cyan;
			this.OutterBEright.Location = new System.Drawing.Point(195, 0);
			this.OutterBEright.Margin = new System.Windows.Forms.Padding(1);
			this.OutterBEright.Name = "OutterBEright";
			this.OutterBEright.RepeatPush = true;
			this.OutterBEright.Size = new System.Drawing.Size(18, 40);
			this.OutterBEright.TabIndex = 6;
			this.OutterBEright.Click += new System.EventHandler(this.OutterBEright_Click);
			this.OutterBEright.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OutterBEright_MouseDoubleClick);
			// 
			// InnerBERight
			// 
			this.InnerBERight.Activation = false;
			this.InnerBERight.BackColor = System.Drawing.Color.LightGreen;
			this.InnerBERight.Location = new System.Drawing.Point(142, 0);
			this.InnerBERight.Margin = new System.Windows.Forms.Padding(1);
			this.InnerBERight.Name = "InnerBERight";
			this.InnerBERight.RepeatPush = true;
			this.InnerBERight.Size = new System.Drawing.Size(51, 41);
			this.InnerBERight.TabIndex = 5;
			this.InnerBERight.Click += new System.EventHandler(this.InnerBERight_Click);
			this.InnerBERight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InnerBERight_MouseDoubleClick);
			// 
			// OutterBEleft
			// 
			this.OutterBEleft.Activation = false;
			this.OutterBEleft.BackColor = System.Drawing.Color.Cyan;
			this.OutterBEleft.Location = new System.Drawing.Point(1, 1);
			this.OutterBEleft.Margin = new System.Windows.Forms.Padding(1);
			this.OutterBEleft.Name = "OutterBEleft";
			this.OutterBEleft.RepeatPush = true;
			this.OutterBEleft.Size = new System.Drawing.Size(43, 40);
			this.OutterBEleft.TabIndex = 7;
			this.OutterBEleft.Click += new System.EventHandler(this.OutterBEleft_Click);
			this.OutterBEleft.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OutterBEleft_MouseDoubleClick);
			// 
			// InnerBEleft
			// 
			this.InnerBEleft.Activation = false;
			this.InnerBEleft.BackColor = System.Drawing.Color.LightGreen;
			this.InnerBEleft.Location = new System.Drawing.Point(46, 0);
			this.InnerBEleft.Margin = new System.Windows.Forms.Padding(1);
			this.InnerBEleft.Name = "InnerBEleft";
			this.InnerBEleft.RepeatPush = true;
			this.InnerBEleft.Size = new System.Drawing.Size(18, 40);
			this.InnerBEleft.TabIndex = 8;
			this.InnerBEleft.Click += new System.EventHandler(this.InnerBEleft_Click);
			this.InnerBEleft.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InnerBEleft_MouseDoubleClick);
			// 
			// LongScaleScroll
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.OutterBEright);
			this.Controls.Add(this.InnerBERight);
			this.Controls.Add(this.OutterBEleft);
			this.Controls.Add(this.InnerBEleft);
			this.Name = "LongScaleScroll";
			this.Size = new System.Drawing.Size(605, 369);
			this.ResumeLayout(false);

		}

		#endregion

		private ButtonEllipse InnerBERight;
		private ButtonEllipse OutterBEright;
		private ButtonEllipse OutterBEleft;
		private ButtonEllipse InnerBEleft;
	}
}
