using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Vacuum
{
	class Pirani_NormalSEM_WSA : ColumnValueBase<double>, SECtype.IControlDouble
	{
		UInt32 repeatLowaer;
		UInt32 repeatUpper;

		/// <summary>
		/// 통신 객체에서 발생한 이벤트를 처리
		/// </summary>
		/// <param name="datas"></param>
		/// <param name="et"></param>
		public override void NanoviewRepose(byte[] datas, SEC.Nanoeye.NanoView.ErrorType et)
		{
			switch (et)
			{
			case SEC.Nanoeye.NanoView.ErrorType.NoResponse:
				OnNoResponse();
				return;

			case SEC.Nanoeye.NanoView.ErrorType.CRC:
			case SEC.Nanoeye.NanoView.ErrorType.RxFail:
			case SEC.Nanoeye.NanoView.ErrorType.StartByte:
			case SEC.Nanoeye.NanoView.ErrorType.TxFail:
				OnCommunicationError();
				return;

			case SEC.Nanoeye.NanoView.ErrorType.Non:
				{
					UInt16 addr;
					UInt32 data;

					NanoView.PacketFixed8Bytes.UnPacket(datas, out addr, out data);

					switch (addr & (UInt16)MiniSEM_DeviceParser.Type)
					{
					case (UInt16)MiniSEM_DeviceType.Set:
						CommunicationAck(data);
						break;
					case (UInt16)MiniSEM_DeviceType.Read:
						if ((addr & ((UInt16)MiniSEM_DeviceParser.Board | (UInt16)MiniSEM_DeviceParser.Inst)) == (UInt16)readLower)
						{
							repeatLowaer = data;
							OnRepeatUpdated(new object[] { repeatLowaer, repeatUpper });
						 }
						else if ((addr & ((UInt16)MiniSEM_DeviceParser.Board | (UInt16)MiniSEM_DeviceParser.Inst)) == (UInt16)readUpper)
						{
							repeatUpper = data;
							OnRepeatUpdated(new object[] { repeatLowaer, repeatUpper });
						}
						else
						{
							throw new InvalidOperationException("Invalid Address");
						}
						break;
					default:
						throw new InvalidOperationException("Undefined Operation");
					}
				}
				break;

			default:
				throw new ArgumentException("Undefined Error type");
			}
		}

		//int prePiraniValue = -100;

		protected override void OnRepeatUpdated(object[] value)
		{

			object[] result = new object[1];
			int val = (int)((UInt32)value[0]);

			//if (prePiraniValue != val)
			//{
			//    System.Diagnostics.Debug.WriteLine(val.ToString("X"), "Pirani_WSA");
			//    prePiraniValue = val;
			//}

			//int pow = (int)(val & 0xff);
			//if (pow == 0x20)
			//{
			//    pow = (int)((val >> 8) & 0xff) - 30 - 10;
			//}
			//else
			//{
			//    pow = (int)((val >> 8) & 0xff);
			//    pow -= 0x30;
			//    pow *= 10;

			//    pow += (int)((val) & 0xff) - 0x30;
			//    pow -= 10;

			//}

			//double gauge = (((val >> 24) & 0xff) - 0x30 + (((val >> 16) & 0xff) - 0x30) / 10d ) * Math.Pow(10, pow);
			//result[0] = gauge;


			if(val == -1) { result[0] = -1d; }

			result[0] = (double)val / 100000d;

			base.OnRepeatUpdated(result);
		}

		public override object[] Read
		{
			get
			{
				if (_Viewer != null)
				{
					ushort addr = 0;
					uint datas = 0;

					addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);

					byte[] result =  _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);


					NanoView.PacketFixed8Bytes.UnPacket(result, out addr, out datas);

					object[] returns = new object[1];

					returns[0] = (double)datas / 100000d;

					return returns;
				}
				else
				{
					return null;
				}
			}
		}

		public override void CommunicationAck(uint ackData)
		{
		
		}
	}
}
