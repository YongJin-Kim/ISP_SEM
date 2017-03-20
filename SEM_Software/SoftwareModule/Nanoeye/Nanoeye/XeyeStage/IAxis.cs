using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public interface IAxis
	{
		short GetAxis();
		void GetTextName(out string szName);
		void SetParam(AXIS_PARAM pParam);
		AXIS_PARAM GetParam();



		void MovePosition(double dPos, double dSpeed);
		void MoveOffset(double dOffset, double dSpeed);
		void MoveVelocity(double dSpeed);

		void MovePosition(double dPos);
		void MoveOffset(double dOffset);
		void MoveVelocity(bool bPos);


		 void Stop(bool bEmergencyStop, short nStopRate);
		void Stop(bool bEmergencyStop);


		 bool IsAxisDone();
		 bool IsMotionDone();
		 bool IsInPosition();
		 void AxisClear();
		 int AxisSource();
		 int AxisStatus();
		 int FrameLeft();
		 void FrameClear();
		 void AmpFaultReset();
		 bool IsAmpFault();


		 bool IsMoveable(out MOTION_ERR_CODE code);


		 void SetHomeEvent(bool bStopEvent);


		 void ReleaseSWLimit();
		 void SetSWLimit(double dPosLimit, double dNegLimit);
		 void GetSWLimit(out double dPosLimit, out double dNegLimit);
		 double GetPositiveSWLimit();
		 double GetNegativeSWLimit();
		 bool IsAmpOn();
		 void SetAmp(bool bActive);

		 double GetCmdPosition();
		 void SetCmdPosition(double dPos);

		 double GetCurPosition();
		 void SetCurPosition(double dPos);

		 void SetSpeed(double dSpeed);
		 void SetOffset(double dOffet);

		// ---------------------------------------------------------------
		//					Check the hardware and software limit
		// ---------------------------------------------------------------
		 bool IsHomeSwitch();			// Home Switch
		 bool IsSwNegLimit();			// Software limit
		 bool IsSwPosLimit();			// Software limit
		 bool IsHwPosLimit();			// Hardware limit
		 bool IsHwNegLimit();			// Hardware limit
	}

	public enum MOTION_ERR_CODE
	{
		MOTION_NO_ERROR,
		MOTION_AMP_OFF,
		MOTION_AMP_FAULT,
		MOTION_NEED_HOME_SEARCH,
		MOTION_LIMIT_CHECK,
		MOTION_DONE,
		MOTION_ERR_UNKNOWN
	};

}
