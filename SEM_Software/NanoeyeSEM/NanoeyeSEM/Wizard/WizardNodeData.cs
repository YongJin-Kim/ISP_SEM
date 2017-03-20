using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SEC.Nanoeye.NanoeyeSEM.Wizard
{
	public class WizardNodeData
	{
		#region Property & Variables
		private string _Language = "";
		public string Language
		{
			get { return _Language; }
			set { _Language = value; }
		}

		Dictionary<string,string> titleDic = new Dictionary<string, string>();
		public string Title
		{
			get
			{
				if (titleDic.ContainsKey(_Language)) { return titleDic[_Language]; }
				else { return "No Titile"; }
			}
		}

		Dictionary<string,string> messageDic = new Dictionary<string, string>();
		public string Message
		{
			get
			{
				if (messageDic.ContainsKey(_Language)) { return messageDic[_Language]; }
				else { return "No Message"; }
			}
		}

		private string _ImagePath = null;
		public string ImagePath
		{
			get { return _ImagePath; }
		}

		private int _Index = -1;
		public int Index
		{
			get { return _Index; }
		}

		private string[][] _Condition = null;
		public string[][] Condition
		{
			get { return _Condition; }
		}

		private bool _IsConditionPass = false;
		public bool IsConditionPass
		{
			get { return _IsConditionPass; }
		}

		protected string[][] _Emphasis = null;
		public string[][] Emphasis
		{
			get { return _Emphasis; }
		}

		protected string[][] _Force = null;
		public string[][] Force
		{
			get { return _Force; }
		}

		protected string[][] _Equipment = null;
		public string[][] Equipment
		{
			get { return _Equipment; }
		}
		#endregion

		public WizardNodeData(System.Xml.XmlReader xr)
		{
			if (!xr.ReadToFollowing("Index"))
			{
				throw new ArgumentException("Invalid wizard data.");
			}

			if (xr.MoveToAttribute("index"))
			{
				try
				{
					_Index = xr.ReadContentAsInt();
				}
				catch (Exception ex)
				{
					Trace.WriteLine("Invalid wizard data index. - " , "Wizard");
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					_Index = -1;
				}
			}

			while(xr.Read())
			{
				
				switch(xr.NodeType){
				case System.Xml.XmlNodeType.Element:
					string name = xr.Name;

					System.Xml.XmlReader xtr= xr.ReadSubtree();

					switch (name)
					{
					case "Title":
						ReadTitle(xtr);
						break;
					case "Message":
						ReadMessage(xtr);
						break;
					case "ImagePath":
						ReadImage(xtr);
						break;
					case "Condition":
						ReadCondition(xtr);
						break;
					case "Emphasis":
						ReadEmphasis(xtr);
						break;
					case "Force":
						ReadForce(xtr);
						break;
					case "Equipment":
						ReadEquipment(xtr);
						break;
					default:
						Debug.WriteLine(xtr, "undefined - " + xtr.Name);
						break;
					}
					break;
				}
			}
		}

		#region 값 읽기
		private void ReadTitle(System.Xml.XmlReader xr)
		{
			xr.Read();
			titleDic.Clear();
			while (xr.Read())
			{
				switch (xr.NodeType)
				{
				case System.Xml.XmlNodeType.Element:
					string title;

					string name = xr.Name;
					try
					{
						//xr.Read();
						title = xr.ReadString();
						titleDic.Add(name, title);
					}
					catch (Exception ex)
					{
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					}

					break;
				}
			}
		}

		private void ReadMessage(System.Xml.XmlReader xml)
		{
			xml.Read();
 			messageDic.Clear();
			while (xml.Read())
			{
				switch (xml.NodeType)
				{
				case System.Xml.XmlNodeType.Element:
					string title;

					string name = xml.Name;
					try
					{
						//xr.Read();
						title = xml.ReadString();
						messageDic.Add(name, title);
					}
					catch (Exception ex)
					{
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					}

					break;
				}
			}		
		}

		private void ReadImage(System.Xml.XmlReader xml)
		{
			if (xml.IsEmptyElement) { return; }
			xml.Read();

			xml.Read();


			string path = xml.ReadString();
			//string path = System.IO.Path.GetDirectoryName( System.Windows.Forms.Application.ExecutablePath )+"\\"+xml.ReadString();


			if (System.IO.File.Exists(path))
			{
				_ImagePath = path;
			}
			else
			{
				_ImagePath = null;
			}
		}

		private void ReadCondition(System.Xml.XmlReader xml)
		{
			xml.Read();

			if (xml.MoveToAttribute("mode"))
			{
				string str = xml.ReadContentAsString();
				_IsConditionPass = (str == "Pass");
			}
			else
			{
				_IsConditionPass = false;
			}

			List<string[]> conditionList = new List<string[]>();

			while (xml.Read())
			{
				switch (xml.NodeType)
				{
				case System.Xml.XmlNodeType.Element:

					string[] con = new string[5];

					con[0] = xml.Name;

					if (xml.MoveToAttribute("name")) { con[1] = xml.ReadContentAsString(); }
					else
					{
						Trace.WriteLine("No condition name. - " + xml.Name, "Wizard");
						continue;
					}

					xml.MoveToFirstAttribute();
					if (xml.MoveToAttribute("type")) { con[2] = xml.ReadContentAsString(); }
					else { con[2] = "true"; }

					xml.MoveToFirstAttribute();
					if (xml.MoveToAttribute("target")) { con[3] = xml.ReadContentAsString(); }
					else { con[3] = null; }

					try
					{
						//xr.Read();
						con[4] = xml.ReadString();
						conditionList.Add(con);
					}
					catch (Exception ex)
					{
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					}

					break;
				}
			}
			_Condition = conditionList.ToArray();
		}

		private void ReadEmphasis(System.Xml.XmlReader xml)
		{
			xml.Read();

			List<string[]> emphasisList = new List<string[]>();

			while (xml.Read())
			{
				switch (xml.NodeType)
				{
				case System.Xml.XmlNodeType.Element:
					try
					{
						string[] con;
						string name = xml.Name;
						switch (name)
						{
						case "Area":
							con = new string[6];
							con[0] = xml.Name;

							if (!xml.MoveToAttribute("left"))
							{
								Trace.WriteLine("Undefined Emphasis left - " + name, "Wizard");
								continue;
							}
							con[1] = xml.ReadContentAsString();
							if (!xml.MoveToAttribute("top"))
							{
								Trace.WriteLine("Undefined Emphasis top - " + name, "Wizard");
								continue;
							}
							con[2] = xml.ReadContentAsString();
							if (!xml.MoveToAttribute("width"))
							{
								Trace.WriteLine("Undefined Emphasis width - " + name, "Wizard");
								continue;
							}
							con[3] = xml.ReadContentAsString();
							if (!xml.MoveToAttribute("height"))
							{
								Trace.WriteLine("Undefined Emphasis height - " + name, "Wizard");
								continue;
							}
							con[4] = xml.ReadContentAsString();
							con[5] = xml.ReadString();
							break;
						case "Control":
							con = new string[2];
							con[0] = xml.Name;
							con[1] = xml.ReadString();
							break;
						default:
							Trace.WriteLine("Undefined Emphasis type - " + name, "Wizard");
							continue;
						}
						emphasisList.Add(con);
					}
					catch (Exception ex)
					{
						SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
					}

					break;
				}
			}
			_Emphasis = emphasisList.ToArray();
		}

		private void ReadForce(System.Xml.XmlReader xml)
		{
			Trace.WriteLine("Force is not supported.");
		}

		private void ReadEquipment(System.Xml.XmlReader xml)
		{
			
		}
		#endregion
	}
}
