using System;
using System.ComponentModel;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class TracekBarWithIControlInt : TrackBar, INanoeyeValueControl
	{
		#region Property & Variables

		protected SECtype.IControlInt _ControlValue =null;
		[DefaultValue(null)]
		public SECtype.IControlValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged -= new EventHandler(_ControlValue_EnableChanged);
				}

				if ((value != null) && !(value is SECtype.IControlInt)) { throw new ArgumentException(); }

				_ControlValue = value as SECtype.IControlInt;

				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);

					ValueReload();
				}
			}
		}

		protected bool _IsLimitMode = true;
		[DefaultValue(true)]
		public bool IsLimitedMode
		{
			get { return _IsLimitMode; }
			set
			{
				if (_IsLimitMode != value)
				{
					_IsLimitMode = value;
					ValueReload();
				}
			}
		}

		protected bool _IsValueOperation = true;
		[DefaultValue(true)]
		public bool IsValueOperation
		{
			get { return _IsValueOperation; }
			set
			{
				if (_IsValueOperation != value)
				{
					_IsValueOperation = value;
					ValueReload();
				}
			}
		}
		#endregion

		public TracekBarWithIControlInt()
		{
			InitializeComponent();
		}

		#region ControlValue 동기화
		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			ValueReload();
		}

		private void ValueReload()
		{
			if (_IsLimitMode)
			{
				this.Maximum = _ControlValue.Maximum;
				this.Minimum = _ControlValue.Minimum;
			}
			else
			{
				this.Maximum = _ControlValue.DefaultMax;
				this.Minimum = _ControlValue.DefaultMin;
			}

			if (_IsValueOperation)
			{
				this.Value = _ControlValue.Value;
			}
			else
			{
				this.Value = _ControlValue.Offset;
			}
		}
		#endregion

		#region Tracebar 동기화
		protected override void OnEnabledChanged(EventArgs e)
		{
			if (_ControlValue.Enable != this.Enabled)
			{
				_ControlValue.Enable = this.Enabled;
			}
			base.OnEnabledChanged(e);
		}

		protected override void OnValueChanged(EventArgs e)
		{
			if (_ControlValue.Value != this.Value)
			{
				_ControlValue.Value = this.Value;
			}
			base.OnValueChanged(e);
		}
		#endregion
	}
}
