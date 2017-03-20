using System;
using System.Runtime.InteropServices;

namespace SEC.Nanoeye.NanoImage
{
    /// <summary>
    /// 개별적 주사 설정의 동작과 관련된 인터페이스.
    /// </summary>
    public interface IScanItemEvent
    {
        /// <summary>
        /// 새로운 이미지 데이타가 업데이트 됨.
        /// </summary>
        event ScanDataUpdateDelegate ScanLineUpdated;

        /// <summary>
        /// 프레임 전체가 업데이트 됨.
        /// </summary>
        event ScanDataUpdateDelegate FrameUpdated;

        /// <summary>
        /// Dispse를 시작하였음을 알림.
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        /// Disposed 되었음을 알림.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// 이미지의 original 정보가 들어있는 배열 ref. (각종 porch 제거되고 Frame Average 및 Bluring 적용 됨) 
        /// </summary>
        IntPtr ImageData { get; }

        IntPtr ImageData2 { get; }

        /// <summary>
        /// 파괴되었는지를 확인
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 주사 설정의 이름. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 수집한 프레임수
        /// </summary>
        int ScanningScanned { get; }

        SettingScanner Setting { get; }
    }

    /// <summary>
    /// 스캔 데이타가 업데이트 됨을 알림
    /// </summary>
    /// <param name="name">주사 설정 이름</param>
    /// <param name="startline">주사 시작 라인</param>
    /// <param name="lines">획득한 라인 수</param>
    [ComVisible(false)]
    public delegate void ScanDataUpdateDelegate(object sender, string name, int startline, int lines);

}
