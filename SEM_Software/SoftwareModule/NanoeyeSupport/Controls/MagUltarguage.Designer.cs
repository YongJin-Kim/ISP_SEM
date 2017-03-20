namespace SEC.Nanoeye.Support.Controls
{
	partial class MagUltarguage
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

			if (_ControlTable != null)
			{
				_ControlTable.SelectedIndexChanged -= new System.EventHandler(_ControlTable_SelectedIndexChanged);
				_ControlTable = null;
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
			this.rightBe = new SEC.GUIelement.ButtonEllipse();
			this.leftBe = new SEC.GUIelement.ButtonEllipse();
			this.display = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// rightBe
			// 
			this.rightBe.Activation = false;
			this.rightBe.BackColor = System.Drawing.Color.White;
			this.rightBe.Enabled = false;
			this.rightBe.Location = new System.Drawing.Point(165, 0);
			this.rightBe.Name = "rightBe";
			this.rightBe.Size = new System.Drawing.Size(0, 0);
			// 
			// 
			// 
			this.rightBe.Style.Font = new System.Drawing.Font("Times New Roman", 10F);
			this.rightBe.Style.ImageOffset = new System.Drawing.Point(0, 0);
			this.rightBe.TabIndex = 2;
			this.rightBe.DoubleClick += new System.EventHandler(this.rightBe_Click);
			this.rightBe.Click += new System.EventHandler(this.rightBe_Click);
			// 
			// leftBe
			// 
			this.leftBe.Activation = false;
			this.leftBe.Enabled = false;
			this.leftBe.Location = new System.Drawing.Point(0, 0);
			this.leftBe.Name = "leftBe";
			this.leftBe.Size = new System.Drawing.Size(0, 0);
			// 
			// 
			// 
			this.leftBe.Style.Font = new System.Drawing.Font("Times New Roman", 10F);
			this.leftBe.Style.ImageOffset = new System.Drawing.Point(0, 0);
			this.leftBe.TabIndex = 1;
			this.leftBe.DoubleClick += new System.EventHandler(this.leftBe_Click);
			this.leftBe.Click += new System.EventHandler(this.leftBe_Click);
			// 
			// display
			// 
			this.display.BackColor = System.Drawing.Color.Black;
			this.display.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.display.ForeColor = System.Drawing.Color.Yellow;
			this.display.Location = new System.Drawing.Point(67, 17);
			this.display.Name = "display";
			this.display.Size = new System.Drawing.Size(55, 25);
			this.display.TabIndex = 4;
			this.display.Text = "x10.0k";
			this.display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MagUltarguage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.display);
			this.Controls.Add(this.rightBe);
			this.Controls.Add(this.leftBe);
			this.Name = "MagUltarguage";
			this.Size = new System.Drawing.Size(221, 53);
			this.ResumeLayout(false);

		}

		#endregion

		private SEC.GUIelement.ButtonEllipse leftBe;
		private SEC.GUIelement.ButtonEllipse rightBe;
		private System.Windows.Forms.Label display;
	}
}
