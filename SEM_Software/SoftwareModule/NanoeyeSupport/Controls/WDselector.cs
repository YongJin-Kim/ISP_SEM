using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class WDselector : UserControl
	{
		#region Property
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public SEC.GUIelement.ButtonEllipse ButtonLeft
		{
			get { return leftBe; }
		}

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public SEC.GUIelement.ButtonEllipse ButtonRight
		{
			get { return rightBe; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get { return label1.ForeColor; }
			set { label1.ForeColor = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color LableColor
		{
			get { return label1.BackColor; }
			set { label1.BackColor = value; }
		}

		[RefreshProperties(RefreshProperties.All),
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				if (DesignMode) { return -1; }

				if(_ControlTable != null)
				{
					return _ControlTable.SelectedIndex;
				}
				else
				{
					return -1;
				}
			}
			set
			{
				if (DesignMode) { return; }

				if (_ControlTable != null)
				{
					try
					{
						_ControlTable.SelectedIndex = value;
					}
					catch { }
				}
			}
		}

		[DefaultValue(null),
		RefreshProperties(RefreshProperties.All),
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				if (DesignMode) { return null; }

				if (_ControlTable != null) { return _ControlTable.SeletedItem; }
				else { return null; }

			}
			set
			{
				if (!DesignMode)
				{
					if (_ControlTable != null) { _ControlTable.SeletedItem = value; }
				}

			}
		}

		private SECtype.ITable _ControlTable = null;
		[Browsable(false),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SECtype.ITable ControlTable
		{
			get
			{
				if (DesignMode) { return null; }
				return _ControlTable;
			}
			set
			{
				if (!DesignMode)
				{
					if (value == _ControlTable)
					{
						return;
					}
					if (_ControlTable != null)
					{
						_ControlTable.SelectedIndexChanged -= new EventHandler(_ControlTable_SelectedIndexChanged);
					}
					_ControlTable = value;
					if (_ControlTable != null)
					{
						_ControlTable.SelectedIndexChanged += new EventHandler(_ControlTable_SelectedIndexChanged);

						_ControlTable.SelectedIndex = _ControlTable.SelectedIndex;
					}
				}
			}
		}
		#endregion

		public WDselector()
		{
			InitializeComponent();
		}

		void _ControlTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_ControlTable.SelectedIndex < 0)
			{
				if (_ControlTable.Length > 0)
				{
					_ControlTable.SelectedIndex = 0;
				}

				else
				{
					rightBe.Enabled = false;
					leftBe.Enabled = false;
					label1.Text = "???";
				}

				return;
			}

			string val;// = SEC.MathematicsSupport.NumberConverter.ToUnitString((double)_ControlTable.SeletedItem * 0.001, 3, false);
			val = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString((int)_ControlTable.SeletedItem, -3, 3, false, 'm');


			Action act = () => { label1.Text = val; };
			if (label1.InvokeRequired) { label1.BeginInvoke(act); }
			else { act(); }


			//this.Invalidate();

			bool right = true;
			bool left = true;

			if (ControlTable.SelectedIndex == ControlTable.Length - 1)
			{
				right = false;
			}

			if (ControlTable.SelectedIndex <= 0)
			{
				left = false;
			}

			Action act2 = () =>
			{
				rightBe.Enabled = right;
				leftBe.Enabled = left;
			};
			if (rightBe.InvokeRequired) { rightBe.BeginInvoke(act2); }
			else { act2(); }
		}

		private void leftBe_Click(object sender, EventArgs e)
		{
			if (_ControlTable != null)
			{
				ControlTable.SelectedIndex--;
			}
		}

		private void rightBe_Click(object sender, EventArgs e)
		{
			if (_ControlTable != null)
			{
				ControlTable.SelectedIndex++;
			}
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);

			#region 리스트 버튼 좌우 배치
			//leftBe.Location = new Point(this.Padding.Left, (this.ClientSize.Height - _ButtonSize.Height) / 2);
			//rightBe.Location = new Point(this.ClientRectangle.Width - this.Padding.Right - rightBe.Width, (this.ClientSize.Height - _ButtonSize.Height) / 2);

			//label1.Size = new Size(rightBe.Left - leftBe.Right - Padding.Horizontal - Padding.Horizontal, this.ClientRectangle.Height - Padding.Vertical);
			//label1.Location = new Point(leftBe.Right + Padding.Horizontal, Padding.Top);
			#endregion

			#region 리스트 버튼 왼쪽 상하 배치

			rightBe.Size = new Size(24, 24);
			leftBe.Size = new Size(24, 24);
			rightBe.Location = new Point(rightBe.Width / 2, this.Height / 2 - rightBe.Height - this.Margin.Bottom);
			leftBe.Location = new Point(leftBe.Width / 2, this.Height / 2 + this.Margin.Top);

			label1.Location = new Point(leftBe.Right + this.Margin.Horizontal + rightBe.Width / 2, this.Height / 4);
			label1.Size = new Size(this.Width - rightBe.Width * 2 - this.Margin.Horizontal, this.Height / 2);
			#endregion

		}

		private void leftBe_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (_ControlTable != null)
			{
				ControlTable.SelectedIndex--;
			}
		}

		private void rightBe_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (_ControlTable != null)
			{
				ControlTable.SelectedIndex++;
			}
		}
	}
}
