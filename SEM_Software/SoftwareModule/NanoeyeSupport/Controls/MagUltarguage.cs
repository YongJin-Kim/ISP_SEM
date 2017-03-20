using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SEC.Nanoeye.NanoView;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class MagUltarguage : UserControl
	{
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

		private SECtype.ITable _ControlTable = null;
		[Browsable( false )]
		[DefaultValue( null )]
		public SECtype.ITable ControlTable
		{
			get { return _ControlTable; }
			set
			{

				if(value == _ControlTable)
				{
					return;
				}
				if(_ControlTable != null)
				{
					_ControlTable.SelectedIndexChanged -= new EventHandler( _ControlTable_SelectedIndexChanged );
				}
				_ControlTable = value;
				if(_ControlTable != null)
				{
					if(_ControlTable.Length > 0)
					{
						if(_ControlTable.SelectedIndex > 0)
						{
							leftBe.Enabled = true;
						}
						if(_ControlTable.SelectedIndex < _ControlTable.Length - 1)
						{
							rightBe.Enabled = true;
						}
					}
					else
					{
						leftBe.Enabled = false;
						rightBe.Enabled = false;

					}
					_ControlTable.SelectedIndexChanged += new EventHandler( _ControlTable_SelectedIndexChanged );
				}
			}
		}

		public MagUltarguage()
		{
			InitializeComponent();

			SetStyle(ControlStyles.ContainerControl, true);
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);

			#region 리스트 버튼 좌우 배치
			//leftBe.Location = new Point(this.ClientRectangle.Left, (this.Height - _ButtonSize.Height) / 2);
			//rightBe.Location = new Point(this.ClientRectangle.Right - _ButtonSize.Width, (this.Height - _ButtonSize.Height) / 2);

			//magUg.Location = new Point(leftBe.Right + this.Margin.Horizontal, this.ClientRectangle.Y);
			//magUg.Size = new Size(this.ClientRectangle.Width - _ButtonSize.Width - _ButtonSize.Width - this.Margin.Horizontal - this.Margin.Horizontal, this.ClientRectangle.Height);
			#endregion

			#region 리스트 버튼 왼쪽 상하 배치
			rightBe.Location = new Point(rightBe.Width / 2, this.Height / 2 - rightBe.Height - this.Margin.Bottom);
			leftBe.Location = new Point(leftBe.Width / 2, this.Height / 2 + this.Margin.Top);

			display.Location = new Point(leftBe.Right + rightBe.Width / 2 + this.Margin.Horizontal, 0);
			display.Size = new Size(this.Width - rightBe.Width * 2 - this.Margin.Horizontal, this.Height);
			#endregion
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
					display.Text = "???";
					rightBe.Enabled = false;
					leftBe.Enabled = false;
				}
				return;
			}

			int mag = (int)_ControlTable.SeletedItem;

			mag = SEC.GenericSupport.Mathematics.NumberConverter.RegularPower(mag, 2, new int[] { 10, 13, 15, 20, 30, 40, 50, 70 });

			string val = "x" + SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(mag, 0, 3, false, (char)0);

			Action<string> displayChange =(x) => { display.Text = x; };
			display.BeginInvoke(displayChange, new object[] { val });

			bool right = true;
			bool left = true;

			if (ControlTable.SelectedIndex == ControlTable.Length - 1) { right = false; }

			if (ControlTable.SelectedIndex <= 0) { left = false; }

			Action<Control, bool> enableCh = (con, en) => { con.Enabled = en; };

			rightBe.BeginInvoke(enableCh, new object[] { rightBe, right });
			leftBe.BeginInvoke(enableCh, new object[] { leftBe, left });
		}

		private void leftBe_Click(object sender, EventArgs e)
		{
			ControlTable.SelectedIndex--;
		}

		private void rightBe_Click(object sender, EventArgs e)
		{
			ControlTable.SelectedIndex++;
		}
	}
}
