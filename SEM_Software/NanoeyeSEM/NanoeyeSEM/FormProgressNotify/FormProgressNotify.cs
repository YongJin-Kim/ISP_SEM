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
	public partial class FormProgressNotify : Form
	{
		public event EventHandler<ProgressCheckingEventArgs> ProgressChecking;
		public event EventHandler CancelRequest;
		protected virtual void OnCancelRequest()
		{
			if (CancelRequest != null)
			{
				CancelRequest(this, EventArgs.Empty);
			}
			else
			{
				m_DialogResult = DialogResult.Cancel;
			}
		}

		private int m_ElapsedCount = 0;
		private DialogResult m_DialogResult = DialogResult.None;
		private Stopwatch m_Stopwatch = new Stopwatch();

		public ProgressBarStyle Style
		{
			get { return m_ProgressBar.Style; }
			set { m_ProgressBar.Style = value; }
		}

		public bool TimerEnabled
		{
			get { return m_ProgressTimer.Enabled; }
			set { m_ProgressTimer.Enabled = value; }
		}

		public int TimerInterval
		{
			get { return m_ProgressTimer.Interval; }
			set { m_ProgressTimer.Interval = value; }
		}

		public int ElapsedCount
		{
			get { return m_ElapsedCount; }
		}

		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}

		public string NotifyMessage
		{
			get { return m_Message.Text; }
			set { m_Message.Text = value; }
		}

		public FormProgressNotify(string text)
		{
			InitializeComponent();

			this.Text = text;

			Trace.WriteLine(this, "Created");
		}

		public void PerformProgressTesting()
		{
			long elapsed = m_Stopwatch.ElapsedMilliseconds;
			OnProgressChecking(new ProgressCheckingEventArgs(elapsed));
		}

		protected void OnProgressChecking(ProgressCheckingEventArgs e)
		{
			if (ProgressChecking != null)
			{
				ProgressChecking(this, e);

				m_ProgressBar.Value =
					(e.Progress < 0) ? 0 : (e.Progress > 100) ? 100 : e.Progress;

				if (e.Cancel)
				{
					m_ProgressTimer.Stop();
					m_ProgressTimer.Dispose();

					this.DialogResult = DialogResult.OK;
					this.Dispose();
				}
			}
		}

		private void FormProgress_Load(object sender, EventArgs e)
		{
			m_Stopwatch.Start();
			m_ProgressTimer.Tick += new EventHandler(m_ProgressTimer_Tick);
		}

		private void m_ProgressTimer_Tick(object sender, EventArgs e)
		{
			long elapsed = m_Stopwatch.ElapsedMilliseconds;
			OnProgressChecking(new ProgressCheckingEventArgs(elapsed));
		}

		private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.DialogResult = m_DialogResult;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			//this.Owner.Focus();
			OnCancelRequest();
		}

		public int NotifyProgress
		{
			get { return m_ProgressBar.Value; }
			set
			{
				if (value > m_ProgressBar.Maximum)
					value = m_ProgressBar.Maximum;
				if (value < m_ProgressBar.Minimum)
					value = m_ProgressBar.Minimum;

				m_ProgressBar.Value = value;
			}
		}

    }
}
