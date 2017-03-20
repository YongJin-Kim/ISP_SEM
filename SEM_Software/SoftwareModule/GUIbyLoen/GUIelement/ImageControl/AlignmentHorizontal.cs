using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SEC.Nanoeye.Controls
{
    internal enum AlignmentHorizontal
    {
        Near =
            ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft,
        Center =
            ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter,
        Far =
            ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight
    }
}
