using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SEC.Nanoeye.NanoView;
using SEC.GUIelement;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class LongscaleScrollWithICVD : LongScaleScroll
	{
		public LongscaleScrollWithICVD()
		{
			InitializeComponent();
		}

		private SEC.Nanoeye.DataType.IControlDouble icvd = null;
		[Browsable(false)]
		[DefaultValue(null)]
		public SEC.Nanoeye.DataType.IControlDouble ControlValue
		{
			get { return icvd; }
			set
			{
				if ( icvd != null )
				{
					icvd.ValueChanged -= new EventHandler(icvd_ValueChanged);
				}
				icvd = value;
				if ( icvd != null )
				{
					icvd.ValueChanged += new EventHandler(icvd_ValueChanged);

					ResetControl();
				}

			}
		}

		[Browsable(false)]
		[DefaultValue(0)]
		public int Offset
		{
			get
			{
				if ( DesignMode )
				{
					return 0;
				}
				return (int)(icvd.Offset / icvd.Precision);
			}
			set
			{
				if ( !DesignMode )
				{
					icvd.Offset = value * icvd.Precision;
				}
			}
		}

		bool changing = false;

		protected override void OnValueChanged(EventArgs e)
		{
			base.OnValueChanged(e);

			if ( changing ) { return; }	// icvd에서 받아서 다시 icvd에 넘겨 주는 것을 막음.
			changing = true;

			if ( icvd != null )
			{
				icvd.Value = base._Value * icvd.Precision;
			}
			changing = false;
		}

		private void ResetControl()
		{
			base._Maximum = (int)(icvd.Maximum / icvd.Precision);
			base._Minimum = (int)(icvd.Minimum / icvd.Precision);
			int value = (int)(icvd.Value / icvd.Precision);
			if ( base.Value !=  value)
			{
				changing = true; // icvd에서 받았음을 마크함.
				base.Value = value;
				changing = false;
			}

			Invalidate();
		}

		void icvd_ValueChanged(object sender, EventArgs e)
		{
			ResetControl();
		}

	}
}
