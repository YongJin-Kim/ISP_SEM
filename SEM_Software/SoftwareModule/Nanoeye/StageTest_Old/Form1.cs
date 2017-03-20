using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECtype = SEC.GenericSupport.DataType;

namespace StageTest
{
	public partial class Form1 : Form
	{
		SEC.Nanoeye.NanoStage.IStage stage;

		public Form1()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SEC.Nanoeye.NanoeyeFactory nf = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE5000M);
			stage = nf.Stage;

			stage.Device = stage.AvailableDevices()[0];
			stage.Initialize();

			foreach (SECtype.IControlValue con in stage)
			{
				comboBox1.Items.Add(con);
			}
			
		}

		private void axXleft_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void axXleft_MouseUp(object sender, MouseEventArgs e)
		{

		}

		private void axXright_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void axXright_MouseUp(object sender, MouseEventArgs e)
		{

		}

		private void axXhome_Click(object sender, EventArgs e)
		{

		}

		private void axesHome_Click(object sender, EventArgs e)
		{

		}

		private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = comboBox1.SelectedItem;
		}
	}
}
