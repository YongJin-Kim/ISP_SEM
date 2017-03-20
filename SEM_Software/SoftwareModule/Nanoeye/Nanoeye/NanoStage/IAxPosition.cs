using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage
{
	public interface IAxPosition : SECtype.IControlLong 
	{
		/// <summary>
		/// Value의 값이 유효한지를 표시 함.
		/// </summary>
		bool ErrorState { get; }

		/// <summary>
		/// ErrorState가 바뀌었음을 알림.
		/// </summary>
		event EventHandler ValueErrorOccured;

		void MoveOffset(long offset);
	}
}
