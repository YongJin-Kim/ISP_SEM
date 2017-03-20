using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace SEC.Nanoeye.XeyeStage
{
	public class HomeThread_HomeSensor : HomeThread
	{
		public static HomeThread CreateInstance(int nAxis)
		{
			return new HomeThread_HomeSensor(nAxis);
		}

		private HomeThread_HomeSensor(int nAxis) : base(nAxis) { }

		//private virtual unsigned _DoSearch()
		protected override void _DoSearch()
		{
			HOME_SEARCH_PROCESS	proc = HOME_SEARCH_PROCESS.SEARCH_START;
			HOME_SEARCH_PROCESS	enErrorIndex;

			m_bHomeSearchDone = false;	// Turn the Home-Search flag off

			m_hHomeThreadTermEvent.Reset();
			DateTime startDt = DateTime.Now;

			//int nAxis = m_pAxis.GetAxis();

			m_bHomeSearchStartedCorrectly = true;

			while (!m_hHomeThreadTermEvent.WaitOne(100))
			{
				switch (proc)
				{
				case HOME_SEARCH_PROCESS.SEARCH_START:
					{
						if (m_pAxis.IsMotionDone())
						{
							_Prepare();
							proc = HOME_SEARCH_PROCESS.GO_NEG_VEL1;

						}
					}
					break;
				case HOME_SEARCH_PROCESS.GO_NEG_VEL1:
					{
						//
						// Home Sensor 체크시 Stop 할 수 있도록 이벤트 체크후, Negative 로 이동.
						//

						if (m_pAxis.IsMotionDone())
						{
							m_pAxis.SetHomeEvent(true);	// Set Stop Event to Home Sensor
							m_pAxis.MoveVelocity( -Math.Abs(m_pAxis.GetParam().dHomeSpeed1));
							// 3분 리미트
							proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET1;
						}
					}
					break;
				case HOME_SEARCH_PROCESS.GO_POS_OFFSET1:
					{
						//
						// Positive 로 이동. Negative Sensor에 체크되면, velocity, Home Sensor에 체크되면 offset이동.
						//
						if (m_pAxis.IsMotionDone())
						{
							m_pAxis.AxisClear();
							System.Threading.Thread.Sleep(10);
							m_pAxis.AmpFaultReset();

							if (m_pAxis.IsHomeSwitch())
							{
								//
								//			When home-sensor is signaled
								//
								// Home Sensor에 체크되어 멈추면, Stop Event를 풀고, 3.0 만큼 Positive로 이동한다.
								//
								m_pAxis.SetHomeEvent(false);
								m_pAxis.AxisClear();
								m_pAxis.AmpFaultReset();
								System.Threading.Thread.Sleep(10);
								m_pAxis.MoveOffset( 1.0, m_pAxis.GetParam().dHomeSpeed1);


								proc = HOME_SEARCH_PROCESS.GO_NEG_VEL2;
							}
							else if (m_pAxis.IsHwPosLimit())
							{
								//
								//				When positive sensor is signaled.
								//
								// 시작 위치가 Postive limit 보다도 더 Positive에 있다.-_-a  
								// Home Search 과정을 다시 시작한다.
								//
								proc = HOME_SEARCH_PROCESS.SEARCH_START;
							}
							else if (m_pAxis.IsHwNegLimit())
							{
								//
								//				When Negative Sensor is Signaled !!!
								//
								// Home Sensor 위치까지(약간 더...) 이동시킨다.
								// Negative 에서 Home Sensor 까지의 이동에서 이 거리는 장비마다 다를 수 있으므로 축 정보에 저장된 
								// 값을 사용한다.
								m_pAxis.MoveOffset( m_pAxis.GetParam().dDistNegToHomeSensor);
								proc = HOME_SEARCH_PROCESS.SEARCH_START;
							}
							else
							{
								m_pAxis.MoveVelocity( -Math.Abs(m_pAxis.GetParam().dHomeSpeed1));
							}
						}
					}
					break;
				case HOME_SEARCH_PROCESS.GO_NEG_VEL2:
					{
						//
						// 다시 HomeSensor에 Stop Event를 걸어주고, HomeSpeed2 로 Negative 로 이동한다.
						//
						if (m_pAxis.IsMotionDone())
						{
							if (m_pAxis.IsHomeSwitch())	// 아직도 HomeSensor의 위치에 있으면, 다시 GO_POS_OFFSET1과정 실행.
							{
								proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET1;
								break;
							}

							m_pAxis.AmpFaultReset();
							m_pAxis.AxisClear();

							m_pAxis.SetHomeEvent(true);
							System.Threading.Thread.Sleep(100);
							m_pAxis.MoveVelocity( -(m_pAxis.GetParam().dHomeSpeed2));
							System.Threading.Thread.Sleep(100);
							proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET2;
						}
					}
					break;
				case HOME_SEARCH_PROCESS.GO_POS_OFFSET2:
					{
						//
						// 다시 Home Sensor 에 설정된 Stop Event를 풀고, Positive 방향으로 Offset만큼 이동한다.
						//
						if (m_pAxis.IsMotionDone())
						{
							if (m_pAxis.IsHomeSwitch())
							{
								// Clear()/Reset() 이후에 홈이벤트를 풀었다면, 제대로 동작하지 않는다.
								m_pAxis.SetHomeEvent(false);
								m_pAxis.AmpFaultReset();
								m_pAxis.AxisClear();

								m_pAxis.SetCmdPosition(0.0);
								m_pAxis.MoveOffset( m_pAxis.GetParam().dHomeOffset, m_pAxis.GetParam().dHomeSpeed1);
								proc = HOME_SEARCH_PROCESS.COMPLETED;
							}
							else if (m_pAxis.IsHwNegLimit())
							{
								proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET1;
							}
							else
							{
								// Why does the axis stop?... 
								// Hm.. I have no idea
								proc = HOME_SEARCH_PROCESS.GO_NEG_VEL2;
							}
						}
					}
					break;
				case HOME_SEARCH_PROCESS.COMPLETED:
					if (m_pAxis.IsAxisDone())
					{
						m_pAxis.SetHomeEvent(false);
						m_pAxis.SetCmdPosition(0.0);
						m_pAxis.SetSWLimit( m_pAxis.GetParam().dPositiveSWLimit, m_pAxis.GetParam().dNegativeSWLimit);
						m_hHomeThreadTermEvent.Set();
						m_bHomeSearchDone = true;
					}
					break;
				}

				//if(time_limit.CheckOverTime())
				if (DateTime.Now - startDt > new TimeSpan(0, 0, 300))
				{
					enErrorIndex = proc;
					_PrintError(enErrorIndex);
					m_hHomeThreadTermEvent.Set();
				}

				//if (!XeyeStageMediator.Instance.GetInput(XeyeStageMediator.InputCheckType.IN_MAIN_POWER_ON))
				//{
				//    m_pAxis.Stop(false);
				//    m_hHomeThreadTermEvent.Set();
				//    //PostMessage(theMainHwnd, WM_SHOW_MESSAGE , ERR_POWER_OFF, ID_MSG_MODALESS);
				//    System.Windows.Forms.MessageBox.Show("ERR_POWER_OFF");
				//    break;
				//}

				System.Threading.Thread.Sleep(0);
			}


			if (!m_pAxis.IsMotionDone())
				m_pAxis.Stop(false);
		}


	}
}
