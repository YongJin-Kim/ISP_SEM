using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SEC.GenericSupport.WindowsAPI
{
	public sealed class User32
	{
		public const int WS_OVERLAPPED       = 0x00000000;
		public const int WS_POPUP            = int.MinValue;
		public const int WS_CHILD            = 0x40000000;
		public const int WS_MINIMIZE         = 0x20000000;
		public const int WS_VISIBLE          = 0x10000000;
		public const int WS_DISABLED         = 0x08000000;
		public const int WS_CLIPSIBLINGS     = 0x04000000;
		public const int WS_CLIPCHILDREN     = 0x02000000;
		public const int WS_MAXIMIZE         = 0x01000000;
		public const int WS_CAPTION          = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
		public const int WS_BORDER           = 0x00800000;
		public const int WS_DLGFRAME         = 0x00400000;
		public const int WS_VSCROLL          = 0x00200000;
		public const int WS_HSCROLL          = 0x00100000;
		public const int WS_SYSMENU          = 0x00080000;
		public const int WS_THICKFRAME       = 0x00040000;
		public const int WS_GROUP            = 0x00020000;
		public const int WS_TABSTOP          = 0x00010000;

		public const int WS_MINIMIZEBOX      = 0x00020000;
		public const int WS_MAXIMIZEBOX      = 0x00010000;

		[DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


		public delegate int HookMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		//Declare wrapper managed POINT class.
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
		}

		//Declare wrapper managed MouseHookStruct class.
		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStruct
		{
			public POINT pt;
			public int hwnd;
			public int wHitTestCode;
			public int dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStructEx
		{
			public MouseHookStruct mhs;
			public int mouseData;
		}

		//Import for SetWindowsHookEx function.
		//Use this function to install thread-specific hook.
		[DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookMouseProc lpfn, IntPtr hInstance, int threadId);

		//Import for UnhookWindowsHookEx.
		//Call this function to uninstall the hook.
		[DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		//Import for CallNextHookEx.
		//Use this function to pass the hook information to next hook procedure in chain.
		[DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

	}
}
