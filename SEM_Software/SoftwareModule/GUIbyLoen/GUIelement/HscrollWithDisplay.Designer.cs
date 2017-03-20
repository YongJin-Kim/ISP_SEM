namespace SEC.GUIelement
{
	partial class HscrollWithDisplay
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

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.hScrollBar = new System.Windows.Forms.HScrollBar();
			this.ValueDisplay = new System.Windows.Forms.TextBox();
			this.ControlName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// hScrollBar
			// 
			this.hScrollBar.LargeChange = 1;
			this.hScrollBar.Location = new System.Drawing.Point(127, 10);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new System.Drawing.Size(80, 17);
			this.hScrollBar.TabIndex = 5;
			this.hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
			this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
			// 
			// ValueDisplay
			// 
			this.ValueDisplay.BackColor = System.Drawing.Color.White;
			this.ValueDisplay.ForeColor = System.Drawing.Color.Black;
			this.ValueDisplay.Location = new System.Drawing.Point(357, 10);
			this.ValueDisplay.Name = "ValueDisplay";
			this.ValueDisplay.Size = new System.Drawing.Size(100, 21);
			this.ValueDisplay.TabIndex = 4;
			this.ValueDisplay.Text = "0";
			this.ValueDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ValueDisplay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ValueDisplay_KeyDown);
			this.ValueDisplay.Leave += new System.EventHandler(this.ValueDisplay_Leave);
			this.ValueDisplay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueDisplay_KeyPress);
			// 
			// ControlName
			// 
			this.ControlName.Location = new System.Drawing.Point(14, 10);
			this.ControlName.Name = "ControlName";
			this.ControlName.Size = new System.Drawing.Size(100, 24);
			this.ControlName.TabIndex = 3;
			this.ControlName.Text = "label1";
			this.ControlName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// HscrollWithDisplay2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.hScrollBar);
			this.Controls.Add(this.ValueDisplay);
			this.Controls.Add(this.ControlName);
			this.Name = "HscrollWithDisplay2";
			this.Size = new System.Drawing.Size(470, 44);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.HScrollBar hScrollBar;
		protected System.Windows.Forms.TextBox ValueDisplay;
		protected System.Windows.Forms.Label ControlName;

	}
}
