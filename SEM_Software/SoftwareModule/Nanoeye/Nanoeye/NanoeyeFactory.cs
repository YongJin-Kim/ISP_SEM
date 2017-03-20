using System;
using System.Runtime.InteropServices;
using SEC.Nanoeye.NanoImage;
using SEC.Nanoeye.NanoColumn;

namespace SEC.Nanoeye
{
	public abstract class NanoeyeFactory 
	{
		public enum NanoeyeType
		{
			MiniSEM,
			//SNE1500M,
			SNE3000M,
			SNE5000M,
			//SH1500,
			//SH3000,
			Evex_MiniSEM,
			SEMTRAC_mini,
			SNE4000M,
            SNE4500M,
            SNE4500P,
            SNE3200M,
            SNE3000MS,
            SNE3000MB
		}

		public static NanoeyeFactory CreateInstance(NanoeyeType nt)
		{
			switch (nt)
			{
			case NanoeyeType.MiniSEM:
			//case NanoeyeType.SNE1500M:
			//case NanoeyeType.SNE3000M:
			//case NanoeyeType.SH1500:
			//case NanoeyeType.SH3000:
			case NanoeyeType.Evex_MiniSEM:
			case NanoeyeType.SEMTRAC_mini:
				return new NanoeyeDevice.MiniSEM();
			case NanoeyeType.SNE5000M:
				return new NanoeyeDevice.NormalSEM();
			case NanoeyeType.SNE4000M:
				return new NanoeyeDevice.SNE4000M();
            case NanoeyeType.SNE4500M:
                return new NanoeyeDevice.SNE4000M();
            case NanoeyeType.SNE4500P:
                return new NanoeyeDevice.AIOsem();
            case NanoeyeType.SNE3200M:
            case NanoeyeType.SNE3000M:
            case NanoeyeType.SNE3000MS:
                return new NanoeyeDevice.SNE4000M();
            case NanoeyeType.SNE3000MB:
                return new NanoeyeDevice.SNE4000M();
			default:
				throw new ArgumentException();
			}
		}

		protected SEC.Nanoeye.NanoImage.IActiveScan _Scanner = null;
		public virtual SEC.Nanoeye.NanoImage.IActiveScan Scanner
		{
			get { return _Scanner; }
		}

		protected SEC.Nanoeye.NanoColumn.ISEMController _Controller = null;
		public virtual SEC.Nanoeye.NanoColumn.ISEMController Controller
		{
			get { return _Controller; }
		}

		protected SEC.Nanoeye.NanoView.INanoView _ControllerCommunicator = null;
		public virtual SEC.Nanoeye.NanoView.INanoView ControllerCommunicator
		{
			get { return _ControllerCommunicator; }
		}

		protected SEC.Nanoeye.NanoStage.IStage _Stage = null;
		public virtual SEC.Nanoeye.NanoStage.IStage Stage
		{
			get { return _Stage; }
		}
	}
}
