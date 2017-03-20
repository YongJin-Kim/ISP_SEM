using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoColumn
{
	public interface IMagCorrector
	{
		event EventHandler MagConstantChanged;

		double MagConstant { get; }
	}
}
