using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMCval =   SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection ;

namespace SEC.Nanoeye.XeyeStage
{
	public class MMCAxis : IAxis
	{
		#region Property & Variables
		AXIS_PARAM	m_param;
		#endregion

		#region 상태 체크
		public bool IsMotionDone()
		{
			return (MMCval.ActionState.motion_done(m_param.nPhysicalAxis) != 0) ? true : false;
		}

		public bool IsAxisDone()
		{
			return (MMCval.ActionState.axis_done(m_param.nPhysicalAxis) != 0) ? true : false;
		}

		public bool IsInPosition()
		{
			return (MMCval.ActionState.in_position(m_param.nPhysicalAxis) != 0) ? true : false;
		}

		public bool IsAmpFault()
		{
			return (MMCval.SensorState.amp_fault_switch(m_param.nPhysicalAxis) != 0) ? true : false;
		}

		public bool IsAmpOn()
		{
			short state = 0;
			MMCval.AmpControl.get_amp_enable(m_param.nPhysicalAxis, ref state);
			return (state != 0) ? true : false;
		}

		public bool IsMoveable(out MOTION_ERR_CODE code)
		{
			code = MOTION_ERR_CODE.MOTION_NO_ERROR;

			if (!IsMotionDone()) { code = MOTION_ERR_CODE.MOTION_DONE; }
			else if (IsAmpFault()) { code = MOTION_ERR_CODE.MOTION_AMP_FAULT; }
			else if (!IsAmpOn()) { code = MOTION_ERR_CODE.MOTION_AMP_OFF; }

			return (code == MOTION_ERR_CODE.MOTION_NO_ERROR);
		}

		public bool IsHomeSwitch()			// Home Switch
		{
			return ((MMCval.ControlState.axis_source(m_param.nPhysicalAxis) & (short)MMCval.Enumurations.EventSourceStatus.HomeSwitch) != 0) ? true : false;
		}

		public bool IsSwPosLimit()		// Software limit
		{
			return ((MMCval.ControlState.axis_source(m_param.nPhysicalAxis) & (short)MMCval.Enumurations.EventSourceStatus.XPosLimit) != 0) ? true : false;
		}

		public bool IsSwNegLimit()			// Software limit
		{
			return ((MMCval.ControlState.axis_source(m_param.nPhysicalAxis) & (short)MMCval.Enumurations.EventSourceStatus.XNegLimit) != 0) ? true : false;
		}

		public bool IsHwPosLimit()			// Hardware limit
		{
			return ((MMCval.ControlState.axis_source(m_param.nPhysicalAxis) & (short)MMCval.Enumurations.EventSourceStatus.PosLimit) != 0) ? true : false;
		}

		public bool IsHwNegLimit()			// Hardware limit
		{
			return ((MMCval.ControlState.axis_source(m_param.nPhysicalAxis) & (short)MMCval.Enumurations.EventSourceStatus.NegLimit) != 0) ? true : false;
		}
		#endregion

		public MMCAxis(AXIS_PARAM pParam)
		{
			SetParam(pParam);
		}

		public short GetAxis()
		{
			return m_param.nAxis;
		}

		public void GetTextName(out string szName)
		{
			szName = m_param.szName;
		}

		public void SetParam(AXIS_PARAM pParam)
		{
			m_param = pParam;
			MMCval.StopAction.set_stop_rate(m_param.nPhysicalAxis, m_param.nStopRate);
			MMCval.AxisConfig.set_coordinate_direction(pParam.nPhysicalAxis, pParam.bInverseDir ? (short)MMCval.Enumurations.Coordinate.CCW : (short)MMCval.Enumurations.Coordinate.CW);
		}

		public AXIS_PARAM GetParam()
		{
			return m_param;
		}

		public void SetHomeEvent(bool bStop)
		{
			MMCval.LimitsOfHardware.set_home(m_param.nPhysicalAxis, (short)(bStop ? MMCval.Enumurations.EventNumber.Stop : MMCval.Enumurations.EventNumber.No));
		}

		#region Move
		public void MovePosition(double dPos, double dSpeed)
		{
			dSpeed = Math.Min(dSpeed, m_param.max_speed);
			dSpeed = Math.Max(dSpeed, m_param.min_speed);
			MMCval.OneAxisMoveTrapezoid.start_move(m_param.nPhysicalAxis, dPos * m_param.dEGearRatio, dSpeed * m_param.dEGearRatio, m_param.nAccel);
		}

		public void MoveOffset(double dOffset, double dSpeed)
		{
			dSpeed = Math.Min(dSpeed, m_param.max_speed);
			dSpeed = Math.Max(dSpeed, m_param.min_speed);
			MMCval.OneAxisMoveTrapezoid.start_r_move(m_param.nPhysicalAxis, dOffset * m_param.dEGearRatio, dSpeed * m_param.dEGearRatio, m_param.nAccel);
		}

		public void MoveVelocity(double dSpeed)
		{
			if (dSpeed != 0)
			{
				bool bNegative = (dSpeed < 0);

				dSpeed *= bNegative ? -1 : 1.0;
				dSpeed = Math.Min(dSpeed, m_param.max_speed);
				dSpeed = Math.Max(dSpeed, m_param.min_speed);

				if (bNegative)
					dSpeed *= -1;
			}
			MMCval.VelocityMove.v_move(m_param.nPhysicalAxis, dSpeed * m_param.dEGearRatio, m_param.nAccel);
		}

