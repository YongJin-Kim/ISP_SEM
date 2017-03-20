using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using SECtype = SEC.GenericSupport.DataType;



namespace SEC.Nanoeye.NanoColumn.Scan
{
	/// <summary>
	/// Spline 함수를 이용하여 배율을 결정 한다.
	/// TableSet시 Feedbak 모드를 이용하여 두개의 테이블로 분리 후 사용 한다.
	/// </summary>
	internal class MagTableSpline : SECtype.TableBase
	{
		#region Property & Variables
		#region Table
		/// <summary>
		/// Feedback이 0일때(고배율) Mag X의 값 List
		/// </summary>
		private SortedList<double, double> tableMagXupper = new SortedList<double, double>();
		/// <summary>
		/// Feedback이 0일때(고배율) Mag Y의 값 List
		/// </summary>
		private SortedList<double, double> tableMagYupper = new SortedList<double, double>();

		/// <summary>
		/// Feedback이 1일때(저배율) Mag X의 값 List
		/// </summary>
		private SortedList<double, double> tableMagXlower = new SortedList<double, double>();
		/// <summary>
		/// Feedback이 1일때(저배율) Mag Y의 값 List
		/// </summary>
		private SortedList<double, double> tableMagYlower = new SortedList<double, double>();
		#endregion

		#region Column Value
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

		private IMagCorrector _MagCorrector = null;
		internal IMagCorrector MagCorrector
		{
			get { return _MagCorrector; }
			set
			{
				if (_MagCorrector != null) { _MagCorrector.MagConstantChanged -= new EventHandler(_MagCorrector_MagConstantChanged); }
				_MagCorrector = value;
				if (_MagCorrector != null) { _MagCorrector.MagConstantChanged += new EventHandler(_MagCorrector_MagConstantChanged); }
			}
		}

		private int _MaximumValue = 300000000;
		/// <summary>
		/// 설정 할 수 있는 최대 배율.
		/// </summary>
		internal int MaximumValue
		{
			get { return _MaximumValue; }
			set { _MaximumValue = value; }
		}

		private int _MinimumValue = 10;
		internal int MinimumValue
		{
			get { return _MinimumValue; }
			set { _MinimumValue = value; }
		}
		#endregion

		public override int IndexMinimum
		{
			get
			{
				int max, min;
				GetIndex_MaxMin(out max, out min);
				return min;
			}
		}

		public override int IndexMaximum
		{
			get
			{
				int max, min;
				GetIndex_MaxMin(out max, out min);
				return max;
			}
		}

		private int _SelectedIndex = -1;
		/// <summary>
		/// 인덱스가 바로 배율이다.
		/// </summary>
		public override int SelectedIndex
		{
			get { return _SelectedIndex; }
			set
			{
				if (value == -1)
				{
					_SelectedIndex = -1;
					return;
				}
				if (value < IndexMinimum)
				{
					value = IndexMinimum;
				}
				else if(value > IndexMaximum)
				{
					value = IndexMaximum;
				}
				_SelectedIndex = value;
				MagValueChange();
			}
		}

		/// <summary>
		/// 구현상의 오차를 최소화 하기 위해
		/// 인덱스와 동일하게 사용 한다.
		/// </summary>
		public override object SeletedItem
		{
			get { return _SelectedIndex; }
			set { SelectedIndex = (int)value; }
		}

		public override int Length
		{
			get
			{
				int max, min;
				GetIndex_MaxMin(out max, out min);

				return max - min;
			}
		}

		/// <summary>
		/// 인덱스가 바로 배율이다.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public override object[] this[int index]
		{
			get
			{
				if ((index < IndexMinimum) || (index > IndexMaximum))
				{
					throw new IndexOutOfRangeException();
				}

				return GetValues(index);
			}
			set
			{
				//if (value.Length != 4) { throw new ArgumentException("Invlid Length. Must 4."); }

				//double preMag = (Math.Round(Math.Pow(10, tableMagXlower.Keys.First())) + index) * _WDtable.MagConstant;

				//TableChange(preMag, value);

				// TODO : MgTableSpline 일단은 동작 확인 및 버그 원을 최소화 하게 위해 구현 하지 않음.
				throw new NotSupportedException();
			}
		}
		#endregion

