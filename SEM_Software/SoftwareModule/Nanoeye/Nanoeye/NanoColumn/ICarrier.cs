using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.Nanoeye.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	public interface ICarrier : ISEMController
	{
		SECtype.IControlInt VacuumState { get; }
		SECtype.IControlInt VacuumRuntime { get; }
		SECtype.IControlBool CameraPower { get; }
		SECtype.IControlInt StageLock { get; }
		SECtype.IControlInt AirSpring { get; }
		SECtype.IControlInt Door { get; }
		SECtype.IControlInt ValveState { get; }
		SECtype.IControlInt ProcessTime { get; }

		SECtype.IControlBool HvEnable { get; }
		SECtype.IControlDouble HvElectronGun { get; }
		SECtype.IControlDouble HvGrid { get; }
		SECtype.IControlDouble HvFilament { get; }
		SECtype.IControlDouble HvCollector { get; }
		SECtype.IControlDouble HvPmt { get; }

		SECtype.IControlDouble GunAlignX { get; }
		SECtype.IControlDouble GunAlignY { get; }
		SECtype.IControlDouble GunAlignRotation { get; }

		SECtype.IControlDouble BeamShiftX { get; }
		SECtype.IControlDouble BeamShiftY { get; }
		SECtype.IControlDouble BeamShiftRoation { get; }

		SECtype.IControlDouble StigX { get; }
		SECtype.IControlDouble StigXab { get; }
		SECtype.IControlDouble StigXcd { get; }
		SECtype.IControlBool StigXWobbleEnable { get; }
		SECtype.IControlDouble StigXWobbleAmplitude { get; }
		SECtype.IControlDouble StigXWobbleFrequence { get; }
		SECtype.IControlBool StigSyncX { get; }
		SECtype.IControlDouble StigY { get; }
		SECtype.IControlDouble StigYab { get; }
		SECtype.IControlDouble StigYcd { get; }
		SECtype.IControlBool StigYWobbleEnable { get; }
		SECtype.IControlDouble StigYWobbleAmplitude { get; }
		SECtype.IControlDouble StigYWobbleFrequence { get; }
		SECtype.IControlBool StigSyncY { get; }

		SECtype.IControlDouble LensCondenser1 { get; }
		SECtype.IControlInt LensCondenser1Direction { get; }
		SECtype.IControlBool LensCondenser1WobbleEnable { get; }
		SECtype.IControlDouble LensCondenser1WobbleAmplitude { get; }
		SECtype.IControlDouble LensCondenser1WobbleFrequence { get; }

		SECtype.IControlDouble LensCondenser2 { get; }
		SECtype.IControlInt LensCondenser2Direction { get; }
		SECtype.IControlBool LensCondenser2WobbleEnable { get; }
		SECtype.IControlDouble LensCondenser2WobbleAmplitude { get; }
		SECtype.IControlDouble LensCondenser2WobbleFrequence { get; }

		SECtype.IControlDouble LensObjectCoarse { get; }
		SECtype.IControlDouble LensObjectFine { get; }
		SECtype.IControlInt LensObjectDirection { get; }
		SECtype.IControlBool LensObjectWobbleEnable { get; }
		SECtype.IControlDouble LensObjectWobbleAmplitude { get; }
		SECtype.IControlDouble LensObjectWobbleFrequence { get; }

		SECtype.IControlTable LensWDtable { get; }
		SECtype.IControlTable SpotSize { get; }

		SECtype.IControlDouble ScanAmplitudeX { get; }
		SECtype.IControlDouble ScanAmplitudeY { get; }
		SECtype.IControlInt ScanFeedbackMode { get; }
		SECtype.IControlDouble ScanMagnificationX { get; }
		SECtype.IControlDouble ScanMagnificationY { get; }
		SECtype.IControlDouble ScanRotation { get; }

		SECtype.IControlTable ScanMagnificationTable { get; }
	}
}
