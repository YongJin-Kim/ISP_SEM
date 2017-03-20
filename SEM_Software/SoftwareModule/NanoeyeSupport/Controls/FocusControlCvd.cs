using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SEC.GenericSupport.WindowsAPI;
using SEC.GUIelement;
using SEC.Nanoeye.NanoView;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public class FocusControlCvd : FocusControl
	{
		private SECtype.IControlDouble _InnerCvd = null;
		[DefaultValue(null)]
		[Browsable(false)]
		public SECtype.IControlDouble InnerCvd
		{
			get { return _InnerCvd; }
			set
			{
				if ( _InnerCvd != null )
				{
					_InnerCvd.ValueChanged -= new EventHandler(_InnerCvd_ValueChanged);
				}
				_InnerCvd = value;
				if ( _InnerCvd != null )
				{
					_InnerCvd.ValueChanged += new EventHandler(_InnerCvd_ValueChanged);

					prevetInnerValueEvent = true;
					base._InnerMinimum = (int)(_InnerCvd.Minimum / _InnerCvd.Precision) ;
					base._InnerMaximum = (int)(_InnerCvd.Maximum / _InnerCvd.Precision) ;
					base.InnerValue = (int)(_InnerCvd.Value / _InnerCvd.Precision);
					prevetInnerValueEvent = false;
				}
			}
		}

		void _InnerCvd_ValueChanged(object sender, EventArgs e)
		{
			if ( !prevetInnerValueEvent )
			{
				prevetInnerValueEvent = true;
				base._InnerMaximum = (int)(_InnerCvd.Maximum / _InnerCvd.Precision);
				base._InnerMinimum = (int)(_InnerCvd.Minimum / _InnerCvd.Precision);
				base.InnerValue = (int)(_InnerCvd.Value / _InnerCvd.Precision);
				prevetInnerValueEvent = false;
			}
		}


		private SECtype.IControlDouble _OutterCvd = null;
		[DefaultValue(null)]
		[Browsable(false)]
		public SECtype.IControlDouble OutterCvd
		{
			get { return _OutterCvd; }
			set
			{
				if ( _OutterCvd != null )
				{
					_OutterCvd.ValueChanged -= new EventHandler(_OutterCvd_ValueChanged);
				}
				_OutterCvd = value;
				if ( _OutterCvd != null )
				{
					_OutterCvd.ValueChanged += new EventHandler(_OutterCvd_ValueChanged);

					prevetOutterValueEvent = true;
					base._OutterMinimum = (int)(_OutterCvd.Minimum / _OutterCvd.Precision);
					base._OutterMaximum = (int)(_OutterCvd.Maximum / _OutterCvd.Precision);
					base.OutterValue = (int)(_OutterCvd.Value / _OutterCvd.Precision);
					prevetOutterValueEvent = false;

				}
			}
		}

		void _OutterCvd_ValueChanged(object sender, EventArgs e)
		{
			if ( !prevetOutterValueEvent )
			{
				prevetOutterValueEvent = true;
				base._OutterMaximum = (int)(_OutterCvd.Maximum / _OutterCvd.Precision);
				base._OutterMinimum = (int)(_OutterCvd.Minimum / _OutterCvd.Precision);
				base.OutterValue = (int)(_OutterCvd.Value / _OutterCvd.Precision);
				prevetOutterValueEvent = false;
			}
		}

		protected bool prevetInnerValueEvent = false;
		protected bool prevetOutterValueEvent = false;

		protected override void OnInnerValueChanged()
		{
			if ( !prevetInnerValueEvent )
			{
				prevetInnerValueEvent = true;
				if ( _InnerCvd != null )
				{
					_InnerCvd.Value = base._InnerValue * _InnerCvd.Precision;
				}
			
				base.OnInnerValueChanged();
				prevetInnerValueEvent = false;
			}
		}

		protected override void OnOutterValueChanged()
		{
			if ( !prevetOutterValueEvent )
			{
				prevetOutterValueEvent = true;
				if ( _OutterCvd != null )
				{
					_OutterCvd.Value = base._OutterValue * _OutterCvd.Precision;
				}
				base.OnOutterValueChanged();
				prevetOutterValueEvent = false;

			}
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}

		public FocusControlCvd()
		{
			InitializeComponent();
		}


		#region Mouse Hooking

		//int mode= 0;
		//int mouserPreLocation = 0;

		//private int hHook = 0;
		//User32.HookMouseProc MouseHookProcedure;

		//private static FocusControlCvd ms;


		//private static int limitLeft = 0;
		//private static int limitRight = 0;

		//private void MouseHooking()
		//{
		//    ms = this;
		//    mouserPreLocation = MousePosition.X;
		//    //this.Capture = true;

		//    MouseHookProcedure = new SEC.GenericSupport.WindowsAPI.User32.HookMouseProc(MouseHookProc);

		//    RuntimeTypeHandle rt = Type.GetTypeHandle(this);

		//    IntPtr hInstance = Kernel32.LoadLibrary(AppDomain.CurrentDomain.FriendlyName);


		//    if ( hHook != 0 )
		//    {
		//        throw new Exception();
		//    }

		//    hHook = User32.SetWindowsHookEx(7,
		//                MouseHookProcedure,
		//                hInstance,

		//        0);
		//    if ( hHook == 0 )
		//    {
		//        int err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

		//        Debug.WriteLine("Hooking Error");
		//        return;
		//    }

		//    Debug.WriteLine("Mouse Hooked");

		//    System.Drawing.Rectangle rect = this.ClientRectangle;
		//    //rect.Inflate(-1, -1);

		//    Cursor.Clip = this.RectangleToScreen(rect);

		//    limitLeft = PointToScreen(this.Location).X + 3;
		//    limitRight = PointToScreen(this.Location).X + Width - 30;
		//}

		//private void UnHookMouseMove(string mode)
		//{
		//    if ( hHook != 0 )
		//    {
		//        SEC.GenericSupport.WindowsAPI.User32.UnhookWindowsHookEx(hHook);
		//        Cursor.Clip = new Rectangle();

		//        Debug.WriteLine("Mouse UnHook." + mode);
		//        hHook = 0;
		//    }
		//}

		//static bool hookprocessed = false;

		//public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		//{

		//    if ( nCode < 0 )
		//    {
		//        return User32.CallNextHookEx(ms.hHook, nCode, wParam, lParam);
		//    }
		//    try
		//    {
		//        if ( !hookprocessed )
		//        {
		//            hookprocessed = true;


		//            User32.MouseHookStruct mhs;
		//            switch ( (int)wParam )
		//            {
		//            case SEC.GenericSupport.WindowsAPI.WindowsMessage.WM_MOUSEMOVE:
		//                mhs = (SEC.GenericSupport.WindowsAPI.User32.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(User32.MouseHookStruct));


		//                if ( mhs.pt.x < limitLeft )
		//                {
		//                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(limitRight - 3, mhs.pt.y);
		//                    ms.mouserPreLocation = limitRight - 3;
		//                }
		//                else if ( mhs.pt.x > limitRight )
		//                {
		//                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(limitLeft + 3, mhs.pt.y);
		//                    ms.mouserPreLocation = limitLeft + 3;
		//                }
		//                else
		//                {
		//                    int deltaX = ms.mouserPreLocation - mhs.pt.x;
		//                    switch ( ms.mode )
		//                    {
		//                    case 1:
		//                        ms.OutterValue -= deltaX;
		//                        break;
		//                    case 2:
		//                        ms.InnerValue -= deltaX;
		//                        break;
		//                    }
		//                    ms.mouserPreLocation = mhs.pt.x;
		//                }


		//                break;
		//            case WindowsMessage.WM_LBUTTONUP:
		//            case WindowsMessage.WM_RBUTTONUP:
		//                ms.UnHookMouseMove("WM_?BUTTONUP");
		//                break;
		//            //default:
		//            //    return SEC.MSWindowsAPI.User32.CallNextHookEx(ms.hHook, nCode, (IntPtr)wParam, (IntPtr)lParam);
		//            }
		//            hookprocessed = false;
		//        }
		//    }
		//    catch ( Exception  )
		//    {
		//        ms.UnHookMouseMove("MoouseHookProc Exception.");
		//    }
		//    return 1;
		//}

		//protected override void OnMouseCaptureChanged(EventArgs e)
		//{

		//    base.OnMouseCaptureChanged(e);
		//}

		//protected override void OnLostFocus(EventArgs e)
		//{
		//    UnHookMouseMove("OnLostFocus");
		//    base.OnLostFocus(e);
		//}

		
		#endregion

	}
}
