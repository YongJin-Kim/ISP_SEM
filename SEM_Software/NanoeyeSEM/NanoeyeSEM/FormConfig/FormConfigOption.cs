using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using SEC.Nanoeye.NanoeyeSEM.Settings.SNE4000M;
using SEC.Nanoeye.NanoeyeSEM.Settings;
using System.IO;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	public partial class FormConfigOption : Form
	{
		private Manager4000M manager = new Manager4000M();
		private SettingData setting = new SettingData();
		

		public FormConfigOption()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			TextManager.Instance.DefineText(this);
			
			base.OnLoad(e);
		}


		private void SensBnt_Click(object sender, EventArgs e)
		{
			SenseFocusCoarseLeftNud.Value = 5;	
			SenseFocusCoarseRightNud.Value = 0.5m;

			SenseFocusFineLeftNud.Value = 5;
			SenseFocusFineRightNud.Value = 0.5m;

			SenseDetectorLeftNud.Value = 5;
			SenseDetectorRightNud.Value = 0.5m;

			SenseSpotLeftNud.Value = 5;
			SenseSpotRightNud.Value = 0.5m;

			SenseBeamshiftLeftNud.Value = 5;
			SenseBeamshiftRightNud.Value = 0.5m;

			SenseGunalignLeftNud.Value = 5;
			SenseGunalignRightNud.Value = 0.5m;

			SenseStigLeftNud.Value = 5;
			SenseStigRightNud.Value = 0.5m;
		}

		private void SysBnt_Click(object sender, EventArgs e)
		{
			this.Close();
		}



		public void OPTION_IE_SetImport_but_Click(object sender, EventArgs e)
		{
			string fileName = null;
		    using (OpenFileDialog dialog = new OpenFileDialog())
		    {
		        dialog.Filter = "NanoeyeSEM.bin(*.bin)|*.bin|NanoeyeSEM.config(*.config)|*.config";
		        dialog.AddExtension = true;
		        dialog.CheckFileExists = false;
		        dialog.DefaultExt = "*.bin";
		        dialog.FileName = "NanoeyeSEM.bin";

		        if (dialog.ShowDialog(this) == DialogResult.OK)
		        {
					fileName = dialog.FileName;

					MiniSEM.ActiveForm.Dispose();
					manager.Load(fileName);
					//manager.ColumnDelete(SystemInfoBinder.Default.SettingFileName);
					//manager.ColumnCreate(fileName);
					SystemInfoBinder.Default.SetManager.Load(fileName);
					


					manager.Save(SystemInfoBinder.Default.SettingFileName);
					//SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.Default.SettingFileName);


					MiniSEM.ActiveForm.Close();
					Application.Restart();

		        }


		    }
			
		}

		private void OPTION_IE_SetExport_but_Click(object sender, EventArgs e)
		{
			string fileName = null;
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "bin";
			dialog.Filter = "bin files (*.bin)|*.bin";
			dialog.FileName = "NanoeyeSEM.bin";
			if (dialog.ShowDialog(this) != DialogResult.OK)
			{
				dialog.Dispose();
				return;
			}

			

			fileName = dialog.FileName;
			SystemInfoBinder.Default.SetManager.Save(fileName);

			this.Close();
			//StreamWriter sw = new StreamWriter(fileName, false);
			
			//setting.WriteXml(sw);
			//sw.Flush();
			//sw.Dispose();
			
		}

		private void OPTION_IE_LogExport_But_Click(object sender, EventArgs e)
		{
			string fileName = null;
			OpenFileDialog OFD = new OpenFileDialog();
			OFD.AddExtension = true;
			OFD.DefaultExt = "log";
			OFD.Filter = "log files (*.log)|*.log";
			
			string logPath = Application.CommonAppDataPath + @".\Log";

			//logPath += "\\";
			//        logPath += DateTime.Now.Year.ToString("00") + "-";
			//logPath += DateTime.Now.Month.ToString("00") + "-";
			//logPath += DateTime.Now.Day.ToString("00");
			//logPath += ".log";

			OFD.InitialDirectory = logPath;
			

			if (OFD.ShowDialog(this) != DialogResult.OK)
			{
				OFD.Dispose();
				return;
			}


			SaveFileDialog dialog = new SaveFileDialog();
			
			dialog.AddExtension = true;
			dialog.DefaultExt = "log";
			dialog.Filter = "log files (*.log)|*.log";
			


			//string logPath = Application.CommonAppDataPath + @".\Log";

			//logPath += "\\";
			//logPath += DateTime.Now.Year.ToString("00") + "-";
			//logPath += DateTime.Now.Month.ToString("00") + "-";
			//logPath += DateTime.Now.Day.ToString("00");
			//logPath += ".log";

			//fileName = logPath;

			fileName += DateTime.Now.Year.ToString("00") + "-";
			fileName += DateTime.Now.Month.ToString("00") + "-";
			fileName += DateTime.Now.Day.ToString("00");
			fileName += ".log";

			//string SavePath = Application.StartupPath;

			
			dialog.InitialDirectory = Application.StartupPath;
			
			if (dialog.ShowDialog(this) != DialogResult.OK)
			{
				dialog.Dispose();
				return;
			}

			File.Copy(OFD.FileName, dialog.FileName);

			this.Close();
		}




	}
}
