using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using System.Drawing;

namespace SEC.Nanoeye.NanoeyeSEM
{
	class UIsetBinder : INotifyPropertyChanged
	{
		private static UIsetBinder _Default = new UIsetBinder();
		public static UIsetBinder Default
		{
			get { return _Default; } 
		}

		public event PropertyChangedEventHandler  PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private Color _LabelForeColor = Color.Black;
		public Color LabelForeColor
		{
			get { return _LabelForeColor; }
			set
			{
				if (_LabelForeColor != value)
				{
					_LabelForeColor = value;
					OnPropertyChanged("LabelForeColor");
				}
			}
		}

		private Color _ButtonForeColor = Color.Black;
		public Color ButtonForeColor
		{
			get { return _ButtonForeColor; }
			set
			{
				if (_ButtonForeColor != value)
				{
					_ButtonForeColor = value;
					OnPropertyChanged("ButtonForeColor");
				}
			}
		}

		private int _MagIndex = 0;
		public int MagIndex
		{
			get { return _MagIndex; }
			set
			{
				if (_MagIndex != value)
				{
					_MagIndex = value;
					OnPropertyChanged("MagIndex");
				}
			}
		}

		private int _MagMinimum = 0;
		public int MagMinimum
		{
			get { return _MagMinimum; }
			set
			{
				if (_MagMinimum != value)
				{
					_MagMinimum = value;
					OnPropertyChanged("MagMinimum");
				}
			}
		}

		private int _MagMaximum = 100;
		[DefaultValue(100)]
		public int MagMaximum
		{
			get { return _MagMaximum; }
			set
			{
				if (_MagMaximum != value)
				{
					_MagMaximum = value;
					OnPropertyChanged("MaxMaximum");
				}
			}
		}

		private string _MagString = "";
		public string MagString
		{
			get { return _MagString; }
			set
			{
				if (_MagString != value)
				{
					_MagString = value;
					OnPropertyChanged("MagString");
				}
			}
		}
	}
}
