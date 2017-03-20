using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SEC.Nanoeye.Support.AutoFunction;

namespace SEC.Nanoeye.NanoeyeSEM
{
	public partial class AutoFunctionManager : Form
	{
		SEC.Nanoeye.NanoImage.IActiveScan scanner = SystemInfoBinder.Default.Nanoeye.Scanner;
		SEC.Nanoeye.NanoImage.SettingScanner preScanSet;
		SEC.Nanoeye.Support.Controls.PaintPanel ppSingle;
		IAutoFunction afRun;

		int afStep;
		int afStartMag;

		public enum AutoFunctionType
		{
			Non,
			AutoStartup,
			AutoVideo,
			AutoFocusWD,
			AutoFocusFine,
			AutoFocusCoarse,
			AutoFocusStig,
			AutoFilament,
			AutoEmission,
			AutoGunAlign,
			ExportSave,
			ExportPrint,
			ExportManager,
			ExportReport
		}

		public AutoFunctionManager()
		{
			InitializeComponent();

			TextManager.Instance.DefineText(this);
		}

        internal void RunAutoFunction(AutoFunctionType aft, SEC.Nanoeye.Support.Controls.PaintPanel ppSingle, SEC.Nanoeye.NanoColumn.Lens.IWDSplineObjBase iwdsob, string detectorStr)
		{
			SEC.Nanoeye.NanoImage.IScanItemEvent isie;
			this.ppSingle = ppSingle;

            try 
            {
                if (scanner.ItemsRunning[0] != null)
                {
                    preScanSet = scanner.ItemsRunning[0].Setting;
                }
                else
                {
                    preScanSet = scanner.ItemsReady[0].Setting;
                }

                
                
                

                switch (aft)
                {
                    case AutoFunctionType.AutoVideo:



                        AutoVideo av = new AutoVideo();
                        afRun = av;
                        av.Name = "Auto Video";
                        AutofunctionEventLink(av);


                        SEC.Nanoeye.NanoImage.SettingScanner ss = AutoVideo.CreateScanItem(scanner.ItemsRunning[0].Setting);
                        //SEC.Nanoeye.NanoImage.SettingScanner ss = AutoVideo.CreateScanItem(scanner.);
                        scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);
                        ppSingle.EventLink(scanner.ItemsReady[0], "Auto Video");
                        

                        av.AutoVideoAnalyzer(scanner.ItemsReady[0], SystemInfoBinder.Default, 3, detectorStr);
                        scanner.Change();

                        break;
                    case AutoFunctionType.AutoFocusCoarse:
                        //WD
                        AutoFocus afc = new AutoFocus();
                        afc.FrameAverage = 2;
                        afc.FrameDiscard = 1;
                        afc.NextGapUp = 2;
                        afc.NextGapDown = 2;
                        afc.DynamicDivider = new int[] { 32, 8 };

                        afRun = afc;
                        afc.Name = "Auto Focus Coarse";
                        AutofunctionEventLink(afc);

                        AutoFocusReady(afc);

                        
                         SEC.Nanoeye.NanoImage.SettingScanner[] aCSetscan = new NanoImage.SettingScanner[] { SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)ScanModeEnum.AutoFocus2]) };
                        if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED)
                        {
                            aCSetscan[0].AiChannel += 4;

                        }
                        
                        
                        scanner.Ready(aCSetscan, 0);

                        

                        isie = scanner.ItemsReady[0];

                       
                        isie.Setting.AiChannel = preScanSet.AiChannel;
                        

                        
                        

                        ppSingle.EventLink(isie, "Auto Focus Coarse");


                        afc.SearchFocusValue(isie, iwdsob, SystemInfoBinder.Default.Equip.ColumnLensOLC as SEC.GenericSupport.DataType.IControlDouble, AutoFocus.AutoFocusModeType.ValueRange, 4096);
                        scanner.Change();



                        break;
                    case AutoFunctionType.AutoFocusFine:
                        AutoFocus aff = new AutoFocus();

                        aff.FrameAverage = 2;
                        aff.FrameDiscard = 1;
                        aff.DynamicDivider = new int[] { 32, 16, 8, 8 };
                        aff.NextGapUp = 2;
                        aff.NextGapDown = 2;


                        afRun = aff;
                        aff.Name = "Auto Focus Fine";
                        AutofunctionEventLink(aff);

                        AutoFocusReady(aff);

                        SEC.Nanoeye.NanoImage.SettingScanner[] afSetscan = new NanoImage.SettingScanner[] { SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)ScanModeEnum.AutoFocus2]) };
                        if (SystemInfoBinder.Default.DetectorMode == SystemInfoBinder.ImageSourceEnum.BSED)
                        {
                            afSetscan[0].AiChannel += 4;

                        }


                        scanner.Ready(afSetscan, 0);

                        

                        isie = scanner.ItemsReady[0];

                        
                        isie.Setting.AiChannel = preScanSet.AiChannel;
                        

                        ppSingle.EventLink(isie, "Auto Focus Fine");
                        aff.SearchFocusValue(isie, null, SystemInfoBinder.Default.Equip.ColumnLensOLF as SEC.GenericSupport.DataType.IControlDouble, AutoFocus.AutoFocusModeType.ValueNear, 4096);
                        scanner.Change();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Auto Stop");
                
                //preScanSet = scanner.ItemsReady[0].Setting;

                //switch (aft)
                //{
                //    case AutoFunctionType.AutoVideo:
                //        AutoVideo av = new AutoVideo();
                //        afRun = av;
                //        av.Name = "Auto Video";
                //        AutofunctionEventLink(av);

                //        SEC.Nanoeye.NanoImage.SettingScanner ss = AutoVideo.CreateScanItem(scanner.ItemsReady[0].Setting);
                //        scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);
                //        ppSingle.EventLink(scanner.ItemsReady[0], "Auto Video");

                //        av.AutoVideoAnalyzer(scanner.ItemsReady[0], SystemInfoBinder.Default, 3);
                //        scanner.Change();

                //        break;
                //    case AutoFunctionType.AutoFocusCoarse:
                //        AutoFocus afc = new AutoFocus();
                //        afc.FrameAverage = 2;
                //        afc.FrameDiscard = 1;
                //        afc.NextGapUp = 2.2;
                //        afc.NextGapDown = 1.2;
                //        afc.DynamicDivider = new int[] { 16, 8, 6, 6 };

                //        afRun = afc;
                //        afc.Name = "Auto Focus Coarse";
                //        AutofunctionEventLink(afc);

                //        AutoFocusReady(afc);

                //        scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)ScanModeEnum.AutoFocus]) }, 0);

                //        isie = scanner.ItemsReady[0];

                        

                //        ppSingle.EventLink(isie, "Auto Focus Coarse");
                //        afc.SearchFocusValue(isie, null, SystemInfoBinder.Default.Equip.ColumnLensOLC as SEC.GenericSupport.DataType.IControlDouble, AutoFocus.AutoFocusModeType.ValueRange, 4096);
                //        scanner.Change();
                //        break;
                //    case AutoFunctionType.AutoFocusFine:
                //        AutoFocus aff = new AutoFocus();
                //        aff.FrameAverage = 4;
                //        aff.FrameDiscard = 1;
                //        aff.DynamicDivider = new int[] { 32, 8, };
                //        aff.NextGapUp = 1.5;
                //        aff.NextGapDown = 1;


                //        afRun = aff;
                //        aff.Name = "Auto Focus Fine";
                //        AutofunctionEventLink(aff);

                //        AutoFocusReady(aff);

                //        scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { SystemInfoBinder.Default.SetManager.ScannerLoad(SystemInfoBinder.ScanNames[(int)ScanModeEnum.AutoFocus2]) }, 0);


                //        isie = scanner.ItemsReady[0];

                //        ppSingle.EventLink(isie, "Auto Focus Fine");
                //        aff.SearchFocusValue(isie, null, SystemInfoBinder.Default.Equip.ColumnLensOLF as SEC.GenericSupport.DataType.IControlDouble, AutoFocus.AutoFocusModeType.ValueRange, 4096);
                //        scanner.Change();
                //        break;
                //    default:
                //        throw new NotImplementedException();
                //}
            }



            if (afRun == null)
            {
                return;
            }

			this.Text = afRun.Name;
			this.ShowDialog(SystemInfoBinder.Default.MainForm);
		}

		private void AutoFocusReady(AutoFocus afc)
		{
			afStep = 0;
			afc.ProcessStepChanged += new EventHandler(afc_ProcessStepChanged);
			afStartMag = UIsetBinder.Default.MagIndex;
		}

		void afc_ProcessStepChanged(object sender, EventArgs e)
		{
			int magIndex;
			switch (afStep)
			{
			case 0:
				magIndex = UIsetBinder.Default.MagIndex - 8;
				magIndex = Math.Max(magIndex, UIsetBinder.Default.MagMinimum);
				UIsetBinder.Default.MagIndex = magIndex;
				SystemInfoBinder.Default.Equip.MagChange(magIndex);
				break;
			default:
				magIndex = UIsetBinder.Default.MagIndex + 12;
				magIndex = Math.Min(magIndex, UIsetBinder.Default.MagMaximum);
				UIsetBinder.Default.MagIndex = magIndex;
				SystemInfoBinder.Default.Equip.MagChange(magIndex);
				break;
			}
			afStep++;
		}

		private void AutofunctionEventLink(AutoFunctionBase av)
		{
			av.ProgressComplet += new EventHandler(av_ProgressComplet);
			av.ProgressChanged += new EventHandler(av_ProgressChanged);
			av.CancelVisiableChanged += new EventHandler(av_CancelVisiableChanged);
			av.ProgressbarVisiableChanged += new EventHandler(av_ProgressbarVisiableChanged);
			av.StopVisiableChanged += new EventHandler(av_StopVisiableChanged);
			av.SubCancelVisiableChanged += new EventHandler(av_SubCancelVisiableChanged);
			av.SubStopVisiableChanged += new EventHandler(av_SubStopVisiableChanged);
			av.SubProgressbarVisiableChanged += new EventHandler(av_SubProgressbarVisiableChanged);
			av.SubProgressChanged += new EventHandler(av_SubProgressChanged);
		}

		#region Auto Function Event
		void av_SubProgressChanged(object sender, EventArgs e)
		{
			//IAutoFunction av = sender as IAutoFunction;

			//Action act=() => { AutoProgressSubUpb.Value = av.SubProcess; };
			//this.Invoke(act);
		}

		void av_SubProgressbarVisiableChanged(object sender, EventArgs e)
		{
			//IAutoFunction av = sender as IAutoFunction;
			//Action act=() => { AutoProgressSubUpb.Visible = av.SubProgressbarVisiable; };
			//this.Invoke(act);
		}

		void av_SubStopVisiableChanged(object sender, EventArgs e)
		{
			//IAutoFunction av = sender as IAutoFunction;
			//Action act=() => { AutoStopSubBut.Visible = av.SubStopVisiable; };
			//this.Invoke(act);
		}

		void av_SubCancelVisiableChanged(object sender, EventArgs e)
		{
			//IAutoFunction av = sender as IAutoFunction;
			//Action act=() => { AutoCancelSubBut.Visible = av.SubCancelVisiable; };
			//this.Invoke(act);
		}

		void av_StopVisiableChanged(object sender, EventArgs e)
		{
			IAutoFunction av = sender as IAutoFunction;
			Action act=() => { AutoStopBut.Visible = av.StopVisiable; };
			if (InvokeRequired) { this.Invoke(act); }
			else { act(); }
		}

		void av_ProgressbarVisiableChanged(object sender, EventArgs e)
		{
			IAutoFunction av = sender as IAutoFunction;
			Action act=() => { progressBar1.Visible = av.ProgressbarVisiable; };
			this.Invoke(act);
		}

		void av_CancelVisiableChanged(object sender, EventArgs e)
		{
			IAutoFunction av = sender as IAutoFunction;
			Action act=() => { AutoCancelBut.Visible = av.CancelVisiable; };
			if (InvokeRequired) { this.Invoke(act); }
			else { act(); }
		}

		void AutoFunctionManager_ScanModeChangeRequest(object sender, EventArgs e)
		{
			//OnScanModeChangeRequeset();
		}

		void av_ProgressChanged(object sender, EventArgs e)
		{
			IAutoFunction iaf = sender as IAutoFunction;
			Action<ProgressBar, int> valueSet = (x, y) =>
			{
				x.Value = y;
				x.Refresh();
			};



			if (progressBar1.InvokeRequired) { progressBar1.BeginInvoke(valueSet, new object[] { progressBar1, iaf.Progress }); }
			else
			{
				valueSet(progressBar1, iaf.Progress);
			}
		}

		void av_ProgressComplet(object sender, EventArgs e)
		{
			IAutoFunction iaf = sender as IAutoFunction;
			switch (iaf.Name)
			{
			//case "Auto Startup":
			//case "AutoEmission":
			//    // Do Nothing.
			//    break;

			////case "AutoFocus":
			//case "AutoFocus - Fine":
			//case "AutoStig":
			//case "AutoGunalign":
				//scanner.Ready(new SettingScanner[] { ScanSets[ScanModeEnum.FastScan] }, 0);
				//_DisplayPanel.EventLink(_Scanner.ItemsReady[0], _Scanner.ItemsReady[0].Name);
				//_Scanner.Change();
				//break;
			case "Auto Focus Coarse":
			case "Auto Focus Fine":

				UIsetBinder.Default.MagIndex = afStartMag;

				scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { preScanSet }, 0);
				ppSingle.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Setting.Name);
				scanner.Change();
				break;
			case "Auto Video":
				scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { preScanSet }, 0);
				ppSingle.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Setting.Name);
				scanner.Change();
				
				break;
			//case "ExportSave":
			//    if (!av.Cancled)
			//    {
			//        Action exportSaveAct = () =>
			//        {
			//            ImageExportGeneral.ImageExport(ImageExportGeneral.ImageExportModeEnum.Save,
			//                                             _Controller,
			//                                             _Stage,
			//                                             _DisplayPanel,
			//                                             _SystemInfo,
			//                                             true);
			//        };
			//        //Action exportSaveAct = () => { ImageExportGeneral.ExportSave(_DisplayPanel.ExportPicture()); };
			//        this.Invoke(exportSaveAct);
			//    }
			//    break;
			//case "ExportPrint":
			//    if (!av.Cancled)
			//    {
			//        //Action exportPrintAct = () => { ImageExportGeneral.ExportPrint(_DisplayPanel.ExportOriginal()); };
			//        Action exportPrintAct = () =>
			//        {
			//            ImageExportGeneral.ImageExport(ImageExportGeneral.ImageExportModeEnum.Print,
			//                                             _Controller,
			//                                             _Stage,
			//                                             _DisplayPanel,
			//                                             _SystemInfo,
			//                                             true);
			//        };
			//        this.Invoke(exportPrintAct);
			//    }
			//    break;
			//case "ExportManager":
			//    if (!av.Cancled)
			//    {
			//        //Action exportManager=() => { ImageExportGeneral.ExportManager(_DisplayPanel.ExportData(), _Controller, _Stage, _DisplayPanel, _SystemInfo); };
			//        Action exportManager=() =>// { ImageExportGeneral.ExportManager(_DisplayPanel.ExportData(), _Controller, XeyeStage.SNE_5000M.Mediator.Instance, _DisplayPanel, _SystemInfo); };
			//        {
			//            ImageExportGeneral.ImageExport(ImageExportGeneral.ImageExportModeEnum.DB,
			//                                             _Controller,
			//                                             _Stage,
			//                                             _DisplayPanel,
			//                                             _SystemInfo,
			//                                             true);
			//        };
			//        this.Invoke(exportManager);
			//    }
			//    break;
			//case "ExportReport":	// 구현 되지 않음.
			////    if (!av.Cancled)
			////    {
			////        Action exportReport =() => { ImageExportGeneral.ExportReport(_DisplayPanel.ExportOriginal(), _Controller, XeyeStage.SNE_5000M.Mediator.Instance, _DisplayPanel, _SystemInfo); };
			////        //Action exportReport =() => { ImageExportGeneral.ExportReport(_DisplayPanel.ExportPicture(), _Controller, _Stage, _DisplayPanel, _SystemInfo); };
			////        this.Invoke(exportReport);
			////    }
			////    break;
			default:
				throw new NotSupportedException();
			}

			Action act = () => { this.Close(); };
			if (this.InvokeRequired) { this.Invoke(act); }
			else { act(); }
		}
		#endregion

		private void AutoStopBut_Click(object sender, EventArgs e)
		{
			afRun.Stop();
		}

		private void AutoCancelBut_Click(object sender, EventArgs e)
		{
			afRun.Cancel();
		}
	}
}
