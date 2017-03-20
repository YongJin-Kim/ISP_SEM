using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn.Lens
{
	/// <summary>
	/// Lens-Object-Coarse를 기준으로 WD를 계산하여 각종 값을 연동 한다.
	/// </summary>
	internal class WDSplineObjBase : SECtype.ControlValueBase, IWDSplineObjBase
	{
		#region Property & Variables
		/// <summary>
		/// Item의 갯수. WD, OBJ, Magconstant, ScanRotationOffset
		/// </summary>
		private const int itemCount = 5;

		#region Table
		SortedList<double, double> tableWD = new SortedList<double, double>();
		SortedList<double, double> tableMagconst = new SortedList<double, double>();
		SortedList<double, double> tableRotationOffset = new SortedList<double, double>();
		SortedList<double, double> tableBeamShiftRotationOffset = new SortedList<double, double>();
		#endregion

		#region Column Value
		private ColumnDouble _LensObjCoarse = null;
		internal ColumnDouble LensObjCoarse
		{
			get { return _LensObjCoarse; }
			set
			{
				if (_LensObjCoarse != null) { _LensObjCoarse.ValueChanged -= new EventHandler(_LensObjCoarse_ValueChanged); }
				_LensObjCoarse = value;
				if (_LensObjCoarse != null) { _LensObjCoarse.ValueChanged += new EventHandler(_LensObjCoarse_ValueChanged); }
			}
		}

		private ColumnDouble _ScanRotation = null;
		internal ColumnDouble ScanRotation
		{
			get { return _ScanRotation; }
			set { _ScanRotation = value; }
		}

        private SECtype.IControlDouble _BeamShiftRotation = null;
        internal SECtype.IControlDouble BeamShiftRotation
        {
            get { return _BeamShiftRotation; }
            set { _BeamShiftRotation = value; }
        }

        //private ColumnDouble _BeamShiftRotation = null;
        //internal ColumnDouble BeamShiftRotation
        //{
        //    get { return _BeamShiftRotation; }
        //    set { _BeamShiftRotation = value; }
        //}
		#endregion

		private double _MagConstant = 1;
		public double MagConstant
		{
			get { return _MagConstant; }
		}

		private int _WorkingDistance = 1;
		public int WorkingDistance
		{
			get { return _WorkingDistance; }
		}

		private bool _IsNegativeOverflow = true;
		public bool IsNegativeOverflow
		{
			get { return _IsNegativeOverflow; }
		}

		private bool _IsPositiveOverflow = true;
		public bool IsPositiveOverflow
		{
			get { return _IsPositiveOverflow; }
		}
		#endregion

		#region Event
		public event EventHandler  TableChanged;
		protected virtual void OnTableChanged()
		{
			if (_IsInited)
			{
				if (TableChanged != null) { TableChanged(this, EventArgs.Empty); }
			}
		}

		public event EventHandler MagConstantChanged;
		protected virtual void OnMagConstantChanged()
		{
			if (MagConstantChanged != null)
			{
				MagConstantChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler WorkingDistanceChagned;
		protected virtual void OnWorkingDistanceChagned()
		{
			if (WorkingDistanceChagned != null)
			{
				WorkingDistanceChagned(this, EventArgs.Empty);
			}
		}
		#endregion

		void _LensObjCoarse_ValueChanged(object sender, EventArgs e)
		{
			InnerValueChange();
		}

		private void InnerValueChange()
		{
			if (!_IsInited) { return; }
			if (!_Enable) { return; }

			double lens = _LensObjCoarse.Value / _LensObjCoarse.Precision;
			double magCon;
			int wdVal;
			double scanOffset;
			double beamOffset;
			switch (tableWD.Count)
			{
			case 0:
				_IsNegativeOverflow = true;
				_IsPositiveOverflow = true;

				magCon = 1;
				wdVal = 1;
				scanOffset = _ScanRotation.Offset;
				beamOffset = _BeamShiftRotation.Value;
				break;
			case 1:
				_IsNegativeOverflow = true;
				_IsPositiveOverflow = true;

				wdVal = (int)(Math.Round(tableWD.Values[0]));
				magCon = tableMagconst.Values[0];
				scanOffset = tableRotationOffset.Values[0];
				beamOffset = tableBeamShiftRotationOffset.Values[0];

				break;
			default:
				if (lens >= tableWD.Keys.Last())
				{
					_IsNegativeOverflow = true;
					_IsPositiveOverflow = false;

					wdVal = (int)(Math.Round(tableWD.Values.Last()));
					magCon = tableMagconst.Values.First();
					scanOffset = tableRotationOffset.Values.First();
					beamOffset = tableBeamShiftRotationOffset.Values.First();
				}
				else if (lens <= tableWD.Keys.First())
				{
					_IsNegativeOverflow = false;
					_IsPositiveOverflow = true;

					wdVal = (int)(Math.Round(tableWD.Values.First()));
					magCon = tableMagconst.Values.Last();
					scanOffset = tableRotationOffset.Values.Last();
					beamOffset = tableBeamShiftRotationOffset.Values.Last();
				}
				else
				{
					_IsNegativeOverflow = false;
					_IsPositiveOverflow = false;

					double temp = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableWD, lens);
					wdVal = (int)(Math.Round(temp));

					magCon = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagconst, wdVal);
					scanOffset = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableRotationOffset, wdVal);
					beamOffset = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableBeamShiftRotationOffset, wdVal);
				}
				break;
			}

			scanOffset *= _ScanRotation.Precision;
			beamOffset *= _BeamShiftRotation.Precision;
			if (beamOffset > _BeamShiftRotation.Maximum) { beamOffset = _BeamShiftRotation.Maximum; }
			else if (beamOffset < _BeamShiftRotation.Minimum) { beamOffset = _BeamShiftRotation.Minimum; }

			if (_MagConstant != magCon)
			{
				_MagConstant = magCon;
				OnMagConstantChanged();
			}
			if (_WorkingDistance != wdVal)
			{
				_WorkingDistance = wdVal;
				OnWorkingDistanceChagned();
			}

			while (scanOffset < -180) { scanOffset += 360; }
			while (scanOffset > 180) { scanOffset -= 360; }
			if (_ScanRotation.Offset != scanOffset) { _ScanRotation.Offset = scanOffset; }

			while (beamOffset < -180) { beamOffset += 360; }
			while (beamOffset > 180) { beamOffset -= 360; }
			if (_BeamShiftRotation.Value != beamOffset) { _BeamShiftRotation.Value = beamOffset; }
		}

		#region Table
		public void TableSet(object[,] values)
		{
			tableMagconst.Clear();
			tableWD.Clear();
			tableRotationOffset.Clear();
			tableBeamShiftRotationOffset.Clear();

			if (values.GetLength(1) != itemCount) { throw new ArgumentException("Column count must be " + itemCount.ToString() + ". WD, Lens-Object, MagConstant", "values"); }

			for (int i = 0; i < values.GetLength(0); i++)
			{
				if (values[i, 0] == null) { continue; }

				int obj = (int)values[i, 1];
				int wd = (int)values[i, 0];

				tableWD.Add(obj, wd);
				tableMagconst.Add(wd, (double)values[i, 2]);
				tableRotationOffset.Add(wd, (int)values[i, 3]);
				tableBeamShiftRotationOffset.Add(wd, (int)values[i, 4]);
			}

			OnTableChanged();
			InnerValueChange();
		}

		public object[,] TableGet()
		{
			object[,] result = new object[tableWD.Count, itemCount];

			int i;

			for (i=0; i < tableWD.Count; i++)
			{
				result[i, 0] = (int)tableWD.Values[i];
				result[i, 1] = (int)tableWD.Keys[i];
				result[tableWD.Count - i - 1, 2] = (double)tableMagconst.Values[i];
				result[tableWD.Count - i - 1, 3] = (int)tableRotationOffset.Values[i];
				result[tableWD.Count - i - 1, 4] = (int)tableBeamShiftRotationOffset.Values[i];
			}

			return result;
		}

		public void TableAppend(object[] values)
		{
			if (values.Length != itemCount) { throw new ArgumentException("Invalid values length", "values"); }

			int wd = (int)values[0];
			int obj = (int)values[1];

			if (tableWD.ContainsKey(obj)) { throw new ArgumentException("Same obj-value already exist."); }
			if (tableMagconst.ContainsKey(wd)) { throw new ArgumentException("Same wd already exist."); }

			tableWD.Add(obj, wd);
			tableMagconst.Add(wd, (double)values[2]);
			tableRotationOffset.Add(wd, (int)values[3]);
			tableBeamShiftRotationOffset.Add(wd, (int)values[4]);

			OnTableChanged();
			InnerValueChange();
		}

		public void TableRemove(object key)
		{
			bool result = false;

			int wd = (int)key;

			int wdIndex = tableWD.IndexOfValue(wd);
			if (wdIndex >= 0)
			{
				result = true;
				tableWD.RemoveAt(wdIndex);
			}

			result |= tableMagconst.Remove(wd);
			result |= tableRotationOffset.Remove(wd);
			result |= tableBeamShiftRotationOffset.Remove(wd);

			if (result)
			{
				OnTableChanged();
				InnerValueChange();
			}
		}

		public void TableChange(object preKey, object[] values)
		{
			BeginInit();

			TableRemove(preKey);
			TableAppend(values);

			EndInit();

			OnTableChanged();
			InnerValueChange();
		}
		#endregion

		public override void BeginInit()
		{
			_IsInited = false;
		}

		public override void EndInit(bool sync)
		{
			_IsInited = true;
			_Enable = true;
			InnerValueChange();
		}

		public override void Sync()
		{
			InnerValueChange();
		}

		public override bool Validate()
		{
			bool same = true;

			double lens = _LensObjCoarse.Value;
			double magCon;
			int wdVal;
			double scanOffset;
			double beamOffset;
			switch (tableWD.Count)
			{
			case 0:
				_IsNegativeOverflow = true;
				_IsPositiveOverflow = true;

				magCon = 1;
				wdVal = 1;
				scanOffset = _ScanRotation.Offset;
				beamOffset = _BeamShiftRotation.Value;
				break;
			case 1:
				_IsNegativeOverflow = true;
				_IsPositiveOverflow = true;
				magCon = tableMagconst.Values[0];
				wdVal = (int)(Math.Round(tableWD.Values[0]));
				scanOffset = tableRotationOffset.Values[0];
				beamOffset = tableBeamShiftRotationOffset.Values[0];
				break;
			default:
				if (lens >= tableWD.Keys.Last())
				{
					_IsNegativeOverflow = true;
					_IsPositiveOverflow = false;

					magCon = tableMagconst.Values.Last();
					wdVal = (int)(Math.Round(tableWD.Values.Last()));
					scanOffset = tableRotationOffset.Values.Last();
					beamOffset = tableBeamShiftRotationOffset.Values.Last();
				}
				else if (lens <= tableWD.Keys.First())
				{
					_IsNegativeOverflow = false;
					_IsPositiveOverflow = true;

					magCon = tableMagconst.Values.First();
					wdVal = (int)(Math.Round(tableWD.Values.First()));
					scanOffset = tableRotationOffset.Values.First();
					beamOffset = tableBeamShiftRotationOffset.Values.First();
				}
				else
				{
					_IsNegativeOverflow = false;
					_IsPositiveOverflow = false;

					magCon = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagconst, lens);
					wdVal = (int)(Math.Round(SEC.GenericSupport.Mathematics.Interpolation.Spline(tableWD, lens)));
					scanOffset = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableRotationOffset, lens);
					beamOffset = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableBeamShiftRotationOffset, lens);
				}
				break;
			}

			if (_MagConstant != magCon) { same = false; }
			if (_WorkingDistance != wdVal) { same = false; }
			if (_ScanRotation.Offset != scanOffset) { same = false; }
			if (_BeamShiftRotation.Value != beamOffset) { same = false; }
			return same;
		}
	}
}
