using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

using SECtype = SEC.GenericSupport.DataType;

using System.Xml;

namespace SEC.Nanoeye.NanoeyeSEM.Initialize
{
	public class Initializer
	{
		public MiniSEM form = null;

		private Initializer()
		{
		}

		internal static void Initialize(MiniSEM miniSEM)
		{

			Initializer init = new Initializer();


            init.ProcessKill();

			init.form = miniSEM;

			TextManager.Instance.LoadStringData(@".\textdata.xml");

			init.SearchController();

			init.Profile();

			miniSEM.InitializedDevice();

			init.ActiveScan();

			init.Display(miniSEM);
		}

        private void ProcessKill()
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("Mini-SEM");

            if (p.GetLength(0) > 1)
            {
                for (int i = 1; i < p.GetLength(0); i++)
                {
                    if (p[i - 1].StartTime.Hour == p[i].StartTime.Hour)
                    {
                        if (p[i - 1].StartTime.Minute == p[i].StartTime.Minute)
                        {

                            if (p[i - 1].StartTime.Second == p[i].StartTime.Second)
                            {

                            }
                            else
                            {
                                if (p[i - 1].StartTime.Second < p[i].StartTime.Second)
                                {
                                    p[i - 1].Kill();
                                }
                                else
                                {
                                    p[i].Kill();
                                }
                            }
                        }
                        else
                        {
                            if (p[i - 1].StartTime.Minute < p[i].StartTime.Minute)
                            {
                                p[i - 1].Kill();
                            }
                            else
                            {
                                p[i].Kill();
                            }
                        }
                    }
                    else
                    {
                        if (p[i - 1].StartTime.Hour < p[i].StartTime.Hour)
                        {
                            p[i - 1].Kill();
                        }
                        else
                        {
                            p[i].Kill();
                        }
                    }
                }
            }


            
        }
       

		private void Display(MiniSEM miniSEM)
		{
			Splash.Default.TxtMessage = "Display User Interface";

			miniSEM.InitiDisplay();
		}

		private void ActiveScan()
		{
			Splash.Default.TxtMessage = "Loading ActiveScan...";

			SEC.Nanoeye.NanoImage.IActiveScan scanner = SystemInfoBinder.Default.Nanoeye.Scanner;

			string[] devList = scanner.GetDevList();
			switch (devList.Length)
			{
			case 0:
				throw new NotSupportedException(TextManager.Instance.GetString("Message_DAQEmpty").Text);
			case 1:
				scanner.Initialize(devList[0]);
				break;
			default:
				DeviceSelector ds = new DeviceSelector();
				ds.comboBox1.Items.AddRange(devList);
				if (ds.ShowDialog(Splash.Default) == DialogResult.OK)
				{
					string com = (string)ds.comboBox1.SelectedItem;

					Trace.WriteLine("DAQ Selected : " + com, "Info");

					scanner.Initialize(com);
				}
				else
				{
					Trace.WriteLine( "Scanning device was not selected.", "Initializer");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_DAQNotSelect").Text);
				}
				break;
			}

			//Kikwak.ProjectSEM.Settings.ScanningProfile profile = new Kikwak.ProjectSEM.Settings.ScanningProfile();

			//for (int i = 0; i < 10; i++)
			//{
			//    profile.SettingsKey = "ScanningProfile" + i.ToString();
			//    profile.Reload();

			//ss.AiChannel = profile.VideoChannel;
			//ss.AiClock = profile.SampleClock;
			//ss.AiDifferential = profile.VideoDifferential;
			//ss.AiMaximum = profile.DeflectionMaximum;
			//ss.AiMinimum = profile.DeflectionMinimum;
			//ss.AoClock = profile.SampleClock / 2;
			//ss.AoMaximum = 10f;
			//ss.AoMinimum = -10f;
			//ss.AreaShiftX = profile.AreaChangeX;
			//ss.AreaShiftY = profile.AreaChangeY;
			//ss.AverageLevel = profile.MeanLevel;
			//ss.BlurLevel = profile.BlurLevel;
			//ss.FrameHeight = profile.FrameSize.Height;
			//ss.FrameWidth = profile.FrameSize.Width;
			//ss.ImageHeight = profile.ImageBounds.Height;
			//ss.ImageLeft = profile.ImageBounds.Left;
			//ss.ImageTop = profile.ImageBounds.Top;
			//ss.ImageWidth = profile.ImageBounds.Width;
			//ss.LineAverage = 1;
			//ss.PaintHeight = profile.PaintBounds.Height / 480f * 0.75f;
			//ss.PaintWidth = profile.PaintBounds.Width / 640f;
			//ss.PaintX = profile.PaintBounds.X / 640f;
			//ss.PaintY = profile.PaintBounds.Y / 480f * 0.75f;
			//ss.PropergationDelay = profile.PropargationDelayX;
			//ss.RatioX = profile.ScanRatioX;
			//ss.RatioY = profile.ScanRatioY;
			//ss.SampleComposite = profile.SampleComposite;
			//ss.ShiftX = profile.ScanShiftX;
			//ss.ShiftY = profile.ScanShiftY;
			//}
		}

