#pragma once


// CNanoeyeStig 대화 상자입니다.

class CNanoeyeStig : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeStig)

public:
	CNanoeyeStig(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeStig();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGSTIG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
};
