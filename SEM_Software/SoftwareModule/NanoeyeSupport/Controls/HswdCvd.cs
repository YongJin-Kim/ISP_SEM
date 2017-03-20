using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class HswdCvd : SEC.GUIelement.HscrollWithDisplay, INanoeyeValueControl
	{
		#region Property & Variables

		private SECtype.IControlDouble _ControlValue;
		[DefaultValue(null)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_Icvd_ValueChanged);
				}

				_ControlValue = value as SECtype.IControlDouble;

				if (value != null)
				{
					RestoreValue();
					_ControlValue.ValueChanged += new EventHandler(_Icvd_ValueChanged);
				}
			}
		}

		private bool _IsLimitedMode = true;
		[DefaultValue(true)]
		public bool IsLimitedMode
		{
			get { return _IsLimitedMode; }
			set
			{
				if (_IsLimitedMode != value)
				{
					_IsLimitedMode = value;
					RestoreValue();
				}
			}
		}

		private bool _IsValueOperation = true;
		/// <summary>
		/// True 이면 Value 제어.
		/// False 이면 Offset 제어.
		/// </summary>
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
		#endregion

		public HswdCvd()
		{
			InitializeComponent();
		}

		private void RestoreValue()
		{
			if (_ControlValue == null) { return; }

			this.SuspendLayout();

			if (_IsLimitedMode)
			{
				this.Maximum = (int)Math.Floor(_ControlValue.Maximum / _ControlValue.Precision);
				this.Minimum = (int)Math.Ceiling(_ControlValue.Minimum / _ControlValue.Precision);
			}
			else
			{
				this.Maximum = (int)Math.Floor(_ControlValue.DefaultMax / _ControlValue.Precision);
				this.Minimum = (int)Math.Ceiling(_ControlValue.DefaultMin / _ControlValue.Precision);
			}

			if (_IsValueOperation)
			{
				base.Value = (int)Math.Round(_ControlValue.Value / _ControlValue.Precision);
			}
			else
			{
				base.Value = (int)Math.Round(_ControlValue.Offset / _ControlValue.Precision);
			}

			ValueDisplayChange();
			this.ResumeLayout();
			this.Invalidate();
		}

		void _Icvd_ValueChanged(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Action act = () => { RestoreValue(); };
				this.BeginInvoke(act);
			}
			else
			{
				RestoreValue();
			}
		}

		public override int Value
		{
			get { return base.Value; }
			set
			{
				if (base.Value != value)
				{
					IcvdValueChage();
					base.Value = value;
				}
			}
		}

		private void IcvdValueChage()
		{
			if (_ControlValue != null)
			{
				double val  = base.Value * _ControlValue.Precision;
				if (_IsValueOperation)
				{
					if (_ControlValue.Value != val)
					{
						_ControlValue.Value = val;
					}
				}
				else
				{
					if (_ControlValue.Offset != val)
					{
						_ControlValue.Offset = val;
					}
				}
			}
		}

		public override int Maximum
		{
			get { return base.Maximum; }
			set
			{
				if (base.Maximum != value)
				{
					base.Maximum = value;
				}
				if (_ControlValue != null)
				{
					if (_IsLimitedMode)
					{
						double val  = value * _ControlValue.Precision;
						if (_ControlValue.Maximum != val)
						{
							_ControlValue.Maximum = val;
						}
					}
				}
			}
		}

		public override int Minimum
		{
			get { return base.Minimum; }
			set
			{
				if (base.Minimum != value)
				{
					base.Minimum = value;
				}
				if (_ControlValue != null)
				{
					if (_IsLimitedMode)
					{
						double val =  value * _ControlValue.Precision;
						if (_ControlValue.Minimum != val)
						{
							_ControlValue.Minimum = val;
						}
					}
				}
			}
		}

		protected override void hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (_ControlValue != null)
			{
				double result = hScrollBar.Value;

				result = result * _ControlValue.Precision;

				if (result < (_ControlValue.Minimum))
				{
					result = (_ControlValue.Minimum);
				}
				else if (result > (_ControlValue.Maximum))
				{
					result = (_ControlValue.Maximum);
				}

				if (_IsValueOperation)
				{
					_ControlValue.Value = result;
				}
				else
				{
					_ControlValue.Offset = result;
				}
			}
			base.hScrollBar_ValueChanged(sender, e);
		}
	}
}
