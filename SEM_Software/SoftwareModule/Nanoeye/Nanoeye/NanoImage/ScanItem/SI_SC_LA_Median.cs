using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoImage.ScanItem
{
	internal class SI_SC_LA_Median : SI_SC_LA_Blur
	{
		public SI_SC_LA_Median(SettingScanner set, DataAcquation.IDaqData daqData)
			: base(set, daqData)
		{

			
		}

		protected override unsafe void Bluring(int line, int average)
		{
			int imgTop = _Setting.ImageTop;
			int imgBottom = _Setting.ImageHeight + _Setting.ImageTop;
			int imgLeft = _Setting.ImageLeft;
			int imgRight = _Setting.ImageLeft + _Setting.ImageWidth;
			int imgWidth = _Setting.ImageWidth;
			int imgHeight = _Setting.ImageHeight;

			// bluring은 현재 스캔된 것의 위에 줄에 적용한다.

			// bluring을 할 수 있는 이미지 영역 밖이다.
			if(line <= imgTop) { return; }
			if(line > imgBottom + 1) { return; }	// 현재 스캔 라인의 뒷 줄은 bluring 한다.

			// 첫번째 줄, 두번째 줄은 bluring을 할 수 없다.
			// 따라서 단순 복사만을 해 둔다.
			if(line < 2)
			{
				short* pID = (short*)_imagedata.ToPointer();
				int* pAB = (int*)averageBuffer.ToPointer();
				pID = &pID[(line - 1) * imgWidth];
				pAB = &pAB[(line - 1) * _Setting.FrameWidth + imgLeft];

				for(int x = 0; x < imgWidth; x++) { *pID++ = (short)(*pAB++); }

				return;
			}

			// 이미지 영역 밑으로 버리는 영역이 없다.
			// 따라서 마지막 라인을 단순 복사해 둔다.
			if(line == _Setting.FrameHeight - 1)
			{

				short* pID = (short*)_imagedata.ToPointer();
				int* pAB = (int*)averageBuffer.ToPointer();
				pID = &pID[line * imgWidth];
				pAB = &pAB[line * _Setting.FrameWidth + imgLeft];

				for(int x = 0; x < imgWidth; x++) { *pID++ = (short)(*pAB++); }

			}

			// bluring되는 영역이다.
			int start = imgLeft;
			int end = imgRight;
			// 이미지 왼쪽으로 버리는 영역이 없는 경우 이다.
			if(start == 0)
			{
				start = 1;

				//_imagedata[line - 1 - _imageRectangle.Top, 0] = (short)averageBuffer[line - 1, 0];

				short* pID = (short*)_imagedata.ToPointer();
				int* pAB = (int*)averageBuffer.ToPointer();

				pID[(line - 1 - imgTop) * imgWidth] = (short)(pAB[(line - 1) * _Setting.FrameWidth]);
			}

			// 이미지 오른쪽으로 버리는 영역이 없는 경우 이다.
			if((imgRight) == _Setting.FrameWidth)
			{
				end -= 1; // 루프문에서는 작다로 계산하므로, 미리 1을 뺀다.

				short* pID = (short*)_imagedata.ToPointer();
				int* pAB = (int*)averageBuffer.ToPointer();

				pID[(line - 1 - _Setting.ImageTop) * imgWidth + end - imgLeft] = (short)(pAB[(line - 1) * _Setting.FrameWidth + end - imgLeft]);
			}

			{
				short* pID = (short*)_imagedata.ToPointer();
				int* pAB1 = (int*)averageBuffer.ToPointer();
				int* pAB4 = (int*)averageBuffer.ToPointer();
				int* pAB7 = (int*)averageBuffer.ToPointer();

				int frameWidth = _Setting.FrameWidth;
				int blur = _Setting.BlurLevel;

				pID = &pID[(line - 1 - imgTop) * imgWidth + start - imgLeft];
				pAB1 = &pAB1[(line - 2) * frameWidth + start + 1];
				pAB4 = &pAB4[(line - 1) * frameWidth + start + 1];
				pAB7 = &pAB7[(line) * frameWidth + start + 1];

				lock(obj)
				{
					int[] value = new int[9];
					value[1] = *pAB1;
					value[2] = *(pAB1 + 1);
					value[4] = *pAB4;
					value[5] = *(pAB4 + 1);
					value[7] = *pAB7;
					value[8] = *(pAB7 + 1);

					List<int> lst = new List<int>();

					// x좌표는 averageBuffer 기준 이다.
					for(int x = start; x < end; x++)
					{
						value[0] = value[1];
						value[1] = value[2];
						value[2] = *pAB1++;
						value[3] = value[4];
						value[4] = value[5];
						value[5] = *pAB4++;
						value[6] = value[7];
						value[7] = value[8];
						value[8] = *pAB7++;

						lst.Clear();
						lst.AddRange(value);
						lst.Sort();

						*pID++ = (short)lst[4];
					}
				}
			}
		}

		object obj = new object();
	}
}
