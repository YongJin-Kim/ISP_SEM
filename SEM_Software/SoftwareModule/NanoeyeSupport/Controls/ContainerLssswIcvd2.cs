using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class ContainerLssswIcvd2 : ContainerLssswIcvd
	{
		public ContainerLssswIcvd2()
		{
			InitializeComponent();
			label1.Dock = DockStyle.None;
			label1.Location = new Point(0, 0);
			label1.Height = autoBB.Height;
		}

		protected override void LayoutControls()
		{
			label1.Location = new Point(Padding.Left,Padding.Top);
			label1.Height = autoBB.Height;

			int right = this.ClientSize.Width - Padding.Right;

			if ( resetBB.Visible )
			{
				resetBB.Location = new Point(right - resetBB.Width, Padding.Top);
				right = resetBB.Left - Margin.Horizontal;
			}
			if ( autoBB.Visible )
			{
				autoBB.Location = new Point(right - autoBB.Width, Padding.Top);
				right = autoBB.Left - Margin.Horizontal;
			}

			int top = label1.Bottom + Margin.Vertical;

			foreach ( LongscaleScrollsingWithICVD lsicvd in LssswicvdList )
			{
				lsicvd.Left = Padding.Left;
				lsicvd.Top = top;
				lsicvd.Width = this.ClientSize.Width - Padding.Horizontal;
				lsicvd.Height = _LssswIcvdHeight;

				top += _LssswIcvdHeight + this.Margin.Vertical;
			}

		}
	}
}
