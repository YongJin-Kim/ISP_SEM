using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;

using SEC.GenericSupport;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class StatusServer : Component
	{
		#region Property & Variables
		public enum DeviceValueEnum
		{
			Magnification,
			AccelatedVoltage
		}

		private static StatusServer _Default = new StatusServer();
		public static StatusServer Default
		{
			get { return _Default; }
		}

		private TcpListener listner;
		private NetworkStream clientNS;

        

		private readonly int HeaderLength = 8;

		private bool _ServerEnable = false;
		public bool ServerEnable
		{
			get { return _ServerEnable; }
			protected set
			{
				if (_ServerEnable != value)
				{
					_ServerEnable = value;
					OnServerEnableChanged();
				}
			}
		}

        private bool _LoopEnable = false;
        public bool LoopEnable
        {
            get { return _LoopEnable; }
            set
            {
                _LoopEnable = value;
            }
        }

		private IPEndPoint _ClientAddress = new IPEndPoint(0, 0);
		public IPEndPoint ClientAddress
		{
			get { return _ClientAddress; }
			protected set
			{
				if (_ClientAddress != value)
				{
					_ClientAddress = value;
					OnClientAddressChanged();
				}
			}
		}

		private int _ListenerPort = 57341;
		public int ListenerpPort
		{
			get { return _ListenerPort; }
			set
			{
				if (_ListenerPort != value)
				{
					if (_ServerEnable) { throw new InvalidOperationException("Listener is running."); }
					_ListenerPort = value;
				}
			}
		}

		public delegate string GetDeviceValueDelegate(DeviceValueEnum device);
		/// <summary>
		/// 외부 요청 값을 읽어 들임. 이 class를 사용하기 위해서는 반듯이 정의 해 주어야 함.
		/// </summary>
		public GetDeviceValueDelegate GetDeviceValue;
		#endregion

		#region Event
		public event EventHandler ScanStealed;
		protected virtual void OnScanStealed()
		{
			if (ScanStealed != null)
			{
				ScanStealed(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 외부 장치에서 주사 권한을 돌려줌.
		/// </summary>
		public event EventHandler ScanFree;
		protected virtual void OnScanFree()
		{
			if (ScanFree != null)
			{
				ScanFree(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Server의 동작 상태가 바뀜.
		/// </summary>
		public event EventHandler ServerEnableChanged;
		protected virtual void OnServerEnableChanged()
		{
			if (ServerEnableChanged != null) { ServerEnableChanged(this, EventArgs.Empty); }
		}

		/// <summary>
		/// Client 가 바뀜.
		/// </summary>
		public event EventHandler ClientAddressChanged;
		protected virtual void OnClientAddressChanged()
		{
			if (ClientAddressChanged != null) { ClientAddressChanged(this, EventArgs.Empty); }
		}

		/// <summary>
		/// 새로운 메시지가 발생 하였음.
		/// </summary>
		public event EventHandler<StringEventArg> MessageGenerated;
		protected virtual void OnMessageGenerated(string msg)
		{
			Trace.WriteLine(msg, this.ToString());
			if (MessageGenerated != null) { MessageGenerated(this, new StringEventArg(msg)); }
		}
		#endregion

		public StatusServer()
		{
			InitializeComponent();
		}

		public StatusServer(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		public override string ToString()
		{
			return "StatusServer";
		}

		public void ServerOn()
		{
			ListenerStart();
			ServerEnable = true;
		}

		public void ServerOff()
		{
			ListnerStop();

			ClientAddress = new IPEndPoint(0, 0);
			ServerEnable = false;
		}

		private void ListenerStart()
		{
			if (_ServerEnable)
			{
				OnMessageGenerated("Server is already running.");
				return;
			}

			OnMessageGenerated("Try to start server.");
            IPHostEntry ipEnty = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress[] ipAddresses = ipEnty.AddressList;
            IPAddress ipAddresses = IPAddress.Loopback;

            if (!_LoopEnable)
            {
                for (int i = 0; i < ipEnty.AddressList.Length; i++)
                {
                    if (ipEnty.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddresses = ipEnty.AddressList[i];
                    }

                }
            }


            StatusServer.Default.ClientAddress = new IPEndPoint(ipAddresses, _ListenerPort);
            listner = new TcpListener(ipAddresses, _ListenerPort);
            Trace.WriteLine("ip :" + ipAddresses.ToString());
			try
			{
				listner.Start(1);
			}
			catch (SocketException se)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(se);
				OnMessageGenerated("Fail to start server.");

				ListnerStop();

				if (se.ErrorCode == 10048) { OnMessageGenerated("ListenerpPort is used by another program."); }
				else { OnMessageGenerated("Can't start server." + se.Message + " " + se.ErrorCode.ToString()); }
				return;
			}
			listner.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listner);


			OnMessageGenerated("Server is started.");
		}

		private void ListnerStop()
		{
			if (!_ServerEnable) { OnMessageGenerated("Server is not running."); }

			if (listner != null)
			{
				try
				{
					listner.Stop();
				}
				catch (SocketException se)
				{
					OnMessageGenerated("Error occured when stop server." + se.Message + " " + se.ErrorCode.ToString());
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(se);
				}

				OnMessageGenerated("Server is Stoped.");
			}
		}

		private void ClinetClose()
		{
			ClinetClose(true);
		}

		private void ClinetClose(bool sendMsg)
		{
			byte[] msg;
			if (clientNS != null)
			{
				if (sendMsg)
				{
					try
					{
						OnMessageGenerated("Try to send close message.");
						msg = MakePacketMsg("Close");
						clientNS.Write(msg, 0, msg.Length);
					}
					catch (Exception se)
					{
						OnMessageGenerated("Fail to send close message." + se.Message);
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(se);
					}
				}

				try
				{
					clientNS.Close();
				}
				catch (Exception ex)
				{
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				}

				try
				{
					clientNS.Dispose();
				}
				catch (Exception ex)
				{
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				}

				clientNS = null;
			}
			OnMessageGenerated("Disconnnect client.");


		}

		private void DoAcceptTcpClientCallback(IAsyncResult ar)
		{
			TcpClient client;

			try
			{
				client = listner.EndAcceptTcpClient(ar);
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				return;
			}
			ListnerStop();
			clientNS = client.GetStream();

			
			OnMessageGenerated("Client is connected.");

			string identify = "MINI-SEM_Status_Client";
			int idLength = Encoding.Unicode.GetByteCount(identify) + 8;

			byte[] receiveBuffer = new byte[idLength];
			int j = 0;
			try
			{
				while (j < idLength)
				{
					j += clientNS.Read(receiveBuffer, j, idLength - j);
				}
			}
			catch (IOException ioe)
			{
				OnMessageGenerated("Fail to receive client identification data.");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ioe);

				ClinetClose();
				client.Close();	// clinet는 argument로 넘어오므로 직접 닫아야 함.
				
				ListenerStart();

				return;
			}

			if (Encoding.Unicode.GetString(receiveBuffer, 8, receiveBuffer.Length - 8) != identify)
			{
				OnMessageGenerated("Fail to identify client.");
				Trace.WriteLine("Indentifycation Fail - " + Encoding.Unicode.GetString(receiveBuffer, 8, receiveBuffer.Length - 8), this.ToString());

				ClinetClose();
				client.Close();	// clinet는 argument로 넘어오므로 직접 닫아야 함.

				ListenerStart();

				return;
			}


			byte[] msg = MakePacketString(ErrWelcom);
			clientNS.Write(msg, 0, msg.Length);

			byte[] packMsg = MakePacketMsg("Ready");
			clientNS.Write(packMsg, 0, packMsg.Length);

			ClientAddress = (client.Client.RemoteEndPoint as IPEndPoint);

			OnMessageGenerated("Client identify sucess. - " + ClientAddress.ToString());
			byte[] header = new byte[HeaderLength];
			try
			{
				clientNS.BeginRead(header, 0, HeaderLength, new AsyncCallback(HeaderReceved), (object)(new object[] { clientNS, header }));
			}
			catch (Exception ex)
			{
				OnMessageGenerated("Error occured when recevie data from client");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);

				ClinetClose();
				client.Close();	// clinet는 argument로 넘어오므로 직접 닫아야 함.

				ListenerStart();

				return;
			}
		}


		/* Packet 구조
 * Header Part
 * 0 byte - 0x02
 * 1 byte - Message Type ( 0x01 : Packet, 0x02 : string)
 * 2-5 byte - Packet Lengths(Header + Data)
 * 6-7 bytes - 0(Reserved)
 */

		private readonly string ErrWelcom = "Welcome. This is Mini-SEM. Ver0.0.0.0";

		protected void HeaderReceved(IAsyncResult ar)
		{
			Debug.WriteLine("HeaderReceved Async Callback", this.ToString());

			object[] state = ar.AsyncState as object[];
			NetworkStream ns = state[0] as NetworkStream;
			byte[] header = state[1] as byte[];
			int recevedLength;
			try
			{
				recevedLength = ns.EndRead(ar);
			}
			catch (System.IO.IOException ioe)
			{
				OnMessageGenerated("Error occured when receive header data from client.");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ioe);

				ClinetClose();


				ServerOff();
				ServerOn();

				return;
			}
			catch (ObjectDisposedException ode)
			{
				OnMessageGenerated("Error occured when receive header data from client.");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ode);

				ClinetClose();

				return;
			}

			if (recevedLength != HeaderLength)
			{
				OnMessageGenerated("Invalid header(length) received from client.");

				ClinetClose();

				ServerOff();
				ServerOn();

				return;
			}

			if (header[0] != 0x02)
			{
				OnMessageGenerated("Invalid header(start byte) received from client.");

				byte[] packMsg = MakePacketMsg("ErrStart");
				ns.Write(packMsg, 0, packMsg.Length);

				while (ns.DataAvailable) { ns.ReadByte(); }

				packMsg = MakePacketMsg("Ready");
				ns.Write(packMsg, 0, packMsg.Length);

				ns.BeginRead(header, 0, HeaderLength, new AsyncCallback(HeaderReceved), (object)(new object[] { ns, header }));
				return;
			}

			if (header[1] != 0x02)
			{
				OnMessageGenerated("Invalid header(data type) received from client.");

				byte[] packMsg = MakePacketMsg("ErrUndefType");
				ns.Write(packMsg, 0, packMsg.Length);

				while (ns.DataAvailable) { ns.ReadByte(); }

				packMsg = MakePacketMsg("Ready");
				ns.Write(packMsg, 0, packMsg.Length);

				ns.BeginRead(header, 0, HeaderLength, new AsyncCallback(HeaderReceved), (object)(new object[] { ns, header }));
				return;
			}

			int dataLength = 0;
			dataLength += (int)header[2];
			dataLength += (int)header[3] << 8;
			dataLength += (int)header[4] << 16;
			dataLength += (int)header[5] << 24;
			dataLength -= HeaderLength;

			byte[] message = new byte[dataLength];

			state = new object[] { (object)ns, (object)dataLength, (object)message };
			ns.BeginRead(message, 0, dataLength, new AsyncCallback(PacketRecevedMessage), (object)state);
			Debug.WriteLine("Try to read data", this.ToString());
		}

		protected void PacketRecevedMessage(IAsyncResult ar)
		{
			object[] state = ar.AsyncState as object[];

			NetworkStream ns = state[0] as NetworkStream;
			int length = (int)(state[1]);
			byte[] message = state[2] as byte[];

			int receveLength = 0;
			try
			{
				receveLength = ns.EndRead(ar);
			}
			catch (System.IO.IOException ioe)
			{
				OnMessageGenerated("Error occured when receive data from client.");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ioe);

				ClinetClose();
				ServerOff();
				ServerOn();
			}

			if (receveLength != length)
			{
				ns.ReadTimeout = 30000;
				try
				{
					ns.Read(message, receveLength, length - receveLength);
				}
				catch (System.IO.IOException)
				{
					OnMessageGenerated("Invalid data(length) received from client.");

					ClinetClose();

					ServerOff();
					ServerOn();

					return;
				}
			}

			byte[] packet;
			byte[] packetHeader;
			string msg = Encoding.Unicode.GetString(message);
			switch (msg)
			{
			case "Magnification":
				string mag = GetDeviceValue(DeviceValueEnum.Magnification);
				packet = MakePacketMsg(mag);
				ns.Write(packet, 0, packet.Length);
				OnMessageGenerated("Client request Magnification.");
				break;
			case "Accelerated Voltage":
				string av = GetDeviceValue(DeviceValueEnum.AccelatedVoltage);
				packet = MakePacketMsg(av);
				ns.Write(packet, 0, packet.Length);
				OnMessageGenerated("Client request A.V.");
				break;
			case "Close":
				OnMessageGenerated("Client request to close.");
				ClinetClose(false);
				ServerOff();
				ServerOn();
				return;
			case "ScanSteal":
				OnMessageGenerated("Client notify scan steal.");
				packet = MakePacketMsg("ScanOK");
				ns.Write(packet, 0, packet.Length);
				OnScanStealed();
				break;
			case "ScanFree":
				OnMessageGenerated("Client notify scan free.");
				packet = MakePacketMsg("ScanOK");
				ns.Write(packet, 0, packet.Length);
				OnScanFree();
				break;
			default:
				OnMessageGenerated("Client send unknown message.");

				packet = MakePacketMsg("ErrMessage");
				ns.Write(packet, 0, packet.Length);

				while (ns.DataAvailable) { ns.ReadByte(); }

				packet = MakePacketMsg("Ready");
				ns.Write(packet, 0, packet.Length);

				byte[] header = new byte[HeaderLength];
				ns.BeginRead(header, 0, HeaderLength, new AsyncCallback(HeaderReceved), (object)(new object[] { ns, header }));
				return;
			}

			packetHeader = new byte[HeaderLength];

			state = new object[2];
			state[0] = (object)ns;
			state[1] = (object)packetHeader;

			ns.BeginRead(packetHeader, 0, HeaderLength, new AsyncCallback(HeaderReceved), (object)state);
			Debug.WriteLine("Receved", this.ToString());
		}

		#region Packet 관련
		private byte[] MakePacketMsg(string value)
		{
			Debug.WriteLine("Make Packet String Value - " + value.ToString(), this.ToString());
			return MakePacket(value, 0x02);
		}

		private byte[] MakePacketString(string msg)
		{
			Debug.WriteLine("Make String Value - " + msg, this.ToString());
			return MakePacket(msg, 0x01);
		}

		private byte[] MakePacket(string msg, int type)
		{
			byte[] msgBytes = Encoding.Unicode.GetBytes(msg);
			int length = msgBytes.Length + HeaderLength;
			byte[] packet = new byte[length];
			packet[0] = 0x02;
			packet[1] = (byte)(type & 0Xff);
			packet[2] = (byte)(length & 0xff);
			packet[3] = (byte)((length >> 8) & 0xff);
			packet[4] = (byte)((length >> 16) & 0xff);
			packet[5] = (byte)((length >> 24) & 0xff);
			packet[6] = 0;
			packet[7] = 0;
			Encoding.Unicode.GetBytes(msg, 0, msg.Length, packet, HeaderLength);
			return packet;
		}
		#endregion
	}
}
