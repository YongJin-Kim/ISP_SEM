using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class RangeSlider : Control
	{
		#region Property & Variables
		private RectangleF rectBar;
		private RectangleF rectGribLeft;
		private RectangleF rectGribRight;
		private ControlMode conMode = ControlMode.Non;
		private Point preMousePnt;
		private double deltaAccum = 0;

		#region Value
		protected int _Maximum = 100;
		[DefaultValue(100)]
		public int Maximum
		{
			get { return _Maximum; }
			set
			{
				if (_Maximum != value)
				{
					_Maximum = value;
					if (ValidateValue()) { OnMaximumChanged(); }
				}
			}
		}

		protected int _Minimum = 0;
		[DefaultValue(0)]
		public int Minimum
		{
			get { return _Minimum; }
			set
			{
				if (_Minimum != value)
				{
					_Minimum = value;
					if (ValidateValue()) { OnMinimumChanged(); }
				}
			}
		}

		protected int _RangeMaximum = 100;
		[DefaultValue(100)]
		public int RangeMaximum
		{
			get { return _RangeMaximum; }
			set
			{
				if (_RangeMaximum != value)
				{
					_RangeMaximum = value;
					if (ValidateValue()) { OnRangeMaximumChanged(); }
				}
			}
		}

		protected int _RangeMinimum = 0;
		[DefaultValue(100)]
		public int RangeMinimum
		{
			get { return _RangeMinimum; }
			set
			{
				if (_RangeMinimum != value)
				{
					_RangeMinimum = value;
					if (ValidateValue()) { OnRangeMinimumChanged(); }
				}
			}
		}
		#endregion

		#region Color
		protected Color _ColorBar = Color.Blue;
		[DefaultValue(typeof(Color), "Blue")]
		public Color ColorBar
		{
			get { return _ColorBar; }
			set { _ColorBar = value; }
		}

		protected Color _ColorGrib = Color.Red;
		[DefaultValue(typeof(Color), "Red")]
		public Color ColorGrib
		{
			get { return _ColorGrib; }
			set { _ColorGrib = value; }
		}
		#endregion
		#endregion

		public RangeSlider()
		{
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);

			InitializeComponent();

			CheckObjectRegion();

		}

		#region Event
		public event EventHandler RangeMinimumChanged;
		protected virtual void OnRangeMinimumChanged()
		{
			if (RangeMinimumChanged != null)
			{
				RangeMinimumChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler RangeMaximumChanged;
		protected virtual void OnRangeMaximumChanged()
		{
			if (RangeMaximumChanged != null)
			{
				RangeMaximumChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler MaximumChanged;
		protected virtual void OnMaximumChanged()
		{
			if (MaximumChanged != null)
			{
				MaximumChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler MinimumChanged;
		protected virtual void OnMinimumChanged()
		{
			if (MinimumChanged != null)
			{
				MinimumChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		#region override
		protected override void OnSizeChanged(EventArgs e)
		{
			CheckObjectRegion();

			base.OnSizeChanged(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			ControlPaint.DrawBorder3D(pe.Graphics, this.ClientRectangle);
			Brush br = new SolidBrush(_ColorBar);
			pe.Graphics.FillRectangle(br, rectBar);
			Helper.GuiPainter.DrawAquaPill(pe.Graphics, rectGribLeft, _ColorGrib, Orientation.Horizontal);
			Helper.GuiPainter.DrawAquaPill(pe.Graphics, rectGribRight, _ColorGrib, Orientation.Horizontal);

			base.OnPaint(pe);
		}

		enum ControlMode
		{
			Non,
			LeftGrib,
			RightGrib,
			Range
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Enabled)
			{
				if (conMode == ControlMode.Non)
				{
					if (rectGribLeft.Contains(e.Location) || rectGribRight.Contains(e.Location)) { this.Cursor = Cursors.SizeWE; }
					else if (rectBar.Contains(e.Location)) { this.Cursor = Cursors.VSplit; }
					else { Cursor = Cursors.Default; }
				}
				else
				{
					double deltaX = e.Location.X-preMousePnt.X;
					preMousePnt = e.Location;

					deltaAccum += (_Maximum - _Minimum) * deltaX / (this.ClientSize.Width);

					int val = (int)Math.Truncate(deltaAccum);
					deltaAccum -= val;
					switch (conMode)
					{
					case ControlMode.LeftGrib:
						RangeMinimum += val;
						break;
					case ControlMode.RightGrib:
						RangeMaximum += val;
						break;
					case ControlMode.Range:
						_RangeMaximum += val;
						_RangeMinimum += val;
						//RangeMaximum = _RangeMaximum;
						//RangeMinimum = _RangeMinimum;
						ValidateValue();
						OnRangeMinimumChanged();
						OnRangeMaximumChanged();
						break;
					}
					this.Invalidate();
				}
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Enabled && e.Button == MouseButtons.Left)
			{
				preMousePnt = e.Location;
				if (rectGribLeft.Contains(e.Location)) { conMode = ControlMode.LeftGrib; }
				else if (rectGribRight.Contains(e.Location)) { conMode = ControlMode.RightGrib; }
				else if (rectBar.Contains(e.Location)) { conMode = ControlMode.Range; }
				else { conMode = ControlMode.Non; }

				if (conMode != ControlMode.Non) { Capture = true; }

				deltaAccum = 0;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Capture = false;
			conMode = ControlMode.Non;
			base.OnMouseUp(e);
		}
		#endregion

		private bool ValidateValue()
		{
			bool right = true;

			if (_Maximum < _Minimum)
			{
				_Maximum = _Minimum + 1;
				OnMaximumChanged();
				right = false;
			}

			if (_RangeMaximum > _Maximum)
			{
				_RangeMaximum = _Maximum;
				OnRangeMaximumChanged();
				right = false;
			}

			if (_RangeMinimum < _Minimum)
			{
				_RangeMinimum = _Minimum;
				OnRangeMinimumChanged();
				right = false;
			}

			if (_RangeMaximum < _RangeMinimum)
			{
				_RangeMaximum = _RangeMinimum + 1;
				OnRangeMaximumChanged();
				right = false;
			}

			CheckObjectRegion();
			return right;
		}

		private void CheckObjectRegion()
		{
			int startX = (_RangeMinimum - _Minimum) * this.ClientSize.Width / (_Maximum - _Minimum);
			int endX = (_RangeMaximum - _Minimum) * this.ClientSize.Width / (_Maximum - _Minimum);

			rectBar = new RectangleF(startX, this.ClientRectangle.Top + this.Padding.Top, endX - startX, this.ClientSize.Height - this.Padding.Vertical);
			rectGribLeft = new RectangleF(startX - this.ClientSize.Height / 2, this.ClientRectangle.Top, this.ClientSize.Height, this.ClientSize.Height);
			rectGribRight = new RectangleF(endX - this.ClientSize.Height / 2, this.ClientRectangle.Top, this.ClientSize.Height, this.ClientSize.Height);
			this.Invalidate();
		}
	}
}
