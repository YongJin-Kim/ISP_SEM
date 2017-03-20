using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoColumn
{
	public enum EnumIControlTableSetStyle : int
	{
		Non = 0,
		Scan_Mag_LinerMode = 1,
		Scan_Mag_WithWD = 2,
		Scan_Mag_Maximum_Set = 3,
		Scan_Mag_Maximum_Get = 4,
		Lens_WD_Constant_Get = 5,
		Table_Validate = 6,
		Scan_Mag_Minimum_Set = 7,
		Scan_Mag_Minimum_Get = 8
	}
}
