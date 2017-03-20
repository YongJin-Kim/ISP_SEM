using System;

namespace SEC.Nanoeye.NanoImage.DataAcquation
{
	internal interface IDAQ : IDaqData, IDisposable
	{
		/// <summary>
		/// 동작 여부
		/// </summary>
		bool Enable { get;	}

		/// <summary>
		/// 테스크의 설정을 가져온다.
		/// </summary>
		SettingScanner[] SettingsRunning { get; }

		/// <summary>
		/// 동작 대기 중인 설정
		/// </summary>
		SettingScanner[] SettingsReady { get; }

		/// <summary>
		/// 동작을 시작한다.
		/// </summary>
		void Ready(SettingScanner[] set);

		/// <summary>
		/// 대기중인 설정으로 동작 시킨다.
		/// </summary>
		void Change();

		/// <summary>
		/// 동작을 중지한다.
		/// </summary>
		void Stop();

		/// <summary>
		/// 설정 값의 유효성을 검사 한다.
		/// </summary>
		/// <param name="setting"></param>
		/// <returns></returns>
		void ValidateSetting(SettingScanner setting);

		/// <summary>
		/// 연속 주사 동작이 가능한지 검증한다.
		/// </summary>
		/// <param name="setting"></param>
		void ValidateSequenceRunable(SettingScanner[] setting);

		void ShowInformation(System.Windows.Forms.IWin32Window owner);

		/// <summary>
		/// 한 지점에 연속 주사 한다.
		/// </summary>
		/// <param name="horizontal">수평 위치. -1.0 ~ 1.0</param>
		/// <param name="vertical">수직 위치. -1.0 ~ 1.0</param>
		void OnePoint(double horizontal, double vertical);

		/// <summary>
		/// 장치를 초기화 한다.
		/// </summary>
		void Reset();

        bool Revers { get; set; }

	}

	internal interface IDaqData
	{
		/// <summary>
		/// 읽기 가능한 sample수
		/// </summary>
		int ReadAvailables { get; }

		/// <summary>
		/// sample 읽기
		/// </summary>
		/// <param name="samples"></param>
		/// <returns></returns>
		short[,] Read(int samples);

        //int AnalogInComposite { get; set; }
        int AiChannel { get; set; }

        bool DualEnable { get; set; }

        void Reset();

       
    }
}
