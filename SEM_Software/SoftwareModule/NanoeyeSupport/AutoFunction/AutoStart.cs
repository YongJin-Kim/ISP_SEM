using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoStart : AutoFunctionBase
	{
		System.Threading.Timer procTimer;
		SEC.Nanoeye.NanoColumn.ISEMController column;

		AutoAIrange autoVideo;

		//AutoFocus autoFocus = null;
		SEC.Nanoeye.Support.Controls.PaintPanel painter;
		SEC.Nanoeye.NanoImage.IActiveScan scanner;
		IVideoVlaue gIvv;
		SEC.Nanoeye.NanoImage.SettingScanner scanSet;
		Dictionary<string, SEC.Nanoeye.NanoImage.SettingScanner> scanSetDic;

		public event EventHandler ScanModeChangeRequest;
		protected virtual void OnScanModeChangeRequest()
		{
			if (ScanModeChangeRequest != null)
			{
				ScanModeChangeRequest(this, EventArgs.Empty);
			}
		}

		public void HV_ON(	SEC.Nanoeye.NanoColumn.INormalSEM controller, 
							SEC.Nanoeye.Support.Controls.PaintPanel pp, 
							SEC.Nanoeye.NanoImage.IActiveScan ias, 
							IVideoVlaue ivv,
							SEC.Nanoeye.NanoImage.SettingScanner scanSetting,
							Dictionary<string, SEC.Nanoeye.NanoImage.SettingScanner> scanSets)

		{
			column = controller;

			painter = pp;
			scanner = ias;
			gIvv = ivv;

			scanSet = scanSetting;
			scanSetDic = scanSets;

			_CancelVisiable = true;
			_StopVisiable = false;
			_ProgressbarVisiable = true;
			OnCancelVisiableChanged();
			OnStopVisiableChanged();
			OnProgressbarVisiableChanged();

			procTimer = new System.Threading.Timer(new System.Threading.TimerCallback(OnProc));

			procCnt = 0;

			_Progress = 0;
			OnProgressChanged();

			procTimer.Change(100, 1000);
		}

		int procCnt;
		void OnProc(object arg)
		{
			switch (procCnt)
			{
			case 0:
				((SECtype.IControlBool)column["HvEnable"]).Value = true;
				break;
			case 1:
			case 2:
				break;
			case 3:
				((SECtype.IControlDouble)column["HvElectronGun"]).Value = ((SECtype.IControlDouble)column["HvElectronGun"]).Value;
				((SECtype.IControlDouble)column["HvGrid"]).Value = ((SECtype.IControlDouble)column["HvGrid"]).Value;
				((SECtype.IControlDouble)column["HvCollector"]).Value = ((SECtype.IControlDouble)column["HvCollector"]).Value;
				((SECtype.IControlDouble)column["HvPmt"]).Value = ((SECtype.IControlDouble)column["HvPmt"]).Value;
                
				break;
			case 4:
				((SECtype.IControlDouble)column["HvFilament"]).Value = ((SECtype.IControlDouble)column["HvFilament"]).Value;
				break;
			case 5:
				OnScanModeChangeRequest();
				break;
			case 6:
				//NanoImage.SettingScanner ss = AutoFunction.AutoVideo.CreateScanItem(scanSet);
				//scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);
				break;
			case 7:
				break;
			case 8:
				procTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

				////scanner.ScannerChange("AutoVideoAS", 0);
				autoVideo = new AutoAIrange();
				autoVideo.ProgressComplet += new EventHandler(autoSub_ProgressComplet);
				autoVideo.ProgressChanged += new EventHandler(autoSub_ProgressChanged);
				////SEC.Nanoeye.NanoImage.IScanItemEvent isie= scanner.GetScanEvent("AutoVideoAS");
				//SEC.Nanoeye.NanoImage.IScanItemEvent isie = scanner.ItemsReady[0];
				//painter.EventLink(isie, isie.Name);

				//autoVideo.AutoVideoAnalyzer(isie, gIvv, 3);
				autoVideo.AIrange(scanSetDic["FastScan"], scanner, painter, 0.1, gIvv);

				_SubCancelVisiable = true;
				_SubStopVisiable = true;
				_SubProgressbarVisiable = true;

				OnSubCancelVisiableChanged();
				OnSubProgressbarVisiableChanged();
				OnSubStopVisiableChanged();

				//scanner.Change();
				break;
			case 9:
				procTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

				scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { scanSet }, 0);
				painter.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);
				scanner.Change();

				OnProgressComplet();
				return;
			}
			procCnt++;
			_Progress = procCnt * 100 / 10;

			OnProgressChanged();
		}

		public override void Cancel()
		{
			procTimer.Dispose();
			if (autoVideo != null)
			{
				autoVideo.Cancel();
				autoVideo = null;
			}
			((SECtype.IControlBool)column["HvEnable"]).Value = false;
			_Cancled = true;
			OnProgressComplet();
		}

		#region SubFunction
		void autoSub_ProgressChanged(object sender, EventArgs e)
		{
			if (autoVideo != null)
			{
				// AUto Vidoe
				_SubProcess = autoVideo.Progress;
				OnSubProgressChanged();
			}
			else
			{
				// Auto Focus
			}
			OnSubProgressChanged();
		}

		void autoSub_ProgressComplet(object sender, EventArgs e)
		{
			if (autoVideo != null)
			{
				autoVideo.ProgressComplet -= new EventHandler(autoSub_ProgressComplet);
				autoVideo.ProgressChanged -= new EventHandler(autoSub_ProgressChanged);
				
				autoVideo = null;
			}
			else
			{	// AutoFocus
			}

			_SubCancelVisiable =false;
			_SubStopVisiable = false;
			_SubProgressbarVisiable = false;

			OnSubCancelVisiableChanged();
			OnSubProgressbarVisiableChanged();
			OnSubStopVisiableChanged();

			procTimer.Change(100, 1000);
		}
		#endregion

	}
}
