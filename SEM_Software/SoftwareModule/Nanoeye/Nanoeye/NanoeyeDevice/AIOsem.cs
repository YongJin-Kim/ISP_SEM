using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeDevice
{
    class AIOsem : NanoeyeFactory
    {
        public AIOsem()
        {
            _Scanner = new NanoImage.ActiveScan();
            _Controller = new NanoColumn.AIOsem();
            _ControllerCommunicator = new NanoView.NanoViewMasterSlave();
        }
    }
}
