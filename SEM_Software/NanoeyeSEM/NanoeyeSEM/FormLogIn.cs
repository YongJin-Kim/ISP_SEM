using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class FormLogIn : Form
	{
		public FormLogIn()
		{
			InitializeComponent();
		}

		private void DialogOkButton_Click(object sender, EventArgs e)
		{
			if (textBox1.Text == Properties.Settings.Default.Password)
			{
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				MessageBox.Show("Password incorrect.");
				textBox1.Text = "";
			}
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
			case Keys.Escape:
				this.Dispose();
				break;
			case Keys.Enter:
				DialogOkButton.PerformClick();
				break;
			}
		}

		private void bChange_Click(object sender, EventArgs e)
		{
			PasswordChange pc = new PasswordChange();
			pc.ShowDialog(this);
		}
	}
}
