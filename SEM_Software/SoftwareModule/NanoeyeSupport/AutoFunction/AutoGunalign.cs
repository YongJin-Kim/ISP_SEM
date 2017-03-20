using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

using System.Diagnostics;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoGunalign : AutoFunctionBase
	{
		struct AlingInfStruct
		{
			public double gax;
			public double gay;
			public double average;

			public AlingInfStruct(double x, double y, double a)
			{
				gax = x;
				gay = y;
				average = a;
			}

			public override string ToString()
			{
				return ("X:" + gax.ToString() + " ,Y:" + gay.ToString() + " ,A:" + average.ToString());
			}
		};

		List<AlingInfStruct> aisList = new List<AlingInfStruct>();

		SECtype.IControlDouble gaX;
		SECtype.IControlDouble gaY;

		double preX;
		double preY;

		Action EventReleaser;

		protected override void OnProgressComplet()
		{
			//scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { preScan }, 0);

			//paint.EventLink(scanner.ItemsReady[0], scanner.ItemsReady[0].Name);

			//scanner.Change();

			base.OnProgressComplet();
		}

		public override void Stop()
		{
			EventReleaser();
			OnProgressComplet();
		}

		public override void Cancel()
		{
			EventReleaser();
			_Cancled = true;
			gaX.Value = preX;
			gaY.Value = preY;


			OnProgressComplet();
		}

		SEC.Nanoeye.NanoImage.SettingScanner preScan;
		SEC.Nanoeye.NanoImage.IActiveScan scanner;
		Controls.PaintPanel paint;

		public void AutoGunAlign(SEC.Nanoeye.NanoImage.IActiveScan scan, SEC.Nanoeye.NanoColumn.ISEMController con, Controls.PaintPanel painter, SEC.Nanoeye.NanoImage.SettingScanner scanSet)
		{
			scanner = scan;

			gaX = (SECtype.IControlDouble)con["GunAlignX"];
			gaY = (SECtype.IControlDouble)con["GunAlignY"];

			preX = gaX.Value;
			preY = gaY.Value;

			gaX.Value = 0;
			gaY.Value = 0;

			preScan = scanner.ItemsRunning[0].Setting;
			paint = painter;

			scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { scanSet }, 0);
			SEC.Nanoeye.NanoImage.IScanItemEvent isie = scanner.ItemsReady[0];
			paint.EventLink(isie, isie.Name);

			isie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isieFirstFrame_FrameUpdated);
			EventReleaser = () =>
			{
				isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isieFirstFrame_FrameUpdated);
			};

			scanner.Change();
		}

		public void AutoGunAlign(NanoImage.IScanItemEvent iScanItemEvent, SECtype.IValue gunX, SECtype.IValue gunY)
		{
			gaX = gunX as SECtype.IControlDouble;
			gaY = gunY as SECtype.IControlDouble;

			preX = gaX.Value;
			preY = gaY.Value;

			gaX.Value = 0;
			gaY.Value = 0;

			iScanItemEvent.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isieFirstFrame_FrameUpdated);
			EventReleaser = () =>
			{
				iScanItemEvent.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isieFirstFrame_FrameUpdated);
			};
		}


		void isieFirstFrame_FrameUpdated(object sender, string name, int startline, int lines)
		{
			SEC.Nanoeye.NanoImage.IScanItemEvent isie = sender as SEC.Nanoeye.NanoImage.IScanItemEvent;

			double gap, average;

			GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(isie.ImageData,
														typeof(short),
														isie.Setting.ImageWidth,
														lines,
														out gap,
														out average,
														0.05f);

			aisList.Add(new AlingInfStruct(gaX.Value, gaY.Value, average));
			gaX.Value = (gaX.Maximum - gaY.Minimum) / 10;

			EventChange(isie, isieXAnal_FrameUpdated);

			Trace.WriteLine("First frame " + aisList.Last<AlingInfStruct>().ToString(), "Auto GunAlign");
		}

		void isieXAnal_FrameUpdated(object sender, string name, int startline, int lines)
		{
			SEC.Nanoeye.NanoImage.IScanItemEvent isie = sender as SEC.Nanoeye.NanoImage.IScanItemEvent;

			double gap, average;

			GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(isie.ImageData,
														typeof(short),
														isie.Setting.ImageWidth,
														lines,
														out gap,
														out average,
														0.05f);

			aisList.Add(new AlingInfStruct(gaX.Value, gaY.Value, average));
			Trace.WriteLine(aisList.Count.ToString() + "'s frame " + aisList.Last<AlingInfStruct>().ToString(), "Auto GunAlign");
			if (aisList[aisList.Count - 1].average > aisList[aisList.Count - 2].average)
			{
				// average값이 증했다면.
				if (aisList.Count == 2)
				{
					Validate(gaX, gaX.Value * 2);
				}
				else
				{
					if ((aisList[aisList.Count - 1].average - aisList[aisList.Count - 2].average) > (aisList[aisList.Count - 2].average - aisList[aisList.Count - 3].average))
					{
						// average 값이 더 많이 증가 했다면.
						// 이전 값의 차 만큼만 증가.
						Validate(gaX, gaX.Value + (aisList[aisList.Count - 1].gax - aisList[aisList.Count - 2].gax));
					}
					else
					{
						// average 값 증가가 감소 했다면.

						if (Math.Abs(aisList[aisList.Count - 1].gax - aisList[aisList.Count - 2].gax) <( (gaX.Maximum - gaX.Minimum) / 100))
						{
							// gax의 차가 1% 이내 라면.
							gaX.Value = aisList[aisList.Count - 1].gax;

							// Y 분석으로 넘어감.

							aisList.Clear();
							aisList.Add(new AlingInfStruct(gaX.Value, gaY.Value, average));
							Validate(gaY, ((gaY.Maximum - gaY.Minimum) / 10));

							EventChange(isie, isieYAnal_FrameUpdated);
						}
						else
						{
							// 이전 값 차의 반만 증가.
							Validate(gaX, gaX.Value + (aisList[aisList.Count - 1].gax - aisList[aisList.Count - 2].gax) / 2);
						}
					}
				}
			}
			else
			{
				// average값이 감소 했다면.
				if (aisList.Count == 2)
				{
					Validate(gaX, -1 * (gaX.Maximum - gaX.Minimum) / 10);
				}
				else
				{
					if (Math.Abs(aisList[aisList.Count - 1].gax - aisList[aisList.Count - 2].gax) < (gaX.Maximum - gaX.Minimum) / 100)
					{
						// gax의 차가 1% 이내 라면.
						gaX.Value = aisList[aisList.Count - 2].gax;

						// Y 분석으로 넘어감.

						aisList.Clear();
						aisList.Add(new AlingInfStruct(gaX.Value, gaY.Value, average));
						gaY.Value = (gaY.Maximum - gaY.Minimum) / 10;

						EventChange(isie, isieYAnal_FrameUpdated);
					}
					else
					{
						// 마지막 것과 그 전것의 차의 반만 증가 시킴.
						// 그리고 마지막 것을 뺌.
						Validate(gaX, aisList[aisList.Count - 2].gax + (aisList[aisList.Count - 2].gax - aisList[aisList.Count - 1].gax) / 2);
						aisList.RemoveAt(aisList.Count - 1);
					}
				}
			}
		}

		private void Validate(SECtype.IControlDouble iControlDouble, double p)
		{
			if (iControlDouble.Maximum < p) { iControlDouble.Value = iControlDouble.Maximum; }
			else if (iControlDouble.Minimum > p) { iControlDouble.Value = iControlDouble.Minimum; }
			else { iControlDouble.Value = p; }
		}

		void isieYAnal_FrameUpdated(object sender, string name, int startline, int lines)
		{
			SEC.Nanoeye.NanoImage.IScanItemEvent isie = sender as SEC.Nanoeye.NanoImage.IScanItemEvent;

			double gap, average;

			GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(isie.ImageData,
														typeof(short),
														isie.Setting.ImageWidth,
														lines,
														out gap,
														out average,
														0.05f);

			aisList.Add(new AlingInfStruct(gaX.Value, gaY.Value, average));
			Trace.WriteLine(aisList.Count.ToString() + "'s frame " + aisList.Last<AlingInfStruct>().ToString(), "Auto GunAlign");
			if (aisList[aisList.Count - 1].average > aisList[aisList.Count - 2].average)
			{
				// average값이 증했다면.
				if (aisList.Count == 2)
				{
					Validate(gaY, gaY.Value + gaY.Value);
				}
				else
				{
					if ((aisList[aisList.Count - 1].average - aisList[aisList.Count - 2].average) > (aisList[aisList.Count - 2].average - aisList[aisList.Count - 3].average))
					{
						// average 값이 더 많이 증가 했다면.
						// 이전 값의 차 만큼만 증가.
						Validate(gaY, gaY.Value + aisList[aisList.Count - 1].gay - aisList[aisList.Count - 2].gay);
					}
					else
					{
						if (Math.Abs(aisList[aisList.Count - 1].gay - aisList[aisList.Count - 2].gay) < (gaY.Maximum - gaY.Minimum) / 100)
						{
							// gax의 차가 1% 이내 라면.
							gaY.Value = aisList[aisList.Count - 2].gay;

							Stop();
						}
						else
						{
							// average 값 증가가 감소 했다면.
							// 이전 값 차의 반만 증가.
							Validate(gaY, gaY.Value + (aisList[aisList.Count - 1].gay - aisList[aisList.Count - 2].gay) / 2);
						}
					}
				}
			}
			else
			{
				// average값이 감소 했다면.
				if (aisList.Count == 2)
				{
					Validate(gaY, gaY.Value + -1 * (gaY.Maximum - gaY.Minimum) / 10);
				}
				else
				{
					if (Math.Abs(aisList[aisList.Count - 1].gay - aisList[aisList.Count - 2].gay) < (gaY.Maximum - gaY.Minimum) / 100)
					{
						// gax의 차가 1% 이내 라면.
						gaY.Value = aisList[aisList.Count - 2].gay;

						Stop();
					}
					else
					{
						// 마지막 것과 그 전것의 차의 반만 증가 시킴.
						// 그리고 마지막 것을 뺌.
						Validate(gaY, gaY.Value + (aisList[aisList.Count - 2].gay - aisList[aisList.Count - 1].gay) / 2);
						aisList.RemoveAt(aisList.Count - 1);
					}
				}
			}
		}

		public void EventChange(SEC.Nanoeye.NanoImage.IScanItemEvent isie, SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate eve)
		{
			EventReleaser();
			isie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(eve);

			releaseIsie = isie;
			releaseEve = eve;

			EventReleaser = RemoveEvent;
		}

		SEC.Nanoeye.NanoImage.IScanItemEvent releaseIsie;
		SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate releaseEve;

		private void RemoveEvent()
		{
			releaseIsie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(releaseEve);
		}

	}
}
