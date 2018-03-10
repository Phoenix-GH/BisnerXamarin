using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General.User
{
    public enum UserStatus
    {
        NoContact = 0,
        New = 1,
        Accepted = 2,
        Ignored = 4,
        Pending = 8
    }

    public class User : ItemBase, IUser
    {
        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public IImage Avatar { get; set; }

        public string AvatarUrl => Avatar?.Small;

        public Guid ContactId { get; set; }

        public UserStatus Status { get; set; }

        public DateTime InviteDateTime { get; set; }

        public string InviteText { get; set; }
        public Guid LocationId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Disabled { get; set; }
        public bool IsPending { get; set; }
        public string ShortAbout { get; set; }
        public string About { get; set; }
        public string City { get; set; }
        public string LinkedInUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string GooglePlusUrl { get; set; }
        public string InstagramUrl { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }
        public IImage Header { get; set; }
        public string Skills { get; set; }
        public bool FromCentralDatabase { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public string CustomField6 { get; set; }
        public string CustomField7 { get; set; }
        public string CustomField8 { get; set; }
        public string CustomField9 { get; set; }
        public string CustomField10 { get; set; }
        public string CustomField11 { get; set; }
        public string CustomField12 { get; set; }
        public string CustomField13 { get; set; }
        public string CustomField14 { get; set; }
        public string CustomField15 { get; set; }
        public string CustomField16 { get; set; }
        public string CustomField17 { get; set; }
        public string CustomField18 { get; set; }
        public string CustomField19 { get; set; }
        public string CustomField20 { get; set; }
        public string CustomField21 { get; set; }
        public string CustomField22 { get; set; }
        public string CustomField23 { get; set; }
        public string CustomField24 { get; set; }
        public string CustomField25 { get; set; }
        public string CustomField26 { get; set; }
        public string CustomField27 { get; set; }
        public string CustomField28 { get; set; }
        public string CustomField29 { get; set; }
        public string CustomField30 { get; set; }
    }
}
