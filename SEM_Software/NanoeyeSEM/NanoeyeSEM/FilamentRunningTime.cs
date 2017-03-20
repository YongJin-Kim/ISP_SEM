using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class FilamentRunningTime : Form
	{
		public event EventHandler TimerReset;
		protected virtual void OnTimerReset()
		{
			if (TimerReset != null)
			{
				TimerReset(this, EventArgs.Empty);
			}
		}

		private TimeSpan runningTime;
		public TimeSpan FilamentTime
		{

            //public filamentTime{get    
			get { return runningTime; }
			set { 
				runningTime = value;
				System.Diagnostics.Debug.WriteLine(runningTime.ToString());
				DisplayRunTime(runningTime);
			}
		}

        //public class minEventTestArgs : EventArgs
        //{
        //    public FilamentRunningTime runningTimer { get; set; }
        //    public minEventTestArgs(FilamentRunningTime Timer)
        //    {
        //        this.runningTimer = Timer;
        //    }
        //}

		public FilamentRunningTime()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			TextManager.Instance.DefineText(this);

			base.OnLoad(e);
		}

		public void DisplayRunTime(TimeSpan ts)
		{
			Action act =() =>
			{
				string result = "";
				result += ts.Days.ToString() + "days ";
				result += ts.Hours.ToString().PadLeft(2, '0') + ":";
				result += ts.Minutes.ToString().PadLeft(2, '0');
				label1.Text = result;
				label1.Invalidate();
				System.Diagnostics.Debug.WriteLine(result, "Filament Time");
			};
			this.BeginInvoke(act);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OnTimerReset();
		}

       
	}
}
