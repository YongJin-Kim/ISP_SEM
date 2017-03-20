using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeSEM.Template
{
	class SNE4000M : ITemplate
	{
		SEC.Nanoeye.NanoColumn.I4000M column;

		public SNE4000M()
		{
			column = SystemInfoBinder.Default.Nanoeye.Controller as SEC.Nanoeye.NanoColumn.I4000M;
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
		public SEC.GenericSupport.DataType.IControlDouble ColumnDynamicFocus { get { return column.ScanDynamicFocus; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnLensCL1 { get { return column.LensCondenser1; } }

		public SEC.GenericSupport.DataType.IControlDouble ColumnLensCL2 { get { return column.LensCondenser2; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLC { get { return column.LensObjectCoarse; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLF { get { return column.LensObjectFine; } }
		public SEC.GenericSupport.DataType.IControlBool ColumnLensOLWE { get { return column.LensObjectWobbleEnable; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLWA { get { return column.LensObjectWobbleAmplitude; } }
		public SEC.GenericSupport.DataType.IControlDouble ColumnLensOLWF { get { return column.LensObjectWobbleFrequence; } }
		public SEC.GenericSupport.DataType.IValue ColumnWD { get { return column.LensWDtable; } }

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
		
		public SEC.GenericSupport.DataType.IValue ColumnVacuumMode { get { throw new NotSupportedException(); } }
		public SEC.GenericSupport.DataType.IValue ColumnVacuumState { get { return column.VacuumState; } }
		public SEC.GenericSupport.DataType.IValue ColumnVacuumLastError { get { return column.VacuumLastError; } }
		public SEC.GenericSupport.DataType.IValue ColumnVacuumResetCode { get { return column.VacuumResetCode; } }

        public SEC.GenericSupport.DataType.IControlInt ColumnBSEAmp { get { return column.BSE_Amp; } }
        public SEC.GenericSupport.DataType.IControlDouble ColumnBSEAmpC { get { return column.BSE_AmpC; } }
        public SEC.GenericSupport.DataType.IControlDouble ColumnBSEAmpD { get { return column.BSE_AmpD; } }

        public SEC.GenericSupport.DataType.IControlInt ColumnCamera { get { return column.VacuumCamera; } }


        


		//private int[] magNumbers = new int[] { 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 30, 33, 36, 39, 42, 46, 50, 55, 60, 65, 71, 77, 84, 92 };
        private int[] magNumbers = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 
                                               40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 
                                               70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99};

		public int MagLenghtGet()
		{
			int length = 0;

			SEC.GenericSupport.DataType.ITable magTable = column["ScanMagSplineTable"] as SEC.GenericSupport.DataType.ITable;

			int magMin = magTable.IndexMinimum;
			// 10미만의 배율은 무시한다.
			magMin = Math.Max(magMin, 10);



			//int magMax = magTable.IndexMaximum;

            //신규 배율 셋팅
            object obj = null;
            column.ScanMagnificationTable.SetStyle((int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Scan_Mag_Maximum_Get, ref obj);
            int magMax = (int)obj;

            //System.Diagnostics.Trace.WriteLine("indexMaximum :" + magMax.ToString());

			int jarisu = (int)Math.Floor(Math.Log10(magMin));

			int magMinAlign = (int)(magMin / Math.Pow(10, jarisu - 1));

			int index;
			for (index = 0; index < magNumbers.Length; index++)
			{
				if (magMinAlign <= magNumbers[index]) { break; }
			}

			// 최소 자리수에서의 배율 갯수.
			length += magNumbers.Length - index;

			// 중간 자리수에서의 배율 갯수
			//length += ((int)Math.Floor(Math.Log10(magMax)) - jarisu - 1) * magNumbers.Length;
            length += ((int)Math.Floor(Math.Log10(magMax)) - jarisu - 1) * magNumbers.Length;

			jarisu = (int)Math.Floor(Math.Log10(magMax));
			int magMaxAlign = (int)(magMax / Math.Pow(10, jarisu - 1));

			for (index = 0; index < magNumbers.Length; index++)
			{
				if (magMaxAlign <= magNumbers[index]) { break; }
			}

			length += index;

			return length;
		}

		public int MagChange(int targetIndex)
		{
			SEC.GenericSupport.DataType.ITable magTable = column["ScanMagSplineTable"] as SEC.GenericSupport.DataType.ITable;

			int magMin = magTable.IndexMinimum;
			// 10미만의 배율은 무시한다.
			magMin = Math.Max(magMin, 10);

			int magMax = magTable.IndexMaximum;

			int jarisu = (int)Math.Floor(Math.Log10(magMin));

			int magMinAlign = (int)(magMin / Math.Pow(10, jarisu - 1));

			int minIndex;
			for (minIndex = 0; minIndex < magNumbers.Length; minIndex++)
			{
				if (magMinAlign <= magNumbers[minIndex]) { break; }
			}

			int targetTemp = targetIndex + minIndex;
			int pow=jarisu - 1;
            while (targetTemp >= magNumbers.Length)
            {
                pow++;
                targetTemp -= magNumbers.Length;
            }

			//int mag = (int)(Math.Pow(10, pow  ) * magNumbers[targetTemp - minIndex]);
			int mag = (int)(Math.Pow(10, pow) * magNumbers[targetTemp]);

			magTable.SelectedIndex = mag;

			_Magnification = mag;

			return mag;
		}

        //public int newMagChange(int targetIndex)
        //{

        //    SEC.GenericSupport.DataType.ITable magTable = column["ScanMagSplineTable"] as SEC.GenericSupport.DataType.ITable;

        //    int magMin = magTable.IndexMinimum;
        //    // 10미만의 배율은 무시한다.
        //    magMin = Math.Max(magMin, 10);

        //    int magMax = magTable.IndexMaximum;

        //    int jarisu = (int)Math.Floor(Math.Log10(magMin));

        //    int magMinAlign = (int)(magMin / Math.Pow(10, jarisu - 1));



        //    return mag;
        //}

		private int _Magnification = -1;
		public int Magnification
		{
			get { return _Magnification; }
		}

	}
}
