using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport
{
	public class StringEventArg : EventArgs
	{
		public string Message
		{
			get;
			protected set;
		}

		public StringEventArg(string msg)
		{
			Message = msg;
		}
	}
}
