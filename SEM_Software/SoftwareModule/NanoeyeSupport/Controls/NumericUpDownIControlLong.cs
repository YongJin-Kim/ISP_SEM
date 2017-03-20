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
	public partial class NumericUpDownIControlLong : NumericUpDown
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

		private bool _UsePrecision = true;
		[DefaultValue(true)]
		public bool UsePrecision
		{
			get { return _UsePrecision; }
			set { _UsePrecision = value; }
		}

		private SECtype.IControlLong _ControlValue = null;
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

				_ControlValue = value as SECtype.IControlLong;

				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);

					ValueReload();
				}
			}
		}

		public NumericUpDownIControlLong()
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

		private bool initing = false;

		private void ValueReload()
		{
			Action act = () =>
			{
				if(_ControlValue != null)
				{
					initing = true;
					if(_UsePrecision)
					{
						this.Maximum = (decimal)(_ControlValue.DefaultMax / _ControlValue.Precision);
						this.Minimum = (decimal)(_ControlValue.DefaultMin / _ControlValue.Precision);
						switch(_DisplayType)
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
							this.Maximum = decimal.MaxValue;
							this.Minimum = decimal.MinValue;
							this.Value = (decimal)(_ControlValue.Offset / _ControlValue.Precision);
							break;
						case DisplayTypeEnum.Value:
							this.Value = (decimal)(_ControlValue.Value / _ControlValue.Precision);
							break;
						}
					}
					else
					{
						this.Maximum = (decimal)(_ControlValue.DefaultMax);
						this.Minimum = (decimal)(_ControlValue.DefaultMin);
						switch(_DisplayType)
						{
						case DisplayTypeEnum.DefaultMaximum:
							this.Value = (decimal)(_ControlValue.DefaultMax);
							break;
						case DisplayTypeEnum.DefaultMinimum:
							this.Value = (decimal)(_ControlValue.DefaultMin);
							break;
						case DisplayTypeEnum.Maximum:
							this.Value = (decimal)(_ControlValue.Maximum);
							break;
						case DisplayTypeEnum.Minimum:
							this.Value = (decimal)(_ControlValue.Minimum);
							break;
						case DisplayTypeEnum.Offset:
							this.Maximum = decimal.MaxValue;
							this.Minimum = decimal.MinValue;
							this.Value = (decimal)(_ControlValue.Offset);
							break;
						case DisplayTypeEnum.Value:
							this.Value = (decimal)(_ControlValue.Value);
							break;
						}
					}

					initing = false;
				}
			};

			if(InvokeRequired) { this.Invoke(act); }
			else { act(); }
		}
		

		protected override void OnValueChanged(EventArgs e)
		{
			base.OnValueChanged(e);

			if (initing) { return; }

			if (_ControlValue != null)
			{
				if (_UsePrecision)
				{
					switch (_DisplayType)
					{
					case DisplayTypeEnum.DefaultMaximum:
					case DisplayTypeEnum.DefaultMinimum:
						break;
					case DisplayTypeEnum.Maximum:
						_ControlValue.Maximum = (long)((double)this.Value * _ControlValue.Precision);
						break;
					case DisplayTypeEnum.Minimum:
						_ControlValue.Minimum = (long)((double)this.Value * _ControlValue.Precision);
						break;
					case DisplayTypeEnum.Offset:
						_ControlValue.Offset = (long)((double)this.Value * _ControlValue.Precision);
						break;
					case DisplayTypeEnum.Value:
						_ControlValue.Value = (long)((double)this.Value * _ControlValue.Precision);
						break;
					}
				}
				else
				{
					switch (_DisplayType)
					{
					case DisplayTypeEnum.DefaultMaximum:
					case DisplayTypeEnum.DefaultMinimum:
						break;
					case DisplayTypeEnum.Maximum:
						_ControlValue.Maximum = (long)((double)this.Value);
						break;
					case DisplayTypeEnum.Minimum:
						_ControlValue.Minimum = (long)((double)this.Value);
						break;
					case DisplayTypeEnum.Offset:
						_ControlValue.Offset = (long)((double)this.Value);
						break;
					case DisplayTypeEnum.Value:
						_ControlValue.Value = (long)((double)this.Value);
						break;
					}
				}
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			SetEnable();
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
