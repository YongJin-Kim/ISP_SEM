using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public abstract class AutoFunctionBase : IAutoFunction
	{
		#region Property & Variables
		protected int _Progress = -1;
		public int Progress
		{
			get { return _Progress; }
		}

		protected string _Name = "";
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		protected bool _Cancled = false;
		public bool Cancled
		{
			get { return _Cancled; }
		}

		protected bool _StopVisiable = false;
		public bool StopVisiable
		{
			get { return _StopVisiable; }
		}

		protected bool _CancelVisiable = false;
		public bool CancelVisiable
		{
			get { return _CancelVisiable; }
		}

		protected bool _ProgressbarVisiable = false;
		public bool ProgressbarVisiable
		{
			get { return _ProgressbarVisiable; }
		}

		protected int _SubProcess = -1;
		public int SubProcess
		{
			get { return _SubProcess; }
		}

		protected bool _SubStopVisiable = false;
		public bool SubStopVisiable
		{
			get { return _SubStopVisiable; }
		}

		protected bool _SubCancelVisiable = false;
		public bool SubCancelVisiable
		{
			get { return _SubCancelVisiable; }
		}

		protected bool _SubProgressbarVisiable = false;
		public bool SubProgressbarVisiable
		{
			get { return _SubProgressbarVisiable; }
		}
		#endregion

		#region System Function

		public virtual void SubCancel()
		{
			throw new NotSupportedException();	
		}

		public virtual void SubStop()
		{
			throw new NotSupportedException();	
		}

		public virtual void Cancel()
		{
			throw new NotSupportedException();	
		}

		public virtual void Stop()
		{
			throw new NotSupportedException();	
		}
		#endregion

		#region Event
		public event EventHandler  ProgressChanged;
		protected virtual void OnProgressChanged()
		{
			if (ProgressChanged != null)
			{
				ProgressChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  ProgressComplet;
		protected virtual void OnProgressComplet()
		{
			if (ProgressComplet != null)
			{
				ProgressComplet(this, EventArgs.Empty);
			}
		}

		public event EventHandler  ProgressbarVisiableChanged;
		protected virtual void OnProgressbarVisiableChanged()
		{
			if (ProgressbarVisiableChanged != null)
			{
				ProgressbarVisiableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  CancelVisiableChanged;
		protected virtual void OnCancelVisiableChanged()
		{
			if (CancelVisiableChanged != null)
			{
				CancelVisiableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  StopVisiableChanged;
		protected virtual void OnStopVisiableChanged()
		{
			if (StopVisiableChanged != null)
			{
				StopVisiableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  SubProgressChanged;
		protected virtual void OnSubProgressChanged()
		{
			if (SubProgressChanged != null)
			{
				SubProgressChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  SubCancelVisiableChanged;
		protected virtual void OnSubCancelVisiableChanged()
		{
			if (SubCancelVisiableChanged != null)
			{
				SubCancelVisiableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  SubStopVisiableChanged;
		protected virtual void OnSubStopVisiableChanged()
		{
			if (SubStopVisiableChanged != null)
			{
				SubStopVisiableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  SubProgressbarVisiableChanged;
		protected virtual void OnSubProgressbarVisiableChanged()
		{
			if (SubStopVisiableChanged != null)
			{
				SubProgressbarVisiableChanged(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}
