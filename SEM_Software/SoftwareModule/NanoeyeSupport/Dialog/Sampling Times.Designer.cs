namespace SEC.Nanoeye.Support.Dialog
{
    partial class Sampling_Times
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.m_ContrastDisp = new SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ultraLabel1.Location = new System.Drawing.Point(22, 15);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(177, 20);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Sampling Times";
            // 
            // m_ContrastDisp
            // 
            this.m_ContrastDisp.BackColor = System.Drawing.Color.Transparent;
            this.m_ContrastDisp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.m_ContrastDisp.ButtonSize = new System.Drawing.Size(27, 27);
            this.m_ContrastDisp.ButtonSizeAuto = false;
            this.m_ContrastDisp.DividInner = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.m_ContrastDisp.DividOutter = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.m_ContrastDisp.LeftIcon = global::SEC.Nanoeye.Support.Properties.Resources.btn_minus_enable;
            this.m_ContrastDisp.Location = new System.Drawing.Point(34, 60);
            this.m_ContrastDisp.Maximum = 6;
            this.m_ContrastDisp.Minimum = 1;
            this.m_ContrastDisp.Name = "m_ContrastDisp";
            this.m_ContrastDisp.Orientation = false;
            this.m_ContrastDisp.PanelColor = System.Drawing.Color.Transparent;
            this.m_ContrastDisp.Position = new System.Drawing.Rectangle(26, 10, 14, 3);
            this.m_ContrastDisp.reBruIcon = global::SEC.Nanoeye.Support.Properties.Resources.btn_progress_dot;
            this.m_ContrastDisp.RightIcon = global::SEC.Nanoeye.Support.Properties.Resources.btn_plus_enable;
            this.m_ContrastDisp.Size = new System.Drawing.Size(256, 27);
            this.m_ContrastDisp.TabIndex = 174;
            this.m_ContrastDisp.Text = "imageTrackBarWithTable1";
            this.m_ContrastDisp.TrackBarIcon = global::SEC.Nanoeye.Support.Properties.Resources.btn_progress_bg_2;
            this.m_ContrastDisp.Value = 1;
            // 
            // Sampling_Times
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SEC.Nanoeye.Support.Properties.Resources.popup_sample_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(325, 128);
            this.Controls.Add(this.m_ContrastDisp);
            this.Controls.Add(this.ultraLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Sampling_Times";
            this.Text = "Sampling_Times";
            this.TransparencyKey = System.Drawing.Color.White;
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle m_ContrastDisp;
    }
}