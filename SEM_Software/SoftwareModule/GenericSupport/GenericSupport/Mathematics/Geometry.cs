using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SEC.GenericSupport.Mathematics
{
	public class Geometry
	{
		/// <summary>
		/// 점과 직선 사이의 거리를 구한다.
		/// 직선의 방정식 : argA * X + argB * Y + argC = 0;
		/// </summary>
		/// <param name="argA"></param>
		/// <param name="argB"></param>
		/// <param name="argC"></param>
		/// <param name="pnt"></param>
		/// <returns></returns>
		public static double DistanceBetweenLineAndPoint(double argA, double argB, double argC, Point pnt)
		{
			return Math.Abs(argA * pnt.X + argB * pnt.Y + argC) / Math.Sqrt(Math.Pow(argA, 2) + Math.Pow(argB, 2));
		}

		/// <summary>
		/// 점과 직선 사이의 거리를 구한다.
		/// 직선의 방정식 : argA * X + argB * Y + argC = 0;
		/// </summary>
		/// <param name="argA"></param>
		/// <param name="argB"></param>
		/// <param name="argC"></param>
		/// <param name="pnt"></param>
		/// <returns></returns>
		public static double DistanceBetweenLineAndPoint(double argA, double argB, double argC, PointF pnt)
		{
			return Math.Abs(argA * pnt.X + argB * pnt.Y + argC) / Math.Sqrt(Math.Pow(argA, 2) + Math.Pow(argB, 2));
		}

		/// <summary>
		/// 두 점 사이의 거리를 구한다.
		/// </summary>
		/// <param name="pnt1"></param>
		/// <param name="pnt2"></param>
		/// <returns></returns>
		public static double DistanceBetweenPointAndPoint(Point pnt1, Point pnt2)
		{
			return Math.Sqrt(Math.Pow(pnt1.X - pnt2.X, 2) + Math.Pow(pnt1.Y - pnt2.Y, 2));
		}

		/// <summary>
		/// 두 점 사이의 거리를 구한다.
		/// </summary>
		/// <param name="pnt1"></param>
		/// <param name="pnt2"></param>
		/// <returns></returns>
		public static double DistanceBetweenPointAndPoint(PointF pnt1, PointF pnt2)
		{
			return Math.Sqrt(Math.Pow(pnt1.X - pnt2.X, 2) + Math.Pow(pnt1.Y - pnt2.Y, 2));
		}

		/// <summary>
		/// 두개의 점을 이용하여 1차 방적식의 해를 구한다.
		/// f(x) : Y = argA * X + argB
		/// </summary>
		/// <param name="pnt1"></param>
		/// <param name="pnt2"></param>
		/// <param name="argA"></param>
		/// <param name="argB"></param>
		public static void GetEquation1D(Point pnt1, Point pnt2, out double argA, out double argB)
		{
			argA = (pnt1.Y - pnt2.Y) / (double)(pnt1.X - pnt2.X);
			argB = pnt1.Y - pnt1.X * argA;
		}

		/// <summary>
		/// 점에서 직선으로 수직으로 가상의 선을 그렸을 때, 직선과 가산의 선이 만나는 지점을 구한다.
		/// </summary>
		/// <param name="argA"></param>
		/// <param name="argB"></param>
		/// <param name="argC"></param>
		/// <param name="pnt"></param>
		/// <returns></returns>
		public static Point OrthogonalPointBeteweenLineAndPoint(double argA, double argB, double argC, Point pnt)
		{
				double argOriA, argOriB, argCorA, argCorB;
				Point pntCross = Point.Empty;

				argOriA = -argA / argB;
				argOriB = -argC / argB;

				argCorA = -1 / argOriA;
				argCorB = pnt.Y - pnt.X * argCorA;

				double crsX = (argOriB - argCorB) / (argCorA - argOriA);
				pntCross.X = (int)(crsX);
				pntCross.Y = (int)(crsX * argOriA + argOriB);

				return pntCross;
		}

		/// <summary>
		/// 점과 직선이 수직으로 만나는 직선의 방정식을 구한다.
		/// f(x) : Y = argA * X + argB
		/// </summary>
		/// <param name="argOriA"></param>
		/// <param name="argOriB"></param>
		/// <param name="pnt"></param>
		/// <param name="argA"></param>
		/// <param name="argB"></param>
		public static void OrthogonalLineBeteweenLineAndPoint(double argOriA, double argOriB, Point pnt, out double argA, out double argB)
		{
			argA = -1 / argOriA;
			argB = pnt.Y - pnt.X * argA;
		}
	}
}
