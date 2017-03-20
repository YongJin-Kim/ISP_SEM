using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using SEC.Nanoeye.FourierTransform;

namespace SEC.Nanoeye.Support.AutoFunction
{
	internal class AutoFocusHelper
	{
		private AutoFocusHelper()
		{ }

		/// <summary>
		/// 영상 데이터를 복소수 영역으로 전송합니다.
		/// </summary>
		/// <param name="complex"></param>
		/// <param name="frameSize"></param>
		/// <param name="source"></param>
		public static unsafe void InitializeComplexArray(ComplexF[] complex, short[,] source)
		{
			int length = complex.Length;
			int width = source.GetLength(0);
			int height = source.GetLength(1);

			// 영상 데이터 크기에 대한 복소수 배율(범위 조정)을 구합니다.
			float scale = 1f / (float)Math.Sqrt(width * height);

			fixed (short* rp_source = source)
			{
				fixed (ComplexF* rp_complex = complex)
				{
					short* p = rp_source;
					ComplexF* p_complex = rp_complex;

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							p_complex->Re = *p * scale;
							p_complex->Im = 0;

							p_complex++;
							p++;
						}
					}
				}
			}
		}

		public static unsafe void InitializeComplexArray(out ComplexF[] complexData, short[] datas, int width, int height)
		{
			complexData = new ComplexF[width * height];

			float scale = 1f / (float)Math.Sqrt(width * height);

			fixed (short* pntSource = datas)
			{
				fixed (ComplexF* pntComplex = complexData)
				{
					short* pSrc = pntSource;
					ComplexF* pCd = pntComplex;

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							pCd->Re = *pSrc * scale;
							pCd->Im = 0;

							pCd++;
							pSrc++;
						}
					}
				}
			}
		}



		[System.Diagnostics.Conditional("DEBUG")]
		public static void PrintFreqImage(float fft, ComplexF[] complex, int width, int height)
		{
			if (!System.IO.Directory.Exists(@".\ImageAF2"))
			{
				System.IO.Directory.CreateDirectory(@".\ImageAF2");
			}

			//string filename = @".\ImageAF2\" + focusedCoarseValue.ToString() + "-" + fine.ToString() + ".bmp";
			string filename = @".\ImageAF2\Freq" + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "FFT" + fft.ToString() + @".bmp";
			//System.IO.StreamWriter sw = new System.IO.StreamWriter(@".\Image\Freq" + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + @".txt");

			Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			BitmapData bd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			unsafe
			{
				int* dest = (int*)bd.Scan0.ToInt32();
				for (int i = 0; i < complex.Length; i++)
				{
					int val = (int)(10 * Math.Log(1 + Math.Abs(complex[i].Re)));
					if (val > 255) { val = 255; }
					*dest++ = val | (val << 8) | (val << 16) | (0xff << 24);
				}
			}

			bm.UnlockBits(bd);
			bm.Save(filename);
			bm.Dispose();
		}

		#region Make Filter
		public static int[] InitializeFreqFilter(int width, int height, float low, float high)
		{
			RectangleF maskBounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			RectangleF startRect = maskBounds;
			startRect.Inflate(
				width * (1 - low) * -0.5F,
				height * (1 - low) * -0.5F);

			RectangleF stopRect = maskBounds;
			stopRect.Inflate(
				width * (1 - high) * -0.5F,
				height * (1 - high) * -0.5F);

			using (Bitmap bitmap = new Bitmap(width, height))
			{
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					g.Clear(Color.Black);

					g.FillEllipse(Brushes.White, stopRect);
					g.FillEllipse(Brushes.Black, startRect);
				}

#if DEBUG
				bitmap.Save(".\\FreqMask.bmp", ImageFormat.Bmp);
#endif

				BitmapData bd = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly,
					bitmap.PixelFormat);

				unsafe
				{
					fixed (int* rpDst = maskData)
					{

						int* pDst = rpDst;
						int* pSrc = (int*)bd.Scan0.ToInt32();
						int length = width * height;

						for (int i = 0; i < length; i++)
						{
							*pDst = *pSrc & 0x000000FF;
							pDst++;
							pSrc++;
						}
					}
				}

				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		// 가운데 타원에서 외곽으로 갈수록 어두워짐.
		public static int[] InitializeFreqFilter2(int width, int height, float stop, float ext)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			bounds.Inflate(
				width * (1 - ext) * -0.5F,
				height * (1 - ext) * -0.5F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			// 버튼을 채울 브러쉬를 초기화 합니다.
			// gp = new GraphicsPath();
			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.Black };

			pgb.FocusScales = new PointF(stop, stop);
			pgb.CenterPoint = new PointF(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			pgb.CenterColor = Color.White;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();

				RectangleF rtf = new RectangleF(new PointF(0, 0), bitmap.Size);
				rtf.Inflate(
					width * (1 - stop) * -0.5F,
					height * (1 - stop) * -0.5F);
				g.FillEllipse(Brushes.Black, rtf);
#if DEBUG
				bitmap.Save(".\\FreqMask2.bmp", ImageFormat.Bmp);
#endif

				BitmapData bd = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly,
					bitmap.PixelFormat);

				unsafe
				{
					fixed (int* rpDst = maskData)
					{

						int* pDst = rpDst;
						int* pSrc = (int*)bd.Scan0.ToInt32();
						int length = width * height;

						for (int i = 0; i < length; i++)
						{
							*pDst = *pSrc & 0x000000FF;
							pDst++;
							pSrc++;
						}
					}
				}

				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		// Filter 2의 반대
		public static int[] InitializeFreqFilter3(int width, int height, float stop, float ext)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			bounds.Inflate(
				width * (1 - ext) * -0.5F,
				height * (1 - ext) * -0.5F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			// 버튼을 채울 브러쉬를 초기화 합니다.
			// gp = new GraphicsPath();
			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.White };

			pgb.FocusScales = new PointF(stop, stop);
			pgb.CenterPoint = new PointF(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			pgb.CenterColor = Color.Black;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();

				RectangleF rtf = new RectangleF(new PointF(0, 0), bitmap.Size);
				rtf.Inflate(
					width * (1 - stop) * -0.5F,
					height * (1 - stop) * -0.5F);
				g.FillEllipse(Brushes.Black, rtf);
			}
