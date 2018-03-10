using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Service;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Base
{
    public abstract class BaseRefreshViewModel : BaseViewModel
    {
        #region Constructor

        protected BaseRefreshViewModel(IPlatformService platformService) : base(platformService)
        {
            RefreshCommand = new MvxAsyncCommand(InvokeRefreshAsync);
        }

        #endregion Constructor

        #region Refresh
        
        private bool _isRefreshing;

        public ICommand RefreshCommand { get; }

        protected abstract Task RefreshAsync();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            protected set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        protected virtual async Task InvokeRefreshAsync()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;

                try
                {
                    await RefreshAsync();
                }
                catch (Exception ex)
                {
                    ExceptionService.HandleException(ex);
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
        }

        #endregion Refresh
    }
}