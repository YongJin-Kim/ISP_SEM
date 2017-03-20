using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEC.GUIelement.CircleButton
{
	public partial class CircleButtonBase : Control
	{
		public CircleButtonBase()
		{
			InitializeComponent();

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		#region 버튼 값
		private float _ValueInnerSide = 0.0f;
		[DefaultValue(0.0f)]
		public float ValueInnerSide
		{
			get { return _ValueInnerSide; }
			set
			{
				_ValueInnerSide = value;
				Refresh();
			}
		}

		private float _ValueInnerUpBottom = 0.0f;
		[DefaultValue(0.0f)]
		public float ValueInnerUpBottom
		{
			get { return _ValueInnerUpBottom; }
			set
			{
				_ValueInnerUpBottom = value; 
				Refresh();
			}
		}


		private float _ValueOutterSide = 0.0f;
		[DefaultValue(0.0f)]
		public float ValueOutterSide
		{
			get { return _ValueOutterSide; }
			set
			{
				_ValueOutterSide = value;
				Refresh();
			}
		}
		
		private float _ValueOutterUpBottom = 0.0f;
		[DefaultValue(0.0f)]
		public float ValueOutterUpBottom
		{
			get { return _ValueOutterUpBottom; }
			set
			{
				_ValueOutterUpBottom = value;
				Refresh();
			}
		}
		#endregion

		#region 버튼이 보여지는지 결정

		private bool _VisiableLeft = true;
		protected bool VisiableLeft {
			get { return _VisiableLeft; }
			set { _VisiableLeft = value; }
		}

		private bool _VisiableRight = true;
		protected bool VisiableRight
		{
			get { return _VisiableRight; }
			set { _VisiableRight = value; }
		}

		private bool _VisiableTop = true;
		protected bool VisiableTop
		{
			get { return _VisiableTop; }
			set { _VisiableTop = value; }
		}

		private bool _VisiableBottom = true;
		protected bool VisiableBottom
		{
			get { return _VisiableBottom; }
			set { _VisiableBottom = value; }
		}

		#endregion

		#region Button Color
		private Color circleColor = Color.Yellow;
		[DefaultValue(typeof(Color),"Yellow")]
		public Color CircleColor
		{
			get { return circleColor; }
			set
			{
				circleColor = value;
				this.Refresh();
			}
		}

		private Color innerColor = Color.LightGreen;
		[DefaultValue(typeof(Color),"LightGreen")]
		public Color InnerColor
		{
			get { return innerColor; }
			set
			{
				innerColor = value;
				this.Refresh();
			}
		}

		private Color outterColor = Color.LawnGreen;
		[DefaultValue(typeof(Color), "LawnGreen")]
		public Color OutterColor
		{
			get { return outterColor; }
			set
			{
				outterColor = value;
				this.Refresh();
			}
		}
		#endregion

		bool areaPaint = false;

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			Graphics g = pe.Graphics;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			if ( !areaPaint || lBSF[0].update ) {
				PaintButton(g, lBSF[0], 0.0f);	// Circle
			}

			if ( _VisiableBottom ) {
				if ( !areaPaint || lBSF[4].update ) {
					PaintButton(g, lBSF[4], ((_ValueInnerUpBottom > 0) ? 0 : _ValueInnerUpBottom * -1));	// Inner Bottom
				}
				if ( !areaPaint || lBSF[8].update ) {
					PaintButton(g, lBSF[8], ((_ValueOutterUpBottom > 0) ? 0 : _ValueOutterUpBottom * -1));	// Inner Bottom
				}
			}
			if ( _VisiableLeft ) {
				if ( !areaPaint || lBSF[1].update ) {
					PaintButton(g, lBSF[1], ((_ValueInnerSide > 0) ? 0 : _ValueInnerSide * -1));	// Inner Left
				}
				if ( !areaPaint || lBSF[5].update ) {
					PaintButton(g, lBSF[5], ((_ValueOutterSide > 0) ? 0 : _ValueOutterSide * -1));	// Inner Left
				}
			}
			if ( _VisiableRight ) {
				if ( !areaPaint || lBSF[3].update ) {
					PaintButton(g, lBSF[3], ((_ValueInnerSide > 0) ? _ValueInnerSide : 0));	// Inner Right
				}
				if ( !areaPaint || lBSF[7].update ) {
					PaintButton(g, lBSF[7], ((_ValueOutterSide > 0) ? _ValueOutterSide : 0));	// Inner Right
				}
			}
			if ( _VisiableTop ) {
				if ( !areaPaint || lBSF[2].update ) {
					PaintButton(g, lBSF[2], ((_ValueInnerUpBottom > 0) ? _ValueInnerUpBottom : 0));	// Inner Top
				}
				if ( !areaPaint || lBSF[6].update ) {
					PaintButton(g, lBSF[6], ((_ValueOutterUpBottom > 0) ? _ValueOutterUpBottom : 0));	// Inner Top
				}
			}
			areaPaint = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bsf"></param>
		/// <param name="value">0~100</param>
		private void PaintButton(Graphics g, ButtonSurfaceDefine bsf, float value)
		{
			float focusScale = value/100;

			bsf.update = false;

			Color paintCol;
			Rectangle rectInner, rectOutter;
			switch ( bsf.ButtonNumber ) {
			case 0:
				focusScale = 0.1f;
				paintCol = circleColor;
				rectInner = circleStartRect;
				rectOutter = circleEndRect;
				break;
			case 1:
				//if ( focusScale != 0 ) {
					focusScale += 0.3f;
				//}
				paintCol = innerColor;
				rectInner = innerStartRect;
				rectOutter = innerEndRect;
				break;
			case 2:
				//if ( focusScale != 0 ) {
					focusScale = focusScale * 0.7f + 0.6f;
				//}
				paintCol = outterColor;
				rectInner = outterStartRect;
				rectOutter = outterEndRect;
				break;
			default:
				throw new Exception();
			}

			Color valColor = Color.Blue;
			switch ( bsf.cs ) {
			case CheckState.Indeterminate:
				paintCol = ControlPaint.Dark(paintCol, 0.1F);
				valColor = ControlPaint.Dark(valColor, 0.1F);
				break;
			case CheckState.Checked:
				paintCol = ControlPaint.Light(paintCol, 0.9F);
				valColor = ControlPaint.Light(valColor, 0.9F);
				break;
			}

			PathGradientBrush pgb;
			pgb = new PathGradientBrush(bsf.gp);
			//pgb.FocusScales = new PointF(0.9F, 0.9F);
			pgb.FocusScales = new PointF(focusScale, focusScale);
			//RectangleF rf = bsf.re.GetBounds(g);
			//pgb.CenterPoint = new PointF(rf.Right, rf.Top + rf.Height/2);
			pgb.CenterPoint = new PointF(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
			pgb.CenterColor = paintCol;// ControlPaint.Light(this.BackColor);
			pgb.SurroundColors = new Color[] { ControlPaint.Dark(paintCol, 0.3F) };
			g.FillRegion(pgb, bsf.re);

			//if ( value == 0 ) { return; }

			//float length = (rectInner.X - rectOutter.X) * value / 100.0f;

			//Pen p = new Pen(valColor);

			//for ( float i =0.0f ; i < length ; i+=0.25f ) {
			//    RectangleF rect = new RectangleF((int)(rectInner.X - i), (int)(rectInner.Top - i), (int)(rectInner.Width + i * 2), (int)(rectInner.Height + i * 2));
			//    g.DrawArc(p, rect, bsf.angleStart, bsf.angleSweep);
			//}

		}

		class ButtonSurfaceDefine
		{
			public GraphicsPath gp;
			public Region re;
			public int ButtonNumber;
			public CheckState cs;
			public float angleStart;
			public float angleSweep;
			public ButtonLocation location;
			public bool update;
		}

		/// <summary>
		/// 버튼 표면의 정의들
		/// </summary>
		List<ButtonSurfaceDefine> lBSF = new List<ButtonSurfaceDefine>();

		/*
		 * 좌우 또는 위아래 버튼이 사용할 각도 및 버튼 사이 각
		 */
		private float angleSide = 90.0f;
		private float angleUpBottom = 90.0f;
		private float angleGap = 6.0f;
		public float AngleGap
		{
			get { return angleGap; }
			set { angleGap = value; }
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			lBSF.Clear();

			// 이 컨트롤의 영역을 잡는다.
			//GraphicsPath gp = new GraphicsPath();
			//gp.AddEllipse(this.DisplayRectangle);
			//this.Region = new Region(gp);

			angleSide = (((float)(this.Height)) * 90.0f / ((float)(this.Width))) - angleGap;
			angleUpBottom = (((float)(this.Width)) * 90.0f / ((float)(this.Height))) - angleGap;

			CircleDefine();

			InnerDefine();

			OutterDefine();

			System.Drawing.Region reg = new Region();
			reg.MakeEmpty();
			foreach ( ButtonSurfaceDefine bsd in lBSF ) {
				if ( bsd.re != null ) {
					reg.Union(bsd.re);
				}
			}
			this.Region = reg;

			Invalidate();
		}

		#region Button Area Define

		Rectangle circleStartRect;
		Rectangle circleEndRect;

		Rectangle innerStartRect;
		Rectangle innerEndRect;

		Rectangle outterStartRect;
		Rectangle outterEndRect;

		private void ButtonRegion(Rectangle rect, GraphicsPath innerEllipse, int colorNum)
		{
			GraphicsPath gpb;
			Region re;
			ButtonSurfaceDefine bs;

			if ( _VisiableLeft ) {
				//		- 왼쪽



				gpb = new GraphicsPath();
				gpb.AddPie(rect, 180.0f - angleSide / 2, angleSide);
				re = new Region(gpb);
				re.Exclude(innerEllipse);	// 내부 원 제거

				bs = new ButtonSurfaceDefine();
				bs.gp = gpb;
				bs.re = re;


				bs.ButtonNumber = colorNum;

				switch ( colorNum ) {
				case 1:
					bs.location = ButtonLocation.InnerLeft;
					break;
				case 2:
					bs.location = ButtonLocation.OutterLeft;
					break;
				default:
					throw new Exception();
				}

				bs.angleStart = 180.0f - angleSide / 2;
				bs.angleSweep = angleSide;

				bs.update = true;

				lBSF.Add(bs);
			}
			else {
				bs = new ButtonSurfaceDefine();
				bs.update = false;	
				lBSF.Add(bs);
			}

			if ( _VisiableTop ) {
				//		-  위
				gpb = new GraphicsPath();
				gpb.AddPie(rect, 270.0f - angleUpBottom / 2, angleUpBottom);
				re = new Region(gpb);
				re.Exclude(innerEllipse);	// 내부 원 제거

				bs = new ButtonSurfaceDefine();
				bs.gp = gpb;
				bs.re = re;
				bs.ButtonNumber = colorNum;

				switch ( colorNum ) {
				case 1:
					bs.location = ButtonLocation.InnerTop;
					break;
				case 2:
					bs.location = ButtonLocation.OutterTop;
					break;
				default:
					throw new Exception();
				}

				bs.angleStart = 270.0f - angleUpBottom / 2;
				bs.angleSweep = angleUpBottom;
				bs.update = true;

				lBSF.Add(bs);
			}
			else {
				bs = new ButtonSurfaceDefine();
				bs.update = false;
				lBSF.Add(bs);
			}

			if ( _VisiableRight ) {
				//		- 오른쪽
				gpb = new GraphicsPath();
				gpb.AddPie(rect, 360.0f - angleSide / 2, angleSide);
				re = new Region(gpb);
				re.Exclude(innerEllipse);	// 내부 원 제거

				bs = new ButtonSurfaceDefine();
				bs.gp = gpb;
				bs.re = re;
				bs.ButtonNumber = colorNum;

				switch ( colorNum ) {
				case 1:
					bs.location = ButtonLocation.InnerRight;
					break;
				case 2:
					bs.location = ButtonLocation.OutterRight;
					break;
				default:
					throw new Exception();
				}

				bs.angleStart = 360.0f - angleSide / 2;
				bs.angleSweep = angleSide;
				bs.update = true;

				lBSF.Add(bs);
			}
			else {
				bs = new ButtonSurfaceDefine();
				bs.update = false;
				lBSF.Add(bs);
			}

			if ( _VisiableBottom ) {
				//		- 아래
				gpb = new GraphicsPath();
				gpb.AddPie(rect, 90.0f - angleUpBottom / 2, angleUpBottom);
				re = new Region(gpb);
				re.Exclude(innerEllipse);	// 내부 원 제거

				bs = new ButtonSurfaceDefine();
				bs.gp = gpb;
				bs.re = re;
				bs.ButtonNumber = colorNum;

				switch ( colorNum ) {
				case 1:
					bs.location = ButtonLocation.InnerBottom;
					break;
				case 2:
					bs.location = ButtonLocation.OutterBottom;
					break;
				default:
					throw new Exception();
				}

				bs.angleStart = 90.0f - angleUpBottom / 2;
				bs.angleSweep = angleUpBottom;
				bs.update = true;

				lBSF.Add(bs);
			}
			else {
				bs = new ButtonSurfaceDefine();
				bs.update = false;
				lBSF.Add(bs);
			}
			return;
		}

		private int buttonGap = 4;
		public int ButtonGap
		{
			get { return buttonGap; }
			set { buttonGap = value; }
		}

		/// <summary>
		/// 바깥쪽 버튼에 대한 정의를 생성한다.
		/// </summary>
		private void OutterDefine()
		{
			outterEndRect = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
			outterStartRect = new Rectangle(Bounds.Width / 5 - buttonGap, Bounds.Height / 5 - buttonGap, Bounds.Width * 3 / 5 + buttonGap * 2, Bounds.Height * 3 / 5 + buttonGap * 2);

			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(outterStartRect);

			ButtonRegion(outterEndRect, gp, 2);
			return ;
		}

		/// <summary>
		/// 안쪽 버튼에 대한 정의를 생성한다
		/// </summary>
		private void InnerDefine()
		{
			innerEndRect = new Rectangle(Bounds.Width / 5, Bounds.Height / 5, Bounds.Width*3 / 5, Bounds.Height*3 / 5);
			innerStartRect = new Rectangle(Bounds.Width * 2 / 5 - buttonGap, Bounds.Height * 2 / 5 - buttonGap, Bounds.Width / 5 + buttonGap * 2, Bounds.Height / 5 + buttonGap * 2);

			// 제거용 원
			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(innerStartRect);

			ButtonRegion(innerEndRect, gp, 1);

			return ;
		}

		/// <summary>
		/// 가운데 타원에 대한 정의
		/// </summary>
		private void CircleDefine()
		{
			// 정중앙 타원의 영역
			circleEndRect = new Rectangle(Bounds.Width * 2 / 5, Bounds.Height * 2 / 5, Bounds.Width / 5, Bounds.Height / 5);
			circleStartRect = new Rectangle();

			GraphicsPath circleGraphicsPath = new GraphicsPath();
			circleGraphicsPath.AddEllipse(circleEndRect);
			Region circleRegion = new Region(circleGraphicsPath);

			ButtonSurfaceDefine bs = new ButtonSurfaceDefine();
			bs.gp = circleGraphicsPath;
			bs.re = circleRegion;
			bs.ButtonNumber = 0;

			bs.location = ButtonLocation.Center;

			bs.update = true;

			lBSF.Add(bs);
			return;
		}
		#endregion

		#region Mouse Event
		public enum ButtonLocation
		{
			InnerLeft,
			InnerRight,
			InnerTop,
			InnerBottom,
			OutterLeft,
			OutterRight,
			OutterTop,
			OutterBottom,
			Center
		}

		protected virtual void OnButtonDown(ButtonLocation bl) { }

		protected virtual void OnButtonUp() { }

		protected virtual void OnButtonClick(ButtonLocation bl) { }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if ( !this.Capture ) {
				foreach ( ButtonSurfaceDefine bsf in lBSF ) {
					if ( bsf.re == null ) {
						continue;
					}


					if ( bsf.re.IsVisible(e.Location) ) {
						if ( bsf.cs == CheckState.Checked ) { return; } // 이미 체크 되어 있음.

						if ( e.Button == MouseButtons.None ) {
							bsf.cs = CheckState.Checked;
						}
						else {
							bsf.cs = CheckState.Indeterminate;
						}
						bsf.update = true;
						areaPaint = true;
						Invalidate(bsf.re);
					}
					else {
						if ( bsf.cs != CheckState.Unchecked ) {
							bsf.cs = CheckState.Unchecked;
							bsf.update = true;
							areaPaint = true;
							Invalidate(bsf.re);
						}
					}
				}
				
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if ( !this.Capture ) {
				foreach ( ButtonSurfaceDefine bsf in lBSF ) {
					if ( bsf.re == null ) {
						continue;
					}
					bsf.cs = CheckState.Unchecked;
				}
			}
			areaPaint = false;
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			ButtonSurfaceDefine bsfTemp = new ButtonSurfaceDefine();

			if ( e.Button == MouseButtons.Left ) {
				foreach ( ButtonSurfaceDefine bsf in lBSF ) {
					if ( bsf.re == null ) {
						continue;
					}

					if ( bsf.re.IsVisible(e.Location) ) {
						bsf.cs = CheckState.Indeterminate;
						OnButtonDown(bsf.location);
						this.Capture = true;

						bsf.update = true;
						areaPaint = true;
						Invalidate(bsf.re);
					}
					else {
						if ( bsf.cs != CheckState.Unchecked ) {
							bsf.cs = CheckState.Unchecked;
							bsf.update = true;
							areaPaint = true;
							Invalidate(bsf.re);
						}
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);


			if ( this.Capture ) {
				this.Capture = false;

				ButtonSurfaceDefine bs =new ButtonSurfaceDefine();
				foreach ( ButtonSurfaceDefine bsf in lBSF ) {

					if ( bsf.cs == CheckState.Indeterminate ) {
						bs = bsf;
						bsf.cs = CheckState.Unchecked;
						bsf.update = true;
						areaPaint = true;
						Invalidate(bsf.re);
					}
				}

				OnButtonUp();
				OnButtonClick(bs.location);
			}

		}
		#endregion
	}
}
