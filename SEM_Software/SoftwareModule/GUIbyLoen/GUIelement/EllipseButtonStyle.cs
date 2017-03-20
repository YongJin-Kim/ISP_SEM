using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Drawing;

namespace SEC.GUIelement
{
	public partial class EllipseButtonStyle : Component, INotifyPropertyChanged
	{
		private bool _RepeatPush = false;
		[DefaultValue(false)]
		public bool RepeatPush
		{
			get { return _RepeatPush; }
			set
			{
				if (_RepeatPush != value)
				{
					_RepeatPush = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("RepeatPush"));
					}
				}
			}
		}

		private double _PaintRatio = 100d;
		[DefaultValue(100d)]
		public double PaintRatio
		{
			get { return _PaintRatio; }
			set
			{
				if (_PaintRatio != value)
				{
					_PaintRatio = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("PaintRatio"));
					}
				}
			}
		}

		private Point _ImageOffset = new Point( 0, 0 );
		public Point ImageOffset
		{
			get { return _ImageOffset; }
			set
			{
				if (_ImageOffset != value)
				{
					_ImageOffset = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("ImageOffset"));
					}
				}
			}
		}

		private Color _ColorStart = Color.Silver;
		[DefaultValue( typeof( Color ), "Silver" )]
		public Color ColorStart
		{
			get { return _ColorStart; }
			set 
			{
				if (_ColorStart != value)
				{
					_ColorStart = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("ColorStart"));
					}
				}
			}
		}

		private Color _ColorCenter = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color ColorCenter
		{
			get { return _ColorCenter; }
			set 
			{
				if (_ColorCenter != value)
				{
					_ColorCenter = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("ColorCenter"));
					}
				}
			}
		}

		private Color _BackColor = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color BackColor
		{
			get { return _BackColor; }
			set 
			{
				if (_BackColor != value)
				{
					_BackColor = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("BackColor"));
					}
				}
			}
		}

		private Color _ActiveColor = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color ActiveColor
		{
			get { return _ActiveColor; }
			set
			{
				if (_ActiveColor != value)
				{
					_ActiveColor = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("ActiveColor"));
					}
				}
			}
		}

		private Font _Font = new Font(FontFamily.GenericSerif, 10);
		public Font Font
		{
			get { return _Font; }
			set
			{
				if (_Font != value)
				{
					_Font = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("Font"));
					}
				}
			}
		}

		private Color _ForeColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color ForeColor
		{
			get { return _ForeColor; }
			set
			{
				if (_ForeColor != value)
				{
					_ForeColor = value;
					if (!DesignMode)
					{
						OnPropertyChanged(new PropertyChangedEventArgs("ForeColor"));
					}
				}
			}
		}

		public event PropertyChangedEventHandler  PropertyChanged;
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, e);
			}
		}

		public EllipseButtonStyle()
		{
			InitializeComponent();
		}

		public EllipseButtonStyle(IContainer container)
		{
			container.Add( this );

			InitializeComponent();
		}
	}
}
