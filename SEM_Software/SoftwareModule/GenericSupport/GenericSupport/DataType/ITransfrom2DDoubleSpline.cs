using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	interface ITransfrom2DDoubleSpline : ITransform2DDouble, ITableContainner
	{
		bool IsHorizontalTableError { get; }
		bool IsVerticalTableError { get; }

		// 아래 이벤트는 ITableContainner로 가야 할텐데....
		// TableError 속성도 함께...
		/// <summary>
		/// Overflow, 또는 Underflow 상태가 변화함.
		/// </summary>
		event EventHandler TableErrorStateChanged;
	}
}
