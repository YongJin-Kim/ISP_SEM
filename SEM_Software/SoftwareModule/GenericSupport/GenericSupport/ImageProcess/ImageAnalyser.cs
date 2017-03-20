using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace SEC.GenericSupport.ImageProcess
{
	public class ImageAnalyser
	{
		public static void VideoAnalyse(IntPtr imgData, Type dataType, int width, int height, out double gap, out double average, float dropRate)
		{
			if (dataType == typeof(short))
			{
				int length = width * height;

				short[] datas = new short[length];

				Marshal.Copy(imgData, datas, 0, length);
				VideoAnalyseShort(datas, out gap, out average, dropRate);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static unsafe void VideoAnalyse(short[,] imgData, out double gap, out double average, float dropRate)
		{
			int legnth = imgData.Length;

			short[] datas = new short[legnth];

			fixed (short* pntData = datas, pntSource = imgData)
			{
				short* pData = pntData;
				short* pSrc = pntSource;

				while (legnth > 0)				
				{
					*pData++ = *pSrc++;
					legnth--;
				}
			}

			VideoAnalyseShort(datas, out gap, out average, dropRate);
		}

		private static unsafe void VideoAnalyseShort(short[] datas, out double gap, out double average, float dropRate)
		{
			int length = datas.Length;

			List<short> oriList = new List<short>(length);

			oriList.AddRange(datas);
			oriList.Sort();

			oriList.RemoveRange((int)(length * (1 - dropRate)), (int)(length * dropRate));
			oriList.RemoveRange(0, (int)(length * dropRate));

			long i=0;
			foreach (short sh in oriList) { i += sh; }
			average = (double)i / oriList.Count;

			gap = oriList.Last() - oriList.First();

			System.Diagnostics.Debug.WriteLine( "Gap - " + gap.ToString() + " vs Avr - " + average.ToString(), "AvideoAnalyse" );
		}
	}
}
