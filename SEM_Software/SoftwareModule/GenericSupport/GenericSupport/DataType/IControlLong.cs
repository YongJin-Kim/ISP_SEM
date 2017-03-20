using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ControlVlaue의 Integer 형태
	/// </summary>
	public interface IControlLong : IValue
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
		long DefaultMax { get; }
		/// <summary>
		/// 설정 가능 한 최소 값
		/// </summary>
		long DefaultMin { get; }
		/// <summary>
		/// 동작 가능한 최대 값
		/// </summary>
		long Maximum { get; set; }
		/// <summary>
		/// 동작 최소 값
		/// </summary>
		long Minimum { get; set; }
		/// <summary>
		/// offset
		/// </summary>
		long Offset { get; set; }
		/// <summary>
		/// 값
		/// </summary>
		long Value { get; set; }

	}
}
