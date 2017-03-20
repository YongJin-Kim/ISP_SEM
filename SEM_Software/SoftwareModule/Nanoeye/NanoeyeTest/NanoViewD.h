#pragma once

#include "stdafx.h"
#include "afxwin.h"


// CNanoViewD 대화 상자입니다.

class CNanoViewD : public CDialog
{
	DECLARE_DYNAMIC(CNanoViewD)

public:
	CNanoViewD(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoViewD();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_Display };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IActiveScan *scanner;
	BOOL OnInitDialog();
	ScanEventHandler *sehlu;
	IScanItemEventPtr isie;
	ISettingManager *iSetManager;
	CStatic imgWindow;
	void OnClose();

	afx_msg void OnPaint();
};
