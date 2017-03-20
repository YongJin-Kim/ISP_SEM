using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public enum AppDeviceEnum
	{
		AutoDetect,
		SNE1500M,
		SNE3000M,
		SNE4000M,
        SNE4500M,
        SNE4500P,
        SNE3200M,
        SNE3000MB,
        SNE3000MS,
        SH4000M,
        SH3500MB,
        SH5000M
	}

	public enum AppSellerEnum
	{
		AutoDetect,
		SEC,
		Evex,
		Hirox,
		Nikkiso
	}

	public enum AppModeEnum
	{
		Run,
		Calibration,
		Debug
	}

	public enum ScanModeEnum : int
	{
		FastScan = 0,
		SlowScan = 1,
		FastPhoto = 2,
		SlowPhoto = 3,
		AutoFocus = 4,
		AutoContrastBrightness = 5,
		AutoStigFocus = 6,
		ScanPause = 7,
		SpotMode = 8,
		AutoFocus2 = 9,
        SlowPhoto2 = 10
	}

    public enum Profile : int
    {
        profile1 = 0,
        profile2 = 1,
        profile3 = 2,
        profile4 = 3,
        profile5 = 4,
        profile6 = 5,
        profile7 = 6

    }


}
