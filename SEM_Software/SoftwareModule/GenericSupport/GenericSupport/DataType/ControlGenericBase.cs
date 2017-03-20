using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.GenericSupport.DataType
{
	public class ControlGenericBase<T> : ControlValueBase
		where T : struct, IComparable, IComparable<T>, IConvertible
	{
		#region Property & Vairables
		protected T _DefaultMax = default(T);
		/// <summary>
		/// 적용 가능한 최대 값.
		/// </summary>
		public T DefaultMax
		{
			get { return _DefaultMax; }
			set { _DefaultMax = value; }
		}

		protected T _DefaultMin = default(T);
		/// <summary>
		/// 적용 가능한 최소 값.
		/// </summary>
		public T DefaultMin
		{
			get { return _DefaultMin; }
			set { _DefaultMin = value; }
		}

		protected double _Precision = 1.0d;
		/// <summary>
		/// Value의 정밀도.
		/// </summary>
		public double Precision
		{
			get { return _Precision; }
			set { _Precision = value; }
		}

		protected T _Maximum;
		/// <summary>
		/// 실제 사용되는 영역의 최대 값.
		/// </summary>
		public virtual T Maximum
		{
			get { return _Maximum; }
			set
			{
				if(_Enable)
				{
					if(!_IsInited)
					{
						_Maximum = value;
					}
					else
					{
						if (value.CompareTo(_DefaultMax) > 0)
						{
							string msg ="Larger then DefaultMaximum.";

							if (_ThrowException)
							{
								throw new ArgumentException(msg + " by " + this._Name);
							}
							else
							{
								Trace.WriteLine(msg, this._Name);
								Debug.WriteLine(Environment.StackTrace);
								_Maximum = _DefaultMax;
							}
						}
						else
						{
							_Maximum = value;
						}
						if(value.CompareTo(_Minimum) < 0) { _Minimum = value; }
						if (value.CompareTo(_Value) < 0) { Value = value; }
						else { OnValueChanged(); }
					}
				}
			}
		}

		protected T _Minimum;
		/// <summary>
		/// 실제 사용되는 영역의 최소 값.
		/// </summary>
		public virtual T Minimum
		{
			get { return _Minimum; }
			set
			{
				if(_Enable)
				{
					if(!_IsInited)
					{
						_Minimum = value;
					}
					else
					{

						if (value.CompareTo(_DefaultMin) < 0)
						{
							string msg = "Smaller then DefaultMinimum.";
							if (_ThrowException)
							{
								throw new ArgumentException(msg + " by " + this._Name);
							}
							else
							{
								Trace.WriteLine(msg, this._Name);
								Debug.WriteLine(Environment.StackTrace);
								_Minimum = _DefaultMin;
							}
						}
						else
						{
							_Minimum = value;
						}
						if (value.CompareTo(_Maximum) > 0) { _Maximum = value; }
						if (value.CompareTo(_Value) > 0) { Value = value; }
						else { OnValueChanged(); }
					}
				}
			}
		}

		protected T _Offset = default(T);
		public virtual T Offset
		{
			get { return _Offset; }
			set
			{
				if(_Enable)
				{
					if(!_IsInited)
					{
						_Offset = value;
					}
					else
					{
						_Offset = value;
						Value = _Value;
					}
				}
			}
		}

		protected T _Value;
		/// <summary>
		/// 현재 설정된 값.
		/// </summary>
		public virtual T Value
		{
			get { return _Value; }
			set
			{
				if(_Enable)
				{
					if(!_IsInited)
					{
						_Value = value;
					}
					else
					{
						if (value.CompareTo(_Maximum) > 0)
						{
							string msg = "Value Larger then Maximum.";
							if (_ThrowException)
							{
								throw new ArgumentException(msg + " by " + this._Name);
							}
							else
							{
								Trace.WriteLine(msg, _Name);
								Debug.WriteLine(Environment.StackTrace);
								_Value = _Maximum;
							}
						}
						else if (value.CompareTo(_Minimum) < 0)
						{
							string msg = "Value Smaller then Mimimum.";
							if (_ThrowException)
							{
								throw new ArgumentException(msg + " by " + this._Name);
							}
							else
							{
								Trace.WriteLine(msg, _Name);
								Debug.WriteLine(Environment.StackTrace);
								_Value = _Minimum;
							}
						}
						else
						{
							_Value = value;
						}
						OnValueChanged();
					}
				}
			}
		}

		/// <summary>
		/// 읽기
		/// </summary>
		public override object[] Read { get { throw new NotSupportedException(); } }
		#endregion

		#region Event
		/// <summary>
		/// 값이 바뀌었음을 알림
		/// </summary>
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged()
		{
			System.Diagnostics.Debug.Assert(_Enable);

			if(ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}

		#endregion

		public override void BeginInit()
		{
			if(_Enable)
			{
				_IsInited = false;
			}
		}

		public override void EndInit(bool sync)
		{
			if(_Enable)
			{
				string msg;
				if (_DefaultMax.CompareTo(_DefaultMin) < 0)
				{
					// 이것은 복구 하면 안된다.
					msg = "DefaultMax is smaller the DefaultMin.";
					throw new ArgumentException(msg + " by " + this._Name);
				}

				if(_Maximum.CompareTo(_DefaultMax) > 0)
				{
					System.Diagnostics.Debug.WriteLine("Maximum is larger then DefaultMax. Will be DefMax.", this._Name);
					_Maximum = _DefaultMax;
				}
				if(_Minimum.CompareTo(_DefaultMin) < 0)
				{
					System.Diagnostics.Debug.WriteLine("Minimum is smaller then DefaultMin.", this._Name);
					_Minimum = _DefaultMin;
				}

				if (_Maximum.CompareTo(_Minimum) < 0)
				{
					msg = "Maximum is smaller then Mimimum.";
					if (_ThrowException)
					{
						throw new ArgumentException(msg + " by " + this._Name);
					}
					else
					{
						Trace.WriteLine(msg, _Name);
						Debug.WriteLine(Environment.StackTrace);
						_Maximum = _DefaultMax;
						_Minimum = _DefaultMin;
					}
				}

				if(_Value.CompareTo(_Maximum) > 0)
				{
					System.Diagnostics.Debug.WriteLine("Value is larger then Maximum. ", this._Name);
					_Value = _Maximum;
				}
				if(_Value.CompareTo(_Minimum) < 0)
				{
					System.Diagnostics.Debug.WriteLine("Value is smaller then Minimum. ", this._Name);
					_Value = _Minimum;
				}

				if (_Precision == 0)
				{
					msg = "Precision must be grate then 0.";
					if (_ThrowException)
					{
						throw new ArgumentException(msg + " by " + this._Name);
					}
					else
					{
						Trace.WriteLine(msg, _Name);
						Debug.WriteLine(Environment.StackTrace);
						_Precision = 1;
					}
				}

				_IsInited = true;

				// 동기화 모드라면,		동기화 작업을 하고
				// 동기화 모드가 아니라면,	Value를 업데이트 함으로써 상속시 각종 동작을 취하도록 한다.
				if(sync) { Sync(); }
				else { Value = _Value; }
			}
		}

		public override void EndInit()
		{
			EndInit(false);
		}

		public override void Sync()
		{
			throw new NotSupportedException();
		}

		public override bool Validate()
		{
			throw new NotSupportedException();
		}
	}
}
