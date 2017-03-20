using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	/// <summary>
	/// ColumnValue interface
	/// </summary>
	public interface IColumnValue : SECtype.IValue
	{
		/// <summary>
		/// 주기적 읽기 동작에 의해 값을 읽었음을 알림.
		/// </summary>
		event ObjectArrayEventHandler RepeatUpdated;

		/// <summary>
		/// 값이 바뀌었음을 알림
		/// </summary>
		event EventHandler ValueChanged;
	}

	/// <summary>
	/// Controller에서 RepeatRead 이벤트를 알리기 위한 delegate
	/// </summary>
	/// <param name="sender">발생 객체</param>
	/// <param name="value">읽은 값</param>
	public delegate void ObjectArrayEventHandler(object sender, object[] value);
}
