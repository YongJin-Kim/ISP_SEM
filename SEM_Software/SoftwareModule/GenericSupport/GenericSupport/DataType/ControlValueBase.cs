using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public abstract class ControlValueBase : IValue
	{
		#region Property & Variables
		protected bool _IsInited = false;
		public bool IsInitied
		{
			get { return _IsInited; }
		}

		protected string _Name = null;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		protected Object _Owner = null;
		public virtual Object Owner
		{
			get { return _Owner; }
			set { _Owner = value; }
		}

		protected bool _Enable = true;
		public virtual bool Enable
		{
			get { return _Enable; }
			set
			{
				if (_Enable != value)
				{
					_Enable = value;
					OnEnableChanged();
				}
			}
		}

		protected bool _ThrowException = false;
		public bool ThrowException
		{
			get { return _ThrowException; }
			set { _ThrowException = value; }
		}
		#endregion

		#region Event
		public event EventHandler  EnableChanged;
		protected virtual void OnEnableChanged()
		{
			if (EnableChanged != null)
			{
				EnableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler  NoResponse;
		protected virtual void OnNoResponse()
		{
			if ( NoResponse != null )
			{
				NoResponse(this, EventArgs.Empty);
			}

			if (_Owner is ControllerBase)
			{
				ControllerBase cb = _Owner as ControllerBase;
				cb.CommunicationErrorInControlValue(this);
			}
		}

		public event EventHandler  CommunicationError;
		protected virtual void OnCommunicationError()
		{
			if ( CommunicationError != null )
			{
				CommunicationError(this, EventArgs.Empty);
			}

			if (_Owner is ControllerBase)
			{
				ControllerBase cb = _Owner as ControllerBase;
				cb.CommunicationErrorInControlValue(this);
			}
		}
		#endregion

		public abstract void BeginInit();

		public virtual void EndInit() 
		{
			if (_Enable) { EndInit(false); }
		}

		public abstract void EndInit(bool sync);

		public abstract void Sync();

		public abstract bool Validate();

		public override string ToString()
		{
			return this.Name;
		}

		~ControlValueBase()
		{
			Dispose();
		}
		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public virtual object[] Read
		{
			get { throw new NotSupportedException(); }
		}
	}
}
