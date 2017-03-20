using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;
using SECcolumn = SEC.Nanoeye.NanoColumn;

namespace SEC.Nanoeye.NanoeyeSEM.Template 
{
	interface ITemplate 
	{
		void FocusReset();

		void WDChanged();

		int Magnification { get; }

		int MagLenghtGet();

		// 아래 두개는 magChange 하나로 처리 가능하다.
		//int MagInc(int nowIndex);
		//int MagDec(int nowIndex);
		/// <summary>
		/// 배율을 변경한다.
		/// </summary>
		/// <param name="nowIndex">목표 index</param>
		/// <returns>배율 값</returns>
		int MagChange(int targetIndex);

		SECtype.IControlBool ColumnHVenable { get; }
		SECtype.IControlDouble ColumnHVGun { get; }
		SECtype.IControlDouble ColumnHVFilament { get; }
		SECtype.IControlDouble ColumnHVGrid { get; }
		SECtype.IControlDouble ColumnHVPMT { get; }
		SECtype.IControlDouble ColumnHVCLT { get; }
		SECtype.IControlDouble ColumnScanRotation { get; }
		SECtype.IControlDouble ColumnLensCL1 { get; }
		SECtype.IControlDouble ColumnLensCL2 { get; }
		SECtype.IControlDouble ColumnLensOLC { get; }
		SECtype.IControlDouble ColumnLensOLF { get; }
		SECtype.IControlBool ColumnLensOLWE { get; }
		SECtype.IControlDouble ColumnLensOLWA { get; }
		SECtype.IControlDouble ColumnLensOLWF { get; }
		SECtype.IValue ColumnWD { get; }
		SECtype.IControlDouble ColumnStigXV { get; }
		SECtype.IControlDouble ColumnStigXAB { get; }
		SECtype.IControlDouble ColumnStigXCD { get; }
		SECtype.IControlBool ColumnStigXWE { get; }
		SECtype.IControlDouble ColumnStigXWF { get; }
		SECtype.IControlDouble ColumnStigXWA { get; }
		SECtype.IControlDouble ColumnStigYV { get; }
		SECtype.IControlDouble ColumnStigYAB { get; }
		SECtype.IControlDouble ColumnStigYCD { get; }
		SECtype.IControlBool ColumnStigYWE { get; }
		SECtype.IControlDouble ColumnStigYWF { get; }
		SECtype.IControlDouble ColumnStigYWA { get; }
		SECtype.IControlBool ColumnStigSyncX { get; }
		SECtype.IControlBool ColumnStigSyncY { get; }
		SECtype.IControlDouble ColumnGAX { get; }
		SECtype.IControlDouble ColumnGAY { get; }
		SECtype.IControlDouble ColumnBSX { get; }
		SECtype.IControlDouble ColumnBSY { get; }
		SECtype.IControlDouble ColumnDynamicFocus { get; }
		

		SECtype.IValue ColumnVacuumMode { get; }
		SECtype.IValue ColumnVacuumState { get; }
		SECtype.IValue ColumnVacuumLastError { get; }
		SECtype.IValue ColumnVacuumResetCode { get; }


        SECtype.IControlDouble ColumnBSEAmpC { get; }
        SECtype.IControlDouble ColumnBSEAmpD { get; }

        SECtype.IControlInt ColumnCamera { get; }
	}
}
