using System;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoImage.DataAcquation.IODA
{
	internal partial class Information : Form
	{
		public Information()
		{
			InitializeComponent();

			verFpga.Text = IODAUSB_API.VersionFpga.ToString();
			verLibrary.Text = "0x" + IODAUSB_API.VersionLibrary.ToString("X");

			monitorPeriodValue.DataBindings.Add(new Binding("Value", monitorTimer, "Interval", false, DataSourceUpdateMode.OnPropertyChanged));

			doGet_Click(doGet, EventArgs.Empty);


			//bool[] dis = IODAUSB_API.DigitalInputs;
			//di0Value.Checked = dis[0];
			//di1Value.Checked = dis[1];
			//di2Value.Checked = dis[2];
			//di3Value.Checked = dis[3];

			monitorTimer.Start();
		}

		private static Information _Default = null;
		public static Information Default
		{
			get { return _Default; }
			set { _Default = value; }
		}

		private void monitorTimer_Tick(object sender, EventArgs e)
		{
			statusADDA.Text = IODAUSB_API.CaptureAioStatus.ToString();
			statusBulkIn.Text = IODAUSB_API.CaptureBulkinStatus.ToString();
			statusBuffer.Text = IODAUSB_API.CaptureBufferStatus.ToString();
			statusMemory.Text = IODAUSB_API.AIbufferCount.ToString();

			bool[] dis = IODAUSB_API.DigitalInputs;
			di0Value.Checked = dis[0];
			di1Value.Checked = dis[1];
			di2Value.Checked = dis[2];
			di3Value.Checked = dis[3];
		}

		private void aioFreqGet_Click(object sender, EventArgs e)
		{
			aioFreqValue.Text = IODAUSB_API.SamplingFrequence.ToString();
		}

		private void aioIdGet_Click(object sender, EventArgs e)
		{
			aioIdValue.Text = IODAUSB_API.PatternId.ToString();
		}

		private void aioAddrGet_Click(object sender, EventArgs e)
		{
			aioAddrValue.Text = IODAUSB_API.PatternAddr.ToString();
		}

		private void aioLengthGet_Click(object sender, EventArgs e)
		{
			aioLengthValue.Text = IODAUSB_API.PatternLength.ToString();
		}

		private void aioDelayGet_Click(object sender, EventArgs e)
		{
			aioDelayValue.Text = IODAUSB_API.AIOdelay.ToString();
		}

		private void aioChGet_Click(object sender, EventArgs e)
		{
			aioChValue.Text = IODAUSB_API.AIchannel.ToString();
		}

		private void aioSourceGet_Click(object sender, EventArgs e)
		{
			aioSourceValue.Text = IODAUSB_API.AIsource.ToString();
		}

		private void aioGainGet_Click(object sender, EventArgs e)
		{
			aioGainValue.Text = IODAUSB_API.AIgainGet(IODAUSB_API.AIchannel).ToString();
		}

		private void aioRatioGet_Click(object sender, EventArgs e)
		{
			aioRatioValue.Text = IODAUSB_API.AIOratio.ToString();
		}

		private void doGet_Click(object sender, EventArgs e)
		{
			bool[] dos = IODAUSB_API.DigitalOutputs;
			do0Value.Checked = dos[0];
			do1Value.Checked = dos[1];
			do2Value.Checked = dos[2];
			do3Value.Checked = dos[3];
			do4Value.Checked = dos[4];
			do5Value.Checked = dos[5];
			do6Value.Checked = dos[6];
			do7Value.Checked = dos[7];
		}

		private void allGet_Click(object sender, EventArgs e)
		{
			aioFreqGet.PerformClick();
			aioIdGet.PerformClick();
			aioAddrGet.PerformClick();
			aioLengthGet.PerformClick();
			aioDelayGet.PerformClick();
			aioChGet.PerformClick();
			aioSourceGet.PerformClick();
			aioGainGet.PerformClick();
			aioRatioGet.PerformClick();
			doGet.PerformClick();
		}
	}
}
