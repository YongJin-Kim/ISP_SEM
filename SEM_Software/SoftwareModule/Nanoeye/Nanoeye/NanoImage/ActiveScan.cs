using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SEC.Nanoeye.NanoImage.DataAcquation;
using SEC.Nanoeye.NanoImage.DataAcquation.IODA;
using SEC.Nanoeye.NanoImage.DataAcquation.NIDaq;
using SEC.Nanoeye.NanoImage.ScanItem;
using SEC.Nanoeye.NanoImage.DataAcquation.CSTDaq;

namespace SEC.Nanoeye.NanoImage
{
	class ActiveScan : IActiveScan, IDisposable
	{
		#region Property & Variables
		enum ScannerMessageType
		{
			Start,
			Stopping,
			Stop
		}

		struct ScannerMessageStrucnt
		{
			public readonly ScannerMessageType msg;
			public readonly object parameter;

			public ScannerMessageStrucnt(ScannerMessageType sct, object par)
			{
				msg = sct;
				parameter = par;
			}
		}

		/// <summary>
		/// 주사 동작을 
		/// </summary>
		System.ComponentModel.BackgroundWorker scannerBW;

		DataAcquation.IDAQ dataAcqurator;

		private string _DaqDevice = null;
		public string DaqDevice
		{
			get { return _DaqDevice; }
		}

		public bool IsRun
		{
			get { return (_ItemsRunning == null) ? false : true; }
		}

		protected IScanItemEvent[] _ItemsRunning = null;
		public IScanItemEvent[] ItemsRunning
		{
			get { return _ItemsRunning; }
		}
		protected IScanItem[] _ItemsReady = null;
		public IScanItemEvent[] ItemsReady
		{
			get { return (IScanItemEvent[])_ItemsReady; }
		}

        
		#endregion

		#region Event
		public event EventHandler  ScanningStarted;
		protected virtual void OnScanningStarted()
		{
			if (ScanningStarted != null)
			{
				ScanningStarted(this, EventArgs.Empty);
			}
		}

		public event EventHandler ScanningStopped;
		protected virtual void OnScanningStopped()
		{
			if (ScanningStopped != null)
			{
				ScanningStopped(this, EventArgs.Empty);
			}
		}

		public event EventHandler  ScanningStopping;
		protected virtual void OnScanningStopping()
		{
			if (ScanningStopping != null)
			{
				ScanningStopping(this, EventArgs.Empty);
			}
		}
		#endregion
		
		#region 생성자 & 소멸자 & Dispose

		public ActiveScan()
		{
			scannerBW = new System.ComponentModel.BackgroundWorker();
			scannerBW.DoWork += new System.ComponentModel.DoWorkEventHandler(scannerBW_DoWork);
			scannerBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(scannerBW_RunWorkerCompleted);
			scannerBW.WorkerSupportsCancellation = true;
		}

		~ActiveScan()
		{
			Dispose();
		}

		public void Dispose()
		{
			Stop(true);

			if (_ItemsReady != null)
			{
				foreach (IScanItem si in _ItemsReady)
				{
					si.Dispose();
				}
				_ItemsReady = null;
			}

			if (_ItemsRunning != null)
			{
				foreach (IScanItem si in _ItemsRunning)
				{
					si.Dispose();
				}
				_ItemsRunning = null;
			}
		}
		#endregion

