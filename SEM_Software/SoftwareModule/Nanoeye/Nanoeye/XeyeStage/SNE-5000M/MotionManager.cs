using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMCval =   SEC.Nanoeye.NanoStage.MMCValues.MMCAPICollection ;

namespace SEC.Nanoeye.XeyeStage.SNE_5000M
{
	public class MMCMotionManager : IMotionManager 
	{

		//private void	_AxesMap(int nNumAxes, int nAxes[], int accel);



		//private 	std::vector<MMCAxis *>	m_axes;

		#region Property & Vriables
		private List<MMCAxis> m_axes;
		private	int						m_nErrorCode;
		private	bool					m_bIsInitialized;
		
		private	string					m_szErrorMsg;
		/// <summary>
		/// 관리되는 축의 수.
		/// </summary>
		private int					m_nAxesCount;
		private int					m_nAvailableAxes;
		#endregion

		#region 생성자 & 소멸자
		public MMCMotionManager()
		{
			m_nErrorCode = 0;
			m_nAxesCount = 0;
			m_bIsInitialized = false;
			m_nAvailableAxes = 4;
			m_axes = new List<MMCAxis>();
		}

		~MMCMotionManager()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (m_bIsInitialized) { 
				ExitBoard();
				GC.SuppressFinalize(this);
			}
		}

		#endregion

		public bool InitBoard(int nTotalBoardNo)
		{

			long[] addr = new long[] { 0xD8000000, 0xD8400000, 0xD8800000, 0xD8C00000 };

			if (nTotalBoardNo < 0 || nTotalBoardNo > 3)
			{
				//MessageBox(NULL, _T("Initialization of Motion board"), _T("Count of board number must be less than 3"), MB_ICONERROR);
				System.Windows.Forms.MessageBox.Show(("Initialization of Motion board"), ("Count of board number must be less than 3"));
				return false;
			}

			//if (m_nErrorCode = MMCval.Initialize.mmc_initx(static_cast<unsigned short>(nTotalBoardNo), addr)) 
			//{
			//    GetErrorMessage(m_szErrorMsg);
			//    MessageBox(NULL, m_szErrorMsg, _T("Initialization of Motion board"), MB_ICONERROR);
			//    return false;
			//}
			int address = (int)addr[0];
			MMCval.ErrorControl.Assert(MMCval.Initialize.mmc_initx(1, ref address));

			m_nAvailableAxes = MMCval.MMCConfig.mmc_all_axes();
			m_bIsInitialized = true;
			return true;
		}

		public void ExitBoard()
		{
			m_bIsInitialized = false;

			m_axes.Clear();
		}

		public void GetErrorMessage(string szMsg)
		{
			//	MultiByteToWideChar(CP_ACP, 0, _error_message((unsigned short)m_nErrorCode) , -1, szMsg , 260);
		}

		public bool IsInitialized()
		{
			return m_bIsInitialized;
		}

		public bool AddAxis(AXIS_PARAM param)
		{
			m_axes.Add(new MMCAxis(param));
			if (m_axes.Count > m_nAvailableAxes)
			{
				System.Windows.Forms.MessageBox.Show("Number of Axes you want to use is excess of available axes of motion_board", "Motion Board");
				return false;
			}

			if (param.bManaged) { m_nAxesCount++; }
			return true;
		}

		public IAxis GetAxis(int nAxis)
		{
			if (nAxis < 0 || nAxis >= m_axes.Count) { throw new ArgumentException(); }
			return m_axes[nAxis];
		}

		public int GetAxesCount()
		{
			return m_nAxesCount;
		}

		public int GetMaxAxes()
		{
			return MMCval.MMCConfig.mmc_all_axes();
		}

		public void Stop()	// 모든 축의 모션을 정지
		{
			short[] axes = new short[m_axes.Count];

			for (int i = 0; i < axes.Length; i++)
			{
				axes[i] = m_axes[i].GetAxis();
			}

			foreach (MMCAxis ax in m_axes)
			{
				if (ax.FrameLeft() > 0)
				{
					ax.FrameClear();
				}

			}

			for (short i = 0; i < GetAxesCount(); i++)
			{
				MMCval.StopAction.set_stop(i);
			}



			short cnt = (short)GetAxesCount();
			MMCval.MultiAxesMove.wait_for_all(cnt, ref axes[0]);

			for (short i=0; i < GetAxesCount(); i++)
				MMCval.ControlState.clear_status(i);
		}

		//void	_AxesMap(int nNumAxes, int nAxes[], int accel)
		//{
		//    std::vector<short> axes(nNumAxes);
		//    for(int i=0; i<nNumAxes; i++)
		//        axes[i] = m_axes[nAxes[i]]->m_param.nPhysicalAxis;

		//    map_axes(nNumAxes, &axes[0]);
		//    set_move_accel(accel);
		//}

		#region	Combine two axes
		public void SplineMove(int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis)
		{
		//    int		pAxes[2]	= {ax1, ax2};
		//    double	pPnt[2]		= {pos1 * m_axes[ax1]->m_param.dEGearRatio, pos2 * m_axes[ax1]->m_param.dEGearRatio};

		//    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

		//    _AxesMap(2/*제어 축 수*/, pAxes, nAccel);
		//    set_move_speed(dSpeed);
		//    spl_line_move2(pPnt, 0 , 0);
		}

