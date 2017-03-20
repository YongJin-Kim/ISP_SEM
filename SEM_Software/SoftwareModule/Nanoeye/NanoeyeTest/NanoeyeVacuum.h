#pragma once


// CNanoeyeVacuum 대화 상자입니다.

class CNanoeyeVacuum : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeVacuum)

public:
	CNanoeyeVacuum(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeVacuum();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGVACUUM };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
};
