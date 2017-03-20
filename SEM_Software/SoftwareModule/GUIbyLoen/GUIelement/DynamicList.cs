using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace SEC.GUIelement
{
	public partial class DynamicList : UserControl
	{
		#region Property

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ButtonEllipse ButtonRight
		{
			get { return changeRightBut; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ButtonEllipse ButtonLeft
		{
			get { return changeLeftBut; }
		}

		private Orientation _ButtonAlign = Orientation.Vertical;
		public Orientation ButtonAlign
		{
			get { return _ButtonAlign; }
			set
			{
				if (_ButtonAlign != value)
				{
					_ButtonAlign = value;
					OnSizeChanged(EventArgs.Empty);
				}
			}
		}

		public override Color ForeColor
		{
			get { return label1.ForeColor; }
			set { label1.ForeColor = value; }
		}

		public Color LableColor
		{
			get { return label1.BackColor; }
			set { label1.BackColor = value; }
		}

		private ArrayList _Items = new ArrayList();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", typeof(UITypeEditor))
		]
		public virtual ArrayList Items
		{
			get
			{
				if (_Items == null) { _Items = new ArrayList(); }

				return _Items;
			}
		}

		private int _SeletedIndex = -1;
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		public int SelectedIndex
		{
			get { return _SeletedIndex; }
			set
			{
				int temp = value;
				if (temp < 0)
				{
					if (_Rotate) { temp = _Items.Count - 1; }
					else { return; }
				}
				if (value >= _Items.Count)
				{
					if (_Rotate) { temp = 0; }
					else { return; }
				}
				_SeletedIndex = temp;

				changeLeftBut.Enabled = (_SeletedIndex > 0);
				changeRightBut.Enabled = (_SeletedIndex < _Items.Count - 1);

				object item = _Items[_SeletedIndex];

				if (item is Control) { label1.Text = ((Control)item).Text; }
				else { label1.Text = item.ToString(); }

				OnSeletedIndexChanged();
			}
		}

		[DefaultValue(null),
		RefreshProperties(RefreshProperties.All),
		Browsable(false)
		]
		public object SelectedItem
		{
			get
			{
				if (_SeletedIndex == -1)
				{
					return null;
				}
				else
				{
					return _Items[_SeletedIndex];
				}
			}
			set
			{
				int index = _Items.IndexOf(value);
				if (index == -1)
				{
					throw new ArgumentException();
				}
				SelectedIndex = index;
			}
		}

		private bool _Rotate = false;
		/// <summary>
		/// 선택 메뉴가 순차적으로 회전식으로 선택 될지를 결정.
		/// </summary>
		[DefaultValue(false)]
		public bool Rotate
		{
			get { return _Rotate; }
			set { _Rotate = value; }
		}

		#endregion

		#region Event
		/// <summary>
		/// 표시 항목이 바뀜.
		/// </summary>
		public event EventHandler SeletedIndexChanged;

		protected virtual void OnSeletedIndexChanged()
		{
			if (SeletedIndexChanged != null)
			{
				SeletedIndexChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		public DynamicList()
		{
			InitializeComponent();



			cf = new ChildForm();
			cf.VisibleChanged += new EventHandler(cf_VisibleChanged);
		}

		/// <summary>
		/// 메뉴 선택 윈도우가 보여지기 여부가 바뀜.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cf_VisibleChanged(object sender, EventArgs e)
		{
			if (!cf.Visible)
			{
				if (cf.SeletedItem != null)
				{

					// 선택 아이템이 바뀌었을 경우의 동작을 실행하도록 하기 위해
					// private 변수가 아닌 속성에 접근 함.
					SelectedItem = cf.SeletedItem;

					cf.SeletedItem = null;
				}
			}
		}

		#region override
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			ReLayout();
		}

		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			ReLayout();
		}

		private void ReLayout()
		{
			System.Drawing.Size butSize;

			switch (_ButtonAlign)
			{
			case Orientation.Horizontal:
				#region 리스트 검색 버튼 좌우 배치
				butSize = new Size(this.Height - this.Padding.Vertical, this.Height - this.Padding.Vertical);
				changeLeftBut.Size = butSize;
				changeRightBut.Size = butSize;

				changeLeftBut.Location = new Point(this.Padding.Left, this.Padding.Top);
				changeRightBut.Location = new Point(this.ClientRectangle.Width - this.Padding.Right - changeRightBut.Width, this.Padding.Top);

				label1.Size = new Size(this.Width - changeLeftBut.Right - changeRightBut.Width - Padding.Right - Padding.Horizontal, this.Height - Padding.Vertical);
				label1.Location = new Point(changeLeftBut.Width + Padding.Horizontal, Padding.Top);
				#endregion
				break;
			case Orientation.Vertical:
				#region 리스트 검색 버튼 왼쪽 상하 배치
				butSize = new Size((this.Height - Margin.Vertical) / 2, (this.Height - Margin.Vertical) / 2);
				changeLeftBut.Size = butSize;
				changeRightBut.Size = butSize;

				changeRightBut.Location = new Point(0, 0);
				changeLeftBut.Location = new Point(0, changeRightBut.Bottom + Margin.Vertical);

				label1.Size = new Size(this.Width - changeRightBut.Right - this.Margin.Horizontal, this.Height / 2);
				label1.Location = new Point(changeRightBut.Right + this.Margin.Horizontal, this.Height / 4);
				#endregion
				break;
			}
		}


		private bool topLevelLocationLinked = false;
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (this.TopLevelControl != null)
			{
				if (!topLevelLocationLinked)
				{
					this.TopLevelControl.LocationChanged += new EventHandler(TopLevelControl_LocationChanged);
					topLevelLocationLinked = true;
				}
			}
		}

		void TopLevelControl_LocationChanged(object sender, EventArgs e)
		{
			if (cf != null)
			{
				cf.Location = this.PointToScreen(new Point(label1.Left, label1.Bottom));
			}
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			if (cf != null)
			{
				cf.Location = PointToScreen(label1.Location);
			}
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);

			if (_SeletedIndex == -1)
			{
				label1.Text = "";
				return;
			}

			object item = _Items[_SeletedIndex];

			if (item is Control)
			{
				label1.Text = ((Control)item).Text;
			}
			else
			{
				label1.Text = item.ToString();
			}

		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

			base.OnPaint(e);
		}
		#endregion

		ChildForm cf;

		/// <summary>
		/// 내부에 아이템 목록을 보여주는 윈도우
		/// </summary>
		private class ChildForm : Form
		{
			TableLayoutPanel tlp;

			public ChildForm()
			{
				this.StartPosition = FormStartPosition.Manual;
				this.FormBorderStyle = FormBorderStyle.None;
				this.ControlBox = false;
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.MinimumSize = new Size(0, 0);
				this.Text = "";
				this.ShowIcon = false;
				this.ShowInTaskbar = false;
				tlp = new TableLayoutPanel();
				this.Controls.Add(tlp);
				//tlp.BackColor = Color.Red;
			}

			public void SetItems(object[] items)
			{
				tlp.Controls.Clear();

				this.Height = items.Length * 20;
				tlp.Dock = DockStyle.Fill;
				tlp.ColumnCount = 1;
				tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				tlp.RowCount = items.Length;

				int i=0;
				for (i = 0; i < tlp.RowCount; i++)
				{
					tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / tlp.RowCount));
				}
				i = 0;
				foreach (object item in items)
				{
					Button but = new Button();
					tlp.Controls.Add(but, 1, i++);
					if (item is Control)
					{
						but.Text = ((Control)item).Text;
					}
					else
					{
						but.Text = item.ToString();
					}
					but.Tag = item;
					but.Dock = DockStyle.Fill;
					but.Margin = new Padding(0);
					but.Click += new EventHandler(but_Click);
					but.Visible = true;
				}
				tlp.Visible = true;
				this.Invalidate();
			}

			void but_Click(object sender, EventArgs e)
			{
				Button bnt = sender as Button;
				_SeletedItem = bnt.Tag;
				this.Hide();
			}

			private object _SeletedItem = null;
			public object SeletedItem
			{
				get { return _SeletedItem; }
				set { _SeletedItem = value; }
			}

			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				this.Focus();
			}
		}

		protected void label1_MouseClick(object sender, MouseEventArgs e)
		{
			if (cf == null)
			{
				cf = new ChildForm();
				cf.Show(this.Parent);
				cf.Hide();
			}
			if (!cf.Visible)
			{
				cf.Location = this.PointToScreen(new Point(label1.Left, label1.Bottom));
				cf.Width = label1.Width;
				cf.SetItems(_Items.ToArray());
				cf.Show(this.Parent);
				this.Focus();
			}
			else
			{
				cf.Hide();
			}
		}

		#region 좌우 전환 버튼

		void ChangeRight()
		{
			SelectedIndex++;
		}

		private void ChangeLeft()
		{
			SelectedIndex--;
		}

		private void changeRight_Click(object sender, EventArgs e)
		{
			ChangeRight();
		}

		private void chenageLeft_Click(object sender, EventArgs e)
		{
			ChangeLeft();
		}

		private void chenageLeft_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ChangeLeft();
		}

		private void changeRight_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ChangeRight();
		}
		#endregion
	}
}
