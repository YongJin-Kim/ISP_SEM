using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class FrontPanelList : Form
	{
		public FrontPanelList()
		{
			InitializeComponent();
		}

		public delegate void PanelSeletedDelegate(string name);
		public PanelSeletedDelegate PanelSeleted;

		List<Button> buttonList = new List<Button>(9);
		private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
		{
			FlowLayoutPanel flp = sender as FlowLayoutPanel;
			flp.Parent.Size = new Size(flp.Size.Width + 6, flp.Size.Height + 24);
		}

		public void AppendButton(string name)
		{
			Button but = new Button();
			but.Anchor = System.Windows.Forms.AnchorStyles.Top;
			but.AutoSize = true;
			but.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			but.Name = name;
			but.Text = name;
			but.UseVisualStyleBackColor = true;
			but.Dock = System.Windows.Forms.DockStyle.Fill;
			but.Margin = new System.Windows.Forms.Padding(0);
			but.Click += new EventHandler(but_Click);
			flowLayoutPanel1.Controls.Add(but);
		}

		void but_Click(object sender, EventArgs e)
		{
			if (PanelSeleted != null) {
				PanelSeleted((sender as Button).Text);
			}
		}

		private void FrontPanelList_Shown(object sender, EventArgs e)
		{
			this.Location = new Point(Cursor.Position.X, Cursor.Position.Y - this.Height);

			TextManager.TextStruct ts = TextManager.Instance.GetString((string)this.Tag);
			this.Text = ts.Text;
			this.Font = ts.Font;
		}

		internal void SetItem(List<Infragistics.Win.UltraWinTabControl.UltraTab> fpList)
		{
			flowLayoutPanel1.Controls.Clear();
			foreach (Infragistics.Win.UltraWinTabControl.UltraTab ut in fpList)
			{
				Button but = new Button();
				but.Anchor = System.Windows.Forms.AnchorStyles.Top;
				but.AutoSize = true;
				but.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
				but.Name = ut.Key;
				but.Text = ut.Text;
				but.Tag = ut;
				but.UseVisualStyleBackColor = true;
				but.Dock = System.Windows.Forms.DockStyle.Fill;
				but.Margin = new System.Windows.Forms.Padding(0);
				but.Click += new EventHandler(butUT_Click);
				flowLayoutPanel1.Controls.Add(but);
			}
		}

		void butUT_Click(object sender, EventArgs e)
		{
			if (PanelSeleted != null)
			{

				Button but = sender as Button;
				Infragistics.Win.UltraWinTabControl.UltraTab ut = but.Tag as Infragistics.Win.UltraWinTabControl.UltraTab;
				PanelSeleted(ut.Key);
			}
		}
	}
}
