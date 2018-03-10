using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Droid.Extensions;
using MvvmCross.Droid.Support.V7.AppCompat;
using Uri = Android.Net.Uri;

namespace Bisner.Mobile.Droid.Views
{
    [Activity(NoHistory = false, ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class LoginView : MvxAppCompatActivity<LoginViewModel>
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
                SetContentView(Resource.Layout.login_view2);
                var drawable = ContextCompat.GetDrawable(Application.Context, Resource.Drawable.background_login);
                Window.SetBackgroundDrawable(drawable);
            }
            else
            {
                SetContentView(Resource.Layout.login_view);
            }

            var registerButton = FindViewById<Button>(Resource.Id.register_button);

            if (registerButton != null)
            {
                registerButton.Click += RegisterButtonOnClick;
            }

            if (Settings.CustomLogin)
            {
                var forgotPasswordButton = FindViewById<Button>(Resource.Id.forgot_password_button);

                if (forgotPasswordButton != null)
                {
                    var color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.white));

                    var newDrawable = ContextCompat.GetDrawable(Application.Context, Resource.Drawable.forgotpassword_icon).TintDrawable(color);

                    forgotPasswordButton.Background = newDrawable;
                }
            }
        }

        private void RegisterButtonOnClick(object sender, EventArgs eventArgs)
        {
            var browserIntent = new Intent(Intent.ActionView, Uri.Parse("https://thebridge.spaces.nexudus.com/en/signup"));
            StartActivity(browserIntent);
        }

        #endregion Activity
    }
}