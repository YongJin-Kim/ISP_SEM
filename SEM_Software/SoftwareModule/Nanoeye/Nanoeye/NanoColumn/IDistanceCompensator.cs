using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoColumn
{
	public interface IDistanceCompensator
	{

		double Distance { get; set; }

		/// <summary>
		/// 현재 값이 Table의 범위를 음의 방향으로 넘어 섰음을 나타냄.
		/// </summary>
		bool IsNegativeOverflow { get; }
		/// <summary>
		/// 현재 값이 Table의 범위를 양의 방향으로 넘어 섰음을 나타냄.
		/// </summary>
		bool IsPositiveOverflow { get; }

		/// <summary>
		/// 거리가 바뀌었음을 알림.
		/// </summary>
		event EventHandler DistanceChanged;
	}
}
