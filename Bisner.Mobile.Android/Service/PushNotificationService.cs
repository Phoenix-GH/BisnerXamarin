using System;
using System.Collections.Generic;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.Media;
using Bisner.Constants;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Droid.Extensions;
using Gcm.Client;
using Java.Util.Concurrent.Atomic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Plugin.CurrentActivity;
using AlertDialog = Android.App.AlertDialog;
using Debug = System.Diagnostics.Debug;

namespace Bisner.Mobile.Droid.Service
{
    [Service]
    public class AndroidPushNotificationService : GcmServiceBase, IPushNotificationService
    {
        #region Variables

        public static string RegistrationId { get; private set; }

        private NotificationHub Hub { get; set; }

        #endregion Variables

        #region Constructor

        public AndroidPushNotificationService() : base(Settings.SenderId)
        {
        }

        #endregion Constructor

        #region IPushNotificationService

        //public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        //{
        //    return StartCommandResult.Sticky;
        //}

        public void RegisterPush()
        {
            // Check if push settings are filled
            if (string.IsNullOrWhiteSpace(Settings.NotificationHubPath) || string.IsNullOrWhiteSpace(Settings.NotificationConnectionString))
                return;

            try
            {
                GcmClient.CheckDevice(Application.Context);
                GcmClient.CheckManifest(Application.Context);

                Debug.WriteLine("Registering with GCM");

                GcmClient.Register(CrossCurrentActivity.Current.Activity, Settings.SenderId);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        public void UnregisterPush()
        {
            try
            {
                GcmClient.UnRegister(Application.Context);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        #endregion IPushNotificationService

        #region GcmService

        protected override void OnMessage(Context context, Intent intent)
        {
            Debug.WriteLine("GCM Message Received!");
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
            setupSingleton.EnsureInitialized();

            intent.PrintExtras();

            var notificationResult = GetNotificationResult(intent);

            // No valid notification
            if (notificationResult == null)
                return;

            // Check if activity is running
            if (MainApplication.ApplicationOnPause)
            {
                // Application is not running, create notification
                if (!string.IsNullOrEmpty(notificationResult.Message) && notificationResult.ViewModelIntent != null)
                {
                    var pendingIntent = PendingIntent.GetActivity(Application.Context, 0,
                        notificationResult.ViewModelIntent, 0);
                    CreateNotification(notificationResult.Message, notificationResult.Message, pendingIntent);
                }
            }
            else
            {
                // App is running!!!!
            }
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Debug.WriteLine($"GCM Registered: {registrationId}");

            RegistrationId = registrationId;

            //CreateNotification("PushHandlerService-GCM Registered...",
            //                    "The device has been Registered!");

            try
            {
                // Create hub with correct settings
                Hub = new NotificationHub(Settings.NotificationHubPath, Settings.NotificationConnectionString, context);

                // Unregister all first
                Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Add user id as tag
            var tags = new List<string> { Settings.UserId.ToString() };

            try
            {
                // Register hub
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Debug.WriteLine($"GCM Unregistered: {registrationId}");

            //CreateNotification("GCM Unregistered...", "The device has been unregistered!");
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Debug.WriteLine($"Recoverable Error: {errorId}");

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Debug.WriteLine($"GCM Error: {errorId}");
        }

        #endregion GcmService

        #region Helpers

        private NotificationResult GetNotificationResult(Intent notificationIntent)
        {
            Intent viewModelIntent = null;

            var messageText = notificationIntent?.Extras?.GetString("message");
            //var notificationString = $"{{\"data\":{{\"message\":\"{displayname}\", \"itemId\":\"{itemId}\", \"typeId\":\"{type}\"}}}}";

            //var message = $"{{\"data\":{{\"message\":\"{displayname} : {text}\", \"conversationId\":\"{conversationId}\"}}}}";
            //var message =
            //    $"{{\"data\":{{\"message\":\"{text}\", \"postId\":\"{postId}\"}}}}";

            var translator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

            // Item notification
            var itemIdString = notificationIntent?.Extras?.GetString("itemId");
            if (!string.IsNullOrWhiteSpace(itemIdString))
            {
                Guid itemId;
                if (!Guid.TryParse(itemIdString, out itemId))
                {
                    // Not a valid item id
                    return null;
                }

                var typeId = notificationIntent.Extras.GetString("typeId");

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
                        var request = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                        request.ParameterValues = new Dictionary<string, string> { { "postId", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request);
                        break;
                    case NotificationTypes.FeedComment:
                        var request2 = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                        request2.ParameterValues = new Dictionary<string, string> { { "postId", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request2);
                        break;
                    case NotificationTypes.WhitelabelEventUserInvite:
                        var request3 = MvxViewModelRequest<EventViewModel>.GetDefaultRequest();
                        request3.ParameterValues = new Dictionary<string, string> { { "id", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request3);
                        break;
                    case NotificationTypes.WhitelabelEventPublished:
                        var request4 = MvxViewModelRequest<EventViewModel>.GetDefaultRequest();
                        request4.ParameterValues = new Dictionary<string, string> { { "id", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request4);
                        break;
                    case NotificationTypes.FeedGroupPost:
                        var request5 = MvxViewModelRequest<FeedViewModel>.GetDefaultRequest();
                        request5.ParameterValues = new Dictionary<string, string> { { "id", itemIdString }, { "feedType", FeedType.Group.ToString() } };
                        viewModelIntent = translator.GetIntentFor(request5);
                        break;
                    default:
                        return null;
                }
            }

            // post notification
            var postIdString = notificationIntent?.Extras?.GetString("postId");
            if (!string.IsNullOrWhiteSpace(postIdString))
            {
                Guid postId;
                if (!Guid.TryParse(postIdString, out postId))
                    return null;

                var request = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                request.ParameterValues = new Dictionary<string, string> { { "postId", postId.ToString() } };
                viewModelIntent = translator.GetIntentFor(request);
            }

            // Chat notification
            var conversationIdString = notificationIntent?.Extras?.GetString("conversationId");
            if (!string.IsNullOrWhiteSpace(conversationIdString))
            {
                Guid conversationId;
                if (!Guid.TryParse(conversationIdString, out conversationId))
                    return null;

                var request = MvxViewModelRequest<ChatConversationViewModel>.GetDefaultRequest();
                request.ParameterValues = new Dictionary<string, string> { { "id", conversationId.ToString() } };
                viewModelIntent = translator.GetIntentFor(request);
            }

            if (viewModelIntent == null)
            {
                var request = MvxViewModelRequest<MainViewModel>.GetDefaultRequest();
                viewModelIntent = translator.GetIntentFor(request);
            }

            return new NotificationResult
            {
                ViewModelIntent = viewModelIntent,
                Message = messageText,
            };
        }

        private void CreateNotification(string title, string desc, PendingIntent pendingIntent)
        {
            //Create notification
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;

            // Get the notificationsound
            var soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            //Create the notification
            var notification = new Notification.Builder(this).SetContentIntent(pendingIntent).SetSmallIcon(Resource.Drawable.Icon).SetContentTitle(desc).SetTicker(title).SetOnlyAlertOnce(true).SetAutoCancel(true).SetSound(soundUri).SetVibrate(new long[] { 1000 }).Build();

            notification.Defaults |= NotificationDefaults.Vibrate;
            notification.Defaults |= NotificationDefaults.Sound;

            //Auto-cancel will remove the notification once the user touches it

            //Show the notification
            notificationManager?.Notify(NotificationId, notification);
        }

        protected void DialogNotify(string title, string message)
        {
            var currentActivity = CrossCurrentActivity.Current.Activity;

            currentActivity.RunOnUiThread(() =>
            {
                var dlg = new AlertDialog.Builder(currentActivity);
                var alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate
                {
                    alert.Dismiss();
                });
                alert.SetMessage(message);
                alert.Show();
            });
        }

        #endregion Helpers

        #region Properties

        private static readonly AtomicInteger Ai = new AtomicInteger(0);
        public static int NotificationId => Ai.GetAndIncrement();

        #endregion Properties
    }

    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new[] { Core.Helpers.Constants.PackageName })]
    [IntentFilter(new[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new[] { Core.Helpers.Constants.PackageName })]
    [IntentFilter(new[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new[] { Core.Helpers.Constants.PackageName })]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BisnerBroadcastReciever : GcmBroadcastReceiverBase<AndroidPushNotificationService>
    {
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);
        }

        //public override void OnReceive(Context context, Intent intent)
        //{
        //    Debug.WriteLine($"GCM Message recieved : {intent}");

        //    var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
        //    setupSingleton.EnsureInitialized();

        //    intent.PrintExtras();

        //    var notificationResult = GetNotificationResult(intent);

        //    // No valid notification
        //    if (notificationResult == null)
        //        return;

        //    // Check if activity is running
        //    if (MainApplication.ApplicationOnPause)
        //    {
        //        // Application is not running, create notification
        //        if (!string.IsNullOrEmpty(notificationResult.Message) && notificationResult.ViewModelIntent != null)
        //        {
        //            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0,
        //                notificationResult.ViewModelIntent, 0);
        //            CreateNotification(context, notificationResult.Message, notificationResult.Message, pendingIntent);
        //        }
        //    }
        //    else
        //    {
        //        // App is running!!!!
        //    }

        //}

        private NotificationResult GetNotificationResult(Intent notificationIntent)
        {
            Intent viewModelIntent = null;

            var messageText = notificationIntent?.Extras?.GetString("message");
            //var notificationString = $"{{\"data\":{{\"message\":\"{displayname}\", \"itemId\":\"{itemId}\", \"typeId\":\"{type}\"}}}}";

            //var message = $"{{\"data\":{{\"message\":\"{displayname} : {text}\", \"conversationId\":\"{conversationId}\"}}}}";
            //var message =
            //    $"{{\"data\":{{\"message\":\"{text}\", \"postId\":\"{postId}\"}}}}";

            var translator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

            // Item notification
            var itemIdString = notificationIntent?.Extras?.GetString("itemId");
            if (!string.IsNullOrWhiteSpace(itemIdString))
            {
                Guid itemId;
                if (!Guid.TryParse(itemIdString, out itemId))
                {
                    // Not a valid item id
                    return null;
                }

                var typeId = notificationIntent.Extras.GetString("typeId");

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
                        var request = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                        request.ParameterValues = new Dictionary<string, string> { { "postId", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request);
                        break;
                    case NotificationTypes.FeedComment:
                        var request2 = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                        request2.ParameterValues = new Dictionary<string, string> { { "postId", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request2);
                        break;
                    case NotificationTypes.WhitelabelEventUserInvite:
                        var request3 = MvxViewModelRequest<EventViewModel>.GetDefaultRequest();
                        request3.ParameterValues = new Dictionary<string, string> { { "id", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request3);
                        break;
                    case NotificationTypes.WhitelabelEventPublished:
                        var request4 = MvxViewModelRequest<EventViewModel>.GetDefaultRequest();
                        request4.ParameterValues = new Dictionary<string, string> { { "id", itemIdString } };
                        viewModelIntent = translator.GetIntentFor(request4);
                        break;
                    case NotificationTypes.FeedGroupPost:
                        var request5 = MvxViewModelRequest<FeedViewModel>.GetDefaultRequest();
                        request5.ParameterValues = new Dictionary<string, string> { { "id", itemIdString }, { "feedType", FeedType.Group.ToString() } };
                        viewModelIntent = translator.GetIntentFor(request5);
                        break;
                    default:
                        return null;
                }
            }

            // post notification
            var postIdString = notificationIntent?.Extras?.GetString("postId");
            if (!string.IsNullOrWhiteSpace(postIdString))
            {
                Guid postId;
                if (!Guid.TryParse(postIdString, out postId))
                    return null;

                var request = MvxViewModelRequest<DetailsViewModel>.GetDefaultRequest();
                request.ParameterValues = new Dictionary<string, string> { { "postId", postId.ToString() } };
                viewModelIntent = translator.GetIntentFor(request);
            }

            // Chat notification
            var conversationIdString = notificationIntent?.Extras?.GetString("conversationId");
            if (!string.IsNullOrWhiteSpace(conversationIdString))
            {
                Guid conversationId;
                if (!Guid.TryParse(conversationIdString, out conversationId))
                    return null;

                var request = MvxViewModelRequest<ChatConversationViewModel>.GetDefaultRequest();
                request.ParameterValues = new Dictionary<string, string> { { "id", conversationId.ToString() } };
                viewModelIntent = translator.GetIntentFor(request);
            }

            if (viewModelIntent == null)
            {
                var request = MvxViewModelRequest<MainViewModel>.GetDefaultRequest();
                viewModelIntent = translator.GetIntentFor(request);
            }

            return new NotificationResult
            {
                ViewModelIntent = viewModelIntent,
                Message = messageText,
            };
        }

        private void CreateNotification(Context context, string title, string desc, PendingIntent pendingIntent)
        {
            //Create notification
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Get the notificationsound
            var soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            //Create the notification
            var notification = new Notification.Builder(Application.Context).SetContentIntent(pendingIntent).SetSmallIcon(Resource.Drawable.Icon).SetContentTitle(desc).SetTicker(title).SetOnlyAlertOnce(true).SetAutoCancel(true).SetSound(soundUri).SetVibrate(new long[] { 1000 }).Build();

            notification.Defaults |= NotificationDefaults.Vibrate;
            notification.Defaults |= NotificationDefaults.Sound;

            //Auto-cancel will remove the notification once the user touches it

            //Show the notification
            notificationManager?.Notify(AndroidPushNotificationService.NotificationId, notification);
        }
    }

    public class NotificationResult
    {
        public Intent ViewModelIntent { get; set; }

        public string Message { get; set; }
    }
}