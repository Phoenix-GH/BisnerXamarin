using Bisner.Mobile.Core.Service;

namespace Bisner.Mobile.iOS.Service
{
    public class ApplePushNotificationService : IPushNotificationService
    {
        public void RegisterPush()
        {
            AppDelegate.RegisterNotifications();
        }

        public void UnregisterPush()
        {
            AppDelegate.UnregisterPushNotifications();
        }
    }
}
