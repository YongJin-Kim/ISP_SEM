using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using SEC.GenericSupport.Mathematics;

namespace SEC.Nanoeye.Support.Controls
{
	public class ScalebarDrawer
	{
		public enum TickStyle
		{
			Ellipse,
			Rectangle
		}

		private ScalebarDrawer() { }

		/// <summary>
		/// 미크론바의 이미지를 생성한다.
		/// </summary>
		/// <param name="area"></param>
		/// <param name="backColor"></param>
		/// <param name="foreColor"></param>
		/// <param name="egdeColor"></param>
		/// <param name="discription"></param>
		/// <param name="hvAlias"></param>
		/// <param name="magAlias"></param>
		/// <param name="dpiSystem"></param>
		/// <param name="realMagnificaion"></param>
		/// <param name="padding"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static Bitmap MakeMicronbar(Size area,
											Color backColor,
											Color foreColor,
											Color egdeColor,
											Font stringFont,
											string discription,
											string hvAlias,
											string magAlias,
											double pixelWidth,
											double realMagnificaion,
											System.Windows.Forms.Padding padding,
											TickStyle style)
		{
			System.Diagnostics.Trace.Assert(pixelWidth != 0);

			// 이미지 생성 및 초기화
			Bitmap bm = new Bitmap(area.Width, area.Height);
			Graphics g = Graphics.FromImage(bm);
			g.Clear(backColor);

			RectangleF scalebarRect = new RectangleF(0, 0, 0, 0);
			RectangleF desciptorRect = new RectangleF(0, 0, 0, 0);
			RectangleF hvRect = new RectangleF(0, 0, 0, 0);
			RectangleF magRect = new RectangleF(0, 0, 0, 0);

			#region 각 종류별 표시 영역 결정
			if ( area.Width < 600 )
			{	// Scale Bar만 표시
				desciptorRect = new RectangleF(0, 0, 0, 0);
				hvRect = new RectangleF(0, 0, 0, 0);
				magRect = new RectangleF(0, 0, 0, 0);
				scalebarRect = new RectangleF(padding.Left, padding.Top, area.Width - padding.Horizontal, area.Height - padding.Vertical);
			}
			else if ( area.Width < 800 )
			{ // Descriptor, Scale Bar
				//desciptorRect = new RectangleF((micronArea.Width / 20), micronBlank / 2, (micronArea.Width / 4), micronArea.Height - micronBlank);
				desciptorRect = new RectangleF(padding.Left, padding.Top, ((area.Width - padding.Horizontal * 2) / 4), area.Height - padding.Vertical);
				hvRect = new RectangleF(0, 0, 0, 0);
				magRect = new RectangleF(0, 0, 0, 0);
				scalebarRect = new RectangleF(desciptorRect.Right + padding.Horizontal, padding.Top, ((area.Width - padding.Horizontal * 2) * 3 / 4), area.Height - padding.Vertical);
			}
			else
			{ // Discriptor, HV, Mag, Scale Bar
				float disWidth = (area.Width - padding.Vertical * 4);
				float disHeight = area.Height - padding.Vertical;
				desciptorRect = new RectangleF(padding.Left, padding.Top, (disWidth / 2 - 128), disHeight);
				hvRect = new RectangleF(desciptorRect.Right + padding.Horizontal, padding.Top, 64, disHeight);
				magRect = new RectangleF(hvRect.Right + padding.Horizontal, padding.Top, 64, disHeight);
				scalebarRect = new RectangleF(magRect.Right + padding.Horizontal, padding.Top, disWidth / 2, disHeight);
			}
			#endregion

			#region 그리기
			Brush textBrush = new SolidBrush(foreColor);
			Brush edgeBrush = new SolidBrush(egdeColor);

			if ( desciptorRect.Width > 0 )
			{
				DrawEdgeString(discription, g, desciptorRect, textBrush, edgeBrush, stringFont);
			}

			if ( hvRect.Width > 0 )
			{
				DrawEdgeString(hvAlias, g, hvRect, textBrush, edgeBrush, stringFont);
			}

			if ( magRect.Width > 0 )
			{
				DrawEdgeString(magAlias, g, magRect, textBrush, edgeBrush, stringFont);
			}

			DrawTicks(realMagnificaion, pixelWidth, g, scalebarRect, style, textBrush, edgeBrush, stringFont);

			textBrush.Dispose();
			edgeBrush.Dispose();
			#endregion

			return bm;
		}

