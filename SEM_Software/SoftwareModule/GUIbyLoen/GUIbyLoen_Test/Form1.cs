using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SEC.GUIelement;

namespace GUIbyLoen_Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			propertyGrid1.SelectedObject = angleBar1;
		}


		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ( tabControl1.SelectedTab.Controls.Count > 0 ) {
				propertyGrid1.SelectedObject = tabControl1.SelectedTab.Controls[0];
			}
		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}

		private void triangle2_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form fm = new Form();
			fm.Show(this);
		}


	}
}
