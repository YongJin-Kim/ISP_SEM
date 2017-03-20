using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Kikwak.AutoFunctionCollection
{
	
	public partial class DebuggingWindow : Form
	{
		
		public DebuggingWindow()
		{
			InitializeComponent();
		}


		private delegate void AppendText(string text);

		[System.Diagnostics.Conditional("DEBUG")]
		public void AddString(string msg, string catagory)
		{
			string text = "[" + catagory + "] : " + msg + "\r\n";
			if (this.InvokeRequired) {
				this.BeginInvoke(new AppendText(InvokeAppend), new object[] { text });
			}
			else {
				InvokeAppend(text);
			}
			
		}

		public void InvokeAppend(string text)
		{
			textBox1.AppendText(text);
			textBox1.Select(textBox1.Text.Length - 1, 0);
			textBox1.ScrollToCaret();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			textBox1.Clear();
		}

	}
}
