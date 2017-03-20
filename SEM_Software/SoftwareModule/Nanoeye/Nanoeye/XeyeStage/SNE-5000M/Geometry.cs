using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace SEC.Nanoeye.XeyeStage.SNE_5000M
{
	public class Geometry : SEC.Nanoeye.XeyeStage.Geometry
	{
		#region Property & Variables
		private XeyeStage.SNE_5000M.Motion	m_pMotion;

		private double _DoorXstart = double.MaxValue;
		public double DoorXstart
		{
			get { return _DoorXstart; }
			set { _DoorXstart = value; }
		}

		private double _DoorYstart = double.MinValue;
		public double DoorYstart
		{
			get { return _DoorYstart; }
			set { _DoorYstart = value; }
		}

		private double _ColumnAngle = 60d;
		public double ColumnAngle
		{
			get { return _ColumnAngle; }
			set { _ColumnAngle = value; }
		}


		private double _TiltCenterDistance = 10;
		public double TiltCenterDistance
		{
			get { return _TiltCenterDistance; }
			set { _TiltCenterDistance = value; }
		}

		private double _StageDiameter = 10;
		public double StateDiameter
		{
			get { return _StageDiameter; }
			set { _StageDiameter = value; }
		}

		private double _SampleDiameter = 10;
		public double SampleDiameter
		{
			get { return _SampleDiameter; }
			set { _SampleDiameter = value; }
		}

		private double _SampleHeight = 10;
		public double SampleHeight
		{
			get { return _SampleHeight; }
			set { _SampleHeight = value; }
		}


		// Column 설정.
		private double	m_dColumnVertexX = 0.0;	// 0
		private double	m_dColumnVertexY = 0.0;	// 0
		private double	m_dColumnVertexZ = -60.0;
		private double	m_dColumnSlope	= 30;

		// Stage 설정.
		private double	m_dStageW = 40;
		private double	m_dStageH = 20;

		// Door 설정.
		private double	m_dDoorX = 35;
		private double	m_dDoorY = -35	;

		// Object 설정
		private double	m_dObjectW = 40;
		private double	m_dObjectH = 20;

		



		#endregion


		public override double GetPosRTLimit(int nAxis)
		{
			if (m_pMotion == null)
			{
				m_pMotion = SNE_5000M.Mediator.Instance.GetMotion() as XeyeStage.SNE_5000M.Motion;
			}

			double dValue = m_pMotion.GetPositiveSWLimit(nAxis);

			//double dFrontZ , dBackZ;
			//_GetProjectionZ(out dFrontZ, out dBackZ);

			//double dFrontY , dBackY;
			//_GetProjectionY(out dFrontY, out dBackY);


			//switch (nAxis)
			//{
			//case (int)Mediator.AxisNumber.X:
			//    {
			//        double dTPos = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.T);

			//        double dStageZ = _GetStageZByY(m_dColumnVertexY, dTPos, dFrontY, dBackY, dFrontZ, dBackZ);

			//        double czyF = _GetColumnZbyY(dFrontY);
			//        double czyB = _GetColumnZbyY(dBackY);

			//        System.Diagnostics.Debug.WriteLine("(" + dFrontY.ToString() + "," + dBackY.ToString() + ") = ");

			//        // 충돌 발생.
			//        if (dFrontZ < czyF)
			//        {
			//            dValue = Math.Min(dValue, -_GetDistOfColumn(dFrontZ) - m_dObjectW/2.0d);

			//            System.Diagnostics.Debug.WriteLine("(Front 충돌 : " + (-_GetDistOfColumn(dFrontZ)).ToString());
			//        }
			//        if (dBackZ < czyB)
			//        {
			//            dValue = Math.Min(dValue, -_GetDistOfColumn(dBackZ) - m_dObjectW / 2.0d);
			//            System.Diagnostics.Debug.WriteLine("(back 충돌 : " + (-_GetDistOfColumn(dBackZ)).ToString());
			//        }
			//        if(dStageZ < m_dColumnVertexZ)
			//        {
			//            dValue = Math.Min(dValue, -_GetDistOfColumn(dStageZ) - m_dObjectW / 2.0d);
			//            System.Diagnostics.Debug.WriteLine("(Vertex 충돌 : " + (-_GetDistOfColumn(dStageZ)).ToString());
			//        }

			//    }
			//    break;

			//case (int)Mediator.AxisNumber.Y:
			//    {
			//        double dTPos = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.T);
			//        if (Math.Min(dFrontZ, dBackZ) < m_dColumnVertexZ)
			//        {
			//            // 스테이지 상단과 충돌.
			//            if (dTPos < 0)
			//            {
			//                // Front Z 에서 충돌 체크.

			//                // Back Z / ColumnVertexZ 충돌 체크.
			//                double dZ = Math.Min(dBackZ, m_dColumnVertexZ);


			//            }
			//            // Stage 측면과 충돌.
			//            else
			//            {
			//                // 스테이지의 끝면과 충돌.

			//                // 컬럼의 꼭지점과 충돌.
			//            }
			//        }
			//    }
			//    break;


			//case (int)Mediator.AxisNumber.T:
			//    {
			//    }
			//    break;
			//}
			return dValue;
		}

		public override double GetNegRTLimit(int nAxis)
		{
			if (m_pMotion == null)
			{
				m_pMotion = SNE_5000M.Mediator.Instance.GetMotion() as XeyeStage.SNE_5000M.Motion;
			}

			double dValue = m_pMotion.GetNegativeSWLimit(nAxis);
			

			//double dFrontZ , dBackZ;
			//_GetProjectionZ(out dFrontZ, out dBackZ);

			//double dFrontY , dBackY;
			//_GetProjectionY(out dFrontY, out dBackY);




			//switch (nAxis)
			//{
			//case (int)Mediator.AxisNumber.X:
			//    //Column 
			//    break;
			//case (int)Mediator.AxisNumber.Y:
			//    //Door 
			//    break;
			//case (int)Mediator.AxisNumber.T:
			//    // Column
			//    break;
			//case (int)Mediator.AxisNumber.Z:
			//    // Column
			//    //double dTPos = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.T);
			//    //double dZPos = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.Z);
			//    //double dStageZ = _GetStageZByY(m_dColumnVertexY, dTPos, dFrontY, dBackY, dFrontZ, dBackZ);



			//    //double dGapFront = dFrontZ - _GetColumnZbyY(dFrontY);
			//    //double dGapBack = dBackZ - _GetColumnZbyY(dBackY);
			//    //double dGapVertex = dStageZ - m_dColumnVertexZ;

			//    //System.Diagnostics.Debug.WriteLine("(" + dFrontY.ToString() + "," + dBackY.ToString() + ") = " + "Front : " + dGapFront.ToString() + " / Back : " + dGapBack.ToString() + " / Vertex : " + dGapVertex.ToString());



			//    //double dOffset = Math.Min(dGapFront, Math.Min(dGapBack, dGapVertex));

			//    //dValue = Math.Max(dValue, (dZPos - dOffset));

			//    break;
			//}
			return dValue;
		}

		// STage 의 좌표를 돌려준다.
		private void _GetProjectionY(out double dFront, out double dBack)
		{

			double dY = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.Y);
			double dT = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.T);

			double ProjectionY =  (m_dStageH + m_dObjectH) * Math.Sin(DegreeToRadian(dT));

			dFront	= m_dColumnVertexY + dY + ProjectionY - (m_dStageW/2.0d * Math.Cos(DegreeToRadian(dT)));
			dBack	= m_dColumnVertexY + dY + ProjectionY + (m_dStageW / 2.0d * Math.Cos(DegreeToRadian(dT)));
		}

		private void _GetProjectionZ(out double dFront, out double dBack)
		{
			double dZ = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.Z);
			double dT = m_pMotion.GetCmdPosition((int)Mediator.AxisNumber.T);

			double ProjectionZ =  -((m_dStageH + m_dObjectH) * Math.Cos(DegreeToRadian(dT)));

			dFront	= dZ + ProjectionZ - (m_dStageW / 2.0d * Math.Sin(DegreeToRadian(dT)));
			dBack	= dZ + ProjectionZ + (m_dStageW / 2.0d * Math.Sin(DegreeToRadian(dT)));
		}

		// Column 의 좌표를 돌려준다.
		private double _GetColumnZbyX(double dX)
		{
			double dDist = Math.Abs(m_dColumnVertexX - dX);
			return _GetZPosOfColumn(dDist);
		}


		private double _GetColumnZbyY(double dY)
		{
			double dDist = Math.Abs(m_dColumnVertexY - dY);
			return _GetZPosOfColumn(dDist);
		}


		private double _GetZPosOfColumn(double dDist)
		{
			return m_dColumnVertexZ - (Math.Tan(DegreeToRadian( m_dColumnSlope ))*  dDist);
		}



		private double _GetDistOfColumn(double dZPos)
		{
			if(dZPos > m_dColumnVertexZ )
			{
				throw new Exception();
			}
			return Math.Abs(m_dColumnVertexZ - dZPos) / Math.Tan(DegreeToRadian(m_dColumnSlope));
		}



		private double _GetStageZByY(double dY, double dCurT, double dFrontY, double dBackY, double dFrontZ, double dBackZ)
		{
			//if (dCurT == 0 || dCurT == -90)
			//{
			//    // 수평 또는 수직으로 서있는 경우는 FrontZ의 값을 가짐.
			//    return dFrontZ ;
			//}


			double dRet = 0.0;
			double dSlope = (dBackZ - dFrontZ) / (dBackY - dFrontY);

			if (dCurT > 0)
			{
				if (dY > dBackY)
				{
					dRet = 0.0;
				}
				else if ( (dY >= dFrontY) && (dY <= dBackY) )
				{
					dRet = dFrontZ + dSlope * (dY - dFrontY);
				}
				else
				{
					// 측면은 상단과 직각을 이루고 있다.
					dSlope = -1.0 / dSlope;
					dRet = dFrontZ - dSlope * (dFrontY - dY);
				}
			}
			else
			{
				if (dY > dBackY)
				{
					// 측면은 상단과 직각을 이루고 있다.
					dSlope = -1.0 / dSlope;
					dRet = dBackZ + dSlope * (dY - dBackY);
				}
				else if (dY >= dFrontY && dY <= dBackY)
				{
					dRet = dFrontZ + dSlope * (dY - dFrontY);
				}
				else
				{
					dRet = 0.0;
				}
			}

			return dRet;
		}

		private bool InRange(double v, double minV, double maxV)
		{
			return ((v >= minV) && (v <= maxV));
		}

		double DegreeToRadian(double d)	
		{
			return d * (Math.PI/180d);
		}

	}
}
