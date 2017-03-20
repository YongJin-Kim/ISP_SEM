#pragma once


// CNanoeyeLens 대화 상자입니다.

class CNanoeyeLens : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeLens)

public:
	CNanoeyeLens(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeLens();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGLENS };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
};
