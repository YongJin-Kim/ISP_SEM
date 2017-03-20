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
	[DefaultEvent("ValueChanged")]
	public partial class HscrollWithDisplay : UserControl
	{
		public HscrollWithDisplay()
		{
			InitializeComponent();
		}

		#region 속성
		public enum LabelLocation
		{
			Top,
			Bottom
		}

		private LabelLocation _NameLocation = LabelLocation.Top;
		/// <summary>
		/// Name Lable의 위치
		/// </summary>
		[DefaultValue(typeof(LabelLocation),"Top")]
		public LabelLocation NameLocation
		{
			get { return _NameLocation; }
			set
			{
				if (_NameLocation != value)
				{
					_NameLocation = value;
					//this.Invalidate();
					this.Update();
				}
			}
		}

		private LabelLocation _ValueLocation = LabelLocation.Top;
		/// <summary>
		/// Value Text box의 위치
		/// </summary>
		[DefaultValue(typeof(LabelLocation), "Top")]
		public LabelLocation ValueLocation
		{
			get { return _ValueLocation; }
			set
			{
				if (_ValueLocation != value)
				{
					_ValueLocation = value;
					this.Update();
				}
			}
		}

		/// <summary>
		/// 스크롤 바의 값
		/// </summary>
		[Bindable(true)]
		public virtual int Value
		{
			get { return hScrollBar.Value; }
			set { hScrollBar.Value = value; }
		}

		/// <summary>
		/// 스크롤 바의 최대치
		/// </summary>
		public virtual int Maximum
		{
			get { return hScrollBar.Maximum; }
			set { hScrollBar.Maximum = value; }
		}

		/// <summary>
		/// 스크롤 바의 최소치
		/// </summary>
		public virtual int Minimum
		{
			get { return hScrollBar.Minimum; }
			set { hScrollBar.Minimum = value; }
		}

		/// <summary>
		/// 스크롤 바의 크기
		/// </summary>
		public Size NameSize
		{
			get { return ControlName.Size; }
			set
			{
				ControlName.Size = value;
				this.Update();
			}
		}

		/// <summary>
		/// 값표시 TextBox의 크기
		/// </summary>
		public Size ValueDisplaySize
		{
			get { return ValueDisplay.Size; }
			set
			{
				ValueDisplay.Size = value;
				this.Update();
			}
		}

		[Browsable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Bindable(true)]
		public string DisplayName
		{
			get { return ControlName.Text; }
			set { ControlName.Text = value; }
		}
		#endregion

		#region Override

		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				if (value == Color.Transparent) { base.BackColor = Parent.BackColor; }
				else { base.BackColor = value; }
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			ControlLocationChange();
			base.OnSizeChanged(e);
		}

		private void ControlLocationChange()
		{
			int x = 0;
			int scrollH, conH;
			switch (_NameLocation)
			{
			case LabelLocation.Top:
				switch (_ValueLocation)
				{
				case LabelLocation.Top:
					hScrollBar.Width = this.Width;

					conH = (this.Height / 2) - 4 - ControlName.Height;
					ControlName.Location = new Point(0, conH);

					scrollH = (this.Height / 2) + 2;
					hScrollBar.Location = new Point(0, scrollH);

					conH = (this.Height / 2) - 2 - ValueDisplay.Height;

					x = hScrollBar.Right - ValueDisplay.Width;
					ValueDisplay.Location = new Point(x, conH);
					break;
				case LabelLocation.Bottom:

					hScrollBar.Width = this.Width - ValueDisplay.Width - 3;

					conH = (this.Height / 2) - 4 - ControlName.Height;
					ControlName.Location = new Point(0, conH);

					scrollH = (this.Height / 2) + 2;
					hScrollBar.Location = new Point(0, scrollH);

					x = hScrollBar.Right + 3;
					ValueDisplay.Location = new Point(x, scrollH);
					break;
				default:
					throw new Exception();
				}
				break;
			case LabelLocation.Bottom:
				int y = this.Height / 2;

				hScrollBar.Width = this.Width - ControlName.Width - ValueDisplay.Width - 6;

				ControlName.Location = new Point(0, y - ControlName.Height / 2);

				x = ControlName.Right + 3;
				hScrollBar.Location = new Point(x, y - hScrollBar.Height / 2);

				x = hScrollBar.Right + 3;
				ValueDisplay.Location = new Point(x, y - ValueDisplay.Height / 2);
				break;
			default:
				throw new Exception();

			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			if (this.Visible) { OnSizeChanged(EventArgs.Empty); }
			base.OnVisibleChanged(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Left:
				this.Value--;
				break;
			case Keys.Right:
				this.Value++;
				break;
			}
			base.OnKeyUp(e);
		}

		#endregion

		#region Event
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null) { ValueChanged(this, e); }
		}
		#endregion

		protected virtual void hScrollBar_Scroll(object sender, ScrollEventArgs e)
		{

		}

		protected virtual void hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			ValueDisplayChange();
			this.OnValidating(new CancelEventArgs());
			OnValueChanged(e);
		}

		protected void ValueDisplayChange()
		{
			ValueDisplay.Text = hScrollBar.Value.ToString();
			ValueDisplay.BackColor = Color.White;
		}

		protected virtual void ValueDisplay_Leave(object sender, EventArgs e)
		{
			int tmp;
			try
			{
				tmp = int.Parse(ValueDisplay.Text);
			}
			catch (OverflowException)
			{
				ValueDisplay.BackColor = Color.Red;
				return;
			}
			catch (FormatException)
			{
				ValueDisplay.BackColor = Color.Yellow;
				return;
			}

			if (tmp != this.Value)
			{
				if ((tmp > this.Maximum) || (tmp < this.Minimum))
				{
					ValueDisplay.BackColor = Color.Red;
				}
				else
				{
					ValueDisplay.BackColor = Color.White;
					this.Value = tmp;
				}
			}
		}

		private bool nonNumberEntered = false;

		private void ValueDisplay_KeyPress(object sender, KeyPressEventArgs e)
		{

			// Check for the flag being set in the KeyDown event.
			if (nonNumberEntered == true)
			{
				// Stop the character from being entered into the control since it is non-numerical.
				e.Handled = true;
			}
		}

		private void ValueDisplay_KeyDown(object sender, KeyEventArgs e)
		{
			// Initialize the flag to false.
			nonNumberEntered = false;

			// Determine whether the keystroke is a number from the top of the keyboard.
			if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
			{
				// Determine whether the keystroke is a number from the keypad.
				if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
				{
					// Determine whether the keystroke is a backspace.
					if (e.KeyCode != Keys.Back)
					{
						if ((ValueDisplay.Text.Length != 0) || (e.KeyCode != Keys.Subtract))
						{
							// A non-numerical keystroke was pressed.
							// Set the flag to true and evaluate in KeyPress event.
							nonNumberEntered = true;
						}
					}
				}
			}
		}
	}
}
