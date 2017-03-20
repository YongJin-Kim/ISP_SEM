using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.GenericSupport.DataType
{
	/// <summary>
	/// ControlValue의 기본 interface
	/// </summary>
	public interface IValue : System.ComponentModel.ISupportInitialize, System.IDisposable
	{
		/// <summary>
		/// 값 읽기
		/// </summary>
		object[] Read { get; }

		/// <summary>
		/// 초기화 여부
		/// </summary>
		bool IsInitied { get; }

		/// <summary>
		/// 제어 명
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 소유자
		/// </summary>
		Object Owner { get; }

		/// <summary>
		/// 동작 여부. 값이 false이면 어떠한 동작도 취하지 않는다.
		/// </summary>
		bool Enable { get; set; }

		/// <summary>
		/// 값 설정 중 예외 상황이 발생할 경우 Exception을 발생 시키지 복구 할지를 결정 한다.
		/// DEBUG 모드에서는 Exception을 발생 시키고, 
		/// RUN 모드에서는 복구를 하기를 권장 한다.
		/// 기본 값은 false이다.
		/// </summary>
		bool ThrowException { get; set; }

		/// <summary>
		/// 통신상에 장애가 있음.
		/// </summary>
		event EventHandler CommunicationError;

		/// <summary>
		/// Enable 값이 바뀜.
		/// </summary>
		event EventHandler EnableChanged;

		/// <summary>
		/// Board와 동기화 한다.
		/// </summary>
		void Sync();

		/// <summary>
		/// Board와 값이 일치하는지 확인 한다.
		/// </summary>
		/// <returns></returns>
		bool Validate();

		/// <summary>
		/// 초기화가 완료됨은 객체에 알림.
		/// </summary>
		/// <param name="sync">true이면 값을 제어 대상으로 부터 읽어 옴.</param>
		void EndInit(bool sync);
	}
}
