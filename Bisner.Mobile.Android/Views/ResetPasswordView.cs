using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Uri = Android.Net.Uri;

namespace Bisner.Mobile.Droid.Views
{
    [Activity(NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class ResetPasswordView : MvxAppCompatActivity<ResetPasswordViewModel>
    {
        #region Variables

        private float _pixelDensity;

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Activity

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var displayMetrics = new DisplayMetrics();

            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            _pixelDensity = TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, displayMetrics);

            if (Settings.CustomLogin)
            {
                SetContentView(Resource.Layout.resetpassword_view2);
                var drawable = ContextCompat.GetDrawable(Application.Context, Resource.Drawable.background_login);
                Window.SetBackgroundDrawable(drawable);
            }
            else
            {
                SetContentView(Resource.Layout.resetpassword_view);
            }
        }

        #endregion Activity
    }
}