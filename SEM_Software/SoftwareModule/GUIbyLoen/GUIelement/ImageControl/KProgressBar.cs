using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace SEC.Nanoeye.Controls
{
	//public partial class KProgressBar : ProgressBar
	[DefaultEvent("ValueChanged")]
	public partial class KProgressBar : Control
	{
		public event EventHandler ValueChanged;

		private int m_Minimum = 0;
		private int m_Maximum = 100;
		private int m_Value = 0;

		private bool delayedChange = false;
		private int delayTime = 200;

		public KProgressBar()
		{
			InitializeComponent();
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


			//base.Maximum = 10000;
		}

		#region Value
		[DefaultValue(0), RefreshProperties(RefreshProperties.Repaint), Bindable(BindableSupport.Yes)]
		//public new int Minimum
		public int Minimum
		{
			get { return m_Minimum; }
			set
			{
				m_Minimum = value;

				//base.Value = ((m_Value - m_Minimum) * 10000) / (m_Maximum - m_Minimum);
			}
		}

		[DefaultValue(100), RefreshProperties(RefreshProperties.Repaint), Bindable( BindableSupport.Yes)]
		//public new int Maximum
		public int Maximum
		{
			get { return m_Maximum; }
			set
			{
				m_Maximum = value;
				//base.Value = ((m_Value - m_Minimum) * 10000) / (m_Maximum - m_Minimum);
			}
		}

		[DefaultValue(0), RefreshProperties(RefreshProperties.Repaint), Bindable(BindableSupport.Yes)]
		//public new int Value
		public int Value
		{
			get { return m_Value; }
			set
			{
				if (m_Value != value) {
					m_Value = value;
					this.Invalidate();
					//base.Value = ((m_Value - m_Minimum) * 10000) / (m_Maximum - m_Minimum);
					OnValueChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// 입력치를 바로 적용할 것인지를 설정.
		/// </summary>
		[DefaultValue(false)]
		public bool DelayedChange
		{
			get { return delayedChange; }
			set { delayedChange = value; }
		}

		/// <summary>
		/// 입력치를 추종해 갈때 각 단계간의 시간 차
		/// </summary>
		[DefaultValue(200)]
		public int DelayTime
		{
			get { return delayTime; }
			set { delayTime = value; }
		}
		#endregion

		#region ForeColor
		private Color forecolorStart;
		public Color ForeColorStart
		{
			get { return forecolorStart; }
			set { forecolorStart = value; }
		}

		private Color forecolorEnd;
		public Color ForeColorEnd
		{
			get { return forecolorEnd; }
			set { forecolorEnd = value; }
		}

		#endregion

		protected override void OnPaint(PaintEventArgs pe)
		//new void OnPaint(PaintEventArgs pe)
		{
			this.SuspendLayout();
			Graphics g = pe.Graphics;

			using (Pen p = new Pen(this.ForeColor, 2))
			{
				g.DrawRectangle(p, this.ClientRectangle);
			}

			int rectWidth;
			if (m_Maximum == m_Minimum)
			{
				rectWidth = 0;
			}
			else
			{
				rectWidth = (this.ClientRectangle.Width - 2) * m_Value / (m_Maximum - m_Minimum);
			}

			Rectangle fillRect = new Rectangle(this.ClientRectangle.X + 1, this.ClientRectangle.Y + 1, rectWidth, this.ClientRectangle.Height - 2);
			Rectangle emptyRect = new Rectangle(rectWidth + 1, this.ClientRectangle.Y + 1, (this.ClientRectangle.Width - 2) - rectWidth, this.ClientRectangle.Height - 2);

			using (Brush b = new SolidBrush(this.BackColor))
			{
				g.FillRectangle(b, emptyRect);
			}

			if (fillRect.Width < 1) { return; }
			if (fillRect.Height < 1) { return; }
			GraphicsPath gp = new GraphicsPath();

			gp.AddRectangle(fillRect);
			PathGradientBrush pgb = null;

			pgb = new PathGradientBrush(gp);

			pgb.SurroundColors = new Color[] { this.forecolorEnd };

			pgb.FocusScales = new PointF(0.1F, 0.9F);
			pgb.CenterPoint = new PointF(0.7f, 0.5f);
			pgb.CenterColor = this.forecolorStart;// ControlPaint.Light(this.BackColor);
			g.FillRectangle(pgb, fillRect);
			pgb.Dispose();
			gp.Dispose();

			//}
			this.ResumeLayout();

			base.OnPaint(pe);
		}

		//protected override void OnPrint(PaintEventArgs e)
		//{
		//    this.OnPaint(e);
		//}


		//protected override void OnPaintBackground(PaintEventArgs pevent)
		//{
		//    base.OnPaintBackground(pevent);
		//    System.Diagnostics.Debug.WriteLine("Progressbar painted Background");
		//}

		//protected override void OnPaint(PaintEventArgs pe)
		//{
		//    // 기본 클래스 OnPaint를 호출하고 있습니다.
		//    //base.OnPaint(pe);
		//    Graphics g = pe.Graphics;
		//    using (Brush b = new SolidBrush(this.BackColor)) {
		//        g.FillRectangle(b, pe.ClipRectangle);
		//        using (Brush bt = new SolidBrush(this.ForeColor)) {
		//            g.FillRectangle(bt, pe.ClipRectangle.X, pe.ClipRectangle.Y, pe.ClipRectangle.Width * this.m_Value / (this.m_Maximum - this.m_Minimum), pe.ClipRectangle.Height);

		//        }
		//    }
		//    System.Diagnostics.Debug.WriteLine("Progressbar painted");
		//    //ControlPaint.draw
		//}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) { ValueChangeReguest(e.X); }

			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) { ValueChangeReguest(e.X); }

			base.OnMouseDown(e);
		}

		Timer changeTimer;

		System.Threading.ManualResetEvent mreChange = new System.Threading.ManualResetEvent(true);

		private void ValueChangeReguest(int pnt)
		{
			mreChange.WaitOne();
			mreChange.Reset();

			int value = ((pnt * (m_Maximum - m_Minimum)) / this.ClientSize.Width) + m_Minimum;

			if (value < m_Minimum) { value = m_Minimum; }
			else if (value > m_Maximum) { value = m_Maximum; }

			// 급격한 변화를 막는 코드
			if (delayedChange) {
				if (changeTimer == null) {
					changeTimer = new Timer();
					changeTimer.Interval = delayTime;
					changeTimer.Tag = value;
					changeTimer.Tick += new EventHandler(changeTimer_Tick);
					changeTimer.Start();
				}
				else {
					changeTimer.Tag = value;
				}
			}
			else {
				this.Value = value;
			}

			mreChange.Set();
		}

		void changeTimer_Tick(object sender, EventArgs e)
		{
			int target = (int)(changeTimer.Tag);

			mreChange.WaitOne();
			mreChange.Reset();
			if (target > this.Value) { this.Value++; }
			else if (target < this.Value) { this.Value--; }

			if (target == this.Value) {
				changeTimer.Stop();
				changeTimer.Dispose();
				changeTimer = null;
			}
			mreChange.Set();
		}

		protected void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null) { ValueChanged(this, e); }
		}
	}
}
