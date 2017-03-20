#pragma once


// CNanoeyeEgps 대화 상자입니다.

class CNanoeyeEgps : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeEgps)

public:
	CNanoeyeEgps(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeEgps();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGEGPS };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
};
