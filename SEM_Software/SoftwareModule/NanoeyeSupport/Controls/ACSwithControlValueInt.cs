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
	public partial class ACSwithControlValueInt : SEC.GUIelement.AutoCenterSlide
	{
		private SECtype.IControlInt _ControlValue = null;
		[System.ComponentModel.DefaultValue(null)]
		public SECtype.IControlInt ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged -= new EventHandler(_ControlValue_EnableChanged);
				}
				_ControlValue = value;
				_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
				_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);

				this.Maximum = _ControlValue.Maximum;
				this.Minimum = _ControlValue.Minimum;
				this.Value = _ControlValue.Value;
			}
		}

		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}

		public ACSwithControlValueInt()
		{
			InitializeComponent();
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			if (this.Maximum != ControlValue.Maximum) { this.Maximum = ControlValue.Maximum; }
			if (this.Minimum != ControlValue.Minimum) { this.Minimum = ControlValue.Minimum; }
			if (this.Value != ControlValue.Value) { this.Value = ControlValue.Value; }
		}

		protected override void OnValueChanged()
		{
			base.OnValueChanged();
			if (_ControlValue != null)
			{
				if (_ControlValue.Value != this.Value) { _ControlValue.Value = this.Value; }
			}
		}
	}
}

