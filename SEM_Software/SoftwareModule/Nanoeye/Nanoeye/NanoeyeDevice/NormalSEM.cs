using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeDevice
{
	class NormalSEM : NanoeyeFactory
	{
		public NormalSEM()
		{
			_Scanner = new NanoImage.ActiveScan();
			_Controller = new NanoColumn.SemNormalSEM();
			_ControllerCommunicator = new NanoView.NanoViewMasterSlave();
			_Stage = new NanoStage.SNE5000M.Stage5000M();
		}
	}
}
