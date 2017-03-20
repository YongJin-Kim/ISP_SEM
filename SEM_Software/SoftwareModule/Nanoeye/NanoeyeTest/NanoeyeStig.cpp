// NanoeyeStig.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeStig.h"


// CNanoeyeStig 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeStig, CDialog)

CNanoeyeStig::CNanoeyeStig(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeStig::IDD, pParent)
{

}

CNanoeyeStig::~CNanoeyeStig()
{
}

void CNanoeyeStig::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CNanoeyeStig, CDialog)
END_MESSAGE_MAP()


// CNanoeyeStig 메시지 처리기입니다.
