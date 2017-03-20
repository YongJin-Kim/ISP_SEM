using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class SpotSizeTableSelector : SEC.GUIelement.DynamicList, INanoeyeValueControl
	{
		#region Proverty & Variables
		private SECtype.ITable _ControlValue = null;
		[DefaultValue(null)]
		public SECtype.IValue ControlValue
		{
			get { return _ControlValue; }
			set
			{
				if (value is SECtype.ITable)
				{
					base.Items.Clear();
					_ControlValue = value as SECtype.ITable;
					if (_ControlValue != null)
					{
						_ControlValue.SelectedIndexChanged += new EventHandler(_ControlValue_SelectedIndexChanged);
						_ControlValue.TableChanged += new EventHandler(_ControlValue_TableChanged);

						TableChanged();

						base.SelectedIndex = _ControlValue.SelectedIndex;
					}
				}
				else { throw new ArgumentException(); }
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override System.Collections.ArrayList Items
		{
			get { return System.Collections.ArrayList.ReadOnly(base.Items); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsLimitedMode
		{
			get { return false; }
			set { throw new NotSupportedException(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsValueOperation
		{
			get { return false; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		public SpotSizeTableSelector()
		{
			InitializeComponent();
		}

		void _ControlValue_TableChanged(object sender, EventArgs e)
		{
			TableChanged();
		}

		void _ControlValue_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (base.SelectedIndex != _ControlValue.SelectedIndex)
			{
				base.SelectedIndex = _ControlValue.SelectedIndex;
			}
		}

		private void TableChanged()
		{
			base.Items.Clear();

			object[,] tab = _ControlValue.TableGet();
			if (tab == null) { return; }
			string[] items = new string[tab.GetLength(0)];

			for (int i = 0; i < items.Length; i++)
			{
				items[i] = tab[i,0].ToString();
			}

			base.Items.AddRange(items);
		}

		protected override void OnSeletedIndexChanged()
		{
			base.OnSeletedIndexChanged();

			if (_ControlValue.SelectedIndex != base.SelectedIndex)
			{
				_ControlValue.SelectedIndex = base.SelectedIndex;
			}
		}
	}
}
