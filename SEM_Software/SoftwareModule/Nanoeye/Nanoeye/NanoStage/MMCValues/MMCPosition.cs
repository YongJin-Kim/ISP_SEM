using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.Nanoeye.NanoStage.MMCValues
{
	internal class MMCPosition : IAxPosition
	{
		#region Property & Variables
		private bool _ThrowException = false;
		public bool ThrowException
		{
			get { return _ThrowException; }
			set { _ThrowException = value; }
		}

		protected bool _ErrorState = true;
		public bool ErrorState
		{
			get { return _ErrorState; }
		}

		protected double _Precision = -1d;
		public double Precision
		{
			get { return _Precision; }
			set { _Precision = value; }
		}

		protected long _DefaultMax = 1;
		public long DefaultMax
		{
			get { return _DefaultMax - _Offset; }
			set { _DefaultMax = value + _Offset; }
		}

		protected long _DefaultMin = -1;
		public long DefaultMin
		{
			get { return _DefaultMin - _Offset; }
			set { _DefaultMin = value + _Offset; }
		}

		private bool _EndlessMove = false;
		public bool EndlessMove
		{
			get { return _EndlessMove; }
			set
			{
				//Debug.Assert(!_Enable);

				_EndlessMove = value;

				// 사용 금지.
				//MMCAPICollection.EndlessMove.set_endless_rotationax(_Owner.AxisLogicNumber, (short)(value ? 1 : 0), (short)_Owner.Resolution);
				//MMCAPICollection.EndlessMove.set_endless_linearax(_Owner.AxisLogicNumber, (short)(value ? 1 : 0), (short)_Owner.Resolution);
				//MMCAPICollection.EndlessMove.set_endless_range(_Owner.AxisLogicNumber, ((Maximum - Minimum) / Precision));
			}
		}

		private double _Maximum = double.NaN;
		private short _swPosLimit = -1;

		/// <summary>
		/// SW Limit - Positive
		/// </summary>
		public long Maximum
		{
			get
			{
				double pos = 0;
				short act = 0;
				MMCAPICollection.LimitsOfSotware.get_positive_sw_limit(_Owner.AxisLogicNumber, ref pos, ref act);

				pos = pos * _Precision - _Offset;
				if (pos > DefaultMax) { pos = DefaultMax; }
				return (long)pos;
			}
			set
			{
				MMCValues.MMCAPICollection.Enumurations.EventNumber eve;

				double pos = ((double)value + _Offset) / _Precision;

				if (((value) > DefaultMax) || _EndlessMove)
				{
					eve = SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection.Enumurations.EventNumber.No;
					//System.Diagnostics.Debug.WriteLine("Positive SW Limit Release.", _Owner.Name);
				}
				else
				{
					eve = SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection.Enumurations.EventNumber.Stop;
					//System.Diagnostics.Debug.WriteLine("Positive SW Limit Set.", _Owner.Name);
				}

				// 같은경우에는 제 업데이트 하지 않게 함.
				if ((pos == _Maximum) && (_swPosLimit == (short)eve)) { return; }
				_Maximum = pos;
				_swPosLimit = (short)eve;

				Debug.WriteLine(_Owner.AxisLogicNumber.ToString() + " - ax, " + pos.ToString() + "- pos, " + ((short)eve).ToString() + "- eve", _Owner.Name);

				MMCAPICollection.ErrorControl.Assert(MMCAPICollection.LimitsOfSotware.set_positive_sw_limit(_Owner.AxisLogicNumber, pos, (short)eve));
			}
		}

		private double _Minimum = double.NaN;
		private short _swNegLimit = -1;
		/// <summary>
		/// SW Limit - Negative
		/// </summary>
		public long Minimum
		{
			get
			{
				double pos = 0;
				short act = 0;
				MMCAPICollection.LimitsOfSotware.get_negative_sw_limit(_Owner.AxisLogicNumber, ref pos, ref act);

				pos = pos * _Precision - _Offset;
				if (pos < DefaultMin) { pos = DefaultMin; }

				return (long)pos;
			}
			set
			{
				MMCValues.MMCAPICollection.Enumurations.EventNumber eve;

				//if(_Owner.Name == "AxZ")
				//{
				//    System.Diagnostics.Debug.WriteLine("Change Min Pos - "+value.ToString(), "AxZ");
				//}

				double pos = ((double)value + _Offset) / _Precision;


				if (((value) < DefaultMin) || _EndlessMove)
				{
					eve = SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection.Enumurations.EventNumber.No;
					//System.Diagnostics.Debug.WriteLine("Negative SW Limit Release.", _Owner.Name);
				}
				else
				{
					eve = SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection.Enumurations.EventNumber.Stop;
					//System.Diagnostics.Debug.WriteLine("Negative SW Limit Set.", _Owner.Name);
				}

				// 같은경우에는 제 업데이트 하지 않게 함.
				if ((pos == _Minimum) && (_swNegLimit == (short)eve)) { return; }

				_Minimum = pos;
				_swNegLimit = (short)eve;

				Debug.WriteLine("Set SW Limit " + _Owner.AxisLogicNumber.ToString() + " - ax, " + pos.ToString() + "- pos, " + ((short)eve).ToString() + "- eve", _Owner.Name);
				try
				{
					MMCAPICollection.ErrorControl.Assert(MMCAPICollection.LimitsOfSotware.set_negative_sw_limit(_Owner.AxisLogicNumber, pos, (short)eve));
				}
				catch (Exception ex)
				{
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				}
			}
		}

		protected long _Offset = 0;
		public long Offset
		{
			get { return _Offset; }
			set
			{
				_Offset = value;
				OnValueChanged();
			}
		}

		/// <summary>
		/// 초기화된 상태라면 절대 좌표 이동.
		/// 초기화 중이라면 그 값으로 설정 됨.
		/// </summary>
		private long prePos = long.MinValue;
		public long Value
		{
			get
			{
				double pos = 0;
				MMCAPICollection.PositionValue.get_command(_Owner.AxisLogicNumber, ref pos);

				pos = pos * _Precision - _Offset;



				//pos -= Minimum;

				//pos = pos % (Maximum - Minimum);

				//pos += Minimum;

				if (_EndlessMove)
				{
					if (pos > 180000)
					{
						pos -= 360000;
					}
					else if (pos < -180000)
					{
						pos += 360000;
					}
				}

				if (_IsInited)
				{
					if ((long)pos != prePos)
					{
						prePos = (long)pos;
						OnValueChanged();
					}
				}

				return prePos;
			}
			set
			{
				double pos = (value + _Offset) / _Precision;

				if (_IsInited)
				{
					double preCmd = 0;
					MMCAPICollection.PositionValue.get_command(_Owner.AxisLogicNumber, ref preCmd);
					//System.Diagnostics.Debug.WriteLine("CMD - " + preCmd.ToString() + ", Tar - " + pos.ToString() + ", Gap - " + (preCmd - pos).ToString(), _Owner.Name);
					if (Math.Round(preCmd) == Math.Round(pos))
					{
						System.Diagnostics.Debug.WriteLine("Move Fail. You want to move same position.", _Owner.Name);
						//this.Enable = false;
						//_Owner.Enable = false;
					}
					else
					{
						//System.Diagnostics.Debug.WriteLine("Move Pos - " + pos.ToString(), _Owner.Name);
						MMCAPICollection.ErrorControl.Assert(MMCAPICollection.OneAxisMoveScurve.start_ts_move(_Owner.AxisLogicNumber, pos, (long)(_Owner.Speed.Value / _Owner.Speed.Precision), (short)_Owner.AccelTime.Value, (short)_Owner.StopTime.Value));

						//short state = MMCAPICollection.ActionState.in_motion(_Owner.AxisLogicNumber);

						bool motion = _Owner.IsMotion;

						//if(_Owner.IsMotion) { System.Diagnostics.Debug.WriteLine("In Motion.", _Owner.Name); }
						//else { System.Diagnostics.Debug.WriteLine("Not Motion.", _Owner.Name); }
					}
				}
				else
				{
					//System.Diagnostics.Debug.WriteLine("Set Pos - " + pos.ToString(), _Owner.Name);
					MMCAPICollection.ErrorControl.Assert(MMCAPICollection.PositionValue.set_command(_Owner.AxisLogicNumber, pos));
				}
			}
		}

		public object[] Read
		{
			get { throw new NotSupportedException(); }
		}

		protected bool _IsInited = false;
		public bool IsInitied
		{
			get { return _IsInited; }
		}

		protected string _Name = null;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		protected MMCAxis _Owner = null;
		public object Owner
		{
			get { return _Owner; }
			set
			{
				if (_Owner != null) { _Owner.MotionStateChanged -= new EventHandler(_Owner_MotionStateChanged); }
				_Owner = value as MMCAxis;
				if (_Owner != null) { _Owner.MotionStateChanged += new EventHandler(_Owner_MotionStateChanged); }
			}
		}

		protected bool _Enable = true;
		public bool Enable
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}
		#endregion

		#region Event
		public event EventHandler  ValueErrorOccured;
		protected virtual void ONValueErrorOccured()
		{
			if (ValueErrorOccured != null)
			{
				ValueErrorOccured(this, EventArgs.Empty);
			}
		}

		public event EventHandler  ValueChanged;
		protected virtual void OnValueChanged()
		{
			if (_IsInited)
			{
				if (ValueChanged != null)
				{
					ValueChanged(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler  EnableChanged;
		protected virtual void OnEnableChanged()
		{
			if (EnableChanged != null)
			{
				EnableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  NoResponse;
		protected virtual void OnNoResponse()
		{
			if (NoResponse != null)
			{
				NoResponse(this, EventArgs.Empty);
			}
		}

		public event EventHandler  CommunicationError;
		protected virtual void OnCommunicationError()
		{
			if (CommunicationError != null)
			{
				CommunicationError(this, EventArgs.Empty);
			}
		}
		#endregion

		void _Owner_MotionStateChanged(object sender, EventArgs e)
		{
			// 무한 회전 시 위치 값을 초기화 시킨다.
			if (_EndlessMove)
			{
				if (!_Owner.IsMotion)
				{
					if (Value > 180000)
					{
						this.BeginInit();
						this.Value -= 360000;
						this.EndInit();
					}
					else if (Value < -180000)
					{
						this.BeginInit();
						this.Value += 360000;
						this.EndInit();
					}
				}
			}
		}

		public void Dispose()
		{

		}

		private void ChangeSWLimit()
		{
			throw new NotImplementedException();
		}

		#region 동기화
		public void Sync()
		{
			double pos = Value;
		}

		public bool Validate()
		{
			double pos = 0;
			MMCAPICollection.PositionValue.get_command(_Owner.AxisLogicNumber, ref pos);

			pos *= _Precision;
			pos -= _Offset;

			return ((long)pos == prePos);
		}
		#endregion

		#region 초기화
		public void EndInit(bool sync)
		{
			System.Diagnostics.Debug.WriteLine("End Init", _Name);

			double pos =0;

			MMCAPICollection.PositionValue.get_command(_Owner.AxisLogicNumber, ref pos);

			pos = pos * _Precision - _Offset;

			if (pos < Minimum) { pos = Minimum; }
			else if (pos > Maximum) { pos = Maximum; }
			else
			{
				_IsInited = true;
				OnValueChanged();
				return;
			}	// Rising Event

			// Set SW Limit
			Minimum = Minimum;
			Maximum = Maximum;

			this.Value = (long)pos;	// Out of range. Reset value.

			_IsInited = true;
		}

		public void BeginInit()
		{
			_IsInited = false;

			System.Diagnostics.Debug.WriteLine("Begin Init", _Name);
		}

		public void EndInit()
		{
			EndInit(true);
		}
		#endregion

		public void MoveOffset(long offset)
		{
			double pos = offset / _Precision;

			//MMCAPICollection.ErrorControl.Assert(MMCAPICollection.OneAxisMoveScurve.start_ts_move(_Owner.AxisLogicNumber, pos, (long)(_Owner.Speed.Value / _Owner.Speed.Precision), (short)_Owner.AccelTime.Value, (short)_Owner.StopTime.Value));
			MMCAPICollection.ErrorControl.Assert(MMCAPICollection.OneAxisMoveScurve.start_trs_move(_Owner.AxisLogicNumber, pos, (long)(_Owner.Speed.Value / _Owner.Speed.Precision), (short)_Owner.AccelTime.Value, (short)_Owner.StopTime.Value));

			bool motion = _Owner.IsMotion;
		}

		#region IValue 멤버



		#endregion
	}
}
