using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SEC.Nanoeye.NanoView;
using SEC.Nanoeye.NanoColumn;
using SECtype = SEC.GenericSupport.DataType;

namespace NanoeyeTestControls
{
	public partial class HScrollWithControlvalueInt : HScrollBar
	{
		public HScrollWithControlvalueInt()
		{
			InitializeComponent();
		}

		private SECtype.IControlInt  _ControlValue = null;
		public SECtype.IControlInt ControlValue
		{
			get { return _ControlValue; }
		}

		public void SetControlValue(SECtype.IControlInt con)
		{
			if ( _ControlValue != null ) {
				((IColumnValue)_ControlValue).ValueChanged -= new EventHandler(HScrollWithControlvalueInt_ValueChanged);
			}

			_ControlValue = con;

			this.Maximum = (int)(_ControlValue.Maximum);
			this.Minimum = (int)(_ControlValue.Minimum);
			this.Value = (int)(_ControlValue.Value);

			((IColumnValue)_ControlValue).ValueChanged += new EventHandler(HScrollWithControlvalueInt_ValueChanged);
		}

		void HScrollWithControlvalueInt_ValueChanged(object sender, EventArgs e)
		{
			this.Maximum = _ControlValue.Maximum;
			this.Minimum = _ControlValue.Minimum;
			if (this.Value != _ControlValue.Value)
			{
				this.Value = _ControlValue.Value;
			}
		}

		protected override void OnValueChanged(EventArgs e)
		{
			base.OnValueChanged(e);
			_ControlValue.Value = this.Value;
		}
	}
}
