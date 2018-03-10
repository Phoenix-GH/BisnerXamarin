using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.PictureChooser;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Bisner.Mobile.Core.ViewModels.Manage.User
{
    public class ProfileViewModel : BaseViewModel
    {
        #region Constructor

        private string _displayName;
        private string _firstName;
        private string _lastName;
        private string _email;

        private string _shortIntro;
        private string _about;
        private string _city;
        private string _linkedInUrl;
        private string _facebookUrl;
        private string _twitterUrl;
        private string _googleUrl;
        private string _instagramUrl;
        private List<ApiCentralLocationModel> _locations;
        private ApiCentralLocationModel _selectedLocation;

        private MvxCommand _updateCommand;
        private bool _isUpdating;
        private string _selectedLanguage;
        private bool _displayNameHasChanges;
        private bool _firstNameHasChanges;
        private string _avatarUrl;

        private readonly IUserService _userService;

        private ApiWhitelabelPrivateUserModel _personalModel;

        public ProfileViewModel(IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _userService = userService;

            ChangeAvatarCommand = new MvxAsyncCommand(ChangeAvatarAsync);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            try
            {
                Locations = await PlatformService.GetLocationsAsync(ApiPriority.UserInitiated);

                _personalModel = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);

                // Header image
                AvatarUrl = $"{_personalModel.Avatar.Medium}";

                // About
                DisplayName = _personalModel.DisplayName;
                FirstName = _personalModel.FirstName;
                LastName = _personalModel.LastName;
                Email = _personalModel.Email;

                // Profile
                ShortIntro = _personalModel.ShortAbout;
                About = _personalModel.About;
                City = _personalModel.City;
                LinkedInUrl = _personalModel.LinkedInUrl;
                FacebookUrl = _personalModel.FacebookUrl;
                TwitterUrl = _personalModel.TwitterUrl;
                GoogleUrl = _personalModel.GooglePlusUrl;
                InstagramUrl = _personalModel.InstagramUrl;

                SelectedLocation = Locations.Find(l => l.Id == _personalModel.LocationId);

                SelectedLanguage = Settings.SelectedLanguageId.ToString();

                OnEnableUpdate(false);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                UserDialogs.Alert(GetResource(ResKeys.mobile_error_server_error));
            }
        }

        #endregion Init

        #region About

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value; RaisePropertyChanged(() => DisplayName);

                DisplayNameHasChanges = value != _personalModel.DisplayName;
            }
        }

        public bool DisplayNameHasChanges
        {
            get => _displayNameHasChanges;
            private set
            {
                _displayNameHasChanges = value;
                RaisePropertyChanged(() => DisplayNameHasChanges);
                CanUpdate();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value; RaisePropertyChanged(() => FirstName);
                FirstNameHasChanges = value != _personalModel.FirstName;
            }
        }

        public bool FirstNameHasChanges
        {
            get => _firstNameHasChanges;
            set { _firstNameHasChanges = value; RaisePropertyChanged(() => FirstNameHasChanges); CanUpdate(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; RaisePropertyChanged(() => LastName); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; RaisePropertyChanged(() => Email); }
        }

        #endregion About

        #region Profile

        public string ShortIntro
        {
            get => _shortIntro;
            set { _shortIntro = value; RaisePropertyChanged(() => ShortIntro); }
        }

        public string About
        {
            get => _about;
            set { _about = value; RaisePropertyChanged(() => About); }
        }

        public string City
        {
            get => _city;
            set { _city = value; RaisePropertyChanged(() => City); }
        }

        public string LinkedInUrl
        {
            get => _linkedInUrl;
            set { _linkedInUrl = value; RaisePropertyChanged(() => LinkedInUrl); }
        }

        public string FacebookUrl
        {
            get => _facebookUrl;
            set { _facebookUrl = value; RaisePropertyChanged(() => FacebookUrl); }
        }

        public string TwitterUrl
        {
            get => _twitterUrl;
            set { _twitterUrl = value; RaisePropertyChanged(() => TwitterUrl); }
        }

        public string GoogleUrl
        {
            get => _googleUrl;
            set { _googleUrl = value; RaisePropertyChanged(() => GoogleUrl); }
        }

        public string InstagramUrl
        {
            get => _instagramUrl;
            set { _instagramUrl = value; RaisePropertyChanged(() => InstagramUrl); }
        }

        public ICommand ChangeAvatarCommand { get; }

        private async Task ChangeAvatarAsync()
        {

            try
            {
                if (await RequestPermissionAsync(Permission.Camera) && await RequestPermissionAsync(Permission.Storage) )
                {
                    var task = Mvx.Resolve<IMvxBisnerImageTask>();
                    task.ChangeDevice(true);
                    task.TakePicture(1024, 30, async s => await AddPicture(s), () =>
                    {
                        Debug.WriteLine("CANCELLED!!!");
                    });
                }
                else
                {
                    await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_no_permission));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.AlertAsync("Something went wrong", "Error", "Ok");
                ExceptionService.HandleException(ex);
            }
        }

        private async Task AddPicture(Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            var userModel  = await _userService.ChangeAvatarAsync(memoryStream);
            _personalModel = userModel;

            AvatarUrl = $"{_personalModel.Avatar.Medium}";
        }

        private async Task<bool> RequestPermissionAsync(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (permissionStatus != PermissionStatus.Granted)
            {
                // TODO : ONly for android, needs resource
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                //{
                //    await Userinteraction.AlertAsync("Blalakemr");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                permissionStatus = results[permission];
            }

            return permissionStatus == PermissionStatus.Granted;
        }

        #endregion Profile

        #region Preferences

        public ApiCentralLocationModel SelectedLocation
        {
            get => _selectedLocation;
            set { _selectedLocation = value; RaisePropertyChanged(() => SelectedLocation); }
        }

        public List<ApiCentralLocationModel> Locations
        {
            get => _locations;
            set { _locations = value; RaisePropertyChanged(() => Locations); }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set { _selectedLanguage = value; RaisePropertyChanged(() => SelectedLanguage); }
        }

        #endregion Preferences

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

        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
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
                    var result = await _userService.UpdateProfileAsync(new ApiUserUpdateModel
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Id = Settings.UserId,
                        City = City,
                        LinkedInUrl = LinkedInUrl,
                        TwitterUrl = TwitterUrl,
                        FacebookUrl = FacebookUrl,
                        InstagramUrl = InstagramUrl,
                        About = About,
                        GooglePlusUrl = GoogleUrl,
                        //LocationId = SelectedLocation != null ? SelectedLocation.Id : Guid.Empty,
                        ShortAbout = ShortIntro,
                    });

                    await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_confirm_your_profile_update));

                    RaiseAllPropertiesChanged();
                    OnEnableUpdate(false);
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
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
            var enable = DisplayNameHasChanges || FirstNameHasChanges;

            OnEnableUpdate(enable);

            return enable;
        }

        #endregion Update

        #region Texts

        public string UpdateButtonText => GetResource(ResKeys.mobile_account_btn_update);

        public string BasicInfoText => GetResource(ResKeys.mobile_account_title_basic_information);

        public string DisplayNameText => GetResource(ResKeys.user_account_manage_profile_displayname);

        public string FirstnameText => GetResource(ResKeys.user_account_manage_profile_firstname);

        public string LastnameText => GetResource(ResKeys.user_account_manage_profile_lastname);

        public string EmailText => GetResource(ResKeys.user_account_manage_profile_email);

        public string AboutYouText => GetResource(ResKeys.mobile_account_title_about_you);

        public string ShortIntroText => GetResource(ResKeys.user_account_manage_profile_introduction_short);

        public string AboutTex => GetResource(ResKeys.user_account_manage_extended_about);

        public string CityText => GetResource(ResKeys.user_account_manage_profile_city);

        #endregion Texts
    }
}
