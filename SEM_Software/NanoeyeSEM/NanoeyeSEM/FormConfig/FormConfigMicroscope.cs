using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using SEC.Nanoeye.NanoeyeSEM.Settings;

using SECtype = SEC.GenericSupport.DataType;
using SECcolumn = SEC.Nanoeye.NanoColumn;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	public partial class FormConfigMicroscope : Form, IMicroscopeSetupWindow
	{
		#region Property & Variables
		private SEC.Nanoeye.NanoColumn.IMiniSEM column = null;
		private Settings.MiniSEM.MicroscopeProfile profile = null;
		private Settings.MiniSEM.ManagerMiniSEM setManager = null;
		#endregion

		#region Event
		public event EventHandler ProfileListChanged;
		protected virtual void OnProfileListChanged()
		{
			if (ProfileListChanged != null)
			{
				ProfileListChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  HVtextChanged;
		protected virtual void OnHVtextChanged()
		{
			if (HVtextChanged != null) { HVtextChanged(this, EventArgs.Empty); }
		}
		#endregion

		#region Form 생성 및 제거 이벤트 및 생성자
		public FormConfigMicroscope()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			column = SystemInfoBinder.Default.Nanoeye.Controller as SEC.Nanoeye.NanoColumn.IMiniSEM;
			setManager = SystemInfoBinder.Default.SetManager as Settings.MiniSEM.ManagerMiniSEM;
			foreach (Settings.MiniSEM.MicroscopeProfile mp in setManager.columnProfiles)
			{
				if (mp.Alias == column.Name)
				{
					profile = mp;
					break;
				}
			}

			ColumnLink();

			SettingChanged();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (column != null)
			{
				ValidateMaxMin();

				switch (MessageBox.Show(this, "Profile is changed.\r\nDo you want to save?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
				{
				case DialogResult.Yes:
					ProfileSave();
					break;
				case DialogResult.No:
					break;
				case DialogResult.Cancel:
					e.Cancel = true;
					base.OnClosing(e);
					return;
				}

				//((SECtype.IControlDouble)column["LensCondenser1"]).BeginInit();
				column.LensCondenser1.BeginInit();
				column.LensCondenser1.Maximum = profile.Con1Maximum * ((SECtype.IControlDouble)column["LensCondenser1"]).Precision;
				column.LensCondenser1.Minimum = profile.Con1Minimum * ((SECtype.IControlDouble)column["LensCondenser1"]).Precision;
				column.LensCondenser1.EndInit();
				((SECtype.IControlDouble)column["LensObjectCoarse"]).BeginInit();
				((SECtype.IControlDouble)column["LensObjectCoarse"]).Maximum = profile.Obj1Maximum * ((SECtype.IControlDouble)column["LensObjectCoarse"]).Precision;
				((SECtype.IControlDouble)column["LensObjectCoarse"]).Minimum = profile.Obj1Minimum * ((SECtype.IControlDouble)column["LensObjectCoarse"]).Precision;
				((SECtype.IControlDouble)column["LensObjectCoarse"]).EndInit();
				((SECtype.IControlDouble)column["LensObjectFine"]).BeginInit();
				((SECtype.IControlDouble)column["LensObjectFine"]).Maximum = profile.Obj2Maximum * ((SECtype.IControlDouble)column["LensObjectFine"]).Precision;
				((SECtype.IControlDouble)column["LensObjectFine"]).Minimum = profile.Obj2Minimum * ((SECtype.IControlDouble)column["LensObjectFine"]).Precision;
				((SECtype.IControlDouble)column["LensObjectFine"]).EndInit();

				ColumnRelease();
			}
			base.OnClosing(e);
		}

		private void ValidateMaxMin()
		{
			if (lensCl1Hswd.Value < cl1MinNudicvd.Value)
			{
				MessageBox.Show(this, "Condenser Lens 1 value is smaller then minimum.\r\nMinimum will be chagned to value.");
				cl1MinNudicvd.Value = lensCl1Hswd.Value;
			}
			if (lensCl1Hswd.Value > cl1MaxNudicvd.Value)
			{
				MessageBox.Show(this, "Condenser Lens 1 value is larger then maximum.\r\nMaximum will be chagned to value.");
				cl1MaxNudicvd.Value = lensCl1Hswd.Value;
			}

			if (focusCoarseHswd.Value < olCoarseMinNudicvd.Value)
			{
				MessageBox.Show(this, " Focus Coarse value is smaller then minimum.\r\nMinimum will be changed to value.");
				olCoarseMinNudicvd.Value = focusCoarseHswd.Value;
			}
			if (focusCoarseHswd.Value > olCoarseMaxNudicvd.Value)
			{
				MessageBox.Show(this, " Focus Coarse value is larger then maximum.\r\nMaximum will be changed to value.");
				olCoarseMaxNudicvd.Value = focusCoarseHswd.Value;
			}

			if (focusFineHswd.Value < olFineMinNudicvd.Value)
			{
				MessageBox.Show(this, " Focus Fine value is smaller then minimum.\r\nMinimum will be changed to value.");
				olFineMinNudicvd.Value = focusFineHswd.Value;
			}
			if (focusFineHswd.Value > olFineMaxNudicvd.Value)
			{
				MessageBox.Show(this, " Focus Fine value is larger then maximum.\r\nMaximum will be changed to value.");
				olFineMaxNudicvd.Value = focusFineHswd.Value;
			}
		}
		#endregion

		public void SettingSave()
		{
			profile.Alias = m_Alias.Text;
			profile.EghvText = column.HVtext;
			profile.BeamShiftRotation = rotationBeamHswd.Value;
			profile.Con1Default = lensCl1Hswd.Value;
			profile.Con1Maximum = (int)cl1MaxNudicvd.Value;
			profile.Con1Minimum = (int)cl1MinNudicvd.Value;
			profile.Con2Default = (int)lensCl2Hswd.Value;
			profile.EgpsAccDefault = (int)avAnodeHswd.Value;
			profile.EgpsTipDefault = (int)avFilamentHswd.Value;
			profile.EgpsGridDefault = (int)avGridHswd.Value;
			profile.EgpsCltDefault = (int)detectorAmplifyHswd.Value;
			profile.EgpsPmtDefault = (int)detectorCollectorHswd.Value;
			profile.GunAlignRotation = (int)rotationGunHswd.Value;
			profile.Obj1Default = (int)focusCoarseHswd.Value;
			profile.Obj1Maximum = (int)olCoarseMaxNudicvd.Value;
			profile.Obj1Minimum = (int)olCoarseMinNudicvd.Value;
			profile.Obj2Default = (int)focusFineHswd.Value;
			profile.Obj2Maximum = (int)olFineMaxNudicvd.Value;
			profile.Obj2Minimum = (int)olFineMinNudicvd.Value;
			profile.ScanRotateOffset = (int)rotationScanHswd.Value;
			profile.SS1CL1 = (int)ss1CL1Nud.Value;
			profile.SS1CL2 = (int)ss1CL2Nud.Value;
			profile.SS2CL1 = (int)ss2CL1Nud.Value;
			profile.SS2CL2 = (int)ss2CL2Nud.Value;
			profile.SS3CL1 = (int)ss3CL1Nud.Value;
			profile.SS3CL2 = (int)ss3CL2Nud.Value;
		}

		public void SettingChanged()
		{
			EghvTextTB.Text = column.HVtext;
			m_Alias.Text = column.Name;
			profilenameLab.Text = column.Name;

			m_Alias.Text = profile.Alias;
			column.HVtext = profile.EghvText;
			rotationBeamHswd.Value = profile.BeamShiftRotation;
			lensCl1Hswd.Value = profile.Con1Default;

			cl1MaxNudicvd.Maximum = lensCl1Hswd.Maximum;
			cl1MaxNudicvd.Minimum = lensCl1Hswd.Minimum;
			cl1MaxNudicvd.Value = profile.Con1Maximum;

			cl1MinNudicvd.Maximum = lensCl1Hswd.Maximum;
			cl1MinNudicvd.Minimum = lensCl1Hswd.Minimum;
			cl1MinNudicvd.Value = profile.Con1Minimum;

			lensCl2Hswd.Value = profile.Con2Default;
			avAnodeHswd.Value = profile.EgpsAccDefault;
			avFilamentHswd.Value = profile.EgpsTipDefault;
			avGridHswd.Value = profile.EgpsGridDefault;
			detectorAmplifyHswd.Value = profile.EgpsCltDefault;
			detectorCollectorHswd.Value = profile.EgpsPmtDefault;
			rotationGunHswd.Value = profile.GunAlignRotation;

			focusCoarseHswd.Value = profile.Obj1Default;
			olCoarseMaxNudicvd.Maximum = focusCoarseHswd.Maximum;
			olCoarseMaxNudicvd.Minimum = focusCoarseHswd.Minimum;
			olCoarseMaxNudicvd.Value = profile.Obj1Maximum;
			olCoarseMinNudicvd.Maximum = focusCoarseHswd.Maximum;
			olCoarseMinNudicvd.Minimum = focusCoarseHswd.Minimum;
			olCoarseMinNudicvd.Value = profile.Obj1Minimum;

			focusFineHswd.Value = profile.Obj2Default;
			olFineMaxNudicvd.Maximum = focusFineHswd.Maximum;
			olFineMaxNudicvd.Minimum = focusFineHswd.Minimum;
			olFineMaxNudicvd.Value = profile.Obj2Maximum;
			olFineMinNudicvd.Maximum = focusFineHswd.Maximum;
			olFineMinNudicvd.Minimum = focusFineHswd.Minimum;
			olFineMinNudicvd.Value = profile.Obj2Minimum;

			rotationScanHswd.Value = profile.ScanRotateOffset;

			ss1CL1Nud.Maximum = ss2CL1Nud.Maximum = ss3CL1Nud.Maximum = lensCl1Hswd.Maximum;
			ss1CL1Nud.Minimum = ss2CL1Nud.Minimum = ss3CL1Nud.Minimum = lensCl1Hswd.Minimum;

			ss1CL2Nud.Maximum = ss2CL2Nud.Maximum = ss3CL2Nud.Maximum = lensCl2Hswd.Maximum;
			ss1CL2Nud.Minimum = ss2CL2Nud.Minimum = ss3CL2Nud.Minimum = lensCl2Hswd.Minimum;

			ss1CL1Nud.Value = profile.SS1CL1;
			ss1CL2Nud.Value = profile.SS1CL2;
			ss2CL1Nud.Value = profile.SS2CL1;
			ss2CL2Nud.Value = profile.SS2CL2;
			ss3CL1Nud.Value = profile.SS3CL1;
			ss3CL2Nud.Value = profile.SS3CL2;

			m_MagSelector.SuspendLayout();
			m_MagSelector.Items.Clear();
			foreach (string s in profile.MagnificationSettings)
			{
				m_MagSelector.Items.Add(s);
			}
			m_MagSelector.ResumeLayout();
		}

		private void ColumnLink()
		{
			avAnodeHswd.ControlValue = column["HvElectronGun"];
			avFilamentHswd.ControlValue = column["HvFilament"];
			avGridHswd.ControlValue = column["HvGrid"];

			detectorAmplifyHswd.ControlValue = column["HvPmt"];
			detectorCollectorHswd.ControlValue = column["HvCollector"];

			lensCl1Hswd.ControlValue = column["LensCondenser1"];
			cl1ReverseCbicvd.ControlValue = (SECtype.IControlInt)column["LensCondenser1Direction"];
			lensCl2Hswd.ControlValue = column["LensCondenser2"];
			cl2ReverseCbicvd.ControlValue = (SECtype.IControlInt)column["LensCondenser2Direction"];

			focusCoarseHswd.ControlValue = column["LensObjectCoarse"];
			olReverseCbicvd.ControlValue = (SECtype.IControlInt)column["LensObjectDirection"];
			focusFineHswd.ControlValue = column["LensObjectFine"];

			rotationScanHswd.ControlValue = column["ScanRotation"];
			rotationBeamHswd.ControlValue = column["BeamShiftRoation"];
			rotationGunHswd.ControlValue = column["GunAlignRotation"];
		}

		private void ColumnRelease()
		{
			AnodeLink(false);
			FilamentLink(false);
			GridLink(false);
			CollectorLink(false);
			AmplifierLink(false);

			column = null;
		}

		#region SystemButton & Profile Management
		private void SystemButton_Click(object sender, EventArgs e)
		{
			Button but = sender as Button;

			switch (but.Name)
			{
			case "btnApply":
				ProfileSave();
				break;
			case "btnRestore":
				ProfileRestore();
				return;
			case "btnClose":
				this.Close();
				this.Dispose();
				return;
			default:
				throw new NotSupportedException();
			}
		}

		private void ProfileRestore()
		{
			column["HvElectronGun"].BeginInit();
			column["HvFilament"].BeginInit();
			column["HvGrid"].BeginInit();
			column["HvPmt"].BeginInit();
			column["HvCollector"].BeginInit();
			column["LensCondenser1"].BeginInit();
			column["LensCondenser2"].BeginInit();
			column["LensObjectCoarse"].BeginInit();
			column["LensObjectFine"].BeginInit();
			column["StigXab"].BeginInit();
			column["StigXcd"].BeginInit();
			column["StigXWobbleFrequence"].BeginInit();
			column["StigXWobbleAmplitude"].BeginInit();
			column["StigYab"].BeginInit();
			column["StigYcd"].BeginInit();
			column["StigYWobbleFrequence"].BeginInit();
			column["StigYWobbleAmplitude"].BeginInit();
			column["ScanRotation"].BeginInit();
			column["BeamShiftRoation"].BeginInit();
			column["GunAlignRotation"].BeginInit();

			SECtype.IControlDouble icd;

			#region HV
			icd = (SECtype.IControlDouble)column["HvElectronGun"];
			icd.Value = profile.EgpsAccDefault * icd.Precision;
			icd = (SECtype.IControlDouble)column["HvFilament"];
			icd.Value = profile.EgpsTipDefault * icd.Precision;
			icd = (SECtype.IControlDouble)column["HvGrid"];
			icd.Value = profile.EgpsGridDefault * icd.Precision;
			icd = (SECtype.IControlDouble)column["HvPmt"];
			icd.Value = profile.EgpsPmtDefault * icd.Precision;
			icd = (SECtype.IControlDouble)column["HvCollector"];
			icd.Value = profile.EgpsCltDefault * icd.Precision;
			#endregion
			#region Lens
			icd = (SECtype.IControlDouble)column["LensCondenser1"];
			icd.Value = profile.Con1Default * icd.Precision;
			icd.Maximum = icd.DefaultMax;
			icd.Minimum = icd.DefaultMin;
			cl1MaxNudicvd.Maximum = cl1MinNudicvd.Maximum = (decimal)(icd.DefaultMax / icd.Precision);
			cl1MaxNudicvd.Minimum = cl1MinNudicvd.Minimum = (decimal)(icd.DefaultMin / icd.Precision);
			cl1MaxNudicvd.Value = profile.Con1Maximum;
			cl1MinNudicvd.Value = profile.Con1Minimum;
			icd = (SECtype.IControlDouble)column["LensCondenser2"];
			icd.Value = profile.Con2Default * icd.Precision;
			icd = (SECtype.IControlDouble)column["LensObjectCoarse"];
			icd.Value = profile.Obj1Default * icd.Precision;
			icd.Maximum = icd.DefaultMax;
			icd.Minimum = icd.DefaultMin;
			olCoarseMaxNudicvd.Maximum = olCoarseMinNudicvd.Maximum = (decimal)(icd.DefaultMax / icd.Precision);
			olCoarseMaxNudicvd.Minimum = olCoarseMinNudicvd.Minimum = (decimal)(icd.DefaultMin / icd.Precision);
			olCoarseMaxNudicvd.Value = profile.Obj1Maximum;
			olCoarseMinNudicvd.Value = profile.Obj1Minimum;
			icd = (SECtype.IControlDouble)column["LensObjectFine"];
			icd.Value = profile.Obj2Default * icd.Precision;
			icd.Maximum = icd.DefaultMax;
			icd.Minimum = icd.DefaultMin;
			olFineMaxNudicvd.Maximum = olFineMinNudicvd.Maximum = (decimal)(icd.DefaultMax / icd.Precision);
			olFineMaxNudicvd.Minimum = olFineMinNudicvd.Minimum = (decimal)(icd.DefaultMin / icd.Precision);
			olFineMaxNudicvd.Value = profile.Obj2Maximum;
			olFineMinNudicvd.Value = profile.Obj2Minimum;
			#endregion
			#region Stig Align
			icd = (SECtype.IControlDouble)column["StigXab"];
			icd.Value = profile.StigAlignXA * icd.Precision;
			icd = (SECtype.IControlDouble)column["StigXcd"];
			icd.Value = profile.StigAlignXB * icd.Precision;
			icd = (SECtype.IControlDouble)column["StigXWobbleFrequence"];
			icd.Value = 0;
			icd = (SECtype.IControlDouble)column["StigXWobbleAmplitude"];
			icd.Value = 0;
			icd = (SECtype.IControlDouble)column["StigYab"];
			icd.Value = profile.StigAlignYA * icd.Precision;
			icd = (SECtype.IControlDouble)column["StigYcd"];
			icd.Value = profile.StigAlignYB * icd.Precision;
			icd = (SECtype.IControlDouble)column["StigYWobbleFrequence"];
			icd.Value = 0;
			icd = (SECtype.IControlDouble)column["StigYWobbleAmplitude"];
			icd.Value = 0;
			#endregion
			#region Rotation
			// Rotation은 직접 사용
			icd = (SECtype.IControlDouble)column["ScanRotation"];
			icd.Offset = profile.ScanRotateOffset;
			icd = (SECtype.IControlDouble)column["BeamShiftRoation"];
			icd.Value = profile.BeamShiftRotation;
			icd = (SECtype.IControlDouble)column["GunAlignRotation"];
			icd.Value = profile.GunAlignRotation;
			#endregion

			ColumnEndInit(column["HvElectronGun"]);
			ColumnEndInit(column["HvFilament"]);
			ColumnEndInit(column["HvGrid"]);
			ColumnEndInit(column["HvPmt"]);
			ColumnEndInit(column["HvCollector"]);
			ColumnEndInit(column["LensCondenser1"]);
			ColumnEndInit(column["LensCondenser2"]);
			ColumnEndInit(column["LensObjectCoarse"]);
			ColumnEndInit(column["LensObjectFine"]);
			ColumnEndInit(column["StigXab"]);
			ColumnEndInit(column["StigXcd"]);
			ColumnEndInit(column["StigXWobbleFrequence"]);
			ColumnEndInit(column["StigXWobbleAmplitude"]);
			ColumnEndInit(column["StigYab"]);
			ColumnEndInit(column["StigYcd"]);
			ColumnEndInit(column["StigYWobbleFrequence"]);
			ColumnEndInit(column["StigYWobbleAmplitude"]);
			ColumnEndInit(column["ScanRotation"]);
			ColumnEndInit(column["BeamShiftRoation"]);
			ColumnEndInit(column["GunAlignRotation"]);

			m_MagSelector.SuspendLayout();
			m_MagSelector.Items.Clear();
			//foreach (string s in profile.MagnificationSettings)
			//{
			//    m_MagSelector.Items.Add(s);
			//}
			m_MagSelector.ResumeLayout();
		}

		private void ColumnEndInit(SECtype.IValue IValue)
		{
			SECtype.IControlDouble icd = IValue as SECtype.IControlDouble;
			try
			{
				icd.EndInit();
			}
			catch (ArgumentException)
			{
				Trace.WriteLine("Init Error in Microscope config.(" + icd.Name + ")");
				MessageBox.Show("Init Error - " + icd.Name);

				icd.BeginInit();
				icd.Maximum = icd.DefaultMax;
				icd.Minimum = icd.DefaultMin;
				icd.Offset = 0;
				icd.Value = 0;
				icd.EndInit();
			}
		}

		private void ProfileSave()
		{
			//  함수를 쓰면 오래 걸리므로 직접 나열함.

			profile.EgpsAccDefault = avAnodeHswd.Value;
			profile.EgpsTipDefault = avFilamentHswd.Value;
			profile.EgpsGridDefault = avGridHswd.Value;

			profile.EgpsPmtDefault = detectorAmplifyHswd.Value;
			profile.EgpsCltDefault = detectorCollectorHswd.Value;

			profile.Con1Default = lensCl1Hswd.Value;
			profile.Con1Maximum = (int)cl1MaxNudicvd.Value;
			profile.Con1Minimum = (int)cl1MinNudicvd.Value;
			profile.Con2Default = lensCl2Hswd.Value;

			profile.Obj1Default = focusCoarseHswd.Value;
			profile.Obj1Maximum = (int)olCoarseMaxNudicvd.Value;
			profile.Obj1Minimum = (int)olCoarseMinNudicvd.Value;
			profile.Obj2Default = focusFineHswd.Value;
			profile.Obj2Maximum = (int)olFineMaxNudicvd.Value;
			profile.Obj2Minimum = (int)olFineMinNudicvd.Value;

			profile.ScanRotateOffset = rotationScanHswd.Value;
			profile.BeamShiftRotation = rotationBeamHswd.Value;
			profile.GunAlignRotation = rotationGunHswd.Value;

			//// Rotation은 직접 사용
			profile.ScanRotateOffset = (int)((SECtype.IControlDouble)column["ScanRotation"]).Offset;
			profile.BeamShiftRotation = (int)((SECtype.IControlDouble)column["BeamShiftRoation"]).Value;
			profile.GunAlignRotation = (int)((SECtype.IControlDouble)column["GunAlignRotation"]).Value;

			profile.Save();

			System.Media.SystemSounds.Beep.Play();
		}

		private void profileCnt_Click(object sender, EventArgs e)
		{
			if (column.HvEnable.Value)
			{
				MessageBox.Show(
					"If Electric Gun is heated, Can't append or remove a profile.",
					this.Text.PadRight(48),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			switch (MessageBox.Show("After profile list is modified, This form will be closed!!!", "Attention", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
			{
			case DialogResult.OK:
				break;
			case DialogResult.Cancel:
				return;
			}

			if (sender == profileAppend)
			{
				Settings.MiniSEM.MicroscopeProfile profile = new Settings.MiniSEM.MicroscopeProfile();
				setManager.columnProfiles.Add(profile);
				profile.SettingsKey = "MicroscopeProfile" + setManager.columnProfiles.IndexOf(profile).ToString();
				profile.Reload();
				profile.Save();

			}
			else if (sender == profileRemove)
			{
				setManager.columnProfiles.Remove(profile);
				foreach (Settings.MiniSEM.MicroscopeProfile mp in setManager.columnProfiles)
				{
					mp.SettingsKey = "MicroscopeProfile" + setManager.columnProfiles.IndexOf(mp).ToString();
					mp.Save();
				}
			}

			OnProfileListChanged();

			this.Close();
			this.Dispose();
		}
		#endregion

		#region Profile Value Changed
		private void EghvTextTB_TextChanged(object sender, EventArgs e)
		{
			profile.EghvText = EghvTextTB.Text;
			column.HVtext = EghvTextTB.Text;
			OnHVtextChanged();
		}

		private void m_Alias_Leave(object sender, EventArgs e)
		{
			string preText = profilenameLab.Text;
			string newText = m_Alias.Text;

			column.Name = newText;
			profile.Alias = newText;
			profile.Save();
			OnProfileListChanged();
		}
		#endregion

		#region Magnificaion Function
		private void cbWDsame_CheckedChanged(object sender, EventArgs e)
		{
			m_MagHeight.Enabled = !cbWDsame.Checked;
		}

		private void m_MagSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = m_MagSelector.SelectedIndex;

			if (index > -1)
			{
				string[] items = profile.MagnificationSettings[index].Split(',');

				m_MagLevel.Value = Convert.ToInt32(items[0].Trim());
				m_MagWidth.Value = Convert.ToDecimal(items[1].Trim());
				m_MagHeight.Value = Convert.ToDecimal(items[2].Trim());
				m_MagFeedback.Value = Convert.ToDecimal(items[3].Trim());
			}
		}

		private void m_MagWidth_ValueChanged(object sender, EventArgs e)
		{
			int index = m_MagSelector.SelectedIndex;

			if (index > -1)
			{
				string value =
		            m_MagLevel.Value.ToString("0") + ",\t" +
					m_MagWidth.Value.ToString("F6") + ",\t" +
					m_MagHeight.Value.ToString("F6") + ",\t" +
					m_MagFeedback.Value.ToString("0");

				profile.MagnificationSettings[index] = value;
				m_MagSelector.Items[index] = value;

				((SECtype.IControlDouble)column["ScanMagnificationX"]).Value = (float)m_MagWidth.Value;
			}
			if (cbWDsame.Checked)
			{
				m_MagHeight.Value = m_MagWidth.Value;
			}
		}

		private void m_MagHeight_ValueChanged(object sender, EventArgs e)
		{
			int index = m_MagSelector.SelectedIndex;

			if (index > -1)
			{
				string value =
		            m_MagLevel.Value.ToString("0") + ",\t" +
					m_MagWidth.Value.ToString("F6") + ",\t" +
					m_MagHeight.Value.ToString("F6") + ",\t" +
					m_MagFeedback.Value.ToString("0");


				profile.MagnificationSettings[index] = value;
				m_MagSelector.Items[index] = value;

				((SECtype.IControlDouble)column["ScanMagnificationY"]).Value = (float)m_MagHeight.Value;
			}
		}

		private void m_MagFeedback_ValueChanged(object sender, EventArgs e)
		{
			int index = m_MagSelector.SelectedIndex;

			if (index > -1)
			{
				string value =
		            m_MagLevel.Value.ToString("0") + ",\t" +
					m_MagWidth.Value.ToString("F6") + ",\t" +
					m_MagHeight.Value.ToString("F6") + ",\t" +
					m_MagFeedback.Value.ToString("0");
				profile.MagnificationSettings[index] = value;
				m_MagSelector.Items[index] = value;

				((SECtype.IControlInt)column["ScanFeedbackMode"]).Value = (int)m_MagFeedback.Value;
			}
		}

		private void m_MagLevel_ValueChanged(object sender, EventArgs e)
		{
			int index = m_MagSelector.SelectedIndex;

			if (index > -1)
			{
				string value =
		            m_MagLevel.Value.ToString("0") + ",\t" +
					m_MagWidth.Value.ToString("F6") + ",\t" +
					m_MagHeight.Value.ToString("F6") + ",\t" +
					m_MagFeedback.Value.ToString("0");

				profile.MagnificationSettings[index] = value;
				m_MagSelector.Items[index] = value;
			}
		}

		private void m_Mag_Click(object sender, EventArgs e)
		{
			Button but = sender as Button;
			int index = m_MagSelector.SelectedIndex;
			if (but == m_MagAdd)
			{

				if (index > -1)
				{
					string newValue = "10,\t1.000000,\t1.000000,\t0";
					profile.MagnificationSettings.Insert(index, newValue);
					m_MagSelector.Items.Insert(index, newValue);
				}
			}
			else if (but == m_MagRemove)
			{
				if (index > -1)
				{
					profile.MagnificationSettings.RemoveAt(index);
					m_MagSelector.Items.RemoveAt(index);
				}
			}
			else if (but == m_MagUpper)
			{
				if (index > 0)
				{
					string value = profile.MagnificationSettings[index];

					m_MagSelector.Items.RemoveAt(index);
					m_MagSelector.Items.Insert(index - 1, value);

					profile.MagnificationSettings.RemoveAt(index);
					profile.MagnificationSettings.Insert(index - 1, value);
				}
			}
			else if (but == m_MagLower)
			{
				if (index < m_MagSelector.Items.Count - 1)
				{
					string value = profile.MagnificationSettings[index];

					m_MagSelector.Items.RemoveAt(index);
					m_MagSelector.Items.Insert(index + 1, value);

					profile.MagnificationSettings.RemoveAt(index);
					profile.MagnificationSettings.Insert(index + 1, value);
				}
			}
			profile.MagnificationSettings = profile.MagnificationSettings;
		}

		#endregion

		private void Function_Click(object sender, EventArgs e)
		{
			Button but = sender as Button;
			switch (but.Name)
			{
			case "fncWobbleBnt":
				SEC.Nanoeye.Support.Dialog.LensWobbleForm lwf = new SEC.Nanoeye.Support.Dialog.LensWobbleForm();

				lwf.Column = column;
				lwf.Show(this.Owner);
				break;
			case "fncHVlogBnt":
				SEC.Nanoeye.Support.Dialog.HVmoniter hvm = new SEC.Nanoeye.Support.Dialog.HVmoniter();
				hvm.Column = column;
				hvm.Show(this.Owner);
				break;
			case "fncMonitorBnt":
				SEC.Nanoeye.Support.Dialog.ColumnMonitor cm = new SEC.Nanoeye.Support.Dialog.ColumnMonitor();
				cm.Controller = column;
				cm.Show(this.Owner);
				break;
			default:
				throw new NotSupportedException("Undefined Button");
			}
		}

		#region HV Moniter
		private void anodeInfo_Click(object sender, EventArgs e)
		{
			if (anodeVmonLab.BackColor == Color.Black) { AnodeLink(true); }
			else { AnodeLink(false); }
		}

		private void AnodeLink(bool link)
		{
			if (column != null)
			{
				if ((anodeVmonLab.BackColor == Color.Black) && (link))
				{
					((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Anode_RepeatUpdated);
					anodeImonLab.BackColor = anodeVmonLab.BackColor = Color.Blue;
				}
				else if ((anodeVmonLab.BackColor == Color.Blue) && (!link))
				{
					((SECcolumn.IColumnValue)column["HvElectronGun"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Anode_RepeatUpdated);
					anodeImonLab.BackColor = anodeVmonLab.BackColor = Color.Black;
				}
			}
		}

		void Anode_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double emmition = (double)value[0];
				string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
				strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 3, false, 'A');

				double voltage = (double)value[1];
				string strVol = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(voltage, 0, 3, false, 'V');

				anodeVmonLab.Text = strVol;
				anodeImonLab.Text = strNew;
			};
			this.BeginInvoke(act);
		}

		private void filamentInfo_Click(object sender, EventArgs e)
		{
			if (tipVmonLab.BackColor == Color.Black) { FilamentLink(true); }
			else { FilamentLink(false); }
		}

		private void FilamentLink(bool link)
		{
			if (column != null)
			{
				if ((tipVmonLab.BackColor == Color.Black) && (link))
				{
					((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Filament_RepeatUpdated);
					tipImonLab.BackColor = tipVmonLab.BackColor = Color.Blue;
				}
				else if ((tipVmonLab.BackColor == Color.Blue) && (!link))
				{
					((SECcolumn.IColumnValue)column["HvFilament"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Filament_RepeatUpdated);
					tipImonLab.BackColor = tipVmonLab.BackColor = Color.Black;
				}
			}
		}

		void Filament_RepeatUpdated(object sender, object[] value)
		{
			Action act=() =>
			{
				double emmition = (double)value[0];
				string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
				strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 3, false, 'A');

				double voltage = (double)value[1];
				string strVol = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(voltage, 0, 3, false, 'V');

				tipVmonLab.Text = strVol;
				tipImonLab.Text = strNew;
			};
			this.BeginInvoke(act);
		}

		private void gridInfo_Click(object sender, EventArgs e)
		{
			if (gridVmonLab.BackColor == Color.Black) { GridLink(true); }
			else { GridLink(false); }
		}

		private void GridLink(bool link)
		{
			if (column != null)
			{
				if ((gridVmonLab.BackColor == Color.Black) && (link))
				{
					((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
					gridVmonLab.BackColor = Color.Blue;
					gridImonLab.BackColor = Color.Blue;
				}
				else if ((gridVmonLab.BackColor == Color.Blue) && (!link))
				{
					((SECcolumn.IColumnValue)column["HvGrid"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Grid_RepeatUpdated);
					gridVmonLab.BackColor = Color.Black;
					gridImonLab.BackColor = Color.Black;
				}
			}
		}

		void Grid_RepeatUpdated(object sender, object[] value)
		{
			Action act=() =>
			{
				double emmition = (double)value[0];
				string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
				strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 3, false, 'A');

				double voltage = (double)value[1];
				string strVol = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(voltage, 0, 3, false, 'V');

				gridVmonLab.Text = strVol;
				gridImonLab.Text = strNew;
			};
			this.BeginInvoke(act);
		}

		void collecotrInfo_Click(object sender, EventArgs e)
		{
			if (collectorVmonLab.BackColor == Color.Black) { CollectorLink(true); }
			else { CollectorLink(false); }
		}

		private void CollectorLink(bool link)
		{
			if (column != null)
			{
				if ((collectorVmonLab.BackColor == Color.Black) && (link))
				{
					((SECcolumn.IColumnValue)column["HvCollector"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Collector_RepeatUpdated);
					collectorImonLab.BackColor = collectorVmonLab.BackColor = Color.Blue;
				}
				else if ((collectorVmonLab.BackColor == Color.Blue) && (!link))
				{
					((SECcolumn.IColumnValue)column["HvCollector"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Collector_RepeatUpdated);
					collectorImonLab.BackColor = collectorVmonLab.BackColor = Color.Black;
				}
			}
		}

		void Collector_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double emmition = (double)value[0];
				string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
				strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 3, false, 'A');

				double voltage = (double)value[1];
				string strVol = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(voltage, 0, 3, false, 'V');

				collectorVmonLab.Text = strVol;
				collectorImonLab.Text = strNew;
			};
			this.BeginInvoke(act);
		}

		private void amplifierInfo_Click(object sender, EventArgs e)
		{
			if (amplifierVmonLab.BackColor == Color.Black) { AmplifierLink(true); }
			else { AmplifierLink(false); }
		}

		private void AmplifierLink(bool link)
		{
			if (column != null)
			{
				if ((amplifierVmonLab.BackColor == Color.Black) && (link))
				{
					((SECcolumn.IColumnValue)column["HvPmt"]).RepeatUpdated += new SECcolumn.ObjectArrayEventHandler(Amplifier_RepeatUpdated);
					amplifierImonLab.BackColor = amplifierVmonLab.BackColor = Color.Blue;
				}
				else if ((amplifierVmonLab.BackColor == Color.Blue) && (!link))
				{
					((SECcolumn.IColumnValue)column["HvPmt"]).RepeatUpdated -= new SECcolumn.ObjectArrayEventHandler(Amplifier_RepeatUpdated);
					amplifierImonLab.BackColor = amplifierVmonLab.BackColor = Color.Black;
				}
			}
		}

		void Amplifier_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				double emmition = (double)value[0];
				string strNew;// = SEC.MathematicsSupport.NumberConverter.ToUnitString(emmition, 1, false) + "uA";
				strNew = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(emmition, -6, 3, false, 'A');

				double voltage = (double)value[1];
				string strVol = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(voltage, 0, 3, false, 'V');

				amplifierVmonLab.Text = strVol;
				amplifierImonLab.Text = "-" + strNew;
			};
			this.BeginInvoke(act);
		}
		#endregion

		private void clNudicvd_ValueChanged(object sender, EventArgs e)
		{
			if (sender == cl1MaxNudicvd)
			{
				profile.Con1Maximum = (int)cl1MaxNudicvd.Value;
			}
			else if (sender == cl1MinNudicvd)
			{
				profile.Con1Minimum = (int)cl1MinNudicvd.Value;
			}
			else
			{
				throw new ArgumentException();
			}
		}

		private void Focus_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown nud = sender as NumericUpDown;
			switch (nud.Name)
			{
			case "olCoarseMaxNudicvd":
				profile.Obj1Maximum = (int)olCoarseMaxNudicvd.Value;
				break;
			case "olCoarseMinNudicvd":
				profile.Obj1Minimum = (int)olCoarseMinNudicvd.Value;
				break;
			case "olFineMaxNudicvd":
				profile.Obj2Maximum = (int)olFineMaxNudicvd.Value;
				break;
			case "olFineMinNudicvd":
				profile.Obj2Minimum = (int)olFineMinNudicvd.Value;
				break;
			default:
				throw new ArgumentException();
			}
		}
	}
}
