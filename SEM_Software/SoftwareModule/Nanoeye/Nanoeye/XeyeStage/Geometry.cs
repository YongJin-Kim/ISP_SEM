using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public abstract class Geometry
	{
		protected	double m_object_height;

		public Geometry()
		{
			m_object_height = 0d;
		}

		public bool IsCamAvailable() { return true; }
		public bool IsXRayAvailable() { return true; }

		/// <summary>
		/// 각 모델은 객체 높이를 계산하는 방법이 모두 다르다. 
		/// </summary>
		/// <param name="start_Y"></param>
		/// <param name="end_Y"></param>
		/// <returns></returns>
		public double CalcObjectHeight(double start_Y, double end_Y) { return 0.0; }

		/// <summary>
		/// 개체 높이 계산에 사용될 축을 리턴한다. 
		/// ex> SF160A(Y-AFT) , SF160B (Y)
		/// </summary>
		/// <returns></returns>
		public int GetAxisToAdjustObjectHeight() { return 0; }

		public virtual double GetPosRTLimit(int nAxis) { 
			return 0; }

		public virtual double GetNegRTLimit(int nAxis) { return 0; }

		void SetObjectHeight(double height) { m_object_height = height; }
		double GetObjectHeight() { return m_object_height; }
	}
}
