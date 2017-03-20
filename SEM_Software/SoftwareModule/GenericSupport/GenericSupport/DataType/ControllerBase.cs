using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public abstract class ControllerBase : IController
	{
		protected Dictionary<string, IValue> controls = new Dictionary<string, IValue>();

		protected bool _Initialized = false;
		public bool Initialized
		{
			get { return _Initialized; }
		}

		protected string _Name = null;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		protected bool _IsDisposed = false;
		public bool IsDisposed
		{
			get { return _IsDisposed; }
		}

		public IValue this[string name]
		{
			get { return controls[name]; }
		}

		protected string _Device = null;
		public string Device
		{
			get
			{
				return _Device;
			}
			set
			{
				if (_Initialized) { 
					throw new InvalidOperationException("Controller is running."); 
				}
				_Device = value;
			}
		}

		~ControllerBase()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			if (!_IsDisposed)
			{
				controls = null;
				_IsDisposed = true;
			}
		}

		public abstract string[] AvailableDevices();

		public abstract void Initialize();

		public abstract int ControlBoard(out string[,] information);

		public abstract string GetControllerType();

		protected bool _Enable = true;
		public virtual bool Enable
		{
			get { return _Enable; }
			set
			{
				if (_Enable != value)
				{
					_Enable = value;
					OnEnabelChanged();
				}
			}
		}

		#region Event
		public event EventHandler  EnableChanged;
		protected virtual void OnEnabelChanged()
		{
			if (EnableChanged != null)
			{
				EnableChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler<CommunicationErrorOccuredEventArgs> CommunicationErrorOccured;
		protected virtual void OnCommunicationErrorOccured(CommunicationErrorOccuredEventArgs e)
		{
			if (CommunicationErrorOccured != null)
			{
				CommunicationErrorOccured(this, e);
			}
		}
		#endregion

		public System.Collections.IEnumerator GetEnumerator()
		{
			return new ControllerEnumerator(controls);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		internal virtual void CommunicationErrorInControlValue(IValue sender)
		{
			if (_RedirectControlValueError)
			{
				if (controls.ContainsValue(sender))
				{
					OnCommunicationErrorOccured(new CommunicationErrorOccuredEventArgs(sender));
				}
				else
				{
					throw new ArgumentException("'sender' is not contains.");
				}
			}
		}

		#region IController 멤버

		protected bool _RedirectControlValueError = true;
		public bool RedirectControlValueError
		{
			get { return _RedirectControlValueError; }
			set { _RedirectControlValueError = value; }
		}

		#endregion
	}

	internal class ControllerEnumerator : System.Collections.IEnumerator
	{
		System.Collections.IEnumerator enumerator = null; 

		public ControllerEnumerator(Dictionary<string, IValue> controls)
		{
			enumerator = controls.Values.GetEnumerator();
		}

		public object Current
		{
			get { return enumerator.Current; }
		}

		public bool MoveNext()
		{
			return enumerator.MoveNext();
		}

		public void Reset()
		{
			enumerator.Reset();
		}
	}

	public class CommunicationErrorOccuredEventArgs : EventArgs
	{
		public	readonly IValue ErrorValue;

		public CommunicationErrorOccuredEventArgs(IValue value)
		{
			ErrorValue = value;
		}
	}
}
