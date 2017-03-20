using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;

using SECtype = SEC.GenericSupport.DataType;
using SECcolumn = SEC.Nanoeye.NanoColumn;


namespace SEC.Nanoeye.NanoeyeSEM.Wizard
{
	public partial class WizardViewer : Form
	{
		#region Property & Variables
		System.Threading.Thread wizardParser;

		private static WizardViewer _Default = null;
		public static WizardViewer Default
		{
			get
			{
				if (_Default == null) { _Default = new WizardViewer(); }
				if (_Default.IsDisposed) { _Default = new WizardViewer(); }
				return _Default;
			}
		}

		public static bool IsCreated
		{
			get
			{
				if (_Default == null) { return false; }
				if (_Default.IsDisposed) { return false; }
				return true;
			}
		}

		LinkedList<WizardNodeData> wizardLL = new LinkedList<WizardNodeData>();
		LinkedListNode<WizardNodeData> wizardNode;
		#endregion

		#region Wizard Load
		private string _WizardPath = null;
		public string WizardPath
		{
			get { return _WizardPath; }
			set
			{
				_WizardPath = value;
				conditionTimer.Enabled = false;
				tableLayoutPanel1.Visible = false;
				waitLab.Text = "Wait for read wizard data.";
				waitLab.Visible = true;

				if (wizardParser.IsAlive) { wizardParser.Abort(); }
				wizardParser = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(WizardParseMethod));
				wizardParser.Start();
			}
		}

		private void WizardLoadFail(string message)
		{
			Action<string> actReadFail = msg => { waitLab.Text = "Fail to read wizard data.\r\n" + msg; };

			if (this.InvokeRequired) { this.BeginInvoke(actReadFail, message); }
			else { actReadFail(message); }
		}

		private void WizardLoadSucess(string wizardName, List<string> languageList)
		{
			Action<string,List<string>> actReadSucess =(name, list) =>
			{
				waitLab.Visible = false;

				nameLab.Text = name;
				languageCb.Items.Clear();
				languageCb.Items.AddRange(languageList.ToArray());
				if (languageCb.Items.Contains(Properties.Settings.Default.Language))
				{
					languageCb.SelectedItem = Properties.Settings.Default.Language;
				}
				else
				{
					languageCb.SelectedIndex = 0;
				}

				SelectedNodeChanged();

				conditionTimer.Enabled = true;

				tableLayoutPanel1.Visible = true;
			};

			if (this.InvokeRequired) { this.BeginInvoke(actReadSucess, wizardName, languageList); }
			else { actReadSucess(wizardName, languageList); }
		}

		private void WizardParseMethod(object arg)
		{
			Trace.WriteLine("Wizard load start - " + _WizardPath, "Wizard");
			System.Xml.XmlReader xtr = System.Xml.XmlReader.Create(_WizardPath);

			List<string> languageList = new List<string>();
			List<WizardNodeData> indexList = new List<WizardNodeData>();
			string wizardName;


			// wizard 선언을 찾음. 없으면 사용 불가.
			if (!xtr.ReadToFollowing("Wizard")) { WizardLoadFail("Can't find Wizard."); }



			// 정의된 언어 목록
			if (xtr.MoveToAttribute("Language"))
			{
				string lanTemp = xtr.ReadContentAsString();

				languageList.AddRange(lanTemp.Split(','));
			}
			else
			{
				WizardLoadFail("Can't find language information.");
			}

			xtr.MoveToFirstAttribute();
			if (xtr.MoveToAttribute("Name")) { wizardName = xtr.ReadContentAsString(); }
			else { wizardName = "Undefined"; }

			Trace.WriteLine("Name - " + wizardName, "Wizard");

			// 표시 항목 목록 읽기
			while (xtr.ReadToFollowing("Index"))
			{

				System.Xml.XmlReader rs = xtr.ReadSubtree();
				try
				{
					indexList.Add(new WizardNodeData(rs));
				}
				catch (Exception ex)
				{
					Trace.WriteLine("Fail to append Index.", "Wizard");
					SEC.GenericSupport.Diagnostics.Helper.ExceptionWriterTrace(ex);
				}
			}
			if (indexList.Count < 1) { WizardLoadFail("No Wizard"); }

			// 파일에서 읽기 종료
			xtr.Close();


			// 순서대로 정렬
			wizardLL = new LinkedList<WizardNodeData>();

			foreach (WizardNodeData wnd in indexList)
			{
				wizardNode = wizardLL.First;

				while (true)
				{
					if (wizardNode == null)
					{
						wizardLL.AddLast(wnd);
						break;
					}
					else if ((wizardNode.Value.Index == -1) || (wizardNode.Value.Index > wnd.Index))
					{
						wizardLL.AddBefore(wizardNode, wnd);
						break;
					}
					else
					{
						wizardNode = wizardNode.Next;
					}
				}

			}

			wizardNode = wizardLL.First;

			WizardLoadSucess(wizardName, languageList);
		}
		#endregion

		#region 생성자
		private WizardViewer()
		{
			InitializeComponent();

			wizardParser = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(WizardParseMethod));

