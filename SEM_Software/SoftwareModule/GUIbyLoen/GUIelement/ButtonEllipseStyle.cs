using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Drawing;


namespace SEC.GUIelement
{
	public partial class ButtonEllipseStyle : Component, System.Windows.Forms
	{
		private bool _RepeatPush = false;
		[DefaultValue( false )]
		public bool RepeatPush
		{
			get { return _RepeatPush; }
			set { _RepeatPush = value; }
		}

		private double _PaintRatio = 100d;
		[DefaultValue( 100d )]
		public double PaintRation
		{
			get { return _PaintRatio; }
			set { _PaintRatio = value; }
		}

		private Point _ImageOffset = new Point( 0, 0 );
		public Point ImageOffset
		{
			get { return _ImageOffset; }
			set { _ImageOffset = value; }
		}

		private Color _ColorStart = Color.Silver;
		[DefaultValue( typeof( Color ), "Silver" )]
		public Color ColorStart
		{
			get { return _ColorStart; }
			set { _ColorStart = value; }
		}

		private Color _ColorCenter = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color ColorCenter
		{
			get { return _ColorCenter; }
			set { _ColorCenter = value; }
		}

		private Color _BackColor = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color BackColor
		{
			get { return _BackColor; }
			set { _BackColor = value; }
		}

		private Color _ActiveColor = Color.White;
		[DefaultValue( typeof( Color ), "White" )]
		public Color ActiveColor
		{
			get { return _ActiveColor; }
			set { _ActiveColor = value; }
		}


		public ButtonEllipseStyle()
		{
			InitializeComponent();
		}

		public ButtonEllipseStyle(IContainer container)
		{
			container.Add( this );

			InitializeComponent();
		}
	}
}
