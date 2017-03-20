using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoView
{
	/// <summary>
	/// Serial Communication 객체
	/// </summary>
	public interface INanoView : IDisposable
	{
		/// <summary>
		/// 사용하는 포트 이름
		/// </summary>
		string PortName { get; set; }

		/// <summary>
		/// 동작 여부
		/// </summary>
		bool Enable { get; set; }

		/// <summary>
		/// 송수신 실패시 제시도 횟수
		/// </summary>
		int RetryCnt { get; set; }

		/// <summary>
		/// 포트 열기
		/// </summary>
		void Open();

		/// <summary>
		/// 포트 닫기
		/// </summary>
		void Close();
	}

	internal interface INanoViewCore : INanoView
	{
		PackeBase PacketControl { get; set; }

		event EventHandler<NanoViewErrorEventArgs> ErrorOccured;
	}
}
