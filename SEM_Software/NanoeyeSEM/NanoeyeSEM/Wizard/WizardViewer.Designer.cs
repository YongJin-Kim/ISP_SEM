namespace SEC.Nanoeye.NanoeyeSEM.Wizard
{
	partial class WizardViewer
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.imagePb = new System.Windows.Forms.PictureBox();
			this.frontBut = new System.Windows.Forms.Button();
			this.nextBut = new System.Windows.Forms.Button();
			this.stateLab = new System.Windows.Forms.Label();
			this.nameLab = new System.Windows.Forms.Label();
			this.indexLab = new System.Windows.Forms.Label();
			this.messageRtb = new System.Windows.Forms.RichTextBox();
			this.languageCb = new System.Windows.Forms.ComboBox();
			this.waitLab = new System.Windows.Forms.Label();
			this.conditionTimer = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imagePb)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.Controls.Add(this.imagePb, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.frontBut, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.nextBut, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.stateLab, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.nameLab, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.indexLab, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.messageRtb, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.languageCb, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 260F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(329, 730);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// imagePb
			// 
			this.imagePb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.imagePb.BackColor = System.Drawing.Color.Black;
			this.imagePb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.tableLayoutPanel1.SetColumnSpan(this.imagePb, 3);
			this.imagePb.Location = new System.Drawing.Point(3, 43);
			this.imagePb.Name = "imagePb";
			this.imagePb.Size = new System.Drawing.Size(323, 254);
			this.imagePb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imagePb.TabIndex = 146;
			this.imagePb.TabStop = false;
			// 
			// frontBut
			// 
			this.frontBut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.frontBut.Location = new System.Drawing.Point(3, 303);
			this.frontBut.Name = "frontBut";
			this.frontBut.Size = new System.Drawing.Size(54, 34);
			this.frontBut.TabIndex = 147;
			this.frontBut.Text = "Front";
			this.frontBut.UseVisualStyleBackColor = true;
			this.frontBut.Click += new System.EventHandler(this.frontBut_Click);
			// 
			// nextBut
			// 
			this.nextBut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nextBut.Location = new System.Drawing.Point(272, 303);
			this.nextBut.Name = "nextBut";
			this.nextBut.Size = new System.Drawing.Size(54, 34);
			this.nextBut.TabIndex = 148;
			this.nextBut.Text = "Next";
			this.nextBut.UseVisualStyleBackColor = true;
			this.nextBut.Click += new System.EventHandler(this.nextBut_Click);
			// 
			// stateLab
			// 
			this.stateLab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.stateLab.BackColor = System.Drawing.Color.Black;
			this.stateLab.Font = new System.Drawing.Font("Arial Narrow", 16F, System.Drawing.FontStyle.Bold);
			this.stateLab.ForeColor = System.Drawing.Color.SpringGreen;
			this.stateLab.Location = new System.Drawing.Point(63, 300);
			this.stateLab.Name = "stateLab";
			this.stateLab.Size = new System.Drawing.Size(203, 40);
			this.stateLab.TabIndex = 149;
			this.stateLab.Text = "OK";
			this.stateLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// nameLab
			// 
			this.nameLab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.nameLab, 2);
			this.nameLab.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLab.Location = new System.Drawing.Point(3, 0);
			this.nameLab.Name = "nameLab";
			this.nameLab.Size = new System.Drawing.Size(263, 40);
			this.nameLab.TabIndex = 150;
			this.nameLab.Text = "Name";
			this.nameLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// indexLab
			// 
			this.indexLab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.indexLab, 3);
			this.indexLab.Font = new System.Drawing.Font("굴림", 10F);
			this.indexLab.Location = new System.Drawing.Point(3, 340);
			this.indexLab.Name = "indexLab";
			this.indexLab.Size = new System.Drawing.Size(323, 40);
			this.indexLab.TabIndex = 151;
			this.indexLab.Text = "Index";
			this.indexLab.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// messageRtb
			// 
			this.messageRtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.messageRtb.BackColor = System.Drawing.Color.White;
			this.messageRtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tableLayoutPanel1.SetColumnSpan(this.messageRtb, 3);
			this.messageRtb.Font = new System.Drawing.Font("Georgia", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.messageRtb.Location = new System.Drawing.Point(3, 383);
			this.messageRtb.Name = "messageRtb";
			this.messageRtb.ReadOnly = true;
			this.messageRtb.ShowSelectionMargin = true;
			this.messageRtb.Size = new System.Drawing.Size(323, 344);
			this.messageRtb.TabIndex = 152;
			this.messageRtb.Text = "";
			// 
			// languageCb
			// 
			this.languageCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.languageCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.languageCb.FormattingEnabled = true;
			this.languageCb.Location = new System.Drawing.Point(272, 10);
			this.languageCb.Name = "languageCb";
			this.languageCb.Size = new System.Drawing.Size(54, 20);
			this.languageCb.TabIndex = 153;
			this.languageCb.SelectedIndexChanged += new System.EventHandler(this.languageCb_SelectedIndexChanged);
			// 
			// waitLab
			// 
			this.waitLab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.waitLab.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.waitLab.Location = new System.Drawing.Point(0, 0);
			this.waitLab.Name = "waitLab";
			this.waitLab.Size = new System.Drawing.Size(329, 730);
			this.waitLab.TabIndex = 1;
			this.waitLab.Text = "Wait for read wizard data...";
			this.waitLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// conditionTimer
			// 
			this.conditionTimer.Tick += new System.EventHandler(this.conditionTimer_Tick);
			// 
			// WizardViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(329, 730);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.waitLab);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "WizardViewer";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Wizard - Filament Exchange";
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imagePb)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox imagePb;
		private System.Windows.Forms.Button frontBut;
		private System.Windows.Forms.Button nextBut;
		private System.Windows.Forms.Label stateLab;
		private System.Windows.Forms.Label nameLab;
		private System.Windows.Forms.Label indexLab;
		private System.Windows.Forms.RichTextBox messageRtb;
		private System.Windows.Forms.ComboBox languageCb;
		private System.Windows.Forms.Label waitLab;
		private System.Windows.Forms.Timer conditionTimer;

	}
}