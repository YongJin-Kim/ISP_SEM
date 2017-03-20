using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SEC.Nanoeye.NanoStage.MMCValue;
using SEC.Nanoeye.NanoStage.MMCValue.MMCAPICollection;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage
{
	// 포지션 및 속도에 관해서는
	// 장비별로 고정 값이고, User가 변경시 스테이지에 큰 부담을 줄 수 있으므로,
	// 하드코딩 한다.

	/// <summary>
	/// Stage Controller for Normal SEM VER.0 (5Axis. X-Y-R-T-Z)
	/// </summary>
	internal class StageMmcNsv0 : SECtype.ControllerBase, IStageNormalSEM
	{
		#region Property & Variables
		private int homeSearchState = 0;

		public override bool Enable
		{
			get { return base.Enable; }
			set
			{
				_AxisX.Enable = value;
				_AxisY.Enable = value;
				_AxisR.Enable = value;
				_AxisT.Enable = value;
				_AxisZ.Enable = value;

				MMCValue.MMCAPICollection.IOPort.set_io((short)0, (uint)(value ? ~1 : 0xfffff));

				base.Enable = value;
			}
		}

		private bool _IsHomeSearched = false;
		public bool IsHomeSearched
		{
			get { return _IsHomeSearched; }
		}

		public bool[] LimitSensor
		{
			get { throw new NotImplementedException(); }
		}

		private System.Threading.Timer _SyncTimer = null;
		#endregion

		#region Event
		public event EventHandler SyncTimerTick;

		public event EventHandler  HomeSearchStateChanged;
		protected virtual void OnHomeSearchStateChanged()
		{
			if (HomeSearchStateChanged != null)
			{
				HomeSearchStateChanged(this, EventArgs.Empty);
			}
		}

		protected override void OnEnabelChanged()
		{
			base.OnEnabelChanged();

			TimerSync();
		}

		#endregion

		public StageMmcNsv0()
		{
			_SyncTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SyncTimerCallback));
			//_SyncTimer.Change(100, 100);
		}

		private void SyncTimerCallback(object state)
		{
			if (SyncTimerTick != null)
			{
				SyncTimerTick(this, EventArgs.Empty);
			}
		}

		public override string[] AvailableDevices()
		{
			List<string> result = new List<string>();
			int addr;
			try
			{
				unchecked { addr = (int)(0xD8000000); }
				ErrorControl.Assert(MMCValue.MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8400000); }
				ErrorControl.Assert(MMCValue.MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8800000); }
				ErrorControl.Assert(MMCValue.MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8C00000); }
				ErrorControl.Assert(MMCValue.MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			return result.ToArray();
		}

		public override void Initialize()
		{
			if (_Device == null)
			{
				throw new InvalidOperationException("사용할 Board가 정의되지 않았음.");
			}

			int addr = int.Parse(_Device);
			ErrorControl.Assert(MMCValue.MMCAPICollection.Initialize.mmc_initx(1, ref addr));

			//#if DEBUG
			//#else
			if (MMCValue.MMCAPICollection.MMCConfig.mmc_all_axes() < 5)
			{
				throw new InvalidOperationException("There are few axes.");
			}
			//#endif

			AxisMMC ax;
			MMCValue.AxisValueLong avl;
			MMCValue.MMCPosition axPos;

			#region X축 초기화
			ax = new AxisMMC();
			ax.BeginInit();
			ax.Name = "Xaxis";
			ax.Owner = this;
			ax.BoardNumber = 0;
			ax.AxisNumber = 0;

			// 최대 80mm
			axPos = new MMCPosition();
			axPos.BeginInit();
			axPos.DefaultMax = 40000000;
			axPos.DefaultMin = -40000000;
			axPos.Maximum = 40000000;	// 40mm
			axPos.Minimum = -40000000;	// -40mm
			axPos.Value = 0;	// 0m
			axPos.Precision = 3.14 * 6 * 14 * 1000000 / 360 / 400;// 732666.666 nm per rol
			axPos.Owner = ax;
			axPos.Name = ax.Name + "-Position";
			axPos.ErrorState = false;
			axPos.ValueMargin = 1;
			axPos.EndInit();
			ax.Position = axPos;

			// 0.7536mm / roll
			// 400 pules / roll
			avl = new AxisValueLong();
			avl.BeginInit();
			avl.DefaultMax = 4000000;	// 4mm/sec
			avl.DefaultMin = 2000;		// 2um/sec
			avl.Maximum = avl.DefaultMax;	// 4mm/sec
			avl.Minimum = avl.DefaultMin;	// 2um/sec
			avl.Value = 20000;	// 20um/sec
			avl.Precision = 1 / axPos.Precision;
			avl.Owner = ax;
			avl.Name = ax.Name + "-Speed";
			avl.EndInit();
			ax.Speed = avl;

			ax.Enable = false;
			ax.EndInit();
			controls.Add(ax.Name, ax);
			_AxisX = ax;
			//축 운동에 맞춰서 IO동작. 사용 하지 않음. ax.EnableChanged += new EventHandler(ax_EnableChanged);
			#endregion

			#region Y축
			ax = new AxisMMC();
			ax.BeginInit();
			ax.Name = "Yaxis";
			ax.Owner = this;
			ax.BoardNumber = 0;
			ax.AxisNumber = 1;

			// 최대 100mm
			axPos = new MMCPosition();
			axPos.BeginInit();
			axPos.DefaultMax = 50000000; // 1m
			axPos.DefaultMin = -50000000; // 1m
			axPos.Maximum = 50000000;	// 50mm
			axPos.Minimum = -50000000;	// -50mm
			axPos.Value = 0;	// 0m
			axPos.Precision = 500000d / 400;// 500nm per 1roll
			axPos.Owner = ax;
			axPos.Name = ax.Name + "-Position";
			axPos.ErrorState = false;
			axPos.ValueMargin = 1;
			axPos.EndInit();
			ax.Position = axPos;


			// 0.5mm / roll
			// 400 pules / roll
			avl = new AxisValueLong();
			avl.BeginInit();
			avl.DefaultMax = 4000000;	// 4mm/sec
			avl.DefaultMin = 2000;		// 2um/sec
			avl.Maximum = avl.DefaultMax;	// 4mm/sec
			avl.Minimum = avl.DefaultMin;	// 2um/sec
			avl.Value = 20000;	// 20um/sec
			avl.Precision = 400d / 500000d;	// 400 pules / 500 nm
			avl.Owner = ax;
			avl.Name = ax.Name + "-Speed";
			avl.EndInit();
			ax.Speed = avl;

			ax.Enable = false;
			ax.EndInit();
			controls.Add(ax.Name, ax);
			_AxisY = ax;
			//축 운동에 맞춰서 IO동작. 사용 하지 않음.ax.EnableChanged += new EventHandler(ax_EnableChanged);
			#endregion

			#region R축.
			// 200 pules / roll
			ax = new AxisMMC();
			ax.BeginInit();
			ax.Name = "Raxis";
			ax.Owner = this;
			ax.BoardNumber = 0;
			ax.AxisNumber = 2;

			axPos = new MMCPosition();
			axPos.BeginInit();
			axPos.DefaultMax = 180000; // 1m
			axPos.DefaultMin = -180000; // 1m
			axPos.Maximum = 180000;	// 1m
			axPos.Minimum = -180000;	// 1m
			axPos.Value = 0;	// 0m
			axPos.Precision = 360 * 1000 / 98 / 400;// 3도 4분 per 1roll
			axPos.Owner = ax;
			axPos.Name = ax.Name + "-Position";
			axPos.ErrorState = false;
			axPos.ValueMargin = 1;
			axPos.EndInit();
			ax.Position = axPos;

			// 0.5mm / roll
			// 400 pules / roll
			avl = new AxisValueLong();
			avl.BeginInit();
			avl.DefaultMax = (long)((3 + 4d / 6d) * 1000 * 2.5);	// 9.167도/sec
			avl.DefaultMin = (long)Math.Ceiling((3 + 4d / 6d) * 1000 / 400);		// 0.01도/sec
			avl.Maximum = avl.DefaultMax;	// 28.8도/sec
			avl.Minimum = avl.DefaultMin;	// 1.8도/sec
			avl.Value = avl.Minimum * 10;	// 7.2도/sec
			avl.Precision = 1 / axPos.Precision;	// 400 pules /  3도 4분
			avl.Owner = ax;
			avl.Name = ax.Name + "-Speed";
			avl.EndInit();
			ax.Speed = avl;

			ax.Enable = false;
			ax.EndInit();
			controls.Add(ax.Name, ax);
			_AxisR = ax;
			//축 운동에 맞춰서 IO동작. 사용 하지 않음. ax.EnableChanged += new EventHandler(ax_EnableChanged);
			#endregion

			#region T축
			ax = new AxisMMC();
			ax.BeginInit();
			ax.Name = "Taxis";
			ax.Owner = this;
			ax.BoardNumber = 0;
			ax.AxisNumber = 3;

			// -20 ~ 90 degree
			axPos = new MMCPosition();
			axPos.BeginInit();
			axPos.DefaultMax = +90000; // 90도
			axPos.DefaultMin = -20000; // -20도m
			axPos.Maximum = +90000;	// 1m
			axPos.Minimum = -20000;	// 1m
			axPos.Value = 0;	// 0m
			axPos.Precision = -360 * 1000 / 100 / 400;// 7.2도 per 1roll
			axPos.Owner = ax;
			axPos.Name = ax.Name + "-Position";
			axPos.ErrorState = false;
			axPos.ValueMargin = 1;
			axPos.EndInit();
			ax.Position = axPos;

			// 3도 36분/ roll
			// 400 pules / roll
			avl = new AxisValueLong();
			avl.BeginInit();
			avl.DefaultMax = 9000;	// 9도/sec
			avl.DefaultMin = 9;		// 0.009도/sec
			avl.Maximum = 9000;	// 9도/sec
			avl.Minimum = 9;	// 0.009도/sec
			avl.Value = 90;	// 7.2도/sec
			avl.Precision = 1 / axPos.Precision;	// 200 pules / 7.2도
			avl.Owner = ax;
			avl.Name = ax.Name + "-Speed";
			avl.EndInit();
			ax.Speed = avl;

			ax.Enable = false;
			ax.EndInit();
			controls.Add(ax.Name, ax);
			_AxisT = ax;
			//축 운동에 맞춰서 IO동작. 사용 하지 않음. ax.EnableChanged += new EventHandler(ax_EnableChanged);
			#endregion

			#region Z축
			ax = new AxisMMC();
			ax.BeginInit();
			ax.Name = "Zaxis";
			ax.Owner = this;
			ax.BoardNumber = 0;
			ax.AxisNumber = 4;

			// -19mm ~ 36mm
			// home at 15mm
			axPos = new MMCPosition();
			axPos.BeginInit();
			axPos.DefaultMax = 31000000; // 1m
			axPos.DefaultMin = -19000000; // 1m
			axPos.Maximum = 31000000;	// 36mm
			axPos.Minimum = -19000000;	// -19mm
			axPos.Value = 15000000;	// 15mm
			axPos.Precision = 1000000d / 800; // 1mm per 1roll
			axPos.Owner = ax;
			axPos.Name = ax.Name + "-Position";
			axPos.ErrorState = false;
			axPos.ValueMargin = 1;
			axPos.EndInit();
			ax.Position = axPos;

			// 1mm / roll
			// 200 pules / roll
			avl = new AxisValueLong();
			avl.BeginInit();
			avl.DefaultMax = 5000000;	// 5mm/sec
			avl.DefaultMin = 2500;		// 2.5um/sec
			avl.Maximum = 5000000;	// 5mm/sec
			avl.Minimum = 2500;	// 5um/sec
			avl.Value = 10000;	// 500um/sec
			avl.Precision = 800d / 1000000;	// 200 pulse / 1mm
			avl.Owner = ax;
			avl.Name = ax.Name + "-Speed";
			avl.EndInit();
			ax.Speed = avl;

			ax.Enable = false;
			ax.EndInit();
			controls.Add(ax.Name, ax);
			_AxisZ = ax;
			//축 운동에 맞춰서 IO동작. 사용 하지 않음. ax.EnableChanged += new EventHandler(ax_EnableChanged);
			#endregion

			_Initialized = true;

			TimerSync();
		}

		#region 축 운동에 맞춰서 IO동작. 사용 하지 않음.
		/*
		List<AxisMMC> enabledLit = new List<AxisMMC>();

		void ax_EnableChanged(object sender, EventArgs e)
		{
			AxisMMC ax = sender as AxisMMC;
			if (ax.Enable)
			{
				IOampChange(true);
				if (!enabledLit.Contains(ax)) { enabledLit.Add(ax); }
			}
			else
			{
				if (homeSearchState == 0)
				{
					IOampChange(false);
					enabledLit.Remove(ax);
				}
			}

		}

		private void IOampChange(bool en)
		{
			unchecked
			{
				MMCValue.MMCAPICollection.IOPort.set_io((short)0, (en ? (uint)(~1) : (uint)(0xfffff)));
			}
		}
		*/
		#endregion

		private void TimerSync()
		{
			if (_Initialized && _Enable)
			{
				_SyncTimer.Change(100, 100);
			}
			else
			{
				_SyncTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
			}
		}

		public override int ControlBoard(out string[,] information)
		{
			short num = 0;
			int temp = 0;

			information = new string[4, 2];

			information[0, 0] = "Version";
			MMCConfig.version_chk(0, ref num);
			information[0, 1] = num.ToString();

			information[1, 0] = "Board FPGA";
			ETC.motion_fpga_version_chk(0, ref num);
			information[1, 1] = num.ToString();

			information[2, 0] = "Option FPGA";
			ETC.option_fpga_version_chk(0, ref num);
			information[2, 1] = num.ToString();

			information[3, 0] = "DPRAM";
			MMCValue.MMCAPICollection.Initialize.get_dpram_addr(0, ref temp);
			information[3, 1] = num.ToString();
			return 4;
		}

		public override string GetControllerType()
		{
			return "StageControlByMMC";
		}

		#region IStageNormalSEM 멤버

		private AxisMMC _AxisX;
		public IAxis AxisX
		{
			get
			{
				Trace.Assert(_Initialized, "Stage is not initialized");
				return _AxisX;
			}
		}

		private AxisMMC _AxisY;
		public IAxis AxisY
		{
			get
			{
				Trace.Assert(_Initialized, "Stage is not initialized");
				return _AxisY;
			}
		}

		private AxisMMC _AxisR;
		public IAxis AxisR
		{
			get
			{
				Trace.Assert(_Initialized, "Stage is not initialized");
				return _AxisR;
			}
		}

		private AxisMMC _AxisT;
		public IAxis AxisT
		{
			get
			{
				Trace.Assert(_Initialized, "Stage is not initialized");
				return _AxisT;
			}
		}

		private AxisMMC _AxisZ;
		public IAxis AxisZ
		{
			get { return controls["Zaxis"] as IAxis; }
		}

		#endregion

		#region HomeSearch
		// HomeSearch 방법
		// 1. Z축을 제일 밑으로 내림
		// 2-1. X-Y축 Positive limit로 이동, 
		// 2-2 T축 Negative limit로 이동.
		// 3. 2번이 모드 끝나면 T-Z축 Home-Sensor 10% 근처까지 고속 이동.
		// 3-1. 2-1번이 끝나면. X-Y축 중심점으로 이동, 
		// 4. T-Z축 Home-Sensor로 저속 이동.
		

		Dictionary<AxisMMC,double[]> homesearchValueList;
		public void HomeSearch(bool value)
		{
			homeSearchState = 0;

			// 모든 축 정지 요청
			AxisX.Stop(false);
			AxisY.Stop(false);
			AxisR.Stop(false);
			AxisT.Stop(false);
			AxisZ.Stop(false);

			//축 운동에 맞춰서 IO동작. 사용 하지 않음. IOampChange(false);

			// HomeSearch가 종료되는 경우.
			if (!value) { return; }

			//축 운동에 맞춰서 IO동작. 사용 하지 않음. IOampChange(true);

			if (_IsHomeSearched)
			{
				_IsHomeSearched = false;
				OnHomeSearchStateChanged();
			}

			// 모든 축이 정지 되기를 기다림.
			while (AxisX.IsMotion.Value) { continue; }
			while (AxisY.IsMotion.Value) { continue; }
			//while (AxisR.IsMotion.Value) { continue; }
			while (AxisT.IsMotion.Value) { continue; }
			while (AxisZ.IsMotion.Value) { continue; }

			_AxisX.IsPositionSync = false;
			_AxisY.IsPositionSync = false;
			_AxisT.IsPositionSync = false;
			_AxisZ.IsPositionSync = false;

			// Z축은 Positive Limit로 가면 안됨

			homesearchValueList = new Dictionary<AxisMMC, double[]>();
			homesearchValueList.Add(_AxisX, new double[] { 1, _AxisX.Position.Maximum, _AxisX.Position.Minimum, 0 });
			homesearchValueList.Add(_AxisY, new double[] { 2, _AxisY.Position.Maximum, _AxisY.Position.Minimum, 0 });
			homesearchValueList.Add(_AxisT, new double[] { 4, _AxisT.Position.Maximum, _AxisT.Position.Minimum, 0 });
			homesearchValueList.Add(_AxisZ, new double[] { 8, _AxisZ.Position.Maximum, _AxisZ.Position.Minimum, 0 });

			homeSearchState |= (int)homesearchValueList[_AxisX][0];
			homeSearchState |= (int)homesearchValueList[_AxisY][0];
			homeSearchState |= (int)homesearchValueList[_AxisT][0];
			homeSearchState |= (int)homesearchValueList[_AxisZ][0];

			axTZmovingHome = false;

			if ((_AxisX.LimitState & LimitSensorEnum.Positive) != LimitSensorEnum.Positive)
			{
				_AxisX.IsMotion.ValueChanged += new EventHandler(MoveToPosX_ValueChanged);
				_AxisX.Speed.Value = _AxisT.Speed.Maximum;
				_AxisX.MoveVelocity(true);
			}
			else
			{
				MoveToPosX_ValueChanged(_AxisX.IsMotion, EventArgs.Empty);
			}

			if ((_AxisZ.LimitState & LimitSensorEnum.Positive) != LimitSensorEnum.Positive)
			{
				// 출발 하고 바로 리미트에 다다를 수 있으므로, 이벤트 연결을 번저 실행 함.
				_AxisZ.IsMotion.ValueChanged += new EventHandler(MoveToPosZ_ValueChanged);
				_AxisZ.Speed.Value = _AxisZ.Speed.Maximum;
				_AxisZ.MoveVelocity(true);
			}
			else
			{
				MoveToPosZ_ValueChanged(_AxisZ.IsMotion, EventArgs.Empty);
			}

			if ((_AxisT.LimitState & LimitSensorEnum.Positive) != LimitSensorEnum.Positive)
			{
				_AxisT.IsMotion.ValueChanged += new EventHandler(MoveToPosT_ValueChanged);
				_AxisT.Speed.Value = _AxisT.Speed.Maximum;
				_AxisT.MoveVelocity(true);
			}
			else
			{
				MoveToPosT_ValueChanged(_AxisT.IsMotion, EventArgs.Empty);
			}
		}

		void MoveToPosX_ValueChanged(object sender, EventArgs e)
		{
			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			// 이벤트 열결이 먼저 되고 후에 동작 하므로,
			// 동작 시작시 이벤트가 발생 함.
			if (ax.IsMotion.Value) { return; }
			ax.IsMotion.ValueChanged -= new EventHandler(MoveToPosX_ValueChanged);
		}


		void MoveToPosZ_ValueChanged(object sender, EventArgs e)
		{
		}

		private void MoveToPosT_ValueChanged(object sender, EventArgs eventArgs)
		{
		}

		void MoveToNegZ_ValueChanged(object sender, EventArgs e)
		{
			// 2. Z축 Negative limit로 이동 완료.
			//    X-Y 축 Negative limit로 이동.
			//    동시에 T축 Negative limit로 이동. (Positive limit로 이동 시 시료대 높이가 올라가서
			//											final apertuer에 부딛힐 수 있으므로 안됨.)

			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			// 이벤트 열결이 먼저 되고 후에 동작 하므로,
			// 동작 시작시 이벤트가 발생 함.
			if (ax.IsMotion.Value) { return; }
			ax.IsMotion.ValueChanged -= new EventHandler(MoveToNegZ_ValueChanged);

			_AxisZ.Position.Value = _AxisZ.Position.Maximum;

			axXPosLimitTouched = false;
			axYPosLimitTouched = false;

			// 2-1.  X 축을 Positive limit로 이동.
			if (_AxisX.LimitState != LimitSensorEnum.Negative)
			{
				// 출발 하고 바로 리미트에 다다를 수 있으므로, 이벤트 연결을 번저 실행 함.
				_AxisX.IsMotion.ValueChanged += new EventHandler(MoveToPosXY_ValueChanged);
				_AxisX.Speed.Value = _AxisX.Speed.Maximum;
				_AxisX.MoveVelocity(false);
			}
			else
			{
				MoveToPosXY_ValueChanged(_AxisX.IsMotion, EventArgs.Empty);
			}

			// 2-1.  Y 축을 Positive limit로 이동.
			if (_AxisY.LimitState != LimitSensorEnum.Negative)
			{
				// 출발 하고 바로 리미트에 다다를 수 있으므로, 이벤트 연결을 번저 실행 함.
				_AxisY.IsMotion.ValueChanged += new EventHandler(MoveToPosXY_ValueChanged);
				_AxisY.Speed.Value = _AxisY.Speed.Maximum;
				_AxisY.MoveVelocity(false);
			}
			else
			{
				MoveToPosXY_ValueChanged(_AxisY.IsMotion, EventArgs.Empty);
			}

			// 2-2.  T 축을 Negative limit로 이동.
			if (_AxisT.LimitState != LimitSensorEnum.Negative)
			{
				// 출발 하고 바로 리미트에 다다를 수 있으므로, 이벤트 연결을 번저 실행 함.
				_AxisT.IsMotion.ValueChanged += new EventHandler(MoveToPosT_ValueChanged);
				_AxisT.Speed.Value = _AxisT.Speed.Maximum;
				_AxisT.MoveVelocity(true);
			}
			else
			{
				MoveToPosT_ValueChanged(_AxisT.IsMotion, EventArgs.Empty);
			}
		}

		private bool axXPosLimitTouched = false;
		private bool axYPosLimitTouched = false;

		//private void MoveToPosT_ValueChanged(object sender, EventArgs eventArgs)
		//{
		//    AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
		//    // 이벤트 열결이 먼저 되고 후에 동작 하므로,
		//    // 동작 시작시 이벤트가 발생 함.
		//    if (ax.IsMotion.Value) { return; }
		//    ax.IsMotion.ValueChanged -= new EventHandler(MoveToPosT_ValueChanged);

		//    _AxisT.Position.Value = _AxisT.Position.DefaultMax;

		//    if (axXPosLimitTouched && axYPosLimitTouched)
		//    {
		//        // 3. 2번이 모드 끝나면 T-Z축 Home-Sensor 10% 근처까지 고속 이동.
		//        MoveAxToHome();
		//    }
		//}

		private void MoveToPosXY_ValueChanged(object sender, EventArgs eventArgs)
		{
			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			// 이벤트 열결이 먼저 되고 후에 동작 하므로,
			// 동작 시작시 이벤트가 발생 함.
			if (ax.IsMotion.Value) { return; }
			ax.IsMotion.ValueChanged -= new EventHandler(MoveToPosXY_ValueChanged);

			if (ax == _AxisX) { axXPosLimitTouched = true; }
			else if (ax == _AxisY) { axYPosLimitTouched = true; }
			else
			{
				System.Diagnostics.Trace.WriteLine("XY Poslimit 이동 완료 함수에 이상한 값이...");
				throw new ArgumentException("XY Poslimit 이동 완료 함수에 이상한 값이...");
			}

			ax.Position.Value = ax.Position.Minimum;
			ax.IsMotion.ValueChanged += new EventHandler(MoveComplete_ValueChanged);

			ax.MovePosition(0, false);

			if (axXPosLimitTouched && axYPosLimitTouched && !_AxisT.IsMotion.Value)
			{
				// 3. 2번이 모드 끝나면 T-Z축 Home-Sensor 10% 근처까지 고속 이동.
				MoveAxToHome();
			}
		}

		bool axTZmovingHome = false;

		private void MoveAxToHome()
		{
			System.Diagnostics.Trace.WriteLine("Try to go home.");

			// 3. 2번이 모드 끝나면 T-Z축 Home-Sensor 10% 근처까지 고속 이동.

			_AxisT.Speed.Value = _AxisT.Speed.Maximum;
			_AxisZ.Speed.Value = _AxisZ.Speed.Maximum;

			_AxisT.Position.Value = _AxisT.Position.DefaultMax;
			_AxisZ.Position.Value = _AxisZ.Position.DefaultMax;

			MMCValue.MMCAPICollection.LimitsOfHardware.set_home(_AxisT.AxisLogicNumber, (int)MMCValue.MMCAPICollection.Enumurations.EventNumber.EStop);
			MMCValue.MMCAPICollection.LimitsOfHardware.set_home(_AxisZ.AxisLogicNumber, (int)MMCValue.MMCAPICollection.Enumurations.EventNumber.EStop);

			_AxisT.IsMotion.ValueChanged += new EventHandler(RearHome_ValueChanged);
			_AxisZ.IsMotion.ValueChanged += new EventHandler(RearHome_ValueChanged);

			_AxisZ.MovePosition((_AxisZ.Position.DefaultMax - _AxisZ.HomePosition) / 10 + _AxisZ.HomePosition, false);
			_AxisT.MovePosition((_AxisT.Position.DefaultMax - _AxisT.HomePosition) / 10 + _AxisT.HomePosition, false);
		}

		void RearHome_ValueChanged(object sender, EventArgs e)
		{
			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			if (ax.IsMotion.Value) { return; }
			ax.IsMotion.ValueChanged -= new EventHandler(RearHome_ValueChanged);

			ax.Speed.Value = ax.Speed.Minimum;
			ax.IsMotion.ValueChanged += new EventHandler(AxHome_ValueChanged);
			ax.MoveVelocity(false);
		}

		void AxHome_ValueChanged(object sender, EventArgs e)
		{
			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			if (ax.IsMotion.Value) { return; }
			ax.IsMotion.ValueChanged -= new EventHandler(RearHome_ValueChanged);

			MMCValue.MMCAPICollection.LimitsOfHardware.set_home(ax.AxisLogicNumber, (int)MMCValue.MMCAPICollection.Enumurations.EventNumber.No);
			MMCValue.MMCAPICollection.ControlState.clear_status(ax.AxisLogicNumber);

			CheckHomesearchEnd(ax);
		}

		/// <summary>
		/// Home 이동 완료 확인
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MoveComplete_ValueChanged(object sender, EventArgs e)
		{
			AxisMMC ax = (sender as SECtype.IControlValue).Owner as AxisMMC;
			if (ax.IsMotion.Value) { return; }	// 이동 중
			ax.IsMotion.ValueChanged -= new EventHandler(MoveComplete_ValueChanged);

			CheckHomesearchEnd(ax);
		}
				     
		private void CheckHomesearchEnd(AxisMMC ax)
		{
			ax.Enable = false;
			homeSearchState &= ~((int)homesearchValueList[ax][0]);
			if (homeSearchState == 0)
			{
				if (!_IsHomeSearched)
				{
					_IsHomeSearched = true;
					//축 운동에 맞춰서 IO동작. 사용 하지 않음. IOampChange(false);
					OnHomeSearchStateChanged();
				}
			}
		}

		#endregion

		public void EmergencyStop()
		{
			//foreach (KeyValuePair<string, SECtype.IControlValue> kvp in controls)
			//{
			//    SECtype.IControlValue icv = kvp.Value;
			//    if (icv is IAxis)
			//    {
			//        IAxis ax = icv as IAxis;
			//        ax.Stop(true);
			//    }
			//}
			_AxisX.Stop(true);
			_AxisY.Stop(true);
			_AxisR.Stop(true);
			_AxisT.Stop(true);
			_AxisZ.Stop(true);
		}
	}
}
