using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Bisner.Mobile.iOS.Controls.Bindings
{
    public class WebviewHtmlStringBinding
        : MvxTargetBinding
    {
        private string _currentValue;

        public void SetButtonBackground(string value)
        {
            if (value != _currentValue)
            {
                _currentValue = value;
                WebView.LoadHtmlString(value, null);
            }
        }

        private UIWebView WebView
        {
            get
            {
                return Target as UIWebView;
            }
        }

        public WebviewHtmlStringBinding(UIWebView webview)
            : base(webview)
        {
        }

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

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }
    }
}