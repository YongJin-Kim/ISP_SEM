using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SEC.Nanoeye.Controls;
using System.Threading;
using System.Globalization;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class InfoZone : Form
	{
        MiniSEM miniSEM;

		public InfoZone()
		{
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Language);

			InitializeComponent();

			TransparentNUD.Value = (decimal)(Properties.Settings.Default.MicronBackColor.A) * 100 / 255;

           
            //TransparentNUD.Value = 100;
            
           
		}

        public InfoZone(MiniSEM minisem)
        {
            InitializeComponent();

            TransparentNUD.Value = (decimal)(Properties.Settings.Default.MicronBackColor.A) * 100 / 255;


            //TransparentNUD.Value = 100;
            this.miniSEM = minisem;

        }

		protected override void OnLoad(EventArgs e)
		{
			TextManager.Instance.DefineText(this);

			base.OnLoad(e);
		}

		private void ReGenerate()
		{
			Bitmap bm = new Bitmap(DescriptionLBL.Width, DescriptionLBL.Height);

			Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);

			using (Graphics g = Graphics.FromImage(bm))
			{

				string str = Properties.Settings.Default.DescriptorText;
				Color bkCol = Properties.Settings.Default.MicronEdgeColor;
				Brush bkBr = new SolidBrush(bkCol);
				Color txCol = Properties.Settings.Default.MicronTextColor;
				Brush txBr = new SolidBrush(txCol);
				Font txFt = Properties.Settings.Default.MicronFont;

				g.Clear(Properties.Settings.Default.MicronBackColor);

				StringFormat style = new StringFormat();
				style.Alignment = StringAlignment.Center;
				style.LineAlignment = StringAlignment.Center;

				rect.Offset(1, 1);
				g.DrawString(str, txFt, bkBr, rect, style);
				rect.Offset(-2, 0);
				g.DrawString(str, txFt, bkBr, rect, style);
				rect.Offset(0, -2);
				g.DrawString(str, txFt, bkBr, rect, style);
				rect.Offset(2, 0);
				g.DrawString(str, txFt, bkBr, rect, style);
				rect.Offset(-1, 1);
				g.DrawString(str, txFt, txBr, rect, style);
			}
			DescriptionLBL.Image = bm;
		}



		private void TransparentNUD_ValueChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.MicronBackColor = Color.FromArgb((int)(TransparentNUD.Value * 255 / 100), Properties.Settings.Default.MicronBackColor);
            
			ReGenerate();
		}

		private void DescriptionTB_TextChanged(object sender, EventArgs e)
		{
			ReGenerate();
		}

		private void BackColBUT_Click(object sender, EventArgs e)
		{
			using (ColorDialog cd = new ColorDialog())
			{
				cd.Color = Properties.Settings.Default.MicronBackColor;
				if (cd.ShowDialog() == DialogResult.OK)
				{
					Properties.Settings.Default.MicronBackColor = Color.FromArgb((int)(TransparentNUD.Value * 255 / 100), cd.Color);
                    BackColLab.BackColor = Properties.Settings.Default.MicronBackColor;
					//ReGenerate();
				}
			}
		}

		private void TextColBut_Click(object sender, EventArgs e)
		{
			using (ColorDialog cd = new ColorDialog())
			{
				cd.Color = Properties.Settings.Default.MicronTextColor;
				if (cd.ShowDialog() == DialogResult.OK)
				{
					Properties.Settings.Default.MicronTextColor = cd.Color;
                    TextColLab.BackColor = Properties.Settings.Default.MicronTextColor;
					//ReGenerate();
				}
			}
		}

		private void EdgeColBut_Click(object sender, EventArgs e)
		{
			using (ColorDialog cd = new ColorDialog())
			{
				cd.Color = Properties.Settings.Default.MicronEdgeColor;
				if (cd.ShowDialog() == DialogResult.OK)
				{
					Properties.Settings.Default.MicronEdgeColor = cd.Color;
                    EdgeColLab.BackColor = Properties.Settings.Default.MicronEdgeColor;
					//ReGenerate();
				}
			}
		}

		private void FontSelectBut_Click(object sender, EventArgs e)
		{
			using (FontDialog fd = new FontDialog())
			{
				fd.Font = Properties.Settings.Default.MicronFont;
				fd.ShowColor = false;
				if (fd.ShowDialog() == DialogResult.OK)
				{
					Properties.Settings.Default.MicronFont = fd.Font;
                    FontSelectBut.Text = Properties.Settings.Default.MicronFont.Name + ", " + Properties.Settings.Default.MicronFont.Size.ToString();
					//ReGenerate();
				}
			}
		}

        private bool _FormEnable = false;
        public bool FormEnable
        {
            get { return _FormEnable; }
            set { _FormEnable = value; }
        }

        
		private void CloseBut_Click(object sender, EventArgs e)
		{
           
            //mainform.Owner = this.Owner;
            //mainform.InfoZonechecked(sender, e);


            miniSEM.InfoFormClose();

            this.Close();

            
		}

        private void InfoEnableCheckedChange(object sender, EventArgs e)
        {
            BitmapRadioButton brb = sender as BitmapRadioButton;

        }

        private void InfoFormMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);
            }
        }

        private Point mouseCurrentPoint = new Point(0, 0);

        private void InfoFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseCurrentPoint = e.Location;
            }
        }

        private void InfoCheckedChange(object sender, EventArgs e)
        {
            BitmapCheckBox bcb = sender as BitmapCheckBox;

            switch (bcb.Name)
            {
                case "VoltageCheckBox":
                    Properties.Settings.Default.MicronVoltage = bcb.Checked;

                    break;
            }
        }

        private void Company_TextChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DescriptorText = 
            
        }

       


       

        
	}
}
