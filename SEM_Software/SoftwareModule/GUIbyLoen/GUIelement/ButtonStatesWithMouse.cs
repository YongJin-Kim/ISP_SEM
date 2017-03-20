using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SEC.GUIelement
{
	[Flags]
	public enum ButtonStatesWithMouse : short
	{
		/// <summary>
		/// 일반 상태
		/// </summary>
		Normal = 0,
		/// <summary>
		/// 버튼이 눌림
		/// </summary>
		ButtonPush = 1,
		/// <summary>
		/// 마우스가 버튼 위에 있음
		/// </summary>
		MouseHover = 2,
		/// <summary>
		/// 일반 상태에서 마우스가 버튼 위에 있음
		/// </summary>
		NormalHover = 0 | 2,
		/// <summary>
		/// 눌린 상태에서 마우스가 버튼 위에 있음
		/// </summary>
		PushHover = 1 | 2,
		/// <summary>
		/// 버튼이 사용 금지 됨.
		/// </summary>
		Disabled = 4
	}
}
