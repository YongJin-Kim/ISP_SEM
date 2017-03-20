using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport
{
	public class GHeapManager
	{
		private static Dictionary<IntPtr,int> AllocedHeapDic;

		/// <summary>
		/// 비관리 힙영역을 할당 받는다.
		/// </summary>
		/// <param name="ptr">할당 받을 포인터</param>
		/// <param name="byteSize">할당 받을 크기(byte 단위)</param>
		/// <param name="allocater">할당을 요청하는자 - Debugging 용</param>
		public static void Alloc(ref IntPtr ptr, int byteSize, string allocater)
		{
			if (AllocedHeapDic == null) { AllocedHeapDic = new Dictionary<IntPtr, int>(); }

			ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(byteSize);
			GC.AddMemoryPressure(byteSize);

			AllocedHeapDic.Add(ptr, byteSize);
			System.Diagnostics.Debug.WriteLine(string.Format("AllocHGloal(0x{0:X}) at {1} by {2}", byteSize, ptr.ToString(), allocater), "GHeapManager");
		}

		/// <summary>
		/// 할당된 비관리 힙영역을 해제한다.
		/// 반듯이 Alloc(...)으로 할당된 영역 이어야 한다.
		/// </summary>
		/// <param name="ptr">할당 해제할 포인터. - Allco(..)으로 할당 되었어야 함.</param>
		/// <param name="freer">해제를 요청하는 자 - Debugging 용</param>
		public static void Free(ref IntPtr ptr, string freer)
		{
			System.Diagnostics.Trace.Assert(ptr != IntPtr.Zero);

			try
			{
				System.Diagnostics.Debug.WriteLine(string.Format("FreeHGlobal(0x{0:X}) at {1} by {2}", AllocedHeapDic[ptr], ptr.ToString(), freer), "GHeapManager");
			}
			catch (Exception) { }
			System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
			GC.RemoveMemoryPressure(AllocedHeapDic[ptr]);
			AllocedHeapDic.Remove(ptr);


			if (AllocedHeapDic.Count == 0) { AllocedHeapDic = null; }

			ptr = IntPtr.Zero;
		}
	}
}
