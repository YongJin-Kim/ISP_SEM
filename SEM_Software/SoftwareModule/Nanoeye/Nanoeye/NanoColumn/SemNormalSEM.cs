using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class SemNormalSEM : SemMiniSEM, INormalSEM
	{
		#region 초기화
		//protected override void BeamShiftInit()
		//{
		//    base.BeamShiftInit();

		//    SECtype.SplineDouble xDistance = new SEC.GenericSupport.DataType.SplineDouble();
		//    xDistance.BeginInit();
		//    xDistance.Owner = this;
		//    xDistance.Name = "BeamShiftXDistance";
		//    xDistance.DefaultMax = 200000;
		//    xDistance.DefaultMin = -200000;
		//    xDistance.Maximum = 200000;
		//    xDistance.Minimum = -200000;
		//    xDistance.Value = 0;
		//    xDistance.Precision = 1;
		//    xDistance.RealControl = controls["BeamShiftXRotate"] as SECtype.IControlDouble;
		//    xDistance.EndInit();
		//    controls.Add(xDistance.Name, xDistance);

		//    SECtype.SplineDouble yDistance = new SEC.GenericSupport.DataType.SplineDouble();
		//    yDistance.BeginInit();
		//    yDistance.Owner = this;
		//    yDistance.Name = "BeamShiftYDistance";
		//    yDistance.DefaultMax = 200000;
		//    yDistance.DefaultMin = -200000;
		//    yDistance.Maximum = 200000;
		//    yDistance.Minimum = -200000;
		//    yDistance.Value = 0;
		//    yDistance.Precision = 1;
		//    yDistance.RealControl = controls["BeamShiftYRotate"] as SECtype.IControlDouble;
		//    yDistance.EndInit();
		//    controls.Add(yDistance.Name, yDistance);
		//}

		protected override void VacuumInit()
		{
			ColumnInt icvi;
			ColumnBool icvb;

			#region VacuumState
			ColumnValueBase<int> vns;

			string type = GetControllerType();
			System.Diagnostics.Trace.WriteLine("VacuumInit, Device Type = " + type, "SemNormalSEM");

			switch (type)
			{
			case "SNE-5000M":
				vns = new Vacuum.VacuumState_NormalSEM001();
				break;
			case "SNE-5001M":
				vns = new Vacuum.VacuumState_Carryer();
				break;
#if DEBUG
			case "SNE-3000M":
			default:
				vns = new Vacuum.VacuumState_MiniSEM();
				break;
#else
			default:
				//vns = new Vacuum.VacuumState_NormalSEM001();
				throw new InvalidOperationException("Controller is not NORMAL-SEM.");
#endif
			}
			vns.BeginInit();
			vns.Owner = this;
			vns.Name = "VacuumState";
			vns.DefaultMax = 3;
			vns.DefaultMin = -1;
			vns.Maximum = 3;
			vns.Minimum = -1;
			vns.Value = 0;
			vns.Precision = 1d;
			vns.setter = MiniSEM_Devices.Vacuum_State;
			vns.readLower = MiniSEM_Devices.Vacuum_State;
			vns.readUpper = MiniSEM_Devices.Noting;
			vns.readlowerConst = 1.0d;
			vns.readupperConst = 1.0d;
			vns.EndInit(true);
			controls.Add(vns.Name, vns);
			#endregion

			#region VacuumRuntime
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "VacuumRuntime";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 1;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Noting;
			icvi.readLower = MiniSEM_Devices.Vacuum_RunTime;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit();
			controls.Add(icvi.Name, icvi);
			#endregion

			AddBoolControl("CameraPower", false, MiniSEM_Devices.Vacuum_Relay1FromC1);

			#region StageLock
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "StageLock";
			icvb.setter = MiniSEM_Devices.Vacuum_StageLock;
			icvb.EndInit(true);
			controls.Add(icvb.Name, icvb);
			#endregion

			#region AirSpring
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "AirSpring";
			icvb.setter = MiniSEM_Devices.Vacuum_AirSpring;
			icvb.EndInit(true);
			controls.Add(icvb.Name, icvb);
			#endregion

			#region Door
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "Door";
			icvb.setter = MiniSEM_Devices.Vacuum_Door;
			icvb.EndInit(true);
			controls.Add(icvb.Name, icvb);
			#endregion

			#region ValveState
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "ValveState";
			icvi.DefaultMax = 0xffff;
			icvi.DefaultMin = 0;
			icvi.Maximum = 0xffff;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.readLower = MiniSEM_Devices.Vacuum_Valve;
			icvi.EndInit(false);
			controls.Add(icvi.Name, icvi);
			#endregion

			#region ProcessTime
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "ProcessTime";
			icvi.DefaultMax = 0xffff;
			icvi.DefaultMin = 0;
			icvi.Maximum = 0xffff;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.readLower = MiniSEM_Devices.Vacuum_ProcessTime;
			icvi.EndInit(false);
			controls.Add(icvi.Name, icvi);
			#endregion

			#region Pirani Gauge
			Vacuum.Pirani_NormalSEM_WSA pnwsa = new Vacuum.Pirani_NormalSEM_WSA();
			pnwsa.BeginInit();
			pnwsa.Owner = this;
			pnwsa.Name = "Pirani";
			pnwsa.DefaultMax = int.MaxValue;
			pnwsa.DefaultMin = int.MinValue;
			pnwsa.Maximum = int.MaxValue;
			pnwsa.Minimum = int.MinValue;
			pnwsa.Value = 0;
			pnwsa.Precision = 1 / 100000d;
			pnwsa.readLower = MiniSEM_Devices.Vacuum_Pirani2;
			pnwsa.EndInit(false);
			controls.Add(pnwsa.Name, pnwsa);
			#endregion

			#region EDS
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "EDS_Power";
			icvb.setter = MiniSEM_Devices.Vacuum_EDS;
			icvb.EndInit(true);
			controls.Add(icvb.Name, icvb);
			#endregion
		}

		protected override void EtcInit()
		{
			base.EtcInit();

			SECtype.ITable ict;

			/*
			#region WD table
			Lens.WDtableByScanAmplitude wdbsa = new Lens.WDtableByScanAmplitude();
			wdbsa.BeginInit();
			wdbsa.Owner = this;
			wdbsa.Name = "LensWDtable";
			wdbsa.AmplitudeX = ScanAmplitudeX as ColumnDouble;
			wdbsa.AmplitudeY = ScanAmplitudeY as ColumnDouble;
			wdbsa.Obj1 = LensObjectCoarse as ColumnDouble;
			wdbsa.ScanRotation = ScanRotation as ColumnDouble;
			controls.Add(wdbsa.Name, wdbsa);
			wdbsa.EndInit();
			#endregion

			#region Magnification Table
			ict = new NanoColumn.Scan.MagTableWithWD();
			((NanoColumn.Scan.MagTableWithWD)ict).Owner = this;
			((NanoColumn.Scan.MagTableWithWD)ict).Name = "ScanMagnificationTable";
			((NanoColumn.Scan.MagTableWithWD)ict).MagXCvd = (ColumnDouble)controls["ScanMagnificationX"];
			((NanoColumn.Scan.MagTableWithWD)ict).MagYCvd = (ColumnDouble)controls["ScanMagnificationY"];
			((NanoColumn.Scan.MagTableWithWD)ict).FeedbackCvi = (ColumnInt)controls["ScanFeedbackMode"];
			((NanoColumn.Scan.MagTableWithWD)ict).WDtable = wdbsa;
			ict.EndInit();
			controls.Add("ScanMagnificationTable", ict);
			#endregion
			*/

			controls.Remove("ScanMagTable");

			#region WDTable Spline
			//Lens.WDtableSplineWDBase wds = new SEC.Nanoeye.NanoColumn.Lens.WDtableSplineWDBase();
			//wds.BeginInit();
			//wds.Owner = this;
			//wds.Name = "LensWDtableSpline";
			//wds.Obj1 = LensObjectCoarse as ColumnDouble;
			//wds.ScanRotation = ScanRotation as ColumnDouble;
			//wds.EndInit();
			//controls.Add(wds.Name, wds);

			//controls.Remove("LensObjectCoarse");

			Lens.WDSplineObjBase wdsob = new SEC.Nanoeye.NanoColumn.Lens.WDSplineObjBase();
			wdsob.BeginInit();
			wdsob.Owner = this;
			wdsob.Name = "LensWDtableSpline";
			wdsob.BeamShiftRotation = BeamShiftAngle as SECtype.IControlDouble;
            //wdsob.BeamShiftRotation = BeamShiftAngle as ColumnDouble;
			wdsob.ScanRotation = ScanRotation as ColumnDouble;
			wdsob.LensObjCoarse = LensObjectCoarse as ColumnDouble;
			wdsob.EndInit();
			controls.Add(wdsob.Name, wdsob);
			#endregion

			#region MagTable Spline
			ict = new NanoColumn.Scan.MagTableSpline();
			ict.BeginInit();
			((NanoColumn.Scan.MagTableSpline)ict).Owner = this;
			((NanoColumn.Scan.MagTableSpline)ict).Name = "ScanMagSplineTable";
			((NanoColumn.Scan.MagTableSpline)ict).MagXCvd = (ColumnDouble)controls["ScanMagnificationX"];
			((NanoColumn.Scan.MagTableSpline)ict).MagYCvd = (ColumnDouble)controls["ScanMagnificationY"];
			((NanoColumn.Scan.MagTableSpline)ict).FeedbackCvi = (ColumnInt)controls["ScanFeedbackMode"];
			((NanoColumn.Scan.MagTableSpline)ict).MagCorrector = wdsob;
			ict.EndInit();
			controls.Add(ict.Name, ict);
			#endregion
		}
		#endregion

		public SECtype.IControlBool StageLock
		{
			get { return controls["StageLock"] as SECtype.IControlBool; }
		}

		public SECtype.IControlBool AirSpring
		{
			get { return controls["AirSpring"] as SECtype.IControlBool; }
		}

		public SECtype.IControlBool Door
		{
			get { return controls["Door"] as SECtype.IControlBool; }
		}

		public SECtype.IControlInt ValveState
		{
			get { return controls["ValveState"] as SECtype.IControlInt; }
		}

		public SECtype.IControlInt ProcessTime
		{
			get { return controls["ProcessTime"] as SECtype.IControlInt; }
		}

		public SECtype.ITable SpotSize
		{
			get { return controls["SpotSize"] as SECtype.ITable; }
		}

		public SECtype.ITable ScanMagnificationTable
		{
			get { return controls["ScanMagSplineTable"] as SECtype.ITable; }
		}

		public SECtype.IControlDouble PiraniGauge
		{
			get { return controls["Pirani"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlBool EdsPower
		{
			get { return controls["EDS_Power"] as SECtype.IControlBool; }
		}

		public SEC.GenericSupport.DataType.IValue LensWDtable
		{
			//get { return controls["LensWDtable"] as SECtype.IControlTable; }
			get { return controls["LensWDtableSpline"] as SECtype.IValue; }
		}

		public SEC.GenericSupport.DataType.ISplineDouble BeamShiftXDistance
		{
			get { return controls["BeamShiftXDistance"] as SECtype.ISplineDouble; }
		}

		public SEC.GenericSupport.DataType.ISplineDouble BeamShiftYDistance
		{
			get { return controls["BeamShiftYDistance"] as SECtype.ISplineDouble; }
		}
	}
}
