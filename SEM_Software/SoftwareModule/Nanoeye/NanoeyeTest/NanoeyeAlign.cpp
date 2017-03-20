// NanoeyeAlign.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeAlign.h"


// CNanoeyeAlign 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeAlign, CDialog)

CNanoeyeAlign::CNanoeyeAlign(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeAlign::IDD, pParent)
	, minisem(NULL)
{

}

CNanoeyeAlign::~CNanoeyeAlign()
{
}

void CNanoeyeAlign::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CNanoeyeAlign, CDialog)
END_MESSAGE_MAP()


// CNanoeyeAlign 메시지 처리기입니다.
