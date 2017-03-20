using System;

namespace SEC.Nanoeye.NanoColumn
{
	internal class NanoeyeDevicesInfo
	{
		internal NanoeyeDevicesInfo(NanoeyeDevices device) : this(device, 0) { }

		internal NanoeyeDevicesInfo(NanoeyeDevices device, Int32 value)
		{
			Device = device;
			Value = value;
		}

		internal NanoeyeDevices Device;
		internal Int32 Value = 0;
	}

	/// <summary>
	/// 통신의 형태
	/// </summary>
	internal enum NanoeyeDeviceType : ushort
	{
		EepromWrite = 0,
		EepromRead = 1,
		Get = 1,
		Set = 2,
		Read = 3,
		Write = 4,
		Info = 7
	}

	internal enum NanoeyeDeviceParser : ushort
	{
		Board = 0xf000,
		Inst = 0x0ff0,
		Type = 0x000f,
	}

	/// <summary>
	/// 통신 대상
	/// </summary>
	internal enum NanoeyeDevices : ushort
	{
		Noting = 0,

		#region Align
		Align_BeamShiftDX = 0x1010,
		Align_BeamShiftDY = 0x1020,
		Align_BeamShiftX = 0x1030,
		Align_BeamShiftY = 0x1040,
		Align_GunAlignDX = 0x1050,
		Align_GunAlignDY = 0x1060,
		Align_GunAlignX = 0x1070,
		Align_GunAlignY = 0x1080,
		Align_Date = 0x1F00,
		Align_Time = 0x1F10,

		Align_EEPROM = 0x7100,
		#endregion

		#region Egps
		Egps_Clt = 0x2010,
		Egps_CltCMon = 0x2020,
		Egps_CltVMon = 0x2030,
		Egps_Eghv = 0x2040,
		Egps_EghvCMon = 0x2050,
		Egps_EghvVMon = 0x2060,
		Egps_Enable = 0x2070,
		Egps_Grid = 0x2080,
		Egps_GridCMon = 0x2090,
		Egps_GridVMon = 0x20A0,
		Egps_Pmt = 0x20B0,
		Egps_PmtCMon = 0x20C0,
		Egps_PmtVMon = 0x20D0,
		Egps_Spare = 0x20E0,
		Egps_Tip = 0x20F0,
		Egps_TipCMon = 0x2100,
		Egps_TipVMon = 0x2110,

		Egps_SystemType = 0x2F20,

		Egps_Date = 0x2F00,
		Egps_Time = 0x2F10,

		Egps_EEPROM = 0x7200,

		#endregion

		#region Lens
		Lens_Degauss1 = 0X3010,
		Lens_Degauss2 = 0X3020,
		Lens_Degauss3 = 0X3030,
		Lens_Direction1 = 0X3040,
		Lens_Direction2 = 0X3050,
		Lens_Direction3 = 0X3060,
		Lens_Lens1 = 0X3070,
		Lens_Lens2 = 0X3080,
		Lens_Lens3 = 0X3090,
		Lens_Lens3C = 0X30A0,
		Lens_Lens3F = 0X30B0,
		Lens_SyncScan1 = 0X30C0,
		Lens_SyncScan2 = 0X30D0,
		Lens_SyncScan3A = 0X30E0,
		Lens_SyncScan3B = 0X30F0,
		Lens_SyncScanEnable = 0X3100,
		Lens_SyncScanGain = 0X3110,
		Lens_Wobble1 = 0X3120,
		Lens_Wobble2 = 0X3130,
		Lens_Wobble3 = 0X3140,
		Lens_WobbleAmpl1 = 0X3150,
		Lens_WobbleAmpl2 = 0X3160,
		Lens_WobbleAmpl3 = 0X3170,
		Lens_WobbleFreq1 = 0X3180,
		Lens_WobbleFreq2 = 0X3190,
		Lens_WobbleFreq3 = 0X31A0,
		Lens_Date = 0x3F00,
		Lens_Time = 0x3F10,

		Lens_EEPROM = 0x7300,

		#endregion

		#region Scan
		Scan_AlignX = 0X4010,
		Scan_AlignY = 0X4020,
		Scan_AmplitudeX = 0X4030,
		Scan_AmplitudeY = 0X4040,
		Scan_ExtSelect = 0X4050,
		Scan_MagnificationX = 0X4060,
		Scan_MagnificationY = 0X4070,
		Scan_Rotation = 0X4080,
		Scan_FeedbackMode = 0X4090,
		Scan_Date = 0x4F00,
		Scan_Time = 0x4F10,

