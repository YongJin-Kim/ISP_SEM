using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.Support.AutoFunction
{
	public class ImageExport : AutoFunctionBase
	{
		public override void Cancel()
		{
			isie.ScanLineUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_ScanLineUpdated);
			isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
			_Cancled = true;
			OnProgressComplet();
		}

		public override void Stop()
		{
			isie.ScanLineUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_ScanLineUpdated);
			isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
			_Cancled = true;
			OnProgressComplet();
		}

		private short[] _Datas = null;
		public short[] Datas
		{
			get { return _Datas; }
		}

		private int _ImageWidth = 0;
		public int ImageWidth
		{
			get { return _ImageWidth; }
		}

		private int _ImageHeight = 0;
		public int ImageHeight
		{
			get { return _ImageHeight; }
		}

		SEC.Nanoeye.NanoImage.IScanItemEvent isie;

		public void GetImage(SEC.Nanoeye.NanoImage.IScanItemEvent isie)
		{
			this.isie = isie;

			isie.ScanLineUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_ScanLineUpdated);
			isie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);

			_ImageHeight = isie.Setting.ImageHeight;
			_ImageWidth = isie.Setting.ImageWidth;
		}


		void isie_FrameUpdated(object sender, string name, int startline, int lines)
		{
			_Datas = new short[isie.Setting.ImageWidth * isie.Setting.ImageHeight];
			System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, _Datas, 0, Datas.Length);

			isie.ScanLineUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_ScanLineUpdated);
			isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(isie_FrameUpdated);
			
			OnProgressComplet();
		}

		void isie_ScanLineUpdated(object sender, string name, int startline, int lines)
		{
			_Progress = (startline + lines) * 100 / isie.Setting.ImageHeight;
			OnProgressChanged();
		}

	}
}
