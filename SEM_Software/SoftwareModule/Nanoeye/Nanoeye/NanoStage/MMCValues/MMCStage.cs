using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SEC.Nanoeye.NanoStage.MMCValues;
using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage.MMCValues
{
	internal abstract class MMCStage : SECtype.ControllerBase, IStage
	{
		#region Property & Variables
		protected System.Threading.Timer syncTimer;

		public abstract bool[] LimitSensor { get; }

		protected bool _IsHomeSearched = false;
		public bool IsHomeSearched
		{
			get { return _IsHomeSearched; }
			set
			{
				if (_IsHomeSearched != value)
				{
					_IsHomeSearched = value;
					OnHomeSearchStateChanged();
				}
			}
		}

		protected bool _IsHomeSearching = false;
		public bool IsHomeSearching
		{
			get { return _IsHomeSearching; }
		}

		protected int _HomeSearchMinimumSpeedModifier = 1;
		public virtual int HomeSearchMinimumSpeedModifier
		{
			get { return _HomeSearchMinimumSpeedModifier; }
			set
			{
				if(value < 1) { throw new ArgumentException("value must be greater then 1."); }

				_HomeSearchMinimumSpeedModifier = value;
			}
		}
		#endregion

		public MMCStage()
		{
			syncTimer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerSync));
		}

        public override void Dispose()
        {
            syncTimer.Dispose();
            base.Dispose();
        }

		#region Event
		public event EventHandler  HomeSearchStateChanged;
		protected virtual void OnHomeSearchStateChanged()
		{
			if (HomeSearchStateChanged != null)
			{
				HomeSearchStateChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		#region 시스템 정보
		public override string[] AvailableDevices()
		{
			List<string> result = new List<string>();
			int addr;
			try
			{
				unchecked { addr = (int)(0xD8000000); }
				MMCAPICollection.ErrorControl.Assert(MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8400000); }
				MMCAPICollection.ErrorControl.Assert(MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8800000); }
				MMCAPICollection.ErrorControl.Assert(MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			try
			{
				unchecked { addr = (int)(0xD8C00000); }
				MMCAPICollection.ErrorControl.Assert(MMCAPICollection.Initialize.mmc_initx(1, ref addr));
				result.Add(addr.ToString());
			}
			catch (System.IO.IOException)
			{
			}
			return result.ToArray();
		}

		public override int ControlBoard(out string[,] information)
		{
			short num = 0;
			int temp = 0;

			information = new string[4, 2];

			information[0, 0] = "Version";
			MMCAPICollection.MMCConfig.version_chk(0, ref num);
			information[0, 1] = num.ToString();

			information[1, 0] = "Board FPGA";
			MMCAPICollection.ETC.motion_fpga_version_chk(0, ref num);
			information[1, 1] = num.ToString();

			information[2, 0] = "Option FPGA";
			MMCAPICollection.ETC.option_fpga_version_chk(0, ref num);
			information[2, 1] = num.ToString();

			information[3, 0] = "DPRAM";
			MMCAPICollection.Initialize.get_dpram_addr(0, ref temp);
			information[3, 1] = num.ToString();
			return 4;
		}

		public override string GetControllerType()
		{
			return "StageControlByMMC";
		}
		#endregion

		public virtual void EmergencyStop()
		{
			foreach (KeyValuePair<string, SECtype.IValue> ax in controls)
			{
				(ax.Value as IAxis).Stop(true);
			}
		}

		public abstract void HomeSearch(bool value);

		protected virtual void TimerSync(object info)
		{
            if (controls == null) { return; }

			foreach (KeyValuePair<string, SECtype.IValue> kvp in controls)
			{
				kvp.Value.Sync();
			}
		}
	}
}