#if DEBUG
			bitmap.Save(".\\FreqMask3.bmp", ImageFormat.Bmp);
#endif

			BitmapData bd = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				bitmap.PixelFormat);

			unsafe
			{
				fixed (int* rpDst = maskData)
				{

					int* pDst = rpDst;
					int* pSrc = (int*)bd.Scan0.ToInt32();
					int length = width * height;

					for (int i = 0; i < length; i++)
					{
						*pDst = (*pSrc & 0x000000FF);
						pDst++;
						pSrc++;
					}
				}


				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		public static int[] InitializeFreqFilter4(int width, int height, float inCi, float outCi, float del)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			bounds.Inflate(
				width * (1 - outCi) * -1F,
				height * (1 - outCi) * -1F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			// 버튼을 채울 브러쉬를 초기화 합니다.
			// gp = new GraphicsPath();
			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.Black };

			pgb.FocusScales = new PointF(inCi, inCi);
			pgb.CenterPoint = new PointF(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			pgb.CenterColor = Color.White;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();

				RectangleF rtf = new RectangleF(new PointF(0, 0), bitmap.Size);
				rtf.Inflate(
					width * (1 - del) * -0.5F,
					height * (1 - del) * -0.5F);
				g.FillEllipse(Brushes.Black, rtf);
#if DEBUG
				bitmap.Save(".\\FreqMask4.bmp", ImageFormat.Bmp);
#endif

				BitmapData bd = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly,
					bitmap.PixelFormat);

				unsafe
				{
					fixed (int* rpDst = maskData)
					{

						int* pDst = rpDst;
						int* pSrc = (int*)bd.Scan0.ToInt32();
						int length = width * height;

						for (int i = 0; i < length; i++)
						{
							*pDst = *pSrc & 0x000000FF;
							pDst++;
							pSrc++;
						}
					}
				}

				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		// 안으로 퍼지는 부채꼴 안으로 갈수록 검은색
		public static int[] InitializeFreqFilter5(int width, int height, float outCi)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			// 원 만들기
			bounds.Inflate(
				width * (1 - outCi) * -1F,
				height * (1 - outCi) * -1F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.Black };
			//pgb.SurroundColors = new Color[] { Color.Black };

			pgb.CenterColor = Color.White;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();
			}

			// 부채꼴 획득
			BitmapData bd = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);

			Bitmap bmResult = new Bitmap(128, 128, PixelFormat.Format32bppArgb);
			BitmapData bdResult = bmResult.LockBits(new Rectangle(new Point(0, 0), bmResult.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe
			{
				int* src = (int*)bd.Scan0.ToInt32();
				int* dest = (int*)bdResult.Scan0.ToInt32();
				for (int h = 0; h < 128; h++)
				{
					for (int w = 0; w < 128; w++)
					{
						*dest++ = *(src + w + h * 256);
					}
				}
			}

			bitmap.UnlockBits(bd);
			bmResult.UnlockBits(bdResult);

			// 각 4분면에 재 배치 하기.
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.DrawImage(bmResult, new Point(128, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 0));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(128, 0));
			}



