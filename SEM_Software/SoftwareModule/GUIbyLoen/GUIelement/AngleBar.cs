using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class AngleBar : Control
	{
		public AngleBar()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);

			InitializeComponent();

			this.Controls.Add(AngleLable);
			
		}

		#region Property
		private double _angle = 0;
		/// <summary>
		/// 표시 각도
		/// </summary>
		[DefaultValue(0)]
		public double Angle
		{
			get { return _angle; }
			set { _angle = value; }
		}
		#endregion

		#region Event
		/// <summary>
		/// 각도가 바뀜.
		/// </summary>
		public event EventHandler<ScrollEventArgs> AngleChanged;

		protected virtual void OnAngleChanged(int value)
		{
			if ( AngleChanged != null ) {
			}
		}
		#endregion

		RectangleF innerCircle;
		Region textRegion;

		protected override void OnSizeChanged(EventArgs e)
		{
			if (( this.Height < 50 )||( this.Width < 100 )) {
				float ratioX = (float)this.Width / 40.0f;
				float ratioY = (float)this.Height / 20.0f;

				float ratio = Math.Max(ratioX, ratioY);
				innerCircle = new RectangleF(this.Width * (0.5f - ratio / 2), this.Height * (0.5f - ratio / 2), this.Width * ratio, this.Height * ratio);
				textRegion = new Region(innerCircle);
			}
			else {
				innerCircle = new RectangleF(this.Width * 0.3f, this.Height * 0.3f, this.Width * 0.4f, this.Height * 0.4f);
				textRegion = new Region(innerCircle);
			}

			AngleLable.Location = new Point((int)(innerCircle.Location.X + (innerCircle.Width - AngleLable.Width) / 2),
											(int)(innerCircle.Location.Y + (innerCircle.Height - AngleLable.Height) / 2));

			base.OnSizeChanged(e);
			this.Refresh();
	
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			if (this.Width < 22) {
				//g.DrawString("This is too Small", Font, Brushes.Black, new PointF(0, 0));
				return;
			}
			if (this.Height < 22) {
				//g.DrawString("This is too Small", Font, Brushes.Black, new PointF(0, 0));
				return;
			}
			g.DrawArc(Pens.Black, new Rectangle(1, 1, this.Width - 2, this.Height - 2), 0, 360);
			//g.DrawArc(Pens.Black, new Rectangle(15, 15, this.Width - 30, this.Height - 30), 0, 360);

			g.FillEllipse(Brushes.Aqua, innerCircle);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			Brush br = new SolidBrush(ForeColor);
			g.DrawString(_angle.ToString(), Font, br, innerCircle, sf);
			br.Dispose();

			//AngleLable.Invalidate();
			// 기본 클래스 OnPaint를 호출하고 있습니다.
			base.OnPaint(pe);
		}

	}
}
