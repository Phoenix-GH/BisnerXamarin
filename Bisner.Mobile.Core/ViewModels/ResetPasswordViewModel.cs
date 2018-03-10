using System;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        #region Constructor & init

        private string _email;
        private bool _isNotBusy;
        private bool _isBusy;

        private readonly IApiService<IPlatformApi> _apiService;

        public ResetPasswordViewModel(IPlatformService platformService, IApiService<IPlatformApi> apiService) : base(platformService)
        {
            _apiService = apiService;

            IsBusy = false;

            ResetCommand = new MvxAsyncCommand(ResetPassword);
            BackCommand = new MvxCommand(() => Close(this));
            SetResources();
        }

        private void SetResources()
        {
            EmailPlaceHolder = GetResource(ResKeys.mobile_signin_email);
            ButtonTitle = "Reset password";
        }

        #endregion Constructor & init

        #region Actions

        public string Message { get; set; }

        public string Email
        {
            get => _email;
            set { _email = value; RaisePropertyChanged(() => Email); }
        }

        public MvxAsyncCommand ResetCommand { get; }
        public string EmailPlaceHolder { get; set; }

        public string ButtonTitle { get; set; }

        public bool IsNotBusy
        {
            get => _isNotBusy;
            set { _isNotBusy = value; RaisePropertyChanged(() => IsNotBusy); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                IsNotBusy = !value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public IMvxCommand BackCommand { get;  }

        private async Task ResetPassword()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    var response = await _apiService.UserInitiated.RequestPasswordReset(Email);

                    if (response.Success)
                    {
                        await UserDialogs.AlertAsync(
                            "We have sent an email with instructions on how to reset your password");

                        Close(this);
                    }
                    else
                    {
                        await UserDialogs.AlertAsync("Something went wrong, please try again");
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                IsBusy = false;
            }
        }

        #endregion Actions
    }
}