		public void MovePosition(double dPos)
		{
			MovePosition(dPos, m_param.dSpeed);
		}

		public void MoveOffset(double dOffset)
		{
			MoveOffset(dOffset, m_param.dSpeed);
		}

		public void MoveVelocity(bool bPos)
		{
			MoveVelocity(m_param.dSpeed * (bPos ? 1 : -1));
		}
		#endregion

		public void Stop(bool bEmergencyStop, short nStopRate)
		{
			if (nStopRate == -1)
				MMCval.StopAction.set_stop_rate(m_param.nPhysicalAxis, m_param.nStopRate);
			else
				MMCval.StopAction.set_stop_rate(m_param.nPhysicalAxis, nStopRate);



			if (bEmergencyStop)
			{
				MMCval.StopEmergenceAction.set_e_stop(m_param.nPhysicalAxis);
			}
			else
			{
				MMCval.StopAction.set_stop(m_param.nPhysicalAxis);
			}

//			FrameClear();
			do
			{
				//MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
				System.Threading.Thread.Sleep(10);
			} while (MMCval.ActionState.motion_done(m_param.nPhysicalAxis) != 1);
			System.Threading.Thread.Sleep(10);
			MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
		}

		public void Stop(bool bEmergencyStop)
		{
			Stop(bEmergencyStop, m_param.nStopRate);
		}

		public void AxisClear()
		{
			MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
		}

		public int AxisSource()
		{
			return MMCval.ControlState.axis_source(m_param.nPhysicalAxis);
		}

		public int AxisStatus()
		{
			return MMCval.ControlState.axis_state(m_param.nPhysicalAxis);
		}
		
		public int FrameLeft()
		{
			return MMCval.ControlState.frames_left(m_param.nPhysicalAxis);
		}

		public void FrameClear()
		{
			MMCval.ControlState.frames_clear(m_param.nPhysicalAxis);
		}

		#region Amp
		public void AmpFaultReset()
		{
			int nCount = 0;

			while (nCount++ > 5)
			{
				MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
				MMCval.AmpFault.amp_fault_set(m_param.nPhysicalAxis);
				System.Threading.Thread.Sleep(30);
				MMCval.AmpFault.amp_fault_reset(m_param.nPhysicalAxis);
				System.Threading.Thread.Sleep(100);
				MMCval.AmpFault.amp_fault_set(m_param.nPhysicalAxis);
				if (!IsAmpFault())
					break;
			}
		}

		public void SetAmp(bool bActive)
		{
			if (bActive)
			{
				// axis 상태를 Clear 한 뒤에 Amp를 켠다.
				AmpFaultReset();
				MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
				MMCval.AmpControl.set_amp_enable(m_param.nPhysicalAxis, 1);
			}
			else
			{
				// Amp를 끄고 Axis 상태를 Clear한다.
				AmpFaultReset();
				MMCval.AmpControl.set_amp_enable(m_param.nPhysicalAxis, 0);
				MMCval.ControlState.clear_status(m_param.nPhysicalAxis);
			}
			System.Threading.Thread.Sleep(10);
		}
		#endregion

		#region SW Limit
		public void ReleaseSWLimit()
		{
			MMCval.LimitsOfSotware.set_positive_sw_limit(m_param.nPhysicalAxis, 2147483000d, (short)MMCval.Enumurations.EventNumber.No);
			MMCval.LimitsOfSotware.set_negative_sw_limit(m_param.nPhysicalAxis, -2147483000d, (short)MMCval.Enumurations.EventNumber.No);
		}

		public void SetSWLimit(double dPosLimit, double dNegLimit)
		{
			MMCval.LimitsOfSotware.set_positive_sw_limit(m_param.nPhysicalAxis, dPosLimit * m_param.dEGearRatio, (short)MMCval.Enumurations.EventNumber.Stop);
			MMCval.LimitsOfSotware.set_negative_sw_limit(m_param.nPhysicalAxis, dNegLimit * m_param.dEGearRatio, (short)MMCval.Enumurations.EventNumber.Stop);
		}

		public void GetSWLimit(out double dPosLimit, out  double dNegLimit)
		{
			dPosLimit = m_param.dPositiveSWLimit;
			dNegLimit = m_param.dNegativeSWLimit;
		}

		public double GetPositiveSWLimit()
		{
			return m_param.dPositiveSWLimit;
		}

		public double GetNegativeSWLimit()
		{
			return m_param.dNegativeSWLimit;
		}
		#endregion

		#region Position
		public double GetCmdPosition()
		{
			double dPos = 0;
			MMCval.PositionValue.get_command(m_param.nPhysicalAxis, ref dPos);
			return dPos /= m_param.dEGearRatio;
		}

		public void SetCmdPosition(double dPos)
		{
			MMCval.PositionValue.set_command(m_param.nPhysicalAxis, dPos * m_param.dEGearRatio);
		}

		public double GetCurPosition()
		{
			double dPos = 0;
			MMCval.PositionValue.get_position(m_param.nPhysicalAxis, ref dPos);
			return dPos /= m_param.dEGearRatio;
		}

		public void SetCurPosition(double dPos)
		{
			MMCval.PositionValue.set_position(m_param.nPhysicalAxis, dPos);
		}
		#endregion

		public void SetSpeed(double dSpeed)
		{
			m_param.dSpeed = dSpeed;

		}

		public void SetOffset(double dOffset)
		{
			m_param.dOffset = dOffset;
		}
	}
}
