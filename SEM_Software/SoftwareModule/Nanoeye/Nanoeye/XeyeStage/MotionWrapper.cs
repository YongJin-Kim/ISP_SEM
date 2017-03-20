using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.Nanoeye.XeyeStage
{
	public abstract class MotionWrapper : IDisposable
	{
		#region Property & Variables
		protected IMotionManager			m_pMotionManager;
		protected HomeSearchManager		m_pHomeSearch;

		protected System.Threading.Thread		m_hLimitThrd;
		protected System.Threading.ManualResetEvent		m_hDestroyEvent;

		protected System.Threading.Thread		m_hAftThrd;
		protected System.Threading.ManualResetEvent		m_hAftTermEvent;
		protected bool		m_bAftThrdStarted;

		/// <summary>
		/// Navigation 이나 Teaching 등으로 Offset 또는 Position 이동 명령을 받아서 
		/// 테이블이 움직이고 있는 경우에는 Velocity 이동을 막아야 한다.
		/// MotionDone() 을 체크하여 막아야 하는 것이 정상이지만 Velocity 이동의 경우에는 
		/// ASA Function 을 지원해야 하므로 축이 이동하는 중에도 속도 이동 명령을 계속해서 보낼 수 있어야 한다.
		/// 그러므로 MotionDone() 상태 체크를 이용할 수 없으므로 이 플래그를 사용한다.
		/// MoveVelocity 나 MovePosition 등의 함수에서는 아래 플래그를  true로 변경하고, Velocity Moving에서는 
		/// MotionDone() 일때 이 플래그를 끄고 이동을 시작한다.
		/// </summary>
		protected List<bool>	m_bOffsetMoving;

		protected Geometry			m_pGeometry;

		protected AFT_ITEM m_AftItem;
		#endregion

		#region 생성자 & 소멸자, 그리고 Dispose
		protected MotionWrapper() { }

		public MotionWrapper(IMotionManager pManager)
		{
			Trace.Assert(pManager != null);

			m_pMotionManager = pManager;
			m_pHomeSearch = null;
			m_hLimitThrd = null;

			m_hDestroyEvent = new System.Threading.ManualResetEvent(false);


			// Z, T축에 대한  AFT 스레드 
			m_hAftThrd = null;
			m_hAftTermEvent = new System.Threading.ManualResetEvent(false);

			m_pGeometry = null;

			m_bOffsetMoving = new List<bool>();
		}

		~MotionWrapper()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (m_pMotionManager != null)
			{
				m_pHomeSearch.Dispose();
				if (m_hDestroyEvent != null)
				{
					m_hDestroyEvent.Close();
					m_hDestroyEvent = null;
				}
				if (m_hAftTermEvent != null)
				{
					m_hAftTermEvent.Close();
					m_hAftTermEvent = null;
				}
				m_pMotionManager.Dispose();
				m_pMotionManager = null;
			}
		}
		#endregion

		public virtual void Release()
		{
			m_pMotionManager.Release();
		}

		//
		// 아래의 함수들은 단순히, IMotionController를 Delegate 한다.
		// ------------------------------------------------------------------------------------------
		public bool InitBoard(int nTotalBoardNo)
		{
			System.Diagnostics.Trace.Assert((nTotalBoardNo > 0) && (nTotalBoardNo < 5));
			if (m_pMotionManager.InitBoard(nTotalBoardNo))
			{
				m_pHomeSearch = _CreateHomeSearchManager();
				//m_hLimitThrd = (HANDLE)_beginthreadex(null, 0, _CheckLimit, this, 0, null);
				m_hLimitThrd = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_CheckLimit));
				m_hLimitThrd.Start(this);
				m_pGeometry = XeyeStageMediator.Instance.GetGeometry();

				System.Diagnostics.Trace.Assert((m_hLimitThrd != null) && (m_pHomeSearch != null) && (m_pGeometry != null));
				return true;
			}
			return false;
		}

		public bool InitBoard()
		{
			return InitBoard(1);
		}

		public void ExitBoard()
		{
			m_hDestroyEvent.Set();
			m_hDestroyEvent.WaitOne();

			m_pHomeSearch.Dispose();
			m_bOffsetMoving.Clear();
			m_hDestroyEvent.Close();

			m_hDestroyEvent = null;
			m_pMotionManager.ExitBoard();
		}

		public bool IsInitialized()
		{
			return m_pMotionManager.IsInitialized();
		}

		public bool AddAxis(AXIS_PARAM param)
		{
			m_bOffsetMoving.Add(false);
			return m_pMotionManager.AddAxis(param);
		}

		public IAxis GetAxis(int nAxis)
		{
			IAxis ax = m_pMotionManager.GetAxis(nAxis);;
			return ax;
		}

		public int GetAxesCount(bool bWithUnmanaged)
		{
			return m_pMotionManager.GetAxesCount(bWithUnmanaged);
		}

		public int GetAxesCount()
		{
			return GetAxesCount(false);
		}

		public void Stop()
		{
			StopAFT();
			m_pMotionManager.Stop();
		}

		// Stop all axes
		public bool IsMotionDone()
		{
			int nTotalAxes = GetAxesCount(true);
			for (int i=0; i < nTotalAxes; i++)
			{
				if (false == GetAxis(i).IsMotionDone())
					return false;
			}
			return true;
		}
		// return true if all axes are motion done


		public void SetParam(int nAxis, AXIS_PARAM pParam)
		{
			GetAxis(nAxis).SetParam(pParam);
		}

		public virtual void SetSWLimit(int nAxis, double dPos, double dNeg)
		{
			GetAxis(nAxis).SetSWLimit(dPos, dNeg);
		}

		public virtual void GetSWLimit(int nAxis, out double dPos, out double dNeg)
		{
			GetAxis(nAxis).GetSWLimit(out dPos, out dNeg);
		}




		// 실시간 체크된 리미트가 아닌 SetAxisParam()에 의해서 설정된 Software 리미트를 리턴한다.
		public double GetPositiveSWLimit(int nAxis) { return GetAxis(nAxis).GetPositiveSWLimit(); }
		public double GetNegativeSWLimit(int nAxis) { return GetAxis(nAxis).GetNegativeSWLimit(); }


		// Software Limit 에 도달하거나, Sensor 가 Signaled 상태이면 true 리턴.
		public bool IsPositiveLimitSignaled(int nAxis) { return (GetAxis(nAxis).IsHwPosLimit() || GetAxis(nAxis).IsSwPosLimit()); }
		public bool IsNegativeLimitSignaled(int nAxis) { return (GetAxis(nAxis).IsHwNegLimit() || GetAxis(nAxis).IsSwNegLimit()); }



		public virtual double GetCmdPosition(int nAxis)
		{
			return GetAxis(nAxis).GetCmdPosition();
		}

		

		//public virtual void MovePosition(Position position)
		//{
		//    int count = position.m_bMove.size();
		//    for (int i=0; i < count; i++)
		//    {
		//        if (position.m_bMove[i])
		//        {
		//            MovePosition(i, position.m_dPos[i], position.m_dSpeed[i]);
		//        }
		//    }
		//}

		//public virtual void MovePosition(Position position, SPEED_LEVEL level)
		//{
		//    XeyeStageMediator pMed = XeyeStageMediator.Instance;

		//    int count = position.m_bMove.size();

		//    for (int i=0; i < count; i++)
		//    {
		//        if (position.m_bMove[i])
		//            MovePosition(i, position.m_dPos[i], pMed.GetSpeed(i, SPEED_LINE));
		//    }
		//}



		// ---------------------------------------------------------------
		//						1 Axis Moving
		// ---------------------------------------------------------------
		public virtual void MovePosition(int nAxis, double dPosition, double dSpeed)
		{
			XeyeStageMediator pMediator = XeyeStageMediator.Instance;
			//if (!pMediator.GetInput(XeyeStageMediator.InputCheckType.IN_DOOR_CLOSE) && !pMediator.CheckUserPermission(XeyeStageMediator.UserPermissionCheckType.DOOR_CHECK))
			//{
			//    ShowIntlockMessage();
			//    return;
			//}

			m_bOffsetMoving[nAxis] = true;
			//	dPosition = max(_GetNegRTLimit(nAxis), min(_GetPosRTLimit(nAxis), dPosition));
			GetAxis(nAxis).AxisClear();
			GetAxis(nAxis).MovePosition(dPosition, dSpeed);
		}

		public virtual void MoveOffset(int nAxis, double dOffset, double dSpeed)
		{
			XeyeStageMediator pMediator = XeyeStageMediator.Instance;
			//if (!pMediator.GetInput(XeyeStageMediator.InputCheckType.IN_DOOR_CLOSE) && !pMediator.CheckUserPermission(XeyeStageMediator.UserPermissionCheckType.DOOR_CHECK))
			//{
			//    ShowIntlockMessage();
			//    return;
			//}

			m_bOffsetMoving[nAxis] = true;
			if (IsHomeSearchDone(nAxis))
				dOffset = (dOffset >= 0.0) ? _GetPositiveOffsetValue(nAxis, dOffset) : _GetNegativeOffsetValue(nAxis, dOffset);
			GetAxis(nAxis).AxisClear();
			GetAxis(nAxis).MoveOffset(dOffset, dSpeed);
		}

		public virtual void MoveVelocity(int nAxis, double dSpeed)
		{
			XeyeStageMediator pMediator = XeyeStageMediator.Instance;
			//if (!pMediator.GetInput(XeyeStageMediator.InputCheckType.IN_DOOR_CLOSE) && !pMediator.CheckUserPermission(XeyeStageMediator.UserPermissionCheckType.DOOR_CHECK))
			//{
			//    ShowIntlockMessage();
			//    return;
			//}

			//if (m_bOffsetMoving[nAxis] && !IsMotionDone())
			//    return;
			//else
			//    m_bOffsetMoving[nAxis] = false;

			if (!IsMotionDone())
			{
				return;
			}


			System.Diagnostics.Debug.WriteLine("MoveVelocity by " + nAxis.ToString(), "MW");

			GetAxis(nAxis).AxisClear();
			GetAxis(nAxis).MoveVelocity(dSpeed);
		}


		public virtual void MovePosition(int nAxis, double dPos)
		{
			double dSpeed = GetAxis(nAxis).GetParam().dSpeed;
			MovePosition(nAxis, dPos, dSpeed);
		}

		public virtual void MoveOffset(int nAxis, double dOffset)
		{
			double dSpeed = GetAxis(nAxis).GetParam().dSpeed;
			MoveOffset(nAxis, dOffset, dSpeed);
		}

		public virtual void MoveVelocity(int nAxis, bool bPos)
		{
			double dSpeed = GetAxis(nAxis).GetParam().dSpeed * (bPos ? 1.0 : -1.0);
			MoveVelocity(nAxis, dSpeed);
		}




		// ---------------------------------------------------------------
		//						2 Axes Moving
		// ---------------------------------------------------------------
		public void SplineMove
		(int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis)
		{
			m_bOffsetMoving[ax1] = true;
			m_bOffsetMoving[ax2] = true;
			m_pMotionManager.SplineMove(ax1, ax2, pos1, pos2, dSpeed, nAccel, nGearAxis);
		}

		public void ArcSplineMove
		(int ax1, int ax2, double pos1, double pos2, double XCenter, double YCenter, double dSpeed, int nAccel, int nStopRate, int nCW, int nGearAxis)
		{
			m_bOffsetMoving[ax1] = true;
			m_bOffsetMoving[ax2] = true;
			m_pMotionManager.ArcSplineMove(ax1, ax2, pos1, pos2, XCenter, YCenter, dSpeed, nAccel, nStopRate, nCW, nGearAxis);
		}


		public void Move2Axes
		(int ax1, int ax2, double pos1, double pos2, double dSpeed, int nAccel, int nGearAxis)
		{
			m_bOffsetMoving[ax1] = true;
			m_bOffsetMoving[ax2] = true;
			m_pMotionManager.Move2Axes(ax1, ax2, pos1, pos2, dSpeed, nAccel, nGearAxis);
		}

		//public void MoveByScreenNavigation(int XOffset, int YOffset)
		//{
		//    double dPixelSize = XeyeStageMediator.Instance.GetPixelSize();

		//    int nAxis1, nAxis2, nAx1Ratio, nAx2Ratio;
		//    _GetScreenNaviDirection(out nAxis1, out nAxis2, out nAx1Ratio, out nAx2Ratio);	// Machine-dependent operation.

		//    IAxis pAx1 = GetAxis(nAxis1);
		//    IAxis pAx2 = GetAxis(nAxis2);

		//    if (!IsMoveable(nAxis1) || !IsMoveable(nAxis2))
		//        return;
		//    double xpos = GetCmdPosition(pAx1.GetAxis()) + (XOffset * dPixelSize * nAx1Ratio);
		//    double ypos = GetCmdPosition(pAx2.GetAxis()) + (YOffset * dPixelSize * nAx2Ratio);

		//    MovePosition(pAx1.GetAxis(), xpos, XeyeStageMediator.Instance.GetSpeed(nAxis1));
		//    MovePosition(pAx2.GetAxis(), ypos, XeyeStageMediator.Instance.GetSpeed(nAxis2));
		//}




		// ---------------------------------------------------------------
		//						Multi Axes Moving
		// ---------------------------------------------------------------
		public void SplineMove
		(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nGearAxis)
		{
			for (int i=0; i < nAxesCount; i++)
			{
				m_bOffsetMoving[pAxes[i]] = true;
				GetAxis(pAxes[i]).AxisClear();
			}

			m_pMotionManager.SplineMove(nAxesCount, pAxes, pPnt, dSpeed, nAccel, nGearAxis);
		}

		public void ArcSplineMove
		(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, double dXCenter, double dYCenter, int nStopRate, int nCW, int nGearAxis)
		{
			for (int i=0; i < nAxesCount; i++)
			{
				m_bOffsetMoving[pAxes[i]] = true;
				GetAxis(pAxes[i]).AxisClear();
			}
			m_pMotionManager.ArcSplineMove(nAxesCount, pAxes, pPnt, dSpeed, nAccel, dXCenter, dYCenter, nStopRate, nCW, nGearAxis);
		}

		public void MultiMove
		(int nAxesCount, int[] pAxes, double[] pPnt, double dSpeed, int nAccel, int nGearAxis)
		{
			for (int i=0; i < nAxesCount; i++)
			{
				m_bOffsetMoving[pAxes[i]] = true;
				GetAxis(pAxes[i]).AxisClear();
			}
			m_pMotionManager.MultiMove(nAxesCount, pAxes, pPnt, dSpeed, nAccel, nGearAxis);
		}





		// ---------------------------------------------------------------
		//						Synchronous control
		// ---------------------------------------------------------------
		public bool GetSyncControl() { return m_pMotionManager.GetSyncControl(); }
		public void SetSyncControl(bool bUse) { m_pMotionManager.SetSyncControl(bUse); }
		public void SetSyncMap(int nMasterAxis, int nSlaveAxis) { m_pMotionManager.SetSyncMap(nMasterAxis, nSlaveAxis); }
		public void SetSyncControl_AX(int master_ax, int slave_ax, bool bEnable) { m_pMotionManager.SetSyncControl_AX(master_ax, slave_ax, bEnable); }





		// ----------------------------------------------------------------
		//					Optional utility functions
		// ----------------------------------------------------------------

		public double GetLineSpeed(int nAxesCount, int[] pAx)
		{
			double dLineSpeed = 0.0;

			for (int i=0; i < nAxesCount; i++)
			{
				dLineSpeed += GetAxis(pAx[i]).GetParam().dSpeed * GetAxis(pAx[i]).GetParam().dSpeed;
			}
			return Math.Pow(dLineSpeed, 1.0 / nAxesCount);
		}

		public virtual int GetMaxAxes() { return m_pMotionManager.GetMaxAxes(); }


		public bool IsMoveable(int nAxis)
		{
			return IsMoveable(nAxis, true);
		}

		public bool IsMoveable(int nAxis, bool bShowMsg)
		{
			IAxis pAxis = m_pMotionManager.GetAxis(nAxis);
			XeyeStageMediator pMediator = XeyeStageMediator.Instance;
			if (pAxis == null)
			{
				return false;
			}


			//if (!(pMediator.GetInput(XeyeStageMediator.InputCheckType.IN_MAIN_POWER_ON)))
			//{
			//    if (bShowMsg)
			//        //::PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ERR_POWER_OFF, 0);

			//        System.Windows.Forms.MessageBox.Show("ERR_POWER_OFF");
			//    //Dlg_ErrorMsg::instance().ShowErrorMsg(IDS_ERR_CATEGORY_IO, theApp.GetString(IDS_ERR_IO_POWEROFF));
			//    return false;
			//}

			//if (!pMediator.GetInput(XeyeStageMediator.InputCheckType.IN_DOOR_CLOSE) && !pMediator.CheckUserPermission(XeyeStageMediator.UserPermissionCheckType.DOOR_CHECK))
			//{
			//    if (bShowMsg)
			//        ShowIntlockMessage();
			//    return false;
			//}

			if (pAxis.GetParam().bManaged && !m_pHomeSearch.IsDone(nAxis))
			{
				if (bShowMsg)
					//::PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ERR_NEED_HOMESEARCH, 0);
					System.Windows.Forms.MessageBox.Show("ERR_NEED_HOMESEARCH");
				return false;
			}


			MOTION_ERR_CODE code = MOTION_ERR_CODE.MOTION_DONE;


			if (!pAxis.IsMoveable(out code))
			{
				if (bShowMsg)
				{
					string szBuf = "";
					pAxis.GetTextName(out szBuf);
					string sDesc="";
					switch (code)
					{
					case MOTION_ERR_CODE.MOTION_AMP_OFF:
						//sDesc.Format(theApp.GetString(IDS_ERR_AMP_OFF) ,szBuf);
						//Message::instance().SetMessage(theApp.GetString(IDS_ERR_CATEGORY_MOTION) , sDesc);
						sDesc = "IDS_ERR_AMP_OFF";
						break;
					case MOTION_ERR_CODE.MOTION_AMP_FAULT:
						//sDesc.Format(theApp.GetString(IDS_ERR_AMP_FAULT) ,szBuf);
						//Message::instance().SetMessage(theApp.GetString(IDS_ERR_CATEGORY_MOTION) , sDesc);
						sDesc = "IDS_ERR_AMP_FAULT";
						break;

					case MOTION_ERR_CODE.MOTION_DONE:
						//sDesc.Format(theApp.GetString(IDS_ERR_TABLE_IS_MOVING) ,szBuf);
						//Message::instance().SetMessage(theApp.GetString(IDS_ERR_CATEGORY_MOTION) , sDesc);
						sDesc = "IDS_ERR_TABLE_IS_MOVING";
						break;
					}

					if (sDesc != "")
					{
						//::PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ERR_CUSTOM_MESSAGE, 0);
						System.Windows.Forms.MessageBox.Show(sDesc, "ERR_CUSTOM_MESSAGE");
					}
				}
				return false;
			}
			return true;
		}

		public void ShowIntlockMessage()
		{
			//string sOpendDoorSet;

			//_GetOpendDoorList(out sOpendDoorSet);

			//string sTitle = System.AppDomain.CurrentDomain.FriendlyName;// (theApp.GetString(IDS_ERR_CATEGORY_MOTION));
			////string sMessage;
			////sMessage.Format(theApp.GetString(IDS_ERR_INTLOCK), sOpendDoorSet);

			////Message::instance().SetMessage(sTitle, sMessage);
			////::PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ERR_CUSTOM_MESSAGE, 0);
			//System.Windows.Forms.MessageBox.Show("IDS_ERR_INTLOCK", "ERR_CUSTOM_MESSAGE");
		}

		public virtual void SetAmp(int nAxis, bool bTrue)
		{
			IAxis ax =  GetAxis(nAxis);
			ax.SetAmp(bTrue);
			//if (!bTrue)
			//    m_pHomeSearch.SetDone(nAxis, false);
		}
		public virtual bool IsAmpOn(int nAxis) { return GetAxis(nAxis).IsAmpOn(); }

		//
		// AFT 기능을 사용하기 위해서, 특정 축들은 지정된 위치로 이동해 주는 편이 좋다.
		//
		// AFT를 시작하려 할때는 다른 포지션에 관계없이 어떻게든 초기 위치로 이동해야 하므로 
		// AFT를 시작하려 할때와 아닐때를 구분한다.
		//
		public virtual void Move_AFT_Home(bool bStartAFT) { }

		//
		// AFT 기능을 사용하여 이동한다.
		//
		// nAxis		: AFT 의 마스터 축
		// bIsOffset	: AFT 기능이 Offset 이동인지 Velocity 이동인지를 표현하는 Boolean 값 
		// bPositive	: 마스터 축의 이동 방향
		// offset		: 마스터 축의 이동량 (bIsOffset 매개변수의 값이 true인 경우에만 사용된다.)
		//
		public virtual void MoveAFT(int nAxis, bool bIsOffset, bool bPositive, double offset/*bIsOffset 이 true인 경우에 사용*/)
		{
			// 기존 스레드가 동작중이라면 리턴
			if (m_hAftThrd.IsAlive) { return; }

			// AFT Thread를 실행하기 위한 구조체 필드 설정
			m_AftItem.nMasterAxis = nAxis;
			m_AftItem.bUseOffset = bIsOffset;
			m_AftItem.bPositive = bPositive;
			m_AftItem.dOffset = Math.Abs(offset);


			// AFT Thread 생성 (생성 완료를 확인하기 위해서 m_bAftThrdStarted 플래그를 사용한다.)
			m_bAftThrdStarted = false;
			//m_hAftThrd = (HANDLE)_beginthreadex(null, 0, _ThrdMoveAFT, this, 0, null);
			m_hAftThrd = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_ThrdMoveAFT));
			m_hAftThrd.Start(this);

			while (m_bAftThrdStarted == false)
			{
				System.Threading.Thread.Sleep(0);
			}
		}

		public virtual void StopAFT()
		{
			if (m_hAftThrd == null) { return; }
			if (m_hAftThrd.IsAlive)
			{
				m_hAftTermEvent.Set();
				m_hAftThrd.Join(500);
				DateTime dtStart = DateTime.Now;
				TimeSpan ts = new TimeSpan(0, 0, 20);
				while (DateTime.Now - dtStart < ts)
					if (IsMotionDone())
						break;
			}
		}





		// ----------------------------------------------------------------
		//				Home Searching Functions
		// ----------------------------------------------------------------
		public virtual void MakeHomeSearchTable()
		{
			for (int i=0; i < m_pMotionManager.GetAxesCount(); i++)
				m_pHomeSearch.AddEntry(i, 0 /*with Home sensor*/);
		}

		public bool IsHomeSearchDone(int nAxis)
		{
			return m_pHomeSearch.IsDone(nAxis);
		}

		public void StartHomeSearch(int nAxis)
		{
			m_pHomeSearch.StartHomeSearch(nAxis);
		}

		public void StopHomeSearch(int nAxis)
		{
			m_pHomeSearch.StopHomeSearch(nAxis);
		}

		public bool IsHomeSearchInProgress(int nAxis)
		{
			bool state = m_pHomeSearch.IsInProgress(nAxis);
			return state;
		}

		public bool IsAnyAxesHomeSearching()
		{
			return m_pHomeSearch.IsAnyAxesHomeSearching();
		}

		public void StopHomeSearchAll()
		{
			m_pHomeSearch.StopAll();
		}

		public void StartHomeSearchAll()
		{
			// Main Power off시 Homesearch기능 사용 못함
			//if (!XeyeStageMediator.Instance.GetInput(XeyeStageMediator.InputCheckType.IN_MAIN_POWER_ON))
			//{
			//    //::PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ERR_POWER_OFF, 0);
			//    System.Windows.Forms.MessageBox.Show("ERR_POWER_OFF");
			//    return;
			//}
			m_pHomeSearch.StartAll();
		}

		// CT축등이 장착된경우에는 이를 확인한 뒤 Home Search를 시작한다.
		public void SetHomeSearchFlag_debug(bool bSet) { }





		protected double _GetPositiveOffsetValue(int nAxis, double dOffset)
		{
			double current_pos = GetCmdPosition(nAxis);
			double limit = m_pGeometry.GetPosRTLimit(nAxis);
			if (current_pos + dOffset > limit)
				return (limit - current_pos);
			return dOffset;
		}

		protected double _GetNegativeOffsetValue(int nAxis, double dOffset)
		{
			double current_pos = GetCmdPosition(nAxis);
			double limit = m_pGeometry.GetNegRTLimit(nAxis);
			dOffset = Math.Abs(dOffset);

			if (current_pos - dOffset < limit) { return (limit - current_pos); }
			return -dOffset;
		}



		protected virtual HomeSearchManager _CreateHomeSearchManager()
		{
			return new HomeSearchManager();
		}

		//protected virtual void _GetOpendDoorList(out string door_list);
		//protected virtual void _GetScreenNaviDirection(out int nAxis1, out int nAxis2, out int nAx1Ratio/*1 or -1*/, out int nAx2Ratio/*1 or -1*/);


		// Data Member 
		protected void _CheckLimit(object param)
		{
			((MotionWrapper)param)._doCheckLimit();
		}

		protected void _doCheckLimit()
		{
			Geometry pGeometry = XeyeStageMediator.Instance.GetGeometry();

			while (!m_hDestroyEvent.WaitOne(10))
			{
				if (!m_pHomeSearch.IsHomeSearchAllDone() || m_pHomeSearch.IsAnyAxesHomeSearching())
					continue;

				for (int i=0; i < GetAxesCount(); i++)
				{
					//if (!XeyeStageMediator.Instance.GetInput(XeyeStageMediator.InputCheckType.IN_MAIN_POWER_ON))
					//{
					//    if (IsAmpOn(i))
					//        SetAmp(i, false);
					//    if (IsHomeSearchDone(i))
					//        SetHomeSearchFlag_debug(false);
					//}
					//else
					//{
						//if(!GetAxis(i).IsMotionDone())
					if (GetAxis(i).GetParam().bManaged)
					{
						SetSWLimit(i, pGeometry.GetPosRTLimit(i), pGeometry.GetNegRTLimit(i));
					}
					//else
					//{
					//    GetAxis(i).ReleaseSWLimit();
					//}
					//}
				}
			}
		}


		protected void _ThrdMoveAFT(object param)
		{
			MotionWrapper me = (MotionWrapper)param;
			this.m_bAftThrdStarted = true;
			this.m_hAftTermEvent.Reset();
			((MotionWrapper)param)._doMoveAFT();
		}
		// Template Method
		protected virtual void _doMoveAFT() { }

		public struct AFT_ITEM
		{
			public int			nMasterAxis;
			public bool		bUseOffset;
			public bool		bPositive;
			public double		dOffset;
		}

	}
}
