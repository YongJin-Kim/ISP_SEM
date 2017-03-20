using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace SEC.Nanoeye.NanoeyeSEM.Initialize
{
	public partial class Splash : Form
	{
		private static Splash _Default = null;
		public static Splash Default
		{
			get { return _Default; }
			set { _Default = value; }
		}

		public Splash()
		{
			InitializeComponent();


		}

		public string TxtCompaney
		{
			get { return labCompaney.Text; }
			set
			{
				labCompaney.Text = value;
				this.Refresh();
			}
		}

		public string TxtProduct
		{
			get { return labProduct.Text; }
			set
			{
				labProduct.Text = value;
				this.Refresh();
			}

		}

		public string TxtModel
		{
			get { return labModel.Text; }
			set
			{
				labModel.Text = value;
				this.Refresh();
			}
		}

		public string TxtMessage
		{
			get { return labMessage.Text; }
			set
			{
				labMessage.Text = value;
				this.Refresh();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (BackgroundImage == null)
			{
				System.Drawing.Drawing2D.LinearGradientBrush lgb = new System.Drawing.Drawing2D.LinearGradientBrush(
				    this.ClientRectangle,
				    Color.AliceBlue,
				    Color.RoyalBlue,
				    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);


				//LinearGradientBrush lgbw = new LinearGradientBrush(this.ClientRectangle, Color.AliceBlue, Color.Navy, spotPoint);
				lgb.GammaCorrection = true;
				lgb.Blend.Factors = new float[] { 0, 0.7f, 1 };
				lgb.Blend.Positions = new float[] { 0, 0.3f, 1 };
				
				//lgb.InterpolationColors =new ColorBlend(3);
				//lgb.InterpolationColors.Colors = new Color[] { Color.AliceBlue, Color.RoyalBlue, Color.Blue };
				//lgb.RotateTransform(spotPoint);

				e.Graphics.FillRectangle(lgb, this.ClientRectangle);

			}

			base.OnPaint(e);
		}


		public void UpdateInfo(AppDeviceEnum ade, AppSellerEnum ase, AppModeEnum ame)
		{
			switch (ase)
			{
			case AppSellerEnum.SEC:
				TxtCompaney = "SEC";
				TxtProduct = "Mini-SEM";
                //BackgroundImage = Properties.Resources.splash_bg;
                //labMessage.ForeColor = Color.FromArgb(159, 123, 167);


				switch (ade)
				{
				case AppDeviceEnum.SNE1500M:
					TxtModel = "SNE-1500M";
					break;
				case AppDeviceEnum.SNE3000M:
					TxtModel = "SNE-3000M";
					break;
				case AppDeviceEnum.SNE4000M:
					TxtModel = "SNE-4000M";
					break;
                case AppDeviceEnum.SNE4500M:
                    TxtModel = "SNE-4500M";
                    break;
                case AppDeviceEnum.SNE3200M:
                    TxtModel = "SNE-3200M";
                    break;
                case AppDeviceEnum.SNE3000MB:
                    TxtModel = "SNE-3000MB";
                    break;
				case AppDeviceEnum.AutoDetect:
                    TxtModel = "Nanoeye";
                    break;

				default:
					TxtModel = "Nanoeye";
					break;
				}
				break;
			case AppSellerEnum.Evex:
				TxtCompaney = "Evex";
				TxtProduct = "Evex Mini-SEM";
				switch (ade)
				{
				case AppDeviceEnum.SNE1500M:
					TxtModel = "SX-1500";
					break;
				case AppDeviceEnum.SNE3000M:
					TxtModel = "SX-3000";
					break;
				case AppDeviceEnum.SNE4000M:
					TxtModel = "SX-4000";
					break;
				case AppDeviceEnum.AutoDetect:
				default:
					TxtModel = TxtProduct;
					break;
				}
				break;
			case AppSellerEnum.Hirox:
				TxtCompaney = "Hirox";
				TxtProduct = "Mini-SEM";
                BackgroundImage = Properties.Resources.splash_bg2;
				switch (ade)
				{
				case AppDeviceEnum.SNE1500M:
					TxtModel = "SH-1500";
					break;
				case AppDeviceEnum.SNE3000M:
					TxtModel = "SH-3000";
					break;
				case AppDeviceEnum.SNE4000M:
                case AppDeviceEnum.AutoDetect:
					TxtModel = "SH-4000M";
					break;

                case AppDeviceEnum.SH3500MB:
                    TxtModel = "SH-3500MB";
                    break;

                case AppDeviceEnum.SH5000M:
                    TxtModel = "SH-5000M";
                    break;
				default:
					TxtModel = TxtProduct;
					break;
				}
				break;
			case AppSellerEnum.Nikkiso:
				TxtCompaney = "Nikkiso";
				TxtProduct = "SEMTRACmini";
				BackgroundImage = Properties.Resources.SEMTRAC_530x333_Square;
				switch (ade)
				{
				case AppDeviceEnum.SNE1500M:
					TxtModel = "SM-1500";
					break;
				case AppDeviceEnum.SNE3000M:
					TxtModel = "SM-3000";
					break;
				case AppDeviceEnum.SNE4000M:
					TxtModel = "SM-4000";
					break;
				case AppDeviceEnum.AutoDetect:
				default:
					TxtModel = TxtProduct;
					break;
				}
				break;
			}

			TxtMessage = "Load UI.";
		}



	}
}
