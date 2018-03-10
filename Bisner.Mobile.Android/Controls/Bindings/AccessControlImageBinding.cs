using System;
using Android.App;
using Android.Support.V4.Content;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using MvvmCross.Binding.Droid.Target;

namespace Bisner.Mobile.Droid.Controls.Bindings
{
    public class AccessControlImageBinding : MvxAndroidTargetBinding
    {
        #region Constructor

        public AccessControlImageBinding(object target) : base(target)
        {
        }

        #endregion Constructor

        #region Properties

        protected MvxRoundedImageView ImageView => (MvxRoundedImageView)Target;

        public override Type TargetType => typeof(MvxRoundedImageView);

        #endregion Properties

        #region Functions

        protected override void SetValueImpl(object target, object value)
        {
            if(value == null)
                return;

            var state = (LockState) value;

            int drawableId;

            switch (state)
            {
                case LockState.Close:
                    drawableId = Resource.Drawable.doorlock_icon_001;
                    break;
                case LockState.Opening:
                    drawableId = Resource.Drawable.doorlock_icon_002;
                    break;
                case LockState.Open:
                    drawableId = Resource.Drawable.doorlock_icon_003;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var drawable = ContextCompat.GetDrawable(Application.Context, drawableId);

            ImageView.SetImageDrawable(drawable);
        }

        #endregion Functions
    }
}