using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class AutoWD : AutoFunctionBase
	{
//        #region Property & Variables
//        NanoImage.IActiveScan scanner;
//        NanoColumn.INormalSEM column;
//        Controls.PaintPanel painter;

//        NanoImage.IScanItemEvent[] preScan;

//        int frameCount;
//        int[] freqFilter;
//        short[][] frameBuffer;

//        const int averFrameCount = 5;

//        Dictionary<int,float> freqList;
//        #endregion

//        public void SearchWD(NanoImage.IActiveScan scan, NanoColumn.INormalSEM con, Controls.PaintPanel paint)
//        {
//            Trace.Assert(scan != null);
//            Trace.Assert(con != null);
//            Trace.Assert(paint != null);

//            Trace.Assert(con.Enable);
//            Trace.Assert(con.HvEnable.Value);

//            scanner = scan;
//            column = con;
//            painter = paint;

//            preScan = scan.ItemsRunning;
//            Trace.Assert(preScan != null);

//            // Lens 안정화 시간때문에 가능한 빠른 타이밍에 index를 설정 한다.
//            column.LensWDtable.SelectedIndex = 0;

//            frameCount = 0;
//            frameBuffer = new short[averFrameCount][];

//            string scanName = "AutoFocusWD";

//            SEC.Nanoeye.NanoImage.SettingScanner ss=  (SEC.Nanoeye.NanoImage.SettingScanner)preScan[0].Setting.Clone();
//            ss.FrameWidth = 320;
//            ss.FrameHeight = 320;
//            ss.ImageHeight = 256;
//            ss.ImageLeft = 32;
//            ss.ImageTop = 32;
//            ss.ImageWidth = 256;
//            ss.PaintHeight = 0.5f;
//            ss.PaintWidth = 0.5f;
//            ss.PaintX = 0.25f;
//            ss.PaintY = 0.125f;

//            scanner.Ready(new SEC.Nanoeye.NanoImage.SettingScanner[] { ss }, 0);

//            freqList = new Dictionary<int, float>();

//            freqFilter = AutoFocusHelper.InitializeFreqFilter6(256, 256, 0.7f);
//            NanoImage.IScanItemEvent isie = scanner.ItemsReady[0];
//            painter.EventLink(isie, scanName);
//            isie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);

//            scanner.Change();
//        }

//        void isie_FrameUpdated(object sender, string name, int startline, int lines)
//        {
//            // 버리는 프레임 수.
//            const int outFrameCount = 3;

//            bool end = false;
//#if DEBUG
//            try
//            {
//#endif
//                frameCount++;
//                // 버리는 프레임 동안은 OL이 안정화 되지 않았다고 보고 계산하지 않음.
//                if (frameCount < outFrameCount + 1) { return; }
//                if (frameCount > averFrameCount + outFrameCount) { return; }

//                NanoImage.IScanItemEvent isie = sender as NanoImage.IScanItemEvent;
//                frameBuffer[frameCount - outFrameCount - 1] = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];

//                System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, frameBuffer[frameCount - outFrameCount - 1], 0, frameBuffer[frameCount - outFrameCount - 1].Length);

//                // 계산 하는 도중에 들어 오는 이미지는 무시하도록 한다.
//                if (frameCount == averFrameCount + outFrameCount)
//                {
//                    int wd = (int)column.LensWDtable.SelectedIndex;
//                    // OL 안정화 시간을 위해 가능한 빠른 타이밍에 WD 를 변경 한다.
//                    if (column.LensWDtable.Length - 1 > column.LensWDtable.SelectedIndex)
//                    {
//                        column.LensWDtable.SelectedIndex++;
//                    }
//                    else
//                    {
//                        end = true;
//                    }

//                    // 제곱 평균을 구한다.
//                    short[] img = AutoFocusHelper.MakePowAvrImage(frameBuffer, averFrameCount);

//                    float freqValue = AutoFocusHelper.GetFreqAmplValue(img, freqFilter, isie.Setting.ImageWidth, isie.Setting.ImageHeight);

//                    freqList.Add(wd, freqValue);

//                    Debug.WriteLine(string.Format("{0} index - {1} freqvalue", wd, freqValue), "AutoFocus WD");

//                    // 모든 WD에 대한 검사를 마침.
//                    if (end)
//                    {
//                        isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);

//                        int index = wd;
//                        float freq = freqValue;

//                        foreach (KeyValuePair<int,float> node in freqList)
//                        {
//                            if (node.Value > freqValue)
//                            {
//                                index = node.Key;
//                                freqValue = node.Value;
//                            }
//                        }

//                        column.LensWDtable.SelectedIndex = index;

//                        Stop();
//                    }

//                    frameCount = 0;
//                }
//#if DEBUG
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//#endif
//        }
	}
}
