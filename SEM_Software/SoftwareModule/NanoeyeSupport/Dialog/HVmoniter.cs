using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Dialog
{
	public partial class HVmoniter : Form
	{
		System.IO.StreamWriter sw;

		protected SEC.Nanoeye.NanoColumn.ISEMController _Column = null;
		public virtual SEC.Nanoeye.NanoColumn.ISEMController Column
		{
			get { return _Column; }
			set
			{
				runCb.Checked = false;

				if (_Column != null)
				{
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvElectronGun"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvElectronGun_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvFilament_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvGrid"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvGrid_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvCollector"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvClt_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvPmt"]).RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvPmt_RepeatUpdated);
					hvenableCbicb.ControlValue = null;
				}

				_Column = value;

				if (_Column != null)
				{
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvElectronGun"]).RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvElectronGun_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvFilament_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvGrid"]).RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvGrid_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvCollector"]).RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvClt_RepeatUpdated);
					((SEC.Nanoeye.NanoColumn.IColumnValue)_Column["HvPmt"]).RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(HvPmt_RepeatUpdated);
					hvenableCbicb.ControlValue = ((SECtype.IControlBool)_Column["HvEnable"]);
					anodeNud.ControlValue = _Column["HvElectronGun"];
					tipNud.ControlValue = _Column["HvFilament"];
					gridNud.ControlValue = _Column["HvGrid"];
					pmtNud.ControlValue = _Column["HvPmt"];
					cltNud.ControlValue = _Column["HvCollector"];
				}
			}
		}


		double egVmon = -1;
		double egImon = -1;

		void HvElectronGun_RepeatUpdated(object sender, object[] value)
		{
			Action act =() =>
			{
				double iMon = (double)value[0];
				double vMon = (double)value[1];

               

                if (vMon > 21.0)
                {
                    vMon += 8000.0;
                }

				if(egImon != iMon)
				{
					egImon = iMon;
					anodeImonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( egImon, -6, 3, false, 'A' );
				}

				if(egVmon != vMon)
				{
					egVmon = vMon;
                   

					anodeVmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( egVmon, 0, 3, false, 'V' );
				}
			};
			this.BeginInvoke( act );
		}

		double tipVmon = -1;
		double tipImon = -1;

		void HvFilament_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double iMon = (double)value[0];
				double vMon = (double)value[1];

				if(tipImon != iMon)
				{
					tipImon = iMon;
					tipCmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( tipImon, -6, 3, false, 'A' );
				}

				if(tipVmon != vMon)
				{
					tipVmon = vMon;
					tipVmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( tipVmon, 0, 3, false, 'V' );
				}
			};
			this.BeginInvoke( act );
		}

		double gridVmon = -1;
		double gridImon = -1;

		void HvGrid_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double iMon = (double)value[0];
				double vMon = (double)value[1];

				if(gridImon != iMon)
				{
					gridImon = iMon;
					gridCmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( gridImon, -6, 3, false, 'A' );
				}

				if(gridVmon != vMon)
				{
					gridVmon = vMon;
					gridVmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( gridVmon, 0, 3, false, 'V' );
				}
			};
			this.BeginInvoke( act );
		}

		double pmtVmon = -1;
		double pmtImon = -1;

		void HvPmt_RepeatUpdated(object sender, object[] value)
		{
			Action act=() =>
			{
				double iMon = (double)value[0];
				double vMon = (double)value[1];

				if(pmtImon != iMon)
				{
					pmtImon = iMon;
					pmtCmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( pmtImon, -6, 3, false, 'A' );
				}

				if(pmtVmon != vMon)
				{
					pmtVmon = vMon;
					pmtVmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( pmtVmon, 0, 3, false, 'V' );
				}
			};
			this.BeginInvoke( act );
		}

		double cltVmon = -1;
		double cltImon = -1;

		void HvClt_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double iMon = (double)value[0];
				double vMon = (double)value[1];

				if(cltImon != iMon)
				{
					cltImon = iMon;
					cltCmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( cltImon, -6, 3, false, 'A' );
				}

				if(cltVmon != vMon)
				{
					cltVmon = vMon;

					cltVmonLab.Text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString( cltVmon, 0, 3, false, 'V' );
				}
			};
			this.BeginInvoke( act );
		}
		
		public HVmoniter()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			filenameTb.Text = @"c:\mini-sem_hvlog_" + DateTime.Now.Date.ToFileTime().ToString() + ".txt";
			base.OnLoad(e);
		}

		private void filenameBut_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.* ";
			sfd.AutoUpgradeEnabled = true;
			if (sfd.ShowDialog(this) == DialogResult.OK)
			{
				filenameTb.Text = sfd.FileName;
			}
		}

		protected virtual void runCb_CheckedChanged(object sender, EventArgs e)
		{
			if (runCb.Checked)
			{
				try
				{
					sw = new System.IO.StreamWriter(filenameTb.Text, true);
					AppendText(string.Format("Logger is started at " + DateTime.Now.ToString()));
					AppendText(string.Format("{0,22} {1,16} {2,16} {3,16} {4,16} {5,16} {6,16}", "Time", "Anode V", "Anode C", "Tip V", "Tip C", "Grid V", "Grid C"));
				}
				catch (Exception)
				{
					MessageBox.Show(this, "Fale to open file. Check file path.", "Error", MessageBoxButtons.OK);
					runCb.Checked = false;
					return;
				}
			}

			periodNud.Enabled = !runCb.Checked;
			updateTimer.Enabled = runCb.Checked;

			if (!runCb.Checked)
			{
				if (sw != null)
				{
					sw.WriteLine("Logger is stoped at " + DateTime.Now.ToString());
					sw.WriteLine("");
					sw.Close();
					sw = null;
				}
			}
		}

		private void periodNud_ValueChanged(object sender, EventArgs e)
		{
			updateTimer.Interval = (int)periodNud.Value * 1000;
		}

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			AppendText(string.Format("{0,22} {1,16} {2,16} {3,16} {4,16} {5,16} {6,16}", DateTime.Now, egVmon, egImon, tipVmon, tipImon, gridVmon, gridImon));
		}

		protected void AppendText(string str)
		{
			sw.WriteLine(str);
			Action act = () =>
			{
				textBox1.Focus();
				textBox1.Text += str + "\r\n";
				textBox1.ScrollToCaret();
			};
			if (textBox1.InvokeRequired) { textBox1.BeginInvoke(act); }
			else { act(); }
			sw.Flush();
		}
	}
}
