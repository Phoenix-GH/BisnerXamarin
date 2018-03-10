using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Gms.Analytics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using MvvmCross.Platform;

namespace Bisner.Mobile.Droid.Service
{
    public class GaService : IAnalyticsService
    {
        #region Variables

        private string TrackingId = "UA-66711055-5";

        private Tracker _defaultTracker;

        private GoogleAnalytics _googleAnalytics;

        #endregion Variables

        #region Constructor

        #endregion Constructor

        #region Init

        public void Initialize()
        {
            _googleAnalytics = GoogleAnalytics.GetInstance(Application.Context);
            _googleAnalytics.SetLocalDispatchPeriod(20);

            // Don't send data when running in debug mode
            if (Mvx.Resolve<IConfiguration>().TestMode)
                _googleAnalytics.SetDryRun(true);


            _defaultTracker = _googleAnalytics.NewTracker(TrackingId);
            _defaultTracker.EnableExceptionReporting(true);
            _defaultTracker.EnableAdvertisingIdCollection(true);
            _defaultTracker.EnableAutoActivityTracking(true);

            var metrics = new DisplayMetrics();
            var windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            windowManager.DefaultDisplay.GetMetrics(metrics);

            var packageInfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);

            _defaultTracker.SetAppName(Application.Context.ApplicationInfo.Name);
            _defaultTracker.SetAppVersion(packageInfo.VersionName);
            _defaultTracker.SetScreenResolution(metrics.WidthPixels, metrics.HeightPixels);
            _defaultTracker.SetAppId(packageInfo.PackageName);
        }

        #endregion Init

        #region Actions

        public void SendScreen(string screenName)
        {
            _defaultTracker.SetScreenName(screenName);
            _defaultTracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }

        public void SendEvent(string gaEventCategory, string @event)
        {
            var bla = new HitBuilders.EventBuilder(gaEventCategory, @event);
            _defaultTracker.Send(bla.Build());
        }

        public void SendException(string exceptionInfo, bool isFatalException)
        {
            HitBuilders.ExceptionBuilder builder = new HitBuilders.ExceptionBuilder();
            builder.SetDescription(exceptionInfo);
            builder.SetFatal(isFatalException);

            _defaultTracker.Send(builder.Build());
        }

        public void AddTracker(string trackingId)
        {
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!! NO IMPLEMENTATION TO ADD A CUSTOM TRACKER FOR THE WHITELABEL PLATFORM!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        #endregion Actions
    }
}