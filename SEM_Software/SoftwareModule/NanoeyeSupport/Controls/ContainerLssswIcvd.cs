using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Kikwak.Controls;
using SEC.GUIelement;
using SEC.Nanoeye.Support.Controls;

namespace SEC.Nanoeye.Support.Controls
{
	public partial class ContainerLssswIcvd : UserControl
	{
		public Font ButtonFont
		{
			get { return resetBB.Font; }
			set { resetBB.Font = value; }
		}

		public string LabelText
		{
			get { return label1.Text; }
			set
			{
				label1.Text = value;
				//LayoutControls();
				this.PerformLayout();
			}
		}

		protected int _IcvdCount = 0;
		[DefaultValue(0)]
		public int IcvdCount
		{
			get { return _IcvdCount; }
			set
			{
				_IcvdCount = value;
				LongscaleScrollsingWithICVD lsi;
				while ( _IcvdCount > LssswicvdList.Count )
				{
					lsi = new LongscaleScrollsingWithICVD();
					LssswicvdList.Add(lsi);
					this.Controls.Add(lsi);
					lsi.Visible = true;
					lsi.PanelColor = Color.Black;
				}
				while ( _IcvdCount < LssswicvdList.Count )
				{
					lsi = LssswicvdList[LssswicvdList.Count - 1];
					this.Controls.Remove(lsi);
					LssswicvdList.RemoveAt(LssswicvdList.Count - 1);
				}
				//LayoutControls();
				this.PerformLayout();
			}
		}

		protected List<LongscaleScrollsingWithICVD> LssswicvdList = new List<LongscaleScrollsingWithICVD>();

		public LongscaleScrollsingWithICVD this[int index]
		{
			get { return LssswicvdList[index]; }
			set
			{
				if ( value == null )
				{
					LssswicvdList.RemoveAt(index);
				}
				else
				{
					LssswicvdList[index] = value;
				}
				this.PerformLayout();
			}
		}

		public int LabelWidth
		{
			get { return label1.Width; }
			set
			{
				label1.Width = value;
				//LayoutControls();
				this.PerformLayout();
			}
		}

		protected int _LssswIcvdHeight = 26;
		[DefaultValue(26)]
		public int LssswIcvdHeight
		{
			get { return _LssswIcvdHeight; }
			set
			{
				_LssswIcvdHeight = value;
				//LayoutControls();
				this.PerformLayout();
			}
		}



		public bool VisiableAuto
		{
			get { return autoBB.Visible; }
			set
			{
				autoBB.Visible = value;
				//LayoutControls();
				this.PerformLayout();
			}
		}

		public bool VisiableReset
		{
			get { return resetBB.Visible; }
			set
			{
				resetBB.Visible = value;
				//LayoutControls();
				this.PerformLayout();
			}
		}

		public event EventHandler AutoClicked;

		protected virtual void OnAutoClicked()
		{
			if ( AutoClicked != null )
			{
				AutoClicked(this, EventArgs.Empty);
			}
		}

		public event EventHandler ResetClicked;
		protected virtual void OnResetClicked()
		{
			if ( ResetClicked != null )
			{
				ResetClicked(this, EventArgs.Empty);
			}
		}

		public ContainerLssswIcvd()
		{
			InitializeComponent();
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			LayoutControls();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			//LayoutControls();
		}

		protected virtual void LayoutControls()
		{
			int left = label1.Right + Margin.Horizontal;
			int right = this.Right;


			#region Auto & Reset 버튼 위치 처리
			if ( autoBB.Visible && resetBB.Visible )
			{
				autoBB.Location = new Point(this.ClientSize.Width - autoBB.Width - Margin.Right, this.ClientSize.Height / 2 - autoBB.Height - Margin.Top);
				resetBB.Location = new Point(autoBB.Left, autoBB.Bottom + Margin.Vertical);
				right = autoBB.Left - Margin.Horizontal;
			}
			else
			{
				if ( autoBB.Visible )
				{
					autoBB.Location = new Point(this.ClientSize.Width - autoBB.Width - Margin.Right, (this.ClientSize.Height - autoBB.Height) / 2);
					right = autoBB.Left - Margin.Horizontal;
				}
				else if ( resetBB.Visible )
				{
					resetBB.Location = new Point(this.ClientSize.Width - resetBB.Width - Margin.Right, (this.ClientSize.Height - resetBB.Height) / 2);
					right = resetBB.Left - Margin.Horizontal;
				}
			}
			#endregion

			int top = this.ClientSize.Height / 2;

			top -= _IcvdCount * (_LssswIcvdHeight + this.Margin.Vertical) / 2;

			foreach (LongscaleScrollsingWithICVD lsicvd in LssswicvdList)
			{
				lsicvd.Left = left;
				lsicvd.Top = top;
				lsicvd.Width = right - left;
				lsicvd.Height = _LssswIcvdHeight;

				top += _LssswIcvdHeight + this.Margin.Vertical;
			}

			this.Invalidate();
		}

		void resetBB_Click(object sender, EventArgs e)
		{
			OnResetClicked();
		}

		void autoBB_Click(object sender, EventArgs e)
		{
			OnAutoClicked();
		}
	}
}
