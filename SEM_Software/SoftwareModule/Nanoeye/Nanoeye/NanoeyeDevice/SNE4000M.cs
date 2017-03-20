using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeDevice
{
	class SNE4000M : NanoeyeFactory
	{
		public SNE4000M()
		{
			_Scanner = new NanoImage.ActiveScan();
			_Controller = new NanoColumn.Sem4000M();
			_ControllerCommunicator = new NanoView.NanoViewMasterSlave();
		}
	}
}
