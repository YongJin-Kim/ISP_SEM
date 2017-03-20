using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace SEC.Nanoeye.XeyeStage
{
	public abstract class HomeThread : IDisposable
	{
		#region Property & Variables
		protected IAxis	m_pAxis;
		protected Thread	m_hHomeThread;
		protected ManualResetEvent	m_hHomeThreadTermEvent;
		protected bool	m_bHomeSearchDone;
		protected bool	m_bHomeSearchStartedCorrectly;

		protected static bool		m_bCreateCS;
		protected static object m_cs;
		private bool IsDisposed = false;
		#endregion

		#region Enumerations
		public enum HOMESEARCH_METHOD { HOME_SENSOR, NEG_SENSOR, NO_SENSOR, POS_SENSOR };
		protected enum HOME_SEARCH_PROCESS { SEARCH_START, GO_NEG_VEL1, GO_POS_OFFSET1, GO_NEG_VEL2, GO_POS_OFFSET2,  COMPLETED };
		#endregion

		public static HomeThread CreateInstance(int nAxis, HOMESEARCH_METHOD method)
		{
			switch (method)
			{
			case HOMESEARCH_METHOD.HOME_SENSOR:
				return HomeThread_HomeSensor.CreateInstance(nAxis);
			case HOMESEARCH_METHOD.NEG_SENSOR:
				return HomeThread_NegSensor.CreateInstance(nAxis);
			case HOMESEARCH_METHOD.NO_SENSOR:
				return HomeThread_NoSensor.CreateInstance(nAxis);
			case HOMESEARCH_METHOD.POS_SENSOR:
				return HomeThread_PosSensor.CreateInstance(nAxis);
			default:
				throw new ArgumentException();
			}
		}

		#region 생성자 & 소멸자, 그리고 Dispose
		public HomeThread(int nAxis)
		{
			//m_pAxis = XeyeMediator::instance().GetMotion().GetAxis(nAxis);
			m_pAxis = XeyeStageMediator.Instance.GetMotion().GetAxis(nAxis);
			m_bHomeSearchDone = false;
			m_hHomeThread = null;
			m_hHomeThreadTermEvent = new ManualResetEvent(false);
		}

		~HomeThread()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Stop();
				m_hHomeThreadTermEvent.Close();
				GC.SuppressFinalize(this);
			}
		}
		#endregion

		public void Start()
		{
			if (IsInProgress()) { return; }

			m_bHomeSearchStartedCorrectly = false;
			m_hHomeThread = new Thread(new ParameterizedThreadStart(_Start));
			m_hHomeThread.Start(this);

			while (!m_bHomeSearchStartedCorrectly) { System.Threading.Thread.Sleep(10); }
		}

		public void Stop()
		{
			if (IsInProgress())
			{
				m_hHomeThreadTermEvent.Set();
				if (!m_hHomeThread.Join(100))
				{
					m_hHomeThread.Abort();
				}

				m_hHomeThread = null;
			}
			//XeyeMediator::instance().GetMotion().SetAmp(m_pAxis.GetAxis(), false);
			XeyeStageMediator.Instance.GetMotion().SetAmp(m_pAxis.GetAxis(), false);
			m_bHomeSearchDone = false;
		}

		public bool IsInProgress()
		{
			if (m_hHomeThread == null) { return false; }
			bool state;
			if (m_hHomeThread.IsAlive)
			{
				state = true;
			}
			else
			{
				state = false;
			}
			return state;
		}

		public void SetDone(bool btrue)
		{
			m_bHomeSearchDone = btrue;
		}

		public bool IsDone()
		{
			return m_bHomeSearchDone;
		}

		public bool WaitForDone(int dwMillisecond)
		{
			return m_hHomeThread.Join(dwMillisecond);
		}

		private void _Start(object pParam)
		{
			((HomeThread)pParam)._DoSearch();
		}

		protected abstract void _DoSearch();

		protected void _PrintError(HOME_SEARCH_PROCESS proc)
		{
			// Show Error
			//uint ErrID;
			string errStr;
			switch (proc)
			{
			case HOME_SEARCH_PROCESS.GO_NEG_VEL1:
				errStr = "ERR_HOMESEARCH_TIMEOUT1";
				//ErrID = ERR_HOMESEARCH_TIMEOUT1;
				break;
			case HOME_SEARCH_PROCESS.GO_POS_OFFSET1:
				errStr = "ERR_HOMESEARCH_TIMEOUT2";
				//ErrID = ERR_HOMESEARCH_TIMEOUT2;
				break;
			case HOME_SEARCH_PROCESS.GO_NEG_VEL2:
				errStr = "ERR_HOMESEARCH_TIMEOUT3";
				//ErrID = ERR_HOMESEARCH_TIMEOUT3;
				break;
			case HOME_SEARCH_PROCESS.GO_POS_OFFSET2:
				errStr = "ERR_HOMESEARCH_TIMEOUT4";
				//ErrID = ERR_HOMESEARCH_TIMEOUT4;
				break;
			default:
				errStr = "ERR_HOMESEARCH_STOPPED";
				//ErrID = ERR_HOMESEARCH_STOPPED;
				break;
			}
			m_pAxis.Stop(false);
			m_pAxis.SetAmp(false);
			//PostMessage(theMainHwnd, WM_SHOW_MESSAGE, ErrID, ID_MSG_MODALESS);
			System.Windows.Forms.MessageBox.Show(errStr);
		}

		protected void _Prepare()
		{
			XeyeStageMediator.Instance.GetMotion().SetAmp(m_pAxis.GetAxis(), true);
			m_pAxis.AxisClear();
			m_pAxis.AmpFaultReset();
			m_pAxis.FrameClear();
			m_pAxis.ReleaseSWLimit();
		}
	}
}
