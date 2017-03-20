using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SEC.GenericSupport
{
	/// <summary>
	/// Form에 대한 Singleton pattern 지원 함수 이다.
	/// 이를 이용해서 모든 Form을 singleton pattern이 적용된 것 처럼 사용 할 수 있다.
	/// </summary>
	public class SingleForm : IDisposable
	{
		Form form;
		/// <summary>
		/// 현재 생성 되어 있는 폼을 가져 온다.
		/// </summary>
		public Form FormInstance
		{
			get {
				if (!IsCreated) { FormCreate(); }
				return form; 
			}
		}

		private void FormCreate()
		{
			form = (Form)(formType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
			form.Disposed += new EventHandler(form_Disposed);
			form.FormClosed += new FormClosedEventHandler(form_FormClosed);
			form.HandleDestroyed += new EventHandler(form_HandleDestroyed);

			System.Diagnostics.Debug.WriteLine("Singleform FormCreate." + formType.FullName);
		}

		void form_HandleDestroyed(object sender, EventArgs e)
		{
			form = null;
			System.Diagnostics.Debug.WriteLine("Singleform Handle Destroyed.." + formType.FullName);
		}

		void form_FormClosed(object sender, FormClosedEventArgs e)
		{
			form = null;
			System.Diagnostics.Debug.WriteLine("Singleform form_FormClosed." + formType.FullName);
		}

		void form_Disposed(object sender, EventArgs e)
		{
			form = null;

			string name = "";
			try
			{
				name = formType.FullName;
				System.Diagnostics.Debug.WriteLine("Singleform form_Disposed." + name);
			}
			catch { }
		}

		public static implicit operator Form(SingleForm fm)
		{
			return fm.FormInstance;
		}

		/// <summary>
		/// 생성 되었는지 여부를 반환한다.
		/// </summary>
		public bool IsCreated
		{
			get
			{
				if ( form == null ) { return false; }
				else if ( form.IsDisposed ) { return false; }
				else if (form.Disposing) { return false; }
				else { return true; }
			}
		}

		public bool IsVisiable
		{
			get
			{
				if (!IsCreated) { return false; }
				else { return form.Visible; }
			}
		}



		Type formType;
		public Type FormType
		{
			get { return FormType; }
		}

		IWin32Window _Owner = null;
		public IWin32Window Owner
		{
			get { return _Owner; }
			set { _Owner = value; }
		}

		private SingleForm() { }

		public SingleForm(Type T)
		{
			formType = T;
		}

		~SingleForm()
		{
			Dispose();
		}

		public void Dispose()
		{
			if ( form != null )
			{
				form.Dispose();
				form = null;
			}
		}

		public bool Create()
		{
			if (!IsCreated) { FormCreate(); }
			else
			{
				return false;
			}
			return true;
		}

		public void Show()
		{
			Show(_Owner);
		}

        public void Show(IWin32Window owner)
        {
            if (!IsCreated)
            {
                FormCreate();
                _Owner = owner;
                form.Show(owner);
            }
            else if (form.Visible)
            {
                form.Activate();
            }
            else
            {
                _Owner = owner;
                form.Show(owner);
            }
            form.WindowState = FormWindowState.Normal;
        }

		public bool Location(int x, int y)
		{
			if (!IsCreated) { return false; }
			form.Location = new System.Drawing.Point(x, y);
			return true;
		}

        public void FormLocation()
        {
            form.Location = new Point(Cursor.Position.X - (int)(form.Width * 0.9), Cursor.Position.Y - (int)(form.Height + 10));
        }

        public void BeamShiftLocation()
        {
            form.Location = new Point(Cursor.Position.X - (int)(form.Width * 0.75), Cursor.Position.Y + 20);
        }

		public bool Hide()
		{
			if (!IsCreated) { return false; }

            
			form.Hide();
            
			return true;
		}

		public bool Close()
		{
			if (!IsCreated) { return false; }

            form.Close();
			//form.Dispose();
			form = null;

			return true;
		}




        private static bool _RecordingEnable = false;
        public bool RecordEnable
        {
            get { return _RecordingEnable; }
            set 
            { 
                _RecordingEnable = value; 
            }
        }




        //void RecordEnable()
        //{

        //}

        //private static bool RecordEnable = false;

        public static void RecordingEnable(bool p)
        {
            _RecordingEnable = p;
            

        }
    }
}
