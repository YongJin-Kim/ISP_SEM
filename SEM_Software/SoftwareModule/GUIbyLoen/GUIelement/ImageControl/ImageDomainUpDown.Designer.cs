namespace SEC.Nanoeye.Controls
{
	partial class FocusTypeSelector
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
			this.TextLab = new System.Windows.Forms.Label();
			this.LeftBE = new SEC.GUIelement.ButtonEllipse();
			this.RightBE = new SEC.GUIelement.ButtonEllipse();
			this.SuspendLayout();
			// 
			// TextLab
			// 
			this.TextLab.BackColor = System.Drawing.Color.White;
			this.TextLab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.TextLab.Location = new System.Drawing.Point(0, 0);
			this.TextLab.Name = "TextLab";
			this.TextLab.Size = new System.Drawing.Size(100, 23);
			this.TextLab.TabIndex = 0;
			this.TextLab.Text = "label1";
			this.TextLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LeftBE
			// 
			this.LeftBE.BackColor = System.Drawing.Color.Orange;
			this.LeftBE.Location = new System.Drawing.Point(0, 0);
			this.LeftBE.Name = "LeftBE";
			this.LeftBE.Size = new System.Drawing.Size(0, 0);
			this.LeftBE.TabIndex = 0;
			this.LeftBE.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LeftBE_MouseDoubleClick);
			this.LeftBE.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LeftBE_MouseClick);
			// 
			// RightBE
			// 
			this.RightBE.BackColor = System.Drawing.Color.Orange;
			this.RightBE.Location = new System.Drawing.Point(0, 0);
			this.RightBE.Name = "RightBE";
			this.RightBE.Size = new System.Drawing.Size(0, 0);
			this.RightBE.TabIndex = 0;
			this.RightBE.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RightBE_MouseDoubleClick);
			this.RightBE.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RightBE_MouseClick);
			this.ResumeLayout(false);

		}

		#endregion

		private SEC.GUIelement.ButtonEllipse LeftBE;
		private SEC.GUIelement.ButtonEllipse RightBE;
		private System.Windows.Forms.Label TextLab;
	}
}
