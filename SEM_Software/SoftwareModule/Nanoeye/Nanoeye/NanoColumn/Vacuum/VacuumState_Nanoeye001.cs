using System;
using SEC.Nanoeye.NanoView;

namespace SEC.Nanoeye.NanoColumn.Vacuum
{
	internal class VacuumState_Nanoeye001 : ColumnInt
	{
		protected override void RepeatUpdate(bool enable)
		{
			UInt16 addr;

			if ( (readLower == MiniSEM_Devices.Noting) && (readUpper == MiniSEM_Devices.Noting) )
			{
				throw new NotSupportedException();
			}

			if ( _Viewer == null )
			{
				return;
			}

			if ( enable )
			{

				if ( rpeatUpdateLinked ) { return; }
				rpeatUpdateLinked = true;

				if ( readLower != MiniSEM_Devices.Noting )
				{
					addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}

				if ( readUpper != MiniSEM_Devices.Noting )
				{
					addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Get);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}
			}
			else
			{
				if ( rpeatUpdateLinked )
				{
					if ( readLower != MiniSEM_Devices.Noting )
					{
						addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					if ( readUpper != MiniSEM_Devices.Noting )
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
			case SEC.Nanoeye.NanoView.ErrorType.CRC:
			case SEC.Nanoeye.NanoView.ErrorType.RxFail:
			case SEC.Nanoeye.NanoView.ErrorType.StartByte:
			case SEC.Nanoeye.NanoView.ErrorType.TxFail:
				base.NanoviewRepose(datas, et);
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
			case 0x01:
			case 0x02:
			case 0x03:
				result[0] = "S-Pumping";
				break;
			case 0x40:
				result[0] = "Ready";
				break;
			case 0x80:
			case 0x81:
			case 0x82:
			case 0x83:
				result[0] = "S-Venting";
				break;
			case 0xC0:
				result[0] = "Air";
				break;
			}
			base.OnRepeatUpdated(result);
		}

		protected override object[] ReadInnder()
		{
			object[] returns = new object[1];

			if (_Viewer == null)
			{
				returns[0] = "Air";
			}
			else
			{
				ushort addr = 0;
				uint datas = 0;

				addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);

				byte[] result =  _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);


				NanoView.PacketFixed8Bytes.UnPacket(result, out addr, out datas);


				switch (datas)
				{
				case 0x00:
				case 0x01:
				case 0x02:
				case 0x03:
					returns[0] = "S-Pumping";
					break;
				case 0x40:
					returns[0] = "Ready";
					break;
				case 0x80:
				case 0x81:
				case 0x82:
				case 0x83:
					returns[0] = "S-Venting";
					break;
				case 0xC0:
					returns[0] = "Air";
					break;
				}
			}

			return returns;
		}
	}
}
