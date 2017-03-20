using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SEC.GUIelement.Helper
{
	internal class ColorMixer
	{
		public static Color OpacityMix(Color blendColor, Color baseColor, int opacity)
		{
			int r1 = blendColor.R;
			int g1 = blendColor.G;
			int b1 = blendColor.B;
			int r2 = baseColor.R;
			int g2 = baseColor.G;
			int b2 = baseColor.B;
			int r3;
			int g3;
			int b3;
			r3 = (int)(((r1 * ((float)opacity / 100)) + (r2 * (1 - ((float)opacity / 100)))));
			g3 = (int)(((g1 * ((float)opacity / 100)) + (g2 * (1 - ((float)opacity / 100)))));
			b3 = (int)(((b1 * ((float)opacity / 100)) + (b2 * (1 - ((float)opacity / 100)))));
			return Color.FromArgb(r3, g3, b3);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseColor"></param>
		/// <param name="blendColor"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		public static Color SoftLightMix(Color baseColor, Color blendColor, int opacity)
		{
			int r1;
			int g1;
			int b1;
			int r2;
			int g2;
			int b2;
			int r3;
			int g3;
			int b3;
			r1 = baseColor.R;
			g1 = baseColor.G;
			b1 = baseColor.B;
			r2 = blendColor.R;
			g2 = blendColor.G;
			b2 = blendColor.B;
			r3 = SoftLightMath(r1, r2);
			g3 = SoftLightMath(g1, g2);
			b3 = SoftLightMath(b1, b2);
			return OpacityMix(Color.FromArgb(r3, g3, b3), baseColor, opacity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseColor"></param>
		/// <param name="blendColor"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		public static Color OverlayMix(Color baseColor, Color blendColor, int opacity)
		{
			int r1;
			int g1;
			int b1;
			int r2;
			int g2;
			int b2;
			int r3;
			int g3;
			int b3;
			r1 = baseColor.R;
			g1 = baseColor.G;
			b1 = baseColor.B;
			r2 = blendColor.R;
			g2 = blendColor.G;
			b2 = blendColor.B;
			r3 = OverlayMath(baseColor.R, blendColor.R);
			g3 = OverlayMath(baseColor.G, blendColor.G);
			b3 = OverlayMath(baseColor.B, blendColor.B);
			return OpacityMix(Color.FromArgb(r3, g3, b3), baseColor, opacity);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ibase"></param>
		/// <param name="blend"></param>
		/// <returns></returns>
		private static int SoftLightMath(int ibase, int blend)
		{
			float dbase;
			float dblend;
			dbase = (float)ibase / 255;
			dblend = (float)blend / 255;
			if (dblend < 0.5)
			{
				return (int)(((2 * dbase * dblend) + (Math.Pow(dbase, 2)) * (1 - (2 * dblend))) * 255);
			}
			else
			{
				return (int)(((Math.Sqrt(dbase) * (2 * dblend - 1)) + ((2 * dbase) * (1 - dblend))) * 255);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ibase"></param>
		/// <param name="blend"></param>
		/// <returns></returns>
		public static int OverlayMath(int ibase, int blend)
		{
			double dbase;
			double dblend;
			dbase = (double)ibase / 255;
			dblend = (double)blend / 255;
			if (dbase < 0.5)
			{
				return (int)((2 * dbase * dblend) * 255);
			}
			else
			{
				return (int)((1 - (2 * (1 - dbase) * (1 - dblend))) * 255);
			}
		}
	}
}
