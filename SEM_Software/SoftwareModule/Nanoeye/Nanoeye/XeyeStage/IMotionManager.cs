using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public interface IMotionManager : IDisposable
	{
		bool InitBoard(int nTotalBoardNo);
		void ExitBoard();
		bool IsInitialized();
		void GetErrorMessage(string szMsg);

		bool AddAxis(AXIS_PARAM param);
		IAxis GetAxis(int nAxis);
		int GetAxesCount(bool bWithUnmanaged);
		int GetAxesCount();
		int GetMaxAxes();

		void Stop();	// 모든 축의 모션을 정지


		// ---------------------------------------------------------------
		// 다축제어시에는 속도에 대한 전자 기어비를 어떤 축에 대한 값을 적용할 것인지 정해져 있지 않다.
		// 이때, nGearAxis에 지정된 축의 전자 기어비를 사용한다.


		// ---------------------------------------------------------------
		//						Combine two axes
		// ---------------------------------------------------------------

		void SplineMove
		   (int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis);
		void ArcSplineMove
		   (int ax1, int ax2, double pos1, double pos2, double XCenter, double YCenter, double dSpeed, int nAccel, int nStopRate, int nCW, int nGearAxis);
		void Move2Axes
		   (int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis);



		// ---------------------------------------------------------------
		//					Combine multi axes more than two axes
		// ---------------------------------------------------------------
		void SplineMove
		   (int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nAxis);
		void ArcSplineMove
		   (int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, double dXCenter, double dYCenter, int nStopRate, int nCW, int nGearAxis);
		void MultiMove
		   (int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nGearAxis);


		// ---------------------------------------------------------------
		//					Synchronization of Axes
		// ---------------------------------------------------------------
		bool GetSyncControl();
		void SetSyncControl(bool bUse);
		void SetSyncMap(int nMasterAxis, int nSlaveAxis);
		void SetSyncControl_AX(int master_ax, int slave_ax, bool bEnable);
		void Release();
	}
}
