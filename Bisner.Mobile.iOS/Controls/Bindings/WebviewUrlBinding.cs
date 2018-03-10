using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class WebviewUrlBinding : MvxTargetBinding
    {
        public WebviewUrlBinding(object target) : base(target)
        {
        }

        private string _currentValue;

        public void SetButtonBackground(string value)
        {
            if (value != _currentValue)
            {
                _currentValue = value;
                WebView.LoadRequest(new NSUrlRequest(new NSUrl(value)));
            }
        }

        private UIWebView WebView => Target as UIWebView;

        public override void SetValue(object value)
        {
            var stringValue = (string)value;
            if (WebView != null)
            {
                SetButtonBackground(stringValue);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {

            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;
    }
}