using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace SEC.Nanoeye.NanoImage.DataAcquation.CSTDaq
{
    class CSTDaq_Backup : IDAQ
    {
        public CSTDaq_Backup(string device)
        {
            daqDevice = device;

           

            DeviceInit();

            //Trace.WriteLine(DaqSystem.Local.DriverMajorVersion.ToString() + "." + DaqSystem.Local.DriverMinorVersion + "." + DaqSystem.Local.DriverUpdateVersion, this.ToString());
        }
        

        public static string[] SearchCSTDaq()
        {

            List<string> deviceList = new List<string>();

            UInt32 ftdDeviceCount = 0;

            FTDI.FT_STATUS ftStatus =FTDI.FT_STATUS.FT_OK;
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

        private string DataString = null;

        private FTDI.FT_STATUS CSTDaqState;
        private FTDI CSTDaqDevice;

        private Thread readThread;

        delegate void receviesDataDelegate();

        public void DeviceInit()
        {
            CSTDaqState = FTDI.FT_STATUS.FT_OK;
            CSTDaqDevice = new FTDI();

            UInt32 ftdDeviceCount = 0;
            CSTDaqState = CSTDaqDevice.GetNumberOfDevices(ref ftdDeviceCount);

            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdDeviceCount];

            CSTDaqState = CSTDaqDevice.GetDeviceList(ftdiDeviceList);

            
            
            

            CSTDaqState = CSTDaqDevice.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);

            CSTDaqState = CSTDaqDevice.SetBaudRate(9600);

            CSTDaqState = CSTDaqDevice.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);

            CSTDaqState = CSTDaqDevice.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13);

            CSTDaqState = CSTDaqDevice.SetTimeouts(5000, 0);
            CSTDaqState = CSTDaqDevice.ResetDevice();

        }

        public delegate void MyEventHandler(string message, byte[] rawData);

        public event MyEventHandler ReciveData;
       
        int bufferCount = 0;
        
        delegate void OnDataRead(byte[] bufferReadData);
        
  
        bool isSerialStart = false;

        private short[] _ReadData;
        public short[] Readdata
        {
            get { return _ReadData; }
            set
            {
                _ReadData = value;
            }
        }

        List<short> m_RecvData = new List<short>();
        UInt32 recvCount = 0;
        private short[] fData, sData;
        List<byte> recvList = new List<byte>();


        private void DoRead()
        {
            UInt32 numBytesAvailable = 0;
            

            this.Write("{PRUN}");

            m_RecvData = new List<short>();
            recvList = new List<byte>();
            recvCount = 0;
            
           
            try
            {
                while (isSerialStart)
                {
                    //Thread.Sleep(20);

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
                        
                        if(flagStart && readBuffer[0] != 0xff && readBuffer[1] != 0xff)
                        {
                            continue;
                        }
                        recvCount += numBytesRead;
                        flagStart = false;
                        #region [ 초기 데이터가 들어온 사항. 그냥 넣어 주면됨. ]
                        if (recvList.Count > 0)
                        {
                            recvList.AddRange(readBuffer);
                            if (recvList.Count % 2 == 0)
                            {
                                // 짝수
                                m_RecvData.AddRange(Utility.NetworkBytesToHostInt16(recvList.ToArray()));
                                recvList = new List<byte>();
                                Trace.WriteLine("1 _ input Count : " + recvCount.ToString());

                            }
                            else
                            {
                                // 홀수
                                byte[] temp = new byte[recvList.Count - 1];
                                Array.Copy(recvList.ToArray(), 0, temp, 0, temp.Length);
                                recvList.RemoveRange(0, temp.Length);
                                m_RecvData.AddRange(Utility.NetworkBytesToHostInt16(temp));
                                recvList.Add(readBuffer[readBuffer.Length - 1]);
                                Trace.WriteLine("2 _ input Count : " + recvCount.ToString());
                            }
                        }
                        else
                        {
                            if (readBuffer.Length % 2 == 0)
                            {
                                // 짝수
                                m_RecvData.AddRange(Utility.NetworkBytesToHostInt16(readBuffer));
                                Trace.WriteLine("3 _ input Count : " + recvCount.ToString());
                            }
                            else
                            {
                                // 홀수
                                byte[] temp = new byte[readBuffer.Length - 1];
                                Array.Copy(readBuffer, 0, temp, 0, temp.Length);
                                m_RecvData.AddRange(Utility.NetworkBytesToHostInt16(temp));
                                recvList.Add(readBuffer[readBuffer.Length - 1]);
                                Trace.WriteLine("4 _ input Count : " + recvCount.ToString());
                            }
                        }
                        #endregion
                        
                        Trace.WriteLine("m_RecvData Count : " + m_RecvData.Count.ToString());
                    }
                    else
                    {
                        if(frameWidth != null && frameheight != null)
                        {
                            if ((frameWidth * frameheight * 2) <= recvCount)
                            {
                                recvCount = 0;
                                //m_RecvData = new List<short>();
                                //recvList = new List<byte>();
                                this.Write("{PRUN}");
                            }
                        }
                        
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        #region  [ DoRead Backup ]
        //private void DoRead()
        //{
        //    UInt32 numBytesAvailable = 0;
        //   //System.Diagnostics.Stopwatch datasw1 = new System.Diagnostics.Stopwatch();
        //   //OnDataRead onRead;
        //   //onRead = new OnDataRead(ReadDataRecive);
        //    //datasw.Start();
        //    List<byte> recvList = new List<byte>();

        //    this.Write("{PRUN}");
        //    try
        //    {
        //        while (isSerialStart)
        //        {
        //            //Thread.Sleep(20);

        //            CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);
        //            if (CSTDaqState != FTDI.FT_STATUS.FT_OK)
        //            {
        //                // Wait for a key press
        //                Console.WriteLine("Failed to get number of bytes available to read (error " + CSTDaqState.ToString() + ")");
        //                return;
        //            }

        //            if (numBytesAvailable > 0)
        //            {
        //                UInt32 numBytesRead = 0;
        //                byte[] readBuffer = new byte[numBytesAvailable];
        //                CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

        //                recvList.AddRange(readBuffer);
        //            }

        //            if (recvList.Count > 1 && recvList[0] != 0xff && recvList[1] != 0xff)
        //            {
        //                recvList.Clear();
        //            }
                    
        //            if (recvList.Count >= _setR.FrameWidth * _setR.FrameHeight * 2)
        //            {
        //                  var d = new RegisterCollection(recvList.ToArray());
        //                _ReadData = d.ToArray();
        //                //onRead.BeginInvoke(recvList.ToArray(), null, null);

        //                recvList = new List<byte>();
        //                this.Write("{PRUN}");
        //            }

        //            #region [ 삭제 ]
        //            //if (numBytesAvailable > 0)
        //            //{
        //            //    string readData = string.Empty;
        //            //    UInt32 numBytesRead = 0;
        //            //    byte[] readBuffer = new byte[numBytesAvailable];
        //            //    CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

        //            //    //Trace.WriteLine("numBytesRead : " + readBuffer.Length.ToString());

        //            //    #region [ 삭제 ]
        //            //    //if (_ReadBufferData == null)
        //            //    //{
        //            //    //    _ReadBufferData = new byte[readBuffer.Length];

        //            //    //    Array.Copy(readBuffer, 0, _ReadBufferData,0, readBuffer.Length);
        //            //    //}
        //            //    //else if (_ReadBufferData.Length < _setR.FrameWidth * _setR.FrameHeight * 2) // 데이터의 길이를 비교하여 값을 넘겨줌.
        //            //    //{
        //            //    //    byte[] testBuffer = new byte[_ReadBufferData.Length + readBuffer.Length];

        //            //    //    Array.Copy(_ReadBufferData, 0, testBuffer, 0, _ReadBufferData.Length);
        //            //    //    Array.Copy(readBuffer, 0, testBuffer, _ReadBufferData.Length, readBuffer.Length);

        //            //    //    _ReadBufferData = null;
        //            //    //    _ReadBufferData = testBuffer;

        //            //    //}
        //            //    //else
        //            //    //{
        //            //    //    byte[] outData = new byte[_setR.FrameWidth * _setR.FrameHeight * 2];
        //            //    //    byte[] reData = new byte[_ReadBufferData.Length - outData.Length + readBuffer.Length];

        //            //    //    Array.Copy(_ReadBufferData, 0, outData, 0, outData.Length);
        //            //    //    Array.Copy(_ReadBufferData, outData.Length, reData, 0, _ReadBufferData.Length - outData.Length);
        //            //    //    Array.Copy(readBuffer, 0, reData, _ReadBufferData.Length - outData.Length, readBuffer.Length);

        //            //    //    _ReadBufferData = null;
        //            //    //    _ReadBufferData = reData;

                            
        //            //    //    onRead.BeginInvoke(outData, null, null);
        //            //    //}
        //            //    #endregion

        //            //}
        //            #endregion
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //}
        #endregion
        private bool Write(string dataBuffer)
        {
            UInt32 numBytesWritten = 0;
            CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);

            if (CSTDaqState != FTDI.FT_STATUS.FT_OK) return false;

            return true;

        }

        private bool WriteEnable = false;
        private bool ReadEnable = false;
        private int DataarrayCount = 0;

        private byte[] _ReadBufferData;
        public byte[] ReadBufferData
        {
            get { return _ReadBufferData; }
            set { _ReadBufferData = value; }
        }

        

        private int ReadDataCount = 0;

        System.Diagnostics.Stopwatch datasw = new System.Diagnostics.Stopwatch();

        int FFCount = -1;
        int startCount = 0;

        bool StartEnable = false;
        
        public void ReadDataRecive(byte[] bufferData)
        {
            int strsum = 0;
            int DataCount = 0;
            startCount = 1;
            short[] dataout = new short[bufferData.Length /2];
            //short[] dataout;

            if (StartEnable)
            {
                bool startTrue = false;

                for (int i = 1; i < bufferData.Length; i += 2)
                {

                    if (bufferData[i - 1] == 255 && bufferData[i] == 255 || startTrue == true)
                    {
                        Trace.WriteLine("FFCount : " + FFCount.ToString());

                        dataout = new short[(bufferData.Length - i) / 2];
                        int x = bufferData[i] * 256;
                        int y = bufferData[i - 1];
                        strsum = x + y;

                        // 정순영 물어 보자
                        dataout[DataCount] = Convert.ToInt16(strsum * 0.5 - 1);

                        
                        DataCount++;
                        StartEnable = false;
                        FFCount = 0;
                    }
                    bufferCount++;
                    FFCount++;
                }
                
            }
            else
            {
                for (int i = 1; i < bufferData.Length; i += 2)
                {

                    if (bufferData[i - 1] == 255 && bufferData[i] == 255)
                    {
                        if (_ReadData != null)
                        {
                            //if (_ReadData.Length >= _setR.FrameWidth * _setR.FrameHeight * 2)
                            //{
                            //    short[] t1 = _ReadData;
                            //    short[] t2 = new short[_ReadData.Length - 240000];

                            //    Array.Copy(t1, _ReadData.Length - 240000, t2, 0, t2.Length);
                            //    _ReadData = t2;
                            //}
                        }
                        
                    }

                    int y = bufferData[i] * 256;
                    int x = bufferData[i - 1];
                    strsum = x + y;



                    dataout[DataCount] = Convert.ToInt16(strsum * 0.5 - 1);

                    bufferCount++;
                    DataCount++;
                    FFCount++;
                }
                DataArray(dataout, DataCount);

            }
          
            
        }

        public void DataArray(short[] data, int datacount)
        {

            
            //short[] ord = new short[_setR.FrameWidth * _setR.FrameHeight * _setR.SampleComposite];

            short[] ord = _ReadData;
            if (_ReadData == null)
            {
                ord = new short[data.Length];

                //ord = new short[_setR.FrameWidth * _setR.FrameHeight * _setR.SampleComposite];
                Array.Copy(data, 0, ord, 0, data.Length);
                _ReadData = ord;

            }
            //else if (data.Length == _setR.FrameWidth * SetR.FrameHeight * _setR.SampleComposite)
            //{
            //    ord = new short[data.Length];
            //    Array.Copy(data, 0, ord, 0, data.Length);
            //    _ReadData = ord;

            //}    
            else
            {


                int avrdatalenght = 0;



                if (0 > ord.Length - ReadDataCount)
                {
                    avrdatalenght = data.Length;
                }
                else
                {
                    avrdatalenght = ord.Length + data.Length - ReadDataCount;
                }

                short[] avrdata = new short[avrdatalenght];

                if (0 > ord.Length - ReadDataCount)
                {
                    //Array.Copy(ord, 0, avrdata, 0, ord.Length);
                    Array.Copy(data, 0, avrdata, 0, data.Length);
                }
                else
                {
                    Array.Copy(ord, ReadDataCount, avrdata, 0, ord.Length - ReadDataCount);
                    Array.Copy(data, 0, avrdata, ord.Length - ReadDataCount, data.Length);
                }


                _ReadData = avrdata;
                ReadDataCount = 0;
            }  


        }

        public void Ready(SettingScanner[] set)
        {

            isSerialStart = false;
           
            _setR = set[0];

            bufferCount = 0;
            
            if(DataString != null)
            {
                DataString = null;
            }

            DataString += "{PSw=START/M:0/";
            DataString += _setR.AiChannel.ToString();
            DataString += "/00/X:";
            DataString += string.Format("{0:000}", _setR.SampleComposite);
            DataString += "/";
            DataString += string.Format("{0:0000}", _setR.FrameWidth);
            DataString += "/0016";
            //DataString += string.Format("{0:0000}", _setR.ImageLeft);
            DataString += "/0046";
            //DataString += string.Format("{0:0000}", _setR.ImageWidth + _setR.ImageLeft);
            DataString += "/000/Y:001";
            //DataString += _setR.LineAverage.ToString();
            DataString += "/";
            DataString += string.Format("{0:0000}", _setR.FrameHeight);
            DataString += "/0016";
            //DataString += string.Format("{0:0000}", _setR.ImageTop);
            DataString += "/0046";
            //DataString += string.Format("{0:0000}", _setR.ImageHeight + _setR.ImageTop);
            DataString += "/255/DA:";

            DataString += string.Format("{0:00000}", (_setR.RatioX + 1) * 5000);
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
            DataString += string.Format("{0:00000}", (_setR.RatioY + 1) * 5000);
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
            //DataString += "/00/0000/0000/0000/0000/}";


            DataString += "8/00/0000/0000/0000/0000/}";

        }
        int? frameWidth, frameheight;
        bool flagStart = false;
        public void Change()
        {
            #region [ 테스트 삭제 ]
            //// 초기 배열 생성
            //fData = new short[_setR.FrameWidth * _setR.FrameHeight];
            //sData = new short[_setR.FrameWidth * _setR.FrameHeight];

            //List<short> temp = new List<short>();

            //for (int i = 1; i < _setR.FrameWidth + 1; i++)
            //{
            //    var value = 255 * ((float)i / (float)_setR.FrameWidth);
            //    temp.Add(Convert.ToInt16(value));
            //}
            //for (int i = 0; i < _setR.FrameHeight * _setR.FrameWidth; i += _setR.FrameWidth)
            //{
            //    Array.Copy(temp.ToArray(), 0, fData, i, temp.Count);
            //    temp.Reverse();
            //    Array.Copy(temp.ToArray(), 0, sData, i, temp.Count);
            //    temp.Reverse();
            //}
            //StartEnable = true;
            //isSerialStart = true;
            //_ReadData = fData;
            //return;
            #endregion

            UInt32 numBytesWritten = 0;
            int endDataX = _setR.ImageLeft + _setR.ImageWidth;
            int endDataY = _setR.ImageTop + _setR.ImageHeight;
            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:" + speed.ToString() +  "/00/0000/0000/0000/0000/}";
            //string dataBuffer = "{PSw=START/M:0/" + _setR.AiChannel.ToString() + "/00/X:00"+ _setR.SampleComposite.ToString() + "/0" + _setR.FrameWidth.ToString() + "/0" + _setR.ImageLeft.ToString() + "/0" + endDataX.ToString() + "/000/Y:00" + _setR.LineAverage.ToString() + "/0" + _setR.ImageHeight.ToString() + "/00" + _setR.ImageTop.ToString() + "/0" + endDataY.ToString() + "/255/DA:" + _setR.RatioX.ToString() + "/" + "00000" + "/" + _setR.RatioY.ToString() + "/" + "00000" + "/Z:" + _setR.AiClock.ToString() + "/00/0000/0000/0000/0000/}";
            string dataBuffer = DataString;

            //string dataBuffer = "{PSw=START/M:0/0/00/X:001/0480/0016/0046/000/Y:001/0250/0016/0046/255/DA:10000/00000/10000/00000/Z:8/00/0000/0000/0000/0000/}";
            isSerialStart = false;

            

            startCount = 0;
            StartEnable = true;
            flagStart = true;
            CSTDaqState = CSTDaqDevice.Write(dataBuffer, dataBuffer.Length, ref numBytesWritten);
            


            Trace.WriteLine(dataBuffer);

            Thread.Sleep(500);

            #region [ 일단 삭제 ]
            //UInt32 numBytesAvailable = 0;
            //List<byte> recvList = new List<byte>();

            //DateTime dt = DateTime.Now;

            //do
            //{
            //    #region [ Ftdi Status Check ]
            //    CSTDaqState = CSTDaqDevice.GetRxBytesAvailable(ref numBytesAvailable);

            //    if (CSTDaqState != FTDI.FT_STATUS.FT_OK)
            //    {
            //        // Wait for a key press
            //        Console.WriteLine("Failed to get number of bytes available to read (error " + CSTDaqState.ToString() + ")");
            //        return;
            //    }
            //    #endregion
            //    UInt32 numBytesRead = 0;

            //    if (numBytesAvailable > 0)
            //    {
            //        byte[] readBuffer = new byte[numBytesAvailable];
            //        CSTDaqState = CSTDaqDevice.Read(readBuffer, numBytesAvailable, ref numBytesRead);

            //        recvList.AddRange(readBuffer);
            //    }
            //    if (recvList.Count > 124 && recvList[0] == Convert.ToChar("{"))
            //    {
            //        break;
            //    }
            //    if(numBytesRead == 0 && dt.AddSeconds(3) < DateTime.Now)
            //    {
            //        break;
            //    }

            //} while (true);
            #endregion

            receviesDataDelegate readData = new receviesDataDelegate(DoRead);
            IAsyncResult iftar = readData.BeginInvoke(null, null);

            isSerialStart = true;
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

        }

        public void Reset()
        {
            
        }

        public int ReadAvailables
        {

            get 
            {
                //int ffIndex = m_RecvData.IndexOf(0x7fff);
                //if (ffIndex > 0)
                //{
                //    return ffIndex;
                //}
                //else
                //{
                if(flagStart)
                    return 0;
                else
                    return m_RecvData.Count;
                //}
                //int length = 0;

                //if (_ReadData != null)
                //{
                //    length = _ReadData.Length;
                //    //length = 120000;
                //}

                //return length; 
                //return DataCount;
            }
        }

        bool isTestFlag;
        public short[,] Read(int samples)
        {
            
            short[] getValue = new short[samples];
            Array.Copy(m_RecvData.ToArray(), 0, getValue, 0, samples);
            Trace.WriteLine("1 : m_RecvData Lenght : " + m_RecvData.Count.ToString());

            if(m_RecvData.Count < samples)
            {
                m_RecvData.RemoveRange(0, m_RecvData.Count);
            }
            else
            {
                m_RecvData.RemoveRange(0, samples);
            }

            Trace.WriteLine("2 : m_RecvData Lenght : " + m_RecvData.Count.ToString());
            short[] data = getValue;

            //short[] data = _ReadData;
            //short[] data;
            //if (isTestFlag)
            //    data = fData;
            //else
            //    data = sData;

            //isTestFlag = !isTestFlag;

            Trace.WriteLine("data Lenght : " + data.Length.ToString());

            short[,] a = new short[1, data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                a[0, i] = data[i];
                //a[0, i] = 0;
            }
            _ReadData = null;
            return  a;
        }

        public short[] OutReadData(int samples)
        {



            Trace.WriteLine("Data Out --------------------------------");
            short[] defaultData = _ReadData;

            short[] result = new short[samples];

            Array.Copy(defaultData, 0, result, 0, result.Length);




            ReadDataCount += samples;
            //short[] newReaddata = new short[defaultData.Length - (count + samples)];

            //Array.Copy(defaultData, count + samples, newReaddata, 0, newReaddata.Length);

            //_ReadData = newReaddata;




            return result;
        }

        public void Dispose()
        {
            
        }


    }
}
