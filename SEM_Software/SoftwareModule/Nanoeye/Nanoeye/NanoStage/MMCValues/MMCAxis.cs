using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SECtype = SEC.GenericSupport.DataType;


using SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection;

namespace SEC.Nanoeye.NanoStage.MMCValues
{
	// Postion 의 사용 방법
	// Default 값은 설계상의 이동 값.
	// Maximum, Minium 값은 SW Limit로 사용.

	/// <summary>
	/// MMC의 축
	/// </summary>
	internal class MMCAxis : SECtype.ControlValueBase, IAxis
	{
		#region Property & Variables
		private System.Threading.Timer disableTimer;

		/// <summary>
		/// MMC는 보드 번호와 축번호를 이용하여 Logic Number를 생성, 이 숫자를 이용하여 축을 구별 한다.
		/// 단일 보드만 사용할 경우에는 _AxisNumber를 사용하여도 무방하나,
		/// 여러 보드를 사용할 경우를 대비하여 미리 적용해 둠.
		/// </summary>
		public short AxisLogicNumber = 0;

		#region 축 설정. 한번 설정되면 다시는 변경할 필요가 없는 값.
		public bool IsDirectionCCW
		{
			get
			{
				short dir = 0;
				ErrorControl.Assert(MMCAPICollection.AxisConfig.get_coordinate_direction(AxisLogicNumber, ref dir));
				return ((dir == 0) ? false : true);
			}
			set
			{
				AxisConfig.set_coordinate_direction(AxisLogicNumber, (short)(value ? 1 : 0));
			}

		}

		public bool IsEndlessMove
		{
			get
			{
				//short state = 0;
				//MMCAPICollection.EndlessMove.get_endless_rotationax(AxisLogicNumber, ref state);
				//return (state != 0);

				return _Position.EndlessMove;
			}
			set
			{
				if (_IsMotion) { throw new InvalidOperationException("Axis is running."); }

				//MMCAPICollection.EndlessMove.set_endless_rotationax(AxisLogicNumber, (short)(value ? 1 : 0), (short)_Resolution);
				////MMCAPICollection.EndlessMove.set_endless_linearax(AxisLogicNumber, (short)(value ? 1 : 0), (short)_Resolution);
				//MMCAPICollection.EndlessMove.set_endless_range(AxisLogicNumber, ((_Position.Maximum - _Position.Minimum) / _Position.Precision));

				//if(value)
				//{
				//    Position.Maximum = Position.DefaultMax + 1;
				//    Position.Minimum = Position.DefaultMin - 1;
				//}

				_Position.EndlessMove = value;
			}
		}

		private short _BoardNumber = 0;
		/// <summary>
		/// 사용할 MMC Board 번호.
		/// MMC에만 있는 값.
		/// </summary>
		public short BoardNumber
		{
			get { return _BoardNumber; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("Axis is running."); }
				_BoardNumber = value;

				ChangeLogicNumber();
			}
		}

