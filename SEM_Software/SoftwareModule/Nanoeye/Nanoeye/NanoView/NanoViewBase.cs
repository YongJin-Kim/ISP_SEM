using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;

namespace SEC.Nanoeye.NanoView
{
	internal abstract class NanoViewBase : INanoViewCore
	{
		#region Property & Variables
		protected object sendThreadSync = new object();

		protected System.IO.Ports.SerialPort 			port = new System.IO.Ports.SerialPort();

		protected System.Timers.Timer repeatupdateTimer;
		protected System.Threading.Thread sendThread;

		protected System.Threading.ManualResetEvent sendingMRE = new System.Threading.ManualResetEvent(false);

		private string _PortName = null;
		public string PortName
		{
			get { return _PortName; }
			set { _PortName = value; }
		}

		public bool Enable
		{
			get { return port.IsOpen; }
			set
			{
				if ( value ) { Open(); }
				else if ( value ) { Close(); }
			}
		}

		protected int _RetryCnt=3;
		public int RetryCnt
		{
			get { return _RetryCnt; }
			set { _RetryCnt = value; }
		}

		protected PackeBase _PacketControl =null;
		public PackeBase PacketControl
		{
			get { return _PacketControl; }
			set { _PacketControl = value; }
		}
		#endregion

		#region Event
		public event EventHandler<NanoViewErrorEventArgs>  ErrorOccured;

		protected virtual void OnErrorOccured(NanoViewErrorEventArgs nveea)
		{
			if ( ErrorOccured != null )
			{
				ErrorOccured(this, nveea);
			}
		}
		#endregion

		#region 생성 및 파괴

		public NanoViewBase()
		{
		}

		~NanoViewBase()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			if (sendThread != null)
			{
				sendThread.Abort();
			}
			Close();
			if (port != null)
			{
				port.Dispose();
				port = null;
			}
			GC.SuppressFinalize(this);
		}
		
		#endregion

		public override string ToString()
		{
			return "NanoViewBase - " + port.PortName;
		}


		//cross Thread 예외
		//private delegate void OnSetThreadCallSendingWorker(bool Enable);

		//private void SetThreadCallSendingWorker(bool Enable)
		//{
		//    if (this.InvokeRequired)
		//    {
		//        OnSetThreadCallSendingWorker onSetThreadCallSendingWorker = null;
		//        onSetThreadCallSendingWorker = new OnSetThreadCallSendingWorker(SetThreadCallSendingWorker);
		//        this.Invoke(onSetThreadCallSendingWorker, new object[] { Enable });
		//    }
		//    else
		//    {
		//        this.sendThread = Enable);
		//    }
		//}

		public void Open()
		{
			Close();

			repeatupdateTimer = new System.Timers.Timer();
			repeatupdateTimer.AutoReset = true;
			repeatupdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(repeatupdateTimer_Elapsed);
			repeatupdateTimer.Interval = 1000;


			lock (sendThreadSync)
			{
				sendThread = new System.Threading.Thread(new System.Threading.ThreadStart(SendingWorker));
				sendThread.IsBackground = true;
				//sendThread.Priority = System.Threading.ThreadPriority.Lowest;
				sendThread.Name = "Serial Communication Worker";
				sendThread.Start();
			}

			InitPort(port);
			port.PortName = _PortName;

			port.Open();
		}

		public void Close()
		{
			try
			{
				if (port != null)
				{
					port.Close();
					port.Dispose();
					port = new SerialPort();
				}
			}
			catch (InvalidOperationException) { System.Diagnostics.Debug.WriteLine("InvalidOperationException discarded in \"Close\" method.", "NanoViewBase"); }

			lock (sendThreadSync)
			{
				if (sendThread != null)
				{
					sendThread.Abort();
					sendThread = null;
				}
			}
			if (repeatupdateTimer != null)
			{
				repeatupdateTimer.Stop();
				repeatupdateTimer = null;
			}
		}

		abstract protected void repeatupdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e);

		abstract protected void SendingWorker();

		protected virtual void InitPort(System.IO.Ports.SerialPort serial)
		{
			serial.BaudRate = 57600;
			serial.DataBits = 8;
			serial.Handshake = Handshake.None;
			serial.Parity = Parity.None;
			serial.StopBits = StopBits.One;
			serial.DiscardNull = false;
			serial.ReadTimeout = 200;
		}
	}
}
