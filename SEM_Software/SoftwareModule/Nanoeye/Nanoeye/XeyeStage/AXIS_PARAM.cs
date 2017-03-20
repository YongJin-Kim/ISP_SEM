using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public class AXIS_PARAM
	{
		public AXIS_PARAM()
		{
			nAxis = 0;		// 논리 축번호	
			nBoardNo = 0;		// 보드 번호
			nPhysicalAxis = 0;		// 물리 축번호
			nSyncWith = 0;		// 타 축과 싱크 관계에 있는 경우
			dEGearRatio = 1;		// 전자 기어비
			dPositiveSWLimit = 100;
			dNegativeSWLimit = 0;
			nStopRate = 10;
			nAccel = 10;
			bManaged = true;		//관리축 (홈서치등이 필요 )
			bInverseDir = false;
			dInitSpeed = 10;
			dInitOffset = 10;
			dSpeed = 10;
			dOffset = 10;
			bUseSpeedPPS = true;

			dDistNegToHomeSensor = 10;
			dHomeSpeed1 = 10.0;
			dHomeSpeed2 = 0.1;
			dHomeOffset = 0.1;
			bHomeSearchWithNegSensor = false;
			min_speed = 0.01;
			max_speed = 100;
		}
		public string szName { get; set; }
		public short nAxis { get; set; }				// 논리 축번호	
		public short nBoardNo { get; set; }			// 보드 번호
		public short nPhysicalAxis { get; set; }		// 물리 축번호
		public short nSyncWith { get; set; }			// 타 축과 싱크 관계에 있는 경우
		public double dEGearRatio { get; set; }		// 전자 기어비
		public double dPositiveSWLimit { get; set; }
		public double dNegativeSWLimit { get; set; }
		public short nStopRate { get; set; }
		public short nAccel { get; set; }
		public bool bManaged { get; set; }			//관리축 (홈서치등이 필요 )
		public bool bInverseDir { get; set; }
		public double dInitSpeed { get; set; }
		public double dInitOffset { get; set; }
		public double dSpeed { get; set; }
		public double dOffset { get; set; }
		public bool bUseSpeedPPS { get; set; }

		// Home Searching 과관련된 멤버
		public double dDistNegToHomeSensor { get; set; }
		public double dHomeSpeed1 { get; set; }
		public double dHomeSpeed2 { get; set; }
		public double dHomeOffset { get; set; }
		public bool bHomeSearchWithNegSensor { get; set; }
		public double min_speed { get; set; }
		public double max_speed { get; set; }
	}
}
