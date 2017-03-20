using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace SEC.Nanoeye.Support.AutoFunction
{
    public class AutoVideo : AutoFunctionBase
    {
        protected delegate void ProcessChecker(int contrast, int brightness, int contrast2, int brightness2);
        ProcessChecker checker;

        public override void Cancel()
        {
            if (gIsie != null)
            {
                gIsie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
            }

            gIvv.Brightness = preBrightness;
            gIvv.Contrast = preContrast;

            gIvv.Brightness2 = preBrightness2;
            gIvv.Contrast2 = preContrast2;

            _Cancled = true;

            System.Diagnostics.Debug.WriteLine("Canceld. " + this._Name, "AutoVideo");

            OnProgressComplet();
        }

        public override void Stop()
        {
            if (gIsie != null)
            {

                gIsie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
            }

            _Cancled = true;

            OnProgressComplet();
        }

        IVideoVlaue gIvv;

        SEC.Nanoeye.NanoImage.IScanItemEvent gIsie;

        int cntTarget = 0;
        int count = -1;

        int preContrast, preBrightness, preContrast2, preBrightness2;

        string DetectorStr;

        /// <summary>
        /// Auto Contrast & Brightness 동작을 수행한다.
        /// </summary>
        /// <param name="isie">Scanner의 주사 동작 이벤트</param>
        /// <param name="ivv">Contast & Brightness 이벤트</param>
        /// <param name="analyzeCnt">분석 횟수</param>
        public void AutoVideoAnalyzer(SEC.Nanoeye.NanoImage.IScanItemEvent isie, IVideoVlaue ivv, int analyzeCnt, string detectorStr)
        {
            gIvv = ivv;
            gIsie = isie;

            count = 0;

            cntTarget = analyzeCnt;
            DetectorStr = detectorStr;

            if (analyzeCnt == 0) { checker = ProcessInfinity; }
            else { checker = ProcessLimited; }

            preContrast = gIvv.Contrast;
            preBrightness = gIvv.Brightness;

            

            preContrast2 = gIvv.Contrast2;
            preBrightness2 = gIvv.Brightness2;

            gIsie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);

            _Progress = 0;
            OnProgressChanged();
        }


        void isie_FrameUpdated(object sender, string name, int startline, int lines)
        {
            int length = gIsie.Setting.ImageWidth * lines;

            System.Diagnostics.Debug.WriteLine(gIsie.Setting.ImageWidth.ToString() + "," + gIsie.Setting.ImageHeight.ToString(), "FrameSize");

            double gap, average;
            int contrast = 0;
            int brightness = 0;
            int contrast2 = 0;
            int brightness2 = 0;

            if (DetectorStr == "Merge")
            {
                GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(gIsie.ImageData,
                                                                        typeof(short),
                                                                        gIsie.Setting.ImageWidth,
                                                                        lines,
                                                                        out gap,
                                                                        out average,
                                                                        0.05f);
                //double amplitude = 256 / gap;
                // Contrast를 크게 잡기 위해서 256 이외의 숫자 사용
                double amplitude = 256 / gap;
                amplitude =
                    (amplitude == float.PositiveInfinity) ? float.MaxValue :
                    ((amplitude == float.NegativeInfinity) ? float.MinValue : amplitude);

                // 명암과 휘도를 계산합니다.
                contrast = (int)(Math.Log10(amplitude) * 100);
                //int contrast = (int)(Math.Log10(amplitude) * 100);
                if (contrast > 256) { contrast = 256; }
                else if (contrast < -256) { contrast = -256; }

                brightness = (int)(128 - average * amplitude);
                //int brightness = (int)(128 - average * amplitude); 
                //int brightness = (int)(256 - average * amplitude);
                if (brightness > 512) { brightness = 512; }
                else if (brightness < -512) { brightness = -512; }

                System.Diagnostics.Debug.WriteLine("sci.SEDContrast ," + gIvv.Contrast.ToString(), "AV");
                System.Diagnostics.Debug.WriteLine("sci.SEDBrightness," + gIvv.Brightness.ToString(), "AV");

                //checker(contrast, brightness);

                contrast2 = 0;
                brightness2 = 0;
                gap = 0;
                average = 0;

                GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(gIsie.ImageData2,
                                                                    typeof(short),
                                                                    gIsie.Setting.ImageWidth,
                                                                    lines,
                                                                    out gap,
                                                                    out average,
                                                                    0.05f);

                //double amplitude = 256 / gap;
                // Contrast를 크게 잡기 위해서 256 이외의 숫자 사용
                amplitude = 512 / gap;
                amplitude =
                    (amplitude == float.PositiveInfinity) ? float.MaxValue :
                    ((amplitude == float.NegativeInfinity) ? float.MinValue : amplitude);

                // 명암과 휘도를 계산합니다.
                contrast2 = (int)(Math.Log10(amplitude) * 100);
                if (contrast2 > 256) { contrast2 = 256; }
                else if (contrast2 < -256) { contrast2 = -256; }

                brightness2 = (int)(128 - average * amplitude);

                if (brightness2 > 512) { brightness2 = 512; }
                else if (brightness2 < -512) { brightness2 = -512; }

                System.Diagnostics.Debug.WriteLine("sci.BSEDContrast2 ," + gIvv.Contrast2.ToString(), "AV");
                System.Diagnostics.Debug.WriteLine("sci.BSEDBrightness2," + gIvv.Brightness2.ToString(), "AV");
            }
            else if(DetectorStr == "DualSEBSE")
            {
                GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(gIsie.ImageData,
                                                                       typeof(short),
                                                                       gIsie.Setting.ImageWidth,
                                                                       lines,
                                                                       out gap,
                                                                       out average,
                                                                       0.05f);
                //double amplitude = 256 / gap;
                // Contrast를 크게 잡기 위해서 256 이외의 숫자 사용
                double amplitude = 384 / gap;
                amplitude =
                    (amplitude == float.PositiveInfinity) ? float.MaxValue :
                    ((amplitude == float.NegativeInfinity) ? float.MinValue : amplitude);

                // 명암과 휘도를 계산합니다.
                contrast = (int)(Math.Log10(amplitude) * 100);
                //int contrast = (int)(Math.Log10(amplitude) * 100);
                if (contrast > 256) { contrast = 256; }
                else if (contrast < -256) { contrast = -256; }

                brightness = (int)(128 - average * amplitude);
                //int brightness = (int)(128 - average * amplitude); 
                //int brightness = (int)(256 - average * amplitude);
                if (brightness > 512) { brightness = 512; }
                else if (brightness < -512) { brightness = -512; }

                System.Diagnostics.Debug.WriteLine("sci.SEDContrast ," + gIvv.Contrast.ToString(), "AV");
                System.Diagnostics.Debug.WriteLine("sci.SEDBrightness," + gIvv.Brightness.ToString(), "AV");

                //checker(contrast, brightness);

                contrast2 = 0;
                brightness2 = 0;
                gap = 0;
                average = 0;

                GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(gIsie.ImageData2,
                                                                    typeof(short),
                                                                    gIsie.Setting.ImageWidth,
                                                                    lines,
                                                                    out gap,
                                                                    out average,
                                                                    0.05f);

                //double amplitude = 256 / gap;
                // Contrast를 크게 잡기 위해서 256 이외의 숫자 사용
                amplitude = 96 / gap;
                amplitude =
                    (amplitude == float.PositiveInfinity) ? float.MaxValue :
                    ((amplitude == float.NegativeInfinity) ? float.MinValue : amplitude);

                // 명암과 휘도를 계산합니다.
                contrast2 = (int)(Math.Log10(amplitude) * 100);
                if (contrast2 > 256) { contrast2 = 256; }
                else if (contrast2 < -256) { contrast2 = -256; }

                brightness2 = (int)(128 - average * amplitude);

                if (brightness2 > 512) { brightness2 = 512; }
                else if (brightness2 < -512) { brightness2 = -512; }

                System.Diagnostics.Debug.WriteLine("sci.BSEDContrast2 ," + gIvv.Contrast2.ToString(), "AV");
                System.Diagnostics.Debug.WriteLine("sci.BSEDBrightness2," + gIvv.Brightness2.ToString(), "AV");
            }
            else
            {
                GenericSupport.ImageProcess.ImageAnalyser.VideoAnalyse(gIsie.ImageData,
                                                                        typeof(short),
                                                                        gIsie.Setting.ImageWidth,
                                                                        lines,
                                                                        out gap,
                                                                        out average,
                                                                        0.05f);
                //double amplitude = 256 / gap;
                // Contrast를 크게 잡기 위해서 256 이외의 숫자 사용
                double amplitude = 384 / gap;
                amplitude =
                    (amplitude == float.PositiveInfinity) ? float.MaxValue :
                    ((amplitude == float.NegativeInfinity) ? float.MinValue : amplitude);

                // 명암과 휘도를 계산합니다.
                contrast = (int)(Math.Log10(amplitude) * 100);
                //int contrast = (int)(Math.Log10(amplitude) * 100);
                if (contrast > 256) { contrast = 256; }
                else if (contrast < -256) { contrast = -256; }

                brightness = (int)(128 - average * amplitude);
                //int brightness = (int)(128 - average * amplitude); 
                //int brightness = (int)(256 - average * amplitude);
                if (brightness > 512) { brightness = 512; }
                else if (brightness < -512) { brightness = -512; }

                System.Diagnostics.Debug.WriteLine("sci.SEDContrast ," + gIvv.Contrast.ToString(), "AV");
                System.Diagnostics.Debug.WriteLine("sci.SEDBrightness," + gIvv.Brightness.ToString(), "AV");

            }

          
            checker(contrast, brightness, contrast2, brightness2);


        }

        private void ProcessInfinity(int contrast, int brightness, int contrast2, int brightness2)
        {
            gIvv.Contrast = contrast;
            gIvv.Brightness = brightness;

            gIvv.Contrast2 = contrast2;
            gIvv.Brightness2 = brightness2;


            _Progress++;
            if (_Progress > 100)
            {
                _Progress = 0;
            }
            OnProgressChanged();
        }

        private void ProcessLimited(int contrast, int brightness, int contrast2, int brightness2)
        {
            gIvv.Contrast = (gIvv.Contrast * count + contrast) / (count + 1);
            gIvv.Brightness = (gIvv.Brightness * count + brightness) / (count + 1);
            gIvv.Contrast2 = (gIvv.Contrast2 * count + contrast2) / (count + 1);
            gIvv.Brightness2 = (gIvv.Brightness2 * count + brightness2) / (count + 1);

            count++;
            _Progress = count * 100 / cntTarget;
            OnProgressChanged();
            if (count == cntTarget)
            {
                gIsie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
                _Progress = 100;
                OnProgressComplet();
            }
        }

        public static NanoImage.SettingScanner CreateScanItem(NanoImage.SettingScanner baseSetting)
        {
            SEC.Nanoeye.NanoImage.SettingScanner ss = (SEC.Nanoeye.NanoImage.SettingScanner)baseSetting.Clone();

            double vRatio = ss.ImageHeight / 240.0d;
            double hRatio = ss.ImageWidth / 320.0d;

            int newheight = (int)(ss.FrameHeight / vRatio);
            newheight += 4 - (newheight % 4);	// 4의 배수에 맞추기
            int newwidth = (int)(ss.FrameWidth / hRatio);
            newwidth += 4 - (newwidth % 4);	// 4의 배수에 맞추기.

            ss.FrameHeight = newheight;
            ss.FrameWidth = newwidth;

            ss.ImageHeight = 240;
            ss.ImageWidth = 320;

            ss.ImageLeft = (int)(ss.ImageLeft / hRatio);
            ss.ImageTop = (int)(ss.ImageTop / vRatio);

            ss.Name = "AutoVideo";

            return ss;
        }
    }
}
