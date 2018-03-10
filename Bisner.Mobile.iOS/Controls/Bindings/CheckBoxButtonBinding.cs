using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public abstract class CheckBoxButtonBinding
        : MvxTargetBinding
    {
        protected virtual bool SupportImage => false;

        protected UIImage YesImage { get; set; }
        protected UIImage NoImage { get; set; }

        protected virtual bool SupportTitleColor => false;

        protected UIColor YesTitleColor;
        protected UIColor NoTitleColor;

        protected virtual bool SupportBackgroundColor => false;

        protected UIColor YesBackgroundColor;
        protected UIColor NoBackgroundColor;

        protected virtual bool SupportBorderWidth => false;

        protected int YesBorderWidth;
        protected int NotBorderWidth;

        protected virtual bool SupportBorderColor => false;

        protected UIColor YesBorderColor;
        protected UIColor NoBorderColor;

        public void SetButtonBackground(UIButton button, bool value)
        {
            if (SupportImage)
            {
                var image = value ? YesImage : NoImage;
                button.SetImage(image, UIControlState.Normal);
            }

            if (SupportTitleColor)
            {
                button.SetTitleColor(value ? YesTitleColor : NoTitleColor, UIControlState.Normal);
            }

            if (SupportBackgroundColor)
            {
                button.BackgroundColor = value ? YesBackgroundColor : NoBackgroundColor;
            }

            if (SupportBorderWidth)
            {
                button.Layer.BorderWidth = value ? YesBorderWidth : NotBorderWidth;
            }

            if (SupportBorderColor)
            {
                button.Layer.BorderColor = value ? YesBorderColor.CGColor : NoBorderColor.CGColor;
            }
        }

        private UIButton Button => Target as UIButton;

        private bool _currentValue;

        protected CheckBoxButtonBinding(UIButton button)
            : base(button)
        {
            button.TouchUpInside += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            SetButtonBackground();
            FireValueChanged(_currentValue);
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;
            SetButtonBackground();
        }

        private void SetButtonBackground()
        {
            var button = Button;
            if (button != null)
            {
                SetButtonBackground(button, _currentValue);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var button = Button;
                if (button != null)
                {
                    button.TouchUpInside -= ButtonOnClick;
                }

                if (YesImage != null)
                {
                    YesImage.Dispose();
                    YesImage = null;
                }

                if (NoImage != null)
                {
                    NoImage.Dispose();
                    NoImage = null;
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(bool);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;
    }
}