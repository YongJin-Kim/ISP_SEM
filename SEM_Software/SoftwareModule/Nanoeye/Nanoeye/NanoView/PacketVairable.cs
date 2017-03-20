using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	internal class PacketVairable : PackeBase
	{

		public override byte[] BytesReceive(byte data)
		{
			throw new NotImplementedException();
		}

		public override void BytesClear()
		{
			throw new NotImplementedException();
		}

		public override bool CheckSumValidate(byte[] datas)
		{
			throw new NotImplementedException();
		}
	}
}
