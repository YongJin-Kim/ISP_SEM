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
	public partial class CheckBoxWithIControlInt: CheckBox
	{
		private bool changing = false;

		public CheckBoxWithIControlInt()
		{
			InitializeComponent();
		}

		private SECtype.IControlInt _ControlValue = null;
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
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
					_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);
					changing = true;
					this.Checked = (_ControlValue.Value != 0);
					changing = false;
				}
			}
		}

		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			if(this.Checked != (_ControlValue.Value != 0))
			{
				changing = true;
				this.Checked = (_ControlValue.Value != 0);
				changing = false;
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			if(!changing) { _ControlValue.Value = (this.Checked ? 1 : 0); }
			base.OnCheckedChanged( e );


		}
	}
}
