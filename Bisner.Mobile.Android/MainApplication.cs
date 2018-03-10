using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Runtime;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Calligraphy;
using MvvmCross.Platform;
using Plugin.CurrentActivity;

namespace Bisner.Mobile.Droid
{
    [Application(Label = Core.Helpers.Constants.AppName, Theme = "@style/Theme.Application", Debuggable = false, Icon = "@drawable/icon")]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        #region Constructor

        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        #endregion Constructor

        #region Application

        public override void OnCreate()
        {
            base.OnCreate();

            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Servicesvar config =

            var config =
                   new CalligraphyConfig.Builder().SetDefaultFontPath("fonts/Lato-Regular.ttf")
                       .SetFontAttrId(Resource.Attribute.fontPath)
                       .Build();

            CalligraphyConfig.InitDefault(config);

            UserDialogs.Init(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        #endregion Application

        #region ActivityListener

        public static bool ApplicationOnPause { get; private set; }

        public static bool MainViewIsRunning { get; set; }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
            ApplicationOnPause = true;
        }

        public void OnActivityResumed(Activity activity)
        {
            ApplicationOnPause = false;

            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        #endregion ActivityListener
    }
}