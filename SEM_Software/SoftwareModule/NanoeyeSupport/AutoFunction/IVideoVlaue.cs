using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public interface IVideoVlaue
	{
        int Contrast { get; set; }
        int Brightness { get; set; }
        int Contrast2 { get; set; }
        int Brightness2 { get; set; }
	}
}
