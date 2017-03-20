using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.Nanoeye.NanoColumn
{
	public sealed class Helper
	{
		public static string[,] EnumerateDevice()
		{
			byte[] response;
			UInt16 addr;
			UInt32 data;
			Trace.Write("Request AvailablePorts : ", "Info");
			string[] allPorts = System.IO.Ports.SerialPort.GetPortNames();
			foreach (string str in allPorts)
			{
				Trace.Write(str + ",");
			}
			Trace.WriteLine(" ");

			List<string[]> goodPort = new List<string[]>();

			Debug.WriteLine("Port check", "Column Helper");

			addr = (ushort)(MiniSEM_DevicesFullName.Egps_SystemType_Read);
			
			foreach (string testPort in allPorts)
			{
				NanoView.NanoViewMasterSlave nvm = null;
				try
				{
					nvm = new SEC.Nanoeye.NanoView.NanoViewMasterSlave();
					nvm.PacketControl = new NanoView.PacketFixed8Bytes();

					nvm.PortName = testPort;
					Debug.WriteLine(testPort + "Try to port open.");
					nvm.Open();


					Debug.WriteLine("Send data");
					response = null;
					response = nvm.Send(null, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, 1), true);

					Debug.WriteLine("check response");
					if (response != null)
					{
						Debug.WriteLine("unpack");
						data = 0;
						NanoView.PacketFixed8Bytes.UnPacket(response, out addr, out data);

						string device = "Unknown";

						switch (data)
						{
						case 1500:
							device = "SNE-1500M";
							break;
						case 1501:
							device = "SH-1500";
							break;
						case 3000:
							device = "SNE-3000M";
							break;
						case 3001:
							device = "SH-3000";
							break;
						case 3002:
							device = "Evex MiniSEM";
							break;
						case 3003:
							device = "SEMTRAC mini";
							break;
                        case 3100:
                            device = "SNE-3000MB";
                            break;
                        case 3200:
                            device = "SNE-3200M";
                            break;
                        case 3300:
                            device = "SNE-3000MS";
                            break;
                        case 3500:
                            device = "SH-3500MB";
                            break;
						case 4000:
							device = "SNE-4000M";
							break;
                        case 4001:
                            device = "SH-4000M";
                            break;
                        case 5002:
                            device = "SH-5000M";
                            break;
                        case 4500:
                            device = "SNE-4500M";
                            break;
						case 5000:
							device = "SNE-5000M";
							break;
						case 5001:
							device = "SNE-5001M";
							break;
                        case 9000:
                            device = "SNE-4500P";
                            break;
						}
						Trace.WriteLine(string.Format("{0} is good port. Device is {1}", testPort, device), "Info");
						goodPort.Add(new string[] { testPort, device });
					}
					Debug.WriteLine("try to close nvm");
                    
					nvm.Close();

                  
				}
				catch (Exception e)
				{
					Trace.WriteLine(e.StackTrace, e.Message);
					Trace.Flush();
				}

				if (nvm != null)
				{
					nvm.Dispose();
					nvm = null;
				}
			}

			if (goodPort.Count == 0)
			{
				return null;
			}
			else
			{
				string[,] result = new string[goodPort.Count, 2];

				int cnt = 0;
				foreach (string[] str in goodPort)
				{
					result[cnt, 0] = str[0];
					result[cnt, 1] = str[1];

					cnt++;
				}

				return result;
			}
		}
	}
}
