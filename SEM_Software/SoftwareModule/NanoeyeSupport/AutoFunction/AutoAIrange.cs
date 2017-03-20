using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECimage = SEC.Nanoeye.NanoImage;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoAIrange : AutoFunctionBase
	{
		bool processing = false;
		public override void Cancel()
		{
			if(!processing)
			{
				scanner.ItemsRunning[0].FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(AutoAIrange_FrameUpdated);
				scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { scanSet }, 0);
				painter.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);
				scanner.Change();
				OnProgressComplet();
			}
		}

		SECimage.IActiveScan scanner;
		SECimage.SettingScanner scanSet;
		SEC.Nanoeye.Support.Controls.PaintPanel painter;
		double drop = 0.1;
		IVideoVlaue video;
		public void AIrange(SECimage.SettingScanner ScanSets, SECimage.IActiveScan ActiveScan, SEC.Nanoeye.Support.Controls.PaintPanel ppSingle, double dropRate, IVideoVlaue ivv)
		{
			video = ivv;
			drop = dropRate;
			scanner = ActiveScan;
			if(scanner.ItemsRunning.Length < 1) { throw new ArgumentException("ActiveScan is not running."); }

			_Progress = 0;
			OnProgressChanged();

			scanSet = ScanSets;
			if(scanSet == null)
			{
				throw new ArgumentException("Running scanset is not contanis in ScanSets.");
			}

			SECimage.SettingScanner processSet = (SECimage.SettingScanner)scanSet.Clone();
			if(scanSet.ImageWidth > 320)
			{
				double divid = scanSet.ImageWidth / 320;
				processSet.AiMaximum = 10f;
				processSet.AiMinimum = -10f;
				processSet.FrameHeight = (int)(processSet.FrameHeight / divid);
				processSet.FrameWidth = (int)(processSet.FrameWidth / divid);
				processSet.ImageHeight = (int)(processSet.ImageHeight / divid);
				processSet.ImageLeft = (int)(processSet.ImageLeft / divid);
				processSet.ImageTop = (int)(processSet.ImageTop / divid);
				processSet.ImageWidth = (int)(processSet.ImageWidth / divid);
			}

			painter = ppSingle;

			scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { processSet }, 0);
			scanner.ItemsReady[0].FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(AutoAIrange_FrameUpdated);
			ppSingle.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);
			scanner.Change();
		}

		void AutoAIrange_FrameUpdated(object sender, string name, int startline, int lines)
		{
			processing = true;
			scanner.ItemsRunning[0].FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(AutoAIrange_FrameUpdated);

			int length = scanner.ItemsRunning[0].Setting.ImageWidth * scanner.ItemsRunning[0].Setting.ImageHeight;

			short[] datas = new short[length];

			System.Runtime.InteropServices.Marshal.Copy(scanner.ItemsRunning[0].ImageData, datas, 0, length);

			System.Threading.ThreadPool.QueueUserWorkItem(
				delegate
				{
					List<short> oriList = new List<short>();
					oriList.AddRange(datas);
					oriList.Sort();



					oriList.RemoveRange((int)(length * (1 - drop)), (int)(length * drop));
					oriList.RemoveRange(0, (int)(length * drop));

					// 최소값이 음수 일 수 있으므로 Maximum 으로 계산 한다.
					double max = 10 * oriList[oriList.Count - 1] / Math.Pow(2, 15);
					double min = -10 * oriList[0] / Math.Pow(2, 15);

					double calVal;
					if(Math.Abs(max) > Math.Abs(min)) { calVal = max; }
					else { calVal = min; }

					double[] aiRange = new double[] { 0.1f, 0.2f, 0.5f, 1f, 2f, 5f, 10f };
					int index = 0;
					for(index = 0; index < aiRange.Length; index++)
					{
						if(aiRange[index] > calVal) { break; }
					}

					// 범위 초과
					if(index == aiRange.Length) { index = aiRange.Length - 1; }

					scanSet.AiMaximum = (float)(aiRange[index]);
					scanSet.AiMinimum = (float)(-1 * aiRange[index]);

					SECimage.SettingScanner processSet = (SECimage.SettingScanner)scanSet.Clone();
					double divid = scanSet.ImageWidth / 320d;
					processSet.FrameHeight = (int)(processSet.FrameHeight / divid);
					processSet.FrameWidth = (int)(processSet.FrameWidth / divid);
					processSet.ImageHeight = (int)(processSet.ImageHeight / divid);
					processSet.ImageLeft = (int)(processSet.ImageLeft / divid);
					processSet.ImageTop = (int)(processSet.ImageTop / divid);
					processSet.ImageWidth = (int)(processSet.ImageWidth / divid);

					scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { processSet }, 0);
					scanner.ItemsReady[0].FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(AutoAIrange_FrameUpdated1);
					painter.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);
					scanner.Change();

					_Progress = 50;
					OnProgressChanged();
				}
			);
			processing = false;
		}

		void AutoAIrange_FrameUpdated1(object sender, string name, int startline, int lines)
		{
			scanner.ItemsRunning[0].FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(AutoAIrange_FrameUpdated1);

			int length = scanner.ItemsRunning[0].Setting.ImageWidth * scanner.ItemsRunning[0].Setting.ImageHeight;

			short[] datas = new short[length];

			System.Runtime.InteropServices.Marshal.Copy(scanner.ItemsRunning[0].ImageData, datas, 0, length);


			double gap, average;

			GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(scanner.ItemsRunning[0].ImageData,
																	typeof(short),
																	scanner.ItemsRunning[0].Setting.ImageWidth,
																	lines,
																	out gap,
																	out average,
																	0.05f);

			int brightness = (int)(128 - average * Math.Pow(10, video.Contrast / 100d));
			if(brightness > 1024) { brightness = 1024; }
			else if(brightness < -1024) { brightness = -1024; }

			video.Brightness = brightness;

			scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { scanSet }, 0);
			painter.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);
			scanner.Change();

			OnProgressComplet();

		}
	}
}
