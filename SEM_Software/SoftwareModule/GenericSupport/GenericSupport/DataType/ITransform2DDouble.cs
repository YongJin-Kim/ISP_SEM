using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// 2차원 평면을 제어하는 두개의 실제 제어 값과 가상의 회전된 값간의 연동을 지원 한다.
	/// </summary>
	public interface ITransform2DDouble : IValue
	{
		IControlDouble HorizontalRotated { get; }
		IControlDouble HorizontalReal { get; }
		IControlDouble VerticalRotated { get; }
		IControlDouble VerticalReal { get; }
		IControlDouble Angle { get; }

		/// <summary>
		/// 수평 반전
		/// </summary>
		bool ReverseHorizontal { get; set; }
		/// <summary>
		/// 수직 반전
		/// </summary>
		bool ReverseVertical { get; set; }

		/// <summary>
		/// Value 계산 시 수평 객체의 값에 대한 상대 치
		/// </summary>
		double PrecisionHorizontal { get; }

		/// <summary>
		/// Value 계산 시 수직 객체의 값에 대한 상대 치
		/// </summary>
		double PrecisionVertical { get; }
	}
}
