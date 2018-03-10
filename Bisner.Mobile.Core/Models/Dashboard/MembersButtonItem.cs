using System;
using System.Linq;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Service;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Dashboard
{
    public class MembersButtonItem : MvxNotifyPropertyChanged, IItemBase
    {
        private MvxCommand _companyCommand;
        private MvxCommand _membersCommand;
        private bool _showCompanies;
        private bool _showMembers;
        private string _companyText;
        private string _membersText;

        public MembersButtonItem(string companyText, string membersText)
        {
            ShowCompanies = true;
            ShowMembers = false;
            CompanyText = companyText;
            MembersText = membersText;
        }

        public Guid Id { get; set; }

        public Action CompanyAction { get; set; }

        public Action MembersAction { get; set; }

        public string CompanyText
        {
            get => _companyText;
            private set { _companyText = value; RaisePropertyChanged(() => CompanyText); }
        }

        public string MembersText
        {
            get => _membersText;
            private set { _membersText = value; RaisePropertyChanged(() => MembersText); }
        }

        public MvxCommand CompanyCommand => _companyCommand ?? (_companyCommand = new MvxCommand(Companies));

        public MvxCommand MembersCommand => _membersCommand ?? (_membersCommand = new MvxCommand(Members));

        public bool ShowCompanies
        {
            get => _showCompanies;
            private set { _showCompanies = value; RaisePropertyChanged(() => ShowCompanies); }
        }

        public bool ShowMembers
        {
            get => _showMembers;
            private set { _showMembers = value; RaisePropertyChanged(() => ShowMembers); }
        }

        public bool CompanyButtonEnabled => Settings.UserRoles.Any(r => r == ApiModels.Security.Roles.Members.CompanyView.ToLower());

        private void Companies()
        {
            ShowCompanies = true;
            ShowMembers = false;

            CompanyAction?.Invoke();
        }

        private void Members()
        {
            ShowCompanies = false;
            ShowMembers = true;

            MembersAction?.Invoke();
        }
    }

    public class MemberSearchItem : IItemBase
    {
        #region Constructor

        private string _searchText;

        #endregion Constructor

        #region Search

        public Guid Id { get; set; }

        public string PlaceholderText { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                Search(value);

                _searchText = value;
            }
        }

        public Action<string> SearchAction { get; set; }

        private void Search(string input)
        {
            SearchAction?.Invoke(input);
        }

        #endregion Search
    }
}