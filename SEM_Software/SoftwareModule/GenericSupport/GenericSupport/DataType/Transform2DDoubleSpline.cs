using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.GenericSupport.DataType
{
	public class Transform2DDoubleSpline : Transfrom2DDouble, ITransfrom2DDoubleSpline
	{
		// Table 구조
		// int : key - 삭제나 수정시 타겟 결정 용.
		// int : axis - 0이면 수평 축. 1이면 수직 축
		// double : rotation value - 회전 축의 값
		// double : real value - 실제 축의 값

		#region Property & Variables
		protected object[,] tableOriginal = null;
		protected SortedList<double, double> tableHorizontalToReal = null;
		protected SortedList<double, double> tableHorizontalFromReal = null;
		protected SortedList<double, double> tableVerticalToReal = null;
		protected SortedList<double, double> tableVerticalFromReal = null;

		protected bool _IsHorizontalTableError = false;
		public bool IsHorizontalTableError
		{
			get { return _IsHorizontalTableError; }
			protected set
			{
				if (_IsHorizontalTableError != value)
				{
					_IsHorizontalTableError = value;
					OnTableErrorStateChanged();
				}
			}
		}

		protected bool _IsVerticalTableError = false;
		public bool IsVerticalTableError
		{
			get { return _IsVerticalTableError; }
			protected set
			{
				if (_IsVerticalTableError != value)
				{
					_IsVerticalTableError = value;
					OnTableErrorStateChanged();
				}
			}
		}

		#endregion

		#region Event
		public event EventHandler  TableChanged;

		protected virtual void OnTableChanged()
		{
			if (TableChanged != null) { TableChanged(this, EventArgs.Empty); }
		}

		public event EventHandler  TableErrorStateChanged;

		protected virtual void OnTableErrorStateChanged()
		{
			if (TableErrorStateChanged != null) { TableErrorStateChanged(this, EventArgs.Empty); }
		}
		#endregion

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
			List<int> keyList = new List<int>();

			int column = tableOriginal.GetLength(1);
			int row = tableOriginal.GetLength(0);

			object[,] tableNew = new object[row, column];

			for (int i = 0; i < row - 1; i++)
			{
				keyList.Add((int)tableOriginal[i, 0]);

				for (int j = 0; j < column; j++)
				{
					tableNew[i, j] = tableOriginal[i, j];
				}
			}

			if (keyList.Contains((int)values[0])) { throw new ArgumentException("Same key exist."); }

			for (int j = 0; j < column; j++)
			{
				tableNew[row - 1, j] = values[j];
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
				if ((int)tableOriginal[i, 0] == (int)key)
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
			List<int> keyList = new List<int>();

			int column = tableOriginal.GetLength(1);
			int row = tableOriginal.GetLength(0);

			object[,] tableNew = new object[row, column];

			for (int i = 0; i < row; i++)
			{
				if ((int)tableOriginal[i, 0] == (int)values[0])
				{
					if (keyList.Contains((int)values[0])) { throw new ArgumentException("Same key exist."); }
					keyList.Add((int)values[0]);
					for (int j = 0; j < column; j++)
					{
						tableNew[i, j] = values[j];
					}
				}
				else
				{
					if (keyList.Contains((int)tableOriginal[i, 0])) { throw new ArgumentException("Same key exist."); }
					keyList.Add((int)tableOriginal[i, 0]);
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
			tableHorizontalToReal = new SortedList<double, double>();
			tableHorizontalFromReal = new SortedList<double, double>();
			tableVerticalToReal = new SortedList<double, double>();
			tableVerticalFromReal = new SortedList<double, double>();

			int column = tableOriginal.GetLength(1);
			int row = tableOriginal.GetLength(0);

			for (int i = 0; i < row; i++)
			{
				if ((int)tableOriginal[i, 1] == 1)
				{
					tableHorizontalToReal.Add((double)tableOriginal[i, 2], (double)tableOriginal[i, 2]);
					tableHorizontalFromReal.Add((double)tableOriginal[i, 3], (double)tableOriginal[i, 2]);
				}
				else
				{
					tableVerticalToReal.Add((double)tableOriginal[i, 2], (double)tableOriginal[i, 2]);
					tableVerticalFromReal.Add((double)tableOriginal[i, 3], (double)tableOriginal[i, 2]);
				}
			}

			OnTableChanged();

			double realX, realY;
			CalculateReal(out realX, out realY);
			_HorizontalReal.Value = realX;
			_VerticalReal.Value = realY;
		}

		#endregion

		#region Spline 계산
		// Rotated 값은 Spline 계산을 위한 값이 들어 있음.
		// 즉 Rotated의 값을 spline으로 변환 하여 Real에 넣으면 됨.

		protected override void CalculateReal(out double realX, out double realY)
		{
			double tempX, tempY;

			try
			{
				tempX = Mathematics.Interpolation.Spline(tableHorizontalToReal, _HorizontalRotated.Value);
				IsHorizontalTableError = false;
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				tempX = 0;
				IsHorizontalTableError = true;
			}

			try
			{
				tempY = Mathematics.Interpolation.Spline(tableVerticalToReal, _VerticalRotated.Value);
				IsVerticalTableError = false;
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				tempY = 0;
				IsVerticalTableError = true;
			}

			double tX = tempX * Math.Cos(Angle.Value / 180 * Math.PI) - tempY * Math.Sin(Angle.Value / 180 * Math.PI);
			double tY = tempX * Math.Sin(Angle.Value / 180 * Math.PI) + tempY * Math.Cos(Angle.Value / 180 * Math.PI);

			double rX = tX * _PrecisionHorizontal;
			double rY = tY * _PrecisionVertical;

			if (_ReverseHorizontal) { rX *= -1; }
			if (_ReverseVertical) { rY *= -1; }

			realX = rX;
			realY = rY;
		}

		protected override void CalculateRotated(out double rotatedX, out double rotatedY)
		{
			double tempX = _HorizontalReal.Value / _PrecisionHorizontal;
			double tempY = _VerticalReal.Value / _PrecisionVertical;

			if (_ReverseHorizontal) { tempX *= -1; }
			if (_ReverseVertical) { tempY *= -1; }

			double tX = tempX * Math.Cos(Angle.Value / 180 * Math.PI) + tempY * Math.Sin(Angle.Value / 180 * Math.PI);
			double tY = -1 * tempX * Math.Sin(Angle.Value / 180 * Math.PI) + tempY * Math.Cos(Angle.Value / 180 * Math.PI);

			try
			{
				rotatedX = Mathematics.Interpolation.Spline(tableHorizontalFromReal, tX);
				IsHorizontalTableError = false;
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				rotatedX = 0;
				IsHorizontalTableError = true;
			}

			try
			{
				rotatedY = Mathematics.Interpolation.Spline(tableVerticalFromReal, tY);
				IsHorizontalTableError = false;
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				rotatedY = 0;
				IsVerticalTableError = true;
			}
		}
		#endregion
	}
}
