using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class WaveformMonitor : Control, IScanItemPainter
	{
		#region Property & Variables

		private BufferedGraphicsContext context;
		private BufferedGraphics grafx;


		private SEC.Nanoeye.NanoImage.IScanItemEvent SiEvent;

		//private Bitmap paintBM;

		System.Collections.Queue imgDatas = new System.Collections.Queue();

		//bool videoChanging = false;
		//bool videoRechange = false;

		private double _Contrast = 1.0d;
		/// <summary>
		/// Contrast
		/// </summary>
		public int Contrast
		{
			get { return (int)(Math.Log10(_Contrast) * 100); }
			set
			{
				double temp = Math.Pow(10d, value / 100d);
				if (_Contrast == temp) { return; }

				_Contrast = temp;

				ChangeVideoLUT();
			}
		}

		private int _Brightness = 0;
		/// <summary>
		/// Brightness
		/// </summary>
		public int Brightness
		{
			get { return _Brightness; }
			set
			{
				if (_Brightness == value) { return; }

				_Brightness = value;

				ChangeVideoLUT();
			}
		}

		protected volatile uint[] videoLUT;
		#endregion

		public WaveformMonitor()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			InitializeComponent();

			//paintBM = new Bitmap(24, 24);

			


			linePnts = new PointF[] { new PointF(0, 0), new PointF(1, 1) };

			context = BufferedGraphicsManager.Current;
			context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

			videoLutCBmodeThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(VideoLutCBmodeProc));

			ChangeVideoLUT();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
			base.OnSizeChanged(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			//lock (paintBM)
			//{
			//    pe.Graphics.DrawImage(paintBM, new Point(0, 0));
			//}


			grafx.Render(pe.Graphics);
			base.OnPaint(pe);
		}

		#region ScanItem Event
		public bool EventLink(SEC.Nanoeye.NanoImage.IScanItemEvent eventSI, string name)
		{
			EventRelease();
			imageMaker.RunWorkerAsync();
			SiEvent = eventSI;


			eventSI.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_FrameUpdated);
			eventSI.Disposing += new EventHandler(SiEvent_Disposing);


			return true;
		}

		public bool EventRelease()
		{
			if (SiEvent == null) { return false; }
			imageMaker.CancelAsync();
			SiEvent.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SiEvent_FrameUpdated);
			SiEvent.Disposing -= new EventHandler(SiEvent_Disposing);
			return true;
		}

		unsafe void SiEvent_FrameUpdated(object sender, string name, int startline, int lines)
		{
			int lenght = SiEvent.Setting.ImageHeight * SiEvent.Setting.ImageWidth;

			short[] datas = new short[lenght];

			fixed (short* pntDatas = datas)
			{
				short* pDatas = pntDatas;
				short* pOri = (short*)SiEvent.ImageData.ToInt32();

				for (int i = 0; i < lenght; i++)
				{
					*pDatas++ = *pOri++;
				}
			}
			lock (imgDatas.SyncRoot)
			{
				imgDatas.Enqueue(datas);
			}
		}

		void SiEvent_Disposing(object sender, EventArgs e)
		{
			EventRelease();
		}
		#endregion

		PointF[] linePnts;

		private void imageMaker_DoWork(object sender, DoWorkEventArgs e)
		{
			System.Threading.Thread.CurrentThread.Name = "Waveform Moniter";

			ChangeVideoLUT();

			while(!imageMaker.CancellationPending)
			{
				while (imgDatas.Count < 1)
				{
					System.Threading.Thread.Sleep(20);
					if (imageMaker.CancellationPending) { return; }
				}
				if (imageMaker.CancellationPending) { return; }

				short[] imgData;
				lock (imgDatas.SyncRoot)
				{
					imgData = (short[])imgDatas.Dequeue();
				}

				float xScale = (float)this.Width / SiEvent.Setting.ImageWidth;

				PointF[] pnf = new PointF[SiEvent.Setting.ImageWidth];

				//// Line Scan
				//for (int i = 0; i < SiEvent.ImageWidth; i++)
				//{
				//    float h = (float)(this.Height - (videoLUT[imgData[i] + 32768] & 0xff) * this.Height / 256f);
				//    pnf[i] = new PointF(i * xScale, h);
				//}

				// Frame Scan
				for (int i = 0; i < SiEvent.Setting.ImageWidth; i++)
				{
					int h =0;

					for (int y = 0; y < SiEvent.Setting.ImageHeight; y++)
					{
						h += imgData[SiEvent.Setting.ImageWidth * y + i];
					}

					h /= SiEvent.Setting.ImageHeight;

					float r = (float)(this.Height - (videoLUT[h + 32768] & 0xff) * this.Height / 256f);
					pnf[i] = new PointF(i * xScale, r);
				}

				Graphics g = grafx.Graphics;
				g.Clear(BackColor);
				g.DrawLines(Pens.White, pnf);

				this.Invalidate();
			}
		}

		protected System.Threading.Thread videoLutCBmodeThread;

		protected unsafe void VideoLutCBmodeProc(object data)
		{
			uint[] lut = new uint[65536];
			int temp;

			System.Threading.Thread thr = (System.Threading.Thread)(((object[])data)[0]);
			double bri = (double)(((object[])data)[1]);
			double con = (double)(((object[])data)[2]);

			fixed(uint* ptr = lut)
			{
				uint* pLut = ptr;

				//for(int i = short.MinValue; i <= short.MaxValue; i++)
				//{
				//    temp = (int)(i * con + bri);
				//    temp = (temp > 255) ? 255 : temp;
				//    temp = (temp < 0) ? 0 : temp;
				//    *pLut++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);

				//    //if (videoLutCBmodeThread.ThreadState == System.Threading.ThreadState.AbortRequested) { return; }
				//}
				int length = (short.MaxValue - short.MinValue + 1) / Environment.ProcessorCount;
				int cnt = Environment.ProcessorCount;
				for(int i = 0; i < Environment.ProcessorCount; i++)
				{
					int t = short.MinValue + (length * i);
					uint* pLutTemp = &(pLut[length * i]);

					System.Threading.ThreadPool.QueueUserWorkItem(delegate
					{
						double tem1 = t * con + bri;
						temp = (tem1 > 255) ? 255 : (int)tem1;
						temp = (temp < 0) ? 0 : temp;
						*pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
						for(int k = t; k < t + length; k++)
						{
							tem1 += con;
							temp = (tem1 > 255) ? 255 : (int)tem1;
							temp = (temp < 0) ? 0 : temp;
							*pLutTemp++ = (uint)((uint)255 << 24) | (uint)(temp << 16) | (uint)(temp << 8) | (uint)(temp);
						}
						cnt--;
					});
				}

				while(cnt > 0)
				{
					System.Threading.Thread.Sleep(1);
					if(thr.ThreadState == System.Threading.ThreadState.AbortRequested) { return; }
				}

			}
			if(thr.ThreadState == System.Threading.ThreadState.AbortRequested)
			{
				return;
			}
			videoLUT = lut;
		}


		/// <summary>
		/// Contrast 및 Brightness가 바뀐 경우.
		/// </summary>
		protected virtual void ChangeVideoLUT()
		{
			if(!imageMaker.IsBusy) { return; }
			while(videoLutCBmodeThread.IsAlive)
			{
				videoLutCBmodeThread.Abort();
				System.Threading.Thread.Sleep(1);
			}

			videoLutCBmodeThread = new System.Threading.Thread(VideoLutCBmodeProc);
			videoLutCBmodeThread.Start((object)(new object[] { videoLutCBmodeThread, (double)_Brightness, (double)_Contrast }));
		}
	}
}
