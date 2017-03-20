#pragma once
#include "stdafx.h"

class ScanEventHandler :
	public IScanEvent
{
public:
	ScanEventHandler(void);
	~ScanEventHandler(void);
    HRESULT __stdcall QueryInterface(const IID &, void **pp);
    ULONG __stdcall AddRef(void) { return 1; }
    ULONG __stdcall Release(void) { return 1; }
    HRESULT EventFired (
        _bstr_t Name,
        long data,
        long startline,
        long lines,
        long linewidth,
        long frameheight );
	HRESULT __stdcall raw_EventFired (
        /*[in]*/ BSTR Name,
        /*[in,out]*/ long data,
        /*[in]*/ long startline,
        /*[in]*/ long lines,
        /*[in]*/ long linewidth,
        /*[in]*/ long frameheight ) ;
	HRESULT (*EventWorker)(_bstr_t Name, long data, long startline, long lines, long linewidth, long frameheight);
};
