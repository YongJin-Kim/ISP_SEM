using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SEC.GUIelement.Helper
{
	internal sealed class GuiPainter
	{
		/// <summary>
		/// 원형 버튼을 그린다.
		/// </summary>
		/// <param name="g">그릴 Graphics</param>
		/// <param name="rect">그릴 영역</param>
		/// <param name="col">기본 색</param>
		/// <param name="bs">버튼의 상태</param>
		public static void DrawButtonEllipse(Graphics g, Rectangle rect, Color col, ButtonStatesWithMouse bs)
		{
			SmoothingMode preSM = g.SmoothingMode;
			InterpolationMode preIM = g.InterpolationMode;
			CompositingQuality preCQ = g.CompositingQuality;

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.CompositingQuality = CompositingQuality.GammaCorrected;


			LinearGradientBrush lgb;

			if ((bs & ButtonStatesWithMouse.Disabled) == ButtonStatesWithMouse.Disabled)
			{
				lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Light(col), ControlPaint.Dark(col));
				g.FillEllipse(lgb, rect);
				return;
			}

			Color backcolor = col;
			Color forecolor = col;

			if ((bs & ButtonStatesWithMouse.MouseHover) == ButtonStatesWithMouse.MouseHover)
			{
				backcolor = Color.FromArgb(col.ToArgb() ^ 0x00ffffff);	// 색반전.
				forecolor = ControlPaint.Light(col);
			}

			if ((bs & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
			{
				backcolor = ControlPaint.Dark(backcolor);
				forecolor = ControlPaint.DarkDark(forecolor);
			}

			if ((bs == ButtonStatesWithMouse.ButtonPush) || (bs == ButtonStatesWithMouse.PushHover))
			{
				lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Dark(backcolor), ControlPaint.Light(backcolor));
				//lgb = new LinearGradientBrush(new Point(0, 0), new Point(this.ClientRectangle.Width, this.ClientRectangle.Height), ControlPaint.DarkDark(col), ControlPaint.LightLight(col));
			}
			else
			{
				lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Light(backcolor), ControlPaint.Dark(backcolor));
				//lgb = new LinearGradientBrush(new Point(0, 0), new Point(this.ClientRectangle.Width, this.ClientRectangle.Height), ControlPaint.LightLight(col), ControlPaint.DarkDark(col));
			}
			g.FillEllipse(lgb, rect);

			rect.Inflate(rect.Width / -10, rect.Height / -10);

			if ((bs & ButtonStatesWithMouse.Disabled) != ButtonStatesWithMouse.Disabled)
			{
				if ((bs == ButtonStatesWithMouse.ButtonPush) || (bs == ButtonStatesWithMouse.PushHover))
				{
					//lgb = new LinearGradientBrush(rect, ControlPaint.Light(forecolor, 0.9f), ControlPaint.Dark(forecolor, 0.1f), 45.0f);
					//lgb = new LinearGradientBrush(rect, ControlPaint.LightLight(col), ControlPaint.DarkDark(col), 45.0f);
					lgb = new LinearGradientBrush(rect, ControlPaint.Light(forecolor), forecolor, 90f, true);
				}
				else
				{
					lgb = new LinearGradientBrush(rect, ControlPaint.Dark(forecolor, 0.01f), forecolor, 90f, true);
					//lgb = new LinearGradientBrush(rect, ControlPaint.DarkDark(col), ControlPaint.LightLight(col), 45.0f);
					//lgb = new LinearGradientBrush(rect, Color.Blue, Color.Red, 90f, true);


				}
				lgb.SetBlendTriangularShape(0.5f, 1f);


				//g.FillEllipse(new SolidBrush(forecolor), rect);
				g.FillEllipse(lgb, rect);


			}

			g.SmoothingMode = preSM;
			g.InterpolationMode = preIM;
			g.CompositingQuality = preCQ;
		}

		/// <summary>
		/// 원형 버튼을 그린다.
		/// </summary>
		/// <param name="g">그릴 Graphics</param>
		/// <param name="rect">그릴 영역</param>
		/// <param name="col">기본 색</param>
		/// <param name="bs">버튼의 상태</param>
		public static void DrawButtonEllipse(Graphics g, Rectangle rect, Color backCol, Color foreStart, Color foreCenter, ButtonStatesWithMouse bs)
		{
			LinearGradientBrush lgb;

			if ((bs & ButtonStatesWithMouse.Disabled) == ButtonStatesWithMouse.Disabled)
			{
				lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Light(backCol), ControlPaint.Dark(backCol));
				g.FillEllipse(lgb, rect);
				return;
			}

			Color backcolor = backCol;
			Color foSt = foreStart;
			Color foCen = foreCenter;

			if ((bs & ButtonStatesWithMouse.MouseHover) == ButtonStatesWithMouse.MouseHover)
			{
				backcolor = Color.FromArgb(backcolor.ToArgb() ^ 0x00ffffff);	// 색반전.
			}

			if ((bs & ButtonStatesWithMouse.ButtonPush) == ButtonStatesWithMouse.ButtonPush)
			{
				foSt = ControlPaint.Dark(foreCenter, 0.05f);
				foCen = foreStart;

				//lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Dark(backcolor), ControlPaint.Light(backcolor));
				lgb = new LinearGradientBrush(rect, ControlPaint.Dark(backcolor), ControlPaint.Light(backcolor), 45f);
			}
			else
			{
				//lgb = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Bottom), ControlPaint.Light(backcolor), ControlPaint.Dark(backcolor));
				//lgb = new LinearGradientBrush(new Point(0, 0), new Point(this.ClientRectangle.Width, this.ClientRectangle.Height), ControlPaint.LightLight(col), ControlPaint.DarkDark(col));
				lgb = new LinearGradientBrush(rect, ControlPaint.Light(backcolor), ControlPaint.Dark(backcolor), 45f);
			}

			g.FillEllipse(lgb, rect);
			//g.FillRectangle(lgb, rect);

			rect.Inflate(rect.Width / -10, rect.Height / -10);

			lgb = new LinearGradientBrush(rect, foSt, foCen, 90f, true);
			lgb.SetBlendTriangularShape(0.5f, 1f);

			g.FillEllipse(lgb, rect);

			lgb.Dispose();
		}

		public static void DrawAquaPill(Graphics g, RectangleF drawRectF, Color drawColor, Orientation orientation)
		{
			Color color1;
			Color color2;
			Color color3;
			Color color4;
			Color color5;
			System.Drawing.Drawing2D.LinearGradientBrush gradientBrush;
			System.Drawing.Drawing2D.ColorBlend colorBlend = new System.Drawing.Drawing2D.ColorBlend();

			color1 = Helper.ColorMixer.OpacityMix(Color.White, Helper.ColorMixer.SoftLightMix(drawColor, Color.Black, 100), 40);
			color2 = Helper.ColorMixer.OpacityMix(Color.White, Helper.ColorMixer.SoftLightMix(drawColor, Color.FromArgb(64, 64, 64), 100), 20);
			color3 = Helper.ColorMixer.SoftLightMix(drawColor, Color.FromArgb(128, 128, 128), 100);
			color4 = Helper.ColorMixer.SoftLightMix(drawColor, Color.FromArgb(192, 192, 192), 100);
			color5 = Helper.ColorMixer.OverlayMix(Helper.ColorMixer.SoftLightMix(drawColor, Color.White, 100), Color.White, 75);

			//			
			colorBlend.Colors = new Color[] { color1, color2, color3, color4, color5 };
			colorBlend.Positions = new float[] { 0, 0.25f, 0.5f, 0.75f, 1 };
			if (orientation == Orientation.Horizontal)
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left, (int)drawRectF.Top - 1), new Point((int)drawRectF.Left, (int)drawRectF.Top + (int)drawRectF.Height + 1), color1, color5);
			}
			else
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left - 1, (int)drawRectF.Top), new Point((int)drawRectF.Left + (int)drawRectF.Width + 1, (int)drawRectF.Top), color1, color5);
			}
			gradientBrush.InterpolationColors = colorBlend;
			FillPill(g, gradientBrush, drawRectF);

			//
			color2 = Color.White;
			colorBlend.Colors = new Color[] { color2, color3, color4, color5 };
			colorBlend.Positions = new float[] { 0, 0.5f, 0.75f, 1 };
			if (orientation == Orientation.Horizontal)
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left + 1, (int)drawRectF.Top), new Point((int)drawRectF.Left + 1, (int)drawRectF.Top + (int)drawRectF.Height - 1), color2, color5);
			}
			else
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left, (int)drawRectF.Top + 1), new Point((int)drawRectF.Left + (int)drawRectF.Width - 1, (int)drawRectF.Top + 1), color2, color5);
			}
			gradientBrush.InterpolationColors = colorBlend;
			FillPill(g, gradientBrush, RectangleF.Inflate(drawRectF, -3, -3));

		}

		public static void DrawAquaPillSingleLayer(Graphics g, RectangleF drawRectF, Color drawColor, Orientation orientation)
		{
			Color color1;
			Color color2;
			Color color3;
			Color color4;
			System.Drawing.Drawing2D.LinearGradientBrush gradientBrush;
			System.Drawing.Drawing2D.ColorBlend colorBlend = new System.Drawing.Drawing2D.ColorBlend();

			color1 = drawColor;
			color2 = ControlPaint.Light(color1);
			color3 = ControlPaint.Light(color2);
			color4 = ControlPaint.Light(color3);

			colorBlend.Colors = new Color[] { color1, color2, color3, color4 };
			colorBlend.Positions = new float[] { 0, 0.25f, 0.65f, 1 };

			if (orientation == Orientation.Horizontal)
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left, (int)drawRectF.Top), new Point((int)drawRectF.Left, (int)drawRectF.Top + (int)drawRectF.Height), color1, color4);
			}
			else
			{
				gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)drawRectF.Left, (int)drawRectF.Top), new Point((int)drawRectF.Left + (int)drawRectF.Width, (int)drawRectF.Top), color1, color4);
			}
			gradientBrush.InterpolationColors = colorBlend;

			FillPill(g, gradientBrush, drawRectF);

		}

		public static void FillPill(Graphics g, Brush b, RectangleF rect)
		{
			g.SmoothingMode = SmoothingMode.HighQuality;
			if (rect.Width > rect.Height)
			{
				g.FillEllipse(b, new RectangleF(rect.Left, rect.Top, rect.Height, rect.Height));
				g.FillEllipse(b, new RectangleF(rect.Left + rect.Width - rect.Height, rect.Top, rect.Height, rect.Height));

				float w = rect.Width - rect.Height;
				float l = rect.Left + ((rect.Height) / 2);
				g.FillRectangle(b, new RectangleF(l, rect.Top, w, rect.Height));
			}
			else if (rect.Width < rect.Height)
			{
				g.FillEllipse(b, new RectangleF(rect.Left, rect.Top, rect.Width, rect.Width));
				g.FillEllipse(b, new RectangleF(rect.Left, rect.Top + rect.Height - rect.Width, rect.Width, rect.Width));

				float t = rect.Top + (rect.Width / 2);
				float h = rect.Height - rect.Width;
				g.FillRectangle(b, new RectangleF(rect.Left, t, rect.Width, h));
			}
			else if (rect.Width == rect.Height)
			{
				g.FillEllipse(b, rect);
			}
			g.SmoothingMode = SmoothingMode.Default;
		}
	}
}
