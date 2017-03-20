namespace SEC.Nanoeye.Support.Dialog
{
	partial class LensWobbleForm
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
            this.olAmplHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.olFreqHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.olWobbleCb = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool();
            this.cl2AmplHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.cl2FreqHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.cl2WobbleCb = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool();
            this.cl1AmplHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.cl1FreqHswd = new SEC.Nanoeye.Support.Controls.HswdCvd();
            this.cl1WobbleCb = new SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool();
            this.SuspendLayout();
            // 
            // olAmplHswd
            // 
            this.olAmplHswd.DisplayName = "Ampl.";
            this.olAmplHswd.Location = new System.Drawing.Point(85, 173);
            this.olAmplHswd.Maximum = 100;
            this.olAmplHswd.Minimum = 0;
            this.olAmplHswd.Name = "olAmplHswd";
            this.olAmplHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.olAmplHswd.NameSize = new System.Drawing.Size(50, 24);
            this.olAmplHswd.Size = new System.Drawing.Size(385, 26);
            this.olAmplHswd.TabIndex = 8;
            this.olAmplHswd.Value = 0;
            this.olAmplHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.olAmplHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // olFreqHswd
            // 
            this.olFreqHswd.DisplayName = "Freq.";
            this.olFreqHswd.Location = new System.Drawing.Point(85, 141);
            this.olFreqHswd.Maximum = 100;
            this.olFreqHswd.Minimum = 0;
            this.olFreqHswd.Name = "olFreqHswd";
            this.olFreqHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.olFreqHswd.NameSize = new System.Drawing.Size(50, 24);
            this.olFreqHswd.Size = new System.Drawing.Size(385, 26);
            this.olFreqHswd.TabIndex = 7;
            this.olFreqHswd.Value = 0;
            this.olFreqHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.olFreqHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // olWobbleCb
            // 
            this.olWobbleCb.Appearance = System.Windows.Forms.Appearance.Button;
            this.olWobbleCb.ControlValue = null;
            this.olWobbleCb.Location = new System.Drawing.Point(12, 141);
            this.olWobbleCb.Name = "olWobbleCb";
            this.olWobbleCb.Size = new System.Drawing.Size(66, 58);
            this.olWobbleCb.TabIndex = 6;
            this.olWobbleCb.Text = "OL";
            this.olWobbleCb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.olWobbleCb.UseVisualStyleBackColor = true;
            // 
            // cl2AmplHswd
            // 
            this.cl2AmplHswd.DisplayName = "Ampl.";
            this.cl2AmplHswd.Location = new System.Drawing.Point(85, 109);
            this.cl2AmplHswd.Maximum = 100;
            this.cl2AmplHswd.Minimum = 0;
            this.cl2AmplHswd.Name = "cl2AmplHswd";
            this.cl2AmplHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.cl2AmplHswd.NameSize = new System.Drawing.Size(50, 24);
            this.cl2AmplHswd.Size = new System.Drawing.Size(385, 26);
            this.cl2AmplHswd.TabIndex = 5;
            this.cl2AmplHswd.Value = 0;
            this.cl2AmplHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.cl2AmplHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // cl2FreqHswd
            // 
            this.cl2FreqHswd.DisplayName = "Freq.";
            this.cl2FreqHswd.Location = new System.Drawing.Point(85, 77);
            this.cl2FreqHswd.Maximum = 100;
            this.cl2FreqHswd.Minimum = 0;
            this.cl2FreqHswd.Name = "cl2FreqHswd";
            this.cl2FreqHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.cl2FreqHswd.NameSize = new System.Drawing.Size(50, 24);
            this.cl2FreqHswd.Size = new System.Drawing.Size(385, 26);
            this.cl2FreqHswd.TabIndex = 4;
            this.cl2FreqHswd.Value = 0;
            this.cl2FreqHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.cl2FreqHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // cl2WobbleCb
            // 
            this.cl2WobbleCb.Appearance = System.Windows.Forms.Appearance.Button;
            this.cl2WobbleCb.ControlValue = null;
            this.cl2WobbleCb.Location = new System.Drawing.Point(12, 77);
            this.cl2WobbleCb.Name = "cl2WobbleCb";
            this.cl2WobbleCb.Size = new System.Drawing.Size(66, 58);
            this.cl2WobbleCb.TabIndex = 3;
            this.cl2WobbleCb.Text = "CL2";
            this.cl2WobbleCb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cl2WobbleCb.UseVisualStyleBackColor = true;
            // 
            // cl1AmplHswd
            // 
            this.cl1AmplHswd.DisplayName = "Ampl.";
            this.cl1AmplHswd.Location = new System.Drawing.Point(85, 45);
            this.cl1AmplHswd.Maximum = 100;
            this.cl1AmplHswd.Minimum = 0;
            this.cl1AmplHswd.Name = "cl1AmplHswd";
            this.cl1AmplHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.cl1AmplHswd.NameSize = new System.Drawing.Size(50, 24);
            this.cl1AmplHswd.Size = new System.Drawing.Size(385, 26);
            this.cl1AmplHswd.TabIndex = 2;
            this.cl1AmplHswd.Value = 0;
            this.cl1AmplHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.cl1AmplHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // cl1FreqHswd
            // 
            this.cl1FreqHswd.DisplayName = "Freq.";
            this.cl1FreqHswd.Location = new System.Drawing.Point(85, 13);
            this.cl1FreqHswd.Maximum = 100;
            this.cl1FreqHswd.Minimum = 0;
            this.cl1FreqHswd.Name = "cl1FreqHswd";
            this.cl1FreqHswd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            this.cl1FreqHswd.NameSize = new System.Drawing.Size(50, 24);
            this.cl1FreqHswd.Size = new System.Drawing.Size(385, 26);
            this.cl1FreqHswd.TabIndex = 1;
            this.cl1FreqHswd.Value = 0;
            this.cl1FreqHswd.ValueDisplaySize = new System.Drawing.Size(50, 21);
            this.cl1FreqHswd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
            // 
            // cl1WobbleCb
            // 
            this.cl1WobbleCb.Appearance = System.Windows.Forms.Appearance.Button;
            this.cl1WobbleCb.ControlValue = null;
            this.cl1WobbleCb.Location = new System.Drawing.Point(12, 13);
            this.cl1WobbleCb.Name = "cl1WobbleCb";
            this.cl1WobbleCb.Size = new System.Drawing.Size(66, 58);
            this.cl1WobbleCb.TabIndex = 0;
            this.cl1WobbleCb.Text = "CL1";
            this.cl1WobbleCb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cl1WobbleCb.UseVisualStyleBackColor = true;
            // 
            // LensWobbleForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(478, 212);
            this.Controls.Add(this.olAmplHswd);
            this.Controls.Add(this.olFreqHswd);
            this.Controls.Add(this.olWobbleCb);
            this.Controls.Add(this.cl2AmplHswd);
            this.Controls.Add(this.cl2FreqHswd);
            this.Controls.Add(this.cl2WobbleCb);
            this.Controls.Add(this.cl1AmplHswd);
            this.Controls.Add(this.cl1FreqHswd);
            this.Controls.Add(this.cl1WobbleCb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LensWobbleForm";
            this.Text = "Lens Wobbling";
            this.ResumeLayout(false);

		}

		#endregion

		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool cl1WobbleCb;
		private SEC.Nanoeye.Support.Controls.HswdCvd cl1FreqHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd cl1AmplHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd cl2AmplHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd cl2FreqHswd;
		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool cl2WobbleCb;
		private SEC.Nanoeye.Support.Controls.HswdCvd olAmplHswd;
		private SEC.Nanoeye.Support.Controls.HswdCvd olFreqHswd;
		private SEC.Nanoeye.Support.Controls.CheckBoxWithIControlBool olWobbleCb;
	}
}