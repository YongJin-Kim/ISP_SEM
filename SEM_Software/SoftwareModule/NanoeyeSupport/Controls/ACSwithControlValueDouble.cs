using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class ACSwithControlValueDouble : SEC.GUIelement.AutoCenterSlide, INanoeyeValueControl
	{
		#region Properties & Variables
		private bool _IsLimitMode = true;
		[DefaultValue(true)]
		public bool IsLimitedMode
		{
			get { return _IsLimitMode; }
			set
			{
				if (_IsLimitMode != value)
				{
					_IsLimitMode = value;
					RestoreValue();
				}
			}
		}

		private bool _IsValueOperation = true;
		[DefaultValue(true)]
		public bool IsValueOperation
		{
			get { return _IsValueOperation; }
			set
			{
				if (_IsValueOperation != value)
				{
					_IsValueOperation = value;
					RestoreValue();
				}
			}
		}

		private SECtype.IControlDouble _ControlValue = null;
		[System.ComponentModel.DefaultValue(null)]
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

				_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
				_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);

				this.SuspendLayout();
				this.Maximum = (int)(_ControlValue.Maximum / _ControlValue.Precision);
				this.Minimum = (int)(_ControlValue.Minimum / _ControlValue.Precision);
				this.Value = (int)(_ControlValue.Value / _ControlValue.Precision);
				this.ResumeLayout();

				this.Enabled = _ControlValue.Enable;
			}
		}
		#endregion

		public ACSwithControlValueDouble()
		{
			InitializeComponent();
		}

		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			RestoreValue();
		}

		protected override void OnValueChanged()
		{
			base.OnValueChanged();
			if (_ControlValue != null)
			{
				if (_IsValueOperation)
				{
					if (_ControlValue.Value != (double)this.Value * _ControlValue.Precision)
					{
						_ControlValue.Value = (double)this.Value * _ControlValue.Precision;
					}
				}
				else
				{
					if (_ControlValue.Offset != (double)this.Value * _ControlValue.Precision)
					{
						_ControlValue.Offset = (double)this.Value * _ControlValue.Precision;
					}
				}
			}
		}
		protected override void UpdateValue(int temp)
		{
			if ((_ControlValue == null) || (_ControlValue.Enable))
			{
				base.UpdateValue(temp);
			}
		}

		private void RestoreValue()
		{
			if (_ControlValue == null) { return; }

			this.SuspendLayout();
			if (_IsLimitMode)
			{
				this._Maximum = (int)(_ControlValue.Maximum / _ControlValue.Precision);
				this._Minimum = (int)(_ControlValue.Minimum / _ControlValue.Precision);
			}

			else
			{
				this._Maximum = (int)(_ControlValue.DefaultMax / _ControlValue.Precision);
				this._Minimum = (int)(_ControlValue.DefaultMin / _ControlValue.Precision);
			}

			if (_IsValueOperation)
			{
				this.Value = (int)(_ControlValue.Value / _ControlValue.Precision);
			}
			else
			{
				this._Value = (int)(_ControlValue.Offset / _ControlValue.Precision);
			}
			this.ResumeLayout();
		}
	}
}
