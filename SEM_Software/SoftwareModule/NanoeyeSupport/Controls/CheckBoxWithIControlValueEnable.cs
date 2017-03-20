using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class CheckBoxWithIControlValueEnable : System.Windows.Forms.CheckBox
	{
		private SECtype.IValue _ConrolValue = null;
		public SECtype.IValue ControlValue
		{
			get { return _ConrolValue; }
			set
			{
				if (_ConrolValue != null)
				{
					_ConrolValue.EnableChanged -= new EventHandler(_ConrolValue_EnableChanged);
				}
				_ConrolValue = value;
				if (_ConrolValue != null)
				{
					_ConrolValue.EnableChanged += new EventHandler(_ConrolValue_EnableChanged);

					this.Checked = _ConrolValue.Enable;
				}
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			if (_ConrolValue.Enable != this.Checked)
			{
				_ConrolValue.Enable = this.Checked;
			}
		}

		void _ConrolValue_EnableChanged(object sender, EventArgs e)
		{
			if (this.Checked != _ConrolValue.Enable)
			{
				this.Checked = _ConrolValue.Enable;
			}
		}

		public CheckBoxWithIControlValueEnable()
		{
			InitializeComponent();
		}

	}
}
