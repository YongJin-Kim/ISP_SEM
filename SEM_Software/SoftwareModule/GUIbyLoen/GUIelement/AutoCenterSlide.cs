using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class AutoCenterSlide : UserControl
	{
		public AutoCenterSlide()
		{
			InitializeComponent();
		}

		private void trackBar1_MouseUp(object sender, MouseEventArgs e)
		{
			trackBar1.Value = 0;
			preScrollValue = 0;
			if (minimumTimer.Enabled)
			{
				minimumTimer.Stop();
				minimumTimer.Dispose();
			}
			if (maximumTimer.Enabled)
			{
				maximumTimer.Stop();
				maximumTimer.Dispose();
			}
		}

		public event EventHandler ValueChanged;
		protected virtual void OnValueChanged()
		{
			panel1.Refresh();
			if (ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}



		#region 속성 및 변수
		protected int _Maximum = 1023;
		public int Maximum
		{
			get { return _Maximum; }
			set
			{
				_Maximum = value;
				UpdateValue(_Value);
			}
		}

		protected int _Minimum = -1024;
		public int Minimum
		{
			get { return _Minimum; }
			set
			{
				_Minimum = value;
				UpdateValue(_Value);
			}
		}

		protected int _Value = 0;
		public int Value
		{
			get { return _Value; }
			set
			{
				UpdateValue(value);
			}
		}

		private int preScrollValue = 0;

		private Timer minimumTimer = new Timer();
		private Timer maximumTimer = new Timer();

		int valueAccel;

		private bool _Rotation = false;
		public bool Rotation
		{
			get { return _Rotation; }
			set
			{
				_Rotation = value;
				UpdateValue(_Value);
			}
		}

		private Color _TrackbarFocusedColor = Color.Green;
		[DefaultValue(typeof(Color), "Green")]
		public Color TrackbarFocusedColor
		{
			get { return _TrackbarFocusedColor; }
			set { _TrackbarFocusedColor = value; }
		}

		private Color _TracebarColor = Color.Silver;
		[DefaultValue(typeof(Color), "Silver")]
		public Color TracebarColor
		{
			get { return _TracebarColor; }
			set
			{
				_TracebarColor = value;
				trackBar1.BackColor = _TracebarColor;
			}
		}

		#endregion

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			if (trackBar1.Value == trackBar1.Minimum) { MinimumTimer(true); }
			else if (trackBar1.Value == trackBar1.Maximum) { MaximumTimer(true); }
			else
			{
				if (minimumTimer.Enabled) { MinimumTimer(false); }
				if (maximumTimer.Enabled) { MaximumTimer(false); }

				int temp = _Value + trackBar1.Value - preScrollValue;

				preScrollValue = trackBar1.Value;

				UpdateValue(temp);
			}
		}

		private void MaximumTimer(bool enable)
		{
			if (enable)
			{
				maximumTimer = new Timer();
				maximumTimer.Interval = 100;
				maximumTimer.Tick += new EventHandler(maximumTimer_Tick);
				valueAccel = 0;
				maximumTimer.Start();
			}
			else
			{
				maximumTimer.Stop();
				maximumTimer.Dispose();
			}
		}

		private void MinimumTimer(bool enable)
		{
			if (enable)
			{
				minimumTimer = new Timer();
				minimumTimer.Interval = 100;
				minimumTimer.Tick += new EventHandler(minimumTimer_Tick);
				valueAccel = 0;
				minimumTimer.Start();
			}
			else
			{
				minimumTimer.Stop();
				minimumTimer.Dispose();
			}
		}

		void maximumTimer_Tick(object sender, EventArgs e)
		{
			int temp;

			valueAccel++;

			if (valueAccel > 50) { temp = _Value + 50; valueAccel = 51; }
			else if (valueAccel > 25) { temp = _Value + 20; }
			else if (valueAccel > 10) { temp = _Value + 10; }
			else { temp = _Value + 5; }

			UpdateValue(temp);
		}

		void minimumTimer_Tick(object sender, EventArgs e)
		{
			int temp;

			valueAccel++;

			if (valueAccel > 50) { temp = _Value - 50; valueAccel = 51; }
			else if (valueAccel > 25) { temp = _Value - 20; }
			else if (valueAccel > 10) { temp = _Value - 10; }
			else { temp = _Value - 5; }

			UpdateValue(temp);
		}

		protected virtual void UpdateValue(int temp)
		{
			if (temp > _Maximum)
			{
				if (_Rotation) { _Value = _Minimum; }
				else { _Value = _Maximum; }
			}
			else if (temp < _Minimum)
			{
				if (_Rotation) { _Value = _Maximum; }
				else { _Value = _Minimum; }
			}
			else { _Value = temp; }
			barRect = new Rectangle((_Value - _Minimum) * panel1.Width / (_Maximum - _Minimum) - 1, 0, 2, panel1.Height);
			OnValueChanged();
		}

		#region Panel1 event
		Rectangle barRect;

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(panel1.BackColor);
			g.FillRectangle(new SolidBrush(ForeColor), barRect);
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseDowned)
			{
				UpdateValue(this.Value - downStartPnt.X + e.X);
				downStartPnt = e.Location;
			}
			//else {
			//    if ( barRegion.IsVisible(e.Location) ) {
			//        Cursor.Current = Cursors.Hand;
			//    }
			//    else {
			//        Cursor.Current = Cursors.Arrow;
			//    }
			//}
		}

		bool mouseDowned = false;
		Point downStartPnt;
		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			downStartPnt = e.Location;
			mouseDowned = true;
		}

		private void panel1_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDowned = false;
		}
		#endregion

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);

			//bbMin.Location = new Point(0, (this.Height - bbMin.Height) / 2);
			//bbMax.Location = new Point(this.Width - bbMax.Width, (this.Height - bbMax.Height) / 2);

			panel1.Size = new Size(this.Width, this.Height * 2 / 5);
			panel1.Location = new Point(0, 0);

			trackBar1.Size = new Size(panel1.Width, this.Height * 3 / 5);
			trackBar1.Location = new Point(panel1.Left, panel1.Bottom);
		}

		private void trackBar1_Enter(object sender, EventArgs e)
		{
			trackBar1.BackColor = _TrackbarFocusedColor;
		}

		private void trackBar1_Leave(object sender, EventArgs e)
		{
			trackBar1.BackColor = _TracebarColor;
		}

	}
}
