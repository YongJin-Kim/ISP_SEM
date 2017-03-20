// Align.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "Align.h"


// cAlign 대화 상자입니다.

IMPLEMENT_DYNAMIC(cAlign, CDialog)

cAlign::cAlign(CWnd* pParent /*=NULL*/)
	: CDialog(cAlign::IDD, pParent)
{

}

cAlign::~cAlign()
{
}

void cAlign::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(cAlign, CDialog)
END_MESSAGE_MAP()


// cAlign 메시지 처리기입니다.
