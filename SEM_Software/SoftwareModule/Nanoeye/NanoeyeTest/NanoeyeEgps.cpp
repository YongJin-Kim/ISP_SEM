// NanoeyeEgps.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NanoeyeTest.h"
#include "NanoeyeEgps.h"


// CNanoeyeEgps 대화 상자입니다.

IMPLEMENT_DYNAMIC(CNanoeyeEgps, CDialog)

CNanoeyeEgps::CNanoeyeEgps(CWnd* pParent /*=NULL*/)
	: CDialog(CNanoeyeEgps::IDD, pParent)
{

}

CNanoeyeEgps::~CNanoeyeEgps()
{
}

void CNanoeyeEgps::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CNanoeyeEgps, CDialog)
END_MESSAGE_MAP()


// CNanoeyeEgps 메시지 처리기입니다.