//#if DEBUG
//            bitmap.Save(".\\FreqMask5.bmp", ImageFormat.Bmp);
//#endif

			bd = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				bitmap.PixelFormat);

			unsafe
			{
				fixed (int* rpDst = maskData)
				{

					int* pDst = rpDst;
					int* pSrc = (int*)bd.Scan0.ToInt32();
					int length = width * height;

					for (int i = 0; i < length; i++)
					{
						*pDst = *pSrc & 0x000000FF;
						pDst++;
						pSrc++;
					}
				}


				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		// 안으로 퍼지는 부채꼴 모서리 검정. 안으로 갈수록 힌색.
		public static int[] InitializeFreqFilter6(int width, int height, float outCi)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			// 원 만들기
			bounds.Inflate(
				width * (1 - outCi) * -1F,
				height * (1 - outCi) * -1F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.White };
			//pgb.SurroundColors = new Color[] { Color.Black };

			pgb.CenterColor = Color.Black;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();
			}

			// 부채꼴 획득
			BitmapData bd = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);

			Bitmap bmResult = new Bitmap(128, 128, PixelFormat.Format32bppArgb);
			BitmapData bdResult = bmResult.LockBits(new Rectangle(new Point(0, 0), bmResult.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe
			{
				int* src = (int*)bd.Scan0.ToInt32();
				int* dest = (int*)bdResult.Scan0.ToInt32();
				for (int h = 0; h < 128; h++)
				{
					for (int w = 0; w < 128; w++)
					{
						*dest++ = *(src + w + h * 256);
					}
				}
			}

			bitmap.UnlockBits(bd);
			bmResult.UnlockBits(bdResult);

			// 각 4분면에 재 배치 하기.
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.DrawImage(bmResult, new Point(128, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 0));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(128, 0));
			}



#if DEBUG
            bitmap.Save(".\\FreqMask6.bmp", ImageFormat.Bmp);
