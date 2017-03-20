using System;
using System.Collections.Generic;
using System.Diagnostics;
using NationalInstruments.DAQmx;

namespace SEC.Nanoeye.NanoImage.DataAcquation.NIDaq
{
	internal class NIDaq6251 : IDAQ
	{
		#region Property & Vairables
		Task aoTaskRun;
		Task aiTaskRun;

		Task aoTaskReady;
		Task aiTaskReady;

		short[,] aoDataReady;

		private AnalogUnscaledReader reader;
		private AnalogUnscaledWriter writer;

       


		private readonly string[] aiChnnelList = { "ai0", "ai1", "ai2", "ai3", "ai4", "ai5", "ai6", "ai7" };

		protected string daqDevice;

		protected bool _Enable = false;
		public bool Enable
		{
			get { return _Enable; }
		}

        private int aichnnel = 0;
        public int AiChannel
        {
            get { return aichnnel; }
            set { aichnnel = value; }
        }

        private bool _Revers = false;
        public bool Revers
        {
            get { return _Revers; }
            set { _Revers = value; }

        }

        private bool _DualEnable = false;
        public bool DualEnable
        {
            get { return _DualEnable; }
            set { _DualEnable = value; }
        }

		private SettingScanner[] _SettingsRunning = null;
		public SettingScanner[] SettingsRunning
		{
			get { return _SettingsRunning; }
		}

		private SettingScanner[] _SettingsReady = null;
		public SettingScanner[] SettingsReady
		{
			get { return _SettingsReady; }
		}
		#endregion

		#region 생성자 & 소멸자 & Dispose
		protected NIDaq6251()
		{
		}

		public NIDaq6251(string device)
		{
			daqDevice = device;
			Trace.WriteLine(DaqSystem.Local.DriverMajorVersion.ToString() + "." + DaqSystem.Local.DriverMinorVersion + "." + DaqSystem.Local.DriverUpdateVersion, this.ToString());
		}