		#region Event
		protected override void OnTableChanged()
		{
			if (_IsInited) { base.OnTableChanged(); }
		}

		protected override void OnSelectedIndexChanged()
		{
			if (_IsInited) { base.OnSelectedIndexChanged(); }
		}
		#endregion

		void _MagCorrector_MagConstantChanged(object sender, EventArgs e)
		{
			OnTableChanged();
			MagValueChange();
		}

		private void MagValueChange()
		{
			if (_SelectedIndex < 0) { return; }
			object[] result = GetValues(_SelectedIndex);

			double temp;

			temp = (double)result[1];
			if (temp < _MagXCvd.Minimum)
			{
				temp = _MagXCvd.Minimum;
				Trace.WriteLine(this.Name + " - MagnificationX Spline result under minimum. Mag - " + SeletedItem.ToString(), "Warring");
			}
			if (temp > _MagXCvd.Maximum)
			{
				temp = _MagXCvd.Maximum;
				Trace.WriteLine(this.Name + " - MagnificationX Spline result over maximum. Mag - " + SeletedItem.ToString(), "Warring");
			}
			_MagXCvd.Value = temp;

			temp = (double)result[1];
			if (temp < _MagYCvd.Minimum)
			{
				temp = _MagYCvd.Minimum;
				Trace.WriteLine(this.Name + " - MagnificationY Spline result under minimum. Mag - " + SeletedItem.ToString(), "Warring");
			}
			if (temp > _MagYCvd.Maximum)
			{
				temp = _MagYCvd.Maximum;
				Trace.WriteLine(this.Name + " - MagnificationY Spline result over maximum. Mag - " + SeletedItem.ToString(), "Warring");
			}
			_MagYCvd.Value = temp;

			_FeedbackCvi.Value = (int)result[3];

			OnSelectedIndexChanged();
		}

		/// <summary>
		/// 인덱스 자체가 배율이다!!!
		/// </summary>
		/// <param name="index">배율</param>
		/// <returns></returns>
		private object[] GetValues(int index)
		{
			object[] result = new object[4];

			SortedList<double, double> tbX, tbY;

			// 배율 찾기.
			// ??? 수정 중... Floor 연산을 하는 이유는 MagConstant가 곱해진후 적은 배율로 맞추는 것으로 정한 정책일뿐!!! Round나 Celling을 사용해도 무관
			//double mag = (int)Math.Ceiling((Math.Round(Math.Pow(10, tableMagXlower.Keys.First())) + index) * _WDtable.MagConstant);
			double mag = index;
			result[0] = (int)mag;
			// Constant에 의해 변형된 배율이 정수를 유지 해야 하므로 이렇게 변환하여 계산한다.
			// 다시 MagConstant를 곱한 후에 정수화를 하지 않는 이유는,
			// Spline 탐색시 double로 탐색이 가능 해서 이다.
			mag = Math.Log10(mag / _MagCorrector.MagConstant);


			
			if(mag < tableMagXlower.Keys.First())
			{
			    mag = tableMagXlower.Keys.First();
			}

			if (mag <= tableMagXlower.Keys.Last())
			{
				tbX = tableMagXlower;
				tbY = tableMagYlower;
				result[3] = 1;
			}
			else
			{
				tbX = tableMagXupper;
				tbY = tableMagYupper;
				result[3] = 0;
			}

			result[1] = SEC.GenericSupport.Mathematics.Interpolation.Spline(tbX, mag);
			result[2] = SEC.GenericSupport.Mathematics.Interpolation.Spline(tbY, mag);
			return result;
		}

