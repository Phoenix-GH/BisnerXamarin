using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class GroupsViewModel : BaseViewModel
    {
        #region Constructor

        private List<IGroup> _items;
        private MvxCommand _refreshCommand;
        private bool _isRefreshing;
        private MvxCommand<IGroup> _itemSelectedCommand;

        private readonly IGroupService _groupService;

        public GroupsViewModel(IPlatformService platformService, IGroupService groupService) : base(platformService)
        {
            _groupService = groupService;
            Title = GetResource(ResKeys.mobile_dashboard_btn_groups);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            Items = await GetGroupsAsync();
        }

        #endregion Init

        #region Items

        public List<IGroup> Items
        {
            get => _items;
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private async Task<List<IGroup>> GetGroupsAsync()
        {
            var groups = new List<IGroup>();

            try
            {
                var groupModels = await _groupService.GetAllAsync(ApiPriority.UserInitiated);

                foreach (var groupModel in groupModels)
                {
                    groups.Add(ToModel(groupModel));
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

            return groups;
        }

        #endregion Items

        #region Selected

        public MvxCommand<IGroup> ItemSelectedCommand
        {
            get { return _itemSelectedCommand ?? (_itemSelectedCommand = new MvxCommand<IGroup>(async group => await ItemSelected(group))); }
        }

        public async Task ItemSelected(IGroup group)
        {

        }

        #endregion Selected

        #region Refresh

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        public MvxCommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await RefreshAsync())); }
        }

        private async Task RefreshAsync()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;

                var groups = await GetGroupsAsync();

                Items = groups;

                IsRefreshing = false;
            }
        }

        #endregion Refresh        

        #region Model

        private IGroup ToModel(ApiWhitelabelGroupModel groupModel)
        {
            var image = groupModel.Image.ToModel();
            var header = groupModel.Header.ToModel();

            if (image == null)
            {
                image = new Image
                {
                    Id = Guid.Empty,
                    Small = Defaults.EventHeaderDefaultString,
                    Large = Defaults.EventHeaderDefaultString,
                    Medium = Defaults.EventHeaderDefaultString,
                    OriginalFileName = Defaults.EventHeaderDefaultString
                };
            }

            if (header == null)
            {
                header = new Image
                {
                    Id = Guid.Empty,
                    Small = Defaults.EventHeaderDefaultString,
                    Large = Defaults.EventHeaderDefaultString,
                    Medium = Defaults.EventHeaderDefaultString,
                    OriginalFileName = Defaults.EventHeaderDefaultString
                };
            }

            // TODO : add all user models for admins members etc

            return new Group
            {
                Id = groupModel.Id,
                AdminIds = groupModel.AdminIds,
                Description = groupModel.Description,
                Name = groupModel.Name,
                InvitedMemberIds = groupModel.InvitedMemberIds,
                UserIds = groupModel.UserIds,
                Image = image,
                Header = header,
            };
        }

        #endregion Model
    }
}
