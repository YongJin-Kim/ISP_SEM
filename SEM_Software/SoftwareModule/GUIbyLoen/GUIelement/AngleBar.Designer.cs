namespace SEC.GUIelement
{
	partial class AngleBar
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
			if (disposing && (components != null)) {
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
			this.AngleLable = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// AngleLable
			// 
			this.AngleLable.Location = new System.Drawing.Point(0, 0);
			this.AngleLable.Name = "AngleLable";
			this.AngleLable.Size = new System.Drawing.Size(30, 21);
			this.AngleLable.TabIndex = 0;
			this.AngleLable.Text = "360";
			this.AngleLable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox AngleLable;

	}
}
