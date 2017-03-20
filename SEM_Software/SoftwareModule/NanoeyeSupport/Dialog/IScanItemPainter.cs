using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.Support.Controls
{
	interface IScanItemPainter
	{
		bool EventLink(SEC.Nanoeye.NanoImage.IScanItemEvent eventSI, string name);
		bool EventRelease();
	}
}
