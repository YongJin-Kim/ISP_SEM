using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public class ControlBool : ControlValueBase, IControlBool
	{
		#region Property & Variables
		protected bool _Value = false;
		public virtual bool Value
		{
			get { return _Value; }
			set
			{
				if (value != _Value)
				{
					_Value = value;
					OnValueChanged();
				}
			}
		}
		#endregion

		#region Event
		public event EventHandler  ValueChanged;
		protected virtual void OnValueChanged()
		{
			if (ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		public static implicit operator bool(ControlBool con) { return con.Value; }

		public override void BeginInit()
		{
			_IsInited = false;
		}

		public override void EndInit()
		{
			EndInit(false);
		}

		public override void EndInit(bool sync)
		{
			_IsInited = true;
			if (sync) { Sync(); }
			else { Value = _Value; }
		}

		public override void Sync()
		{
			throw new NotSupportedException();
		}

		public override bool Validate()
		{
			throw new NotSupportedException();
		}
	}
}
