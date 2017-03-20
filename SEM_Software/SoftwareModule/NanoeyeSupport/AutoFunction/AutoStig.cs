using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SECtype = SEC.GenericSupport.DataType;

using SECcolumn = SEC.Nanoeye.NanoColumn;
using SECimage = SEC.Nanoeye.NanoImage;
namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoSplieTwo : AutoFunctionBase
	{
		#region Property & Variables
		private	SECimage.IScanItemEvent scanItem;

		private SECtype.IControlDouble stigX;
		private SECtype.IControlDouble stigY;
		#endregion

		public override void Stop()
		{
			scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
			OnProgressComplet();
		}

		public override void Cancel()
		{
			OnProgressComplet();
		}

		protected override void OnProgressComplet()
		{
			Debug.WriteLine("AutoStig Complete");
			base.OnProgressComplet();
		}

		public void SearchTwoValue(SECimage.IScanItemEvent isie,
										SECtype.IControlDouble stigX,
										SECtype.IControlDouble stigY)
		{
			scanItem = isie;
			this.stigX = stigX;
			this.stigY = stigY;

			_StopVisiable = true;
			OnStopVisiableChanged();

			frameCount = 0;
			frameBuffer = new short[averFrameCount][];

			nearTable = new SortedList<double, double>();

			freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);

			searchIndex = 0;

			scanItem.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
		}

		int frameCount;
		int[] freqFilter;
		short[][] frameBuffer;

		const int averFrameCount = 5;

		SortedList<double, double> nearTable;

		int searchIndex = 0;


		void SearchNear_FrameUpdated(object sender, string name, int startline, int lines)
		{
			// 버리는 프레임 수.
			const int outFrameCount = 3;
#if DEBUG
			try
			{
#endif
				frameCount++;
				// 첫번째 프레임은 OL이 안정화 되지 않았다고 보고 버린다.
				if (frameCount < outFrameCount + 1) { return; }
				if (frameCount > averFrameCount + outFrameCount) { return; }

				NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
				frameBuffer[frameCount - outFrameCount - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

				System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - outFrameCount - 1], 0, frameBuffer[frameCount - outFrameCount - 1].Length);

				// 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
				if (frameCount == averFrameCount + outFrameCount)
				{
					//int focus = (int)(focusValue.Value / focusValue.Precision);

					SEC.GenericSupport.DataType.IControlDouble icd;

					switch (searchIndex)
					{
					case 0:
						icd = stigX;
						break;
					case 1:
						icd = stigY;
						break;
					default:
						throw new ArgumentException();
					}

					double focus	 = icd.Value;

					// 제곱 평균을 구한다.
					short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, averFrameCount);

					float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

					Debug.WriteLine(string.Format("{0} index - {1} freqValue", focus, freqValue), "AutoFocus-Range");

					nearTable.Add(icd.Value, freqValue);

					frameCount = 0;

					double focusCen = (icd.Maximum + icd.Minimum) / 2;

					switch (nearTable.Count)
					{
					case 1:
						if (focus > focusCen) { icd.Value = (focus + icd.Minimum) / 2; }
						else { icd.Value = (focus + icd.Maximum) / 2; }
						break;
					case 2:
						icd.Value = (nearTable.Keys.First() + nearTable.Keys.Last()) / 2;
						break;
					default:
						{
							int maxIndex = FindMaxValue(nearTable);
							if (maxIndex == 0)
							{
								double fVal = ((nearTable.Keys[maxIndex] + icd.Minimum) / 2);

								if (fVal == nearTable.Keys[maxIndex]) { Stop(); }
								else { icd.Value = fVal; }
							}
							else if (maxIndex == nearTable.Count - 1)
							{
								double fVal = ((nearTable.Keys[maxIndex] + icd.Maximum) / 2);

								if (fVal == nearTable.Keys[maxIndex]) { Stop(); }
								else { icd.Value = fVal ; }
							}
							else
							{
								double fftVal = nearTable.Values[maxIndex];

								double foR = nearTable.Keys[maxIndex];

								for (double index = nearTable.Keys[maxIndex - 1]; index < nearTable.Keys[maxIndex + 1]; index += icd.Precision)
								{
									double calFFT = SEC.GenericSupport.Mathematics.Interpolation.Spline(nearTable, index);

									if (calFFT > fftVal)
									{
										foR = index;
										fftVal = calFFT;
									}
								}

								icd.Value = foR;
								switch (searchIndex)
								{
								case 0:
									nearTable.Clear();
									frameCount = 0;
									searchIndex++;
									break;
								case 1:
									scanItem.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(SearchNear_FrameUpdated);
									Stop();
									break;
								default:
									throw new ArgumentException();
								}
							}
						}
						break;
					}
				}
#if DEBUG
			}
			catch (Exception ex)
			{
				throw new Exception("AutoFocus-Range Error", ex);
			}
#endif
		}

		private int FindMaxValue(SortedList<double, double> nearTable)
		{
			int result = 0;
			double val = nearTable.Values[result];

			foreach (KeyValuePair<double, double> kvp in nearTable)
			{
				if (kvp.Value > val)
				{
					result = nearTable.IndexOfKey(kvp.Key);
					val = kvp.Value;
				}
			}

			return result;
		}
	}
}
