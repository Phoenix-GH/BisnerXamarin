using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class ControlErrorBinding
        : MvxTargetBinding
    {
        public void SetColors(UIControl control, bool value)
        {
            control.Layer.BackgroundColor = value ? Appearance.Colors.Error.CGColor : Appearance.Colors.White.CGColor;
            control.Layer.BorderWidth = value ? 1 : 1;
            control.Layer.BorderColor = value ? Appearance.Colors.Error.CGColor : Appearance.Colors.TextFieldBorderColor.CGColor;
        }

        private UIControl Control
        {
            get
            {
                return Target as UIControl;
            }
        }

        private bool _currentValue;

        public ControlErrorBinding(UIControl control)
            : base(control)
        {
            control.EditingDidEnd += ControlValueChanged;
        }

        private void ControlValueChanged(object sender, EventArgs eventArgs)
        {
            SetColors();
            FireValueChanged(_currentValue);
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;
            SetColors();
        }

        private void SetColors()
        {
            if (Control != null)
            {
                SetColors(Control, _currentValue);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var button = Control;
                if (button != null)
                {
                    button.TouchUpInside -= ControlValueChanged;
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType
        {
            get { return typeof(bool); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }
    }
}