using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class FastScanAreaSelect : Form
	{
		private int selectedArea = 5;

		[DefaultValue(5)]
		public int ScanningArea
		{
			get { return selectedArea; }
			set
			{
				selectedArea = value;

				switch (selectedArea) {
				case 1: radioButton1.Checked = true; break;
				case 2: radioButton2.Checked = true; break;
				case 3: radioButton3.Checked = true; break;
				case 4: radioButton4.Checked = true; break;
				case 5: radioButton5.Checked = true; break;
				case 6: radioButton6.Checked = true; break;
				case 7: radioButton7.Checked = true; break;
				case 8: radioButton8.Checked = true; break;
				case 9: radioButton9.Checked = true; break;
				default: throw new ArgumentOutOfRangeException("Scanning Area가 1과 9사이의 값이 아님");
				}
			}

		}

		public FastScanAreaSelect()
		{
			InitializeComponent();
		}

		private void radioButton_MouseClick(object sender, MouseEventArgs e)
		{
			if (sender == radioButton1) { selectedArea = 1; }
			else if (sender == radioButton2) { selectedArea = 2; }
			else if (sender == radioButton3) { selectedArea = 3; }
			else if (sender == radioButton4) { selectedArea = 4; }
			else if (sender == radioButton5) { selectedArea = 5; }
			else if (sender == radioButton6) { selectedArea = 6; }
			else if (sender == radioButton7) { selectedArea = 7; }
			else if (sender == radioButton8) { selectedArea = 8; }
			else if (sender == radioButton9) { selectedArea = 9; }
			else { throw new ArgumentOutOfRangeException("Scanning Area가 1과 9사이의 값이 아님"); }
			if (this.Visible == false) { return; }
			this.Close();

		}
	}
}
