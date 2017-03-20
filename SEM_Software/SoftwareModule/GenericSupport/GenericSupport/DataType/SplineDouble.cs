using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public class SplineDouble : ControlDouble, ISplineDouble
	{
		#region Property & Variables
		bool syncFlag = false;

		protected object[,] tableOriginal;
		protected SortedList<double,double> tableToReal = new SortedList<double, double>();
		protected SortedList<double,double> tableFromReal = new SortedList<double, double>();

		protected IControlDouble _RealControl = null;
		public IControlDouble RealControl
		{
			get { return _RealControl; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This is already inited."); }

				if (_RealControl != null) { _RealControl.ValueChanged -= new EventHandler(RealControl_ValueChanged); }
				_RealControl = value;
				if (_RealControl != null) { _RealControl.ValueChanged += new EventHandler(RealControl_ValueChanged); }
			}
		}

		protected bool _TableError = false;
		public bool TableError
		{
			get { return _TableError; }
			protected set
			{
				if (_TableError != value)
				{
					_TableError = value;
					OnTableErrorChanged();
				}
			}
		}

		public override double Value
		{
			get { return base.Value; }
			set
			{
				base.Value = value;

				SetReal();
			}
		}
		#endregion

		#region Event
		public event EventHandler  TableErrorChanged;
		protected virtual void OnTableErrorChanged()
		{
			if (TableErrorChanged != null) { TableErrorChanged(this, EventArgs.Empty); }
		}

		public event EventHandler  TableChanged;
		protected virtual void OnTableChanged()
		{
			if (TableChanged != null) { TableChanged(this, EventArgs.Empty); }
		}
		#endregion

		public override void Dispose()
		{
			_IsInited = false;
			RealControl = null;

			base.Dispose();
		}

		void RealControl_ValueChanged(object sender, EventArgs e)
		{
			if (Enable && IsInitied) { SetThis(); }
		}

		#region Table Management
		public void TableSet(object[,] values)
		{
			tableOriginal = values;

			MakeInnerTable();
		}

		public object[,] TableGet()
		{
			return tableOriginal;
		}

		public void TableAppend(object[] values)
		{
			List<double> keyList = new List<double>();

			int column = values.Length;
			int row;
			if (tableOriginal != null) { row = tableOriginal.GetLength(0); }
			else { row = 0; }

			object[,] tableNew = new object[row + 1, column];

			for (int i = 0; i < row; i++)
			{
				keyList.Add((double)tableOriginal[i, 0]);

				for (int j = 0; j < column; j++)
				{
					tableNew[i, j] = tableOriginal[i, j];
				}
			}

			if (keyList.Contains((double)values[0])) { throw new ArgumentException("Same key exist."); }

			for (int j = 0; j < column; j++)
			{
				tableNew[row, j] = values[j];
			}

			tableOriginal = tableNew;
			MakeInnerTable();
		}

		public void TableRemove(object key)
		{
			int column = tableOriginal.GetLength(1);
			int row = tableOriginal.GetLength(0);
			int sub = 0;

			object[,] tableNew = new object[row - 1, column];

			for (int i = 0; i < row; i++)
			{
				if ((double)tableOriginal[i, 0] == (double)key)
				{
					sub++;
					continue;
				}

				for (int j = 0; j < column; j++)
				{
					tableNew[i - sub, j] = tableOriginal[i, j];
				}
			}

			tableOriginal = tableNew;
			MakeInnerTable();
		}

		public void TableChange(object preKey, object[] values)
		{
			List<double> keyList = new List<double>();

			int column = tableOriginal.GetLength(1);
			int row = tableOriginal.GetLength(0);

			object[,] tableNew = new object[row, column];

			for (int i = 0; i < row; i++)
			{
				if ((double)tableOriginal[i, 0] == (int)values[0])
				{
					if (keyList.Contains((double)values[0])) { throw new ArgumentException("Same key exist."); }
					keyList.Add((double)values[0]);
					for (int j = 0; j < column; j++)
					{
						tableNew[i, j] = values[j];
					}
				}
				else
				{
					if (keyList.Contains((double)tableOriginal[i, 0])) { throw new ArgumentException("Same key exist."); }
					keyList.Add((double)tableOriginal[i, 0]);
					for (int j = 0; j < column; j++)
					{
						tableNew[i, j] = tableOriginal[i, j];
					}
				}
			}

			tableOriginal = tableNew;
			MakeInnerTable();
		}

		private void MakeInnerTable()
		{
			SortedList<double,double> tReal = new SortedList<double, double>();
			SortedList<double,double> fReal = new SortedList<double, double>();

			int row = tableOriginal.GetLength(0);
			for (int i = 0; i < row; i++)
			{
				tReal.Add((double)tableOriginal[i,0],(double)tableOriginal[i,1]);
				fReal.Add((double)tableOriginal[i,1],(double)tableOriginal[i,0]);
			}

			tableToReal = tReal;
			tableFromReal = fReal;

			OnTableChanged();

			SetReal();
		}
		#endregion

		#region 동기화
		public override void Sync()
		{
			SetThis();
		}

		public override bool Validate()
		{
			double val;
			CalculrateThis(out val);

			return (val == this.Value);
		}
		#endregion

		#region 값 계산
		private void SetReal()
		{
			if (_Enable && _IsInited)
			{
				if (syncFlag) { return; }

				syncFlag = true;

				double real;
				CalculrateReal(out real);
				_RealControl.Value = real;

				syncFlag = false;
			}
		}

		private void SetThis()
		{
			if (_Enable && _IsInited)
			{
				if (syncFlag) { return; }

				syncFlag = true;

				double val;
				CalculrateThis(out val);
				this.Value = val;

				syncFlag = false;
			}
		}

		private void CalculrateReal(out double real)
		{
			try
			{
				real = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableToReal, _Value);
				TableError = false;
			}
			catch (Exception)
			{
				TableError = true;
				real = _RealControl.Value;
			}
		}

		private void CalculrateThis(out double val)
		{
			try
			{
				val = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableFromReal, _RealControl.Value);
				TableError = false;
			}
			catch (Exception)
			{
				TableError = true;
				val = _Value;
			}
		}
		#endregion
	}
}
