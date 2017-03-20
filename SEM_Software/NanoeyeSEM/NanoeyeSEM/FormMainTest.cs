using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
    public partial class NewMiniSEM : Form
    {
        #region Variablese
        DateTime communicationErrorLastTime;

        #endregion

        #region 생성자 & 초기화 & 소멸자
        public NewMiniSEM()
            : this(AppDeviceEnum.AutoDetect, AppSellerEnum.AutoDetect, AppModeEnum.Run)
        {
        }

        public NewMiniSEM(AppDeviceEnum device, AppSellerEnum seller, AppModeEnum mode)
        {
            this.Opacity = 0;

            communicationErrorLastTime = DateTime.Now;

            SystemInfoBinder.Default.AppDevice = device;
            SystemInfoBinder.Default.AppSeller = seller;
            SystemInfoBinder.Default.AppMode = mode;

            int cultureCode = 0;

            switch (Properties.Settings.Default.Language)
            {
                case "Korean":
                    cultureCode = 0x0412;	//ko-KR
                    break;
                case "Japanese":
                    cultureCode = 0x0411;	//ja-JP
                    break;
                case "English":
                default:
                    cultureCode = 0x0409;	//en-US
                    break;
            }


            InitializeComponent(); ;
        }

        #endregion

        
    }
}
