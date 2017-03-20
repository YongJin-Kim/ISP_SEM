using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericSupportTester
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void argNud_ValueChanged(object sender, EventArgs e)
		{
			areaTb.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToAreaString((double)argNud.Value, (int)expNud.Value, (int)digitNud.Value, false, ' ');
			unitTb.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString((double)argNud.Value, (int)expNud.Value, (int)digitNud.Value, false, ' ');
		}

		private void testSpline_Click(object sender, EventArgs e)
		{
			SortedList<double, double>row = new SortedList<double, double>();

			for (int i = 0; i < 10; i++)
			{
				row.Add(i * 0.023, i);
			}

			splineResult.Items.Clear();

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			SEC.GenericSupport.Mathematics.Interpolation.Spline(row, 0.2);
			sw.Stop();

			splineResult.Items.Add("Normal - " + sw.Elapsed.ToString());

			sw.Reset();
			sw.Start();
			SEC.GenericSupport.Mathematics.Interpolation.SplineMulti(row, 8800);
			sw.Stop();
			splineResult.Items.Add("Multi - " + sw.Elapsed.ToString());
		}
	}
}
