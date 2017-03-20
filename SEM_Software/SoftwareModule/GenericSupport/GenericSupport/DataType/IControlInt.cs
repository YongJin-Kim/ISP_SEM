using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ControlVlaue의 Integer 형태
	/// </summary>
	public interface IControlInt : IValue
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
		int DefaultMax { get; }
		/// <summary>
		/// 설정 가능 한 최소 값
		/// </summary>
		int DefaultMin { get; }
		/// <summary>
		/// 동작 가능한 최대 값
		/// </summary>
		int Maximum { get; set; }
		/// <summary>
		/// 동작 최소 값
		/// </summary>
		int Minimum { get; set; }
		/// <summary>
		/// offset
		/// </summary>
		int Offset { get; set; }
		/// <summary>
		/// 값
		/// </summary>
		int Value { get; set; }

	}
}
