using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Whitelabel;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.General.Notifications;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Feed;
using Microsoft.AspNet.SignalR.Client;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Notifications
{
    public class AndroidNotificationsViewModel : NotificationsViewModel
    {
        public AndroidNotificationsViewModel(IPlatformService platformService, INotificationService notificationService, ISignalRClient signalRClient) : base(platformService, notificationService, signalRClient)
        {
        }
    }

    /// <summary>
    /// ViewModel for notifications list
    /// </summary>
    public class NotificationsViewModel : BaseViewModel
    {
        #region Constructor

        private ExtendedObservableCollection<INotification> _items;
        private MvxCommand<INotification> _selectCommand;
        private ICommand _refreshCommand;
        private bool _isLoading;

        private readonly INotificationService _notificationService;
        private readonly ISignalRClient _signalRClient;

        public NotificationsViewModel(IPlatformService platformService, INotificationService notificationService, ISignalRClient signalRClient) : base(platformService)
        {
            _notificationService = notificationService;
            _signalRClient = signalRClient;
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await GetNotifications();
        }

        #endregion Init

        #region IEventViewModel

        public override void Attach()
        {
            base.Attach();

            _signalRClient.UpdateNotification += AddNotification;
        }

        public override void Unattach()
        {
            base.Unattach();

            _signalRClient.UpdateNotification += AddNotification;
        }

        #endregion IEventViewModel

        #region Items

        private void UpdateList(IEnumerable<INotification> notifications)
        {
            foreach (var notification in notifications)
            {
                var currentNotification = Items.FirstOrDefault(n => n.Id == notification.Id);

                if (currentNotification != null)
                {
                    // Update current
                    currentNotification.Update(notification);
                }
                else
                {
                    // Insert new
                    Items.Insert(0, notification);
                }

                SendUpdateUnread();
            }
        }

        private async Task GetNotifications()
        {
            try
            {
                var notificationModels = await _notificationService.GetAllAsync(ApiPriority.UserInitiated);

                var notifications = new List<INotification>();

                if (notificationModels?.Notifications != null)
                {
                    foreach (var notificationModel in notificationModels.Notifications)
                    {
                        // Get user 
                        notifications.Add(ToModel(notificationModel, notificationModels.RelatedItems));
                    }
                }

                Items = new ExtendedObservableCollection<INotification>(notifications.Where(n => n != null).OrderByDescending(n => n.CreationDateTime));

                SendUpdateUnread();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        public ExtendedObservableCollection<INotification> Items
        {
            get => _items;
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        /// <summary>
        /// Add notification to list or update existing one
        /// </summary>
        /// <param name="model"></param>
        /// <param name="relatedItemsModel"></param>
        private void AddNotification(ApiWhitelabelNotificationModel model, ApiRelatedItemsModel relatedItemsModel)
        {
            var notification = ToModel(model, relatedItemsModel);

            // Get existing notification if it exists
            var oldNotification = Items.FirstOrDefault(n => n.Id == notification.Id || n.RelatedItemId == notification.RelatedItemId);

            // Update list on main thread
            InvokeOnMainThread(() =>
            {
                if (oldNotification != null)
                {
                    oldNotification.Update(notification);
                }
                else
                {
                    Items.Insert(0, notification);
                }

                SendUpdateUnread();
            });
        }

        private void SendUpdateUnread()
        {
            var unread = Items.Count(i => !i.IsRead);// &&
                                                     //(i.NotificationType == NotificationTypes.WhitelabelContactAccepted ||
                                                     // i.NotificationType == NotificationTypes.CompanyConnectInvite ||
                                                     // i.NotificationType == NotificationTypes.CompanyConnectRejected ||
                                                     // i.NotificationType == NotificationTypes.WhitelabelCompanyPendingUser ||
                                                     // i.NotificationType == NotificationTypes.FeedMention));

            SendUnreadNotificationsUpdate(unread);
        }

        public MvxCommand<INotification> SelectedCommand
        {
            get { return _selectCommand ?? (_selectCommand = new MvxCommand<INotification>(async notification => await SelectAsync(notification))); }
        }

        private async Task SelectAsync(INotification notification)
        {
            var oldValue = notification.IsRead;

            notification.IsRead = true;

            if (await SetIsReadAsync(notification.Id))
            {
                SendUpdateUnread();
            }
            else
            {
                notification.IsRead = oldValue;
            }

            notification.ShowRelatedItem();
        }

        private async Task<bool> SetIsReadAsync(Guid id)
        {
            try
            {
                return await _notificationService.SetIsReadAsync(id, true);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                return false;
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        #endregion Items

        #region Refresh

        private async Task RefreshAsync()
        {
            if (!IsLoading)
            {
                IsLoading = true;

                await GetNotifications();

                IsLoading = false;
            }
        }

        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await RefreshAsync())); }
        }

        #endregion Refresh

        #region Build

        protected INotification ToModel(ApiWhitelabelNotificationModel notificationModel, ApiRelatedItemsModel relatedItemsModel)
        {
            // Can be null if it is an unsupported notification
            var notification = GetInstance(notificationModel, relatedItemsModel);

            if (notification != null)
            {
                notification.DisplayName = notificationModel.ByUser?.DisplayName;
                notification.AvatarUrl = notificationModel.ByUser?.Avatar?.Small;
                notification.UserId = notificationModel.ByUser?.Id ?? Guid.Empty;
            }

            return notification;
        }

        private INotification GetInstance(ApiWhitelabelNotificationModel notificationModel, ApiRelatedItemsModel relatedItemsModel)
        {
            INotification notification;

            string description;
            string descriptExtra = null;

            switch (notificationModel.Type)
            {
                case NotificationTypes.WhitelabelContactAccepted:
                    description = "Has accept your contact request";
                    notification = new UserNotification();
                    break;
                case NotificationTypes.WhitelabelContactInvite:
                    description = "Has invited you to become a contact";
                    notification = new UserNotification();
                    break;
                case NotificationTypes.WhitelabelContactRejected:
                    description = "Has rejected your contact request";
                    notification = new UserNotification();
                    break;
                case NotificationTypes.WhitelabelGroupUserJoin:
                    description = "Has joined your group {0}";
                    // This notitfication type has the group id as the relateditem id
                    var groupdModel = GetGroupModel(notificationModel, relatedItemsModel);
                    if (groupdModel == null)
                        return null;
                    descriptExtra = groupdModel.Name;
                    notification = new GroupNotification(groupdModel.Id);
                    break;
                case NotificationTypes.WhitelabelCompanyPendingUser:
                    description = "Wants to join your company";
                    notification = new CompanyNotification();
                    break;
                //case NotificationTypes.WhitelabelNewSuggestion:
                //    description = "{0} left a suggestion";
                //    break;
                case NotificationTypes.FeedMention:
                    description = "Mentioned you in a post";
                    notification = new FeedPostNotification();
                    SetFeedPostText(notification, notificationModel.RelatedItemId, relatedItemsModel);
                    break;
                case NotificationTypes.FeedComment:
                    description = "Commented on a feedpost you are following";
                    notification = new FeedPostNotification();
                    SetFeedPostText(notification, notificationModel.RelatedItemId, relatedItemsModel);
                    break;
                case NotificationTypes.WhitelabelEventUserInvite:
                    description = "Invited you to attend event {0}";
                    notification = new EventNotification();
                    descriptExtra = GetEventName(notificationModel, relatedItemsModel);
                    break;
                case NotificationTypes.WhitelabelEventPublished:
                    description = "New event coming up : {0}";
                    notification = new EventNotification();
                    descriptExtra = GetEventName(notificationModel, relatedItemsModel);
                    break;
                case NotificationTypes.FeedGroupPost:
                    description = "Just posted in {0}";
                    // This notitfication type has the group id in the data model
                    var groupEntity2 = GetGroupModel(notificationModel, relatedItemsModel);
                    if (groupEntity2 == null)
                        return null;
                    descriptExtra = groupEntity2.Name;
                    notification = new GroupNotification(groupEntity2.Id);
                    SetFeedPostText(notification, notificationModel.RelatedItemId, relatedItemsModel);
                    break;
                default:
                    Debug.WriteLine("UNKNOWN NOTIFICATION TYPE : {0} !!!!!!!!!!!!!!!!!!!!!", notificationModel.Type);
                    return null;
            }

            if (descriptExtra != null)
            {
                description = string.Format(description, descriptExtra);
            }

            notification.Text = description;
            notification.Id = notificationModel.Id;
            notification.CreationDateTime = notificationModel.CreationDateTime;
            notification.IsRead = notificationModel.IsRead;
            notification.IsReadOnDateTime = notificationModel.IsReadOnDateTime;
            notification.RelatedItemId = notificationModel.RelatedItemId;
            notification.NotificationType = notificationModel.Type;

            return notification;
        }

        private ApiWhitelabelGroupModel GetGroupModel(ApiWhitelabelNotificationModel notificationModel, ApiRelatedItemsModel relatedItemsModel)
        {
            var groupModel = relatedItemsModel?.Groups?.FirstOrDefault(g => g.Id == notificationModel.RelatedItemId);

            return groupModel;
        }

        private string GetEventName(ApiWhitelabelNotificationModel notificationModel, ApiRelatedItemsModel relatedItemsModel)
        {
            var eventModel = relatedItemsModel?.Events?.FirstOrDefault(e => e.Id == notificationModel?.RelatedItemId);

            if (eventModel != null)
            {
                return eventModel.Title;
            }

            return "Unknown event";
        }

        private void SetFeedPostText(INotification notification, Guid postId, ApiRelatedItemsModel relatedItemsModel)
        {
            var item = relatedItemsModel.Posts.FirstOrDefault(p => p.Id == postId);

            var dataModel = item?.Data?.ToDataModel();

            if (dataModel?.Text != null)
            {
                var bbCode = BbCode.ConvertToHtml(dataModel.Text);

                notification.ExtraText = bbCode;
            }
        }

        #endregion Build
    }
}
