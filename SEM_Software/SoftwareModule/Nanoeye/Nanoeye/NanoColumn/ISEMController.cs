using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	/// <summary>
	/// SEM Column제어 객체의 기본 interface
	/// </summary>
	public interface ISEMController : SECtype.IController
	{
		/// <summary>
		/// Electron Gun 출력 전압 표시 문자열
		/// </summary>
		string HVtext { get; set; }

		/// <summary>
		/// Communication Object
		/// </summary>
		SEC.Nanoeye.NanoView.INanoView Viewer { get; set; }
	}
}
