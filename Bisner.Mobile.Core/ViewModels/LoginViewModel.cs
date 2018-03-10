using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Variables

        private bool _isBusy, _initError;
        private string _email;
        private string _password;
        private MvxCommand _loginCommand, _registerCommand, _forgotPasswordCommand;
        private string _statusText;

        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private string _passwordPlaceholder;
        private string _emailPlaceholder;
        private string _signinButtonTitle;

        #endregion Variables

        #region Constructor

        public LoginViewModel(ITokenService tokenService, IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _tokenService = tokenService;
            _userService = userService;

            // Set username from settings
            Email = Settings.Username;
            IsBusy = true;
        }

        #endregion Constructor

        #region Init

        public void SetResources()
        {
            try
            {
                PasswordPlaceholder = GetResource(ResKeys.mobile_sign_password);
                EmailPlaceholder = GetResource(ResKeys.mobile_signin_email);
                SigninButtonTitle = GetResource(ResKeys.mobile_signin_btn_signin);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        public async Task Init()
        {
            while (true)
            {
                try
                {
                    // Get the platform
                    await GetPlatformModelAsync();
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }

                if (Settings.DefaultLanguage == null)
                {
                    // No platform and no selected language yet, first time, show connection error
                    await UserDialogs.AlertAsync("Connection error, please make sure you are connected to the internet and press OK");
                }
                else
                {
                    // Language is set, meanse we have platform, continue
                    break;
                }
            }

            SetResources();

            IsBusy = false;
        }

        private async Task GetPlatformModelAsync()
        {
            // Get the platform model
            var platformModel = await PlatformService.GetPublicPlatformAsync(ApiPriority.Explicit);

            if (platformModel != null)
            {
                // Always set default language
                var defaultLanguage = platformModel.Languages.FirstOrDefault(l => l.IsDefault);

                // First time set the selected language to the default
                if (Settings.SelectedLanguageId == Guid.Empty)
                {
                    Settings.SelectedLanguageId = defaultLanguage.Id;
                }

                Settings.BlobUrl = platformModel.CdnBasePath;
                Settings.CompanyHeaderUrl = platformModel.Images?.CompanyCardHeader?.Medium;
                Settings.EventHeaderUrl = platformModel.Images?.EventHeader?.Medium;
                Settings.GroupsHeaderUrl = platformModel.Images?.GroupsHeader?.Medium;
            }
        }

        #endregion Init

        #region Properties

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); RaisePropertyChanged(() => IsNotBusy); LoginCommand.RaiseCanExecuteChanged(); }
        }

        public bool IsNotBusy => !IsBusy;

        public string Email
        {
            get => _email;
            set { _email = value; RaisePropertyChanged(() => Email); LoginCommand.RaiseCanExecuteChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; RaisePropertyChanged(() => Password); LoginCommand.RaiseCanExecuteChanged(); }
        }

        #endregion Properties

        #region Init

        public bool InitError
        {
            get => _initError;
            set { _initError = value; RaisePropertyChanged(() => InitError); }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; RaisePropertyChanged(() => StatusText); }
        }

        public string PasswordPlaceholder
        {
            get => _passwordPlaceholder;
            set { _passwordPlaceholder = value; RaisePropertyChanged(() => PasswordPlaceholder); }
        }

        public string EmailPlaceholder
        {
            get => _emailPlaceholder;
            set { _emailPlaceholder = value; RaisePropertyChanged(() => EmailPlaceholder); }
        }

        public string SigninButtonTitle
        {
            get => _signinButtonTitle;
            set { _signinButtonTitle = value; RaisePropertyChanged(() => SigninButtonTitle); }
        }

        #endregion Init

        #region Register

        public MvxCommand RegisterCommand => _registerCommand ?? (_registerCommand = new MvxCommand(Register, CanRegister));

        private void Register()
        {
            try
            {
                UserDialogs.Alert("Register is not yet implemented");
            }
            catch (Exception ex)
            {
                Mvx.Resolve<IExceptionService>().HandleException(ex);
            }
        }

        private bool CanRegister()
        {
            return true;
        }

        #endregion Register

        #region ForgotPassword

        public MvxCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand(ForgotPassword, CanForgotPassword));

        private void ForgotPassword()
        {
            ShowViewModel<ResetPasswordViewModel>();
        }

        private bool CanForgotPassword()
        {
            return true;
        }

        #endregion ForgotPassword

        #region Authentication

        public MvxCommand LoginCommand { get { return _loginCommand ?? (_loginCommand = new MvxCommand(async () => await Login())); } }

        private async Task Login()
        {
            // Reset status text
            StatusText = "";
            IsBusy = true;

            // Save for later use
            Settings.Username = Email;

            try
            {
                var authenticationResult = await _tokenService.GetTokenAsync(Email, Password);

                if (authenticationResult.Success)
                {
                    // Authenticated
                    await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);
                    await Mvx.Resolve<ISignalRClient>().StartAsync();
                    Mvx.Resolve<IPushNotificationService>().RegisterPush();
                    Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(new LogInPresentationHint());
                }
                else
                {
                    // Auth failed
                    if (authenticationResult.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Invalid login
                        await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_signin_error_invaliduserpw_message), GetResource(ResKeys.mobile_signin_error_invaliduserpw_title));
                    }

                    if (authenticationResult.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // TODO : Resources voor message
                    }

                    if (authenticationResult.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // TODO : Resources voor message
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);

                // TODO : andere message tonen
                await UserDialogs.AlertAsync(GetResource(ResKeys.mobile_error_server_error));

                IsBusy = false;
            }
        }

        #endregion Authentication
    }
}

