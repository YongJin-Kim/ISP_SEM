using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace SEC.Nanoeye.NanoStage
{
	class HomeSearch : IDisposable
	{
		#region Property & Variables
		LinkedList<EventHandler> searchSequence;
		LinkedListNode<EventHandler> ssNode;

		public enum HomeSearchType 
		{
			NegSensor,
			PosSensor,
			HomeSensor,
			NoSensor
		}

		private bool _IsSearched = false;
		public bool IsSearched
		{
			get { return _IsSearched; }
		}

		private bool _IsHomeSearching = false;
		public bool IsHomeSearching
		{
			get { return _IsHomeSearching; } 
		}

		private MMCValues.MMCAxis _Axis = null;
		public MMCValues.MMCAxis Axis
		{
			get { return _Axis; }
		}

		protected int _MinimumSpeedModifier = 1;
		public virtual int MinimumSpeedModifier
		{
			get { return _MinimumSpeedModifier; }
			set
			{
				if(value < 1) { throw new ArgumentException("value must be greater then 1."); }

				_MinimumSpeedModifier = value;
			}
		}
		#endregion

		#region Event
		public event EventHandler HomeSearchDone;
		protected virtual void OnHomeSearchDone()
		{
			if (HomeSearchDone != null)
			{
				HomeSearchDone(this, EventArgs.Empty);
			}
		}
		#endregion

		#region 생성자 & 소멸자
		public HomeSearch(MMCValues.MMCAxis ax)
		{
			if (ax == null) { throw new ArgumentException(); }

			_Axis = ax;
		}

		~HomeSearch()
		{
			Dispose();
		}

		public void Dispose()
		{
			Stop();
		}
		#endregion

		#region Start & Stop
		public void Stop() 
		{
			if (_IsHomeSearching)
			{
				lock(ssNode) { _Axis.MotionStateChanged -= ssNode.Value; }

				MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (int)MMCValues.MMCAPICollection.Enumurations.EventNumber.No);

				_Axis.Stop(false);

				_IsHomeSearching = false;

				OnHomeSearchDone();
			}
		}

		public void Start(HomeSearchType type) 
		{
			// Offset 이동시 필요한 변수.
			_IsHomeSearching = true;
			Debug.WriteLine("Home Search Started.", _Axis.Name);

			_Axis.Stop(false);

			while (_Axis.IsMotion) { System.Threading.Thread.Sleep(10); }

			// Sw Limit Off

			// MMC의 경우 축 상태 초기화가 필요. (Error State, Event 등)
			_Axis.Enable = false;
			_Axis.Position.Maximum = _Axis.Position.DefaultMax + 1;
			_Axis.Position.Minimum = _Axis.Position.DefaultMin - 1;
			_Axis.BeginInit();
			_Axis.Position.Value = 0;
			_Axis.EndInit();

			System.Threading.Thread.Sleep(10);
			MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (short)MMCValues.MMCAPICollection.Enumurations.EventNumber.No);
			_Axis.Enable = true;

			searchSequence = new LinkedList<EventHandler>();

			switch (type)
			{
			case HomeSearchType.NoSensor:
				SearchNo(searchSequence);
				Debug.WriteLine("Home Search - NoSensor", _Axis.Name);
				break;
			case HomeSearchType.HomeSensor:
				SearchHome(searchSequence);
				Debug.WriteLine("Home Search - HomeSensor", _Axis.Name);
				break;
			case HomeSearchType.NegSensor:
				SearchNeg(searchSequence);
				Debug.WriteLine("Home Search - NegSensor", _Axis.Name);
				break;
			case HomeSearchType.PosSensor:
				SearchPos(searchSequence);
				Debug.WriteLine("Home Search - PosSensor", _Axis.Name);
				break;
			default:
				throw new ArgumentException();
			}



			ssNode = searchSequence.First;

			_Axis.MotionStateChanged += ssNode.Value;

			ssNode.Value(_Axis, EventArgs.Empty);
		}

		private void SearchPos(LinkedList<EventHandler> ssList)
		{
			if (_Axis.LimitState == LimitSensorEnum.HW_Positive)
			{
			}
			else
			{
				ssList.AddLast(GoPosFast);
			}

			ssList.AddLast(FadeoutNeg);
			ssList.AddLast(GoPosSlow);
			ssList.AddLast(GoOffset);
			ssList.AddLast(SearchEnd);
		}

		private void SearchNeg(LinkedList<EventHandler> ssList)
		{
			if (_Axis.LimitState == LimitSensorEnum.HW_Negative)
			{
			}
			else
			{
				ssList.AddLast(GoNegFast);
			}

			ssList.AddLast(FadeoutPos);
			ssList.AddLast(GoNegSlow);
			ssList.AddLast(GoOffset);
			ssList.AddLast(SearchEnd);
		}

		private void SearchHome(LinkedList<EventHandler> ssList)
		{
			if (_Axis.LimitState == LimitSensorEnum.HW_Home)
			{
			}
			else if (_Axis.LimitState == LimitSensorEnum.HW_Negative)
			{
				ssList.AddLast(GoHomeFast);
			}
			else
			{
				ssList.AddLast(GoPosFast);
				ssList.AddLast(GoHomeFast);
			}

			ssList.AddLast(FadeoutPos);
			ssList.AddLast(GoHomeSlow);
			ssList.AddLast(GoOffset);
			ssList.AddLast(SearchEnd);
		}

		private void SearchNo(LinkedList<EventHandler> ssList)
		{
			_Axis.Position.BeginInit();
			_Axis.Position.Value = 0;
			_Axis.Position.EndInit();

			ssList.AddLast(SearchEnd);
		}
		#endregion

		#region Search Method
		private void DoNextSequence()
		{
			Debug.WriteLine("Home Search - DoNextSequence", _Axis.Name);

			MMCValues.MMCAPICollection.ControlState.clear_status(_Axis.AxisLogicNumber);

            lock(ssNode)
            {
                _Axis.MotionStateChanged -= ssNode.Value;
                ssNode = ssNode.Next;
                _Axis.MotionStateChanged += ssNode.Value;
                ssNode.Value(_Axis, EventArgs.Empty);
            }

		}

		private enum SensorType
		{
			HW,
			SW,
			ALL
		}

		bool CheckSensor(LimitSensorEnum value, LimitSensorEnum comp, SensorType st)
		{
			LimitSensorEnum remainer;
			switch (st)
			{
			case SensorType.ALL:
				remainer = LimitSensorEnum.HW_Home | LimitSensorEnum.HW_Negative | LimitSensorEnum.HW_Positive | LimitSensorEnum.SW_Home | LimitSensorEnum.SW_Negative | LimitSensorEnum.SW_Positive;
				break;
			case SensorType.HW:
				remainer = LimitSensorEnum.HW_Home | LimitSensorEnum.HW_Negative | LimitSensorEnum.HW_Positive;
				break;
			case SensorType.SW:
				remainer = LimitSensorEnum.SW_Home | LimitSensorEnum.SW_Negative | LimitSensorEnum.SW_Positive;
				break;
			default:
				throw new ArgumentException();
			}

			value = (value & remainer);

			return (value == comp);

		}

        void GoNegFast(object sender, EventArgs e)
        {
            Debug.WriteLine("Home Search - GoNegFast " + _Axis.LimitState.ToString(), _Axis.Name);

            if(_Axis.IsMotion) { return; }


            if(CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Negative, SensorType.HW))
            {
                DoNextSequence();
            }
            else
            {
                _Axis.Speed.Value = _Axis.Speed.Maximum;
                _Axis.MoveVelocity(false);
            }
        }

		void GoNegSlow(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoNegSlow " + _Axis.LimitState.ToString(), _Axis.Name);
			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Negative, SensorType.HW))
			{
				DoNextSequence();
			}
			else
			{
				_Axis.Speed.Value = Math.Min(_Axis.Speed.Minimum * _MinimumSpeedModifier, _Axis.Speed.Maximum);
				_Axis.MoveVelocity(false);
			}
		}

		void GoPosFast(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoPosFast " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Positive, SensorType.HW))
			{
				DoNextSequence();
			}
			else
			{
				_Axis.Speed.Value = _Axis.Speed.Maximum;
				_Axis.MoveVelocity(true);
			}
		}

		void GoPosSlow(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoPosSlow " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Positive, SensorType.HW))
			{
				DoNextSequence();
			}
			else
			{
				_Axis.Speed.Value = Math.Min(_Axis.Speed.Minimum * _MinimumSpeedModifier, _Axis.Speed.Maximum);
				_Axis.MoveVelocity(true);
			}
		}

		void GoHomeFast(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoHomeFast " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Home, SensorType.HW))
			{
				MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (int)MMCValues.MMCAPICollection.Enumurations.EventNumber.No);
				DoNextSequence();
			}
			else
			{
				MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (int)MMCValues.MMCAPICollection.Enumurations.EventNumber.Stop);
				_Axis.Speed.Value = _Axis.Speed.Maximum;
				_Axis.MoveVelocity(false);
			}
		}

		void GoHomeSlow(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoHomeSlow " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.HW_Home, SensorType.HW))
			{
				MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (int)MMCValues.MMCAPICollection.Enumurations.EventNumber.No);
				DoNextSequence();
			}
			else
			{
				MMCValues.MMCAPICollection.LimitsOfHardware.set_home(_Axis.AxisLogicNumber, (int)MMCValues.MMCAPICollection.Enumurations.EventNumber.Stop);
				_Axis.Speed.Value = Math.Min(_Axis.Speed.Minimum * _MinimumSpeedModifier, _Axis.Speed.Maximum);
				_Axis.MoveVelocity(false);
			}
		}

		void FadeoutPos(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - FadeoutPos " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.Non, SensorType.HW))
			{
				DoNextSequence();
			}
			else
			{
				_Axis.Speed.Value = _Axis.Speed.Maximum;
				
				// 3초간 뒤로 뺌.
				_Axis.Position.Value += _Axis.Speed.Minimum * 3;
			}
		}

		void FadeoutNeg(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - FadeoutNeg " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (CheckSensor(_Axis.LimitState, LimitSensorEnum.Non, SensorType.HW))
			{
				DoNextSequence();
			}
			else
			{
				_Axis.Speed.Value = _Axis.Speed.Maximum;

				// 3초간 뒤로 뺌.
				_Axis.Position.Value -= _Axis.Speed.Minimum * 3;
			}
		}

		void GoOffset(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoOffset " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

			if (_Axis.HomePosition == 0)
			{
				DoNextSequence();
			}
			else
			{
				_Axis.MotionStateChanged -= ssNode.Value;
				_Axis.MotionStateChanged += GoOffsetInner;

				_Axis.Speed.Value = _Axis.Speed.Maximum;
				_Axis.Position.Value += _Axis.HomePosition;
			}
		}

		void GoOffsetInner(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - GoOffsetInner " + _Axis.LimitState.ToString(), _Axis.Name);

			if (_Axis.IsMotion) { return; }

            lock(ssNode)
            {
                _Axis.MotionStateChanged -= GoOffsetInner;
                ssNode = ssNode.Next;
                _Axis.MotionStateChanged += ssNode.Value;
                ssNode.Value(_Axis, EventArgs.Empty);
            }
		}

		void SearchEnd(object sender, EventArgs e)
		{
			Debug.WriteLine("Home Search - SearchEnd " + _Axis.LimitState.ToString(), _Axis.Name);

			_Axis.MotionStateChanged -= SearchEnd;

			_IsHomeSearching = false;
			_Axis.Stop(false);

			_IsSearched = true;

			_Axis.BeginInit();
			_Axis.Position.Value = _Axis.Position.Offset * -1;
			_Axis.EndInit();

			OnHomeSearchDone();
		}
		#endregion
	}
}
