using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindowsAzure.Messaging;
using Acr.UserDialogs;
using Akavache;
using Bisner.Constants;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.MvvmcrossApp;
using Bisner.Mobile.iOS.Views.Notifications;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        #region Constructor

        public static UIWindow MainWindow;
        private NotificationAlertAlertWindow _notificationAlertAlertWindow;

        private static Setup _setup;

        #endregion Constructor

        #region FinishedLaunching

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MainWindow = new UIWindow(UIScreen.MainScreen.Bounds);

            // Appearance api
            Appearance.SetAppearance();

            _setup = new Setup(this, MainWindow);
            _setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Task.Run(() =>
            {
                SetupNotificationTab();
            });

            MainWindow.MakeKeyAndVisible();

            ProcessNotification(options, true);

            return true;
        }

        private void SetupNotificationTab()
        {
            // Setup notification alert window
            var frame = UIScreen.MainScreen.Bounds;
            frame.Height = 64;

            _notificationAlertAlertWindow = new NotificationAlertAlertWindow(frame);
            Mvx.RegisterSingleton(typeof(INotificationAlertWindow), _notificationAlertAlertWindow);

            Mvx.Resolve<IAnalyticsService>().Initialize();
        }

        #endregion FinishedLaunching

        #region Activated/Background

        public override void OnActivated(UIApplication application)
        {
            // Never call base here
            Debug.WriteLine("OnActivated");

            BlobCache.EnsureInitialized();

            if (Settings.Token != null)
            {
                Task.Run(async () => await Mvx.Resolve<ISignalRClient>().StartAsync());
            }
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Never call base here
            Debug.WriteLine("OnResignActivation");

            Mvx.Resolve<ISignalRClient>().Stop();
        }

        public override void DidEnterBackground(UIApplication application)
        {
            base.DidEnterBackground(application);

            Debug.WriteLine("DidEnterBackground");
        }

        public override void WillEnterForeground(UIApplication application)
        {
            base.WillEnterForeground(application);

            Debug.WriteLine("WillEnterForeground");
        }

        #endregion Activated/Background

        #region Notifications

        private SBNotificationHub _hub;

        #region Registration

        public static void RegisterNotifications()
        {
            //This tells our app to go ahead and ask the user for permission to use Push Notifications
            // You have to specify which types you want to ask permission for
            // Most apps just ask for them all and if they don't use one type, who cares
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Sound |
                                                                              UIUserNotificationType.Alert | UIUserNotificationType.Badge, null);

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Badge |
                                                                                   UIRemoteNotificationType.Sound | UIRemoteNotificationType.Alert);
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            try
            {
                var userInteraction = Mvx.Resolve<IUserDialogs>();

                Debug.WriteLine("Failed to register for remote notifications for user ID");

                userInteraction.Alert($"Failed to register for remote notifications : {error}");
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try
            {
                var connectionString = Settings.NotificationConnectionString;
                var hubPath = Settings.NotificationHubPath;

                _hub = new SBNotificationHub(connectionString, hubPath);

                _hub.UnregisterAllAsync(deviceToken, error =>
                {
                    if (error != null)
                    {
                        Debug.WriteLine($"Error calling Unregister: {0}", error.ToString());
                        return;
                    }

                    var tags = new NSSet(Settings.UserId.ToString()); // create tags if you want
                    _hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
                    {
                        if (errorCallback != null)
                            Debug.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                    });
                });

                Debug.WriteLine($"RegisteredForRemoteNotifications, DeviceToken = {deviceToken}");
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public static void UnregisterPushNotifications()
        {
            UIApplication.SharedApplication.UnregisterForRemoteNotifications();
        }

        #endregion Registration

        #region Processing

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // check for a notification
            if (options != null)
            {
                // check for a local notification
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    Debug.WriteLine("Local notification recieved");
                    ProcessLocalNotification(options);
                    return;
                }

                // Check for push notification
                if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                {
                    options = options.GetTypeValueFromOptions<NSDictionary>(UIApplication.LaunchOptionsRemoteNotificationKey);
                }

                if (options.ContainsKey(new NSString("aps")))
                {
                    Debug.WriteLine("Push notification recieved");
                    ProcessPushNotification(options, fromFinishedLaunching);
                    return;
                }

                Debug.WriteLine($"Notification recieved but is not a local and does not contain 'aps' signature, first entry : {options.First()}");
            }
            else
            {
                Debug.WriteLine("Process notification options = null");
            }
        }

        private void ProcessLocalNotification(NSDictionary options)
        {
            var userInteraction = Mvx.Resolve<IUserDialogs>();

            userInteraction.Alert("Notification recieved from local!");
        }

        private void ProcessPushNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check for push notification
            var aps = options.GetTypeValueFromOptions<NSDictionary>("aps");

            //Extract the alert text
            if (aps != null && aps.ContainsKey(new NSString("alert")))
            {
                var alert = aps.GetTypeValueFromOptions<NSString>("alert");

                CheckBisnerNotification(options, fromFinishedLaunching);

                CheckChatNotification(options, fromFinishedLaunching);

                CheckPostNotification(options, fromFinishedLaunching);

                // Invalid notification
            }
        }

        #endregion Processing

        #region Bisner notifications

        private static void CheckBisnerNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            var itemId = options.GetTypeValueFromOptions<NSString>("itemId");

            if (itemId != null)
            {
                Guid relatedItemId;

                if (Guid.TryParse(itemId, out relatedItemId))
                {
                    var typeId = options.GetTypeValueFromOptions<NSString>("typeId");

                    if (typeId != null)
                    {
                        // Valid notification
                        ProcessBisnerNotification(relatedItemId, typeId, fromFinishedLaunching);
                    }
                }
            }
        }

        private static void ProcessBisnerNotification(Guid relatedItemId, NSString typeId, bool fromFinishedLaunching)
        {
            MvxPresentationHint presentationHint = null;

            switch (typeId)
            {
                //case NotificationTypes.WhitelabelContactAccepted:
                //    break;
                //case NotificationTypes.WhitelabelContactInvite:
                //    break;
                //case NotificationTypes.WhitelabelContactRejected:
                //    break;
                //case NotificationTypes.WhitelabelGroupUserJoin:
                //    break;
                //case NotificationTypes.WhitelabelCompanyPendingUser:
                //    break;
                //case NotificationTypes.WhitelabelNewSuggestion:
                //    break;
                case NotificationTypes.FeedMention:
                    App.PostId = relatedItemId;
                    presentationHint = new DetailHint { PostId = relatedItemId };
                    break;
                case NotificationTypes.FeedComment:
                    App.PostId = relatedItemId;
                    presentationHint = new DetailHint { PostId = relatedItemId };
                    break;
                case NotificationTypes.WhitelabelEventUserInvite:
                    App.EventId = relatedItemId;
                    presentationHint = new EventHint { EventId = relatedItemId };
                    break;
                case NotificationTypes.WhitelabelEventPublished:
                    App.EventId = relatedItemId;
                    presentationHint = new EventHint { EventId = relatedItemId };
                    break;
                case NotificationTypes.FeedGroupPost:
                    App.GroupId = relatedItemId;
                    presentationHint = new GroupHint { GroupId = relatedItemId };
                    break;
                default:
                    return;
            }

            if (!fromFinishedLaunching)
            {
                App.ConversationId = null;
                App.EventId = null;
                App.GroupId = null;
                App.PostId = null;

                TryShowScreen(presentationHint);
            }
        }

        private static void TryShowScreen(MvxPresentationHint presentationHint)
        {
            // If application is active don't show the screen
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) return;

            Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(presentationHint);
        }

        #endregion Bisner notification

        #region Post

        /// <summary>
        /// Check if this is a post notification
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fromFinishedLaunching"></param>
        private void CheckPostNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            var postIdString = options.GetTypeValueFromOptions<NSString>("postId");

            if (postIdString != null)
            {
                Guid postId;

                if (Guid.TryParse(postIdString, out postId))
                {
                    if (fromFinishedLaunching)
                    {
                        // App was killed
                        App.PostId = postId;
                        return;
                    }

                    // App was in background mode
                    TryShowPost(postId);
                }
            }
        }

        /// <summary>
        /// Try to show the detail view
        /// </summary>
        /// <param name="postId"></param>
        private void TryShowPost(Guid postId)
        {
            // If application is active don't show the screen
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) return;

            // If app is coming from the background and the viisble view is the post detail don't show
            if (MainWindow.RootViewController.VisibleViewController().IsDetailsViewController(postId)) return;

            MvxPresentationHint presentationHint = new DetailHint { PostId = postId };

            Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(presentationHint);
        }

        #endregion Post

        #region Chat

        /// <summary>
        /// Check if this is a chat notification
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fromFinishedLaunching"></param>
        private void CheckChatNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            var convsersationIdString = options.GetTypeValueFromOptions<NSString>("conversationId");

            if (convsersationIdString != null)
            {
                Guid conversationId;

                if (Guid.TryParse(convsersationIdString, out conversationId))
                {
                    if (fromFinishedLaunching)
                    {
                        // If from finishedlaunching set global id
                        App.ConversationId = conversationId;
                        return;
                    }

                    // If application is active show the notification bar
                    if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                    {
                        ProcessChatNotification(conversationId, options);
                        return;
                    }

                    // App was in background mode
                    TryShowConversation(conversationId);
                }
            }
        }

        /// <summary>
        /// Invoked when application is coming from killed state
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="options"></param>
        private void ProcessChatNotification(Guid conversationId, NSDictionary options)
        {
            var text = options.GetTypeValueFromOptions<NSString>("text");
            var msgId = options.GetTypeValueFromOptions<NSString>("messageId");

            Guid messageId;
            if (text != null && Guid.TryParse(msgId, out messageId))
            {
                var bisnerClient = Mvx.Resolve<ISignalRClient>();

                bisnerClient.ProcessNotificationMessage(conversationId, messageId, Settings.UserId, text);

                if (!MainWindow.RootViewController.VisibleViewController().IsChatConversation(conversationId))
                {
                    Mvx.Resolve<INotificationAlertWindow>().ShowNotification(conversationId, text);
                }
            }
        }

        /// <summary>
        /// Invoked when app is coming from a background state
        /// </summary>
        /// <param name="conversationId"></param>
        private void TryShowConversation(Guid conversationId)
        {
            if (!MainWindow.RootViewController.VisibleViewController().IsChatConversation(conversationId))
            {
                MvxPresentationHint presentationHint = new ChatConversationHint { SelectedUser = conversationId };

                Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(presentationHint);
            }
        }

        #endregion Chat

        #endregion Notifications
    }
}