using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal abstract class ColumnValueBase<T> : SECtype.ControlGenericBase<T>, IColumnValue, NanoView.IMasterObjet
													where T : struct, IComparable, IComparable<T>, IConvertible
	{
		internal MiniSEM_Devices readLower = 0;
		internal MiniSEM_Devices readUpper = 0;
		internal MiniSEM_Devices setter = 0;

		internal double readlowerConst = 1;
		internal double readupperConst = 1;

		/// <summary>
		/// 주기적 읽기 동작이 동작 중인지 여부.
		/// </summary>
		protected bool rpeatUpdateLinked = false;

		protected NanoView.NanoViewMasterSlave _Viewer = null;

		// 통신 과부하 방지를 위해 읽기 값을 cache 한다.
		protected object[] readCache = null;
		public override object[] Read
		{
			get
			{
				if (rpeatUpdateLinked)
				{
					if (readCache != null) { return readCache; }
					else { return ReadInnder(); }
				}
				else
				{
					return ReadInnder();
				}
			}
		}

		protected virtual object[] ReadInnder()
		{
			object[] result = new object[2];
			if (_Enable)
			{
				result[0] = null;
				result[1] = null;

				ushort addr = 0;
				uint datas = 0;
				byte[] datsArry;
				if (readLower != 0)
				{
					if (_Viewer != null)
					{
						addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Read);
						datsArry = _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);


						NanoView.PacketFixed8Bytes.UnPacket(datsArry, out addr, out datas);

						result[0] = (double)(datas) * readlowerConst;
					}
				}
				if (readUpper != 0)
				{
					if (_Viewer != null)
					{
						addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Read);
						datsArry = _Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);

						NanoView.PacketFixed8Bytes.UnPacket(datsArry, out addr, out datas);

						result[1] = (double)(datas) * readupperConst;
					}
				}
			}

			return result;
		}

		public override void EndInit()
		{
			EndInit( false );
		}

		public override void EndInit(bool sync)
		{
			if (_Enable)
			{
				_Viewer = ((ISEMController)_Owner).Viewer as NanoView.NanoViewMasterSlave;

				base.EndInit(sync);
			}
		}

		/// <summary>
		/// 주기적 읽기 동작을 정의 함.
		/// </summary>
		/// <param name="enable"></param>
		protected virtual void RepeatUpdate(bool enable)
		{
			UInt16 addr;

			if ((readLower == 0) && (readUpper == 0)) { throw new NotSupportedException(); }

			if (_Viewer == null) { return; }

			byte[] datas;

			if (enable)
			{
				if (rpeatUpdateLinked) { return; }
				rpeatUpdateLinked = true;


				if (readLower != 0)
				{
					addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Read);
					datas = NanoView.PacketFixed8Bytes.MakePacket(addr, 0);
					_Viewer.RepeatUpdateAdd(this, datas, addr);
				}
				if (readUpper != 0)
				{
					addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Read);
					datas = NanoView.PacketFixed8Bytes.MakePacket(addr, 0);
					_Viewer.RepeatUpdateAdd(this, datas, addr);
				}
			}
			else
			{
				if (rpeatUpdateLinked)
				{
					if (readLower != 0)
					{
						addr = (ushort)((ushort)readLower | (ushort)MiniSEM_DeviceType.Read);
						datas = NanoView.PacketFixed8Bytes.MakePacket((ushort)readLower, 0);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					if (readUpper != 0)
					{
						addr = (ushort)((ushort)readUpper | (ushort)MiniSEM_DeviceType.Read);
						datas = NanoView.PacketFixed8Bytes.MakePacket((ushort)readUpper, 0);
						_Viewer.RepeatUpdateRemove(this, addr);
					}
					rpeatUpdateLinked = false;
				}
			}
		}

		/// <summary>
		/// 통신 객체로 부터 즉시 값을 읽어 들임.
		/// </summary>
		/// <returns></returns>
		protected UInt32 GetDeviceValue()
		{
			ushort addr;
			uint data;

			addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Get);
			if(_Viewer != null)
			{
				byte[] response = _Viewer.Send( this, addr, NanoView.PacketFixed8Bytes.MakePacket( addr, 0 ), true );
				if (response == null)
				{
					return 0;
				}
				NanoView.PacketFixed8Bytes.UnPacket( response, out addr, out data );
			}
			else
			{
				data = 0;
			}

			return data;
		}

		double repeatLowaer = 0;
		double repeatUpper = 0;

		/// <summary>
		/// 통신 객체에서 발생한 이벤트를 처리
		/// </summary>
		/// <param name="datas"></param>
		/// <param name="et"></param>
		public virtual void NanoviewRepose(byte[] datas, SEC.Nanoeye.NanoView.ErrorType et)
		{
			if (_Enable)
			{
				switch (et)
				{
				case SEC.Nanoeye.NanoView.ErrorType.NoResponse:
					System.Diagnostics.Debug.WriteLine("NoResponse." , _Name);
					OnNoResponse();
					return;

				case SEC.Nanoeye.NanoView.ErrorType.CRC:
				case SEC.Nanoeye.NanoView.ErrorType.RxFail:
				case SEC.Nanoeye.NanoView.ErrorType.StartByte:
				case SEC.Nanoeye.NanoView.ErrorType.TxFail:
					System.Diagnostics.Debug.WriteLine("CommunicationError", _Name);
					OnCommunicationError();
					return;

				case SEC.Nanoeye.NanoView.ErrorType.Non:
					{
						UInt16 addr;
						UInt32 data;

						NanoView.PacketFixed8Bytes.UnPacket(datas, out addr, out data);

                        //System.Diagnostics.Debug.WriteLine("UnPacket - " + (addr & (UInt16)MiniSEM_DeviceParser.Type).ToString());

						switch (addr & (UInt16)MiniSEM_DeviceParser.Type)
						{
						case (UInt16)MiniSEM_DeviceType.Set:
							CommunicationAck(data);
							break;
						case (UInt16)MiniSEM_DeviceType.Read:
							if ((addr & ((UInt16)MiniSEM_DeviceParser.Board | (UInt16)MiniSEM_DeviceParser.Inst)) == (UInt16)readLower)
							{
								repeatLowaer = (double)data * readlowerConst;
								OnRepeatUpdated(new object[] { repeatLowaer, repeatUpper });
							}
							else if ((addr & ((UInt16)MiniSEM_DeviceParser.Board | (UInt16)MiniSEM_DeviceParser.Inst)) == (UInt16)readUpper)
							{
								repeatUpper = (double)data * readupperConst;
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
		}

		public abstract void CommunicationAck(UInt32 ackData);

		#region RepeatUpdate


		/// <summary>
		/// Repeat Update 동작을 사용할지 여부를 결정 하기 위해 event를 redirection 함.
		/// </summary>
		protected event ObjectArrayEventHandler RepeatUpdatedInternal;

		/// <summary>
		/// 주기적 읽기 동작에 의해 값을 읽었음을 알림.
		/// </summary>
		public event ObjectArrayEventHandler RepeatUpdated
		{
			add
			{
				RepeatUpdatedInternal += value;
				RepeatUpdatedref++;
			}
			remove
			{
				RepeatUpdatedInternal -= value;
				RepeatUpdatedref--;
			}
		}

		protected virtual void OnRepeatUpdated(object[] oaea)
		{
			readCache = oaea;
			if (RepeatUpdatedInternal != null)
			{
				RepeatUpdatedInternal(this, oaea);
			}
		}

		/// <summary>
		/// Repeat Update 이벤트에 연결된 갯수.
		/// </summary>
		int repeatRef = 0;
		private int RepeatUpdatedref
		{
			get { return repeatRef; }
			set
			{
				System.Diagnostics.Debug.Assert(value >= 0);

				if (value == 1)
				{
					if (repeatRef == 0)
					{
						RepeatUpdate(true);
					}
				}
				else if (value == 0)
				{
					RepeatUpdate(false);
				}
				repeatRef = value;
				System.Diagnostics.Debug.WriteLine("Repeat Update Ref changed - " + repeatRef.ToString(), _Name);
			}
		}


		#endregion

	}
}
