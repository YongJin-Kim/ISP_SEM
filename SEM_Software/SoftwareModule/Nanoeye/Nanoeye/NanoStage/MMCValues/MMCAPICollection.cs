using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using System.Security.Permissions;

[assembly: SecurityPermission(
   SecurityAction.RequestMinimum, Execution = true)]

namespace SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection
{
#pragma warning disable
	/// <summary>
	/// 초기화
	/// </summary>
	sealed public class Initialize
	{
		private Initialize() { }
		/// <summary>
		/// MMC 보드 및 Parameter 초기화
		/// </summary>
		/// <param name="totalbdnum">사용할 총 보드의 수</param>
		/// <param name="addr">보드의 주소</param>
		/// <returns>Error Return</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short mmc_initx(short totalbdnum, ref int addr);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_dpram_addr(short bdNum, ref int addr);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_dpram_addr(short bdNum, int addr);
	}

	/// <summary>
	/// Error 처리
	/// </summary>
	sealed public class ErrorControl
	{
		private ErrorControl() { }
		/// <summary>
		/// Error Message를 dst 버퍼에 copy
		/// </summary>
		/// <param name="code">Error code</param>
		/// <param name="det">string buffer</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short error_message(short code, StringBuilder dst);
		/// <summary>
		/// Error Message의 어드레스를 돌려준다.
		/// </summary>
		/// <param name="code">error code</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern StringBuilder _error_message(short code);
		/// <summary>
		/// Windows 프로그램에서 mmc_error를 읽을때 사용
		/// </summary>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_mmc_error();

		public static void Assert(short error)
		{
			if (error == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder((int)(Enumurations.ErrorNumber.MaxErrorLen));
			MMCAPICollection.ErrorControl.error_message(error, sb);
			//Trace.WriteLine(sb.ToString(), "Manager");
			//Trace.TraceError(sb.ToString());
			//Trace.Fail(sb.ToString());
			//throw new System.IO.IOException(sb.ToString());
			System.Diagnostics.Trace.Fail(sb.ToString());
		}
	}

	/// <summary>
	/// 단축이동
	/// </summary>
	sealed public class OneAxisMoveTrapezoid
	{
		private OneAxisMoveTrapezoid() { }
		/// <summary>
		/// 사다리꼴 속도 Profile로 지정축 이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// 사다리꼴 속도 Profile로 이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// 사다리꼴 속도 Profile로 지정축 상대이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_r_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// 사다리꼴 속도 Profile로 상댕이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short r_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// 비대칭 사다리꼴 속도 Profile로 지정축 이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_t_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 사다리꼴 속도 Profile로 이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short t_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 사다리꼴 속도 Profile로 지정축 상대이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_tr_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 사다리꼴 속도 Profile로 상대이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short tr_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 지정축의 동작완료시 까지 다음 명령수행을 중지
		/// </summary>
		/// <param name="ax"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short wait_for_done(short ax);

	}

	/// <summary>
	/// S_CURVE 속도 Profile 단축이동
	/// </summary>
	sealed public class OneAxisMoveScurve
	{
		private OneAxisMoveScurve() { }
		/// <summary>
		/// S_CURVE 속도 Profile로 이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_s_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// S_CURVE 속도 Profile로 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short s_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// S_CURVE 속도 Profile로 지정축 상대이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_rs_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// S_CURVE 속도 Profile로 상대이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short rs_move(short axis, double pos, double vel, short acc);
		/// <summary>
		/// 비대칭 S_CURVE 속도 Profile로 이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_ts_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 S_CURVE 속도 Profile로 이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short ts_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 S_CURVE 속도 Profile로 지정축 상대이동
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_trs_move(short axis, double pos, double vel, short acc, short decel);
		/// <summary>
		/// 비대칭 S_CURVE 속도 Profile로 상대이동 및 완료시까지 기다린다.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="pos"></param>
		/// <param name="vel"></param>
		/// <param name="acc"></param>
		/// <param name="decel"></param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short trs_move(short axis, double pos, double vel, short acc, short decel);
	}

	/// <summary>
	/// 다축 이동
	/// </summary>
	sealed public class MultiAxesMove
	{
		private MultiAxesMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_s_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short s_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_t_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc, ref short decel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short t_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc, ref short decel);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short start_ts_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc, ref short decel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short ts_move_all(short len, ref short axis, ref double pos, ref double vel, ref short acc, ref short decel);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short wait_for_all(short len, ref short axes);
	}

	/// <summary>
	/// Velocity 이동
	/// </summary>
	sealed public class VelocityMove
	{
		private VelocityMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short v_move(short axis, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short v_move_stop(short axis);
	}

