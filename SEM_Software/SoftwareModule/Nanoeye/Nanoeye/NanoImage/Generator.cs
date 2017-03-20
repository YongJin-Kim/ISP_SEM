using System;
using System.Drawing;

namespace SEC.Nanoeye.NanoImage
{
	internal class ScanGenerator
	{
		#region Property & Variables
		private Size _framsize = new Size(320, 240);
		public Size FrameSize
		{
			get { return _framsize; }
			set { _framsize = value; }
		}

		private double _ratioX = 0.707;
		public double RatioX
		{
			get { return _ratioX; }
			set { _ratioX = value; }
		}

		private double _ratioY = 0.707;
		public double RatioY
		{
			get { return _ratioY; }
			set { _ratioY = value; }
		}

		private double _shiftX = 0;
		public double ShiftX
		{
			get { return _shiftX; }
			set { _shiftX = value; }
		}

		private double _shiftY = 0;
		public double ShiftY
		{
			get { return _shiftY; }
			set { _shiftY = value; }
		}

		public RectangleF ScanningBound
		{
			get { return ScanGenerator.CalScanningBound(_ratioX, _ratioY, _shiftX, _shiftY); }
		}

		private int _LineAverage = 1;
		public int LineAverage
		{
			get { return _LineAverage; }
			set { _LineAverage = value; }
		}

		private int _devid = 2;
		/// <summary>
		/// 출력 주파수 감소 비율.
		/// </summary>
		public int Divid
		{
			get { return _devid; }
			set { _devid = value; }
		}
		#endregion

        private int _device = 2;
        public int Device
        {
            get { return _device; }
            set { _device = value; }
        }


		public short[,] Generate()
		{
            return CreateBase();
		}

		private short[,] CreateBase()
		{

            
            int height = _framsize.Height;
            int width = _framsize.Width /_devid / _device;
            

            RectangleF bound = this.ScanningBound;

            short[,] data = new short[2, width * height * _LineAverage];

            int x, y;
			
            // '1'이란 무한한 소수들의 합이다...

            // 수평 데이터
            for (x = 0; x < width; x++)
            {
                data[0, x] = (short)(bound.Left + bound.Width * (x + 1) / width);
                //data[0, x] = (short)(bound.Left + (bound.Width * x) / (width - 1));
            } 
            for (y = 1; y < height * _LineAverage; y++)
            {
                Array.ConstrainedCopy(data, 0, data, width * y, width);
            }

            // 수직 데이터
            short value;
            for (y = 0; y < height; y++)
            {
                value = (short)(bound.Top + bound.Height * (y + 1) / height);
                for (x = 0; x < width * _LineAverage; x++)
                {
                   data[1, y * width * _LineAverage + x] = value;
                   
                }
            }

            //WriteData(data);


            
			return data;
		}

        private short[,] MutiCreateBase()
        {


            int height = _framsize.Height;
            int width = _framsize.Width / _devid / _device;


            RectangleF bound = this.ScanningBound;

            short[,] data = new short[2, width * height * _LineAverage];

            int x, y;

            // '1'이란 무한한 소수들의 합이다...

            // 수평 데이터
            for (x = 0; x < width; x++)
            {
                data[0, x] = (short)(bound.Left + bound.Width * (x + 1) / width);
                //data[0, x] = (short)(bound.Left + (bound.Width * x) / (width - 1));
            }
            

            for (y = 1; y < height * _LineAverage; y++)
            {
                Array.ConstrainedCopy(data, 0, data, width * y, width);
            }

            // 수직 데이터
            short value;
            for (y = 0; y < height; y++)
            {
                value = (short)(bound.Top + bound.Height * (y + 1) / height);
                for (x = 0; x < width * _LineAverage; x++)
                {
                    data[1, y * width * _LineAverage + x] = value;

                }
            }

            //WriteData(data);



            return data;
        }



        public short[,] OnePointCreateBase()
        {


            int height = 3;
            int width = 3 / _devid;


            RectangleF bound = this.ScanningBound;

            short[,] data = new short[2, width * height * _LineAverage];

            int x, y;

            // '1'이란 무한한 소수들의 합이다...

            // 수평 데이터
            for (x = 0; x < width; x++)
            {
                data[0, x] = (short)(bound.Left + bound.Width * (x + 1) / width);
                //data[0, x] = (short)(bound.Left + (bound.Width * x) / (width - 1));
            }
            for (y = 1; y < height * _LineAverage; y++)
            {
                Array.ConstrainedCopy(data, 0, data, width * y, width);
            }

            // 수직 데이터
            short value;
            for (y = 0; y < height; y++)
            {
                value = (short)(bound.Top + bound.Height * (y + 1) / height);
                for (x = 0; x < width * _LineAverage; x++)
                {
                    data[1, y * width * _LineAverage + x] = value;

                }
            }

            //WriteData(data);



            return data;
        }


		private void WriteData(short[,] data)
		{
			string fileName = System.Windows.Forms.Application.CommonAppDataPath + @"\sample";

			int cnt = 0;
			while (System.IO.File.Exists(fileName))
			{
				fileName += cnt.ToString();
				cnt++;
			}
			fileName += ".txt";

			System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName);

			sw.WriteLine("Scan Data : sample");
			sw.WriteLine("Frame Size : " + _framsize.Width.ToString() + "," + _framsize.Height.ToString());
			sw.WriteLine("RatioX : " + _ratioX.ToString());
			sw.WriteLine("RatioY : " + _ratioY.ToString());
			sw.WriteLine("ShiftX : " + _shiftX.ToString());
			sw.WriteLine("ShiftY : " + _shiftY.ToString());
			sw.WriteLine("LineAvergae : " + _LineAverage.ToString());
			sw.WriteLine("Divid : " + _devid.ToString());

			int length = data.GetLength(1);
			sw.Write("X : ");

			for (int i = 0; i < length; i++)
			{
				sw.Write(data[0, i].ToString() + ",");
			}
			sw.WriteLine();

			sw.Write("Y : ");

			for (int i = 0; i < length; i++)
			{
				sw.Write(data[1, i].ToString() + ",");
			}

			sw.Close();
		}

		public void Generate(out short[] ax1, out short[] ax2)
		{
			int height = _framsize.Height;
			int width = _framsize.Width / _devid;

			RectangleF bound = this.ScanningBound;

			//short[,] data = new short[2, width * height * _LineAverage];
			ax1 = new short[width * height * _LineAverage];
			ax2 = new short[width * height * _LineAverage];


			int x, y;

			// 수평 데이터
			for (x = 0; x < width; x++)
			{
				ax1[x] = (short)(bound.Left + bound.Width * x / width);
			}
			for (y = 1; y < height * _LineAverage; y++)
			{
				Array.ConstrainedCopy(ax1, 0, ax1, width * y, width);
			}

			// 수직 데이터
			short value;
			for (y = 0; y < height; y++)
			{
				value = (short)(bound.Top + bound.Height * y / height);
				for (x = 0; x < width * _LineAverage; x++)
				{
					ax2[y * width * _LineAverage + x] = value;
				}
			}
		}

		private static RectangleF CalScanningBound(double _ratioX, double _ratioY, double _shiftX, double _shiftY)
		{
			RectangleF rect = new RectangleF();

			rect.X = (float)(short.MinValue * _ratioX + ushort.MaxValue * _shiftX);
			rect.Y = (float)(short.MinValue * _ratioY + ushort.MaxValue * _shiftY);
			rect.Width = (float)(ushort.MaxValue * _ratioX);
			rect.Height = (float)(ushort.MaxValue * _ratioY);

			return rect;
		}
	}
}
