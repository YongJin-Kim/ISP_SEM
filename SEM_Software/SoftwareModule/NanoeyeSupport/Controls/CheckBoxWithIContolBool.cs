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
	public partial class CheckBoxWithIControlBool : CheckBox
	{
		public CheckBoxWithIControlBool()
		{
			InitializeComponent();
		}

		private SECtype.IControlBool _ControlValue = null;
		public SECtype.IControlBool ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
					//_ControlValue.EnableChanged -= new EventHandler(_ControlValue_EnableChanged);
				}
				_ControlValue = value;
				if (_ControlValue != null)
				{
					_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
					//_ControlValue.EnableChanged += new EventHandler(_ControlValue_EnableChanged);
					this.Checked = _ControlValue.Value;
				}
			}
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			if (this.Checked != _ControlValue.Value)
			{
				if (InvokeRequired)
				{
					Action act = () => { this.Checked = _ControlValue.Value; };
					this.BeginInvoke(act);
				}
				else
				{
					this.Checked = _ControlValue.Value;
				}
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			if (this.Checked != _ControlValue.Value)
			{
				_ControlValue.Value = this.Checked ;
			}
			
		}
	}
}
