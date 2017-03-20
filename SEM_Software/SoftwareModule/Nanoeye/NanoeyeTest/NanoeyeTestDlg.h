
// NanoeyeTestDlg.h : 헤더 파일
//

#pragma once
#include "afxwin.h"

#include "NanoViewD.h"

#include <GdiPlusTypes.h>
#include <gdiplus.h>

// CNanoeyeTestDlg 대화 상자
class CNanoeyeTestDlg : public CDialog
{
// 생성입니다.
public:
	CNanoeyeTestDlg(CWnd* pParent = NULL);	// 표준 생성자입니다.

// 대화 상자 데이터입니다.
	enum { IDD = IDD_NANOEYETEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 지원입니다.

private:
	IController * minisem;


// 구현입니다.
protected:
	HICON m_hIcon;

	// 생성된 메시지 맵 함수
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnClose();
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedButtonscan();
	afx_msg void OnBnClickedButtonlens();
	afx_msg void OnBnClickedButtonalign();
	afx_msg void OnBnClickedButtonstig();
	afx_msg void OnBnClickedButtonegps();
	afx_msg void OnBnClickedButtonvacuum();

	CEdit cNanoViewState;
	CNanoViewD *cnvd;
	ISettingManager *iSetManager;
};