		Scan_EEPROM = 0x7400,

		#endregion

		#region Stig
		Stig_AlignXAB = 0X5010,
		Stig_AlignXCD = 0X5020,
		Stig_AlignYAB = 0X5030,
		Stig_AlignYCD = 0X5040,
		Stig_StigX = 0X5050,
		Stig_StigY = 0X5060,
		Stig_SyncScanX = 0X5070,
		Stig_SyncScanY = 0X5080,
		Stig_WobbleAmplX = 0X5090,
		Stig_WobbleAmplY = 0X50A0,
		Stig_WobbleFreqX = 0X50B0,
		Stig_WobbleFreqY = 0X50C0,
		Stig_WobbleX = 0X50D0,
		Stig_WobbleY = 0X50E0,

		Stig_Date = 0x5F00,
		Stig_Time = 0x5F10,

		Stig_EEPROM = 0x7500,

		#endregion

		#region Vacuum
		Vacuum_Pirani1 = 0X6010,
		Vacuum_Pirani2 = 0X6020,
		Vacuum_Enable = 0X6030,
		Vacuum_State = 0x6040,
		Vacuum_RunTime = 0x6050,
		Vacuum_Relay1FromC1 = 0x6060,
		Vacuum_LowVacuum = 0x6070,

		Vacuum_Date = 0x6F00,
		Vacuum_Time = 0x6F10,

		Vacuum_EEPROM = 0x7600,

		#endregion
	}

	/// <summary>
	/// NanoeyeDevices | NanoeyeDeviceType
	/// </summary>
	internal enum NanoeyeDevicesFullName : ushort
	{
		#region Align
		Align_BeamShiftDX_Get = 0x1011,
		Align_BeamShiftDX_Set = 0x1012,
		Align_BeamShiftDY_Get = 0x1021,
		Align_BeamShiftDY_Set = 0x1022,
		Align_BeamShiftX_Get = 0x1031,
		Align_BeamShiftX_Set = 0x1032,
		Align_BeamShiftX_Read = 0x1033,
		Align_BeamShiftX_Min = 0x1035,
		Align_BeamShiftX_Max = 0x1036,
		Align_BeamShiftY_Get = 0x1041,
		Align_BeamShiftY_Set = 0x1042,
		Align_BeamShiftY_Read = 0x1043,
		Align_BeamShiftY_Min = 0x1045,
		Align_BeamShiftY_Max = 0x1046,
		Align_GunAlignDX_Get = 0x1051,
		Align_GunAlignDX_Set = 0x1052,
		Align_GunAlignDY_Get = 0x1061,
		Align_GunAlignDY_Set = 0x1062,
		Align_GunAlignX_Get = 0x1071,
		Align_GunAlignX_Set = 0x1072,
		Align_GunAlignX_Read = 0x1073,
		Align_GunAlignX_Min = 0x1075,
		Align_GunAlignX_Max = 0x1076,
		Align_GunAlignY_Get = 0x1081,
		Align_GunAlignY_Set = 0x1082,
		Align_GunAlignY_Read = 0x1083,
		Align_GunAlignY_Min = 0x1085,
		Align_GunAlignY_Max = 0x1086,

		Align_BeamShiftRotation_Get = 0x1711,
		Align_BeamShiftRotation_Set = 0x1712,
		Align_GunAlignRotation_Get = 0x1721,
		Align_GunAlignRotation_Set = 0x1722,

		#endregion

