using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	public class Radio : ControlValueBase, IRadio
	{
		protected List<IValue> _Values = new List<IValue>();
		public IList<IValue> Values
		{
			get { return _Values.AsReadOnly(); }
		}

		public bool Add(IValue con)
		{
			if (_Values.Contains(con)) { return false; }

			con.Enable = false;
			con.EnableChanged += new EventHandler(Value_EnableChanged);
			_Values.Add(con);
			return true;
		}

		public bool Remove(IValue con)
		{
			if (!_Values.Contains(con)) { return false; }

			con.EnableChanged -= new EventHandler(Value_EnableChanged);
			_Values.Remove(con);
			return true;
		}

		void Value_EnableChanged(object sender, EventArgs e)
		{
			IValue con = sender as IValue;
			if (con.Enable == false) { return; }

			foreach (IValue val in _Values)
			{
				if (val == con) { continue; }
				val.Enable = false;
			}
		}

		public override void BeginInit()
		{
			_IsInited = false;
		}

		public override void EndInit(bool sync)
		{
			_IsInited = false;
		}

		public override void Sync()
		{
			
		}

		public override bool Validate()
		{
			return true;
		}
	}
}
