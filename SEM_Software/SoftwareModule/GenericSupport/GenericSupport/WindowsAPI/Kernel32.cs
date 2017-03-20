using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace SEC.GenericSupport.WindowsAPI
{
	public sealed class Kernel32
	{
		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the library</param>
		/// <returns>A handle to the library</returns>
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string lpFileName);
	}
}
