using System;
using System.Collections.Generic;
using System.Text;

namespace SEC.GUIelement.MeasuringTools
{
	internal enum ItemState
	{
		/// <summary>
		/// 마우스 이벤트에 대한 기본 상태입니다.
		/// </summary>
		Default,
		/// <summary>
		/// 마우스 이벤트에 대한 아이템 추가 상태입니다.
		/// </summary>
		ItemAppend,
		/// <summary>
		/// 마우스 이벤트에 대한 핸들 변경 상태입니다.
		/// </summary>
		ItemHandleUpdate,
		/// <summary>
		/// 마우스 이벤트에 대한 위치 변경 상태입니다.
		/// </summary>
		ItemLocationUpdate
	}
}
