using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.Mathematics
{
	public class Interpolation
	{
		#region Spline
		public static double Spline(SortedList<double, double> samples, double target)
		{
			int sampleCount = samples.Count;

			// 인덱스 0에 있는 것은 검색이 안된다.
			// 따라서 다음 문을 이용하여 인덱스 0도 검색 가능하게 한다.
			// 단, 평균 검색 시간은 증가 한다.
			if(samples.ContainsKey(target)) { return samples[target]; }

			if(sampleCount < 2)
			{
				throw new ArgumentException("samples length must be greate then 2.", "samples");
			}

			double[] a = new double[sampleCount];
			double x1, x2;
			double result;
			double h0, h1;
			double[] h = new double[sampleCount];
			int i;

			for(i = 1; i < sampleCount; i++)
			{
				h[i] = samples.Keys[i] - samples.Keys[i - 1];
			}

			if(sampleCount > 2)
			{
				double[] sub = new double[sampleCount - 1];
				double[] diag = new double[sampleCount - 1];
				double[] sup = new double[sampleCount - 1];

				for(i = 1; i <= sampleCount - 2; i++)
				{
					h0 = h[i];
					h1 = h[i + 1];

					diag[i] = (h0 + h1) / 3;
					sup[i] = h1 / 6;
					sub[i] = h0 / 6;
					a[i] = (samples.Values[i + 1] - samples.Values[i]) / h1 - (samples.Values[i] - samples.Values[i - 1]) / h0;
				}

				SolveTridiag(sub, diag, sup, ref a, sampleCount - 2);
			}

			int gap = 0;
			double previous = double.MinValue;

			i = 0;
			while (i < samples.Keys.Count - 1)
			{
				if((samples.Keys[i] > previous) && (samples.Keys[i] < target))
				{
					previous = samples.Keys[i];
					gap = i + 1;
				}
				else
				{
					break;
				}
				i++;
			}

			x1 = target - previous;
			x2 = h[gap] - x1;


            

			result = ((-a[gap - 1] / 6 * (x2 + h[gap]) * x1 + samples.Values[gap - 1]) * x2 +
					  (-a[gap] / 6 * (x1 + h[gap]) * x2 + samples.Values[gap]) * x1) / h[gap];

			return result;
		}

		private static void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
		{
			int i;

			for(i = 2; i <= n; i++)
			{
				sub[i] = sub[i] / diag[i - 1];
				diag[i] = diag[i] - sub[i] * sup[i - 1];
				b[i] = b[i] - sub[i] * b[i - 1];
			}

			b[n] = b[n] / diag[n];

			for(i = n - 1; i >= 1; i--)
			{
				b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
			}
		}


		public static double SplineMulti(SortedList<double, double> samples, double target)
		{
			int sampleCount = samples.Count;

			// 인덱스 0에 있는 것은 검색이 안된다.
			// 따라서 다음 문을 이용하여 인덱스 0도 검색 가능하게 한다.
			// 단, 평균 검색 시간은 증가 한다.
			if(samples.ContainsKey(target)) { return samples[target]; }

			if(sampleCount < 2)
			{
				throw new ArgumentException("samples length must be greate then 2.", "samples");
			}

			double[] a = new double[sampleCount];
			double x1, x2;
			double result;
			double[] h = new double[sampleCount];
			int i;

			for(i = 1; i < sampleCount; i++)
			{
				h[i] = samples.Keys[i] - samples.Keys[i - 1];
			}

			if(sampleCount > 2)
			{

				double[] sub = new double[sampleCount - 1];
				double[] diag = new double[sampleCount - 1];
				double[] sup = new double[sampleCount - 1];

				using(var done = new System.Threading.Semaphore(Environment.ProcessorCount, Environment.ProcessorCount))
				{
					int length = (sampleCount - 2) / Environment.ProcessorCount;
					for(int t = 0; t < Environment.ProcessorCount; t++)
					{
						int num = t;
						System.Threading.ThreadPool.QueueUserWorkItem(delegate
						{
							double h0, h1;
							double temp;
							for(int k = 1 + length * num; k <= length * (num + 1); k++)
							{
								h0 = h[k];
								h1 = h[k + 1];

								temp = (h0 + h1) / 3;
								lock(diag) { diag[k] = temp; }

								temp = h1 / 6;
								lock(sup) { sup[k] = temp; }

								temp = h0 / 6;
								lock(sub) { sub[k] = temp; }

								temp = (samples.Values[k + 1] - samples.Values[k]) / h1 - (samples.Values[k] - samples.Values[k - 1]) / h0;
								lock(a) { a[k] = temp; }
							}
						});
					}
				}

				SolveTridiagMulti(sub, diag, sup, ref a, sampleCount - 2);
			}

			int gap = 0;
			double previous = double.MinValue;

			i = 0;
			while(true)
			{
				if((samples.Keys[i] > previous) && (samples.Keys[i] < target))
				{
					previous = samples.Keys[i];
					gap = i + 1;
				}
				else
				{
					break;
				}
				i++;
			}

			x1 = target - previous;
			x2 = h[gap] - x1;

			result = ((-a[gap - 1] / 6 * (x2 + h[gap]) * x1 + samples.Values[gap - 1]) * x2 +
					  (-a[gap] / 6 * (x1 + h[gap]) * x2 + samples.Values[gap]) * x1) / h[gap];

			return result;
		}

		private static void SolveTridiagMulti(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
		{
			int i;

			for(i = 2; i <= n; i++)
			{
				sub[i] = sub[i] / diag[i - 1];
				diag[i] = diag[i] - sub[i] * sup[i - 1];
				b[i] = b[i] - sub[i] * b[i - 1];
			}

			b[n] = b[n] / diag[n];

			for(i = n - 1; i >= 1; i--)
			{
				b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
			}
		}
		#endregion

		#region Exponential Interpolation
		public static double Exponential(SortedList<int, double> samples, int target)
		{
			if(samples.ContainsKey(target))
			{
				return samples[target];
			}

			int pntRight = -1;
			for(int i = 0; i < samples.Count; i++)
			{
				if(samples.Keys[i] >= target)
				{
					pntRight = i;
					break;
				}
			}

			if(pntRight <= 0)
			{
				throw new ArgumentException("target is not in samples'keys.");
			}

			int gap = samples.Keys[pntRight] - samples.Keys[pntRight - 1];

			if(gap < 2)
			{
				throw new ArgumentException("처리 불가.");
			}

			target -= samples.Keys[pntRight - 1];

			double result = samples.Values[pntRight - 1] *
				Math.Pow(samples.Values[pntRight] / samples.Values[pntRight - 1], (double)target / gap);


			return result;
		}
		#endregion

		#region Bezier
		//public static double Bezier(SortedList<double, double> table, double target)
		//{
		//    int k, kn, nn, nkn;
		//    double blend, muk, munk;

		//    double result = 0;

		//    muk = 1;
		//    munk = Math.Pow(1 - ((table.Keys.First() + table.Keys.Last()) * target - table.Keys.First()), table.Count);

		//    for(k = 0; k < table.Count; k++)
		//    {
		//        nn = table.Count;

		//        kn = k;
		//        nkn = table.Count - k;

		//        blend = muk * munk;

		//        muk *= muk;

		//        munk /= 1 - ((table.Keys.First() + table.Keys.Last()) * target - table.Keys.First());

		//        while(nn >= 1)
		//        {

		//            blend *= nn;
		//            nn--;

		//            if(kn > 1)
		//            {
		//                blend /= kn;
		//                kn--;
		//            }

		//            if(nkn > 1)
		//            {
		//                blend /= nkn;
		//                nkn--;
		//            }
		//        }

		//        result += table.Values[k] * blend;
		//    }
		//    return result;
		//}

		public static double Bezier(SortedList<double, double> table, double target)
		{
			int tableCount = table.Count;

			double tarAdj = (target - table.Keys.First()) / (table.Keys.Last() - table.Keys.First());

			double sum = 0;



			for(int i = 0; i < tableCount; i++)
			{
				sum += table.Values[i] * BezierJ(tableCount, i, tarAdj);
			}
			return sum;
		}

		private static double BezierJ(int tableCount, int index, double target)
		{
			double result = 0;

			result = Helper.BinomialCoefficient(tableCount, index) * Math.Pow(1 - target, tableCount - index) * Math.Pow(target, index);

			return result;
		}
		#endregion
	}
}
