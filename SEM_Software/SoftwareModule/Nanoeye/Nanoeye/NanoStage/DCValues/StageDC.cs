using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage
{
	internal class StageDC : SECtype.ControllerBase, IStageCoare
	{
		#region Propety & Variables
		protected NanoView.NanoViewMultiMaster _Viewer = null;
		public NanoView.INanoView Viewer
		{
			get { return _Viewer; }
		}

		protected bool _SearchHome = false;
		public bool SearchHome
		{
			get { return _SearchHome; }
			set
			{
				_SearchHome = value;
				Send(MakeAddr(StageChannel.ChannelNull, StageInst.Sys_SearchHome, StageType.Type_Set), new byte[1] { (byte)(value ? 0xff : 0x00) });
			}
		}

		protected int _LimitSensor = 0;
		public int LimitSensor
		{
			get { return _LimitSensor; }
		}

		protected int _Mode = 0;
		public int Mode
		{
			get { return _Mode; }
			set
			{
				if (value < 0) { throw new ArgumentException(); }
				if (value > _ModeMax) { throw new ArgumentException(); }

				_Mode = value;
				Send(MakeAddr(StageChannel.ChannelNull, StageInst.Sys_ModeChange, StageType.Type_Set), new byte[1] { (byte)_Mode });
			}
		}

		protected int _ModeMax = 4;
		public int ModeMax
		{
			get { return _ModeMax; }
			set
			{
				if (value < 0) { throw new ArgumentException(); }
				if (value > 7) { throw new ArgumentException(); }

				_ModeMax = value;
				Send(MakeAddr(StageChannel.ChannelNull, StageInst.Sys_UsingeChannel, StageType.Type_Set), new byte[1] { (byte)_ModeMax });
			}
		}
		#endregion

		#region 생성자 및 소멸자 그리고 초기화.

		public override void Initialize()
		{
			if (_Viewer != null)
			{
				if (!(_Viewer is NanoView.NanoViewMultiMaster))
				{
					throw new ArgumentException("Invalied viewer");
				}
			}

			if (_Viewer.PacketControl != null)
			{
				if (!(_Viewer.PacketControl is NanoView.PacketVairable))
				{
					throw new ArgumentException("Invalied Viewer PacketControl.");
				}
			}
			else
			{
				_Viewer.PacketControl = new NanoView.PacketVairable();
			}

			AxisDC ax = new AxisDC();

			ax.BeginInit();
			ax.Name = "X";
			ax.Owner = this;
			ax.EndInit();
			controls.Add("Xaxis", new AxisDC());

			ax.BeginInit();
			ax.Name = "Y";
			ax.Owner = this;
			ax.EndInit();
			controls.Add("Yaxis", new AxisDC());

			ax.BeginInit();
			ax.Name = "R";
			ax.Owner = this;
			ax.EndInit();
			controls.Add("Raxis", new AxisDC());

			ax.BeginInit();
			ax.Name = "T";
			ax.Owner = this;
			ax.EndInit();
			controls.Add("Taxis", new AxisDC());
		}
		#endregion

		public override string[] AvailableDevices()
		{
			throw new NotImplementedException();
		}

		public override int ControlBoard(out string[,] information)
		{
			throw new NotImplementedException();
		}

		public override string GetControllerType()
		{
			return this.ToString();
		}

		#region  통신
		/// <summary>
		/// INanoView로 부터 호출 됨.
		/// </summary>
		/// <param name="datas"></param>
		/// <param name="et"></param>
		public void NanoviewRepose(byte[] datas, SEC.Nanoeye.NanoView.ErrorType et)
		{
			throw new NotImplementedException();
		}

		public void Send(ushort addr, byte[] datas)
		{

		}

		ushort MakeAddr(StageChannel ch, StageInst inst, StageType type)
		{
			return (ushort)((ushort)ch | (ushort)inst | (ushort)type);
		}
		#endregion


		bool[] IStage.LimitSensor
		{
			get { throw new NotImplementedException(); }
		}

		public SECtype.IValue[] GetAxes()
		{
			throw new NotImplementedException();
		}

		public bool IsHomeSearched
		{
			get { throw new NotImplementedException(); }
		}

		public event EventHandler  HomeSearchStateChanged;
		private void OnHomeSearchStaterChanged()
		{
			if (HomeSearchStateChanged != null) { HomeSearchStateChanged(this, EventArgs.Empty); }
		}

		public void HomeSearch(bool value)
		{
			throw new NotImplementedException();
		}

		public void EmergencyStop()
		{
			throw new NotImplementedException();
		}

		#region IStage 멤버


		public bool IsHomeSearching
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IStage 멤버


		public int HomeSearchMinimumSpeedModifier
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IStage 멤버

		bool IStage.IsHomeSearched
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
