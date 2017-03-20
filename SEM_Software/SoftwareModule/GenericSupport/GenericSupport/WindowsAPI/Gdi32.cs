using System;
using System.Runtime.InteropServices;

namespace SEC.GenericSupport.WindowsAPI
{
	public sealed class Gdi32
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BITMAP
		{
			public long   bmType;
			public long    bmWidth;
			public long   bmHeight;
			public long   bmWidthBytes;
			public int   bmPlanes;
			public int bmBitsPixel;
			public IntPtr bmBits;
		};

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
		int nWidth, int nHeight, IntPtr hObjectSource,
		int nXSrc, int nYSrc, int dwRop);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
		int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern int GetObjectW(IntPtr h, int c, out IntPtr pv);
	}
}
