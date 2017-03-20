using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
	public interface INanoeyeValueControl
	{
		/// <summary>
		/// 연동될 SEC.Nanoeye.DataType.IControlValue
		/// IControlDouble 또는 IControlInt만 허용.
		/// </summary>
		[System.ComponentModel.DefaultValue(null)]
		SECtype.IValue ControlValue { get; set; }

		/// <summary>
		/// Maximum, Minimum 을 결정할 속성.
		/// true 이면 ControlValue의 Maximum, Mimimum에 연동. false이면 Default값에 연동.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		bool IsLimitedMode { get; set; }

		/// <summary>
		/// 연동될 속성.
		/// true 이면 ControlValue의 Value에 연동. false이면 Offset에 연동.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		bool IsValueOperation { get; set; }
	}
}
