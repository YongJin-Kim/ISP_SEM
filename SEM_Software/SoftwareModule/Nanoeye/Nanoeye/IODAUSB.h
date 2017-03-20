///////////////////////////////////////////////////////////////////////////////////////////////////
// IODAUSB.h : IODAUSB.DLL header
//
//                              Copyright (c) 2010
//                      CANTOPS - SEOUL, REPUBLIC OF KOREA
//								www.cantops.biz
//
// This program is the property of the CANTOPS. and it shall not be reproduced, distributed or 
// used without permission of an authorized Company official. 
// This is an unpublished work subject to Trade Secret and Copyright protection.
// 
// DLL Version: V1.2.0.5
// Date: April 27, 2010
//
///////////////////////////////////////////////////////////////////////////////////////////////////
#ifdef __cplusplus
extern "C" 
{
#endif

#ifndef __IODAUSB_LIB_H__
#define __IODAUSB_LIB_H__

#ifdef IODAUSB_EXPORTS
	#define IODAUSB_API	__declspec(dllexport)
#else
	#define IODAUSB_API	__declspec(dllimport)
#endif

///////////////////////////////////////////////////////////////////////////////////////////////////
// type definition
//
typedef unsigned char			UCHAR;
typedef			 char			CHAR;
typedef unsigned short			USHORT;
typedef unsigned short			WORD;
typedef signed   short			SHORT;
typedef unsigned int			UINT;
typedef signed   int			SINT;
typedef unsigned long			ULONG;
typedef signed   long			LONG;

///////////////////////////////////////////////////////////////////////////////////////////////////
// USB communication Definition
//
#define IODAUSB_MEMORY_POOL_SIZE		(0x04000000) //64 MB

#define IODAUSB_BULK_IN_BUF_SIZE		8192
#define IODAUSB_BULK_OUT_BUF_SIZE		512

#define IODAUSB_ADDA_STATUS_ADDA_STOP	0
#define IODAUSB_ADDA_STATUS_ADDA_START	1
#define IODAUSB_ADDA_STATUS_BULK_STOP	0
#define IODAUSB_ADDA_STATUS_BULK_BUSY	1
#define IODAUSB_ADDA_STATUS_BUF_EMPTY	0
#define IODAUSB_ADDA_STATUS_BUF_FULL	1

#define IODAUSB_INTERFACE_MODE_BULKIN	0
#define IODAUSB_INTERFACE_MODE_BULKOUT	1

#define IODAUSB_PATTERN_MAX				8


///////////////////////////////////////////////////////////////////////////////////////////////////
// error number definition
//
enum IoDaUsbErrNo  
{
	IODAUSB_NOERR=0,
	IODAUSB_ERR_DEV_INIT,
	IODAUSB_ERR_DEV_CLOSE,
	IODAUSB_ERR_DEV_RESET,
	IODAUSB_ERR_DEV_ABORT,
	IODAUSB_ERR_CONTROL_READ,
	IODAUSB_ERR_CONTROL_WRITE,
	IODAUSB_ERR_BULK_READ,
	IODAUSB_ERR_BULK_WRITE,
	IODAUSB_ERR_IO_READ,
	IODAUSB_ERR_IO_WRITE,
	IODAUSB_ERR_AD_READ,
	IODAUSB_ERR_DA_READ,
	IODAUSB_ERR_DA_WRITE,
	IODAUSB_ERR_INTERFACE_MODE_CHANGE,
	IODAUSB_ERR_READ_FRAME_DATA,
	IODAUSB_ERR_WRITE_FRAME_DATA,
	IODAUSB_ERR_PATTERN_INFO_READ,
	IODAUSB_ERR_PATTERN_INFO_WRITE,
	IODAUSB_ERR_PATTERN_INFO_SELECT,
	IODAUSB_ERR_READ_LINE_DATA,
	IODAUSB_ERR_READ_STACK_DATA_LENGTH,
	IODAUSB_ERR_PATTERN_ADDRESS,
};

///////////////////////////////////////////////////////////////////////////////////////////////////
// error string definition
//
char gcIoDaUsbErrorString[][256] =
{
	"",
	"Device is not initialized.",
	"Device is not closed.",
	"Device is not reset.",
	"Device is not abort.",
	"USB Communication error. control register reading.",
	"USB Communication error. control register writing.",
	"USB Communication error. bulk data reading.",
	"USB Communication error. bulk data writing.",
	"USB Communication error. I/O data reading.",
	"USB Communication error. I/O data writing.",
	"IODA AD value read failed.",
	"IODA DA value read failed.",
	"IODA DA value write failed.",
	"USB Communication error. Interface mode change.",
	"IODA frame data read failed.",
	"IODA frame data write failed.",
	"IODA pattern image information read failed.",
	"IODA pattern image information write failed.",
	"IODA pattern image information select failed.",
	"IODA frame data read failed.",
	"IODA stack data read length is too larger than bulk-in size.",
	"IODA pattern address error. It should be the value of 4 times.",
	"",
};

///////////////////////////////////////////////////////////////////////////////////////////////////
// USER FUNCTIONS
//
// library and USB communication
IODAUSB_API	bool ioda_usb_open(UCHAR devID);
IODAUSB_API	void ioda_usb_close();
IODAUSB_API	void ioda_usb_reset();
IODAUSB_API	void ioda_usb_abort();

IODAUSB_API	bool get_library_version(ULONG *version);
IODAUSB_API	bool get_fpga_version(UCHAR* version);
IODAUSB_API	bool get_sampling_freq(ULONG* freq);
IODAUSB_API	bool set_sampling_freq(ULONG freq);

// pattern management
IODAUSB_API	bool get_pattern_addr(ULONG* addr);
IODAUSB_API	bool set_pattern_addr(ULONG addr);
IODAUSB_API	bool get_pattern_metrics(USHORT* width, USHORT* height);
IODAUSB_API	bool set_pattern_metrics(USHORT width, USHORT height);
IODAUSB_API	bool select_pattern(UCHAR id);
IODAUSB_API	bool get_selected_pattern(UCHAR* id);

IODAUSB_API	bool write_frame_data(USHORT width, USHORT height, void *pSrcImage);
IODAUSB_API	bool get_pattern_memory_info(UCHAR id, ULONG* addr, USHORT* width, USHORT* height);
IODAUSB_API	bool set_pattern_memory_info(UCHAR id, ULONG addr, USHORT width, USHORT height);
IODAUSB_API bool get_upload_data_type(UCHAR* value);
IODAUSB_API bool set_upload_data_type(UCHAR value);

// AD/DA management
IODAUSB_API	bool get_ad_start_delay(ULONG* delay);
IODAUSB_API	bool set_ad_start_delay(ULONG delay);
IODAUSB_API	bool get_ad_capture_select(UCHAR* channel);
IODAUSB_API	bool set_ad_capture_select(UCHAR channel);
IODAUSB_API	bool get_ad_input_source(UCHAR* type);
IODAUSB_API	bool set_ad_input_source(UCHAR type);
IODAUSB_API	bool get_ad_gain_type(UCHAR ch, UCHAR* type);
IODAUSB_API	bool set_ad_gain_type(UCHAR ch, UCHAR type);
IODAUSB_API	bool get_ad_value(UCHAR ch, SHORT* value);
IODAUSB_API	bool get_da_value(UCHAR ch, SHORT* value);
IODAUSB_API	bool set_da_value(UCHAR ch, SHORT value);
IODAUSB_API	bool get_ad_sampling_ratio(UCHAR* value);
IODAUSB_API	bool set_ad_sampling_ratio(UCHAR value);

IODAUSB_API	bool capture_start();
IODAUSB_API	bool capture_stop();
IODAUSB_API	bool capture_adda_status(UCHAR *status);
IODAUSB_API	bool capture_bulkin_status(UCHAR *status);
IODAUSB_API	bool capture_buffer_status(UCHAR *status);

// Digital IO management
IODAUSB_API	bool get_digital_input(UCHAR *indata, UCHAR nBytesToRead);
IODAUSB_API	bool get_digital_output(UCHAR *outdata, UCHAR nBytesToRead);
IODAUSB_API	bool set_digital_output(UCHAR *outdata, UCHAR nBytesToWrite);
IODAUSB_API	bool get_digital_input_bit(UCHAR *indata, UCHAR bitnum);
IODAUSB_API	bool get_digital_output_bit(UCHAR *outdata, UCHAR bitnum);
IODAUSB_API	bool set_digital_output_bit(UCHAR outdata, UCHAR bitnum);


///////////////////////////////////////////////////////////////////////////////////////////////////
// error handling function 
//
IODAUSB_API int get_last_error();
IODAUSB_API void get_last_error_string(CHAR* cErr);


///////////////////////////////////////////////////////////////////////////////////////////////////
// AD memory pool management function
//
IODAUSB_API	bool memory_clear();
IODAUSB_API	bool memory_read(UCHAR *pDst, ULONG nBytesToRead, ULONG *nBytesRead);
IODAUSB_API	bool memory_stacked_size(ULONG *nSize);


///////////////////////////////////////////////////////////////////////////////////////////////////
// special functions for library interface test
//
IODAUSB_API	bool add_1b(CHAR a, CHAR* b, LONG* sum);
IODAUSB_API	bool add_2b(SHORT a, SHORT* b, LONG* sum);
IODAUSB_API	bool add_4b(LONG a, LONG* b, LONG* sum);


///////////////////////////////////////////////////////////////////////////////////////////////////
// library management function
// note. !!! Don't use the following functions, it's served only for this library developers !!!
//
IODAUSB_API void ioda_init();
IODAUSB_API void ioda_exit();

IODAUSB_API	bool pattern_download_start();
IODAUSB_API	bool pattern_download_stop();
IODAUSB_API	bool pattern_download_status(UCHAR *status);

IODAUSB_API	void MutexLock();
IODAUSB_API	void MutexUnlock();
IODAUSB_API	void USBInformationPrint(char * sTitle);
IODAUSB_API	bool get_vsync_low_width(USHORT* clockcount);
IODAUSB_API	bool set_vsync_low_width(USHORT clockcount);
IODAUSB_API	bool ioda_usb_change_bulk_mode(UCHAR mode);
IODAUSB_API bool get_upload_data_gray(SHORT* graylevel);
IODAUSB_API bool set_upload_data_gray(SHORT graylevel);

IODAUSB_API	bool READ_1BYTE(UCHAR cmd, UCHAR addr, UCHAR *pDst);
IODAUSB_API	bool READ_2BYTE(UCHAR cmd, UCHAR addr, USHORT *pDst);
IODAUSB_API	bool READ_4BYTE(UCHAR cmd, UCHAR addr, ULONG *pDst);
IODAUSB_API	bool READ_NBYTE(UCHAR cmd, UCHAR addr, UCHAR *pDst, LONG nBytesToRead, LONG *nBytesRead);
IODAUSB_API	bool WRITE_1BYTE(UCHAR cmd, UCHAR addr, UCHAR data);
IODAUSB_API	bool WRITE_2BYTE(UCHAR cmd, UCHAR addr, USHORT data);
IODAUSB_API	bool READ_BULK(UCHAR *pDst, ULONG nBytesToRead, ULONG *nBytesRead);
IODAUSB_API	bool WRITE_BULK(UCHAR *pSrc, ULONG nByteToWrite, int EventID);

IODAUSB_API void ioda_pump_start();
IODAUSB_API void ioda_pump_stop();

#endif // end of __IODAUSB_LIB_H__

#ifdef __cplusplus
}
#endif