using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;

namespace SEC.Nanoeye.NanoView
{
	/// <summary>
	/// Mini-SEM과 통신을 한다.
	/// </summary>
	internal class NanoView : IDisposable
	{
		System.IO.Ports.SerialPort port;
		System.Timers.Timer repeatupdateTimer;
		System.Threading.Thread sendThread;

		#region Property & Variable
		private string _NanoeyePort = null;
		/// <summary>
		/// 통신에 사용하는 포트 이름.
		/// </summary>
		public string NanoeyePort
		{
			get { return _NanoeyePort; }
		}
		#endregion

		#region Event
		/// <summary>
		/// 통신에 실패하였다.
		/// </summary>
		public event EventHandler<NanoviewErrorEventArgs> CommunicationError;
		/// <summary>
		/// 주기적 읽기 값이, 수신되었다.
		/// </summary>
		public event EventHandler<NanoviewEventArgs> RepeatUpdated;

		protected virtual void OnCommunicationError(NanoviewErrorEventArgs nea)
		{
			if ( CommunicationError != null )
			{
				CommunicationError(this, nea);
			}
		}

		protected virtual void OnRepeatUpdated(NanoviewEventArgs nea)
		{
			if ( RepeatUpdated != null )
			{
				RepeatUpdated(this, nea);
			}
		}
		#endregion

		#region IDisposable 멤버
		private bool _IsDisposed = false;
		public bool IsDisposed
		{
			get { return _IsDisposed; }
		}

		public void Dispose()
		{
			if ( port != null )
			{
				Debug.WriteLine(this, "[Dispose]");
				port.Dispose();
				port = null;

				_IsDisposed = true;
			}
		}
		#endregion

		#region 생성 & 소멸
		private NanoView() { }
		/// <summary>
		/// 새로운 NanoView를 생성한다.
		/// </summary>
		/// <param name="portName">Mini-SEM과 통신에 사용할 포트의 이름</param>
		public NanoView(string portName)
		{
			port = new System.IO.Ports.SerialPort(portName);
			InitPort(port);
			port.ErrorReceived += new SerialErrorReceivedEventHandler(port_ErrorReceived);

			port.Open();

			repeatupdateTimer = new System.Timers.Timer();
			repeatupdateTimer.AutoReset = true;
			repeatupdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(repeatupdateTimer_Elapsed);
			repeatupdateTimer.Interval = 1000;

			sendThread = new System.Threading.Thread(new System.Threading.ThreadStart(SendingWorker));
			sendThread.IsBackground = true;
			//sendThread.Priority = System.Threading.ThreadPriority.Lowest;
			sendThread.Name = "Serial Communication Worker";
			sendThread.Start();

			_NanoeyePort = portName;
		}

		~NanoView()
		{
			this.Dispose();
		}
		#endregion

		public override string ToString()
		{
			return "NanoView - " + port.PortName;
		}

		#region 통신
		System.Threading.ManualResetEvent sendingMRE = new System.Threading.ManualResetEvent(false);
		System.Threading.ManualResetEvent receivingMRE = new System.Threading.ManualResetEvent(false);
		List<NanoviewEventArgs> sendingList = new List<NanoviewEventArgs>();
		//Queue<NanoviewEventArgs> sendingList = new Queue<NanoviewEventArgs>();
		List<NanoviewEventArgs> CheckImmList = new List<NanoviewEventArgs>();
		List<NanoviewEventArgs> CheckBackList = new List<NanoviewEventArgs>();

		NanoviewEventArgs checkedValue;

