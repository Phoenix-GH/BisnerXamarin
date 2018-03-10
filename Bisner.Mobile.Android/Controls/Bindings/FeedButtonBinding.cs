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
    public class FeedButtonBinding : MvxAndroidTargetBinding
    {
        protected Button Button => (Button)Target;

        private bool _currentValue;

        public FeedButtonBinding(Button button)
            : base(button)
        {
            button.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            // Value is being changed in the viewmodel like on iOS
            //_currentValue = !_currentValue;
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
            var button = Button;
            if (button == null)
                return;

            var colorId = _currentValue ? Resource.Color.bisnerblue : Resource.Color.feedbuttonnormaltextcolor;

            var color = new Color(ContextCompat.GetColor(Application.Context, colorId));

            var compoundDrawables = button.GetCompoundDrawables();

            var compoundDrawableLeft = compoundDrawables[0];
            compoundDrawableLeft?.TintDrawable(color);

            var compoundDrawableTop = compoundDrawables[1];
            compoundDrawableTop?.TintDrawable(color);

            var compoundDrawableRight = compoundDrawables[2];
            compoundDrawableRight?.TintDrawable(color);

            var compoundDrawableBottom = compoundDrawables[3];
            compoundDrawableBottom?.TintDrawable(color);

            button.SetCompoundDrawablesWithIntrinsicBounds(compoundDrawableLeft, compoundDrawableTop, compoundDrawableRight, compoundDrawableBottom);

            button.SetTextColor(color);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var button = Button;
                if (button != null)
                {
                    button.Click -= ButtonOnClick;
                }
            }
            base.Dispose(isDisposing);
        }

        public override Type TargetType => typeof(bool);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;
    }
}