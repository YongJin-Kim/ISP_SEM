using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using SEC.Nanoeye.NanoImage;

namespace NanoeyeTestControls
{
	public partial class PaintPanel : Panel
	{
		public PaintPanel()
		{
			InitializeComponent();

			Contrast = 0; // for calculate vidoe LUT

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		private SEC.Nanoeye.NanoImage.IScanItemEvent runISIE = null;

		public void LinkeEvent(SEC.Nanoeye.NanoImage.IScanItemEvent isie)
		{
			ReleseEvent();
			runISIE = isie;
			runISIE.ScanLineUpdated += new ScanDataUpdateDelegate(runISIE_ScanLineUpdated);
			//runISIE.SettingChangedEvent += new EventHandler(runISIE_SettingChangedEvent);

			ScanItemSettingChanged();
		}

		public void ReleseEvent()
		{
			if ( runISIE != null )
			{
				runISIE.ScanLineUpdated -= new ScanDataUpdateDelegate(runISIE_ScanLineUpdated);
				//runISIE.SettingChangedEvent -= new EventHandler(runISIE_SettingChangedEvent);
			}
		}

		private int[] videoLUT;

		private double _Contrast = 1.0d;
		[DefaultValue(0)]
		public int Contrast
		{
			get { return (int)(Math.Log10(_Contrast) * 100); }
			set
			{
				_Contrast = (double)(Math.Pow(10.0d, (double)value / 100.0d));
				ChangeVideoLUT();
			}
		}

		private int _Brightness = 0;
		[DefaultValue(0)]
		public int Brightness
		{
			get { return _Brightness; }
			set
			{
				_Brightness = value;
				ChangeVideoLUT();
			}
		}

		private void ChangeVideoLUT()
		{
			int[] lut = new int[65536];
			int temp;

			for ( int i = 0 ; i < 65536 ; i++ )
			{
				temp = (int)(i * _Contrast + _Brightness);
				temp = (temp > 255) ? 255 : temp;
				temp = (temp < 0) ? 0 : temp;
				lut[i] = (int)((255 << 24) | (temp << 16) | (temp << 8) | (temp));
			}

			videoLUT = lut;
		}

		// cache a scaned image
		Bitmap bakImg = null;

		IntPtr imgPtr = IntPtr.Zero;

		RectangleF paintRect;

		void runISIE_SettingChangedEvent(object sender, EventArgs e)
		{
			ScanItemSettingChanged();
		}

		private void ScanItemSettingChanged()
		{
			areBGimg.WaitOne();
			imgPtr = runISIE.ImageData;
			//paintRect = new RectangleF((float)runISIE.PaintX, (float)runISIE.PaintY, (float)runISIE.PaintWidth, (float)runISIE.PaintHeight);
			paintRect = new RectangleF(runISIE.Setting.PaintX, runISIE.Setting.PaintY, runISIE.Setting.PaintWidth, runISIE.Setting.PaintHeight);
			//bakImg = new Bitmap(runISIE.ImageWidth, runISIE.ImageHeight);
			bakImg = new Bitmap(runISIE.Setting.ImageWidth, runISIE.Setting.ImageHeight);
			areBGimg.Set();
		}


		// for prevent race-condition. the backImg is used runISIE_ScanLineUpdated() and OnPaint()
		System.Threading.AutoResetEvent areBGimg = new System.Threading.AutoResetEvent(true);

		unsafe void runISIE_ScanLineUpdated(object sender, string name, int startline, int lines)
		{
			BitmapData bd;

			int * pBM;
			short * pData;

			int i;
			//try
			//{

			areBGimg.WaitOne();

			bd = bakImg.LockBits(new Rectangle(0, startline, bakImg.Width, lines), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			pBM = (int*)(bd.Scan0.ToInt32());

			pData = (short*)(imgPtr.ToInt32());
			pData = &(pData[bakImg.Width * startline]);

			for (i = 0; i < lines * bakImg.Width; i++)
			{
				*pBM++ = videoLUT[*pData++ + 32768];
			}
			bakImg.UnlockBits(bd);

			// The display window is setted 640 pixels * 480 pixels. (1:0.75)
			int redrawStart = (int)Math.Floor((double)(startline * this.Height * paintRect.Height / 0.75d) / (double)bakImg.Height + this.Height * paintRect.Y / 0.75d);
			int redrawHeigth = (int)Math.Ceiling((double)(lines * this.Height * paintRect.Height / 0.75d) / (double)bakImg.Height);

			if (redrawStart > 0)
			{
				redrawStart--;
				redrawHeigth++;
			}

			areBGimg.Set();
			int redrawX = (int)Math.Floor(this.ClientRectangle.Width * paintRect.X);
			int redrawWidth = (int)Math.Ceiling(this.ClientRectangle.Width * paintRect.Width);


			//Trace.WriteLine(redrawX.ToString() + " , " + redrawStart.ToString() + " , " + redrawWidth.ToString() + " , " + redrawHeigth.ToString() + " = " + (redrawStart + redrawHeigth).ToString());

			this.Invalidate(new Rectangle(redrawX, redrawStart, redrawWidth, redrawHeigth));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if ( bakImg == null ) { return; }

			int drX, drY, drWidth, drHeight;

			drX = (int)Math.Floor(this.ClientRectangle.Width * paintRect.X);
			drY = (int)Math.Floor(this.ClientRectangle.Height * paintRect.Y / 0.75d);
			drWidth = (int)Math.Ceiling(this.ClientRectangle.Width * paintRect.Width);
			drHeight = (int)Math.Ceiling(this.ClientRectangle.Height * paintRect.Height / 0.75d);

			Graphics g = e.Graphics;

			areBGimg.WaitOne();
			g.DrawImage(bakImg, drX, drY, drWidth, drHeight);
			areBGimg.Set();
		}
	}
}
