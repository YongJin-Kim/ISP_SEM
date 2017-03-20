namespace SEC.Nanoeye.NanoeyeSEM.Initialize
{
	partial class Splash
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
            this.labModel = new System.Windows.Forms.Label();
            this.labCompaney = new System.Windows.Forms.Label();
            this.labProduct = new System.Windows.Forms.Label();
            this.labMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labModel
            // 
            this.labModel.BackColor = System.Drawing.Color.Transparent;
            this.labModel.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labModel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.labModel.Location = new System.Drawing.Point(230, 93);
            this.labModel.Name = "labModel";
            this.labModel.Size = new System.Drawing.Size(238, 40);
            this.labModel.TabIndex = 5;
            this.labModel.Text = "---";
            this.labModel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labModel.Visible = false;
            // 
            // labCompaney
            // 
            this.labCompaney.BackColor = System.Drawing.Color.Transparent;
            this.labCompaney.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCompaney.Location = new System.Drawing.Point(26, 160);
            this.labCompaney.Name = "labCompaney";
            this.labCompaney.Size = new System.Drawing.Size(456, 40);
            this.labCompaney.TabIndex = 3;
            this.labCompaney.Text = "---";
            this.labCompaney.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labCompaney.Visible = false;
            // 
            // labProduct
            // 
            this.labProduct.BackColor = System.Drawing.Color.Transparent;
            this.labProduct.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(218)))), ((int)(((byte)(255)))));
            this.labProduct.Location = new System.Drawing.Point(30, -1);
            this.labProduct.Name = "labProduct";
            this.labProduct.Size = new System.Drawing.Size(456, 70);
            this.labProduct.TabIndex = 4;
            this.labProduct.Text = "---";
            this.labProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labProduct.Visible = false;
            // 
            // labMessage
            // 
            this.labMessage.BackColor = System.Drawing.Color.Transparent;
            this.labMessage.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(123)))), ((int)(((byte)(167)))));
            this.labMessage.Location = new System.Drawing.Point(30, 170);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(456, 91);
            this.labMessage.TabIndex = 6;
            this.labMessage.Text = "Starting...";
            this.labMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Splash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.透明logo新;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(510, 268);
            this.Controls.Add(this.labProduct);
            this.Controls.Add(this.labMessage);
            this.Controls.Add(this.labModel);
            this.Controls.Add(this.labCompaney);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labModel;
		private System.Windows.Forms.Label labCompaney;
		private System.Windows.Forms.Label labProduct;
        private System.Windows.Forms.Label labMessage;
	}
}