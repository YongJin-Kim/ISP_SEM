using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoStage
{
	/// <summary>
	/// Stage 제어 객체의 기본 interface
	/// </summary>
	public interface IStage : SECtype.IController
	{
		/// <summary>
		/// Home Search가 되었는지를 확인함.
		/// </summary>
		bool IsHomeSearched { get; set; }

		/// <summary>
		/// 현재 Home Search 작업 중인지를 확인한다.
		/// </summary>
		bool IsHomeSearching { get; }

		/// <summary>
		/// Limit Sensor의 상태
		/// </summary>
		bool[] LimitSensor { get; }

		int HomeSearchMinimumSpeedModifier { get; set; }

		event EventHandler HomeSearchStateChanged;

		/// <summary>
		/// Home 검색 동작을 지정함.
		/// </summary>
		/// <param name="value">True이면 시작. False이면 강제 종료.</param>
		void HomeSearch(bool value);

		/// <summary>
		/// 모든 축을 긴급 정지 시킨다.
		/// </summary>
		void EmergencyStop();
	}

	internal interface IStageCoare : IStage, NanoView.IMasterObjet
	{
		void Send(UInt16 addr, byte[] datas);
	}
}
