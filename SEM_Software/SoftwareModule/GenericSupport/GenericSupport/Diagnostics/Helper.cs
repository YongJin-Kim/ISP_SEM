using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SEC.GenericSupport.Diagnostics
{
	public sealed class Helper
	{
		public static void LogerInit(string fileName)
		{
			string filePath = Path.GetDirectoryName(fileName) + @"\";

			if (!Directory.Exists(filePath)) { Directory.CreateDirectory(Path.GetDirectoryName(filePath)); }


			SEC.GenericSupport.Diagnostics.TraceVer1 traceWriter = new SEC.GenericSupport.Diagnostics.TraceVer1();


			FileStream fs;
			while (true)
			{
				try
				{
					fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
				}
				catch
				{
					fileName += ".txt";
					continue;
				}
				break;
			}

			traceWriter.Writer = new StreamWriter(fs);
			traceWriter.Name = "FileWriter";
			traceWriter.TraceOutputOptions |= TraceOptions.DateTime;
			traceWriter.CategoryMargin = 25;

			Trace.Listeners.Add(traceWriter);
			Trace.AutoFlush = true;

			Trace.WriteLine(AppDomain.CurrentDomain.FriendlyName + " started.", "System");
			Trace.WriteLine("BIOS Name - " + Environment.MachineName, "System");
			Trace.WriteLine("OS Version - " + Environment.OSVersion.ToString(), "System");
			Trace.WriteLine("OS Platform - " + Environment.OSVersion.Platform.ToString(), "System");
			Trace.WriteLine("CLR Version - " + Environment.Version.ToString(), "System");
		}

		public static void LogerExit()
		{
			Trace.WriteLine(AppDomain.CurrentDomain.FriendlyName + " program End.", "System");
			Trace.WriteLine("");
			Trace.WriteLine("");
			Trace.WriteLine("");
			Trace.Flush();
		}

		public static void ExceptionWriterDebug(Exception ex)
		{
			if (ex == null) { return; }

			Debug.WriteLine(ex.Message, "Exception");
			Debug.WriteLine(ex.StackTrace);

			Debug.Indent();
			ExceptionWriterDebug(ex.InnerException);
			Debug.Unindent();
		}

		public static void ExceptionWriterTrace(Exception ex)
		{
			if (ex == null) { return; }

			Trace.WriteLine(ex.Message, "Exception");
			Trace.WriteLine(ex.StackTrace);

			Trace.Indent();
			ExceptionWriterTrace(ex.InnerException);
			Trace.Unindent();
		}
	}
}
