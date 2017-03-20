using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoImage
{
	public interface IActiveScan : IDisposable
	{
		#region Event
		/// <summary>
		/// Scanning이 시작 되었음을 알림.
		/// </summary>
		event EventHandler ScanningStarted;

		/// <summary>
		/// Scanning이 중지 되었음을 알림.
		/// ScanningStopping Event없이 발생 할 수 있음.
		/// </summary>
		event EventHandler ScanningStopped;

		/// <summary>
		/// Scanning 중지를 시도함을 알림.
		/// </summary>
		event EventHandler ScanningStopping;
		#endregion

		/// <summary>
		/// 사용중인 DAQ 장치 명
		/// </summary>
		string DaqDevice { get; }

		/// <summary>
		/// 사용 가능한 DAQ 장치 목록을 가져 옮.
		/// </summary>
		/// <returns>DAQ 장치 목록</returns>
		string[] GetDevList();

		/// <summary>
		/// DAQ 장치를 이용하여 초기화 함.
		/// </summary>
		/// <param name="dev">DAQ 장치 명</param>
		/// <returns>성공 여부</returns>
		void Initialize(string dev);

		/// <summary>
		/// 연속 주사 동작이 가능한지 여부를 판단한다.
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		bool IsSequenceRunable(SettingScanner[] settings);

		/// <summary>
		/// 주사 동작을 중지 시킴
		/// </summary>
		/// <param name="immediately">즉시 중지 여부</param>
		void Stop(bool immediately);

		/// <summary>
		/// 설정 값을 이용하여 지정한 횟수 만큼 주사 동작준비 시킨다.
		/// </summary>
		/// <param name="settings">설정</param>
		/// <param name="count">횟수</param>
		void Ready(SettingScanner[] settings, int count);

		/// <summary>
		/// 준비된 주사 동작을 실행 한다.
		/// </summary>
		void Change();

		/// <summary>
		/// 현재 동작 중인지를 확인한다.
		/// </summary>
		bool IsRun { get; }

		/// <summary>
		/// 동작중인 주사 목록을 가져 온다.
		/// </summary>
		IScanItemEvent[] ItemsRunning { get; }

		/// <summary>
		/// 동작중인 주사 목록을 가져 온다.
		/// </summary>
		IScanItemEvent[] ItemsReady { get; }

		/// <summary>
		/// 설정의 유효성을 검사 한다.
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		void ValidateSetting(SettingScanner set);

		/// <summary>
		/// 주사 장치의 정보 창을 연다.
		/// </summary>
		/// <param name="owner"></param>
		void ShowInformation(System.Windows.Forms.IWin32Window owner);

		/// <summary>
		/// 한 지점에 연속 주사 한다.
		/// </summary>
		/// <param name="horizontal">수평 위치. -1.0 ~ 1.0</param>
		/// <param name="vertical">수직 위치. -1.0 ~ 1.0</param>
		void OnePoint(double horizontal, double vertical);



        void ScanMode(bool p);

        void Revers(bool enable);

        void AFTest(string path);

        void scanFrameStop();

        void DualMode(bool DualEnable);

       
    }
}
