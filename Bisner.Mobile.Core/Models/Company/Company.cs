using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Company
{
    public class Company : ItemBase, ICompany, INamed
    {
        #region Constructor

        private bool _collaborationEnabled;
        private string _name;
        private string _description;
        private int _founded;
        private string _summary;
        private string _address;
        private string _address2;
        private string _city;
        private string _country;
        private string _telephone;
        private string _webUrl;
        private string _facebookUrl;
        private string _instagramUrl;
        private string _twitterUrl;
        private string _linkedInUrl;
        private IImage _logo;
        private IImage _header;
        private Guid _locationId;
        private List<IUser> _users;
        private List<Guid> _pendingUsersIds;
        private List<Guid> _guestsIds;
        private List<Guid> _pendingGuests;
        private List<Guid> _userIds;
        private List<Guid> _guestUserIds;
        private List<Guid> _pendingGuestUserIds;
        private List<Guid> _adminIds;
        private Guid _industryId;
        private string _location;
        private string _industry;
        private MvxCommand _showCompanyCommand;

        #endregion Constructor

        #region Properties

        public bool CollaborationEnabled
        {
            get => _collaborationEnabled;
            set { _collaborationEnabled = value; RaisePropertyChanged(() => CollaborationEnabled); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; RaisePropertyChanged(() => Description); }
        }

        public int Founded
        {
            get => _founded;
            set { _founded = value; RaisePropertyChanged(() => Founded); }
        }

        public string Summary
        {
            get => _summary;
            set { _summary = value; RaisePropertyChanged(() => Summary); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; RaisePropertyChanged(() => Address); }
        }

        public string Address2
        {
            get => _address2;
            set { _address2 = value; RaisePropertyChanged(() => Address2); }
        }

        public string City
        {
            get => _city;
            set { _city = value; RaisePropertyChanged(() => City); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; RaisePropertyChanged(() => Country); }
        }

        public string Telephone
        {
            get => _telephone;
            set { _telephone = value; RaisePropertyChanged(() => Telephone); }
        }

        public string WebUrl
        {
            get => _webUrl;
            set { _webUrl = value; RaisePropertyChanged(() => WebUrl); }
        }

        public string FacebookUrl
        {
            get => _facebookUrl;
            set { _facebookUrl = value; RaisePropertyChanged(() => FacebookUrl); }
        }

        public string InstagramUrl
        {
            get => _instagramUrl;
            set { _instagramUrl = value; RaisePropertyChanged(() => InstagramUrl); }
        }

        public string TwitterUrl
        {
            get => _twitterUrl;
            set { _twitterUrl = value; RaisePropertyChanged(() => TwitterUrl); }
        }

        public string LinkedInUrl
        {
            get => _linkedInUrl;
            set { _linkedInUrl = value; RaisePropertyChanged(() => LinkedInUrl); }
        }

        public IImage Logo
        {
            get => _logo;
            set { _logo = value; RaisePropertyChanged(() => Logo); }
        }

        public string LogoUrl => Logo?.Small;

        public MvxCommand ShowCompanyCommand
        {
            get
            {
                return _showCompanyCommand ?? (_showCompanyCommand = new MvxCommand(() =>
                                                                                {
                                                                                    ShowViewModel<FeedViewModel>(new { id = Id, feedType = FeedType.Company });
                                                                                }));
            }
        }

        public IImage Header
        {
            get => _header;
            set { _header = value; RaisePropertyChanged(() => Header); }
        }

        public string HeaderUrl => Header?.Small;

        public Guid LocationId
        {
            get => _locationId;
            set
            {
                _locationId = value;
                RaisePropertyChanged(() => LocationId);
            }
        }

        public List<IUser> Users
        {
            get => _users ?? (_users = new List<IUser>());
            set { _users = value; RaisePropertyChanged(() => Users); }
        }

        public List<Guid> PendingUsersIds
        {
            get => _pendingUsersIds ?? (_pendingUsersIds = new List<Guid>());
            set { _pendingUsersIds = value; RaisePropertyChanged(() => PendingUsersIds); }
        }

        public List<Guid> GuestsIds
        {
            get => _guestsIds ?? (_guestsIds = new List<Guid>());
            set { _guestsIds = value; RaisePropertyChanged(() => GuestsIds); }
        }

        public List<Guid> PendingGuests
        {
            get => _pendingGuests ?? (_pendingGuests = new List<Guid>());
            set { _pendingGuests = value; RaisePropertyChanged(() => PendingGuests); }
        }

        public List<Guid> UserIds
        {
            get => _userIds ?? (_userIds = new List<Guid>());
            set { _userIds = value; RaisePropertyChanged(() => UserIds); }
        }

        public List<Guid> GuestUserIds
        {
            get => _guestUserIds ?? (_guestUserIds = new List<Guid>());
            set { _guestUserIds = value; RaisePropertyChanged(() => GuestUserIds); }
        }

        public List<Guid> PendingGuestUserIds
        {
            get => _pendingGuestUserIds ?? (_pendingGuestUserIds = new List<Guid>());
            set { _pendingGuestUserIds = value; RaisePropertyChanged(() => PendingGuestUserIds); }
        }

        public List<Guid> AdminIds
        {
            get => _adminIds;
            set { _adminIds = value; RaisePropertyChanged(() => AdminIds); }
        }

        public Guid IndustryId
        {
            get => _industryId;
            set { _industryId = value; RaisePropertyChanged(() => IndustryId); }
        }

        public string Industry
        {
            get => _industry;
            set { _industry = value; RaisePropertyChanged(() => Industry); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        #endregion Properties

        #region Feed

        public DateTime DateTime { get; set; }

        #endregion Feed
    }
}