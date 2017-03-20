using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Lens
{
	public interface IWDSplineObjBase : SECtype.IValue, SECtype.ITableContainner, IMagCorrector
	{
		/// <summary>
		/// WD 값이 바뀌었음을 알림.
		/// </summary>
		event EventHandler WorkingDistanceChagned;

		/// <summary>
		/// WD value
		/// </summary>
		int WorkingDistance { get; }
		/// <summary>
		/// 현재 Lens 값이 Table의 범위를 음의 방향으로 넘어 섰음을 나타냄.
		/// </summary>
		bool IsNegativeOverflow { get; }
		/// <summary>
		/// 현재 Lens 값이 Table의 범위를 양의 방향으로 넘어 섰음을 나타냄.
		/// </summary>
		bool IsPositiveOverflow { get; }
	}


}
