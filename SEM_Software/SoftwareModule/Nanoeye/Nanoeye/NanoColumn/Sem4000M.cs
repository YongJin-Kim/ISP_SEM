using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	class Sem4000M : SEMbase, I4000M
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

            BSEInit();

            VariableInit();

			_Initialized = true;
		}

		protected virtual void VacuumInit()
		{
			ColumnInt icvi;

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

			#region LastError
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "VacuumLastError";
			icvi.DefaultMax = int.MaxValue;	// unsigned 임.
			icvi.DefaultMin = int.MinValue;	// unsigned 임.
			icvi.Maximum = int.MaxValue;
			icvi.Minimum = int.MinValue;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Vacuum_LastError;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.EndInit(true);
			controls.Add(icvi.Name, icvi);
			#endregion

			#region VacuumResetCode
			icvi = new ColumnInt();
			icvi.BeginInit();
			icvi.Owner = this;
			icvi.Name = "VacuumResetCode";
			icvi.DefaultMax = int.MaxValue;	// unsigned 임.
			icvi.DefaultMin = int.MinValue;	// unsigned 임.
			icvi.Maximum = int.MaxValue;
			icvi.Minimum = int.MinValue;
			icvi.Value = 0;
			icvi.Precision = 1d;
			icvi.setter = MiniSEM_Devices.Vacuum_ResetCode;
			icvi.readLower = MiniSEM_Devices.Noting;
			icvi.readUpper = MiniSEM_Devices.Noting;
			icvi.EndInit(true);
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
            icvi.Value = 0;
            icvi.Precision = 1d;
            icvi.setter = MiniSEM_Devices.Vacuum_Camera;
            icvi.readLower = MiniSEM_Devices.Vacuum_Camera;
            icvi.readUpper = MiniSEM_Devices.Noting;
            icvi.EndInit(true);
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
			rotation.Name = "GunAlignRotation";
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


            ColumnDoubleRotation bsX, bsY;

            #region BeamShiftX
            bsX = new ColumnDoubleRotation();
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

            #endregion

            #region BeamShiftY
            bsY = new ColumnDoubleRotation();
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
            #endregion

            bsX.RotationValue = (SECtype.IControlDouble)angle;
            bsX.AxisType = ColumnDoubleRotation.AxisTypeEnum.X;
            bsX.RelatedAxis = bsY;

            bsY.RotationValue = (SECtype.IControlDouble)angle;
            bsY.AxisType = ColumnDoubleRotation.AxisTypeEnum.Y;
            bsY.RelatedAxis = bsX;

            bsX.EndInit();
            bsY.EndInit();
            controls.Add(bsX.Name, bsX);
            controls.Add(bsY.Name, bsY);

            //ColumnDouble bsX = new ColumnDouble();
            //bsX.BeginInit();
            //bsX.Owner = this;
            //bsX.Name = "BeamShiftX";
            //bsX.DefaultMax = 1d;
            //bsX.DefaultMin = -1d;
            //bsX.Maximum = 1d;
            //bsX.Minimum = -1d;
            //bsX.Value = 0d;
            //bsX.Precision = 1d / 2047d;
            //bsX.setter = MiniSEM_Devices.Align_BeamShiftX;
            //bsX.readLower = MiniSEM_Devices.Noting;
            //bsX.readUpper = MiniSEM_Devices.Noting;
            //bsX.readlowerConst = 1.0d;
            //bsX.readupperConst = 1.0d;
            //bsX.EndInit();
            //controls.Add(bsX.Name, bsX);

            //ColumnDouble bsY = new ColumnDouble();
            //bsY.BeginInit();
            //bsY.Owner = this;
            //bsY.Name = "BeamShiftY";
            //bsY.DefaultMax = 1d;
            //bsY.DefaultMin = -1d;
            //bsY.Maximum = 1d;
            //bsY.Minimum = -1d;
            //bsY.Value = 0d;
            //bsY.Precision = 1d / 2047d;
            //bsY.setter = MiniSEM_Devices.Align_BeamShiftY;
            //bsY.readLower = MiniSEM_Devices.Noting;
            //bsY.readUpper = MiniSEM_Devices.Noting;
            //bsY.readlowerConst = 1.0d;
            //bsY.readupperConst = 1.0d;
            //bsY.EndInit();
            //controls.Add(bsY.Name, bsY);

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
			d2d.Enable = false;

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
			icvd.readLower = MiniSEM_Devices.Noting;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
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
			icvd.readLower = MiniSEM_Devices.Noting;
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
			icvd.readLower = MiniSEM_Devices.Noting;
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

			#region ScanDynamicFocus
            //icvd = new ColumnDouble();
            //icvd.BeginInit();
            //icvd.Owner = this;
            //icvd.Name = "ScanDynamicFocus";
            //icvd.DefaultMax = 1d;
            //icvd.DefaultMin = 0.0d;
            //icvd.Maximum = 1d;
            //icvd.Minimum = 0d;
            //icvd.Value = 0d;
            //icvd.Precision = 1d / 4095d;
            //icvd.setter = MiniSEM_Devices.Scan_DynamicFocus;
            //icvd.readLower = MiniSEM_Devices.Noting;
            //icvd.readUpper = MiniSEM_Devices.Noting;
            //icvd.readlowerConst = 1.0d;
            //icvd.readupperConst = 1.0d;
            //icvd.EndInit();
            //controls.Add(icvd.Name, icvd);
			#endregion
		}

		protected virtual void EtcInit()
		{
			#region WDTable Spline
			Lens.WDSplineObjBase wdsob = new SEC.Nanoeye.NanoColumn.Lens.WDSplineObjBase();
			wdsob.BeginInit();
			wdsob.Owner = this;
			wdsob.Name = "LensWDtableSpline";
			wdsob.BeamShiftRotation = BeamShiftAngle as SECtype.IControlDouble;
			wdsob.ScanRotation = ScanRotation as ColumnDouble;
			wdsob.LensObjCoarse = LensObjectCoarse as ColumnDouble;
			wdsob.EndInit();
			controls.Add(wdsob.Name, wdsob);
			#endregion

			#region MagTable Spline
			SECtype.ITable ict = new NanoColumn.Scan.MagTableSpline();
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

        private void BSEInit()
        {

            #region BSE_Detector
            ColumnInt BSE_Detector;

            BSE_Detector = new ColumnInt();

            BSE_Detector.BeginInit();
            BSE_Detector.Owner = this;
            BSE_Detector.Name = "BSE_Detector";
            BSE_Detector.DefaultMax = 0xff;
            BSE_Detector.DefaultMin = 0;
            BSE_Detector.Maximum = 0xff;
            BSE_Detector.Minimum = 0;
            BSE_Detector.Value = 0;
            BSE_Detector.Precision = 1d;
            BSE_Detector.setter = MiniSEM_Devices.BSE_Detector;
            BSE_Detector.readLower = MiniSEM_Devices.BSE_Detector;
            BSE_Detector.readUpper = MiniSEM_Devices.Noting;
            BSE_Detector.readlowerConst = 1.0d;
            BSE_Detector.readupperConst = 1.0d;
            BSE_Detector.EndInit(true);
            controls.Add("BSE_Detector", BSE_Detector);
            #endregion

            #region BSE_Filter
            ColumnInt BSE_Filter;

            BSE_Filter = new ColumnInt();

            BSE_Filter.BeginInit();
            BSE_Filter.Owner = this;
            BSE_Filter.Name = "BSE_Filter";
            BSE_Filter.DefaultMax = 0xff;
            BSE_Filter.DefaultMin = 0;
            BSE_Filter.Maximum = 0xff;
            BSE_Filter.Minimum = 0;
            BSE_Filter.Value = 0;
            BSE_Filter.Precision = 1d;
            BSE_Filter.setter = MiniSEM_Devices.BSE_Filter;
            BSE_Filter.readLower = MiniSEM_Devices.BSE_Filter;
            BSE_Filter.readUpper = MiniSEM_Devices.Noting;
            BSE_Filter.readlowerConst = 1.0d;
            BSE_Filter.readupperConst = 1.0d;
            BSE_Filter.EndInit(true);
            controls.Add("BSE_Filter", BSE_Filter);
            #endregion

            //#region BSE_Scroller
            //ColumnInt BSE_Scroller;

            //BSE_Scroller = new ColumnInt();

            //BSE_Scroller.BeginInit();
            //BSE_Scroller.Owner = this;
            //BSE_Scroller.Name = "BSE_Scroller";
            //BSE_Scroller.DefaultMax = 0xff;
            //BSE_Scroller.DefaultMin = 0;
            //BSE_Scroller.Maximum = 0xff;
            //BSE_Scroller.Minimum = 0;
            //BSE_Scroller.Value = 0;
            //BSE_Scroller.Precision = 1d;
            //BSE_Scroller.setter = MiniSEM_Devices.BSE_Scroller;
            //BSE_Scroller.readLower = MiniSEM_Devices.BSE_Scroller;
            //BSE_Scroller.readUpper = MiniSEM_Devices.Noting;
            //BSE_Scroller.readlowerConst = 1.0d;
            //BSE_Scroller.readupperConst = 1.0d;
            //BSE_Scroller.EndInit(true);
            //controls.Add("BSE_Scroller", BSE_Scroller);
            //#endregion

            #region BSE_Amp
            ColumnInt BSE_Amp;

            BSE_Amp = new ColumnInt();

            BSE_Amp.BeginInit();
            BSE_Amp.Owner = this;
            BSE_Amp.Name = "BSE_Amp";
            BSE_Amp.DefaultMax = 0xff;
            BSE_Amp.DefaultMin = 0x01;
            BSE_Amp.Maximum = 0xff;
            BSE_Amp.Minimum = 0x01;
            BSE_Amp.Value = 2;
            BSE_Amp.Precision = 1d;
            BSE_Amp.setter = MiniSEM_Devices.BSE_Amp;
            BSE_Amp.readLower = MiniSEM_Devices.BSE_Amp;
            BSE_Amp.readUpper = MiniSEM_Devices.Noting;
            BSE_Amp.readlowerConst = 1.0d;
            BSE_Amp.readupperConst = 1.0d;
            BSE_Amp.EndInit(true);
            controls.Add("BSE_Amp", BSE_Amp);
            #endregion

            //#region BSE_AmpC
            //ColumnDouble BSE_AmpC;

            //BSE_AmpC = new ColumnDouble();

            //BSE_AmpC.BeginInit();
            //BSE_AmpC.Owner = this;
            //BSE_AmpC.Name = "BSE_AmpC";
            //BSE_AmpC.DefaultMax = 0xff;
            //BSE_AmpC.DefaultMin = 0x01;
            //BSE_AmpC.Maximum = 0xff;
            //BSE_AmpC.Minimum = 0x01;
            //BSE_AmpC.Value = 1;
            //BSE_AmpC.Precision = 1d;
            //BSE_AmpC.setter = MiniSEM_Devices.BSE_AmpC;
            //BSE_AmpC.readLower = MiniSEM_Devices.BSE_AmpC;
            //BSE_AmpC.readUpper = MiniSEM_Devices.Noting;
            //BSE_AmpC.readlowerConst = 1.0d;
            //BSE_AmpC.readupperConst = 1.0d;
            //BSE_AmpC.EndInit(true);
            //controls.Add("BSE_AmpC", BSE_AmpC);
            //#endregion

            //#region BSE_AmpD
            //ColumnDouble BSE_AmpD;

            //BSE_AmpD = new ColumnDouble();

            //BSE_AmpD.BeginInit();
            //BSE_AmpD.Owner = this;
            //BSE_AmpD.Name = "BSE_AmpD";
            //BSE_AmpD.DefaultMax = 0xff;
            //BSE_AmpD.DefaultMin = 0x01;
            //BSE_AmpD.Maximum = 0xff;
            //BSE_AmpD.Minimum = 0x01;
            //BSE_AmpD.Value = 1;
            //BSE_AmpD.Precision = 1d;
            //BSE_AmpD.setter = MiniSEM_Devices.BSE_AmpD;
            //BSE_AmpD.readLower = MiniSEM_Devices.BSE_AmpD;
            //BSE_AmpD.readUpper = MiniSEM_Devices.Noting;
            //BSE_AmpD.readlowerConst = 1.0d;
            //BSE_AmpD.readupperConst = 1.0d;
            //BSE_AmpD.EndInit(true);
            //controls.Add("BSE_AmpD", BSE_AmpD);
            //#endregion




            //throw new NotImplementedException();
        }


        private void VariableInit()
        {

            //#region Variable Aperture

            //ColumnDouble icvd;
            //icvd = new ColumnDouble();
            //icvd.BeginInit();
            //icvd.Owner = this;
            //icvd.Name = "VriableX";
            //icvd.DefaultMax = 1d;
            //icvd.DefaultMin = 0d;
            //icvd.Maximum = 1d;
            //icvd.Minimum = 0d;
            //icvd.Value = 0.5d;
            //icvd.Precision = 1d / 4095d;
            //icvd.setter = MiniSEM_Devices.VariableX;
            //icvd.readLower = MiniSEM_Devices.Noting;
            //icvd.readUpper = MiniSEM_Devices.Noting;
            //icvd.readlowerConst = 1.0d;
            //icvd.readupperConst = 1.0d;
            //icvd.EndInit();
            //controls.Add("VriableX", icvd);


            //icvd = new ColumnDouble();
            //icvd.BeginInit();
            //icvd.Owner = this;
            //icvd.Name = "VriableY";
            //icvd.DefaultMax = 1d;
            //icvd.DefaultMin = 0d;
            //icvd.Maximum = 1d;
            //icvd.Minimum = 0d;
            //icvd.Value = 0.5d;
            //icvd.Precision = 1d / 4095d;
            //icvd.setter = MiniSEM_Devices.VariableY;
            //icvd.readLower = MiniSEM_Devices.Noting;
            //icvd.readUpper = MiniSEM_Devices.Noting;
            //icvd.readlowerConst = 1.0d;
            //icvd.readupperConst = 1.0d;
            //icvd.EndInit();
            //controls.Add("VriableY", icvd);
            //#endregion


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
		public SECtype.IControlDouble PiraniGauge
		{
			get { return controls["Pirani"] as SECtype.IControlDouble; }
		}

		public SECtype.IControlInt VacuumState
		{
			get { return controls["VacuumState"] as SECtype.IControlInt; }
		}

		public SECtype.IControlInt VacuumRuntime
		{
			get { return controls["VacuumRuntime"] as SECtype.IControlInt; }
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

		public virtual SECtype.ITable ScanMagnificationTable
		{
			get { return controls["ScanMagSplineTable"] as SECtype.ITable; }
		}

		public SEC.GenericSupport.DataType.IValue LensWDtable
		{
			get { return controls["LensWDtableSpline"] as SECtype.IValue; }
		}

        public SECtype.IControlInt BSE_Amp
        {
            get { return controls["BSE_Amp"] as SECtype.IControlInt; }
        }

        public SECtype.IControlDouble BSE_AmpC
        {
            get { return controls["BSE_AmpC"] as SECtype.IControlDouble; }
        }

        public SECtype.IControlDouble BSE_AmpD
        {
            get { return controls["BSE_AmpD"] as SECtype.IControlDouble; }
        }

        public SECtype.IControlInt VacuumCamera
        {
            get { return controls["VacuumCamera"] as SECtype.IControlInt; }
        }

		#endregion

		#region I4000M 멤버
		public SEC.GenericSupport.DataType.IControlDouble ScanDynamicFocus
		{
			get { return controls["ScanDynamicFocus"] as SECtype.IControlDouble; }
		}

		public SEC.GenericSupport.DataType.IControlInt VacuumLastError
		{
			get { return controls["VacuumLastError"] as SECtype.IControlInt; }
		}

		public SEC.GenericSupport.DataType.IControlInt VacuumResetCode
		{
			get { return controls["VacuumResetCode"] as SECtype.IControlInt; }
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
			get { return controls["GunAlignRotation"] as SECtype.ITransform2DDouble; }
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
			get { return controls["BeamShiftRotationTransform"] as SECtype.ITransform2DDouble; }
		}






        public SEC.GenericSupport.DataType.IControlDouble ScanAmpX
        {
            get { return controls["ScanAmpX"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble ScanAmpY
        {
            get { return controls["ScanAmpY"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble ScanGeoXA
        {
            get { return controls["ScanGeoXA"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble ScanGeoXB
        {
            get { return controls["ScanGeoXB"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble ScanGeoYA
        {
            get { return controls["ScanGeoYA"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble ScanGeoYB
        {
            get { return controls["ScanGeoYB"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble Scan_AmpXA
        {
            get { return controls["Scan_AmpXA"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble Scan_AmpXB
        {
            get { return controls["Scan_AmpXB"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble Scan_AmpYA
        {
            get { return controls["Scan_AmpYA"] as SECtype.IControlDouble; }
        }


        public SEC.GenericSupport.DataType.IControlDouble Scan_AmpYB
        {
            get { return controls["Scan_AmpYB"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble VariableX
        {
            get { return controls["VariableX"] as SECtype.IControlDouble; }
        }

        public SEC.GenericSupport.DataType.IControlDouble VariableY
        {
            get { return controls["VariableY"] as SECtype.IControlDouble; }
        }


		#endregion
	}
}
