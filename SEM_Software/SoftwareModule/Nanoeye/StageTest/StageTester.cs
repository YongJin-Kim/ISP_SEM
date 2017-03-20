using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SECstage = SEC.Nanoeye.NanoStage;

namespace StageTest
{
	public partial class StageTester : Form
	{
		SECstage.SNE5000M.IStage5000M stage;

		public StageTester()
		{
			InitializeComponent();

		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SEC.Nanoeye.NanoeyeFactory nf = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE5000M);
			stage = nf.Stage as SECstage.SNE5000M.IStage5000M;

			stage.Device = stage.AvailableDevices()[0];
			stage.Initialize();

			axesListCb.BeginUpdate();
			axesListCb.Items.Clear();
			foreach (SECstage.IAxis ax in stage)
			{
				axesListCb.Items.Add(ax);
			}
			axesListCb.EndUpdate();

			axesListCb.SelectedIndex = 0;

			//stage.AxisX.Resolution = 800;
			//stage.AxisX.StepDistance = 753600;

			//stage.AxisY.Resolution = 800;
			//stage.AxisY.StepDistance = 500000;

			//stage.AxisR.Resolution = 800;
			//stage.AxisR.StepDistance = 3666;

			//stage.AxisT.Resolution = 800;
			//stage.AxisT.StepDistance = 3600;

			//stage.AxisZ.Resolution = 800;
			//stage.AxisZ.StepDistance = 1000000;
		}

		private void leftBut_MouseDown(object sender, MouseEventArgs e)
		{
			SECstage.IAxis ax = axisInfoPg.SelectedObject as SECstage.IAxis;

			ax.MoveVelocity(false);
		}

		private void leftBut_MouseUp(object sender, MouseEventArgs e)
		{
			SECstage.IAxis ax = axisInfoPg.SelectedObject as SECstage.IAxis;

			ax.Stop(false);
		}

		private void rightBut_MouseDown(object sender, MouseEventArgs e)
		{
			SECstage.IAxis ax = axisInfoPg.SelectedObject as SECstage.IAxis;

			ax.MoveVelocity(true);
		}

		private void rightBut_MouseUp(object sender, MouseEventArgs e)
		{
			SECstage.IAxis ax = axisInfoPg.SelectedObject as SECstage.IAxis;

			ax.Stop(false);
		}

		private void homeOneBut_Click(object sender, EventArgs e)
		{
			SECstage.IAxis ax = axisInfoPg.SelectedObject as SECstage.IAxis;

			SECstage.SNE5000M.DebugHelper.HomesearchOneAxis(ax, (int)homeOneIndexNud.Value);
		}

		private void homeAllBut_Click(object sender, EventArgs e)
		{
			stage.HomeSearch(!stage.IsHomeSearching);
		}

		private void axesListCb_SelectedValueChanged(object sender, EventArgs e)
		{
			axisInfoPg.SelectedObject = axesListCb.SelectedItem;
		}
	}
}
