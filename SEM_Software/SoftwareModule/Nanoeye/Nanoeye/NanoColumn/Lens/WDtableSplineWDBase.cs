using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Lens
{
	/// <summary>
	/// Spline 함수를 이용하여 Lens-Object-Coarse 값과 배율 보정 값을 결정 한다.
	/// </summary>
	internal class WDtableSplineWDBase : SECtype.TableBase, IMagCorrector
	{
		#region Property & Variables
		/// <summary>
		/// Item의 갯수. WD, OBJ, Magconstant, ScanRotationOffset
		/// </summary>
		private const int itemCount = 4;

		#region Table
		SortedList<double, double> tableObject = new SortedList<double, double>();
		SortedList<double, double> tableMagconst = new SortedList<double, double>();
		SortedList<double, double> tableRotationOffset = new SortedList<double, double>();
		#endregion

		#region Column Value
		private ColumnDouble _Obj1 = null;
		/// <summary>
		/// Object Lens
		/// </summary>
		internal ColumnDouble Obj1
		{
			get { return _Obj1; }
			set { _Obj1 = value; }
		}

		private ColumnDouble _ScanRotation = null;
		internal ColumnDouble ScanRotation
		{
			get { return _ScanRotation; }
			set { _ScanRotation = value; }
		}

		private double _MagConstant = 1;
		public double MagConstant
		{
			get { return _MagConstant; }
		}
		#endregion

		private int _SelectedIndex = -1;
		public override int SelectedIndex
		{
			get { return _SelectedIndex; }
			set
			{
				_SelectedIndex = value;
				WDvalueChange();
			}
		}

		public override object SeletedItem
		{
			get
			{
				if(tableObject.Count == 0) { return null; }
				else { return (int)(tableObject.Keys.First() + _SelectedIndex); }
			}
			set { SelectedIndex = (int)((int)value - tableObject.Keys.First()); }
		}

		public override int Length
		{
			get
			{
				if (tableObject.Count == 0) { return 0; }
				return (int)tableObject.Keys.Last() - (int)tableObject.Keys.First();
			}
		}

		public override object[] this[int index]
		{
			get
			{
				if ((index < 0) || (index > Length)) { throw new ArgumentException("Invalid index."); }

				return GetValues(index);
			}
			set
			{
				if (value.Length != itemCount) { throw new ArgumentException("Invlid Length. Must 3."); }

				double preWD = tableMagconst.Keys.First();

				TableChange(preWD, value);
			}
		}
		#endregion

		#region Event
		protected override void OnTableChanged()
		{
			if (_IsInited) { base.OnTableChanged(); }
		}

		public event EventHandler MagConstantChanged;
		protected virtual void OnMagConstantChanged()
		{
			if (MagConstantChanged != null)
			{
				MagConstantChanged(this, EventArgs.Empty);
			}
		}

		protected override void OnSelectedIndexChanged()
		{
			if (_IsInited) { base.OnSelectedIndexChanged(); }
		}
		#endregion

		private void WDvalueChange()
		{
			if (_SelectedIndex < 0)
			{
				_SelectedIndex = -1;
				return;
			}

			if (_SelectedIndex > Length)
			{
				SelectedIndex = Length;
				return;
			}

			object[] result = GetValues(_SelectedIndex);

			double temp;

			// Lens 설정
			temp = (double)result[1];
			if (temp > _Obj1.Maximum)
			{
				temp = _Obj1.Maximum;
				Trace.WriteLine(this.Name + " - object Spline result over Maximum. WD - " + SeletedItem.ToString(), "Warring");
			}
			if (temp < _Obj1.Minimum)
			{
				temp = _Obj1.Minimum;
				Trace.WriteLine(this.Name + " - object Spline result under Minimum. WD - " + SeletedItem.ToString(), "Warring");
			}
			_Obj1.Value = temp;

			// Magnification Constatn 설정
			temp = (double)result[2];
			if (temp <= 0)
			{
				temp = 0.001;
				Trace.WriteLine(this.Name + " - mag constant Spline result under 0. WD - " + SeletedItem.ToString(), "Warring");
			}
			_MagConstant = temp;

			// Scan-Rotation Offset 설정.
			temp = (double)result[3];

			temp += 180;
			while (temp < 0)
			{
				temp += 360;
			}

			temp %= 360;
			temp -= 180;
			_ScanRotation.Offset = temp;

			OnSelectedIndexChanged();
			OnMagConstantChanged();
		}

		private object[] GetValues(int index)
		{
			object[] result = new object[itemCount];

			double wd = tableObject.Keys.First() + index;

			result[0] = (int)wd;
			try
			{
				result[1] = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableObject, wd);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Fail to get WD distance.", "Error");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				result[1] = 0d;
			}

			try
			{
				result[2] = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagconst, wd);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Fail to get WD MagConstant.", "Error");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				result[2] = 1d;
			}

			try
			{
				result[3] = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableRotationOffset, wd);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Fail to get WD Rotation Offset.", "Error");
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);
				result[3] = 0d;
			}

			return result;
		}

		#region Table 관련
		public override void TableSet(object[,] values)
		{
			tableMagconst.Clear();
			tableObject.Clear();
			tableRotationOffset.Clear();

			if(values.GetLength(1) != itemCount) { throw new ArgumentException("Column count must be 3. WD, Lens-Object, MagConstant", "values"); }

			for (int i = 0; i < values.GetLength(0); i++)
			{
				double wd = (int)values[i, 0];

				tableObject.Add(wd, (double)values[i, 1]);
				tableMagconst.Add(wd, (double)values[i, 2]);
				tableRotationOffset.Add(wd, (double)values[i, 3]);
			}

			OnTableChanged();

			if(Length > 0) 
			{
				SelectedIndex = 0;
			}
		}

		public override object[,] TableGet()
		{
			object[,] result = new object[tableObject.Count, itemCount];

			for (int i=0; i < tableObject.Count; i++)
			{
				result[i, 0] = (int)tableObject.Keys[i];
				result[i, 1] = tableObject.Values[i];
				result[i, 2] = tableMagconst.Values[i];
				result[i, 3] = tableRotationOffset.Values[i];
			}

			return result;
		}

		public override void TableAppend(object[] values)
		{
			if (values.Length != itemCount) { throw new ArgumentException("Invalid values length", "values"); }

			double key = (int)values[0];

			if (tableObject.ContainsKey(key)) { throw new ArgumentException("Same magnification already exist."); }

			tableObject.Add(key, (double)values[1]);
			tableMagconst.Add(key, (double)values[2]);
			tableRotationOffset.Add(key, (double)values[3]);

			OnTableChanged();
		}

		public override void TableRemove(object key)
		{
			bool result = false;

			int dKey = (int)key;

			result |= tableMagconst.Remove(dKey);
			result |= tableObject.Remove(dKey);
			result |= tableRotationOffset.Remove(dKey);

			if (result) { OnTableChanged(); }
		}
		#endregion

		public override void SetStyle(int index, ref object value)
		{
			switch (index)
			{
			case (int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Lens_WD_Constant_Get:
				value = _MagConstant;
				break;
			case (int)SEC.Nanoeye.NanoColumn.EnumIControlTableSetStyle.Table_Validate:
				value = TableValidate();
				break;
			default:
				throw new NotSupportedException();
			}
		}

		private object TableValidate()
		{
			List<string> result = new List<string>();

			for (double t = tableObject.Keys.First(); t <= tableObject.Keys.Last(); t += 1)
			{

				double lens = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableObject, t);
				double magCon = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagconst, t);

				if ((lens < _Obj1.Minimum) || (lens > _Obj1.Maximum) || (magCon <= 0))
				{
					result.Add(t.ToString() + "\t" + lens.ToString() + "\t" + magCon.ToString());
				}
			}

			if (result.Count == 0) { return null; }
			else { return result.ToArray(); }
		}

		#region 동기화
		public override void Sync()
		{
			throw new NotImplementedException();
		}

		public override bool Validate()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Init
		public override void BeginInit()
		{
			_IsInited = false;
		}

		public override void EndInit(bool sync)
		{
			_IsInited = true;

			OnTableChanged();
			SelectedIndex = _SelectedIndex;
		}
		#endregion

		public override int IndexMinimum
		{
			get { return 0; }
		}

		public override int IndexMaximum
		{
			get { return Length; }
		}

	}
}
