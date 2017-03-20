using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NanoeyeTestControls;

using System.IO;

using SEC.Nanoeye;
using SEC.Nanoeye.NanoImage;
using SEC.Nanoeye.NanoView;
using SEC.Nanoeye.Setting;
using SEC.Nanoeye.NanoColumn;
using SECtype = SEC.GenericSupport.DataType;

using System.Diagnostics;

namespace NanoeyeTest_MiniSEM
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		#region Varialbes
		IActiveScan scanner;
		ISettingManager setManage;
		SEC.Nanoeye.NanoColumn.IMiniSEM column;
		//SEC.Nanoeye.NanoStage.IStage stage;

		SEC.Nanoeye.NanoeyeFactory nanoeyeFct;
		#endregion

		readonly string _SetFileName = @".\Nanoeye001.bin";


		#region Init

		bool inited = false;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			nanoeyeFct = SEC.Nanoeye.NanoeyeFactory.CreateInstance(NanoeyeFactory.NanoeyeType.Evex_MiniSEM);


			SBfastscan.Tag = "Fast Scan";
			SBslowscan.Tag = "Slow Scan";
			SBfastphoto.Tag = "Fast Photo";
			SBslowphoto.Tag = "Slow Photo";
			SBpause.Tag = "Scan Pause";

			string[] items;

			scanner = nanoeyeFct.Scanner;

			items = scanner.GetDevList();
			if ( items == null )
			{
				MessageBox.Show("There is no DAQ devices.");
				Application.Exit();
				return;
			}
			scanner.Initialize(items[0]);
			DAQTextLable.Text = items[0];

			column = nanoeyeFct.Controller as SEC.Nanoeye.NanoColumn.IMiniSEM;
			items = column.AvailableDevices();
			if (items.Length == 0)
			{
				MessageBox.Show("There is no controller.");
				Application.Exit();
				return;
				//column.ColumnInit(null);
				//ComPortTextLable.Text = "Non";
			}
			else
			{
				column.Device = items[0];
				column.Initialize();
				ComPortTextLable.Text = items[0];
			}

			LinkeColumnEvent();

			setManage = nanoeyeFct.SettingManager;
			if ( File.Exists(_SetFileName) )
			{
			}
			else
			{
				CreateNewConfig(_SetFileName);
			}
			SettingLoad(_SetFileName);

			SMlistCB.Items.AddRange(setManage.ActiveScanList());
			SMlistCB.SelectedIndex = 0;

			inited = true;

			string[,] info;
			int cnt = column.ControlBoard(out info);
			for ( int i = 0 ; i < cnt ; i++ )
			{
				switch ( info[i, 0] )
				{
				case "Scan":
					ScanDate.Text = info[i, 1];
					ScanTime.Text = info[i, 2];
					break;
				case "Lens":
					LensDate.Text = info[i, 1];
					LensTime.Text = info[i, 2];
					break;
				case "Align":
					AlignDate.Text = info[i, 1];
					AlignTime.Text = info[i, 2];
					break;
				case "Stig":
					StigDate.Text = info[i, 1];
					StigTime.Text = info[i, 2];
					break;
				case "Egps":
					EgpsDate.Text = info[i, 1];
					EgpsTime.Text = info[i, 2];
					break;
				case "Vacuum":
					VacuumDate.Text = info[i, 1];
					VacuumTime.Text = info[i, 2];
					break;
				}
			}
		}

		private void LinkeColumnEvent()
		{

			EghvHSCD.SetControlValue(column.HvElectronGun);
			TipHSCD.SetControlValue(column.HvFilament);
			GridHSCD.SetControlValue(column.HvGrid);
			PmtHSCD.SetControlValue(column.HvPmt);
			CltHSCD.SetControlValue(column.HvCollector);

			CL1HSCD.SetControlValue((SECtype.IControlDouble)column.LensCondenser1);
			CL2HSCD.SetControlValue((SECtype.IControlDouble)column["LensCondenser2"]);
			OLcHSCD.SetControlValue((SECtype.IControlDouble)column["LensObjectCoarse"]);
			OLfHSCD.SetControlValue((SECtype.IControlDouble)column["LensObjectFine"]);

			MagxHSCD.SetControlValue(column.ScanMagnificationX);
			MagyHSCD.SetControlValue(column.ScanMagnificationY);
			AmpxHSCD.SetControlValue((SECtype.IControlDouble)column["ScanAmplitudeX"]);
			AmpyHSCD.SetControlValue((SECtype.IControlDouble)column["ScanAmplitudeY"]);
			RotateHSCD.SetControlValue((SECtype.IControlDouble)column.ScanRotation);

			BSxHSCD.SetControlValue((SECtype.IControlDouble)column["BeamShiftX"]);
			BSyHSCD.SetControlValue((SECtype.IControlDouble)column["BeamShiftY"]);
			GAxHSCD.SetControlValue((SECtype.IControlDouble)column["GunAlignX"]);
			GAyHSCD.SetControlValue((SECtype.IControlDouble)column["GunAlignY"]);

			StigXvalHSCD.SetControlValue((SECtype.IControlDouble)column.StigX);
			StigXabHSCD.SetControlValue((SECtype.IControlDouble)column.StigXab);
			StigXcdHSCD.SetControlValue((SECtype.IControlDouble)column["StigXcd"]);
			StigYvalHSCD.SetControlValue((SECtype.IControlDouble)column.StigY);
			StigYabHSCD.SetControlValue((SECtype.IControlDouble)column["StigYab"]);
			StigYcdHSCD.SetControlValue((SECtype.IControlDouble)column["StigYcd"]);

			((IColumnValue)column.HvElectronGun).RepeatUpdated += new ObjectArrayEventHandler(Eghv_RepeatUpdated);
			((IColumnValue)column["VacuumState"]).RepeatUpdated += new ObjectArrayEventHandler(VacuumState_RepeatUpdated);
		}

		void VacuumState_RepeatUpdated(object sender, object[] value)
		{
			VacuumState.Text = (string)(value[0]);
		}

		private void CreateNewConfig(string filename)
		{
			setManage.ControllerCreate("Column 0");
			setManage.ActiveScanCreate(scanner, "Fast Scan");
			setManage.ActiveScanCreate(scanner, "Slow Scan");
			setManage.ActiveScanCreate(scanner, "Fast Photo");
			setManage.ActiveScanCreate(scanner, "Slow Photo");
			setManage.ActiveScanCreate(scanner, "Scan Pause");
			setManage.Save(filename);
		}

		private void SettingLoad(string newName)
		{
			setManage.Load(newName);
			setManage.ActiveScanInit(scanner);
			HVListCB.Items.AddRange(setManage.ControllerList());
			HVListCB.SelectedIndex = 0;
		}
		#endregion

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
		}

		void column_CommunicationError(object sender, string board, object obj)
		{
			Trace.WriteLine(board, "Communication Error");
		}

		void Eghv_RepeatUpdated(object sender, object[] value)
		{
			Action<object[]> eud = x => { EmmitionTextLable.Text = ((double)x[0]).ToString(); };
			EmmitionTextLable.BeginInvoke(eud, new object[] { value });
		}

		#region video
		private void BrightnessTB_ValueChanged(object sender, EventArgs e)
		{
			paintPanel1.Brightness = BrightnessTB.Value;
		}

		private void ContrastTB_ValueChanged(object sender, EventArgs e)
		{
			paintPanel1.Contrast = ContrastTB.Value;
		}
		#endregion

		#region H.V. & Controller setting
		private void HVListCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			HVtextLable.Text = column.HVtext;

			MagReload.PerformClick();
		}

		private void ColAdd_Click(object sender, EventArgs e)
		{
			string nameBase = "Column";
			string nameResult;

			int i = 0;
			while (true)
			{
				if (!HVListCB.Items.Contains(nameBase + i.ToString()))
				{
					nameResult = nameBase + i.ToString();
					break;
				}
				i++;
			}

			setManage.ControllerCreate(nameResult);
			HVListCB.Items.Add(nameResult);
		}

		private void ColRemove_Click(object sender, EventArgs e)
		{
			if (HVListCB.Items.Count < 2)
			{
				MessageBox.Show("Column setup is few.");
				return;
			}
			string name = (string)(HVListCB.SelectedItem);

			HVListCB.Items.Remove(name);

			setManage.ControllerDelete(name);

			HVListCB.SelectedIndex = 0;
		}

		private void HVonoffBut_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if (cb.Checked)
			{
				using (Form fm = new Form())
				{

					((SECtype.IControlBool)column["EgpsEnable"]).Value = true;
					//column.ScanAmplitudeX.Value = 0.707;
					//column.ScanAmplitudeY.Value = 0.707;
					System.Threading.Thread.Sleep(3000);
					((SECtype.IControlDouble)column["HvElectronGun"]).Value = ((SECtype.IControlDouble)column["HvElectronGun"]).Value;
					((SECtype.IControlDouble)column["HvFilament"]).Value = ((SECtype.IControlDouble)column["HvFilament"]).Value;
					((SECtype.IControlDouble)column["HvGrid"]).Value = ((SECtype.IControlDouble)column["HvGrid"]).Value;
					((SECtype.IControlDouble)column["HvPmt"]).Value = ((SECtype.IControlDouble)column["HvPmt"]).Value;
					((SECtype.IControlDouble)column["HvCollector"]).Value = ((SECtype.IControlDouble)column["HvCollector"]).Value;
					System.Threading.Thread.Sleep(1000);
				}
			}
			else
			{
				column.HvEnable.Value = false;
			}
		}

		#endregion

		#region Scanner Setup
		private void SB_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton but = sender as RadioButton;

			if (!but.Checked)
			{
				return;
			}

			string mode = (string)(but.Tag);
			scanner.ScannerChange(mode, 0);

			if (mode == "Scan Pause")
			{
				paintPanel1.ReleseEvent();
			}
			else
			{
				IScanItemEvent isie = scanner.GetScanEvent(mode);
				paintPanel1.LinkeEvent(isie);
			}
		}

		private void SMlistCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			double[] sets = scanner.SettingGets((string)(SMlistCB.SelectedItem));

			aichNUD.Value = (decimal)sets[0];
			aifreqNUD.Value = (decimal)sets[1];
			aidCB.Checked = ((int)sets[2]) == 0 ? false : true;

			foreach (object it in aimaxDUD.Items)
			{
				double dou = double.Parse((string)it);
				if (dou == sets[3])
				{
					aimaxDUD.SelectedItem = it;
					break;
				}
			}
			foreach (object it in aiminDUD.Items)
			{
				double dou = double.Parse((string)it);
				if (dou == sets[4])
				{
					aiminDUD.SelectedItem = it;
					break;
				}
			}

			aofreqNUD.Value = (decimal)sets[5];

			foreach (object it in aomaxDUD.Items)
			{
				double dou = double.Parse((string)it);
				if (dou == sets[6])
				{
					aomaxDUD.SelectedItem = it;
					break;
				}
			}
			foreach (object it in aominDUD.Items)
			{
				double dou = double.Parse((string)it);
				if (dou == sets[7])
				{
					aominDUD.SelectedItem = it;
					break;
				}
			}

			framehNUD.Value = (decimal)sets[8];
			framewNUD.Value = (decimal)sets[9];
			pdelayNUD.Value = (decimal)sets[10];
			ratioxNUD.Value = (decimal)sets[11];
			ratioyNUD.Value = (decimal)sets[12];
			shiftxNUD.Value = (decimal)sets[13];
			shiftyNUD.Value = (decimal)sets[14];

			frameavrNUD.Value = (decimal)sets[15];
			blurNUD.Value = (decimal)sets[16];
			samcomNUD.Value = (decimal)sets[17];

			imghNUD.Value = (decimal)sets[18];
			imagxNUD.Value = (decimal)sets[19];
			imageyNUD.Value = (decimal)sets[20];
			imgwNUD.Value = (decimal)sets[21];
		}

		private void ScanNUD_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown nud = sender as NumericUpDown;
			scanner.SettingChange((string)SMlistCB.SelectedItem, int.Parse((string)nud.Tag), (double)nud.Value);
		}

		private void ScanDUD_SelectedItemChanged(object sender, EventArgs e)
		{
			DomainUpDown dud = sender as DomainUpDown;
			scanner.SettingChange((string)SMlistCB.SelectedItem, int.Parse((string)dud.Tag), double.Parse((string)dud.SelectedItem));
		}

		private void ScanCB_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			scanner.SettingChange((string)SMlistCB.SelectedItem, int.Parse((string)cb.Tag), cb.Checked ? 1 : 0);
		}

		private void SBrestart_Click(object sender, EventArgs e)
		{
			try
			{
				//scanner.ReStart((string)SMlistCB.SelectedItem);
				setManage.ActiveScanSettingSave(scanner, (string)SMlistCB.SelectedItem);
			}
			catch (ArgumentException ae)
			{
				MessageBox.Show(ae.Message);
			}
		}
		#endregion
	}
}
