	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM.Settings.MiniSEM
{
	class ManagerMiniSEM : ISettingManager
	{
		// "MiniSEM 은 SetManger에서는 Column의 경우 RunningSetting 만 관리 할 수 있다.
		// 따라서 MiniSEM Column 설정창에서는 다음의 List를 받아 직접 처리 한다.
		// Manager에서 Column의 Factory Setting을 관리 하는데 많은 어려움이 있음.

		/// <summary>
		/// 외부 Class에서 사용시 주의 할 것!!! 
		/// </summary>
		public List<MicroscopeProfile> columnProfiles = new List<MicroscopeProfile>();
		public List<ScanningProfile> scanProfiles = new List<ScanningProfile>();

		private bool _Inited = false;
		public bool Inited
		{
			get { return _Inited; }
		}

		public void Save(string fileName)
		{
			Properties.Settings.Default.NumberOfProfile = columnProfiles.Count;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				mp.Save();
			}

			foreach (ScanningProfile sp in scanProfiles)
			{
				sp.Save();
			}
		}

		public void Load(string fileName)
		{
			InitSetting();

			#region Microscope
			columnProfiles.Clear();
			for (int i = 0; i < Properties.Settings.Default.NumberOfProfile; i++)
			{
				columnProfiles.Add(new MicroscopeProfile());

			}
			foreach (MicroscopeProfile profile in columnProfiles)
			{
				profile.SettingsKey = "MicroscopeProfile" + columnProfiles.IndexOf(profile).ToString();
				profile.Reload();
				profile.Save();
			}
			#endregion

			#region Scanning
			scanProfiles.Clear();
			scanProfiles.Add(new ScanningProfile());	// 0 Fast Scan
			scanProfiles.Add(new ScanningProfile());	// 1 Slow Scan
			scanProfiles.Add(new ScanningProfile());	// 2 Fast Photo
			scanProfiles.Add(new ScanningProfile());	// 3 Slow Photo
			scanProfiles.Add(new ScanningProfile());	// 4 Auto Focus
			scanProfiles.Add(new ScanningProfile());	// 5 Auto Contrast & Brightness
			scanProfiles.Add(new ScanningProfile());	// 6 Auto Stig & Focus
			scanProfiles.Add(new ScanningProfile());	// 7 Scan Pause Mode
			scanProfiles.Add(new ScanningProfile());	// 8 Spot Mode
			scanProfiles.Add(new ScanningProfile());	// 9 Auto Focus 2

			foreach (ScanningProfile profile in scanProfiles)
			{
				profile.SettingsKey = "ScanningProfile" + scanProfiles.IndexOf(profile).ToString();
				profile.Reload();
				switch (scanProfiles.IndexOf(profile))
				{
				case 0:
					profile.Alias = "Fast Scan";
					break;
				case 1:
					profile.Alias = "Slow Scan";
					break;
				case 2:
					profile.Alias = "Fast Photo";
					break;
				case 3:
					profile.Alias = "Slow Photo";
					break;
				case 4:
					profile.Alias = "Auto Focus";
					break;
				case 5:
					profile.Alias = "Auto Contrast Brightness";
					break;
				case 6:
					profile.Alias = "Auto Stig Focus";
					break;
				case 7:
					profile.Alias = "Scan Pause";
					break;
				case 8:
					profile.Alias = "Spot Mode";
					break;
				case 9:
					profile.Alias = "Auto Focus 2";
					break;
				}
				profile.Save();
			}
			#endregion
			
		}

		public void InitSetting()
		{
			_Inited = true;
		}

		#region Scanner
		public SEC.Nanoeye.NanoImage.SettingScanner ScannerLoad(string name)
		{
			SEC.Nanoeye.NanoImage.SettingScanner ss = new SEC.Nanoeye.NanoImage.SettingScanner();

			ScanningProfile profile = null;

			foreach (ScanningProfile sp in scanProfiles)
			{
				if (sp.Alias == name)
				{
					profile = sp;
					break;
				}
			}

			if (profile == null)
			{
				throw new ArgumentException("Undefined Scanning Profile");
			}

			ss.Name = profile.Alias;
			ss.AiChannel = profile.VideoChannel;
			ss.AiClock = profile.SampleClock;
			ss.AiDifferential = profile.VideoDifferential;
			ss.AiMaximum = profile.DeflectionMaximum;
			ss.AiMinimum = profile.DeflectionMinimum;
			ss.AoClock = profile.SampleClock / 2;
			ss.AoMaximum = 10f;
			ss.AoMinimum = -10f;
			ss.AreaShiftX = profile.AreaChangeX;
			ss.AreaShiftY = profile.AreaChangeY;
			ss.AverageLevel = profile.MeanLevel;
			ss.BlurLevel = profile.BlurLevel;
			ss.FrameHeight = profile.FrameSize.Height;
			ss.FrameWidth = profile.FrameSize.Width;
			ss.ImageHeight = profile.ImageBounds.Height;
			ss.ImageLeft = profile.ImageBounds.Left;
			ss.ImageTop = profile.ImageBounds.Top;
			ss.ImageWidth = profile.ImageBounds.Width;
			ss.LineAverage = 1;
			ss.PaintHeight = profile.PaintBounds.Height / 600f * 0.75f;
			ss.PaintWidth = profile.PaintBounds.Width / 800f;
			ss.PaintX = profile.PaintBounds.X / 800f;
			ss.PaintY = profile.PaintBounds.Y / 600f * 0.75f;
			ss.PropergationDelay = profile.PropargationDelayX;
			ss.RatioX = profile.ScanRatioX;
			ss.RatioY = profile.ScanRatioY;
			ss.SampleComposite = profile.SampleComposite;
			ss.ShiftX = profile.ScanShiftX;
			ss.ShiftY = profile.ScanShiftY;

			return ss;
		}

		public void ScannerSave(SEC.Nanoeye.NanoImage.SettingScanner set)
		{
			SEC.Nanoeye.NanoImage.SettingScanner ss = new SEC.Nanoeye.NanoImage.SettingScanner();

			ScanningProfile profile = null;

			foreach (ScanningProfile sp in scanProfiles)
			{
				if (sp.Alias == set.Name)
				{
					profile = sp;
					break;
				}
			}

			if (profile == null)
			{
				profile = new ScanningProfile();
				scanProfiles.Add(profile);
				profile.SettingsKey = "ScanningProfile" + scanProfiles.IndexOf(profile).ToString();
				profile.Reload();
			}

			profile.Alias = ss.Name;
			profile.VideoChannel = ss.AiChannel;
			profile.SampleClock = ss.AiClock;
			profile.VideoDifferential = ss.AiDifferential;
			profile.DeflectionMaximum = ss.AiMaximum;
			profile.DeflectionMinimum = ss.AiMinimum;
			profile.AreaChangeX = (float)ss.AreaShiftX;
			profile.AreaChangeY = (float)ss.AreaShiftY;
			profile.MeanLevel = ss.AverageLevel;
			profile.BlurLevel = ss.BlurLevel;
			profile.FrameSize = new System.Drawing.Size(ss.FrameWidth, ss.FrameHeight);
			profile.ImageBounds = new System.Drawing.Rectangle(ss.ImageLeft, ss.ImageTop, ss.ImageWidth, ss.ImageHeight);
			profile.PaintBounds = new System.Drawing.Rectangle((int)ss.PaintX * 800,
																(int)(ss.PaintY * 600 / 0.75),
																(int)ss.PaintWidth * 800,
																(int)(ss.PaintHeight * 600 / 0.75));
			profile.PropargationDelayX = ss.PropergationDelay;
			profile.ScanRatioX = (float)ss.RatioX;
			profile.ScanRatioY = (float)ss.RatioY;
			profile.SampleComposite = ss.SampleComposite;
			profile.ScanShiftX = (float)ss.ShiftX;
			profile.ScanShiftY = (float)ss.ShiftY;

			profile.Save();
		}

		public string[] ScannerList()
		{
			List<string> list = new List<string>();

			foreach (ScanningProfile sp in scanProfiles)
			{
				list.Add(sp.Alias);
			}
			return list.ToArray();
		}
		#endregion

		#region Column
		public void ColumnCreate(string name)
		{
			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == name)
				{
					throw new ArgumentException("Already same name setting exist.");
				}
			}

			MicroscopeProfile profile = new MicroscopeProfile();
			profile.Alias = name;
			columnProfiles.Add(profile);
			profile.SettingsKey = "MicroscopeProfile" + columnProfiles.IndexOf(profile).ToString();
			profile.Reload();
			profile.Save();
		}

		public bool ColumnDelete(string name)
		{
			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == name)
				{
					profile = mp;
				}
			}
			if (profile == null) { return false; }
			else
			{
				columnProfiles.Remove(profile);
				return true;
			}
		}

		public string[] ColumnList()
		{
			List<string> list = new List<string>();

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				list.Add(mp.Alias);
			}

			return list.ToArray();
		}

		public void ColumnLoad(string name, SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode)
		{
			if (mode != ColumnOnevalueMode.Run)
			{
				throw new NotSupportedException("MiniSEM 은 SetManger에서는 RunningSetting 만 관리 할 수 있다.");
			}

			SEC.Nanoeye.NanoColumn.IMiniSEM mini = column as SEC.Nanoeye.NanoColumn.IMiniSEM;
            //SEC.Nanoeye.NanoColumn.I4000M mini = column as SEC.Nanoeye.NanoColumn.I4000M;

			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == name)
				{
					profile = mp;
				}
			}
			if (profile == null) { throw new ArgumentException("There is no same name setting."); }

			mini.Name = name;
			mini.HVtext = profile.EghvText;
			mini.BeamShiftAngle.Value = profile.BeamShiftRotation * mini.BeamShiftAngle.Precision;

			mini.GunAlignAngle.Value = profile.GunAlignRotation * mini.GunAlignAngle.Precision;
			mini.HvElectronGun.Value = profile.EgpsAccDefault * mini.HvElectronGun.Precision;

			mini.LensCondenser1Direction.Value = 0;
			mini.LensCondenser1WobbleAmplitude.Value = 0;
			mini.LensCondenser1WobbleEnable.Value = false;
			mini.LensCondenser1WobbleFrequence.Value = 0;
			mini.LensCondenser2Direction.Value = 0;
			mini.LensCondenser2WobbleAmplitude.Value = 0;
			mini.LensCondenser2WobbleEnable.Value = false;
			mini.LensCondenser2WobbleFrequence.Value = 0;

			mini.LensObjectDirection.Value = 0;

			mini.LensObjectWobbleAmplitude.Value = 0;
			mini.LensObjectWobbleEnable.Value = false;
			mini.LensObjectWobbleFrequence.Value = 0;
			mini.ScanAmplitudeX.Value = 0.707;
			mini.ScanAmplitudeY.Value = 0.707;
			mini.ScanRotation.Value = profile.ScanRotateOffset;
			mini.StigSyncX.Value = false;
			mini.StigSyncY.Value = false;

			mini.StigXWobbleAmplitude.Value = 0;
			mini.StigXWobbleEnable.Value = false;
			mini.StigXWobbleFrequence.Value = 0;

			mini.StigYWobbleAmplitude.Value = 0;
			mini.StigYWobbleEnable.Value = false;
			mini.StigYWobbleFrequence.Value = 0;

			mini.LensObjectFine.BeginInit();
			mini.LensObjectFine.Maximum = profile.Obj2Maximum * mini.LensObjectFine.Precision;
			mini.LensObjectFine.Minimum = profile.Obj2Minimum * mini.LensObjectFine.Precision;
			mini.LensObjectFine.Value = profile.Obj2Default * mini.LensObjectFine.Precision;
			mini.LensObjectFine.EndInit();

			mini.LensCondenser2.Value = profile.Con2Default * mini.LensCondenser2.Precision;

			mini.LensObjectCoarse.BeginInit();
			mini.LensObjectCoarse.Maximum = profile.Obj1Maximum * mini.LensObjectCoarse.Precision;
			mini.LensObjectCoarse.Minimum = profile.Obj1Minimum * mini.LensObjectCoarse.Precision;
			mini.LensObjectCoarse.Value = profile.WorkingDistance * mini.LensObjectCoarse.Precision;
			mini.LensObjectCoarse.EndInit();

			mini.LensCondenser1.Value = profile.SpotSize * mini.LensCondenser1.Precision;

			mini.HvGrid.Value = profile.EgpsGridRun * mini.HvGrid.Precision;
			mini.HvFilament.Value = profile.EgpsTipRun * mini.HvFilament.Precision;

			mini.HvCollector.Value = profile.Collector * mini.HvCollector.Precision;
			mini.HvPmt.Value = profile.Amplifier * mini.HvPmt.Precision;

			mini.BeamShiftX.Value = profile.BeamShiftX * mini.BeamShiftX.Precision;
			mini.BeamShiftY.Value = profile.BeamShiftY * mini.BeamShiftY.Precision;
			mini.GunAlignX.Value = profile.GunAlignX * mini.GunAlignX.Precision;
			mini.GunAlignY.Value = profile.GunAlignY * mini.GunAlignX.Precision;

			mini.StigX.Value = profile.StigX * mini.StigX.Precision;
			mini.StigXab.Value = profile.StigAlignXA * mini.StigXab.Precision;
			mini.StigXcd.Value = profile.StigAlignXB * mini.StigXcd.Precision;

			mini.StigY.Value = profile.StigY * mini.StigX.Precision;
			mini.StigYab.Value = profile.StigAlignYA * mini.StigYab.Precision;
			mini.StigYcd.Value = profile.StigAlignYB * mini.StigYcd.Precision;
		}

		public void ColumnSave(SEC.Nanoeye.NanoColumn.ISEMController column, ColumnOnevalueMode mode)
		{
			if (mode != ColumnOnevalueMode.Run)
			{
				throw new NotSupportedException("MiniSEM 은 SetManger에서는 RunningSetting 만 관리 할 수 있다.");
			}

			SEC.Nanoeye.NanoColumn.IMiniSEM mini = column as SEC.Nanoeye.NanoColumn.IMiniSEM;

			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == column.Name)
				{
					profile = mp;
				}
			}
			if (profile == null) { throw new ArgumentException("There is no same name setting."); }

			profile.WorkingDistance = (int)(mini.LensObjectCoarse.Value / mini.LensObjectCoarse.Precision);

			profile.SpotSize = (int)(mini.LensCondenser1.Value / mini.LensCondenser1.Precision);

			profile.EgpsGridRun = (int)(mini.HvGrid.Value / mini.HvGrid.Precision);
			profile.EgpsTipRun = (int)(mini.HvFilament.Value / mini.HvFilament.Precision);

			profile.Collector = (int)(mini.HvCollector.Value / mini.HvCollector.Precision);
			profile.Amplifier = (int)(mini.HvPmt.Value / mini.HvPmt.Precision);

			profile.BeamShiftX = (int)(mini.BeamShiftX.Value / mini.BeamShiftX.Precision);
			profile.BeamShiftY = (int)(mini.BeamShiftY.Value / mini.BeamShiftY.Precision);
			profile.GunAlignX = (int)(mini.GunAlignX.Value / mini.GunAlignX.Precision);
			profile.GunAlignY = (int)(mini.GunAlignY.Value / mini.GunAlignX.Precision);

			profile.StigX = (int)(mini.StigX.Value / mini.StigX.Precision);
			profile.StigAlignXA = (int)(mini.StigXab.Value / mini.StigXab.Precision);
			profile.StigAlignXB = (int)(mini.StigXcd.Value / mini.StigXcd.Precision);

			profile.StigY = (int)(mini.StigY.Value / mini.StigX.Precision);
			profile.StigAlignYA = (int)(mini.StigYab.Value / mini.StigYab.Precision);
			profile.StigAlignYB = (int)(mini.StigYcd.Value / mini.StigYcd.Precision);
		}

		public void ColumnOneSave(SECtype.IValue icv, ColumnOnevalueMode mode)
		{
			if (mode != ColumnOnevalueMode.Factory)
			{

				//throw new NotSupportedException("MiniSEM 은 SetManger에서는 RunningSetting 만 관리 할 수 있다.");
			}

			SECtype.IControlDouble icd = icv as SECtype.IControlDouble;

			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == (icd.Owner as SECtype.IController).Name)
				{
					profile = mp;
				}
			}
			if (profile == null) { throw new ArgumentException("There is no same name setting."); }

			switch (icv.Name)
			{
			case "HvElectronGun":
				if (mode == ColumnOnevalueMode.Factory) { profile.EgpsAccDefault = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvFilament":
				if (mode == ColumnOnevalueMode.Factory) { profile.EgpsTipDefault = (int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { profile.EgpsTipRun = (int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvGrid":
				if (mode == ColumnOnevalueMode.Factory) { profile.EgpsGridDefault=(int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { profile.EgpsGridRun=(int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvCollector":
				if (mode == ColumnOnevalueMode.Factory) { profile.EgpsCltDefault=(int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { profile.Collector=(int)(icd.Value/icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { profile.EACollector=(int)(icd.Value/icd.Precision); }
				break;
			case "HvPmt":
				if (mode == ColumnOnevalueMode.Factory) { profile.EgpsPmtDefault = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { profile.Amplifier = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { profile.EAAmplifier = (int)(icd.Value / icd.Precision); }
				break;
			case "LensCondenser1":
				if (mode == ColumnOnevalueMode.Factory)
				{
					profile.Con1Default = (int)(icd.Value / icd.Precision);
					profile.Con1Maximum = (int)(icd.Maximum / icd.Precision);
					profile.Con1Minimum = (int)(icd.Minimum / icd.Precision);
				}
				else if (mode == ColumnOnevalueMode.Run) { profile.SpotSize = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { profile.EACL1 = (int)(icd.Value / icd.Precision); }
				break;
			case "LensCondenser2":
				if (mode == ColumnOnevalueMode.Factory) { profile.Con2Default = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.Run) { }
				else if (mode == ColumnOnevalueMode.External) { profile.EACL2 = (int)(icd.Value / icd.Precision); }
				break;
			case "BeamShiftX":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.BeamShiftX = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "BeamShiftY":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.BeamShiftY = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigXab":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigAlignXA = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigXcd":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigAlignXB = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigYab":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigAlignYA = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigYcd":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigAlignYB = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "GunAlignX":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.GunAlignX = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "GunAlignY":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.GunAlignY = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigX":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigX = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigY":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { profile.StigY = (int)(icd.Value / icd.Precision); }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			}
		}

		public void ColumnOneLoad(SECtype.IValue icv, ColumnOnevalueMode mode)
		{
			if (mode != ColumnOnevalueMode.Factory)
			{
				throw new NotSupportedException("MiniSEM 은 SetManger에서는 RunningSetting 만 관리 할 수 있다.");
			}

			SECtype.IControlDouble icd = icv as SECtype.IControlDouble;

			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == (icv.Owner as SEC.Nanoeye.NanoColumn.ISEMController).Name)
				{
					profile = mp;
				}
			}
			if (profile == null) { throw new ArgumentException("There is no same name setting."); }

			switch (icv.Name)
			{
			case "HvElectronGun":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.EgpsAccDefault * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvFilament":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.EgpsTipDefault * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.EgpsTipRun * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvGrid":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.EgpsGridDefault * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.EgpsGridRun * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "HvCollector":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.EgpsCltDefault * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.Collector * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { icd.Value = profile.EACollector * icd.Precision; }
				break;
			case "HvPmt":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.EgpsPmtDefault * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.Amplifier * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { icd.Value = profile.EAAmplifier * icd.Precision; }
				break;
			case "LensCondenser1":
				icd.BeginInit();
				if (mode == ColumnOnevalueMode.Factory)
				{
					icd.Value = profile.Con1Default * icd.Precision;
					icd.Maximum = profile.Con1Maximum * icd.Precision;
					icd.Minimum = profile.Con1Minimum * icd.Precision;
				}
				else if (mode == ColumnOnevalueMode.Run)
				{
					icd.Value = profile.SpotSize * icd.Precision;
					icd.Maximum = profile.Con1Maximum * icd.Precision;
					icd.Minimum = profile.Con1Minimum * icd.Precision;
				}
				else if (mode == ColumnOnevalueMode.External)
				{
					icd.Value = profile.EACL1 * icd.Precision;
					icd.Maximum = icd.DefaultMax;
					icd.Minimum = icd.DefaultMin;
				}
				icd.EndInit();
				break;
			case "LensCondenser2":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.Con2Default * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.Con2Default * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { icd.Value = profile.EACL2 * icd.Precision; }
				break;
			case "BeamShiftX":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.BeamShiftX * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "BeamShiftY":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.BeamShiftY * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigXab":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigAlignXA * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigXcd":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigAlignXB * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigYab":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigAlignYA * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigYcd":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigAlignYB * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "GunAlignX":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.GunAlignX * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "GunAlignY":
				if (mode == ColumnOnevalueMode.Factory) { }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.GunAlignY * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigX":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigX * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			case "StigY":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = 0; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.StigY * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;

			case "LensObjectCoarse":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.Obj1Default * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run){icd.Value = profile.WorkingDistance * icd.Precision;}
				else if (mode == ColumnOnevalueMode.External){}
				break;
			case "LensObjectFine":
				if (mode == ColumnOnevalueMode.Factory) { icd.Value = profile.Obj2Default * icd.Precision; }
				else if (mode == ColumnOnevalueMode.Run) { icd.Value = profile.Obj2Default * icd.Precision; }
				else if (mode == ColumnOnevalueMode.External) { }
				break;
			}
		}
		#endregion

		public void GetSizesizeMode(int mode, out int cl1, out int cl2)
		{
			MicroscopeProfile profile = null;

			foreach (MicroscopeProfile mp in columnProfiles)
			{
				if (mp.Alias == SystemInfoBinder.Default.Nanoeye.Controller.Name)
				{
					profile = mp;
				}
			}
			if (profile == null) { throw new ArgumentException("There is no same name setting."); }

			switch (mode)
			{
			case 0:
				cl1 = profile.SS1CL1;
				cl2 = profile.SS1CL2;
				break;
			case 1:
				cl1 = profile.SS2CL1;
				cl2 = profile.SS2CL2;
				break;
			case 2:
				cl1 = profile.SS3CL1;
				cl2 = profile.SS3CL2;
				break;
			default:
				throw new ArgumentException();
			}
		}

		#region ISettingManager 멤버


		public void ColumnNameChagne(SEC.GenericSupport.DataType.IController column, string newName)
		{
			throw new NotImplementedException();
		}

        public void StageSave(string name)
        {
            throw new NotImplementedException();
        }

        public void StageLoad(string name)
        {
            throw new NotImplementedException();
        }

        public void StageCreate(string name)
        {
            throw new NotImplementedException();
        }

         

		#endregion
	}
}

