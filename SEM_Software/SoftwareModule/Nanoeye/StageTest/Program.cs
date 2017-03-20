using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using System.Diagnostics;

namespace StageTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());

			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ConIO));

			Trace.WriteLine(Environment.OSVersion.ToString(), "OSVersion");
			Trace.WriteLine(Environment.Version.ToString(), "Version");

			

			Application.Run(new StageTester());

			Console.ReadLine();
		}

		static void ConIO(object arg)
		{
			while (true)
			{
				string msg = Console.ReadLine();
				msg = msg.ToLower();
				switch (msg)
				{
				case "clr":
					Console.Clear();
					break;
				case "assem":
					foreach(System.Reflection.Assembly ase in  AppDomain.CurrentDomain.GetAssemblies()){
						Console.WriteLine(ase.FullName);
					}
					break;
				}
			}
		}
	}
}
