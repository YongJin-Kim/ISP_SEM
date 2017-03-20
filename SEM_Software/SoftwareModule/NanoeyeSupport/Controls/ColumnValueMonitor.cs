using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECcvt = SEC.Nanoeye.NanoColumn.IColumnValue;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class ColumnValueMonitor : UserControl
	{
		private bool repeatReadLinked = false;
		Label lowerLab;
		Label upperLab;

		private SEC.Nanoeye.NanoColumn.ISEMController _Controller;
		public SEC.Nanoeye.NanoColumn.ISEMController Controller
		{
			get{return _Controller;}
			set
			{
				if (_ColumnValue is SECcvt)
				{
					if (repeatReadLinked)
					{
						SECcvt cvt = _ColumnValue as SECcvt;
						cvt.RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(_ColumnValue_RepeatUpdated);
						repeatReadLinked = false;
					}
				}
				_Controller = value;

				comboBox1.Items.Clear();
				comboBox1.Items.Add("Non");
				foreach (SECtype.IValue icv in _Controller)
				{
					comboBox1.Items.Add(icv.Name);
				}
			}
		}

		private SECtype.IValue _ColumnValue = null;
		public SECtype.IValue ColumnValue
		{
			get { return _ColumnValue; }
			set
			{
				flowLayoutPanel1.Controls.Clear();

				if (repeatReadLinked)
				{
					SECcvt cvt = _ColumnValue as SECcvt;
					cvt.RepeatUpdated -= new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(_ColumnValue_RepeatUpdated);
					repeatReadLinked = false;
				}

				if (value == null)
				{
					_ColumnValue = null;
					OnValueDeselected();
					return;
				}

				if (value is SECtype.IControlDouble)
				{
					_ColumnValue = value;

					checkBoxWithIControlValueEnable1.ControlValue = _ColumnValue;

					NumericUpDownIControlDouble nudi = new NumericUpDownIControlDouble();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlDouble.DisplayTypeEnum.DefaultMaximum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlDouble();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlDouble.DisplayTypeEnum.Maximum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					HswdCvd	hsd = new HswdCvd();
					hsd.ControlValue = _ColumnValue;
					hsd.NameLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
					hsd.NameSize = new System.Drawing.Size(60, 21);
					hsd.Size = new System.Drawing.Size(354, 24);
					hsd.ValueDisplaySize = new System.Drawing.Size(60, 21);
					hsd.ValueLocation = SEC.GUIelement.HscrollWithDisplay.LabelLocation.Bottom;
					flowLayoutPanel1.Controls.Add(hsd);

					nudi = new NumericUpDownIControlDouble();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlDouble.DisplayTypeEnum.Offset;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlDouble();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlDouble.DisplayTypeEnum.Minimum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlDouble();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlDouble.DisplayTypeEnum.DefaultMinimum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					if (_ColumnValue is SECcvt)
					{
						SECcvt cvt = _ColumnValue as SECcvt;
						try
						{
							cvt.RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(_ColumnValue_RepeatUpdated);
							repeatReadLinked = true;
							lowerLab = new Label();
							lowerLab.Size = new Size(80, 24);
							lowerLab.TextAlign = ContentAlignment.MiddleCenter;
							lowerLab.BorderStyle = BorderStyle.Fixed3D;
							flowLayoutPanel1.Controls.Add(lowerLab);

							upperLab = new Label();
							upperLab.Size = new Size(80, 24);
							upperLab.TextAlign = ContentAlignment.MiddleCenter;
							upperLab.BorderStyle = BorderStyle.Fixed3D;
							flowLayoutPanel1.Controls.Add(upperLab);
						}
						catch
						{
							repeatReadLinked = false;
							lowerLab = null;
							upperLab = null;
						}
					}
				}
				else if (value is SECtype.IControlInt)
				{
					_ColumnValue = value;

					checkBoxWithIControlValueEnable1.ControlValue = _ColumnValue;

					NumericUpDownIControlInt nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.DefaultMaximum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.Maximum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.Value;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.Offset;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.Minimum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					nudi = new NumericUpDownIControlInt();
					nudi.ControlValue = _ColumnValue;
					nudi.DisplayType = NumericUpDownIControlInt.DisplayTypeEnum.DefaultMinimum;
					nudi.Size = new Size(80, 21);
					flowLayoutPanel1.Controls.Add(nudi);

					if (_ColumnValue is SECcvt)
					{
						SECcvt cvt = _ColumnValue as SECcvt;
						try
						{
							cvt.RepeatUpdated += new SEC.Nanoeye.NanoColumn.ObjectArrayEventHandler(_ColumnValue_RepeatUpdated);
							repeatReadLinked = true;
							lowerLab = new Label();
							lowerLab.Size = new Size(80, 24);
							lowerLab.TextAlign = ContentAlignment.MiddleCenter;
							lowerLab.BorderStyle = BorderStyle.Fixed3D;
							flowLayoutPanel1.Controls.Add(lowerLab);

							upperLab = new Label();
							upperLab.Size = new Size(80, 24);
							upperLab.TextAlign = ContentAlignment.MiddleCenter;
							upperLab.BorderStyle = BorderStyle.Fixed3D;
							flowLayoutPanel1.Controls.Add(upperLab);
						}
						catch
						{
							repeatReadLinked = false;
							lowerLab = null;
							upperLab = null;
						}
					}
				}
				else if (value is SECtype.IControlBool)
				{
					_ColumnValue = value;

					checkBoxWithIControlValueEnable1.ControlValue = _ColumnValue;

					CheckBoxWithIControlBool cbib = new CheckBoxWithIControlBool();
					cbib.ControlValue = (SECtype.IControlBool)(_ColumnValue);
					cbib.Size = new Size(80, 24);
					cbib.Text = "True";
					flowLayoutPanel1.Controls.Add(cbib);
				}
				else
				{
					_ColumnValue = null;
					OnValueDeselected();
					return;
				}

				OnValueSelected();
			}
		}

		void _ColumnValue_RepeatUpdated(object sender, object[] value)
		{
			Action act = () =>
			{
				upperLab.Text = ((double)value[0]).ToString();
				lowerLab.Text = ((double)value[1]).ToString();
			};
			this.BeginInvoke(act);
		}

		public event EventHandler ValueSelected;
		protected virtual void OnValueSelected()
		{
			if (ValueSelected != null)
			{
				ValueSelected(this, EventArgs.Empty);
			}
		}

		public event EventHandler ValueDeselected;
		protected virtual void OnValueDeselected()
		{
			if (ValueDeselected != null)
			{
				ValueDeselected(this, EventArgs.Empty);
			}
		}

		public ColumnValueMonitor()
		{
			InitializeComponent();
		}

		private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem == null)
			{
				ColumnValue = null;
				return;
			}

			if (comboBox1.SelectedItem is string)
			{
				string str = comboBox1.SelectedItem as string;
				if (str == "Non")
				{
					ColumnValue = null;
					return;
				}
				else
				{
					ColumnValue = _Controller[str];
					return;
				}
			}
			else
			{
				System.Diagnostics.Trace.Fail("Undefined.");
			}
		}
	}
}
