using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core
{
    public class App : MvxApplication
    {
        #region Initialize

        /// <summary>
        /// Gets or sets the application platform.
        /// </summary>
        /// <value>
        /// The application platform.
        /// </value>
        public static AppPlatform AppPlatform { get; set; }

        public override void Initialize()
        {
            BlobCache.ApplicationName = "Bisner";
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
            BlobCache.EnsureInitialized();

            // Configuration
            Mvx.RegisterType<IConfiguration, DefaultConfiguration>();

            // Api's
            Mvx.RegisterType<IApiService<IAuthenticationApi>, ApiService<IAuthenticationApi>>();
            Mvx.RegisterType<IApiService<IPlatformApi>, ApiService<IPlatformApi>>();
            Mvx.RegisterType<IApiService<IEventApi>, ApiService<IEventApi>>();
            Mvx.RegisterType<IApiService<IFeedApi>, ApiService<IFeedApi>>();
            Mvx.RegisterType<IApiService<IGroupApi>, ApiService<IGroupApi>>();
            Mvx.RegisterType<IApiService<ICompanyApi>, ApiService<ICompanyApi>>();
            Mvx.RegisterType<IApiService<IMembersApi>, ApiService<IMembersApi>>();
            Mvx.RegisterType<IApiService<INotificationApi>, ApiService<INotificationApi>>();
            Mvx.RegisterType<IApiService<IUserApi>, ApiService<IUserApi>>();
            Mvx.RegisterType<IApiService<IChatApi>, ApiService<IChatApi>>();
            Mvx.RegisterType<IApiService<IBookingApi>, ApiService<IBookingApi>>();
            Mvx.RegisterType<IApiService<IIntegrationApi>, ApiService<IIntegrationApi>>();

            // Repositories
            Mvx.RegisterType<ITokenService, TokenService>();
            Mvx.RegisterType<IFeedService, FeedService>();
            Mvx.RegisterType<INotificationService, NotificationService>();
            Mvx.RegisterType<IUserService, UserService>();
            Mvx.RegisterType<IChatService, ChatService>();
            Mvx.RegisterType<ICompanyService, CompanyService>();
            Mvx.RegisterType<IEventService, EventService>();
            Mvx.RegisterType<IPlatformService, PlatformService>();
            Mvx.RegisterType<IGroupService, GroupService>();
            Mvx.RegisterType<IBookingService, BookingService>();
            Mvx.RegisterType<IIntegrationService, IntegrationService>();

            Mvx.ConstructAndRegisterSingleton<ISignalRClient, SignalRClient>();
            Mvx.RegisterSingleton(() => UserDialogs.Instance);

            Mvx.Resolve<IAnalyticsService>().Initialize();
            Settings.CustomLogin = Mvx.Resolve<IConfiguration>().AppId == Guid.Parse("465c7e32-4158-43c6-837c-14fa764694b5");

            RegisterAppStart(new BisnerAppStart());
        }

        private async Task UpdateBadgesAsync()
        {
            try
            {
                var chatService = Mvx.Resolve<IChatService>();

                var numberUnreadMessages = await chatService.GetUnreadAsync(ApiPriority.Background);

                Mvx.Resolve<IMvxMessenger>().Publish(new UpdateUnreadChatMessagesMessage(new object()) { NumberUnread = numberUnreadMessages.Count });
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        #endregion Initialize

        #region Logout

        public static async Task LogOut()
        {
            Settings.RefreshToken = null;
            Settings.Token = null;
            Settings.UserId = Guid.Empty;

            // Clear entire cache
            // TODO : Skip platform
            await BlobCache.LocalMachine.InvalidateAll();

            Mvx.Resolve<IPushNotificationService>().UnregisterPush();

            var hint = new LogOutPresentationHint();
            var viewDispatcher = Mvx.Resolve<IMvxViewDispatcher>();
            viewDispatcher.ChangePresentation(hint);
        }

        #endregion Logout

        #region Screen ids from notifications

        public static Guid? ConversationId { get; set; }
        public static Guid? PostId { get; set; }
        public static Guid? EventId { get; set; }
        public static Guid? GroupId { get; set; }

        #endregion Screen ids from notifications
    }

    public static class Defaults
    {
        public const string DefaultTrackerName = "BisnerDefaultTracker";

        public const string UserDefaultString = "UserDefault";
        public const string CompanyHeaderDefaultString = "CompanyHeader";
        public const string MembersHeaderDefaultString = "MembersHeader";
        public const string EventHeaderDefaultString = "EventHeader";
        public const string GroupHeaderDefault = "GroupHeader";
        public const string LocationHeaderDefault = "LocationHeader";
        public const string RoomHeaderDefault = "RoomHeader";
    }
}