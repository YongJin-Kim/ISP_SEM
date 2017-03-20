using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement.CircleButton
{
	public partial class CircleSingleXY : CircleButtonBase
	{
		public CircleSingleXY()
		{
			InitializeComponent();

			buttonRepeatTimer.Interval = 100;
			buttonRepeatTimer.Tick += new EventHandler(buttonRepeatTimer_Tick);
		}

		#region Circle Button Click
		protected override void OnButtonClick(ButtonLocation bl)
		{
			if ( bl == ButtonLocation.Center ) {
				HorizontalValue = _HorizontalDefault;
				VerticalValue = _VerticalDefault;
			}
		}

		#endregion
		#region Values
		public enum ValueType
		{
			Horizontal,
			Vertical
		}

		public delegate void ValueChangeEventHandler(object sender, ValueType type, int value);

		/// <summary>
		/// 값이 바뀌었음을 알림.
		/// </summary>
		public event ValueChangeEventHandler ValueChanged;

		protected virtual void OnValueChanged(ValueType type, int value)
		{
			if ( ValueChanged != null ) {
				ValueChanged(this, type, value);
			}
		}

		Timer buttonRepeatTimer = new Timer();

		ButtonLocation buttonDowned = ButtonLocation.Center;

		int repeatAccel = 0;

		protected override void OnButtonDown(ButtonLocation bl)
		{
			base.OnButtonDown(bl);
			if ( bl == ButtonLocation.Center ) { return; }
			buttonDowned = bl;
			repeatAccel = 0;
			buttonRepeatTimer_Tick(null, EventArgs.Empty);
			buttonRepeatTimer.Start();
		}

		protected override void OnButtonUp()
		{
			base.OnButtonUp();
			buttonRepeatTimer.Stop();
			buttonDowned = ButtonLocation.Center;
		}

		void buttonRepeatTimer_Tick(object sender, EventArgs e)
		{
			int addValue = 1;

			repeatAccel++;
			if ( repeatAccel > 50 ) {
				addValue = 50;
			}
			else if ( repeatAccel > 10 ) {
				addValue = 10;
			}

			int temp;

			switch ( buttonDowned ) {
			case ButtonLocation.InnerLeft:

				temp = _HorizontalValue - addValue;

				if ( temp < _HorizontalMin ) { HorizontalValue = _HorizontalMin; }
				else { HorizontalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _HorizontalValue);
				break;

			case ButtonLocation.OutterLeft:

				temp = _HorizontalValue - addValue * 5;

				if ( temp < _HorizontalMin ) { HorizontalValue = _HorizontalMin; }
				else { HorizontalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _HorizontalValue);
				break;

			case ButtonLocation.InnerRight:

				temp = _HorizontalValue + addValue;

				if ( temp > _HorizontalMax ) { HorizontalValue = _HorizontalMax; }
				else { HorizontalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _HorizontalValue);
				break;

			case ButtonLocation.OutterRight:

				temp = _HorizontalValue + addValue * 5;

				if ( temp > _HorizontalMax ) { HorizontalValue = _HorizontalMax; }
				else { HorizontalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _HorizontalValue);
				break;




			case ButtonLocation.InnerTop:

				temp = _VerticalValue + addValue;

				if ( temp < _VerticalMin ) { VerticalValue = _VerticalMin; }
				else { VerticalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _VerticalValue);
				break;

			case ButtonLocation.OutterTop:

				temp = _VerticalValue + addValue * 5;

				if ( temp < _VerticalMin ) { VerticalValue = _VerticalMin; }
				else { VerticalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _VerticalValue);
				break;

			case ButtonLocation.InnerBottom:

				temp = _VerticalValue - addValue;

				if ( temp > _VerticalMax ) { VerticalValue = _VerticalMax; }
				else { VerticalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _VerticalValue);
				break;

			case ButtonLocation.OutterBottom:

				temp = _VerticalValue - addValue * 5;

				if ( temp > _VerticalMax ) { VerticalValue = _VerticalMax; }
				else { VerticalValue = temp; }

				OnValueChanged(ValueType.Horizontal, _VerticalValue);
				break;

			default:
				throw new Exception();
			}
		}


		private void HorizontalValueChange()
		{
			float value = (_HorizontalValue - _HorizontalMin) * 200 / (_HorizontalMax - _HorizontalMin) - 100;
			if ( (value < -50f) || (value > 50f) ) {
				if ( value < 0 ) {
					ValueOutterSide = value * 2 + 100;
					ValueInnerSide = -100;
				}
				else {
					ValueOutterSide = value * 2 - 100;
					ValueInnerSide = 100;
				}
			}
			else {
				ValueOutterSide = 0;
				ValueInnerSide = value * 2;
			}
		}

		private void VerticalValueChange()
		{
			float value = _VerticalValue * 200 / (_VerticalMax - _VerticalMin) ;
			if ( (value < -50f) || (value > 50f) ) {
				if ( value < 0 ) {
					ValueOutterUpBottom = value * 2 + 100;
					ValueInnerUpBottom = -100;
				}
				else {
					ValueOutterUpBottom = value * 2 - 100;
					ValueInnerUpBottom = 100;
				}
			}
			else {
				ValueOutterUpBottom = 0;
				ValueInnerUpBottom = value * 2;
			}
		}



		private int _HorizontalMax = 2047;
		public int HorizontalMax
		{
			get { return _HorizontalMax; }
			set
			{
				_HorizontalMax = value;
				HorizontalValueChange();
			}
		}

		private int _HorizontalMin = -2048;
		public int HorizontalMin
		{
			get { return _HorizontalMin; }
			set
			{
				_HorizontalMin = value; 
				HorizontalValueChange();
			}
		}


		private int _HorizontalValue = 0;
		public int HorizontalValue
		{
			get { return _HorizontalValue; }
			set
			{
				_HorizontalValue = value;
				HorizontalValueChange();
			}
		}
		
		private int _HorizontalDefault = 0;
		public int HorizontalDefault
		{
			get { return _HorizontalDefault; }
			set { _HorizontalDefault = value; }
		}

		private int _VerticalMax = 2047;
		public int VerticalMax
		{
			get { return _VerticalMax; }
			set { _VerticalMax = value;
			VerticalValueChange();
			}
		}

		private int _VerticalMin = -2048;
		public int VerticalMin
		{
			get { return _VerticalMin; }
			set
			{
				_VerticalMin = value;
				VerticalValueChange();
			}
		}

		private int _VerticalValue = 0;
		public int VerticalValue
		{
			get { return _VerticalValue; }
			set
			{
				_VerticalValue = value;
				VerticalValueChange();
			}
		}

		private int _VerticalDefault = 0;
		public int VerticalDefault
		{
			get { return _VerticalDefault; }
			set { _VerticalDefault = value; }
		}
		#endregion

	}
}
