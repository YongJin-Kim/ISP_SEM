﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	internal class SemMiniSEM : SEMbase, IMiniSEM
	{
		#region 초기화
		public override void Initialize()
		{
			base.Initialize();

			ScanInit();

			LensInit();

			StigInit();

			BeamShiftInit();

			GunAlignInit();

			HVInit();

			VacuumInit();

			EtcInit();

			_Initialized = true;
		}

		protected virtual void VacuumInit()
		{
			ColumnInt icvi;

			#region VacuumState
			icvi = new Vacuum.VacuumState_Nanoeye001();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "VacuumState";
			icvi.DefaultMax = 0xff;
			icvi.DefaultMin = 0;
			icvi.Maximum = 0xff;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Vacuum_State;
			icvi.readLower = MiniSEM_Devices.Vacuum_State;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit(true);
			controls.Add("VacuumState", icvi);
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

			#region VacuumMode
			icvi = new Vacuum.VacuumMode_Nanoeye001();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "VacuumMode";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Vacuum_LowVacuum;
			icvi.readLower = MiniSEM_Devices.Vacuum_LowVacuum;
			icvi.EndInit();
			controls.Add(icvi.Name, icvi);
			#endregion


            #region VacuumCamera
            icvi = new ColumnInt();
            icvi.BeginInit();
            icvi.Owner = this;
            icvi.Name = "VacuumCamera";
            icvi.DefaultMax = 1;
            icvi.DefaultMin = 0;
            icvi.Maximum = 1;
            icvi.Minimum = 0;
            icvi.Value = 1;
            icvi.Precision = 1d;
            icvi.setter = MiniSEM_Devices.Noting;
            icvi.readLower = MiniSEM_Devices.Vacuum_Camera;
            icvi.readUpper = MiniSEM_Devices.Noting;
            icvi.readlowerConst = 1.0d;
            icvi.readupperConst = 1.0d;
            icvi.EndInit();
            controls.Add(icvi.Name, icvi);
            #endregion

		}

		protected virtual void HVInit()
		{
			AddDoubleControl("HvElectronGun", 1, 0d, 1d, 0d, 0d, 1d / 255d,
							MiniSEM_Devices.Egps_Eghv, MiniSEM_Devices.Egps_EghvCMon, MiniSEM_Devices.Egps_EghvVMon, 200d / 65520d, 30000d / 65536d);

			AddDoubleControl("HvFilament", 1, 0d, 1d, 0d, 0d, 1d / 255d,
								MiniSEM_Devices.Egps_Tip, MiniSEM_Devices.Egps_TipCMon, MiniSEM_Devices.Egps_TipVMon, 1.0d / 6.8d, 22.0d / 10.0d);

			AddDoubleControl("HvGrid", 1d, 0d, 1d, 0d, 0d, 1d / 255d,
								 MiniSEM_Devices.Egps_Grid, MiniSEM_Devices.Egps_GridCMon, MiniSEM_Devices.Egps_GridVMon, 1.0d / 0.44d, 22.0d / 10.0d);

			AddDoubleControl("HvCollector", 1d, 0d, 1d, 0d, 0d, 1d / 255d,
								 MiniSEM_Devices.Egps_Clt, MiniSEM_Devices.Egps_CltCMon, MiniSEM_Devices.Egps_CltVMon, 4000d / 65536d, 10000d / 65536d);

			AddDoubleControl("HvPmt", 1d, 0d, 1d, 0d, 0d, 1d / 255d,
								 MiniSEM_Devices.Egps_Pmt, MiniSEM_Devices.Egps_PmtCMon, MiniSEM_Devices.Egps_PmtVMon, 400d / 65536d, 1000d / 65536d);

			AddBoolControl("HvEnable", false, MiniSEM_Devices.Egps_Enable);
		}

		protected virtual void GunAlignInit()
		{
			SECtype.ControlDouble cdr;
			cdr = new SECtype.ControlDouble();
			cdr.BeginInit();
			cdr.Owner = this;
			cdr.Name = "GunAlignAngle";
			cdr.DefaultMax = 180d;
			cdr.DefaultMin = -180d;
			cdr.Maximum = 180d;
			cdr.Minimum = -180d;
			cdr.Value = 0;
			cdr.Precision = 1d / 10d;
			cdr.EndInit();
			controls.Add(cdr.Name, cdr);


			ColumnDouble icvdX, icvdY;

			#region GunAlignX
			icvdX = new ColumnDouble();
			icvdX.BeginInit();
			icvdX.Owner = this;
			icvdX.Name = "GunAlignX";
			icvdX.DefaultMax = 1d;
			icvdX.DefaultMin = -1d;
			icvdX.Maximum = 1d;
			icvdX.Minimum = -1d;
			icvdX.Value = 0d;
			icvdX.Precision = 1d / 2047d;
			icvdX.setter = MiniSEM_Devices.Align_GunAlignX;
			icvdX.readLower = MiniSEM_Devices.Noting;
			icvdX.readUpper = MiniSEM_Devices.Noting;
			icvdX.readlowerConst = 1.0d;
			icvdX.readupperConst = 1.0d;
			icvdX.EndInit();
			controls.Add(icvdX.Name, icvdX);
			#endregion

			#region GunAlignY
			icvdY = new ColumnDouble();
			icvdY.BeginInit();
			icvdY.Owner = this;
			icvdY.Name = "GunAlignY";
			icvdY.DefaultMax = 1d;
			icvdY.DefaultMin = -1d;
			icvdY.Maximum = 1d;
			icvdY.Minimum = -1d;
			icvdY.Value = 0d;
			icvdY.Precision = 1d / 2047d;
			icvdY.setter = MiniSEM_Devices.Align_GunAlignY;
			icvdY.readLower = MiniSEM_Devices.Noting;
			icvdY.readUpper = MiniSEM_Devices.Noting;
			icvdY.readlowerConst = 1.0d;
			icvdY.readupperConst = 1.0d;
			icvdY.EndInit();
			controls.Add(icvdY.Name, icvdY);
			#endregion

			SECtype.ControlDouble rotateX, rotateY;

			#region GunAlignXRotate
			rotateX = new SEC.GenericSupport.DataType.ControlDouble();
			rotateX.BeginInit();
			rotateX.Owner = this;
			rotateX.Name = "GunAlignXRotate";
			rotateX.DefaultMax = 1d;
			rotateX.DefaultMin = -1d;
			rotateX.Maximum = 1d;
			rotateX.Minimum = -1d;
			rotateX.Value = 0d;
			rotateX.Precision = 1d / 2047d;
			rotateX.EndInit();
			controls.Add(rotateX.Name, rotateX);
			#endregion

			#region GunAlignYRotate
			rotateY = new SEC.GenericSupport.DataType.ControlDouble();
			rotateY.BeginInit();
			rotateY.Owner = this;
			rotateY.Name = "GunAlignYRotate";
			rotateY.DefaultMax = 1d;
			rotateY.DefaultMin = -1d;
			rotateY.Maximum = 1d;
			rotateY.Minimum = -1d;
			rotateY.Value = 0d;
			rotateY.Precision = 1d / 2047d;
			rotateY.EndInit();
			controls.Add(rotateY.Name, rotateY);
			#endregion

			SECtype.Transfrom2DDouble rotation;
			#region GunAlignTransfromRotation
			rotation = new SEC.GenericSupport.DataType.Transfrom2DDouble();
			rotation.BeginInit();
			rotation.Owner = this;
			rotation.Name = "GunAlignTransfromRotation";
			rotation.Angle = cdr;
			rotation.HorizontalReal = icvdX;
			rotation.HorizontalRotated = rotateX;
			rotation.VerticalReal = icvdY;
			rotation.VerticalRotated = rotateY;
			rotation.EndInit();
			controls.Add(rotation.Name, rotation);
			#endregion
		}

		protected virtual void BeamShiftInit()
		{
			SECtype.ControlDouble angle;
			angle = new SECtype.ControlDouble();
			angle.BeginInit();
			angle.Owner = this;
			angle.Name = "BeamShiftAngle";
			angle.DefaultMax = 180d;
			angle.DefaultMin = -180d;
			angle.Maximum = 180d;
			angle.Minimum = -180d;
			angle.Value = 0;
			angle.Precision = 1d / 10d;
			angle.EndInit();
			controls.Add(angle.Name, angle);

			ColumnDouble bsX = new ColumnDouble();
			bsX.BeginInit();
			bsX.Owner = this;
			bsX.Name = "BeamShiftX";
			bsX.DefaultMax = 1d;
			bsX.DefaultMin = -1d;
			bsX.Maximum = 1d;
			bsX.Minimum = -1d;
			bsX.Value = 0d;
			bsX.Precision = 1d / 2047d;
			bsX.setter = MiniSEM_Devices.Align_BeamShiftX;
			bsX.readLower = MiniSEM_Devices.Noting;
			bsX.readUpper = MiniSEM_Devices.Noting;
			bsX.readlowerConst = 1.0d;
			bsX.readupperConst = 1.0d;
			bsX.EndInit();
			controls.Add(bsX.Name, bsX);

			ColumnDouble bsY = new ColumnDouble();
			bsY.BeginInit();
			bsY.Owner = this;
			bsY.Name = "BeamShiftY";
			bsY.DefaultMax = 1d;
			bsY.DefaultMin = -1d;
			bsY.Maximum = 1d;
			bsY.Minimum = -1d;
			bsY.Value = 0d;
			bsY.Precision = 1d / 2047d;
			bsY.setter = MiniSEM_Devices.Align_BeamShiftY;
			bsY.readLower = MiniSEM_Devices.Noting;
			bsY.readUpper = MiniSEM_Devices.Noting;
			bsY.readlowerConst = 1.0d;
			bsY.readupperConst = 1.0d;
			bsY.EndInit();
			controls.Add(bsY.Name, bsY);

			SECtype.ControlDouble xRotate, yRotate;

			xRotate = new SEC.GenericSupport.DataType.ControlDouble();
			xRotate.BeginInit();
			xRotate.Owner = this;
			xRotate.Name = "BeamShiftXRotate";
			xRotate.DefaultMax = 1;
			xRotate.DefaultMin = -1;
			xRotate.Maximum = 1;
			xRotate.Minimum = -1;
			xRotate.Value = 0;
			xRotate.Precision = 1 / 2047d;
			xRotate.EndInit();
			controls.Add(xRotate.Name, xRotate);

			yRotate = new SEC.GenericSupport.DataType.ControlDouble();
			yRotate.BeginInit();
			yRotate.Owner = this;
			yRotate.Name = "BeamShiftYRotate";
			yRotate.DefaultMax = 1;
			yRotate.DefaultMin = -1;
			yRotate.Maximum = 1;
			yRotate.Minimum = -1;
			yRotate.Value = 0;
			yRotate.Precision = 1 / 2047d;
			yRotate.EndInit();
			controls.Add(yRotate.Name, yRotate);

			SECtype.Transfrom2DDouble d2d = new SEC.GenericSupport.DataType.Transfrom2DDouble();
			d2d.BeginInit();
			d2d.Owner = this;
			d2d.Name = "BeamShiftRotationTransform";
			d2d.Angle = angle;
			d2d.PrecisionHorizontal = 1;
			d2d.PrecisionVertical = 1;
			d2d.HorizontalReal = bsX;
			d2d.HorizontalRotated = xRotate;
			d2d.VerticalReal = bsY;
			d2d.VerticalRotated = yRotate;
			d2d.EndInit();
			controls.Add(d2d.Name, d2d);
		}

		protected virtual void StigInit()
		{
			ColumnDouble icvd;
			ColumnBool icvb;

			#region StigX
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigX";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 2047d;
			icvd.setter = MiniSEM_Devices.Stig_StigX;
			icvd.readLower = MiniSEM_Devices.Stig_StigX;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigX", icvd);
			#endregion

			#region StigXab
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigXab";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 5000d;
			icvd.setter = MiniSEM_Devices.Stig_AlignXAB;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigXab", icvd);
			#endregion

			#region StigXcd
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigXcd";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 5000d;
			icvd.setter = MiniSEM_Devices.Stig_AlignXCD;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigXcd", icvd);
			#endregion

			#region StigXWobbleEnable
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "StigXWobbleEnable";
			icvb.setter = MiniSEM_Devices.Stig_WobbleX;
			icvb.EndInit();
			controls.Add("StigXWobbleEnable", icvb);
			#endregion

			#region StigXWobbleAmplitude
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigXWobbleAmplitude";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 128d;
			icvd.setter = MiniSEM_Devices.Stig_WobbleAmplX;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigXWobbleAmplitude", icvd);
			#endregion

			#region StigXWobbleFrequence
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigXWobbleFrequence";
			icvd.DefaultMax = 1.0d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 8d;
			icvd.setter = MiniSEM_Devices.Stig_WobbleFreqX;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigXWobbleFrequence", icvd);
			#endregion

			#region StigY
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigY";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 2047d;
			icvd.setter = MiniSEM_Devices.Stig_StigY;
			icvd.readLower = MiniSEM_Devices.Stig_StigY;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigY", icvd);
			#endregion

			#region StigYab
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigYab";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 5000d;
			icvd.setter = MiniSEM_Devices.Stig_AlignYAB;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigYab", icvd);
			#endregion

			#region StigYcd
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigYcd";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = -1d;
			icvd.Maximum = 1d;
			icvd.Minimum = -1d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 5000d;
			icvd.setter = MiniSEM_Devices.Stig_AlignYCD;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigYcd", icvd);
			#endregion

			#region StigYWobbleEnable
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "StigYWobbleEnable";
			icvb.setter = MiniSEM_Devices.Stig_WobbleY;
			icvb.EndInit();
			controls.Add("StigYWobbleEnable", icvb);
			#endregion

			#region StigYWobbleAmplitude
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigYWobbleAmplitude";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 128d;
			icvd.setter = MiniSEM_Devices.Stig_WobbleAmplY;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigYWobbleAmplitude", icvd);
			#endregion

			#region StigYWobbleFrequence
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "StigYWobbleFrequence";
			icvd.DefaultMax = 1.0d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 8d;
			icvd.setter = MiniSEM_Devices.Stig_WobbleFreqY;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("StigYWobbleFrequence", icvd);
			#endregion

			#region StigSyncX
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Name = "StigSyncX";
			icvb.Owner = this;
			icvb.setter = MiniSEM_Devices.Stig_SyncScanX;
			icvb.EndInit();
			controls.Add(icvb.Name, icvb);
			#endregion

			#region StigSyncY
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Name = "StigSyncY";
			icvb.Owner = this;
			icvb.setter = MiniSEM_Devices.Stig_SyncScanY;
			icvb.EndInit();
			controls.Add(icvb.Name, icvb);
			#endregion

		}

		protected virtual void LensInit()
		{
			ColumnInt icvi;
			ColumnDouble icvd;
			ColumnBool icvb;

			#region LensCondenser1
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser1";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 255d;
			icvd.setter = MiniSEM_Devices.Lens_Lens1;
			icvd.readLower = MiniSEM_Devices.Lens_Lens1;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 2.56d / 16384d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser1", icvd);
			#endregion

			#region LensCondenser1Direction
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "LensCondenser1Direction";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1;
			icvi.setter = MiniSEM_Devices.Lens_Direction1;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit();
			controls.Add("LensCondenser1Direction", icvi);
			#endregion

			#region LensCondenser1WobbleEnable
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "LensCondenser1WobbleEnable";
			icvb.setter = MiniSEM_Devices.Lens_Wobble1;
			icvb.EndInit();
			controls.Add("LensCondenser1WobbleEnable", icvb);
			#endregion

			#region LensCondenser1WobbleAmplitude
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser1WobbleAmplitude";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 128d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleAmpl1;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser1WobbleAmplitude", icvd);
			#endregion

			#region LensCondenser1WobbleFrequence
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser1WobbleFrequence";
			icvd.DefaultMax = 1.0d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 8d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleFreq1;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser1WobbleFrequence", icvd);
			#endregion

			#region LensCondenser2
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser2";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 255d;
			icvd.setter = MiniSEM_Devices.Lens_Lens2;
			icvd.readLower = MiniSEM_Devices.Lens_Lens2;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 2.56d / 16384d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser2", icvd);
			#endregion

			#region LensCondenser2Direction
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "LensCondenser2Direction";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Lens_Direction2;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit();
			controls.Add("LensCondenser2Direction", icvi);
			#endregion

			#region LensCondenser2WobbleEnable
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "LensCondenser2WobbleEnable";
			icvb.setter = MiniSEM_Devices.Lens_Wobble2;
			icvb.EndInit();
			controls.Add("LensCondenser2WobbleEnable", icvb);
			#endregion

			#region LensCondenser2WobbleAmplitude
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser2WobbleAmplitude";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 128d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleAmpl2;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser2WobbleAmplitude", icvd);
			#endregion

			#region LensCondenser2WobbleFrequence
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensCondenser2WobbleFrequence";
			icvd.DefaultMax = 1.0d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 8d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleFreq2;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensCondenser2WobbleFrequence", icvd);
			#endregion

			#region LensObjectCoarse
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensObjectCoarse";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 4095d;
			icvd.setter = MiniSEM_Devices.Lens_Lens3C;
			icvd.readLower = MiniSEM_Devices.Lens_Lens3;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 2.56d / 16384d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensObjectCoarse", icvd);
			#endregion

			#region LensObjectFine
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensObjectFine";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 4095d;
			icvd.setter = MiniSEM_Devices.Lens_Lens3F;
			icvd.readLower = MiniSEM_Devices.Lens_Lens3;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 2.56d / 16384d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensObjectFine", icvd);
			#endregion

			#region LensObjectDirection
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "LensObjectDirection";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Lens_Direction3;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit();
			controls.Add("LensObjectDirection", icvi);
			#endregion

			#region LensObjectWobbleEnable
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "LensObjectWobbleEnable";
			icvb.setter = MiniSEM_Devices.Lens_Wobble3;
			icvb.EndInit();
			controls.Add("LensObjectWobbleEnable", icvb);
			#endregion

			#region LensObjectWobbleAmplitude
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensObjectWobbleAmplitude";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 128d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleAmpl3;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensObjectWobbleAmplitude", icvd);
			#endregion

			#region LensObjectWobbleFrequence
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "LensObjectWobbleFrequence";
			icvd.DefaultMax = 1.0d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 8d;
			icvd.setter = MiniSEM_Devices.Lens_WobbleFreq3;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("LensObjectWobbleFrequence", icvd);
			#endregion

			#region LensObjectWobbleFrequence
			icvb = new ColumnBool();
			icvb.BeginInit();
			icvb.Owner = this;
			icvb.Name = "LensObjectDegauss";
			icvb.Value = false;
			icvb.setter = MiniSEM_Devices.Lens_Degauss3;
			icvb.EndInit();
			controls.Add(icvb.Name, icvb);
			#endregion

			AddBoolControl("LensSyncEnable", false, MiniSEM_Devices.Lens_SyncScanEnable);
			AddBoolControl("LensSyncCL1", false, MiniSEM_Devices.Lens_SyncScan1);
			AddBoolControl("LensSyncCL2", false, MiniSEM_Devices.Lens_SyncScan2);
			AddBoolControl("LensSyncOLC", false, MiniSEM_Devices.Lens_SyncScan3A);
			AddBoolControl("LensSyncOLF", false, MiniSEM_Devices.Lens_SyncScan3B);
			AddDoubleControl("LensSyncGain", 1, -1, 1, -1, 0, 1 / 128d, MiniSEM_Devices.Lens_SyncScanGain, MiniSEM_Devices.Noting, MiniSEM_Devices.Noting, 0, 0);
		}

		protected virtual void ScanInit()
		{
			ColumnInt icvi;
			ColumnDouble icvd;
			Scan.MagTableBase ctb;

			#region ScanAmplitudeX
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "ScanAmplitudeX";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0.707d;
			icvd.Precision = 1d / 1000d;
			icvd.setter = MiniSEM_Devices.Scan_AmplitudeX;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("ScanAmplitudeX", icvd);
			#endregion

			#region ScanAmplitudeY
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "ScanAmplitudeY";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0.707d;
			icvd.Precision = 1d / 1000d;
			icvd.setter = MiniSEM_Devices.Scan_AmplitudeY;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("ScanAmplitudeY", icvd);
			#endregion

			#region ScanFeedbackMode
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "ScanFeedbackMode";
			icvi.DefaultMax = 1;
			icvi.DefaultMin = 0;
			icvi.Maximum = 1;
			icvi.Minimum = 0;
			icvi.Value = 1;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Scan_FeedbackMode;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.readlowerConst = 1.0d;
			icvi.readupperConst = 1.0d;
			icvi.EndInit();
			controls.Add("ScanFeedbackMode", icvi);
			#endregion

			#region ScanMagnificationX
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "ScanMagnificationX";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 1048575.0d;
			icvd.setter = MiniSEM_Devices.Scan_MagnificationX;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("ScanMagnificationX", icvd);
			#endregion

			#region ScanMagnificationY
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "ScanMagnificationY";
			icvd.DefaultMax = 1d;
			icvd.DefaultMin = 0.0d;
			icvd.Maximum = 1d;
			icvd.Minimum = 0d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 1048575.0d;
			icvd.setter = MiniSEM_Devices.Scan_MagnificationY;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("ScanMagnificationY", icvd);
			#endregion

			ctb = new Scan.MagTableBase();
			ctb.BeginInit();
			ctb.Owner = this;
			ctb.Name = "ScanMagTable";
			ctb.FeedbackCvi = controls["ScanFeedbackMode"] as ColumnInt;
			ctb.MagXCvd = controls["ScanMagnificationX"] as ColumnDouble;
			ctb.MagYCvd = controls["ScanMagnificationY"] as ColumnDouble;
			ctb.EndInit();
			controls.Add(ctb.Name, ctb);

			#region ScanRotation
			icvd = new ColumnDouble();
			icvd.BeginInit();
			icvd.Owner = this;
			icvd.Name = "ScanRotation";
			icvd.DefaultMax = 180d;
			icvd.DefaultMin = -180d;
			icvd.Maximum = 180d;
			icvd.Minimum = -180d;
			icvd.Value = 0d;
			icvd.Precision = 1d / 1000.0d;
			icvd.setter = MiniSEM_Devices.Scan_Rotation;
			icvd.readLower = MiniSEM_Devices.Noting;
			icvd.readUpper = MiniSEM_Devices.Noting;
			icvd.readlowerConst = 1.0d;
			icvd.readupperConst = 1.0d;
			icvd.EndInit();
			controls.Add("ScanRotation", icvd);
			#endregion
		}

		protected virtual void EtcInit() { }
		#endregion

		void icvd_CommunicationError(object sender, EventArgs e)
		{
			OnCommunicationErrorOccured(new SECtype.CommunicationErrorOccuredEventArgs(sender as SECtype.IValue));
		}

		public override int ControlBoard(out string[,] information)
		{
			if (_Viewer == null) { information = null; return 0; }

			information = new string[6, 3];

			ushort addr;

			information[0, 0] = "Scan";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Scan_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[0, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Scan_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[0, 2], ".");



			information[1, 0] = "Lens";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Lens_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[1, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Lens_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[1, 2], ".");


			information[2, 0] = "Align";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Align_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[2, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Align_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[2, 2], ".");


			information[3, 0] = "Stig";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Stig_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[3, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Stig_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[3, 2], ".");


			information[4, 0] = "Egps";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Egps_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[4, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Egps_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[4, 2], ".");


			information[5, 0] = "Vacuum";

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Vacuum_Date | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[5, 1], "/");

			addr = (ushort)((ushort)SEC.Nanoeye.NanoColumn.MiniSEM_Devices.Vacuum_Time | (ushort)MiniSEM_DeviceType.Get);
			ControlBoardInfoGet(addr, ref information[5, 2], ".");
			return 6;
		}

		private void ControlBoardInfoGet(ushort addr, ref string info, string se)
		{
			byte[] result;
			uint data;

			result = _Viewer.Send(null,
									addr,
									SEC.Nanoeye.NanoView.PacketFixed8Bytes.MakePacket(addr, 0), true);

			if (result == null)
			{
				info = "Error";
				return;
			}

			SEC.Nanoeye.NanoView.PacketFixed8Bytes.UnPacket(result, out addr, out data);

			info = (data >> 16).ToString() + se + ((data >> 8) & 0x0ff).ToString() + se + (data & 0xff).ToString();
		}

		public override string GetControllerType()
		{
			UInt16 addr;
			UInt32 data;

			addr = (ushort)(MiniSEM_DevicesFullName.Egps_SystemType_Read);

			if (_Viewer == null)
			{
				return "UnConnected";
			}

			byte[] response = _Viewer.Send(null, addr,
				NanoView.PacketFixed8Bytes.MakePacket(addr, 0),
				true);

			if (response == null)
			{
				return "Unknow";
			}
			NanoView.PacketFixed8Bytes.UnPacket(response, out addr, out data);

			switch (data)
			{
			case 1500:
				return "SNE-1500M";
			case 1501:
				return "SH-1500";
			case 3000:
				return "SNE-3000M";
			case 3001:
				return "SH-3000";
			case 3002:
				return "Evex MiniSEM";
			case 3003:
				return "SEMTRAC mini";
			case 5000:
				return "SNE-5000M";
			case 5001:
				return "SNE-5001M";
			default:
				return "Undefined";
			}
		}

		#region IMiniSEM 멤버

		public SECtype.IControlInt VacuumState
		{
			get { return controls["VacuumState"] as SECtype.IControlInt; }
		}

		public SECtype.IControlInt VacuumRuntime
		{
			get { return controls["VacuumRuntime"] as SECtype.IControlInt; }
		}

		public SECtype.IControlBool CameraPower
		{
			get { return controls["CameraPower"] as SECtype.IControlBool; }
		}

		public virtual SECtype.IControlInt VacuumMode
		{
			get { return controls["VacuumMode"] as SECtype.IControlInt; }
		}

		public SECtype.IControlBool HvEnable
		{
			get { return controls["HvEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble HvElectronGun
		{
			get { return controls["HvElectronGun"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble HvGrid
		{
			get { return controls["HvGrid"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble HvFilament
		{
			get { return controls["HvFilament"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble HvCollector
		{
			get { return controls["HvCollector"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble HvPmt
		{
			get { return controls["HvPmt"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble GunAlignX
		{
			get { return controls["GunAlignX"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble GunAlignY
		{
			get { return controls["GunAlignY"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble GunAlignAngle
		{
			get { return controls["GunAlignAngle"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.IControlDouble GunAlignXRotate
		{
			get { return controls["GunAlignXRotate"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.IControlDouble GunAlignYRotate
		{
			get { return controls["GunAlignYRotate"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.ITransform2DDouble GunAlignRotationTransfrom
		{
			get { return controls["GunAlignRotationTransfrom"] as SECtype.ITransform2DDouble; }
		}

		public SECtype.IControlDouble BeamShiftX
		{
			get { return controls["BeamShiftX"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble BeamShiftY
		{
			get { return controls["BeamShiftY"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble BeamShiftAngle
		{
			get { return controls["BeamShiftAngle"] as SECtype.IControlDouble; }
		}


		public SEC.GenericSupport.DataType.IControlDouble BeamShiftXRotate
		{
			get { return controls["BeamShiftXRotate"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.IControlDouble BeamShiftYRotate
		{
			get { return controls["BeamShiftYRotate"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.ITransform2DDouble BeamShiftRotationTransfrom
		{
			get { return controls["BeamShiftRotationTransfrom"] as SECtype.ITransform2DDouble; }
		}

		public SECtype.IControlDouble StigX
		{
			get { return controls["StigX"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigXab
		{
			get { return controls["StigXab"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigXcd
		{
			get { return controls["StigXcd"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlBool StigXWobbleEnable
		{
			get { return controls["StigXWobbleEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble StigXWobbleAmplitude
		{
			get { return controls["StigXWobbleAmplitude"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigXWobbleFrequence
		{
			get { return controls["StigXWobbleFrequence"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlBool StigSyncX
		{
			get { return controls["StigSyncX"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble StigY
		{
			get { return controls["StigY"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigYab
		{
			get { return controls["StigYab"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigYcd
		{
			get { return controls["StigYcd"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlBool StigYWobbleEnable
		{
			get { return controls["StigYWobbleEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble StigYWobbleAmplitude
		{
			get { return controls["StigYWobbleAmplitude"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble StigYWobbleFrequence
		{
			get { return controls["StigYWobbleFrequence"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlBool StigSyncY
		{
			get { return controls["StigSyncY"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble LensCondenser1
		{
			get { return controls["LensCondenser1"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlInt LensCondenser1Direction
		{
			get { return controls["LensCondenser1Direction"] as SECtype.IControlInt; }
		}

		public SECtype.IControlBool LensCondenser1WobbleEnable
		{
			get { return controls["LensCondenser1WobbleEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble LensCondenser1WobbleAmplitude
		{
			get { return controls["LensCondenser1WobbleAmplitude"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensCondenser1WobbleFrequence
		{
			get { return controls["LensCondenser1WobbleFrequence"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensCondenser2
		{
			get { return controls["LensCondenser2"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensCondenser2Ext
		{
			get { return controls["LensCondenser2Ext"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlInt LensCondenser2Direction
		{
			get { return controls["LensCondenser2Direction"] as SECtype.IControlInt; }
		}

		public SECtype.IControlBool LensCondenser2WobbleEnable
		{
			get { return controls["LensCondenser2WobbleEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble LensCondenser2WobbleAmplitude
		{
			get { return controls["LensCondenser2WobbleAmplitude"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensCondenser2WobbleFrequence
		{
			get { return controls["LensCondenser2WobbleFrequence"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensObjectCoarse
		{
			get { return controls["LensObjectCoarse"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensObjectFine
		{
			get { return controls["LensObjectFine"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlInt LensObjectDirection
		{
			get { return controls["LensObjectDirection"] as SECtype.IControlInt; }
		}

		public SECtype.IControlBool LensObjectWobbleEnable
		{
			get { return controls["LensObjectWobbleEnable"] as SECtype.IControlBool; }
		}

		public SECtype.IControlDouble LensObjectWobbleAmplitude
		{
			get { return controls["LensObjectWobbleAmplitude"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble LensObjectWobbleFrequence
		{
			get { return controls["LensObjectWobbleFrequence"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble ScanAmplitudeX
		{
			get { return controls["ScanAmplitudeX"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble ScanAmplitudeY
		{
			get { return controls["ScanAmplitudeY"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlInt ScanFeedbackMode
		{
			get { return controls["ScanFeedbackMode"] as SECtype.IControlInt; }
		}

		public SECtype.IControlDouble ScanMagnificationX
		{
			get { return controls["ScanMagnificationX"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble ScanMagnificationY
		{
			get { return controls["ScanMagnificationY"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlDouble ScanRotation
		{
			get { return controls["ScanRotation"] as SECtype.IControlDouble; }
		}

		public virtual SECtype.ITable ScanMagTable
		{
			get { return controls["ScanMagTable"] as SECtype.ITable; }
		}

        public SECtype.IControlDouble BSEAmpC
        {
            get { return controls["BSEAmpC"] as SECtype.IControlDouble; }
        }

        public SECtype.IControlDouble BSEAmpD
        {
            get { return controls["BSEAmpD"] as SECtype.IControlDouble; }
        }

        public SECtype.IControlInt VacuumCamera
        {
            get { return controls["VacuumCamera"] as SECtype.IControlInt; }
        }
		#endregion
	}
}
