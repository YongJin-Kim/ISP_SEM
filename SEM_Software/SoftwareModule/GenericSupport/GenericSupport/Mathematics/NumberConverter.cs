using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.GenericSupport.Mathematics
{
	public class NumberConverter
	{
		/// <summary>
		/// 숫자를 단위를 포함한 문자열로 바꿔 준다. 마지막 자리는 단위를 위해 예약 할 수 있다.
		/// </summary>
		/// <param name="arg">변환할 숫자</param>
		/// <param name="exponent">기본 지수(arg에 10^exponenet를 곱한 값을 변환 한다.</param>
		/// <param name="digit">유효 자리 숫자</param>
		/// <param name="replaceBalnk">단위 자리 문자 없을 경우 공백 처리 할 지 여부</param>
		/// <param name="unit">문자열의 단위 ex) M(meter), A(ampear) 등 </param>
		/// <returns></returns>
		public static string ToUnitString(double arg, int exponent, int digit, bool replaceBalnk, char unit)
		{
			return NumberToRegular(arg, exponent, digit, replaceBalnk, unit, 3);
		}

		/// <summary>
		/// 면적등의 숫자를 단위를 포함한 문자열로 바꿔 준다. 마지막 자리는 단위를 위해 예약 되어 있다.
		/// </summary>
		/// <param name="arg">변환할 숫자</param>
		/// <param name="exponent">변환할 숫자의 기본 제곱 수</param>
		/// <param name="digit">변환된 문자열의 길이</param>
		/// <param name="replaceBalnk">단위 자리를 공백 처리 할지 여부</param>
		/// <returns>변환된 문자 열</returns>
		public static string ToAreaString(double arg, int exponent, int digit, bool replaceBalnk, char unit)
		{
			return NumberToRegular(arg, exponent, digit, replaceBalnk, unit, 6) + "\x00B2";
		}

		private static string NumberToRegular(double arg, int exponent, int digit, bool replaceBalnk, char unit, int tenPow)
		{
			Trace.Assert(digit > 0);

			string unitStr;
			if (unit == 0)
			{
				unitStr = "";
			}
			else
			{
				unitStr = unit.ToString();
			}

			if (arg == 0)
			{
				if (replaceBalnk)
				{
					return string.Format("{0:D" + digit.ToString() + "}", 0) + " " + unitStr;
				}
				else
				{
					return string.Format("{0:D" + digit.ToString() + "}", 0) + unitStr;
				}
			}

			bool minus = false;
			if (arg < 0)
			{
				minus = true;
				arg *= -1;
			}

			arg *= Math.Pow(10, exponent % tenPow);
			exponent -= exponent % tenPow;

			int tenJisu = (int)Math.Floor(Math.Log10(arg));
			arg = Math.Round(arg * Math.Pow(10, tenJisu * -1), digit - 1) * Math.Pow(10, tenJisu);
			tenJisu = (int)Math.Floor(Math.Log10(arg));

			int powValue;
			int flo;
			if (arg < 1)
			{
				if ((tenJisu % tenPow) == 0)
				{
					powValue = tenJisu;
					flo = digit - 1;
				}
				else
				{
					powValue = tenJisu - tenPow - tenJisu % tenPow;
					int temp = tenJisu % tenPow;
					flo = digit - 1 - (tenPow + temp);
				}
			}
			else
			{
				powValue = tenJisu - tenJisu % tenPow;
				flo = digit - 1 - tenJisu % tenPow;
			}
			if (flo < 0) { flo = 0; }
			arg *= Math.Pow(10, powValue * -1);

			string result = "?";

			int sw = powValue + exponent;
			if (sw == tenPow * -4) { result = "p"; }
			else if (sw == tenPow * -3) { result = "n"; }
			else if (sw == tenPow * -2) { result = "u"; }
			else if (sw == tenPow * -1) { result = "m"; }
			else if (sw == 0)
			{
				if (replaceBalnk) { result = " "; }
				else { result = ""; }
			}
			else if (sw == tenPow * 1) { result = "K"; }
			else if (sw == tenPow * 2) { result = "M"; }
			else if (sw == tenPow * 3) { result = "G"; }

			if (minus)
			{
				return "-" + arg.ToString("F" + flo.ToString()) + result + unitStr;
			}
			else
			{
				return arg.ToString("F" + flo.ToString()) + result + unitStr;
			}
		}

		/// <summary>
		/// 선택한 digit의 끝자리를 allowedLastNumber에 의해 결정된 숫자로 변환 한다.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="digit"></param>
		/// <param name="allowedLastNumber"></param>
		/// <returns></returns>
		[Obsolete("Use \"RegularPower\"",true)]
		public static int RegularLastDigit(int num, int digit, int[] allowedLastNumber)
		{
			int value = num;

			Trace.Assert(digit > 0);

			while (value >= Math.Pow(10, digit))
			{
				value /= 10;
			}

			int namuji = value % 10;

			Array.Sort(allowedLastNumber);

			int[] gap = new int[allowedLastNumber.Length + 1];

			for (int i = 0; i < gap.Length - 1; i++)
			{
				if (allowedLastNumber[i] > 10)
				{
					throw new ArgumentException();
				}
				gap[i] = Math.Abs(allowedLastNumber[i] - namuji);
			}

			if (allowedLastNumber[0] != 0)
			{
				gap[allowedLastNumber.Length] = Math.Abs(allowedLastNumber[0] * 10 - namuji);
			}
			else
			{
				gap[allowedLastNumber.Length] = 10;
			}

			int index = 0;

			for (int i = 1; i < gap.Length; i++)
			{
				if (gap[i] < gap[index]) { index = i; }
			}

			int result = (value / 10) * 10 + allowedLastNumber[index];

			if (index == gap.Length - 1)
			{
				result += 10;
			}

			while (num >= Math.Pow(10, digit))
			{
				num /= 10;
				result *= 10;
			}

			return result;
		}

		/// <summary>
		/// 선택한 앞의 자리수를 allowedNumber에 의해 정의된 수로 버림 변환 한다.
		/// </summary>
		/// <param name="num"></param>
		/// <param name="digit"></param>
		/// <param name="allowedNumber"></param>
		/// <returns></returns>
		[Obsolete("Use \"RegularPower\"", true)]
		public static int RegularAllDigit(int num, int digit, int[] allowedNumber)
		{
			// allowedNumber 검증.
			foreach (int i in allowedNumber)
			{
				if (i >= Math.Pow(10, digit))
				{
					throw new ArgumentException();
				}
				else if (i < 0)
				{
					throw new ArgumentException();
				}
			}

			double value = num;

			if (num < Math.Pow(10, digit))
			{
				while (value < Math.Pow(10, digit))
				{
					value *= 10;
				}
			}
			while (value >= Math.Pow(10, digit))
			{
				value /= 10;
			}

			//value *= 10;

			int target = (int)Math.Round(value);

			int seletecdNumber = allowedNumber.Length - 1;

			Array.Sort(allowedNumber);


			for (int i = allowedNumber.Length-1; i >= 0; i--)
			{
				if (target == allowedNumber[i])
				{
					break;
				}
				else if (target > allowedNumber[i])
				{
					break;
				}
				else
				{
					seletecdNumber--;
				}
			}



			if (seletecdNumber < 0)
			{
				throw new ArgumentException();
			}

			seletecdNumber = allowedNumber[seletecdNumber];


			if (num < Math.Pow(10, digit))
			{
				while (num < Math.Pow(10, digit))
				{
					num *= 10;
					seletecdNumber /= 10;
				}
			}
			while (num >= Math.Pow(10, digit))
			{
				num /= 10;
				seletecdNumber *= 10;
			}

			return seletecdNumber;
		}

		/// <summary>
		/// 10의 지수승으로 버림 연산
		/// </summary>
		/// <param name="mag">입력 값</param>
		/// <param name="digit">유효 자리 수</param>
		/// <param name="allowedNumber">사용 가능 숫자</param>
		/// <returns></returns>
		public static int RegularPower(int value, int digit, int[] allowedNumber)
		{
			// allowedNumber 검증.
			foreach (int i in allowedNumber)
			{
				if (i >= Math.Pow(10, digit))
				{
					throw new ArgumentException();
				}
				else if (i < Math.Pow(10, digit - 1))
				{
					throw new ArgumentException();
				}
			}

			// 'digit' 범위 안에 넣기.
			double magTemp = value;

			while (magTemp < Math.Pow(10, digit - 1))
			{
				magTemp *= 10;
			}
			while (magTemp >= Math.Pow(10, digit))
			{
				magTemp /= 10;
			}


			// 허용된 숫자 찾기
			int cnt;

			for (cnt = allowedNumber.Length-1; cnt >= 0; cnt--)
			{
				if (allowedNumber[cnt] <= magTemp)
				{
					break;
				}
			}

			// 실제 숫자보다 작은 허용된 숫자를 사용 하도록 함.
			// 허용된 숫자보다 작은 숫자면 제일 큰 허용된 숫자를 사용.
			if (cnt < 0) { cnt = allowedNumber.Length - 1; }

			magTemp = allowedNumber[cnt];

			if (magTemp > value)
			{
				while (magTemp > value)
				{
					magTemp /= 10;
				}
			}
			else
			{
				while (magTemp <= value)
				{
					magTemp *= 10;
				}
				magTemp /= 10;
			}

			return (int)magTemp;
		}

		/// <summary>
		/// 10의 지수승으로 버림 연산
		/// </summary>
		/// <param name="mag">입력 값</param>
		/// <param name="digit">유효 자리 수</param>
		/// <param name="allowedNumber">사용 가능 숫자</param>
		/// <returns></returns>
		public static int RegularPower(int mag, int digit)
		{
			// 'digit' 범위 안에 넣기.
			double magTemp = mag;

			int cnt = 0;

			while (magTemp < Math.Pow(10, digit - 1))
			{
				magTemp *= 10;
				cnt++;
			}
			while (magTemp > Math.Pow(10, digit))
			{
				magTemp /= 10;
				cnt++;
			}

			Math.Floor(magTemp);


			if (magTemp > mag)
			{
				while (cnt>0)
				{
					magTemp /= 10;
					cnt--;
				}
			}
			else
			{
				while (cnt>0)
				{
					magTemp *= 10;
					cnt--;
				}
				//magTemp /= 10;
			}

			return (int)magTemp;
		}

	}
}
