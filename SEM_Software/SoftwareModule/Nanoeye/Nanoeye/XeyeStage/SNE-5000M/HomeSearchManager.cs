using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage.SNE_5000M
{
	public class HomeSearchManager : SEC.Nanoeye.XeyeStage.HomeSearchManager
	{
		protected override void _DoStartHomeSearchAllAxes()
		{
			string step = "START";

			MotionWrapper mw = Mediator.Instance.GetMotion();
			m_hHomeSearchThreadTermEvent.Reset();

			for (int i = 0; i < 5; i++)
			{
				m_homeTable[i].SetDone(false);
			}

			while (!m_hHomeSearchThreadTermEvent.WaitOne(50))
			{
				switch (step)
				{
				case "START":
					if (mw.IsMotionDone())
					{
						m_homeTable[(int)Mediator.AxisNumber.Z].Start();
						m_homeTable[(int)Mediator.AxisNumber.R].Start();
						step = "SEARCH_XYT";
					}
					break;
				case "SEARCH_XYT":
					if (IsDone((int)Mediator.AxisNumber.Z))
					{
						m_homeTable[(int)Mediator.AxisNumber.X].Start();
						m_homeTable[(int)Mediator.AxisNumber.Y].Start();
						m_homeTable[(int)Mediator.AxisNumber.T].Start();
						step = "COMPLETE";
					}
					break;
				case "COMPLETE":
					if (IsDone((int)Mediator.AxisNumber.X) && IsDone((int)Mediator.AxisNumber.Y) && IsDone((int)Mediator.AxisNumber.T))
					{
						m_hHomeSearchThreadTermEvent.Set();
					}
					break;

				}
			}
		}
	}
}
