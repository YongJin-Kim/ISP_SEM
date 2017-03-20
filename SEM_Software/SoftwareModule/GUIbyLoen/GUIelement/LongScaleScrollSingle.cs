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
	public partial class LongScaleScrollSingle : UserControl
	{
		public LongScaleScrollSingle()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			//SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color ButtonColor
		{
			get { return LeftBe.BackColor; }
			set
			{
				LeftBe.BackColor = value;
				RightBe.BackColor = value;
			}
		}

		private Brush revBru = Brushes.White;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color PanelColor
		{
			get { return ValuePanel.BackColor; }
			set
			{
				ValuePanel.BackColor = value;
				revBru = new SolidBrush(Color.FromArgb(255,
										255 - value.R,
										255 - value.G,
										255 - value.B));
			}
		}

		protected int _Maximum = 100;
		[DefaultValue(100)]
		public int Maximum
		{
			get { return _Maximum; }
			set
			{
				_Maximum = value;
				if (_Value > _Maximum )
				{
					_Value = value;
				}
				ValuePanel.Invalidate();
			}
		}

		protected int _Minimum = 0;
		[DefaultValue(0)]
		public int Minimum
		{
			get { return _Minimum; }
			set
			{
				_Minimum = value;
				if ( _Value < _Minimum )
				{
					_Value = _Minimum;
				}
				ValuePanel.Invalidate();
			}
		}

		Action<Control,bool> EnableChangeAction=(x, y) => { x.Enabled = y; };

		protected int _Value = 0;
		[DefaultValue(0)]
		public int Value
		{
			get { return _Value; }
			set
			{
				if ( value >= _Maximum )
				{
					_Value = _Maximum;

					if ( RightBe.InvokeRequired )
					{
						RightBe.BeginInvoke(EnableChangeAction, new object[] { RightBe, false });
						LeftBe.BeginInvoke(EnableChangeAction, new object[] { LeftBe, true });
					}
					else
					{
						RightBe.Enabled = false;
						LeftBe.Enabled = true;
					}
				}
				else if ( value <= _Minimum )
				{
					_Value = _Minimum;

					if ( RightBe.InvokeRequired )
					{
						RightBe.BeginInvoke(EnableChangeAction, new object[] { RightBe, true });
						LeftBe.BeginInvoke(EnableChangeAction, new object[] { LeftBe, false });
					}
					else
					{
						RightBe.Enabled = true;
						LeftBe.Enabled = false;
					}
				}
				else
				{
					_Value = value;

					if ( RightBe.InvokeRequired )
					{
						RightBe.BeginInvoke(EnableChangeAction, new object[] { RightBe, true });
						LeftBe.BeginInvoke(EnableChangeAction, new object[] { LeftBe, true });
					}
					else
					{
						RightBe.Enabled = true;
						LeftBe.Enabled = true;
					}
				}
				ValuePanel.Invalidate();
				OnValueChanged(EventArgs.Empty);
			}
		}

		public event EventHandler ValueChanged;
		protected virtual void OnValueChanged(EventArgs e)
		{
			if ( ValueChanged != null )
			{
				ValueChanged(this, e);
			}
		}

		protected double _DividInner = 5d;
		[DefaultValue(5), Bindable(true)]

		public Decimal DividInner
		{
			get { return (decimal)_DividInner; }
			set { _DividInner = (double)value; }
		}

		protected double _DividOutter = 0.5d;
		[DefaultValue(0.5), Bindable(true)]
		public Decimal DividOutter
		{
			get { return (decimal)_DividOutter; }
			set { _DividOutter = (double)value; }
		}

		public enum ValueDisplayModeEnum
		{
			Non,
			Number,
			PercentUnsigned,
			PercentSigned
		}

		private ValueDisplayModeEnum _ValueDisplayMode = ValueDisplayModeEnum.Non;
		[DefaultValue(typeof(ValueDisplayModeEnum), "Non")]
		public ValueDisplayModeEnum ValueDisplayMode
		{
			get { return _ValueDisplayMode; }
			set
			{
				_ValueDisplayMode = value;
				ValuePanel.Invalidate();
			}
		}

		//private Size _SizeOri = new Size(100, 100);
		//public Size SizeOri
		//{
		//    get { return _SizeOri; }
		//    set
		//    {
		//        _SizeOri = value;
		//        this.Size = _SizeOri;
		//    }
		//}

		protected override void OnSizeChanged(EventArgs e)
		{
			//this.SetClientSizeCore(_SizeOri.Width, this._SizeOri.Height);
			base.OnSizeChanged(e);
		}

		protected override void OnLayout(LayoutEventArgs e)
		{


			LeftBe.Location = new Point(this.ClientRectangle.X, this.ClientRectangle.Y);
			LeftBe.Size = new Size(this.ClientSize.Height, this.ClientSize.Height);
			RightBe.Location = new Point(LeftBe.Right, this.ClientRectangle.Y);
			RightBe.Size = new Size(this.ClientSize.Height, this.ClientSize.Height);
			ValuePanel.Location = new Point(RightBe.Right + this.Padding.Horizontal, this.ClientRectangle.Y);
			ValuePanel.Size = new Size(this.ClientSize.Width - RightBe.Right - this.Padding.Horizontal, this.ClientSize.Height);

			base.OnLayout(e);
		}

		private void ValuePanel_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			int start = (_Value - _Minimum) * ValuePanel.ClientSize.Width / (_Maximum - _Minimum) - 1;

			g.FillRectangle(revBru, start, ValuePanel.ClientRectangle.Y, 3, ValuePanel.ClientSize.Height);

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			switch ( _ValueDisplayMode )
			{
			case ValueDisplayModeEnum.Non:
				break;
			case ValueDisplayModeEnum.Number:
				g.DrawString(_Value.ToString(), Font, revBru, ValuePanel.ClientRectangle, sf);
				break;
			case ValueDisplayModeEnum.PercentSigned:
				g.DrawString(((_Value - _Minimum) * 100 / (_Maximum - _Minimum)).ToString() + "%", Font, revBru, ValuePanel.ClientRectangle, sf);
				break;
			case ValueDisplayModeEnum.PercentUnsigned:
				g.DrawString((_Value * 100 / _Maximum).ToString() + "%", Font, revBru, ValuePanel.ClientRectangle, sf);
				break;
			}
		}

		Rectangle preClipRect;
		int premouseposX;

		bool Captuared = false;

		private void ValuePanel_MouseDown(object sender, MouseEventArgs e)
		{
			if ( Captuared ) { return; }

			Captuared = true;

			preClipRect = Cursor.Clip;
			Rectangle clipRect = ValuePanel.ClientRectangle;
			clipRect.Inflate(-2, -2);
			Cursor.Clip = ValuePanel.RectangleToScreen(clipRect);
			Cursor.Current = Cursors.NoMoveHoriz;
			premouseposX = e.X;

			moveAccum = 0;

			ValuePanel.MouseMove += new MouseEventHandler(ValuePanel_MouseMove);
		}

		private void ValuePanel_MouseUp(object sender, MouseEventArgs e)
		{
			Captuared = false;

			Cursor.Clip = preClipRect;
			Cursor.Current = Cursors.Default;

			ValuePanel.MouseMove -= new MouseEventHandler(ValuePanel_MouseMove);
		}

		double moveAccum = 0;

		void ValuePanel_MouseMove(object sender, MouseEventArgs e)
		{
			if ( e.X < 5 )
			{
				Cursor.Position = ValuePanel.PointToScreen(new Point(ValuePanel.ClientSize.Width - 8, e.Y));
			}
			else if ( e.X > ValuePanel.ClientSize.Width - 5 )
			{
				Cursor.Position = ValuePanel.PointToScreen(new Point(8, e.Y));
			}
			else
			{

				int add = 0;

				switch ( e.Button )
				{
				case MouseButtons.Left:
					moveAccum += (premouseposX - e.X) / _DividInner;
					add = (int)Math.Truncate(moveAccum);
					moveAccum -= add;
					this.Value -= add;
					break;
				case MouseButtons.Right:
					moveAccum += (premouseposX - e.X) / _DividOutter;
					add = (int)Math.Truncate(moveAccum);
					moveAccum -= add;
					this.Value -= add;
					break;
				}

			}

			premouseposX = ValuePanel.PointToClient(Cursor.Position).X;
		}

		private void RightBe_Click(object sender, EventArgs e)
		{
			Value++;
		}

		private void LeftBe_Click(object sender, EventArgs e)
		{
			Value--;
		}
	}
}
