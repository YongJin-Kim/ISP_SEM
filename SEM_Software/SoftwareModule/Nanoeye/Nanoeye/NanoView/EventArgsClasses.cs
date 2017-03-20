using System;
using System.Runtime.InteropServices;

namespace SEC.Nanoeye.NanoView
{
	internal enum ErrorType
	{
		Non,
		/// <summary>
		/// Transmision Fail
		/// </summary>
		TxFail,
		/// <summary>
		/// Receive Fail
		/// </summary>
		RxFail,
		/// <summary>
		/// No Response
		/// </summary>
		NoResponse,
		/// <summary>
		/// CRC Error
		/// </summary>
		CRC,
		/// <summary>
		/// Start Byte Error
		/// </summary>
		StartByte
	}

	internal class NanoViewErrorEventArgs : EventArgs
	{
		public readonly ErrorType Type;

		public readonly byte[]	Datas;

		public NanoViewErrorEventArgs(ErrorType err, byte[] datas)
		{
			this.Type = err;
			this.Datas = datas;
		}
	}
}
