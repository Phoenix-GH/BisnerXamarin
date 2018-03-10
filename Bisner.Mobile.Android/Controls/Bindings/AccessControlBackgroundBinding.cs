using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding.Droid.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class AccessControlBackgroundBinding : MvxAndroidTargetBinding
    {
        #region Constructor

        public AccessControlBackgroundBinding(object target) : base(target)
        {
        }

        #endregion Constructor

        #region Properties

        protected View View => (View)Target;

        public override Type TargetType => typeof(ShapeDrawable);

        #endregion Properties

        #region Functions

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null)
                return;

            var state = (LockState)value;

            var drawable = ContextCompat.GetDrawable(Application.Context, Resource.Drawable.accesscontrol_button_background);

            var gradientDrawable = drawable as GradientDrawable;

            Color borderColor;

            switch (state)
            {
                case LockState.Close:
                    borderColor = new Color(230, 230, 230);
                    gradientDrawable?.SetColor(Color.White.ToArgb());
                    break;
                case LockState.Opening:
                    borderColor = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.bisnerblue));
                    gradientDrawable?.SetColor(Color.White.ToArgb());
                    break;
                case LockState.Open:
                    borderColor = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.bisnerblue));
                    gradientDrawable?.SetColor(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.bisnerblue)).ToArgb());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gradientDrawable?.SetStroke(2, borderColor);

            View.Background = drawable;
        }

        #endregion Functions
    }
}