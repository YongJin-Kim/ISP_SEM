using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class StatusServerViewer : Form
	{
		Action<TextBox, string> actTextBoxtSet = (x, y) =>
		{
			x.Text = y;
		};

		public StatusServerViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			StatusServer.Default.ClientAddressChanged += new EventHandler(Status_ClientAddressChanged);
			//StatusServer.Default.MessageGenerated += new EventHandler<SEC.GenericSupport.StringEventArg>(Status_MessageGenerated);
			StatusServer.Default.ServerEnableChanged += new EventHandler(Status_ServerEnableChanged);

			Status_ServerEnableChanged(StatusServer.Default, EventArgs.Empty);
			Status_ClientAddressChanged(StatusServer.Default, EventArgs.Empty);

			nudPort.Value = StatusServer.Default.ListenerpPort;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			StatusServer.Default.ClientAddressChanged -= new EventHandler(Status_ClientAddressChanged);
			//StatusServer.Default.MessageGenerated -= new EventHandler<SEC.GenericSupport.StringEventArg>(Status_MessageGenerated);
			StatusServer.Default.ServerEnableChanged -= new EventHandler(Status_ServerEnableChanged);
		}

		#region Status Server 이벤트 핸들링
		void Status_ServerEnableChanged(object sender, EventArgs e)
		{
			Action act = () => { ServerOnOff.Checked = StatusServer.Default.ServerEnable; };
			if (InvokeRequired) { this.BeginInvoke(act); }
			else { act(); }
		}

        //void Status_MessageGenerated(object sender, SEC.GenericSupport.StringEventArg e)
        //{
        //    string msg = e.Message;
        //    Action act = () =>
        //    {
        //        logLb.Items.Add(DateTime.Now.ToString() + "\t" + msg + "\r\n");
        //        while (logLb.Items.Count > 30)
        //        {
        //            logLb.Items.RemoveAt(0);
        //        }
        //    };
        //    if (InvokeRequired) { this.BeginInvoke(act); }
        //    else { act(); }
        //}

		void Status_ClientAddressChanged(object sender, EventArgs e)
		{
			byte[] addr = StatusServer.Default.ClientAddress.Address.GetAddressBytes();

			if (InvokeRequired)
			{
				this.BeginInvoke(actTextBoxtSet, tbIP1, addr[0].ToString());
				this.BeginInvoke(actTextBoxtSet, tbIP2, addr[1].ToString());
				this.BeginInvoke(actTextBoxtSet, tbIP3, addr[2].ToString());
				this.BeginInvoke(actTextBoxtSet, tbIP4, addr[3].ToString());
			}
			else
			{
				actTextBoxtSet(tbIP1, addr[0].ToString());
				actTextBoxtSet(tbIP2, addr[1].ToString());
				actTextBoxtSet(tbIP3, addr[2].ToString());
				actTextBoxtSet(tbIP4, addr[3].ToString());
			}

            //MiniSEM
            
		}

		#endregion

		private void ServerOnOff_Click(object sender, EventArgs e)
		{
			if (StatusServer.Default.ServerEnable)
			{
                ServerOnOff.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.status_off;
				StatusServer.Default.ServerOff();
			}
			else
			{
                ServerOnOff.BackgroundImage = global::SEC.Nanoeye.NanoeyeSEM.Properties.Resources.status_on;
				StatusServer.Default.ServerOn();
			}
		}

		private void nudPort_ValueChanged(object sender, EventArgs e)
		{

		}

        private void FromShown(object sender, EventArgs e)
        {
            this.Location = new Point((Cursor.Position.X - (int)(this.Width)) - 4, (Cursor.Position.Y - (int)(this.Height)) - 10);
        }

        private Point mouseCurrentPoint = new Point(0, 0);

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseCurrentPoint = e.Location;
            }
        }

        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);
            }
        }


        private void NetWorkBtn_Click(object sender, EventArgs e)
        {
            if (NetWorkBtn.Checked)
            {
                StatusServer.Default.LoopEnable = false;

                //LoopBtn.Checked = false;

                

            }
            else
            {

                StatusServer.Default.LoopEnable = true;
                //NetWorkBtn.Checked = false;
            }
        }



	}
}
