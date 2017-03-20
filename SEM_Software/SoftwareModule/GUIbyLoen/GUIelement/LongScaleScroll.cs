using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SEC.GUIelement
{
	public partial class LongScaleScroll : UserControl
	{
		#region Property
		protected Timer redrawingTimer;
		protected int redrawingValue = 0;

		protected Color _InnerColor = Color.LightGreen;
		[DefaultValue(typeof(Color),"LightGreen")]
		public Color InnerColor
		{
			get { return _InnerColor; }
			set { _InnerColor = value; }
		}

		protected Color _OutterColor = Color.Cyan;
		[DefaultValue(typeof(Color), "Cyan")]
		public Color OutterColor
		{
			get { return _OutterColor; }
			set { _OutterColor = value; }
		}

		protected Color _ActiveColor = Color.Violet;
		[DefaultValue(typeof(Color), "Violet")]
		public Color ActiveColor
		{
			get { return _ActiveColor; }
			set { _ActiveColor = value; }
		}

		protected Color _PanelBackColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color PanelBackColor
		{
			get
			{
				return _PanelBackColor;
			}
			set
			{
				_PanelBackColor = value;
				reversPanelCol = Color.FromArgb(255,
										255 - _PanelBackColor.R,
										255 - _PanelBackColor.G,
										255 - _PanelBackColor.B);
				this.Invalidate(valueDisRect);
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
				CalculrateValuedisplyRegion();
				this.Invalidate(valueDisRect);
			}
		}

		protected int _Minimum =0;
		[DefaultValue(0)]
		public int Minimum
		{
			get { return _Minimum; }
			set
			{
				_Minimum = value;
				CalculrateValuedisplyRegion();
				this.Invalidate(valueDisRect);
			}
		}

		protected int _Value = 0;
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Value
		{
			get { return _Value; }
			set
			{
				if ( value > _Maximum ) { _Value = _Maximum; }
				else if ( value < _Minimum ) { _Value = _Minimum; }
				else { _Value = value; }

				redrawingValue = _Value;

				CalculrateValuedisplyRegion();
				this.Invalidate(valueDisRect);
				OnValueChanged(EventArgs.Empty);
			}
		}

		protected float _DividInner = 5;
		[DefaultValue(5f)]
		public float DividInner
		{
			get { return _DividInner; }
			set { _DividInner = value; }
		}

		protected float _DividOutter = 0.5f;
		[DefaultValue(0.5f)]
		public float DividOutter
		{
			get { return _DividOutter; }
			set { _DividOutter = value; }
		}
		#endregion

		#region Event
		public event EventHandler ValueChanged;
		protected virtual void OnValueChanged(EventArgs e)
		{
			if ( ValueChanged != null )
			{
				ValueChanged(this, e);
			}
		}
		#endregion

		Rectangle valueDisRect;
		Rectangle valueIndRect;
		Color reversPanelCol = Color.White;

		public LongScaleScroll()
		{
			InitializeComponent();
			//activeTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ActiveTimerCallBack));

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			//SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			CalculrateValuedisplyRegion();
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			OutterBEleft.Size = new Size(this.ClientRectangle.Height - 2, this.ClientRectangle.Height - 2);

			InnerBEleft.Size = new Size(this.ClientRectangle.Height - 2, this.ClientRectangle.Height - 2);
			InnerBEleft.Location = new Point(this.ClientRectangle.Height, 1);


			InnerBERight.Size = new Size(this.ClientRectangle.Height - 2, this.ClientRectangle.Height - 2);
			InnerBERight.Location = new Point(this.ClientRectangle.Right - this.ClientRectangle.Height * 2 + 2, 1);

			OutterBEright.Size = new Size(this.ClientRectangle.Height - 2, this.ClientRectangle.Height - 2);
			OutterBEright.Location = new Point(this.ClientRectangle.Right - this.ClientRectangle.Height + 1, 1);

			CalculrateValuedisplyRegion();
		}

		private void CalculrateValuedisplyRegion()
		{
			valueDisRect = new Rectangle(this.ClientRectangle.Height * 2, 0, this.ClientSize.Width - (int)this.ClientRectangle.Height * 4, this.ClientSize.Height);
			valueIndRect = new Rectangle(valueDisRect.X + valueDisRect.Width * (_Value - _Minimum) / (_Maximum - _Minimum), 0, 3, valueDisRect.Height);

			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.FillRectangle(new SolidBrush(_PanelBackColor), valueDisRect);
			e.Graphics.FillRectangle(new SolidBrush(reversPanelCol), valueIndRect);

		}

		bool mouseCapture = false;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(valueDisRect.Contains(e.Location))
			{
				MouseCaptureSelect();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ( mouseCapture )
			{
				MouseCaptureSelect();
			}
		}

		int premouseposX;
		double moveAccum;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if ( mouseCapture )
			{
				if ( e.X < valueDisRect.Left + 3 )
				{
					if ( _Value > _Minimum )
					{
						premouseposX = valueDisRect.Right - 4;
						Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
					}
				}
				else if ( e.X > valueDisRect.Right - 4 )
				{
					if ( _Value < _Maximum )
					{
						premouseposX = valueDisRect.Left + 3;
						Cursor.Position = this.PointToScreen(new Point(premouseposX, e.Y));
					}
				}
				else
				{
					int truncateValue = 0;
					switch ( e.Button)
					{
					case MouseButtons.Left:
						moveAccum += (e.X - premouseposX) / _DividInner;
						truncateValue = (int)Math.Truncate(moveAccum);
						if ( truncateValue != 0 )
						{
							moveAccum -= truncateValue;
							this.Value += truncateValue;
						}
						break;
					case MouseButtons.Right:
						moveAccum += (e.X - premouseposX) / _DividOutter;
						truncateValue = (int)Math.Truncate(moveAccum);
						if ( truncateValue != 0 )
						{
							moveAccum -= truncateValue;
							redrawingValue += truncateValue;
						}
						break;
					}
					premouseposX = e.X;
				}
			}
		}

		Rectangle preClipRect;

		private void MouseCaptureSelect()
		{
			if ( mouseCapture )
			{
				mouseCapture = false;
				Cursor.Clip = preClipRect;
				Cursor.Current = Cursors.Default;

				redrawingTimer = new Timer();
				redrawingTimer.Tick += new EventHandler( redrawingTimer_Tick );
				redrawingTimer.Interval = 100;
				redrawingTimer.Start();
			}
			else
			{
				redrawingTimer.Stop();
				redrawingTimer.Dispose();


				mouseCapture = true;
				preClipRect = Cursor.Clip;
				Rectangle clipRect = valueDisRect;
				clipRect.Inflate(-2, -2);
				Cursor.Clip = this.RectangleToScreen(clipRect);
				Cursor.Current = Cursors.NoMoveHoriz;
				moveAccum = 0;
				premouseposX = this.PointToClient(Cursor.Position).X;
			}
		}

		void redrawingTimer_Tick(object sender, EventArgs e)
		{
			if(_Value != redrawingValue)
			{
				Value = redrawingValue;
			}
		}

		#region Button Click 처리
		private void ClickInneRight()
		{
			if ( _DividInner > 1 )
			{
				buttonAccum = 0;
				this.Value++;
			}
			else
			{
				buttonAccum += 1.0f / _DividInner;
				int tran = (int)Math.Truncate(buttonAccum);
				if ( tran != 0 )
				{
					buttonAccum -= tran;
					this.Value += (tran * InnerBERight.AccelState);
				}
			}
		}

		private void ClickInnerLeft()
		{
			if ( _DividInner > 1 )
			{
				buttonAccum = 0;
				this.Value--;
			}
			else
			{
				buttonAccum -= 1.0f / _DividInner;
				int tran = (int)Math.Truncate(buttonAccum);
				if ( tran != 0 )
				{
					buttonAccum -= tran;
					this.Value += (tran * InnerBEleft.AccelState);
				}
			}
		}

		private void ClickOutterRight()
		{
			buttonAccum += 1.0f * _DividInner / _DividOutter;
			int tran = (int)Math.Truncate(buttonAccum);
			if ( tran != 0 )
			{
				buttonAccum -= tran;
				this.Value += tran;
			}
		}

		private void ClickOutterLeft()
		{
			buttonAccum -= 1.0f * _DividInner / _DividOutter;
			int tran = (int)Math.Truncate(buttonAccum);
			if ( tran != 0 )
			{
				buttonAccum -= tran;
				this.Value += tran;
			}
		}

		float buttonAccum = 0;
		private void InnerBERight_Click(object sender, EventArgs e)
		{
			ClickInneRight();
		}

		private void InnerBEleft_Click(object sender, EventArgs e)
		{
			ClickInnerLeft();
		}

		private void OutterBEright_Click(object sender, EventArgs e)
		{
			ClickOutterRight();
		}

		private void OutterBEleft_Click(object sender, EventArgs e)
		{
			ClickOutterLeft();
		}

		private void OutterBEright_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ClickOutterRight();
		}

		private void InnerBERight_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ClickInneRight();
		}

		private void InnerBEleft_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ClickInnerLeft();
		}

		private void OutterBEleft_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ClickOutterLeft();
		}
		#endregion

		#region Activation
		/*
		System.Threading.Timer activeTimer;

		int actCnt = 0;

		Color runningBackColr;

		void ActiveTimerCallBack(Object obj)
		{
			int conv = 0;
			if ( actCnt < 5 ) // Active Color로 바꾸는 중.
			{
				conv = actCnt++;
			}
			else // TextLabelBackColor로 바꾸는 중.
			{
				conv = 10 - actCnt;
				actCnt++;
				if ( actCnt == 10 )
				{
					actCnt = 0;
				}

			}


			runningBackColr = Color.FromArgb(255,
				(_PanelBackColor.R - _ActiveColor.R) * conv / 5 + _ActiveColor.R,
				(_PanelBackColor.G - _ActiveColor.G) * conv / 5 + _ActiveColor.G,
				(_PanelBackColor.B - _ActiveColor.B) * conv / 5 + _ActiveColor.B
				);

			panel1.BeginInvoke(new Action(RegeneratePanelImg));
			//panel1.Refresh();

		}

		private void ActiveationNoty(bool enable)
		{
			actCnt = 0;
			if ( !enable )
			{
				activeTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
				runningBackColr = _PanelBackColor;
				RegeneratePanelImg();
			}
			else
			{
				activeTimer.Change(0, 200);
			}
		}
		*/
		#endregion

		#region Mouse Hooking
		/*
		private void panel1_Click(object sender, EventArgs e)
		{
			MouseHooking();
		}

		int mouserPreLocation = 0;

		public int hHook = 0;
		SEC.MSWindowsAPI.User32.HookMouseProc MouseHookProcedure;

		public static LongScaleScroll ms;

		public static double mouseMoveAccum = 0;


		private void MouseHooking()
		{
			ms = this;
			mouserPreLocation = MousePosition.X;
			//this.Capture = true;

			MouseHookProcedure = new SEC.MSWindowsAPI.User32.HookMouseProc(MouseHookProc);

			RuntimeTypeHandle rt = Type.GetTypeHandle(this);

			//IntPtr hInstance = SEC.MSWindowsAPI.Kernel32.LoadLibrary("User32");
			IntPtr hInstance = SEC.MSWindowsAPI.Kernel32.LoadLibrary(AppDomain.CurrentDomain.FriendlyName);


			if ( hHook != 0 )
			{
				throw new Exception();
			}

			hHook = SEC.MSWindowsAPI.User32.SetWindowsHookEx(7,
						MouseHookProcedure,
						hInstance,
				//Process.GetCurrentProcess().Id);
				//System.Threading.Thread.CurrentThread.ManagedThreadId);
				//AppDomain.CurrentDomain.Id);
				0);
			if ( hHook == 0 )
			{

				//MessageBox.Show("SetWindowsHookEx Failed");

				int err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				return;
			}


			Debug.WriteLine("Mouse Hook.");
			this.Capture = true;
			Cursor.Clip = this.TopLevelControl.RectangleToScreen(this.TopLevelControl.ClientRectangle);
			ActiveationNoty(true);
		}

		private void UnHookMouseMove(string mode)
		{
			if ( hHook != 0 )
			{
				SEC.MSWindowsAPI.User32.UnhookWindowsHookEx(hHook);
				Cursor.Clip = new Rectangle();
				Capture = false;
				Debug.WriteLine("Mouse UnHook." + mode);
				hHook = 0;
			}

			ActiveationNoty(false);

		}

		static bool hookProcessed = false;

		public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{

			if ( nCode < 0 )
			{
				return SEC.MSWindowsAPI.User32.CallNextHookEx(ms.hHook, nCode, wParam, lParam);
			}
			try
			{
				if ( !hookProcessed )
				{
					hookProcessed = true;

					double roundValue;
					SEC.MSWindowsAPI.User32.MouseHookStruct mhs;
					switch ( (int)wParam )
					{
					case SEC.MSWindowsAPI.WindowsMessage.WM_MOUSEMOVE:
						mhs = (SEC.MSWindowsAPI.User32.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(SEC.MSWindowsAPI.User32.MouseHookStruct));
						double divider = 1;

						switch ( MouseButtons )
						{
						case MouseButtons.Left:
							divider = 2;
							mouseMoveAccum += (double)(ms.mouserPreLocation - mhs.pt.x) / divider;
							roundValue = Math.Round(mouseMoveAccum);
							if ( roundValue == 0 ) { break; }
							mouseMoveAccum -= roundValue;
							ms.Value -= (int)roundValue;
							break;
						case MouseButtons.Right:
							divider = 0.5;
							mouseMoveAccum += (double)(ms.mouserPreLocation - mhs.pt.x) / divider;
							roundValue = Math.Round(mouseMoveAccum);
							if ( roundValue == 0 ) { break; }
							mouseMoveAccum -= roundValue;
							ms.Value -= (int)roundValue;
							break;
						}

						ms.mouserPreLocation = mhs.pt.x;
						break;
					case SEC.MSWindowsAPI.WindowsMessage.WM_MBUTTONUP:
						ms.UnHookMouseMove("WM_MBUTTONDBLCLK");
						break;
					//default:
					//    return SEC.MSWindowsAPI.User32.CallNextHookEx(ms.hHook, nCode, (IntPtr)wParam, (IntPtr)lParam);
					}
					hookProcessed = false;
				}
			}
			catch ( Exception ex )
			{
				Trace.WriteLine(ex.StackTrace, ex.Message);

				ms.UnHookMouseMove("MoouseHookProc Exception.");
			}
			return 1;
		}

		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			UnHookMouseMove("OnMouseCaptureChanged");
			base.OnMouseCaptureChanged(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			UnHookMouseMove("OnLostFocus");
			base.OnLostFocus(e);
		}
		*/
		#endregion
	}
}
