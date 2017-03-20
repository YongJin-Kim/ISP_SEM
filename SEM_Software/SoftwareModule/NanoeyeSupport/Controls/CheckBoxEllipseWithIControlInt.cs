using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class CheckBoxEllipseWithIControlInt : SEC.GUIelement.CheckBoxEllipse, INanoeyeValueControl
	{
		#region Property & Variables
		private bool changing = false;

		private SECtype.IControlInt _ControlValue;
		[DefaultValue(null)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if(value == null)
				{
					if(_ControlValue != null)
					{
						_ControlValue.ValueChanged -= new EventHandler( _ControlValue_ValueChanged );
						_ControlValue = null;
					}
					return;
				}

				if(value is SECtype.IControlInt)
				{
					if(_ControlValue != null)
					{
						_ControlValue.ValueChanged -= new EventHandler( _ControlValue_ValueChanged );
					}

					_ControlValue = value as SECtype.IControlInt;
					if(_ControlValue != null)
					{
						_ControlValue.ValueChanged += new EventHandler( _ControlValue_ValueChanged );
						changing = true;
						this.Checked = (_ControlValue.Value != 0);
						changing = false;
					}
				}
				else
				{
					throw new ArgumentException( "Argument is not IControlBool." );
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
		[DefaultValue( true ), Browsable( false )]
		public bool IsValueOperation
		{
			get { return true; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		public CheckBoxEllipseWithIControlInt()
		{
			InitializeComponent();
		}

		void _ControlValue_ValueChanged(object sender, EventArgs e)
		{
			if (this.Checked != (_ControlValue.Value != 0))
			{
				changing = true;
				this.Checked = (_ControlValue.Value != 0);
				changing = false;
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged( e );

			if (!changing) { _ControlValue.Value = (this.Checked ? 1 : 0); }
		}
	}
}
