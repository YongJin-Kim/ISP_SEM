using System;
using System.Diagnostics;

using System.ComponentModel;

namespace SEC.Nanoeye.NanoImage
{
	/// <summary>
	/// DAQ 설정 Data.
	/// </summary>
	public class SettingScanner : ICloneable
	{
		private string _Name = "";
		//private string _Name;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private bool _Modifiable = true;
		internal bool Modifiable
		{
			get { return _Modifiable; }
			set { _Modifiable = value; }
		}

		[Category("Info"),	Description("Is modifiable")]
		public bool IsModifialble
		{
			get { return _Modifiable; }
		}

		private int _AiChannel = 0;
		//private int _AiChannel;
		/// <summary>
		/// DAQ AI 입력 채널
		/// </summary>
		[Category("Analog IO"),
		Description("Analog input channel")]
		public int AiChannel
		{
			get { return _AiChannel; }
			set
			{
				//Debug.Assert(value >= 0, "DAQ AI Channel은 0보다 크거나 같아야 한다.");
				//Debug.Assert(value < 4, "DAQ AI Channel은 4보다 작아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AiChannel = value;
			}
		}

		private double _AiClock = 1250000;
		//private double _AiClock;
		/// <summary>
		/// DAQ AI 샘플링 주파수
		/// </summary>
		[Category("Analog IO"),
		Description("Analog input sampling frequence"),
		RefreshProperties(RefreshProperties.All)]
		public double AiClock
		{
			get { return _AiClock; }
			set
			{
				//Debug.Assert(value > 0, "DAQ AI Clock 은 0보다 커야 한다.");
				//Debug.Assert(value <= 2000000, "DAQ AI Clock 은 1.25MHz 보다 작거나 같아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AiClock = value;
			}
		}

		private bool _AiDifferential = true;
		//private bool _AiDifferential;
		/// <summary>
		/// DAQ AI 입력이 차동인지 여부
		/// </summary>
		[Category("Analog IO"),
		Description("Use Differential input")]
		public bool AiDifferential
		{
			get { return _AiDifferential; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AiDifferential = value;
			}
		}

		private float _AiMaximum = 10.0F;
		//private float _AiMaximum;
		/// <summary>
		/// DAQ AI 신호의 최대 값. voltage 단위
		/// </summary>
		[Category("Analog IO"),
		Description("Analog input maximum signal potential")]
		public float AiMaximum
		{
			get { return _AiMaximum; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AiMaximum = value;
			}
		}

		private float _AiMinimum = -10.0F;
		//private float _AiMinimum;
		/// <summary>
		/// DAQ AI 신호의 최소 값. voltage 단위
		/// </summary>
		[Category("Analog IO"),
		Description("Analog input minimum signal potential"),
		]

		public float AiMinimum
		{
			get { return _AiMinimum; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AiMinimum = value;
			}
		}

		private double _AoClock = 625000;
		//private double _AoClock;
		/// <summary>
		/// DAQ AO 샘플링 주파수
		/// </summary>
		[Category("Analog IO"),
		Description("Analog output sampling frequence")]
		public double AoClock
		{
			get { return _AoClock; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AoClock = value;
			}
		}

		private float _AoMaximum = 10.0F;
		//private float _AoMaximum;
		/// <summary>
		/// DAQ AO 신호의 최대 값. voltage 단위
		/// </summary>
		[Category("Analog IO"),
		Description("Analog output maimum signal potential")]
		public float AoMaximum
		{
			get { return _AoMaximum; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AoMaximum = value;
			}
		}

		private float _AoMinimum = -10.0F;
		//private float _AoMinimum;
		/// <summary>
		/// DAQ AO 신호의 최소 값. voltage 단위
		/// </summary>
		[Category("Analog IO"),
		Description("Analog output minimum signal potential")]
		public float AoMinimum
		{
			get { return _AoMinimum; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AoMinimum = value;
			}
		}

		private double _PropergationDelay = 0;
		//private double _PropergationDelay;
		/// <summary>
		/// DAQ AO와 AI간의 시간 지연. us 단위.
		/// </summary>
		[Category("Analog IO"),
		Description("Propergation delay from AnalogOut to AnalogIn.")]
		public double PropergationDelay
		{
			get { return _PropergationDelay; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_PropergationDelay = value;
			}
		}

