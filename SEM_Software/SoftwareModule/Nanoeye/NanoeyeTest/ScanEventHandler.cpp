#include "StdAfx.h"
#include "ScanEventHandler.h"

ScanEventHandler::ScanEventHandler(void)
{
}

ScanEventHandler::~ScanEventHandler(void)
{
}

HRESULT ScanEventHandler::EventFired(_bstr_t Name, long data, long startline, long lines, long linewidth, long frameheight)
{
	return raw_EventFired(Name, data, startline, lines, linewidth, frameheight);
}

HRESULT ScanEventHandler::QueryInterface(const IID &iid, void **pp)
{
    if (iid == __uuidof(IScanEvent) ||
        iid == __uuidof(IUnknown))
    {
        *pp = this;
        AddRef();
        return S_OK;
    }
    return E_NOINTERFACE;
}

HRESULT ScanEventHandler::raw_EventFired(
        /*[in]*/ BSTR Name,
        /*[in,out]*/ long data,
        /*[in]*/ long startline,
        /*[in]*/ long lines,
        /*[in]*/ long linewidth,
        /*[in]*/ long frameheight )
{
	return (*EventWorker)(Name, data, startline, lines, linewidth, frameheight);
}

