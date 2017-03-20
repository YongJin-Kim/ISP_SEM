using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class NumericUpDownIControlDouble : NumericUpDown
	{
		private DisplayTypeEnum _DisplayType = DisplayTypeEnum.Value;
		[DefaultValue(typeof(DisplayTypeEnum), "Value")]
		public DisplayTypeEnum DisplayType
		{
			get { return _DisplayType; }
			set
			{
				_DisplayType = value;
				SetEnable();
				ValueReload();
			}
		}

		private SECtype.IControlDouble _ControlValue = null;
		[DefaultValue(null)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged -= new EventHandler(_ControlValue_EnableChanged);
				}

				_ControlValue = value as SECtype.IControlDouble;

				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);

					ValueReload();
				}
			}
		}

		public NumericUpDownIControlDouble()
		{
			InitializeComponent();
		}

		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			SetEnable();
		}

		private void SetEnable()
		{
			switch (_DisplayType)
			{
			case DisplayTypeEnum.DefaultMaximum:
			case DisplayTypeEnum.DefaultMinimum:
				this.Enabled = false;
				break;
			case DisplayTypeEnum.Maximum:
			case DisplayTypeEnum.Minimum:
			case DisplayTypeEnum.Offset:
			case DisplayTypeEnum.Value:
				if (_ControlValue != null)
				{
					this.Enabled = _ControlValue.Enable;
				}
				break;
			}
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			ValueReload();
		}

		private void ValueReload()
		{
			if (_ControlValue != null)
			{
				this.Maximum = (decimal)(_ControlValue.DefaultMax / _ControlValue.Precision);
				this.Minimum = (decimal)(_ControlValue.DefaultMin / _ControlValue.Precision);
				switch (_DisplayType)
				{
				case DisplayTypeEnum.DefaultMaximum:
					this.Value = (decimal)(_ControlValue.DefaultMax / _ControlValue.Precision);
					break;
				case DisplayTypeEnum.DefaultMinimum:
					this.Value = (decimal)(_ControlValue.DefaultMin / _ControlValue.Precision);
					break;
				case DisplayTypeEnum.Maximum:
					this.Value = (decimal)(_ControlValue.Maximum / _ControlValue.Precision);
					break;
				case DisplayTypeEnum.Minimum:
					this.Value = (decimal)(_ControlValue.Minimum / _ControlValue.Precision);
					break;
				case DisplayTypeEnum.Offset:
					this.Value = (decimal)(_ControlValue.Offset / _ControlValue.Precision);
					break;
				case DisplayTypeEnum.Value:
					this.Value = (decimal)(_ControlValue.Value / _ControlValue.Precision);
					break;
				}
			}
		}

		protected override void OnValueChanged(EventArgs e)
		{
			double 	temp = (double)this.Value * _ControlValue.Precision;
			if (_ControlValue != null)
			{
				switch (_DisplayType)
				{
				case DisplayTypeEnum.DefaultMaximum:
				case DisplayTypeEnum.DefaultMinimum:
					break;
				case DisplayTypeEnum.Maximum:
					if (_ControlValue.Maximum != temp) { _ControlValue.Maximum = temp; }
					break;
				case DisplayTypeEnum.Minimum:
					if (_ControlValue.Minimum != temp) { _ControlValue.Minimum = temp; }
					break;
				case DisplayTypeEnum.Offset:
					temp = (double)this.Value * _ControlValue.Precision;
					if (_ControlValue.Offset != temp) { _ControlValue.Offset = temp; }
					break;
				case DisplayTypeEnum.Value:
					if (_ControlValue.Value != temp) { _ControlValue.Value = temp; }
					break;
				}
			}
			base.OnValueChanged(e);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			SetEnable();
			base.OnEnabledChanged(e);
		}

		public enum DisplayTypeEnum
		{ 
			DefaultMaximum,
			DefaultMinimum,
			Maximum,
			Minimum,
			Value,
			Offset
		}
	}
}
