using System;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;
using Bisner.Mobile.Droid.Extensions;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class FeedImageBinding : MvxAndroidTargetBinding
    {
        protected ImageView ImageView => (ImageView)Target;

        private bool _currentValue;

        public FeedImageBinding(ImageView imageView)
            : base(imageView)
        {
            imageView.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            SetButtonBackground();
            FireValueChanged(_currentValue);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;
            SetButtonBackground();
        }

        private void SetButtonBackground()
        {
            if (ImageView == null)
                return;

            var colorId = _currentValue ? Resource.Color.bisnerblue : Resource.Color.feedbuttonnormaltextcolor;

            var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

            var newDrawable = ImageView.Drawable?.TintDrawable(color);

            ImageView.SetImageDrawable(newDrawable);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (ImageView != null)
                {
                    ImageView.Click -= ButtonOnClick;
                }
            }

            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(bool);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;
    }
}