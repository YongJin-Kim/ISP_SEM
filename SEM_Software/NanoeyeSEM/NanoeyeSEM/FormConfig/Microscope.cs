using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SECcolumn = SEC.Nanoeye.NanoColumn;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	public partial class Microscope : Form, IMicroscopeSetupWindow
	{
		#region Delegate
		Action<Label,string> TextSetAct = (lab, str) => { lab.Text = str; };
		#endregion

		#region Property
		private SEC.Nanoeye.NanoeyeSEM.Settings.ISettingManager setManager = null;
		private SECcolumn.I4000M column = null;

		#endregion

		#region Event
		public event EventHandler  HVtextChanged;
		protected virtual void OnHVtextChanged()
		{
			if (HVtextChanged != null) { HVtextChanged(this, EventArgs.Empty); }
		}

		public event EventHandler  ProfileListChanged;
		protected virtual void OnProfileListChanged()
		{
			if (ProfileListChanged != null) { ProfileListChanged(this, EventArgs.Empty); }
		}

		#endregion

		#region 생성 및 파괴
		public Microscope()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

          
            column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;
       
			setManager = SystemInfoBinder.Default.SetManager;

			setManager.ColumnLoad(column.Name, column, SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);

			ColumnLink();

			scanMagTableSet(column.ScanMagnificationTable.TableGet());
			WDMatTableSet((column.LensWDtable as SECtype.ITableContainner).TableGet());

          
            EDSSettingsLoad();

            CameraSettingsLoad();
		}

		private void DisposeInner(bool disposing)
		{
			if (column != null)
			{
				if (hvEghvvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
				}
				if (hvTipvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
				}
				if (hvGridvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
				}

				if (MessageBox.Show(this, "Save setting?", "Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					//SettingSave();
				}

				ColumnRelease();
				System.Diagnostics.Debug.WriteLine("Microscope Disposed.");
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (column != null)
			{
				switch (MessageBox.Show(this, "Save setting?", "Close", MessageBoxButtons.YesNoCancel))
				{
				case DialogResult.Yes:
					SettingSave();
					break;
				case DialogResult.No:
					break;
				case DialogResult.Cancel:
					e.Cancel=true;
					base.OnClosing(e);
					return;
				}

				if (hvEghvvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
				}
				if (hvTipvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
				}
				if (hvGridvolLab.BackColor == Color.Red)
				{
					((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
				}

				ColumnRelease();

				System.Diagnostics.Debug.WriteLine("Microscope Closing.");
			}
			base.OnClosing(e);
		}

		#endregion

		public void SettingSave()
		{
			setManager.ColumnSave(column, SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
		}

		public void SettingChanged()
		{
			infoNameTb.Text = column.Name;
			hvTextTb.Text = column.HVtext;

			object obj = null;

			column.ScanMagnificationTable.SetStyle((int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Get, ref obj);
			scanMagMaxNud.Value = (int)obj;
		}

		private void ColumnRelease()
		{
			infoNameTb.Text = column.Name;

			hvEghvvalHswd.ControlValue = null;
			hvTipvalHswd.ControlValue = null;
			hvGridvalHswd.ControlValue = null;
			hvPmtvalHswd.ControlValue = null;
			hvCltvalHswd.ControlValue = null;
			hvTextTb.Text = column.HVtext;

			beamshfitRoataionHswdcvd.ControlValue = null;

			lensCL1ValHc.ControlValue = null;
			lensCL2ValHc.ControlValue = null;
           

			column.ScanMagnificationTable.TableChanged -= new EventHandler(ScanMagnificationTable_TableChanged);
			(column.LensWDtable as SECtype.ITableContainner).TableChanged -= new EventHandler(LensWDtable_TableChanged);

			column.ScanMagnificationTable.SelectedIndexChanged -= new EventHandler(ScanMagnificationTable_SelectedIndexChanged);
			column.LensObjectCoarse.ValueChanged -= new EventHandler(LensObjCoarse_ValueChanged);
			

			column = null;
		}

		private void ColumnLink()
		{
			infoNameTb.Text = column.Name;

			hvEghvvalHswd.ControlValue = ((SECtype.IControlDouble)column["HvElectronGun"]);
			hvTipvalHswd.ControlValue = ((SECtype.IControlDouble)column["HvFilament"]);
			hvGridvalHswd.ControlValue = ((SECtype.IControlDouble)column["HvGrid"]);
			hvPmtvalHswd.ControlValue = ((SECtype.IControlDouble)column["HvPmt"]);
			hvCltvalHswd.ControlValue = ((SECtype.IControlDouble)column["HvCollector"]);


			lensCL1ValHc.ControlValue = ((SECtype.IControlDouble)column["LensCondenser1"]);
			lensCL2ValHc.ControlValue = ((SECtype.IControlDouble)column["LensCondenser2"]);
            

			lensCL1MinNudicd.ControlValue = column.LensCondenser1;
			lensCL1MaxNudicd.ControlValue = column.LensCondenser1;

			hvTextTb.Text = column.HVtext;

			beamshfitRoataionHswdcvd.ControlValue = column.BeamShiftAngle;
            //beamshfitRoataionHswdcvd.ControlValue = ((SECtype.IControlDouble)column["BeamShiftRoation"]);
            

			scanRatioYHcvd.ControlValue = column.ScanMagnificationY;
			scanRatioXHcvd.ControlValue = column.ScanMagnificationX;
			scanRotationHcvd.ControlValue = column.ScanRotation;
			scanFeedbackIci.ControlValue = column.ScanFeedbackMode;

			wdLensobjHcvd.ControlValue = column.LensObjectCoarse;

			scanMagTableSet(column.ScanMagnificationTable.TableGet());
			WDMatTableSet((column.LensWDtable as SECtype.ITableContainner).TableGet());

			wdOlcMaxNudicd.ControlValue = column.LensObjectCoarse;
			wdOlcMinNudicd.ControlValue = column.LensObjectCoarse;

            stigXabHswd.ControlValue =  ((SECtype.IControlDouble)column["StigXab"]);
            stigXcdHswd.ControlValue =  ((SECtype.IControlDouble)column["StigXcd"]);
            stigXfreqHswd.ControlValue =  ((SECtype.IControlDouble)column["StigXWobbleFrequence"]);
            stigXamplHswd.ControlValue =  ((SECtype.IControlDouble)column["StigXWobbleAmplitude"]);

            stigYabHswd.ControlValue =  ((SECtype.IControlDouble)column["StigYab"]);
            stigYcdHswd.ControlValue =  ((SECtype.IControlDouble)column["StigYcd"]);
            stigYfreqHswd.ControlValue =  ((SECtype.IControlDouble)column["StigYWobbleFrequence"]);
            stigYamplHswd.ControlValue =  ((SECtype.IControlDouble)column["StigYWobbleAmplitude"]);


			column.ScanMagnificationTable.TableChanged += new EventHandler(ScanMagnificationTable_TableChanged);
			column.ScanMagnificationTable.SelectedIndexChanged += new EventHandler(ScanMagnificationTable_SelectedIndexChanged);
			(column.LensWDtable as SECtype.ITableContainner).TableChanged += new EventHandler(LensWDtable_TableChanged);
			column.LensObjectCoarse.ValueChanged += new EventHandler(LensObjCoarse_ValueChanged);

			object obj = null;

			column.ScanMagnificationTable.SetStyle((int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Get, ref obj);
			scanMagMaxNud.Value = (int)obj;

            //newController 추가부분
            //ScanGeoXA.ControlValue = column.ScanGeoXA;
            //ScanGeoXB.ControlValue = column.ScanGeoXB;
            //ScanGeoYA.ControlValue = column.ScanGeoYA;
            //ScanGeoYB.ControlValue = column.ScanGeoYB;

            //Scan_AmpXA.ControlValue = column.Scan_AmpXA;
            //Scan_AmpXB.ControlValue = column.Scan_AmpXB;

            //Scan_AmpYA.ControlValue = column.Scan_AmpYA;
            //Scan_AmpYB.ControlValue = column.Scan_AmpYB;

            //VariableX.ControlValue = column.VariableX;
            //VariableY.ControlValue = column.VariableY;


		}

		#region Common Button
		private void ProfileAdd_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "Do you want to append new Microscope Profile?", "Microscope", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SystemInfoBinder.Default.SetManager.ColumnCreate("Non" + (SystemInfoBinder.Default.SetManager.ColumnList().Length * 2).ToString());
				OnProfileListChanged();
			}
		}

		private void ProfileDelete_Click(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			if (MessageBox.Show(this, "Do you want to delete this Microscope Profile?", "Microscope", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SystemInfoBinder.Default.SetManager.ColumnDelete(_Column.Name);
				OnProfileListChanged();
			}
		}

		private void Apply_Click(object sender, EventArgs e)
		{

           
            EDSSettingsSave();

			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			SystemInfoBinder.Default.SetManager.ColumnSave(_Column, SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
            SystemInfoBinder.Default.SetManager.Save(SystemInfoBinder.Default.SettingFileName);
			System.Media.SystemSounds.Beep.Play();
		}

		private void OkClose_Click(object sender, EventArgs e)
		{
			//SetManger.ControllerSettingSave(_Column, 0);
			this.Close();
		}

		private void CancelClose_Click(object sender, EventArgs e)
		{

		}
		#endregion

		#region HV Monitor
		private void hvMonitor_Click(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			if (_Column == null) { return; }

			Label lab = sender as Label;

			bool linked = false;

			if (lab.BackColor == Color.Black) { linked = false; }
			else { linked = true; }

			switch (lab.Name)
			{
			case "hvEghvvolLab":
			case "hvEghvcurLab":
				if (linked)
				{
					((SECcolumn.IColumnValue)_Column["HvElectronGun"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
					hvEghvvolLab.BackColor = Color.Black;
					hvEghvcurLab.BackColor = Color.Black;
				}
				else
				{
					((SECcolumn.IColumnValue)_Column["HvElectronGun"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Eghv_RepeatUpdated);
					hvEghvvolLab.BackColor = Color.Red;
					hvEghvcurLab.BackColor = Color.Red;
				}
				break;
			case "hvTipvolLab":
			case "hvTipcurLab":
				if (linked)
				{
					((SECcolumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
					hvTipvolLab.BackColor = Color.Black;
					hvTipcurLab.BackColor = Color.Black;
				}
				else
				{
					((SECcolumn.IColumnValue)_Column["HvFilament"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Tip_RepeatUpdated);
					hvTipvolLab.BackColor = Color.Red;
					hvTipcurLab.BackColor = Color.Red;
				}
				break;
			case "hvGridvolLab":
			case "hvGridcurLab":
				if (linked)
				{
					((SECcolumn.IColumnValue)_Column["HvGrid"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
					hvGridvolLab.BackColor = Color.Black;
					hvGridcurLab.BackColor = Color.Black;
				}
				else
				{
					((SECcolumn.IColumnValue)_Column["HvGrid"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
					hvGridvolLab.BackColor = Color.Red;
					hvGridcurLab.BackColor = Color.Red;
				}
				break;
			case "hvPmtvolLab":
			case "hvPmtcurLab":
                if (linked)
                {
                    ((SECcolumn.IColumnValue)_Column["HvPmt"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Pmt_RepeatUpdated);
                    hvPmtvolLab.BackColor = Color.Black;
                    hvPmtcurLab.BackColor = Color.Black;
                }
                else
                {
                    ((SECcolumn.IColumnValue)_Column["HvPmt"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Pmt_RepeatUpdated);
                    hvPmtvolLab.BackColor = Color.Red;
                    hvPmtcurLab.BackColor = Color.Red;
                }
                break;

			case "hvCltvolLab":
			case "hvCltcurHswd":
                if (linked)
                {
                    ((SECcolumn.IColumnValue)_Column["HvCollector"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Clt_RepeatUpdated);
                    hvCltvolLab.BackColor = Color.Black;
                    hvCltcurHswd.BackColor = Color.Black;
                }
                else
                {
                    ((SECcolumn.IColumnValue)_Column["HvCollector"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Clt_RepeatUpdated);
                    hvCltvolLab.BackColor = Color.Red;
                    hvCltcurHswd.BackColor = Color.Red;
                }
				break;
			}
		}

		void Grid_RepeatUpdated(object sender, object[] value)
		{
			if (InvokeRequired)
			{
				hvGridvolLab.BeginInvoke(TextSetAct, new object[] { hvGridvolLab, ((double)(value[1])).ToString() });
				hvGridcurLab.BeginInvoke(TextSetAct, new object[] { hvGridcurLab, ((double)(value[0])).ToString() });
			}
		}

		void Tip_RepeatUpdated(object sender, object[] value)
		{
			if (InvokeRequired)
			{
				hvTipvolLab.BeginInvoke(TextSetAct, new object[] { hvTipvolLab, ((double)(value[1])).ToString() });
				hvTipcurLab.BeginInvoke(TextSetAct, new object[] { hvTipcurLab, ((double)(value[0])).ToString() });
			}
		}



		void Eghv_RepeatUpdated(object sender, object[] value)
		{
			if (InvokeRequired)
			{
				hvEghvvolLab.BeginInvoke(TextSetAct, new object[] { hvEghvvolLab, ((double)(value[1])).ToString() });
				hvEghvcurLab.BeginInvoke(TextSetAct, new object[] { hvEghvcurLab, ((double)(value[0])).ToString() });
			}
		}

        void Pmt_RepeatUpdated(object sender, object[] value)
        {
            if (InvokeRequired)
            {
                hvPmtvolLab.BeginInvoke(TextSetAct, new object[] { hvPmtvolLab, ((double)(value[1])).ToString() });
                hvPmtcurLab.BeginInvoke(TextSetAct, new object[] { hvPmtcurLab, ((double)(value[0])).ToString() });
            }
        }

        void Clt_RepeatUpdated(object sender, object[] value)
        {
            if (InvokeRequired)
            {
                hvCltvolLab.BeginInvoke(TextSetAct, new object[] { hvPmtvolLab, ((double)(value[1])).ToString() });
                hvCltcurHswd.BeginInvoke(TextSetAct, new object[] { hvPmtcurLab, ((double)(value[0])).ToString() });
            }
        }
		#endregion

		#region Info
		private void infoNameTb_TextChanged(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			string preName = _Column.Name;

			if (_Column.Name != infoNameTb.Text)
			{
				SystemInfoBinder.Default.SetManager.ColumnNameChagne(_Column, infoNameTb.Text);
				OnProfileListChanged();
			}
		}

		private void infoSequenceNud_ValueChanged(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
		}
		#endregion

		#region HV
		private void hvTextTb_TextChanged(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			if (_Column.HVtext != hvTextTb.Text)
			{
				_Column.HVtext = hvTextTb.Text;
				
				if (this.Visible)
				{
					SystemInfoBinder.Default.SetManager.ColumnSave(_Column, SEC.Nanoeye.NanoeyeSEM.Settings.ColumnOnevalueMode.Factory);
				}

				OnHVtextChanged();
			}
		}
		#endregion

		#region Function
		private void Function_Click(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			Button bnt = sender as Button;

			switch (bnt.Name)
			{
			case "fncHVlogBnt":
				SEC.Nanoeye.Support.Dialog.HVmoniter hvm = new SEC.Nanoeye.Support.Dialog.HVmoniter();
				hvm.Column = _Column;
				hvm.Show(this.Owner);
				break;
			case "fncLenswobbleBnt":
				SEC.Nanoeye.Support.Dialog.LensWobbleForm lwf = new SEC.Nanoeye.Support.Dialog.LensWobbleForm();
				lwf.Column = _Column;
				lwf.Show(this.Owner);
				break;
			case "fncMonitorBnt":
				SEC.Nanoeye.Support.Dialog.ColumnMonitor cm = new SEC.Nanoeye.Support.Dialog.ColumnMonitor();
				cm.Controller = _Column;
				cm.Show(this.Owner);
				break;
			default:
				throw new NotSupportedException();
			}
		}

		private void InfoBnt_Click(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;


			string[,] infos;
			int cnt = _Column.ControlBoard(out infos);

			if (infos == null)
			{
				infoBoardTb.Text = "There are no informations.";
				return;
			}

			int w = infos.GetLength(1);

			infoBoardTb.Text = "";

			for (int i = 0; i < cnt; i++)
			{
				for (int j = 0; j < w; j++)
				{
					infoBoardTb.Text += infos[i, j] + "\t";
				}
				infoBoardTb.Text += "\r\n";
			}
		}
		#endregion

		#region WD
		void LensWDtable_TableChanged(object sender, EventArgs e)
		{
			WDMatTableSet((sender as SECtype.ITableContainner).TableGet());
		}

		void LensObjCoarse_ValueChanged(object sender, EventArgs e)
		{
			Action act = () =>
			{
				wdDistanceNud.Value = (column.LensWDtable as SECcolumn.Lens.IWDSplineObjBase).WorkingDistance;
				wdConstNud.Value = (decimal)(column.LensWDtable as SECcolumn.Lens.IWDSplineObjBase).MagConstant;
			};

			if (InvokeRequired) { this.BeginInvoke(act); }
			else { act(); }
		}

		private void WDMatTableSet(object[,] table)
		{
			wdTableLb.BeginUpdate();
			wdTableLb.Items.Clear();

			for (int i = 0; i < table.GetLength(0); i++)
			{
				string str = "";
				for (int j = 0; j < table.GetLength(1); j++)
				{
					str += table[i, j].ToString() + "\t";
				}
				wdTableLb.Items.Add(str);
			}
			wdTableLb.EndUpdate();
		}

		private void wdAppendBut_Click(object sender, EventArgs e)
		{
			try
			{
				(column.LensWDtable as SECcolumn.Lens.IWDSplineObjBase).TableAppend(new object[]{
					(int)(wdDistanceNud.Value),
					(int)(wdLensobjHcvd.Value),
					(double)(wdConstNud.Value),
					(int)(scanRotationHcvd.Value),
					(int)(beamshfitRoataionHswdcvd.Value)});
			}
			catch (ArgumentException ae)
			{
				MessageBox.Show(this, ae.Message);
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ae);
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				System.Diagnostics.Trace.Flush();
				MessageBox.Show(this, "Program error occured. Program will be closed.");
				this.Owner.Close();
			}
		}

		private void wdDeleteBut_Click(object sender, EventArgs e)
		{
			int index = wdTableLb.SelectedIndex;

			if (index < 0)
			{
				MessageBox.Show("Table is not selected.");
				return;
			}

			int wd = int.Parse(((string)wdTableLb.SelectedItem).Split('\t')[0]);
			(column.LensWDtable as SECtype.ITableContainner).TableRemove(wd);
		}

		private void wdChangeBut_Click(object sender, EventArgs e)
		{
			int index = wdTableLb.SelectedIndex;

			if (index < 0)
			{
				MessageBox.Show("Table is not selected.");
				return;
			}

			int obj = int.Parse(((string)wdTableLb.SelectedItem).Split('\t')[0]);
			try
			{
				(column.LensWDtable as SECcolumn.Lens.IWDSplineObjBase).TableChange(obj, new object[]{
					(int)(wdDistanceNud.Value),
					(int)(wdLensobjHcvd.Value),
					(double)(wdConstNud.Value),
					(int)(scanRotationHcvd.Value),
					(int)(beamshfitRoataionHswdcvd.Value)});
			}
			catch (ArgumentException ae)
			{
				MessageBox.Show(this, ae.Message);
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ae);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Program error occured. Program will be closed.");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				System.Diagnostics.Trace.Flush();
				this.Owner.Close();
			}
		}
		#endregion

		#region Magnification
		void ScanMagnificationTable_TableChanged(object sender, EventArgs e)
		{
			scanMagTableSet((sender as SECtype.ITable).TableGet());
		}

		void ScanMagnificationTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				object[] vals= column.ScanMagnificationTable[column.ScanMagnificationTable.SelectedIndex];

				scanMagnificationNud.Value = (int)vals[0];

				scanFeedbackIci.Value = (int)vals[3];
			}
			catch (IndexOutOfRangeException ioore)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ioore);
			}
		}

		private void scanMagTableSet(object[,] table)
		{
			scanMagtableLb.BeginUpdate();
			scanMagtableLb.Items.Clear();

			for(int i = 0; i < table.GetLength(0); i++)
			{
				string str = "";
				str += ((int)table[i, 0]).ToString("D8") + "\t";
				str += ((double)table[i, 1]).ToString("F6") + "\t";
				str += ((double)table[i, 2]).ToString("F6") + "\t";
				str += ((int)table[i, 3]).ToString("D1") ;
				scanMagtableLb.Items.Add(str);
			}
			scanMagtableLb.EndUpdate();
		}

		private void scanMagappBut_Click(object sender, EventArgs e)
		{
			SECcolumn.I4000M _Column = SystemInfoBinder.Default.Nanoeye.Controller as SECcolumn.I4000M;

			try
			{
				_Column.ScanMagnificationTable.TableAppend(new object[]{
				(int)(scanMagnificationNud.Value),
				(double)(scanRatioXHcvd.ControlValue as SECtype.IControlDouble).Value,
				(double)(scanRatioYHcvd.ControlValue as SECtype.IControlDouble).Value,
				(int)(scanFeedbackIci.ControlValue as SECtype.IControlInt).Value});
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Same magnification exist.");
			}
		}

		private void scanMagdelBut_Click(object sender, EventArgs e)
		{
			int index = scanMagtableLb.SelectedIndex;
			if (index < 0)
			{
				MessageBox.Show("Table is not selected.");
				return;
			}

			double magConst = (column.LensWDtable as SECcolumn.IMagCorrector).MagConstant;

			int mag = (int)Math.Floor(int.Parse((((string)scanMagtableLb.SelectedItem).Split('\t'))[0]) * magConst);


			column.ScanMagnificationTable.TableRemove(mag);
		}

		private void scanMagChangeBut_Click(object sender, EventArgs e)
		{
			int index = scanMagtableLb.SelectedIndex;
			if (index < 0)
			{
				MessageBox.Show("Table is not selected.");
				return;
			}

			object obj = new object();

			double magConst = (column.LensWDtable as SECcolumn.IMagCorrector).MagConstant;

			int mag = (int)Math.Floor(int.Parse((((string)scanMagtableLb.SelectedItem).Split('\t'))[0]) * magConst);

			column.ScanMagnificationTable.TableChange(mag, new object[]{
			    (int)(scanMagnificationNud.Value),
			    (double)(scanRatioXHcvd.ControlValue as SECtype.IControlDouble).Value,
			    (double)(scanRatioYHcvd.ControlValue as SECtype.IControlDouble).Value,
			    (int)(scanFeedbackIci.ControlValue as SECtype.IControlInt).Value});
		}

		private void scanMagMaxNud_ValueChanged(object sender, EventArgs e)
		{
			int maxMag = (int)scanMagMaxNud.Value;
			object obj = maxMag;

			column.ScanMagnificationTable.SetStyle((int)SECcolumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Set, ref obj);
		}

		private void scanMagXYSameCb_CheckedChanged(object sender, EventArgs e)
		{
			scanRatioYHcvd.Enabled = !scanMagXYSameCb.Checked;
		}

		private void scanRatioXHcvd_ValueChanged(object sender, EventArgs e)
		{
			if (scanMagXYSameCb.Checked)
			{
				(scanRatioYHcvd.ControlValue as SECtype.IControlDouble).Value = (scanRatioXHcvd.ControlValue as SECtype.IControlDouble).Value;
			}
		}

		private void scanMagValidateBut_Click(object sender, EventArgs e)
		{
			object obj =new object();

			column.ScanMagnificationTable.SetStyle((int)SECcolumn.EnumIControlTableSetStyle.Table_Validate, ref obj);
			ShowSplieTableError(obj);
		}
		#endregion

		private void ShowSplieTableError(object obj)
		{
			if (obj == null)
			{
				MessageBox.Show("Table Ok");
			}
			else
			{
				//StringBuilder sb = new StringBuilder();
				string[] strs = (string[])obj;

				//foreach (string st in strs)
				//{
				//    sb.AppendLine(st);
				//}

				Form fm = new Form();
				ListBox lb = new ListBox();
				lb.Dock = DockStyle.Fill;
				lb.Items.AddRange(strs);
				fm.Controls.Add(lb);
				fm.ShowDialog(this);
			}
		}

        private void stigWobble_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            switch (cb.Name)
            {
                case "stig_XWobble":
                    ((SECtype.IControlBool)column["StigXWobbleEnable"]).Value = cb.Checked;
                    break;
                case "stig_YWobble":
                    ((SECtype.IControlBool)column["StigYWobbleEnable"]).Value = cb.Checked;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void StigXDefault(object sender, EventArgs e)
        {
            stigXabHswd.Value = 0;
            stigXcdHswd.Value = 0;
        }

        private void StigYDefault(object sender, EventArgs e)
        {
            stigYabHswd.Value = 0;
            stigYcdHswd.Value = 0;
        }

        private void EDSSettingsSave()
        {


            Properties.Settings.Default.EDSHighSECL1 = Convert.ToInt32(EDSHighSECL1.Text);
            Properties.Settings.Default.EDSHighSECL2 = Convert.ToInt32(EDSHighSECL2.Text);
            Properties.Settings.Default.EDSHighSEPmt = Convert.ToInt32(EDSHighSEPmt.Text);
            Properties.Settings.Default.EDSHighSEClt = Convert.ToInt32(EDSHighSEClt.Text);

            Properties.Settings.Default.EDSHighBSECL1 = Convert.ToInt32(EDSHighBSECL1.Text);
            Properties.Settings.Default.EDSHighBSECL2 = Convert.ToInt32(EDSHighBSECL2.Text);
            Properties.Settings.Default.EDSHighBSEAmp = Convert.ToInt32(EDSHighBSEAmp.Text);

            Properties.Settings.Default.EDSLowBSECL1 = Convert.ToInt32(EDSLowBSECL1.Text);
            Properties.Settings.Default.EDSLowBSECL2 = Convert.ToInt32(EDSLowBSECL2.Text);
            Properties.Settings.Default.EDSLowBSEAmp = Convert.ToInt32(EDSLowBSEAmp.Text);
        }

        

        private void EDSSettingsLoad()
        {
            EDSHighSECL1.Text = Properties.Settings.Default.EDSHighSECL1.ToString();
            EDSHighSECL2.Text = Properties.Settings.Default.EDSHighSECL2.ToString();
            EDSHighSEPmt.Text = Properties.Settings.Default.EDSHighSEPmt.ToString();
            EDSHighSEClt.Text = Properties.Settings.Default.EDSHighSEClt.ToString();

            EDSHighBSECL1.Text = Properties.Settings.Default.EDSHighBSECL1.ToString();
            EDSHighBSECL2.Text = Properties.Settings.Default.EDSHighBSECL2.ToString();
            EDSHighBSEAmp.Text = Properties.Settings.Default.EDSHighBSEAmp.ToString();

            EDSLowBSECL1.Text = Properties.Settings.Default.EDSLowBSECL1.ToString();
            EDSLowBSECL2.Text = Properties.Settings.Default.EDSLowBSECL2.ToString();
            EDSLowBSEAmp.Text = Properties.Settings.Default.EDSLowBSEAmp.ToString();

        }

        private void CameraSettingsLoad()
        {
            CTLeft.Text = Properties.Settings.Default.CameraLeft.ToString();
            CTRight.Text = Properties.Settings.Default.CameraRight.ToString();
            CTWidth.Text = Properties.Settings.Default.CameraWidth.ToString();
            CTHight.Text = Properties.Settings.Default.CameraHight.ToString();
        }

        private void CTLeft_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraLeft = Convert.ToInt16(CTLeft.Text);
        }

        private void CTRight_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraRight = Convert.ToInt16(CTRight.Text);
        }

        private void CTWidth_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraWidth = Convert.ToInt16(CTWidth.Text);
        }

        private void CTHight_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraHight = Convert.ToInt16(CTHight.Text);
        }

        //private void alignStigWF_ValueChanged(object sender, EventArgs e)
        //{
        //    equip.ColumnStigXWF.Value = equip.ColumnStigYWF.Value = stigXfreqHswd.Value / 8d;
        //}

        //private void alignStigWA_ValueChanged(object sender, EventArgs e)
        //{
        //    equip.ColumnStigXWA.Value = equip.ColumnStigYWA.Value = stigXamplHswd.Value / 128d;
        //}

	}
}
