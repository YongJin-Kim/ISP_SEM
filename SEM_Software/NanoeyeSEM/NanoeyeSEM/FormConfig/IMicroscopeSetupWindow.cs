using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEC.Nanoeye.NanoeyeSEM.FormConfig
{
	interface IMicroscopeSetupWindow
	{
		event EventHandler ProfileListChanged;
		event EventHandler HVtextChanged;

		// 설정 값을 저장한다.
		void SettingSave();

		// 설정 값이 바뀌었음을 설정창에 알려줄때 사용한다.
		void SettingChanged();
	}
}
