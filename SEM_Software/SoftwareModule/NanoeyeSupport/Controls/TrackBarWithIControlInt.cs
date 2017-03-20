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
	public partial class TrackBarWithIControlInt : TrackBar, INanoeyeValueControl
	{
		#region Property & Variables
		private SECtype.IControlInt _ControlValue = null;
		[DefaultValue(null), Browsable(false)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_Icvd_ValueChanged);
					_ControlValue.EnableChanged -= new EventHandler(_Icvd_EnableChanged);
				}

				_ControlValue = value as SECtype.IControlInt;

				if (value != null)
				{
					RestoreValue();
					_ControlValue.ValueChanged += new EventHandler(_Icvd_ValueChanged);
					_ControlValue.EnableChanged += new EventHandler(_Icvd_EnableChanged);

					this.Enabled = _ControlValue.Enable;
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

		public TrackBarWithIControlInt()
		{
			InitializeComponent();
		}

		void _Icvd_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}

		private void RestoreValue()
		{
			if (_ControlValue == null) { return; }

			this.SuspendLayout();

			if (_IsLimitedMode)
			{
				this.Maximum = (int)Math.Ceiling(_ControlValue.Maximum / _ControlValue.Precision);
				this.Minimum = (int)(_ControlValue.Minimum / _ControlValue.Precision);
			}
			else
			{
				this.Maximum = (int)Math.Ceiling(_ControlValue.DefaultMax / _ControlValue.Precision);
				this.Minimum = (int)(_ControlValue.DefaultMin / _ControlValue.Precision);
			}

			if (_IsValueOperation)
			{
				base.Value = (int)(_ControlValue.Value / _ControlValue.Precision);
			}
			else
			{
				base.Value = (int)(_ControlValue.Offset / _ControlValue.Precision);
			}

			this.ResumeLayout();
			this.Invalidate();
		}

		void _Icvd_ValueChanged(object sender, EventArgs e)
		{
			if(InvokeRequired)
			{
				Action act = () =>
				{
					RestoreValue();
				};
				this.BeginInvoke( act );
			}
			else
			{
				RestoreValue();
			}
		}

		protected override void OnValueChanged(EventArgs e)
		{
			IcvdValueChage();
			base.OnValueChanged(e);
		}

		private void IcvdValueChage()
		{
			if (_ControlValue != null)
			{
				int val  = (int)(base.Value * _ControlValue.Precision);
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

		//public override int Maximum
		//{
		//    get { return base.Maximum; }
		//    set
		//    {
		//        if (base.Maximum != value)
		//        {
		//            base.Maximum = value;
		//        }
		//        if (_ControlValue != null)
		//        {
		//            if (_IsLimitedMode)
		//            {
		//                int val  = (int)(value * _ControlValue.Precision);
		//                if (_ControlValue.Maximum != val)
		//                {
		//                    _ControlValue.Maximum = val;
		//                }
		//            }
		//        }
		//    }
		//}

		//public override int Minimum
		//{
		//    get { return base.Minimum; }
		//    set
		//    {
		//        if (base.Minimum != value)
		//        {
		//            base.Minimum = value;
		//        }
		//        if (_ControlValue != null)
		//        {
		//            if (_IsLimitedMode)
		//            {
		//                int val = (int)(value * _ControlValue.Precision);
		//                if (_ControlValue.Minimum != val)
		//                {
		//                    _ControlValue.Minimum = val;
		//                }
		//            }
		//        }
		//    }

		//}

		
	}
}
