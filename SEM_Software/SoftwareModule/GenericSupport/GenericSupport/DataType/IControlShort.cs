using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ContorlValue of short type
	/// </summary>
	public interface IControlShort :IValue
	{
		/// <summary>
		/// 값이 바뀌었음을 알림
		/// </summary>
		event EventHandler ValueChanged;

		/// <summary>
		/// Value의 정밀도
		/// </summary>
		double Precision { get; }


		/// <summary>
		/// 설정 가능 한 최대 값
		/// </summary>
		short DefaultMax { get; }
		/// <summary>
		/// 설정 가능 한 최소 값
		/// </summary>
		short DefaultMin { get; }
		/// <summary>
		/// 동작 가능한 최대 값
		/// </summary>
		short Maximum { get; set; }
		/// <summary>
		/// 동작 최소 값
		/// </summary>
		short Minimum { get; set; }
		/// <summary>
		/// offset
		/// </summary>
		short Offset { get; set; }
		/// <summary>
		/// 값
		/// </summary>
		short Value { get; set; }

	}
}
