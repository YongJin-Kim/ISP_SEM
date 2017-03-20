using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEC.Nanoeye.NanoView;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Scan
{
	internal class Magnification_Nanoeye001 : ColumnInt
	{
		ColumnDouble magnificationX;
		ColumnDouble magnificationY;
		ColumnInt feedBack;

		private struct MagStruct
		{
			//public int mag;
			public double ratiox;
			public double ratioy;
			public int fm;
			public MagStruct(/*int mag, */double ratiox, double ratioy, int fm)
			{
				//this.mag = mag;
				this.ratiox = ratiox;
				this.ratioy = ratioy;
				this.fm = fm;
			}
		}

		private SECtype.IController _column = null;
		public SECtype.IController Column
		{
			get { return _column; }
			set
			{
				_column = value;
				magnificationX = (ColumnDouble)_column["ScanMagnificationX"];
				magnificationY = (ColumnDouble)_column["ScanMagnificationY"];
				feedBack = (ColumnInt)_column["ScanFeedbackMode"];
			}
		}

		//private List<MagStruct> magTable = new List<MagStruct>();
		private SortedDictionary<int, MagStruct> magTable = new SortedDictionary<int, MagStruct>();

		public void TableSet(object[,] table)
		{
			if ( table.GetLength(1) != 4 ) {
				throw new ArgumentException("Table must consist 4 columns.");
			}

			magTable.Clear();

			int len = table.GetLength(0);

			for ( int i = 0 ; i < len ; i++ ) {
				//magTable.Add(new MagStruct((int)(table[i, 0]), (double)(table[i, 1]), (double)(table[i, 2]), (int)(table[i, 3])));
				magTable.Add((int)table[i, 0], new MagStruct((double)(table[i, 1]), (double)(table[i, 2]), (int)(table[i, 3])));
			}

			TableInfoUpdate();
		}

		public object[,] TableGet()
		{
			object[,] result = new object[magTable.Count, 4];

			int i = 0;
			foreach ( KeyValuePair<int, MagStruct>  ms in magTable ) {
				result[i, 0] = ms.Key;
				result[i, 1] = ms.Value.ratiox;
				result[i, 2] = ms.Value.ratioy;
				result[i, 3] = ms.Value.fm;
				i++;
			}

			return result;
		}

		public void TableChagne(int mag, object[] value)
		{
			if ( value == null ) {
				magTable.Remove(mag);
			}
			else {
				if ( mag != (int)(value[0]) ) {
					throw new ArgumentException("mag and value[0] must be same.");
				}

				if ( magTable.ContainsKey(mag) ) {
					magTable[mag] = new MagStruct((double)(value[1]), (double)(value[2]), (int)(value[3]));
				}
				else {
					magTable.Add(mag, new MagStruct((double)(value[1]), (double)(value[2]), (int)(value[3])));
				}
			}
			TableInfoUpdate();
		}

		private void TableInfoUpdate()
		{
			KeyValuePair<int, MagStruct>  msFirst = magTable.First();
			KeyValuePair<int, MagStruct>  msLast = magTable.Last();

			this._Minimum = msFirst.Key;
			this._Maximum = msLast.Key;
		}

		public override int Value
		{
			get { return _Value; }
			set
			{
				if ( !magTable.ContainsKey(value) ) {
					throw new ArgumentException("Undefined magnification");
				}

				_Value = value;

				MagStruct ms = magTable[_Value];

				magnificationX.Value = ms.ratiox;
				magnificationY.Value = ms.ratioy;
				feedBack.Value = ms.fm;

				OnValueChanged();
			}
		}
	}
}