		private short _AxisNumber = 0;
		/// <summary>
		/// MMC Board에서는 축 번호.
		/// MMC에만 있는 값.
		/// </summary>
		public short AxisNumber
		{
			get { return _AxisNumber; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("Axis is running."); }
				_AxisNumber = value;

				ChangeLogicNumber();
			}
		}

		private long _HomePosition = 0;
		public long HomePosition
		{
			get { return _HomePosition; }
			set { _HomePosition = value; }
		}

		private int _Resolution = 1;
		public int Resolution
		{
			get { return _Resolution; }
			set
			{
				if(_Enable) { throw new ArgumentException("Axis is Enabled."); }
				_Resolution = value;

				_Position.Precision = (double)_StepDistance / _Resolution;
				_Speed.Precision = _Position.Precision;

				//System.Diagnostics.Debug.WriteLine(_Name + " Pos   Precision - " + _Position.Precision.ToString());
				//System.Diagnostics.Debug.WriteLine(_Name + " Speed Precision - " + _Speed.Precision.ToString());
			}
		}

		private int _StepDistance = 1;
		public int StepDistance
		{
			get { return _StepDistance; }
			set
			{
				if (_Enable) { throw new ArgumentException("Axis is Enabled."); }
				_StepDistance = value;

				_Position.Precision = (double)_StepDistance / _Resolution;
				_Speed.Precision =  _Position.Precision;

				//System.Diagnostics.Debug.WriteLine(_Name + " Pos   Precision - " + _Position.Precision.ToString());
				//System.Diagnostics.Debug.WriteLine(_Name + " Speed Precision - " + _Speed.Precision.ToString());
			}
		}
		#endregion

		private SECtype.ControlLong _Speed = null;
		public SECtype.IControlLong Speed { get { return _Speed; } }

		private MMCPosition _Position = null;
		public IAxPosition Position { get { return _Position; } }

		//private MMCValue.AxisValueInt _LimitChannel = null;
		public SECtype.IControlInt LimitChannel
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		private SECtype.ControlInt _AccelTime = null;
		public SECtype.IControlInt AccelTime { get { return _AccelTime; } }

		private SECtype.ControlInt	_StopTime = null;
		public SECtype.IControlInt StopTime { get { return _StopTime; } }

		private SECtype.ControlInt _EmergencyStopTime = null;
		public SECtype.IControlInt EmergencyStopTime { get { return _EmergencyStopTime; } }

		private bool _IsMotion = false;
		public bool IsMotion
		{
			get
			{
				bool inMotion = ((MMCAPICollection.ActionState.axis_done(AxisLogicNumber) == 1) ? false : true);
				if (_IsMotion != inMotion)
				{
					_IsMotion = inMotion;
					OnMotionStateChanged();
				}
				return _IsMotion;
			}
		}

		public override bool Enable
		{
			get 
			{ 
				short state=0;
				AmpControl.get_amp_enable(AxisLogicNumber, ref state);

				if(base.Enable != (state == 1))
				{
					base.Enable = (state == 1);
				}

				return base.Enable; 
			}
			set
			{
				if (_IsInited)
				{
					//if (!value)
					//{
					//    System.Diagnostics.Trace.WriteLine("Amp Off\r\n" + System.Environment.StackTrace, this.ToString());
					//}

					if(value)
					{
						disableTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
					}

					ControlState.clear_status(AxisLogicNumber);
					AmpControl.set_amp_enable(AxisLogicNumber, (short)(value ? 1 : 0));
					System.Threading.Thread.Sleep(10);
					ControlState.clear_status(AxisLogicNumber);
				}
				base.Enable = (value & innerOwner.Enable);
			}
		}

		private MMCStage innerOwner = null;
		public override object Owner
		{
			get { return base.Owner; }
			set
			{
				if (_IsInited)
				{
					throw new InvalidOperationException("Axis is running.");
				}

				if (innerOwner != null)
				{
					innerOwner.EnableChanged -= new EventHandler(innerOwner_EnableChanged);
				}

				if (value is MMCStage)
				{
					innerOwner = value as MMCStage;

					base.Owner = value;

					innerOwner.EnableChanged += new EventHandler(innerOwner_EnableChanged);
				}
				else
				{
					throw new ArgumentException();
				}
			}
		}

		private LimitSensorEnum _LimitState = LimitSensorEnum.Non;
		public LimitSensorEnum LimitState
		{
			get
			{
				LimitSensorEnum lse = LimitSensorEnum.Non;
				short val = MMCAPICollection.ControlState.axis_source(AxisLogicNumber);
				int tmp = (int)MMCAPICollection.Enumurations.EventSourceStatus.HomeSwitch;
				if ((val & tmp) == tmp) { lse |= LimitSensorEnum.HW_Home; }
				tmp = (int)MMCAPICollection.Enumurations.EventSourceStatus.NegLimit;
				if ((val & tmp) == tmp) { lse |= LimitSensorEnum.HW_Negative; }
				tmp = (int)MMCAPICollection.Enumurations.EventSourceStatus.PosLimit;
				if ((val & tmp) == tmp) { lse |= LimitSensorEnum.HW_Positive; }
				tmp = (int)MMCAPICollection.Enumurations.EventSourceStatus.XNegLimit;
				if ((val & tmp) == tmp) { lse |= LimitSensorEnum.SW_Negative; }
				tmp = (int)MMCAPICollection.Enumurations.EventSourceStatus.XPosLimit;
				if ((val & tmp) == tmp) { lse |= LimitSensorEnum.SW_Positive; }
				if (lse != _LimitState)
				{
					_LimitState = lse;
					OnLimitStateChanged();
				}
				return _LimitState;
			}
		}

		private bool _AutoOff = true;
		public bool AutoOff
		{
			get { return _AutoOff; }
			set { _AutoOff = value; }
		}
		#endregion

		#region 생성자
		public MMCAxis()
		{
			_Speed = new SECtype.ControlLong();
			_Position = new MMCPosition();
			//_LimitChannel = new MMCValue.AxisValueInt();
			_AccelTime = new SECtype.ControlInt();
			_StopTime = new SECtype.ControlInt();
			_EmergencyStopTime = new SECtype.ControlInt();
			_IsMotion = new SECtype.ControlBool();

			_Speed.BeginInit();
			_Speed.DefaultMax = 10000000;	// 10mm/sec
			_Speed.DefaultMin = 100;		// 100nm/sec
			_Speed.Maximum = 10000000;	// 10mm/sec
			_Speed.Minimum = 100;	// 100nm/sec
			_Speed.Value = 1000000;	// 1mm/sec
			_Speed.Precision = 1;	// 1nm/sec
			_Speed.Owner = this;
			_Speed.Name = this.Name + "-Speed";
			_Speed.EndInit();

			_Position.BeginInit();
			_Position.Owner = this;
			_Position.Precision = 1;	// 1nm
			_Position.DefaultMax = 1000000000; // 1m
			_Position.DefaultMin = -1000000000; // 1m
			_Position.Maximum = 1000000000;	// 1m
			_Position.Minimum = -1000000000;	// 1m
			//_Position.Value = 0;	// 0m
			_Position.Name = this.Name + "-Position";
			_Position.EndInit();

			//_LimitChannel.BeginInit();
			//_LimitChannel.DefaultMax = 100;	// 100th
			//_LimitChannel.DefaultMin = 0;	// 0th
			//_LimitChannel.Maximum = 100;	// 100th
			//_LimitChannel.Minimum = 0;	// 0th
			//_LimitChannel.Value = 0;	// 0th
			//_LimitChannel.Precision = 1;	// 1
			//_LimitChannel.Owner = this;
			//_LimitChannel.Name = this.Name + "-LimitChannel";

			_AccelTime.BeginInit();
			_AccelTime.DefaultMax = 100000;	// 100sec
			_AccelTime.DefaultMin = 50;	// 50msec
			_AccelTime.Maximum = 100000;	// 100sec
			_AccelTime.Minimum = 100;	// 100msec
			_AccelTime.Value = 1000;	// 1sec
			_AccelTime.Precision = 1;	// 1msec
			_AccelTime.Owner = this;
			_AccelTime.Name = this.Name + "-AccelTime";
			_AccelTime.EndInit();

			_StopTime.BeginInit();
			_StopTime.DefaultMax = 100000;	// 100sec
			_StopTime.DefaultMin = 50;	// 50msec
			_StopTime.Maximum = 100000;	// 100sec
			_StopTime.Minimum = 100;	// 100msec
			_StopTime.Value = 1000;		// 1sec
			_StopTime.Precision = 1;	// 1msec
			_StopTime.Owner = this;
			_StopTime.Name = this.Name + "-DeaccelTime";
			_StopTime.EndInit();

			_EmergencyStopTime.BeginInit();
			_EmergencyStopTime.DefaultMax = 100000;	// 100sec
			_EmergencyStopTime.DefaultMin = 50;	// 50msec
			_EmergencyStopTime.Maximum = 100000;	// 100sec
			_EmergencyStopTime.Minimum = 100;	// 100msec
			_EmergencyStopTime.Value = 100;		// 100msec
			_EmergencyStopTime.Precision = 1;	// 1msec
			_EmergencyStopTime.Owner = this;
			_EmergencyStopTime.Name = this.Name + "-EmergencyDeaccelTime";
			_EmergencyStopTime.EndInit();

			disableTimer = new System.Threading.Timer(new System.Threading.TimerCallback(AmpDisable));
		}

		~MMCAxis()
		{
			Dispose();
		}

		public override void Dispose()
		{
			Enable = false;
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Event
		public event EventHandler  LimitStateChanged;
		protected virtual void OnLimitStateChanged()
		{
			if (LimitStateChanged != null)
			{
				LimitStateChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  MotionStateChanged;
		protected virtual void OnMotionStateChanged()
		{
			if (MotionStateChanged != null)
			{
				MotionStateChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		#region Sync
		/// <summary>
		/// Owner의 Enable과 동기화를 한다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void innerOwner_EnableChanged(object sender, EventArgs e)
		{
			//Enable = _EnableCore;
		}

		public override void Sync()
		{
			_Position.Sync();

			LimitSensorEnum lse = LimitState;

			bool inMotion = IsMotion;
			bool en	 = Enable;
		}

		public override bool Validate()
		{
			bool result = true;

			if (!_Position.Validate()) { result = false; }

			bool inMotion = ((MMCAPICollection.ActionState.motion_done(AxisLogicNumber) == 1) ? false : true);
			if (_IsMotion != inMotion) { result = false; }

			return result;
		}

		#endregion

		#region Init
		public override void BeginInit()
		{
			_IsInited = false;
			_Enable = false;

			_Speed.BeginInit();
			_Position.BeginInit();
			_AccelTime.BeginInit();
			_StopTime.BeginInit();
			_EmergencyStopTime.BeginInit();
		}

		public override void EndInit()
		{
			EndInit(false);
		}

		public override void EndInit(bool sync)
		{
			ChangeLogicNumber();

			_Speed.Name = _Name + ":Speed";
			_Position.Name = _Name + ":Position";
			_AccelTime.Name = _Name + ":AccelTime";
			_StopTime.Name = _Name + ":StopTime";
			_EmergencyStopTime.Name = _Name + ":E-StopTime";

			_Speed.EndInit();
			_Position.EndInit(true);
			_AccelTime.EndInit();
			_StopTime.EndInit();
			_EmergencyStopTime.EndInit();

			// 리미트 설정.
			MMCAPICollection.LimitsOfHardware.set_positive_limit(AxisLogicNumber, (short)MMCAPICollection.Enumurations.EventNumber.EStop);
			MMCAPICollection.LimitsOfHardware.set_negative_limit(AxisLogicNumber, (short)MMCAPICollection.Enumurations.EventNumber.EStop);

			_IsInited = true;

			this.Enable = false;

			
		}

		private void ChangeLogicNumber()
		{
			short axnumber = 0;
			short axSum=0;

			for (short i = 0; i < _BoardNumber; i++)
			{
				ErrorControl.Assert(MMCConfig.mmc_axes(i, ref axnumber));
				axSum += axnumber;
			}
			AxisLogicNumber = (short)(axSum + _AxisNumber);
		}
		#endregion

		#region Motion
		//public void MovePosition(long pos, bool sync)
		//{
		//    Enable = true;
		//    System.Threading.Thread.Sleep(100);

		//    ControlState.clear_status(AxisLogicNumber);
		//    AssertMotionable();

		//    pos = (int)( pos * Position.Precision );
		//    //System.Diagnostics.Debug.WriteLine(( (double)_Speed.Value * _Speed.Precision ).ToString() + "-Speed");
		//    if (sync)
		//    {
		//        ErrorControl.Assert(OneAxisMoveScurve.s_move(AxisLogicNumber, (double)pos , (double)_Speed.Value * _Speed.Precision, (short)_AccelTime.Value));
		//    }
		//    else
		//    {
		//        ErrorControl.Assert(OneAxisMoveScurve.start_s_move(AxisLogicNumber, (double)pos , (double)_Speed.Value * _Speed.Precision, (short)_AccelTime.Value));
		//    }
		//}

		//public void MoveOffset(long pos, bool sync)
		//{
		//    Enable = true;
		//    System.Threading.Thread.Sleep(100);

		//    ErrorControl.Assert(ControlState.clear_status(AxisLogicNumber));
		//    AssertMotionable();

		//    if (sync)
		//    {
		//        ErrorControl.Assert(OneAxisMoveScurve.rs_move(AxisLogicNumber, (double)pos, (double)_Speed.Value, (short)_AccelTime.Value));
		//    }
		//    else
		//    {
		//        ErrorControl.Assert(OneAxisMoveScurve.start_rs_move(AxisLogicNumber, (double)pos, (double)_Speed.Value, (short)_AccelTime.Value));
		//    }
		//}

		public void MoveVelocity(bool direction)
		{
			Enable = true;
			//System.Threading.Thread.Sleep(100);

			//ErrorControl.Assert(ControlState.clear_status(AxisLogicNumber));
			ControlState.clear_status(AxisLogicNumber);
			AssertMotionable();

			//if (IsMotion.Value) {
			//    throw new InvalidOperationException("Axis is in motion state.");
			//}

			ErrorControl.Assert(VelocityMove.v_move(AxisLogicNumber, direction ? (double)_Speed.Value / _Speed.Precision : -1d * _Speed.Value / _Speed.Precision, (short)AccelTime.Value));
		}

		public void Stop(bool emergency)
		{
			//if (!(_IsInited && Enable)) { return; }

			if (emergency)
			{
				ErrorControl.Assert(StopEmergenceAction.set_e_stop(AxisLogicNumber));
			}
			else
			{
				ErrorControl.Assert(StopAction.set_stop(AxisLogicNumber));
			}

			while (_IsMotion) { System.Threading.Thread.Sleep(10); }

			//ErrorControl.Assert(ControlState.clear_status(AxisLogicNumber));
			ControlState.clear_status(AxisLogicNumber);

			if (AutoOff) { disableTimer.Change(3000, System.Threading.Timeout.Infinite); }
		}
		#endregion

		#region ETC
		private void AssertMotionable()
		{
			if (_IsInited && _Enable) { return; }

			string msg = "";
			if (!_IsInited)
			{
				msg += "This axis isn't Inited. ";
			}
			if (!_Enable)
			{
				msg += "This axis isn't enabled.";
			}
			//throw new InvalidOperationException(msg);
		}

		private void AmpDisable(object state)
		{
			this.Enable = false;
		}
		#endregion
	}
}
