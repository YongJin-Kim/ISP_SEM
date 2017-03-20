using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Pipes;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoeyeSEM
{
	class ImageExportManager
	{
		private ImageExportManager() { }

		public enum ImageExportModeEnum
		{
			Print,
			File,
			Manager,
            autoFile
		}


        public static string SystemInfoFileName;
       
		public static void ImageExport(SEC.Nanoeye.Support.Controls.PaintPanel ppSingle, ImageExportModeEnum exportMode, bool fromImagePanel, string AutoName)
		{
			string infoString = GetSystemInfo(ppSingle);

			Bitmap exportBM;
			short[,] datas = null;

			if (fromImagePanel) { exportBM = ppSingle.ExportPicture(); }
			else
			{

				ImageExportManager iem = new ImageExportManager();

				datas = iem.GetOneFrameImage( ppSingle);
                


                
                //datas.Initialize();

                exportBM = ppSingle.ExportOriginal();

				//exportBM = ppSingle.ExportOriginal();
			}

			switch (exportMode)
			{
                case ImageExportModeEnum.autoFile:
                    ExportFile(exportBM, infoString, AutoName);
                    break;

			    case ImageExportModeEnum.File:
				    ExportFile(exportBM, infoString, AutoName);
				    break;
			    case ImageExportModeEnum.Print:
				    ExportPrint(exportBM);
				    break;
			    case ImageExportModeEnum.Manager:
				    if (fromImagePanel) { ExportManager(exportBM, infoString); }
				    else { ExportManager(datas, infoString); }
				    break;
			    default:
				    throw new ArgumentException("Undefined Export Mode");
			}



            

           

		}

		#region Export
		private static void ExportManager(short[,] datas, string infoString)
		{
			ImageExportManager iem = new ImageExportManager();
			iem.SendManager(datas, infoString);
		}

		private static void ExportManager(Bitmap exportBM, string infoString)
		{
			ImageExportManager iem = new ImageExportManager();
			iem.SendManager(exportBM, infoString);
		}

		private void SendManager(short[,] exportData, string infoString)
		{
			try
			{
				NamedPipeClientStream npcs = PipConnect();

				npcs.Write(new byte[] { 0xF1 }, 0, 1);	// 16bit array format

				npcs.Write(BitConverter.GetBytes(exportData.GetLength(1)), 0, 4);
				npcs.Write(BitConverter.GetBytes(exportData.GetLength(0)), 0, 4);

				byte[] datas = SEC.GenericSupport.Converter.ArrayToBytearray(exportData);
				npcs.Write(BitConverter.GetBytes(datas.Length), 0, 4);
				npcs.Write(datas, 0, datas.Length);

				// image를 전달 할 경우 Contast와 Brightness는 0이므로 전송 형식에 따라 다르게 전송 한다.
				//SendValue(npcs, PipeDataType.ImageInfo_Contrast, sic.Contrast);
				//SendValue(npcs, PipeDataType.ImageInfo_Brightness, sic.Brightness);

				//SendValue(npcs, PipeDataType.ImageInfo_PixelDefaultSize, (double)(1280d * ppSingle.PixelDefaultSize / exportData.GetLength(1)));

				//PipSendinfoAndClose(npcs, column, xeyeStageMediator, ppSingle, sic);
				PipSendinfoAndClose(npcs, infoString);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Fail to send image. - " + ex.Message, "Warring");
				Debug.WriteLine(ex.StackTrace, ex.Message);
				MessageBox.Show(TextManager.Instance.GetString("Message_ExportManagerFail") + "\r\n" + ex.Message, "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
		}

		private void SendManager(Bitmap exportImg, string infoString)
		{
			NamedPipeClientStream npcs = null;
			try
			{
				npcs = PipConnect();

				switch (exportImg.PixelFormat)
				{
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					npcs.Write(new byte[] { 3 }, 0, 1);
					break;
				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
					npcs.Write(new byte[] { 0 }, 0, 1);
					break;
				default:
					throw new InvalidOperationException("ExportManager - Invalid Image pixelformat");
				}

				npcs.Write(BitConverter.GetBytes(exportImg.Width), 0, 4);	// width 4bytes
				npcs.Write(BitConverter.GetBytes(exportImg.Height), 0, 4);	// height 4bytes

				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				exportImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				byte[] datas = ms.GetBuffer();
				npcs.Write(BitConverter.GetBytes(datas.Length), 0, 4);	// Image Data 4bytes
				npcs.Write(datas, 0, datas.Length);

				//// image를 전달 할 경우 Contast와 Brightness는 0이다.
				//SendValue(npcs, PipeDataType.ImageInfo_Contrast, 0);
				//SendValue(npcs, PipeDataType.ImageInfo_Brightness, 0);

				//SendValue(npcs, PipeDataType.ImageInfo_PixelDefaultSize, (double)(1280d * ppSingle.PixelDefaultSize / exportImg.Width));


				PipSendinfoAndClose(npcs, infoString);
			}
			catch (Exception ex)
			{
				if (npcs != null)
				{
					npcs.Close();
					npcs.Dispose();
					npcs = null;
				}

				Trace.WriteLine("Fail to send image.", "Warring");
				Debug.WriteLine(ex.StackTrace, ex.Message);
				MessageBox.Show(TextManager.Instance.GetString("Message_ExportManagerFail").Text, "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}
			Trace.WriteLine("Sending image to ImageDB complete.", "Info");

		}

		private void PipSendinfoAndClose(NamedPipeClientStream npcs, string infoString)
		{
			byte[] buffer = Encoding.Unicode.GetBytes(infoString);
			npcs.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
			npcs.Write(buffer, 0, buffer.Length);

			npcs.Flush();

			npcs.Close();
			npcs.Dispose();
		}

		private NamedPipeClientStream PipConnect()
		{
			NamedPipeClientStream npcs = new NamedPipeClientStream(".", "NanoeyeDBPipe", PipeDirection.InOut);
			try
			{
				npcs.Connect(100);
			}
			catch (Exception)
			{

				// Manager 프로그램을 실행 시킴.

				Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\SEC");
				if (rk == null)
				{
					throw new InvalidOperationException("NanoeyeDB is not installed.");
				}

				Process ps = new Process();

				string imgDbPath = (rk.GetValue("NanoeyeDBLocation") as string);

				if (imgDbPath == null)
				{
					throw new InvalidOperationException("NanoeyeDB is not installed.");
				}

				imgDbPath += @"\NanoeyeDB.exe";

				if (!System.IO.File.Exists(imgDbPath))
				{
					throw new InvalidOperationException("NanoeyeDB does not exist.");
				}


				ps.StartInfo.FileName = imgDbPath;


				ps.StartInfo.ErrorDialog = true;
				ps.StartInfo.CreateNoWindow = true;
				ps.Start();

				Trace.WriteLine("Image Manager Started.", "Info");

				npcs.Connect();

			}

			byte[] buffer = Encoding.Unicode.GetBytes("MiniSEM");
			npcs.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
			npcs.Write(buffer, 0, buffer.Length);

			return npcs;
		}

		private static void ExportPrint(Bitmap exportBM)
		{
			ImageExportManager iem = new ImageExportManager();

			iem.Print(exportBM);
		}

		Bitmap printBM;

		private void Print(Bitmap exportBM)
		{
			printBM = exportBM;

			System.Windows.Forms.PrintDialog pd = new PrintDialog();
			pd.Document = new System.Drawing.Printing.PrintDocument();
			if (pd.ShowDialog() == DialogResult.OK)
			{
				pd.Document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Document_PrintPage);
				pd.Document.Print();
			}
		}

		void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			if (printBM == null)
			{
				throw new InvalidOperationException("Image는 null일 수 없습니다..");
			}

			SizeF imageSize = new SizeF(480F, 360F);
			RectangleF imageBounds = new RectangleF(
				(e.PageBounds.Width - imageSize.Width) / 2,
				(e.PageBounds.Height - imageSize.Height) / 2,
				imageSize.Width,
				imageSize.Height);

			e.Graphics.DrawImage(printBM, imageBounds);
		}


        private static int fileCount = 0;
		private static void ExportFile(Bitmap exportBM, string infoString, string AutoName)
		{
			System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();

			sfd.AddExtension = true;
			sfd.DefaultExt = ".jpg";
			sfd.Filter = "Joint Photographic Experts Group (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Tagged Image File Format (*.tiff)|*.tiff";
			sfd.FilterIndex = 1;
            //sfd.InitialDirectory = Properties.Settings.Default.ArchAdress;
            
            sfd.RestoreDirectory = true;

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Properties.Settings.Default.ArchAdress);

            if (!di.Exists)
            {
                di = new System.IO.DirectoryInfo(Application.StartupPath);  
            }


            
            bool fileCountEnable = false;
            string strName = AutoName + "_" + fileCount.ToString(); 
            //while (true)
            //{
            //    fileCountEnable = false;
            //    fileCount = 0;
            //    foreach (var fi in di.GetFiles())
            //    {


            //        if (strName + ".jpg" == fi.Name)
            //        {
            //            fileCount++;
            //            strName = AutoName + "_" + fileCount.ToString();
            //            fileCountEnable = true;
            //        }
        
                    
            //    }

            //    if (fileCountEnable == false)
            //    {
            //        break;
            //    }
            //}
            AutoName = strName;
            


           

            string[] directoryAllName = sfd.FileNames;
            
            //Diretory
            


            sfd.FileName = AutoName;

            SystemInfoFileName = null;
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
                //Properties.Settings.Default.ArchAdress = sfd.
                string[] spstring = sfd.FileName.Split('\\');
                string directoryPath = null;

                for(int i = 0; i < spstring.Length - 1; i++)
                {
                    directoryPath += spstring[i] + "\\";
                }

                Properties.Settings.Default.ArchAdress = directoryPath;

				System.IO.Stream str = sfd.OpenFile();
				System.Drawing.Imaging.ImageFormat imgfor;
				switch (sfd.FilterIndex)
				{
				case 1:
					imgfor = System.Drawing.Imaging.ImageFormat.Jpeg;
					break;
				case 2:
					imgfor = System.Drawing.Imaging.ImageFormat.Png;
					break;
				case 3:
					imgfor = System.Drawing.Imaging.ImageFormat.Bmp;
					break;
				case 4:
					imgfor = System.Drawing.Imaging.ImageFormat.Tiff;
					break;
				default:
					throw new ArgumentException();
				}

				exportBM.Save(str, imgfor);

                
				str.Flush();
				str.Close();
				str.Dispose();
                //str.Close();

                exportBM.Dispose();
                //exportBM.Dispose();
                

                

                SystemInfoFileName = sfd.FileName;

                if (sfd.FileName.Contains(AutoName))
                {
                    Properties.Settings.Default.ArchAdress = SystemInfoFileName.Remove(SystemInfoFileName.Length - (AutoName.Length+5));
                }

                sfd.Dispose();
                //sfd.Dispose();
                //GC.Collect();
                

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName + ".txt");
                //sw.Write(infoString);
                //sw.Flush();
                //sw.Close();
                //sw.Dispose();
                fileCount++;
                
			}

            if (exportBM != null)
            {
                exportBM.Dispose();
            }


		}

       

		#endregion

		#region Export One Frame

		private SEC.Nanoeye.NanoImage.IScanItemEvent isie;

		FormProgressNotify fmProcess = null;

		private short[,] imgDatas = null;

		SEC.Nanoeye.Support.Controls.PaintPanel ppSingle;

		int proc = 0;

		private short[,] GetOneFrameImage(SEC.Nanoeye.Support.Controls.PaintPanel ppSingle)
		{
			fmProcess = new FormProgressNotify("Image Export");
			fmProcess.NotifyMessage = "Acquire image.";
			fmProcess.Owner = Application.OpenForms[0];
			fmProcess.ProgressChecking += new EventHandler<ProgressCheckingEventArgs>(fmProcess_ProgressChecking);
			fmProcess.TimerEnabled = true;
			fmProcess.TimerInterval = 200;
            fmProcess.TopMost = true;

			this.ppSingle = ppSingle;

			SEC.Nanoeye.NanoImage.IActiveScan scanner = SystemInfoBinder.Default.Nanoeye.Scanner;

			isie = scanner.ItemsRunning[0];

			isie.FrameUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(ImageExportManager_FrameUpdated);
			isie.ScanLineUpdated += new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(ImageExportManager_ScanLineUpdated);

			fmProcess.ShowDialog();
            
			isie.ScanLineUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(ImageExportManager_ScanLineUpdated);
			isie.FrameUpdated -= new SEC.Nanoeye.NanoImage.ScanDataUpdateDelegate(ImageExportManager_FrameUpdated);
            

			return imgDatas;
		}

		void ImageExportManager_ScanLineUpdated(object sender, string name, int startline, int lines)
		{
			proc = (startline + lines) * 100 / isie.Setting.ImageHeight;
		}

		void ImageExportManager_FrameUpdated(object sender, string name, int startline, int lines)
		{
			//imgDatas = new short[isie.Setting.ImageHeight * isie.Setting.ImageWidth];
			//System.Runtime.InteropServices.Marshal.Copy(isie.ImageData, imgDatas, 0, isie.Setting.ImageHeight * isie.Setting.ImageWidth);

            
			//imgDatas = ppSingle.ExportData();
            

			Action act = () => { fmProcess.Dispose(); };
            
			fmProcess.Invoke(act);
		}

		void fmProcess_ProgressChecking(object sender, ProgressCheckingEventArgs e)
		{
			e.Progress = proc;
		}
		#endregion

		#region Generate Image Info
		private static string GetSystemInfo(SEC.Nanoeye.Support.Controls.PaintPanel ppSingle)
		{
			StringBuilder sb = new StringBuilder();

			XmlWriterSettings xmlSet = new XmlWriterSettings();
			xmlSet.Indent = true;
			xmlSet.OmitXmlDeclaration = true;
			XmlWriter writer = XmlWriter.Create(sb, xmlSet);

			writer.WriteStartDocument();

			writer.WriteStartElement("MiniSEM_Image_Info", "http://www.seceng.co.kr/sem/ImageInfo/MiniSEM");

			writer.WriteElementString("DateTime", DateTime.Now.ToString());

			WriteRegularInfo(ppSingle, writer);

			WriteColumnInfo(writer);

			WriteScannerInfo(writer);

			WritePaintInfo(ppSingle, writer);

			writer.WriteEndElement();	//MiniSEM_Image_Info

			writer.Flush();

			//System.Windows.Forms.MessageBox.Show(sb.ToString());

			return sb.ToString();
		}

		private static void WriteRegularInfo(SEC.Nanoeye.Support.Controls.PaintPanel ppSingle, XmlWriter writer)
		{
			writer.WriteStartElement("RegularInfo");

			SEC.Nanoeye.NanoColumn.ISEMController ims = SystemInfoBinder.Default.Nanoeye.Controller;

			string hvTxt;
			try
			{
				if (ims.HVtext == null)
				{
					hvTxt = (-1d).ToString();
				}
				else
				{
					switch (ims.HVtext.Substring(0, 2))
					{
					case "30":
						hvTxt = (30d).ToString();
						break;
					case "25":
						hvTxt = (25d).ToString();
						break;
					case "20":
						hvTxt = (20d).ToString();
						break;
					case "15":
						hvTxt = (15d).ToString();
						break;
					case "10":
						hvTxt = (10d).ToString();
						break;
					case "5k":
						hvTxt = (5d).ToString();
						break;
					case "1k":
						hvTxt = (1d).ToString();
						break;
					default:
						hvTxt = (-1d).ToString();
						break;
					}
				}
			}
			catch (Exception)
			{
				hvTxt = (-1d).ToString();
			}
			writer.WriteElementString("HV", hvTxt);

			double dTemp;

			//Emission	Double
			//try { dTemp = (double)ims.HvElectronGun.Read[1]; }
			try { dTemp = (double)((SystemInfoBinder.Default.Equip.ColumnHVGun as SECtype.IControlDouble).Read[1]); }
			catch (NullReferenceException) { dTemp = -1; }
			writer.WriteElementString("Emission", dTemp.ToString());

			//CL1	Double
			//try { dTemp = (double)ims.LensCondenser1.Read[0]; }
			try { dTemp = (double)((SystemInfoBinder.Default.Equip.ColumnLensCL1 as SECtype.IControlDouble).Read[1]); }

            //try { dTemp = SystemInfoBinder.Default.Equip.ColumnLensCL1; }
            catch (NullReferenceException) { dTemp = -1; }
			writer.WriteElementString("CL1", dTemp.ToString());

			//CL2	Double
			//try { dTemp = (double)ims.LensCondenser2.Read[0]; }
			try { dTemp = (double)((SystemInfoBinder.Default.Equip.ColumnLensCL2 as SECtype.IControlDouble).Read[1]); }
			catch (NullReferenceException) { dTemp = -1; }
			writer.WriteElementString("CL2", dTemp.ToString());
			//Magnification	Int32
			//writer.WriteElementString("Magnification", (ppSingle.Magnification / 2).ToString());
            writer.WriteElementString("Magnification", (ppSingle.Magnification).ToString());
			//WorkingDistance	Int32
			//writer.WriteElementString("WD", (-1).ToString());

			//MicronBack	Int32
			writer.WriteElementString("MicronBack", ppSingle.BackColor.ToArgb().ToString());
			//MicronEdge	Int32
			writer.WriteElementString("MicronEdge", ppSingle.EdgeColor.ToArgb().ToString());
			//MicronFont	Object(Font)
			//writer.WriteElementString("MicronFont", ppSingle.Font.ToString());
			//writer.WriteStartElement("MicronFont");
			//new System.Xml.Serialization.XmlSerializer(typeof(Font)).Serialize(writer, ppSingle.Font);
			//writer.WriteEndElement();

			//MicronFore	Int32
			writer.WriteElementString("MicronFore", ppSingle.ForeColor.ToArgb().ToString());
			//MicronText	String
			writer.WriteElementString("MicronText", ppSingle.MicronDescriptor.ToString());
			//MicronScanningTime	Double
			//writer.WriteElementString("MicronScanningTime", ppSingle.BackColor.ToArgb().ToString());
			//MicronContrast	Int32
			writer.WriteElementString("MicronContrast", ppSingle.Contrast.ToString());
			//MicronBightness	Int32
			writer.WriteElementString("MicronBightness", ppSingle.Brightness.ToString());
			//MicronPixelSize	Double
			writer.WriteElementString("MicronPixelSize", ppSingle.LengthPerPixel.ToString());


			writer.WriteEndElement();

			//AxisX
			//AxisY
			//AxisR
			//AxisT
			//AxisZ
		}

		private static void WriteColumnInfo(XmlWriter writer)
		{
			writer.WriteStartElement("ColumnInfo");

			foreach (SEC.GenericSupport.DataType.IValue icv in SystemInfoBinder.Default.Nanoeye.Controller)
			{
				writer.WriteStartElement(icv.Name);

				writer.WriteStartAttribute("ControlType");

				if (icv is SEC.GenericSupport.DataType.IControlBool)
				{
					writer.WriteString("IControlBool");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.IControlBool icb = icv as SEC.GenericSupport.DataType.IControlBool;

					writer.WriteStartElement("Value");
					writer.WriteString(icb.Value.ToString());
				}
				else if (icv is SEC.GenericSupport.DataType.IControlDouble)
				{
					writer.WriteString("IControlDouble");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.IControlDouble icd = icv as SEC.GenericSupport.DataType.IControlDouble;
					writer.WriteStartElement("Value");
					writer.WriteString(icd.Value.ToString());
				}
				else if (icv is SEC.GenericSupport.DataType.IControlInt)
				{
					writer.WriteString("IControlInt");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.IControlInt ici = icv as SEC.GenericSupport.DataType.IControlInt;
					writer.WriteStartElement("Value");
					writer.WriteString(ici.Value.ToString());
				}
				else if (icv is SEC.GenericSupport.DataType.IControlLong)
				{
					writer.WriteString("IControlLong");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.IControlLong ici = icv as SEC.GenericSupport.DataType.IControlLong;
					writer.WriteStartElement("Value");
					writer.WriteString(ici.Value.ToString());
				}
				else if (icv is SEC.GenericSupport.DataType.IControlShort)
				{
					writer.WriteString("IControlShort");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.IControlLong ici = icv as SEC.GenericSupport.DataType.IControlLong;
					writer.WriteStartElement("Value");
					writer.WriteString(ici.Value.ToString());
				}
				else if (icv is SEC.GenericSupport.DataType.ITable)
				{
					writer.WriteString("IControlTable");
					writer.WriteEndAttribute();

					SEC.GenericSupport.DataType.ITable ict = icv as SEC.GenericSupport.DataType.ITable;
					writer.WriteStartElement("Value");
					writer.WriteString(ict.SeletedItem.ToString());
				}
				else
				{
					writer.WriteString("IValue");
					writer.WriteEndAttribute();
					writer.WriteStartElement("NULL");
				}

				writer.WriteEndElement();

				WriteColumnValue(writer, icv);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();	// ColumnInfo
		}

		private static void WriteColumnValue(XmlWriter writer, SEC.GenericSupport.DataType.IValue icv)
		{
			writer.WriteStartElement("IsColumnValue");
			if (icv is SEC.Nanoeye.NanoColumn.IColumnValue)
			{
				writer.WriteString("True");
			}
			else
			{
				writer.WriteString("False");
			}
			writer.WriteEndElement();

			object[] readV;
			try
			{
				readV = icv.Read;
			}
			catch (NotSupportedException)
			{
				return;
			}

			if (readV == null) { return; }

			for (int i = 0; i < readV.Length; i++)
			{
				writer.WriteStartElement("Read" + i.ToString());
				if (readV[i] == null)
				{
					writer.WriteString("NULL");
				}
				else
				{
					writer.WriteString(readV[i].ToString());
				}
				writer.WriteEndElement();
			}
		}

		private static void WriteScannerInfo(XmlWriter writer)
		{
			writer.WriteStartElement("ScannerInfo");
			SEC.Nanoeye.NanoImage.IScanItemEvent[] runList = SystemInfoBinder.Default.Nanoeye.Scanner.ItemsRunning;

			if (runList == null) { return; }

			if  (runList.Length > 0)
			{
				SEC.Nanoeye.NanoImage.IScanItemEvent isie = runList[0];

				writer.WriteStartElement("Name");
				//writer.WriteStartAttribute("Type");
				//writer.WriteString("String");
				//writer.WriteEndAttribute();
				writer.WriteAttributeString("Type", "String");
				writer.WriteString(isie.Name);
				writer.WriteEndElement();

				writer.WriteStartElement("AiChannel");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AiChannel.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AiClock");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AiClock.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AiDifferential");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Bool");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AiDifferential.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AiMaximum");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AiMaximum.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AiMinimum");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AiMinimum.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AoClock");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AoClock.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AoMaximum");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AoMaximum.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AoMinimum");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AoMinimum.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AreaShiftX");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AreaShiftX.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("AreaShiftY");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.AreaShiftY.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("BlurLevel");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.BlurLevel.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("FrameHeight");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.FrameHeight.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("FrameWidth");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.FrameWidth.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ImageHeight");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ImageHeight.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ImageLeft");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ImageLeft.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ImageTop");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ImageTop.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ImageWidth");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ImageWidth.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("LineAverage");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.LineAverage.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("PaintHeight");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.PaintHeight.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("PaintWidth");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.PaintWidth.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("PaintX");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.PaintX.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("PaintY");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Float");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.PaintY.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("PropergationDelay");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.PropergationDelay.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("RatioX");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.RatioX.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("RatioY");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.RatioY.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("SampleComposite");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Int32");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.SampleComposite.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ShiftX");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ShiftX.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("ShiftY");
				writer.WriteStartAttribute("Type");
				writer.WriteString("Double");
				writer.WriteEndAttribute();
				writer.WriteString(isie.Setting.ShiftY.ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();	// ScannerInfo
		}

		private static void WritePaintInfo(SEC.Nanoeye.Support.Controls.PaintPanel ppSingle, XmlWriter writer)
		{
			writer.WriteStartElement("PaintInfo");

			writer.WriteStartElement("BackColor");
			writer.WriteAttributeString("Type", "Color");
			writer.WriteString(ppSingle.BackColor.ToArgb().ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("Brightness");
			writer.WriteAttributeString("Type", "Int32");
			writer.WriteString(ppSingle.Brightness.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("Contrast");
			writer.WriteAttributeString("Type", "Int32");
			writer.WriteString(ppSingle.Contrast.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("EdgeColor");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Color");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.EdgeColor.ToArgb().ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("Font");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Font");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.Font.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("ForeColor");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Color");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.ForeColor.ToArgb().ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("LenthOf640Pixel");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Decimal");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.LenthOf640Pixel.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("Magnification");
			writer.WriteAttributeString("Type", "Int32");
			writer.WriteString(ppSingle.Magnification.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("MicronDescriptor");
			writer.WriteStartAttribute("Type");
			writer.WriteString("String");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.MicronDescriptor.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("MicronEghv");
			writer.WriteStartAttribute("Type");
			writer.WriteString("String");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.MicronEghv.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("MicronEnable");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Bool");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.MicronEnable.ToString());
			writer.WriteEndElement();

            writer.WriteStartElement("MicronVoltage");
            writer.WriteStartAttribute("Type");
            writer.WriteString("Bool");
            writer.WriteEndAttribute();
            writer.WriteString(ppSingle.MicronVoltage.ToString());
            writer.WriteEndElement();

			writer.WriteStartElement("MicronMagnification");
			writer.WriteStartAttribute("Type");
			writer.WriteString("String");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.MicronMagnification.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("PixelDefaultSize");
			writer.WriteStartAttribute("Type");
			writer.WriteString("Double");
			writer.WriteEndAttribute();
			writer.WriteString(ppSingle.LengthPerPixel.ToString());
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		#endregion
	}
}
