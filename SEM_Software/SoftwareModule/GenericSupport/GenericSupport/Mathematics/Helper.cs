using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.Mathematics
{
	public class Helper
	{
		public static int Factorial(int num)
		{
			int result = 1;

			while(num > 0)
			{
				result *= num--;
			}

			return result;
		}

		public static double BinomialCoefficient(int n, int k)
		{
			double result = 1;

			for(int i = n; i > (n - k); i--)
			{
				result *= i;
			}

			result /= Factorial(k);

			return result;
		}
	}
}
