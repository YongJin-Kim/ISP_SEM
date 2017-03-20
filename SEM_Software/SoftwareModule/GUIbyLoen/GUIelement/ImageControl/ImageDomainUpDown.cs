using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SEC.GUIelement;

namespace SEC.Nanoeye.Controls
{
	public partial class FocusTypeSelector : Control
	{
		public event EventHandler IndexChanged;

		protected virtual void OnIndexChanged()
		{
			if ( IndexChanged != null ) {
				IndexChanged(this, EventArgs.Empty);
			}
		}

		private string[] focusStr = new string[] { "Coarse - High", "Coarse - Medium", "Coarse - Low", "Fine - High", "Fine - Medimum", "Fine - Low" };

		private int index = 0;
		[DefaultValue(0)]
		public int SelectedIndex
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
				TextLab.Text = focusStr[index];
				if ( index == 0 ) {
					LeftBE.Enabled = false;
				}
				else {
					LeftBE.Enabled = true;
				}
				if ( index == 5 ) {
					RightBE.Enabled = false;
				}
				else {
					RightBE.Enabled = true;
				}

				OnIndexChanged();
			}
		}

		private Color _TextLabelBackColor;
		public Color TextLabelBackColor
		{
			get { return TextLab.BackColor; }
			set
			{
				TextLab.BackColor = value;
				_TextLabelBackColor = value;
			}
		}

		public Color TextLabelForecolor
		{
			get { return TextLab.ForeColor; }
			set { TextLab.ForeColor = value; }
		}

		private Color _ActivationColor = Color.Blue;
		[DefaultValue(typeof(Color),"Blue")]
		public Color ActivationColor
		{
			get { return _ActivationColor; }
			set { _ActivationColor = value; }
		}

		[DefaultValue(typeof(Color),"Black")]
		public Color ButtonColor
		{
			get { return RightBE.BackColor; }
			set
			{
				RightBE.BackColor = value;
				LeftBE.BackColor = value;
			}

		}

		public override string Text
		{
			get
			{
				return TextLab.Text;
			}
		}

		System.Threading.Timer ActiveTimer;

		bool _Activation = false;
		public bool Activation
		{

			get { return _Activation; }
			set
			{
				actCnt = 0;
				_Activation = value;
				if ( _Activation )
				{
					ActiveTimer.Change(0, 150);
				}
				else
				{
					ActiveTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
					TextLabelBackColor = _TextLabelBackColor;
				}
			}
		}

		public FocusTypeSelector()
		{
			InitializeComponent();

			this.Controls.Add(LeftBE);
			this.Controls.Add(RightBE);
			this.Controls.Add(TextLab);

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);

			SelectedIndex = SelectedIndex;
			_TextLabelBackColor = TextLab.BackColor;

			ActiveTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ActiveTimerCallback));

		}

		int actCnt = 0;

		void ActiveTimerCallback(Object obj)
		{
			int conv = 0;
			if ( actCnt < 10 ) // Active Color로 바꾸는 중.
			{
				conv = actCnt++;
			}
			else // TextLabelBackColor로 바꾸는 중.
			{
				conv = 20 - actCnt;
				actCnt++;
				if ( actCnt == 20 )
				{
					actCnt = 0;
				}

			}

			Color col = Color.FromArgb(255,
				(_ActivationColor.R - _TextLabelBackColor.R) * conv / 10 + _TextLabelBackColor.R,
				(_ActivationColor.G - _TextLabelBackColor.G) * conv / 10 + _TextLabelBackColor.G,
				(_ActivationColor.B - _TextLabelBackColor.B) * conv / 10 + _TextLabelBackColor.B
				);
			TextLab.BackColor = col;
			TextLab.Invalidate();
			//System.Diagnostics.Debug.WriteLine(_ActivationColor.ToString() + _TextLabelBackColor.ToString() + col.ToString());

		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			int sz = (int)(this.Height * 0.8);

			LeftBE.Size = new Size(sz, sz);
			RightBE.Size = new Size(sz, sz);

			LeftBE.Location = new Point(0, (this.Height - sz) / 2);
			RightBE.Location = new Point(this.Width - sz, (this.Height - sz) / 2);

			TextLab.Location = new Point(this.Height, 0);
			TextLab.Size = new Size(this.Width - this.Height * 2, this.Height);
		}

		private void RightBE_Click(object sender, EventArgs e)
		{

		}

		private void LeftBE_Click(object sender, EventArgs e)
		{

		}

		private void RightBE_MouseClick(object sender, MouseEventArgs e)
		{
			SelectedIndex++;
		}

		private void LeftBE_MouseClick(object sender, MouseEventArgs e)
		{
			SelectedIndex--;
		}

		private void LeftBE_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			SelectedIndex--;
		}

		private void RightBE_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			SelectedIndex++;
		}

	}
}
