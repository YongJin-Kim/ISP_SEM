using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECimage = SEC.Nanoeye.NanoImage;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	public partial class Scanner : Form
	{
		#region Proerty & Vairables
		private Settings.ISettingManager _SetManager = null;
		//private SECimage.IActiveScan _ActiveScan = null;
		#endregion

		public Scanner()
		{
			InitializeComponent();
			_SetManager = SystemInfoBinder.Default.SetManager;
			//_ActiveScan = SystemInfoBinder.Default.Nanoeye.Scanner;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			//itemListCombo.SelectedIndex = 0;
			itemListCombo.Items.Clear();
			itemListCombo.Items.AddRange(_SetManager.ScannerList());
			itemListCombo.SelectedIndex = 0;
		}

		private void itemListCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = _SetManager.ScannerLoad((string)itemListCombo.SelectedItem);

			ReCalculrate();
		}

		private void ReCalculrate()
		{
			
			SEC.Nanoeye.NanoImage.SettingScanner ss =(SEC.Nanoeye.NanoImage.SettingScanner)propertyGrid1.SelectedObject;

			fpsDisplayLab.Text = ss.FramePerSecond.ToString();
			spfDisplayLab.Text = ss.SecondPerFrame.ToString();
		}

		#region System Button

		private void ApplyBut_Click(object sender, EventArgs e)
		{
			System.Console.Beep();
			try
			{
				SystemInfoBinder.Default.Nanoeye.Scanner.ValidateSetting(propertyGrid1.SelectedObject as SECimage.SettingScanner);
                _SetManager.ScannerSave(propertyGrid1.SelectedObject as SECimage.SettingScanner);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}

			_SetManager.ScannerSave(propertyGrid1.SelectedObject as SECimage.SettingScanner);
		}

		private void okBut_Click(object sender, EventArgs e)
		{
			System.Console.Beep();
			_SetManager.ScannerSave(propertyGrid1.SelectedObject as SECimage.SettingScanner);
			this.Close();
		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			ReCalculrate();
		}

		private void InfoBut_Click(object sender, EventArgs e)
		{
			SystemInfoBinder.Default.Nanoeye.Scanner.ShowInformation(this);
		}
	}
}