		public void ArcSplineMove(int ax1, int ax2, double pos1, double pos2, double XCenter, double YCenter, double dSpeed, int nAccel, int nStopRate, int nCW, int nGearAxis)
		{
			//    int		pAxes[2]	= {ax1, ax2};
			//    double	pPnt[2]		= {pos1 * m_axes[ax1]->m_param.dEGearRatio, pos2 * m_axes[ax1]->m_param.dEGearRatio};

			//    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

			//    _AxesMap(2, pAxes, nAccel);
			//    set_move_speed(dSpeed);
			//    spl_arc_move2(dXCenter, dYCenter, pPnt, 0, 0, nCW);
		}

		public void Move2Axes(int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis)
		{
			//    int		pAxes[2]	= {ax1, ax2};
			//    double	pPnt[2]		= {pos1 * m_axes[ax1]->m_param.dEGearRatio, pos2 * m_axes[ax1]->m_param.dEGearRatio};
			//    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

			//    _AxesMap(2,pAxes, nAccel);
			//    set_move_speed(dSpeed );

			//    smove_n(pPnt);
		}
		#endregion

		#region Combine multi axes more than two axes
		//// ---------------------------------------------------------------
		////					Combine multi axes more than two axes
		//// ---------------------------------------------------------------
		public void SplineMove(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nAxis)
		{
			//    for(int i=0; i<nAxesCount; i++)
			//        pPnt[i] *= m_axes[pAxes[i]]->m_param.dEGearRatio;	// 논리축을 이용해서 Position값을 변경하고,

			//    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

			//    _AxesMap(nAxesCount, pAxes, nAccel);
			//    set_move_speed(dSpeed);

			//    if(nAxesCount == 2)			spl_line_move2(pPnt, 0, 0);
			//    else if(nAxesCount == 3)	spl_line_move3(pPnt, 0, 0);
		}

		public void ArcSplineMove(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, double dXCenter, double dYCenter, int nStopRate, int nCW, int nGearAxis)
		{
			//    for(int i=0; i<nAxesCount; i++)
			//        pPnt[i] *= m_axes[pAxes[i]]->m_param.dEGearRatio;	// 논리축을 이용해서 Position값을 변경하고,


			//    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

			//    _AxesMap(nAxesCount, pAxes, nAccel);
			//    set_move_speed(dSpeed);

			//    if		(nAxesCount == 2) 	spl_arc_move2(dXCenter, dYCenter,pPnt, 0, 0, nCW);
			//    else if	(nAxesCount == 3) 	spl_arc_move3(dXCenter, dYCenter,pPnt, 0, 0, nCW);
		}

		public void MultiMove(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nGearAxis)
		{
			/*
		    for(int i=0; i<nAxesCount; i++)
		        pPnt[i] *= m_axes[pAxes[i]]->m_param.dEGearRatio;	// 논리축을 이용해서 Position값을 변경하고,

		    dSpeed *= m_axes[nGearAxis]->m_param.dEGearRatio;

		    _AxesMap(nAxesCount, pAxes, nAccel);
		    set_move_speed(dSpeed );	

		    smove_n(pPnt);
			 */
		}
		#endregion

		/*
		// ---------------------------------------------------------------
		//					Synchronization of Axes
		// ---------------------------------------------------------------
		bool	GetSyncControl()
		{
			short state;
			get_sync_control(&state);
			return state ? true : false;
		}






		void	SetSyncControl(bool bUse)
		{
			set_sync_control(bUse ? 1 : 0);
		}






		void	SetSyncMap(int nMasterAxis, int nSlaveAxis)
		{
			set_sync_map_axes(nMasterAxis, nSlaveAxis);
		}






		void	SetSyncControl_AX(int master_ax, int slave_ax, bool bEnable)
		{
			// INT slave_ax : Slave 축을 지정합니다.
			// INT enable	: 동기제어를 설정하는 인자입니다.'1' 일 경우 설정, '0'일 경우 해제가 됩니다.
			// INT master_ax: Master 축을 설정합니다.
			// LONG gain	: Master 에 Slave 가 대응하는 gain 값을 설정합니다.
			//일반적으로 설정값을 별도로 주지 않고 서보 드라이버의 게인을 설정하고 그 값에 의해 동작을 시킵니다.'0'

			set_sync_control_ax(slave_ax, bEnable? 1 : 0 , master_ax, 0);	
		}
		*/

		#region IMotionManager 멤버
		public int GetAxesCount(bool bWithUnmanaged)
		{
			return m_axes.Count;
		}

		public bool GetSyncControl()
		{
			throw new NotImplementedException();
		}

		public void SetSyncControl(bool bUse)
		{
			throw new NotImplementedException();
		}

		public void SetSyncMap(int nMasterAxis, int nSlaveAxis)
		{
			throw new NotImplementedException();
		}

		public void SetSyncControl_AX(int master_ax, int slave_ax, bool bEnable)
		{
			throw new NotImplementedException();
		}

		public void Release()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
