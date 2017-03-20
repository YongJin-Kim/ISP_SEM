#pragma once
#include "afxwin.h"


// CNanoeyeScan 대화 상자입니다.

class CNanoeyeScan : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeScan)
public:
	CNanoeyeScan(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeScan();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGSCAN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
	CScrollBar csbMagX;
	CScrollBar csbMagY;
	CScrollBar csbAmpX;
	CScrollBar csbAmpY;
	CScrollBar csbRoation;
	CEdit ceMagX;
	CEdit ceMagY;
	CEdit ceAmpX;
	CEdit ceAmpY;
	CEdit ceRotation;
	afx_msg void OnBnClickedRadio1();
	afx_msg void OnBnClickedRadio2();
	virtual BOOL OnInitDialog();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
};
