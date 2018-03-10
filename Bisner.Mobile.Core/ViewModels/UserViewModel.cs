using System;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Chat;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        #region Constructor

        private ApiWhitelabelPublicUserModel _user;
        private bool _isBusy;
        private MvxCommand _messageCommand;
        private string _messageText;
        private bool _isCurrentUser;

        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public UserViewModel(IPlatformService platformService, IUserService userService, ICompanyService companyService) : base(platformService)
        {
            MessageText = GetResource(ResKeys.mobile_members_user_message);

            _userService = userService;
            _companyService = companyService;
        }

        #endregion Constructor

        #region Init

        public async Task Init(Guid userId, bool enableChat = true)
        {
            EnableChat = enableChat;

            try
            {
                _user = await _userService.GetUserAsync(userId, ApiPriority.UserInitiated);

                if (_user != null)
                {
                    UserId = _user.Id;

                    _isCurrentUser = _user.Id != Settings.UserId;

                    RaiseAllPropertiesChanged();
                }
                else
                {
                    // TODO : Wat dan?
                }

            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the chat button shows.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable chat]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableChat { get; set; }

        //public UserStatus Status => _user.Status;

        public string AvatarUrl => _user.Avatar.Small;

        public string HeaderUrl => _user.Header.Medium;

        public string DisplayName => _user.DisplayName;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;

                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => IsNotBusy);
            }
        }

        public bool IsNotBusy => !IsBusy;

        #endregion Properties

        #region Add contact

        //public MvxCommand ContactCommand
        //{
        //    get
        //    {
        //        return _contactCommand ?? (_contactCommand = new MvxCommand(async () => await ChangeContact(), CanAddContact));
        //    }
        //}

        //public string ContactText
        //{
        //    get
        //    {
        //        switch (_user.Status)
        //        {
        //            case UserStatus.NoContact:
        //                return GetResource(ResKeys.mobile_members_user_addcontact);
        //            case UserStatus.New:
        //                return "Accept/Reject contact";
        //            case UserStatus.Accepted:
        //                return "Remove contact";
        //            case UserStatus.Ignored:
        //                return "Add contact";
        //            case UserStatus.Pending:
        //                return "Pending";
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }
        //}

        //private async Task ChangeContact()
        //{
        //    if (!IsBusy)
        //    {
        //        try
        //        {
        //            var contactTask = await GetContactTask();

        //            if (contactTask != null)
        //            {
        //                if (CrossConnectivity.Current.IsConnected)
        //                {
        //                    IsBusy = true;

        //                    // Send to api
        //                    var userModel = await contactTask;

        //                    if (userModel != null)
        //                    {
        //                        // Success
        //                        var contact = userModel.Contacts.FirstOrDefault(c => c.User.Id == _user.Id);

        //                        using (var userService = Mvx.Resolve<IUserService>())
        //                        {
        //                            userService.AddOrUpdate(contact.User);
        //                        }

        //                        RaiseAllPropertiesChanged();
        //                    }
        //                    else
        //                    {
        //                        // Error
        //                        await Userinteraction.AlertAsync(GetResource(ResKeys.mobile_error_server_error));
        //                    }
        //                }
        //                else
        //                {
        //                    await Userinteraction.AlertAsync(GetResource(ResKeys.mobile_error_no_connection));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Mvx.Resolve<IExceptionService>().HandleException(ex);
        //        }
        //        finally
        //        {
        //            IsBusy = false;
        //        }
        //    }
        //}

        //private async Task<Task<ApiWhitelabelPrivateUserModel>> GetContactTask()
        //{
        //    switch (Status)
        //    {
        //        case UserStatus.NoContact:
        //            // Send contact request
        //            Userinteraction.Input(GetResource(ResKeys.mobile_members_invite_message), async (b, s) =>
        //            {
        //                if (b)
        //                {
        //                    await Mvx.Resolve<IBisnerClient>().AddContactAsync(_user.Id, s);
        //                }
        //            });
        //            break;
        //        case UserStatus.New:
        //            // Accept or reject contact invite
        //            var response =
        //                await
        //                    Userinteraction.ConfirmThreeButtonsAsync(ResKeys.mobile_members_accept_invite_message, null,
        //                        GetResource(ResKeys.mobile_members_btn_accept), GetResource(ResKeys.mobile_members_btn_reject), GetResource(ResKeys.platform_btn_cancel));

        //            switch (response)
        //            {
        //                case ConfirmThreeButtonsResponse.Positive:
        //                    return Mvx.Resolve<IBisnerClient>().AcceptContactAsync(_user.Id);
        //                case ConfirmThreeButtonsResponse.Negative:
        //                    return Mvx.Resolve<IBisnerClient>().RejectContactAsync(_user.Id);
        //                case ConfirmThreeButtonsResponse.Neutral:
        //                    return null;
        //                default:
        //                    throw new ArgumentOutOfRangeException();
        //            }
        //        case UserStatus.Ignored:
        //            // Accept again
        //            return null;
        //        case UserStatus.Pending:
        //            // Cancel current request
        //            return null;
        //    }

        //    return null;
        //}

        //private bool CanAddContact()
        //{
        //    return true;
        //}

        #endregion Add contact

        #region message

        public string MessageText
        {
            get { return _messageText; }
            private set { _messageText = value; RaisePropertyChanged(() => MessageText); }
        }

        public MvxCommand MessageCommand => _messageCommand ?? (_messageCommand = new MvxCommand(Message));

        //public bool CanChat => _user.Id != Settings.UserId && (PlatformSettingsApp.AllowAllUsersToChat || _user.Status == UserStatus.Accepted) && EnableChat;

        public string CompanyName { get; }

        public string Skills => _user.Skills?.Replace(",", ", ");

        public string About => _user.About;

        public bool ContactButtonAvailable => _isCurrentUser && IsNotBusy;

        public bool ShowAbout => !string.IsNullOrWhiteSpace(_user.About);

        public string AboutHeaderText => GetResource(ResKeys.mobile_account_title_about_you);

        public string SkillsHeaderText => "Skills";

        public bool ShowSkills => !string.IsNullOrWhiteSpace(_user.Skills);

        private void Message()
        {
            ShowViewModel<ChatConversationViewModel>(new { id = _user.Id });
        }

        #endregion Message
    }
}