		/// <summary>
		/// 최대 배율과 최소 배율을 구한다.
		/// </summary>
		/// <param name="max">최대 배율</param>
		/// <param name="min">최소 배율</param>
		private void GetIndex_MaxMin(out int max, out int min)
		{
			if (tableMagXlower.Count == 0)
			{
				if (tableMagXupper.Count == 0)
				{
					max = -2;
					min = -1;
				}
				else
				{
					min = (int)Math.Round(Math.Pow(10, tableMagXupper.Keys.First()));
					max = (int)Math.Round(Math.Pow(10, tableMagXupper.Keys.Last()));
				}
			}
			else if (tableMagXupper.Count == 0)
			{
				min = (int)Math.Round(Math.Pow(10, tableMagXlower.Keys.First()));
				max = (int)Math.Round(Math.Pow(10, tableMagXlower.Keys.Last()));
			}
			else
			{
				min = (int)Math.Round(Math.Pow(10, tableMagXlower.Keys.First()));
				max = (int)Math.Round(Math.Pow(10, tableMagXupper.Keys.Last()));
			}

			max = (int)Math.Floor(max * _MagCorrector.MagConstant);
			min = (int)Math.Ceiling(min * _MagCorrector.MagConstant);

			//if (max > _MaximumValue) { max = _MaximumValue; }
            if (_MaximumValue != 300000000)
            {
                if (_MaximumValue != 150000)
                {
                    if (max < _MaximumValue) { max = _MaximumValue; }
                }
               
            }


            if (min < _MinimumValue) { min = _MinimumValue; }
		}

		#region Table 관리
		public override void TableSet(object[,] values)
		{
			tableMagXlower.Clear();
			tableMagXupper.Clear();
			tableMagYlower.Clear();
			tableMagYupper.Clear();

			if (values.GetLength(1) != 4) { throw new ArgumentException("Column count must be 4. Mag-int, MagX-double, MagY-double, F.B.-int", "values"); }

			for (int i = 0; i < values.GetLength(0); i++)
			{
				SortedList<double, double> tbX, tbY;
				// Low magnification
				if ((int)values[i, 3] == 1)
				{
					tbX = tableMagXlower;
					tbY = tableMagYlower;
				}
				// High magnification
				else
				{
					tbX = tableMagXupper;
					tbY = tableMagYupper;
				}

				double mag = Math.Log10((int)values[i, 0] );

				tbX.Add(mag, (double)values[i, 1]);
				tbY.Add(mag, (double)values[i, 2]);
			}

			OnTableChanged();

			//if(Length > 0) 
			//{
			//     SelectedIndex = ; 				
			//}
		}

		public override object[,] TableGet()
		{
			object[,] result = new object[tableMagXlower.Count + tableMagXupper.Count, 4];

			int cnt = 0;

			for (int i = 0; i < tableMagXlower.Count; i++)
			{
				// 캐스팅을 하면 자리수 오차가 발생 한다.
				// 반올림 연산을 하면, int-double 형 변환간 버림에 의한 오차를 보상 할 수 있다.
				result[cnt, 0] = (int)Math.Round(Math.Pow(10, tableMagXlower.Keys[i])) ;
				result[cnt, 1] = tableMagXlower.Values[i];
				result[cnt, 2] = tableMagYlower.Values[i];
				result[cnt, 3] = 1;
				cnt++;
			}

			for (int i = 0; i < tableMagXupper.Count; i++)
			{
				result[cnt, 0] = (int)Math.Round(Math.Pow(10, tableMagXupper.Keys[i]));
				result[cnt, 1] = tableMagXupper.Values[i];
				result[cnt, 2] = tableMagYupper.Values[i];
				result[cnt, 3] = 0;
				cnt++;
			}
			return result;
		}

