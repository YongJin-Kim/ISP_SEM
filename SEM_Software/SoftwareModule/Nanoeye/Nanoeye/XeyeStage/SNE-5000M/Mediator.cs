using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.XeyeStage.SNE_5000M
{
	public class Mediator : XeyeStageMediator
	{
		public enum AxisNumber : short
		{
			X = 0,
			Y = 1,
			R = 2,
			T = 3,
			Z = 4
		}

		private MotionWrapper mw = null;
		public override MotionWrapper GetMotion()
		{
			if (mw == null)
			{
				mw = new Motion(new MMCMotionManager());
			}
			return mw;
		}

		public override bool GetInput(XeyeStageMediator.InputCheckType ict)
		{
			//throw new NotImplementedException();
			return false;
		}

		public static readonly double StageZDisplayOffset = 46.5d;

		private Geometry gm = null;
		public override SEC.Nanoeye.XeyeStage.Geometry GetGeometry()
		{
			if (gm == null)
			{
				gm = new Geometry();
			}
			return gm;
		}

		public override bool CheckUserPermission(XeyeStageMediator.UserPermissionCheckType upct)
		{
			throw new NotImplementedException();
		}

		public override double GetPixelSize()
		{
			throw new NotImplementedException();
		}

		public override double GetSpeed(int ax)
		{
			throw new NotImplementedException();
		}
	}
}