			tableLayoutPanel1.Visible = false;
			waitLab.Text = "Wait for read wizard data.";
			waitLab.Visible = true;
		}
		#endregion

		#region UI 응답
		private void languageCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			wizardNode.Value.Language = languageCb.Text;

			indexLab.Text = wizardNode.Value.Title;
			messageRtb.Text = wizardNode.Value.Message;

			Properties.Settings.Default.Language = languageCb.Text;
		}

		private void nextBut_Click(object sender, EventArgs e)
		{
			wizardNode = wizardNode.Next;
			SelectedNodeChanged();
		}

		private void frontBut_Click(object sender, EventArgs e)
		{
			wizardNode = wizardNode.Previous;
			SelectedNodeChanged();
		}
		#endregion

		#region Wizard 처리
		private void SelectedNodeChanged()
		{
			wizardNode.Value.Language = languageCb.Text;

			indexLab.Text = wizardNode.Value.Title;
			messageRtb.Text = wizardNode.Value.Message;

			imagePb.ImageLocation = wizardNode.Value.ImagePath;

			frontBut.Enabled = (wizardNode.Previous != null);
			nextBut.Enabled = (wizardNode.Next != null);

			CheckCondition(true);
			SystemInfoBinder.Default.MainForm.WizardEmphasis(wizardNode.Value.Emphasis);
		}

		#region Condition
		private void conditionTimer_Tick(object sender, EventArgs e)
		{
			CheckCondition(false);
		}

		/// <summary>
		/// Condition을 check 한다.
		/// </summary>
		/// <param name="log">잘못된 데이터가 있을 경우 log에 기록할 지를 결정 한다.</param>
		private void CheckCondition(bool log)
		{
			string[][] con = wizardNode.Value.Condition;

			// condition 상태.
			// 하나라도 성립 하지 않았다면 true가 되도록 한다.
			bool state = false;

			foreach (string[] conInner in con)
			{
				string condition = conInner[0];
				string name = conInner[1];
				bool type;
				try
				{
					type = bool.Parse(conInner[2]);
				}
				catch (Exception)
				{
					if (log) { Trace.WriteLine("Can't parse type - name : " + name + ", type : " + conInner[1], "Wizard"); }
					continue;
				}
				string target = conInner[3];
				string value = conInner[4];

				switch (condition)
				{
				case "Column":
					state |= !ConditionCheckColumn(name, type, target, value, log);
					break;
				case "UI":
					state |= !ConditionCheckUI(name, type, target, value, log);
					break;
				default:
					if (log) { Trace.WriteLine("Undefined Condition. - " + condition, "Wizard"); }
					continue;
				}
			}

			if (state)
			{
				stateLab.Text = "NO";
				stateLab.ForeColor = Color.Red;
			}
			else
			{
				if (wizardNode.Value.IsConditionPass)
				{
					stateLab.Text = "Pass";
					stateLab.ForeColor = Color.Aqua;
				}
				else
				{
					stateLab.Text = "OK";
					stateLab.ForeColor = Color.SpringGreen;
				}
			}
		}

		private bool ConditionCheckUI(string name, bool type, string target, string value, bool log)
		{
			return SystemInfoBinder.Default.MainForm.WizardConditionCheckUI(name, type, target, value, log);
		}

		/// <summary>
		/// Column의 condition을 check 한다.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="target"></param>
		/// <param name="value"></param>
		/// <param name="log"></param>
		/// <returns>check 결과. condition 정보가 잘못 되었을 경우에도 true를 리턴 한다.</returns>
		private bool ConditionCheckColumn(string name, bool type, string target, string value, bool log)
		{
			SECtype.IValue iVal;

			try
			{
				iVal = SystemInfoBinder.Default.Nanoeye.Controller[name];
			}
			catch (Exception)
			{
				if (log) { Trace.WriteLine("Fail to get IValue. - " + name, "Wizard"); }
				return true; 	// 잘못된 condition은 무시 한다.
			}

			object result;

			switch (target)
			{
			case "Value":
				if (iVal is SECtype.IControlBool)
				{
					SECtype.IControlBool icb = iVal as SECtype.IControlBool;
					result = icb.Value;
				}
				else if (iVal is SECtype.IControlDouble)
				{
					SECtype.IControlDouble icd = iVal as SECtype.IControlDouble;
					result = icd.Value;
				}
				else if (iVal is SECtype.IControlInt)
				{
					SECtype.IControlInt ici = iVal as SECtype.IControlInt;
					result = ici.Value;
				}
				else if (iVal is SECtype.IControlLong)
				{
					SECtype.IControlLong icl = iVal as SECtype.IControlLong;
					result = icl.Value;
				}
				else if (iVal is SECtype.IControlShort)
				{
					SECtype.IControlShort ics = iVal as SECtype.IControlShort;
					result = ics.Value;
				}
				else
				{
					if (log) { Trace.WriteLine("Undefined target type - name : " + name + ", type - " + target, "Wizard"); }
					return true;
				}
				break;
			case "Read":
				if (iVal is SECtype.IControlBool)
				{
					SECtype.IControlBool icb = iVal as SECtype.IControlBool;
					result = icb.Read;
				}
				else if (iVal is SECtype.IControlDouble)
				{
					SECtype.IControlDouble icd = iVal as SECtype.IControlDouble;
					result = icd.Read;
				}
				else if (iVal is SECtype.IControlInt)
				{
					SECtype.IControlInt ici = iVal as SECtype.IControlInt;
					result = ici.Read[0];
				}
				else if (iVal is SECtype.IControlLong)
				{
					SECtype.IControlLong icl = iVal as SECtype.IControlLong;
					result = icl.Read;
				}
				else if (iVal is SECtype.IControlShort)
				{
					SECtype.IControlShort ics = iVal as SECtype.IControlShort;
					result = ics.Read;
				}
				else
				{
					if (log) { Trace.WriteLine("Undefined target type - name : " + name + ", type - " + target, "Wizard"); }
					return true;
				}
				break;
			default:
				if (log) { Trace.WriteLine("Undefined target type - name : " + name + ", type - " + target, "Wizard"); }
				return true;
			}

			return (type == (value == result.ToString()));
		}
		#endregion
		#endregion
	}
}
