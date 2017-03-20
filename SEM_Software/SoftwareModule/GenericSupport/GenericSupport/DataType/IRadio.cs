using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// 여러개의 Value 하나 이하의 Value 만 동작 함을 보증 한다.
	/// </summary>
	public interface IRadio : IValue
	{
		// 현재 등록 되어 있는 목록
		IList<IValue> Values { get; }

		/// <summary>
		/// IValue의 등록. 등록시 자동으로 Enable의 값에 false가 할당 된다.
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		bool Add(IValue con);
		bool Remove(IValue con);
	}
}
