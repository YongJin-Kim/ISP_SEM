using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	internal interface IMasterObjet
	{
		void NanoviewRepose(byte[] datas, ErrorType et);

		string Name { get; }
	}
}
