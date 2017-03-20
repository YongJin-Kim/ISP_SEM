using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.Nanoeye.NanoView
{
	internal class NanoViewMasterSlave : NanoViewBase
	{
		struct DatainfoStruct
		{
			public IMasterObjet Imo;
			public byte[] Datas;
			public int FailCnt;
			public int Id;

			public DatainfoStruct(IMasterObjet imo, byte[] datas, int failCnt, int id)
			{
				Imo = imo;
				Datas = datas;
				FailCnt = failCnt;
				Id = id;
			}
		}

		volatile List<DatainfoStruct> sendList = new List<DatainfoStruct>();

		System.Threading.AutoResetEvent ImmAre = new System.Threading.AutoResetEvent(false);
		DatainfoStruct ImmDatas;
		byte[] immResponse;

		public byte[] Send(IMasterObjet imo, int id, byte[] datas, bool sendImm)
		{
			return Send(imo, id, datas, sendImm, 0);
		}

		private byte[] Send(IMasterObjet imo, int id, byte[] datas, bool sendImm, int failCnt)
		{

			DatainfoStruct removeDis = new DatainfoStruct();
			bool remove = false;

			//string name;

			//if (imo != null) { name = imo.Name; }
			//else { name = "NULL"; }

			//Debug.WriteLine(string.Format("Try to send {0}, ID {1}, FailCnt {2}, Imm {3}", name, id, failCnt, sendImm), "NanoViewMS");

			
			lock (sendList)
			{
				foreach (DatainfoStruct dis in sendList)
				{
					if ((dis.Imo == imo) && (dis.Id == id))
					{
						remove = true;
						removeDis = dis;
						break;
					}
				}

				if (remove)
				{
					sendList.Remove(removeDis);
					if (sendList.Count == 0) { sendingMRE.Reset(); }
				}
			}

			if (sendImm)
			{
				ImmDatas = new DatainfoStruct(imo, datas, failCnt, id);

				lock (sendList)
				{
					sendList.Add(new DatainfoStruct(imo, datas, failCnt, id));
					sendingMRE.Set();
				}

				//if (ImmAre.WaitOne(5000))
                if (ImmAre.WaitOne())
				{
					ImmDatas = new DatainfoStruct(null, null, 0, 0);
					byte[] result = immResponse;
					return result;
				}
				else
				{
					if (ImmDatas.Imo != null)
					{
						ImmDatas.Imo.NanoviewRepose(ImmDatas.Datas, ErrorType.NoResponse);
					}
					ImmDatas = new DatainfoStruct(null, null, 0, 0);
					return null;
				}
			}
			else
			{
				lock (sendList)
				{
					sendList.Add(new DatainfoStruct(imo, datas, failCnt, id));
					sendingMRE.Set();
				}
				return null;
			}
		}


		

		protected override void SendingWorker()
		{
			DatainfoStruct dis;

			byte data=0;

			byte[] response=null;

			int inbytesCnt;

			bool restart = false;
			try
			{
				while (true)
				{
					sendingMRE.WaitOne();	// 송신할 데이터가 발생 할때까지 thread를 잠근다.

					lock (sendList)
					{
						if (sendList.Count == 0)
						{
							// 발생 해서는 안됨. 하지만 발생한다... 반듯이 고칠것..
							// TODO : 통신 불량. 임시 처리. 반듯이 수정할것.
							System.Diagnostics.Debug.WriteLine("Count of sendlist is 0.", "System");
							continue;
						}
						dis = sendList[0];
						sendList.Remove(dis);
						if (sendList.Count == 0) { sendingMRE.Reset(); }
					}

					// 패킷 전송 정보 확인.
					//System.Diagnostics.Debug.Write( "Packet Send - 0x" );
					//for(int i = 0; i < dis.Datas.Length; i++)
					//{
					//    System.Diagnostics.Debug.Write( dis.Datas[i].ToString( "X" ) );
					//}
					//System.Diagnostics.Debug.WriteLine( "" );

                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[0].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[1].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[2].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[3].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[4].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[5].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[6].ToString());
                    //System.Diagnostics.Trace.WriteLine("Write - " + dis.Datas[7].ToString());

					port.Write(dis.Datas, 0, dis.Datas.Length);
                    
					inbytesCnt = 0;

					while (true)
					{
						try
						{
							data = (byte)port.ReadByte();

                            //System.Diagnostics.Trace.WriteLine("Read - " + data.ToString());

                        }
						catch (TimeoutException)
						{
							FailControl(ref dis, inbytesCnt, "TimeOut");
							restart = true;
							break;

						}
						if ((response = _PacketControl.BytesReceive(data)) != null) { break; }
						inbytesCnt++;
					}

					if (restart)
					{
						restart = false;
						continue;
					}

					if (!_PacketControl.CheckSumValidate(response))
					{
						FailControl(ref dis, inbytesCnt, "CheckSum");
						continue;
					}

					if (ImmDatas.Imo == dis.Imo)
					{
						if (ImmDatas.Datas == dis.Datas)
						{
							immResponse = response;
							ImmAre.Set();
						}
					}
					else
					{
						dis.Imo.NanoviewRepose(response, ErrorType.Non);
					}
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch (TimeoutException te)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(te);
			}
			catch (Exception e)
			{
				lock (sendThreadSync)
				{
					if (sendThread != null)
					{
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(e);
						System.Diagnostics.Trace.WriteLine("Program will be closed.", "System");
						System.Diagnostics.Trace.Fail(e.Message);
						System.Diagnostics.Trace.Flush();

						System.Windows.Forms.Application.Exit();
					}
				}
			}
		}

		private void FailControl(ref DatainfoStruct dis, int inbytesCnt, string type)
		{

			dis.FailCnt++;

			if (dis.Imo != null) { Debug.WriteLine(string.Format("{0} _ ID{1}, C{2}, B{3} Fail - {4}", dis.Imo.Name,dis.Id, dis.FailCnt,inbytesCnt, type), "NanoViewMS"); }
			else { Debug.WriteLine(string.Format("NULL _ ID{0}, C{1}, B{2}  Fail - {3}",dis.Id, dis.FailCnt, inbytesCnt, type), "NanoViewMS"); }

			if (dis.FailCnt >= _RetryCnt)
			{
				// 패킷 전송 정보 확인.
				if (dis.Imo != null) { System.Diagnostics.Debug.Write(dis.Imo.Name + " Packet Fail - 0x", "NanoViewMS"); }
				else { System.Diagnostics.Debug.Write("Packet Fail - 0x", "NanoViewMS"); }
				for (int i = 0; i < dis.Datas.Length; i++)
				{
					System.Diagnostics.Debug.Write(dis.Datas[i].ToString("X2"));
				}
				System.Diagnostics.Debug.WriteLine("");

				if (ImmDatas.Id == dis.Id)
				{
					immResponse = null;
					ImmAre.Set();
				}
				else
				{
					if (inbytesCnt == 0)
					{
						dis.Imo.NanoviewRepose(null, ErrorType.NoResponse);
					}
					else
					{
						dis.Imo.NanoviewRepose(null, ErrorType.RxFail);
					}
				}
			}
			else
			{
				//if (dis.Imo != null) { Debug.WriteLine(string.Format("Resend - {0}, C{1}", dis.Imo.Name, dis.FailCnt), "NanoViewMS"); }
				//else { Debug.WriteLine(string.Format("Resend - NULL, C{0}", dis.FailCnt), "NanoViewMS"); }

				//System.Threading.Timer delayedResender = new System.Threading.Timer(new System.Threading.TimerCallback(DelayedResender), dis, 100, System.Threading.Timeout.Infinite);
				//System.Threading.Thread.Sleep(100);
				Send(dis.Imo, dis.Id, dis.Datas, false, dis.FailCnt);
			}
			_PacketControl.BytesClear();
		}

		private void DelayedResender(object info)
		{
			DatainfoStruct dis = (DatainfoStruct)info;
			Send(dis.Imo, dis.Id, dis.Datas, false, dis.FailCnt);
		}

		List<DatainfoStruct> repeatList = new List<DatainfoStruct>();

		public bool RepeatUpdateAdd(IMasterObjet imo, byte[] datas, int id)
		{
			DatainfoStruct disRemove = new DatainfoStruct();
			bool sameExist = false;

			if (id < 0) { throw new ArgumentException("Id must be grate then 0."); }

			foreach (DatainfoStruct dis in repeatList)
			{
				if (dis.Imo == imo)
				{
					if (dis.Id == id)
					{
						disRemove = dis;
						sameExist = true;
						break;
					}
				}
			}

			if (sameExist) { repeatList.Remove(disRemove); }

			repeatList.Add(new DatainfoStruct(imo, datas, 0, id));

			repeatupdateTimer.Start();
			repeatupdateTimer.Enabled = true;

			return true;
		}

		public bool RepeatUpdateRemove(IMasterObjet imo, int id)
		{
			DatainfoStruct disRemove = new DatainfoStruct();
			bool sameExist = false;

			if (id < 0) { throw new ArgumentException("Id must be grate then 0."); }

			lock (repeatList)
			{
				foreach (DatainfoStruct dis in repeatList)
				{
					if (dis.Imo == imo)
					{
						if (dis.Id == id)
						{
							disRemove = dis;
							sameExist = true;
							break;
						}
					}
				}

				if (sameExist) { repeatList.Remove(disRemove); }

				if (repeatList.Count == 0) { repeatupdateTimer.Stop(); }
			}
			return sameExist;
		}

		protected override void repeatupdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (repeatList)
			{
				foreach (DatainfoStruct dis in repeatList)
				{

					lock (sendList)
					{
						bool same  = false;
						foreach (DatainfoStruct dist in sendList)
						{
							if ((dis.Imo == dist.Imo) && (dis.Id == dist.Id))
							{
								same = true;
							}
						}
						
						if(!same){
							Send(dis.Imo, dis.Id, dis.Datas, false, 0);
						}
					}
				}
			}
		}
	}
}
