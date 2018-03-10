using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Foundation;
using Google.Analytics;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Service
{
    public class GaService : IAnalyticsService
    {
        #region Variables

        public string TrackingId = "UA-66711055-4";

        public ITracker Tracker;

        private const string AllowTrackingKey = "AllowTracking";

        #endregion Variables

        #region Constructor
        
        #endregion Constructor

        #region Init

        public void Initialize()
        {
            var optionsDict = NSDictionary.FromObjectAndKey(new NSString("YES"), new NSString(AllowTrackingKey));
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(optionsDict);

            Gai.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey(AllowTrackingKey);

            Gai.SharedInstance.DispatchInterval = 20;
            Gai.SharedInstance.TrackUncaughtExceptions = true;

            // Don't send data when running in debug mode
            if (Mvx.Resolve<IConfiguration>().TestMode)
                Gai.SharedInstance.DryRun = true;

            Tracker = Gai.SharedInstance.GetTracker(Defaults.DefaultTrackerName, TrackingId);
            Tracker.Set(GaiConstants.ScreenResolution, UIScreen.MainScreen.Bounds.ToString());
            Tracker.Set(GaiConstants.AppName, NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"].ToString());
            Tracker.Set(GaiConstants.AppVersion, NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString());
            Tracker.Set(GaiConstants.Version, NSBundle.MainBundle.InfoDictionary["DTSDKName"].ToString());
            Tracker.Set(GaiConstants.AppId, NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"].ToString());
        }

        #endregion Init

        #region Actions

        public void SendScreen(string screenName)
        {
            Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, screenName);
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
        }

        public void SendEvent(string gaEventCategory, string @event)
        {
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent(gaEventCategory, @event, "AppEvent", null).Build());
            Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately
        }

        public void SendException(string exceptionInfo, bool isFatalException)
        {
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateException(exceptionInfo, isFatalException).Build());
        }

        public void AddTracker(string trackingId)
        {
            var tracker = Gai.SharedInstance.GetTracker("BisnerCustomerTracker", trackingId);

            tracker.Set(GaiConstants.ScreenResolution, UIScreen.MainScreen.Bounds.ToString());
            tracker.Set(GaiConstants.AppName, NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"].ToString());
            tracker.Set(GaiConstants.AppVersion, NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString());
            tracker.Set(GaiConstants.Version, NSBundle.MainBundle.InfoDictionary["DTSDKName"].ToString());
            tracker.Set(GaiConstants.AppId, NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"].ToString());
        }

        #endregion Actions
    }
}
