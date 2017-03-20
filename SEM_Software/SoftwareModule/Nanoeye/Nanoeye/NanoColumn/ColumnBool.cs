using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class ColumnBool : SECtype.ControlValueBase, SECtype.IControlBool, SEC.Nanoeye.NanoView.IMasterObjet, IColumnValue
	{
		#region Property & Variables
		internal MiniSEM_Devices setter = 0;

		protected NanoView.NanoViewMasterSlave _Viewer = null;

		protected bool _Value = false;
		public bool Value
		{
			get { return _Value; }
			set
			{
				if (_Enable)
				{
					_Value = value;
					if (_IsInited)
					{
						if (_Viewer != null)
						{
							if (setter != 0)
							{
								UInt16 addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Set);

								_Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, (uint)(_Value ? 1 : 0)), false);
							}
						}
						OnValueChanged();
					}
				}
			}
		}


		#endregion

		#region Event
		public event  ObjectArrayEventHandler  RepeatUpdated;

		protected virtual void OnRepeatUpdated(object[] datas)
		{
			if ( RepeatUpdated != null )
			{
				RepeatUpdated(this, datas);
			}
		}

		public event EventHandler  ValueChanged;

		protected virtual void OnValueChanged()
		{
			if ( ValueChanged != null )
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		public override void Sync()
		{
			if (_Enable)
			{
				this.Value = GetDeviceValue();
			}
		}

		public override bool Validate()
		{
			if (_Enable)
			{
				return (this._Value == GetDeviceValue());
			}
			else
			{
				return false;
			}
		}

		public override void BeginInit()
		{
			if (_Enable)
			{
				_IsInited = false;
			}
		}

		public override void EndInit()
		{
			EndInit(false);
		}

		public override void EndInit(bool sync)
		{
			if (_Enable)
			{
				_Viewer = ((ISEMController)_Owner).Viewer as NanoView.NanoViewMasterSlave;
				_IsInited = true;
				if (sync) { Sync(); }
				else { Value = _Value; }
			}
		}

		protected bool GetDeviceValue()
		{
			ushort addr;
			uint data = 0;

			addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Get);

			if ( _Viewer != null )
			{
				byte[] response = _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);
				NanoView.PacketFixed8Bytes.UnPacket(response, out addr, out data);
			}

			return (data == 1 ? true : false);
		}

		public void NanoviewRepose(byte[] datas, SEC.Nanoeye.NanoView.ErrorType et)
		{
			if (_Enable)
			{
				switch (et)
				{
				case SEC.Nanoeye.NanoView.ErrorType.NoResponse:
					OnNoResponse();
					break;
				case SEC.Nanoeye.NanoView.ErrorType.CRC:
				case SEC.Nanoeye.NanoView.ErrorType.RxFail:
				case SEC.Nanoeye.NanoView.ErrorType.StartByte:
				case SEC.Nanoeye.NanoView.ErrorType.TxFail:
					OnCommunicationError();
					break;
				case SEC.Nanoeye.NanoView.ErrorType.Non:
					break;
				}
			}
		}
	}
}
