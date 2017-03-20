using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class HistogramModifier : UserControl
	{
		public HistogramModifier()
		{
			InitializeComponent();
		}

		private void histogramViewer1_HistogramMaximumChanged(object sender, EventArgs e)
		{
			ChangeHistogramBar();
		}

		public short[,] ImageData
		{
			get { return histogramViewer1.ImageData; }
			set { histogramViewer1.ImageData = value; }
		}

		private void histogramViewer1_HistogramMinimumChanged(object sender, EventArgs e)
		{
			ChangeHistogramBar();
		}

		private void ChangeHistogramBar()
		{
			hScrollBar1.Maximum = 1024 - 1024 * (histogramViewer1.HistogramMaximum - histogramViewer1.HistogramMinimum) / (short.MaxValue - short.MinValue);
			//int cen = (histogramViewer1.HistogramMaximum + histogramViewer1.HistogramMinimum) / 2;
		}

		private void hScrollBar1_ValueChanged(object sender, EventArgs e)
		{

		}
	}
}