		private void SendingWorker()
		{
			bool cont;

			NanoviewEventArgs nea;
			NanoeyeDevices device;
			NanoeyeDeviceType type;
			UInt32 value;

			UInt16 dev;

			byte[] buffer;
			System.Diagnostics.Stopwatch span = new System.Diagnostics.Stopwatch();
			NanoviewEventArgs checkTemp;

			while ( true )
			{
				#region Send
				sendingMRE.WaitOne();	// 송신할 데이터가 발생 할때까지 thread를 잠근다.

				lock ( sendingList )
				{
					//nea = sendingList.Dequeue();
					nea = sendingList[0];
					device = nea.device;
					type = nea.type;
					value = nea.value;

					sendingList.Remove(nea);
				}
				if ( sendingList.Count == 0 ) { sendingMRE.Reset(); }	// 송신할 데이타가 없다면 thread를 잠그도록 요청한다.

				dev = (UInt16)((UInt16)device | (UInt16)type);

				//Debug.WriteLine("Try to send - " + dev.ToString("X") + "," + value.ToString(), this.ToString());

				buffer = PacketControl.MakePacket(dev, value);

				port.DiscardInBuffer();
				port.DiscardOutBuffer();

				port.Write(buffer, 0, buffer.Length);
				#endregion

				#region Receive
				buffer = new byte[8];
				buffer[0] = 0;

				cont = true; // 시작 바이트 수신 timeout이 발생 하였는지를 체크한다.
				span.Reset();
				span.Start();

				while ( buffer[0] != 0x02 )
				{
					if ( span.ElapsedMilliseconds > 500 )
					{// 500밀리초 이상 경과시 Timeout
						
						cont = false;
						//Trace.WriteLine("시작 byte 수신 실패", this.ToString());
						//Debug.WriteLine(dev.ToString("X") + "-" + value.ToString(), this.ToString());
						break;

					}
					try
					{
						buffer[0] = (byte)(port.ReadByte());
					}
					catch { }
				}
				span.Stop();

				// 시작 바이트 수신 실패. List 정리함.
				if ( cont == false )
				{
					OnCommunicationError(new NanoviewErrorEventArgs(device, type, value, "Start Byte Error"));

					checkTemp = new NanoviewEventArgs((NanoeyeDevices)device, (NanoeyeDeviceType)type, value);

					if ( CheckImmList.Contains(checkTemp) )
					{ // 수신 대기 중임.
						checkedValue = checkTemp;
						lock ( CheckImmList ) { CheckImmList.Remove(checkTemp); }
						receivingMRE.Set();
					}
					else if ( CheckBackList.Contains(checkTemp) )
					{
						//OnRepeatUpdated(checkTemp); //수신 실패 했으므로 이벤트 발생 시키지 않음.
						lock ( CheckBackList ) { CheckBackList.Remove(checkTemp); }
					}
					continue;
				}	// 시작 바이트 수신 실패

				port.Read(buffer, 1, 7);

				if ( buffer[7] != PacketControl.ByteCheckSum(buffer, 1, 6) )
				{ // CRC Error
					OnCommunicationError(new NanoviewErrorEventArgs(device, type, value, "CRC"));
					//Trace.WriteLine("CRC Error", this.ToString());
					//Debug.WriteLine(dev.ToString("X") + "-" + value.ToString(), this.ToString());
					continue;
				}
				PacketControl.Unpack(buffer, ref dev, ref value);

				//Debug.WriteLine("Received - " + dev.ToString("X") + "," + value.ToString(), this.ToString());

				if ( dev != ((UInt16)device | (UInt16)type) )
				{// 송신 장치와 수신 장치가 다름
					OnCommunicationError(new NanoviewErrorEventArgs(device, type, value, "Invalid Device"));
					//Trace.WriteLine("송신 장치와 수신 장치가 다름", this.ToString());
					//Debug.WriteLine(device.ToString("X") + "|" + type.ToString("X"), this.ToString());
					//Debug.WriteLine(dev.ToString("X") + "-" + value.ToString(), this.ToString());
					continue;
				}

				checkTemp = new NanoviewEventArgs((NanoeyeDevices)(dev & 0xfff0), (NanoeyeDeviceType)(dev & 0xf), value);

				if ( CheckImmList.Contains(checkTemp) )
				{ // 수신 대기 중임.
					checkedValue = checkTemp;
					lock ( CheckImmList ) { CheckImmList.Remove(checkTemp); }
					receivingMRE.Set();
				}
				else if ( CheckBackList.Contains(checkTemp) )
				{
					OnRepeatUpdated(checkTemp);
					lock ( CheckBackList ) { CheckBackList.Remove(checkTemp); }
				}
				#endregion
			}
		}

		void port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			Trace.WriteLine("Serial Error Received", this.ToString());
			Debug.WriteLine(e.EventType.ToString(), this.ToString());
			OnCommunicationError(null);
		}
		#endregion