		~NIDaq6251()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_Enable) { Stop(); }
		}
		#endregion

		public override string ToString()
		{
			return "NiDaq6251-" + daqDevice;
		}

		#region 시작 및 종료
		public void Ready(SettingScanner[] set)
		{
			_SettingsRunning = set;

			ValidateSequenceRunable(set);

			SettingScanner sDaq = MakeRunableSet(set);

			if (aiTaskReady != null)
			{
				aiTaskReady.Dispose();
				aiTaskReady = null;
			}

			if (aoTaskReady != null)
			{
				aoTaskReady.Dispose();
				aoTaskReady = null;
			}

			string aiName = "Video-";
			string aoName = "Scan-";
			foreach (SettingScanner ss in set)
			{
				aiName += ss.Name + "-";
				aoName += ss.Name + "-";
			}

			aiTaskReady = new Task();
			Debug.WriteLine(aiName + " - Crated", this.ToString());
			aiTaskReady.AIChannels.CreateVoltageChannel(
				daqDevice + "/" + aiChnnelList[sDaq.AiChannel],
				"aichannel",
				sDaq.AiDifferential ? AITerminalConfiguration.Differential : AITerminalConfiguration.Rse,
				sDaq.AiMinimum,
				sDaq.AiMaximum,
				AIVoltageUnits.Volts);
			aiTaskReady.Timing.ConfigureSampleClock(
				"",
				sDaq.AiClock,
				SampleClockActiveEdge.Rising,
				SampleQuantityMode.ContinuousSamples);
			//aiTask.Timing.DelayFromSampleClockUnits = DelayFromSampleClockUnits.Seconds;
			aiTaskReady.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger("ao/StartTrigger", DigitalEdgeStartTriggerEdge.Rising);
			aiTaskReady.Triggers.StartTrigger.DelayUnits = StartTriggerDelayUnits.SampleClockPeriods;
			if (sDaq.PropergationDelay > 0)
			{
				aiTaskReady.Triggers.StartTrigger.Delay = sDaq.PropergationDelay;
			}
			aiTaskReady.Stream.Timeout = 5000;
			aiTaskReady.Control(TaskAction.Verify);
			aiTaskReady.Stream.ConfigureInputBuffer(aiTaskReady.Stream.Buffer.InputBufferSize * 16);
			aiTaskReady.Control(TaskAction.Verify);


			aoTaskReady = new Task();
			Debug.WriteLine(aoName + " - Crated", this.ToString());
			aoTaskReady.AOChannels.CreateVoltageChannel(
				daqDevice + "/ao0:1",
				"aochannel",
				sDaq.AoMinimum,
				sDaq.AoMaximum,
				AOVoltageUnits.Volts);
			aoTaskReady.Control(TaskAction.Verify);

			aoTaskReady.Timing.ConfigureSampleClock(
			   "",
			   (sDaq.AiClock / 2 / sDaq.SampleComposite),
			   SampleClockActiveEdge.Rising,
			   SampleQuantityMode.ContinuousSamples);
			aoTaskReady.Stream.Timeout = 0;
			aoTaskReady.Control(TaskAction.Verify);

			//List<short[]> ax1List = new List<short[]>();
			//List<short[]> ax2List = new List<short[]>();
			//foreach (SettingScanner ss in set)
			//{
			ScanGenerator generator = new ScanGenerator();
			generator.Divid = (int)(sDaq.AiClock / sDaq.AoClock);
			generator.FrameSize = new System.Drawing.Size(sDaq.FrameWidth, sDaq.FrameHeight);
			generator.RatioX = sDaq.RatioX;
			generator.RatioY = sDaq.RatioY;
			generator.ShiftX = sDaq.ShiftX;
			generator.ShiftY = sDaq.ShiftY;
			generator.LineAverage = sDaq.LineAverage;

			//    short[] ax1, ax2;
			//    generator.Generate(out ax1, out ax2);

			//    ax1List.Add(ax1);
			//    ax2List.Add(ax2);
			//}

			//int length = 0;
			//foreach (short[] ax in ax1List)
			//{
			//    length += ax.Length;
			//}

			//aoDataReady = new short[2, length];

			//int sum = 0;
			//for (int i = 0; i < ax1List.Count; i++)
			//{
			//    Array.Copy(ax1List[i], 0, aoDataReady.GetLowerBound(), sum, ax1List[i].Length);
			//    Array.Copy(ax2List[i], 0, aoDataReady, length + sum, ax2List[i].Length);
			//    sum += ax1List[i].Length;
			//}

			// TODO : 다중 스캔이 가능 하도록 수정 해야 함.
           
			aoDataReady = generator.Generate();
		}

		public void Change()
		{
			Stop();
			aiTaskRun = aiTaskReady;
			aoTaskRun = aoTaskReady;

			aiTaskReady = null;
			aoTaskReady = null;

			aiTaskRun.Control(TaskAction.Verify);
			Debug.WriteLine(aiTaskRun.ToString() + " - Started.", this.ToString());
			reader = new AnalogUnscaledReader(aiTaskRun.Stream);
			aiTaskRun.Start();

			Debug.WriteLine(aoTaskRun.ToString() + " - Started.", this.ToString());
			writer = new AnalogUnscaledWriter(aoTaskRun.Stream);
			writer.Write(true, aoDataReady);

			_Enable = true;
		}

		public void Stop()
		{
			if (aiTaskRun != null)
			{
				try
				{
					aiTaskRun.Stop();
					Debug.WriteLine(aiTaskRun.ToString() + " - Stopped AI.", this.ToString());
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.StackTrace, e.Message);
				}
				aiTaskRun.Dispose();
				aiTaskRun = null;
			}

			if (aoTaskRun != null)
			{
				try
				{
					aoTaskRun.Stop();
					Debug.WriteLine(aoTaskRun.ToString() + " - Stopped AO.", this.ToString());
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.StackTrace, e.Message);
				}
				aoTaskRun.Dispose();
				aoTaskRun = null;
			}

			_Enable = false;
		}
		#endregion

		#region 유효성 검증
		public virtual void ValidateSetting(SettingScanner setting)
		{
			System.Text.StringBuilder msg = new System.Text.StringBuilder();
			System.Text.StringBuilder arg = new System.Text.StringBuilder();

			if (setting.AiChannel >= 8)
			{
				setting.AiChannel = 7;
				msg.AppendLine("AiChannel is large then 7.");
				arg.AppendLine("AiChannel");
			}
			if (setting.AiClock > 1250000)
			{
				setting.AiClock = 1250000;
				msg.AppendLine("AiClock is large then 1250000.");
				arg.AppendLine("AiClock");
			}
			if ((setting.AiMaximum != 10) && (setting.AiMaximum != 5) && (setting.AiMaximum != 2) && (setting.AiMaximum != 1) && (setting.AiMaximum != 0.5f) && (setting.AiMaximum != 0.2f) && (setting.AiMaximum != 0.1f))
			{
				setting.AiMaximum = 10;
				msg.AppendLine("AiMaximum is invalid number.");
				arg.AppendLine("AiMaximum");
			}

			if (setting.AiMaximum != (setting.AiMinimum * -1))
			{
				setting.AiMinimum = setting.AiMaximum * -1;
				msg.AppendLine("AiMinimum is invalid number.");
				arg.AppendLine("AiMinimum");
			}

			if (setting.AoClock > 1250000)
			{
				setting.AoClock = 1250000;
				msg.AppendLine("AoClock is large then 1250000.");
				arg.AppendLine("AoClock");
			}

			if ((setting.AoMaximum != 10) && (setting.AiMaximum != 5))
			{
				setting.AoMaximum = 10;
				msg.AppendLine("AoMaximum is invalid number.");
				arg.AppendLine("AoMaximum");
			}

			if (setting.AoMaximum != (setting.AoMinimum * -1))
			{
				setting.AoMinimum = setting.AoMaximum * -1;
				msg.AppendLine("AoMinimum is invalid number.");
				arg.AppendLine("AoMinimum");
			}

			if ((setting.AreaShiftX > 1) || (setting.AreaShiftX < -1))
			{
				setting.AreaShiftX = 0;
				msg.AppendLine("AreaShiftX must be bettwen from -1 to 1.");
				arg.AppendLine("AreaShiftX");
			}

			if ((setting.AreaShiftY > 1) || (setting.AreaShiftY < -1))
			{
				setting.AreaShiftY = 0;
				msg.AppendLine("AreaShiftY must be bettwen from -1 to 1.");
				arg.AppendLine("AreaShiftY");
			}

			if ((setting.RatioX < 0.1) || (setting.RatioX > 1))
			{
				setting.RatioX = 1;
				msg.AppendLine("RatioX must be bettwen from 0.1 to 1.");
				arg.AppendLine("RatioX");
			}

			if ((setting.RatioY < 0.1) || (setting.RatioY > 1))
			{
				setting.RatioY = 1;
				msg.AppendLine("RatioY must be bettwen from 0.1 to 1.");
				arg.AppendLine("RatioY");
			}

			if ((Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
			{
				setting.ShiftX = 0;
				msg.AppendLine("ShiftX is invalid number.");
				arg.AppendLine("ShiftX");
			}

			if ((Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
			{
				setting.ShiftY = 0;
				msg.AppendLine("ShiftY is invalid number.");
				arg.AppendLine("ShiftY");
			}

			if ((Math.Abs(setting.AreaShiftX) + Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
			{
				setting.AreaShiftX = 0;
				msg.AppendLine("AreaShiftX is invalid number.");
				arg.AppendLine("AreaShiftX");
			}

			if ((Math.Abs(setting.AreaShiftY) + Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
			{
				setting.AreaShiftY = 0;
				msg.AppendLine("AreaShiftY is invalid number.");
				arg.AppendLine("AreaShiftY");
			}

			if (msg.Length > 0)
			{
				throw new ArgumentException(msg.ToString(), arg.ToString());
			}
		}

		public void ValidateSequenceRunable(SettingScanner[] setting)
		{
			if (setting.Length > 1)
			{
				throw new NotImplementedException("This will be implemented some days.");
			}
		}

		private SettingScanner MakeRunableSet(SettingScanner[] set)
		{
			return set[0];
		}
		#endregion

		#region Read
		/// <summary>
		/// 읽기 가능한 sample수
		/// </summary>
		public int ReadAvailables
		{
			get { return (int)aiTaskRun.Stream.AvailableSamplesPerChannel; }
		}

		/// <summary>
		/// sample 읽기
		/// </summary>
		/// <param name="samples"></param>
		/// <returns></returns>
		public short[,] Read(int samples)
		{
			return reader.ReadInt16(samples);
		}
		#endregion

		/// <summary>
		/// Ni Daq 6251 제품의 목록을 반환한다.
		/// </summary>
		/// <returns></returns>
		public static string[] SearchNIDaq6251()
		{
			List<string> deviceList = new List<string>();

			foreach (string deviceName in DaqSystem.Local.Devices)
			{
				Device device = DaqSystem.Local.LoadDevice(deviceName);

				if (device.ProductType.Contains("6251"))
				{
					deviceList.Add(deviceName);
				}
				device.Dispose();
			}
			return deviceList.ToArray();
		}


		public void ShowInformation(System.Windows.Forms.IWin32Window owner)
		{
			if ((Information.Default == null) || (Information.Default.IsDisposed))
			{
				Information.Default = new Information();
				Information.Default.Show(owner);
			}
			else
			{
				Information.Default.Location = new System.Drawing.Point(0, 0);
				Information.Default.WindowState = System.Windows.Forms.FormWindowState.Normal;
				Information.Default.Activate();
			}
		}

		public void OnePoint(double horizontal, double vertical)
		{
            Stop();

            aoTaskRun = new Task();
            aoTaskRun.AOChannels.CreateVoltageChannel(
                daqDevice + "/ao0:1",
                "aochannel",
                -10d,
                +10d,
                AOVoltageUnits.Volts);
            aoTaskRun.Control(TaskAction.Verify);


            aoTaskReady.Timing.ConfigureSampleClock(
               "",
               (1250000 / 2 / 11),
               SampleClockActiveEdge.Rising,
               SampleQuantityMode.ContinuousSamples);
            aoTaskReady.Stream.Timeout = 0;
            aoTaskReady.Control(TaskAction.Verify);

            writer = new AnalogUnscaledWriter(aoTaskRun.Stream);
            short[,] aoDatas = new short[2, 1];

            aoDatas[0, 0] = (short)(horizontal * short.MaxValue);
            aoDatas[1, 0] = (short)(vertical * short.MaxValue);


            writer.Write(true, aoDatas);
		}

		public void Reset()
		{
			
		}
	}
}
