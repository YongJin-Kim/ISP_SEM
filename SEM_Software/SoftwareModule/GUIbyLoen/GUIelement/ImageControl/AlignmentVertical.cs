using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SEC.Nanoeye.Controls
{
    internal enum AlignmentVertical
    {
        Near =
            ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight,
        Center =
            ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight,
        Far =
            ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight
    }
}
