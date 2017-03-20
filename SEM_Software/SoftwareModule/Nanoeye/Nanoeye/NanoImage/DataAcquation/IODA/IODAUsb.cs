using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.Nanoeye.NanoImage.DataAcquation.IODA
{
    internal class IODAUsb : IDAQ
    {
        #region Property & Variables
        private string iodaDev = "";

        protected bool _Enable = false;
        public bool Enable
        {
            get { return (IODAUSB_API.CaptureAioStatus == IODAUSB_API.CaptureStateType.Run); }
        }

        private bool _DualEnable = false;
        public bool DualEnable
        {
            get { return _DualEnable; }
            set { _DualEnable = value; }
        }

        private SSioda _SettingsRunning = null;
        public SettingScanner[] SettingsRunning
        {
            get { return new SettingScanner[] { _SettingsRunning }; }
        }

        private SSioda _SettingsReady = null;
        public SettingScanner[] SettingsReady
        {
            get { return new SettingScanner[] { _SettingsReady }; }
        }

        /// <summary>
        /// 보드에 패턴이 있으면 0~7
        /// 아니면 -1
        /// remove 하지 말것. 다른 패턴으로 왔다 갔다 할 수 있으므로, patternHit를 이용 해야 함.
        /// </summary>
        private Dictionary<string, int> patternId = new Dictionary<string, int>();
        /// <summary>
        /// 패턴의 히트 수. board상에서 패턴을 삭제해야 할 경우 삭제할 패턴을 결정 한다.
        /// 패턴의 크기에 따라 가중치를 둘 것!!!
        /// </summary>
        private Dictionary<string, int> patternHit = new Dictionary<string, int>();
        /// <summary>
        /// 같은 이름의 주사 설정이라도 설정이 다를 수 있다 -> AutoVideo!!!
        /// 따라서 실제 같은 주사 설정인지 검증을 해야 한다.
        /// </summary>
        private Dictionary<string, SSioda> patternData = new Dictionary<string, SSioda>();

        System.Threading.AutoResetEvent readyAre = new System.Threading.AutoResetEvent(true);
        #endregion

        #region 생성자
        private IODAUsb() { }

        public IODAUsb(string dev)
        {
            iodaDev = dev;

            dev = dev.Replace("Dev", "");

            int ch;

            try
            {
                ch = int.Parse(dev);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fail to parse dev ID.", "IODAUsb");
                SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterDebug(ex);

                throw new ArgumentException("Invalid device ID", ex);
            }

            IODAUSB_API.Open();

            Trace.WriteLine(string.Format("IODAUSB Created. ID - {0}, FPGA - {1}, Library - {2}", dev, IODAUSB_API.VersionFpga, IODAUSB_API.VersionLibrary), "IODAUsb");

            //IODAUSB_API.AIsource = IODAUSB_API.AIsourceType.AD_Input;
        }

        public static string[] SearchIODAUSB()
        {
            if (IODAUSB_API.IsDeviceExist)
            {
                return new string[] { "Dev0" };
            }
            else
            {
                return new string[] { };
            }
        }
        
        private int aiChannel = 0;
        public int AiChannel
        {
            get { return aiChannel;}
            set { aiChannel = value; }
        
        }

        private bool _Revers = false;
        public bool Revers
        {
            get { return _Revers; }
            set { _Revers = value; }

        }


        public void ScanMode(int detector)
        {
            aiChannel = detector;
            //return aiChannel;
        }


        ~IODAUsb()
        {
            Dispose();
        }

        public void Dispose()
        {
            IODAUSB_API.Abort();
            IODAUSB_API.Close();
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return "IODAUSB";
        }

        #endregion

        #region 동작 상태 제어
        public void Ready(SettingScanner[] set)
        {
            if (set.Length != 1) { throw new ArgumentException("Multi scan is not supported"); }

            readyAre.WaitOne();

            SSioda ioda = new SSioda(set[0]);

            // 기존에 패턴이 다운로드 되어 있는지 확인 한다.
            if (patternId.ContainsKey(ioda.Name))
            {
                int id = patternId[ioda.Name];
                if (id < 0) { ReadyKeyempty(ioda); }	// 기존에 실행한 적은 있으나 패턴이 삭제된 경우
                else { ReadyKeycontains(ioda); }		// 기존에 패턴이 다운로드 되어 있는 경우
            }
            else
            {
                ReadyKeyempty(ioda);					// 새로운 패턴
            }

            _SettingsReady = ioda;

            readyAre.Set();
        }

        struct PatternMemoryStruct
        {
            public string Name;
            public int Id;
            public long Addr;
            public int Hit;
            /// <summary>
            /// byte 단위 이다.
            /// </summary>
            public long Length;
            public PatternMemoryStruct(string name, int id, int addr, int hit, long length)
            {
                Id = id;
                Addr = addr;
                Hit = hit;
                Length = length;
                Name = name;
            }
        }

        /// <summary>
        /// board상에 패턴이 없는 경우
        /// </summary>
        /// <param name="ioda"></param>
        private void ReadyKeyempty(SSioda ioda)
        {
            // 기존에 패턴을 사용한 적이 있는가?
            bool isContains = patternId.ContainsKey(ioda.Name);

            // 기존에 사용한 적이 있다면, 그때와 설정이 같은가?
            bool isSame = false;

            if (isContains)
            {
                isSame = ComparerSSioda(ioda, patternData[ioda.Name]);
            }

            ScanGenerator sg = new ScanGenerator();
            sg.Divid = 1;
            sg.Device = 1;
            sg.FrameSize = new System.Drawing.Size(ioda.FrameWidth, ioda.FrameHeight);
            sg.LineAverage = ioda.LineAverage;
            sg.RatioX = ioda.RatioX;
            sg.RatioY = ioda.RatioY;
            sg.ShiftX = ioda.ShiftX;
            sg.ShiftY = ioda.ShiftY;

            //#if DEBUG
            //            if (isSame)
            //            {
            //                short[] prePattern = patternData[ioda.Name].PatternData;
            //                short[] newPattern = AlignPatternData(sg.Generate());

            //                if (prePattern.Length != newPattern.Length)
            //                {
            //                    throw new Exception();
            //                }

            //                for (int i = 0; i < prePattern.Length; i++)
            //                {
            //                    if (prePattern[i] != newPattern[i])
            //                    {
            //                        throw new Exception();
            //                    }
            //                }

            //                ioda.PatternData = newPattern;
            //            }
            //            else
            //            {
            //                ioda.PatternData = AlignPatternData(sg.Generate());
            //            }
            //#else
            ioda.PatternData = AlignPatternData(sg.Generate());

            //WriteFilePatternData(ioda);

            //#endif
            ioda.IsDownloaded = false;



            int patternSize = ioda.FrameWidth * ioda.FrameHeight * 2 * 2;	// 2byte, 2channel

            int id;
            long addr;
            List<PatternMemoryStruct> pmsList = GeneratePMSlist();
            GetPatternArea(patternSize, pmsList, out id, out addr);
            ioda.PatternAddr = addr;
            ioda.PatternId = id;



            if (isSame)
            {
                // 기존 패턴과 동일한 경우
                patternId[ioda.Name] = ioda.PatternId;
                patternHit[ioda.Name]++;
                patternData[ioda.Name] = ioda;
            }
            else
            {
                if (isContains)
                {
                    // 사용한 적은 있지만 기존 패턴과 다른 경우
                    patternId[ioda.Name] = ioda.PatternId;
                    patternHit[ioda.Name] = 0;
                    patternData[ioda.Name] = ioda;
                }
                else
                {
                    // 처음 사용 하는 경우
                    patternId.Add(ioda.Name, ioda.PatternId);
                    patternHit.Add(ioda.Name, 0);
                    patternData.Add(ioda.Name, ioda);
                }
            }
        }

        [Conditional("DEBUG")]
        private void WriteFilePatternData(SSioda ioda)
        {
            string fileName = System.Windows.Forms.Application.CommonAppDataPath + ioda.Name;

            if (System.IO.File.Exists(fileName + ".txt"))
            {
                int cnt = 0;
                while (System.IO.File.Exists(fileName + cnt.ToString() + ".txt"))
                {
                    cnt++;
                }
                fileName = fileName + cnt.ToString();
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName + ".txt");
            sw.WriteLine("Clock            : " + ioda.AiClock.ToString());
            sw.WriteLine("AO Max           : " + ioda.AoMaximum.ToString());
            sw.WriteLine("AIO delay        : " + ioda.PropergationDelay.ToString());
            sw.WriteLine("Pattern Information");
            sw.WriteLine("X Offset         : " + ioda.ShiftX.ToString());
            sw.WriteLine("Y Offset         : " + ioda.ShiftY.ToString());
            sw.WriteLine("Ratio Horizontal : " + ioda.RatioX.ToString());
            sw.WriteLine("Ratio Vertical   : " + ioda.RatioY.ToString());
            sw.WriteLine("Frame Width      : " + ioda.FrameWidth.ToString());
            sw.WriteLine("Frame Height     : " + ioda.FrameHeight.ToString());

            int length = ioda.PatternData.Length;

            for (int i = 0; i < length; i += 2)
            {
                sw.WriteLine(string.Format("{0:D6}\t{1:D6}", ioda.PatternData[i], ioda.PatternData[i + 1]));
            }
            sw.Close();
        }

        /// <summary>
        /// ScanGenerator에서 생선된 패턴을 IODAUSB에서 사용 할 수 있는 패턴 형태로 정렬 시킨다.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private unsafe short[] AlignPatternData(short[,] data)
        {

            //System.Drawing.Bitmap bm = new System.Drawing.Bitmap(1280, 960);
            //for (int l = 0; l < 320 * 240; l++)
            //{
            //    int pntX = (data[0, l] - short.MinValue) * 1280 / (short.MaxValue - short.MinValue);
            //    int pntY = (data[1, l] - short.MinValue) * 960 / (short.MaxValue - short.MinValue);

            //    try
            //    {

            //        bm.SetPixel(pntX, pntY, System.Drawing.Color.Red);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //bm.Save(@"d:\pattern.bmp");

            int length = data.Length / 2;
            short[] result = new short[length * 2];
            

            fixed (short* pntData = data, pntResult = result)
            {
                short* pXdata = (pntData);
                short* pYdata = (pntData + length);

                short* pResult = pntResult;

                for (int i = 0; i < length; i++)
                {
                    *pResult++ = *pXdata++;
                    *pResult++ = *pYdata++;
                }
            }

            //#if DEBUG
            //int resultIndex=0;
            //for (int i = 0; i < length; i++)
            //{
            //    if (data[0, i] != result[resultIndex++])
            //    {
            //        throw new Exception();
            //    }
            //    if (data[1, i] != result[resultIndex++])
            //    {
            //        throw new Exception();
            //    }
            //}
            //#endif
            return result;
        }

        /// <summary>
        /// 설정 가능한 패턴 위치를 결정 한다.
        /// </summary>
        /// <param name="patternSize">byte 단위의 패턴 크기</param>
        /// <param name="pmsList"></param>
        /// <param name="id"></param>
        /// <param name="addr"></param>
        private void GetPatternArea(int patternSize, List<PatternMemoryStruct> pmsList, out int id, out long addr)
        {
            #region ID 선택
            id = -1;

            bool empty = true;

            for (int i = 0; i < 8; i++)
            {
                empty = true;
                // ID 검사
                foreach (PatternMemoryStruct pID in pmsList)
                {
                    if (pID.Id == i)
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty)
                {
                    id = i;
                    break;
                }
            }

            // 비어 있는 id가 없다면.
            if (id == -1)
            {
                int nId = -1;
                int hit = int.MaxValue;
                string name = "";

                PatternMemoryStruct removePms = new PatternMemoryStruct();

                foreach (PatternMemoryStruct pt in pmsList)
                {
                    if (pt.Hit < hit)
                    {
                        hit = pt.Hit;
                        nId = pt.Id;
                        name = pt.Name;
                        removePms = pt;
                    }
                }

                if (nId == -1)
                {
                    throw new Exception("발생 해서는 안됨...");
                }

                // 기존 ID 삭제
                patternId[name] = -1;
                patternData[name].PatternId = -1;

                id = nId;

                pmsList.Remove(removePms);
            }
            #endregion

            #region Address 선택
            int max = 0x2000000;

            long start = 0;
            long end = patternSize;

            // 삭제 할것 선택
            while (pmsList.Count > 0)
            {
                start = 0;
                end = patternSize;
                // 시작점과 끝점 변경
                foreach (PatternMemoryStruct pmsArea in pmsList)
                {

                    empty = true;
                    // 영역 검사
                    foreach (PatternMemoryStruct pms in pmsList)
                    {
                        long pmsEnd = pms.Addr + pms.Length;

                        // 겹치는 영역이 존재하는지 체크.
                        if (((pms.Addr >= start) && (pms.Addr < end)) || ((pmsEnd > start) && (pmsEnd <= end)))
                        {
                            empty = false;
                            break;
                        }

                    }// 영역 검사

                    if (empty)
                    {
                        addr = start;

                        Debug.WriteLine("New position Pattern  1. ID : " + id.ToString("X") + ", Addr : " + addr.ToString("X") + ", Length : " + patternSize.ToString("X"), "IODAUSB");
                        return;
                    }

                    start = pmsArea.Addr + pmsArea.Length;

                    start += 32 - (start % 32);// 32byte 단위로 정렬. API 스펙임.

                    end = start + patternSize;
                    if (end > max) { break; }	// 범위 초과
                }// 시작점과 끝점 변경

                if (end < max)
                {
                    addr = start;
                    Debug.WriteLine("New position Pattern  2. ID : " + id.ToString("X") + ", Addr : " + addr.ToString("X") + ", Length : " + patternSize.ToString("X"), "IODAUSB");
                    return;
                }

                PatternMemoryStruct remove = new PatternMemoryStruct();

                int hitCnt = int.MaxValue;
                foreach (PatternMemoryStruct pmsRemove in pmsList)
                {
                    if (pmsRemove.Hit < hitCnt)
                    {
                        hitCnt = pmsRemove.Hit;
                        remove = pmsRemove;
                    }

                }

                string nameRemove = remove.Name;
                patternId[nameRemove] = -1;
                patternData[nameRemove].PatternId = -1;
                pmsList.Remove(remove);

            } // 삭제 할것 선택

            addr = start;
            #endregion

            Debug.WriteLine("New position Pattern  0. ID : " + id.ToString("X") + ", Addr : " + addr.ToString("X") + ", Length : " + patternSize.ToString("X"), "IODAUSB");
        }

        private List<PatternMemoryStruct> GeneratePMSlist()
        {
            List<PatternMemoryStruct> pmsList = new List<PatternMemoryStruct>();

            foreach (KeyValuePair<string, int> kvp in patternId)
            {
                if (kvp.Value >= 0)
                {
                    PatternMemoryStruct pms = new PatternMemoryStruct();

                    string name = kvp.Key;
                    pms.Name = name;
                    pms.Id = kvp.Value;
                    pms.Hit = patternHit[name];

                    SSioda ss = patternData[name];
                    pms.Addr = ss.PatternAddr;
                    pms.Length = ss.FrameWidth * ss.FrameHeight * 2 * 2;



                    pmsList.Add(pms);
                }
            }

            return pmsList;
        }

        /// <summary>
        /// board 상에 패턴이 있는 경우
        /// </summary>
        /// <param name="ioda"></param>
        private void ReadyKeycontains(SSioda ioda)
        {
            SSioda ori = patternData[ioda.Name];

            if (!ComparerSSioda(ioda, ori))
            {
                ReadyKeyempty(ioda);
                return;
            }

            // 패턴 이외의 값이 다를 수 있다.
            ioda.PatternAddr = ori.PatternAddr;
            ioda.PatternData = ori.PatternData;
            ioda.PatternId = ori.PatternId;
            ioda.IsDownloaded = true;

            patternData[ioda.Name] = ioda;

            // 패턴을 재사용 하였다.
            int hitCount = patternHit[ioda.Name];

            // integer 타입의 overflow가 발생 할까???
            if (hitCount < 10000) { patternHit[ioda.Name]++; }

            Debug.WriteLine("Pre position Pattern 1. ID : " + ioda.PatternId.ToString() + ", Addr : " + ioda.PatternAddr.ToString("X") + ", Hit : " + patternHit[ioda.Name].ToString(), "IODAUSB");
        }

        /// <summary>
        /// SSioda 타입의 두 객체가 AO 패턴이 같은지 비교 한다.(설정 값으로...)
        /// </summary>
        /// <param name="ioda"></param>
        /// <param name="ori"></param>
        /// <returns></returns>
        private bool ComparerSSioda(SSioda ioda, SSioda ori)
        {
            if (ioda.FrameHeight != ori.FrameHeight) { return false; }
            if (ioda.FrameWidth != ori.FrameWidth) { return false; }
            if (ioda.LineAverage != ori.LineAverage) { return false; }
            if (ioda.RatioX != ori.RatioX) { return false; }
            if (ioda.RatioY != ori.RatioY) { return false; }
            if (ioda.ShiftX != ori.ShiftX) { return false; }
            if (ioda.ShiftY != ori.ShiftY) { return false; }
            if (ioda.SampleComposite != ori.SampleComposite) { return false; }

            return true;
        }

        /// <summary>
        /// DAQ의 동작을 SettingReady의 설정으로 변화 시킨다.
        /// 이때 설정 값에 대한 특별한 검증은 진행 하지 않는다. 따라서 Change()전에 ValidateSetting(...)을 진행 하여야 한다.
        /// </summary>
        public void Change()
        {
            readyAre.WaitOne();

            if (_SettingsReady == null) { throw new ArgumentException("Setting is null."); }


            IODAUSB_API.CaptureStop();
            IODAUSB_API.Abort();	// DIO는 걱정 하지 않아도 됨... DIO는 이 함수에서만 사용 할 계획 -> Filter 선택

            if (_SettingsReady.IsDownloaded)
            {
                IODAUSB_API.PatternId = _SettingsReady.PatternId;
            }
            else
            {
                IODAUSB_API.PatternMemoryInfoSet(_SettingsReady.PatternId, _SettingsReady.PatternAddr, _SettingsReady.FrameHeight * _SettingsReady.FrameWidth);
                IODAUSB_API.PatternId = _SettingsReady.PatternId;
                IODAUSB_API.PatternDataWrite(_SettingsReady.PatternData);
            }

            //IODAUSB_API.PatternMemoryInfoSet(0, 0, _SettingsReady.FrameHeight * _SettingsReady.FrameWidth);
            //IODAUSB_API.PatternId = 0;
            //IODAUSB_API.PatternDataWrite(_SettingsReady.PatternData);

            IODAUSB_API.SamplingFrequence = (long)(_SettingsReady.AiClock);

            switch ((int)_SettingsReady.AiMaximum)
            {
                case 10:
                    IODAUSB_API.AIgainSet(0, IODAUSB_API.AIgainType.x0d4_10V);
                    break;
                case 5:
                    IODAUSB_API.AIgainSet(0, IODAUSB_API.AIgainType.x0d8_5V);
                    break;
                case 2:
                    IODAUSB_API.AIgainSet(0, IODAUSB_API.AIgainType.x2_2V);
                    break;
                case 1:
                    IODAUSB_API.AIgainSet(0, IODAUSB_API.AIgainType.x4_1V);
                    break;
                default:
                    throw new ArgumentException("Invalid AiMax.");
            }

            //if(

            IODAUSB_API.AIchannel = aiChannel;
            IODAUSB_API.AIOdelay = (int)(_SettingsReady.AiClock * _SettingsReady.PropergationDelay / Math.Pow(10, 6));
            IODAUSB_API.AIOratio = _SettingsReady.SampleComposite;
            IODAUSB_API.AIsource = IODAUSB_API.AIsourceType.AD_Input;

            IODAUSB_API.DigitalOutputFlag = (byte)_SettingsReady.AiChannel;

            // 검증.
            ValidateIODAUSB();

            IODAUSB_API.AIbufferClear();
            IODAUSB_API.CaptureStart();

            _SettingsRunning = _SettingsReady;
            _SettingsReady = null;

            readyAre.Set();
        }


        /// <summary>
        /// DAQ의 동작을 중지 시킨다.
        /// </summary>
        public void Stop()
        {
            IODAUSB_API.CaptureStop();
        }
        #endregion

        #region 유효성 검증
        public void ValidateSetting(SettingScanner setting)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            System.Text.StringBuilder arg = new System.Text.StringBuilder();

            if (setting.AiChannel >= 8)
            {
                setting.AiChannel = 7;
                msg.AppendLine("AiChannel is large then 7.");
                arg.AppendLine("AiChannel");
            }
            if (setting.AiClock > 2500000)
            {
                setting.AiClock = 2500000;
                msg.AppendLine("AiClock is large then 2500000.");
                arg.AppendLine("AiClock");
            }
            if ((setting.AiMaximum != 10) && (setting.AiMaximum != 5) && (setting.AiMaximum != 2) && (setting.AiMaximum != 1))
            {
                setting.AiMaximum = 10;
                msg.AppendLine("AiMaximum is invalid number.");
                arg.AppendLine("AiMaximum");
            }

            if (setting.AiMaximum != (setting.AiMinimum * -1))
            {
                setting.AiMinimum = setting.AiMaximum * -1;
                msg.AppendLine("AiMinimum is invalid number.");
                arg.AppendLine("AiMinimum");
            }

            if (setting.AoClock > 2500000)
            //if (setting.AoClock != setting.AiClock)
            {
                setting.AoClock = setting.AiClock;
                msg.AppendLine("AoClock is deference with AiClock.");
                arg.AppendLine("AoClock");
            }

            if ((setting.AoMaximum != 10))
            {
                setting.AoMaximum = 10;
                msg.AppendLine("AoMaximum is invalid number.");
                arg.AppendLine("AoMaximum");
            }

            if (setting.AoMaximum != (setting.AoMinimum * -1))
            {
                setting.AoMinimum = setting.AoMaximum * -1;
                msg.AppendLine("AoMinimum is invalid number.");
                arg.AppendLine("AoMinimum");
            }

            if ((setting.AreaShiftX > 1) || (setting.AreaShiftX < -1))
            {
                setting.AreaShiftX = 0;
                msg.AppendLine("AreaShiftX must be bettwen from -1 to 1.");
                arg.AppendLine("AreaShiftX");
            }

            if ((setting.AreaShiftY > 1) || (setting.AreaShiftY < -1))
            {
                setting.AreaShiftY = 0;
                msg.AppendLine("AreaShiftY must be bettwen from -1 to 1.");
                arg.AppendLine("AreaShiftY");
            }

            if ((setting.RatioX < 0.1) || (setting.RatioX > 1))
            {
                setting.RatioX = 1;
                msg.AppendLine("RatioX must be bettwen from 0.1 to 1.");
                arg.AppendLine("RatioX");
            }

            if ((setting.RatioY < 0.1) || (setting.RatioY > 1))
            {
                setting.RatioY = 1;
                msg.AppendLine("RatioY must be bettwen from 0.1 to 1.");
                arg.AppendLine("RatioY");
            }

            if ((Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
            {
                setting.ShiftX = 0;
                msg.AppendLine("ShiftX is invalid number.");
                arg.AppendLine("ShiftX");
            }

            if ((Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
            {
                setting.ShiftY = 0;
                msg.AppendLine("ShiftY is invalid number.");
                arg.AppendLine("ShiftY");
            }

            if ((Math.Abs(setting.AreaShiftX) + Math.Abs(setting.ShiftX) + Math.Abs(setting.RatioX)) > 1)
            {
                setting.AreaShiftX = 0;
                msg.AppendLine("AreaShiftX is invalid number.");
                arg.AppendLine("AreaShiftX");
            }

            if ((Math.Abs(setting.AreaShiftY) + Math.Abs(setting.ShiftY) + Math.Abs(setting.RatioY)) > 1)
            {
                setting.AreaShiftY = 0;
                msg.AppendLine("AreaShiftY is invalid number.");
                arg.AppendLine("AreaShiftY");
            }

            if ((setting.SampleComposite < 1) || (setting.SampleComposite > 255))
            {
                setting.SampleComposite = 1;
                msg.AppendLine("SampleComposite is not supported.");
                arg.AppendLine("SampleComposite");
            }

            if (msg.Length > 0)
            {
                throw new ArgumentException(msg.ToString(), arg.ToString());
            }
        }

        public void ValidateSequenceRunable(SettingScanner[] setting)
        {
            if (setting.Length > 1)
            {
                throw new NotImplementedException("This will be implemented some days.");
            }
        }

        [Conditional("DEBUG")]
        private void ValidateIODAUSB()
        {
            long selectedAddr;
            long selectedLength;

            long len = _SettingsReady.FrameHeight * _SettingsReady.FrameWidth;

            int selectedPatternId = IODAUSB_API.PatternId;
            IODAUSB_API.PatternMemoryInfoGet(selectedPatternId, out selectedAddr, out selectedLength);
            Debug.WriteLine(string.Format("ID   - Same {2}, Ready {0}, Selected {1}", _SettingsReady.PatternId, selectedPatternId, _SettingsReady.PatternId == selectedPatternId), "IODAUSB");


            selectedAddr = IODAUSB_API.PatternAddr;
            selectedLength = IODAUSB_API.PatternLength;
            Debug.WriteLine(string.Format("Get Addr - Same {2}, Ready {0:X}, Selected {1:X}", _SettingsReady.PatternAddr, selectedAddr, _SettingsReady.PatternAddr == selectedAddr), "IODAUSB");
            Debug.WriteLine(string.Format("Get Len  - Same {2}, Ready {0:X}, Selected {1:X}", len, selectedLength, len == selectedLength), "IODAUSB");

            Debug.WriteLine(string.Format("Freq - Same {2}, Ready {0}, Selected {1}", _SettingsReady.AiClock, IODAUSB_API.SamplingFrequence, _SettingsReady.AiClock == IODAUSB_API.SamplingFrequence), "IODAUSB");

            Debug.WriteLine(string.Format("Inpu - Same {2}, Ready {0}, Selected {1}", IODAUSB_API.AIsourceType.AD_Input, IODAUSB_API.AIsource, IODAUSB_API.AIsourceType.AD_Input == IODAUSB_API.AIsource), "IODAUSB");

            IODAUSB_API.AIgainType ait = IODAUSB_API.AIgainGet(0);
            Debug.WriteLine(string.Format("Gain - Same {2}, Ready {0}, Selected {1}", _SettingsReady.AiMaximum, ait, "???"), "IODAUSB");

            Debug.WriteLine(string.Format("Chan - Same {2}, Ready {0}, Selected {1}", 0, IODAUSB_API.AIchannel, IODAUSB_API.AIchannel == 0), "IODAUSB");

            int delay = (int)(_SettingsReady.AiClock * _SettingsReady.PropergationDelay / Math.Pow(10, 6));
            Debug.WriteLine(string.Format("Dela - Same {2}, Ready {0}, Selected {1}", delay, IODAUSB_API.AIOdelay, delay == IODAUSB_API.AIOdelay), "IODAUSB");

            int aioRatio = (int)IODAUSB_API.AIOratio;
            Debug.WriteLine(string.Format("Rati - Same {2}, Ready {0}, Selected {1}", _SettingsReady.SampleComposite, aioRatio, _SettingsReady.SampleComposite == aioRatio), "IODAUSB");

            int doValue = (int)IODAUSB_API.DigitalOutputFlag;
            Debug.WriteLine(string.Format("DO(Channel) - Same {2}, Ready {0}, Selected {1}", _SettingsReady.AiChannel, doValue, (byte)_SettingsReady.AiChannel == doValue), "IODAUSB");
        }

        #endregion

        #region Read
        public int ReadAvailables
        {
            get { return IODAUSB_API.AIbufferCount; }
        }

        //int readCout = 10000;

        public unsafe short[,] Read(int samples)
        {
            short[] datas = IODAUSB_API.AIbufferRead(samples);

            short[,] result = new short[1, datas.Length];

            //// fixed 보다 이것이 더 빠르다....
            //for (int i = 0; i < datas.Length; i++)
            //{
            //    if (readCout > 0)
            //    {
            //        Trace.WriteLine(datas[i].ToString() + " - " + readCout.ToString(), "Sample");
            //        readCout--;
            //    }
            //    result[0, i] = datas[i];
            //}

            if (_Revers)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    result[0, i] = (short)((-1) * datas[i]);
                }
            }
            else
            {
                Buffer.BlockCopy(datas, 0, result, 0, datas.Length * 2);
            }

            




            //fixed (short* pntData = datas, pntResult = result)
            //{

            //    short* pData = pntData;
            //    short* pResult = pntResult;

            //    for (int i = 0; i < datas.Length; i++)
            //    {
            //        *pResult++ = *pData++;
            //    }
            //}

            //Array.Copy(datas, result, datas.Length);
            return result;
        }
        #endregion

        public void ShowInformation(System.Windows.Forms.IWin32Window owner)
        {
            if ((Information.Default == null) || (Information.Default.IsDisposed))
            {
                Information.Default = new Information();
                Information.Default.Show(owner);
            }
            else
            {
                Information.Default.Location = new System.Drawing.Point(0, 0);
                Information.Default.WindowState = System.Windows.Forms.FormWindowState.Normal;
                Information.Default.Activate();
            }
        }

        public void OnePoint(double horizontal, double vertical)
        {
            Stop();

            IODAUSB_API.AOvalueSet(0, (short)(horizontal * short.MaxValue));
            IODAUSB_API.AOvalueSet(1, (short)(vertical * short.MaxValue));
        }

        public void Reset()
        {
            IODAUSB_API.Reset();
        }
    }

    internal class SSioda : SettingScanner
    {
        public SSioda(SettingScanner ss)
        {
            this.AiChannel = ss.AiChannel;
            this.AiClock = ss.AiClock;
            this.AiDifferential = ss.AiDifferential;
            this.AiMaximum = ss.AiMaximum;
            this.AiMinimum = ss.AiMinimum;
            this.AoClock = ss.AoClock;
            this.AoMaximum = ss.AoMaximum;
            this.AoMinimum = ss.AoMinimum;
            this.AreaShiftX = ss.AreaShiftX;
            this.AreaShiftY = ss.AreaShiftY;
            this.AverageLevel = ss.AverageLevel;
            this.BlurLevel = ss.BlurLevel;
            this.FrameHeight = ss.FrameHeight;
            this.FrameWidth = ss.FrameWidth;
            this.ImageHeight = ss.ImageHeight;
            this.ImageLeft = ss.ImageLeft;
            this.ImageTop = ss.ImageTop;
            this.ImageWidth = ss.ImageWidth;
            this.LineAverage = ss.LineAverage;
            this.Name = ss.Name;
            this.PaintHeight = ss.PaintHeight;
            this.PaintWidth = ss.PaintWidth;
            this.PaintX = ss.PaintX;
            this.PaintY = ss.PaintY;
            this.PropergationDelay = ss.PropergationDelay;
            this.RatioX = ss.RatioX;
            this.RatioY = ss.RatioY;
            this.SampleComposite = ss.SampleComposite;
            this.ShiftX = ss.ShiftX;
            this.ShiftY = ss.ShiftY;
        }

        private bool _IsDownloaded = false;
        public bool IsDownloaded
        {
            get { return _IsDownloaded; }
            set { _IsDownloaded = value; }
        }

        private long _PatternAddr = -1;
        public long PatternAddr
        {
            get { return _PatternAddr; }
            set { _PatternAddr = value; }
        }

        private int _PatternId = -1;
        public int PatternId
        {
            get { return _PatternId; }
            set { _PatternId = value; }
        }

        private short[] _PatternData = null;
        public short[] PatternData
        {
            get { return _PatternData; }
            set { _PatternData = value; }
        }
    }
}
