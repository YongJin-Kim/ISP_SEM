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
	public partial class HScrollWithControlvalueDouble : HScrollBar
	{
		public HScrollWithControlvalueDouble()
		{
			InitializeComponent();
		}

		private SECtype.IControlDouble  _ControlValue = null;
		public SECtype.IControlDouble ControlValue
		{
			get { return _ControlValue; }
		}

		private int _Multifly = 1000;
		[DefaultValue(1000)]
		public int Multifly
		{
			get { return _Multifly; }
//			set { _Multifly = value; }
		}

		public void SetControlValue(SECtype.IControlDouble con)
		{
			if ( _ControlValue != null ) {
				((IColumnValue)_ControlValue).ValueChanged -= new EventHandler(HScrollWithControlvalueDouble_ValueChanged);
			}

			_ControlValue = con;

			_Multifly = (int)(1 / _ControlValue.Precision);
			this.Maximum = (int)(_ControlValue.Maximum * _Multifly);
			this.Minimum = (int)(_ControlValue.Minimum * _Multifly);
			this.Value = (int)(_ControlValue.Value * _Multifly);

			((IColumnValue)_ControlValue).ValueChanged += new EventHandler(HScrollWithControlvalueDouble_ValueChanged);
		}

		void HScrollWithControlvalueDouble_ValueChanged(object sender, EventArgs e)
		{
			this.Maximum = (int)(_ControlValue.Maximum / _ControlValue.Precision);
			this.Minimum = (int)(_ControlValue.Minimum / _ControlValue.Precision);
			if (this.Value != (int)(_ControlValue.Value / _ControlValue.Precision))
			{
				this.Value = (int)(_ControlValue.Value / _ControlValue.Precision);
			}
		}

		protected override void OnValueChanged(EventArgs e)
		{
			base.OnValueChanged(e);
			_ControlValue.Value = this.Value * _ControlValue.Precision;
		}
	}
}
