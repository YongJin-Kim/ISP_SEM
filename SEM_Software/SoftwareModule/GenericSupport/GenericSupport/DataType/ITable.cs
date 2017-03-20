using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ControlValue의 Table 형태
	/// </summary>
	public interface ITable : IValue, ITableContainner
	{
		/// <summary>
		/// 최저 인덱스
		/// </summary>
		int IndexMinimum { get; }
		/// <summary>
		/// 최고 인덱스
		/// </summary>
		int IndexMaximum { get; }

		/// <summary>
		/// 선택된 아이템 번호
		/// </summary>
		int SelectedIndex { get; set; }

		/// <summary>
		/// 선택된 아이템
		/// </summary>
		object SeletedItem { get; set; }

		/// <summary>
		/// 아이템의 갯수.
		/// </summary>
		int Length { get; }

		/// <summary>
		/// 값들을 가져 온다.
		/// </summary>
		/// <param name="index">값을 가져올 인덱스</param>
		/// <returns>값들</returns>
		object[] this[int index] { get; set; }

		/// <summary>
		/// 선택된 인덱스가 바뀌었음을 알림.
		/// </summary>
		event EventHandler SelectedIndexChanged;

		
		/// <summary>
		/// 특별 설정.
		/// </summary>
		/// <param name="index">설정 번호</param>
		/// <param name="value">설정 값.</param>
		void SetStyle(int index, ref object value);

	}

}