		public override void TableAppend(object[] values)
		{
			if (values.Length != 4) { throw new ArgumentException("values's length must be 4.", "values"); }
			double mag = Math.Log10((int)values[0] / _MagCorrector.MagConstant);

			SortedList<double, double> tbX, tbY;
			// Low magnification
			if ((int)values[3] == 1)
			{
				tbX = tableMagXlower;
				tbY = tableMagYlower;
			}
			// High magnification
			else
			{
				tbX = tableMagXupper;
				tbY = tableMagYupper;
			}

			if (tbX.ContainsKey(mag)) { throw new ArgumentException("Same magnification already exist."); }

			tbX.Add(mag, (double)values[1]);
			tbY.Add(mag, (double)values[2]);

			OnTableChanged();
		}

		public override void TableRemove(object key)
		{
			bool result = false;

			double mag = Math.Log10((int)key / _MagCorrector.MagConstant);

			result |= tableMagXlower.Remove(mag);
			result |= tableMagYlower.Remove(mag);

			// High magnification
			result |= tableMagXupper.Remove(mag);
			result |= tableMagYupper.Remove(mag);
            

			if (result) { OnTableChanged(); }
		}
		#endregion

		#region 초기화
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

		#region 동기화
		public override void Sync()
		{
			throw new NotSupportedException();
		}

		public override bool Validate()
		{
			throw new NotSupportedException();
		}
		#endregion

		public override void SetStyle(int index, ref object value)
		{
			EnumIControlTableSetStyle ectss = (EnumIControlTableSetStyle)index;
			switch (ectss)
			{
			case EnumIControlTableSetStyle.Scan_Mag_Maximum_Set:
				_MaximumValue = (int)value;
				OnTableChanged();
				break;
			case EnumIControlTableSetStyle.Scan_Mag_Maximum_Get:
				value = _MaximumValue;
				break;
			case EnumIControlTableSetStyle.Table_Validate:
				value = TableValidate();
				break;
			case EnumIControlTableSetStyle.Scan_Mag_Minimum_Set:
				_MinimumValue = (int)value;
				OnTableChanged();
				break;
			case EnumIControlTableSetStyle.Scan_Mag_Minimum_Get:
				value = _MinimumValue;
				break;
			default:
				throw new NotSupportedException();
			}
		}

		private object TableValidate()
		{
			List<string> result = new List<string>();

			if (tableMagXupper.Count > 0)
			{
				for (int t = (int)Math.Round(Math.Pow(10, tableMagXupper.Keys.First())); t <= (int)Math.Round(Math.Pow(10, tableMagXupper.Keys.Last())); t++)
				{

					double ratioX = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagXupper, Math.Log10(t));
					double ratioY = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagYupper, Math.Log10(t));

					if ((ratioX < _MagXCvd.Minimum) || (ratioX > _MagXCvd.Maximum) || (ratioY < _MagYCvd.Minimum) || (ratioY > _MagYCvd.Maximum))
					{
						result.Add(t.ToString() + "\t" + ratioX.ToString() + "\t" + ratioY.ToString() + "\t1");
					}
				}
			}

			if (tableMagXlower.Count > 0)
			{
				for (int t = (int)Math.Round(Math.Pow(10, tableMagXlower.Keys.First())); t <= (int)Math.Round(Math.Pow(10, tableMagXlower.Keys.Last())); t++)
				{

					double ratioX = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagXlower, Math.Log10(t));
					double ratioY = SEC.GenericSupport.Mathematics.Interpolation.Spline(tableMagYlower, Math.Log10(t));

					if ((ratioX < _MagXCvd.Minimum) || (ratioX > _MagXCvd.Maximum) || (ratioY < _MagYCvd.Minimum) || (ratioY > _MagYCvd.Maximum))
					{
						result.Add(t.ToString() + "\t" + ratioX.ToString() + "\t" + ratioY.ToString() + "\t0");
					}
				}
			}

			if (result.Count == 0) { return null; }
			else { return result.ToArray(); }
		}
	}
}

