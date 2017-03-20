using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SEC.Nanoeye.Support.Controls;

namespace SEC.Nanoeye.Support.Dialog
{
	public partial class ColumnMonitor : Form
	{
		List<ColumnValueMonitor> cvmList = new List<ColumnValueMonitor>();

		private SEC.Nanoeye.NanoColumn.ISEMController _Controller;
		public SEC.Nanoeye.NanoColumn.ISEMController Controller
		{
			get { return _Controller; }
			set
			{
				flowLayoutPanel1.Controls.Clear();
				_Controller = value;
				if (_Controller != null)
				{
					AddNewColumnValueMonitor();
				}
			}
		}

		public ColumnMonitor()
		{
			InitializeComponent();
		}

		private void AddNewColumnValueMonitor()
		{
			ColumnValueMonitor cvm = new ColumnValueMonitor();
			cvm.Size = new Size(flowLayoutPanel1.Width, 28);
			cvm.BorderStyle = BorderStyle.FixedSingle;
			cvm.ValueSelected += new EventHandler(cvm_ValueSelected);
			cvm.Controller = _Controller;
			flowLayoutPanel1.Controls.Add(cvm);
		}

		void cvm_ValueSelected(object sender, EventArgs e)
		{
			ColumnValueMonitor cvm = sender as ColumnValueMonitor;

			cvm.ValueSelected -= new EventHandler(cvm_ValueSelected);
			cvm.ValueDeselected += new EventHandler(cvm_ValueDeselected);

			AddNewColumnValueMonitor();
		}

		void cvm_ValueDeselected(object sender, EventArgs e)
		{
			ColumnValueMonitor cvm = sender as ColumnValueMonitor;

			flowLayoutPanel1.Controls.Remove(cvm);

			if (flowLayoutPanel1.Controls.Count == 0)
			{
				AddNewColumnValueMonitor();
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			foreach (Control con in flowLayoutPanel1.Controls)
			{
				if (con is ColumnValueMonitor)
				{
					ColumnValueMonitor cvm = con as ColumnValueMonitor;
					cvm.Size = new Size(flowLayoutPanel1.Width, 28);
				}
			}
		}
	}
}
