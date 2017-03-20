using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage
{
	internal class AxisDC : SECtype.ControlValueBase, IAxis
	{

		#region IAxis 멤버

		public bool IsDirectionCCW
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

		public SEC.GenericSupport.DataType.IControlLong Speed
		{
			get { throw new NotImplementedException(); }
		}

		public IAxPosition Position
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IControlInt LimitChannel
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IControlInt AccelTime
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IControlInt StopTime
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IControlInt EmergencyStopTime
		{
			get { throw new NotImplementedException(); }
		}

		public SEC.GenericSupport.DataType.IControlBool IsMotion
		{
			get { throw new NotImplementedException(); }
		}

		public long HomePosition
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

		public LimitSensorEnum LimitState
		{
			get { throw new NotImplementedException(); }
		}

		public event EventHandler  LimitStateChanged;
		protected virtual void OnLimitStateChanged()
		{
			if (LimitStateChanged != null)
			{
				LimitStateChanged(this, EventArgs.Empty);
			}
		}

		public void MovePosition(long pos, bool sync)
		{
			throw new NotImplementedException();
		}

		public void MoveOffset(long pos, bool sync)
		{
			throw new NotImplementedException();
		}

		public void MoveVelocity(bool direction)
		{
			throw new NotImplementedException();
		}

		public void Stop(bool emergency)
		{
			throw new NotImplementedException();
		}

		#endregion

		public override void BeginInit()
		{
			throw new NotImplementedException();
		}

		public override void EndInit()
		{
			throw new NotImplementedException();
		}

		public override void EndInit(bool sync)
		{
			throw new NotImplementedException();
		}

		public override void Sync()
		{
			throw new NotImplementedException();
		}

		public override bool Validate()
		{
			throw new NotImplementedException();
		}

		#region IAxis 멤버


		public bool IsEndlessMove
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

		#region IAxis 멤버


		public SEC.GenericSupport.DataType.IControlInt DecelTime
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IAxis 멤버


		public int Resolution
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

		#region IAxis 멤버


		public event EventHandler  MotionStateChanged;
		protected virtual void OnMotionStateChanged()
		{
			if (MotionStateChanged != null)
			{
				MotionStateChanged(this, EventArgs.Empty);
			}
		}

		public int StepDistance
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

		#region IAxis 멤버


		bool IAxis.IsMotion
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IAxis 멤버


		public bool AutoOff
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