		public string[] GetDevList()
		{
			List<string> devs = new List<string>();


			string[] tmp;


            for (int i = 0; i < 5; i++)
            {

                switch (i)
                {
                    case 0:
                        try
                        {
                            tmp = NIDaq6251.SearchNIDaq6251();

                            foreach (string str in tmp)
                            {
                                devs.Add("DAQ6251" + str);
                            }
                        }
                        catch
                        {
                            //tmp = null;
                        }
                        
                        break;

                    case 1:
                        try
                        {
                            tmp = NIDaq6351.SearchNIDaq6351();

                            foreach (string str in tmp)
                            {
                                devs.Add("DAQ6351" + str);
                            }
                        }
                        catch
                        {
                            //tmp = null;
                        }
                        break;

                    case 2:
                        try
                        {
                            tmp = NIDaq6361.SearchNIDaq6361();
                            foreach (string str in tmp)
                            {
                                devs.Add("DAQ6361" + str);
                            }

                        }
                        catch
                        {
                            //tmp = null;
                        }
                        
                        break;

                    case 3:

                        try
                        {
                             tmp = NIDaq6353.SearchNIDaq6353();
                            foreach (string str in tmp)
                            {
                                devs.Add("DAQ6353" + str);
                            }
                        }
                        catch
                        {
                            //tmp = null;
                        }
                       
                        break;

                    case 4:
                        try
                        {
                            tmp = CSTDaq.SearchCSTDaq();
                            foreach (string str in tmp)
                            {
                                devs.Add(str);
                                _DaqDevice = "CSTDaq";
                            }
                        }
                        catch
                        {

                        }

                        break;
                } 
               
                //if(tmp
               
               
            }

           
         

#if DEBUG
            // DAQ 장치가 하나도 없는 경우 가상의 장치를 장착 하여 테스트가 가능 하도록 한다.
            if (devs.Count == 0) { devs.Add("TSTDevice"); }
            //devs.Add("TSTDevice");
#endif

			return devs.ToArray();
		}

        public void Revers(bool enable)
        {
            if (dataAcqurator.ToString() != "IODAUSB") { return; }

            dataAcqurator.Revers = enable;
        }

        public void ScanMode(bool detector)
        {

            if (dataAcqurator.ToString() != "IODAUSB") { return; }

            if (detector)
            {
                dataAcqurator.AiChannel = 1;
            }
            else
            {
                dataAcqurator.AiChannel = 0;
            }

            
        }

        public void DualMode(bool DualEnable)
        {
            dataAcqurator.DualEnable = DualEnable;
        }

		public void Initialize(string dev)
		{
			Trace.Assert(dev != null);

			_ItemsRunning = null;

			switch (dev.Substring(0, 3))
			{
			case "TST":
				dataAcqurator = new VirtualDAQ();
				break;
			case "DAQ":
				switch (dev.Substring(0, 7))
				{
				case "DAQ6251":
					dataAcqurator = new NIDaq6251(dev.Substring(7));
					break;

                case "DAQ6351":
                       
                    dataAcqurator = new NIDaq6351(dev.Substring(7));
                    break;

				case "DAQ6361":
					dataAcqurator = new NIDaq6361(dev.Substring(7));
					break;

                case "DAQ6353":
                    dataAcqurator = new NIDaq6353(dev.Substring(7));
                    break;
				}
				break;
			case "IOD":
				dataAcqurator = new IODAUsb(dev.Substring(7));
				break;

            case "CST":
                dataAcqurator = new CSTDaq(dev.Substring(0));
                break;
			default:
				throw new ArgumentException("Undefined Device.");
			}
		}

		public bool IsSequenceRunable(SettingScanner[] settings)
		{
			throw new NotImplementedException();
		}

		public void Stop(bool immediately)
		{
			if (!scannerBW.IsBusy) { return; }

			OnScanningStopping();
            

			if (immediately)
			{
				scanRestartRequest = false;
				scannerBW.CancelAsync();
				scannerStopMre.WaitOne();
			}
			else
			{
				scanStopRequeset = true;
			}
		}

		#region Scanner
		bool scanRestartRequest = false;
		bool scanStopRequeset = false;
		System.Threading.ManualResetEvent scannerStopMre = new System.Threading.ManualResetEvent(false);

		void scannerBW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			OnScanningStopped();

			if (e.Error != null)
			{
				dataAcqurator.Reset();
				throw new Exception("Scanner Stop Error.", e.Error);
			}

			if (scanRestartRequest)
			{
				scanRestartRequest = false;
				scannerBW.RunWorkerAsync();
			}
		}

		void scannerBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			scannerStopMre.Reset();

			System.Threading.Thread.CurrentThread.Name = "Scanner Thread";

			System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

			_ItemsRunning = _ItemsReady;
			_ItemsReady = null;

			dataAcqurator.Change();

			OnScanningStarted();

