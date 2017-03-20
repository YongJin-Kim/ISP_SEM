using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// 제어집합의 기본 객체.
	/// </summary>
	public interface IController : IDisposable, System.Collections.IEnumerable
	{
		/// <summary>
		/// 동작 여부.
		/// </summary>
		bool Enable { get; set; }

		/// <summary>
		/// 초기화 되었는지를 알려 준다.
		/// </summary>
		bool Initialized { get; }

		/// <summary>
		/// 이름
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// 객체가 파괴되었음을 확인 함.
		/// </summary>
		bool IsDisposed { get; }

		/// <summary>
		/// ControlValue에서 통신 에러 이벤트가 발생하면
		/// 이 객체의 CommunicationErrorOccured Event에도 발생시킨다.
		/// </summary>
		bool RedirectControlValueError { get; set; }

		/// <summary>
		/// 보드 이름을 통해 보드가 제어하는 IControlValue의 목록을 가져옴.
		/// </summary>
		/// <param name="name">ControlValue 명</param>
		/// <returns>ControlValue</returns>
		IValue this[string name] { get; }

		/// <summary>
		/// 사용 되는 장치
		/// </summary>
		string Device { get; set; }

		/// <summary>
		/// 사용 가능한 장치 목록
		/// </summary>
		/// <returns></returns>
		string[] AvailableDevices();

		/// <summary>
		/// Controller 설정을 초기화 한다.
		/// </summary>
		/// <returns></returns>
		void Initialize();

		/// <summary>
		/// 사용되고 있는 보드의 목록과 각종 정보들
		/// </summary>
		/// <param name="information">보드 정보</param>
		/// <returns>Controller Type</returns>
		int ControlBoard(out string[,] information);

		/// <summary>
		/// Mini-SEM 판매 명. (such as, SNE-1500M, SNE-3000M...)
		/// </summary>
		/// <returns></returns>
		string GetControllerType();

		/// <summary>
		/// Enable 변경됨을 알림
		/// </summary>
		event EventHandler EnableChanged;

		/// <summary>
		/// 통신 에러가 발생함.
		/// </summary>
		event EventHandler<CommunicationErrorOccuredEventArgs> CommunicationErrorOccured;
	}
}
