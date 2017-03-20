using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoImage.DataAcquation.CSTDaq
{
    class CSTQueue : Queue<byte>
    {
        private Object thisLock = new Object();

        private bool m_IsSetting = true;
        public bool IsSettings
        {
            get { return m_IsSetting; }
            set { m_IsSetting = value; }
        }

        private int _DataLength = 0;
        public int DataLength
        {
            get { return _DataLength; }
            set
            {
                _DataLength = value;
            }
        }

        public bool IsSetting
        {
            get
            {
                return m_IsSetting;
            }
        }

        public void Set()
        {
            m_IsSetting = false;
        }

        int datacount = 0;

        private int _pixelAvr = 0;
        public int PixelAvr
        {
            get { return _pixelAvr; }
            set { _pixelAvr = value; }
        }

        private int _fullDatacount = 0;
        public int FullDataCount
        {
            get { return _fullDatacount; }
            set { _fullDatacount = value; }
        }


        public void AddRange(params byte[] enu)
        {
            lock(thisLock)
            {
                if (!m_IsSetting && enu.Length > 1 && enu[0] == 0xff && enu[_pixelAvr -1] == 0xff)
                {
                    m_IsSetting = true;

                    System.Diagnostics.Trace.WriteLine("Data Change------------------------------------------------");

                    //byte[] data = new byte[enu.Length + 1];
                    //data[0] = 0xff;

                    //Array.Copy(enu, 0, data, 1, enu.Length);

                    //enu = null;
                    //enu = new byte[data.Length];

                    //Array.Copy(data, 0, enu, 0, data.Length);

                    //System.Diagnostics.Trace.WriteLine("data 1 : " + enu[0].ToString() + "------data 2 : " + enu[1].ToString() + "---------Data Count :" + datacount.ToString() + "-----DataLength : " + _DataLength.ToString());
                    System.Diagnostics.Trace.WriteLine("-Data Count :" + datacount.ToString() + "-----DataLength : " + _DataLength.ToString());
                    
                    datacount = 0;
                }

                if (m_IsSetting)
                {
                    foreach (byte obj in enu)
                        this.Enqueue(obj);
                    //datacount += enu.Length;
                    //if (datacount == 4530239)
                    //{
                    //    this.Enqueue(0x00);
                    //}
                }

                
            }
        }
        public void testAddRange(byte[] enu, bool isPrunStart, ref bool isChecked, ref uint recvCount)
        {
            lock (thisLock)
            {
                
                isChecked = true;
                m_IsSetting = true;
                isPrunStart = false;
                /*
                if (!m_IsSetting && enu.Length > 1 && enu[0] == 0xff && enu[1] == 0xff)
                {

                    
                    //int ffCount = _DataLength * 2 - datacount;
                    int ffCount = 0;
                    for (int i = 0; i < _pixelAvr; i++)
                    {
                        if (enu[i] != 0xff)
                        {
                            ffCount++;
                        }
                    }


                    if (ffCount != 0)
                    {
                        System.Diagnostics.Trace.WriteLine("Start Count---------------------------------------" + ffCount.ToString());

                        byte[] data = new byte[enu.Length + ffCount];

                        for (int i = 0; i < ffCount; i++)
                        {
                            data[i] = 0xff;
                        }


                        Array.Copy(enu, 0, data, ffCount, enu.Length);

                        enu = null;
                        enu = new byte[data.Length];

                        Array.Copy(data, 0, enu, 0, data.Length);
                    }


                    m_IsSetting = true;

                    //System.Diagnostics.Trace.WriteLine("Data Change------------------------------------------------");

                    //System.Diagnostics.Trace.WriteLine("-Data Count :" + datacount.ToString() + "-----DataLength : " + _DataLength.ToString());

                    datacount = 0;
                    isChecked = false;
                    isPrunStart = false;
                    //recvCount = (uint)ffCount;
                }
                */


                if (m_IsSetting && !isPrunStart)
                {
                    foreach (byte obj in enu)
                        this.Enqueue(obj);
                    /*
                    datacount += enu.Length;
                    

                    if (datacount == 4530239)
                    {
                        System.Diagnostics.Trace.WriteLine("daq Count 667 :" + datacount.ToString());
                        this.Enqueue(0x00);
                    }
                    else if (datacount > 4530239)
                    {
                        System.Diagnostics.Trace.WriteLine("daq Count 666 :" + datacount.ToString());

                    }
                    */

                }
            }
        }

        public byte[] Dequeues(int count)
        {
            lock (thisLock)
            {

                byte[] ret = new byte[count * 2];
                for (int i = 0; i < count * 2; i++)
                {
                    if (this.Count > 0)
                        ret[i] = this.Dequeue();
                }
                return ret;
            }


            
        }

        public int Int16Count()
        {
            return (this.Count > 0) ? this.Count / 2 : 0;
        }
    }
}
