using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Notifications;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel, IEventViewModel
    {
        #region Constructor

        protected BaseViewModel(IPlatformService platformService)
        {
            // TODO : AsyncErrorHandler package
            ExceptionService = Mvx.Resolve<IExceptionService>();
            PlatformService = platformService;

            UserCommandEnableChat = true;
        }

        #endregion Constructor

        #region Properties

        public string Title
        {
            get => _title;
            protected set { _title = value; RaisePropertyChanged(() => Title); }
        }

        protected IPlatformService PlatformService { get; }

        public IExceptionService ExceptionService { get; }

        #endregion Properties

        #region Interaction
        
        private IUserDialogs _userDialogs;
        protected IUserDialogs UserDialogs => _userDialogs ?? (_userDialogs =  Mvx.Resolve<IUserDialogs>());

        #endregion Interaction

        #region Resources

        protected string GetResource(string resourceKey)
        {
            var resource = Settings.GetResource(resourceKey);

            return resource;
        }

        #endregion Resources

        #region Commands

        private ICommand _closeCommand;
        private MvxCommand _userCommand;
        private string _title;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this))); }
        }

        public MvxCommand UserCommand => _userCommand ?? (_userCommand = new MvxCommand(ShowUser));

        /// <summary>
        /// Gets or sets a value indicating whether the user modal window will allow chat.
        /// This is used in the chat conversation window where you would get an infinite possible loop of opening the same chat over and over again
        /// </summary>
        /// <value>
        /// <c>true</c> if chat should be enabled; otherwise, <c>false</c>.
        /// </value>
        protected bool UserCommandEnableChat { get; set; }

        private void ShowUser()
        {
            ShowViewModel<UserViewModel>(new { userId = UserId, enableChat = UserCommandEnableChat });
        }

        public virtual Guid UserId { get; set; }

        #endregion Commands

        #region Event hooks

        public virtual void Attach()
        {

        }

        public virtual void Unattach()
        {

        }

        #endregion Event hooks

        #region Badges

        protected void SendUnreadNotificationsUpdate(int count)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new UpdateUnreadNotificationMessage(this)
            {
                NumberUnread = count
            });
        }

        protected void SendUnreadChatMessagesUpdate(int numberUnread)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new UpdateUnreadChatMessagesMessage(this) { NumberUnread = numberUnread });
        }

        #endregion Badges

        #region Helpers

        protected void Execute<T>(T handlers, Action<T> action) where T : class
        {
            Task.Factory.StartNew(() =>
            {
                if (handlers != null)
                {
                    try
                    {
                        action(handlers);
                    }
                    catch (Exception ex)
                    {
                        ExceptionService.HandleException(ex);
                    }
                }
            });
        }

        protected void LogMessage(string message)
        {
            Debug.WriteLine(message);
        }

        #endregion Helpers
    }
}
