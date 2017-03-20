using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public class HomeThread_NegSensor : HomeThread
	{
		public static HomeThread CreateInstance(int nAxis)
		{
			return new HomeThread_NegSensor(nAxis);
		}

		private HomeThread_NegSensor(int nAxis) : base(nAxis) { }
		protected override void _DoSearch()
		{
			HOME_SEARCH_PROCESS	proc = HOME_SEARCH_PROCESS.SEARCH_START;
			HOME_SEARCH_PROCESS	enErrorIndex;

			m_bHomeSearchStartedCorrectly = true;
			m_bHomeSearchDone = false;	// Turn the Home-Search flag off

			m_hHomeThreadTermEvent.Reset();

			DateTime startDt = DateTime.Now;

			while (!m_hHomeThreadTermEvent.WaitOne(50))
			{
				switch (proc)
				{
				case HOME_SEARCH_PROCESS.SEARCH_START:
					{
						_Prepare();
						proc = HOME_SEARCH_PROCESS.GO_NEG_VEL1;
						
					}
					break;
				case HOME_SEARCH_PROCESS.GO_NEG_VEL1:
					{
						if (m_pAxis.IsMotionDone())
						{
							m_pAxis.MoveVelocity(-m_pAxis.GetParam().dHomeSpeed1);
							//while (m_pAxis.IsMotionDone())
							//    System.Threading.Thread.Sleep(10);
							proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET1;

							if (m_pAxis.GetAxis() == 0)
							{
								System.Diagnostics.Debug.WriteLine("GO_POS_OFFSET1", "axX");
							}
							
						}
					}
					break;




				// Positive 로 이동. Negative Sensor에 체크되면, velocity, Home Sensor에 체크되면 offset이동.
				case HOME_SEARCH_PROCESS.GO_POS_OFFSET1:
					{
						if (m_pAxis.IsMotionDone())
						{
							//ConsolePrint(_T("GO_POS_OFFSET1\n"));
							
							m_pAxis.AxisClear();
							System.Threading.Thread.Sleep(10);
							m_pAxis.AmpFaultReset();

							if (m_pAxis.IsHwNegLimit())
							{
								m_pAxis.AxisClear();
								m_pAxis.AmpFaultReset();
								System.Threading.Thread.Sleep(10);
								//pMotion.MoveOffset(nAxis, 1.0, m_pAxis.GetParam().dHomeSpeed1);
								m_pAxis.MoveOffset(1.0, m_pAxis.GetParam().dHomeSpeed1);
								//while (m_pAxis.IsMotionDone())
								//    System.Threading.Thread.Sleep(10);

								proc = HOME_SEARCH_PROCESS.GO_NEG_VEL2;


								if (m_pAxis.GetAxis() == 0)
								{
									System.Diagnostics.Debug.WriteLine("GO_NEG_VEL2", "axX");
								}

							}
							else if (m_pAxis.IsHwPosLimit())
							{
								proc = HOME_SEARCH_PROCESS.SEARCH_START;

								if (m_pAxis.GetAxis() == 0)
								{
									System.Diagnostics.Debug.WriteLine("SEARCH_START", "axX");
								}
							}
							else
							{
								//pMotion.MoveVelocity(nAxis, -m_pAxis.GetParam().dHomeSpeed1);
								m_pAxis.MoveVelocity( -m_pAxis.GetParam().dHomeSpeed1);
								
							}
						}
					}
					break;




				case HOME_SEARCH_PROCESS.GO_NEG_VEL2:
					{
						if (m_pAxis.IsMotionDone())
						{
							
							if (m_pAxis.IsHwNegLimit())
							{
								proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET1;
								if (m_pAxis.GetAxis() == 0)
								{
									System.Diagnostics.Debug.WriteLine("GO_POS_OFFSET1", "axX");
								}

								break;
							}

							m_pAxis.AmpFaultReset();
							System.Threading.Thread.Sleep(100);
							m_pAxis.AxisClear();
							System.Threading.Thread.Sleep(100);
							//pMotion.MoveVelocity(nAxis, -m_pAxis.GetParam().dHomeSpeed2);
							m_pAxis.MoveVelocity( -m_pAxis.GetParam().dHomeSpeed2);
							//while (m_pAxis.IsMotionDone())
							//    System.Threading.Thread.Sleep(10);
							proc = HOME_SEARCH_PROCESS.GO_POS_OFFSET2;
							if (m_pAxis.GetAxis() == 0)
							{
								System.Diagnostics.Debug.WriteLine("GO_POS_OFFSET2", "axX");
							}
							
						}
					}
					break;



				// 다시 Home Sensor 에 설정된 Stop Event를 풀고, Positive 방향으로 Offset만큼 이동한다.
				case HOME_SEARCH_PROCESS.GO_POS_OFFSET2:
					{
						if (m_pAxis.IsMotionDone())
						{
							// Clear()/Reset() 이후에 홈이벤트를 풀었다면, 제대로 동작하지 않는다.
							m_pAxis.AmpFaultReset();
							System.Threading.Thread.Sleep(100);
							m_pAxis.AxisClear();
							System.Threading.Thread.Sleep(100);

							if (m_pAxis.IsHwNegLimit())
							{
								m_pAxis.MoveOffset(m_pAxis.GetParam().dHomeOffset, m_pAxis.GetParam().dHomeSpeed1);
								//while (m_pAxis.IsMotionDone())
								//    System.Threading.Thread.Sleep(10);
								System.Diagnostics.Debug.WriteLine("IsHwNegLimit" + m_pAxis.GetParam().szName);
								proc = HOME_SEARCH_PROCESS.COMPLETED;
								break;
							}
							else
							{
								proc = HOME_SEARCH_PROCESS.GO_NEG_VEL2;
								break;
							}
						}
					}
					break;

				case HOME_SEARCH_PROCESS.COMPLETED:
					if (m_pAxis.IsMotionDone())
					{
						m_bHomeSearchDone = true;
						m_pAxis.SetHomeEvent(false);
						m_pAxis.SetCmdPosition(0.0);
						m_pAxis.SetCurPosition(0.0);
						m_pAxis.SetSWLimit( m_pAxis.GetParam().dPositiveSWLimit, m_pAxis.GetParam().dNegativeSWLimit);
						m_hHomeThreadTermEvent.Set();
						System.Diagnostics.Debug.WriteLine("HomeSearch END" + m_pAxis.GetParam().szName);
					}
					break;
				}


				//if (time_limit.CheckOverTime())
				if (DateTime.Now - startDt > new TimeSpan(0, 0, 300))
				{
					enErrorIndex = proc;
					_PrintError(enErrorIndex);
					m_hHomeThreadTermEvent.Set();

				}
			}
			m_pAxis.Stop(false);
			System.Diagnostics.Debug.WriteLine("HomeThread_Neg Stopped.");
		}
	}
	
}
