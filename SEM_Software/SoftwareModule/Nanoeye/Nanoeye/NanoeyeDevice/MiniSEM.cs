using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeDevice
{
	class MiniSEM : NanoeyeFactory
	{
		public MiniSEM()
		{
			_Scanner = new NanoImage.ActiveScan();
			_Controller = new NanoColumn.SemMiniSEM();
			_ControllerCommunicator = new NanoView.NanoViewMasterSlave();
		}
	}
}
