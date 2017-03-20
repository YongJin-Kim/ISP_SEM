using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public class HomeSearchManager : IDisposable
	{
		#region Poperty & Variables
		protected List<HomeThread> m_homeTable;

		protected 	System.Threading.Thread	m_hHomeSearchThread;
		protected System.Threading.ManualResetEvent	m_hHomeSearchThreadTermEvent;
		#endregion

		#region 생성자 & 소멸자, 그리고 Dispose
		public HomeSearchManager()
		{
			m_hHomeSearchThread = null;
			m_hHomeSearchThreadTermEvent = new System.Threading.ManualResetEvent(false);
			m_homeTable = new List<HomeThread>();
		}

		~HomeSearchManager()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (m_homeTable != null)
			{
				StopAll();
				m_hHomeSearchThreadTermEvent.Set();
				m_hHomeSearchThreadTermEvent.Close();
				foreach (HomeThread ht in m_homeTable)
				{
					if (ht != null)
					{
						ht.Stop();
						ht.Dispose();
					}
				}
				m_homeTable.Clear();
				m_homeTable = null;
				GC.SuppressFinalize(this);
			}
		}
		#endregion

		public void AddEntry(int nAxis, HomeThread.HOMESEARCH_METHOD method)
		{
			lock (m_homeTable)
			{
				m_homeTable.Add(HomeThread.CreateInstance(nAxis, method));
			}
#if DEBUG
			SetDone(nAxis, true);
#endif
		}

		public void SetDone(int nAxis, bool bTrue)	// For Debugging
		{
			m_homeTable[nAxis].SetDone(bTrue);
		}

		public bool IsDone(int nAxis)
		{
			if (m_homeTable.Count <= nAxis) { return false; }

			return m_homeTable[nAxis].IsDone();
		}

		public bool IsInProgress(int nAxis)
		{
			bool state = m_homeTable[nAxis].IsInProgress();
			return state;
		}

		public virtual bool IsHomeSearchAvailable()
		{
			MotionWrapper pMotion = XeyeStageMediator.Instance.GetMotion();
			for (int i=0; i < pMotion.GetAxesCount(); i++)
			{
				if (!IsHomeSearchAvailable(i))
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool IsHomeSearchAvailable(int nAxis)
		{
			MotionWrapper pMotion = XeyeStageMediator.Instance.GetMotion();
			if (IsInProgress(nAxis))
				return false;

			if (pMotion.GetAxis(nAxis).IsAmpFault())
			{
				//string sMessage;
				//string szAxisName;
				//XeyeStageMediator.Instance.GetMotion().GetAxis(nAxis).GetTextName(szAxisName);
				//sMessage.Format(theApp.GetString(IDS_ERR_AMP_FAULT), szAxisName);
				//Message::instance().SetMessage(theApp.GetString(IDS_ERR_CATEGORY_MOTION) , sMessage);
				//::PostMessage(theMainHwnd, WM_SHOW_MESSAGE , ERR_CUSTOM_MESSAGE ,ID_MSG_MODAL);
				System.Windows.Forms.MessageBox.Show("ERR_CUSTOM_MESSAGE");
				return false;
			}

			if (pMotion.GetAxis(nAxis).IsMotionDone() == false)
			{
				System.Windows.Forms.MessageBox.Show("ERR_TABLE_MOVING");
				return false;
			}

			return true;
		}

		public virtual bool StartHomeSearch(int nAxis)
		{
			if (!IsHomeSearchAvailable(nAxis)) { return false; }
			m_homeTable[nAxis].Start();
			return true;
		}

		public void StopHomeSearch(int nAxis)
		{
			m_homeTable[nAxis].Stop();
		}

		/// <summary>
		///  If there is at least one axis that is searching home, return true.
		/// </summary>
		/// <returns></returns>
		public bool IsAnyAxesHomeSearching()
		{
			if (m_hHomeSearchThread == null) { return false; }
			if (m_hHomeSearchThread.IsAlive) { return true; }

			foreach (HomeThread th in m_homeTable)
			{
				if (th.IsInProgress())
				{
					return true;
				}
			}

			return false;
		}

		public bool IsHomeSearchAllDone()
		{
			lock (m_homeTable)
			{
				foreach (HomeThread th in m_homeTable)
				{
					if (!th.IsDone()) { return false; }
				}
			}
			return true;
		}

		public void StartAll()
		{
			if (!IsHomeSearchAvailable())
			{
				throw new Exception();
				//return;
			}

			//for (HOME_TABLE::iterator it = m_homeTable.begin(); it != m_homeTable.end(); ++it)
			//    (*it).SetDone(false);
			foreach (HomeThread th in m_homeTable)
			{
				th.SetDone(false);
			}

			//m_hHomeSearchThread = (HANDLE)_beginthreadex(null, 0, _StartHomeSearchAllAxes, this, 0, null);
			m_hHomeSearchThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_StartHomeSearchAllAxes));
			m_hHomeSearchThread.Start(this);

			System.Diagnostics.Trace.Assert(m_hHomeSearchThread != null);
		}

		public void StopAll()
		{
			_DoStopHomeSearchAllAxes();
		}

		protected void _StartHomeSearchAllAxes(object pParam)
		{
			((HomeSearchManager)pParam)._DoStartHomeSearchAllAxes();
		}
		// template method thread

		protected virtual void _DoStartHomeSearchAllAxes()
		{
			//HOME_TABLE::iterator it;
			//for(it = m_homeTable.begin(); it != m_homeTable.end(); ++it)
			//    (*it).Start();
			//}
			foreach (HomeThread th in m_homeTable)
			{
				th.Start();
			}
		}

		protected virtual void _DoStopHomeSearchAllAxes()
		{
			//HOME_TABLE::iterator it;
			//for(it = m_homeTable.begin(); it != m_homeTable.end(); ++it)
			//    (*it).Stop();
			m_hHomeSearchThreadTermEvent.Set();

			foreach (HomeThread th in m_homeTable)
			{
				if (th != null)
				{
					th.Stop();
				}
			}

		}
	}
}
