using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class BeamShiftCrossPoint : Form
	{
        MiniSEM MainForm;

		/// <summary>
		/// 휠이 눌린 위치를 그리는 값.
		/// </summary>
		private Point bDNMPoint;
		private bool bDrawNoMove2D;

		/// <summary>
		/// 휠을 누를 경우 시간에 따른 이동을 위한 타이머.
		/// </summary>
		private Timer mt = new Timer();

		private Point _bsPoint;
		private Point bsPoint
		{
			get { return _bsPoint; }
			set
			{
				if (value.X > bsnX.Maximum) { _bsPoint.X = (int)(bsnX.Maximum); }
				else if (value.X < bsnX.Minimum) { _bsPoint.Y = (int)(bsnX.Minimum); }
				else { _bsPoint.X = value.X; }
				if (value.Y > bsnY.Maximum) { _bsPoint.Y = (int)(bsnY.Maximum); }
				else if (value.Y < bsnY.Minimum) { _bsPoint.Y = (int)(bsnY.Minimum); }
				else { _bsPoint.Y = value.Y; }
				CrossPointBox.Invalidate();

				(SystemInfoBinder.Default.Equip.ColumnBSX as SEC.GenericSupport.DataType.IControlDouble).Value = bsPoint.X * (SystemInfoBinder.Default.Equip.ColumnBSX as SEC.GenericSupport.DataType.IControlDouble).Precision;
				(SystemInfoBinder.Default.Equip.ColumnBSY as SEC.GenericSupport.DataType.IControlDouble).Value = bsPoint.Y * (SystemInfoBinder.Default.Equip.ColumnBSY as SEC.GenericSupport.DataType.IControlDouble).Precision;
			}
		}

		/// <summary>
		/// 실제적으로 -2048 ~ 2047사이의 값을 가지는 BeamShift 값.
		/// </summary>
		public Point PointLocation { set { bsPoint = value; } get { return bsPoint; } }

		public BeamShiftCrossPoint()
		{
			InitializeComponent();
		}

        public BeamShiftCrossPoint(MiniSEM main)
        {
            MainForm = main;

            InitializeComponent();
        }

		protected override void OnLoad(EventArgs e)
		{
			mt.Interval = 100;
			mt.Tick += new EventHandler(mt_Tick);

			bDNMPoint = new Point(CrossPointBox.Width / 2, CrossPointBox.Height / 2);

			TextManager.Instance.DefineText(this);

			base.OnLoad(e);
		}

		void mt_Tick(object sender, EventArgs e)
		{
			int absX, absY;
			Point mousePoint = CrossPointBox.PointToClient(MousePosition);
			if (bDrawNoMove2D) {
				int defX = mousePoint.X - bDNMPoint.X;
				int defY = mousePoint.Y - bDNMPoint.Y;
				absX = Math.Abs(defX);
				absY = Math.Abs(defY);

				if (Math.Abs(defX) > Math.Abs(defY)) { if (defX > 0) { Cursor = Cursors.PanEast; } else { Cursor = Cursors.PanWest; } }
				else { if (defY > 0) { Cursor = Cursors.PanSouth; } else { Cursor = Cursors.PanNorth; } }

				bsPoint = new Point(bsPoint.X + defX / 2, bsPoint.Y);
				bsPoint = new Point(bsPoint.X, bsPoint.Y - defY / 2);
				CrossPointBox.Invalidate();
			}
		}

		private void CrossPointBox_Paint(object sender, PaintEventArgs e)
		{
			//e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point(((bsPoint.X) * CrossPointBox.Width / 2 / 2047) + 128 - 3, ((bsPoint.Y) * CrossPointBox.Height / 2 / -2047) + 128 - 3), new Size(5, 5)));
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(new Point(((bsPoint.X) * CrossPointBox.Width / 2 / 2047) + 101 - 3, ((bsPoint.Y) * CrossPointBox.Height / 2 / -2047) + 101 - 3), new Size(5, 5)));
			if (bDrawNoMove2D) { Cursors.NoMove2D.Draw(e.Graphics, new Rectangle(new Point(bDNMPoint.X - 15, bDNMPoint.Y - 15), new Size(15, 15))); }
            
			bsnX.Value = bsPoint.X;
			bsnY.Value = bsPoint.Y;

			Pen p = (Pen )Pens.Black.Clone();
			p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
			p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
			p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

			e.Graphics.DrawLine(p, new Point(0, CrossPointBox.Height / 2), new Point(CrossPointBox.Width, CrossPointBox.Height / 2));
			e.Graphics.DrawLine(p, new Point(CrossPointBox.Width / 2, 0), new Point(CrossPointBox.Width / 2, CrossPointBox.Height));
		}

		private void CrossPointBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				//bDNMPoint = e.Location;
				//e.Location = bDNMPoint;
				CrossPointBox.MouseMove += new MouseEventHandler(CrossPointBox_MouseMove);
			}
		}

		private void CrossPointBox_MouseMove(object sender, MouseEventArgs e)
		{
			CrossPointBox.MouseMove -= new MouseEventHandler(CrossPointBox_MouseMove);
			if (e.Button == MouseButtons.Left) {
				bDrawNoMove2D = true;
				Cursor.Position = CrossPointBox.PointToScreen(bDNMPoint);

				int defX = e.Location.X - bDNMPoint.X;
				int defY = e.Location.Y - bDNMPoint.Y;
				if ((Math.Abs(defX) < 10) && (Math.Abs(defY) < 10)) {
					Cursor = Cursors.NoMove2D;
				}
				else if (Math.Abs(defX) > Math.Abs(defY)) {
					if (defX > 0) { Cursor = Cursors.PanEast; }
					else { Cursor = Cursors.PanWest; }
				}
				else {
					if (defY > 0) { Cursor = Cursors.PanSouth; }
					else { Cursor = Cursors.PanNorth; }
				}
				mt.Start();
			}
		}

		private void CrossPointBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			if (bDrawNoMove2D == false) {
				bsPoint = new Point((e.Location.X - CrossPointBox.Width / 2) * 2047 / (CrossPointBox.Width / 2), (e.Location.Y - CrossPointBox.Height / 2) * -2047 / (CrossPointBox.Width / 2));
				CrossPointBox.MouseMove -= new MouseEventHandler(CrossPointBox_MouseMove);
			}
			if (mt.Enabled) { mt.Stop(); }
			bDrawNoMove2D = false;
			Cursor = Cursors.Default;
			CrossPointBox.Invalidate();
		}

		private void bsnX_Leave(object sender, EventArgs e)
		{
			bsPoint = new Point((int)(bsnX.Value), (int)(bsnY.Value));
		}

		private void CenterButton_Click(object sender, EventArgs e)
		{
			bsPoint = new Point(0, 0);
		}

        private void FormWhown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X - (int)(this.Width * 0.75), Cursor.Position.Y + 20);
        }

        private void FormClose(object sender, EventArgs e)
        {
            MainForm.bsWindows_Close();

            this.Hide();
        }

        //internal void BeamShiftLocation()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