		private int _FrameWidth = 800;
		//private int _FrameWidth;
		/// <summary>
		/// 한 라인을 샘플링 할 횟수.
		/// </summary>
		[Category("Image Buffer"),
		Description("Internal buffered image width."),
		RefreshProperties(RefreshProperties.All)]
		public int FrameWidth
		{
			get { return _FrameWidth; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_FrameWidth = value;
			}
		}

		private int _FrameHeight = 600;
		//private int _FrameHeight;
		/// <summary>
		/// 한 프레임을 구성하는 라인 수
		/// </summary>
		[Category("Image Buffer"),
		Description("Internal buffered image height."),
		RefreshProperties(RefreshProperties.All)]
		public int FrameHeight
		{
			get { return _FrameHeight; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_FrameHeight = value;
			}
		}

		private double _RatioX = 1.0d;
		//private double _RatioX;
		/// <summary>
		/// X축 출력 스케일
		/// </summary>
		[Category("Analog IO"),
		Description("Horizontal signal ratio of ananlog output.")]
		public double RatioX
		{
			get { return _RatioX; }
			set
			{
				//Debug.Assert(value > 0.0d, "출력 스케일은 0보다 커야 한다.");
				//Debug.Assert(value <= 1.0d, "출력 스케일은 1보다 작거나 같아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_RatioX = value;
			}
		}

		private double _RatioY = 0.75d;
		//private double _RatioY;
		/// <summary>
		/// Y축 출력 스케일
		/// </summary>
		[Category("Analog IO"),
		Description("Vertical signal ratio of analog output.")]
		public double RatioY
		{
			get { return _RatioY; }
			set
			{
				//Debug.Assert(value > 0.0d, "출력 스케일은 0보다 커야 한다.");
				//Debug.Assert(value <= 1.0d, "출력 스케일은 1보다 작거나 같아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_RatioY = value;
			}
		}

		private double _ShiftX = 0.0d;
		//private double _ShiftX;
		/// <summary>
		/// X축 중심점 이동 비율
		/// </summary>
		[Category("Analog IO"),
		Description("Horizontal signal offset of analog output")]
		public double ShiftX
		{
			get { return _ShiftX; }
			set
			{
				//Debug.Assert(value >= -1.0d, "이동 비율은 0보다 커야 한다.");
				//Debug.Assert(value <= 1.0d, "이동 비율은 1보다 작거나 같아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ShiftX = value;
			}
		}

		private double _ShiftY = 0.0d;
		//private double _ShiftY;
		/// <summary>
		/// Y축 중심점 이동 비율
		/// </summary>
		[Category("Analog IO"),
		Description("Vertical signal offset of anlaog output")]
		public double ShiftY
		{
			get { return _ShiftY; }
			set
			{
				//Debug.Assert(value >= -1.0d, "이동 비율은 0보다 커야 한다.");
				//Debug.Assert(value <= 1.0d, "이동 비율은 1보다 작거나 같아야 한다.");
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ShiftY = value;
			}
		}

		private int _SampleComposite = 1;
		//private int _SampleComposite;
		[Category("Image Processing"),
		Description("Sample Composite")]
		public int SampleComposite
		{
			get { return _SampleComposite; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_SampleComposite = value;
			}
		}

		private int _LineAverage = 1;
		//private int _LineAverage;
		[Category("Image Processing"),
		Description("Line Average")]
		public int LineAverage
		{
			get { return _LineAverage; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_LineAverage = value;
			}
		}

		private int _ImageLeft = 0;
		//private int _ImageLeft;
		[Category("Image Buffer"),
		Description("Real image horizontal start postion from Frame Buffer")]
		public int ImageLeft
		{
			get { return _ImageLeft; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ImageLeft = value;
			}
		}

		private int _ImageTop = 0;
		//private int _ImageTop;
		[Category("Image Buffer"),
		Description("Real image vertical start postion from Frame Buffer")]
		public int ImageTop
		{
			get { return _ImageTop; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ImageTop = value;
			}
		}

		private int _ImageWidth = 320;
		//private int _ImageWidth;
		[Category("Image Buffer"),
		Description("Real image width")]
		public int ImageWidth
		{
			get { return _ImageWidth; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ImageWidth = value;
			}
		}

