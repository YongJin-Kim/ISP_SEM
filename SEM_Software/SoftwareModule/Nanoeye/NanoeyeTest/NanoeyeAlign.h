#pragma once


// CNanoeyeAlign 대화 상자입니다.

class CNanoeyeAlign : public CDialog
{
	DECLARE_DYNAMIC(CNanoeyeAlign)

public:
	CNanoeyeAlign(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CNanoeyeAlign();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGALIGN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	IController *minisem;
};
