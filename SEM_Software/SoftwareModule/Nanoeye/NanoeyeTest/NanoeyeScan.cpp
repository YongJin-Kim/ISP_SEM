// NanoeyeScan.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeScan.h"


// CNanoeyeScan 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeScan, CDialog)

CNanoeyeScan::CNanoeyeScan(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeScan::IDD, pParent)
{
}

CNanoeyeScan::~CNanoeyeScan()
{
}

void CNanoeyeScan::DoDataExchange(CDataExchange* pDX)
{
	

	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SCROLLBAR1, csbMagX);
	DDX_Control(pDX, IDC_SCROLLBAR2, csbMagY);
	DDX_Control(pDX, IDC_SCROLLBAR3, csbAmpX);
	DDX_Control(pDX, IDC_SCROLLBAR4, csbAmpY);
	DDX_Control(pDX, IDC_SCROLLBAR5, csbRoation);
	DDX_Control(pDX, IDC_EDIT1, ceMagX);
	DDX_Control(pDX, IDC_EDIT2, ceMagY);
	DDX_Control(pDX, IDC_EDIT3, ceAmpX);
	DDX_Control(pDX, IDC_EDIT4, ceAmpY);
	DDX_Control(pDX, IDC_EDIT5, ceRotation);

}


BEGIN_MESSAGE_MAP(CNanoeyeScan, CDialog)
	ON_BN_CLICKED(IDC_RADIO1, &CNanoeyeScan::OnBnClickedRadio1)
	ON_BN_CLICKED(IDC_RADIO2, &CNanoeyeScan::OnBnClickedRadio2)
	ON_WM_HSCROLL()
END_MESSAGE_MAP()

BOOL CNanoeyeScan::OnInitDialog()
{
	CDialog::OnInitDialog();

	csbMagX.SetScrollRange(0,1000);
	csbMagX.SetScrollPos(0);
	csbMagX.EnableScrollBar(ESB_ENABLE_BOTH);

	csbMagY.SetScrollRange(0,1000);
	csbMagY.SetScrollPos(0);
	csbMagY.EnableScrollBar(ESB_ENABLE_BOTH);

	csbAmpX.SetScrollRange(0,2000);
	csbAmpX.SetScrollPos(1000);
	csbAmpX.EnableScrollBar(ESB_ENABLE_BOTH);

	csbAmpY.SetScrollRange(0,2000);
	csbAmpY.SetScrollPos(1000);
	csbAmpY.EnableScrollBar(ESB_ENABLE_BOTH);

	csbRoation.SetScrollRange(0,360);
	csbRoation.SetScrollPos(180);
	csbRoation.EnableScrollBar(ESB_ENABLE_BOTH);

	return TRUE;
}


// CNanoeyeScan 메시지 처리기입니다.

void CNanoeyeScan::OnBnClickedRadio1()
{
	
}

void CNanoeyeScan::OnBnClickedRadio2()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
}

