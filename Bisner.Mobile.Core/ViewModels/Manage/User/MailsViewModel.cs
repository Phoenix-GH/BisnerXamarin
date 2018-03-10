using System;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Manage.User
{
    public class MailsViewModel : BaseViewModel
    {
        #region Constructor
        
        private MvxCommand _updateCommand;
        private bool _isUpdating;
        private bool _disableAllMails;
        private bool _disableDailyDigest;
        private bool _disableNewFeedPost;
        private bool _disableNewFeedPostComment;
        private bool _disableMention;
        private bool _disableAssigned;
        private bool _disableAdminMails;

        private readonly IUserService _userService;

        private ApiWhitelabelPrivateUserModel _personalModel;

        public MailsViewModel(IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _userService = userService;
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            try
            {
                _personalModel = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);

                DisableAllMails = _personalModel.NotificationSettings.DisableAllMails;
                DisableDailyDigest = _personalModel.NotificationSettings.DisableDailyDigest;
                DisableNewFeedPost = _personalModel.NotificationSettings.DisableNewFeedPost;
                DisableNewFeedPostComment = _personalModel.NotificationSettings.DisableNewFeedPostComment;
                DisableMention = _personalModel.NotificationSettings.DisableMention;
                DisableAssigned = _personalModel.NotificationSettings.DisableAssigned;
                DisableAdminMails = _personalModel.NotificationSettings.DisableAdminMails;

                OnEnableUpdate(false);
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
                UserDialogs.Alert(GetResource(ResKeys.mobile_error_server_error));
            }
        }

        #endregion Init

        #region About

        // TODO: Settings

        public string HeaderText => "Mails & Notifications";
        public string Description => "Change your mail and notification settings here";
        public string DisableAllMailsText => "All mails";
        public string DisableDailyDigestText => "Daily digest";
        public string DisableNewFeedPostText => "New feed post";
        public string DisableNewFeedPostCommentText => "Feed post comment";
        public string DisableMentionText => "Mentions";
        public string DisableAssignedText => "Assign";
        public string DisableAdminMailsText => "Admin emails";

        public bool DisableAllMailsChanged => DisableAllMails != _personalModel.NotificationSettings.DisableAllMails;
        public bool DisableAllMails
        {
            get => _disableAllMails;
            set
            {
                _disableAllMails = value;
                CanUpdate();
            }
        }

        public bool DisableDailyDigestChanged => DisableDailyDigest != _personalModel.NotificationSettings.DisableDailyDigest;
        public bool DisableDailyDigest
        {
            get => _disableDailyDigest;
            set
            {
                _disableDailyDigest = value;
                CanUpdate();
            }
        }

        public bool DisableNewFeedPostChanged => DisableNewFeedPost != _personalModel.NotificationSettings.DisableNewFeedPost;
        public bool DisableNewFeedPost
        {
            get => _disableNewFeedPost;
            set
            {
                _disableNewFeedPost = value;
                CanUpdate();
            }
        }

        public bool DisableNewFeedPostCommentChanged => DisableNewFeedPostComment != _personalModel.NotificationSettings.DisableNewFeedPostComment;
        public bool DisableNewFeedPostComment
        {
            get => _disableNewFeedPostComment;
            set
            {
                _disableNewFeedPostComment = value;
                CanUpdate();
            }
        }

        public bool DisableMentionChanged => DisableMention != _personalModel.NotificationSettings.DisableMention;
        public bool DisableMention
        {
            get => _disableMention;
            set
            {
                _disableMention = value;
                CanUpdate();
            }
        }

        public bool DisableAssignedChanged => DisableAssigned != _personalModel.NotificationSettings.DisableAssigned;
        public bool DisableAssigned
        {
            get => _disableAssigned;
            set
            {
                _disableAssigned = value;
                CanUpdate();
            }
        }

        public bool DisableAdminMailsChanged => DisableAdminMails != _personalModel.NotificationSettings.DisableAdminMails;
        public bool DisableAdminMails
        {
            get => _disableAdminMails;
            set
            {
                _disableAdminMails = value;
                CanUpdate();
            }
        }

        #endregion Settings

        #region Update

        public bool IsUpdating
        {
            get => _isUpdating;
            private set
            {
                _isUpdating = value; RaisePropertyChanged(() => IsUpdating);
                if (value)
                {
                    OnStartUpdating();
                }
                else
                {
                    OnStopUpdating();
                }
            }
        }

        public MvxCommand UpdateCommand
        {
            get { return _updateCommand ?? (_updateCommand = new MvxCommand(async () => await UpdateAsync(), CanUpdate)); }
        }

        public event Action<bool> EnableUpdate;

        private void OnEnableUpdate(bool enable)
        {
            EnableUpdate?.Invoke(enable);
        }

        public event Action StartUpdating;

        private void OnStartUpdating()
        {
            StartUpdating?.Invoke();
        }

        public event Action StopUpdating;

        private void OnStopUpdating()
        {
            StopUpdating?.Invoke();
        }

        private async Task UpdateAsync()
        {
            if (!IsUpdating)
            {
                IsUpdating = true;
                OnStartUpdating();

                try
                {
                    _personalModel = await _userService.UpdateNotificationSettingsAsync(new ApiNotificationSettings
                    {
                        DisableAllMails = DisableMention,
                        DisableAdminMails = _personalModel.NotificationSettings.DisableAdminMails,
                        DisableAssigned = DisableAssigned,
                        DisableDailyDigest = DisableAllMails,
                        DisableMention = DisableNewFeedPostComment,
                        DisableNewFeedPost = _personalModel.NotificationSettings.DisableNewFeedPost,
                        DisableNewFeedPostComment = _personalModel.NotificationSettings.DisableNewFeedPostComment,
                        OverwriteDefault = _personalModel.NotificationSettings.OverwriteDefault,
                    });

                    RaiseAllPropertiesChanged();

                    await UserDialogs.AlertAsync("Your profile has been updated");

                    CanUpdate();
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                    await UserDialogs.AlertAsync("An error occured while updating, please make sure you have an internet connection and try again.");
                    CanUpdate();
                }
                finally
                {
                    IsUpdating = false;
                    OnStopUpdating();
                }
            }
        }

        private bool CanUpdate()
        {
            RaiseAllPropertiesChanged();

            var enable = DisableAllMailsChanged || DisableDailyDigestChanged || DisableNewFeedPostChanged || DisableNewFeedPostCommentChanged || DisableMentionChanged || DisableAssignedChanged;

            OnEnableUpdate(enable);

            return enable;
        }

        #endregion Update
    }
}