using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoStage.SNE5000M
{
	public interface IStage5000M : IStage
	{
		IAxis AxisX { get; }
		IAxis AxisY { get; }
		IAxis AxisR { get; }
		IAxis AxisT { get; }
		IAxis AxisZ { get; }

        CollisionAreaType CollisionData { get; set; }

        bool UseCollisionPrevention { get; set; }

        event EventHandler UseCollisionPrevention_ValueChanged;

	}

    /// <summary>
    /// 간섭방지기능은 Door와 T-Motor간의 간섭과, tilt시 Column과 시료대 간의 간섭, 두가지를 방지한다.
    /// Tilt 간섭의 경우 Z축이 0인 지점 위로 못 올라 가게 한다.
    /// um단위.
    /// </summary>
    public struct CollisionAreaType
    {
        /// <summary>
        /// Door와 T-Motor간의 X 축으로 부딪히는 거리
        /// </summary>
        public long DoorXRight;
        /// <summary>
        /// Door와 T-Motor간의 Y 축으로 부딪히는 거리
        /// </summary>
        public long DoorYBottom;

        /// <summary>
        /// 멀티 홀더 위의 시료대 윗면으로 부터 Tilit 중심점 까지의 거리.
        /// </summary>
        public long TiltHeight;
        /// <summary>
        /// 시료대의 반지름.
        /// </summary>
        public long TiltRadius;

        public CollisionAreaType(long doorX, long doorY, long tiltH, long tiltR)
        {
            DoorXRight = doorX;
            DoorYBottom = doorY;

            TiltHeight = tiltH;
            TiltRadius = tiltR;
        }
    }
}
