using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SEC.Nanoeye.Controls
{
	[DefaultProperty("Value")]
	[DefaultEvent("Turn")]
	public partial class RadioKnob : Control
	{
		#region Private 멤버
		private Rectangle m_ControlBounds;
		private GraphicsPath m_ControlPath;
		private Region m_ControlRegion;

		private KnobStates m_KnobState = KnobStates.Normal;
		private bool m_UpdateKnob;
		private Int32 m_FrameWidth = 5;

		private float m_KnobStartAngle = 135F;
		private float m_KnobSweepAngle = 270F;
		private float m_KnobRotate = 0;
		private float m_KnobSensitivity = 0.5F;

		private Int32 m_GageWidth = 12;
		private float m_GageStartAngle = 135F;
		private float m_GageSweepAngle = 270F;
		private float m_GageRotate = 0;

		private Int32 m_PositionX;
		//private Point m_StartPosition;

		private Timer m_UpdateTimer;

		private int m_Minimum = 0;
		private int m_Maximum = 100;
		private float m_Value = 0;

		/// <summary>
		/// 현재 커서의 표시 상태를 나타냅니다.
		/// </summary>
		private bool m_CursorVisible = true;
		private Rectangle m_CursorClip;

		private int m_WheelDelta;
		private int m_WheelTime;
		private float m_WheelValue = 1;
		private int m_DrawInterval = 50;
		#endregion

		#region Public 멤버
		public event EventHandler ValueChanged;
		public event EventHandler Turn;
		#endregion

		#region Public 속성
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get { return m_Minimum; }
			set
			{
				m_Minimum = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get { return m_Maximum; }
			set
			{
				m_Maximum = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		public int Value
		{
			get { return (int)m_Value; }
			set
			{
				m_Value = (value < m_Minimum) ? m_Minimum : (value > m_Maximum) ? m_Maximum : value;

				this.UpdateKnob();
				this.Invalidate();

				OnValueChanged();
				OnTurn(new EventArgs());
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		public Single KnobStartAngle
		{
			get { return m_KnobStartAngle; }
			set
			{
				m_KnobStartAngle = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		[Description("노브의 총 회전각도입니다.")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public Single KnobSweepAngle
		{
			get { return m_KnobSweepAngle; }
			set
			{
				m_KnobSweepAngle = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		public float KnobSensitivity
		{
			get { return m_KnobSensitivity; }
			set { m_KnobSensitivity = value; }
		}

		[DefaultValue(12)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int GageWidth
		{
			get { return m_GageWidth; }
			set
			{
				m_GageWidth = value;

				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(135F)]
		public Single GageStartAngle
		{
			get { return m_GageStartAngle; }
			set
			{
				m_GageStartAngle = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(270F)]
		public Single GageSweepAngle
		{
			get { return m_GageSweepAngle; }
			set
			{
				m_GageSweepAngle = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		public int FrameWidth
		{
			get { return m_FrameWidth; }
			set
			{
				m_FrameWidth = value;

				this.UpdateKnob();
				this.Invalidate();
			}
		}
		#endregion

		#region Public 메서드
		public RadioKnob()
		{
			InitializeComponent();

			this.DoubleBuffered = true;

			m_WheelDelta = SystemInformation.MouseWheelScrollDelta;

			this.SetClientSizeCore(128, 128);

		}

		public void ResetValues(int vlue, int maximum, int minimum)
		{
			m_Maximum = maximum;
			m_Minimum = minimum;
			Value = vlue;
		}
		#endregion

		#region Protected 메서드
		protected void OnTurn(EventArgs e)
		{
			if ( Turn != null )
			{
				Turn(this, e);
			}
		}

		protected virtual void OnValueChanged()
		{
			if ( ValueChanged != null )
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}

		//System.Threading.ManualResetEvent mrePaint = new System.Threading.ManualResetEvent(true);
		protected override void OnPaint(PaintEventArgs pe)
		{
			//			mrePaint.WaitOne();
			//			mrePaint.Reset();
			//Trace.WriteLine("MRE Setted", this.ToString());
			this.m_ControlPath = GetControlPath(this.ClientRectangle);
			this.m_ControlRegion = new Region(this.m_ControlPath);
			this.Region = m_ControlRegion;

			this.m_ControlBounds = GetControlBounds(this.ClientRectangle);
			
			Draw(
				pe.Graphics,
				this.m_ControlBounds,
				this.m_FrameWidth,
				this.m_KnobState,
				this.m_KnobRotate,
				this.m_GageWidth,
				this.m_GageStartAngle,
				this.m_GageSweepAngle,
				this.m_GageRotate);

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			pe.Graphics.DrawString(Text, Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
			base.OnPaint(pe);
			//			mrePaint.Set();
			//Trace.WriteLine("MRE ReSetted", this.ToString());
		}


		protected override void OnMouseDown(MouseEventArgs e)
		{
			if ( e.Button == MouseButtons.Left )
			{
				this.Focus();

				m_CursorClip = CursorCatch();
				//m_PositionX = m_StartPosition.X;
				//m_PositionX = this.ClientRectangle.Right - this.ClientRectangle.Width / 2;
				//Cursor.Position = this.PointToScreen(new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2));
				//Cursor.Current = Cursors.NoMoveHoriz;
				m_PositionX = this.PointToClient(Cursor.Position).X;
				//Trace.WriteLine("StartPoint : " + m_PositionX.ToString() + "\tThis Right : "+this.Right.ToString(),"OnMouseDown");

				m_UpdateTimer = new Timer();
				m_UpdateTimer.Tick += new EventHandler(m_UpdateTimer_Tick);
				m_UpdateTimer.Interval = m_DrawInterval;
				m_UpdateTimer.Start();

			}

			this.m_KnobState = KnobStates.Pushed;
			this.Invalidate();

			base.OnMouseDown(e);
		}

		double mouseMoveAccum = 0;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if ( e.Button == MouseButtons.Left )
			{
				// 컨트롤 영역바깥쪽에 도달하면 반대편으로 되돌립니다.
				if ( e.X > this.ClientRectangle.Right - 3 )
				{
					Point location = new Point(this.ClientRectangle.Left + 3, this.ClientRectangle.Height / 2);
					Cursor.Position = this.PointToScreen(location);
					m_PositionX = this.ClientRectangle.Left + 3;
				}
				else if ( e.X < this.ClientRectangle.Left + 2 )
				{
					Point location = new Point(this.ClientRectangle.Right - 4, this.ClientRectangle.Height / 2);
					Cursor.Position = this.PointToScreen(location);
					m_PositionX = this.ClientRectangle.Right - 4;
				}
				else
				{
					// 수평 이동량을 가져옵니다.
					int delta = (e.X - m_PositionX);
					mouseMoveAccum += (float)delta * m_KnobSensitivity;

					int iDelta = (int)Math.Truncate(mouseMoveAccum);

					if ( iDelta != 0 )
					{
						mouseMoveAccum -= iDelta;
						m_Value += iDelta;
						if ( m_Value > m_Maximum ) { m_Value = m_Maximum; }
						if ( m_Value < m_Minimum ) { m_Value = m_Minimum; }

						OnTurn(new EventArgs());

						this.UpdateKnob();
					}
					m_PositionX = e.X;
				}
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if ( e.Button == MouseButtons.Left )
			{
				CursorRelease(m_CursorClip);
				if ( m_UpdateTimer != null )
				{
					m_UpdateTimer.Stop();
					m_UpdateTimer.Dispose();
					m_UpdateTimer = null;
				}
			}


			this.m_KnobState = KnobStates.Hover;
			this.Invalidate();

			base.OnMouseUp(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			this.m_KnobState = KnobStates.Hover;
			this.Invalidate();

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//ShowCursor();

			this.m_KnobState = KnobStates.Normal;
			this.Invalidate();

			base.OnMouseLeave(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int time = Environment.TickCount - m_WheelTime;

			if ( time < 50 )
			{
				m_WheelValue = (m_Maximum - m_Minimum) * 0.008F;
				m_WheelValue = (m_WheelValue < 0) ? 1 : m_WheelValue;
			}
			else if ( time < 100 )
			{
				m_WheelValue = (m_Maximum - m_Minimum) * 0.004F;
				m_WheelValue = (m_WheelValue < 0) ? 1 : m_WheelValue;
			}
			else if ( time < 200 )
			{
				m_WheelValue = (m_Maximum - m_Minimum) * 0.002F;
				m_WheelValue = (m_WheelValue < 0) ? 1 : m_WheelValue;
			}
			else
			{
				m_WheelValue = 1;
			}

			m_WheelTime = Environment.TickCount;

			//float temp = (e.Delta / m_WheelDelta) * m_WheelValue;

			//Debug.WriteLine("Scrolled   -   " + temp.ToString() + "   ," + e.Delta.ToString() + "," + m_WheelDelta.ToString() + "," + m_WheelValue.ToString());
			//m_Value += temp;
			m_Value += e.Delta / SystemInformation.MouseWheelScrollDelta;
			//m_Value += e.Delta;

			if ( m_Value > m_Maximum )
				m_Value = m_Maximum;
			if ( m_Value < m_Minimum )
				m_Value = m_Minimum;

			OnTurn(new EventArgs());

			this.UpdateKnob();

			this.Invalidate();

			base.OnMouseWheel(e);

			//Trace.WriteLine(e.Delta);
		}


		#endregion

		#region private 메서드
		private Rectangle CursorCatch()
		{
			Rectangle previousRect = Cursor.Clip;

			Rectangle clipRect = this.ClientRectangle;
			//clipRect.X -= 1;
			clipRect.Y += clipRect.Height / 2;
			//clipRect.Width += 2;
			clipRect.Height = 1;
			clipRect.X += 1;
			clipRect.Width -= 2;

			Cursor.Clip = this.RectangleToScreen(clipRect);
			Cursor.Position = new Point(Cursor.Clip.Left + Cursor.Clip.Width / 2, Cursor.Clip.Top);
			Cursor.Current = Cursors.NoMoveHoriz;

			return previousRect;
		}

		private void CursorRelease(Rectangle clip)
		{
			Cursor.Position = new Point(Cursor.Clip.Left + Cursor.Clip.Width / 2, Cursor.Clip.Top);
			Cursor.Clip = clip;
			Cursor.Current = Cursors.Default;
		}

		private float GetKnobRotate(
			float knobStartAngle,
			float knobSweepAngle,
			float minimum,
			float maximum,
			float value)
		{
			float angle =
				((knobSweepAngle * (value - minimum)) / (maximum - minimum)) % 360F;

			return angle + knobStartAngle;
		}

		private float GetGageRotate(
			float gageSweepAngle,
			float minimum,
			float maximum,
			float value)
		{
			float angle =
				((gageSweepAngle * (value - minimum)) / (maximum - minimum));

			return angle;
		}

		private void UpdateKnob()
		{
			m_UpdateKnob = true;

			m_KnobRotate =
				GetKnobRotate(m_KnobStartAngle, m_KnobSweepAngle, m_Minimum, m_Maximum, m_Value);
			m_GageRotate =
				GetGageRotate(m_GageSweepAngle, m_Minimum, m_Maximum, m_Value);
		}

		/// <summary>
		/// 커서를 나타냅니다.
		/// </summary>
		private void ShowCursor()
		{
			if ( !m_CursorVisible )
			{
				m_CursorVisible = true;

				Cursor.Show();
			}
		}

		/// <summary>
		/// 커서를 숨깁니다.
		/// </summary>
		private void HideCursor()
		{
			if ( m_CursorVisible )
			{
				m_CursorVisible = false;

				Cursor.Hide();
			}
		}

		/// <summary>
		/// 유효한 컨트롤 영역(정사각형)을 구합니다.
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		private Rectangle GetControlBounds(Rectangle bounds)
		{
			Rectangle controlBounds = bounds;

			if ( bounds.Width > bounds.Height )
			{
				controlBounds.X += (bounds.Width - bounds.Height) / 2;
				controlBounds.Width = bounds.Height;
			}
			else
			{
				controlBounds.Y += (bounds.Height - bounds.Width) / 2;
				controlBounds.Height = bounds.Width;
			}

			// 여백을 만들면 부드럽게 그려집니다.
			controlBounds.Inflate(-1, -1);

			return controlBounds;
		}

		private GraphicsPath GetControlPath(Rectangle bounds)
		{
			GraphicsPath gp = new GraphicsPath();

			gp.AddEllipse(bounds);

			return gp;
		}


		private void DrawFrame(Graphics g, Rectangle bounds, Int32 frameWidth)
		{
			LinearGradientBrush lgb;
			Rectangle frameBounds = bounds;
			lgb = new LinearGradientBrush(frameBounds, Color.Empty, Color.Empty, LinearGradientMode.ForwardDiagonal);

			ColorBlend blend = new ColorBlend(3);
			blend.Colors[0] = ControlPaint.Dark(this.BackColor);
			blend.Colors[1] = this.BackColor;
			blend.Colors[2] = Color.White;
			blend.Positions[0] = 0;
			blend.Positions[2] = 1;

			Pen p;
			for ( int i = 0 ; i < frameWidth ; i++ )
			{
				blend.Positions[1] = 0.7F - (0.3F * i) / frameWidth;
				lgb.InterpolationColors = blend;

				p = new Pen(lgb, 2F);
				g.DrawEllipse(p, frameBounds);
				frameBounds.Inflate(-1, -1);
				p.Dispose();
			}

			lgb.Dispose();
		}

		private void DrawControl(Graphics g, Rectangle bounds, KnobStates state)
		{
			PathGradientBrush pgb;
			GraphicsPath gp = new GraphicsPath();

			// 버튼을 채울 브러쉬를 초기화 합니다.
			// gp = new GraphicsPath();
			gp.AddEllipse(bounds);
			pgb = new PathGradientBrush(gp);
			gp.Dispose();


			if ( this.Focused )
			{
				switch ( state )
				{
				case KnobStates.Normal:
					pgb.SurroundColors = new Color[] { ControlPaint.Light(this.BackColor) };
					break;
				case KnobStates.Hover:
					pgb.SurroundColors = new Color[] { ControlPaint.Light(Color.SkyBlue, 0.1F) };
					break;
				case KnobStates.Pushed:
					pgb.SurroundColors = new Color[] { ControlPaint.Light(Color.Orange, 0.1F) };
					break;
				}
			}
			else
			{
				switch ( state )
				{
				case KnobStates.Normal:
					pgb.SurroundColors = new Color[] { ControlPaint.Dark(this.BackColor) };
					break;
				case KnobStates.Hover:
					pgb.SurroundColors = new Color[] { ControlPaint.Dark(Color.SkyBlue, 0.1F) };
					break;
				case KnobStates.Pushed:
					pgb.SurroundColors = new Color[] { ControlPaint.Dark(Color.Orange, 0.1F) };
					break;
				}
			}

			pgb.FocusScales = new PointF(0.1F, 0.1F);
			pgb.CenterPoint = new PointF(bounds.Left + bounds.Width / 4, bounds.Top + bounds.Height / 4);
			pgb.CenterColor = Color.White;// ControlPaint.Light(this.BackColor);
			g.FillEllipse(pgb, bounds);
			pgb.Dispose();
			g.DrawEllipse(Pens.Black, bounds);
		}

		private void DrawMark(Graphics g, RectangleF bounds)
		{
			LinearGradientBrush lgb;

			lgb = new LinearGradientBrush(
				bounds,
				ControlPaint.Dark(this.BackColor),
				Color.White,
				LinearGradientMode.ForwardDiagonal);
			g.FillEllipse(lgb, bounds);

			lgb.Dispose();
		}

		/// <summary>
		/// 회전 마크 영역을 계산합니다.
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		RectangleF GetMarkBounds(Rectangle bounds, float angle)
		{
			// 마크 표시 크기를 계산합니다. (1 / 4)
			Size markSize = new Size(bounds.Width / 4, bounds.Height / 4);

			// 각도 0인 기준점을 지정하고 angle만큼 시계방향으로 회전합니다. 
			PointF pt = new PointF(bounds.Right - bounds.Width / 4F, bounds.Bottom - bounds.Height / 2F);
			pt = GetRotatePoint(bounds, pt, angle);
			//pt.Offset(-markSize.Width / 2F, -markSize.Height / 2F);
			pt.X -= markSize.Width / 2F;
			pt.Y -= markSize.Height / 2F;

			return new RectangleF(pt, markSize);
		}

		private void DrawGage(
			Graphics g,
			Rectangle bounds,
			int width,
			float gageStart,
			float gageSweep,
			float gageRotate)
		{
			Pen pBack = new Pen(SystemColors.ControlDarkDark, width);
			Pen pSweep= new Pen(Color.White, width - 4);
			Pen pRotate;

			if ( this.Focused )
			{
				pRotate = new Pen(ControlPaint.Light(this.ForeColor), width - 6);
				//ControlPaint.DrawFocusRectangle(g, bounds);
			}
			//else {
			pRotate = new Pen(this.ForeColor, width - 6);
			//}

			bounds.Inflate(-width / 2, -width / 2);
			//p = new Pen(ControlPaint.Dark(this.BackColor), width);
			g.DrawEllipse(pBack, bounds);

			pSweep.StartCap = LineCap.Round;
			pSweep.EndCap = LineCap.Round;
			g.DrawArc(pSweep, bounds, gageStart, gageSweep);

			//g.Flush(FlushIntention.Flush);
			gageRotate = (float)Math.Round((double)gageRotate, 0);
			//p.DashStyle = DashStyle.Dot;
			//p.DashPattern = new float[] { 1F, 0.5F };
			g.DrawArc(pRotate, bounds, gageStart, gageRotate);

			pBack.Dispose();
			pSweep.Dispose();
			pRotate.Dispose();
		}

		private void Draw(
			Graphics g,
			Rectangle bounds,
			Int32 frameWidth,
			KnobStates state,
			float knobRotate,
			Int32 gageWidth,
			float gageStart,
			float gageSweep,
			float gageRotate)
		{
			// 그래픽스의 품질을 지정합니다.
			g.SmoothingMode = SmoothingMode.AntiAlias;

			// 프레임을 그립니다.
			DrawFrame(g, bounds, frameWidth);

			// 값 표시를 그립니다.
			bounds.Inflate(-frameWidth, -frameWidth);
			DrawGage(g, bounds, gageWidth, gageStart, gageSweep, gageRotate);

			// 컨트롤을 그립니다.
			bounds.Inflate(-gageWidth, -gageWidth);
			DrawControl(g, bounds, state);

			// 회전 표시를 그립니다.
			RectangleF markBounds = GetMarkBounds(bounds, knobRotate);
			DrawMark(g, markBounds);
		}

		private static PointF GetRotatePoint(Rectangle bounds, PointF location, float degree)
		{
			PointF basis =
				new PointF(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height / 2F);

			float radian = (float)(degree * Math.PI / 180);
			float sin = (float)Math.Sin(radian);
			float cos = (float)Math.Cos(radian);

			PointF p = new PointF(
				cos * (location.X - basis.X) + sin * (location.Y - basis.Y) + basis.X,
				sin * (location.X - basis.X) - cos * (location.Y - basis.Y) + basis.Y);

			return p;
		}

		private void m_UpdateTimer_Tick(object sender, EventArgs e)
		{
			if ( m_UpdateKnob )
			{
				m_UpdateKnob = false;

				this.Invalidate();
			}


			//Point leftCmp = this.PointToScreen(new Point( Cursor.Clip.Left + 1, 0));
			//Point rightCmp = this.PointToScreen(new Point(Cursor.Clip.Right -2, 0));
			//if ( Cursor.Position.X > rightCmp.X ) {
			//    m_PositionX = this.ClientRectangle.Left + 2;// +2;
			//    Point location = new Point(m_PositionX, this.ClientRectangle.Height / 2);
			//    Cursor.Position = this.PointToScreen(location);

			//}
			//else if ( Cursor.Position.X <  leftCmp.X) {
			//    m_PositionX = this.ClientRectangle.Right - 3;// -2;
			//    Point location = new Point(m_PositionX, this.ClientRectangle.Height / 2);
			//    Cursor.Position = this.PointToScreen(location);
			//}

		}
		#endregion

		#region Focus 값이 바뀌면 다시 그림
		protected override void OnGotFocus(EventArgs e)
		{
			this.Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			this.Invalidate();
			base.OnLostFocus(e);
		}
		#endregion
	}

	public enum KnobStates
	{
		Normal,
		Hover,
		Pushed
	}
}
