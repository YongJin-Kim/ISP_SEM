using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Vacuum
{
	class VacuumState_Carryer : ColumnValueBase<int>, SECtype.IControlInt
	{
		protected override void RepeatUpdate(bool enable)
		{
			UInt16 addr;

			if ((readLower == MiniSEM_Devices.Noting) && (readUpper == MiniSEM_Devices.Noting)) { throw new NotSupportedException(); }

			if (_Viewer == null) { return; }

			if (enable)
			{

				if (rpeatUpdateLinked) { return; }
				rpeatUpdateLinked = true;

				if (readLower != MiniSEM_Devices.Noting)
				{
					addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Read);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}

				if (readUpper != MiniSEM_Devices.Noting)
				{
					addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Read);
					_Viewer.RepeatUpdateAdd(this, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), addr);
				}
			}
			else
			{
				if (rpeatUpdateLinked)
				{
					if (readLower != MiniSEM_Devices.Noting)
					{
						addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Read);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					if (readUpper != MiniSEM_Devices.Noting)
					{
						addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Read);
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
					case (UInt16)MiniSEM_DeviceType.Read:
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
			StateToString(result, state);
			base.OnRepeatUpdated(result);
		}

		public override object[] Read
		{
			get
			{
				ushort addr = 0;
				uint datas = 0;

				addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Get);

				byte[] result =  _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);


				NanoView.PacketFixed8Bytes.UnPacket(result, out addr, out datas);

				object[] returns = new object[1];

				StateToString(returns, datas);

				return returns;
			}
		}

		private static void StateToString(object[] result, uint state)
		{
			switch (state)
			{
			case 0x00:
				result[0] = "Air";
				break;
			case 0x10:
				result[0] = "System Venting";
				break;
			case 0x20:
				result[0] = "System Pumping";
				break;
			case 0x30:
				result[0] = "Exchange";
				break;
			case 0x40:
				result[0] = "Chamber Venting";
				break;
			case 0x50:
				result[0] = "Chamber Pumping";
				break;
			case 0x60:
				result[0] = "Observation Ready";
				break;
			case 0xff:
				result[0] = "Error";
				break;
			default:
				throw new ArgumentException("Undefined Vacuum State");
			}
		}


		bool innerUpdateSync = false;
		bool innerUpdateCommunicationAck = false;
		public override int Value
		{
			get { return base.Value; }
			set
			{
				base.Value = value;

				if(!(innerUpdateSync | innerUpdateCommunicationAck))
				{
					if((_Viewer != null) && _IsInited && _Enable)
					{
						uint viwerSet;
						switch(value)
						{
						case 0:
							// Turobo Off는 PC S/W에서는 지원 하지 않음.
							throw new NotSupportedException();
						case 1:
							viwerSet = 0x30;
							break;
						case 2:
							viwerSet = 0x60;
							break;
						default:
							throw new ArgumentException();
						}
						ushort addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Set);

						_Viewer.Send( this, addr, NanoView.PacketFixed8Bytes.MakePacket( addr, viwerSet ), false );
					}
				}
			}
		}

		public override void Sync()
		{
			if(!_Enable) {
				throw new InvalidOperationException( "This is not enabled." ); 
			}
			uint data = base.GetDeviceValue();
			innerUpdateSync = true;
			switch(data & 0xF0)
			{
			case 0x00:
			case 0x10:
			case 0x20:
				this.Value = 0;
				break;
			case 0x30:
			case 0x40:
			case 0x50:
				this.Value = 1;
				break;
			case 0x60:
				this.Value = 2;
				break;
			case 0xff:
				this.Value = -1;
				break;
			default:
				throw new ArgumentException();

			}
			innerUpdateSync = false;
		}

		public override bool Validate()
		{
			if(!_Enable) {
				throw new InvalidOperationException( "This is not enabled." );
			}
			uint data = base.GetDeviceValue();
			switch(data)
			{
			case 0x00:
			case 0x10:
			case 0x20:
				return (this.Value == 0);
			case 0x30:
			case 0x40:
			case 0x50:
				return (this.Value == 1);
			case 0x60:
				return (this.Value == 2);
			case 0xff:
				return (this.Value == -1);
			default:
				throw new ArgumentException();

			}
		}

		public override void CommunicationAck(uint ackData)
		{
			int val;

			switch(ackData)
			{
			case 0x00:
			case 0x10:
			case 0x20:
				val = 0;
				break;
			case 0x30:
			case 0x40:
			case 0x50:
				val = 1;
				break;
			case 0x60:
				val = 2;
				break;
			case 0xff:
				val = -1;
				break;
			default:
				throw new ArgumentException();

			}

			if(this.Value != val)
			{
				innerUpdateSync = true;
				this.Value = val;
				innerUpdateSync = false;
			}
		}
	}
}
