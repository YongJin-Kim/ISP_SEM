namespace SEC.Nanoeye.NanoeyeSEM
{
    partial class NewMiniSEM
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
            this.ultraTilePanel1 = new Infragistics.Win.Misc.UltraTilePanel();
            this.ultraTile1 = new Infragistics.Win.Misc.UltraTile();
            this.StartMainButton = new SEC.Nanoeye.Controls.BitmapCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTilePanel1)).BeginInit();
            this.ultraTilePanel1.SuspendLayout();
            this.ultraTile1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTilePanel1
            // 
            this.ultraTilePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTilePanel1.LargeTileOrientation = Infragistics.Win.Misc.TileOrientation.Vertical;
            this.ultraTilePanel1.LargeTilePosition = Infragistics.Win.Misc.LargeTilePosition.Top;
            this.ultraTilePanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraTilePanel1.Name = "ultraTilePanel1";
            this.ultraTilePanel1.NormalModeDimensions = new System.Drawing.Size(1, 1);
            this.ultraTilePanel1.Size = new System.Drawing.Size(1305, 718);
            this.ultraTilePanel1.TabIndex = 0;
            this.ultraTilePanel1.Tiles.Add(this.ultraTile1);
            // 
            // ultraTile1
            // 
            this.ultraTile1.Caption = "ultraTile1";
            this.ultraTile1.Control = this.StartMainButton;
            this.ultraTile1.Controls.Add(this.StartMainButton);
            this.ultraTile1.Name = "ultraTile1";
            this.ultraTile1.PositionInNormalMode = new System.Drawing.Point(0, 0);
            this.ultraTile1.TabIndex = 0;
            // 
            // StartMainButton
            // 
            this.StartMainButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.StartMainButton.BackColor = System.Drawing.Color.Transparent;
            this.StartMainButton.Image = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.Align;
            this.StartMainButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.StartMainButton.Location = new System.Drawing.Point(0, 18);
            this.StartMainButton.Name = "StartMainButton";
            this.StartMainButton.Size = new System.Drawing.Size(64, 32);
            this.StartMainButton.Surface = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.PushToolButtonSmall1;
            this.StartMainButton.TabIndex = 174;
            this.StartMainButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartMainButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.StartMainButton.UseCompatibleTextRendering = true;
            // 
            // NewMiniSEM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 718);
            this.ControlBox = false;
            this.Controls.Add(this.ultraTilePanel1);
            this.Name = "NewMiniSEM";
            this.Text = "FormMainTest";
            ((System.ComponentModel.ISupportInitialize)(this.ultraTilePanel1)).EndInit();
            this.ultraTilePanel1.ResumeLayout(false);
            this.ultraTile1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraTilePanel ultraTilePanel1;
        private Infragistics.Win.Misc.UltraTile ultraTile1;
        private SEC.Nanoeye.Controls.BitmapCheckBox StartMainButton;
    }
}