using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SEC.Nanoeye.Controls.ScaleBar
{
	public partial class ScaleBar : Control
	{
		private SizeF m_SmallTick = new SizeF(8, 8);
		private SizeF m_LargeTick = new SizeF(8, 16);
		private PointF[] m_Ticks;
		//private double m_Length = 0;
		private int m_TickFrequency = 10;
		private int m_TickCount = 11;
		private double m_ValuePerPixel = 1.0;
		private TickStyle m_TickStyle = TickStyle.Ellipse;

		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public TickStyle TickStyle
		{
			get { return m_TickStyle; }
			set
			{
				if (m_TickStyle != value) {
					m_TickStyle = value;
					ReGenerateImage();
				}
			}
		}

		[DefaultValue("8, 8")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public SizeF SmallTick
		{
			get { return m_SmallTick; }
			set
			{
				if (m_SmallTick != value)
				{
					m_SmallTick = value;
					ReGenerateImage();
				}
			}
		}

		[DefaultValue("8, 16")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public SizeF LargeTick
		{
			get { return m_LargeTick; }
			set
			{
				if (m_LargeTick != value)
				{
					m_LargeTick = value;
					ReGenerateImage();
				}
			}
		}

		[DefaultValue(11)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int TickCount
		{
			get { return m_TickCount; }
			set
			{
				if (m_TickCount != value)
				{
					m_TickCount = value;
					ReGenerateImage();
				}
			}
		}

		[DefaultValue(10)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int TickFrequency
		{
			get { return m_TickFrequency; }
			set
			{
				if (value <= 0)
				{
					throw new InvalidOperationException("TickFrequency가 0보다 큰 정수가 아닙니다.");
				}

				m_TickFrequency = value;

				ReGenerateImage();
			}
		}

		/// <summary>
		/// 1 픽셀당 미터단위의 값입니다.
		/// </summary>
		[DefaultValue(1.0)]
		public double ValuePerPixel
		{
			get { return m_ValuePerPixel; }
			set
			{
				if (m_ValuePerPixel != value)
				{
					m_ValuePerPixel = value;
					ReGenerateImage();
				}
			}
		}

		private void ReGenerateImage()
		{
			Bitmap bm = new Bitmap(this.Width, this.Height);

			Graphics g = Graphics.FromImage(bm);

			g.Clear(Color.Transparent);

			// 물리적 거리를 계산합니다.
			double phisicalLength = (this.ClientSize.Width - 16) * m_ValuePerPixel;

			// 공학수치로 변환합니다.
			Engineer scale = new Engineer(phisicalLength);

			// 가수를 지정된 수로 근사화합니다. (1, 2, 5)
			scale = Engineer.ApproximateMantissa(scale, new double[] { 1, 2, 3, 5 });

			// 변환된 물리적 거리를 가져옵니다.
			phisicalLength = scale.Value;

			double length = phisicalLength / m_ValuePerPixel;

			PointF start = new PointF(
				(float)((this.ClientSize.Width - length) / 2),
				this.Margin.Top + m_LargeTick.Height / 2);
			PointF end = new PointF(
				(float)(length + (this.ClientSize.Width - length) / 2),
				this.Margin.Top + m_LargeTick.Height / 2);

			// 눈금의 위치를 계산합니다.
			m_Ticks = ComputeTicks(start, end, m_TickCount);

			DrawTicks(g, this.ForeColor, m_LargeTick, m_SmallTick, m_TickFrequency, m_Ticks, m_TickStyle);

			DrawScale(g, this.ForeColor, this.Font, scale, this.ClientRectangle);

			this.BackgroundImage = bm;
		}

		public ScaleBar()
		{
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			ReGenerateImage();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		private void DrawScale(
			Graphics g, 
			Color color, 
			Font font, 
			Engineer scale, 
			Rectangle bounds)
		{
			string[] unitTable = { " km", " m", " mm", " um", " nm", " pm", " fm" };
			int unitIndex = 0;
			double value = 0;

			for (int i = 0; i < unitTable.Length; i++)
			{
				int max = (i - 2) * -3;
				int min = (i - 1) * -3;

				if (min <= scale.Exponent && scale.Exponent < max)
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

            bounds.Height -= this.Padding.Bottom;

			using (SolidBrush brush = new SolidBrush(color))
			{
				g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brush, bounds, format);
			}
		}

		/// <summary>
		/// 눈금의 위치를 계산합니다.
		/// </summary>
		/// <param name="p1">시작 위치입니다.</param>
		/// <param name="p2">끝 위치입니다.</param>
		/// <param name="tickCount">눈금의 수입니다.</param>
		/// <returns></returns>
		private PointF[] ComputeTicks(
			PointF p1, 
			PointF p2,
			int tickCount)
		{
			PointF[] ticks = new PointF[tickCount];

			for (int i = 0; i < ticks.Length; i++)
			{
				ticks[i].X = p1.X + ((p2.X - p1.X) * i) / (ticks.Length - 1);
				ticks[i].Y = p1.Y + ((p2.Y - p1.Y) * i) / (ticks.Length - 1);
			}

			return ticks;
		}

		private void DrawTicks(
			Graphics g, 
			Color color, 
			SizeF largeTick, 
			SizeF smallTick,
			int tickFreq,
			PointF[] ticks,
			TickStyle tickStyle)			
		{
			if (ticks == null)
			{
				throw new ArgumentNullException("ticks는 null일 수 없습니다.");
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;

			using (SolidBrush brush = new SolidBrush(color))
			{
				switch (tickStyle)
				{
				case TickStyle.Ellipse:
					for (int i = 0; i < ticks.Length; i++)
					{
						if (i % tickFreq > 0)
						{
							g.FillEllipse(brush,
								ticks[i].X - smallTick.Width / 2F,
								ticks[i].Y - smallTick.Height / 2F,
								smallTick.Width,
								smallTick.Height);
						}
						else
						{
							g.FillEllipse(brush,
								ticks[i].X - largeTick.Width / 2F,
								ticks[i].Y - largeTick.Height / 2F,
								largeTick.Width,
								largeTick.Height);
						}
					}
					break;
				case TickStyle.Rectangle:
					for (int i = 0; i < ticks.Length; i++)
					{
						if (i % tickFreq > 0)
						{
							g.FillRectangle(brush,
								ticks[i].X - smallTick.Width / 2F,
								ticks[i].Y - smallTick.Height / 2F,
								smallTick.Width,
								smallTick.Height);
						}
						else
						{
							g.FillRectangle(brush,
								ticks[i].X - largeTick.Width / 2F,
								ticks[i].Y - largeTick.Height / 2F,
								largeTick.Width,
								largeTick.Height);
						}
					}
					break;
				}
			}

			g.SmoothingMode = SmoothingMode.Default;
		}
	}
}
