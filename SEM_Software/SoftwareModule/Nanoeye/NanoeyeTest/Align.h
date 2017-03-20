#pragma once


// cAlign 대화 상자입니다.

class cAlign : public CDialog
{
	DECLARE_DYNAMIC(cAlign)

public:
	cAlign(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~cAlign();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOGALIGN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
};
