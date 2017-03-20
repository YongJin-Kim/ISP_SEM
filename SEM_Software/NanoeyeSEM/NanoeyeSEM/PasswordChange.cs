using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class PasswordChange : Form
	{
		public PasswordChange()
		{
			InitializeComponent();
		}

		private void bApply_Click(object sender, EventArgs e)
		{
			if (mtbOriginal.Text != Properties.Settings.Default.Password) {
				MessageBox.Show(this,"Origianl password is not correct!!!", "Error");
				return;
			}

			if (mtbNew.Text != mtbNewRepeat.Text) {
				MessageBox.Show(this,"New password is not same with repeated password!!!","Error");
				return;
			}

			Properties.Settings.Default.Password = mtbNew.Text;
			Properties.Settings.Default.Save();
			this.Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}