using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.Nanoeye.NanoImage.DataAcquation
{
	class VirtualDAQ : IDAQ
	{
		#region Property & Variables

		Dictionary<string, DataTableStruct> SettingDataDic = new Dictionary<string, DataTableStruct>();

		System.Threading.Timer dataGenerateTimer;

		private int dataIncrease = 0;
		private int linePnt = 0;
		private int runHeight = 0;
		private int runWidth = 0;
		private DataTableStruct runDts;


        private int aichnnel = 0;
        public int AiChannel
        {
            get { return aichnnel; }
            set { aichnnel = value; }
        }

		private bool _enable = false;
		/// <summary>
		/// 동작 여부
		/// </summary>
		public bool Enable
		{
			get { return _enable; }
		}

		private string _RunningTask = "";
		public string RunningTask
		{
			get { return _RunningTask; }
		}

		private int _ReadAvailables = 0;
		public int ReadAvailables
		{
			get { return _ReadAvailables; }
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

		#endregion

		#region 생성자 & 소멸자 & 파괴자
		public VirtualDAQ()
		{
			dataGenerateTimer = new System.Threading.Timer(new System.Threading.TimerCallback(DataGenerateTimerCallback));
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
		#endregion

		private void DataGenerateTimerCallback(object state)
		{
			_ReadAvailables += dataIncrease;
			linePnt += dataIncrease / (runWidth * runDts.setting.SampleComposite);
			if (linePnt >= runHeight)
			{
				linePnt -= runHeight;
			}
		}

		#region Task 관리
		/// <summary>
		/// 새로운 테스크를 만든다.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="sDaq"></param>
		/// <returns></returns>
		public void CreateTask(string name, SettingScanner sDaq)
		{
			Debug.WriteLine("Try to Make task - " + name, "VirtualDAQ");

			if (SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("Same Task exist.");
			}

			SettingDataDic.Add(name, new DataTableStruct(name, sDaq, null, false, true));
		}

		public void DeleteTask(string name)
		{
			Debug.WriteLine("Try to delete task - " + name, "VirtualDAQ");

			//if ( name == _RunningTask )
			//{
			//    throw new InvalidOperationException("Setting is Running. - DeleteTask");
			//}

			if (!SettingDataDic.Remove(name))
			{
				throw new ArgumentException("There are no same task.", "VirtualDAQ");
			}
		}

		public void CloneTask(string nameOri, string nameNew)
		{
			if (!SettingDataDic.ContainsKey(nameOri))
			{
				throw new ArgumentException("There are no same task.", "VirtualDAQ");
			}

			DataTableStruct dts = SettingDataDic[nameOri];

			SettingDataDic.Add(nameNew, new DataTableStruct(nameNew,  dts.setting, null, false, true));
		}

		/// <summary>
		/// 여러개의 Task를 묶어 하나의 Task로 만든다.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="names"></param>
		public void MakeTask(string name, string[] names)
		{
			if (_RunningTask == name)
			{
				throw new InvalidOperationException("Same name task is running.");
			}

			// 동일 이름의 settingTable이 있다면 삭제 한다.
			if (SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("Same Task exist.", "VirtualDAQ");
			}

			DataTableStruct dts = SettingDataDic[names[0]];
			CreateTask(name, dts.setting);
			Ready(name);

			ScanGenerator generator = new ScanGenerator();
			SettingScanner setting;

			int length = 0;
			short[,] temp = null;
			short[,] ao;
			List<short[,]> aodatas = new List<short[,]>();
			foreach (string nam in names)
			{
				setting = SettingDataDic[nam].setting;

				generator.Divid = (int)(setting.AiClock / setting.AoClock);
				generator.FrameSize = new System.Drawing.Size(setting.FrameWidth, setting.FrameHeight);
				generator.RatioX = setting.RatioX;
				generator.RatioY = setting.RatioY;
				generator.ShiftX = setting.ShiftX;
				generator.ShiftY = setting.ShiftY;

				temp = generator.Generate();

				length += temp.GetLength(1);

				aodatas.Add(temp);
			}

			ao = new short[temp.GetLength(0), length];

			int cnt = 0;
			foreach (short[,] list in aodatas)
			{
				for (int i = 0; i < list.GetLength(1); i++)
				{
					ao[0, cnt] = list[0, i];
					ao[1, cnt++] = list[1, i];
				}
			}
			dts = SettingDataDic[name];
			dts.aoData = ao;
		}
		#endregion

		#region Setting
		public SettingScanner GetSetting(string name)
		{
			if (!SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("There are no same task.", "VirtualDAQ");
			}
			return SettingDataDic[name].setting;
		}

		public void SetSetting(string name, SettingScanner sDAQ)
		{
			if (!SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("There are no same task.", "VirtualDAQ");
			}
			DataTableStruct dts = SettingDataDic[name];
			if (dts.IsReady)
			{
				throw new InvalidOperationException("Setting is ready.");
			}
			if (!dts.ModifyAble)
			{
				throw new InvalidOperationException("This setting is not support modify.");
			}
			dts.setting = sDAQ;
		}
		#endregion

		#region 동작
		public void Ready(string name)
		{
			if (!SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("Same Task doesn't exist.");
			}

			DataTableStruct dts = SettingDataDic[name];

			if (dts.IsReady)
			{
				throw new InvalidOperationException("Setting already.");
			}

			SettingScanner sDaq = SettingDataDic[name].setting;

			ScanGenerator generator = new ScanGenerator();
			generator.Divid = (int)(sDaq.AiClock / sDaq.AoClock);
			generator.FrameSize = new System.Drawing.Size(sDaq.FrameWidth, sDaq.FrameHeight);
			generator.RatioX = sDaq.RatioX;
			generator.RatioY = sDaq.RatioY;
			generator.ShiftX = sDaq.ShiftX;
			generator.ShiftY = sDaq.ShiftY;

			dts.aoData = generator.Generate();

			dts.IsReady = true;
		}

		public void Release(string name)
		{
			if (!SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("Same Task doesn't exist.");
			}

			DataTableStruct dts = SettingDataDic[name];

			if (!dts.IsReady)
			{
				throw new InvalidOperationException("Setting doesn't already.");
			}

			dts.aoData = null;

			dts.IsReady = false;
		}

		public void Start(string name)
		{
			if (_enable)
			{
				throw new InvalidOperationException("Task is running.");
			}

			if (!SettingDataDic.ContainsKey(name))
			{
				throw new ArgumentException("Same Task doesn't exist.");
			}

			DataTableStruct dts = SettingDataDic[name];

			if (!dts.IsReady)
			{
				throw new InvalidOperationException("Task is not ready.");
			}

			int dataCnt = 0;
			// 일정 라인씩 업데이트는 frameHeight가 2의 배수가 아닐 수 있으므로 불가.
			if ((double)dts.setting.FrameWidth * dts.setting.SampleComposite / dts.setting.AiClock > 0.01)	// GDI+가 초당 10번 이상 그리기 를 시도 할경우 시스템 반응속도가 느려짐.
			{
				// 한줄씩 업데이트
				dataCnt = dts.setting.FrameWidth * dts.setting.SampleComposite;
			}
			else
			{
				// 프레임 전체 업데이트.
				dataCnt = dts.setting.FrameWidth * dts.setting.FrameHeight * dts.setting.SampleComposite;
			}

			_ReadAvailables = 0;
			dataIncrease = dataCnt;
			linePnt = 0;
			runHeight = dts.setting.FrameHeight;
			runWidth = dts.setting.FrameWidth;
			runDts = dts;

			// timer가 ms 단위이므로 sec 단위로 변경하기 위해 1000을 곱함.
			dataGenerateTimer.Change((long)((long)dataCnt  * 1000 / dts.setting.AiClock), (long)((long)dataCnt * 1000 / dts.setting.AiClock));



			_enable = true;

			_RunningTask = name;

			Debug.WriteLine("VirtualDAQ Started - " + name);
		}

		public void Stop()
		{
			Debug.WriteLine("Try VirtualDAQ Stopped");

			if (!_enable)
			{
				throw new InvalidOperationException("Task is not running.");
			}

			dataGenerateTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

			_enable = false;
			_RunningTask = "";
		}
		#endregion

		public short[,] Read(int samples)
		{
			long tmp, tmp2;

			long cnt = samples < _ReadAvailables ? samples : _ReadAvailables;

			_ReadAvailables -= (int)cnt;

			long index = 0;
			short[,] result = new short[1, cnt];

			int divider = (int)(runDts.setting.AiClock / runDts.setting.AoClock);

			if ((cnt /( runWidth * runDts.setting.SampleComposite)) > linePnt)
			{

				tmp = runHeight * runWidth * runDts.setting.SampleComposite;
				tmp2 = (cnt / (runWidth * runDts.setting.SampleComposite) - linePnt) * runWidth * runDts.setting.SampleComposite;
				for (long j = tmp - tmp2;
								j < tmp;
								j++)
				{
					result[0, index++] = (short)((runDts.aoData[0, j / divider / runDts.setting.SampleComposite] + runDts.aoData[1, j / divider / runDts.setting.SampleComposite]) / 2);
				}

				cnt -= tmp2;
			}

			tmp = (linePnt - cnt / (runWidth * runDts.setting.SampleComposite)) * runWidth * runDts.setting.SampleComposite;

			for (long i = tmp; 
					  i < tmp + cnt; 
					  i++)
			{
				result[0, index++] = (short)((runDts.aoData[0, i / divider / runDts.setting.SampleComposite] + runDts.aoData[1, i / divider / runDts.setting.SampleComposite]) / 2);
			}
			return result;
		}

		class DataTableStruct
		{
			public SettingScanner setting;
			public short[,] aoData;
			public string Name;
			public bool IsReady;
			public bool ModifyAble;

			public DataTableStruct(string name, SettingScanner set, short[,] data, bool ready, bool moddify)
			{
				Name = name;
				setting = set;
				aoData = data;
				IsReady = ready;
				ModifyAble = moddify;
			}
		}

		#region IDAQ 멤버


		public bool VaildateSetting(SettingScanner setting)
		{
			return true;
		}

		#endregion

		#region IDAQ 멤버


		public SettingScanner[] Settings
		{
			get { throw new NotImplementedException(); }
		}

		public void Start(SettingScanner[] set)
		{
			throw new NotImplementedException();
		}

		public void ValidateSetting(SettingScanner setting)
		{
			throw new NotImplementedException();
		}

		public void ValidateSequenceRunable(SettingScanner[] setting)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDAQ 멤버


		public SettingScanner[] SettingsRunning
		{
			get { throw new NotImplementedException(); }
		}

		public SettingScanner[] SettingsReady
		{
			get { throw new NotImplementedException(); }
		}

		public void Ready(SettingScanner[] set)
		{
			throw new NotImplementedException();
		}

		public void Change()
		{
			throw new NotImplementedException();
		}

		#endregion

		public void ShowInformation(System.Windows.Forms.IWin32Window owner)
		{
			throw new NotImplementedException();
		}

		public void OnePoint(double horizontal, double vertical)
		{
			throw new NotImplementedException();
		}

		#region IDAQ 멤버


		public void Reset()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
