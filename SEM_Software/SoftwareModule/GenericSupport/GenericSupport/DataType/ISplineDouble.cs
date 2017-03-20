using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public interface ISplineDouble : ITableContainner, IControlDouble
	{
		/// <summary>
		/// Spline 변환이 적용된 값을 전달할 Control
		/// </summary>
		IControlDouble RealControl { get; }

		bool TableError { get; }

		event EventHandler TableErrorChanged;
	}
}
