using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage
{
	public abstract class XeyeStageMediator
	{
		#region Peoperty & Variables
		private double _SampleDiameter = 1d;
		/// <summary>
		/// Sample의 지름
		/// um 단위
		/// </summary>
		public double SampleDiameter
		{
			get { return _SampleDiameter; }
			set { _SampleDiameter = value; }
		}

		private double _SampleHeigth = 1d;
		/// <summary>
		/// Sample의 높이
		/// um 단위
		/// </summary>
		public double SampleHeight
		{
			get { return _SampleHeigth; }
			set { _SampleHeigth = value; }
		}
		#endregion

		protected XeyeStageMediator() { }

		public enum StageType
		{
			M5000
		};

		private static XeyeStageMediator mediator = null;
		public static void CreateInstance(StageType st)
		{
			switch (st)
			{
			case StageType.M5000:
				mediator = new SNE_5000M.Mediator();
				break;
			}
		}

		public static XeyeStageMediator Instance
		{
			get
			{
				if (mediator == null)
				{
					throw new Exception();
				}
				return mediator;
			}
		}

		public abstract MotionWrapper GetMotion();

		public enum InputCheckType
		{
			IN_DOOR_CLOSE,
			IN_MAIN_POWER_ON
		}

		public abstract bool GetInput(InputCheckType ict);

		public abstract Geometry GetGeometry();

		public enum UserPermissionCheckType
		{
			DOOR_CHECK
		}

		public abstract bool CheckUserPermission(UserPermissionCheckType upct);

		public abstract double GetPixelSize();

		public abstract double GetSpeed(int ax);
	}
}
