using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoStage
{
	internal enum StageParser : ushort
	{
		Board = 0xF000,
		Channel = 0x0F00,
		Instruction = 0x00F0,
		Type = 0x000F
	}

	internal enum StageChannel : ushort
	{
		Board = 0x8000,

		ChannelNull = 0x0000,	// System Inst
		Channel1 = 0x0100,
		Channel2 = 0x0200,
		Channel3 = 0x0300,
		Channel4 = 0x0400,
		Channel5 = 0x0500,
		Channel6 = 0x0600
	}

	internal enum StageInst : ushort
	{

		Sys_SearchHome = 0x0010,	// Get(Is Searching), Set(Searching start or stop)
		Sys_LimitSensor = 0x0020,	// Get
		Sys_ModeChange = 0x0030,	// Get, Set
		Sys_UsingeChannel = 0x0040,	// Get, Set
		Sys_VerDate = 0x00E0,	// Get
		Sys_VerTime = 0x00F0,	// Get

		Inst_Position = 0x0010,	// Get(Target), Read(Encoder)
		Inst_Speed = 0x0020,	// Get(Target), Set(Target), Read(Real)
		Inst_LimitCh = 0x0030,	// Get, Set
		Inst_Flag = 0x0040,		// Get, Set
		Inst_PID_P = 0x0050,	// Get, Set
		Inst_PID_I = 0x0060,	// Get, Set
		Inst_PID_D = 0x0070,	// Get, Set
		Inst_DeltaLimit = 0x0080,	// Get, Set
	}

	internal enum StageType : ushort
	{
		Type_Get = 0x0001,
		Type_Set = 0x0002,
		Type_Read = 0x0003,
		Type_Write = 0x004,
		Type_Info = 0x0005,
		Type_MaxGet = 0x0007,
		Type_MaxSet = 0x0008,
		Type_MinGet = 0x0009,
		Type_MinSet = 0x000A
	}

}
