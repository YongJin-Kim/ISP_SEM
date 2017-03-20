using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.Diagnostics
{
	public class TraceVer1 : System.Diagnostics.TextWriterTraceListener
	{
		private string _CategoryMargin = "15";
		public int CategoryMargin
		{
			get { return int.Parse(_CategoryMargin); }
			set { _CategoryMargin = value.ToString(); }
		}

		public override void WriteLine(string message)
		{
			base.WriteLine(message);
		}

		public override void WriteLine(string message, string category)
		{
			string catNew = string.Format("{0}{1," + _CategoryMargin + "}", DateTime.Now.ToString(), category);

			base.WriteLine(message, catNew);
		}

		public override void Write(string message, string category)
		{
			string catNew = string.Format("{0}{1," + _CategoryMargin + "}", DateTime.Now.ToString(), category);

			base.Write(message, catNew);
		}

	}
}
