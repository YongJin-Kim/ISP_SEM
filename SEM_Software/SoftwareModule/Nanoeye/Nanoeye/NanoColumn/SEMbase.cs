using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal abstract class SEMbase : SECtype.ControllerBase, ISEMController
	{
		protected string _HVtext = null;
		public string HVtext
		{
			get { return _HVtext; }
			set { _HVtext = value; }
		}

		protected NanoView.NanoViewMasterSlave _Viewer = null;
		public SEC.Nanoeye.NanoView.INanoView Viewer
		{
			get
			{
				return _Viewer;
			}
			set
			{
				if (_Initialized)
				{
					throw new InvalidOperationException("Controller is running.");
				}
				_Viewer = (NanoView.NanoViewMasterSlave)value;
			}
		}

		public override string[] AvailableDevices()
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

			List<string> goodPort = new List<string>();

			NanoView.NanoViewMasterSlave nvm = new SEC.Nanoeye.NanoView.NanoViewMasterSlave();
			nvm.PacketControl = new NanoView.PacketFixed8Bytes();
			Debug.WriteLine("Port check", _Name);
			foreach (string testPort in allPorts)
			{
				try
				{
					nvm.PortName = testPort;
					Debug.WriteLine(testPort + "Try to port open.");
					nvm.Open();
					addr = (ushort)(MiniSEM_DevicesFullName.Egps_Enable_Get);
					Debug.WriteLine("Send data");
					response = nvm.Send(null, addr,
						NanoView.PacketFixed8Bytes.MakePacket(addr, 1),
						true);
					Debug.WriteLine("check response");
					if (response != null)
					{
						Debug.WriteLine("unpack");
						NanoView.PacketFixed8Bytes.UnPacket(response, out addr, out data);
						if (data == 0)
						{
							Debug.WriteLine(testPort + " is good port");
							goodPort.Add(testPort);
						}
					}
					Debug.WriteLine("try to close nvm");
					nvm.Close();
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.StackTrace, e.Message);
					Debug.Flush();
				}
			}
			nvm.Dispose();

			Trace.Write("Good port - ", "Info");
			foreach (string strt in goodPort)
			{
				Trace.Write(strt + ",");
			}
			Trace.WriteLine(" ");

			return goodPort.ToArray();
		}

		public override void Initialize()
		{
			if (_Viewer != null)
			{
				if (!(_Viewer is NanoView.NanoViewMasterSlave))
				{
					throw new ArgumentException("Invaild viewer.");
				}

				if (this._Viewer.PacketControl == null)
				{
					this._Viewer.PacketControl = new NanoView.PacketFixed8Bytes();
				}
				else if (!(this._Viewer.PacketControl is NanoView.PacketFixed8Bytes))
				{
					throw new ArgumentException("Invaild NanoView. Packet Controller must be PacketFixed8Bytes.");
				}
			}
		}

        public override void Dispose()
        {
			if (_Viewer != null)
			{
				_Viewer.Dispose();
			}

            base.Dispose();
        }

		protected void AddBoolControl(string name, bool value, MiniSEM_Devices setter)
		{
			ColumnBool icvb;
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = name;
			icvb.Value = value;
			icvb.setter = setter;
			icvb.EndInit();
			controls.Add(name, icvb);
		}

		/// <summary>
		/// IControlDouble 타입을 추가한다.
		/// </summary>
		/// <param name="name">이름</param>
		/// <param name="defMax">기본 최대 값</param>
		/// <param name="defMin">기본 최소 값</param>
		/// <param name="max">최대 값</param>
		/// <param name="min">최소 값</param>
		/// <param name="value">초기화 값</param>
		/// <param name="pricision">최소 변화 단위</param>
		/// <param name="setter">통신 주소</param>
		/// <param name="readLower">읽기 하위 주소</param>
		/// <param name="readUpper">읽기 상위 주소</param>
		/// <param name="constLower">읽기 하위 변환 상수</param>
		/// <param name="constUpper">읽기 상위 변환 상수</param>
		protected void AddDoubleControl(string name, double defMax, double defMin, double max, double min, double value, double pricision, MiniSEM_Devices setter, MiniSEM_Devices readLower, MiniSEM_Devices readUpper, double constLower, double constUpper)
		{
            
			ColumnDouble icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = name;
			icvd.DefaultMax = defMax;
			icvd.DefaultMin = defMin;
			icvd.Maximum = max;
			icvd.Minimum = min;
			icvd.Value = value;
			icvd.Precision = pricision;
			icvd.setter = setter;
			icvd.readLower = readLower;
			icvd.readUpper = readUpper;
			icvd.readlowerConst = constLower;
			icvd.readupperConst = constUpper;
			icvd.EndInit();

			controls.Add(name, icvd);
		}

	}
}
