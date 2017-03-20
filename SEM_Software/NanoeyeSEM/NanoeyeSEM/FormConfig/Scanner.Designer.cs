namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	partial class Scanner
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
			if ( disposing && (components != null) )
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
            this.fpsDisplayLab = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.spfDisplayLab = new System.Windows.Forms.Label();
            this.okBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.ApplyBut = new System.Windows.Forms.Button();
            this.itemListCombo = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.InfoBut = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpsDisplayLab
            // 
            this.fpsDisplayLab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsDisplayLab.Location = new System.Drawing.Point(45, 4);
            this.fpsDisplayLab.Name = "fpsDisplayLab";
            this.fpsDisplayLab.Size = new System.Drawing.Size(118, 19);
            this.fpsDisplayLab.TabIndex = 65;
            this.fpsDisplayLab.Text = "label3";
            this.fpsDisplayLab.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 12);
            this.label4.TabIndex = 66;
            this.label4.Text = "FPS";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(178, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 68;
            this.label5.Text = "SPF";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // spfDisplayLab
            // 
            this.spfDisplayLab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spfDisplayLab.Location = new System.Drawing.Point(216, 4);
            this.spfDisplayLab.Name = "spfDisplayLab";
            this.spfDisplayLab.Size = new System.Drawing.Size(118, 19);
            this.spfDisplayLab.TabIndex = 67;
            this.spfDisplayLab.Text = "label6";
            this.spfDisplayLab.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point(213, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(53, 23);
            this.okBut.TabIndex = 69;
            this.okBut.Text = "Ok";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.Location = new System.Drawing.Point(272, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(53, 23);
            this.cancelBut.TabIndex = 70;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Visible = false;
            // 
            // ApplyBut
            // 
            this.ApplyBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBut.Location = new System.Drawing.Point(154, 2);
            this.ApplyBut.Name = "ApplyBut";
            this.ApplyBut.Size = new System.Drawing.Size(53, 23);
            this.ApplyBut.TabIndex = 71;
            this.ApplyBut.Text = "Apply";
            this.ApplyBut.UseVisualStyleBackColor = true;
            this.ApplyBut.Click += new System.EventHandler(this.ApplyBut_Click);
            // 
            // itemListCombo
            // 
            this.itemListCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.itemListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemListCombo.FormattingEnabled = true;
            this.itemListCombo.Location = new System.Drawing.Point(0, 0);
            this.itemListCombo.Margin = new System.Windows.Forms.Padding(0);
            this.itemListCombo.Name = "itemListCombo";
            this.itemListCombo.Size = new System.Drawing.Size(346, 20);
            this.itemListCombo.TabIndex = 73;
            this.itemListCombo.SelectedIndexChanged += new System.EventHandler(this.itemListCombo_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.itemListCombo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(346, 401);
            this.tableLayoutPanel1.TabIndex = 87;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fpsDisplayLab);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.spfDisplayLab);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 34);
            this.panel1.TabIndex = 74;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.InfoBut);
            this.panel2.Controls.Add(this.ApplyBut);
            this.panel2.Controls.Add(this.okBut);
            this.panel2.Controls.Add(this.cancelBut);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 371);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(340, 27);
            this.panel2.TabIndex = 75;
            // 
            // InfoBut
            // 
            this.InfoBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoBut.Location = new System.Drawing.Point(9, 3);
            this.InfoBut.Name = "InfoBut";
            this.InfoBut.Size = new System.Drawing.Size(53, 23);
            this.InfoBut.TabIndex = 72;
            this.InfoBut.Text = "Info";
            this.InfoBut.UseVisualStyleBackColor = true;
            this.InfoBut.Click += new System.EventHandler(this.InfoBut_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 24);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(340, 301);
            this.propertyGrid1.TabIndex = 76;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // Scanner
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(346, 401);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Scanner";
            this.Text = "Maintanace - Scanner";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label fpsDisplayLab;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label spfDisplayLab;
		private System.Windows.Forms.Button okBut;
		private System.Windows.Forms.Button cancelBut;
		private System.Windows.Forms.Button ApplyBut;
		private System.Windows.Forms.ComboBox itemListCombo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button InfoBut;
	}
}