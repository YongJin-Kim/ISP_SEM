using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	internal abstract class PackeBase
	{
		public abstract byte[] BytesReceive(byte data);
		public abstract void BytesClear();
		public abstract bool CheckSumValidate(byte[] datas);

		public static byte MakeByteCRC(byte[] data, int start, int length)
		{
			byte result = 0xff;

			for ( int i = start ; i < length + start ; i++ )
			{
				result ^= data[i];
			}
			return result;
		}
	}
}
