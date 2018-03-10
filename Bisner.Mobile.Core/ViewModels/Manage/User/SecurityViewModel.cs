using System;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Manage.User
{
    public class SecurityViewModel : BaseViewModel
    {
        #region Constructor
        
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        private MvxCommand _updateCommand;
        private bool _isUpdating;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;
        private bool _passwordsDontMatch;
        private bool _oldPasswordEmpty;

        public SecurityViewModel(IPlatformService platformService, ITokenService tokenService, IUserService userService): base(platformService)
        {
            _tokenService = tokenService;
            _userService = userService;

            OldPasswordEmpty = false;
            PasswordsDontMatch = false;
        }

        #endregion Constructor

        #region Password
        
        public string PasswordHeaderText => GetResource(ResKeys.mobile_more_btn_security_privacy);

        public string PasswordDescription => GetResource(ResKeys.user_account_manage_security_password_info);

        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                if (!string.IsNullOrEmpty(value))
                {
                    OldPasswordEmpty = false;
                }
                RaisePropertyChanged(() => OldPassword);
            }
        }

        public bool OldPasswordEmpty
        {
            get => _oldPasswordEmpty;
            private set { _oldPasswordEmpty = value; RaisePropertyChanged(() => OldPasswordEmpty); }
        }

        public string OldPasswordText => GetResource(ResKeys.user_account_manage_security_oldpassword);

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PasswordsDontMatch = false;
                }
                _newPassword = value; RaisePropertyChanged(() => NewPassword);
            }
        }

        public string NewPasswordText => GetResource(ResKeys.user_account_manage_security_newspassword);

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PasswordsDontMatch = false;
                }
                _confirmPassword = value; RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public string ConfirmPasswordText => GetResource(ResKeys.user_account_manage_security_repeatpassword);

        public bool PasswordsDontMatch
        {
            get => _passwordsDontMatch;
            private set { _passwordsDontMatch = value; RaisePropertyChanged(() => PasswordsDontMatch); }
        }

        #endregion Password

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
            get { return _updateCommand ?? (_updateCommand = new MvxCommand(async () => await UpdateAsync())); }
        }

        public string UpdateText => GetResource(ResKeys.mobile_account_btn_update);

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
            OldPasswordEmpty = string.IsNullOrEmpty(OldPassword);

            if (NewPassword != ConfirmPassword)
            {
                PasswordsDontMatch = true;
            }

            if (!IsUpdating && !OldPasswordEmpty && !PasswordsDontMatch)
            {
                IsUpdating = true;
                OnStartUpdating();

                try
                {
                    // Try login with password first
                    var loginresult = await _tokenService.GetTokenAsync(Settings.Username, OldPassword);

                    if (!loginresult.Success)
                    {
                        // Old password is wrong
                        await UserDialogs.AlertAsync("Password is incorrect");
                    }
                    else
                    {
                        // Change password
                        var result = await _userService.ChangePasswordAsync(OldPassword, NewPassword, ConfirmPassword);

                        if (result)
                        {
                            OldPassword = null;
                            NewPassword = null;
                            ConfirmPassword = null;
                            OldPasswordEmpty = false;
                            PasswordsDontMatch = false;
                            await UserDialogs.AlertAsync("Your password has been changed");

                            // Close message
                            AfterChangedAction?.Invoke();
                        }
                        else
                        {
                            // Error occurred

                        }
                    }

                    RaiseAllPropertiesChanged();
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }
                finally
                {
                    IsUpdating = false;
                    OnStopUpdating();
                }
            }
        }

        public Action AfterChangedAction { get; set; }

        #endregion Update
    }
}
