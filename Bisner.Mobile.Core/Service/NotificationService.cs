using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.Service
{
    public class NotificationResponseModel
    {
        public List<ApiWhitelabelNotificationModel> Notifications { get; set; }

        public ApiRelatedItemsModel RelatedItems { get; set; }
    }

    public class NotificationService : INotificationService
    {
        #region Constructor

        private readonly IApiService<INotificationApi> _notificationApiService;

        public NotificationService(IApiService<INotificationApi> notificationApiService)
        {
            _notificationApiService = notificationApiService;
        }

        #endregion Constructor

        #region Get

        public async Task<NotificationResponseModel> GetAllAsync(ApiPriority priority)
        {
            var cachedNotifications = BlobCache.LocalMachine.GetOrFetchObject("notifications", () => GetAllRemoteAsync(priority), DateTimeOffset.Now.AddSeconds(10));

            var notificationModels = await cachedNotifications.FirstOrDefaultAsync();

            return notificationModels;
        }

        private async Task<NotificationResponseModel> GetAllRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _notificationApiService.GetApi(priority);

            var notificationModels = await api.GetAll();

            return new NotificationResponseModel
            {
                Notifications = notificationModels.Data,
                RelatedItems = notificationModels.AdditionalData
            };
        }

        public async Task<int> GetNumberUnreadAsync()
        {
            var allNotifications = await GetAllAsync(ApiPriority.Background);

            var unreadNotifications = allNotifications.Notifications?
                .Where(
                    n =>
                        !n.IsRead &&
                        (n.Type == NotificationTypes.WhitelabelContactAccepted ||
                         n.Type == NotificationTypes.WhitelabelContactInvite ||
                         n.Type == NotificationTypes.WhitelabelContactRejected ||
                         n.Type == NotificationTypes.WhitelabelGroupUserJoin ||
                         n.Type == NotificationTypes.WhitelabelCompanyPendingUser ||
                         n.Type == NotificationTypes.FeedMention ||
                         n.Type == NotificationTypes.FeedComment ||
                         n.Type == NotificationTypes.WhitelabelEventUserInvite ||
                         n.Type == NotificationTypes.WhitelabelEventPublished ||
                         n.Type == NotificationTypes.FeedGroupPost)).ToList();



            return unreadNotifications?.Count ?? 0;
        }

        #endregion Get

        #region Update

        public async Task<bool> SetIsReadAsync(Guid id, bool isRead)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var api = _notificationApiService.GetApi(ApiPriority.UserInitiated);

            var response = await api.SetReadAsync(id, isRead);

            return response.Success;
        }

        public async Task<bool> SetAllReadAsync()
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var api = _notificationApiService.GetApi(ApiPriority.UserInitiated);

            var response = await api.SetAllReadAsync(true);

            return response.Success;
        }

        #endregion Update
    }
}
