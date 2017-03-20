using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace SEC.Nanoeye.NanoImage.ScanItem
{
	internal interface IScanItem : IScanItemEvent, IDisposable
	{
		int ScanningRequest { get; set; }
		int ScanningPersentage { get; }
		DataAcquation.IDaqData DaqData { get; set; }
        

		event EventHandler SettingChangedEvent;

		void Ready();
		unsafe void Scanning(BackgroundWorker worker);
	}
}
