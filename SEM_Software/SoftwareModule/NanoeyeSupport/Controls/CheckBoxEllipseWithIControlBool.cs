using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class CheckBoxEllipseWithIControlBool : SEC.GUIelement.CheckBoxEllipse, INanoeyeValueControl
	{
		#region Property & Variables
		private bool changing = false;

       

		private SECtype.IControlBool _ControlValue;
		[DefaultValue(null)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (value == null)
				{
					if (_ControlValue != null)
					{
						_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
						_ControlValue = null;
					}
					return;
				}

				if (value is SECtype.IControlBool)
				{
					if (_ControlValue != null)
					{
						_ControlValue.ValueChanged -= new EventHandler(_ControlValue_ValueChanged);
					}

					_ControlValue = value as SECtype.IControlBool;
					if (_ControlValue != null)
					{
						_ControlValue.ValueChanged += new EventHandler(_ControlValue_ValueChanged);
						changing = true;
						this.Checked = _ControlValue.Value;
						changing = false;
					}
				}
				else
				{
					throw new ArgumentException("Argument is not IControlBool.");
				}
			}
		}

		/// <summary>
		/// Not Supproted
		/// </summary>
		[DefaultValue(true), Browsable(false)]
		public bool IsLimitedMode
		{
			get { return true; }
			set { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Not Supproted
		/// </summary>
		[DefaultValue(true), Browsable(false)]
		public bool IsValueOperation
		{
			get { return true; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		public CheckBoxEllipseWithIControlBool()
		{
			InitializeComponent();
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			if (this.Checked != _ControlValue.Value)
			{
				changing = true;
				this.Checked = _ControlValue.Value;
				changing = false;
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);


			if (!changing && (_ControlValue != null)) { _ControlValue.Value = this.Checked; }
		}
	}
}
