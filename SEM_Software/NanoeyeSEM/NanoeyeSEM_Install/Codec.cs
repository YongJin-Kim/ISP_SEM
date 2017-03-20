using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace NanoeyeSEM_Install
{
	[RunInstaller(true)]
	public partial class Codec : Installer
	{
		public Codec()
		{
			InitializeComponent();
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
		}

		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);
		}

		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
		}

		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		protected override void OnBeforeInstall(IDictionary savedState)
		{
			base.OnBeforeInstall(savedState);

			System.Windows.Forms.MessageBox.Show("테스트");
		}
	}
}
