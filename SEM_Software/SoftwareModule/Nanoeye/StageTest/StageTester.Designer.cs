namespace StageTest
{
	partial class StageTester
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.axesListCb = new System.Windows.Forms.ComboBox();
			this.axisInfoPg = new System.Windows.Forms.PropertyGrid();
			this.leftBut = new System.Windows.Forms.Button();
			this.rightBut = new System.Windows.Forms.Button();
			this.homeOneBut = new System.Windows.Forms.Button();
			this.homeAllBut = new System.Windows.Forms.Button();
			this.homeOneIndexNud = new System.Windows.Forms.NumericUpDown();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.homeOneIndexNud)).BeginInit();
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
			this.splitContainer1.Panel1.Controls.Add(this.homeOneIndexNud);
			this.splitContainer1.Panel1.Controls.Add(this.homeAllBut);
			this.splitContainer1.Panel1.Controls.Add(this.homeOneBut);
			this.splitContainer1.Panel1.Controls.Add(this.rightBut);
			this.splitContainer1.Panel1.Controls.Add(this.leftBut);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.axisInfoPg);
			this.splitContainer1.Panel2.Controls.Add(this.axesListCb);
			this.splitContainer1.Size = new System.Drawing.Size(465, 262);
			this.splitContainer1.SplitterDistance = 262;
			this.splitContainer1.TabIndex = 0;
			// 
			// axesListCb
			// 
			this.axesListCb.Dock = System.Windows.Forms.DockStyle.Top;
			this.axesListCb.FormattingEnabled = true;
			this.axesListCb.Location = new System.Drawing.Point(0, 0);
			this.axesListCb.Name = "axesListCb";
			this.axesListCb.Size = new System.Drawing.Size(199, 20);
			this.axesListCb.TabIndex = 0;
			this.axesListCb.SelectedValueChanged += new System.EventHandler(this.axesListCb_SelectedValueChanged);
			// 
			// axisInfoPg
			// 
			this.axisInfoPg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axisInfoPg.Location = new System.Drawing.Point(0, 20);
			this.axisInfoPg.Name = "axisInfoPg";
			this.axisInfoPg.Size = new System.Drawing.Size(199, 242);
			this.axisInfoPg.TabIndex = 1;
			// 
			// leftBut
			// 
			this.leftBut.Location = new System.Drawing.Point(13, 13);
			this.leftBut.Name = "leftBut";
			this.leftBut.Size = new System.Drawing.Size(75, 23);
			this.leftBut.TabIndex = 0;
			this.leftBut.Text = "Left";
			this.leftBut.UseVisualStyleBackColor = true;
			this.leftBut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.leftBut_MouseDown);
			this.leftBut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.leftBut_MouseUp);
			// 
			// rightBut
			// 
			this.rightBut.Location = new System.Drawing.Point(94, 13);
			this.rightBut.Name = "rightBut";
			this.rightBut.Size = new System.Drawing.Size(75, 23);
			this.rightBut.TabIndex = 1;
			this.rightBut.Text = "Right";
			this.rightBut.UseVisualStyleBackColor = true;
			this.rightBut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rightBut_MouseDown);
			this.rightBut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rightBut_MouseUp);
			// 
			// homeOneBut
			// 
			this.homeOneBut.Location = new System.Drawing.Point(94, 42);
			this.homeOneBut.Name = "homeOneBut";
			this.homeOneBut.Size = new System.Drawing.Size(75, 23);
			this.homeOneBut.TabIndex = 2;
			this.homeOneBut.Text = "One Home";
			this.homeOneBut.UseVisualStyleBackColor = true;
			this.homeOneBut.Click += new System.EventHandler(this.homeOneBut_Click);
			// 
			// homeAllBut
			// 
			this.homeAllBut.Location = new System.Drawing.Point(175, 42);
			this.homeAllBut.Name = "homeAllBut";
			this.homeAllBut.Size = new System.Drawing.Size(75, 23);
			this.homeAllBut.TabIndex = 3;
			this.homeAllBut.Text = "All Home";
			this.homeAllBut.UseVisualStyleBackColor = true;
			this.homeAllBut.Click += new System.EventHandler(this.homeAllBut_Click);
			// 
			// homeOneIndexNud
			// 
			this.homeOneIndexNud.Location = new System.Drawing.Point(13, 42);
			this.homeOneIndexNud.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.homeOneIndexNud.Name = "homeOneIndexNud";
			this.homeOneIndexNud.Size = new System.Drawing.Size(75, 21);
			this.homeOneIndexNud.TabIndex = 4;
			// 
			// StageTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(465, 262);
			this.Controls.Add(this.splitContainer1);
			this.Name = "StageTester";
			this.Text = "StageTester";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.homeOneIndexNud)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button homeAllBut;
		private System.Windows.Forms.Button homeOneBut;
		private System.Windows.Forms.Button rightBut;
		private System.Windows.Forms.Button leftBut;
		private System.Windows.Forms.PropertyGrid axisInfoPg;
		private System.Windows.Forms.ComboBox axesListCb;
		private System.Windows.Forms.NumericUpDown homeOneIndexNud;
	}
}