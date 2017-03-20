using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NationalInstruments.DAQmx;

namespace SEC.Nanoeye.NanoImage.DataAcquation.NIDaq
{
	internal partial class Information : Form
	{
		public Information()
		{
			InitializeComponent();

			textBox1.Text = string.Format("{0}.{1}.{2}", DaqSystem.Local.DriverMajorVersion, DaqSystem.Local.DriverMinorVersion, DaqSystem.Local.DriverUpdateVersion);
		}

		private static Information _Default = null;
		public static Information Default
		{
			get { return _Default; }
			set { _Default = value; }
		}
	}
}
