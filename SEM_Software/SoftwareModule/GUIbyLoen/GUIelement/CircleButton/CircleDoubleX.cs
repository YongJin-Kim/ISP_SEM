using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace SEC.GUIelement.CircleButton
{
	public partial class CircleDoubleX : CircleButtonBase
	{
		public CircleDoubleX()
		{
			InitializeComponent();

			VisiableTop = false;
			VisiableBottom = false;

			buttonRepeatTimer.Interval = 100;
			buttonRepeatTimer.Tick += new EventHandler(buttonRepeatTimer_Tick);
		}

		#region Circle Button Click
		protected override void OnButtonClick(ButtonLocation bl)
		{
			if ( bl == ButtonLocation.Center ) {
				CoarseValue = _CoarseDefault;
				FineValue = _FineDefault;
			}
		}
		#endregion

		#region Values

		public enum ValueType
		{
			Coarse,
			Fine
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
				
				temp = FineValue - addValue;
				if ( temp < FineMin ) { FineValue = FineMin; }
				else { FineValue = temp; }

				OnValueChanged(ValueType.Fine, FineValue);
				break;
			case ButtonLocation.InnerRight:

				temp = FineValue + addValue;
				if ( temp > FineMax ) { FineValue = FineMax; }
				else { FineValue = temp; }

				OnValueChanged(ValueType.Fine, FineValue);
				break;
			case ButtonLocation.OutterLeft:
				CoarseValue -= addValue;

				temp = CoarseValue - addValue;
				if ( temp < CoarseMin ) { CoarseValue = CoarseMin; }
				else { CoarseValue = temp; }

				OnValueChanged(ValueType.Coarse, CoarseValue);
				break;
			case ButtonLocation.OutterRight:

				temp = CoarseValue + addValue;
				if ( temp > CoarseMax ) { CoarseValue = CoarseMax; }
				else { CoarseValue = temp; }
				
				OnValueChanged(ValueType.Coarse, CoarseValue);
				break;
			default:
				throw new Exception();
			}
		}


		private void CoarseValueChange()
		{
			ValueOutterSide = _CoarseValue * 200 / (_CoarseMax - _CoarseMin) - 100;
		}

		private void FineValueChange()
		{
			ValueInnerSide = _FineValue * 200 / (_FineMax - _FineMin) -  100;
		}

		private int _CoarseMax = 4095;
		public int CoarseMax
		{
			get { return _CoarseMax; }
			set
			{
				_CoarseMax = value;
				CoarseValueChange();
			}
		}

		private int _CoarseMin = 0;
		public int CoarseMin
		{
			get { return _CoarseMin; }
			set
			{
				_CoarseMin = value;
				CoarseValueChange();
			}
		}

		private int _CoarseValue = 2048;
		public int CoarseValue
		{
			get { return _CoarseValue; }
			set
			{
				_CoarseValue = value;
				CoarseValueChange();
			}
		}

		private int _CoarseDefault = 2048;
		public int CoarseDefault
		{
			get { return _CoarseDefault; }
			set { _CoarseDefault = value; }
		}

		private int _FineMax = 4095;
		public int FineMax
		{
			get { return _FineMax; }
			set
			{
				_FineMax = value;
				FineValueChange();
			}
		}

		private int _FineMin = 0;
		public int FineMin
		{
			get { return _FineMin; }
			set
			{
				_FineMin = value;
				FineValueChange();
			}
		}

		private int _FineValue = 2048;
		public int FineValue
		{
			get { return _FineValue; }
			set
			{
				_FineValue = value;
				FineValueChange();
			}
		}

		private int _FineDefault = 2048;
		public int FineDefault
		{
			get { return _FineDefault; }
			set { _FineDefault = value; }
		}
		#endregion

	}
}
