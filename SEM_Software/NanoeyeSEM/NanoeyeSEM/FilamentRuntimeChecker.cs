using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace SEC.Nanoeye.NanoeyeSEM
{
	class FilamentRuntimeChecker : IDisposable
	{
		private static FilamentRuntimeChecker checker = new FilamentRuntimeChecker();

		bool started = false;
		//bool formShowed = false;
		FilamentRunningTime sfViewer = null;

		System.Threading.Timer checkTimer;

		private FilamentRuntimeChecker() { }

		public static void Start()
		{
			if (checker.started) { return; }

			checker.started = true;
			checker.checkTimer = new Timer(new TimerCallback(checker.TimerTick));
			checker.checkTimer.Change(60000, 60000);
		}

		private void TimerTick(object obj)
		{
			Properties.Settings.Default.FilamentRunningTime += new TimeSpan(0, 1, 0);
			if (sfViewer != null)
			{
				if (sfViewer.Visible)
				{
					sfViewer.FilamentTime = Properties.Settings.Default.FilamentRunningTime;
				}
			}
		}

		public static void Stop()
		{
			if (!checker.started) { return; }

			checker.checkTimer.Dispose();
			checker.started = false;
		}

		public static void ShowForm(System.Windows.Forms.IWin32Window owner)
		{
			if (checker.sfViewer == null)
			{
				checker.sfViewer = new FilamentRunningTime();
				checker.sfViewer.Show(owner);
				checker.sfViewer.FilamentTime = Properties.Settings.Default.FilamentRunningTime;

				checker.sfViewer.TimerReset += new EventHandler(FilamentRuntimeChecker_TimerReset);
			}
			else if (checker.sfViewer.IsDisposed)
			{
				checker.sfViewer = new FilamentRunningTime();
				checker.sfViewer.Show(owner);
				checker.sfViewer.FilamentTime = Properties.Settings.Default.FilamentRunningTime;

				checker.sfViewer.TimerReset += new EventHandler(FilamentRuntimeChecker_TimerReset);
			}
			else
			{
				checker.sfViewer.Show(owner);
				checker.sfViewer.WindowState = System.Windows.Forms.FormWindowState.Normal;
				checker.sfViewer.Location = new System.Drawing.Point(0, 0);
			}

		}

		static void FilamentRuntimeChecker_TimerReset(object sender, EventArgs e)
		{
			checker.sfViewer.FilamentTime = Properties.Settings.Default.FilamentRunningTime = TimeSpan.Zero;
		}

		#region IDisposable 멤버

		public void Dispose()
		{
			if (started)
			{
				checker.checkTimer.Dispose();
			}
		}
		#endregion
	}
}
