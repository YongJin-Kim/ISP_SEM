using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	internal class PacketFixed8Bytes : PackeBase
	{
		int cnt = 0;
		byte[] datas = new byte[8];

		public override byte[] BytesReceive(byte data)
		{
			if ( cnt == 0 )
			{
				if ( data != 0x02 ) { return null; }	// 시작 바이트가 0x02가 아니면 무시 함.
			}

			datas[cnt++] = data;

			if ( cnt == 8 )
			{
				cnt = 0;
				byte[] result = datas;
				datas = new byte[8];
				return result;
			}
			return null;
		}

		public override void BytesClear()
		{
			cnt = 0;
		}

		public override bool CheckSumValidate(byte[] datas)
		{
			return VaildateChecSum(datas);
		}

		#region Static Method
		public static byte[] MakePacket(UInt16 addr, UInt32 data)
		{
			byte[] result = new byte[8];

			result[0] = 0x02;
			result[1] = (byte)(addr);
			result[2] = (byte)(addr >> 8);
			result[3] = (byte)(data);
			result[4] = (byte)(data >> 8);
			result[5] = (byte)(data >> 16);
			result[6] = (byte)(data >> 24);
			result[7] = MaketCheckSum(result);

			return result;
		}

		private static byte MaketCheckSum(byte[] result)
		{
			return MakeByteCRC(result, 1, 6);
		}

		public static  bool VaildateChecSum(byte[] datas)
		{
			if ( datas.Length != 8 )
			{
				throw new ArgumentException("'datas' length is not 8.");
			}

			return (datas[7] == MaketCheckSum(datas));
		}

		public static void UnPacket(byte[] datas, out UInt16 addr, out UInt32 data)
		{
			if (datas == null)
			{
				addr = 0;
				data = 0;
				return;
			}
			if ( datas.Length != 8 )
			{
				throw new ArgumentException("'datas' length is not 8.");
			}

			addr = (ushort)((UInt16)datas[1] | ((UInt16)datas[2] << 8));
			data = (UInt32)datas[3] | ((UInt32)datas[4] << 8) | ((UInt32)datas[5] << 16) | ((UInt32)datas[6] << 24);
		}
		#endregion
	}
}
