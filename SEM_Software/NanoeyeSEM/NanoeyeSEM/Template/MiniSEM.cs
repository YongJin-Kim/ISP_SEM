using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM.Template
{
	class MiniSEM : ITemplate
	{
		SEC.Nanoeye.NanoColumn.IMiniSEM column;

		public MiniSEM()
		{
			column = SystemInfoBinder.Default.Nanoeye.Controller as SEC.Nanoeye.NanoColumn.IMiniSEM;
			
		}

		public void FocusReset()
		{
			throw new NotImplementedException();
		}

		public void WDChanged()
		{
			throw new NotImplementedException();
		}

		public SEC.GenericSupport.DataType.IControlBool ColumnHVenable { get { return column.HvEnable; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnHVGun { get { return column.HvElectronGun; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnHVFilament { get { return column.HvFilament; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnHVGrid { get { return column.HvGrid; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnHVPMT { get { return column.HvPmt; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnHVCLT { get { return column.HvCollector; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnScanRotation { get { return column.ScanRotation; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnDynamicFocus { get { return null; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnLensCL1 { get { return column.LensCondenser1; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnLensCL2 { get { return column.LensCondenser2; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLC { get { return column.LensObjectCoarse; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLF { get { return column.LensObjectFine; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnLensOLWE { get { return column.LensObjectWobbleEnable; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLWA { get { return column.LensObjectWobbleAmplitude; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLWF { get { return column.LensObjectWobbleFrequence; } }
		public SEC.GenericSupport.DataType.IValue ColumnWD { get { return null; } }
		

		public SEC.GenericSupport.DataType.IControlDouble ColumnStigXV { get { return column.StigX; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigXAB { get { return column.StigXab; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigXCD { get { return column.StigXcd; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnStigXWE { get { return column.StigXWobbleEnable; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigXWF { get { return column.StigXWobbleFrequence; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigXWA { get { return column.StigXWobbleAmplitude; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigYV { get { return column.StigY; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigYAB { get { return column.StigYab; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigYCD { get { return column.StigYcd; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnStigYWE { get { return column.StigYWobbleEnable; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigYWF { get { return column.StigYWobbleFrequence; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnStigYWA { get { return column.StigYWobbleAmplitude; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnStigSyncX { get { return column.StigSyncX; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnStigSyncY { get { return column.StigSyncY; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnGAX { get { return column.GunAlignX; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnGAY { get { return column.GunAlignY; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnBSX { get { return column.BeamShiftX; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnBSY { get { return column.BeamShiftY; } }
		public SEC.GenericSupport.DataType.IValue ColumnVacuumMode { get { return column.VacuumMode; } }
		public SEC.GenericSupport.DataType.IValue ColumnVacuumState { get { return column.VacuumState; } }

        public SEC.GenericSupport.DataType.IControlDouble ColumnBSEAmpC { get { return column.BSEAmpC; } }
        public SEC.GenericSupport.DataType.IControlDouble ColumnBSEAmpD { get { return column.BSEAmpD; } }
        public SEC.GenericSupport.DataType.IControlInt ColumnCamera { get { return column.VacuumCamera; } }


		#region Magnification
		public int MagLenghtGet()
		{
			List<Settings.MiniSEM.MicroscopeProfile> list = (SystemInfoBinder.Default.SetManager as Settings.MiniSEM.ManagerMiniSEM).columnProfiles;

			Settings.MiniSEM.MicroscopeProfile profile=null;

			foreach (Settings.MiniSEM.MicroscopeProfile mp in list)
			{
				if (mp.Alias == column.Name)
				{
					profile = mp;
					break;
				}
			}

			if (profile == null) { throw new InvalidOperationException(); }

			return profile.MagnificationSettings.Count;
		}

		public int MagInc(int nowIndex)
		{
			Settings.MiniSEM.MicroscopeProfile profile = FindProfile();

			if (profile == null)
			{
				Debug.WriteLine("Can't fined same profile.");
				return -1;
			}

			int index = nowIndex + 1;

			if (index >= profile.MagnificationSettings.Count)
			{
				Debug.WriteLine("Now magnification is last mag.", "Error");
				return -1;
			}

			return ChangeMagnification(profile.MagnificationSettings[index]);
		}

		public int MagDec(int nowIndex)
		{
			Settings.MiniSEM.MicroscopeProfile profile = FindProfile();

			if (profile == null)
			{
				Debug.WriteLine("Can't fined same profile.");
				return -1;
			}

			int index = nowIndex - 1;

			if (index <0)
			{
				Debug.WriteLine("Now magnification is last mag.", "Error");
				return -1;
			}

			return ChangeMagnification(profile.MagnificationSettings[index]);
		}

		public int MagChange(int targetIndex)
		{
			Settings.MiniSEM.MicroscopeProfile profile = FindProfile();

			if (targetIndex < 0)
			{
				Debug.WriteLine("Now magnification is last mag.", "Error");
				return -1;
			}
			if (targetIndex >= profile.MagnificationSettings.Count)
			{
				Debug.WriteLine("Now magnification is last mag.", "Error");
				return -1;
			}

			return ChangeMagnification(profile.MagnificationSettings[targetIndex]);
		}

		private int ChangeMagnification(string magStr)
		{
			string[] magInfo = magStr.Split(',');

			double magWidth = Convert.ToDouble(magInfo[1].Trim(new char[] { '\t', ' ' }));
			double magHeight = Convert.ToDouble(magInfo[2].Trim(new char[] { '\t', ' ' }));

			// 예전 설정 파일에는 FeedBack 모드 정보가 들어 있지 않다.
			int magFeedBack = 0;
			if (magInfo.Length == 4)
				magFeedBack = Convert.ToInt32(magInfo[3].Trim());

			if (column != null)
			{
				((SECtype.IControlDouble)column["ScanMagnificationX"]).Value = magWidth;
				((SECtype.IControlDouble)column["ScanMagnificationY"]).Value = magHeight;
				((SECtype.IControlInt)column["ScanFeedbackMode"]).Value = magFeedBack;
			}

			_Magnification = int.Parse(magInfo[0].Trim(new char[] { '\t', ' ' }));

			return _Magnification;
		}

		private Settings.MiniSEM.MicroscopeProfile FindProfile()
		{
			List<Settings.MiniSEM.MicroscopeProfile> list = (SystemInfoBinder.Default.SetManager as Settings.MiniSEM.ManagerMiniSEM).columnProfiles;

			Settings.MiniSEM.MicroscopeProfile profile=null;

			foreach (Settings.MiniSEM.MicroscopeProfile mp in list)
			{
				if (mp.Alias == column.Name)
				{
					profile = mp;
					break;
				}
			}
			return profile;
		}


		private int _Magnification = -1;
		public int Magnification
		{
			get { return _Magnification; }
		}
		#endregion

		#region ITemplate 멤버


		public SEC.GenericSupport.DataType.IValue ColumnVacuumLastError
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IValue ColumnVacuumResetCode
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}
