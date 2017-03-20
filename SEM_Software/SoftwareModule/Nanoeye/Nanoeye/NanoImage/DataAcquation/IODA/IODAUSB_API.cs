using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SEC.Nanoeye.NanoImage.DataAcquation.IODA
{
    internal class IODAUSB_API
    {
        public IODAUSB_API() { }

        #region iodausb.dll import
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // type definition
        //
        //typedef unsigned char			UCHAR;
        //typedef			 char			CHAR;
        //typedef unsigned short			USHORT;
        //typedef unsigned short			WORD;
        //typedef signed   short			SHORT;
        //typedef unsigned int			UINT;
        //typedef signed   int			SINT;
        //typedef unsigned long			ULONG;
        //typedef signed   long			LONG;

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // USB communication Definition
        //
        //private static int IODAUSB_MEMORY_POOL_SIZE			= 0x04000000; //64 MB

        //private static int IODAUSB_BULK_IN_BUF_SIZE			= 1024;
        //private static int IODAUSB_BULK_OUT_BUF_SIZE		= 512;

        //private static int IODAUSB_ADDA_STATUS_ADDA_STOP	= 0;
        //private static int IODAUSB_ADDA_STATUS_ADDA_START	= 1;
        //private static int IODAUSB_ADDA_STATUS_BULK_STOP	= 0;
        //private static int IODAUSB_ADDA_STATUS_BULK_BUSY	= 1;
        //private static int IODAUSB_ADDA_STATUS_BUF_EMPTY	= 0;
        //private static int IODAUSB_ADDA_STATUS_BUF_FULL		= 1;

        //private static int IODAUSB_INTERFACE_MODE_BULKIN	= 0;
        //private static int IODAUSB_INTERFACE_MODE_BULKOUT	= 1;

        //private static int IODAUSB_PATTERN_MAX				= 8;


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // error number definition
        //
        private enum IoDaUsbErrNo
        {
            IODAUSB_NOERR = 0,
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
        };

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // error string definition
        //
        //char gcIoDaUsbErrorString[][256] =
        //{
        //    "",
        //    "Device is not initialized.",
        //    "Device is not closed.",
        //    "Device is not reset.",
        //    "Device is not abort.",
        //    "USB Communication error. control register reading.",
        //    "USB Communication error. control register writing.",
        //    "USB Communication error. bulk data reading.",
        //    "USB Communication error. bulk data writing.",
        //    "USB Communication error. I/O data reading.",
        //    "USB Communication error. I/O data writing.",
        //    "IODA AD value read failed.",
        //    "IODA DA value read failed.",
        //    "IODA DA value write failed.",
        //    "USB Communication error. Interface mode change.",
        //    "IODA frame data read failed.",
        //    "IODA frame data write failed.",
        //    "IODA pattern image information read failed.",
        //    "IODA pattern image information write failed.",
        //    "IODA pattern image information select failed.",
        //    "IODA frame data read failed.",
        //    "IODA stack data read length is too larger than bulk-in size.",
        //    "",
        //};

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // USER FUNCTIONS
        //

        // library and USB communication
        //IODAUSB_API	bool ioda_usb_open(UCHAR devID);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ioda_usb_open(byte devID);
        //IODAUSB_API	void ioda_usb_close();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ioda_usb_close();
        //IODAUSB_API	void ioda_usb_reset();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ioda_usb_reset();
        //IODAUSB_API	void ioda_usb_abort();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ioda_usb_abort();

        //IODAUSB_API	bool get_library_version(ULONG *version);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_library_version(ref uint versiont);
        //IODAUSB_API	bool get_fpga_version(UCHAR* version);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_fpga_version(ref byte version);
        //IODAUSB_API	bool get_sampling_freq(ULONG* freq);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_sampling_freq(ref uint freq);
        //IODAUSB_API	bool set_sampling_freq(ULONG freq);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_sampling_freq(uint freq);

        // pattern management
        //IODAUSB_API	bool get_pattern_addr(ULONG* addr);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_pattern_addr(ref uint addr);
        //IODAUSB_API	bool set_pattern_addr(ULONG addr);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_pattern_addr(uint addr);
        //IODAUSB_API	bool get_pattern_metrics(USHORT* width, USHORT* height);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_pattern_metrics(ref ushort width, ref ushort height);
        //IODAUSB_API	bool set_pattern_metrics(USHORT width, USHORT height);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_pattern_metrics(ushort width, ushort height);
        //IODAUSB_API	bool select_pattern(UCHAR id);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool select_pattern(byte id);
        //IODAUSB_API	bool get_selected_pattern(UCHAR* id);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_selected_pattern(ref byte id);

        //IODAUSB_API	bool write_frame_data(USHORT width, USHORT height, void *pSrcImage);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool write_frame_data(ushort width, ushort height, IntPtr pSrcImage);
        //IODAUSB_API	bool get_pattern_memory_info(UCHAR id, ULONG* addr, USHORT* width, USHORT* height);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_pattern_memory_info(byte id, ref uint addr, ref ushort width, ref ushort height);
        //IODAUSB_API	bool set_pattern_memory_info(UCHAR id, ULONG addr, USHORT width, USHORT height);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_pattern_memory_info(byte id, uint addr, ushort width, ushort height);
        //IODAUSB_API bool get_upload_data_type(UCHAR* value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_upload_data_type(ref byte value);
        //IODAUSB_API bool set_upload_data_type(UCHAR value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_upload_data_type(byte value);

        // AD/DA management
        //IODAUSB_API	bool get_ad_start_delay(ULONG* delay);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_start_delay(ref uint delay);
        //IODAUSB_API	bool set_ad_start_delay(ULONG delay);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_ad_start_delay(uint delay);
        //IODAUSB_API	bool get_ad_capture_select(UCHAR* channel);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_capture_select(ref byte channel);
        //IODAUSB_API	bool set_ad_capture_select(UCHAR channel);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_ad_capture_select(byte channel);
        //IODAUSB_API	bool get_ad_input_source(UCHAR* type);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_input_source(ref byte type);
        //IODAUSB_API	bool set_ad_input_source(UCHAR type);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_ad_input_source(byte type);
        //IODAUSB_API	bool get_ad_gain_type(UCHAR ch, UCHAR* type);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_gain_type(byte ch, ref byte type);
        //IODAUSB_API	bool set_ad_gain_type(UCHAR ch, UCHAR type);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_ad_gain_type(byte ch, byte type);
        //IODAUSB_API	bool get_ad_value(UCHAR ch, SHORT* value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_value(byte ch, ref short value);
        //IODAUSB_API	bool get_da_value(UCHAR ch, SHORT* value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_da_value(byte ch, ref short value);
        //IODAUSB_API	bool set_da_value(UCHAR ch, SHORT value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_da_value(byte ch, short value);
        //IODAUSB_API	bool get_ad_sampling_ratio(UCHAR* value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_ad_sampling_ratio(ref byte value);
        //IODAUSB_API	bool set_ad_sampling_ratio(UCHAR value);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_ad_sampling_ratio(byte value);

        //IODAUSB_API	bool capture_start();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool capture_start();
        //IODAUSB_API	bool capture_stop();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool capture_stop();
        //IODAUSB_API	bool capture_adda_status(UCHAR *status);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool capture_adda_status(ref byte status);
        //IODAUSB_API	bool capture_bulkin_status(UCHAR *status);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool capture_bulkin_status(ref byte status);
        //IODAUSB_API	bool capture_buffer_status(UCHAR *status);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool capture_buffer_status(ref byte status);

        // Digital IO management
        //IODAUSB_API	bool get_digital_input(UCHAR *indata, UCHAR nBytesToRead);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_digital_input(ref byte indata, byte nBytesToRead);
        //IODAUSB_API	bool get_digital_output(UCHAR *outdata, UCHAR nBytesToRead);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_digital_output(ref byte outdata, byte nBytesToRead);
        //IODAUSB_API	bool set_digital_output(UCHAR *outdata, UCHAR nBytesToWrite);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_digital_output(ref byte outdata, byte nBytesToWrite);
        //IODAUSB_API	bool get_digital_input_bit(UCHAR *indata, UCHAR bitnum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_digital_input_bit(ref byte indata, byte bitnum);
        //IODAUSB_API	bool get_digital_output_bit(UCHAR *outdata, UCHAR bitnum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_digital_output_bit(ref byte outdata, byte bitnum);
        //IODAUSB_API	bool set_digital_output_bit(UCHAR outdata, UCHAR bitnum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_digital_output_bit(byte outdata, byte bitnum);


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // error handling function 
        //
        //IODAUSB_API int get_last_error();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int get_last_error();
        //IODAUSB_API void get_last_error_string(CHAR* cErr);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void get_last_error_string(StringBuilder cErr);


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // AD memory pool management function
        //
        //IODAUSB_API	bool memory_clear();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool memory_clear();
        //IODAUSB_API	bool memory_read(UCHAR *pDst, ULONG nBytesToRead, ULONG *nBytesRead);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool memory_read(IntPtr pDst, uint nBytesToRead, ref uint nBytesRead);
        //IODAUSB_API	bool memory_stacked_size(ULONG *nSize);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool memory_stacked_size(ref uint nSize);


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // special functions for library interface test
        //
        //IODAUSB_API	bool add_1b(CHAR a, CHAR* b, LONG* sum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool add_1b(byte a, ref byte b, ref int sum);
        //IODAUSB_API	bool add_2b(SHORT a, SHORT* b, LONG* sum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool add_2b(short a, ref short b, ref int sum);
        //IODAUSB_API	bool add_4b(LONG a, LONG* b, LONG* sum);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool add_4b(int a, ref int b, ref int sum);


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // library management function
        // note. !!! Don't use the following functions, it's served only for this library developers !!!
        //
        //IODAUSB_API void ioda_init();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ioda_init();
        //IODAUSB_API void ioda_exit();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ioda_exit();

        //IODAUSB_API	bool pattern_download_start();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool pattern_download_start();
        //IODAUSB_API	bool pattern_download_stop();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool pattern_download_stop();
        //IODAUSB_API	bool pattern_download_status(UCHAR *status);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool pattern_download_status(ref byte status);

        //IODAUSB_API	void MutexLock();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void MutexLock();
        //IODAUSB_API	void MutexUnlock();
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void MutexUnlock();
        //IODAUSB_API	void USBInformationPrint(char * sTitle);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void USBInformationPrint(StringBuilder sTitle);
        //IODAUSB_API	bool get_vsync_low_width(USHORT* clockcount);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_vsync_low_width(ref ushort clockcount);
        //IODAUSB_API	bool set_vsync_low_width(USHORT clockcount);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_vsync_low_width(ushort clockcount);
        //IODAUSB_API	bool ioda_usb_change_bulk_mode(UCHAR mode);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ioda_usb_change_bulk_mode(byte mode);
        //IODAUSB_API bool get_upload_data_gray(SHORT* graylevel);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool get_upload_data_gray(ref short graylevel);
        //IODAUSB_API bool set_upload_data_gray(SHORT graylevel);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool set_upload_data_gray(short graylevel);

        //IODAUSB_API	bool READ_1BYTE(UCHAR cmd, UCHAR addr, UCHAR *pDst);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool READ_1BYTE(byte cmd, byte addr, ref byte pDst);
        //IODAUSB_API	bool READ_2BYTE(UCHAR cmd, UCHAR addr, USHORT *pDst);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool READ_2BYTE(byte cmd, byte addr, ref byte pDst);
        //IODAUSB_API	bool READ_4BYTE(UCHAR cmd, UCHAR addr, ULONG *pDst);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool READ_4BYTE(byte cmd, byte addr, ref uint pDst);
        //IODAUSB_API	bool READ_NBYTE(UCHAR cmd, UCHAR addr, UCHAR *pDst, LONG nBytesToRead, LONG *nBytesRead);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool READ_NBYTE(byte cmd, byte addr, ref byte pDst, int nBytesToRead, ref int nBytesRead);
        //IODAUSB_API	bool WRITE_1BYTE(UCHAR cmd, UCHAR addr, UCHAR data);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WRITE_1BYTE(byte cmd, byte addr, byte data);
        //IODAUSB_API	bool WRITE_2BYTE(UCHAR cmd, UCHAR addr, USHORT data);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WRITE_2BYTE(byte cmd, byte addr, ushort data);
        //IODAUSB_API	bool READ_BULK(UCHAR *pDst, ULONG nBytesToRead, ULONG *nBytesRead);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool READ_BULK(ref byte pDst, uint nBytesToRead, ref uint nBytesRead);
        //IODAUSB_API	bool WRITE_BULK(UCHAR *pSrc, ULONG nByteToWrite, int EventID);
        [DllImport("IODAUSB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WRITE_BULK(ref byte pSrc, uint nByteToWrite, int EventID);
        #endregion

        #region Property & Variables
        private static int _ReadTimeout = 100;

        /// <summary>
        /// 읽기 제한 시간. ms 단위.
        /// </summary>
        public static int ReadTimeout
        {
            get { return _ReadTimeout; }
            set { _ReadTimeout = value; }
        }
        #endregion

        #region Error Handling
        private static void ErrorOccured()
        {
            int errCode = get_last_error();
            StringBuilder sb = new StringBuilder(256);
            get_last_error_string(sb);

            string errMsg = sb.ToString();
            Trace.WriteLine(errCode.ToString() + " - " + errMsg, "IODAUSB_API");

            throw new IODAException(errMsg, errCode);
        }
        #endregion

        #region Device Handling
        /// <summary>
        /// 사용 가능한 IODAUSB board가 있는지 확인한다.
        /// </summary>
        public static bool IsDeviceExist
        {
            get
            {
                if (!ioda_usb_open(0)) { return false; }

                ioda_usb_close();
                return true;
            }
        }

        /// <summary>
        /// IODAUSB board와 연결 한다.
        /// </summary>
        public static void Open()
        {
            if (!ioda_usb_open(0)) { ErrorOccured(); }
        }

        /// <summary>
        /// IODAUSB board와의 연결을 끊는다.
        /// Open() 호출시 자동으로 호출되므로, 일반적으로 사용할 필요가 없다.
        /// </summary>
        public static void Close()
        {
            ioda_usb_close();
        }

        /// <summary>
        /// IODAUSB board를 Power-on 상태로 초기화 한다.
        /// 현재 작동중인 데이터는 저장되지 않는다.
        /// </summary>
        public static void Reset()
        {
            ioda_usb_reset();
        }

        /// <summary>
        /// IODAUSB board와 pending된 통신을 취소한다.
        /// </summary>
        public static void Abort()
        {
            ioda_usb_abort();
        }

        /// <summary>
        /// IODAUSB 메인 보드의 FPGA 버전을 가져온다.
        /// </summary>
        /// <returns></returns>
        public static int VersionFpga
        {
            get
            {
                byte ver = 0;

                if (!get_fpga_version(ref ver)) { ErrorOccured(); }

                return (int)ver;
            }
        }

        public static int VersionLibrary
        {
            get
            {
                uint ver = 0;

                if (!get_library_version(ref ver)) { ErrorOccured(); }

                return (int)ver;
            }
        }
        #endregion

        #region Sampling Frequence
        /// <summary>
        /// board의 동작 주파수를 가져오거나 설정한다.
        /// </summary>
        public static long SamplingFrequence
        {
            get
            {
                uint freq = 0;
                if (!get_sampling_freq(ref freq)) { ErrorOccured(); }
                return (long)freq;
            }
            set
            {
                if ((value < 1) || (value > 0x00ffffff)) { throw new ArgumentException("Invalid frequence range. 0 < Freq <= 0x00ff ffff"); }
                uint freq = (uint)value;
                if (!set_sampling_freq(freq)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// Analog 출력과 입력간의 시작 시간 딜레이를 가져오거나 설정 한다.
        /// </summary>
        public static int AIOdelay
        {
            get
            {
                uint delay = 0;

                if (!get_ad_start_delay(ref delay)) { ErrorOccured(); }
                return (int)delay;
            }
            set
            {
                if (value < 0) { throw new ArgumentException("value is must greate then 0."); }	// API상에서는 unsigned이지만 0x7fff ffff 이상 쓸 일이 없음.

                uint delay = (uint)value;

                if (!set_ad_start_delay(delay)) { ErrorOccured(); }
            }
        }
        #endregion

        #region Pattern
        /// <summary>
        /// 현재 사용 중인 패턴의 board상에서의 주소를 가져오거나 설정한다.
        /// </summary>
        public static long PatternAddr
        {
            get
            {
                uint addr = 0;
                if (!get_pattern_addr(ref addr)) { ErrorOccured(); }
                return (long)addr;
            }
            set
            {
                if ((value < 0) || (value > 0x1ffffff)) { throw new ArgumentException("Invalid address range. 0 <= Addr < 0x0200 0000"); }
                if ((value & 0x20) != 0) { throw new ArgumentException("Value must be multiples of 32."); }

                uint addr = (uint)value;
                if (!set_pattern_addr(addr)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// 현재 사용 중인 패턴의 길이를 가져오거나 설정한다.
        /// </summary>
        public static int PatternLength
        {
            get
            {
                ushort width = 0;
                ushort height = 0;
                if (!get_pattern_metrics(ref width, ref height)) { ErrorOccured(); }
                return ((int)width * (int)height);	// 2 patterns are mixed
            }
            set
            {
                if ((value < 1) || (value > 0x1ffff)) { throw new ArgumentException("Invalid length. 0 < length < 0x0200 0000"); }
                if ((value & 0x01) != 0) { throw new ArgumentException("Invalid lenght. Must be muliples of 2."); }

                int width = 1;
                int height = value;
                while (height > 0xffff)
                {
                    height /= 2;
                    width *= 2;
                }

                ushort usWidth = (ushort)width;
                ushort usHeight = (ushort)height;

                if (!set_pattern_metrics(usWidth, usHeight)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// 현재 사용 중인 패턴의 id를 가져오거나 설정한다.
        /// </summary>
        public static int PatternId
        {
            get
            {
                byte id = 0xff;
                if (!get_selected_pattern(ref id)) { ErrorOccured(); }
                return (int)id;
            }
            set
            {
                if ((value < 0) || (value > 7)) { throw new ArgumentException("Invalid id. 0 <= id <= 7."); }

                byte id = (byte)value;

                if (!select_pattern(id)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// AO 출력의 ch0, ch1의 값을 전송 한다.
        /// </summary>
        /// <param name="imgData"></param>
        public static unsafe void PatternDataWrite(short[] patternData)
        {
            int length = patternData.Length;	// short, ch0+ch1

            if (length > 0x1FFFFFF) { throw new ArgumentException("imgData length must be smaller then 0x2000000."); }
            if ((length & 0x01) == 0x01) { throw new ArgumentException("Invalid patternData length. length must be even number.", "patternData"); }

            int width = 1;
            int height = length;
            while (height > 0xffff)
            {
                height /= 2;
                width *= 2;
            }

            ushort usW = (ushort)width;
            ushort usH = (ushort)height;


            fixed (short* pntImgData = patternData)
            {
                if (!write_frame_data(usW, usH, (IntPtr)pntImgData)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// board에 설정 되어 있는 패턴의 정보를 가져 온다.
        /// </summary>
        /// <param name="id">패턴의 ID</param>k
        /// <param name="addr">패턴의 주소</param>
        /// <param name="length">패턴의 길이</param>
        public static void PatternMemoryInfoGet(int id, out long addr, out long length)
        {
            if ((id < 0) || (id > 7)) { throw new ArgumentException("Invalid id. 0 <= id <= 7."); }

            uint uiAddr = 0;
            ushort usWidth = 0;
            ushort usHeight = 0;
            byte bId = (byte)id;

            if (!get_pattern_memory_info(bId, ref uiAddr, ref usWidth, ref usHeight)) { ErrorOccured(); }

            addr = (long)uiAddr;
            length = (long)((int)usWidth) * (int)usHeight; // 2 patterns are mixed
        }

        /// <summary>
        /// board에 패턴의 정보를 설정 한다.
        /// </summary>
        /// <param name="id">패턴의 ID. 0 ~ 7</param>
        /// <param name="addr">패턴의 위치. byte 단위. 32의 배수 이여야 함.</param>
        /// <param name="length">패턴의 길이. width * height 값. 실제 byte 크기는 length의 4배가 됨(1 data == 2byte. 2 patterns are mixed.).</param>
        public static void PatternMemoryInfoSet(int id, long addr, int length)
        {
            if ((id < 0) || (id > 7)) { throw new ArgumentException("Invalid id. 0 <= id <= 7.", "id"); }
            if ((addr < 0) || (addr > 0x1ffffff)) { throw new ArgumentException("Invalid address. 0<= addr < 0x200 0000", "addr"); }
            if ((addr % 0x20) != 0) { throw new ArgumentException("Invalid address. addr must be multiples of 32.", "addr"); }
            if ((length < 1) || (length > 0x1ffffff)) { throw new ArgumentException("Invalid length. 0 < length < 0x200 0000", "length"); }
            if ((length & 0x01) == 0x01) { throw new ArgumentException("Invalid length. length must be even number.", "length"); }
            if ((addr + length) >= 0x2000000) { throw new ArgumentException("Invalid location. addr+length < 0x200 0000."); }

            byte bId = (byte)id;
            uint uiAddr = (uint)addr;
            ushort usWidth = 0;
            ushort usHeight = 0;

            int width = 1;
            int height = length;
            while (height > 0xffff)
            {
                height /= 2;
                width *= 2;
            }

            usWidth = (ushort)width;
            usHeight = (ushort)height;

            if (!set_pattern_memory_info(bId, uiAddr, usWidth, usHeight)) { ErrorOccured(); }
        }
        #endregion

        #region AD/DA management

        /// <summary>
        /// 사용하는 Analog 입력의 채널을 가져오거나 설정 한다.
        /// </summary>
        public static int AIchannel
        {
            get
            {
                byte channel = 0;

                if (!get_ad_capture_select(ref channel)) { ErrorOccured(); }
                return (int)channel;
            }
            set
            {
                if ((value < 0) || (value > 1)) { throw new ArgumentException("channel is 0 or 1."); }

                byte channel = (byte)value;

                if (!set_ad_capture_select(channel)) { ErrorOccured(); }
            }
        }

        public enum AIsourceType
        {
            AD_Input = 0,
            DA_Output,
            Reference_6dot95V,
            Ground
        }

        /// <summary>
        /// Analog 입력의 소스를 설정 한다.
        /// 일반적으로 AD_Input을 사용 하면, 특수한 경우에만 다른 소스를 선택 한다.
        /// </summary>
        public static AIsourceType AIsource
        {
            get
            {
                byte source = 0;

                if (!get_ad_input_source(ref source)) { ErrorOccured(); }

                return (AIsourceType)((int)source);
            }
            set
            {
                byte source = (byte)((int)value);

                if (!set_ad_input_source(source)) { ErrorOccured(); }
            }
        }

        public enum AIgainType
        {
            /// <summary>
            /// 0 : x 0.4 (+/- 10.0 Volt)
            /// </summary>
            x0d4_10V = 0,
            /// <summary>
            /// 1 : x 0.8 (+/- 5.0 Volt)
            /// </summary>
            x0d8_5V,
            /// <summary>
            /// 2 : x 2 (+/- 2.0 Volt)
            /// </summary>
            x2_2V,
            /// <summary>
            /// 3 : x 4 (+/- 1.0 Volt)
            /// </summary>
            x4_1V
        }

        /// <summary>
        /// Analog 입력의 gain을 가져온다.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static AIgainType AIgainGet(int channel)
        {
            if ((channel < 0) || (channel > 1)) { throw new ArgumentException("channel is 0 or 1."); }

            byte ch = (byte)channel;
            byte type = 0;

            if (!get_ad_gain_type(ch, ref type)) { ErrorOccured(); }

            int iType = (int)type;

            return (AIgainType)iType;
        }

        /// <summary>
        /// Analog 입력의 gain을 설정 한다.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="aiType"></param>
        public static void AIgainSet(int channel, AIgainType aiType)
        {
            if ((channel < 0) || (channel > 1)) { throw new ArgumentException("channel is 0 or 1."); }

            byte ch = (byte)channel;
            byte type = (byte)((int)aiType);

            if (!set_ad_gain_type(ch, type)) { ErrorOccured(); }
        }

        /// <summary>
        /// Analgo 입력의 값을 가져온다.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static short AIvalueGet(int channel)
        {
            if ((channel < 0) || (channel > 1)) { throw new ArgumentException("channel is 0 or 1."); }

            byte ch = (byte)channel;
            short value = 0;

            if (!get_ad_value(ch, ref value)) { ErrorOccured(); }

            return value;
        }

        /// <summary>
        /// Analog 출력의 값을 가져온다.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static short AOvalueGet(int channel)
        {
            if ((channel < 0) || (channel > 1)) { throw new ArgumentException("channel is 0 or 1."); }

            byte ch = (byte)channel;
            short value = 0;

            if (!get_da_value(ch, ref value)) { ErrorOccured(); }

            return value;
        }

        /// <summary>
        /// Analog 출력의 값을 설정한다.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        public static void AOvalueSet(int channel, short value)
        {
            if ((channel < 0) || (channel > 1)) { throw new ArgumentException("channel is 0 or 1."); }

            byte ch = (byte)channel;

            if (!set_da_value(ch, value)) { ErrorOccured(); }
        }

        /// <summary>
        /// AIO 작업을 시작 한다.
        /// </summary>
        public static void CaptureStart()
        {
            if (!capture_start()) { ErrorOccured(); }
        }

        /// <summary>
        /// AIO 작업을 중지 한다.
        /// </summary>
        public static void CaptureStop()
        {
            if (!capture_stop()) { ErrorOccured(); }
        }

        public enum CaptureStateType
        {
            Stop = 0,
            Run
        }

        public static CaptureStateType CaptureAioStatus
        {
            get
            {
                byte state = 0;
                if (!capture_adda_status(ref state)) { ErrorOccured(); }
                return (CaptureStateType)((int)state);
            }
        }

        public static CaptureStateType CaptureBulkinStatus
        {
            get
            {
                byte state = 0;
                if (!capture_bulkin_status(ref state)) { ErrorOccured(); }
                return (CaptureStateType)((int)state);
            }
        }

        public enum CaptureBufferStatusType
        {
            NotFull = 0,
            Full = 1,
        }

        public static CaptureBufferStatusType CaptureBufferStatus
        {
            get
            {
                byte state = 0;
                if (!capture_buffer_status(ref state)) { ErrorOccured(); }
                return (CaptureBufferStatusType)((int)state);
            }
        }

        /// <summary>
        /// 버퍼를 초기화 한다.
        /// </summary>
        public static void AIbufferClear()
        {
            if (!memory_clear()) { ErrorOccured(); }
        }

        /// <summary>
        /// 버퍼에 있는 AI sample을 가져 온다.
        /// </summary>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static unsafe short[] AIbufferRead(int length)
        {
            uint rCount = (uint)(length * 2);
            short[] datas = new short[length];
            uint reads = 0;
            //short[] result = new short[length];

            //int start = 0;

            Stopwatch timeout = new Stopwatch();
            timeout.Start();

            //while (true)
            //{

            //    //reads = 0;
            //    //if (!memory_stacked_size(ref reads)) { ErrorOccured(); }
            //    reads = 0;
            //    fixed (short* pntDatas = datas)
            //    {
            //        if (!memory_read((IntPtr)pntDatas, rCount, ref reads)) { ErrorOccured(); }
            //    }

            //    //Debug.WriteLine(string.Format("Want : {0}, Read : {1}", rCount, reads), "IODAUSB");

            //    int cnt = (int)reads / 2;	// reads가 2의 배수라는 보장이 없다!!!!
            //    Array.Copy(datas, 0, result, start, cnt);

            //    start += cnt;
            //    rCount -= reads;

            //    if (timeout.ElapsedMilliseconds > _ReadTimeout)
            //    {
            //        timeout.Stop();
            //        throw new TimeoutException("Fail to read IODAUSB data.");
            //    }

            //    if (start == length)
            //    {
            //        break;
            //    }
            //    else if (start > length)
            //    {
            //        string msg = "Over read.";
            //        Debug.WriteLine(msg, "IODAUSB_API");
            //        throw new System.IO.IOException(msg);
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(5);
            //        Debug.WriteLine("Re-read.");
            //    }
            //}

            //timeout.Stop();
            //return result;

            fixed (short* pntDatas = datas)
            {
                byte* pData = (byte*)pntDatas;
                while (true)
                {
                    reads = 0;
                    if (!memory_read((IntPtr)pData, rCount, ref reads)) { ErrorOccured(); }
                    rCount -= reads;
                    if (timeout.ElapsedMilliseconds > _ReadTimeout)
                    {
                        timeout.Stop();
                        throw new TimeoutException("Fail to read IODAUSB data.");
                    }

                    if (rCount == 0)
                    {
                        break;
                    }
                    else if (rCount < 0)
                    {
                        string msg = "Over read.";
                        Debug.WriteLine(msg, "IODAUSB_API");
                        throw new System.IO.IOException(msg);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5);
                        //System.Threading.Thread.Sleep(1);
                        Debug.WriteLine("Re-read.");

                        pData += reads;
                    }
                }
            }

            timeout.Stop();

            return datas;
        }

        /// <summary>
        /// 버퍼에 있는 AI sample의 수를 가져 온다.
        /// </summary>
        public static int AIbufferCount
        {
            get
            {
                uint nSize = 0;

                if (!memory_stacked_size(ref nSize)) { ErrorOccured(); }

                if (IODAUSB_API.CaptureBufferStatus == CaptureBufferStatusType.Full)
                {
                    throw new System.IO.IOException("USB translation doesn't operate.");
                }

                //Debug.WriteLine(IODAUSB_API.CaptureAioStatus.ToString() + " - " + nSize.ToString(), "IODA");
                //Debug.WriteLine(IODAUSB_API.CaptureBufferStatus.ToString(), "IODA");
                //Debug.WriteLine(IODAUSB_API.CaptureBulkinStatus.ToString(), "IODA");


                return ((int)nSize / 2);
            }
        }

        public static int AIOratio
        {
            get
            {
                byte val = 0;

                if (!get_ad_sampling_ratio(ref val)) { ErrorOccured(); }
                return (int)val;
            }
            set
            {
                if ((value < 1) || (value > 255)) { throw new ArgumentException("ratio is in 1 to 255."); }

                byte val = (byte)value;

                if (!set_ad_sampling_ratio(val)) { ErrorOccured(); }
            }
        }
        #endregion

        #region DIO managenent
        public const int DInumbers = 4;
        public const int DOnumbers = 8;

        public static long DigitalInputFlag
        {
            get
            {
                byte result = 0;

                if (!get_digital_input(ref result, 1)) { ErrorOccured(); }
                return result;
            }
        }

        public static bool[] DigitalInputs
        {
            get
            {
                byte result = 0;

                if (!get_digital_input(ref result, 1)) { ErrorOccured(); }

                bool[] bits = new bool[DInumbers];

                for (int i = 0; i < DInumbers; i++)
                {
                    bits[i] = ((result & 0x01) == 0x01);
                    result = (byte)(result >> 1);
                }

                return bits;
            }
        }

        public static bool DigitalInputGet(int channel)
        {
            if ((channel < 0) || (channel >= DInumbers)) { throw new ArgumentException("Invalid channel range. 0 <= channel <= " + DInumbers.ToString() + "."); }

            byte outdata = 0;

            if (!get_digital_input_bit(ref outdata, 1)) { ErrorOccured(); }

            return (outdata == 1);
        }

        public static bool[] DigitalOutputs
        {
            get
            {
                byte result = 0;

                if (!get_digital_output(ref result, 1)) { ErrorOccured(); }

                bool[] bits = new bool[DOnumbers];

                for (int i = 0; i < DOnumbers; i++)
                {
                    bits[i] = ((result & 0x01) == 0x01);
                    result = (byte)(result >> 1);
                }

                return bits;
            }
            set
            {
                if (value.Length != DOnumbers) { throw new ArgumentException("value's length must be same 'DOnumbers'."); }

                byte outdata = 0;
                for (int i = 0; i < DOnumbers; i++)
                {
                    outdata |= (byte)((value[i] ? 1 : 0) << i);
                }

                if (!set_digital_output(ref outdata, 1)) { ErrorOccured(); }
            }
        }

        /// <summary>
        /// DO flag
        /// </summary>
        public static long DigitalOutputFlag
        {
            get
            {
                byte result = 0;

                if (!get_digital_output(ref result, 1)) { ErrorOccured(); }
                return result;
            }
            set
            {
                if ((value & ~((int)Math.Pow(2, DOnumbers) - 1)) != 0) { throw new ArgumentException("Invalid length. Must be less then 2^DOnumbers."); }

                byte outdata = (byte)value;
                if (!set_digital_output(ref outdata, 1)) { ErrorOccured(); }
            }
        }

        public static bool DigitalOutputGet(int channel)
        {
            if ((channel < 0) || (channel > 7)) { throw new ArgumentException("Invalid channel range. 0 <= channel < DOnumbers."); }

            byte outdata = 0;

            if (!get_digital_output_bit(ref outdata, 1)) { ErrorOccured(); }

            return (outdata == 1);
        }

        public static void DigitalOutputSet(int channel, bool value)
        {
            if ((channel < 0) || (channel > DOnumbers)) { throw new ArgumentException("Invalid channel range. 0 <= channel <DOnumbers."); }

            byte outdata = (byte)(value ? 1 : 0);
            byte bitnum = (byte)channel;

            if (!set_digital_output_bit(outdata, bitnum)) { ErrorOccured(); }
        }
        #endregion
    }

    public class IODAException : Exception
    {
        public readonly int ErrorCode = 0;
        public IODAException(int err)
        {
            ErrorCode = err;
        }

        public IODAException(string message, int err)
            : base(message)
        {
            ErrorCode = err;
        }

        public IODAException(string message, Exception inner, int err)
            : base(message, inner)
        {
            ErrorCode = err;
        }
    }
}
