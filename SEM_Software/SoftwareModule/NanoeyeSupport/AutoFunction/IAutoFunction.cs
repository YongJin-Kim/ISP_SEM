using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.Support.AutoFunction
{
	/// <summary>
	/// Auto Function. 시작 함수는 개별적으로 정의 한다.(시작 argument가 다르므로.)
	/// </summary>
	public interface IAutoFunction
	{
		/// <summary>
		/// 주 프로세스 진행 상태(0~100)
		/// </summary>
		int Progress { get; }
		/// <summary>
		/// 서브 프로세스의 진행 상태(0~100)
		/// </summary>
		int SubProcess { get; }

		/// <summary>
		/// 프로세스 이름
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// 작업중 취소 되었는지 여부
		/// </summary>
		bool Cancled { get; }

		/// <summary>
		/// Stop 버튼이 보여야 하는지 여부
		/// </summary>
		bool StopVisiable { get; }
		/// <summary>
		/// Cancel 버튼이 보여야 하는지 여부
		/// </summary>
		bool CancelVisiable { get; }
		/// <summary>
		/// 진행 바가 보여야 하는지 여부
		/// </summary>
		bool ProgressbarVisiable { get; }

		/// <summary>
		/// 서브 프롯세서의 Stop 버튼이 보여야 하는지 여부
		/// </summary>
		bool SubStopVisiable { get; }
		/// <summary>
		/// 서브 프로세서의 Cancel 버튼이 보여야 하는지 여부
		/// </summary>
		bool SubCancelVisiable { get; }
		/// <summary>
		/// 서브 프로세스의 진행 바가 보여야 하는지 여부
		/// </summary>
		bool SubProgressbarVisiable { get; }

		/// <summary>
		/// 프로세스가 취소 되었음
		/// </summary>
		event EventHandler ProgressChanged;

		/// <summary>
		/// 프로세스가 종료 됨
		/// </summary>
		event EventHandler ProgressComplet;

		event EventHandler CancelVisiableChanged;

		event EventHandler StopVisiableChanged;

		event EventHandler ProgressbarVisiableChanged;

		event EventHandler SubProgressChanged;

		event EventHandler SubCancelVisiableChanged;

		event EventHandler SubStopVisiableChanged;

		event EventHandler SubProgressbarVisiableChanged;

		/// <summary>
		/// 작업을 취소 한다. 변경한 값은 본래 값으로 복구 한다.
		/// </summary>
		void Cancel();

		/// <summary>
		/// 작업을 중지 한다. 지금까지 찾은 값 중, 가장 목표치와 비슷한 값으로 설정 한다.
		/// </summary>
		void Stop();

		void SubCancel();
		void SubStop();
	}
}
