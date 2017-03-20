using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Dialog
{
	public partial class LensWobbleForm : Form
	{
		public LensWobbleForm()
		{
			InitializeComponent();
		}

		private SEC.Nanoeye.NanoColumn.ISEMController _Column = null;
		public SEC.Nanoeye.NanoColumn.ISEMController Column
		{
			get { return _Column; }
			set
			{
				_Column = value;
				cl1WobbleCb.ControlValue = (SECtype.IControlBool)_Column["LensCondenser1WobbleEnable"];
				cl1AmplHswd.ControlValue = _Column["LensCondenser1WobbleAmplitude"];
				cl1FreqHswd.ControlValue = _Column["LensCondenser1WobbleFrequence"];
				cl2WobbleCb.ControlValue = (SECtype.IControlBool)_Column["LensCondenser2WobbleEnable"];
				cl2AmplHswd.ControlValue = _Column["LensCondenser2WobbleAmplitude"];
				cl2FreqHswd.ControlValue = _Column["LensCondenser2WobbleFrequence"];
				olWobbleCb.ControlValue = (SECtype.IControlBool)_Column["LensObjectWobbleEnable"];
				olAmplHswd.ControlValue = _Column["LensObjectWobbleAmplitude"];
				olFreqHswd.ControlValue = _Column["LensObjectWobbleFrequence"];
			}
		}

		public double OL_Frequency
		{
			set
			{
				olFreqHswd.Value = (int)value;
			}
		}

		public double OL_Amplitude
		{
			set
			{
				olAmplHswd.Value = (int)value;
			}
		}

        public bool OL_Wobble
        {
            set
            {
                olWobbleCb.Checked = value;
            }
            
        }

       
	}
}