		#region 통신 요청
		/// <summary>
		/// 값을 설정한다.
		/// </summary>
		/// <param name="nd"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public void ValueSet(NanoeyeDevices nd, UInt32 value)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs(nd, NanoeyeDeviceType.Set, value);
			lock ( sendingList )
			{
				if ( sendingList.Contains(nea) )
				{
					sendingList.Remove(nea);	// 비교시 value를 비교 하지 않는다.
					sendingList.Add(nea);		// 따라서 새로운 value로 값을 set 하게 된다.
				}
				else { sendingList.Add(nea); }
			}
			sendingMRE.Set();
		}

		public void ValueWrite(NanoeyeDevices nd, UInt32 value)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs(nd, NanoeyeDeviceType.Write, value);
			lock ( sendingList )
			{
				if ( sendingList.Contains(nea) )
				{
					sendingList.Remove(nea);	// 비교시 value를 비교 하지 않는다.
					sendingList.Add(nea);		// 따라서 새로운 value로 값을 set 하게 된다.
				}
				else { sendingList.Add(nea); }
			}
			sendingMRE.Set();
		}

		public void EepromWrite(NanoeyeDevices nd, byte address, uint value)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs((NanoeyeDevices)((uint)nd | (uint)address), NanoeyeDeviceType.EepromWrite, value);
			lock ( sendingList )
			{
				if ( sendingList.Contains(nea) )
				{
					sendingList.Remove(nea);
					sendingList.Add(nea);
				}
				else { sendingList.Add(nea); }
			}
			sendingMRE.Set();
		}

		public uint EepromRead(NanoeyeDevices nd, byte address)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs((NanoeyeDevices)((uint)nd | (uint)address * 2), NanoeyeDeviceType.EepromRead, 0);
			return ReceiveValueImmediatle(nea);
		}

		/// <summary>
		/// 설정된 값을 읽어 온다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns></returns>
		public UInt32 ValueGet(NanoeyeDevices nd)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs(nd, NanoeyeDeviceType.Get, 0);
			return ReceiveValueImmediatle(nea);
		}

		/// <summary>
		/// 값을 읽어 온다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns></returns>
		public UInt32 ValueRead(NanoeyeDevices nd)
		{
			NanoviewEventArgs nea = new NanoviewEventArgs(nd, NanoeyeDeviceType.Read, 0);
			return ReceiveValueImmediatle(nea);
		}

		private uint ReceiveValueImmediatle(NanoviewEventArgs nea)
		{
			lock ( sendingList )
			{
				if ( sendingList.Contains(nea) )
				{
					sendingList.Remove(nea);	// 비교시 value를 비교 하지 않는다.
					sendingList.Add(nea);		// 따라서 새로운 value로 값을 set 하게 된다.
				}
				else { sendingList.Add(nea); }
			}
			lock ( CheckImmList )
			{
				if ( CheckImmList.Contains(nea) == false ) { CheckImmList.Add(nea); }
			}
			sendingMRE.Set();

			while ( true )
			{
				receivingMRE.WaitOne();
				if ( checkedValue == nea )
				{
					nea = checkedValue;
					receivingMRE.Reset();
					break;
				}
			}
			return nea.value;
		}
		#endregion

		#region 반복 업데이트
		List<UInt16> repeatupdateList = new List<UInt16>();

		/// <summary>
		/// 주기적 읽기가 요청된 종류의 갯수
		/// </summary>
		public int RepeatUpdateCount
		{
			get { return repeatupdateList.Count; }
		}

		/// <summary>
		/// 주지적 읽기의 주기
		/// </summary>
		public double RepeatUpdateInterval
		{
			get { return repeatupdateTimer.Interval; }
			set { repeatupdateTimer.Interval = value; }
		}

		/// <summary>
		/// 주기적 읽기의 동작 여부
		/// </summary>
		public bool RepeatUpdateEnable
		{
			get { return repeatupdateTimer.Enabled; }
			set { repeatupdateTimer.Enabled = value; }
		}

		/// <summary>
		/// 각 device의 Read 값을 주기적으로 읽어온다. 만약, 동작이 멈춰있다면, 주기적 읽기를 시작한다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns>이미 등록되어 있으면, false를 리턴한다.</returns>
		public bool RepeatUpdateAddRead(NanoeyeDevices nd)
		{
			return RepeatUpdateAdd(nd, NanoeyeDeviceType.Read);
		}

		/// <summary>
		/// 각 device의 Get 값을 주기적으로 읽어온다. 만약, 동작이 멈춰있다면, 주기적 읽기를 시작한다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns>이미 등록되어 있으면, false를 리턴한다.</returns>
		public bool RepeatUpdateAddGet(NanoeyeDevices nd)
		{
			return RepeatUpdateAdd(nd, NanoeyeDeviceType.Get);
		}

		private bool RepeatUpdateAdd(NanoeyeDevices nd, NanoeyeDeviceType ndt)
		{
			UInt16 dev = (UInt16)(((UInt16)nd) | ((UInt16)ndt));
			if ( repeatupdateList.Contains(dev) )
			{
				return false;
			}
			lock ( repeatupdateList )
			{
				repeatupdateList.Add(dev);
			}
			if ( RepeatUpdateEnable == false ) { RepeatUpdateEnable = true; }
			return true;
		}

		/// <summary>
		/// 각 device의 Read 값을 주기적으로 읽어오는 것을 취소한다. 만약, 더이상 읽어올 것이 없다면, 주지적 읽기를 종료한다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns>등록되어 있지 않다면, false를 리턴한다.</returns>
		public bool RepeatUpdateRemoveRead(NanoeyeDevices nd)
		{
			return RepeatUpdateRemove(nd, NanoeyeDeviceType.Read);
		}

		/// <summary>
		/// 각 device의 Get 값을 주기적으로 읽어오는 것을 취소한다. 만약, 더이상 읽어올 것이 없다면, 주지적 읽기를 종료한다.
		/// </summary>
		/// <param name="nd"></param>
		/// <returns>등록되어 있지 않다면, false를 리턴한다.</returns>
		public bool RepeatUpdateRemoveGet(NanoeyeDevices nd)
		{
			return RepeatUpdateRemove(nd, NanoeyeDeviceType.Get);
		}

		private bool RepeatUpdateRemove(NanoeyeDevices nd, NanoeyeDeviceType ndt)
		{
			UInt16 dev = (UInt16)(((UInt16)nd) | ((UInt16)ndt));
			if ( repeatupdateList.Contains(dev) )
			{
				lock ( repeatupdateList ) { repeatupdateList.Remove(dev); }
				if ( repeatupdateList.Count == 0 ) { repeatupdateTimer.Enabled = false; }
				return true;
			}
			return false;
		}

		private void repeatupdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock ( repeatupdateList )
			{
				foreach ( ushort rul in repeatupdateList )
				{
					NanoviewEventArgs nea;
					switch ( rul & 0xf )
					{
					case 1:
						nea = new NanoviewEventArgs((NanoeyeDevices)(rul & 0xfff0), NanoeyeDeviceType.Get, 0);
						break;
					case 3:
						nea = new NanoviewEventArgs((NanoeyeDevices)(rul & 0xfff0), NanoeyeDeviceType.Read, 0);
						break;
					default:
						throw new Exception();
					}
					lock ( sendingList )
					{
						if ( sendingList.Contains(nea) )
						{
							sendingList.Remove(nea);	// 비교시 value를 비교 하지 않는다.
							sendingList.Add(nea);		// 따라서 새로운 value로 값을 set 하게 된다.
						}
						else { sendingList.Add(nea); }
					}
					lock ( CheckBackList )
					{
						if ( CheckBackList.Contains(nea) == false ) { CheckBackList.Add(nea); }
					}
					sendingMRE.Set();
				}
			}
		}
		#endregion

		#region static Function
		/// <summary>
		/// Serial Port에 대한 셋팅을 한다.
		/// </summary>
		/// <param name="serial"></param>
		private static void InitPort(System.IO.Ports.SerialPort serial)
		{
			serial.BaudRate = 57600;
			serial.DataBits = 8;
			serial.Handshake = Handshake.None;
			serial.Parity = Parity.None;
			serial.StopBits = StopBits.One;
			serial.DiscardNull = false;
			serial.ReadTimeout = 500;
		}

		/// <summary>
		/// MiniSEM을 찾는다.
		/// </summary>
		/// <returns>Mini-SEM이라고 응답한 포트들의 목록</returns>
		public static string[] SearchMINISEM()
		{
			Debug.WriteLine("Search Mini-SEM", "NanoView");

			List<string> ports = new List<string>();

			SerialPort m_SerialPort = new SerialPort();

			InitPort(m_SerialPort);

			UInt16 address = (UInt16)((UInt16)(NanoeyeDevices.Egps_Enable) | ((UInt16)(NanoeyeDeviceType.Get)));

			byte[] buffer = new byte[10];
			foreach ( string pName in SerialPort.GetPortNames() )
			{
				Debug.WriteLine("Try to connect - " + pName, "Nanoview");
				try
				{
					m_SerialPort.PortName = pName;
					m_SerialPort.Open();
				}
				catch ( UnauthorizedAccessException uae )
				{
					Trace.WriteLine(uae.Message);
					continue;
				}
				catch ( Exception ex )
				{
					Trace.WriteLine(ex.Message, ex.StackTrace);
					continue;
				}

				try
				{
					buffer = PacketControl.MakePacket(address, 0xff);

					m_SerialPort.Write(buffer, 0, buffer.Length);

					m_SerialPort.BaseStream.Flush();
				}
				catch ( Exception ex )
				{
					Trace.WriteLine(m_SerialPort.PortName + " - " + ex.Message, ex.StackTrace);
					m_SerialPort.Close();
					continue;
				}

				int iCnt = 10;
				while ( m_SerialPort.BytesToWrite > 0 )
				{
					System.Threading.Thread.Sleep(50);

					iCnt--;
					if ( iCnt < 0 ) { break; }
				}

				System.Threading.Thread.Sleep(100);

				try
				{
					m_SerialPort.Read(buffer, 0, 8);
					if ( (buffer[0] == 0x02) && (buffer[3] == 0x00) && (buffer[4] == 0x00) && (buffer[5] == 0x00) && (buffer[6] == 0x00) )
					{
						ports.Add(m_SerialPort.PortName);
					}
					else
					{
						Debug.WriteLine("Receved Data Error", "Nanoview");
						Debug.WriteLine(buffer);
					}
				}
				catch ( Exception )
				{
				}
				m_SerialPort.Close();
			}

			Debug.WriteLine("Mini-SEM search completed.", "NanoView");
			Debug.Write("Port List - ", "NanoView");
			foreach ( string str in ports )
			{
				Debug.Write(str + ", ");
			}
			Debug.WriteLine("");
			if ( ports.Count == 0 )
			{
				return null;
			}
			else
			{
				return ports.ToArray();
			}
		}

		/// <summary>
		/// EGPS 보드로 부터 장비의 이름을 얻어 온다.
		/// </summary>
		/// <param name="port">사용할 Serial Port</param>
		/// <returns>장비 이름</returns>
		public static string GetName(string port)
		{
#if DEBUG
			return "DEBUG";
#else
			SerialPort m_SerialPort = new SerialPort();

			InitPort(m_SerialPort);

			UInt32 address = (UInt32)(NanoeyeDevicesFullName.Egps_SystemType_Read);

			byte[] buffer = new byte[10];
			try {
				m_SerialPort.PortName = port;
				m_SerialPort.Open();
			}
			catch (UnauthorizedAccessException uae) {
				Trace.WriteLine(uae.Message);
				return "UAE";
			}
			catch (Exception ex) {
				Trace.WriteLine(ex.Message, ex.StackTrace);
				return "EXC";
			}

			try {
				buffer[0] = 0x02;
				buffer[1] = (byte)address;
				buffer[2] = (byte)(address >> 8);
				buffer[3] = (byte)1;
				buffer[4] = (byte)0;
				buffer[5] = (byte)0;
				buffer[6] = (byte)0;
				buffer[7] = PacketControl.ByteCheckSum(buffer, 1, 6);
				buffer[8] = 0;
				buffer[9] = 0;

				m_SerialPort.Write(buffer, 0, buffer.Length);

				m_SerialPort.BaseStream.Flush();
			}
			catch (Exception ex) {
				Trace.WriteLine(ex.Message, ex.StackTrace);
				m_SerialPort.Close();
				return ("EXC");
			}

			int iCnt = 100;
			while (m_SerialPort.BytesToWrite > 0) {
				System.Threading.Thread.Sleep(50);

				iCnt--;
				if (iCnt < 0) { break; }
			}

			System.Threading.Thread.Sleep(200);

			try {
				m_SerialPort.Read(buffer, 0, 8);
				m_SerialPort.Close();
				if (buffer[0] != 0x02) {
					return "ErrStart";
				}
				int value = 0;
				value += ((int)buffer[6] << 24);
				value += ((int)buffer[5] << 16);
				value += ((int)buffer[4] << 8);
				value += ((int)buffer[3] << 0);

				switch (value) {
				case 1500:	// SNE-1500M
					return "SNE-1500M";
				case 1501:	// SH-1500(Hirox)
					return "SH-1500";
				case 3000:	// SNE-3000M
					return "SNE-3000M";
				case 3001:	// SH-3000(Hirox)
					return "SH-3000";
				case 3002:	// EvexMiniSEM(Evex)
					return "Evex Mini-SEM";
				case 5000:	// SNE-5000M
					return "SNE-5000M";
				default:
					return "UNKONW";
				}
			}
			catch (Exception) {
				return "EXC";
			}
#endif
		}

		#endregion
	}
}
