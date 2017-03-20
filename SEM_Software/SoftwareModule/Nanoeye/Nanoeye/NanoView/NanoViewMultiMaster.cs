using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	internal class NanoViewMultiMaster : NanoViewBase
	{
		List<byte> sendDataList = new List<byte>();
		Dictionary<int, byte[]> repeatDataDic = new Dictionary<int, byte[]>();

		System.Threading.AutoResetEvent sendAre = new System.Threading.AutoResetEvent(false);

		protected override void InitPort(System.IO.Ports.SerialPort serial)
		{
			base.InitPort(serial);
			serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
		}

		void serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			System.IO.Ports.SerialPort serial = sender as System.IO.Ports.SerialPort;

			int byteCnt = serial.BytesToRead;

			byte[] packet;
			byte[] receive = new byte[byteCnt];

			serial.Read(receive, 0, byteCnt);

			for ( int i = 0 ; i < byteCnt ; i++ )
			{
				if (( packet = _PacketControl.BytesReceive(receive[i]) ) != null)
				{
					//this.
				}
			}
		}

		public void Send(byte[] datas)
		{
			lock ( sendDataList )
			{
				this.sendDataList.AddRange(datas);
			}
			sendAre.Set();
		}

		public bool RepeatUpdateAdd(int id, byte[] datas)
		{
			if ( id < 0 )
			{
				throw new ArgumentException("Id must be grate then 0.");
			}

			lock ( repeatDataDic )
			{
				if ( repeatDataDic.ContainsKey(id) )
				{
					return false;
				}

				repeatDataDic.Add(id, datas);
			}
			return true;
		}

		public bool RepeatUpdateRemove(int id)
		{
			lock ( repeatDataDic )
			{

				if ( repeatDataDic.ContainsKey(id) )
				{
					repeatDataDic.Remove(id);
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		protected override void repeatupdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock ( repeatDataDic )
			{
				foreach ( KeyValuePair<int,byte[]> kvp in repeatDataDic )
				{
					Send(kvp.Value);
				}
			}
		}

		protected override void SendingWorker()
		{
			byte[] datas;

			while ( true )
			{
				sendAre.WaitOne();
				lock ( sendDataList )
				{
					datas = sendDataList.ToArray();
				}

				port.Write(datas, 0, datas.Length);
			}
		}
	}
}
