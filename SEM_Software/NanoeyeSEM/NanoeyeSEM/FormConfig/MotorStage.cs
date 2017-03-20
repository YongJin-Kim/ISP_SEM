using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using OpenCvSharp;
using System.Diagnostics;



namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
    public partial class MotorStage : Form
    {
        private Template.ITemplate equip = null;
        //private System.Windows.Forms.Timer CameraTimer;

        Thread rsThread;
        string rsData;
        bool isSerialStart = false;

        public string CrLf = "\r\n";
        public string Cr = "\r";

        private string[] _MotorXvalue = new string[5];
        public string[] MotorXvalue
        {
            get { return _MotorXvalue; }
            set
            {
                _MotorXvalue = value;
            }
        }

        private string[] _MotorYvalue = new string[5];
        public string[] MotorYvalue
        {
            get { return _MotorYvalue; }
            set
            {
                _MotorYvalue = value;
            }
        }

        private string[] _MotorRvalue = new string[5];
        public string[] MotorRvalue
        {
            get { return _MotorRvalue; }
            set
            {
                _MotorRvalue = value;
            }
        }

        private string[] _MotorTvalue = new string[5];
        public string[] MotorTvalue
        {
            get { return _MotorTvalue; }
            set
            {
                _MotorTvalue = value;
            }
        }

        private string[] _MotorZvalue = new string[5];
        public string[] MotorZvalue
        {
            get { return _MotorZvalue; }
            set
            {
                _MotorZvalue = value;
            }
        }



        public MotorStage()
        {
            InitializeComponent();

            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string[] portsArray = null;

        //public SerialPort serialport2 = null;

        

        public void StageLoad()
        {
            //string[] portsArray = SerialPort.GetPortNames();

            //foreach (string portnumber in portsArray)
            //{
               // comboBox1.Items.Add(portnumber);
            //}


            //serialPort1.PortName = Properties.Settings.Default.StagePortName;

            //portsArray = SerialPort.GetPortNames();


            //if (Properties.Settings.Default.StagePortName == "COM1")
            //{
            //    return;
            //}

           

            

            

            serialPort1 = new SerialPort();
            

            serialPort1.PortName = Properties.Settings.Default.StagePortName;
            //serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 38400;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Encoding = Encoding.ASCII;
            //serialPort1.
            
            
            

            

            //serialPort1.ReadTimeout = 200;
            
            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(Data_Received);

            if (!serialPort1.IsOpen)
            {
                try
                {


                    serialPort1.Open();
                    

                    //serialPort1.NewLine = CrLf;
                    serialPort1.WriteLine("{M1R}");

                    rsData = serialPort1.ReadExisting();
                    
                   //serialPort1.Close();
                  

                }
                catch
                {


                    
                    serialPort1.Close();
                    return;
                      


                    
                    //MessageBox.Show("Motor Stage Close");
                }
                
                
                
                
            }

            

            if (serialPort1.IsOpen)
            {
                isSerialStart = true;

                rsThread = new Thread(new ThreadStart(receivesData));
                rsThread.IsBackground = true;
                rsThread.Start();

              

                
            }


            //JoyStickControlInit();

            equip = SystemInfoBinder.Default.Equip;

            bsX.ControlValue = equip.ColumnBSX;
            bsY.ControlValue = equip.ColumnBSY;

            detectClt.ControlValue = equip.ColumnHVCLT;
            detectPmt.ControlValue = equip.ColumnHVPMT;


            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            //loadImage = Cv.LoadImage(strPath + "icon05-01-01.png");

            //Bitmap locationimage = new Bitmap(Properties.Resources.icon05_01_01);

            //loadImage = Cv.
            

            //if (Properties.Settings.Default.SpControlEnable == "0")
            //{
            //    JoystickBtn.Checked = true;
            //}
            //else
            //{
            //    PCBtn.Checked = true;
            //}


            //magMotorSpeed[0] = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSx100));
            //magMotorSpeed[1] = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSx500));
            //magMotorSpeed[2] = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSx1000));
            //magMotorSpeed[3] = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSx3000));
            //magMotorSpeed[4] = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSx5000));
            //motorRSpeed = String.Format("{0:D3}", Convert.ToInt16(Properties.Settings.Default.MSSR));

            //SpeedChange();

            WriteEnable = true;

            while (readEnable)
            {
                Thread.Sleep(1);
            }


            if (serialPort1.IsOpen)
            {
                serialPort1.WriteLine("{M1S/-1234567/100/z}");
                Thread.Sleep(20);
                serialPort1.WriteLine("{M2S/-1234567/100/z}");
                Thread.Sleep(20);
                serialPort1.WriteLine("{M3S/-1234567/100/z}");
                Thread.Sleep(20);
                serialPort1.WriteLine("{M4S/-1234567/100/z}");
                Thread.Sleep(20);
                serialPort1.WriteLine("{M5S/-1234567/100/z}");
                Thread.Sleep(20);

            }
            

            WriteEnable = false;
            



        }

        public void MotorLocationChange(int mainLeft, int top)
        {
            this.Location = new Point(mainLeft, top);
            this.Invalidate();
        }

        public void MotorSizeChange(Size mainSize, bool enable)
        {
            if (!enable)
            {
                this.Size = new Size(this.Size.Width, mainSize.Height);
                this.TopMost = true;
               
            }
            else
            {
                this.Size = new Size(this.Size.Width, 394);
                this.TopMost = true;
            }

            
            this.Invalidate();
            this.TopMost = false;
            
        }

        private bool readEnable = false;
        private bool WriteEnable = false;

        private void receivesData()
        {
            try
            {
                while (isSerialStart)
                {
                    if (!serialPort1.IsOpen)
                    {
                        //MessageBox.Show("MotorStage Close");

                        return;
                    }

                    //Thread.Sleep(200);
                    


                    try
                    {
                        //serialPort1.NewLine = CrLf;

                        

                        while (WriteEnable)
                        {
                            Thread.Sleep(10);
                        }

                        readEnable = true;
                        serialPort1.WriteLine("{M1R}");

                        while (true)
                        {
                            Thread.Sleep(20);
                            
                            rsData = serialPort1.ReadExisting();
                            

                            if (rsData.Length != 20)
                            {
                                serialPort1.DiscardInBuffer();
                                serialPort1.DiscardOutBuffer();
                                serialPort1.WriteLine("{M1R}");

                            }
                            else
                            {
                                readEnable = false;
                                parsingMsg(rsData);
                                break;
                            }

                        }






                        
                        while (WriteEnable)
                        {
                            Thread.Sleep(5);
                        }

                        readEnable = true;

                        serialPort1.WriteLine("{M2R}");
                        

                        while (true)
                        {
                            Thread.Sleep(20);
                            
                            rsData = serialPort1.ReadExisting();

                            if (rsData.Length != 20)
                            {
                                serialPort1.DiscardInBuffer();
                                serialPort1.DiscardOutBuffer();
                                serialPort1.WriteLine("{M2R}");

                            }
                            else
                            {
                                readEnable = false;
                                parsingMsg(rsData);
                                break;
                            }

                        }





                        

                        while (WriteEnable)
                        {
                            Thread.Sleep(5);
                        }

                        readEnable = true;

                        serialPort1.WriteLine("{M3R}");

                        while (true)
                        {
                            Thread.Sleep(20);
                            
                            rsData = serialPort1.ReadExisting();

                            if (rsData.Length != 20)
                            {
                                serialPort1.DiscardInBuffer();
                                serialPort1.DiscardOutBuffer();
                                serialPort1.WriteLine("{M3R}");

                            }
                            else
                            {
                                readEnable = false;
                                parsingMsg(rsData);
                                break;
                            }

                        }





                       

                        while (WriteEnable)
                        {
                            Thread.Sleep(5);
                        }


                        readEnable = true;
                        serialPort1.WriteLine("{M4R}");

                        while (true)
                        {
                            Thread.Sleep(20);
                            
                            rsData = serialPort1.ReadExisting();

                            if (rsData.Length != 20)
                            {
                                serialPort1.DiscardInBuffer();
                                serialPort1.DiscardOutBuffer();
                                serialPort1.WriteLine("{M4R}");

                            }
                            else
                            {
                                readEnable = false;
                                parsingMsg(rsData);
                                break;
                            }

                        }





                        

                        while (WriteEnable)
                        {
                            Thread.Sleep(5);
                        }


                        readEnable = true;
                        serialPort1.WriteLine("{M5R}");

                        while (true)
                        {
                            Thread.Sleep(20);
                            
                            rsData = serialPort1.ReadExisting();

                            if (rsData.Length != 20)
                            {
                                serialPort1.DiscardInBuffer();
                                serialPort1.DiscardOutBuffer();
                                serialPort1.WriteLine("{M5R}");

                            }
                            else
                            {
                                readEnable = false;
                                parsingMsg(rsData);
                                break;
                            }

                        }
                                              
                        

                        //picturePoint();
                       
                        

                        

                    }
                    catch (Exception e)
                    {
                        if (!serialPort1.IsOpen) { return; }

                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        //MessageBox.Show(rsData);
                        //return;
                    }

                }

                


            }
            catch (ThreadInterruptedException)
            {
                return;
            }
        }

        private double Xpoint = 0.0;
        private double Ypoint = 0.0;



        private int MotorMoving = 0;

        private double MotorPulse = 360/0.525;

        public void parsingMsg(string Msg)
        {
           
                //string[] strArr = Msg.Split('+');
                string st = Msg.Substring(1, 18);
                       
                string[] str = st.Split(new string[] {"/"}, StringSplitOptions.None);
                double limitleft = 0;
                double limitright = 0;

                try
                {
                    //string str = Msg.Substring(2, 1);

                    switch (str[0])
                    {
                        case "M1R":
                            MotorXvalue = str;

                            if (Properties.Settings.Default.MotorXDirection)
                            {

                                MotorXvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorXvalue[1]) / MotorPulse * Properties.Settings.Default.MotorXPitch * -1);

                                MotorXvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorXvalue[1]) + Properties.Settings.Default.MotorXOffset);

                                if (MotorXvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageXvalue > Convert.ToDouble(MotorXvalue[1]))
                                    {
                                        StageStop(1);
                                    }
                                }

                            }
                            else
                            {
                                MotorXvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorXvalue[1]) / MotorPulse * Properties.Settings.Default.MotorXPitch);

                                MotorXvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorXvalue[1]) + Properties.Settings.Default.MotorXOffset);

                                if (MotorXvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageXvalue < Convert.ToDouble(MotorXvalue[1]))
                                    {
                                        StageStop(1);
                                    }
                                }
                            }

                            

                            limitleft = (double)Properties.Settings.Default.MotorStageXLeft;
                            limitright = (double)Properties.Settings.Default.MotorStageXRight;


                            if (limitleft > Convert.ToDouble(MotorXvalue[1]) && CalibrationEnable == false)
                            {
                                StageStop(1);
                            }

                            if(Convert.ToDouble(MotorXvalue[1]) > limitright && CalibrationEnable == false)
                            {
                                StageStop(1);
                            }

                            Properties.Settings.Default.MotorStageXvalue = Convert.ToDouble(MotorXvalue[1]);
                            

                            break;

                        case "M2R":
                            MotorYvalue = str;
                            if (Properties.Settings.Default.MotorYDirection)
                            {
                                MotorYvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorYvalue[1]) / MotorPulse * Properties.Settings.Default.MotorYPitch * -1);

                                MotorYvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorYvalue[1]) + Properties.Settings.Default.MotorYOffset);
                                if (MotorYvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageYvalue < Convert.ToDouble(MotorYvalue[1]))
                                    {
                                        StageStop(2);
                                    }
                                }
                            }
                            else
                            {

                                MotorYvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorYvalue[1]) / MotorPulse * Properties.Settings.Default.MotorYPitch);

                                MotorYvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorYvalue[1]) + Properties.Settings.Default.MotorYOffset);

                                if (MotorYvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageYvalue > Convert.ToDouble(MotorYvalue[1]))
                                    {
                                        StageStop(2);
                                    }
                                }


                            }


                            

                            limitleft = Properties.Settings.Default.MotorStageYTop;
                            limitright = Properties.Settings.Default.MotorStageYBottom;

                            if (limitleft > Convert.ToDouble(MotorYvalue[1]) && CalibrationEnable == false)
                            {
                                StageStop(2);
                            }

                            if (Convert.ToDouble(MotorYvalue[1]) > limitright && CalibrationEnable == false)
                            {
                                StageStop(2);
                            }

                            Properties.Settings.Default.MotorStageYvalue = Convert.ToDouble(MotorYvalue[1]);
                                
                            break;

                        case "M3R":
                            MotorRvalue = str;
                            if (Properties.Settings.Default.MotorRDirection)
                            {
                                MotorRvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorRvalue[1]) / MotorPulse * Properties.Settings.Default.MotorRPitch * -1);

                                
                            }
                            else
                            {
                                MotorRvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorRvalue[1]) / MotorPulse * Properties.Settings.Default.MotorRPitch);
                            }


                            MotorRvalue[1] = string.Format("{0:0}", Convert.ToDouble(MotorRvalue[1]) + Properties.Settings.Default.MotorROffset);
                            

                            Properties.Settings.Default.MotorStageRvalue = Convert.ToDouble(MotorRvalue[1]);
                            break;

                        case "M4R":
                            MotorTvalue = str;
                            if (Properties.Settings.Default.MotorTDirection)
                            {
                                MotorTvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorTvalue[1]) / MotorPulse * Properties.Settings.Default.MotorTPitch * -1);

                                MotorTvalue[1] = string.Format("{0:0}", Convert.ToDouble(MotorTvalue[1]) + Properties.Settings.Default.MotorTOffset);

                                if (MotorTvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageTvalue > Convert.ToDouble(MotorTvalue[1]))
                                    {
                                        StageStop(4);
                                    }
                                }
                            }
                            else
                            {
                                MotorTvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorTvalue[1]) / MotorPulse * Properties.Settings.Default.MotorTPitch);

                                MotorTvalue[1] = string.Format("{0:0}", Convert.ToDouble(MotorTvalue[1]) + Properties.Settings.Default.MotorTOffset);

                                if (MotorTvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageTvalue < Convert.ToDouble(MotorTvalue[1]))
                                    {
                                        StageStop(4);
                                    }
                                }
                            }



                           
                            

                            limitleft = Properties.Settings.Default.MotorStageTLeft;
                            limitright = Properties.Settings.Default.MotorStageTRight;

                            if (limitleft > Convert.ToDouble(MotorTvalue[1]) && CalibrationEnable == false)
                            {
                                StageStop(4);
                            }

                            if (Convert.ToDouble(MotorTvalue[1]) > limitright && CalibrationEnable == false)
                            {
                                StageStop(4);
                            }


                            Properties.Settings.Default.MotorStageTvalue = Convert.ToDouble(MotorTvalue[1]);
                            break;

                        case "M5R":
                            MotorZvalue = str;
                            if (Properties.Settings.Default.MotorZDirection)
                            {
                                MotorZvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorZvalue[1]) / MotorPulse * Properties.Settings.Default.MotorZPitch * -1);

                                MotorZvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorZvalue[1]) + Properties.Settings.Default.MotorZOffset);
                                if (MotorZvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageZvalue < Convert.ToDouble(MotorZvalue[1]))
                                    {
                                        double x = Properties.Settings.Default.MotorStageZvalue;
                                        StageStop(5);
                                    }
                                }
                            }
                            else
                            {


                                MotorZvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorZvalue[1]) / MotorPulse * Properties.Settings.Default.MotorZPitch);

                                MotorZvalue[1] = string.Format("{0:0.000}", Convert.ToDouble(MotorZvalue[1]) + Properties.Settings.Default.MotorZOffset);

                                if (MotorZvalue[3] == "L")
                                {
                                    if (Properties.Settings.Default.MotorStageZvalue > Convert.ToDouble(MotorZvalue[1]))
                                    {
                                        StageStop(5);
                                    }
                                }
                            }


                            
                            

                            limitleft = (double)Properties.Settings.Default.MotorStageZTop;
                            limitright = (double)Properties.Settings.Default.MotorStageZBottom;

                            if (limitleft > Convert.ToDouble(MotorZvalue[1]) && CalibrationEnable == false)
                            {
                                StageStop(5);
                            }

                            if (Convert.ToDouble(MotorZvalue[1]) > limitright && CalibrationEnable == false)
                            {
                                StageStop(5);
                            }

                            Properties.Settings.Default.MotorStageZvalue = Convert.ToDouble(MotorZvalue[1]);
                            break;

                        default:
                            break;

                        
                    }

                    OnPropertyChanged("MotorLocationChange");

                }
                catch
                {

                }
                   
                   


            


        }

        private IplImage imgSrc;
        private IplImage loadImage;

        private void picturePoint()
        {

            //imgSrc = new IplImage();

            imgSrc = new IplImage(320, 240, BitDepth.U8, 3);
            //imgSrc.back


            //imgSrc = Cv.LoadImage(Properties.Resources.icon05_01_01);
            imgSrc = Cv.CloneImage(loadImage);
            //imgSrc.image
            
            //imgSrc = loadImage;
            double x = Convert.ToDouble(CurrentX.Text);
            double y = Convert.ToDouble(CurrentY.Text);

            x = pictureBoxIpl1.Size.Width / 2 + (x * (160/ 20));
            y = pictureBoxIpl1.Size.Height / 2 - (y * (102 / 20));


            CvPoint centerPoint = new CvPoint((int)x, (int)y);



            imgSrc.DrawMarker(centerPoint.X, centerPoint.Y, Cv.RGB(50, 235, 251), MarkerStyle.Cross, 250, LineType.AntiAlias, 2);
            //imgSrc.DrawCircle(new CvPoint(pictureBoxIpl1.Size.Width /2, pictureBoxIpl1.Size.Height /2)  , 5, Cv.RGB(255, 0, 0));

            imgSrc.DrawCircle(centerPoint, 63, Cv.RGB(50, 235, 251), 2);
            imgSrc.DrawCircle(centerPoint, 105, Cv.RGB(50, 235, 251), 2);

            imgSrc.DrawCircle(new CvPoint(pictureBoxIpl1.Size.Width / 2, pictureBoxIpl1.Size.Height / 2), 0, Cv.RGB(255, 0, 0), 8);

            
             
             pictureBoxIpl1.ImageIpl = imgSrc;
             //pictureBoxIpl1.SizeMode = PictureBoxSizeMode.CenterImage;
             //pictureBoxIpl1.BackgroundImage = SEC.Nanoeye.NanoeyeSEM.Properties.Resources.Camera_CrossPoint;
        }

       

        private void threadCall()
        {
            //this.disp_Message(rsData);
        }

        public void MotorClose()
        {
            if (!serialPort1.IsOpen) { return; }

            //rsThread.Join();
            //rsThread.Abort();
            isSerialStart = false;
          
            //rsThread.;
            serialPort1.Close();
            this.Dispose();
            this.Close();
        }

        public void ProtClose()
        {
            serialPort1.Close();
            //rsThread.
        }

      

        //private void StageHome_Click(object sender, EventArgs e)
        //{
        //    StageHome.Checked = false;


        //    serialPort1.WriteLine("POS" + "+00.000" + "+00.000");
            
        //}

        



        System.Windows.Forms.Timer MouseTimer;
        private void MoveLeft_MouseDown(object sender, MouseEventArgs e)
        {
            
            MouseTimer = new System.Windows.Forms.Timer();
            MouseTimer.Tick += new EventHandler(MoveLeftChange);
            MouseTimer.Interval = 100;
            MouseTimer.Start();

            
            
            

        }

        private void MoveLeft_MouseUp(object sender, MouseEventArgs e)
        {
            MouseTimer.Stop();

            
        }

        private void MoveLeftChange(object sender, EventArgs e)
        {
            //double x = Convert.ToDouble(CurrentX.Text);
            double x = Xpoint;
            x += -0.001;
            Xpoint = x;

            string x1 = string.Format("{0:00.000}", x);


            if (x1.Length < 7)
            {
                x1 = x1.PadLeft(7, '+');
            }

            serialPort1.WriteLine("POS" + x1 + CurrentY.Text);
        }



        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
           

            MouseTimer = new System.Windows.Forms.Timer();
            MouseTimer.Tick += new EventHandler(MoveRightChange);
            MouseTimer.Interval = 100;
            MouseTimer.Start();

            
        }

        private void MoveRightChange(object sender, EventArgs e)
        {
            //double x = Convert.ToDouble(CurrentX.Text);
            double x = Xpoint;
            x += +0.001;
            Xpoint = x;

            string x1 = string.Format("{0:00.000}", x);


            if (x1.Length < 7)
            {
                x1 = x1.PadLeft(7, '+');
            }



            serialPort1.WriteLine("POS" + x1 + CurrentY.Text);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseTimer.Stop();

            
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            

            MouseTimer = new System.Windows.Forms.Timer();
            MouseTimer.Tick += new EventHandler(MoveUpChange);
            MouseTimer.Interval = 100;
            MouseTimer.Start();

            
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            MouseTimer.Stop();

            
        }

        private void MoveUpChange(object sender, EventArgs e)
        {
            //double x = Convert.ToDouble(CurrentY.Text);
            double x = Ypoint;
            x += +0.001;
            Ypoint = x;

            string x1 = string.Format("{0:00.000}", x);


            if (x1.Length < 7)
            {
                x1 = x1.PadLeft(7, '+');
            }



            serialPort1.WriteLine("POS" + CurrentX.Text + x1);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            

            MouseTimer = new System.Windows.Forms.Timer();
            MouseTimer.Tick += new EventHandler(MoveDownChange);
            MouseTimer.Interval = 100;
            MouseTimer.Start();

            //double x = Convert.ToDouble(CurrentY.Text);
            double x = Ypoint;
            x += -0.001;
            Ypoint = x;

            string x1 = string.Format("{0:00.000}", x);


            if (x1.Length < 7)
            {
                x1 = x1.PadLeft(7, '+');
            }



            serialPort1.WriteLine("POS" + CurrentX.Text + x1);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            MouseTimer.Stop();

           
        }

        private void MoveDownChange(object sender, EventArgs e)
        {
            //double x = Convert.ToDouble(CurrentY.Text);
            double x = Ypoint;
            x += -0.001;
            Ypoint = x;

            string x1 = string.Format("{0:00.000}", x);


            if (x1.Length < 7)
            {
                x1 = x1.PadLeft(7, '+');
            }



            serialPort1.WriteLine("POS" + CurrentX.Text + x1);
        }



        public void StageMoveChange(double x, double y)
        {
            if (!serialPort1.IsOpen)
            {
                return;
            }


            double Motorx = Properties.Settings.Default.MotorStageXvalue;
            double Motory = Properties.Settings.Default.MotorStageYvalue;
            
            Motorx = Motorx - x;
            Motory = Motory + y;

            string valueStrX = null;
            string valueStrY = null;

            if (Motorx < 0)
            {
                double x1 = Motorx * -1;

                valueStrX = x1.ToString().PadLeft(7, '0');
                valueStrX = valueStrX.PadLeft(8, '-');
            }
            else
            {
                valueStrX = Motorx.ToString().PadLeft(8, '0');
            }

            if (Motory < 0)
            {
                double y1 = Motory * -1;

                valueStrY = y1.ToString().PadLeft(7, '0');
                valueStrY = valueStrY.PadLeft(8, '-');
            }
            else
            {
                valueStrX = Motorx.ToString().PadLeft(8, '0');
            }

            string strX = null;
            string strY = null;

            strX += "{M1S/";
            strX += valueStrX.ToString();
            strX += "{010/r}";

            strY += "{M2S/";
            strY += valueStrY.ToString();
            strY += "{010/r}";

            try
            {
                serialPort1.WriteLine(strX);
                Thread.Sleep(10);
                serialPort1.WriteLine(strY);
            }
            catch
            {
                Thread.Sleep(20);
                serialPort1.WriteLine(strX);
                Thread.Sleep(10);
                serialPort1.WriteLine(strY);
            }

            
        }

        private void StageMoveStop_Click(object sender, EventArgs e)
        {
            MoveStop.Checked = false;
            serialPort1.WriteLine("STOP");

        }


        private Point mouseCurrentPoint = new Point(0, 0);
        private void MotorStage_MouseDown(object sender, MouseEventArgs e)
        {
            

            if (e.Button == MouseButtons.Left)
            {
                mouseCurrentPoint = e.Location;
            }
        }

        private void MotorStage_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Height > 700) { return; }

            if (e.Button == MouseButtons.Left)
            {
                Point mouseNewPoint = e.Location;

                this.Location = new Point(mouseNewPoint.X - mouseCurrentPoint.X + this.Location.X, mouseNewPoint.Y - mouseCurrentPoint.Y + this.Location.Y);
            }
        }



     

        private void bsX_ValueChanged(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            bsXValue.Location = new Point(left, top);

            bsXValue.Text = value.ToString();
        }

        private void bsY_ValueChanged(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)(((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)) - 50); ;


            left = (((tb.Left + 34) - 21) + (value * 2)) + 100;

            bsYValue.Location = new Point(left, top);

            bsYValue.Text = value.ToString();
        }

        public void MotorConnet(string portName)
        {
           

            if (serialPort1.IsOpen) { serialPort1.Close(); }

            
            //Properties.Settings.Default.StagePortName = comboBox1.SelectedItem.ToString();


            serialPort1.PortName = portName;
            serialPort1.BaudRate = 9600;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(Data_Received);

            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open();
                    this.Show();
                }
                catch
                {
                    MessageBox.Show("Motor Stage Close");
                }




            }


            //serialPort1.NewLine = CrLf;
            //serialPort1.WriteLine("READ");

            if (serialPort1.IsOpen)
            {
                isSerialStart = true;

                rsThread = new Thread(new ThreadStart(receivesData));
                rsThread.IsBackground = true;
                rsThread.Start();
            }

            //loadImage = Cv.LoadImage("C:\\Program Files (x86)\\SEC\\Mini-SEM\\CrossPoint.png");
        }

        private void detectClt_ValueChanged(object sender, EventArgs e)
        {

            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            CltText.Location = new Point(left, top);


            CltText.Text = value.ToString();
        }

        private void detectPmt_ValueChanged(object sender, EventArgs e)
        {
            SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle tb = sender as SEC.Nanoeye.Support.Controls.ImageTrackBarWithSingle;

            if (tb == null) { return; }

            int left = tb.Left;
            int top = tb.Top - 20;
            int value = (int)((tb.Value - tb.Minimum) * 100f / (tb.Maximum - tb.Minimum)); ;


            left = ((tb.Left + 34) - 21) + (value * 2);

            AmpText.Location = new Point(left, top);


            AmpText.Text = value.ToString();
        }

        private void SpControl_Changed(object sender, EventArgs e)
        {
            SEC.Nanoeye.Controls.BitmapCheckBox bcb = sender as SEC.Nanoeye.Controls.BitmapCheckBox;

            if (!bcb.Checked)
            {
                bcb.Checked = true;
                return;
            }


            if (bcb.Name == "JoystickBtn")
            {
                Properties.Settings.Default.SpControlEnable = "0";
                PCBtn.Checked = false;
                SpeedChange();
            }
            else
            {
                Properties.Settings.Default.SpControlEnable = "1";
                JoystickBtn.Checked = false;
                SpeedChange();
            }

        }

        private string[] magMotorSpeed = new string[5];
        public string[] MagMotorSpeed
        {
            get { return magMotorSpeed; }
            set { magMotorSpeed = value; }
        }

        public string motorRSpeed = "100";
        



        public void SpeedChange()
        {
            if (!serialPort1.IsOpen) { return; }

            int magX = 0;
            int magY = 0;
            int magR = 0;

            if (Properties.Settings.Default.SpControlEnable == "0")
            {
                magX = Convert.ToInt32(MoveSpeed.Text);
                magY = magY;
                magR = magX;

                if (magX == 100)
                {
                    serialPort1.WriteLine("SPD0" + magX.ToString() + "0" + magY.ToString() + magR.ToString());
                }
                else
                {
                    serialPort1.WriteLine("SPD00" + magX.ToString() + "0" + magY.ToString() + "0" + magR.ToString());
                }
                
            }
            else
            {
                if (UIsetBinder.Default.MagIndex == UIsetBinder.Default.MagMinimum)
                {
                    serialPort1.WriteLine("FORCETIME010100");
                    serialPort1.WriteLine("SPD1100080100");
                }
                else if (equip.Magnification >= 5000)
                {
                    serialPort1.WriteLine("FORCETIME010030");
                    serialPort1.WriteLine("SPD1015015100");
                }
                else
                {
                    magX = 100 - (UIsetBinder.Default.MagIndex * 5);
                    magY = (int)(magX / 1.25);
                    magR = magX;

                    serialPort1.WriteLine("FORCETIME0100" + magX.ToString());
                    serialPort1.WriteLine("SPD10" + magX.ToString() + "0" + magY.ToString() + "100");
                }
            }

        }



        private void CurrentX_TextChanged(object sender, EventArgs e)
        {
            picturePoint();
        }


       
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        //double polepitch = 70300 / 360; //DC

        double polepitch = 26918 / 360; //Step
        double step = 360 / Convert.ToDouble(Properties.Settings.Default.MSSStep);
        private void MotorRStepL_Click(object sender, EventArgs e)
        {
            if (MotorMoving == 1)
            {
                MotorRStepL.Checked = false;
                return;
            }
           
            //polepitch = polepitch * (42 / 14);



            int MoveRDelay = (int)(polepitch * step);
            MotorMoveRotateStart(0);
            Delay(MoveRDelay);
            MotorMoveRotateStop();

            

            MotorRotation -= step;

            if (MotorRotation < 0)
            {
                MotorRotation = 360 + MotorRotation;
            }

            int rotation = (int)MotorRotation;
            CurrentR.Text = rotation.ToString() + "\x00B0";
            
            //CurrentR.Text = MotorRotation.ToString() + "\x00B0";
            MotorRStepL.Checked = false;
        }

        public void MotorRStep()
        {
            step = 360 / Convert.ToDouble(Properties.Settings.Default.MSSStep);

            
        }

        private void MotorRStepR_Click(object sender, EventArgs e)
        {

            if (MotorMoving == 1)
            {
                MotorRStepR.Checked = false;
                return;
            }

            //double polepitch = 128 / 6;
            //polepitch = (34 / polepitch) * (42 / 14);


           // int MoveRDelay = (int)(polepitch * 25.7 * 1000.0);
            int MoveRDelay = (int)(polepitch * step);
            MotorMoveRotateStart(1);
            Delay(MoveRDelay);
            MotorMoveRotateStop();

            ;

            //MotorRotation -= strRoation;

            MotorRotation += step;

            if (MotorRotation > 359)
            {
                MotorRotation = MotorRotation - 360;
            }


            int rotation = (int)MotorRotation;
            CurrentR.Text = rotation.ToString() + "\x00B0";

            MotorRStepR.Checked = false;
        }

        private double MotorRotation = 0;
        private int RotationEnabel = 0;// r = 0 left, r= 1 right
        private void MotorMoveRotateStart(int r)
        {
            

            
            // r = 0 left, r= 1 right
            if (r == 0)
            {
                serialPort1.WriteLine("JR-");
                
            }
            else
            {
                serialPort1.WriteLine("JR+");
               
            }

        }

        private void MotorMoveRotateStop()
        {

            serialPort1.WriteLine("STOP");

        }

        private void MotorRReset_Click(object sender, EventArgs e)
        {
            MotorRotation = 0;

            CurrentR.Text = MotorRotation.ToString() + "\x00B0";
            MotorRReset.Checked = false;
        }

        System.Windows.Forms.Timer MotorRTimer;
        private void MotorRR_MouseDown(object sender, MouseEventArgs e)
        {
            if (MotorMoving == 1)
            {
                MotorRR.Checked = false;
                return;
            }

            
            MotorRTimer = new System.Windows.Forms.Timer();
            MotorRTimer.Tick += new EventHandler(MotorRotationChange);
            MotorRTimer.Interval = (int)(polepitch);

            serialPort1.WriteLine("JR+");
            RotationEnabel = 1;
            MotorRTimer.Start();
        }

        private void MotorRotationChange(object sender, EventArgs e)
        {
            if (RotationEnabel == 0)
            {
                MotorRotation--;
            }
            else
            {
                MotorRotation++;
            }

            if (MotorRotation < 0)
            {
                MotorRotation = 359;
            }

            if (MotorRotation > 359)
            {
                MotorRotation = 0;
            }

            int rotation = (int)MotorRotation;
            CurrentR.Text = rotation.ToString() + "\x00B0";
            //CurrentR.Text = MotorRotation.ToString() + "\x00B0";
        }

        private void MotorRR_MouseUp(object sender, MouseEventArgs e)
        {

            serialPort1.WriteLine("STOP");
            MotorRTimer.Stop();

            
            
            MotorRR.Checked = false;
        }

        private void MotorRL_MouseDown(object sender, MouseEventArgs e)
        {

            if (MotorMoving == 1)
            {
                MotorRL.Checked = false;
                return; 
            }

            MotorRTimer = new System.Windows.Forms.Timer();
            MotorRTimer.Tick += new EventHandler(MotorRotationChange);
            MotorRTimer.Interval = (int)(polepitch);

            serialPort1.WriteLine("JR-");
            RotationEnabel = 0;
            MotorRTimer.Start();
        }

        private void MotorRL_MouseUp(object sender, MouseEventArgs e)
        {
            serialPort1.WriteLine("STOP");
            MotorRTimer.Stop();

          
            MotorRL.Checked = false;
        }

        public bool MSSEnable = false;
        private void MotorSpdSettings_Click(object sender, EventArgs e)
        {
            if (MSSEnable)
            {
                MotorSpdSettings.Checked = false;
                return;
            }

            MotorSpeedSetting MSS = new MotorSpeedSetting(this);
            MSS.Show();
            MSSEnable = true;
        }


        public void MotorXLMove()
        {


            double Motorx = Convert.ToDouble(CurrentX.Text);
            double Motory = Convert.ToDouble(CurrentY.Text);

            Motorx = Motorx - 1;
            Motory = Motory + 0;

            string MoveX = string.Format("{0:00.000}", Motorx);
            string MoveY = string.Format("{0:00.000}", Motory);

            if (MoveX.Length < 7)
            {

                MoveX = MoveX.PadLeft(7, '+');
            }

            if (MoveY.Length < 7)
            {
                MoveY = MoveY.PadLeft(7, '+');
            }

            serialPort1.WriteLine("POS" + MoveX + MoveY);

        }

        public void MotorXRMove()
        {


            double Motorx = Convert.ToDouble(CurrentX.Text);
            double Motory = Convert.ToDouble(CurrentY.Text);

            Motorx = Motorx + 1;
            Motory = Motory + 0;

            string MoveX = string.Format("{0:00.000}", Motorx);
            string MoveY = string.Format("{0:00.000}", Motory);

            if (MoveX.Length < 7)
            {

                MoveX = MoveX.PadLeft(7, '+');
            }

            if (MoveY.Length < 7)
            {
                MoveY = MoveY.PadLeft(7, '+');
            }

            serialPort1.WriteLine("POS" + MoveX + MoveY);

        }

        public void MotorYTMove()
        {


            double Motorx = Convert.ToDouble(CurrentX.Text);
            double Motory = Convert.ToDouble(CurrentY.Text);

            Motorx = Motorx + 0;
            Motory = Motory - 1;






            string MoveX = string.Format("{0:00.000}", Motorx);
            string MoveY = string.Format("{0:00.000}", Motory);

            if (MoveX.Length < 7)
            {

                MoveX = MoveX.PadLeft(7, '+');
            }

            if (MoveY.Length < 7)
            {
                MoveY = MoveY.PadLeft(7, '+');
            }

            serialPort1.WriteLine("POS" + MoveX + MoveY);

        }

        public void MotorYBMove()
        {


            double Motorx = Convert.ToDouble(CurrentX.Text);
            double Motory = Convert.ToDouble(CurrentY.Text);

            Motorx = Motorx + 0;
            Motory = Motory + 1;

            string MoveX = string.Format("{0:00.000}", Motorx);
            string MoveY = string.Format("{0:00.000}", Motory);

            if (MoveX.Length < 7)
            {

                MoveX = MoveX.PadLeft(7, '+');
            }

            if (MoveY.Length < 7)
            {
                MoveY = MoveY.PadLeft(7, '+');
            }

            serialPort1.WriteLine("POS" + MoveX + MoveY);

        }



        public void StageTextChange(int motorNum, double value)
        {
            if (!serialPort1.IsOpen) { return; }


            string str = null;


            string valueStr = null;

            int MotorValue = 0;

            int speed = 0;


            switch (motorNum)
            {
                case 1:

                    value -= Properties.Settings.Default.MotorXOffset;
                    MotorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorXPitch));

                    if (Properties.Settings.Default.MotorXDirection)
                    {
                        MotorValue = MotorValue * -1;
                    }

                    if (CalibrationEnable)
                    {
                        speed = Properties.Settings.Default.MotorSpeedMax;
                    }
                    else
                    {
                        speed = Properties.Settings.Default.MotorSpeed;
                    }
                    

                    break;

                case 2:
                    value -= Properties.Settings.Default.MotorYOffset;
                    MotorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorYPitch));
                    if (Properties.Settings.Default.MotorYDirection)
                    {
                        MotorValue = MotorValue * -1;
                    }

                    if (CalibrationEnable)
                    {
                        speed = Properties.Settings.Default.MotorSpeedMax;
                    }
                    else
                    {
                        speed = Properties.Settings.Default.MotorSpeed;
                    }
                    break;

                case 3:
                    value -= Properties.Settings.Default.MotorROffset;
                    MotorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorRPitch));
                    if (Properties.Settings.Default.MotorRDirection)
                    {
                        MotorValue = MotorValue * -1;
                    }
                    speed = Properties.Settings.Default.MotorRspeed;

                    break;

                case 4:
                    value -= Properties.Settings.Default.MotorTOffset;
                    MotorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorTPitch));
                    if (Properties.Settings.Default.MotorTDirection)
                    {
                        MotorValue = MotorValue * -1;
                    }
                    speed = Properties.Settings.Default.MotorTspeed;
                    break;

                case 5:
                    value -= Properties.Settings.Default.MotorZOffset;
                    MotorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorZPitch));
                    if (Properties.Settings.Default.MotorZDirection)
                    {
                        MotorValue = MotorValue * -1;
                    }

                    speed = Properties.Settings.Default.MotorZspeed;
                    break;

            }


            if (MotorValue < 0)
            {
                double x = MotorValue * -1;

                valueStr = x.ToString().PadLeft(7, '0');
                valueStr = valueStr.PadLeft(8, '-');
            }
            else
            {
                valueStr = MotorValue.ToString().PadLeft(8, '0');
            }



            str += "{M";
            str += motorNum.ToString();
            str += "S/";
            str += valueStr;
            str += "/";

            if (speed < 100)
            {
                str += "0";
            }

            if (speed < 10)
            {
                str += "0";
            }

            str += speed.ToString();
            str += "/r}";

            //str += "/002/r}";

            try
            {
                serialPort1.WriteLine(str);
            }
            catch
            {
                Thread.Sleep(20);
                Trace.WriteLine("Motor Delay----------------------------------------------------------");
                //serialPort1.WriteLine(str);
            }
            


        }

        public void StageStop(int motorNum)
        {

            if (!serialPort1.IsOpen) { return; }

            WriteEnable = true;
            while (readEnable)
            {
                Thread.Sleep(20);
            }

            switch(motorNum)
            {
                case 1:

                    serialPort1.WriteLine("{M1S/-1234567/100/s}");
                    break;
                case 2:
                    serialPort1.WriteLine("{M2S/-1234567/100/s}");
                    break;
                case 3:
                    serialPort1.WriteLine("{M3S/-1234567/100/s}");
                    break;
                case 4:
                    serialPort1.WriteLine("{M4S/-1234567/100/s}");
                    break;
                case 5:
                    serialPort1.WriteLine("{M5S/-1234567/100/s}");
                    break;
                case 6:
                    serialPort1.WriteLine("{M1S/-1234567/100/s}");
                    Thread.Sleep(20);
                    serialPort1.WriteLine("{M2S/-1234567/100/s}");
                    Thread.Sleep(20);
                    serialPort1.WriteLine("{M3S/-1234567/100/s}");
                    Thread.Sleep(20);
                    serialPort1.WriteLine("{M4S/-1234567/100/s}");
                    Thread.Sleep(20);
                    serialPort1.WriteLine("{M5S/-1234567/100/s}");


                    break;
            }
            WriteEnable = false;

        }

        public void StageAllHome()
        {
            WriteEnable = true;

            while (readEnable)
            {
                Thread.Sleep(10);
            }

            serialPort1.WriteLine("{M1S/00000000/002/r}");
            Thread.Sleep(20);
            serialPort1.WriteLine("{M2S/00000000/002/r}");
            Thread.Sleep(20);
            serialPort1.WriteLine("{M3S/00000000/002/r}");
            Thread.Sleep(20);
            serialPort1.WriteLine("{M4S/00000000/002/r}");
            Thread.Sleep(20);
            serialPort1.WriteLine("{M5S/00000000/002/r}");
            Thread.Sleep(20);

            Properties.Settings.Default.MotorXOffset = 0;
            Properties.Settings.Default.MotorYOffset = 0;
            Properties.Settings.Default.MotorROffset = 0;
            Properties.Settings.Default.MotorTOffset = 0;
            Properties.Settings.Default.MotorZOffset = 0;


            WriteEnable = false;
        }

        public void StageMotorZero(int motorname)
        {
            string value = "{M";
            value += motorname.ToString();
            value += "S/-1234567/100/z}";

            Thread.Sleep(100);
            serialPort1.WriteLine(value);
            
            switch (motorname)
            {
                case 1:
                    Properties.Settings.Default.MotorXOffset = 0;

                    break;

                case 2:
                    Properties.Settings.Default.MotorYOffset = 0;

                    break;

                case 3:
                    Properties.Settings.Default.MotorROffset = 0;

                    break;

                case 4:
                    Properties.Settings.Default.MotorTOffset = 0;
                  
                    break;

                case 5:
                    Properties.Settings.Default.MotorZOffset = 0;
                   
                    break;

                default:
                    break;
            }
        }

        public void StageMoveChange(int motornum, double value)
        {
            string str = "{M";
            string valueStr;
            int motorValue = 0;

            switch (motornum)
            {
                case 1:
                    value = Properties.Settings.Default.MotorStageXvalue + value - Properties.Settings.Default.MotorXOffset;
                    motorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorXPitch));
                   
                    break;

                case 2:
                    value = Properties.Settings.Default.MotorStageYvalue + value - Properties.Settings.Default.MotorYOffset;
                    motorValue = (int)(value * MotorPulse * (1 / Properties.Settings.Default.MotorYPitch));
                    break;

                default:
                    break;
            }

            if (motorValue < 0)
            {
                motorValue = motorValue * -1;

                valueStr = motorValue.ToString().PadLeft(7, '0');
                valueStr = valueStr.PadLeft(8, '-');
            }
            else
            {
                valueStr = motorValue.ToString().PadLeft(8, '0');
            }
            

            str += motornum.ToString();
            str += "S/";
            str += valueStr;
            str += "/";

            if (Properties.Settings.Default.MotorSpeed < 100)
            {
                str += "0";
            }

            if (Properties.Settings.Default.MotorSpeed < 10)
            {
                str += "0";
            }

            str += Properties.Settings.Default.MotorSpeed.ToString();
            str += "/r}";


            WriteEnable = true;

            while (readEnable)
            {
                Thread.Sleep(1);
            }

            //switch (motornum)
            //{
            //    case 1:
            //        StageStop(1);
            //        break;

            //    case 2:
            //        StageStop(2);
            //        break;

            //    default:
            //        break;
            //}

            try
            {
                serialPort1.WriteLine(str);
            }
            catch
            {
                Trace.WriteLine("prot write error ----------------------");
                Thread.Sleep(5);
                serialPort1.WriteLine(str);
            }
            

            WriteEnable = false;
            


        }

        public bool CalibrationEnable = false;

        public void Calibration()
        {
            CalibrationEnable = true;
            StageMotorZero(1);
            Thread.Sleep(50);
            StageMotorZero(2);
            Thread.Sleep(50);
            StageMotorZero(3);
            Thread.Sleep(50);
            StageMotorZero(4);
            Thread.Sleep(50);

            StageMotorZero(5);
            Thread.Sleep(50);


            WriteEnable = true;

            while (readEnable)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }
                Thread.Sleep(10);
                
            }

            StageStop(6);

            Thread.Sleep(50);

            StageTextChange(5, 100);

            Thread.Sleep(50);

            StageTextChange(4, -360);

            WriteEnable = false;

            while (true)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }

                if (MotorZvalue[3] == "L" && MotorTvalue[3] == "L")
                {
                    StageMotorZero(4);

                    Thread.Sleep(50);

                    StageMotorZero(5);
                    Thread.Sleep(50);
                    break;
                }

                
                Thread.Sleep(50);
            }

            WriteEnable = true;
            while (readEnable)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }
                Thread.Sleep(10);
            }

            Thread.Sleep(50);
            StageMotorZero(5);

            Thread.Sleep(50);

            StageMotorZero(4);

            Thread.Sleep(50);

            StageTextChange(2, -100);
            Thread.Sleep(50);

            StageTextChange(5, Properties.Settings.Default.MotorZHome);
            Thread.Sleep(50);

            WriteEnable = false;

            while (true)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }

                if (MotorYvalue[3] == "L")
                {
                    StageMotorZero(2);
                    Thread.Sleep(50);
                    break;
                }
                Thread.Sleep(50);
            }

            WriteEnable = true;
            while (readEnable)
            {

                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }
                Thread.Sleep(10);
            }

            StageTextChange(1, -100);

            WriteEnable = false;

            while (true)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }

                if (MotorXvalue[3] == "L")
                {
                    Thread.Sleep(50);
                    StageMotorZero(1);
                    Thread.Sleep(50);
                    break;
                }
                Thread.Sleep(50);
            }

            WriteEnable = true;

            StageTextChange(2, Properties.Settings.Default.MotorYHome);
            Thread.Sleep(50);

            StageTextChange(1, Properties.Settings.Default.MotorXHome);
            Thread.Sleep(50);

            WriteEnable = false;


            while (true)
            {
                if (!CalibrationEnable)
                {
                    StageStop(6);
                    return;
                }

                if (Math.Round(Properties.Settings.Default.MotorStageXvalue) == Properties.Settings.Default.MotorXHome && Math.Round(Properties.Settings.Default.MotorStageYvalue) == Properties.Settings.Default.MotorYHome)
                {
                    WriteEnable = true;
                    Thread.Sleep(2000);
                    StageMotorZero(1);
                    Thread.Sleep(100);

                    StageMotorZero(2);
                    Thread.Sleep(100);

                    StageMotorZero(5);
                    Thread.Sleep(100);

                    WriteEnable = false;
                    break;
                }
            }

            

            CalibrationEnable = false;

        }
       
        
    }
}
