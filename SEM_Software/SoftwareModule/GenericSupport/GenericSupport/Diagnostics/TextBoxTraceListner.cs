using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.GenericSupport.Diagnostics
{
	public class TextBoxTraceListner : TraceListener
	{
		Action<System.Windows.Forms.TextBox,string> act = (tb, str) => { tb.AppendText(str); };

		private System.Windows.Forms.TextBox con;

		public TextBoxTraceListner(System.Windows.Forms.TextBox control)
		{
			con = control;
		}

		public override void Write(string message)
		{
			if (con.InvokeRequired) { con.BeginInvoke(act,con, message); }
			else { act(con, message); }
		}

		public override void WriteLine(string message)
		{
			if (con.InvokeRequired) { con.BeginInvoke(act, con, message + "\r\n"); }
			else { act(con, message + "\r\n"); }
		}
	}
}