		#region Egps
		Egps_Cl_Gett = 0x2011,
		Egps_Clt_Set = 0x2012,
		Egps_CltCMon_Read = 0x2023,
		Egps_CltCMon_Min = 0x2025,
		Egps_CltCMon_Max = 0x2026,
		Egps_CltVMon_Read = 0x2033,
		Egps_CltVMon_Min = 0x2035,
		Egps_CltVMon_Max = 0x2036,
		Egps_Eghv_Get = 0x2041,
		Egps_Eghv_Set = 0x2042,
		Egps_EghvCMon_Read = 0x2053,
		Egps_EghvCMon_Min = 0x2055,
		Egps_EghvCMon_Max = 0x2056,
		Egps_EghvVMon_Read = 0x2063,
		Egps_EghvVMon_Min = 0x2065,
		Egps_EghvVMon_Max = 0x2066,
		Egps_Enable_Get = 0x2071,
		Egps_Enable_Set = 0x2072,
		Egps_Grid_Get = 0x2081,
		Egps_Grid_Set = 0x2082,
		Egps_GridCMon_Read = 0x2093,
		Egps_GridCMon_Min = 0x2095,
		Egps_GridCMon_Max = 0x2096,
		Egps_GridVMon_Read = 0x20A3,
		Egps_GridVMon_Min = 0x20A5,
		Egps_GridVMon_Max = 0x20A6,
		Egps_Pm_Gett = 0x20B1,
		Egps_Pmt_Set = 0x20B2,
		Egps_PmtCMon_Read = 0x20C3,
		Egps_PmtCMon_Min = 0x20C5,
		Egps_PmtCMon_Max = 0x20C6,
		Egps_PmtVMon_Read = 0x20D3,
		Egps_PmtVMon_Min = 0x20D5,
		Egps_PmtVMon_Max = 0x20D6,
		Egps_Spare_Get = 0x20E1,
		Egps_Spare_Set = 0x20E2,
		Egps_Tip_Get = 0x20F1,
		Egps_Tip_Set = 0x20F2,
		Egps_TipCMon_Read = 0x2103,
		Egps_TipCMon_Min = 0x2105,
		Egps_TipCMon_Max = 0x2106,
		Egps_TipVMon_Read = 0x2113,
		Egps_TipVMon_Min = 0x2115,
		Egps_TipVMon_Max = 0x2116,

		Egps_SystemType_Read = 0x2F23,
		#endregion

		#region Lens
		Lens_Degauss1_Get = 0X3011,
		Lens_Degauss1_Set = 0X3012,
		Lens_Degauss2_Get = 0X3021,
		Lens_Degauss2_Set = 0X3022,
		Lens_Degauss3_Get = 0X3031,
		Lens_Degauss3_Set = 0X3032,
		Lens_Direction1_Get = 0X3041,
		Lens_Direction1_Set = 0X3042,
		Lens_Direction2_Get = 0X3051,
		Lens_Direction2_Set = 0X3052,
		Lens_Direction3_Get = 0X3061,
		Lens_Direction3_Set = 0X3062,
		Lens_Lens1_Get = 0X3071,
		Lens_Lens1_Set = 0X3072,
		Lens_Lens1_Read = 0X3073,
		Lens_Lens1_Min = 0X3075,
		Lens_Lens1_Max = 0X3076,
		Lens_Lens2_Get = 0X3081,
		Lens_Lens2_Set = 0X3082,
		Lens_Lens2_Read = 0X3083,
		Lens_Lens2_Min = 0X3085,
		Lens_Lens2_Max = 0X3086,
		Lens_Lens3_Read = 0X3093,
		Lens_Lens3_Min = 0X3095,
		Lens_Lens3_Max = 0X3096,
		Lens_Lens3C_Get = 0X30A1,
		Lens_Lens3C_Set = 0X30A2,
		Lens_Lens3F_Get = 0X30B1,
		Lens_Lens3F_Set = 0X30B2,
		Lens_SyncScan1_Get = 0X30C1,
		Lens_SyncScan1_Set = 0X30C2,
		Lens_SyncScan2_Get = 0X30D1,
		Lens_SyncScan2_Set = 0X30D2,
		Lens_SyncScan3A_Get = 0X30E1,
		Lens_SyncScan3A_Set = 0X30E2,
		Lens_SyncScan3B_Get = 0X30F1,
		Lens_SyncScan3B_Set = 0X30F2,
		Lens_SyncScanEnable_Get = 0X3101,
		Lens_SyncScanEnable_Set = 0X3102,
		Lens_SyncScanGain_Get = 0X3111,
		Lens_SyncScanGain_Set = 0X3112,
		Lens_Wobble1_Get = 0X3121,
		Lens_Wobble1_Set = 0X3122,
		Lens_Wobble2_Get = 0X3131,
		Lens_Wobble2_Set = 0X3132,
		Lens_Wobble3_Get = 0X3141,
		Lens_Wobble3_Set = 0X3142,
		Lens_WobbleAmpl1_Get = 0X3151,
		Lens_WobbleAmpl1_Set = 0X3152,
		Lens_WobbleAmpl2_Get = 0X3161,
		Lens_WobbleAmpl2_Set = 0X3162,
		Lens_WobbleAmpl3_Get = 0X3171,
		Lens_WobbleAmpl3_Set = 0X3172,
		Lens_WobbleFreq1_Get = 0X3181,
		Lens_WobbleFreq1_Set = 0X3182,
		Lens_WobbleFreq2_Get = 0X3191,
		Lens_WobbleFreq2_Set = 0X3192,
		Lens_WobbleFreq3_Get = 0X31A1,
		Lens_WobbleFreq3_Set = 0X31A2,
		#endregion

