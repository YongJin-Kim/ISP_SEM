using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoImage
{
	public sealed class ScanModifier
	{
		private ScanModifier() { }

		/// <summary>
		/// 주사 영역을 축소 시킨다.
		/// </summary>
		/// <param name="ss">수정할 Scan 설정</param>
		/// <param name="ratioW">넓이 축소 비율 (0~1)</param>
		/// <param name="ratioH">높이 축소 비율 (0~1)</param>
		/// <param name="widthPaintArea">표시 영역도 축소 시킬지 여부</param>
		/// <returns></returns>
		public static SettingScanner Reduce(SettingScanner ssArg, double ratioW, double ratioH, bool widthPaintArea)
		{
			SettingScanner ss = (SettingScanner)ssArg.Clone();

			ss.RatioX = ss.RatioX * ratioW;
			ss.RatioY = ss.RatioY * ratioH;

			if (widthPaintArea)
			{
				double reducedW = ss.PaintWidth * ratioW;
				double reducedH = ss.PaintHeight * ratioH;
				ss.PaintX = (float)(ss.PaintX - (ss.PaintWidth - reducedW) / 2);
				ss.PaintY = (float)(ss.PaintY - (ss.PaintHeight - reducedH) / 2);
				ss.PaintWidth = (float)reducedW;
				ss.PaintHeight = (float)reducedH;
			}

			return ss;
		}

		/// <summary>
		/// 주사를 이동 한다.
		/// </summary>
		/// <param name="ss">수정할 설정</param>
		/// <param name="shiftHorizontal">수평 이동 도, 표시되는 영역 기준임.</param>
		/// <param name="shiftVertical">수직 이동 도, 표시되는 영역 기준임.</param>
		/// <returns></returns>
		public static SettingScanner Shift(SettingScanner ssArg, double shiftHorizontal, double shiftVertical)
		{
			SettingScanner ss = (SettingScanner)ssArg.Clone();

			double sH =  ss.PaintWidth * shiftHorizontal;
			double sV =  ss.PaintHeight * shiftVertical;

			ss.RatioX += ss.AreaShiftX * sH;
			ss.RatioY += ss.AreaShiftY * sV;

			ss.PaintX = (float)(ss.PaintX + sH);
			ss.PaintY = (float)(ss.PaintY + sV);

			return ss;
		}

		public enum AreaSelectDividEnum
		{
			Half,
			Quard
		}

		/// <summary>
		/// Area Scan을 한다.
		/// </summary>
		/// <param name="ss">수정할 설정</param>
		/// <param name="asde">분할 방식</param>
		/// <param name="shiftHorizontal">수평 이동 도</param>
		/// <param name="shiftVertical">수직 이동 도</param>
		/// <returns></returns>
		public static SettingScanner Area(SettingScanner ssArg, AreaSelectDividEnum asde, double shiftHorizontal, double shiftVertical)
		{
			SettingScanner ss = (SettingScanner)ssArg.Clone();

			switch (asde)
			{
			case AreaSelectDividEnum.Half:
				ss = Reduce(ss, 0.5, 0.5, true);
				break;
			case AreaSelectDividEnum.Quard:
				ss = Reduce(ss, 0.25, 0.25, true);
				break;
			default:
				throw new ArgumentException("Undefined Reduce mode");
			}

			ss = Shift(ss, shiftHorizontal, shiftVertical);
			return ss;
		}
	}
}
