using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;

namespace SEC.GUIelement.MeasuringTools
{
    [Serializable]
    class ItemPoint : ItemBase
    {
        public ItemPoint() : this(true)
		{
		}

        public ItemPoint(bool text)
		{
			this.MaxHandleCount = 2;
			_DrawText = text;
		}

        public override string ToString()
        {
            if (_DrawText)
            {
                return "Point to Point";
            }
            else
            {
                return "Point";
            }
        }

        internal override void HandleAdd(Point pnt)
        {

            base.HandleAdd(pnt);

            //Parent.IsSymetric = true;
            if (Parent.IsSymetric && (HandleCount == 2)) { MakeSymetric(); }
        }

       

        internal override void UpdateHandle(Point location)
        {

            base.UpdateHandle(location);

           
            if (Parent.IsSymetric && (HandleCount == 2))
            {
               
                MakeSymetric(); 
            }
        }

        private void MakeSymetric()
        {
            int index = Math.Abs(HandleIndex - 1);

            Point pnt = Handles[HandleIndex];
            if (Math.Abs(Handles[index].X - pnt.X) > Math.Abs(Handles[index].Y - pnt.Y))
            {
                pnt.Y = Handles[index].Y;
            }
            else
            {
                pnt.X = Handles[index].X;
            }
            Handles[HandleIndex] = pnt;

            Update();
        }

        protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
        {
            
            if (handles.Length >= 2)
            {
                path.StartFigure();
                path.AddLines(handles);
            }
        }

       
        private Point location = new Point(0, 0);

        protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
        {
            if (!_DrawText) { return false; }

            if (handles.Length == this.MaxHandleCount)
            {
                // Text 정보를 초기화 합니다.
                Font font = this.Parent.Font;

                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                // 두점의 거리를 계산합니다.
                double length = GetLinearLength(handles[0], handles[1]);
                // 시작점의 X축에서 끝점까지 시계방향의 각도를 구합니다.
                float angle = GetAngleByPoint(handles[0], handles[1]);
                //angle = angle % 90;	// 상하 반전이 되지 않도록 함.
                angle += 90;
                angle = angle % 180;
                angle -= 90;

                // 문자열이 표시될 중심점을 계산합니다.
                PointF point = new PointF((handles[0].X + handles[1].X) / 2, (handles[0].Y + handles[1].Y) / 2);
                // 표시될 측정 문자열을 구성합니다.
                length *= this.Parent.PixelLength;

                string text;

                if (!this.Parent.textEnable)
                {
                    text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(length, 0, 3, false, 'm');
                    this.ItemText = text;
                }
                else
                {
                    text = this.ItemText;
                }

                GraphicsPath textPath;
                textPath = new GraphicsPath();
                textPath.AddString(text, this.Parent.Font.FontFamily, (int)FontStyle.Regular, font.Size, point, format);
                //textPath.AddRectangle(RectangleF.Inflate(textPath.GetBounds(), 2, 2));

                Matrix m = new Matrix();
                m.RotateAt(angle, point);
                m.Translate(0, font.Size);

                textPath.Transform(m);
                //textPath.Flatten(null, 0.5F); ;
                path.AddPath(textPath, false);

                return true;
            }

            return false;
               
         
        }
    }

}
