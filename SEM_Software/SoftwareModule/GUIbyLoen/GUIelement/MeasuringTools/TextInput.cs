using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement.MeasuringTools
{
	internal partial class TextInput : Form
	{
		public Font ft;
		public string str;

		public Color colFont = Color.White;
		public TextInput()
		{
			InitializeComponent();

		}

		private void button1_Click(object sender, EventArgs e)
		{
			fontDialog1.Font = ft;
			if (fontDialog1.ShowDialog() == DialogResult.OK) {
				ft = fontDialog1.Font;
				label1.Font = ft;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();
			str = textBox1.Text;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void TextInput_Load(object sender, EventArgs e)
		{
			textBox1.Text = str;
			label1.Font = ft;
			label1.ForeColor = colFont;
		}

		private void buttonFontColor_Click(object sender, EventArgs e)
		{
			ColorDialog cd = new ColorDialog();
			cd.Color = colFont;
			if (cd.ShowDialog() == DialogResult.OK) {
				colFont = cd.Color;
				label1.ForeColor = colFont;
			}
		}
	}
}