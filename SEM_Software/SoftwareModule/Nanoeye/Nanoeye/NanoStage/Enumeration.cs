using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoStage
{
	/// <summary>
	/// Limit Senser State 
	/// </summary>
	[Flags]
	public enum LimitSensorEnum : int
	{
		/// <summary>
		/// Normal
		/// </summary>
		Non = 0,
		/// <summary>
		/// Negative limit sensor actived.
		/// </summary>
		HW_Negative = 1,
		/// <summary>
		/// Positive limit sensor actived.
		/// </summary>
		HW_Positive = 2,
		/// <summary>
		/// Home sensor actived.
		/// </summary>
		HW_Home = 4,
		SW_Negative = 8,
		SW_Positive = 16,
		SW_Home = 32
	}
}
