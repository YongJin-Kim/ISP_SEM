using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECcolumn = SEC.Nanoeye.NanoColumn;
using SECtype = SEC.GenericSupport.DataType;

using System.Threading;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoEmission : AutoFunctionBase
	{
		System.Threading.Timer checker;

		double targetCur;
		double oriGrid;
		SECtype.IControlDouble gridICD;
		SECtype.IControlDouble anodeICD;

		protected override void OnProgressComplet()
		{
			System.Diagnostics.Trace.WriteLine("AutoEmission end ", "Info");

			checker.Change(Timeout.Infinite, Timeout.Infinite);
			checker.Dispose();
			base.OnProgressComplet();
		}

		public override void Cancel()
		{
			gridICD.Value = oriGrid;
			_Cancled = true;
			OnProgressComplet();
		}

		public override void Stop()
		{
			OnProgressComplet();
		}

		struct CurrentInfoStruct
		{
			public double current;
			public double gird;
			public CurrentInfoStruct(double i, double g)
			{
				current = i;
				gird = g;
			}

			public override string ToString()
			{
				return ("Grid - " + gird.ToString() + " ,I - " + current.ToString());
			}
		}

		List<CurrentInfoStruct> cis = new List<CurrentInfoStruct>();

		public void EmissionControl(double targetCurrent, SECcolumn.IColumnValue eghv, SECcolumn.IColumnValue grid)
		{
			System.Diagnostics.Trace.WriteLine("AutoEmission Start - " + targetCurrent.ToString(), "Info");
			checker = new Timer(new TimerCallback(CurrentChecker));

			targetCur = targetCurrent;

			gridICD = grid as SECtype.IControlDouble;
			anodeICD = eghv as SECtype.IControlDouble;

			oriGrid = gridICD.Value;

			cis.Add(new CurrentInfoStruct((double)anodeICD.Read[0], gridICD.Value));

			if (cis[0].current < targetCurrent)
			{
				if (gridICD.Value == gridICD.Maximum)
				{
					OnProgressComplet();
					return;
				}
				else if (gridICD.Value < (gridICD.Maximum - (gridICD.Maximum - gridICD.Minimum) / 10))
				{
					Validate(gridICD, gridICD.Value + (gridICD.Maximum - gridICD.Minimum) / 10);
				}
				else
				{
					Validate(gridICD, gridICD.Maximum);
				}
			}
			else
			{
				if (gridICD.Value == gridICD.Minimum)
				{
					OnProgressComplet();
					return;
				}
				else if (gridICD.Value > (gridICD.Minimum + (gridICD.Maximum - gridICD.Minimum) / 10))
				{
					Validate(gridICD, gridICD.Value + (gridICD.Maximum - gridICD.Minimum) / 10);
				}
				else
				{
					Validate(gridICD, gridICD.Minimum);
				}
			}

			checker.Change(1000, 1000);	// 500msec period
		}

		public void CurrentChecker(Object stateInfo)
		{
			checker.Change(Timeout.Infinite, Timeout.Infinite);
			double current = (double)anodeICD.Read[0];
			double gridValue = gridICD.Value;
			cis.Add(new CurrentInfoStruct(current, gridValue));
			System.Diagnostics.Debug.WriteLine(gridValue.ToString() + " grid, " + current.ToString() + " current", "AutoEmission");


			if (Math.Abs(cis[cis.Count - 1].current - targetCur) < Math.Abs(targetCur * 0.05))
			{
				// target 값의 1% 이내 들었다면.
				OnProgressComplet();
				System.Diagnostics.Debug.WriteLine("Stop by 1%", "AutoEmission");
				return;
			}

			if (Math.Abs(cis[cis.Count - 1].current - targetCur) < Math.Abs(cis[cis.Count - 2].current - targetCur))
			{
				// 오차가 줄었다면.
				if (!Validate(gridICD, gridICD.Value + cis[cis.Count - 1].gird - cis[cis.Count - 2].gird)) {
					System.Diagnostics.Debug.WriteLine("Stop 오차 줄고 최대최소에 걸림", "AutoEmission");
					OnProgressComplet(); return; 
				}
			}
			else
			{
				if (cis.Count == 2)
				{
					// 두번째 것이라면.
					if (!Validate(gridICD, -1 * (gridICD.Maximum - gridICD.Minimum) / 10))
					{
						System.Diagnostics.Debug.WriteLine("두번째거", "AutoEmission");
						OnProgressComplet();
						return;
					}
				}
				else
				{
					if (!Validate(gridICD, gridICD.Value - (cis[cis.Count - 1].gird - cis[cis.Count - 2].gird) / 2))
					{
						System.Diagnostics.Debug.WriteLine("최대 최소에 걸림", "AutoEmission");
						OnProgressComplet();
						return;
					}
				}
			}
			checker.Change(1000, 1000);	// 500msec period
		}

		private bool Validate(SECtype.IControlDouble gridICD, double p)
		{
			double target;

			if (p > gridICD.Value + (gridICD.Maximum - gridICD.Minimum) * 0.05)
			{
				target = gridICD.Value + (gridICD.Maximum - gridICD.Minimum) * 0.05;
			}
			else if (p < gridICD.Value - (gridICD.Maximum - gridICD.Minimum) * 0.05)
			{
				target = gridICD.Value - (gridICD.Maximum - gridICD.Minimum) * 0.05;
			}
			else
			{
				target = p;
			}

			if (target > gridICD.Maximum)
			{
				if (gridICD.Value == gridICD.Maximum) { return false; }
				gridICD.Value = gridICD.Maximum;
			}
			else if (target < gridICD.Minimum)
			{
				if (gridICD.Value == gridICD.Minimum) { return false; }
				gridICD.Value = gridICD.Minimum;
			}
			else
			{
				gridICD.Value = target;
			}

			return true;
		}

	}
}
