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
	public partial class Emphasis : Control
	{
		public enum EmphasisModeType
		{
			Control,
			Area
		}

		#region Property & Variables
		int colorChangeValue = 0;

		private EmphasisModeType _EmphasisMode = EmphasisModeType.Area;
		[DefaultValue(typeof(EmphasisModeType), "Area")]
		public EmphasisModeType EmphasisMode
		{
			get { return _EmphasisMode; }
			set
			{
				if (_EmphasisMode != value)
				{
					_EmphasisMode = value;
					ChangeLocationAndSize();
				}
			}
		}

		private Control _Control = null;
		[DefaultValue(null)]
		public Control Control
		{
			get { return _Control; }
			set
			{
				if (_Control != value)
				{
					_Control = value;
					ChangeLocationAndSize();
				}
			}
		}

		private Rectangle _Area = Rectangle.Empty;
		public Rectangle Area
		{
			get { return _Area; }
			set
			{
				if (_Area != value)
				{
					_Area = value;
					ChangeLocationAndSize();
				}
			}
		}

		private int _BoardWidth = 3;
		[DefaultValue(3)]
		public int BoardWidth
		{
			get { return _BoardWidth; }
			set { _BoardWidth = value; }
		}

		[DefaultValue(150)]
		public int ColorChangeInterval
		{
			get { return colorChangeTimer.Interval; }
			set { colorChangeTimer.Interval = value; }
		}

		private Color _ColorStart = Color.White;
		[DefaultValue(typeof(Color), "White")]
		public Color ColorStart
		{
			get { return _ColorStart; }
			set { _ColorStart = value; }
		}

		private Color _ColorEnd = Color.Red;
		[DefaultValue(typeof(Color), "Red")]
		public Color ColorEnd
		{
			get { return _ColorEnd; }
			set { _ColorEnd = value; }
		}
		#endregion

		public Emphasis()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			InitializeComponent();

			this.Region = new System.Drawing.Region();

			colorChangeTimer.Start();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			this.Region.MakeEmpty();

			base.OnPaint(pe);

			Region reg = new Region(this.ClientRectangle);
			Rectangle rect = this.ClientRectangle;
			rect.Inflate(-_BoardWidth, -_BoardWidth);
			reg.Xor(rect);

			this.Region = reg;

			//pe.Graphics.Clear(Color.Transparent);

			float angle = (float)(Math.Cos((colorChangeValue) * Math.PI / 50)) * 180+90.1f;
			//System.Diagnostics.Trace.WriteLine(angle.ToString() + " vs " + colorChangeValue, "Angle");

			System.Drawing.Drawing2D.LinearGradientBrush lgb = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, _ColorStart, _ColorEnd, angle);

			pe.Graphics.FillRectangle(lgb, -20, -20, this.Width+40, this.Height+40);

			//Pen p = new Pen(lgb, _BoardWidth);

			//pe.Graphics.DrawRectangle(p, this.ClientRectangle);
		}

		private void colorChangeTimer_Tick(object sender, EventArgs e)
		{
			colorChangeValue+=1;
			if (colorChangeValue > 50) { colorChangeValue = 0; }
			this.Invalidate();
		}

		private void ChangeLocationAndSize()
		{
			switch (_EmphasisMode)
			{
			case EmphasisModeType.Area:
				this.Bounds = _Area;
				break;
			case EmphasisModeType.Control:
				if (_Control != null)
				{
					Rectangle ret = _Control.Bounds;
					//ret = _Control.TopLevelControl.RectangleToClient(_Control.Parent.RectangleToScreen(ret));
					ret.Inflate(_BoardWidth, _BoardWidth);
					this.Bounds = ret;
				}
				break;
			}
		}
	}
}
