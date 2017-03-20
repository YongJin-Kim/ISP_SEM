// NanoeyeVacuum.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeVacuum.h"


// CNanoeyeVacuum 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeVacuum, CDialog)

CNanoeyeVacuum::CNanoeyeVacuum(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeVacuum::IDD, pParent)
{

}

CNanoeyeVacuum::~CNanoeyeVacuum()
{
}

void CNanoeyeVacuum::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CNanoeyeVacuum, CDialog)
END_MESSAGE_MAP()


// CNanoeyeVacuum 메시지 처리기입니다.
