using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

/*
 *	Position :	최대 최소를 -1000 ~ 1000으로 설정
 *				Precision을 변경하여 거리 설정.
 */ 

namespace SEC.Nanoeye.NanoStage
{
	/// <summary>
	/// Motor 축
	/// </summary>
	public interface IAxis : SECtype.IValue
	{
		/// <summary>
		/// Speed. nm/sec
		/// </summary>
		SECtype.IControlLong Speed { get; }

		/// <summary>
		/// Current Position. nm
		/// </summary>
		IAxPosition Position { get; }

		/// <summary>
		/// Limit Channel. Number
		/// </summary>
		SECtype.IControlInt LimitChannel { get; }

		/// <summary>
		/// Accelation time. msec
		/// </summary>
		SECtype.IControlInt AccelTime { get; }

		/// <summary>
		/// Deaccelation time. msec
		/// </summary>
		SECtype.IControlInt StopTime { get; }

		/// <summary>
		/// Emeragency deaccelation time. msec
		/// </summary>
		SECtype.IControlInt EmergencyStopTime { get; }

		/// <summary>
		/// 동작 중인지 여부
		/// </summary>
		bool IsMotion { get; }

		/// <summary>
		/// 진행 방항을 역방향으로 설정한다.
		/// </summary>
		bool IsDirectionCCW { get; set; }

		/// <summary>
		/// Home의 위치
		/// </summary>
		long HomePosition { get; set; }

		/// <summary>
		/// 무한 회적축인지 여부
		/// </summary>
		bool IsEndlessMove { get; set; }

		/// <summary>
		/// LimitSensor 상태
		/// </summary>
		LimitSensorEnum LimitState { get; }

		/// <summary>
		/// limit state change event
		/// </summary>
		event EventHandler LimitStateChanged;

		/// <summary>
		/// 동작 상태가 변경 됨.
		/// </summary>
		event EventHandler MotionStateChanged;

		/// <summary>
		/// Move axis uniform motion
		/// </summary>
		/// <param name="direction"></param>
		void MoveVelocity(bool direction);

		/// <summary>
		/// Stop axis
		/// </summary>
		/// <param name="emergency"></param>
		void Stop(bool emergency);

		/// <summary>
		/// Step이나 Servo Motor의 1회전당 필요한 Pulse 수.
		/// DC Motor의 경우 필요 없음.
		/// </summary>
		int Resolution { get; set; }

		/// <summary>
		/// 1회전당 이동 거리. nm
		/// </summary>
		int StepDistance { get; set; }

		/// <summary>
		/// 움직임이 종료되면 자동으로 축을 Off시킬지를 결정한다.
		/// </summary>
		bool AutoOff { get; set; }

		
	}
}
