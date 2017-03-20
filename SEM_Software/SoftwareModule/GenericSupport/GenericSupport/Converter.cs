using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport
{
	public class Converter
	{
		public unsafe static byte[] ArrayToBytearray(short[,] arr)
		{
			byte[] result = new byte[arr.Length * 2];

			fixed (short* pntArr = arr)
			{
				short* pArr = pntArr;

				System.Runtime.InteropServices.Marshal.Copy((IntPtr)pArr, result, 0, result.Length);
			}
			
			return result;
		}

		public unsafe static short[,] ArrayFromBytearray(byte[] arr, int width, int height)
		{
			short[,] result = new short[height, width];

			fixed (short* pntArr = result)
			{
				short* pArr = pntArr;

				System.Runtime.InteropServices.Marshal.Copy(arr, 0, (IntPtr)pArr, arr.Length);
			}

			return result;
		}
	}
}
