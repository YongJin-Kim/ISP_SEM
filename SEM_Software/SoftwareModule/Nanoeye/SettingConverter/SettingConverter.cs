using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SettingConverter
{
	public partial class SettingConverter : Form
	{
		public SettingConverter()
		{
			InitializeComponent();
		}

		SEC.Nanoeye.Setting.SettingConverter sc = new SEC.Nanoeye.Setting.SettingConverter();

		private void inputBut_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Multiselect = false;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					inputTb.Text = ofd.FileName;
					int ver = sc.GetSettingVersion(inputTb.Text);

					switch ((SEC.Nanoeye.Setting.SettingVersionEnum) ver)
					{
					case SEC.Nanoeye.Setting.SettingVersionEnum.MiniSEM:
						inputLb.Text = "Mini-SEM";
						break;
					case SEC.Nanoeye.Setting.SettingVersionEnum.Nanoeye001:
						inputLb.Text = "Nanoeye";
						break;
					case SEC.Nanoeye.Setting.SettingVersionEnum.NanoeyeMiniSEM:
						inputLb.Text = "Nanoeye_Mini-SEM";
						break;
					case SEC.Nanoeye.Setting.SettingVersionEnum.Non:
					default:
						inputLb.Text = "Invalid";
						break;
					}

				}
			}
		}

		private void outputBut_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				switch (outputCb.SelectedIndex)
				{
				case 0:
					sfd.Filter = "Mini-SEM|*.config";
					break;
				case 1:
				case 2:
					sfd.Filter = "Nanoeye|*.bin";
					break;
				default:
					break;
				}

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					outputTb.Text = sfd.FileName;
				}
			}
		}

		private void outputCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			outputBut.Enabled = convertBut.Enabled = (outputCb.SelectedIndex >= 0);
		}

		private void convertBut_Click(object sender, EventArgs e)
		{
			sc.Convert(inputTb.Text, outputTb.Text, outputCb.SelectedIndex +1);
		}
	}
}
