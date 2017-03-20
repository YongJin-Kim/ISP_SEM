using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SEC.GenericSupport.DataType
{
	public class Transfrom2DDouble : ControlValueBase, ITransform2DDouble
	{
		#region Property & Variables
		bool syncFlag = false;

		protected IControlDouble _HorizontalRotated = null;
		public IControlDouble HorizontalRotated
		{
			get { return _HorizontalRotated; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }

				if (_HorizontalRotated != null) { _HorizontalRotated.ValueChanged -= new EventHandler(Rotated_ValueChanged); }
				_HorizontalRotated = value;
				if (_HorizontalRotated != null) { _HorizontalRotated.ValueChanged += new EventHandler(Rotated_ValueChanged); }
			}
		}

		protected IControlDouble _HorizontalReal = null;
		public IControlDouble HorizontalReal
		{
			get { return _HorizontalReal; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }

				if (_HorizontalReal != null) { _HorizontalReal.ValueChanged -= new EventHandler(Real_ValueChanged); }
				_HorizontalReal = value;
				if (_HorizontalReal != null) { _HorizontalReal.ValueChanged += new EventHandler(Real_ValueChanged); }
			}
		}

		protected IControlDouble _VerticalRotated = null;
		public IControlDouble VerticalRotated
		{
			get { return _VerticalRotated; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }

				if (_VerticalRotated != null) { _VerticalRotated.ValueChanged -= new EventHandler(Rotated_ValueChanged); }
				_VerticalRotated = value;
				if (_VerticalRotated != null) { _VerticalRotated.ValueChanged += new EventHandler(Rotated_ValueChanged); }
			}
		}

		protected IControlDouble _VerticalReal = null;
		public IControlDouble VerticalReal
		{
			get { return _HorizontalReal; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }

				if (_VerticalReal != null) { _VerticalReal.ValueChanged -= new EventHandler(Real_ValueChanged); }
				_VerticalReal = value;
				if (_VerticalReal != null) { _VerticalReal.ValueChanged += new EventHandler(Real_ValueChanged); }
			}
		}

		protected IControlDouble _Angle = null;
		public IControlDouble Angle
		{
			get { return _Angle; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }

				if (_Angle != null) { _Angle.ValueChanged -= new EventHandler(Rotated_ValueChanged); }
				_Angle = value;
				if (_Angle != null) { _Angle.ValueChanged += new EventHandler(Rotated_ValueChanged); }
			}
		}

		protected bool _ReverseHorizontal = false;
		public bool ReverseHorizontal
		{
			get { return _ReverseHorizontal; }
			set
			{
				_ReverseHorizontal = value;
				SetRealValue();
			}
		}

		protected bool _ReverseVertical = false;
		public bool ReverseVertical
		{
			get { return _ReverseVertical; }
			set
			{
				_ReverseVertical = value;
				SetRealValue();
			}
		}

		protected double _PrecisionHorizontal = 1;
		public double PrecisionHorizontal
		{
			get { return _PrecisionHorizontal; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }
				_PrecisionHorizontal = value;
			}
		}

		protected double _PrecisionVertical = 1;
		public double PrecisionVertical
		{
			get { return _PrecisionVertical; }
			set
			{
				if (_IsInited) { throw new InvalidOperationException("This object is already inited."); }
				_PrecisionVertical = value;
			}
		}
		#endregion

		#region 소멸자
		public override void Dispose()
		{
			_IsInited = false;
			_Enable = false;

			HorizontalRotated = null;
			HorizontalReal = null;
			VerticalRotated = null;
			VerticalReal = null;
			Angle = null;

			base.Dispose();
		}
		#endregion

		#region 초기화
		public override void BeginInit()
		{
			_IsInited = false;
		}

		public override void EndInit(bool sync)
		{
			if (HorizontalRotated == null) { throw new InvalidOperationException("HotizontalRotaed is not setted."); }
			if (HorizontalReal == null) { throw new InvalidOperationException("HorizontalReal is not setted."); }
			if (VerticalRotated == null) { throw new InvalidOperationException("VerticalRotated is not setted."); }
			if (VerticalReal == null) { throw new InvalidOperationException("VerticalReal is not setted."); }
			if (Angle == null) { throw new InvalidOperationException("Angle is not setted."); }

			_IsInited = true;
		}
		#endregion

		#region 동기화
		public override void Sync()
		{
			SetRotatedValue();
		}

		public override bool Validate()
		{
			double rotatedX, rotatedY;
			CalculateRotated(out rotatedX, out rotatedY);

			bool result = true;

			if (_HorizontalRotated.Value != rotatedX) { result = false; }
			if (_VerticalRotated.Value != rotatedY) { result = false; }

			return result;
		}
		#endregion

		#region Control 값 바뀜 이벤트 수신
		void Rotated_ValueChanged(object sender, EventArgs e)
		{
			SetRealValue();
		}

		void Real_ValueChanged(object sender, EventArgs e)
		{
			SetRotatedValue();
		}
		#endregion

		#region 값 계산
		private void SetRealValue()
		{
			if (_Enable && _IsInited)
			{
				if (syncFlag) { return; }

				syncFlag = true;
				double realX, realY;
				CalculateReal(out realX, out realY);

				if (realX > _HorizontalReal.Maximum) { realX = _HorizontalReal.Maximum; }
				if (realX < _HorizontalReal.Minimum) { realX = _HorizontalReal.Minimum; }

				if (realY > _VerticalReal.Maximum) { realY = _VerticalReal.Maximum; }
				if (realY < _VerticalReal.Minimum) { realY = _VerticalReal.Minimum; }


				_HorizontalReal.Value = realX;
				_VerticalReal.Value = realY;
				syncFlag = false;
			}
		}

		private void SetRotatedValue()
		{
			if (_Enable && _IsInited)
			{
				if (syncFlag) { return; }

				syncFlag = true;

				double rotatedX, rotatedY;
				CalculateRotated(out rotatedX, out rotatedY);

				if (rotatedX > _HorizontalRotated.Maximum) { rotatedX = _HorizontalRotated.Maximum; }
				if (rotatedX < _HorizontalRotated.Minimum) { rotatedX = _HorizontalRotated.Minimum; }

				if (rotatedY > _VerticalRotated.Maximum) { rotatedY = _VerticalRotated.Maximum; }
				if (rotatedY < _VerticalRotated.Minimum) { rotatedY = _VerticalRotated.Minimum; }

				_HorizontalRotated.Value = rotatedX;
				_VerticalRotated.Value = rotatedY;

				syncFlag = false;
			}
		}

		/// <summary>
		/// 실제 제어 값으로 부터 회전 제어기의 값을 계산한다.
		/// </summary>
		/// <param name="rotatedX"></param>
		/// <param name="rotatedY"></param>
		protected virtual void CalculateReal(out double realX, out double realY)
		{
			double tempX, tempY;

			tempX = _HorizontalRotated.Value;
			tempY = _VerticalRotated.Value;

			double tX = tempX * Math.Cos(Angle.Value / 180 * Math.PI) - tempY * Math.Sin(Angle.Value / 180 * Math.PI);
			double tY = tempX * Math.Sin(Angle.Value / 180 * Math.PI) + tempY * Math.Cos(Angle.Value / 180 * Math.PI);

			double rX = tX * _PrecisionHorizontal;
			double rY = tY * _PrecisionVertical;

			if (_ReverseHorizontal) { rX *= -1; }
			if (_ReverseVertical) { rY *= -1; }

			realX = rX;
			realY = rY;
		}

		/// <summary>
		/// 회전 제어 값으로 부터 실제 제어기의 값을 계산한다.
		/// </summary>
		/// <param name="rotatedX"></param>
		/// <param name="rotatedY"></param>
		protected virtual void CalculateRotated(out double rotatedX, out double rotatedY)
		{
			double tempX = _HorizontalReal.Value / _PrecisionHorizontal;
			double tempY = _VerticalReal.Value / _PrecisionVertical;

			if (_ReverseHorizontal) { tempX *= -1; }
			if (_ReverseVertical) { tempY *= -1; }

			rotatedX = tempX * Math.Cos(Angle.Value / 180 * Math.PI) + tempY * Math.Sin(Angle.Value / 180 * Math.PI);
			rotatedY = -1 * tempX * Math.Sin(Angle.Value / 180 * Math.PI) + tempY * Math.Cos(Angle.Value / 180 * Math.PI);

		}
		#endregion
	}
}
