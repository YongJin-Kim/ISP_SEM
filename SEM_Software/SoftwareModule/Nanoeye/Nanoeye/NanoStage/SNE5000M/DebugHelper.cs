using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoStage.SNE5000M
{
	public class DebugHelper
	{
		private static Dictionary<MMCValues.MMCAxis, HomeSearch> hoaList;

		[System.Diagnostics.Conditional("DEBUG")]
		public static void HomesearchOneAxis(IAxis axis, int type)
		{
			MMCValues.MMCAxis ax = axis as MMCValues.MMCAxis;
			HomeSearch hs;
			if (hoaList == null)
			{
				hoaList = new Dictionary<SEC.Nanoeye.NanoStage.MMCValues.MMCAxis, HomeSearch>();
				hs = new HomeSearch(ax );
				hoaList.Add(ax, hs);
			}
			else
			{
				if (hoaList.ContainsKey(ax))
				{
					hs = hoaList[ax];
				}
				else
				{
					hs = new HomeSearch(ax);
					hoaList.Add(ax, hs);
				}
			}

			if (hs.IsHomeSearching)
			{
				hs.Stop();
				hoaList.Remove(hs.Axis);
				if (hoaList.Count == 0) { hoaList = null; }
			}
			else
			{
				HomeSearch.HomeSearchType ht = HomeSearch.HomeSearchType.NoSensor;
				switch (type)
				{
				case 0:
					ht = HomeSearch.HomeSearchType.NoSensor;
					break;
				case 1:
					ht = HomeSearch.HomeSearchType.HomeSensor;
					break;
				case 2:
					ht = HomeSearch.HomeSearchType.NegSensor;
					break;
				case 3:
					ht = HomeSearch.HomeSearchType.PosSensor;
					break;
				default:
					throw new ArgumentException();
				}
				hs.Start(ht);
			}
		}
	}
}
