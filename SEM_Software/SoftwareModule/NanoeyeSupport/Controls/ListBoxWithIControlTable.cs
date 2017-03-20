using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	/// <summary>
	/// 구현 완료 되지 않음.
	/// </summary>
	public partial class ListBoxWithIControlTable : System.Windows.Forms.ListBox
	{
		private SECtype.ITable _ControlTable;
		public SECtype.ITable ControlTable
		{
			get { return _ControlTable; }
			set
			{
				if(_ControlTable != null)
				{
					_ControlTable.SelectedIndexChanged -= new EventHandler( _ControlTable_SelectedIndexChanged );
					_ControlTable.TableChanged -= new EventHandler( _ControlTable_TableChanged );
				}
				_ControlTable = value;

				if(_ControlTable != null)
				{
					_ControlTable.SelectedIndexChanged += new EventHandler( _ControlTable_SelectedIndexChanged );
					_ControlTable.TableChanged += new EventHandler( _ControlTable_TableChanged );
					ResetTable();
				}
				else
				{
					this.Items.Clear();
				}
			}
		}

		public ListBoxWithIControlTable()
		{
			InitializeComponent();
		}

		private void ResetTable()
		{
			this.Items.Clear();

			object[,] table =  _ControlTable.TableGet();

			int rowCnt = table.GetLength( 0 );
			int colCnt = table.GetLength( 1 );
			string[] str = new string[rowCnt];
			for(int i = 0; i < rowCnt; i++)
			{
				string tmp = "";
				for(int j = 0; j < colCnt; j++)
				{
					tmp += table[i, j].ToString();
				}
				str[i] = tmp;
			}
			this.Items.AddRange( str );
		}

		void _ControlTable_TableChanged(object sender, EventArgs e)
		{
			ResetTable();
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged( e );

			_ControlTable.SelectedIndex = this.SelectedIndex;
		}

		void _ControlTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SelectedIndex = _ControlTable.SelectedIndex;
		}

		protected override void OnValueMemberChanged(EventArgs e)
		{
			
			base.OnValueMemberChanged( e );
		}

	}
}
