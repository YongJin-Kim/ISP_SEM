using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public interface ITableContainner
	{
		/// <summary>
		/// Table이 바뀌었음을 알림.
		/// </summary>
		event EventHandler TableChanged;

		/// <summary>
		/// 테이블을 설정 한다.
		/// </summary>
		/// <param name="values"></param>
		void TableSet(object[,] values);

		/// <summary>
		/// 테이블을 가져 온다.
		/// </summary>
		/// <returns></returns>
		object[,] TableGet();

		/// <summary>
		/// 테이블에 값을 추가 한다.
		/// </summary>
		/// <param name="values"></param>
		void TableAppend(object[] values);

		/// <summary>
		/// 특정 키의 테이블 값을 삭제 한다.
		/// </summary>
		/// <param name="key"></param>
		void TableRemove(object key);

		/// <summary>
		/// 테이블의 특정 값을 변경 한다.
		/// </summary>
		/// <param name="preKey">기존 key</param>
		/// <param name="values">새로운 key 값의 배열</param>
		void TableChange(object preKey, object[] values);
	}
}
