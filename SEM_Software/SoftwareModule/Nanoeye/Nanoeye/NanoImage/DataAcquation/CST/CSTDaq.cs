using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace SEC.Nanoeye.NanoImage.DataAcquation.CSTDaq
{
    class CSTDaq : IDAQ
    {
        delegate void receviesDataDelegate();

        public CSTDaq(string device)
        {
            daqDevice = device;

            DeviceInit();
        }

        public static string[] SearchCSTDaq()
        {
            List<string> deviceList = new List<string>();

            UInt32 ftdDeviceCount = 0;

            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            FTDI CSTDaq = new FTDI();


            ftStatus = CSTDaq.GetNumberOfDevices(ref ftdDeviceCount);

            if (ftdDeviceCount == 0)
            {
                return null;
            }
            else
            {
                deviceList.Add("CSTDaq");
            }

            return deviceList.ToArray();
        }

        protected string daqDevice;

        protected bool _Enable = false;
        public bool Enable
        {
            get { return _Enable; }
        }

        private SettingScanner[] _SettingsRunning = null;
        public SettingScanner[] SettingsRunning
        {
            get { return _SettingsRunning; }
        }

        private SettingScanner[] _SettingsReady = null;
        public SettingScanner[] SettingsReady
        {
            get { return _SettingsReady; }
        }

        private SettingScanner _setR = null;
        public SettingScanner SetR
        {
            get { return _setR; }
        }

        private bool _Revers = false;
        public bool Revers
        {
            get { return _Revers; }
            set { _Revers = value; }

        }

        private bool _DualEnable = false;
        public bool DualEnable
        {
            get { return _DualEnable; }
            set { _DualEnable = value; }
        }

        private int aichnnel = 0;
        public int AiChannel
        {
            get { return aichnnel; }
            set { aichnnel = value; }
        }

        private int speed = 8;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private FTDI.FT_STATUS CSTDaqState;
        private FTDI CSTDaqDevice;

        private string DataString = null;

        int bufferCount = 0;

        bool isSerialStart = false;
        int isChangeFlag = 1;
        int isStopFlag = 1;

        public void DeviceInit()
        {
            CSTDaqState = FTDI.FT_STATUS.FT_OK;
            CSTDaqDevice = new FTDI();

            UInt32 ftdDeviceCount = 0;
            CSTDaqState = CSTDaqDevice.GetNumberOfDevices(ref ftdDeviceCount);

            //Trace.WriteLine("Device Count : " + ftdDeviceCount.ToString());

           FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdDeviceCount];

            CSTDaqState = CSTDaqDevice.GetDeviceList(ftdiDeviceList);

            int listSelectCount = 0;
            for (int i = 0; i < ftdDeviceCount; i++)
            {
                if (ftdiDeviceList[i].Type == FTDI.FT_DEVICE.FT_DEVICE_232H)
                {
                    listSelectCount = i;
                }
                 
            }
            

            CSTDaqState = CSTDaqDevice.OpenBySerialNumber(ftdiDeviceList[listSelectCount].SerialNumber);

           

            //CSTDaqState = CSTDaqDevice.SetBaudRate(9600);
            CSTDaqState = CSTDaqDevice.SetBaudRate(57600);

            CSTDaqState = CSTDaqDevice.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);

            CSTDaqState = CSTDaqDevice.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13);

            CSTDaqState = CSTDaqDevice.SetTimeouts(5000, 0);
            CSTDaqState = CSTDaqDevice.ResetDevice();

        }

        
        private void BufferClear()
        {
            UInt32 numBytesAvailable = 0;
            DateTime timeout = DateTime.Now;

            CSTDaqState = CSTDaqDevice.SetResetPipeRetryCount(1024);

            #region [ 일단 두자 ]
            while (isSerialStart)
            {
                try
                {
                    CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);
                   
                    if (CSTDaqState != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Console.WriteLine("Failed to get number of bytes available to read (error " + CSTDaqState.ToString() + ")");
                        return;
                    }

                    if (numBytesAvailable > 0)
                    {

                        Trace.WriteLine("set bufferclear : " + numBytesAvailable.ToString());

                        


                        UInt32 numBytesRead = 0;
                        byte[] readBuffer = new byte[numBytesAvailable];
                        CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

                        timeout = DateTime.Now;
                    }

                    if (timeout.AddMilliseconds(100) < DateTime.Now)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
        }

        private bool Write(string dataBuffer)
        {
            UInt32 numBytesWritten = 0;
            CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);

            if (CSTDaqState != FTDI.FT_STATUS.FT_OK) return false;

            return true;

        }





        private int ReadDataCount = 0;

        int FFCount = -1;
        int startCount = 0;

        
        

        bool StartEnable = false;

        CSTQueue daqQueue = new CSTQueue();

        public void Ready(SettingScanner[] set)
        {

            isSerialStart = false;

            _setR = set[0];
            
            bufferCount = 0;

            if (DataString != null)
            {
                DataString = null;
            }

            //DataString += "{PSw=START/M:0/";
            DataString += "{PSw=START/M:";

            if (_DualEnable)
            {
                DataString += "1/";
            }
            else
            {
                DataString += "0/";
            }

            int channel = 0;

            if (_setR.AiChannel > 3)
            {
                channel = 1;
            }

            //DataString += _setR.AiChannel.ToString();
            DataString += channel.ToString();
            DataString += "/00/X:";
            DataString += string.Format("{0:000}", _setR.SampleComposite);
            DataString += "/";
            DataString += string.Format("{0:0000}", _setR.FrameWidth);
            DataString += "/0016";
             
            //DataString += string.Format("{0:0000}", _setR.ImageLeft);
            DataString += "/0046";
            //DataString += string.Format("{0:0000}", _setR.ImageWidth + _setR.ImageLeft);
            DataString += "/000/Y:000";

            //DataString += _setR.LineAverage.ToString();
            DataString += "/";
            DataString += string.Format("{0:0000}", _setR.FrameHeight);
            DataString += "/0016";
            //DataString += string.Format("{0:0000}", _setR.ImageTop);
            DataString += "/0046";
            //DataString += string.Format("{0:0000}", _setR.ImageHeight + _setR.ImageTop);
            DataString += "/255/DA:";

            DataString += string.Format("{0:00000}", _setR.RatioX * 10000);
            DataString += "/";

            if (_setR.ShiftX < 0)
            {
                DataString += string.Format("{0:0000}", _setR.ShiftX * 5000);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.ShiftX * 5000);
            }


            DataString += "/";
            DataString += string.Format("{0:00000}", _setR.RatioY * 10000);
            DataString += "/";

            if (_setR.ShiftY < 0)
            {
                DataString += string.Format("{0:0000}", _setR.ShiftY * 5000);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.ShiftY * 5000);
            }

            DataString += "/Z:";
            //DataString += string.Format("{0:0}", _setR.AiClock / 1000000);

            int aispeed = (int)_setR.AiClock / 1000000 * 4;
            
            //DataString += string.Format("{0:0}", _setR.AiClock / 1000000);
            DataString += aispeed.ToString();

            DataString += "/";
            DataString += string.Format("{0:00000}", _setR.SEGain);
            DataString += "/";

            if (_setR.SEOffset < 0)

            {
                DataString += string.Format("{0:0000}", _setR.SEOffset);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.SEOffset);
            }
            DataString += "/";

            DataString += string.Format("{0:00000}", _setR.BSEGain);

            DataString += "/";

            if (_setR.BSEOffset < 0)
            {
                DataString += string.Format("{0:0000}", _setR.BSEOffset);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.BSEOffset);
            }

            DataString += "}";

            Trace.WriteLine("Read Data : " + DataString.ToString());
            daqQueue.DataLength = _setR.FrameWidth * _setR.FrameHeight * _setR.SampleComposite * 2;
            daqQueue.PixelAvr = _setR.SampleComposite*2;   

            //DataString += "1/00/0000/0000/0000/0000/}";

        }


        public void Change()
        {
            isChangeFlag = 0;
            isSerialStart = false;

            Trace.WriteLine("set change11111 : " + isSerialStart.ToString());
            //BufferClear();
            daqQueue.Clear();
            daqQueue.Set();

            UInt32 numBytesWritten = 0;
            int endDataX = _setR.ImageLeft + _setR.ImageWidth;
            int endDataY = _setR.ImageTop + _setR.ImageHeight;
            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:" + speed.ToString() +  "/00/0000/0000/0000/0000/}";
            //string dataBuffer = "{PSw=START/M:0/" + _setR.AiChannel.ToString() + "/00/X:00"+ _setR.SampleComposite.ToString() + "/0" + _setR.FrameWidth.ToString() + "/0" + _setR.ImageLeft.ToString() + "/0" + endDataX.ToString() + "/000/Y:00" + _setR.LineAverage.ToString() + "/0" + _setR.ImageHeight.ToString() + "/00" + _setR.ImageTop.ToString() + "/0" + endDataY.ToString() + "/255/DA:" + _setR.RatioX.ToString() + "/" + "00000" + "/" + _setR.RatioY.ToString() + "/" + "00000" + "/Z:" + _setR.AiClock.ToString() + "/00/0000/0000/0000/0000/}";
            string dataBuffer = DataString;

            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:8/00/0000/0000/0000/0000/}";
            

            CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);

            Thread.Sleep(100);
            //BufferClear();

            Trace.WriteLine(dataBuffer);

            receviesDataDelegate readData = new receviesDataDelegate(DoRead);
            IAsyncResult iftar = readData.BeginInvoke(null, null);

            isSerialStart = true;
        }
        bool isPrunStart;
        private void DoRead()
        {
            Trace.WriteLine("set change2222 : " + isSerialStart.ToString());

            

            //DateTime timeout = DateTime.Now;

            BufferClear();
            daqQueue.Clear();
            





            int dual_flag = 1;
            if (_DualEnable)
            {
                dual_flag = 2;
            }
            else
            {
                dual_flag = 1;
            }

            int queue_MAX_limit_count = 0;
            queue_MAX_limit_count = SetR.FrameHeight* SetR.FrameWidth* _setR.SampleComposite * 2 * dual_flag;

            int readbuffcount = 0;
            UInt32 availablecount = 0;
            UInt32 returncount = 0;
            int aaacount = 0;
            int bbbcount = 0;
            int ccccount = 0;
            int dddcount = 0;


            int receiver_data_short_count = 0;
            UInt32 receiver_temp_count = 0;
            UInt32 numBytesAvailable = 0;

            UInt32 recvCount = 0;
            UInt32 past_recvCount = 0;
            int daqQueue_count = 0;
            bool recvCountEnable = false;

            if ((isStopFlag == 0) && (isChangeFlag == 1))
            {
                
            }
            else
            {
                daqQueue_count = daqQueue.Int16Count();
                if(daqQueue_count>=1)
                {
                    Trace.WriteLine("set bufferclear dorun1111111111111 : " + daqQueue_count.ToString());
                    BufferClear();
                    daqQueue.Clear();
                    daqQueue_count = 0;
                    numBytesAvailable = 0;
                }

                
                CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);
                if(numBytesAvailable>=1)
                {
                    Trace.WriteLine("set bufferclear dorun222222222222 : " + numBytesAvailable.ToString());
                    BufferClear();
                    daqQueue.Clear();
                    daqQueue_count = 0;
                    numBytesAvailable = 0;
                }

                this.Write("{PRUN}");
                isStopFlag = 1;
                isChangeFlag = 1;
            }

            //if (isChangeFlag == 0) { isChangeFlag = 1; isStopFlag = 1; }

            while (isSerialStart)
            {
                try
                {
                    numBytesAvailable = 0;
                    CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);
                    if (CSTDaqState != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Console.WriteLine("Failed to get number of bytes available to read (error " + CSTDaqState.ToString() + ")");
                        return;
                    }
                    else
                    {
                        if (numBytesAvailable > 0)
                        {
                            receiver_temp_count = receiver_temp_count + numBytesAvailable;
                            if (receiver_temp_count <= queue_MAX_limit_count) //정상적으로 덜어올 경우
                            {
                                UInt32 numBytesRead = 0;
                                byte[] readBuffer = new byte[numBytesAvailable];
                                CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

                               

                                bool ischecked = false;
                                uint returCount = 0;
                                recvCountEnable = true;

                                //recvCount += numBytesRead;
                                recvCount += numBytesRead;
                                readbuffcount += readBuffer.Length;
                                availablecount += numBytesAvailable;
                                returncount += numBytesRead;
                                daqQueue.testAddRange(readBuffer, isPrunStart, ref ischecked, ref returCount);
                                receiver_data_short_count = 0;

                                if (queue_MAX_limit_count == recvCount)
                                {
                                    recvCountEnable = false;
                                    daqQueue_count = daqQueue.Int16Count();
                                    if (daqQueue_count <= queue_MAX_limit_count)
                                    {
                                        aaacount++;
                                        Trace.WriteLine("DAQ Count 111 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString() + " / over:" + bbbcount.ToString() + " / short:" + ccccount.ToString() + " / queue:" + dddcount.ToString());
                                        readbuffcount = 0;
                                        availablecount = 0;
                                        returncount = 0;
                                        receiver_temp_count = 0;
                                        daqQueue.Set();

                                        past_recvCount = 0;
                                        recvCount = 0;
                                        this.Write("{PRUN}");
                                        isPrunStart = true;
                                        //timeout = DateTime.Now;
                                    }
                                }
                            }
                            else  //많이 덜어올 경우//
                            {
                                bbbcount++;

                                UInt32 over_data_count = 0;
                                over_data_count = receiver_temp_count - (UInt32)queue_MAX_limit_count;
                                UInt32 tempAvailable = 0;
                                tempAvailable = numBytesAvailable - over_data_count;
                                numBytesAvailable = tempAvailable;


                                
                                UInt32 numBytesRead = 0;
                                byte[] readBuffer = new byte[numBytesAvailable];
                                CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

                                bool ischecked = false;
                                uint returCount = 0;
                                recvCountEnable = true;

                                recvCount += numBytesRead;
                                readbuffcount += readBuffer.Length;
                                availablecount += numBytesAvailable;
                                returncount += numBytesRead;
                                daqQueue.testAddRange(readBuffer, isPrunStart, ref ischecked, ref returCount);
                                receiver_data_short_count = 0;

                                BufferClear();

                                if (queue_MAX_limit_count == recvCount)
                                {
                                    recvCountEnable = false;
                                    daqQueue_count = daqQueue.Int16Count();
                                    if (daqQueue_count <= queue_MAX_limit_count)
                                    {
                                        aaacount++;
                                        Trace.WriteLine("DAQ Count 222 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString() + " over=" + over_data_count.ToString() + " / queue:" + dddcount.ToString());
                                        readbuffcount = 0;
                                        availablecount = 0;
                                        returncount = 0;
                                        receiver_temp_count = 0;
                                        daqQueue.Set();

                                        past_recvCount = 0;
                                        recvCount = 0;
                                        this.Write("{PRUN}");
                                        isPrunStart = true;
                                        //timeout = DateTime.Now;
                                    }
                                }
                            }
                        }
                        else //같거나 , 부족하게 덜어올 경우//
                        {
                            if (recvCount == queue_MAX_limit_count) //같을 경우, Queue에 데이터 개수가 많을 경우 가져갈때 까지 대기//
                            {
                                receiver_data_short_count = 0;

                                recvCountEnable = false;
                                daqQueue_count = daqQueue.Int16Count();
                                if (daqQueue_count <= queue_MAX_limit_count)
                                {
                                    dddcount++;
                                    Trace.WriteLine("DAQ Count 444 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString() + " / over:" + bbbcount.ToString() + " / short:" + ccccount.ToString() + " / queue:" + dddcount.ToString());
                                    readbuffcount = 0;
                                    availablecount = 0;
                                    returncount = 0;
                                    receiver_temp_count = 0;
                                    daqQueue.Set();

                                    past_recvCount = 0;
                                    recvCount = 0;
                                    this.Write("{PRUN}");
                                    isPrunStart = true;
                                    //timeout = DateTime.Now;
                                }
                            }
                            else if (recvCount >= 1) //일시정지가 아닐 경우//, 부족하게 덜어올 경우
                            {
                                if (past_recvCount == recvCount)
                                {
                                    receiver_data_short_count++;
                                    Thread.Sleep(1); //1ms delay
                                }
                                else if (past_recvCount < recvCount)
                                {
                                    receiver_data_short_count = 0;
                                }

                                past_recvCount = recvCount;
                            }
                            

                            if (receiver_data_short_count >= 1000) //약 1000msec// 
                            {
                                ccccount++;

                                int short_data_count = 0;
                                short_data_count = queue_MAX_limit_count - (int)receiver_temp_count;

                                UInt32 numBytesRead = (UInt32)short_data_count;
                                byte[] readBuffer = new byte[short_data_count];
                                int s = 0;
                                for (s=0; s< short_data_count; s++)
                                {
                                    readBuffer[s] = 0x00;
                                }



                                bool ischecked = false;
                                uint returCount = 0;
                                recvCountEnable = true;

                                recvCount += numBytesRead;
                                readbuffcount += readBuffer.Length;
                                availablecount += numBytesAvailable;
                                returncount += numBytesRead;
                                daqQueue.testAddRange(readBuffer, isPrunStart, ref ischecked, ref returCount);
                                receiver_data_short_count = 0;

                                BufferClear();

                                if (queue_MAX_limit_count == recvCount)
                                {
                                    recvCountEnable = false;
                                    daqQueue_count = daqQueue.Int16Count();
                                    if (daqQueue_count <= queue_MAX_limit_count)
                                    {
                                        aaacount++;
                                        Trace.WriteLine("DAQ Count 333 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString() + " short=" + short_data_count.ToString() + " / queue:" + dddcount.ToString());
                                        readbuffcount = 0;
                                        availablecount = 0;
                                        returncount = 0;
                                        receiver_temp_count = 0;
                                        daqQueue.Set();

                                        past_recvCount = 0;
                                        recvCount = 0;
                                        this.Write("{PRUN}");
                                        isPrunStart = true;
                                        //timeout = DateTime.Now;
                                    }
                                }
                            }
                        }
                    }

                    /*
                    numBytesAvailable = 0;
                    CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);
                    if (CSTDaqState != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Console.WriteLine("Failed to get number of bytes available to read (error " + CSTDaqState.ToString() + ")");
                        return;
                    }



                    
                    if (numBytesAvailable > 0)
                    {
                        UInt32 numBytesRead = 0;
                        byte[] readBuffer = new byte[numBytesAvailable];
                        CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);
                        
                        bool ischecked = false;
                        uint returCount = 0;
                        recvCountEnable = true;
                        
                        //for (i=0; i< numBytesRead; i++)
                        //{
                        //
                        //    //readBuffer[i] = 0x10;
                        //    ttttt_count++;
                        //    receheight = ttttt_count / Line_i;
                        //    recewidth = ttttt_count - (Line_i * receheight);
                        //
                        //    if (receheight == 50)
                        //    {
                        //        readBuffer[i] = 0x00;
                        //    }
                        //    else if (receheight == 100)
                        //    {
                        //        readBuffer[i] = 0x00;
                        //    }
                        //    else if (receheight == 150)
                        //    {
                        //        readBuffer[i] = 0x00;
                        //    }
                        //
                        //    if (_setR.SampleComposite <= 2)
                        //    {
                        //        ////////////////////////////
                        //        if (recewidth == 697)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //        else if (recewidth == 699)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //        ////////////////////////////
                        //        else if (recewidth == 897)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //        else if (recewidth == 899)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //        ////////////////////////////
                        //        else if (recewidth == 1797)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //        else if (recewidth == 1799)
                        //        {
                        //            readBuffer[i] = 0x00;
                        //        }
                        //    }
                        //    
                        //
                        //}
                        
                        recvCount += numBytesRead;


                        readbuffcount += readBuffer.Length;
                        availablecount += numBytesAvailable;
                        returncount += numBytesRead;


                        

                        daqQueue.testAddRange(readBuffer, isPrunStart, ref ischecked, ref returCount);

                        receiver_data_short_count = 0;


                    
                   

                    //if ((SetR.FrameHeight * SetR.FrameWidth * 2) - 1 < recvCount && timeout.AddMilliseconds(200) < DateTime.Now)

                    if (_DualEnable)
                    {
                        if (((SetR.FrameHeight * SetR.FrameWidth * _setR.SampleComposite * 4) - 1) < recvCount)
                        {
                            #region 삭제
                            //if (_setR.SampleComposite > 1)
                            //{
                            //    Trace.WriteLine("DAQ Queue Clear Start");
                            //    daqQueue.Clear();
                            //    Trace.WriteLine("DAQ Queue Clear END");
                            //}
                            #endregion
                            daqQueue.Set();
                            Trace.WriteLine("DAQ Count :" + recvCount.ToString());
                            recvCount = 0;

                            this.Write("{PRUN}");
                            isPrunStart = true;
                            
                            timeout = DateTime.Now;
                            
                        }
                    }
                    else
                    {

                        //if (((SetR.FrameHeight * SetR.FrameWidth * _setR.SampleComposite * 2)) == recvCount)
                        if (queue_MAX_limit_count == recvCount)
                        {

                            recvCountEnable = false;

                            daqQueue_count = daqQueue.Int16Count();

                            
                            
                            if (daqQueue_count <= queue_MAX_limit_count)
                            {
                                aaacount++;

                                Trace.WriteLine("DAQ Count 111 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString());
                                readbuffcount = 0;
                                availablecount = 0;
                                returncount = 0;

                                ttttt_count = 0;

                                

                                daqQueue.Set();
                                //Trace.WriteLine("DAQ Count 222 :" + recvCount.ToString() + " 444 :" + daqQueue_count.ToString() + " max :" + queue_MAX_limit_count.ToString());
                                

                                recvCount = 0;
                                this.Write("{PRUN}");
                                isPrunStart = true;

                                //timeout = DateTime.Now;
                                

                            }
                        }
                        else if (((SetR.FrameHeight * SetR.FrameWidth * _setR.SampleComposite * 2)) < recvCount)
                        {
                            // UInt32 size_t = recvCount - (UInt32)(SetR.FrameHeight * SetR.FrameWidth * _setR.SampleComposite * 2);
                            //daqQueue.Dequeues((int)size_t);
                            Trace.WriteLine("err Count 222 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString());
                            //daqQueue.Set();
                            //recvCount = 0;
                            //this.Write("{PRUN}");
                            //isPrunStart = true;

                            timeout = DateTime.Now;
                        }
                        

                        
                    }

                    }
                    else if ((numBytesAvailable == 0) && (recvCount < queue_MAX_limit_count) && (recvCountEnable == true))
                    {
                        //Trace.WriteLine("numBytesAvailable : " + numBytesAvailable.ToString());
                        receiver_data_short_count++;
                        if (receiver_data_short_count >= 90000)
                        {
                            receiver_data_short_count = 0;
                            Trace.WriteLine("err Count 000 :" + readbuffcount.ToString() + "/" + availablecount.ToString() + "/" + returncount.ToString() + "/" + aaacount.ToString());

                        }

                    }

                    */
                }
                catch (Exception ex)
                {

                }
                

            }
            //Trace.WriteLine("DoRead Out!!");
            
        }

        private int _DaqCounterTest = 0;
        public int DaqCounterTest
        {
            get { return _DaqCounterTest; }
            set { _DaqCounterTest = value; }
        }


        int rddddeturn_count = 0;
        public int ReadAvailables
        {
            
            get
            {
                //rddddeturn_count = 0;
                //Trace.WriteLine( "DAQ Count :" + daqQueue.Int16Count().ToString());
                rddddeturn_count = daqQueue.Int16Count();
                if (isChangeFlag == 0) { rddddeturn_count = 0; }
                if (isStopFlag == 0)   { rddddeturn_count = 0; }
                return rddddeturn_count;
            }
        }

        public short[,] Read(int samples)
        {
            int samplesCount = 0;
            //int samplesCount2 = 0;

            if (_DualEnable)
            {
                samplesCount = samples * 2;
            }
            else
            {
                samplesCount = samples;
                //samplesCount2 = samples * 2;
                //display_read_count = display_read_count + samplesCount2;
                //Trace.WriteLine("DAQ Count 333 :" + display_read_count.ToString());
                //display_read_count = 0;
            }

            //short[] data = Utility.NetworkBytesToHostInt16(daqQueue.Dequeues(samplesCount));

            short[,] data = Utility.NetworkBytesToHostInt16(daqQueue.Dequeues(samplesCount), _DualEnable);

            //Trace.WriteLine("SE Start Data : " + data[0,0].ToString() + " -" +data[0,9].ToString() );
            //Trace.WriteLine("BSE Start Data : " + data[1, 0].ToString() + " -" + data[0, 9].ToString());

            ////short[,] a = new short[1, data.Length];
            //short[,] a = null;

            //if (_DualEnable)
            //{
            //    a = new short[2, data.Length / 2];

            //    int datacount = 0;

            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            a[0, datacount] = data[i];
                        
            //        }
            //        else
            //        {
            //            a[1, datacount] = data[i];

                       

            //            datacount++;
            //        }
                    
            //    }

            //}
            //else
            //{
            //    a = new short[1, data.Length];

            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        a[0, i] = data[i];
            //    }
            //}


            
            return data;
        }

        public virtual void ValidateSetting(SettingScanner setting)
        {

        }

        public void UserStop()
        {
            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:8/00/0000/0000/0000/0000/}";

            UInt32 numBytesWritten = 0;
            string dataBuffer = DataString;

            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:8/00/0000/0000/0000/0000/}";

            startCount = 0;
            StartEnable = true;

            //CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);
        }

        public void Stop()
        {
            isSerialStart = false;


            //if (readThread.ThreadState == System.Threading.ThreadState.Running)
            //{
            //    readThread.Suspend();
            //    DataCount = 0;
            //}
        }

        public void ValidateSequenceRunable(SettingScanner[] setting)
        {

        }

        public void ShowInformation(System.Windows.Forms.IWin32Window owner)
        {

        }

        public void OnePoint(double horizontal, double vertical)
        {
            Trace.WriteLine("set stop333333 : " + isSerialStart.ToString());
            isStopFlag = 0;
            Stop();

            isSerialStart = false;
            daqQueue.Clear();
            daqQueue.Set();

            UInt32 numBytesWritten = 0;

            bufferCount = 0;

            int xpoint = (int)_setR.FrameWidth / 2;
            int ypoint = (int)_setR.FrameHeight / 2;

            if (DataString != null)
            {
                DataString = null;
            }

            DataString += "{PSw=START/M:";

            if (_DualEnable)
            {
                DataString += "1/";
            }
            else
            {
                DataString += "0/";
            }

            DataString += _setR.AiChannel.ToString();
            DataString += "/00/X:";
            DataString += string.Format("{0:000}", _setR.SampleComposite);
            DataString += "/";
            DataString += string.Format("{0:0000}", _setR.FrameWidth);
            //DataString += "/0016";
            DataString += "/";
            DataString += string.Format("{0:0000}", xpoint);
            
            DataString += "/0046";
            DataString += "/000/Y:001/";
            //DataString += _setR.LineAverage.ToString();
            
            DataString += string.Format("{0:0000}", _setR.FrameHeight);
            //DataString += "/0016";
            DataString += "/";
            DataString += string.Format("{0:0000}", ypoint);
            //DataString += string.Format("{0:0000}", _setR.ImageTop);
            DataString += "/0046";
            //DataString += string.Format("{0:0000}", _setR.ImageHeight + _setR.ImageTop);
            DataString += "/255/DA:";

            DataString += string.Format("{0:00000}", _setR.RatioX * 10000);
            DataString += "/";

            if (_setR.ShiftX < 0)
            {
                DataString += string.Format("{0:0000}", _setR.ShiftX * 5000);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.ShiftX * 5000);
            }


            DataString += "/";
            DataString += string.Format("{0:00000}", _setR.RatioY * 10000);
            DataString += "/";

            if (_setR.ShiftY < 0)
            {
                DataString += string.Format("{0:0000}", _setR.ShiftY * 5000);
            }
            else
            {
                DataString += string.Format("{0:00000}", _setR.ShiftY * 5000);
            }


            DataString += "/Z:";
            DataString += string.Format("{0:0}", _setR.AiClock / 1000000);
            DataString += "/00/0000/0000/0000/0000/}";

            //Trace.WriteLine("Read Data : " + DataString.ToString());

            
            string dataBuffer = DataString;

            


            CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);

            Thread.Sleep(100);

            Trace.WriteLine(dataBuffer);

            receviesDataDelegate readData = new receviesDataDelegate(DoRead);
            IAsyncResult iftar = readData.BeginInvoke(null, null);

            isSerialStart = true;



        }

        public void Reset()
        {

        }

        

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
