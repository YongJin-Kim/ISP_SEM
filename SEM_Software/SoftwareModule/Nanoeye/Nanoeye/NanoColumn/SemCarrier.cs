using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.Nanoeye.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class SemCarrier : SemMiniSEM, ICarrier
	{
		protected override void VacuumInit()
		{
			ColumnInt icvi;
			ColumnBool icvb;

			#region VacuumState
			Vacuum.VacuumState_NormalSEM001 vns = new Vacuum.VacuumState_NormalSEM001();
			vns.BeginInit();
			vns.Owner = this;
			vns.Name = "VacuumState";
			vns.DefaultMax = 2;
			vns.DefaultMin = 0;
			vns.Maximum = 2;
			vns.Minimum = 0;
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
			controls.Add( icvi.Name, icvi );
			#endregion

			AddBoolControl( "CameraPower", false, MiniSEM_Devices.Vacuum_Relay1FromC1 );


			#region StageLock
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "StageLock";
			icvb.setter = MiniSEM_Devices.Vacuum_StageLock;
			icvb.EndInit( true );
			controls.Add( icvb.Name, icvb );
			#endregion

			#region AirSpring
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "AirSpring";
			icvb.setter = MiniSEM_Devices.Vacuum_AirSpring;
			icvb.EndInit( true );
			controls.Add( icvb.Name, icvb );
			#endregion

			#region Door
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "Door";
			icvb.setter = MiniSEM_Devices.Vacuum_Door;
			icvb.EndInit( true );
			controls.Add( icvb.Name, icvb );
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
			icvi.EndInit( false );
			controls.Add( icvi.Name, icvi );
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
			icvi.EndInit( false );
			controls.Add( icvi.Name, icvi );
			#endregion
		}

		protected override void LensInit()
		{
			base.LensInit();

			Lens.SpotSizeTable sst = new SEC.Nanoeye.NanoColumn.Lens.SpotSizeTable();
			sst.Owner = this;
			sst.Name = "SpotSize";
			sst.Cl1ValueCd = (ColumnDouble)controls["LensCondenser1"];
			sst.Cl1ReverseCi = (ColumnInt)controls["LensCondenser1Direction"];
			sst.Cl2ValueCd = (ColumnDouble)controls["LensCondenser2"];
			sst.Cl2ReverseCi = (ColumnInt)controls["LensCondenser2Direction"];
			this.controls.Add(sst.Name, sst);

			DataType.IControlTable ict = new Lens.StaticWorkingDistanceTable();
			((Lens.StaticWorkingDistanceTable)ict).Name = "LensWDtable";
			((Lens.StaticWorkingDistanceTable)ict).Owner = this;
			((Lens.StaticWorkingDistanceTable)ict).Obj1 = (ColumnDouble)controls["LensObjectCoarse"];
			controls.Add("LensWDtable", ict);
		}

		protected override void EtcInit()
		{
			base.EtcInit();

			SECtype.IControlTable ict;

			#region Magnification Table
			ict = new NanoColumn.Scan.MagTable();
			((NanoColumn.Scan.MagTable)ict).Owner = this;
			((NanoColumn.Scan.MagTable)ict).Name = "ScanMagnificationTable";
			((NanoColumn.Scan.MagTable)ict).MagXCvd = (ColumnDouble)controls["ScanMagnificationX"];
			((NanoColumn.Scan.MagTable)ict).MagYCvd = (ColumnDouble)controls["ScanMagnificationY"];
			((NanoColumn.Scan.MagTable)ict).FeedbackCvi = (ColumnInt)controls["ScanFeedbackMode"];
			((NanoColumn.Scan.MagTable)ict).AmpXCvd = (ColumnDouble)controls["ScanAmplitudeX"];
			((NanoColumn.Scan.MagTable)ict).AmpYCvd = (ColumnDouble)controls["ScanAmplitudeY"];
			((NanoColumn.Scan.MagTable)ict).WDtable = (Lens.StaticWorkingDistanceTable)controls["LensWDtable"];
			((NanoColumn.Scan.MagTable)ict).SetStyle((int)EnumIControlTableSetStyle.Scan_Mag_WithWD, 1);
			controls.Add("ScanMagnificationTable", ict);
			ict.EndInit();
			#endregion
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		#region ICarrier 멤버
		public override SEC.Nanoeye.DataType.IControlInt VacuumMode
		{
			get { return controls["ScanAmplitudeY"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlInt StageLock
		{
			get { return controls["StageLock"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlInt AirSpring
		{
			get { return controls["AirSpring"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlInt Door
		{
			get { return controls["Door"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlInt ValveState
		{
			get { return controls["ValveState"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlInt ProcessTime
		{
			get { return controls["ProcessTime"] as SECtype.IControlInt; }
		}

		public SEC.Nanoeye.DataType.IControlTable LensWDtable
		{
			get { return controls["LensWDtable"] as SECtype.IControlTable; }
		}

		public SEC.Nanoeye.DataType.IControlTable SpotSize
		{
			get { return controls["SpotSize"] as SECtype.IControlTable; }
		}

		public SEC.Nanoeye.DataType.IControlTable ScanMagnificationTable
		{
			get { return controls["ScanMagnificationTable"] as SECtype.IControlTable; }
		}

		#endregion
	}
}