		private int _ImageHeight = 240;
		//private int _ImageHeight;
		[Category("Image Buffer"),
		Description("Real image height")]
		public int ImageHeight
		{
			get { return _ImageHeight; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_ImageHeight = value;
			}
		}

		private float _PaintX = 0f;
		//private float _PaintX;
		[Category("Image Display"),
		Description("Vertical start postion of display")]
		public float PaintX
		{
			get { return _PaintX; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_PaintX = value;
			}
		}

		private float _PaintY = 0f;
		//private float _PaintY;
		[Category("Image Display"),
		Description("Horizontal start postion of display")]
		public float PaintY
		{
			get { return _PaintY; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_PaintY = value;
			}
		}

		private float _PaintWidth = 1f;
		//private float _PaintWidth;
		[Category("Image Display"),
		Description("Vertical width of display")]
		public float PaintWidth
		{
			get { return _PaintWidth; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_PaintWidth = value;
			}
		}

		private float _PaintHeight = 0.75f;
		//private float _PaintHeight;
		[Category("Image Display"),
		Description("Horizontal height of display")]
		public float PaintHeight
		{
			get { return _PaintHeight; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_PaintHeight = value;
			}
		}

		private double _AreaShiftX = 0d;
		//private double _AreaShiftX;
		[Category("Analog IO"),
		Description("Horizontaol image offset for Area Scan")]
		public double AreaShiftX
		{
			get { return _AreaShiftX; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AreaShiftX = value;
			}
		}

		private double _AreaShiftY = 0d;
		//private double _AreaShiftY;
		[Category("Analog IO"),
		Description("Vertical image offset for Area Scan")]
		public double AreaShiftY
		{
			get { return _AreaShiftY; }
			set
			{
				if (!_Modifiable) { throw new InvalidOperationException("Can't modify."); }
				_AreaShiftY = value;
			}
		}

		private int _AverageLevel = 0;
		//private int _AverageLevel;
		/// <summary>
		/// Image Processing - Frame Average
		/// </summary>
		[Category("Image Processing"), Description("Frame Average")]
		public int AverageLevel
		{
			get { return _AverageLevel; }
			set
			{
				_AverageLevel = value;
			}
		}

		private int _BlurLevel = 0;
		//private int _BlurLevel;
		[Category("Image Processing"), Description("Bluring")]
		public int BlurLevel
		{
			get { return _BlurLevel; }
			set
			{
				//if (value > 32) { throw new ArgumentException("Blur의 최대 값은 32임."); }
				_BlurLevel = value;
			}
		}

		/// <summary>
		/// 초당 프레임 수
		/// </summary>
		[Category("Info"),Description("Frames / Second")]
		public double FramePerSecond
		{
			get { return (AiClock / (FrameHeight * FrameWidth * SampleComposite * LineAverage)); }
		}

		/// <summary>
		/// 한 프레임을 획득하는데 걸리는 시간.
		/// </summary>
		[Category("Info"),Description("Second / Frame")]
		public double SecondPerFrame
		{
			get { return (FrameHeight * FrameWidth * SampleComposite * LineAverage / AiClock); }
		}

		public object Clone()
		{
			SettingScanner ss = (SettingScanner)this.MemberwiseClone();

			ss._Modifiable = true;

			return ss;
		}

		public override string ToString()
		{
			return "ScanItem - " + _Name;
		}

        #region CSTDAQ
        private int _SEGain = 0;
        [Category("CST Data Level"), Description("SE Gain")]
        public int SEGain
        {
            get { return _SEGain; }
            set
            {
                _SEGain = value;
            }
        }

        private int _SEOffset = 0;
        [Category("CST Data Level"), Description("SE Offset")]
        public int SEOffset
        {
            get { return _SEOffset; }
            set
            {
                _SEOffset = value;
            }
        }                                                                  
        private int _BSEGain = 0;
        [Category("CST Data Level"), Description("BSE Gain")]
        public int BSEGain
        {
            get { return _BSEGain; }
            set
            {
                _BSEGain = value;
            }
        }

        private int _BSEOffset = 0;
        [Category("CST Data Level"), Description("BSE Offset")]
        public int BSEOffset
        {
            get { return _BSEOffset; }
            set
            {
                _BSEOffset = value;
            }
        }

        #endregion
	}
}
