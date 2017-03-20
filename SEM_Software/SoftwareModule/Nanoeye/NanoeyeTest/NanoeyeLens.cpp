// NanoeyeLens.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeLens.h"


// CNanoeyeLens 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeLens, CDialog)

CNanoeyeLens::CNanoeyeLens(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeLens::IDD, pParent)
{

}

CNanoeyeLens::~CNanoeyeLens()
{
}

void CNanoeyeLens::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CNanoeyeLens, CDialog)
END_MESSAGE_MAP()


// CNanoeyeLens 메시지 처리기입니다.
