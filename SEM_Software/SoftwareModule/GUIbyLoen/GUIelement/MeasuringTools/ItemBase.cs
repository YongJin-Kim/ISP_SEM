using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SEC.GUIelement.MeasuringTools
{
	[Serializable]
	public abstract class ItemBase : IDisposable
	{
		#region Variables & Property

		[ NonSerialized()]
		private MToolsManager m_Parent = null;

		private bool m_Visible = true;
		private bool m_IsSelected = false;

		private Region m_Region = new Region(Rectangle.Empty);
		private Point m_LocationOffset;

		private int m_MaxHandleCount = int.MaxValue;
		/// <summary>
		/// 가능한 핸들의 최대 수 입니다.
		/// </summary>
		public virtual int MaxHandleCount
		{
			get { return m_MaxHandleCount; }
			set { m_MaxHandleCount = value; }
		}

		private GraphicsPath m_ItemPath = new GraphicsPath();
		/// <summary>
		/// 아이템 경로 입니다.
		/// </summary>
		protected GraphicsPath ItemPath
		{
			get { return m_ItemPath; }
			set { m_ItemPath = value; }
		}

		/// <summary>
		/// 측정 정보를 표시할지 결정한다.
		/// </summary>
		protected bool _DrawText = true;
		public bool DrawText
		{
			get { return _DrawText; }
			set { _DrawText = value; }
		}

		public int HandleCount
		{
			get { return Handles.Count; }
		}

		private List<Point> HandleList = new List<Point>();
		[Browsable(false)]
		/// <summary>
		/// 아이템 핸들 리스트입니다.
		/// </summary>
		public List<Point> Handles
		{
			get { return HandleList; }
			set
			{
				HandleList = value;
				HandleIndex = HandleList.Count - 1;
				this.IsSelected = false;
			}
		}

		private Color m_ItemColor = Color.Black;
		/// <summary>
		/// 아이템의 색입니다.
		/// </summary>
		[Browsable(false)]
		public Color ItemColor
		{
			get { return m_ItemColor; }
			set
			{
				m_ItemColor = value;
				this.Update();
			}
		}

        private string m_ItemText = null;

        public string ItemText
        {
            get { return m_ItemText; }
            set
            {
                m_ItemText = value;
                //this.Update();
            }
        }

        private string m_itemTextR1 = null;
        public string itemTextR1
        {
            get { return m_itemTextR1; }
            set
            {
                m_itemTextR1 = value;
                //this.Update();
            }
        }

        private string m_itemTextR2 = null;
        public string itemTextR2
        {
            get { return m_itemTextR2; }
            set
            {
                m_itemTextR2 = value;
                //this.Update();
            }
        }

        private string m_itemTextArea = null;
        public string itemTextArea
        {
            get { return m_itemTextArea; }
            set
            {
                m_itemTextArea = value;
                //this.Update();
            }
        }

        private string m_itemTextwidh = null;
        public string itemTextwidh
        {
            get { return m_itemTextwidh; }
            set
            {
                m_itemTextwidh = value;
                //this.Update();
            }
        }

        private string m_itemTexthight = null;
        public string itemTexthight
        {
            get { return m_itemTexthight; }
            set
            {
                m_itemTexthight = value;
                //this.Update();
            }
        }

		/// <summary>
		/// 아이템의 소유자 핸들입니다.
		/// </summary>
		[Browsable(false)]
		internal MToolsManager Parent
		{
			get { return m_Parent; }
			set { m_Parent = value; }
		}

		/// <summary>
		/// 아이템 영역입니다.
		/// </summary>
		internal Region Region
		{
			get { return m_Region; }
			set { m_Region = value; }
		}

		/// <summary>
		/// 아이템이 그려지는지 여부를 나타내거나 지정합니다.
		/// </summary>
		[Browsable(false)]
		internal bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}

		/// <summary>
		/// 아이템이 선택되었는지 여부를 나타내거나 지정합니다.
		/// </summary>
		[Browsable(false)]
		internal bool IsSelected
		{
			get { return m_IsSelected; }
			set
			{
				if (m_IsSelected != value)
				{
					m_IsSelected = value;

					this.Update();
				}
			}
		}

		protected int m_HandleIndex = 0;
		/// <summary>
		/// 핸들의 인덱스를 지정합니다.
		/// </summary>
		/// <param name="index">지정할 핸들 인덱스입니다.</param>
		internal int HandleIndex
		{
			get { return m_HandleIndex; }
			set
			{
				if (value >= m_MaxHandleCount)
				{
					throw new IndexOutOfRangeException("핸들 인덱스가 범위를 초과하였습니다.");
				}

				m_HandleIndex = value;
			}

		}

		/// <summary>
		/// 아이템의 이동 시작점을 지정합니다.
		/// </summary>
		/// <param name="location"></param>
		internal Point LocationOffset
		{
			get { return m_LocationOffset; }
			set { m_LocationOffset = value; }
		}

		/// <summary>
		/// 현재 핸들이 마지막인지의 여부를 나타냅니다.
		/// </summary>
		[Browsable(false)]
		internal virtual bool IsLastHandle
		{
			get { return (this.HandleIndex == this.MaxHandleCount - 1); }
		}

		#endregion

		internal virtual void HandleAdd(Point pnt)
		{
			Handles.Add(pnt);
			m_HandleIndex++;
		}

		internal virtual void HandleRemoveAt(int index)
		{
			Handles.RemoveAt(index);

			if (m_HandleIndex > HandleList.Count - 1)
			{
				m_HandleIndex = HandleList.Count - 1;
			}
		}

		#region internal 메서드

		internal Point PointToZoom(Point pnt)
		{
			Point result = new Point();

			result.X = (int)((pnt.X - Parent.Origin.X) / Parent.Zoom);
			result.Y = (int)((pnt.Y - Parent.Origin.Y) / Parent.Zoom);

			return result;
		}

		internal Point ZoomToPoint(Point pnt)
		{
			Point result = new Point();

			result.X = (int)(pnt.X * Parent.Zoom) + Parent.Origin.X;
			result.Y = (int)(pnt.Y * Parent.Zoom) + Parent.Origin.Y;

			return result;

		}

		/// <summary>
		/// 아이템을 지정된 그래픽스 핸들에 그립니다.
		/// </summary>
		/// <param name="g">그려질 그래픽스 핸들입니다.</param>
		internal virtual void Draw(Graphics g)
		{
			if (this.ItemPath != null)
			{
                SolidBrush b;

                //b = new SolidBrush(Color.FromArgb(m_ItemColor.ToArgb() ^ 0x00ffffff));
                //b = new SolidBrush(Color.FromArgb(m_ItemColor.ToArgb() ^ 0x00ffffff));
                //b = new SolidBrush(Color.Black);
                b = new SolidBrush(m_ItemColor);


                g.FillRegion(b, this.Region);
                b.Dispose();

                b = new SolidBrush(m_ItemColor);
                g.FillPath(b, this.ItemPath);
                b.Dispose();

                

			}
		}

		/// <summary>
		/// 지정된 위치를 이 아이템이 포함 하는지 여부를 반환합니다.
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		internal virtual bool ContainsPoint(Point location)
		{
			return (this.Region.IsVisible(location) || this.SearchHandle(location) > -1);
		}

       
        
		/// <summary>
		/// 현재 핸들을 지정된 좌표로 업데이트 합니다.
		/// </summary>
		/// <param name="p">현재 핸들을 업데이트 할 좌표입니다.</param>
		internal virtual void UpdateHandle(Point location)
		{
			//Debug.Print("Item.UpdateHandle({0})", location);

			location.X = (int)((location.X - Parent.Origin.X) / Parent.Zoom);
			location.Y = (int)((location.Y - Parent.Origin.Y) / Parent.Zoom);


			//if (m_Parent.Canvas.ClientRectangle.Contains(location))
			//{
				if (HandleList.Count == m_HandleIndex)
				{
					HandleList.Add(location);
				}
				else
				{
					HandleList[m_HandleIndex] = location;
				}


                Update();
                
                
            //}
		}



		/// <summary>
		/// 지정된 위치로 아이템을 이동합니다.
		/// </summary>
		/// <param name="location"></param>
		internal void UpdateLocation(Point location)
		{
			location.X = (int)((location.X + Parent.Origin.X) / Parent.Zoom);
			location.Y = (int)((location.Y + Parent.Origin.Y) / Parent.Zoom);


			//if (this.Parent.Canvas.ClientRectangle.Contains(location))
			{
				int dx = location.X - m_LocationOffset.X;
				int dy = location.Y - m_LocationOffset.Y;

				m_LocationOffset = location;

				for (int i = 0; i < HandleList.Count; i++)
				{
					Point p = HandleList[i];
					p.Offset(dx, dy);
					HandleList[i] = p;
				}

				this.Update();
			}
		}

		///// <summary>
		///// 핸들리스트 끝에 새 핸들을 추가합니다.
		///// </summary>
		//internal void AddHandle()
		//{
		//    if (m_Handles.Count == m_MaxHandleCount)
		//    {
		//        throw new InvalidOperationException("핸들을 추가할 수 없습니다.");
		//    }

		//    m_Handles.Add(Point.Empty);
		//    m_HandleIndex = m_Handles.Count - 1;

		//    //Debug.Print("Item.AddHandle()");
		//}

		/// <summary>
		/// 핸들리스트 끝에서 핸들을 제거합니다.
		/// </summary>
		internal void RemoveHandle()
		{
			if (HandleList.Count == 1)
			{
				throw new InvalidOperationException("핸들을 제거할 수 없습니다.");
			}

			HandleList.RemoveAt(HandleList.Count - 1);
			m_HandleIndex = HandleList.Count - 1;
		}

		/// <summary>
		/// 이 아이템의 지정된 위치에 있는 핸들을 검색합니다.
		/// </summary>
		/// <param name="location">검색할 위치입니다.</param>
		/// <returns>핸들의 인덱스입니다.</returns>
		internal int SearchHandle(Point location)
		{
			location.X = (int)((location.X - Parent.Origin.X) / Parent.Zoom);
			location.Y = (int)((location.Y - Parent.Origin.Y) / Parent.Zoom);

			int index = 0;

			foreach (Point p in HandleList)
			{
				Rectangle rect = new Rectangle(p.X - 8, p.Y - 8, 16, 16);
				if (rect.Contains(location))
				{
					return index;
				}

				index++;
			}

			return -1;
		}

		#endregion

		#region protected 메서드

		protected ItemBase()
		{
			Debug.WriteLine(this.ToString(), "[Created]");
		}

		/// <summary>
		/// 아이템의 Shape를 그립니다.
		/// </summary>
		/// <param name="path">그려질 경로입니다.</param>
		/// <param name="handles">핸들 목록입니다.</param>
		protected abstract void UpdateShapePath(GraphicsPath path, Point[] handles);

		/// <summary>
		/// 아이템의 Text를 그립니다.
		/// </summary>
		/// <param name="path">그려질 경로입니다.</param>
		/// <param name="handles">핸들 목록입니다.</param>
		protected abstract bool UpdateTextPath(GraphicsPath path, Point[] handles);

		/// <summary>
		/// pBase를 기준으로 X축에서 pRotate까지 시계방향으로 측정된 각도를 반환합니다.
		/// </summary>
		/// <param name="p1">시작점입니다.</param>
		/// <param name="p2">끝점입니다.</param>
		/// <returns>측정된 각도(degree)입니다.</returns>
		protected static float GetAngleByPoint(PointF pBase, PointF pRotate)
		{
			// r == 90, 270
			if (pBase.X == pRotate.X)
			{
				return (pBase.Y < pRotate.Y) ? 90F : 270F;
			}
			// r == 0, 180
			else if (pBase.Y == pRotate.Y)
			{
				return (pBase.X < pRotate.X) ? 0F : 180F;
			}
			else
			{
				float dx = pRotate.X - pBase.X;
				float dy = pRotate.Y - pBase.Y;
				float r = (float)Math.Sqrt(dx * dx + dy * dy);

				// 0 < r < 90 or 90 < r < 180
				if (pBase.Y < pRotate.Y)
				{
					return (float)(Math.Acos(dx / r) * 180 / Math.PI);
				}
				// 180 < r < 270 or 270 < r < 360
				else
				{
					return (float)(360F - Math.Acos(dx / r) * 180 / Math.PI);
				}
			}
		}

		/// <summary>
		/// pBase를 기준으로 X축에서 지정된 각도로 pRotate를 시계방향으로 회전한 위치를 반환합니다.
		/// </summary>
		/// <param name="pBase"></param>
		/// <param name="pRotate"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		protected static PointF GetPointByAngle(Point pBase, PointF pRotate, float angle)
		{
			double radian = angle * Math.PI / 180.0;
			float sin = (float)Math.Sin(radian);
			float cos = (float)Math.Cos(radian);

			PointF p = new PointF(
				cos * (pRotate.X - pBase.X) + sin * (pRotate.Y - pBase.Y) + pBase.X,
				sin * (pRotate.X - pBase.X) - cos * (pRotate.Y - pBase.Y) + pBase.Y);

			return p;
		}

       

		/// <summary>
		/// 아이템의 전체 경로를 업데이트 합니다.
		/// </summary>
		/// <param name="selected">Selection의 경로를 포함하는지의 여부입니다.</param>
		/// <returns>업데이트된 Region입니다.</returns>
		internal virtual void Update()
		{
			//Debug.Print("Item.Update()");
			Pen p;
			//Point[] handles = m_Handles.ToArray();

			Point[] handles = HandleAlign();


			m_ItemPath = new GraphicsPath();
			UpdateShapePath(m_ItemPath, handles);
			m_ItemPath.StartFigure();
			if (this._DrawText)
			{
				UpdateTextPath(m_ItemPath, handles);
				m_ItemPath.StartFigure();
			}

			UpdateHandlePath(m_ItemPath, handles);
			m_ItemPath.StartFigure();

            p = new Pen(Color.Empty, 1);
            p.LineJoin = LineJoin.Round;
            m_ItemPath.Widen(p);
            p.Dispose();




            GraphicsPath path = (GraphicsPath)m_ItemPath.Clone();

            p = new Pen(Color.Empty, 2.5F);
            p.LineJoin = LineJoin.Round;
            path.Widen(p);
            p.Dispose();
		

			this.Region = new Region(path);

		}

		protected Point[] HandleAlign()
		{
			Point[] handles = new Point[HandleList.Count];
			int i = 0;
			foreach (Point pnt in HandleList)
			{
				handles[i].X = (int)((pnt.X) * Parent.Zoom + Parent.Origin.X);
				handles[i].Y = (int)((pnt.Y) * Parent.Zoom + Parent.Origin.Y);
				i++;
			}

			return handles;
		}

		/// <summary>
		/// 아이템 경로를 생성합니다.
		/// </summary>
		/// <param name="handles"></param>
		/// <returns></returns>
		internal virtual void UpdateHandlePath(GraphicsPath path, Point[] handles)
		{
			if (handles == null)
			{
				throw new ArgumentNullException("handles는 null일 수 없습니다.");
			}

			if (handles.Length == 0)
			{
				//throw new InvalidOperationException("요소의 수는 0보다 커야 합니다.");
				return;
			}

			if (this.IsSelected)
			{
				foreach (Point pt in handles)
				{
					path.AddRectangle(new RectangleF(pt.X - 4, pt.Y - 4, 8, 8));
					path.AddRectangle(new RectangleF(pt.X - 3, pt.Y - 3, 6, 6));
					//path.AddRectangle(new RectangleF(pt.X - 2, pt.Y - 2, 4, 4));
					//path.AddRectangle(new RectangleF(pt.X - 1, pt.Y - 1, 2, 2));
				}
			}
			else
			{
				foreach (Point pt in handles)
				{
					path.AddLine(pt.X - 4, pt.Y, pt.X + 4, pt.Y);
					path.StartFigure();
					path.AddLine(pt.X, pt.Y - 4, pt.X, pt.Y + 4);
					path.StartFigure();
				}
			}
		}

		protected float GetLinearLength(PointF p1, PointF p2)
		{
			float dx = p1.X - p2.X;
			float dy = p1.Y - p2.Y;

			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		#endregion

		#region IDisposable 멤버

		public virtual void Dispose()
		{
			HandleList.Clear();
			if (m_ItemPath != null) { m_ItemPath.Dispose(); }
			if (m_Region != null) { m_Region.Dispose(); }
			Debug.WriteLine(this.ToString(), "[Disposed]");
		}

		#endregion

		internal void LocationUpdateStart(Point location)
		{
			location.X = (int)((location.X + Parent.Origin.X) / Parent.Zoom);
			location.Y = (int)((location.Y + Parent.Origin.Y) / Parent.Zoom);

			LocationOffset = location;
		}
	}
}