		private void Profile()
		{
			Splash.Default.TxtMessage = "Loading Profiles";

			switch (SystemInfoBinder.Default.AppDevice)
			{
			case AppDeviceEnum.SNE1500M:
				SystemInfoBinder.Default.SetManager = new Settings.MiniSEM.ManagerMiniSEM();
				break;
            case AppDeviceEnum.SNE3000M:
			case AppDeviceEnum.SNE4000M:
            case AppDeviceEnum.SNE4500M:
            case AppDeviceEnum.SNE3200M:
            case AppDeviceEnum.SNE3000MS:
            case AppDeviceEnum.SNE3000MB:
            case AppDeviceEnum.SH4000M:
            case AppDeviceEnum.SH3500MB:
            case AppDeviceEnum.SH5000M:
            case AppDeviceEnum.AutoDetect:
				SystemInfoBinder.Default.SetManager = new Settings.SNE4000M.Manager4000M();
				break;
            case AppDeviceEnum.SNE4500P:
                SystemInfoBinder.Default.SetManager = new Settings.AIOsem.Manager4500P();
                break;

			
			default:
				throw new NotSupportedException("Device is not defined.");
			}

			SystemInfoBinder.Default.SetManager.Load(SystemInfoBinder.Default.SettingFileName);
		}

		private void SearchController()
		{
			switch (SystemInfoBinder.Default.AppSeller)
			{
			case AppSellerEnum.AutoDetect:
			case AppSellerEnum.SEC:
			case AppSellerEnum.Hirox:
				Splash.Default.TxtMessage = "Searching Mini-SEM...";
				break;
			case AppSellerEnum.Evex:
				Splash.Default.TxtMessage = "Searching Evex Mini-SEM...";
				break;
			case AppSellerEnum.Nikkiso:
				Splash.Default.TxtMessage = "Searching SEMTRAC mini...";
				break;
			default:
				throw new ArgumentException();
			}

			//string[,] devices = SEC.Nanoeye.NanoColumn.Helper.EnumerateDevice();
            string[,] devices = null;
			if (devices == null)
			{
				if (SystemInfoBinder.Default.AppMode == AppModeEnum.Debug)
				{
                    #if DEBUG

                    SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3200M;
                    #endif
                    
					switch (SystemInfoBinder.Default.AppSeller)
					{
					case AppSellerEnum.SEC:
					case AppSellerEnum.Hirox:
                        if (SystemInfoBinder.Default.AppDevice == AppDeviceEnum.SNE3200M)
						{
                            SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE3200M);
						}
						else
						{
							SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.MiniSEM);
						}
						break;
					case AppSellerEnum.Evex:
						SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.Evex_MiniSEM);
						break;
					case AppSellerEnum.Nikkiso:
						SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SEMTRAC_mini);
						break;
					case AppSellerEnum.AutoDetect:
					default:
						throw new NotSupportedException("Debug mode. but AppMode is not setted.");
					}
					SystemInfoBinder.Default.Nanoeye.Controller.Initialize();
				}
				else
				{
					throw new NotSupportedException(TextManager.Instance.GetString("Message_ControllerEmpty").Text);
				}
			}
			else
			{
		
				//devices[0, 1] = "SNE-4000M";
				switch (devices.GetLength(0))
				{
				case 0:
					throw new NotSupportedException(TextManager.Instance.GetString("Message_ControllerEmpty").Text);
				case 1:
                    Trace.WriteLine("-----------------------------------------------------------------");    

					DefineDevice(devices[0, 0], devices[0, 1]);
					break;
				default:
					DeviceSelector ds = new DeviceSelector();
					ds.Devices = devices;
					if (ds.ShowDialog(Splash.Default) == DialogResult.OK)
					{
						DefineDevice(devices[ds.SelectedIndex, 0], devices[ds.SelectedIndex, 1]);
					}
					else
					{
						Trace.WriteLine("There are more then two SEM controller. But user didn't select. Program will be shutdown.", "Initializer");
						throw new NotSupportedException(TextManager.Instance.GetString("Message_ControllerNotSelect").Text);
					}
					break;
				}
			}

			string formText = SystemInfoBinder.GetEquipmentName() + " - " + SystemInfoBinder.GetCompaneyName();

			if (SystemInfoBinder.Default.AppSeller == AppSellerEnum.SEC)
			{
				formText = "Nanoeye " + formText;
			}

			form.Text = formText;
            

			Splash.Default.TxtModel = SystemInfoBinder.GetEquipmentName();
			//formSplash.companey = SystemInfoBinder.GetCompaneyName();


			switch (SystemInfoBinder.Default.AppDevice)
			{
			case AppDeviceEnum.SNE1500M:
				SystemInfoBinder.Default.SettingFileName = System.Windows.Forms.Application.CommonAppDataPath + @".\NanoeyeSEM.config";
				break;
            case AppDeviceEnum.SNE3000M:
			case AppDeviceEnum.SNE4000M:
            case AppDeviceEnum.SNE4500M:
            case AppDeviceEnum.SNE4500P:
            case AppDeviceEnum.SNE3200M:
            case AppDeviceEnum.SNE3000MS:
            case AppDeviceEnum.SNE3000MB:
            case AppDeviceEnum.SH4000M:
            case AppDeviceEnum.SH3500MB:
            case AppDeviceEnum.SH5000M:
            case AppDeviceEnum.AutoDetect:
				SystemInfoBinder.Default.SettingFileName = System.Windows.Forms.Application.CommonAppDataPath + @".\NanoeyeSEM.bin";
				break;
			
			default:
				throw new NotSupportedException();
			}
		}

		private void DefineDevice(string port, string equ)
		{
			switch (equ)
			{
			case "SNE-1500M":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE1500M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE1500M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.MiniSEM);

				break;
			case "SH-1500":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.Hirox))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE1500M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.Hirox;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE1500M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.MiniSEM);
				break;

			case "SNE-3000M":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE3000M);
				break;
			case "SH-3000":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.Hirox))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.Hirox;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.MiniSEM);
				break;
            case "SH-3500MB":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.Hirox))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.Hirox;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SH3500MB;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4500M);
                break;
            case "SH-4000M":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.Hirox))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.Hirox;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SH4000M;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4500M);
                break;
            case "SH-5000M":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.Hirox))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.Hirox;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SH5000M;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4500M);
                break;

			case "Evex MiniSEM":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.Evex))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.Evex;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.Evex_MiniSEM);
				break;

			case "SEMTRAC mini":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.Nikkiso))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.Nikkiso;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SEMTRAC_mini);
				break;

			case "SNE-4000M":
				if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
				}
				if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
					(SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE4000M))
				{
					Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
					throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
				}

				SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
				SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE4000M;
				SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4000M);
				break;

            case "SNE-4500M":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE4500M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE4500M;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4500M);
                break;
            case "SNE-4500P":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE4500M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE4500P;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE4500P);
                break;

            case "SNE-3000MB":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3000MB))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000MB;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE3000MB);
                break;

            case "SNE-3200M":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3200M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3200M;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE3200M);
                break;
            case "SNE-3000MS":
                if ((SystemInfoBinder.Default.AppSeller != AppSellerEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppSeller != AppSellerEnum.SEC))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidSeller").Text);
                }
                if ((SystemInfoBinder.Default.AppDevice != AppDeviceEnum.AutoDetect) &&
                    (SystemInfoBinder.Default.AppDevice != AppDeviceEnum.SNE3200M))
                {
                    Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppDevice.ToString(), "Error");
                    throw new NotSupportedException(TextManager.Instance.GetString("Message_InvalidDevice").Text);
                }

                SystemInfoBinder.Default.AppSeller = AppSellerEnum.SEC;
                SystemInfoBinder.Default.AppDevice = AppDeviceEnum.SNE3000MS;
                SystemInfoBinder.Default.Nanoeye = SEC.Nanoeye.NanoeyeFactory.CreateInstance(SEC.Nanoeye.NanoeyeFactory.NanoeyeType.SNE3000MS);
                break;

			case "SNE-5000M":
			case "SNE-5001M":
			case "Unknown":
			default:
				Trace.WriteLine(equ + " vs " + SystemInfoBinder.Default.AppSeller.ToString(), "Error");
				throw new NotSupportedException(TextManager.Instance.GetString("Message_Unsupported").Text);
			}

			SystemInfoBinder.Default.Nanoeye.ControllerCommunicator.PortName = port;
			SystemInfoBinder.Default.Nanoeye.ControllerCommunicator.Open();

			SystemInfoBinder.Default.Nanoeye.Controller.Viewer = SystemInfoBinder.Default.Nanoeye.ControllerCommunicator;
			SystemInfoBinder.Default.Nanoeye.Controller.Initialize();

			Trace.WriteLine(string.Format("Controller inited. Port - {0}, Equipment - {1}", port, equ), "Initializer");
		}

		#region 실제 화면 비율 확인 하는 파트.
		// 해상도 별로 물리적 거리가 바뀌어서 확인 불가.

		//[DllImport("gdi32.dll")]
		//public static extern int GetDeviceCaps(
		//    IntPtr hdc,     // handle to DC
		//    int nIndex   // index of capability
		//);

		//private void ResizeImageWindow()
		//{
		//    Graphics g = this.CreateGraphics();
		//    IntPtr dc = g.GetHdc();

		//    int xmm, ymm, xpixel, ypixel;

		//    xmm = GetDeviceCaps(dc, 4);	// phisical with (mm)
		//    ymm = GetDeviceCaps(dc, 6);	// phisical heigth (mm)
		//    xpixel = GetDeviceCaps(dc, 8);	// with resolution
		//    ypixel = GetDeviceCaps(dc, 10);	// height reslution

		//    double xdpmm = (double)xpixel / (double)xmm;	// real with resolution
		//    double ydpmm = (double)ypixel / (double)ymm;	// real hegith resolution

		//    double ratio = ydpmm / xdpmm;


		//    if (ratio == 0.75){	// 4:3 비율
		//        // 아무 것도 안함.
		//    }
		//    else if ( ratio > 0.75 ) {	// Y축으로 김.
		//        double heigth = 480.0d / ratio;
		//        m_ScreenWindow.Height = (int)(heigth);
		//    }
		//    else {	// X 축으로 김.
		//        double width = 640 * ratio;
		//        m_ScreenWindow.Width = (int)(width);
		//    }

		//    AutoFunctionCollection.DebugginLog.Instance.Show();
		//    AutoFunctionCollection.DebugginLog.Instance.AddString(m_ScreenWindow.Size.ToString(), "ScreenWindowSize");
		//}
		#endregion
	}
}
