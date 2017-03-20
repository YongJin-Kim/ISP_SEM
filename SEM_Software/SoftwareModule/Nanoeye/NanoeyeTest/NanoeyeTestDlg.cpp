
// NanoeyeTestDlg.cpp : 구현 파일
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeTestDlg.h"

#include "NanoeyeAlign.h"
#include "NanoeyeEgps.h"
#include "NanoeyeLens.h"
#include "NanoeyeScan.h"
#include "NanoeyeStig.h"
#include "NanoeyeVacuum.h"
#include "NanoViewD.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 응용 프로그램 정보에 사용되는 CAboutDlg 대화 상자입니다.

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

// 구현입니다.
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CNanoeyeTestDlg 대화 상자




CNanoeyeTestDlg::CNanoeyeTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeTestDlg::IDD, pParent), minisem(NULL)

	, cnvd(NULL)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CNanoeyeTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, cNanoViewState);
}

BEGIN_MESSAGE_MAP(CNanoeyeTestDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BUTTON1, &CNanoeyeTestDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDOK, &CNanoeyeTestDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDC_BUTTONSCAN, &CNanoeyeTestDlg::OnBnClickedButtonscan)
	ON_BN_CLICKED(IDC_BUTTONLENS, &CNanoeyeTestDlg::OnBnClickedButtonlens)
	ON_BN_CLICKED(IDC_BUTTONALIGN, &CNanoeyeTestDlg::OnBnClickedButtonalign)
	ON_BN_CLICKED(IDC_BUTTONSTIG, &CNanoeyeTestDlg::OnBnClickedButtonstig)
	ON_BN_CLICKED(IDC_BUTTONEGPS, &CNanoeyeTestDlg::OnBnClickedButtonegps)
	ON_BN_CLICKED(IDC_BUTTONVACUUM, &CNanoeyeTestDlg::OnBnClickedButtonvacuum)
END_MESSAGE_MAP()

// CNanoeyeTestDlg 메시지 처리기

BOOL CNanoeyeTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 시스템 메뉴에 "정보..." 메뉴 항목을 추가합니다.

	// IDM_ABOUTBOX는 시스템 명령 범위에 있어야 합니다.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 이 대화 상자의 아이콘을 설정합니다. 응용 프로그램의 주 창이 대화 상자가 아닐 경우에는
	//  프레임워크가 이 작업을 자동으로 수행합니다.
	SetIcon(m_hIcon, TRUE);			// 큰 아이콘을 설정합니다.
	SetIcon(m_hIcon, FALSE);		// 작은 아이콘을 설정합니다.

	HRESULT hr = CoInitialize(NULL);

	if(FAILED(hr)){
		AfxMessageBox(_T("COM 라이브러리 초기화 실패"));
		SendMessage(WM_CLOSE);
		return true;
	}

	// SettingManager 생성
	hr = CoCreateInstance(CLSID_SettingManager,
							NULL,
							CLSCTX_INPROC_SERVER,
							IID_ISettingManager,
							reinterpret_cast<void**>(&iSetManager));
	




	cnvd = new CNanoViewD();
	cnvd->iSetManager = iSetManager;
	cnvd->Create(IDD_Display);
	cnvd->ShowWindow(SW_SHOW);
	

	return TRUE;  // 포커스를 컨트롤에 설정하지 않으면 TRUE를 반환합니다.
}

void CNanoeyeTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// 대화 상자에 최소화 단추를 추가할 경우 아이콘을 그리려면
//  아래 코드가 필요합니다. 문서/뷰 모델을 사용하는 MFC 응용 프로그램의 경우에는
//  프레임워크에서 이 작업을 자동으로 수행합니다.

void CNanoeyeTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 그리기를 위한 디바이스 컨텍스트

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 클라이언트 사각형에서 아이콘을 가운데에 맞춥니다.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 아이콘을 그립니다.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// 사용자가 최소화된 창을 끄는 동안에 커서가 표시되도록 시스템에서
//  이 함수를 호출합니다.
HCURSOR CNanoeyeTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CNanoeyeTestDlg::OnClose()
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CoUninitialize();

	if(minisem != NULL){	minisem->Release();	}

	delete(cnvd);

	CDialog::OnClose();
}

void CNanoeyeTestDlg::OnBnClickedButton1()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.


	HRESULT hr = CoCreateInstance(CLSID_ControllerMiniSEM,
									NULL,
									CLSCTX_INPROC_SERVER,
									IID_IController,
									reinterpret_cast<void**>(&minisem));

	if(FAILED(hr)){
		cNanoViewState.SetWindowTextW(_T("에러"));
		minisem = NULL;
		return;
	}


	SAFEARRAY * portList = minisem->GetPortList();

	BSTR arr;
	_bstr_t temp;

	long lot = 0;
	SafeArrayGetElement(portList, &lot, &arr);

	temp.Assign(arr);


	VARIANT_BOOL vb = minisem->ColumnInit(temp);

	

	switch(vb){
	case 0:
		cNanoViewState.SetWindowTextW(_T("실패"));
		break;
	case -1:
		cNanoViewState.SetWindowTextW(_T("성공"));
		break;
	default:
		cNanoViewState.SetWindowTextW(_T("???"));
		break;
	}
}

void CNanoeyeTestDlg::OnBnClickedOk()
{
	OnOK();
}

void CNanoeyeTestDlg::OnBnClickedButtonscan()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.

	static bool created = false;

	static CNanoeyeScan *cScan = NULL;

	if(minisem == NULL){
		return;
	}

	if(cScan == NULL){
		cScan = new CNanoeyeScan();
		cScan->Create(IDD_DIALOGSCAN);
		cScan->ShowWindow(SW_SHOW     );
		cScan->minisem = this->minisem;
	}
	else{
		if(::IsWindowVisible(cScan->m_hWnd) == FALSE){
			cScan->ShowWindow(SW_SHOW     );
		}
	}
}

void CNanoeyeTestDlg::OnBnClickedButtonlens()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
}

void CNanoeyeTestDlg::OnBnClickedButtonalign()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.

	static bool created = false;

	static CNanoeyeAlign *cAlign;

	if(::IsWindow(cAlign->m_hWnd)){
		if(::IsWindowVisible(cAlign->m_hWnd) == FALSE){
			cAlign->ShowWindow(SW_SHOW     );
		}
	}
	else{
		 cAlign = new CNanoeyeAlign();
		 cAlign->ShowWindow(SW_SHOW     );
	}
}

void CNanoeyeTestDlg::OnBnClickedButtonstig()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
}

void CNanoeyeTestDlg::OnBnClickedButtonegps()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
}

void CNanoeyeTestDlg::OnBnClickedButtonvacuum()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
}
