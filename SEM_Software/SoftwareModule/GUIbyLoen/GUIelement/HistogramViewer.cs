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
	public partial class HistogramViewer : Control
	{
		#region Property & Vairalbes
		Action<Control, Bitmap> BackImageSetAct = (con, bm) =>
		{
			Image temp = con.BackgroundImage;
			con.BackgroundImage = bm;
			con.BackgroundImageLayout = ImageLayout.Stretch;
			if (temp != null)
			{
				temp.Dispose();
			}
		};

		const int dataWidth = 512;
		const int dataHeight = 512;

		Pen dataCol = Pens.Red;
		Pen hisMinCol = Pens.Yellow;
		Pen hisMaxCol = Pens.Green;

		private short[,] _ImageData = null;
		public short[,] ImageData
		{
			get { return _ImageData; }
			set
			{
				_ImageData = value;
				if (!DesignMode) { GenerateBackgroundImage(); }
			}
		}

		private short _HistogramMinimum = short.MinValue;
		[DefaultValue(short.MinValue)]
		public short HistogramMinimum
		{
			get { return _HistogramMinimum; }
			set
			{
				_HistogramMinimum = value;
				if (_HistogramMinimum > _HistogramRangeMinimum) { _HistogramRangeMinimum = HistogramMinimum; }
				OnHistogramMinimumChanged();
				if (!DesignMode) { GenerateBackgroundImage(); }
				this.Invalidate();
			}
		}

		private short _HistogramMaximum = short.MaxValue;
		[DefaultValue(short.MaxValue)]
		public short HistogramMaximum
		{
			get { return _HistogramMaximum; }
			set
			{
				_HistogramMaximum = value;
				if (_HistogramMaximum < _HistogramRangeMaximum) { _HistogramRangeMaximum = HistogramMaximum; }
				if (!DesignMode) { GenerateBackgroundImage(); }
				OnHistogramMaximumChanged();
				this.Invalidate();
			}
		}

		private short _HistogramRangeMinimum = short.MinValue;
		[DefaultValue(short.MinValue)]
		public short HistogramRangeMinimum
		{
			get { return _HistogramRangeMinimum; }
			set
			{
				_HistogramRangeMinimum = value;
				if (_HistogramMinimum > _HistogramRangeMinimum) { _HistogramRangeMinimum = HistogramMinimum; }
				this.Invalidate();
			}
		}

		private short _HistogramRangeMaximum = short.MaxValue;
		[DefaultValue(short.MaxValue)]
		public short HistogramRangeMaximum
		{
			get { return _HistogramRangeMaximum; }
			set
			{
				_HistogramRangeMaximum = value;
				if (_HistogramMaximum < _HistogramRangeMaximum) { _HistogramRangeMaximum = HistogramMaximum; }
				this.Invalidate();
			}
		}


		#endregion

		#region Event
		public event EventHandler HistogramMinimumChanged;
		protected virtual void OnHistogramMinimumChanged()
		{
			if (HistogramMinimumChanged != null)
			{
				HistogramMinimumChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler HistogramMaximumChanged;
		protected virtual void OnHistogramMaximumChanged()
		{
			if (HistogramMaximumChanged != null)
			{
				HistogramMaximumChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		public HistogramViewer()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			float divider = dataWidth / (float)this.ClientSize.Width;

			pe.Graphics.DrawLine(hisMinCol, 
				this.ClientSize.Width * (_HistogramRangeMinimum - _HistogramMinimum) / (_HistogramMaximum - _HistogramMinimum), 
				0,
				this.ClientSize.Width * (_HistogramRangeMinimum - _HistogramMinimum) / (_HistogramMaximum - _HistogramMinimum), 
				this.ClientSize.Height);
			pe.Graphics.DrawLine(hisMaxCol,
				this.ClientSize.Width * (_HistogramRangeMaximum- _HistogramMinimum) / (_HistogramMaximum - _HistogramMinimum),
				0,
				this.ClientSize.Width * (_HistogramRangeMaximum - _HistogramMinimum) / (_HistogramMaximum - _HistogramMinimum),
				this.ClientSize.Height);
			base.OnPaint(pe);
		}

		private unsafe void GenerateBackgroundImage()
		{
			if ((_ImageData == null) || (_ImageData.Length < 10))
			{
				Bitmap bmBlack = new Bitmap(10, 10);
				if (InvokeRequired)
				{
					this.Invoke(BackImageSetAct, this, bmBlack);
				}
				else
				{
					BackImageSetAct(this, bmBlack);
				}
				return;
			}

			//int divider = (short.MaxValue - short.MinValue + 1) / dataWidth;
			int[] rowData = new int[dataWidth];

			int maximum = 0;
			int index;
			int length = _ImageData.Length;
			int k;

			fixed (short* pntData = _ImageData)
			{
				short* pData = pntData;
				for (k = 0; k < length; k++)
				{
					//index = (*pData++ - short.MinValue) / divider;

					int val = *pData;

					if (val < _HistogramMinimum) { val = _HistogramMinimum; }
					else if (val >= _HistogramMaximum) { val = _HistogramMaximum - 1; }

					index = (val - _HistogramMinimum) * dataWidth / (_HistogramMaximum - _HistogramMinimum);
					//+_HistogramRangeMinimum;

					pData++;

					rowData[index]++;

					maximum = Math.Max(rowData[index], maximum);
				}
			}

			Bitmap bm = new Bitmap(dataWidth, dataHeight);
			Graphics g = Graphics.FromImage(bm);
			g.Clear(this.BackColor);

			double div = maximum / (double)dataHeight;

			for (index = 0; index < dataWidth; index++)
			{
				g.DrawLine(Pens.Red, (float)index, dataHeight, (float)index, dataHeight - (float)(rowData[index] / div));
			}
			g.Dispose();

			if (InvokeRequired)
			{
				this.Invoke(BackImageSetAct, this, bm);
			}
			else
			{
				BackImageSetAct(this, bm);
			}
			this.Invalidate();
		}



		//bool hisLeftControl = true;
		//Point preMousePnt;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//if (e.Location.X < this.Width / 2)
			//{
			//    hisLeftControl = true;
			//}
			//else
			//{
			//    hisLeftControl = false;
			//}

			//preMousePnt = e.Location;

			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			//if (e.Button == MouseButtons.Left)
			//{
			//    if (e.Location.X < this.Width / 2)
			//    {
			//        HistogramMinimum = (short)((_HistogramRangeMaximum - _HistogramRangeMinimum) * e.X / this.ClientSize.Width + _HistogramRangeMinimum);
			//    }
			//    else
			//    {
			//        HistogramMaximum = (short)((_HistogramRangeMaximum - _HistogramRangeMinimum) * e.X / this.ClientSize.Width + _HistogramRangeMinimum);
			//    }

			//}
			//this.Invalidate();
			base.OnMouseClick(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{

			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
		//    int newVal;


		//    switch (e.Button)
		//    {
		//    case MouseButtons.Left:

		//        break;
		//    case MouseButtons.Right:
		//        if (hisLeftControl)
		//        {
		//            newVal = _HistogramRangeMinimum + (short)(preMousePnt.X - e.Location.X);

		//            if (newVal < short.MinValue) { newVal = short.MinValue; }
		//            else if (newVal > _HistogramRangeMaximum - 1) { newVal = _HistogramRangeMaximum - 1; }

		//            HistogramRangeMinimum = (short)newVal;
		//        }

		//        else
		//        {
		//            newVal = _HistogramRangeMaximum + (short)(preMousePnt.X - e.Location.X);

		//            if (newVal > short.MaxValue) { newVal = short.MaxValue; }
		//            else if (newVal < _HistogramRangeMinimum + 1) { newVal = _HistogramRangeMinimum + 1; }

		//            HistogramRangeMaximum = (short)newVal;


		//        }

		//        preMousePnt = e.Location;
		//        break;
		//    }
			base.OnMouseMove(e);
		}
	}
}
