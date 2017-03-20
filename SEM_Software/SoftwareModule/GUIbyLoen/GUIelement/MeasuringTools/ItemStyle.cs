using System;
using System.Collections.Generic;
using System.Text;

namespace SEC.GUIelement.MeasuringTools
{
	[Flags]
	public enum ItemStyle : int
	{
		DrawText = 0x01,

		/// <summary>
		/// 표시 없는 직선
		/// </summary>
		Line = 0x02,
		/// <summary>
		/// 표시 있는 직선
		/// </summary>
		Linear = 0x03,

		/// <summary>
		/// 표시 없는 다각형
		/// </summary>
		ClosePath = 0x04,
		/// <summary>
		/// 표시 있는 다각형
		/// </summary>
		Area = 0x05,

		/// <summary>
		/// 표시 없는 여러지점을 이은 직선
		/// </summary>
		OpenPath = 0x6,
		/// <summary>
		/// 표시 있는 여러 지점을 이은 직선
		/// </summary>
		Length = 0x7,

		/// <summary>
		/// 표시 없는 사각형
		/// </summary>
		Rectangle = 0x8,
		/// <summary>
		/// 표시 있는 사각형
		/// </summary>
		AreaSquare = 0x9,

		/// <summary>
		/// 표시 없는 타원
		/// </summary>
		Ellipse = 0x0A,
		/// <summary>
		/// 표시 있는 타원
		/// </summary>
		AreaEllipse = 0x0B,

		/// <summary>
		/// 표시 있는 각도
		/// </summary>
		Angle = 0x0C,

		/// <summary>
		/// 표시 있는 텍스트 상자
		/// </summary>
		TextBox = 0x0E,

		/// <summary>
		/// 평행선
		/// </summary>
		MarquiosScale = 0x0F,

		/// <summary>
		/// 화살표
		/// </summary>
		Arrow = 0x10,

		/// <summary>
		/// 하나 지우기
		/// </summary>
		DeleteOne = 0xF0,
		/// <summary>
		/// 전체 지우기
		/// </summary>
		DeleteAll = 0xF1,
        /// <summary>
        /// Point
        /// </summary>
        Point = 0x0D
	}
}