		#region Scan
		Scan_AlignX_Get = 0X4011,
		Scan_AlignX_Set = 0X4012,
		Scan_AlignY_Get = 0X4021,
		Scan_AlignY_Set = 0X4022,
		Scan_AmplitudeX_Get = 0X4031,
		Scan_AmplitudeX_Set = 0X4032,
		Scan_AmplitudeY_Get = 0X4041,
		Scan_AmplitudeY_Set = 0X4042,
		Scan_ExtSelect_Get = 0X4051,
		Scan_ExtSelect_Set = 0X4052,
		Scan_MagnificationX_Get = 0X4061,
		Scan_MagnificationX_Set = 0X4062,
		Scan_MagnificationY_Get = 0X4071,
		Scan_MagnificationY_Set = 0X4072,
		Scan_Rotation_Get = 0X4081,
		Scan_Rotation_Set = 0X4082,
		Scan_FeedbackMode_Get = 0X4091,
		Scan_FeedbackMode_Set = 0X4092,

		#endregion

		#region Stig
		Stig_AlignXAB_Get = 0X5011,
		Stig_AlignXAB_Set = 0X5012,
		Stig_AlignXCD_Get = 0X5021,
		Stig_AlignXCD_Set = 0X5022,
		Stig_AlignYAB_Get = 0X5031,
		Stig_AlignYAB_Set = 0X5032,
		Stig_AlignYCD_Get = 0X5041,
		Stig_AlignYCD_Set = 0X5042,
		Stig_StigX_Get = 0X5051,
		Stig_StigX_Set = 0X5052,
		Stig_StigX_Read = 0X5053,
		Stig_StigX_Min = 0X5055,
		Stig_StigX_Max = 0X5056,
		Stig_StigY_Get = 0X5061,
		Stig_StigY_Set = 0X5062,
		Stig_StigY_Read = 0X5063,
		Stig_StigY_Min = 0X5065,
		Stig_StigY_Max = 0X5066,
		Stig_SyncScanX_Get = 0X5071,
		Stig_SyncScanX_Set = 0X5072,
		Stig_SyncScanY_Get = 0X5081,
		Stig_SyncScanY_Set = 0X5082,
		Stig_WobbleAmplX_Get = 0X5091,
		Stig_WobbleAmplX_Set = 0X5092,
		Stig_WobbleAmplY_Get = 0X50A1,
		Stig_WobbleAmplY_Set = 0X50A2,
		Stig_WobbleFreqX_Get = 0X50B1,
		Stig_WobbleFreqX_Set = 0X50B2,
		Stig_WobbleFreqY_Get = 0X50C1,
		Stig_WobbleFreqY_Set = 0X50C2,
		Stig_WobbleX_Get = 0X50D1,
		Stig_WobbleX_Set = 0X50D2,
		Stig_WobbleY_Get = 0X50E1,
		Stig_WobbleY_Set = 0X50E2,
		#endregion

		#region Vacuum
		Vacuum_Pirani1_Read = 0X6013,
		Vacuum_Pirani1_Min = 0X6013,
		Vacuum_Pirani1_Max = 0X6013,
		Vacuum_Pirani2_Read = 0X6023,
		Vacuum_Pirani2_Min = 0X6023,
		Vacuum_Pirani2_Max = 0X6023,
		Vacuum_Enable_Get = 0X6031,
		Vacuum_Enable_Set = 0X6032,
		Vacuum_RunTime_Read = 0x6053,
		Vacuum_RunTime_Write = 0x6054,
		Vacuum_State_Get = 0x6041,
		Vacuum_State_Info = 0x6047,
		#endregion

		MaximumAddress
	}
}
