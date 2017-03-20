using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.NanoColumn
{
	//[Obsolete("Use \"GenericSupport.DataType.ITransform2DDouble\"",true)]	
	internal class ColumnDoubleRotation : ColumnValueBase<double>, SECtype.IControlDouble
	{
		public enum AxisTypeEnum
		{
			X,
			Y
		}

		protected AxisTypeEnum _AxisType = AxisTypeEnum.X;
		public AxisTypeEnum AxisType
		{
			get { return _AxisType; }
			set
			{
				
				if (_AxisType != value)
				{
					_AxisType = value;
					this.Value = _Value;
				}
			}
		}

		protected ColumnDoubleRotation _RelatedAxis = null;
		public ColumnDoubleRotation RelatedAxis
		{
			get { return _RelatedAxis; }
			set
			{
				if (_RelatedAxis != value)
				{
					if (_RelatedAxis != null)
					{
						_RelatedAxis.ValueChanged -= new EventHandler(_Related_ValueChanged);
					}
					_RelatedAxis = value;
					_RelatedAxis.ValueChanged += new EventHandler(_Related_ValueChanged);
					SendCalcuratedValue();
				}
			}
		}

		protected SECtype.IControlDouble _RotationValue = null;
		public SECtype.IControlDouble RotationValue
		{
			get { return _RotationValue; }
			set
			{
				if (_RotationValue != value)
				{
					if (_RotationValue != null)
					{
						_RotationValue.ValueChanged -= new EventHandler(_Related_ValueChanged);
					}
					_RotationValue = value;
					_RotationValue.ValueChanged += new EventHandler(_Related_ValueChanged);
					SendCalcuratedValue();
				}
			}
		}

		void _Related_ValueChanged(object sender, EventArgs e)
		{
			if (_IsInited)
			{
				SendCalcuratedValue();
			}
		}

		public override double Value
		{
			get { return base.Value; }
			set
			{
				base.Value = value;
				SendCalcuratedValue();
			}
		}

		private void SendCalcuratedValue()
		{
			//if( _IsInited && _Enable)
			if ((_Viewer != null) && _IsInited && _Enable)
			{
				int val;
				ushort addr = (ushort)((ushort)setter | (ushort)MiniSEM_DeviceType.Set);

				double coarse = Math.Cos(_RotationValue.Value * Math.PI / 180);
				double sine = Math.Sin(_RotationValue.Value * Math.PI / 180);

				switch (_AxisType)
				{
				case AxisTypeEnum.X:
					val = (int)((base._Value + this._Offset) / this._Precision * coarse - (_RelatedAxis.Value + _RelatedAxis.Offset) / _RelatedAxis.Precision * sine);
					break;
				case AxisTypeEnum.Y:
					val = (int)((base._Value + this._Offset) / this._Precision * coarse + (_RelatedAxis.Value + _RelatedAxis.Offset) / _RelatedAxis.Precision * sine);
					break;
				default:
					throw new NotSupportedException();
				}

				//val = (int)(oriX * coarse - oriY * sine);

				if (val > (this._Maximum / this._Precision)) { val = (int)(this._Maximum / this._Precision); }
				else if (val < (this._Minimum / this._Precision)) { val = (int)(this._Minimum / this._Precision); }
				//System.Diagnostics.Debug.WriteLine( val.ToString(), this.Name );
				//System.Diagnostics.Debug.WriteLine( _AxisType.ToString() + coarse.ToString() + "," + sine.ToString() + "," + val.ToString() );
				//if(_Viewer != null)
				//{
				_Viewer.Send(this, addr, NanoView.PacketFixed8Bytes.MakePacket(addr, (uint)val), false);
				//}
			}
		}

		public override void Sync()
		{
			if (!_Enable)
			{
				throw new InvalidOperationException("This is not enabled.");
			}
			uint data = base.GetDeviceValue();
			this.Value = (double)data * _Precision - _Offset;
		}

		public override bool Validate()
		{
			if (!_Enable)
			{
				throw new InvalidOperationException("This is not enabled.");
			}
			uint data = base.GetDeviceValue();
			return (this.Value == (double)data * _Precision - _Offset);
		}

		public override void EndInit()
		{
			if ((_RotationValue == null) || (_RelatedAxis == null))
			{
				throw new ArgumentException();
			}

			base.EndInit();

		}

		public override void CommunicationAck(uint ackData)
		{
		}
	}
}