	/// <summary>
	/// 좌표축의 구성
	/// </summary>
	sealed public class MapAxes
	{
		private MapAxes() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short map_axes(short n_axes, ref short map_array);
	}

	/// <summary>
	/// Coordinated 동작변수
	/// </summary>
	sealed public class CoordinatedActionSet
	{
		private CoordinatedActionSet() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_move_speed(double speed);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_move_accel(short accel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short all_done();
	}

	/// <summary>
	/// 다축직선이동
	/// </summary>
	sealed public class MulitAxesStraightMove
	{
		private MulitAxesStraightMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_2(double x, double y);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_2ax(short ax1, short ax2, double x, double y, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_2axgr(short grN, short ax1, short ax2, double x, double y, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_3(double x, double y, double z);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_3ax(short ax1, short ax2, short ax3, double x, double y, double z, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_3axgr(short grN, short ax1, short ax2, short ax3, double x, double y, double z, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_4(double x, double y, double z, double w);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_4ax(short ax1, short ax2, short ax3, short ax4, double x, double y, double z, double w, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_4axgr(short grN, short ax1, short ax2, short ax3, short ax4, double x, double y, double z, double w, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_n(ref double axes);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short move_nax(short len, ref short axes, ref double pos, double v, short acc);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_2(double x, double y);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_2ax(short ax1, short ax2, double x, double y, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_2axgr(short grN, short ax1, short ax2, double x, double y, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_3(double x, double y, double z);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_3ax(short ax1, short ax2, short ax3, double x, double y, double z, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_3axgr(short grN, short ax1, short ax2, short ax3, double x, double y, double z, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_4(double x, double y, double z, double w);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_4ax(short ax1, short ax2, short ax3, short ax4, double x, double y, double z, double w, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_4axgr(short grN, short ax1, short ax2, short ax3, short ax4, double x, double y, double z, double w, double v, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_n(ref double axes);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short smove_nax(short len, ref short axes, ref double pos, double v, short acc);
	}

	/// <summary>
	/// 원 및 원호이동
	/// </summary>
	sealed public class ArcMove
	{
		private ArcMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_arc_division(double degrees);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short arc_2(double x_c, double y_c, double angle);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short arc_2ax(short ax1, short ax2, double x_center, double y_center, double angle, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short arc_3(double xc, double yc, double angle, ref double pnt);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short arc_3ax(short ax1, short ax2, short ax3, double xc, double yc, double angle, ref double pnt, double vel, short acc);
	}

	/// <summary>
	/// Spline 이동
	/// </summary>
	sealed public class SplineMove
	{
		private SplineMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_move(short len, short ax1, short ax2, short ax3, ref double x, ref double y, ref double z, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short rect_move(short ax1, short ax2, ref double pntxy, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_movex(short spl_num, short ax1, short ax2, short ax3);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_move_data(short spl_num, short len, short ax1, short ax2, short ax3, ref double pnt1, ref double pnt2, ref double pnt3, double vel, short acc);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_deg_move2(double xc, double yc, ref double degpnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_deg_move2ax(short ax1, short ax2, double xc, double yc, ref double degpnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_deg_move3(double xc, double yc, ref double degpnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_deg_move3ax(short ax1, short ax2, short ax3, double xc, double yc, ref double degpnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_deg_movenax(short len, ref short ax, double xc, double yc, ref double degpnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_move2(double xo, double yo, ref double pntxy, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_move2ax(short ax1, short ax2, double xo, double yo, ref double pntxy, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_move3(double xo, double yo, ref double pntxyz, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_move3ax(short ax1, short ax2, short ax3, double xo, double yo, ref double pntxyz, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_arc_movenax(short len, ref short ax, double xc, double yc, ref double pnt, double vel, short acc, short dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_deg_move2ax(short ax1, short ax2, double xc, double yc, ref double degpnt, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_deg_move3ax(short ax1, short ax2, short ax3, double xc, double yc, ref double degpnt, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_deg_movenax(short len, ref short ax, double xc, double yc, ref double degpnt, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_move2ax(short ax1, short ax2, double x_center, double y_center, ref double pntxy, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_move3ax(short ax1, short ax2, short ax3, double x_center, double y_center, ref double pntxyz, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_arc_movenax(short len, ref short ax, double x_center, double y_center, ref double pnt, double vel, short acc, short dir, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_line_move1ax(short ax, ref double pnt, double vel, short acc, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_line_move2ax(short ax1, short ax2, ref double pntxy, double vel, short acc, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_line_move3ax(short ax1, short ax2, short ax3, ref double pntxyz, double vel, short acc, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_auto_line_movenax(short len, ref short ax, ref double pnt, double vel, short acc, short auto);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move1(ref double pnt, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move1ax(short ax, ref double pnt, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move2(ref double pntxy, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move2ax(short ax1, short ax2, ref double pntxy, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move3(ref double pntxyz, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_move3ax(short ax1, short ax2, short ax3, ref double pntxyz, double vel, short acc);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short spl_line_movenax(short len, ref short ax, ref double pntxyz, double vel, short acc);
	}

	/// <summary>
	/// 정지동작
	/// </summary>
	sealed public class StopAction
	{
		private StopAction() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_stop(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_stop_rate(short axis, ref short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_stop_rate(short axis, short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_stop_rate(short axis, ref short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_stop_rate(short axis, short rate);
	}

	/// <summary>
	/// 긴급정지동작
	/// </summary>
	sealed public class StopEmergenceAction
	{
		private StopEmergenceAction() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_e_stop(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_e_stop_rate(short axis, ref short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_e_stop_rate(short axis, short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_e_stop_rate(short axis, ref short rate);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_e_stop_rate(short axis, short rate);
	}

	/// <summary>
	/// 위치변수
	/// </summary>
	sealed public class PositionValue
	{
		private PositionValue() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_position(short axis, ref double position);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_position(short axis, double position);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_command(short axis, ref double command);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_command(short axis, double command);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_error(short axis, ref double error);
	}

	/// <summary>
	/// 위치에 따른 I/O 출력
	/// </summary>
	sealed public class PositionIO
	{
		private PositionIO() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_io_allclear(short ax);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_io_clear(short ax, short pos_num);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_io_onoff(short pos_num, short bitNo, short ax, double pos, short encflag);
	}

	/// <summary>
	/// 동작상태
	/// </summary>
	sealed public class ActionState
	{
		private ActionState() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short in_sequence(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short in_motion(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short in_position(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short motion_done(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short axis_done(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_com_velocity(short Ax);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_act_velocity(short Ax);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_command_rpm(short ax, ref short rpm_val);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_encoder_rpm(short ax, ref short rpm_val);
	}

	/// <summary>
	/// 제어 상태
	/// </summary>
	sealed public class ControlState
	{
		private ControlState() { }
		/// <summary>
		/// 축의 현재 EVENT 발생상태를 돌려 준다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>Event 번호(MMCEvent)</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short axis_state(short axis);
		/// <summary>
		/// MMC보드로 부터 축의 현재상태를 읽어온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>EVENT 발생 원인(MMCEventSourceStatus)</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short axis_source(short axis);
		/// <summary>
		/// 발생된 EVENT를 해제하고, 다음명령부터 실행한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>MMC Error(MMCError)</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short clear_status(short axis);

		/// <summary>
		/// MMC보드의 Frame Buffer를 Clear 시킨다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>MMC Error(MMCError)</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short frames_clear(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short frames_left(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short controller_idle(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short controller_run(short axis);
	}

	/// <summary>
	/// Sensor 상태
	/// </summary>
	sealed public class SensorState
	{
		private SensorState() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short home_switch(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short pos_switch(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short neg_switch(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short amp_fault_switch(short axis);
	}

	/// <summary>
	/// Analog 입,출력
	/// </summary>
	sealed public class AnalogIO
	{
		private AnalogIO() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_analog(short channel, ref short value);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_dac_output(short axis, ref short volt);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_dac_output(short axis, short volt);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_analog_limit(short ax, ref int limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_analog_limit(short ax, int limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_analog_limit(short ax, int limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_analog_limit(short ax, ref int limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_analog_offset(short ax, ref short offset);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_analog_offset(short ax, short offset);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_analog_offset(short ax, ref short offset);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_analog_offset(short ax, short offset);
	}

	/// <summary>
	/// Motion 제어
	/// </summary>
	sealed public class MotionControl
	{
		private MotionControl() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short mmc_dwell(short axis, int duration);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short mmc_io_trigger(short axis, short bitNo, short state);
		//[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		//public static extern short mmcdelay(long duration);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_servo_linear_flag(short ax, ref short flag);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_servo_linear_flag(short ax, short flag);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_servo_linear_flag(short ax, ref short flag);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_servo_linear_flag(short ax, short flag);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_axis_runstop(short bdNum, ref short stateBit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_axis_runstop(short bdNum, short stateBit);
	}

	/// <summary>
	/// MMC 보드 구성
	/// </summary>
	sealed public class MMCConfig
	{
		private MMCConfig() { }
		/// <summary>
		/// 해당 MMC 보드의 제어 축 수를 돌려 준다.
		/// </summary>
		/// <param name="bdNum">In : 보드 번호</param>
		/// <param name="axes">Out : 축 수</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short mmc_axes(short bdNum, ref short axes);
		/// <summary>
		/// 전체 제어 축 수를 돌려 준다.
		/// </summary>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short mmc_all_axes();

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short version_chk(short bn, ref short ver);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_control_timer(short ax, ref short time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_control_timer(short ax, short time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_control_timer(short ax, ref short time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_control_timer(short ax, short time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_control_timer_ax(short ax, ref double time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_control_timer_ax(short ax, double time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_control_timer_ax(short ax, ref double time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_control_timer_ax(short ax, double time);
	}

	/// <summary>
	/// 축 구성
	/// </summary>
	sealed public class AxisConfig
	{
		private AxisConfig() { }
		/// <summary>
		/// 지정축의 제어 방식을 가져 온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="loop">Out : 제어 방식(MMCControlLoop)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_closed_loop(short axis, ref short loop);
		/// <summary>
		/// 지정축의 제어 방식을 설정 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="loop">In : 제어 방식(MMCControlLoop)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_closed_loop(short axis, short loop);
		/// <summary>
		/// 지정축의 제어 방식을 Boot File에서 가져 온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="loop">Out : 제어 방식(MMCControlLoop)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_closed_loop(short axis, ref short loop);
		/// <summary>
		/// 지정축의 제어 방식을 설정 하고, Boot File에 저장한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="loop">In : 제어 방식(MMCControlLoop)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_closed_loop(short axis, short loop);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_feedback(short axis, ref short device);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_feedback(short axis, short device);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_feedback(short axis, ref short device);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_feedback(short axis, short device);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_unipolar(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_unipolar(short axis, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_unipolar(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_unipolar(short axis, short state);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_control(short axis, ref short cont);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_control(short axis, short cont);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_control(short axis, ref short cont);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_control(short axis, short cont);

		/// <summary>
		/// 지정축의 step pulse 출력 형태를 가져 온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="mode">Out : 출력 형태(MMCPulseType)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_step_mode(short axis, ref short mode);
		/// <summary>
		/// 지정축의 step pulse 출력 형태를 설정 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="mode">In : 출력 형태(MMCPulseType)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_step_mode(short axis, short mode);
		/// <summary>
		/// 지정축의 step pulse 출력 형태를 Boot File에서 가져 온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="mode">Out : 출력 형태(MMCPulseType)</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_step_mode(short axis, ref short mode);
		/// <summary>
		/// 지정축의 step pulse 출력 형태를 설정 하고, Boot File에 저장 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="mode">In : 출력 형태(MMCPulseType)</param>
		/// <returns>r</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_step_mode(short axis, short mode);

		/// <summary>
		/// 해당 축을 일반 Stepper나 Mcro Stepper로 지정 되어있는지를 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>1 이면 True, 0이면 False</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_stepper(short axis);
		/// <summary>
		/// 해당 축을 일반 Stepper나 Mcro Stepper를 제어하는 축으로 지정한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_stepper(short axis);
		/// <summary>
		/// 해당 축을 일반 Stepper나 Mcro Stepper로 지정 되어있는지를 Boot File에서 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns>1 이면 True, 0이면 False</returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_stepper(short axis);
		/// <summary>
		/// 해당 축을 일반 Stepper나 Mcro Stepper를 제어하는 축으로 지정하고 Boot File에 저장한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_stepper(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_micro_stepper(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_micro_stepper(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_micro_stepper(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_micro_stepper(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_servo(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_servo(short axis);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_inposition_required(short axis, ref short req);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_inposition_required(short axis, short req);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_inposition_required(short axis, ref short req);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_inposition_required(short axis, short req);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_coordinate_direction(short ax, ref short cord_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_coordinate_direction(short ax, short cord_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_coordinate_direction(short ax, ref short cord_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_coordinate_direction(short ax, short cord_dir);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_encoder_direction(short ax, ref short enco_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_encoder_direction(short ax, short enco_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_encoder_direction(short ax, ref short enco_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_encoder_direction(short ax, short enco_dir);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_encoder_ratioa(short ax, ref short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_encoder_ratioa(short ax, short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_encoder_ratioa(short ax, ref short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_encoder_ratioa(short ax, short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_encoder_ratiob(short ax, ref short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_encoder_ratiob(short ax, short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_encoder_ratiob(short ax, ref short ratioa);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_encoder_ratiob(short ax, short ratioa);
	}

	/// <summary>
	/// PUlse 분주비
	/// </summary>
	sealed public class PulseRatio
	{
		private PulseRatio() { }
		/// <summary>
		/// 지정 축의 Pulse 분주비를 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="pgratio">Out : 분주비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_pulse_ratio(short axis, ref short pgratio);
		/// <summary>
		/// 지정 축의 Pulse 분주비를 지정 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="pgratio">In : 분주비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_pulse_ratio(short axis, short pgratio);
		/// <summary>
		/// 지정 축의 Pulse 분주비를 Boot File에서 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="pgratio">Out : 분주비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_pulse_ratio(short axis, ref short ratio);
		/// <summary>
		/// 지정 축의 Pulse 분주비를 지정 하고, Boot File에 저장 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="pgratio">In : 분주비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_pulse_ratio(short axis, short pgratio);
	}

	/// <summary>
	/// Gain 변수
	/// </summary>
	sealed public class GainSetup
	{
		private GainSetup() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_gain(short axis, ref int coeff);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_v_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_v_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_v_gain(short axis, ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_v_gain(short axis, ref int coeff);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_p_integration(short ax, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_p_integration(short ax, short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_p_integration(short ax, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_p_integration(short ax, short mode);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_v_integration(short ax, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_v_integration(short ax, short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_v_integration(short ax, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_v_integration(short ax, short mode);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_friction_gain(short axis, ref int gain);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_friction_gain(short axis, int gain);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_friction_gain(short axis, ref int gain);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_friction_gain(short axis, int gain);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_friction_range(short axno, ref double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_friction_range(short axno, double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_friction_range(short axno, ref double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_friction_range(short axno, double range);
	}

	/// <summary>
	/// Software Limits
	/// </summary>
	sealed public class LimitsOfSotware
	{
		private LimitsOfSotware() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_positive_sw_limit(short axis, ref double pos, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_positive_sw_limit(short axis, double pos, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_positive_sw_limit(short axis, ref double pos, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_positive_sw_limit(short axis, double pos, short action);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_negative_sw_limit(short axis, ref double pos, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_negative_sw_limit(short axis, double pos, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_negative_sw_limit(short axis, ref double pos, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_negative_sw_limit(short axis, double pos, short action);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_vel_limit(short axis, ref double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_vel_limit(short axis, double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_vel_limit(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_vel_limit(short axis, short limit);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_accel_limit(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_accel_limit(short axis, short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_accel_limit(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_accel_limit(short axis, short limit);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_in_position(short axis, ref double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_in_position(short axis, double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_in_position(short axis, ref double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_in_position(short axis, double limit);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_error_limit(short axis, ref double limit, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_error_limit(short axis, double limit, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_error_limit(short axis, ref double limit, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_error_limit(short axis, double limit, short action);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_inposition_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_inposition_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_inposition_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_inposition_level(short axis, short level);

	}

	/// <summary>
	/// Hardware Limits
	/// </summary>
	sealed public class LimitsOfHardware
	{
		private LimitsOfHardware() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_positive_level(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_positive_level(short axis, short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_positive_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_positive_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_positive_limit(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_positive_limit(short axis, short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_positive_limit(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_positive_limit(short axis, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_negative_level(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_negative_level(short axis, short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_negative_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_negative_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_negative_limit(short axis, ref short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_negative_limit(short axis, short limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_negative_limit(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_negative_limit(short axis, short action);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_home(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_home(short axis, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_home(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_home(short axis, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_home_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_home_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_home_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_home_level(short axis, short level);
	}

	/// <summary>
	/// AMP 제어
	/// </summary>
	sealed public class AmpControl
	{
		private AmpControl() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_enable(short axis, ref short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_enable(short axis, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_enable_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_enable_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_enable_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_enable_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_resolution(short ax, ref short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_resolution(short ax, short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_resolution(short ax, ref short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_resolution(short ax, short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_resolution32(short ax, ref int resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_resolution32(short ax, int resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_resolution32(short ax, ref int resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_resolution32(short ax, int resolution);
	}

	/// <summary>
	/// AMP Fault
	/// </summary>
	sealed public class AmpFault
	{
		private AmpFault() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_fault(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_fault(short axis, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_fault(short axis, ref short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_fault(short axis, short action);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_fault_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_fault_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_fault_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_fault_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_amp_reset_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_amp_reset_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_amp_reset_level(short axis, ref short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_amp_reset_level(short axis, short level);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short amp_fault_reset(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short amp_fault_set(short axis);
	}

	/// <summary>
	/// I/O Interrupt 구성
	/// </summary>
	sealed public class IOInterruptSetup
	{
		private IOInterruptSetup() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short io_interrupt_enable(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short io_interrupt_on_e_stop(short axis, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short io_interrupt_on_stop(short axis, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short io_interrupt_pcirq(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short io_interrupt_pcirq_eoi(short bdNum);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fio_interrupt_enable(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fio_interrupt_on_e_stop(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fio_interrupt_on_stop(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fio_interrupt_pcirq(short bdNum, short state);
	}

	/// <summary>
	/// I/O Port 입,출력
	/// </summary>
	sealed public class IOPort
	{
		private IOPort() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_io(short port, ref UInt32 value);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_io(short port, UInt32 value);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_out_io(short port, ref UInt32 value);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_bit(short bitNo);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short reset_bit(short bitNo);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_system_io(short ax, ref short onoff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_system_io(short ax, short onoff);
	}

	/// <summary>
	/// 전자기어비
	/// </summary>
	sealed public class ElectricGearRatio
	{
		private ElectricGearRatio() { }
		/// <summary>
		/// 지정축의 전자기어비(모타의 Shaft와 부하축 사이에 기구적으로 결합되는 감속기의 비율)를 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="ratio">Out : 기어비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_electric_gear(short axis, ref double ratio);
		/// <summary>
		/// 지정축의 전자기어비(모타의 Shaft와 부하축 사이에 기구적으로 결합되는 감속기의 비율)를 지정 한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="ratio">In : 기어비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_electric_gear(short axis, double ratio);
		/// <summary>
		/// 지정축의 전자기어비(모타의 Shaft와 부하축 사이에 기구적으로 결합되는 감속기의 비율)를 Boot File에서 가져온다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="ratio">Out : 기어비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_electric_gear(short axis, ref double ratio);
		/// <summary>
		/// 지정축의 전자기어비(모타의 Shaft와 부하축 사이에 기구적으로 결합되는 감속기의 비율)를 지정 한고 Boot File에 저장한다.
		/// </summary>
		/// <param name="axis">In : 축 번호</param>
		/// <param name="ratio">In : 기어비</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_electric_gear(short axis, double ratio);
	}

	/// <summary>
	/// 동기제어
	/// </summary>
	sealed public class SyncContorl
	{
		private SyncContorl() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_sync_map_axes(short master, short slave);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_sync_control(ref short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_sync_control(short sate);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_sync_gain(ref int coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_sync_gain(int coeff);
		//[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		//public static extern short fget_sync_gain(ref long coeff);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_sync_gain(int coeff);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_sync_position(ref double pos_m, ref double pos_s);

		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_sync_control_ax(short ax, ref short enable, ref short master_ax, ref int gain);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_sync_control_ax(short ax, short enable, short master_ax, int gain);
	}

	/// <summary>
	/// 위치보정
	/// </summary>
	sealed public class PositionCompensation
	{
		private PositionCompensation() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short compensation_pos(short len, ref short axes, ref double c_dist, ref short c_acc);
	}

	/// <summary>
	/// Interpolation 제어
	/// </summary>
	sealed public class InterpolationControl
	{
		private InterpolationControl() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_interpolation(short lne, ref short ax, ref int delt_s, short flag);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short frames_interpolation(short axis);
	}

	/// <summary>
	/// Latch
	/// </summary>
	sealed public class Latch
	{
		private Latch() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short arm_latch(short bdNum, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short latch_status(short bdNum);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_latched_position(short axis, ref double pos);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short latch(short bdNum);
	}

	/// <summary>
	/// 절대치 Encoder
	/// </summary>
	sealed public class EncoderABS
	{
		private EncoderABS() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_abs_encoder_type(short ax, ref short type);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_abs_encoder_type(short ax, short type);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_abs_encoder(short ax);
	}

	/// <summary>
	/// 충돌 방지
	/// </summary>
	sealed public class CollisionPrevent
	{
		private CollisionPrevent() { }
		#region 충돌 방지
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_collision_prevent(short max, short sax, short add_sub, short non_equal, double pos);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_collision_prevent_ax(short ax, ref short enable);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_collision_prevent_ax(short ax, short enable, short slave_ax, short add_sub, short non_equal, double c_pos, short type);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_collision_prevent_flag(short bdNum, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_collision_prevent_flag(short bdNum, short mode);
		#endregion
	}

	/// <summary>
	/// 한쪽방향 무한 이동
	/// EndlessMove 관련 함수 사용 금지.
	/// 범위 초과시 초기화 안됨. 마이너스 절대위치 이동시 무한 이동 함.
	/// </summary>
	sealed public class EndlessMove
	{
		private EndlessMove() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_endless_linearax(short ax, ref short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_endless_linearax(short ax, short status, short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_endless_linearax(short ax, ref short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_endless_linearax(short ax, short status, short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_endless_range(short ax, ref double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_endless_range(short ax, double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_endless_range(short ax, ref double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_endless_range(short ax, double range);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_endless_rotationax(short ax, ref short status);
		/// <summary>
		/// 지정축을 회전운동하는 무한회전축으로 설정한다.
		/// </summary>
		/// <param name="ax">축 번호</param>
		/// <param name="status">1이면 지정. 0이면 해제.</param>
		/// <param name="resolution">모터 1회전에 필요한 펄스 수</param>
		/// <returns></returns>
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_endless_rotationax(short ax, short status, short resolution);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_endless_rotationax(short ax, ref short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_endless_rotationax(short ax, short status, short resolution);
	}

	/// <summary>
	/// Filter 관련 함수.
	/// </summary>
	sealed public class Filter
	{
		private Filter() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_position_lowpass_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_position_lowpass_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_position_lowpass_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_position_lowpass_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_position_notch_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_position_notch_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_position_notch_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_position_notch_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_velocity_lowpass_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_velocity_lowpass_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_velocity_lowpass_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_velocity_lowpass_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_velocity_notch_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_velocity_notch_filter(short axis, double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_velocity_notch_filter(short axis, ref double filter);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_velocity_notch_filter(short axis, double filter);
	}

	/// <summary>
	/// Position Compare / Capture
	/// </summary>
	sealed public class PositionCompareAndCapture
	{
		private PositionCompareAndCapture() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare(short index_sel, short index_num, short bitNo, short ax1, short ax2, short latch, short function, short out_mode, double pos, int time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare_enable(short bdNum, short flag);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare_index_clear(short bdNum, short index_sel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare_init(short index_sel, short ax1, short ax2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare_interval(short dir, short ax, short bitNo, double startpos, double limitpos, int interval, int time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short position_compare_read(short index_sel, short ax, ref double pos);
	}

	#region DLL 함수에 없음
	///// <summary>
	///// Jog 관련 함수.
	///// </summary>
	//sealed public class JogFunction
	//{
	//    private JogFunction() { }
	//    /// <summary>
	//    /// 해당 축의 조그 기능을 En/Disable 설정 상태를 읽는다.
	//    /// </summary>
	//    /// <param name="ax">In : 축 번호</param>
	//    /// <param name="state"></param>
	//    /// <returns></returns>
	//    [DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
	//    public static extern short get_jog_enable(short ax, ref short state);
	//    /// <summary>
	//    /// 해당 축의 조그 기능을 En/Disable 한다.
	//    /// </summary>
	//    /// <param name="ax">In : 축 번호</param>
	//    /// <param name="state"></param>
	//    /// <returns></returns>
	//    [DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
	//    public static extern short set_jog_enable(short ax, short state);
	//    [DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
	//    public static extern short get_jog_velocity(ref short ax, ref short JogPosIONum, ref short JogNegIONum, ref double jog_vel);
	//    /// <summary>
	//    /// User Input을 이용하여 조그 기능을 사용
	//    /// </summary>
	//    /// <param name="ax">축 번호</param>
	//    /// <param name="JogPosIONum">Positive 방향으로 이동시킬 입력번호</param>
	//    /// <param name="JogNegIONum">Negative 방향으로 이동시킬 입력번호</param>
	//    /// <param name="jog_vel">속도</param>
	//    /// <returns></returns>
	//    [DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
	//    public static extern short set_jog_velocity(short ax, short JogPosIONum, short JogNegIONum, double jog_vel);
	//    [DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
	//    public static extern short fset_jog_velocity(short ax, short JogPosIONum, short JogNegIONum, double jog_vel);
	//}
	#endregion

	/// <summary>
	/// 메뉴얼에 정의 되어 있지 않은 함수.
	/// </summary>
	sealed public class ETC
	{
		private ETC() { }
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_analog_direction(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_analog_direction(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_analog_direction(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_analog_direction(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short axis_all_status(short ax, ref short instatus, ref int lstatus, ref double dstatus);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern int axis_sourcex(short axis);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_axis_num();
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_bd_num();
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_collision_position(short ax, ref double position);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_counter(short axis, ref double count);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_encoder_filter_num(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_encoder_filter_num(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_encoder_filter_num(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_encoder_filter_num(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_fast_position(short val1, ref double val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_fast_read_encoder(short ax, ref short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_fast_read_encoder(short ax, short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_in_position(short axis, ref double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_in_position(short axis, double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_in_position(short axis, ref double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_in_position(short axis, double limit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_index_required(short axis, ref short indexRequired);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_index_required(short axis, short indexRequired);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_index_required(short axis, ref short indexRequired);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_index_required(short axis, short indexRequired);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_io_mode(short bdNum, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_io_mode(short bdNum, short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_io_mode(short bdNum, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_io_mode(short bdNum, short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_io_num(short bdNum, ref short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_linear_all_stop_flag(short ax, ref short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_linear_all_stop_flag(short ax, short status);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_mmc_init_chkx();
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_mmc_init_chkx(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_mmc_led_num(short bdNum, ref short num);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_mmc_led_num(short num);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_mmc_parameter_init(short bdNum);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short motion_fpga_version_chk(short bn, ref short ver);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_mpg_enable(short ax, ref short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_mpg_enable(short ax, short state);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_mpg_velocity(ref short mpg_vel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_mpg_velocity(short mpg_vel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_pause_control(short bn, short enable, int io_bit);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short option_fpga_version_chk(short bn, ref short ver);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_sensor_auto_off(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_sensor_auto_off(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fget_sensor_auto_off(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short fset_sensor_auto_off(short val1, short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short save_boot_frame();
		//[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		//public static extern short set_spl_auto_Off(short bdNum, short mode);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_spline_move_num(short val1, ref short val2);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_teachposition(short ax, ref double position);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_timer(short bdNum, ref int time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short set_timer(short bdNum, int time);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_velocity(short vel);
		[DllImport("mmcwhp10.Dll", CharSet = CharSet.Ansi)]
		public static extern short get_version();
	}

	sealed public class Enumurations
	{

		public enum Level : short
		{
			High = 1,
			Low = 0
		};

		public enum CircleMove : short
		{
			CCW = 1,
			CW = 0
		};

		public enum Coordinate : short
		{
			CCW = 1,
			CW = 0
		};

		public enum EncoderDir : short
		{
			CCW = 1,
			CW = 0
		};

		public enum EventNumber : short
		{
			No = 0,
			Stop = 1,
			EStop = 2,
			Abort = 3
		};

		[Flags]
		public enum EventSourceStatus : int
		{
			None = 0,
			HomeSwitch = 0x0001,
			PosLimit = 0x0002,
			NegLimit = 0x0004,
			AmpFault = 0x0008,
			ALimit = 0x0010,
			VLimit = 0x0020,
			XNegLimit = 0x0040,
			XPosLimit = 0x0080,
			ErrorLimit = 0x0100,
			PCCommand = 0x0200,
			OutOfFrames = 0x0400,
			AmpPowerOnOff = 0x0800,
			ABSCommError = 0x1000,
			InpositionStatus = 0x2000,
			RunStopCommand = 0x4000,
			CollisionState = 0x8000,
			PaustateState = 0x10000
		};

		public enum DigitlaFilter : short
		{
			Number = 5,
			P = 0,
			I = 1,
			D = 2,
			F = 3,
			ILimit = 4
		};

		public enum ErrorNumber : short
		{
			MaxErrorLen = 80,
			Ok = 0,
			NotInitialized = 1,
			TimeoutErr = 2,
			InvalidAxis = 3,
			IllegalAnalog = 4,
			IllegalIO = 5,
			IllegalParameter = 6,
			NoMap = 7,
			AmpFault = 8,
			OnMotion = 9,
			NonExitst = 10,
			BootOpenError = 11,
			ChksumOpenError = 12,
			WinntDriverOpenError = 13,
			EventOccurError = 14,
			AmpPowerOff = 15,
			DataDirectoryOpenError = 16,
			InvlidCpmotionGroup = 17,
			VelocityIllegalParameter = 18,
			AccelIllegalParameter = 19,
			FuncError = -1,
		};

		/// <summary>
		/// Motor의 종류
		/// </summary>
		public enum MotorType : short
		{
			Servo = 0,
			Stepper = 1,
			Micro = 2
		};

		/// <summary>
		/// 
		/// </summary>
		public enum FeedbackConfig : short
		{
			Encoder = 0,
			Unipolar = 1,
			Bipolar = 2
		};

		public enum ControlLoop : short
		{
			Open = 0,
			Closed = 1,
			Semi = 2
		};

		public enum ControlMethod : short
		{
			Vcontrol = 0,
			Tcontrol = 1,

			Standing = 0,
			Alwys = 1,
		};

		public enum PulseType : short
		{
			Two = 0,
			Sign = 1
		};

		public enum LimitValue : int
		{
			None = 0,
			AccelLimit = 25000,
			VelLimit = 5000000,
			PositiveLimitSW = 2147483640,
			NegativeLimitSW = -2147483640,
			ErrorLimit = 35000,
			PulseRatio = 255
		};

		public enum ActiveEdge
		{
			High,
			Low
		};
	}
#pragma warning restore
}

