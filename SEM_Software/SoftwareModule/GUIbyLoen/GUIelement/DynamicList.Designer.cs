namespace SEC.GUIelement
{
	partial class DynamicList
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
			if ( disposing && (components != null) ) {
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
			this.changeRightBut = new SEC.GUIelement.ButtonEllipse();
			this.changeLeftBut = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Black;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(67, 23);
			this.label1.Margin = new System.Windows.Forms.Padding(1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(210, 22);
			this.label1.TabIndex = 0;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
			// 
			// changeRightBut
			// 
			this.changeRightBut.Activation = false;
			this.changeRightBut.Location = new System.Drawing.Point(292, 23);
			this.changeRightBut.Margin = new System.Windows.Forms.Padding(1);
			this.changeRightBut.Name = "changeRightBut";
			this.changeRightBut.Size = new System.Drawing.Size(24, 24);
			this.changeRightBut.TabIndex = 1;
			this.changeRightBut.Click += new System.EventHandler(this.changeRight_Click);
			this.changeRightBut.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.changeRight_MouseDoubleClick);
			// 
			// changeLeftBut
			// 
			this.changeLeftBut.Activation = false;
			this.changeLeftBut.Location = new System.Drawing.Point(22, 23);
			this.changeLeftBut.Margin = new System.Windows.Forms.Padding(1);
			this.changeLeftBut.Name = "changeLeftBut";
			this.changeLeftBut.Size = new System.Drawing.Size(24, 24);
			this.changeLeftBut.TabIndex = 2;
			this.changeLeftBut.Click += new System.EventHandler(this.chenageLeft_Click);
			this.changeLeftBut.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chenageLeft_MouseDoubleClick);
			// 
			// DynamicList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.changeRightBut);
			this.Controls.Add(this.changeLeftBut);
			this.Name = "DynamicList";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Size = new System.Drawing.Size(613, 194);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Label label1;
		protected ButtonEllipse changeRightBut;
		protected ButtonEllipse changeLeftBut;

	}
}