void CNanoeyeScan::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{

	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.

	HRESULT hr;

    if(!pScrollBar || !pScrollBar->m_hWnd)
    return;

	int min=0;
	int max=0;
	int nCurPos = pScrollBar->GetScrollPos();
	int result;

	pScrollBar->GetScrollRange(&min, &max);

    switch(nSBCode)
    {
    case SB_LEFT:         // 7-Scroll to bottom.
        pScrollBar->SetScrollPos(min);
		result = min;
        break;

    case SB_ENDSCROLL:      // 8-End scroll.
		CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
		return;

    case SB_LINERIGHT :       // 1-Scroll one line down.
        nCurPos += 1;
        if(nCurPos > max)
            nCurPos = max;
        pScrollBar->SetScrollPos(nCurPos);
		result = nCurPos;
        break;

    case SB_LINELEFT   :         // 0-Scroll one line up.
        nCurPos -= 1;
        if(nCurPos < min)
            nCurPos = min;
        pScrollBar->SetScrollPos(nCurPos);
		result = nCurPos;
        break;

    case SB_PAGERIGHT:       // 3-Scroll one page down.
        nCurPos += 5;
        if(nCurPos > max)
            nCurPos = max;
        pScrollBar->SetScrollPos(nCurPos);
		result = nCurPos;
        break;

    case SB_PAGELEFT:         // 2-Scroll one page up.
        nCurPos -= 5;
        if(nCurPos < min)
            nCurPos = min;
        pScrollBar->SetScrollPos(nCurPos);
		result = nCurPos;
        break;

    case SB_THUMBPOSITION:  // 4-Scroll to the absolute position. The current position is provided in nPos.
        pScrollBar->SetScrollPos(nPos);
		result = nPos;
        break;

    case SB_THUMBTRACK:     // 5-Drag scroll box to specified position. The current position is provided in nPos.
        pScrollBar->SetScrollPos(nPos);
		result = nPos;
        break;

    case SB_RIGHT   :            // 6-Scroll to top.
        pScrollBar->SetScrollPos(min);
		result = min;
        break;
    }

	IControlValueDouble * icvd;
//	IControlValueInt * icvi;

	UINT nID = pScrollBar->GetDlgCtrlID();
	CString cstr;

	switch(nID){
	case IDC_SCROLLBAR1:	// magX
		//TRACE(_T("MagX Change\r\n"));

		hr = minisem->MagnificationX->QueryInterface(IID_IControlValueDouble, (void**)&icvd);

		if(FAILED(hr)){
			AfxMessageBox(_T("Query 실패"));
			break;
		}

		//icvd = (IControlValueDouble *)minisem->MagnificationX.GetInterfacePtr();

		icvd->value = (double)result / (double)1000;
		//icvd->MaxValue = (double)result / (double)1000;
		//icvd->MinValue = (double)result / (double)1000;
		//icvd->Offset = (double)result / (double)1000;

		cstr.Format(_T("%d"), result);
		ceMagX.SetWindowTextW(cstr);
		break;
	case IDC_SCROLLBAR2:	// magY
		hr = minisem->MagnificationY->QueryInterface(IID_IControlValueDouble, (void**)&icvd);

		if(FAILED(hr)){
			AfxMessageBox(_T("Query 실패"));
			break;
		}

		icvd->value = (double)result / (double)1000;

		cstr.Format(_T("%d"), result);
		ceMagY.SetWindowTextW(cstr);
		break;
	case IDC_SCROLLBAR3:	// AmpX
		hr = minisem->ScanAmplitudeX->QueryInterface(IID_IControlValueDouble, (void**)&icvd);

		if(FAILED(hr)){
			AfxMessageBox(_T("Query 실패"));
			break;
		}

		icvd->value = (double)((int)result - 1000)/(double)1000 ;

		cstr.Format(_T("%e"), (double)((int)result - 1000)/(double)1000);
		ceAmpX.SetWindowTextW(cstr);
		break;
	case IDC_SCROLLBAR4:	// AmpY
		hr = minisem->ScanAmplitudeY->QueryInterface(IID_IControlValueDouble, (void**)&icvd);

		if(FAILED(hr)){
			AfxMessageBox(_T("Query 실패"));
			break;
		}

		icvd->value = (double)((int)result - 1000)/(double)1000 ;

		cstr.Format(_T("%e"), (double)((int)result - 1000)/(double)1000);
		ceAmpY.SetWindowTextW(cstr);
		break;
	case IDC_SCROLLBAR5:	// Rotation
		hr = minisem->Rotation->QueryInterface(IID_IControlValueDouble, (void**)&icvd);

		if(FAILED(hr)){
			AfxMessageBox(_T("Query 실패"));
			break;
		}

		icvd->value = (double)((int)result-180);

		cstr.Format(_T("%e"), (double)((int)result-180));
		ceRotation.SetWindowTextW(cstr);
		break;
	default:
		AfxMessageBox(_T("정의되지 않은 ScrollBar"));
		break;

	}

	CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
}

