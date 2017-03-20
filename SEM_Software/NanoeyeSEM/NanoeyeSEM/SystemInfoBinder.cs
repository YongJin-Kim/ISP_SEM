using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SEC.Nanoeye.NanoeyeSEM
{
    class SystemInfoBinder : INotifyPropertyChanged, SEC.Nanoeye.Support.AutoFunction.IVideoVlaue
    {
        public static string[] ScanNames ={	"Fast Scan",
											"Slow Scan",
											"Fast Photo",
											"Slow Photo",
											"Auto Focus",
											"Auto Contrast Brightness",
											"Auto Stig Focus",
											"Scan Pause",
											"Spot Mode",
											"Auto Focus 2",
                                            "Slow Photo2"};

        private static SystemInfoBinder _Default = new SystemInfoBinder();
        public static SystemInfoBinder Default
        {
            get { return _Default; }
        }

        #region INotifyPropertyChanged 멤버

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        public enum ImageSourceEnum
        {
            SED,
            BSED,
            DualSEBSE,
            Merge,
            Camera
        }

        private ImageSourceEnum _DetectorMode = ImageSourceEnum.SED;
        public ImageSourceEnum DetectorMode
        {
            get { return _DetectorMode; }
            set
            {
                if (_DetectorMode != value)
                {
                    System.Diagnostics.Trace.Assert(((value == ImageSourceEnum.SED) ? (_VacuumMode == VacuumModeEnum.HighVacuum) : true));
                    _DetectorMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DetectorMode"));
                }
            }
        }



        public enum VacuumModeEnum
        {
            HighVacuum,
            LowVacuum
        }

        private VacuumModeEnum _VacuumMode = VacuumModeEnum.HighVacuum;
        public VacuumModeEnum VacuumMode
        {
            get { return _VacuumMode; }
            set
            {
                if (_VacuumMode != value)
                {
                    _VacuumMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("VacuumMode"));
                }
            }
        }

        private int _ScanSource = 0;
        public int ScanSource
        {
            get { return _ScanSource; }
            set
            {
                if (_ScanSource != value)
                {
                    _ScanSource = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ScanSource"));
                }
            }
        }

        private bool _DZenabled = false;
        public bool DZenabled
        {
            get { return _DZenabled; }
            set
            {
                if (_DZenabled != value)
                {
                    _DZenabled = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DZenabled"));
                }
            }
        }

        #region Image 관련
        private int _ImageContrast = 0;
        public int Contrast
        {
            get { return _ImageContrast; }
            set
            {
                if (_ImageContrast != value)
                {
                    _ImageContrast = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ImageContrast"));
                }

            }
        }

        private int _ImageBrightness = 0;
        public int Brightness
        {
            get { return _ImageBrightness; }
            set
            {
                if (_ImageBrightness != value)
                {
                    _ImageBrightness = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ImageBrightness"));
                }
            }
        }

        private int _ImageContrast2 = 0;
        public int Contrast2
        {
            get { return _ImageContrast2; }
            set
            {
                if (_ImageContrast2 != value)
                {
                    _ImageContrast2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ImageContrast2"));
                }

            }
        }

        private int _ImageBrightness2 = 0;
        public int Brightness2
        {
            get { return _ImageBrightness2; }
            set
            {
                if (_ImageBrightness2 != value)
                {
                    _ImageBrightness2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ImageBrightness2"));
                }
            }
        }

        private bool _ImageExportable = false;
        public bool ImageExportable
        {
            get { return _ImageExportable; }
            set
            {
                if (_ImageExportable != value)
                {
                    _ImageExportable = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ImageExportable"));
                }
            }
        }
        #endregion

        #region 프로그램 설정
        private AppDeviceEnum _AppDevice = AppDeviceEnum.AutoDetect;
        public AppDeviceEnum AppDevice
        {
            get { return _AppDevice; }
            set
            {
                if (_AppDevice != value)
                {
                    _AppDevice = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AppDevice"));
                }
            }
        }

        private AppSellerEnum _AppSeller = AppSellerEnum.AutoDetect;
        public AppSellerEnum AppSeller
        {
            get { return _AppSeller; }
            set
            {
                if (_AppSeller != value)
                {
                    _AppSeller = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AppSeller"));
                }
            }
        }

        private AppModeEnum _AppMode = AppModeEnum.Run;
        public AppModeEnum AppMode
        {
            get { return _AppMode; }
            set
            {
                if (_AppMode != value)
                {
                    _AppMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AppMode"));
                }
            }
        }

        public static string GetCompaneyName()
        {
            switch (SystemInfoBinder.Default.AppSeller)
            {
                case AppSellerEnum.AutoDetect:
                case AppSellerEnum.SEC:
                default:
                    return "SEC";
                case AppSellerEnum.Evex:
                    return "Evex";
                case AppSellerEnum.Hirox:
                    return "Hirox";
                case AppSellerEnum.Nikkiso:
                    return "Nikkiso";
            }
        }

        public static string GetEquipmentName()
        {
            switch (SystemInfoBinder.Default.AppSeller)
            {
                case AppSellerEnum.AutoDetect:
                case AppSellerEnum.SEC:
                    switch (SystemInfoBinder.Default.AppDevice)
                    {
                        case AppDeviceEnum.AutoDetect:
                        case AppDeviceEnum.SNE1500M:
                            return "SNE-1500M";
                        case AppDeviceEnum.SNE3000M:
                            return "SNE-3000M";
                        case AppDeviceEnum.SNE4000M:
                            return "SNE-4000M";
                        case AppDeviceEnum.SNE3000MB:
                            return "SNE-3000MB";
                        case AppDeviceEnum.SNE4500M:
                            return "SNE-4500M";
                        case AppDeviceEnum.SNE3200M:
                            return "SNE-3200M";

                    }
                    break;
                case AppSellerEnum.Evex:
                    return "SX-3000";
                case AppSellerEnum.Hirox:
                    switch (SystemInfoBinder.Default.AppDevice)
                    {
                        case AppDeviceEnum.AutoDetect:
                        case AppDeviceEnum.SNE1500M:
                            return "SH-1500";
                        case AppDeviceEnum.SNE3000M:
                            return "SH-3000";
                        case AppDeviceEnum.SNE4000M:
                            return "SH-4000M";
                        case AppDeviceEnum.SNE4500M:
                            return "SH-4500M";
                        case AppDeviceEnum.SNE3200M:
                            return "SH-3200M";
                        case AppDeviceEnum.SNE3000MB:
                            return "SH-3000MB";
                    }
                    break;
                case AppSellerEnum.Nikkiso:
                    return "SEMTRACEmini";
            }
            return "Mini-SEM";
        }
        #endregion

        private SEC.Nanoeye.NanoeyeFactory _Nanoeye = null;
        public SEC.Nanoeye.NanoeyeFactory Nanoeye
        {
            get { return _Nanoeye; }
            set
            {
                if (_Nanoeye != value)
                {
                    _Nanoeye = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Nanoeye"));
                }
            }
        }

        private Settings.ISettingManager _SetManager = null;
        public Settings.ISettingManager SetManager
        {
            get { return _SetManager; }
            set
            {
                if (_SetManager != value)
                {
                    _SetManager = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SetManager"));
                }
            }
        }

        private Template.ITemplate _Equip = null;
        public Template.ITemplate Equip
        {
            get { return _Equip; }
            set
            {
                if (_Equip != value)
                {
                    _Equip = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Equip"));
                }
            }
        }

        private string _SettingFileName;
        public string SettingFileName
        {
            get { return _SettingFileName; }
            set
            {
                if (_SettingFileName != value)
                {
                    _SettingFileName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SettingFileName"));
                }
            }
        }

        private MiniSEM _MainForm;
        public MiniSEM MainForm
        {
            get { return _MainForm; }
            set { _MainForm = value; }
        }
    }
}
