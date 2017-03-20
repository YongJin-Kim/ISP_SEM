using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class ColumnInt : ColumnValueBase<int>, SECtype.IControlInt
	{
		protected bool innerUpdateSync = false;
		bool innerUpdateCommunicationAck = false;

		public override int Value
		{
			get { return base.Value; }
			set
			{
				base.Value = value;
				SendValue(value);
			}
		}

		protected virtual void SendValue(int value)
		{
			if (!(innerUpdateSync || innerUpdateCommunicationAck))
			{
				if ((_Viewer != null) && _IsInited && _Enable)
				{
					if (setter != MiniSEM_Devices.Noting)
					{
						uint viwerSet = (uint)((value + _Offset) / _Precision);
						ushort addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Set);

						_Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, viwerSet), false);
					}
				}
			}
		}

		public override void Sync()
		{
			if(!_Enable)
			{
				throw new InvalidOperationException( "This is not enabled." );
			}
			uint data = base.GetDeviceValue();
				
			innerUpdateSync = true;
			this.Value = (int)(data * _Precision - _Offset);
			innerUpdateSync = false;
		}

		public override bool Validate()
		{
			if(!_Enable)
			{
				throw new InvalidOperationException( "This is not enabled." );
			}
			uint data = base.GetDeviceValue();
			return (this.Value == (int)data * _Precision - _Offset);
		}

		public override void CommunicationAck(uint ackData)
		{
			//int val = (int)(ackData * _Precision);

			//if(this.Value != val)
			//{
			//    innerUpdateSync = true;
			//    this.Value = val;
			//    innerUpdateSync = false;
			//}
		}
	}
}
