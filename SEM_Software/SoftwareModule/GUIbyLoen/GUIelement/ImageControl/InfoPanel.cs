using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace SEC.Nanoeye.Controls
{
	public partial class InfoPanel : Panel
	{
		public InfoPanel()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

			InitializeComponent();
		}

		#region Property & Variables
		#region Display
		private int _opacity=255;
		public int Opacity
		{
			get
			{
				return _opacity;
			}
		}

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				_opacity = value.A;
				ReGenerateImage();
			}
		}

		private Color _EdgeColor;
		public Color EdgeColor
		{
			get { return _EdgeColor; }
			set
			{
				_EdgeColor = value;
				ReGenerateImage();
			}
		}

		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				ReGenerateImage();
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				ReGenerateImage();
			}
		}
		#endregion

		#region Scale
		private SizeF m_SmallTick = new SizeF(8, 8);
		private SizeF m_LargeTick = new SizeF(8, 16);
		private PointF[] m_Ticks;
		//private double m_Length = 0;
		private int m_TickFrequency = 10;
		private int m_TickCount = 11;
		private double m_ValuePerPixel = 1.0;
		private SEC.Nanoeye.Controls.ScaleBar.TickStyle m_TickStyle = SEC.Nanoeye.Controls.ScaleBar.TickStyle.Ellipse;

		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public SEC.Nanoeye.Controls.ScaleBar.TickStyle TickStyle
		{
			get { return m_TickStyle; }
			set
			{
				if ( m_TickStyle != value )
				{
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
				if ( m_SmallTick != value )
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
				if ( m_LargeTick != value )
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
				if ( m_TickCount != value )
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
				if ( value <= 0 )
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
				if ( m_ValuePerPixel != value )
				{
					m_ValuePerPixel = value;
					ReGenerateImage();
				}
			}
		}
		#endregion

		private string _Magnification="x0";
		public string Magnification
		{
			get { return _Magnification; }
			set
			{
				_Magnification = value;
				ReGenerateImage();
			}
		}

		private string _Eghv = "?Kv";
		public string Eghv
		{
			get { return _Eghv; }
			set
			{
				_Eghv = value;
				ReGenerateImage();
			}
		}

		private string _Description = "";
		public string Desciption
		{
			get { return _Description; }
			set
			{
				_Description = value;
				ReGenerateImage();
			}
		}

		public enum DetectorMode
		{
			SED,
			BSED
		}

		private string _Detector = "SED";
		public DetectorMode Detector
		{
			get
			{
				switch ( _Detector )
				{
				case "BSED":
					return DetectorMode.BSED;
				case "SED":
					return DetectorMode.SED;
				}
				throw new Exception();
			}
			set
			{
				switch ( value )
				{
				case DetectorMode.BSED:
					_Detector = "BSED";
					break;
				case DetectorMode.SED:
					_Detector = "SED";
					break;
				}
				ReGenerateImage();
			}
		}

		public enum VacuumMode
		{
			HighVacuum,
			LowVacuum
		}

		private string _Vacuuum = "High vac.";
		public VacuumMode Vacuum
		{
			get
			{
				switch ( _Vacuuum )
				{
				case "High vac.":
					return VacuumMode.HighVacuum;
				case "Low vac.":
					return VacuumMode.LowVacuum;
				}
				throw new Exception();
			}
			set
			{
				switch ( value )
				{
				case VacuumMode.HighVacuum:
					_Vacuuum = "High vac.";
					break;
				case VacuumMode.LowVacuum:
					_Vacuuum = "Low vac.";
					break;
				}
				ReGenerateImage();
			}
		}
		#endregion

		private Rectangle descRect = new Rectangle(0, 0, 172, 32);
		private Rectangle eghvRect = new Rectangle(172, 0, 64, 32);
		private Rectangle magRect = new Rectangle(236, 0, 64, 32);
		private Rectangle scaleRect = new Rectangle(300, 0, 240, 32);
		private Rectangle detectorRect = new Rectangle(540, 0, 100, 16);
		private Rectangle vacuumRect = new Rectangle(540, 16, 100, 16);

		void ReGenerateImage()
		{
			Bitmap bm = new Bitmap(this.Width, this.Height);

			Graphics g = Graphics.FromImage(bm);

			g.Clear(Color.FromArgb(64, Color.Black));

			DrawString(g, _Description, descRect);
			DrawString(g, _Eghv, eghvRect);
			DrawString(g, _Magnification, magRect);
			DrawScale(g);
			DrawString(g, _Vacuuum, vacuumRect);
			DrawString(g, _Detector, detectorRect);


			this.BackgroundImage = bm;
			this.Invalidate();
		}

		private void DrawString(Graphics g, string str, Rectangle rect)
		{
			StringFormat style = new StringFormat();
			style.Alignment = StringAlignment.Center;
			style.LineAlignment = StringAlignment.Center;

			Brush bkBr = new SolidBrush(_EdgeColor);
			Brush txBr = new SolidBrush(ForeColor);

			rect.Offset(1, 1);
			g.DrawString(str, this.Font, bkBr, rect, style);
			rect.Offset(-2, 0);
			g.DrawString(str, this.Font, bkBr, rect, style);
			rect.Offset(0, -2);
			g.DrawString(str, this.Font, bkBr, rect, style);
			rect.Offset(2, 0);
			g.DrawString(str, this.Font, bkBr, rect, style);
			rect.Offset(-1, 1);
			g.DrawString(str, this.Font, txBr, rect, style);
		}

		private void DrawScale(Graphics g)
		{
			// 그리기가 물리적 거리를 계산합니다.
			double phisicalLength = (scaleRect.Width - 16) * m_ValuePerPixel;

			// 공학수치로 변환합니다.
			SEC.Nanoeye.Controls.ScaleBar.Engineer scale = new SEC.Nanoeye.Controls.ScaleBar.Engineer(phisicalLength);

			// 가수를 지정된 수로 근사화합니다. (1, 2, 5)
			scale = SEC.Nanoeye.Controls.ScaleBar.Engineer.ApproximateMantissa(scale, new double[] { 1, 2, 3, 5 });

			// 변환된 물리적 거리를 가져옵니다.
			phisicalLength = scale.Value;

			double length = phisicalLength / m_ValuePerPixel;

			PointF start = new PointF(
				(float)((scaleRect.Width - length) / 2) + scaleRect.X,
				this.Margin.Top + m_LargeTick.Height / 2);
			PointF end = new PointF(
				(float)(length + (scaleRect.Width - length) / 2) + scaleRect.X,
				this.Margin.Top + m_LargeTick.Height / 2);

			// 눈금의 위치를 계산합니다.
			m_Ticks = ComputeTicks(start, end, m_TickCount);

			DrawTicks(g, this.ForeColor, m_LargeTick, m_SmallTick, m_TickFrequency, m_Ticks, m_TickStyle);

			DrawScale(g, this.ForeColor, this.Font, scale, scaleRect);
		}

		#region Scale Method
		private void DrawScale(
								Graphics g,
								Color color,
								Font font,
								SEC.Nanoeye.Controls.ScaleBar.Engineer scale,
								Rectangle bounds)
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

			bounds.Height -= this.Padding.Bottom;

			SolidBrush brush = new SolidBrush(color);
			Brush brushBack = new SolidBrush(_EdgeColor);


			bounds.Offset(1, 1);
			g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brushBack, bounds, format);
			bounds.Offset(-2, 0);
			g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brushBack, bounds, format);
			bounds.Offset(0, -2);
			g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brushBack, bounds, format);
			bounds.Offset(2, 0);
			g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brushBack, bounds, format);
			bounds.Offset(-1, 1);
			g.DrawString(value.ToString("0") + unitTable[unitIndex], font, brush, bounds, format);

			brush.Dispose();
			brushBack.Dispose();
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

			for ( int i = 0 ; i < ticks.Length ; i++ )
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
			SEC.Nanoeye.Controls.ScaleBar.TickStyle tickStyle)
		{
			if ( ticks == null )
			{
				throw new ArgumentNullException("ticks는 null일 수 없습니다.");
			}

			//g.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle rect;

			Brush backBr = new SolidBrush(_EdgeColor);
			using ( SolidBrush brush = new SolidBrush(color) )
			{
				switch ( tickStyle )
				{
				case SEC.Nanoeye.Controls.ScaleBar.TickStyle.Ellipse:
					for ( int i = 0 ; i < ticks.Length ; i++ )
					{
						if ( i % tickFreq > 0 )
						{
							rect = new Rectangle((int)(ticks[i].X - smallTick.Width / 2F),
								(int)(ticks[i].Y - smallTick.Height / 2F),
								(int)(smallTick.Width),
								(int)(smallTick.Height));
							DrawEllipse(g, backBr, brush, rect);
						}
						else
						{
							rect = new Rectangle(
								(int)(ticks[i].X - largeTick.Width / 2F),
								(int)(ticks[i].Y - largeTick.Height / 2F),
								(int)(largeTick.Width),
								(int)(largeTick.Height));
							DrawEllipse(g, backBr, brush, rect);
						}
					}
					break;
				case SEC.Nanoeye.Controls.ScaleBar.TickStyle.Rectangle:
					for ( int i = 0 ; i < ticks.Length ; i++ )
					{
						if ( i % tickFreq > 0 )
						{
							rect = new Rectangle((int)(ticks[i].X - smallTick.Width / 2F),
								(int)(ticks[i].Y - smallTick.Height / 2F),
								(int)(smallTick.Width),
								(int)(smallTick.Height));
							DrawRectangle(g, backBr, brush, rect);
						}
						else
						{
							rect = new Rectangle(
								(int)(ticks[i].X - largeTick.Width / 2F),
								(int)(ticks[i].Y - largeTick.Height / 2F),
								(int)(largeTick.Width),
								(int)(largeTick.Height));
							DrawRectangle(g, backBr, brush, rect);
						}
					}
					break;
				}
			}
			backBr.Dispose();
			//g.SmoothingMode = SmoothingMode.Default;
		}

		private void DrawEllipse(Graphics g, Brush backBr, Brush oriBr, Rectangle rect)
		{
			rect.Offset(1, 1);
			g.FillEllipse(backBr, rect);
			rect.Offset(-2, 0);
			g.FillEllipse(backBr, rect);
			rect.Offset(0, -2);
			g.FillEllipse(backBr, rect);
			rect.Offset(2, 0);
			g.FillEllipse(backBr, rect);
			rect.Offset(-1, 1);
			g.FillEllipse(oriBr, rect);
		}

		private void DrawRectangle(Graphics g, Brush backBr, Brush oriBr, Rectangle rect)
		{
			rect.Offset(1, 1);
			g.FillRectangle(backBr, rect);
			rect.Offset(-2, 0);
			g.FillRectangle(backBr, rect);
			rect.Offset(0, -2);
			g.FillRectangle(backBr, rect);
			rect.Offset(2, 0);
			g.FillRectangle(backBr, rect);
			rect.Offset(-1, 1);
			g.FillRectangle(oriBr, rect);
		}
		#endregion
	}
}
