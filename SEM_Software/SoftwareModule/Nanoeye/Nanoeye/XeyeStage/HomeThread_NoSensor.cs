using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public class HomeThread_NoSensor : HomeThread
	{
		public static HomeThread CreateInstance(int nAxis)
		{
			return new HomeThread_NoSensor(nAxis);
		}
		public HomeThread_NoSensor(int nAxis) : base(nAxis) { }
		protected override void _DoSearch()
		{
			m_pAxis.SetCmdPosition(0.0);
			m_pAxis.SetAmp(true);

			m_bHomeSearchStartedCorrectly = true;
			m_bHomeSearchDone = true;
		}
	}
}
