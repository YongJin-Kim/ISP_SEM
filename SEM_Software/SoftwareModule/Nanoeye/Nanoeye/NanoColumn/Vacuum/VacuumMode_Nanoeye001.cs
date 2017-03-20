using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoColumn.Vacuum
{
	internal class VacuumMode_Nanoeye001 : ColumnInt
	{
		protected override void RepeatUpdate(bool enable)
		{
			UInt16 addr;

			if ((readLower == MiniSEM_Devices.Noting) && (readUpper == MiniSEM_Devices.Noting))
			{
				throw new NotSupportedException();
			}

			if (_Viewer == null)
			{
				return;
			}

			if (enable)
			{
				if (rpeatUpdateLinked) { return; }
				rpeatUpdateLinked = true;

				if (readLower != MiniSEM_Devices.Noting)
				{
					addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}

				if (readUpper != MiniSEM_Devices.Noting)
				{
					addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Get);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}
			}
			else
			{
				if (rpeatUpdateLinked)
				{
					if (readLower != MiniSEM_Devices.Noting)
					{
						addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					if (readUpper != MiniSEM_Devices.Noting)
					{
						addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Get);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					rpeatUpdateLinked = false;
				}
			}
		}

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
					case (UInt16)MiniSEM_DeviceType.Get:
						OnRepeatUpdated(new object[] { data });
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

		protected override void OnRepeatUpdated(object[] value)
		{
			object[] result = new object[1];
			uint state = (uint)(value[0]);
			switch (state)
			{
			case 0x00:
				result[0] = "HighVacuum";
				break;
			case 0x01:
				result[0] = "LowVacuum";
				break;
			}
			base.OnRepeatUpdated(result);
		}

		public override object[] Read
		{
			get
			{
				if (_Viewer == null) { return new object[1]; }

				ushort addr = 0;
				uint datas = 0;

				addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);

				byte[] result =  _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);


				NanoView.PacketFixed8Bytes.UnPacket(result, out addr, out datas);

				object[] returns = new object[1];

				switch (datas)
				{
				case 0x00:
					returns[0] = "HighVacuum";
					break;
				case 0x01:
					returns[0] = "LowVacuum";
					break;
				}

				return returns;
			}
		}
	}
}
