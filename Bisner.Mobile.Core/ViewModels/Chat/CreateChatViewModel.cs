using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class CreateChatViewModel : ViewModelWithTitleBase
    {
        #region Constructor

        private List<IUser> _items, _allContacts;
        private string _searchInput;
        private ObservableCollection<IUser> _selectedItems;
        private ICommand _refreshCommand;
        private MvxCommand<IUser> _itemSelectCommand;
        private bool _isLoading;

        private readonly IUserService _userService;

        public CreateChatViewModel(IPlatformService platformService, IUserService userService) : base(platformService)
        {
            _userService = userService;

            Title = "Start conversation";
            SelectedItems = new ObservableCollection<IUser>();
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            await UpdateUsers();
        }

        #endregion Init

        #region Items

        public List<IUser> Items
        {
            get => _items;
            private set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public string SearchInput
        {
            get => _searchInput;
            set
            {
                _searchInput = value;
                Search(value);
                RaisePropertyChanged(() => SearchInput);
            }
        }

        public ObservableCollection<IUser> SelectedItems
        {
            get => _selectedItems;
            set { _selectedItems = value; RaisePropertyChanged(() => SelectedItems); }
        }

        private void Search(string value)
        {
            var contacts = value.Length > 1 ? _allContacts.Where(c => c.DisplayName.ToLower().Contains(value.ToLower())) : _allContacts;

            Items = contacts.OrderBy(c => c.DisplayName).ToList();
        }

        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await UpdateUsers())); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        public MvxCommand<IUser> ItemSelectCommand => _itemSelectCommand ?? (_itemSelectCommand = new MvxCommand<IUser>(SelectUser));

        private void SelectUser(IUser user)
        {
            Close(this);

            ChangePresentation(new ChatConversationHint { SelectedUser = user.Id });
        }

        private async Task UpdateUsers()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                
                IEnumerable<IUser> users = await UpdateAllMembers();

                _allContacts = users.ToList();

                Items = users.OrderBy(i => i.DisplayName).ToList();

                IsLoading = false;
            }
        }

        private async Task<List<IUser>> UpdateAllMembers()
        {
            List<IUser> users = null;

            try
            {
                var userModels = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                users = userModels.Where(u => u.Id != Settings.UserId).Select(u => u.ToModel()).ToList();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

            return users;
        }

        #endregion Items
    }
}