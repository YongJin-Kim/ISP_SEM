namespace StageTest
{
	partial class Form1
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
			if (disposing && ( components != null ))
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.axXright = new System.Windows.Forms.Button();
			this.axXleft = new System.Windows.Forms.Button();
			this.axesHome = new System.Windows.Forms.Button();
			this.axXhome = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.axXright);
			this.splitContainer1.Panel1.Controls.Add(this.axXleft);
			this.splitContainer1.Panel1.Controls.Add(this.axesHome);
			this.splitContainer1.Panel1.Controls.Add(this.axXhome);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
			this.splitContainer1.Size = new System.Drawing.Size(484, 266);
			this.splitContainer1.SplitterDistance = 278;
			this.splitContainer1.TabIndex = 0;
			// 
			// axXright
			// 
			this.axXright.Location = new System.Drawing.Point(93, 12);
			this.axXright.Name = "axXright";
			this.axXright.Size = new System.Drawing.Size(75, 23);
			this.axXright.TabIndex = 7;
			this.axXright.Text = "X CW";
			this.axXright.UseVisualStyleBackColor = true;
			this.axXright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.axXright_MouseDown);
			this.axXright.MouseUp += new System.Windows.Forms.MouseEventHandler(this.axXright_MouseUp);
			// 
			// axXleft
			// 
			this.axXleft.Location = new System.Drawing.Point(12, 12);
			this.axXleft.Name = "axXleft";
			this.axXleft.Size = new System.Drawing.Size(75, 23);
			this.axXleft.TabIndex = 6;
			this.axXleft.Text = "X CCW";
			this.axXleft.UseVisualStyleBackColor = true;
			this.axXleft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.axXleft_MouseDown);
			this.axXleft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.axXleft_MouseUp);
			// 
			// axesHome
			// 
			this.axesHome.Location = new System.Drawing.Point(174, 41);
			this.axesHome.Name = "axesHome";
			this.axesHome.Size = new System.Drawing.Size(75, 23);
			this.axesHome.TabIndex = 1;
			this.axesHome.Text = "Home";
			this.axesHome.UseVisualStyleBackColor = true;
			this.axesHome.Click += new System.EventHandler(this.axesHome_Click);
			// 
			// axXhome
			// 
			this.axXhome.Location = new System.Drawing.Point(174, 12);
			this.axXhome.Name = "axXhome";
			this.axXhome.Size = new System.Drawing.Size(75, 23);
			this.axXhome.TabIndex = 0;
			this.axXhome.Text = "X Home";
			this.axXhome.UseVisualStyleBackColor = true;
			this.axXhome.Click += new System.EventHandler(this.axXhome_Click);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 20);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(202, 246);
			this.propertyGrid1.TabIndex = 1;
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(202, 20);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 266);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button axXright;
		private System.Windows.Forms.Button axXleft;
		private System.Windows.Forms.Button axesHome;
		private System.Windows.Forms.Button axXhome;

	}
}

