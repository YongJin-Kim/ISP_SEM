using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;

namespace SEC.Nanoeye.Support.Controls
{
    public partial class ImageForcusBarWithcvd : SEC.GUIelement.ImageForcusBar, INanoeyeValueControl
    {
        #region Property & Variables
        private SECtype.IControlDouble icvd = null;
        [Browsable(false)]
        [DefaultValue(null)]
        public SECtype.IValue ControlValue
        {
            get { return icvd; }
            set
            {
                if (icvd != null)
                {
                    icvd.ValueChanged -= new EventHandler(icvd_ValueChanged);
                    icvd.EnableChanged -= new EventHandler(icvd_EnableChanged);
                }

                icvd = value as SECtype.IControlDouble;

                if (icvd != null)
                {
                    icvd.ValueChanged += new EventHandler(icvd_ValueChanged);
                    icvd.EnableChanged += new EventHandler(icvd_EnableChanged);

                    ResetControl();
                }

                this.Invalidate();
            }
        }

        private bool _IsLimitedMode = true;
        [DefaultValue(true)]
        public bool IsLimitedMode
        {
            get { return _IsLimitedMode; }
            set
            {
                _IsLimitedMode = value;
                ResetControl();
            }
        }

        private bool _IsValueOperation = true;
        [DefaultValue(true)]
        public bool IsValueOperation
        {
            get { return _IsValueOperation; }
            set
            {
                _IsValueOperation = value;
                ResetControl();
            }
        }

        public override int Maximum
        {
            get { return base.Maximum; }
            set
            {
                base.Maximum = value;
                if (icvd != null)
                {
                    icvd.Maximum = value * icvd.Precision;
                }
            }
        }

        public override int Minimum
        {
            get
            {
                return base.Minimum;
            }
            set
            {
                base.Minimum = value;
                if (icvd != null)
                {
                    icvd.Minimum = value * icvd.Precision;
                }
            }
        }
        #endregion

        public ImageForcusBarWithcvd()
        {
            InitializeComponent();
        }

        bool changing = false;

        protected override void OnValueChanged()
        {
            if (!changing)
            {
                if (icvd != null)
                {
                    if (_IsValueOperation)
                    {
                        icvd.Value = base._Value * icvd.Precision;
                    }
                    else
                    {
                        icvd.Offset = base._Value * icvd.Precision;
                    }
                }
            }
            base.OnValueChanged();
        }

        private void ResetControl()
        {
            if (_IsLimitedMode)
            {
                if (base._Maximum != (int)Math.Floor(icvd.Maximum / icvd.Precision))
                {
                    base._Maximum = (int)Math.Floor(icvd.Maximum / icvd.Precision);
                }
                if (base._Minimum != (int)Math.Ceiling(icvd.Minimum / icvd.Precision))
                {
                    base._Minimum = (int)Math.Ceiling(icvd.Minimum / icvd.Precision);
                }
            }
            else
            {
                if (base._Maximum != (int)Math.Floor(icvd.DefaultMax / icvd.Precision))
                {
                    base._Maximum = (int)Math.Floor(icvd.DefaultMax / icvd.Precision);
                }
                if (base._Minimum != (int)Math.Ceiling(icvd.DefaultMin / icvd.Precision))
                {
                    base._Minimum = (int)Math.Ceiling(icvd.DefaultMin / icvd.Precision);
                }
            }

            int value;

            if (_IsValueOperation)
            {
                value = (int)Math.Round(icvd.Value / icvd.Precision);

            }
            else
            {
                value = (int)Math.Round(icvd.Offset / icvd.Precision);
            }

            base.Value = value;

            Invalidate();
        }

        void icvd_ValueChanged(object sender, EventArgs e)
        {
            changing = true;
            ResetControl();
            changing = false;
        }

        void icvd_EnableChanged(object sender, EventArgs e)
        {
            this.Enabled = icvd.Enable;
        }

        protected override string GetDisplayString()
        {
            if (DesignMode) { return ""; }

            switch (_ValueView)
            {
                default:
                    return base.GetDisplayString();
                case ValueViewMode.Number:
                    string str;
                    if (icvd == null)
                    {
                        str = _Value.ToString("F" + _DecimalPlace.ToString());
                    }
                    else
                    {
                        str = (_Value * icvd.Precision).ToString("F" + _DecimalPlace.ToString());
                    }

                    if (_ValueSymbolIsBack)
                    {
                        str += _ValueSymbol;
                    }
                    else
                    {
                        str = _ValueSymbol + str;
                    }
                    return str;
            }
        }
    }
}
