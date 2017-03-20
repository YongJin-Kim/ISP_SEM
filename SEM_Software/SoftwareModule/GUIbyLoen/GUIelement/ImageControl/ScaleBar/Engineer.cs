using System;
using System.Collections.Generic;
using System.Text;

namespace SEC.Nanoeye.Controls.ScaleBar
{
	public struct Engineer
	{
		private double m_Mantissa;
		private int m_Exponent;
		private const int m_Base = 10;

		public double Value
		{
			get { return m_Mantissa * Math.Pow(10, m_Exponent); }
		}

		public double Mantissa
		{
			get { return m_Mantissa; }
			set { m_Mantissa = value; }
		}

		public int Exponent
		{
			get { return m_Exponent; }
		}

		public Engineer(double value)
		{
			this = Engineer.ConvertToEngineer(value);
		}

		public Engineer(double mantissa, int exponent)
		{
			m_Mantissa = mantissa;
			m_Exponent = exponent;
		}

		public override string ToString()
		{
			return this.Value.ToString("E");
		}

		/// <summary>
		/// 가수부를 지정된 값중 하나로 내림 근사화 합니다.
		/// </summary>
		/// <param name="engineer"></param>
		/// <param name="approximate"></param>
		/// <returns></returns>
		public static Engineer ApproximateMantissa(Engineer engineer, double[] approximate)
		{
			Array.Sort(approximate);

			for (int i = approximate.Length - 1; i >= 0; i--)
			{
				if (approximate[i] <= engineer.Mantissa)
				{
					engineer.Mantissa = approximate[i];
					break;
				}
			}

			return engineer;
		}

		private static Engineer ConvertToEngineer(double value)
		{
			Engineer engineer = new Engineer();

			if (Math.Abs(value) < 1)
			{
				while (Math.Abs(value) < 1)
				{
					value *= m_Base;
					engineer.m_Exponent--;
				}
				engineer.m_Mantissa = value;
			}
			else if (Math.Abs(value) >= 10)
			{
				while (Math.Abs(value) >= 10)
				{
					value /= 10;
					engineer.m_Exponent++;
				}
				engineer.m_Mantissa = value;
			}
			else
			{
				engineer.m_Mantissa = value;
				engineer.m_Exponent = 0;
			}

			return engineer;
		}
	}
}
