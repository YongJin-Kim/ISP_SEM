using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;

namespace SEC.GUIelement
{
	public class LableFixedText : System.Windows.Forms.Label
	{
		private string _TextFixed = "";
		[DefaultValue("")]
		public string TextFixed
		{
			get { return _TextFixed; }
			set
			{
				_TextFixed = value;
				ChangeText();
			}
		}

		private string _TextVariable = "";
		[DefaultValue("")]
		public string TextVariable
		{
			get { return _TextVariable; }
			set
			{
				_TextVariable = value;
				ChangeText();
			}
		}

		private int _TextLength = 5;
		[DefaultValue(5)]
		public int TextLength
		{
			get { return _TextLength; }
			set
			{
				_TextLength = value;
				ChangeText();
			}
		}

		private void ChangeText()
		{
			string str = _TextFixed;
			switch (_StrAlign)
			{
			case HorizontalAlignment.Center:
				str += _TextVariable;
				break;
			case HorizontalAlignment.Left:
				str += _TextVariable.PadRight(_TextLength);
				break;
			case HorizontalAlignment.Right:
				str += _TextVariable.PadLeft(_TextLength);
				break;
			}

			Action act = () => { Text = str; };

			if (InvokeRequired) { this.Invoke(act); }
			else { act(); }
		}


		HorizontalAlignment _StrAlign = HorizontalAlignment.Right;
		[DefaultValue(typeof(HorizontalAlignment), "Right")]
		public HorizontalAlignment StrAlign
		{
			get { return _StrAlign; }
			set
			{
				_StrAlign = value;
				ChangeText();
			}
		}
	}
}
