using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage.SNE_5000M
{
	public class Motion : MotionWrapper
	{
		protected Motion() { }

		public Motion(IMotionManager pManager)
			: base(pManager)
		{
			m_pHomeSearch = new HomeSearchManager();
		}

		public override void MakeHomeSearchTable()
		{
			m_pHomeSearch.AddEntry(0, HomeThread.HOMESEARCH_METHOD.NEG_SENSOR);
			m_pHomeSearch.AddEntry(1, HomeThread.HOMESEARCH_METHOD.NEG_SENSOR);
			m_pHomeSearch.AddEntry(2, HomeThread.HOMESEARCH_METHOD.NO_SENSOR);
			m_pHomeSearch.AddEntry(3, HomeThread.HOMESEARCH_METHOD.HOME_SENSOR);
			m_pHomeSearch.AddEntry(4, HomeThread.HOMESEARCH_METHOD.POS_SENSOR);
		}

		protected override SEC.Nanoeye.XeyeStage.HomeSearchManager _CreateHomeSearchManager()
		{
			return new SEC.Nanoeye.XeyeStage.SNE_5000M.HomeSearchManager();
		}
	}
}
