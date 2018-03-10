using System;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Target;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class TextViewHtmlClickBinding : MvxAndroidTargetBinding
    {
        #region Constructor

        public TextViewHtmlClickBinding(object target) : base(target)
        {
        }

        #endregion Constructor

        #region Conversion

        public override Type TargetType => typeof(ISpanned);

        protected override void SetValueImpl(object target, object value)
        {
            return;

            var text = value as string;

            if (!string.IsNullOrEmpty(text))
            {
                SetTextViewHtml(TextView, text);
            }
        }

        private TextView TextView => Target as TextView;

        #endregion Conversion

        #region Html Clickable

        private ISpanned ConvertToHtml(string value)
        {
            ISpanned result;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                result = Html.FromHtml(value, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                result = Html.FromHtml(value);
            }
            return result;
        }

        private void MakeLinkClickable(SpannableStringBuilder strBuilder, URLSpan span)
        {
            var start = strBuilder.GetSpanStart(span);
            var end = strBuilder.GetSpanEnd(span);
            var flags = strBuilder.GetSpanFlags(span);
            ClickableSpan clickable = new FuckingSpannend(span);

            strBuilder.SetSpan(clickable, start, end, flags);
            strBuilder.RemoveSpan(span);
        }

        private void SetTextViewHtml(TextView text, string html)
        {
            var sequence = ConvertToHtml(html);
            var strBuilder = new SpannableStringBuilder(sequence);

            var urls = text.GetUrls();

            foreach (var url in urls)
            {
                var urlSpan = url;
                MakeLinkClickable(strBuilder, urlSpan);
            }

            text.TextFormatted = strBuilder;
            text.MovementMethod = LinkMovementMethod.Instance;
        }

        #endregion Html Clickable
    }

    public class FuckingSpannend : ClickableSpan
    {
        private readonly URLSpan _span;

        public FuckingSpannend(URLSpan span)
        {
            _span = span;
        }

        public override void OnClick(View widget)
        {
            Debug.WriteLine(_span.URL);
        }
    }
}