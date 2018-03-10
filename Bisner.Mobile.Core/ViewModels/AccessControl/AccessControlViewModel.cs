using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Integrations;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.AccessControl
{
    public class AccessControlViewModel : BaseViewModel
    {
        #region Constructor

        private readonly IApiService<IIntegrationApi> _apiService;

        public AccessControlViewModel(IPlatformService platformService, IApiService<IIntegrationApi> apiService) : base(platformService)
        {
            _apiService = apiService;

            ItemSelected = new MvxCommand<AccessControlItemViewModel>(SelectItem);
            RefreshCommand = new MvxAsyncCommand(RefreshAsync);
        }

        private static void SelectItem(AccessControlItemViewModel item)
        {
            item.OpenCommand.Execute(null);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            try
            {
                await RefreshAsync();
                //Items = new ObservableCollection<AccessControlItemViewModel>
                //{
                //    new AccessControlItemViewModel
                //    {
                //        Title = "Front door",
                //        SubTitle = "Free entry",
                //        State = LockState.Close
                //    },
                //    new AccessControlItemViewModel
                //    {
                //        Title = "Conference room A",
                //        SubTitle = "Limited access",
                //        State = LockState.Close
                //    },
                //    new AccessControlItemViewModel
                //    {
                //        Title = "Meeting room B",
                //        SubTitle = "Free entry",
                //        State = LockState.Close
                //    },
                //    new AccessControlItemViewModel
                //    {
                //        Title = "Conference room C",
                //        SubTitle = "Limited access",
                //        State = LockState.Close
                //    },
                //};
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);

                Items = new ObservableCollection<AccessControlItemViewModel>();
            }
        }

        #endregion Init

        #region Items

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<AccessControlItemViewModel> Items
        {
            get => _items;
            set { _items = value; RaisePropertyChanged(() => Items); }
        }
        private ObservableCollection<AccessControlItemViewModel> _items;
        private bool _isRefreshing;

        public ICommand ItemSelected { get; private set; }

        #endregion Items

        #region Refresh

        public ICommand RefreshCommand { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        private async Task RefreshAsync()
        {
            if(IsRefreshing)
                return;
            
            IsRefreshing = true;

            try
            {
                var locks = await GetAccessControlsAsync(ApiPriority.UserInitiated);

                var itemViewModels = new List<AccessControlItemViewModel>();

                foreach (var apiAccessControlModel in locks)
                {
                    itemViewModels.Add(new AccessControlItemViewModel(this)
                    {
                        Id = apiAccessControlModel.Id,
                        State = LockState.Close,
                        SubTitle = apiAccessControlModel.SubTitle,
                        Title = apiAccessControlModel.Title,
                    });
                }

                Items = new ObservableCollection<AccessControlItemViewModel>(itemViewModels);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                Items = new ObservableCollection<AccessControlItemViewModel>();
            }

            IsRefreshing = false;
        }

        #endregion Refresh

        #region API

        public async Task<List<ApiAccessControlModel>> GetAccessControlsAsync(ApiPriority priority)
        {
            var api = _apiService.GetApi(priority);

            var locks = await api.GetLocks();

            return locks?.Data;
        }

        public async Task<ApiResponse<bool>> OpenAccessControlAsync(Guid id, ApiPriority priority)
        {
            var api = _apiService.GetApi(priority);

            var locks = await api.OpenLock(id);

            return locks;
        }

        public async Task<ApiResponse<bool>> CloseAccessControlAsync(Guid id, ApiPriority priority)
        {
            var api = _apiService.GetApi(priority);

            var locks = await api.CloseLock(id);

            return locks;
        }

        #endregion API
    }
}