using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Members
{
    public class MembersViewModel : BaseRefreshViewModel
    {
        #region Constructor

        private List<ICompany> _companies;
        private List<IUser> _users;

        private List<IItemBase> _members;
        private bool _showCompanies;
        private MvxCommand<IItemBase> _itemSelectedCommand;

        private MembersButtonItem _buttonItem;
        //private readonly MemberSearchItem _searchItem;

        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public MembersViewModel(IPlatformService platformService, IUserService userService, ICompanyService companyService) : base(platformService)
        {
            _userService = userService;
            _companyService = companyService;
            ShowCompanies = true;
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            //_searchItem = new MemberSearchItem
            //{
            //    PlaceholderText = "Search members ...",
            //    SearchAction = SearchAction,
            //};

            var companyText = Settings.GetResource(ResKeys.mobile_members_list_companies);
            var membersText = Settings.GetResource(ResKeys.mobile_members_list_members);

            _buttonItem = new MembersButtonItem(companyText, membersText)
            {
                CompanyAction = () =>
                {
                    ShowCompanies = true;
                    FillMembers(null);
                },
                MembersAction = () =>
                {
                    ShowCompanies = false;
                    FillMembers(null);
                }
            };

            await LoadMembersAsync();
        }

        #endregion Init

        #region Items

        public bool ShowCompanies
        {
            get => _showCompanies;
            set
            {
                _showCompanies = value;
                RaisePropertyChanged(() => ShowCompanies);
            }
        }

        public List<IItemBase> Members
        {
            get => _members;
            private set { _members = value; RaisePropertyChanged(() => Members); }
        }

        private void FillMembers(string input)
        {
            var items = new List<IItemBase> { _buttonItem };

            input = input?.ToLower();

            if (ShowCompanies)
            {
                var companies = string.IsNullOrEmpty(input)
                    ? _companies
                    : _companies.Where(c => c.Name.ToLower().Contains(input)).ToList();

                items.AddRange(companies.OrderBy(c => c.Name));
            }
            else
            {
                var users = string.IsNullOrEmpty(input) ? _users :
                    _users.Where(u => u.DisplayName.ToLower().Contains(input) || u.CompanyName.ToLower().Contains(input))
                        .OrderBy(u => u.DisplayName).ToList();

                items.AddRange(users.OrderBy(u => u.DisplayName));
            }

            Members = new List<IItemBase>(items);
        }

        private async Task LoadMembersAsync()
        {
            try
            {
                if (!IsRefreshing)
                {
                    IsRefreshing = true;

                    await GetUsersAsync();
                    await GetCompaniesAsync();

                    FillMembers(null);

                    IsRefreshing = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private async Task GetUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                _users = users.Select(u => u.ToModel()).ToList();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private async Task GetCompaniesAsync()
        {
            try
            {
                var companyModels = await _companyService.GetAllAsync(ApiPriority.UserInitiated);

                var companies = companyModels.Select(c => c.ToModel());

                _companies = companies.ToList();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        public MvxCommand<IItemBase> ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new MvxCommand<IItemBase>(MemberSelected));

        public void MemberSelected(IItemBase baseItem)
        {
            if (baseItem is IUser)
            {
                ShowViewModel<UserViewModel>(new { userId = baseItem.Id });
            }

            if (baseItem is ICompany)
            {
                ShowViewModel<FeedViewModel>(new { id = baseItem.Id, feedType = FeedType.Company });
            }
        }

        #endregion Items

        #region Search

        private string _searchInput;

        public string SearchInput
        {
            get => _searchInput;
            set
            {
                FillMembers(value);

                _searchInput = value; RaisePropertyChanged(() => SearchInput);
            }
        }

        #endregion Search

        #region Refresh

        protected override async Task RefreshAsync()
        {
            await LoadMembersAsync();
        }

        #endregion Refresh
    }
}
