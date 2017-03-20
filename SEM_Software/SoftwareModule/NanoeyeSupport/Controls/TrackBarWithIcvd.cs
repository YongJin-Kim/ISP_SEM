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
	public partial class TrackBarWithIcvd : TrackBar, INanoeyeValueControl
	{
		#region Property & Variables

		protected SECtype.IControlDouble icd = null;	
		[DefaultValue(null),		Browsable(false)]
		public SEC.GenericSupport.DataType.IValue ControlValue
		{
			get { return icd; }
			set
			{
				if (icd != null) { icd.ValueChanged -= new EventHandler(icd_ValueChanged); }
				icd = value as SECtype.IControlDouble;
				if (icd != null)
				{
					icd.ValueChanged += new EventHandler(icd_ValueChanged);
					ResetValue();
				}
			}
		}

		protected bool _IsLimitedMode = true;
		[DefaultValue(true)]
		public bool IsLimitedMode
		{
			get { return _IsLimitedMode; }
			set
			{
				_IsLimitedMode = value;
				ResetValue();
			}
		}

		protected bool _IsValueOperation = true;
		[DefaultValue(true)]
		public bool IsValueOperation
		{
			get { return _IsValueOperation; }
			set
			{
				_IsValueOperation = value;
				ResetValue();
			}
		}

		#endregion

		public TrackBarWithIcvd()
		{
			InitializeComponent();
		}

		protected override void OnValueChanged(EventArgs e)
		{
			icd.Value = this.Value * icd.Precision;
			base.OnValueChanged(e);
		}

		Action<TrackBarWithIcvd, SECtype.IControlDouble> act = (tbwi, icd) =>
		{
			tbwi.BeginInit();
			tbwi.Maximum = (int)Math.Floor (icd.Maximum / icd.Precision);
			tbwi.Minimum = (int)Math.Ceiling(icd.Minimum / icd.Precision);
			tbwi.Value = (int)Math.Round(icd.Value / icd.Precision);
			tbwi.EndInit();
		};

		private void ResetValue()
		{
			this.BeginInvoke(act, this, icd);	
		}

		void icd_ValueChanged(object sender, EventArgs e)
		{
			ResetValue();
		}
	}
}
