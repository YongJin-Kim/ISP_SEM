using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Diagnostics;

namespace SEC.Nanoeye.Controls
{
	[DesignerAttribute(typeof(BitmapButtonDesigner))]
	public class BitmapButton : Button
	{
		public event EventHandler SurfaceChanged;

		#region Private 멤버
		private Bitmap m_Surface;
		private Bitmap TrueImage;
		private Bitmap FalseImage;
		#endregion

		#region Public 속성
		protected override bool ShowFocusCues
		{
			get
			{
				return false;// base.ShowFocusCues;
			}
		}

		public Bitmap Surface
		{
			get { return m_Surface; }
			set
			{
				if (m_Surface != value)
				{
					m_Surface = value;
					this.OnSurfaceChanged(EventArgs.Empty);
				}
			}
		}

		private int repeatDueTime = 300;
		[DefaultValue(300)]
		public int RepeatDueTime
		{
			get { return repeatDueTime; }
			set { repeatDueTime = value; }
		}
		private System.Threading.Timer repeatTimer;
		private bool repeatPush = false;
		private MouseEventArgs mea;
		[DefaultValue(false)]
		public bool RepeatPush
		{
			get { return repeatPush; }
			set { repeatPush = value; }
		}
		#endregion

		#region Public 메서드
		public BitmapButton()
		{
			base.SetStyle(
				ControlStyles.DoubleBuffer |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint,
				true);
			//base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles.Selectable, false);

			base.BackgroundImageLayout = ImageLayout.None;
			base.FlatStyle = FlatStyle.Flat;
			base.FlatAppearance.BorderSize = 0;
			//base.FlatAppearance.MouseDownBackColor = Color.Transparent;
			//base.FlatAppearance.MouseOverBackColor = Color.Transparent;
			//base.FlatAppearance.CheckedBackColor = Color.Transparent;
			base.BackColor = Color.Transparent;
			base.UseVisualStyleBackColor = false;
			base.UseCompatibleTextRendering = true;
			base.TabStop = false;
			base.DoubleBuffered = true;
			base.TabStop = false;
			repeatTimer = new System.Threading.Timer(new System.Threading.TimerCallback(RepeatTimerProc));
		}
		#endregion

		#region Protected 메서드
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.FlatAppearance.MouseDownBackColor = this.BackColor;
			base.FlatAppearance.MouseOverBackColor = this.BackColor;
			base.FlatAppearance.CheckedBackColor = this.BackColor;

			base.OnBackColorChanged(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Region != null)
			{
				e.Graphics.SetClip(this.Region, CombineMode.Intersect);
				e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
				e.Graphics.InterpolationMode = InterpolationMode.High;
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			}
			base.OnPaint(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (this.Surface != null)
			{
				//base.SetClientSizeCore(this.Surface.Width, this.Surface.Height / 2);
                base.SetClientSizeCore(this.Surface.Width, this.Surface.Height);
			}

			base.OnSizeChanged(e);
		}

		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
			this.AutoSize = false;
		}

		protected virtual void OnSurfaceChanged(EventArgs e)
		{
			if (this.Surface != null)
			{
				this.TrueImage = Helper.GetSurfaceImage(this.Surface, true);
				this.FalseImage = Helper.GetSurfaceImage(this.Surface, false);
				this.Region = Helper.GetBitmapRegion(this.FalseImage);

				this.BackgroundImage = this.FalseImage;

				base.SetClientSizeCore(this.TrueImage.Width, this.TrueImage.Height);
			}

			if (this.SurfaceChanged != null)
			{
				this.SurfaceChanged(this, e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.BackgroundImage = this.TrueImage;
			base.OnMouseDown(e);
			mea = e;
			if (repeatPush)
			{
				repeatTimer.Change(repeatDueTime, repeatDueTime);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.BackgroundImage = this.FalseImage;
			base.OnMouseUp(e);
			if (repeatPush)
			{
				repeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
			}
		}

		private delegate void MouseEventdelegate(MouseEventArgs e);

		private void RepeatTimerProc(Object state)
		{
			this.BeginInvoke(new MouseEventdelegate(base.OnMouseUp), new object[] { mea });
			this.BeginInvoke(new MouseEventdelegate(base.OnMouseDown), new object[] { mea });
		}
		#endregion
	}
}
