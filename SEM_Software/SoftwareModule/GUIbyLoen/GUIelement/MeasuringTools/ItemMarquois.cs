using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SEC.GUIelement.MeasuringTools
{
	/// <summary>
	/// Marquois Scale
	/// 평행선 측정
	/// </summary>
	[Serializable]
	internal class ItemMarquois : ItemBase
	{
		public ItemMarquois()
		{
			this.MaxHandleCount = 4;
		}

		public override string ToString()
		{
			return "MarquoisScale";
		}

		internal override void HandleAdd(Point pnt)
		{
			base.HandleAdd(pnt);

			RestrictedPoint();
		}

		internal override void UpdateHandle(Point location)
		{
			base.UpdateHandle(location);

			RestrictedPoint();
		}

		private void RestrictedPoint()
		{
			bool update = false;

			if (Parent.IsSymetric && (HandleIndex < 2))
			{
				MakeSymetric();
				update = true;
			}

			if (update) { Update(); }

			// 수평 표시 위치는 제한됨.
			if (HandleCount > 2) { Handles[2] = RestrictedParallelPoint(Handles[2]); }

			// 수치 표시 위치는 제한됨.
			if (HandleCount > 3) { Handles[3] = RestrictedNumericalPoint(Handles[3]); }

			if ((HandleCount > 2) || update) { Update(); }
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
		}

		private Point RestrictedNumericalPoint(Point pnt)
		{
			Point pntR = pnt;

			Point ref1 = Handles[0];
			Point ref2 = Handles[1];
			Point ref3 = Handles[2];
			if (ref1.X == ref2.X)
			{
				Point top, bottom;
				if (ref1.Y > ref2.Y)
				{
					top = ref2;
					bottom = ref1;
				}
				else
				{
					top = ref1;
					bottom = ref2;
				}

				if (pntR.Y < top.Y) { pntR.Y = top.Y; }
				if (pntR.Y > bottom.Y) { pntR.Y = bottom.Y; }

				Point left, right;
				if (ref1.X < ref3.X)
				{
					left = ref1;
					right = ref3;
				}
				else
				{
					left = ref3;
					right = ref1;
				}

				if (pntR.X < left.X) { pntR.X = left.X; }
				if (pntR.X > right.X) { pntR.X = right.X; }
			}
			else if (ref1.Y == ref2.Y)
			{
				Point left, right;

				if (ref1.X < ref2.X)
				{
					left = ref1;
					right = ref2;
				}
				else
				{
					left = ref2;
					right = ref1;
				}

				if (pntR.X < left.X) { pntR.X = left.X; }
				if (pntR.X > right.X) { pntR.X = right.X; }

				Point top, bottom;
				if (ref1.Y > ref3.Y)
				{
					top = ref3;
					bottom = ref1;
				}
				else
				{
					top = ref1;
					bottom = ref3;
				}

				if (pntR.Y < top.Y) { pntR.Y = top.Y; }
				if (pntR.Y > bottom.Y) { pntR.Y = bottom.Y; }


			}
			else
			{
				double argOriA, argOriB;

				GenericSupport.Mathematics.Geometry.GetEquation1D(ref1, ref2, out argOriA, out argOriB);

				double argCro1A, argCro1B;
				GenericSupport.Mathematics.Geometry.OrthogonalLineBeteweenLineAndPoint(argOriA, argOriB, ref1, out argCro1A, out argCro1B);

				double argCro2A , argCro2B;
				GenericSupport.Mathematics.Geometry.OrthogonalLineBeteweenLineAndPoint(argOriA, argOriB, ref2, out argCro2A, out argCro2B);

				double dist1 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argCro1A, -1, argCro1B, pntR);
				double dist2 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argCro2A, -1, argCro2B, pntR);
				double distOri = GenericSupport.Mathematics.Geometry.DistanceBetweenPointAndPoint(ref1, ref2);

				// 평행선 허용 범위를 벗어남.
				if (dist1 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro2A, -1, argCro2B, pntR);
					//Debug.WriteLine("dist1 over.");
				}
				else if (dist2 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro1A, -1, argCro1B, pntR);
					//Debug.WriteLine("dist2 over.");
				}

				
				Point pntCor1 = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro1A, -1, argCro1B, ref3);
				Point pntCor2 = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro2A, -1, argCro2B, ref3);
				double argPalA, argPalB;
				GenericSupport.Mathematics.Geometry.GetEquation1D(pntCor1, pntCor2, out argPalA, out argPalB);

				dist1 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argOriA, -1, argOriB, pntR);
				dist2 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argPalA, -1, argPalB, pntR);
				distOri = GenericSupport.Mathematics.Geometry.DistanceBetweenPointAndPoint(ref1, pntCor1);

				if (dist1 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argPalA, -1, argPalB, pntR);
				}
				else if (dist2 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argOriA, -1, argOriB, pntR);
				}
			}

			return pntR;
		}

		private Point RestrictedParallelPoint(Point pntT)
		{
			Point pntR = pntT;

			Point ref1 = Handles[0];
			Point ref2 = Handles[1];

			if (ref1.X == ref2.X)
			{
				Point top, bottom;
				if (ref1.Y > ref2.Y)
				{
					top = ref2;
					bottom = ref1;
				}
				else
				{
					top = ref1;
					bottom = ref2;
				}

				if (pntR.Y < top.Y) { pntR.Y = top.Y; }
				if (pntR.Y > bottom.Y) { pntR.Y = bottom.Y; }
			}
			else if (ref1.Y == ref2.Y)
			{
				Point left, right;

				if (ref1.X < ref2.X)
				{
					left = ref1;
					right = ref2;
				}
				else
				{
					left = ref2;
					right = ref1;
				}

				if (pntR.X < left.X) { pntR.X = left.X; }
				if (pntR.X > right.X) { pntR.X = right.X; }
			}
			else
			{
				double argOriA, argOriB;

				GenericSupport.Mathematics.Geometry.GetEquation1D(ref1, ref2, out argOriA, out argOriB);

				double argCro1A = -1 / argOriA;
				double argCro1B = ref1.Y - ref1.X * argCro1A;

				double argCro2A = -1 / argOriA;
				double argCro2B = ref2.Y - ref2.X * argCro2A;

				double dist1 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argCro1A, -1, argCro1B, pntR);
				double dist2 = GenericSupport.Mathematics.Geometry.DistanceBetweenLineAndPoint(argCro2A, -1, argCro2B, pntR);
				double distOri = GenericSupport.Mathematics.Geometry.DistanceBetweenPointAndPoint(ref1, ref2);

				// 허용 범위를 벗어남.
				if (dist1 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro2A, -1, argCro2B, pntR);
				}
				else if (dist2 > distOri)
				{
					pntR = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCro1A, -1, argCro1B, pntR);
				}
			}
			return pntR;
		}

		protected override void UpdateShapePath(GraphicsPath path, Point[] handles)
		{
			if (handles.Length < 2) { return; }

			path.StartFigure();
			path.AddLine(handles[0], handles[1]);



			// 보조 평행선
			if (handles.Length < 3) { return; }

			Point pnt1;
			Point pnt2;

			path.StartFigure();
			Point pntCross = GetParallelPoint(handles[0], handles[1], handles[2], out pnt1, out pnt2);
			path.AddLine(pnt1, pnt2);

			//path.StartFigure();
			//path.AddEllipse(pntCross.X - 1, pntCross.Y - 1, 3, 3);


			// 수치 표시 선
			if (handles.Length < 4) { return; }
			GetCrossPoint(handles[0], handles[1], handles[2], handles[3], out pnt1, out pnt2);

			path.StartFigure();
			path.AddLine(pnt1, pnt2);
		}

		private void GetCrossPoint(Point ref1, Point ref2, Point paralle, Point cross, out Point pnt1, out Point pnt2)
		{
			pnt1 = Point.Empty;
			pnt2 = Point.Empty;

			Point cro1,cro2;
			GetParallelPoint(ref1, ref2, paralle, out cro1, out cro2);
			
			if (ref1.X == ref2.X)
			{
				pnt1.Y = cross.Y;
				pnt2.Y = cross.Y;

				pnt1.X = ref1.X;
				pnt2.X = cro1.X;
			}
			else if (ref1.Y == ref2.Y)
			{
				pnt1.X = cross.X;
				pnt2.X = cross.X;

				pnt1.Y = ref1.Y;
				pnt2.Y = cro1.Y;
			}
			else
			{

				double argOriA, argOriB;
				GenericSupport.Mathematics.Geometry.GetEquation1D(ref1, ref2, out argOriA, out argOriB);
				double argCroA, argCroB;
				GenericSupport.Mathematics.Geometry.GetEquation1D(cro1, cro2, out argCroA, out argCroB);
				pnt1 = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argOriA, -1, argOriB, cross);
				pnt2 = GenericSupport.Mathematics.Geometry.OrthogonalPointBeteweenLineAndPoint(argCroA, -1, argCroB, cross);
			}
		}

		private Point GetParallelPoint(Point ref1, Point ref2, Point target, out Point pnt1, out Point pnt2)
		{
			pnt1 = Point.Empty;
			pnt2 = Point.Empty;

			double argOriA, argOriB, argCorA, argCorB;
			Point pntCross = Point.Empty;
			if (ref1.X == ref2.X)
			{
				pntCross.X = ref1.X;
				
				//pntCross.Y = (int)Math.Round((ref1.Y + ref2.Y) / 2d);
				pntCross.Y = target.Y;
			}
			else if (ref1.Y == ref2.Y)
			{
				//pntCross.X = (int)Math.Round((ref1.X + ref2.X) / 2d);
				pntCross.X = target.X;
				pntCross.Y = ref1.Y;
			}
			else
			{
				argOriA = (ref1.Y - ref2.Y) / (double)(ref1.X - ref2.X);
				argOriB = ref1.Y - ref1.X * argOriA;

				argCorA = -1 / argOriA;
				argCorB = target.Y - target.X * argCorA;

				double crsX = (argOriB - argCorB) / (argCorA - argOriA);
				pntCross.X = (int)(crsX);
				pntCross.Y = (int)(crsX * argOriA + argOriB);
			}

			pnt1.X = (int)(target.X - (pntCross.X - ref1.X));
			pnt1.Y = (int)(target.Y - (pntCross.Y - ref1.Y));

			pnt2.X = (int)(target.X - (pntCross.X - ref2.X));
			pnt2.Y = (int)(target.Y - (pntCross.Y - ref2.Y));

			return pntCross;
		}

		protected override bool UpdateTextPath(GraphicsPath path, Point[] handles)
		{
            if ((!_DrawText) || (handles.Length != this.MaxHandleCount)) { return false; }

            Font font = this.Parent.Font;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Point pnt1, pnt2;
            GetCrossPoint(handles[0], handles[1], handles[2], handles[3], out pnt1, out pnt2);

            // 두점의 거리를 계산합니다.
            double length = GetLinearLength(pnt1, pnt2);
            // 시작점의 X축에서 끝점까지 시계방향의 각도를 구합니다.
            float angle = GetAngleByPoint(pnt1, pnt2);
            //angle = angle % 90;	// 상하 반전이 되지 않도록 함.
            angle += 90;
            angle = angle % 180;
            angle -= 90;

            // 문자열이 표시될 중심점을 계산합니다.
            PointF point = new PointF((pnt1.X + pnt2.X) / 2, (pnt1.Y + pnt2.Y) / 2);
            // 표시될 측정 문자열을 구성합니다.
            length *= this.Parent.PixelLength;
            //string text = SEC.GenericSupport.Mathematics.NumberConverter.ToUnitString(length, 0, 3, false, 'm');

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
	}
}
