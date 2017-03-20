namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class AutoFunctionManager
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.AutoStopBut = new System.Windows.Forms.Button();
			this.AutoCancelBut = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 12);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(260, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// AutoStopBut
			// 
			this.AutoStopBut.Location = new System.Drawing.Point(37, 41);
			this.AutoStopBut.Name = "AutoStopBut";
			this.AutoStopBut.Size = new System.Drawing.Size(75, 26);
			this.AutoStopBut.TabIndex = 1;
			this.AutoStopBut.Tag = "AFM_Stop";
			this.AutoStopBut.Text = "AFM_Stop";
			this.AutoStopBut.UseVisualStyleBackColor = true;
			this.AutoStopBut.Click += new System.EventHandler(this.AutoStopBut_Click);
			// 
			// AutoCancelBut
			// 
			this.AutoCancelBut.Location = new System.Drawing.Point(172, 41);
			this.AutoCancelBut.Name = "AutoCancelBut";
			this.AutoCancelBut.Size = new System.Drawing.Size(75, 26);
			this.AutoCancelBut.TabIndex = 2;
			this.AutoCancelBut.Tag = "AFM_Cancel";
			this.AutoCancelBut.Text = "AFM_Cancel";
			this.AutoCancelBut.UseVisualStyleBackColor = true;
			this.AutoCancelBut.Click += new System.EventHandler(this.AutoCancelBut_Click);
			// 
			// AutoFunctionManager
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(284, 75);
			this.ControlBox = false;
			this.Controls.Add(this.AutoCancelBut);
			this.Controls.Add(this.AutoStopBut);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AutoFunctionManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Tag = "AFM_Title";
			this.Text = "AFM_Title";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button AutoStopBut;
		private System.Windows.Forms.Button AutoCancelBut;
	}
}