#endif

			bd = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				bitmap.PixelFormat);

			unsafe
			{
				fixed (int* rpDst = maskData)
				{

					int* pDst = rpDst;
					int* pSrc = (int*)bd.Scan0.ToInt32();
					int length = width * height;

					for (int i = 0; i < length; i++)
					{
						*pDst = *pSrc & 0x000000FF;
						pDst++;
						pSrc++;
					}
				}


				bitmap.UnlockBits(bd);
			}

			return maskData;
		}

		// 안으로 퍼지는 부채꼴 모두 흰색
		public static int[] InitializeFreqFilter7(int width, int height, float outCi)
		{
			RectangleF bounds = new RectangleF(0, 0, width, height);
			int[] maskData = new int[width * height];

			// 원 만들기
			bounds.Inflate(
				width * (1 - outCi) * -1F,
				height * (1 - outCi) * -1F);

			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();

			pgb.SurroundColors = new Color[] { Color.White };
			//pgb.SurroundColors = new Color[] { Color.Black };

			pgb.CenterColor = Color.White;// ControlPaint.Light(this.BackColor);

			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.FillEllipse(pgb, bounds);
				pgb.Dispose();
			}

			// 부채꼴 획득
			BitmapData bd = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);

			Bitmap bmResult = new Bitmap(128, 128, PixelFormat.Format32bppArgb);
			BitmapData bdResult = bmResult.LockBits(new Rectangle(new Point(0, 0), bmResult.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe
			{
				int* src = (int*)bd.Scan0.ToInt32();
				int* dest = (int*)bdResult.Scan0.ToInt32();
				for (int h = 0; h < 128; h++)
				{
					for (int w = 0; w < 128; w++)
					{
						*dest++ = *(src + w + h * 256);
					}
				}
			}

			bitmap.UnlockBits(bd);
			bmResult.UnlockBits(bdResult);

			// 각 4분면에 재 배치 하기.
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.Black);
				g.DrawImage(bmResult, new Point(128, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 128));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(0, 0));

				bmResult.RotateFlip(RotateFlipType.Rotate90FlipNone);
				g.DrawImage(bmResult, new Point(128, 0));
			}


			bd = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				bitmap.PixelFormat);

			unsafe
			{
				fixed (int* rpDst = maskData)
				{

					int* pDst = rpDst;
					int* pSrc = (int*)bd.Scan0.ToInt32();
					int length = width * height;

					for (int i = 0; i < length; i++)
					{
						*pDst = *pSrc & 0x000000FF;
						pDst++;
						pSrc++;
					}
				}


				bitmap.UnlockBits(bd);
			}

			return maskData;
		}
		#endregion

		public static unsafe float GetFreqAmplValue(short[,] samples, int[] masks)
		{
			float sumAmpl = 0;

			int width = samples.GetLength(1);
			int height = samples.GetLength(0);

			ComplexF[] complexData = new ComplexF[width * height];

			InitializeComplexArray(complexData, samples);

			// 2D FFT를 계산합니다.
			Fourier.FFT2(complexData, width, height, FourierDirection.Forward);


			fixed (ComplexF* rpSrc = complexData)
			{
				fixed (int* rpMasks = masks)
				{
					ComplexF* pSrc = rpSrc;
					int* pMasks = rpMasks;

					// DC 성분 제거.
					//sumAmpl -= pSrc->GetModulus() * *pMasks;

					for (int y = 0; y < height / 2; y++)
					{
						//pSrc = rpSrc + width * y;

						for (int x = 0; x < width; x++)
						{
							sumAmpl += pSrc->GetModulus() * *pMasks;
							pSrc++;
							pMasks++;
						}
					}

				}
			}

			

			float result = sumAmpl / (width * height / 2);
			PrintFreqImage(result, complexData, width, height);
			return result;
		}

		public static unsafe float GetFreqAmplValue(short[] datas, int[] freqFilter, int width, int height)
		{
			float sumAmpl = 0;
			ComplexF[] complexData;

			InitializeComplexArray(out complexData, datas, width, height);
			Fourier.FFT2(complexData, width, height, FourierDirection.Forward);

			fixed (ComplexF* pntCf = complexData)
			{
				fixed (int* pntMask = freqFilter)
				{
					ComplexF* pCf = pntCf;
					int* pMask = pntMask;

					for (int y = 0; y < height / 2; y++)
					{
						for (int x = 0; x < width; x++)
						{
							sumAmpl += pCf->GetModulus() * *pMask;
							//sumAmpl += pCf->GetModulusSquared() * *pMask;
							pCf++;
							pMask++;
						}

					}
				}
			}

			float result = sumAmpl / (width * height / 2);
			PrintFreqImage(result, complexData, width, height);
			return result;
		}

		public static unsafe float GetFreqAmplValue(short[,] samples)
		{
			float sumAmpl = 0;

			int width = samples.GetLength(1);
			int height = samples.GetLength(0);

			ComplexF[] complexData = new ComplexF[width * height];

			InitializeComplexArray(complexData, samples);

			// 2D FFT를 계산합니다.
			Fourier.FFT2(complexData, width, height, FourierDirection.Forward);

			fixed (ComplexF* rpSrc = complexData)
			{
				ComplexF* pSrc = rpSrc;

				for (int y = 0; y < height / 2; y++)
				{
					//pSrc = rpSrc + width * y;

					for (int x = 0; x < width; x++)
					{
						sumAmpl += pSrc->GetModulus();
						pSrc++;
					}
				}
			}

			float result = sumAmpl / (width * height / 2);
			PrintFreqImage(result, complexData, width, height);
			return result;
		}



		public static unsafe short[,] MakePowAvrImage(short[][,] images, int cnt)
		{
			short[,] img = images[0];
			int width = img.GetLength(1);
			int height = img.GetLength(0);
			img = new short[height, width];

			int w, h, i;
			double val = 0;
			unsafe
			{

				fixed (short* pntImg = img)
				{

					short* pImg = pntImg;

					for (h = 0; h < height; h++)
					{
						for (w = 0; w < width; w++)
						{
							val = 0;
							for (i = 0; i < cnt; i++)
							{
								val += Math.Pow(((double)images[i][h, w] - (double)short.MinValue), 2.0);
							}
							*pImg++ = (short)((int)(Math.Sqrt(val / cnt)) + (int)short.MinValue);
						}
					}
				}
			}
			//#if DEBUG
			//            System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
			//            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
			//            bgw.RunWorkerAsync((object)(new object[] { img, "Pow" }));
			//#endif

			return img;

		}

		public static unsafe short[] MakePowAvrImage(short[][] frameBuffer, int cnt)
		{
			short[] img = new short[frameBuffer[0].Length];

			double val;

			for (int i = 0; i < img.Length; i++)
			{
				val = 0;
				for (int j = 0; j < cnt; j++)
				{
					val += Math.Pow((double)frameBuffer[j][i] - (double)short.MinValue, 2);
				}
				img[i] = (short)((int)(Math.Sqrt(val / cnt)) + (int)short.MinValue);
			}
			return img;
		}

		public static unsafe short[,] MakeAvrImage(short[][,] images, int cnt)
		{
			short[,] img = images[0];
			int width = img.GetLength(1);
			int height = img.GetLength(0);
			img = new short[height, width];

			int w, h, i;
			double val = 0;
			unsafe
			{

				fixed (short* pntImg = img)
				{

					short* pImg = pntImg;

					for (h = 0; h < height; h++)
					{
						for (w = 0; w < width; w++)
						{
							val = 0;
							for (i = 0; i < cnt; i++)
							{
								val += images[i][h, w];
							}
							*pImg++ = (short)(val / cnt);
						}
					}
				}
			}
			//#if DEBUG
			//            System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
			//            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
			//            bgw.RunWorkerAsync((object)(new object[] { img, "Pow" }));
			//#endif

			return img;

		}

		public static unsafe short[] MakeAvrImage(short[][] frameBuffer, int cnt)
		{
			short[] img = new short[frameBuffer[0].Length];

			double val;

			for (int i = 0; i < img.Length; i++)
			{
				val = 0;
				for (int j = 0; j < cnt; j++)
				{
					val += frameBuffer[j][i];
				}
				img[i] = (short)(val / cnt);
			}
			return img;
		}

		private static void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			object[] arg = (object[])(e.Argument);
			short[,] img = (short[,])(arg[0]);
			string name = (string)(arg[1]);

			int width = img.GetLength(1);
			int height = img.GetLength(0);

			Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			BitmapData bd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe
			{
				int* pnt = (int*)bd.Scan0.ToInt32();
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int value = (int)(img[y, x]) + (int)(short.MaxValue) + 1;
						*pnt++ = (int)value + (255 << 24);
					}
				}
			}
			bm.UnlockBits(bd);
			bm.Save(@".\" + name + DateTime.Now.Ticks.ToString() + @".bmp");
		}

		#region Image Filter
		public static unsafe short[,] HistogramEqual(short[,] img)
		{
			int[] histogram = new int[0xffff + 1];
			int[] soHistogram = new int[0xffff + 1];

			int width = img.GetLength(1);
			int height = img.GetLength(0);

			short[,] reuImg = new short[height, width];

			int k;

			unsafe
			{
				fixed (short* pntImg = img, pntReuImg = reuImg)
				{

					short* pImg = pntImg;
					short* pReuImg = pntReuImg;

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							k = *pImg++ - short.MinValue;
							histogram[k] += 1;
						}
					}

					fixed (int* pntHist = histogram, pntSOH = soHistogram)
					{

						int* pHist = pntHist;
						int* pSOH = pntSOH;

						int sum = 0;
						for (int i = 0; i < 0xffff + 1; i++)
						{
							sum += *pHist++;
							*pSOH++ = sum;
						}
					}

					pImg = pntImg;

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							k = *pImg++ - short.MinValue;
							*pReuImg++ = (short)(soHistogram[k] * (short.MaxValue - short.MinValue) / height / width + short.MinValue);
						}
					}
				}
			}
			//#if DEBUG
			//            System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
			//            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
			//            bgw.RunWorkerAsync((object)(new object[] { reuImg, "Equ" }));
			//#endif
			return reuImg;
		}

		public static unsafe short[,] HistogramStretch(short[,] img)
		{
			int[] histogram = new int[0xffff + 1];
			int[] soHistogram = new int[0xffff + 1];

			int width = img.GetLength(1);
			int height = img.GetLength(0);

			short[,] reuImg = new short[height, width];
			int k;
			unsafe
			{
				fixed (short* pntImg = img, pntReuImg = reuImg)
				{

					short* pImg = pntImg;
					short* pReuImg = pntReuImg;

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							k = *pImg++ - short.MinValue;
							histogram[k] += 1;
						}
					}

					int threshLow = 0;
					int threshHight = 0xffff;
					int i;

					for (i = 0; i < 0xffff + 1; i++)
					{
						if (histogram[i] > 0)
						{
							threshLow = i;
							break;
						}
					}

					for (i = 0xffff; i >= 0; i--)
					{
						if (histogram[i] > 0)
						{
							threshHight = i;
							break;
						}
					}

					int[] LUT = new int[0xffff + 1];
					double scale = (double)((0xffff + 1) / (threshHight - threshLow + 1));

					for (i = 0; i < threshLow; i++)
					{
						LUT[i] = 0;
					}
					for (; i < threshHight; i++)
					{
						LUT[i] = (ushort)((i - threshLow) * scale);
					}
					for (; i < 0xffff + 1; i++)
					{
						LUT[i] = 0xffff;
					}

					pImg = pntImg;

					int pnt;
					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							pnt = *pImg++ - short.MinValue;
							*pReuImg++ = (short)(LUT[pnt] + short.MinValue);
						}
					}
				}
			}
			//#if DEBUG
			//            System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
			//            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
			//            bgw.RunWorkerAsync((object)(new object[] { reuImg, "Stresh" }));
			//#endif
			return reuImg;
		}

		public static unsafe short[] HistogramEqual(short[] img)
		{
			int[] histogram = new int[0xffff + 1];
			int[] soHistogram = new int[0xffff + 1];

			int length = img.Length;

			short[] reuImg = new short[img.Length];

			int k;

			unsafe
			{
				fixed (short* pntImg = img, pntReuImg = reuImg)
				{

					short* pImg = pntImg;
					short* pReuImg = pntReuImg;

					for (int y = 0; y < length; y++)
					{
						k = *pImg++ - short.MinValue;
						histogram[k]++;
					}

					fixed (int* pntHist = histogram, pntSOH = soHistogram)
					{

						int* pHist = pntHist;
						int* pSOH = pntSOH;

						int sum = 0;
						for (int i = 0; i < 0xffff + 1; i++)
						{
							sum += *pHist++;
							*pSOH++ = sum;
						}
					}

					pImg = pntImg;

					for (int y = 0; y < length; y++)
					{
						k = *pImg++ - short.MinValue;
						*pReuImg++ = (short)(soHistogram[k] * (short.MaxValue - short.MinValue) / length + short.MinValue);
					}
				}
			}

			return reuImg;
		}
		#endregion

        public static float GetFreqAmplValue2(short[] img, int[] freqFilter, int p, int p_4)
        {
           
            float ave = 0;
            for (int i = 0; i < img.Length; i++)
            {
                ave += img[i];
            }

            ave = ave / img.Length;

            float min = 0;
            float max = 0;
            float gap = 0;


            for (int j = 0; j < img.Length; j++)
            {
                gap = ave - img[j];

                if (min < gap)
                {
                    min = gap;
                }

                if (max > gap)
                {
                    max = gap;
                }
            }



            return max + min;
        
        }
    }

	internal class FocusItem
	{
		public readonly float LensValue;
		public float FocusValue;
		public float TrendValue;

		public FocusItem()
		{
			LensValue = 0;
			FocusValue = 0;
			TrendValue = 0;
		}

		public FocusItem(float lensValue)
		{
			LensValue = lensValue;
			FocusValue = 0;
			TrendValue = 0;
		}

		public override string ToString()
		{
			return "{" + LensValue.ToString("F1") + ", " + FocusValue.ToString("F4") + ", " + TrendValue.ToString("F4") + "}";
		}
	}

	internal class FocusList : List<FocusItem>
	{
		public FocusList(int capacity) : base(capacity) { }

		public void UpdateFocusValue(int index, float focusValue)
		{
			this[index].FocusValue = focusValue;
		}

		public void UpdateTrendValue(int index, int count)
		{
			float sum = 0;

			int start = index - count;
			start = (start < 0) ? 0 : start;

			for (int i = start; i <= index; i++)
			{
				sum += this[i].FocusValue;
			}

			this[index].TrendValue = sum / count;
		}

		public int IndexOfMaximumFocusValue()
		{
			float[] focusList = new float[this.Count];

			for (int i = 0; i < focusList.Length; i++)
			{
				focusList[i] = this[i].FocusValue;
			}

			focusList[0] /= 2;
			focusList[focusList.Length - 1] /= 2;

			for (int index = 1; index < focusList.Length - 1; index++)
			{
				focusList[index] =
					focusList[index - 1] * 0.025F +
					focusList[index] * 0.950F +
					focusList[index + 1] * 0.025F;
			}

			float maxValue = float.MinValue;
			int maxIndex = int.MinValue;

			for (int index = 1; index < focusList.Length - 1; index++)
			{
				if (focusList[index] > maxValue)
				{
					maxValue = focusList[index];
					maxIndex = index;
				}
			}

			return maxIndex;
		}

		public void AdjustFocusItems(float leftWidth, float rightWidth, int focusedIndex, int count)
		{
			int leftIndex = focusedIndex - (int)(this.Count * leftWidth);
			leftIndex = (leftIndex < 0) ? 0 : leftIndex;

			int rightIndex = focusedIndex + (int)(this.Count * rightWidth);
			rightIndex = (rightIndex >= this.Count) ? (this.Count - 1) : rightIndex;

			float leftValue = this[leftIndex].LensValue;
			float rightValue = this[rightIndex].LensValue;

			this.Clear();

			for (int i = 0; i < count; i++)
			{
				this.Add(new FocusItem(leftValue + i * ((rightValue - leftValue) / (count - 1))));
			}
		}

		public void CreateFocusList(float minLens, float maxLens, int count)
		{
			if (minLens > maxLens)
			{
				throw new ArgumentException();
			}

			this.Clear();

			for (int i = 0; i < count; i++)
			{
				this.Add(new FocusItem(minLens + i * ((maxLens - minLens) / (count - 1))));
			}
		}

		/// <summary>
		/// 포커스 레벨의 트리거 상태를 가져옵니다.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool GetFocusTriggerState(int index, int windowSize)
		{
			if (index < windowSize)
			{
				return false;
			}

			bool isTriggered = true;
			int start = index - windowSize;
			start = (start <= 0) ? 1 : start;

			for (int i = start; i <= index; i++)
			{
				if (this[i - 1].TrendValue <= this[i].TrendValue)
				{
					isTriggered = false;
				}
			}

			return isTriggered;
		}

        
    }
}
