using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM.Initialize
{
	public partial class DeviceSelector : Form
	{
		string[,] _Devices=null;
		public string[,] Devices
		{
			get { return null; }
			set {
				_Devices = value;

				comboBox1.Items.Clear();
				for (int i = 0; i < _Devices.GetLength(0); i++)
				{
					comboBox1.Items.Add(_Devices[i, 0]);
				}

				comboBox1.SelectedIndex = 0;
			}
		}

		public int SelectedIndex
		{
			get { return comboBox1.SelectedIndex; }
		}

		public DeviceSelector()
		{
			InitializeComponent();
			this.DialogResult = DialogResult.No;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
