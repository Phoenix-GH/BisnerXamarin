using System;
using System.Threading.Tasks;
using Bisner.Constants;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Menu;

namespace Bisner.Mobile.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;

        public MainViewModel(IPlatformService platformService, IUserService userService, IChatService chatService, INotificationService notificationService) : base(platformService)
        {
            _userService = userService;
            _chatService = chatService;
            _notificationService = notificationService;

            UnreadUpdateTask = Task.Factory.StartNew(async () =>
            {
                while (true) // or some condition
                {
                    await UpdateUnreadAsync();
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            });
        }

        #endregion Constructor

        #region Init

        private bool _isInitializing = false;
        private DateTime _lastInitTime = DateTime.MinValue;

        public async Task Init()
        {
            if (!_isInitializing && _lastInitTime < DateTime.Now.AddMinutes(3))
            {
                _isInitializing = true;
                try
                {
                    await PlatformService.GetPublicPlatformAsync(ApiPriority.Background);
                    await _userService.GetPersonalModelAsync(ApiPriority.Background);
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }

                _isInitializing = false;
                _lastInitTime = DateTime.Now;
            }
        }

        #endregion Init

        #region Background updates
        
        private Task UnreadUpdateTask { get; }

        private async Task UpdateUnreadAsync()
        {
            try
            {
                var unreadChatMessages = await _chatService.GetUnreadAsync(ApiPriority.Background);
                SendUnreadChatMessagesUpdate(unreadChatMessages.Count);

                var unreadNotifications = await _notificationService.GetNumberUnreadAsync();
                SendUnreadNotificationsUpdate(unreadNotifications);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Background updates

        #region Menu

        public void ShowMenu()
        {
            switch (App.AppPlatform)
            {
                case AppPlatform.iOS:
                    ShowViewModel<IosBaseMenuViewModel>();
                    break;
                case AppPlatform.Android:
                    ShowViewModel<AndroidMenuViewModel>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion Menu
    }
}
