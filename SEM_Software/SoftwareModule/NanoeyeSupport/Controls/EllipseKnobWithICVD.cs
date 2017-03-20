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
	public partial class EllipseKnobWithICVD : SEC.GUIelement.EllipseKnob
	{
		private SECtype.IControlDouble _ControlValue = null;
		[DefaultValue(null)]
		[Browsable(false)]
		public SECtype.IControlDouble ControlValue
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
					RestoreValue();
				}
			}
		}

		void _ControlValue_EnableChanged(object sender, EventArgs e)
		{
			this.Enabled = _ControlValue.Enable;
		}


		public EllipseKnobWithICVD()
		{
			InitializeComponent();
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			Action act = () =>
			{
				RestoreValue();
			};
			if (this.InvokeRequired) { this.BeginInvoke(act); }
			else { act(); }
		}

		private void RestoreValue()
		{
			int max = (int)Math.Floor(_ControlValue.Maximum / _ControlValue.Precision);
			int min = (int)Math.Ceiling(_ControlValue.Minimum / _ControlValue.Precision);
			int val = (int)Math.Round(_ControlValue.Value / _ControlValue.Precision);

			base.BeginInit();
			if (base.Maximum != max) { base.Maximum = max; }
			if (base.Minimum != min) { base.Minimum = min; }
			if (base.Value != val) { base.Value = val; }
			base.EndInit();
		}

		protected override void OnValueChanged()
		{
			if (_ControlValue != null)
			{
				double val  =  base.Value * _ControlValue.Precision;
				if (_ControlValue.Value != val)
				{
					_ControlValue.Value = val;
				}
			}

			base.OnValueChanged();
		}

		public override int Maximum
		{
			get { return base.Maximum; }
			set
			{
				if (base.Maximum != value)
				{
					base.Maximum = value;
				}
				if (_ControlValue != null)
				{
					double val  = base.Maximum * _ControlValue.Precision;
					if (_ControlValue.Maximum != val)
					{
						_ControlValue.Maximum = val;
					}
				}
			}
		}

		public override int Minimum
		{
			get { return base.Minimum; }
			set
			{
				if (base.Minimum != value)
				{
					base.Minimum = value;
				}
				if (_ControlValue != null)
				{
					double val =  base.Minimum * _ControlValue.Precision;
					if (_ControlValue.Minimum != val)
					{
						_ControlValue.Minimum = val;
					}
				}
			}
		}



        public void MouseEventChange(MouseEventArgs e)
        {
            //this.OnMouseUp(e);
        }
    }
}
