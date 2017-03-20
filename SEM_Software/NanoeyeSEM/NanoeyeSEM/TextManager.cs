using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace SEC.Nanoeye.NanoeyeSEM
{
	class TextManager
	{
		#region Proerpty & Variables
		public struct TextStruct
		{
			public readonly Font Font;
			public readonly string Text;
			public TextStruct(Font font, string text)
			{
				this.Font = font;
				this.Text = text;
			}
		}

		private Dictionary<string,Font> fontData;

		Dictionary<string,Dictionary<string,TextStruct>> strDic;

		private Dictionary<string,string> languageDic = new Dictionary<string, string>();
		public string[] LanguageList
		{
			get { return languageDic.Keys.ToArray(); }
		}

		private string _Language = "";
		private string lan;
		public string Language
		{
			get { return _Language; }
			set
			{
				_Language = value;
				if (languageDic.ContainsKey(_Language))
				{
					lan = languageDic[_Language];
				}
			}
		}

		private static TextManager _Instance = new TextManager();
		public static TextManager Instance
		{
			get { return _Instance; }
		}

		private string _SchemaInfo = null;
		public string SchemaInfo
		{
			get { return _SchemaInfo; }
		}

		private string _Version = null;
		public string Version
		{
			get { return _Version; }
		}

		private string _Target = null;
		public string Target
		{
			get { return _Target; }
		}
		#endregion

		private TextManager() { }

		#region Load Data
		/// <summary>
		/// 파일로 부터 UI의 표시 문자 정보를 읽어 온다.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool LoadStringData(string path)
		{
			try
			{
				_Language = Properties.Settings.Default.Language;

				strDic = new Dictionary<string, Dictionary<string, TextStruct>>();
				fontData = new Dictionary<string, Font>();

				if (!System.IO.File.Exists(path)) { return false; }

				XmlTextReader xtr = new XmlTextReader(path);
				xtr.WhitespaceHandling = WhitespaceHandling.None;

				if (!xtr.ReadToFollowing("textData")) { return false; }
				_SchemaInfo = xtr.NamespaceURI;
				if (xtr.MoveToAttribute("Target"))
				{
					_Target = xtr.ReadContentAsString();
				}
				if (xtr.MoveToAttribute("Version"))
				{
					_Version = xtr.ReadContentAsString();
				}

				while (xtr.Read())
				{
					if (xtr.NodeType == XmlNodeType.Element)
					{
						switch (xtr.Name)
						{
						case "fonts":
							ReadFont(xtr);
							break;
						case "languages":
							ReadLanguage(xtr);
							Language = _Language;
							break;
						case "items":
							ReadItems(xtr);
							break;
						default:
							Debug.WriteLine("Undefined element type. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
							return false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				return false;
			}
			return true;
		}

		private void ReadItems(XmlTextReader xtr)
		{
			bool cont = true;

			while (xtr.Read())
			{
				switch (xtr.NodeType)
				{
				case XmlNodeType.Element:
					if (xtr.Name == "item")
					{
						string name = null;

						if (xtr.MoveToAttribute("key"))
						{
							name = xtr.ReadContentAsString();
						}
						else
						{
							Trace.WriteLine("Undefined key. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
							continue;
						}

						Dictionary<string, TextStruct> dic = new Dictionary<string, TextStruct>();

						cont = true;
						while (cont && xtr.Read())
						{
							switch (xtr.NodeType)
							{
							case XmlNodeType.Element:
								switch (xtr.Name)
								{
								case "string":
									try
									{
										string font = null;
										string lan = null;
										string msg = null;

										Font fn;

										if (xtr.MoveToAttribute("font")) { font = xtr.ReadContentAsString(); }

										if ((font == null) || (!fontData.ContainsKey(font)))
										{
											fn = SystemFonts.DefaultFont;
											Trace.WriteLine("Undfeind font." + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
										}
										else
										{
											fn = fontData[font];
										}


										if (xtr.MoveToAttribute("language"))
										{
											lan = xtr.ReadContentAsString();
										}
										msg = xtr.ReadString();

										dic.Add(lan, new TextStruct(fn, msg));
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									}
									break;
								default:
									Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									break;
								}
								break;
							case XmlNodeType.EndElement:
								if (xtr.Name == "item")
								{
									cont = false;
								}
								break;
							}
						}

						if ((name != null) && (dic.Count>0))
						{
							strDic.Add(name, dic);
						}
					}
					break;
				case XmlNodeType.EndElement:
					if (xtr.Name == "items") { return; }
					break;
				}
			}
		}

		private void ReadLanguage(XmlTextReader xtr)
		{
			bool cont = true;

			while (xtr.Read())
			{
				switch (xtr.NodeType)
				{
				case XmlNodeType.Element:
					if (xtr.Name == "language")
					{
						string dis = null;
						string code = null;

						cont = true;
						while (cont && xtr.Read())
						{
							switch (xtr.NodeType)
							{
							case XmlNodeType.Element:
								switch (xtr.Name)
								{
								case "display":
									try
									{
										dis = xtr.ReadString();
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									}
									break;
								case "code":
									try
									{
										code = xtr.ReadString();
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									}
									break;
								default:
									Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									break;
								}
								break;
							case XmlNodeType.EndElement:
								if (xtr.Name == "language")
								{
									cont = false;
								}
								break;
							}
						}

						if ((dis != null) && ( code != null))
						{
							languageDic.Add(dis, code);
						}
					}
					break;
				case XmlNodeType.EndElement:
					if (xtr.Name == "languages") { return; }
					break;
				}
			}
		}

		private void ReadFont(XmlTextReader xtr)
		{
			bool cont = true;

			while (xtr.Read())
			{
				switch (xtr.NodeType)
				{
				case XmlNodeType.Element:
					if (xtr.Name == "font")
					{
						string key = null;
						string font = SystemFonts.DefaultFont.Name;
						float size = SystemFonts.DefaultFont.Size;

						bool bold = false;
						bool italic = false;

						cont = true;
						while (cont && xtr.Read())
						{
							switch (xtr.NodeType)
							{
							case XmlNodeType.Element:
								switch (xtr.Name)
								{
								case "key":
									//key = xtr.ReadElementContentAsString();
									key = xtr.ReadString();
									break;
								case "font":
									try
									{
										font = xtr.ReadString();
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
										font = SystemFonts.DefaultFont.Name;
									}
									break;
								case "size":
									try
									{
										size = float.Parse(xtr.ReadString());
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
										size = SystemFonts.DefaultFont.Size;
									}
									break;
								case "bold":
									try
									{
										bold = Boolean.Parse(xtr.ReadString());
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									}
									break;
								case "italic":
									try
									{
										italic = Boolean.Parse(xtr.ReadString());
									}
									catch
									{
										Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									}
									break;
								default:
									Trace.WriteLine("Can't parse Font data. line - " + xtr.LineNumber.ToString() + ", pos - " + xtr.LinePosition.ToString(), "TextManager");
									break;
								}
								break;
							case XmlNodeType.EndElement:
								if (xtr.Name == "font")
								{
									cont = false;
								}
								break;
							}
						}

						if (key != null)
						{
							Font fn;

							FontStyle fs  = FontStyle.Regular;

							if (bold) { fs |= FontStyle.Bold; }
							if (italic) { fs |= FontStyle.Italic; }

							try
							{
								fn = new Font(font, size, fs);
							}
							catch
							{
								fn = SystemFonts.DefaultFont;
							}

							fontData.Add(key, fn);
						}
					}
					break;
				case XmlNodeType.EndElement:
					if (xtr.Name == "fonts") { return; }
					break;
				}
			}
		}
		#endregion

		#region Get Data
		/// <summary>
		/// Font 정보를 가져온다.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Font FontData(string name)
		{
			if (fontData.ContainsKey(name)) { return fontData[name]; }
			else { return SystemFonts.DefaultFont; }
		}

		/// <summary>
		/// 문자열을 가져 온다.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool GetString(string type, out TextStruct data)
		{
			bool result;

			result = strDic.ContainsKey(type);
			if (result)
			{
				Dictionary<string,TextStruct> dic = strDic[type];

				if (dic.ContainsKey(lan)) { data = dic[lan]; }
				else { data = dic.Values.First(); }
			}
			else
			{
				Trace.WriteLine("No string information. - " + type, "TextManager");

				data = new TextStruct(SystemFonts.DefaultFont, type);
			}

			return result;
		}

		/// <summary>
		/// 문자열 정보를 가져 온다.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public TextStruct GetString(string type)
		{
			if (strDic.ContainsKey(type))
			{
				Dictionary<string,TextStruct> dic = strDic[type];

				if (dic.ContainsKey(lan)) { return dic[lan]; }
				else { return dic.Values.First(); }
			}
			else
			{
				Trace.WriteLine("No string information. - " + type, "TextManager");
				return new TextStruct(SystemFonts.DefaultFont, type);
			}
		}
		#endregion

		#region Define UI
		/// <summary>
		/// Control의 표시 문자를 정의 한다.
		/// </summary>
		/// <param name="con"></param>
		public void DefineText(Control con)
		{
			string str;
			TextManager.TextStruct ts;

			try
			{
				str = con.Text;

				if (str != null)
				{
					if (GetString(str, out ts))
					{
						con.Font = ts.Font;
						con.Text = ts.Text;
					}
					else
					{

					}
				}
			}
			catch { }

			foreach (Control iCon in con.Controls)
			{
				if (iCon is MenuStrip)
				{
					DefineText((iCon as MenuStrip));
				}
				else if (iCon is Infragistics.Win.UltraWinTabControl.UltraTabControl)
				{
					Infragistics.Win.UltraWinTabControl.UltraTabControl utc = iCon as Infragistics.Win.UltraWinTabControl.UltraTabControl;
					DefineText(utc);
					DefineText(iCon);
				}
				else
				{
					DefineText(iCon);
				}
			}
		}

		/// <summary>
		/// MenuStrip의 표시 문자를 정의 한다.
		/// </summary>
		/// <param name="menu"></param>
		public void DefineText(MenuStrip menu)
		{
			foreach (ToolStripItem tsi in menu.Items)
			{
				DefineText(tsi);
			}
		}

		/// <summary>
		/// ToolStripItem의 표시 문자를 정의 한다.
		/// </summary>
		/// <param name="tsi"></param>
		private void DefineText(ToolStripItem tsi)
		{
			if (tsi is ToolStripMenuItem)
			{
				ToolStripMenuItem tsmi  = tsi as ToolStripMenuItem;

				string str;
				TextManager.TextStruct ts;
				try
				{
					str = tsmi.Text;

					if ((str != null) || (str != ""))
					{
						if (GetString(str, out ts))
						{
							tsmi.Text = ts.Text;
							tsmi.Font = ts.Font;
						}
					}
				}
				catch { }

				foreach (ToolStripItem iTsi in tsmi.DropDownItems)
				{
					DefineText(iTsi);
				}
			}
		}

		/// <summary>
		/// UltraTabControl의 표시 문자를 정의 한다.
		/// </summary>
		/// <param name="utc"></param>
		public void DefineText(Infragistics.Win.UltraWinTabControl.UltraTabControl utc)
		{
			foreach (Infragistics.Win.UltraWinTabControl.UltraTab ut in utc.Tabs)
			{
				string str;
				TextManager.TextStruct ts;
				try
				{
					str = ut.Text;
					if ((str != null) || (str != ""))
					{
						if (GetString(str, out ts))
						{
							ut.Text = ts.Text;
						}
					}
				}
				catch { }
			}
		}
		#endregion
	}
}