		private static void DrawTicks(double realMagnificaion, double pixelWidth, Graphics g, RectangleF scalebarRect, TickStyle style, Brush textBrush, Brush edgeBrush, Font strFont)
		{
			SizeF tickSmall = new SizeF(6, 8);
			SizeF tickLarge = new SizeF(12, 16);

			#region 실제 그릴 거리 측정
			//double phisicalLength = ((double)scalebarRect.Width - tickLarge.Width) * pixelWidth / realMagnificaion;
			double phisicalLength = ((double)scalebarRect.Width - tickLarge.Width) * pixelWidth;

			Engineer scale = new Engineer(phisicalLength);
			scale = Engineer.ApproximateMantissa(scale, new double[] { 1, 2, 3, 5 });

			phisicalLength = scale.Value;

			// 실제로 그릴 pixels
			//float drawLength = (float)(phisicalLength * realMagnificaion / pixelWidth);
			float drawLength = (float)(phisicalLength  / pixelWidth);
			#endregion

			#region tick 위치 결정

			const int tickCnt = 11;

			float tickStart = ((float)scalebarRect.Width - drawLength) / 2;
			float tickEnd = tickStart + drawLength;
			float tickY = tickLarge.Height / 2;

			PointF[] pntsTicks = CalTicksLocation(tickStart, tickEnd, tickY, tickCnt);
			#endregion

			#region Tick 그리기
			SmoothingMode oriSM = g.SmoothingMode;

			g.SmoothingMode = SmoothingMode.AntiAlias;

			Action<Brush,RectangleF> tdd;

			switch ( style )
			{
			case TickStyle.Ellipse:
				tdd = g.FillEllipse;
				break;
			case TickStyle.Rectangle:
				tdd = g.FillRectangle;
				break;
			default:
				throw new ArgumentException("Undefined TickStyle");
			}

			SizeF tickSize;

			for ( int i = 0 ; i < tickCnt ; i++ )
			{
				if ( i % (tickCnt - 1) > 0 )
				{
					tickSize = tickSmall;
				}
				else
				{
					tickSize = tickLarge;
				}

				RectangleF rect = new RectangleF(pntsTicks[i].X - tickSize.Width / 2f + scalebarRect.Left, pntsTicks[i].Y - tickSize.Height / 2F + scalebarRect.Top, tickSize.Width, tickSize.Height);
				rect.Offset(1, 1);
				tdd(edgeBrush, rect);
				rect.Offset(0, -2);
				tdd(edgeBrush, rect);
				rect.Offset(-2, 0);
				tdd(edgeBrush, rect);
				rect.Offset(0, 2);
				tdd(edgeBrush, rect);
				rect.Offset(1, -1);
				tdd(textBrush, rect);
			}

			g.SmoothingMode = oriSM;
			#endregion

			#region Scale
			RectangleF scaleRect = new RectangleF(scalebarRect.X, scalebarRect.Y + tickLarge.Height, scalebarRect.Width, scalebarRect.Height - tickLarge.Height);
			DrawScale(g, strFont, scale, scaleRect, textBrush, edgeBrush);
			#endregion

		}

		private static PointF[] CalTicksLocation(float tickStart, float tickEnd, float tickY, int cnt)
		{
			PointF[] ticks = new PointF[cnt];

			float length = tickEnd - tickStart;

			for ( int i = 0 ; i < cnt ; i++ )
			{
				ticks[i].X = tickStart + length * i / (cnt - 1);
				ticks[i].Y = tickY;
			}

			return ticks;
		}

		private static void DrawEdgeString(string str, Graphics g, RectangleF rectDisplay, Brush textBrush, Brush edgeBrush, Font stringFont)
		{
			StringFormat style = new StringFormat();
			style.Alignment = StringAlignment.Center;
			style.LineAlignment = StringAlignment.Center;

			RectangleF rect = rectDisplay;
			rect.Offset(1, 1);
			g.DrawString(str, stringFont, edgeBrush, rect, style);
			rect.Offset(0, -2);
			g.DrawString(str, stringFont, edgeBrush, rect, style);
			rect.Offset(-2, 0);
			g.DrawString(str, stringFont, edgeBrush, rect, style);
			rect.Offset(0, 2);
			g.DrawString(str, stringFont, edgeBrush, rect, style);

			g.DrawString(str, stringFont, textBrush, rectDisplay, style);

		}

		private static void DrawScale(
						Graphics g,
						Font font,
						Engineer scale,
						RectangleF bounds,
						Brush textBrush,
						Brush edgeBrush)
		{
			string[] unitTable = { " km", " m", " mm", " um", " nm", " pm", " fm" };
			int unitIndex = 0;
			double value = 0;

			for ( int i = 0 ; i < unitTable.Length ; i++ )
			{
				int max = (i - 2) * -3;
				int min = (i - 1) * -3;

				if ( min <= scale.Exponent && scale.Exponent < max )
				{
					unitIndex = i;
					value = scale.Mantissa * Math.Pow(10, scale.Exponent - min);
					//Trace.WriteLine("Mantissa : "+scale.Mantissa.ToString());
					break;
				}
			}

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Far;

			string drawStr = value.ToString("0") + unitTable[unitIndex];

			bounds.Offset(1, 1);
			g.DrawString(drawStr, font, edgeBrush, bounds, format);
			bounds.Offset(0, -2);
			g.DrawString(drawStr, font, edgeBrush, bounds, format);
			bounds.Offset(-2, 0);
			g.DrawString(drawStr, font, edgeBrush, bounds, format);
			bounds.Offset(0, 2);
			g.DrawString(drawStr, font, edgeBrush, bounds, format);
			bounds.Offset(1, -1);
			g.DrawString(drawStr, font, textBrush, bounds, format);
		}

	}
}
