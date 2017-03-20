using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Scan
{
	internal class MagTableBase : SECtype.TableBase
	{
		#region Property & Vairables
		private bool withWD = false;
		private SortedList<int,object[]> magtable = new SortedList<int, object[]>();
		//private bool initing = false;

		private ColumnDouble _MagXCvd = null;
		internal ColumnDouble MagXCvd
		{
			get { return _MagXCvd; }
			set { _MagXCvd = value; }
		}

		private ColumnDouble _MagYCvd = null;
		internal ColumnDouble MagYCvd
		{
			get { return _MagYCvd; }
			set { _MagYCvd = value; }
		}

		private ColumnInt _FeedbackCvi = null;
		internal ColumnInt FeedbackCvi
		{
			get { return _FeedbackCvi; }
			set { _FeedbackCvi = value; }
		}

		private int _SelectedIndex = -1;
		public override int SelectedIndex
		{
			get { return _SelectedIndex; }
			set
			{
				if (value < 0)
				{
					_SelectedIndex = -1;
					OnSelectedIndexChanged();
					return;
				}

				_SelectedIndex = value;

				object[] val = magtable.Values[value];
				_MagXCvd.Value = (double)val[0];
				_MagYCvd.Value = (double)val[1];
				_FeedbackCvi.Value = (int)val[2];

				OnSelectedIndexChanged();
			}
		}

		/// <summary>
		/// 적용 중인 배율.
		/// </summary>
		public override object SeletedItem
		{
			get
			{
				if (_SelectedIndex == -1) { return -1; }

				return magtable.Keys[_SelectedIndex];
			}
			set
			{
				SelectedIndex = (magtable.IndexOfKey((int)value));
			}
		}

		public override int Length
		{
			get { return magtable.Count; }
		}

		public override object[] this[int index]
		{
			get { return magtable.Values[index]; }
			set
			{
				if (value is object[])
				{
					object[] objs = value as object[];
					if (objs.Length != 3) { throw new ArgumentException(); }

					magtable.Values[index] = objs;
					OnTableChanged();
					if (index == _SelectedIndex) { OnSelectedIndexChanged(); }
				}
				else
				{
					throw new ArgumentException();
				}
			}
		}
		#endregion

		void _WDtable_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (withWD)
			{
				// 단순히 배율이 바뀌었음을 알려 주기만 하면 됨.
				OnSelectedIndexChanged();
			}
		}

		public override void TableSet(object[,] values)
		{
			if (values == null)
			{
				magtable.Clear();
				SelectedIndex = -1;
				return;
			}

			if (values.GetLength(1) != 4)
			{
				throw new ArgumentException("column length must be 4");
			}

			magtable.Clear();

			for (int i = 0; i < values.GetLength(0); i++)
			{
				magtable.Add((int)(values[i, 0]), new object[] { values[i, 1], values[i, 2], values[i, 3] });
			}

			// for riging event
			OnTableChanged();

			SelectedIndex = SelectedIndex;

		}

		public override object[,] TableGet()
		{
			if (magtable.Count == 0) { return null; }

			object[,] obj = new object[magtable.Count, 4];

			for (int i=0; i < magtable.Count; i++)
			{
				obj[i, 0] = magtable.Keys[i];
				obj[i, 1] = (magtable.Values[i])[0];
				obj[i, 2] = (magtable.Values[i])[1];
				obj[i, 3] = (magtable.Values[i])[2];
			}

			return obj;
		}

		public override void TableChange(object preKey, object[] values)
		{
			Trace.Assert(values != null);
			Trace.Assert(values.Length == 4);

			bool rigingEvent = false;

			if (SeletedItem != null)
			{
				if ((int)values[0] == (int)SeletedItem)
				{
					rigingEvent = true;
				}
			}

			magtable.Remove((int)preKey);

			if (magtable.ContainsKey((int)values[0]))
			{
				magtable[(int)values[0]] = new object[] { values[1], values[2], values[3] };
			}
			else
			{
				magtable.Add((int)values[0], new object[] { values[1], values[2], values[3] });
			}

			if (rigingEvent)
			{
				// for riging event
				OnTableChanged();

				SelectedIndex = SelectedIndex;
			}
		}

		public override void TableAppend(object[] values)
		{
			Trace.Assert(values != null);
			Trace.Assert(values.Length == 4);


			magtable.Add((int)values[0], new object[] { values[1], values[2], values[3], });

			OnTableChanged();
		}

		public override void TableRemove(object item)
		{
			if (!magtable.ContainsKey((int)item))
			{
				throw new ArgumentException();
			}

			OnTableChanged();

			magtable.Remove((int)item);
		}

		public override void SetStyle(int index,ref object value)
		{

			throw new NotSupportedException();

		}

		public override void Sync()
		{
			throw new NotSupportedException();
		}

		public override bool Validate()
		{
			throw new NotSupportedException();
		}

		public override void BeginInit()
		{
			//initing = true;
		}

		public override void EndInit()
		{
			EndInit(false);
		}

		public override void EndInit(bool sync)
		{

		}

		public override int IndexMinimum
		{
			get { throw new NotImplementedException(); }
		}

		public override int IndexMaximum
		{
			get { throw new NotImplementedException(); }
		}
	}
}
