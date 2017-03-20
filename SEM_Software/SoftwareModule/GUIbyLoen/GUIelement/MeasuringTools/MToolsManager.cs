using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SEC.GUIelement.MeasuringTools
{
    public class MToolsManager : IEnumerable<ItemBase>
    {
        public static implicit operator object[](MToolsManager manager)
        {
            return manager.itemCollection.ToArray();
        }

        #region Event
        public event EventHandler ItemListChanged;
        protected virtual void OnItemListChanged()
        {
            if (ItemListChanged != null)
            {
                ItemListChanged(this, EventArgs.Empty);
            }
        }

        internal event EventHandler OriginChanged;
        protected virtual void OnOriginChanged()
        {
            if (OriginChanged != null)
            {
                OriginChanged(this, EventArgs.Empty);
            }
        }

        internal event EventHandler ZoomChanged;
        protected virtual void OnZoomChanged()
        {
            if (ZoomChanged != null)
            {
                ZoomChanged(this, EventArgs.Empty);
            }
        }

        internal event EventHandler FontChanged;
        protected virtual void OnFontChanged()
        {
            if (FontChanged != null)
            {
                FontChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;
        protected virtual void OnSeletedItemChanged()
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ItemPropertyChanged;
        protected virtual void OnItemPropertyChanged()
        {
            if (ItemPropertyChanged != null)
            {
                ItemPropertyChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Variables & Propertes

        private bool _Visiable = true;
        public bool Visiable
        {
            get { return _Visiable; }
            set { _Visiable = value; }

        }

        Control _Canvas = null;
        /// <summary>
        /// MTools를 표시할 control
        /// </summary>
        public Control Canvas
        {
            get { return _Canvas; }
            set { _Canvas = value; }
        }

        private bool _IsSymetric = false;
        public bool IsSymetric
        {
            get { return _IsSymetric; }
            set { _IsSymetric = value; }
        }

        Point _Origin = new Point(0, 0);
        /// <summary>
        /// 표시 원점. 
        /// </summary>
        public Point Origin
        {
            get { return _Origin; }
            set
            {
                _Origin = value;
                OnOriginChanged();
            }
        }

        double _Zoom = 1d;
        /// <summary>
        /// 줌. digital zoom
        /// </summary>
        public double Zoom
        {
            get { return _Zoom; }
            set
            {
                _Zoom = value;
                OnZoomChanged();
            }
        }

        private Font _Font = new Font("Arial", 30);
        public Font Font
        {
            get { return _Font; }
            set
            {
                _Font = value;
                OnFontChanged();
            }
        }

        private Color _Color = Color.Black;
        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        /// <summary>
        /// item 모음.
        /// </summary>
        private ItemCollection itemCollection;
        public ItemBase this[int index]
        {
            get { return itemCollection[index]; }
        }

        public int ItemCount
        {
            get { return itemCollection.Count; }
        }

        /// <summary>
        /// 현재 동작 상태
        /// </summary>
        private ItemState itemState = ItemState.Default;

        /// <summary>
        /// 선택된 아이템
        /// </summary>
        private ItemBase _IteamActive = null;
        /// <summary>
        /// 활성 상태 아이템입니다.
        /// </summary>
        protected ItemBase ItemActive
        {
            get { return _IteamActive; }
            set
            {
                _IteamActive = value;
                //if ( _IteamActive == null ) { Debug.WriteLine("Null"); }
                //else { Debug.WriteLine(_IteamActive.ToString()); }
                OnSeletedItemChanged();
            }
        }

        /// <summary>
        /// 픽셀당 길이
        /// </summary>
        private float _PixelLength = 1;
        public float PixelLength
        {
            get { return _PixelLength; }
            set { _PixelLength = value; }
        }

        private int m_AnglePlaces = 0;
        public int AnglePlaces
        {
            get { return m_AnglePlaces; }
            set { m_AnglePlaces = value; }
        }

        internal Color ItemColor
        {
            get
            {
                //Color c = m_Colors[m_ColorIndex];
                //m_ColorIndex =
                //    (m_ColorIndex < m_Colors.Length - 1) ? m_ColorIndex + 1 : 0;

                //return c;

                return Color.White;
            }
        }

        private Color[] m_Colors = new Color[]
			{
				Color.FromArgb(0, 0, 127),
				Color.FromArgb(0, 127, 0),
				Color.FromArgb(0, 63, 63),
				Color.FromArgb(127, 0, 0),
				Color.FromArgb(63, 0, 63),
				Color.FromArgb(255,63, 63, 63)
			};
        //private int m_ColorIndex = 0;
        #endregion

        public MToolsManager()
        {
            itemCollection = new ItemCollection(this);
        }

        /// <summary>
        /// item을 그린다.
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            if (!_Visiable) { return; }

            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;

            foreach (ItemBase item in itemCollection)
            {
                item.Draw(g);
            }

            g.SmoothingMode = smoothingMode;
        }

        /// <summary>
        /// 아이템을 삭제 한다.
        /// </summary>
        public void DeleteItem()
        {
            if (ItemActive == null)
                return;

            itemCollection.Remove(ItemActive);
            OnItemListChanged();

            Canvas.Invalidate(ItemActive.Region);
            Canvas.Cursor = Cursors.Default;
            SetItemState(ItemState.Default);
            ItemActive.Dispose();
            ItemActive = null;
        }

        public void DeleteItemAll()
        {
            ItemBase[] ic = new ItemBase[itemCollection.Count];
            itemCollection.CopyTo(ic);
            foreach (ItemBase ib in ic)
            {
                ItemActive = ib;
                DeleteItem();
            }
            ItemActive = null;
        }

        public void SelectNull()
        {
            SetItemState(ItemState.Default);
            ItemActive = null;
            SetDeselectAllItem();
            Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 아이템을 추가합니다.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemStyle style, bool text)
        {
            switch (style)
            {
                case ItemStyle.Line:
                    ItemActive = new ItemLinear(text);
                    break;
                case ItemStyle.Angle:
                    ItemActive = new ItemAngular();
                    break;
                case ItemStyle.ClosePath:
                    ItemActive = new ItemArea(text);
                    break;
                case ItemStyle.OpenPath:
                    ItemActive = new ItemLength(text);
                    break;
                case ItemStyle.Arrow:
                    ItemActive = new ItemArrow();
                    break;
                case ItemStyle.TextBox:
                    ItemActive = new ItemTextbox();
                    ((ItemTextbox)ItemActive).ChangeText();
                    break;
                case ItemStyle.Rectangle:
                    ItemActive = new ItemRectangle(text);
                    break;
                case ItemStyle.Ellipse:
                    ItemActive = new ItemEllipse(text);
                    break;
                case ItemStyle.MarquiosScale:
                    ItemActive = new ItemMarquois();
                    break;
                case ItemStyle.DeleteOne:
                    DeleteItem();
                    return;
                case ItemStyle.DeleteAll:
                    DeleteItemAll();
                    return;
                case ItemStyle.Point:
                    ItemActive = new ItemPoint();
                    break;
                default:
                    throw new ArgumentException("잘못된 아이템입니다.");
            }

            itemCollection.Add(ItemActive);
            OnItemListChanged();
            SetItemState(ItemState.ItemAppend);

            Canvas.Cursor = Cursors.Cross;
        }

        /// <summary>
        /// 아이템을 추가합니다.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemStyle style)
        {
            if (((int)style >= 2) && ((int)style <= 11))
            {
                int val = (int)style;
                bool txt = ((val & 0x01) == 0x01) ? true : false;

                val = val & 0xFE;

                AddItem((ItemStyle)val, txt);
            }
            else
            {
                AddItem(style, true);
            }
        }

        /// <summary>
        /// 아이템 상태를 지정합니다.
        /// </summary>
        /// <param name="state"></param>
        private void SetItemState(ItemState state)
        {
            itemState = state;
        }

        /// <summary>
        /// 아이템을 선택 상태로 지정합니다.
        /// </summary>
        /// <param name="location">아이템을 선택할 위치입니다.</param>
        public void SetSelectItem(ItemBase item)
        {
            if ((ItemActive != null) && (item != ItemActive))
            {
                DeselectItem(ItemActive);
            }

            if (item == null) { return; }

            if (!item.IsSelected)
            {
                item.Visible = false;
                Canvas.Invalidate(item.Region);

                item.IsSelected = true;

                item.Visible = true;
                Canvas.Invalidate(item.Region);
            }

            ItemActive = item;
        }

        public ItemBase GetSelectItem()
        {
            return ItemActive;
        }

        /// <summary>
        /// 아이템을 비선택 상태로 지정합니다.
        /// </summary>
        /// <param name="item"></param>
        private void DeselectItem(ItemBase item)
        {
            if (item.IsSelected)
            {
                item.Visible = false;
                Canvas.Invalidate(item.Region);

                item.IsSelected = false;

                item.Visible = true;
                Canvas.Invalidate(item.Region);
            }
        }

        /// <summary>
        /// 모든 아이템을 선택 상태로 지정합니다.
        /// </summary>
        private void SelectAllItem()
        {
            // 모든 아이템을 선택 합니다.
            foreach (ItemBase item in itemCollection)
            {
                this.SetSelectItem(item);
            }
        }

        /// <summary>
        /// 모든 아이템을 비선택 상태로 지정합니다.
        /// </summary>
        private void SetDeselectAllItem()
        {
            // 모든 아이템을 선택 해제합니다.
            foreach (ItemBase item in itemCollection)
            {
                this.DeselectItem(item);
            }
        }

        /// <summary>
        /// 지정된 위치에 있는 아이템을 검색합니다.
        /// </summary>
        /// <param name="location">검색할 위치입니다.</param>
        /// <returns>검색된 아이템 입니다.</returns>
        private ItemBase SearchItem(Point location)
        {
            foreach (ItemBase item in itemCollection)
            {
                if (item.ContainsPoint(location))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 컬렉션의 마지막 아이템을 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private ItemBase LastItem()
        {
            if (itemCollection.Count == 0)
            {
                throw new IndexOutOfRangeException("컬렉션에 아이템이 없습니다.");
            }

            return itemCollection[itemCollection.Count - 1];
        }

        private Point poLocation = new Point(0, 0);
        //int count = 0;

        #region Mouse
        public void MouseDown(Point location, MouseButtons button)
        {
            //location.X = (int)((location.X + _Origin.X) * _Zoom);
            //location.Y = (int)((location.Y + _Origin.Y) * _Zoom);
           
            switch (itemState)
            {
                case ItemState.Default:
                    #region ItemState.Default:
                    switch (button)
                    {
                        case MouseButtons.Left:
                            ItemActive = this.SearchItem(location);
                            if (ItemActive != null)
                            {
                                // 선택이 지정되지 않았으면
                                if (!ItemActive.IsSelected)
                                {
                                    this.SetDeselectAllItem();
                                    this.SetSelectItem(ItemActive);
                                }

                                int index = ItemActive.SearchHandle(location);
                                if (index > -1)
                                {
                                    SetItemState(ItemState.ItemHandleUpdate);
                                    //ItemActive.HandleUpdateStart(location);
                                    ItemActive.HandleIndex = index;
                                    //Debug.Print("m_ActiveItem.SetHandleIndex({0})", index);
                                }
                                else
                                {
                                    SetItemState(ItemState.ItemLocationUpdate);
                                    //ItemActive.LocationOffset = location;
                                    ItemActive.LocationUpdateStart(location);
                                    //Debug.Print("m_ActiveItem.SetMoveOrigin({0})", e.Location);
                                }
                            }
                            else
                            {
                                this.SetDeselectAllItem();
                            }
                            break;
                    }
                    #endregion
                    break;
                case ItemState.ItemAppend:
                    #region ItemState.ItemAppend:
                    switch (button)
                    {
                        case MouseButtons.Left:

                            if (ItemActive.ToString() == "Point to Point" && ItemActive.HandleCount != 2)
                            {
                                location.X = (int)((location.X - _Origin.X) / _Zoom);
                                location.Y = (int)((location.Y - _Origin.Y) / _Zoom);


                                bool symetric = this.IsSymetric;
                                //IsSymetric = false;	// 여기서 false로 하지 않으면 HandleIndex 가 잘못 계산되어오류 발생 함.

                                ItemActive.HandleAdd(location);
                                //ItemActive.HandleAdd(location);

                                ItemActive.HandleIndex--;

                                IsSymetric = symetric;

                                //ItemActive.HandleAdd(location);

                                Canvas.Invalidate(ItemActive.Region);
                                //ItemActive.Handles[ItemActive.Handles.Count - 1] = location;

                                //ItemActive.UpdateHandle(location);
                                //ItemActive.Update();

                                //reg.Union(ItemActive.Region);
                                //Canvas.Invalidate(reg);
                                Canvas.Invalidate(ItemActive.Region);


                                SetDeselectAllItem();
                                SetSelectItem(ItemActive);
                                // 추가 작업을 종료합니다.

                                if (ItemActive.HandleCount == 2)
                                {
                                    SetItemState(ItemState.Default);
                                    Canvas.Cursor = Cursors.Default;
                                }


                                return;
                            }

                            if (ItemActive.HandleCount == ItemActive.MaxHandleCount)
                            {
                                // 아이템을 선택 상태로 지정합니다.
                                SetDeselectAllItem();
                                SetSelectItem(ItemActive);
                                // 추가 작업을 종료합니다.
                                SetItemState(ItemState.Default);
                                Canvas.Cursor = Cursors.Default;
                            }
                            else
                            {


                                location.X = (int)((location.X - _Origin.X) / _Zoom);
                                location.Y = (int)((location.Y - _Origin.Y) / _Zoom);



                                if (ItemActive.HandleCount == 0)
                                {
                                    bool symetric = this.IsSymetric;
                                    IsSymetric = false;	// 여기서 false로 하지 않으면 HandleIndex 가 잘못 계산되어오류 발생 함.

                                    ItemActive.HandleAdd(location);
                                    ItemActive.HandleAdd(location);

                                    ItemActive.HandleIndex--;

                                    IsSymetric = symetric;
                                }
                                else
                                {
                                    ItemActive.HandleAdd(location);
                                }
                            }
                            break;
                        case MouseButtons.Right:
                            if (ItemActive.MaxHandleCount < int.MaxValue)
                            {
                                if (ItemActive.HandleCount > 2)
                                {
                                    // 핸들을 삭제합니다.
                                    Canvas.Invalidate(ItemActive.Region);
                                    ItemActive.HandleRemoveAt(ItemActive.HandleCount - 1);
                                    ItemActive.Update();
                                    Canvas.Invalidate(ItemActive.Region);
                                }
                                else
                                {
                                    // 아이템을 삭제하고 캔버스에서 지웁니다.
                                    itemCollection.Remove(ItemActive);
                                    OnItemListChanged();

                                    Canvas.Invalidate(ItemActive.Region);
                                    Canvas.Cursor = Cursors.Default;
                                    SetItemState(ItemState.Default);
                                }
                            }
                            else
                            {
                                if (ItemActive.HandleCount > 2)
                                {
                                    // 핸들을 삭제합니다.
                                    Canvas.Invalidate(ItemActive.Region);
                                    ItemActive.HandleRemoveAt(ItemActive.HandleCount - 1);
                                    ItemActive.MaxHandleCount = ItemActive.HandleCount;
                                    ItemActive.Update();
                                    // 아이템을 선택 상태로 지정합니다.
                                    SetDeselectAllItem();
                                    SetSelectItem(ItemActive);
                                }
                                else
                                {
                                    // 아이템을 삭제하고 캔버스에서 지웁니다.
                                    itemCollection.Remove(ItemActive);
                                    OnItemListChanged();
                                }
                                Canvas.Invalidate(ItemActive.Region);
                                Canvas.Cursor = Cursors.Default;
                                SetItemState(ItemState.Default);
                            }
                            break;
                    }
                    #endregion
                    break;
            }
        }

        public void MouseUp(Point location, MouseButtons button)
        {
            //location.X = (int)((location.X + _Origin.X) * _Zoom);
            //location.Y = (int)((location.Y + _Origin.Y) * _Zoom);

            switch (itemState)
            {
                case ItemState.Default:
                case ItemState.ItemAppend:
                    break;
                case ItemState.ItemHandleUpdate:
                case ItemState.ItemLocationUpdate:
                    if (button == MouseButtons.Left)
                    {
                        Canvas.Cursor = Cursors.Default;
                        SetItemState(ItemState.Default);
                        break;
                    }
                    break;
            }
        }



        public void MouseMove(Point location, MouseButtons button)
        {

            if (ItemActive != null)
            {
               

                switch (itemState)
                {
                    case ItemState.Default:
                        break;
                    case ItemState.ItemAppend:
                        if (ItemActive.HandleCount > 0)
                        {
                            if (ItemActive.ToString() == "Point to Point")
                            {
                                return;
                            }

                            //Region reg = ItemActive.Region.Clone();

                            // 아이템의 핸들을 업데이트하고 다시 그립니다.

                            Canvas.Invalidate(ItemActive.Region);
                            //ItemActive.Handles[ItemActive.Handles.Count - 1] = location;

                            ItemActive.UpdateHandle(location);
                            //ItemActive.Update();

                            //reg.Union(ItemActive.Region);
                            //Canvas.Invalidate(reg);
                            Canvas.Invalidate(ItemActive.Region);
                        }
                        break;
                    case ItemState.ItemHandleUpdate:
                        if ((button == MouseButtons.Left) && (ItemActive != null))
                        {
                            Canvas.Invalidate(ItemActive.Region);
                            ItemActive.UpdateHandle(location);
                            Canvas.Invalidate(ItemActive.Region);
                        }
                        break;
                    case ItemState.ItemLocationUpdate:
                        if ((button == MouseButtons.Left) && (ItemActive != null))
                        {
                            Region reg = ItemActive.Region.Clone();
                            //	Canvas.Invalidate(ItemActive.Region);
                            ItemActive.UpdateLocation(location);

                            // Union을 이용해 두 region을 통합 하면 잔상이 남음.
                            Canvas.Invalidate(reg);
                            Canvas.Invalidate(ItemActive.Region);
                        }
                        break;
                }
            }
        }
        #endregion

        internal void Validate()
        {
            foreach (ItemBase ib in itemCollection)
            {
                ib.Update();
            }
        }

        public bool textEnable = false;

        #region IEnumerable<ItemBase> 멤버

        public IEnumerator<ItemBase> GetEnumerator()
        {
            return itemCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable 멤버

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)itemCollection).GetEnumerator();
        }

        #endregion
    }
}
