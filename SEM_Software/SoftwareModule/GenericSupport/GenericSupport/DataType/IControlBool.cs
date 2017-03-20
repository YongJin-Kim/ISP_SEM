using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ControlValue의 Bool 형태
	/// </summary>
	public interface IControlBool : IValue
	{
		/// <summary>
		/// 값이 바뀌었음을 알림
		/// </summary>
		event EventHandler ValueChanged;


		/// <summary>
		/// 값.
		/// </summary>
		bool Value { get; set; }
	}


}
