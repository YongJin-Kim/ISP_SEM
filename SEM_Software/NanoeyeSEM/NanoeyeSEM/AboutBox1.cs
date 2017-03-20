using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace SEC.Nanoeye.NanoeyeSEM
{
	partial class AboutBox1 : Form
	{
		public AboutBox1()
		{
			InitializeComponent();
		}

		private void InfoChanged()
		{
			//  어셈블리 정보에서 제품 정보를 표시하기 위해 AboutBox를 초기화합니다.
			//  다음 방법 중 하나를 사용하여 응용 프로그램의 어셈블리 정보 설정을 변경합니다.
			//  - [프로젝트]->[속성]->[응용 프로그램]->[어셈블리 정보]
			//  - AssemblyInfo.cs
			this.Text = String.Format("{0} Information", SystemInfoBinder.Default.AppDevice);
			this.labelProductName.Text = SystemInfoBinder.Default.AppDevice.ToString();
			this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
			this.labelCompanyName.Text = SystemInfoBinder.Default.AppSeller.ToString();
			switch ( SystemInfoBinder.Default.AppSeller ) {
			case AppSellerEnum.SEC:
			case AppSellerEnum.Hirox:
			case AppSellerEnum.AutoDetect:
				this.labelCopyright.Text = "Copyright © 2010 SEC";
				this.textBoxDescription.Text = "SEC Co.,Ltd\r\nTech Support : 82-31-215-7341\r\nwww.seceng.co.kr\r\nsecmaster@seceng.co.kr";
				break;
			case AppSellerEnum.Evex:
				this.labelCopyright.Text = "Copyright © 2010 Evex";
				this.textBoxDescription.Text = "Evex Inc.\r\nTech Support – 609-252-9192\r\nwww.evex.com";
				break;
			case AppSellerEnum.Nikkiso:
				this.labelCopyright.Text = "Copyright © 2010 NIKKISO";
				this.textBoxDescription.Text = "NIKKISO Co.,Ltd\r\nPhone : +81-3-3443-3732\r\nFax: +81-3-3473-5473\r\nhttp://www.nikkiso.co.jp";
				break;
			default:
				throw new ArgumentException("정의되지 않은 Companey Type");
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			InfoChanged();
		}

		#region 어셈블리 특성 접근자

		public static string AssemblyTitle
		{
			get
			{
				// 이 어셈블리에 있는 모든 Title 특성을 가져옵니다.
				//object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				object[] attributes = Assembly.LoadFrom(Application.ExecutablePath).GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				// 하나 이상의 Title 특성이 있는 경우
				if (attributes.Length > 0) {
					// 첫 번째 항목을 선택합니다.
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					// 빈 문자열이 아닌 경우 항목을 반환합니다.
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}
				// Title 특성이 없거나 Title 특성이 빈 문자열인 경우 .exe 이름을 반환합니다.
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				//return Assembly.GetExecutingAssembly().GetName().Version.ToString();
				return Assembly.LoadFrom(Application.ExecutablePath).GetName().Version.ToString();
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				// 이 어셈블리에 있는 모든 Description 특성을 가져옵니다.
				//object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				object[] attributes = Assembly.LoadFrom(Application.ExecutablePath).GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				// Description 특성이 없는 경우 빈 문자열을 반환합니다.
				if (attributes.Length == 0)
					return "";
				// Description 특성이 있는 경우 해당 값을 반환합니다.
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				// 이 어셈블리에 있는 모든 Product 특성을 가져옵니다.
				//object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				object[] attributes = Assembly.LoadFrom(Application.ExecutablePath).GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				// Product 특성이 없으면 빈 문자열을 반환합니다.
				if (attributes.Length == 0)
					return "";
				// Product 특성이 있으면 해당 값을 반환합니다.
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				// 이 어셈블리에 있는 모든 Copyright 특성을 가져옵니다.
				//object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				object[] attributes = Assembly.LoadFrom(Application.ExecutablePath).GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				// Copyright 특성이 없으면 빈 문자열을 반환합니다.
				if (attributes.Length == 0)
					return "";
				// Copyright 특성이 있으면 해당 값을 반환합니다.
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				// 이 어셈블리에 있는 모든 Company 특성을 가져옵니다.
				//object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				object[] attributes = Assembly.LoadFrom(Application.ExecutablePath).GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				// Company 특성이 없으면 빈 문자열을 반환합니다.
				if (attributes.Length == 0)
					return "";
				// Company 특성이 있으면 해당 값을 반환합니다.
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion
	}
}
