using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class ColumnDouble : ColumnValueBase<double>, SECtype.IControlDouble
	{
		bool innerUpdateSync = false;
		bool innerUpdateCommunicationAck = false;

		public override double Value
		{
			get { return base.Value; }
			set
			{
				if (_Enable)
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

					//System.Diagnostics.Debug.WriteLine(((value + _Offset) / _Precision).ToString());
					base.Value = value;
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
			this.Value = (double)data * _Precision - _Offset;
			innerUpdateSync = false;
		}

		public override bool Validate()
		{
			if(!_Enable)
			{
				throw new InvalidOperationException( "This is not enabled." );
			}
			uint data = base.GetDeviceValue();
			return (this.Value == (double)data * _Precision - _Offset);
		}

		public override void CommunicationAck(uint ackData)
		{
			//double val = (double)((int)ackData) * _Precision;

			//if(this.Value != val)
			//{
			//    innerUpdateCommunicationAck = true;
			//    this.Value = val;
			//    innerUpdateCommunicationAck = true;
			//}
		}
	}
}
