using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class LongScrollWithTable : SEC.GUIelement.Longscroll, INanoeyeValueControl
	{
		#region Property & Variables

		protected SECtype.ITable _ControlValue;

		[DefaultValue(null),
		Browsable(false)]
		public SEC.GenericSupport.DataType.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if(_ControlValue != null)
				{
					_ControlValue.SelectedIndexChanged -= new EventHandler(_ControlValue_SelectedIndexChanged);
					_ControlValue.TableChanged -= new EventHandler(_ControlValue_TableChanged);
				}

				_ControlValue = value as SECtype.ITable;

				if(_ControlValue != null)
				{
					_ControlValue.SelectedIndexChanged += new EventHandler(_ControlValue_SelectedIndexChanged);
					_ControlValue.TableChanged += new EventHandler(_ControlValue_TableChanged);
					this.Enabled = _ControlValue.Enable;

					ResetControl();
				}
				else
				{
					this.Enabled = false;
				}

				this.Invalidate();
			}
		}

		[DefaultValue(true), Browsable(false)]
		public bool IsLimitedMode
		{
			get { return true; }
			set { throw new NotSupportedException(); }
		}

		[DefaultValue(true), Browsable(false)]
		public bool IsValueOperation
		{
			get { return true; }
			set { throw new NotSupportedException(); }
		}

		[DefaultValue(100), Browsable(false)]
		public override int Maximum
		{
			get { return base.Maximum; }
			set { throw new NotSupportedException(); }
		}

		[DefaultValue(0), Browsable(false)]
		public override int Minimum
		{
			get { return base.Minimum; }
			set { throw new NotSupportedException(); }
		}

		private bool _UseAllowedNumber = false;
		/// <summary>
		/// Vlaue 의 상위 자릿수를 AllowedNumber에 정의된 숫자로 제한 한다.
		/// 이 값이 true로 설정 되면 "UseCustomStep"은 무시 된다.
		/// </summary>
		[DefaultValue(false)]
		public bool UseAllowedNumber
		{
			get { return _UseAllowedNumber; }
			set
			{
				_UseAllowedNumber = value;
				if(value) { ChagneAllowedMaxMin(); }
			}
		}

		private List<int> _AllowedNumber = new List<int>();
		/// <summary>
		/// Value을 이 목록에 지정된 숫자로만 선택 한다.
		/// 이때, 이 목록에 들어있는 숫자의 자릿수는 같아야 하며,
		/// Value의 앞자리 숫자는 이 목록에 들어 있는 숫자로만 결정되며, 나머지는 0으로 채워진다.
		/// </summary>
		public List<int> AllowedNumber
		{
			get { return _AllowedNumber; }
		}

		#endregion

		public LongScrollWithTable()
		{
			InitializeComponent();
		}

		protected override void updateTimer_Tick(object sender, EventArgs e)
		{
			if(targetValue != _Value)
			{
				if(_UseAllowedNumber)
				{
					targetValue = ChangeAllowedNumber(targetValue, _Value);
				}
				else if(_UseCustomStep)
				{
					targetValue = CustomStep(targetValue, _Value, _Maximum, _Minimum);
				}
				Value = targetValue;
			}
		}

		private int ChangeAllowedNumber(int target, int value)
		{
			int gap = target - value;
			int result;
			int jarisu = (int)Math.Log10(_AllowedNumber[0]) + 1;

			if(target > Maximum) { target = Maximum; }
			if(target < Minimum) { target = Minimum; }

			do
			{
				result = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower(target, jarisu, _AllowedNumber.ToArray());

				target += gap;

			} while(result == value);

			return result;
		}

		protected override void OnValueChanged()
		{
			_ControlValue.SelectedIndex = base._Value;
			base.OnValueChanged();
		}

		void _ControlValue_TableChanged(object sender, EventArgs e)
		{
			ResetControl();
		}

		void _ControlValue_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(Value != _ControlValue.SelectedIndex)
			{
				Value = _ControlValue.SelectedIndex;
			}
		}

		private void ResetControl()
		{
			if(_UseAllowedNumber)
			{
				ChagneAllowedMaxMin();
			}
			else
			{
				_Maximum = _ControlValue.IndexMaximum;
				_Minimum = _ControlValue.IndexMinimum;
			}

			if (_ControlValue.Length > 0)
			{
				Value = _Value;
			}
		}

		private void ChagneAllowedMaxMin()
		{
			if(_ControlValue == null) { return; }
			if(_AllowedNumber.Count < 1) { return; }
			if(_ControlValue.Length < 1) { return; }

			int controlMax = _ControlValue.IndexMaximum;
			int controlMin = _ControlValue.IndexMinimum;

			List<int> numList = _AllowedNumber;

			double jarisu;

			#region AllowedNumber 검증
			jarisu = Math.Log10(numList[0]);	
			//if(Math.Truncate(jarisu) == jarisu) { jarisu++; }	// 10,100 처럼 10의 지수 값은 1을 더해 줌으로써 숫자의 갯수를 맞추어 준다.
			jarisu = Math.Floor(jarisu);
			foreach(int num in numList)
			{
				double numJari= Math.Log10(num);
				//if(Math.Truncate(numJari) == numJari) { numJari++; }	// 10,100 처럼 10의 지수 값은 1을 더해 줌으로써 숫자의 갯수를 맞추어 준다.
				numJari = Math.Floor(numJari);
				if(jarisu != numJari) { throw new InvalidOperationException("AllowedNumber has defference pow."); }
			}
			#endregion

			#region 최소값
			double tarJari = Math.Log10(controlMin);
			//if(Math.Truncate(tarJari) == tarJari) { tarJari++; }
			tarJari = Math.Floor(tarJari);

			double jariGap = tarJari - jarisu;

			double mulVal = Math.Pow(10, jariGap);

			double adjustedTar =  controlMin / mulVal;

			int index;
			for(index = 0; index < numList.Count; index++)
			{
				if(adjustedTar <= numList[index]) { break; }
			}

			if(index == numList.Count)
			{
				adjustedTar = numList[0];
				mulVal *= 10;
			}
			else
			{
				adjustedTar = numList[index];
			}

			base.Minimum = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower((int)(adjustedTar * mulVal), (int)jarisu + 1, numList.ToArray());

			#endregion
			
			#region 최대값
			// 아래 식이 버림 연산이므로 다른 처리가 필요 없음.
			base.Maximum = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower(controlMax, (int)jarisu+1, numList.ToArray());
			#endregion
		}

		protected override string GetDisplayString()
		{
			string str = "0";

			switch(_ValueView)
			{
			case ValueViewMode.Non:
				return "";
			case ValueViewMode.AbsolutePerccent:
			case ValueViewMode.Percent:
			case ValueViewMode.Number:

				if(_ControlValue == null) { str = "0"; }
				else
				{
					if(_ControlValue.SeletedItem != null) { str = _ControlValue.SeletedItem.ToString(); }
					else { str = "0"; }
				}

				if(_ValueSymbolIsBack) { str += _ValueSymbol; }
				else { str = _ValueSymbol + str; }

				return str;
			case ValueViewMode.User:
				if(UserValueString != null) { str = UserValueString(this); }
				else { str = "-"; }

				if(_ValueSymbolIsBack) { str += _ValueSymbol; }
				else { str = _ValueSymbol + str; }

				return str;
			default:
				throw new ArgumentException();
			}
		}
	}
}
