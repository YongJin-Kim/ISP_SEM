// NanoViewD.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoViewD.h"

// CNanoViewD 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoViewD, CDialog)

CNanoViewD::CNanoViewD(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoViewD::IDD, pParent)
	, sehlu(NULL)
{

}

CNanoViewD::~CNanoViewD()
{
}

void CNanoViewD::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_Image, imgWindow);
}


BEGIN_MESSAGE_MAP(CNanoViewD, CDialog)

	ON_WM_PAINT()
END_MESSAGE_MAP()

BITMAPINFOHEADER bmih;
BYTE * pBits;
HBITMAP hBitmap = NULL;

long imgWidth;
long imgHeight;

CStatic * imgWind;

HRESULT LineUpdatedBase(_bstr_t Name, long dataAddr, long startline, long lines, long linewidth, long frameheight )
{
	short *data;

	data = (short *)(dataAddr);

	if((imgWidth != linewidth) || (imgHeight != frameheight)){
		imgWidth = linewidth;
		imgHeight = frameheight;

		bmih.biSize = sizeof(BITMAPINFOHEADER);
		bmih.biWidth = imgWidth;
		bmih.biHeight = imgHeight;
		bmih.biPlanes = 1;
		bmih.biBitCount = 24;
		bmih.biCompression = BI_RGB;
		bmih.biSizeImage = 0;
		bmih.biXPelsPerMeter = 0;
		bmih.biYPelsPerMeter = 0;
		bmih.biClrUsed = 0;
		bmih.biClrImportant = 0;

		DeleteObject(hBitmap);

		hBitmap = CreateDIBSection(NULL, (BITMAPINFO *) &bmih, 0,(void**) &pBits, NULL, 0);
	}

	BYTE * dataPnt = (BYTE *)(pBits + startline * linewidth );


	short *tData = (short *)( data + startline*linewidth);


	for(int i = 0 ; i < lines * linewidth; i++){
		BYTE valTmp = (BYTE)( ( (*tData++) ) >>8);

		*dataPnt++ = valTmp;	// R
		*dataPnt++ = valTmp;	// G
		*dataPnt++ = valTmp;	// B
	}

	imgWind->PostMessageW(WM_PAINT);

	return 0;
}

void CNanoViewD::OnClose()
{
	isie->RemoveScanLineUpdatedEvent(sehlu);
}

// CNanoViewD 메시지 처리기입니다.
BOOL CNanoViewD::OnInitDialog()
{
	CDialog::OnInitDialog();

	imgWind = &imgWindow;

	imgWidth = 640;
	imgHeight = 480;
	bmih.biSize = sizeof(BITMAPINFOHEADER);
	bmih.biWidth = 640;
	bmih.biHeight = 480;
	bmih.biPlanes = 1;
	bmih.biBitCount = 24;
	bmih.biCompression = BI_RGB;
	bmih.biSizeImage = 0;
	bmih.biXPelsPerMeter = 0;
	bmih.biYPelsPerMeter = 0;
	bmih.biClrUsed = 0;
	bmih.biClrImportant = 0;

	hBitmap = CreateDIBSection(NULL, (BITMAPINFO *) &bmih, 0, (void**)&pBits, NULL, 0);

	HRESULT hr = CoCreateInstance(CLSID_ActiveScan,
									NULL,
									CLSCTX_INPROC_SERVER,
									IID_IActiveScan,
									reinterpret_cast<void**>(&scanner));
	if(FAILED(hr)){
		AfxMessageBox(_T("Scanner 초기화 실패"));
		OnClose();
		return FALSE;
	}

	SAFEARRAY * devList = scanner->GetDevList();

	BSTR arr;
	
	_bstr_t temp;

	long lot = 0;
	SafeArrayGetElement(devList, &lot, &arr);

		temp.Assign(arr);

	//devName.Assign((BSTR)(devList[0]));

	//hr = ::SafeArrayAccessData(devList, reinterpret_cast<void**>(&devName));
	scanner->Initialize(temp);
	//::SafeArrayUnaccessData(devList);
	
	_bstr_t text34(L"test");

	double* pbstr = NULL;
	SAFEARRAY* psa = ::SafeArrayCreateVector(VT_R8, 0, 22);
	if (NULL == psa)
	{
		AfxMessageBox(_T("Array 생성 실패"));
		OnClose();
		return FALSE;
	}

	hr = ::SafeArrayAccessData(psa, reinterpret_cast<void**>(&pbstr));
	if(FAILED(hr)){
		::SafeArrayDestroy(psa);
		AfxMessageBox(_T("Array 접근 실패"));
		OnClose();
		return FALSE;
	}

	pbstr[0] =  0;		// ai channel
	pbstr[1] =  1250000;	// ai clock
	pbstr[2] =  1;			// ai deference
	pbstr[3] =  10;		// ai max
	pbstr[4] = -10;		// ai min
	pbstr[5] =  625000;	// ao clock
	pbstr[6] =  10;		// ao max
	pbstr[7] = -10;		// ao min
	pbstr[8] =  240;		// frame height
	pbstr[9] =  340;		// frame width
	pbstr[10] =  1;		// propergation delay
	pbstr[11] =  1;		// ratio x
	pbstr[12] =  0.75;		// ratio y
	pbstr[13] =  0;		// shift x
	pbstr[14] =  0;		// shift y
	pbstr[15] =  0;		// average level
	pbstr[16] =  0;		// blur level
	pbstr[17] =  1;		// sample composite
	pbstr[18] =  240;		// image height
	pbstr[19] =  0;		// image left
	pbstr[20] =  0;			// image top
	pbstr[21] =  320;		// image width

	scanner->ScannerCreate(text34, psa);

	::SafeArrayUnaccessData(psa);
	::SafeArrayDestroy(psa);

	isie = scanner->GetScanEvent(text34);
	sehlu = new ScanEventHandler();
	sehlu->EventWorker = LineUpdatedBase;
	isie->AddScanLineUpdatedEvent(sehlu);

	scanner->ScannerChange(text34,0);

	return TRUE;
}

void CNanoViewD::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// 그리기 메시지에 대해서는 CDialog::OnPaint()을(를) 호출하지 마십시오.

	BITMAP bitmap;
//	HDC hdc, hdcMem;
//	PAINTSTRUCT ps;

	CDC * imgDC = imgWindow.GetDC();
	

	if(hBitmap){
		GetObject(hBitmap, sizeof(BITMAP), &bitmap);

		CDC * comDC = new CDC();
		comDC->CreateCompatibleDC(imgDC);

		comDC->SelectObject(hBitmap);

		imgDC->BitBlt(0,0,imgWidth,imgHeight, comDC, 0,0,SRCCOPY);
		comDC->DeleteDC();
	}
	imgWindow.ReleaseDC(imgDC);
}
