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
	public partial class AreaSelectPanel : Panel
	{
		public AreaSelectPanel()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		private Color _SelectedColor = Color.Green;
		[DefaultValue(typeof(Color), "Green")]
		public Color SelectedColor
		{
			get { return _SelectedColor; }
			set
			{
				_SelectedColor = value;
				if (DesignMode)
				{
					this.Invalidate();
				}
			}
		}

		public enum OperationModeEnum
		{
			AreaSelect,
			PointSelect
		}

		private OperationModeEnum _OperationMode = OperationModeEnum.AreaSelect;
		[DefaultValue(typeof(OperationModeEnum), "AreaSelect")]
		public OperationModeEnum OperationMode
		{
			get { return _OperationMode; }
			set
			{
				_OperationMode = value;
				this.Invalidate();
			}
		}


		private int _SelectedIndex = 5;
		[DefaultValue(5)]
		public int SelectedIndex
		{
			get { return _SelectedIndex; }
			set
			{
				_SelectedIndex = value;
				if (_OperationMode == OperationModeEnum.AreaSelect)
				{
					this.Invalidate();
					OnSelectedIndexChanged();
				}
			}
		}

		private Point _SelectedPoint = new Point(0, 0);
		public Point SelectedPoint
		{
			get { return _SelectedPoint; }
			set
			{
				_SelectedPoint = value;
				if (_OperationMode == OperationModeEnum.PointSelect)
				{
					this.Invalidate();
					OnSelectedPointChanged();
				}
			}
		}

		public event EventHandler SelectedPointChanged;
		protected virtual void OnSelectedPointChanged()
		{
			if (SelectedPointChanged != null)
			{
				SelectedPointChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler SelectedIndexChanged;
		protected virtual void OnSelectedIndexChanged()
		{
			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		private int drwIndex = 0;
		private Point drwPoint = new Point(0, 0);

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			switch (_OperationMode)
			{
			case OperationModeEnum.AreaSelect:
				AreaSelectModePaint(pe);
				break;
			case OperationModeEnum.PointSelect:
				PointSelectModePaint(pe);
				break;
			default:
				throw new InvalidOperationException("AreaSelectpanel - Undefined Operation Mode");
			}
		}

		private void PointSelectModePaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			g.FillEllipse(new SolidBrush(this._SelectedColor), _SelectedPoint.X - 1, _SelectedPoint.Y - 1, 2, 2);
			g.FillEllipse(new SolidBrush(this.ForeColor), drwPoint.X - 1, drwPoint.Y - 1, 2, 2);
		}

		private void AreaSelectModePaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;

			switch (_SelectedIndex)
			{
			case 1:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(0, 0, this.Width / 2, this.Height / 2));
				break;
			case 2:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 4, 0, this.Width / 2, this.Height / 2));
				break;
			case 3:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 2, 0, this.Width / 2, this.Height / 2));
				break;
			case 4:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(0, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 5:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 4, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 6:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 2, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 7:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(0, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			case 8:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 4, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			case 9:
				g.FillRectangle(new SolidBrush(_SelectedColor), new Rectangle(this.Width / 2, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			default:
				throw new InvalidOperationException("AreaSelectPanel - Undefined selectedindex");
			}

			switch (drwIndex)
			{
			case 1:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(0, 0, this.Width / 2, this.Height / 2));
				break;
			case 2:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 4, 0, this.Width / 2, this.Height / 2));
				break;
			case 3:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 2, 0, this.Width / 2, this.Height / 2));
				break;
			case 4:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(0, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 5:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 4, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 6:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 2, this.Height / 4, this.Width / 2, this.Height / 2));
				break;
			case 7:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(0, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			case 8:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 4, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			case 9:
				g.FillRectangle(new SolidBrush(ForeColor), new Rectangle(this.Width / 2, this.Height / 2, this.Width / 2, this.Height / 2));
				break;
			case 0:
				break;
			default:
				throw new InvalidOperationException("AreaSelectPanel - Undefined drwIndex");
			}

			/*
		switch (drwIndex)
		{
		case 0:
			break;
		case 1:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen( new Rectangle(0, 0, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 2:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 4, 0, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 3:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 2, 0, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 4:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(0, this.Height / 4, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 5:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 4, this.Height / 4, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 6:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 2, this.Height / 4, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 7:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(0, this.Height / 2, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 8:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 4, this.Height / 2, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		case 9:
			ControlPaint.FillReversibleRectangle(this.RectangleToScreen(new Rectangle(this.Width / 2, this.Height / 2, this.Width / 2, this.Height / 2)), ForeColor);
			break;
		default:
			throw new InvalidOperationException();
		}
		 * */
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			SelectedIndex = GetLocation(e.Location);
			SelectedPoint = e.Location;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			drwIndex = GetLocation(e.Location);
			drwPoint = e.Location;
			this.Invalidate();
		}

		private int GetLocation(Point point)
		{
			int index;

			if (point.X < this.Width / 3)
			{
				index = 1;
			}
			else if (point.X < this.Width*2 / 3)
			{
				index = 2;
			}
			else
			{
				index = 3;
			}

			if (point.Y < this.Height / 3)
			{
				index += 0;
			}
			else if (point.Y < this.Height * 2/3)
			{
				index += 3;
			}
			else
			{
				index += 6;
			}

			return index;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			drwIndex = 0;
			this.Invalidate();
		}
	}
}
