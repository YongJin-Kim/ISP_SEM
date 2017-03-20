using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM.Settings.MiniSEM
{


	// This class allows you to handle specific events on the settings class:
	//  The SettingChanging event is raised before a setting's value is changed.
	//  The PropertyChanged event is raised after a setting's value is changed.
	//  The SettingsLoaded event is raised after the setting values are loaded.
	//  The SettingsSaving event is raised before the setting values are saved.
	internal sealed partial class ScanningProfile
	{
		public ScanningProfile()
		{
			// 설정 파일을 사용자 저장소로 보냅니다.
			string configPath =
				ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

			string exePath = SystemInfoBinder.Default.SettingFileName;

			if (File.Exists(exePath))
			{
				FileInfo info = new FileInfo(configPath);

				if (!Directory.Exists(info.DirectoryName))
				{
					Directory.CreateDirectory(info.DirectoryName);
				}

				File.Copy(exePath, configPath, true);
			}
		}

		public override void Save()
		{
			base.Save();

			// 설정 파일을 사용자 저장소에서 가져옵니다.
			string configPath =
				ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

			string exePath = SystemInfoBinder.Default.SettingFileName;

			if (File.Exists(configPath))
			{
				File.Copy(configPath, exePath, true);
			}
		}
	}
}