			try
			{
				while (!worker.CancellationPending)
				{
                    
					foreach (IScanItem si in _ItemsRunning)
					{
                         
						si.Scanning(scannerBW);

						if (worker.CancellationPending) { break; }
					}
					if (scanStopRequeset)
					{
						scanStopRequeset = false;
						break;
					}
				}
			}
			finally
			{
				dataAcqurator.Stop();

				foreach (IScanItem si in _ItemsRunning)
				{
					si.Dispose();
				}
				_ItemsRunning = null;

				scannerStopMre.Set();
			}
		}
		#endregion

		public void ValidateSetting(SettingScanner set)
		{
			StringBuilder msg = new StringBuilder();
			StringBuilder arg = new StringBuilder();

			try { dataAcqurator.ValidateSetting(set); }
			catch (ArgumentException ae)
			{
				msg.AppendLine(ae.Message);
				arg.AppendLine(ae.ParamName);
			}

			// TODO : 어떤 ScanItema을 사용할 것인가?
			try { SI_SC_LA_Blur.ValidateSetting(set); }
			catch (ArgumentException ae)
			{
				msg.AppendLine(ae.Message);
				arg.AppendLine(ae.ParamName);
			}

			if (msg.Length > 0)
			{
				throw new ArgumentException(msg.ToString(), arg.ToString());
			}
		}

		public void Ready(SettingScanner[] settings, int count)
		{
			foreach (SettingScanner set in settings)
			{
				try
				{
					ValidateSetting(set);
				}
				catch (ArgumentException are)
				{
					int index = Array.BinarySearch<SettingScanner>(settings, set);
					throw new ArgumentException("Invalied Setting.", "settings at " + index.ToString(), are);
				}
			}

			if (settings.Length > 1)
			{
				if (!IsSequenceRunable(settings))
				{
					throw new ArgumentException("settings is not sequence run able.", "settings");
				}
			}

			List<IScanItem> siList = new List<IScanItem>();
			foreach (SettingScanner ss in settings)
			{
				// TODO : 어떤 Scan Item을 사용할 것인가?
				//IScanItem si = new SI_SC_LA_Median(ss, dataAcqurator);	// 사용 금지. 동작 하지 않음.
				IScanItem si = new SI_SC_LA_Blur(ss, dataAcqurator);
				si.Ready();
				siList.Add(si);
			}
			_ItemsReady = siList.ToArray();

			dataAcqurator.Ready(settings);

			Debug.WriteLine("Ready - " + settings[0].Name, "Scanner");
		}

		public void Change()
		{
			if(_ItemsReady == null) { throw new ArgumentException("ItemsReady is null!!!"); }
			if (scannerBW.IsBusy)
			{
				scanRestartRequest = true;
				scannerBW.CancelAsync();
			}
			else
			{
				scannerBW.RunWorkerAsync();
			}

			Debug.WriteLine("Change " , "Scanner");
		}

		public void ShowInformation(System.Windows.Forms.IWin32Window owner)
		{
			dataAcqurator.ShowInformation(owner);
		}

		public void OnePoint(double horizontal, double vertical)
		{
            
			Stop(true);

			dataAcqurator.OnePoint(horizontal, vertical);
		}

        public void AFTest(string path)
        {
            int samplecount = _ItemsRunning[0].Setting.FrameWidth * _ItemsRunning[0].Setting.FrameHeight * _ItemsRunning[0].Setting.SampleComposite * _ItemsRunning[0].Setting.LineAverage;

            short[,] input = dataAcqurator.Read(samplecount);
            System.Drawing.Bitmap afimage = new System.Drawing.Bitmap(_ItemsRunning[0].Setting.FrameWidth, _ItemsRunning[0].Setting.FrameHeight);
            //System.Drawing.Bitmap afimage = new System.Drawing.Bitmap(320, 240);

            int count = 0;

            int[,] img = new int[_ItemsRunning[0].Setting.FrameHeight, _ItemsRunning[0].Setting.FrameWidth];

            for (int i = 0; i < _ItemsRunning[0].Setting.FrameHeight; i++)
            {
                for (int j = 0; j < _ItemsRunning[0].Setting.FrameWidth; j++)
                {
                    img[i, j] = input[0, count] % 256;

                    if (img[i, j] < 0)
                    {
                        img[i, j] = 0;
                    }

                    if (img[i, j] > 255)
                    {
                        img[i, j] = 255;
                    }

                    afimage.SetPixel(j, i, System.Drawing.Color.FromArgb(img[i, j], img[i, j], img[i, j]));
                    count++;
                }
            }

            
            
            afimage.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
            afimage.Dispose();
          

        }

        public void scanFrameStop()
        {
            dataAcqurator.OnePoint(0, 0);
        }

    
	}
}
