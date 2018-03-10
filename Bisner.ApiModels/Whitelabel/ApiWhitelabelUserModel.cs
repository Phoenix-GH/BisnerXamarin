using System;
using System.Collections.Generic;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.General;
using Bisner.Constants;

namespace Bisner.ApiModels.Whitelabel
{
    [Flags]
    public enum UserRoles
    {
        //Use None as the name of the flag enumerated constant whose value is zero. 
        //You cannot use the None enumerated constant in a bitwise AND operation to 
        //test for a flag because the result is always zero. However, you can 
        //perform a logical, not a bitwise, comparison between the numeric value and 
        //the None enumerated constant to determine whether any bits in the numeric 
        //value are set.

        /// <summary>
        /// Anonymous user
        /// </summary>
        None = 0,

        /// <summary>
        /// Registered but guest user
        /// </summary>
        Guest = 1 << 0, // 1

        /// <summary>
        /// Registered and community user
        /// </summary>
        CommunityUser = 1 << 1, // 2

        /// <summary>
        /// Registered and admin
        /// </summary>
        Admin = 1 << 2, // 4

        /// <summary>
        /// Digital member needs to have an active payment plan to login
        /// </summary>
        DigitalMember = 1 << 3, // 8
    }
    
    public enum Gender
    {
        Male = 0,
        Female = 1,
    }
    
    public class ApiUserUpdateModel
    {
        public ApiUserUpdateModel()
        {

        }

        public ApiUserUpdateModel(ApiWhitelabelPrivateUserModel model)
        {
            Id = model.Id;
            LocationId = model.LocationId;
            LanguageId = model.LanguageId;
            FirstName = model.FirstName;
            MiddleName = model.MiddleName;
            LastName = model.LastName;
            NickName = model.DisplayName;
            Gender = model.Gender;
            ShortAbout = model.ShortAbout;
            About = model.About;
            City = model.City;
            Address1 = model.Address1;
            Address2 = model.Address2;
            PostalCode = model.PostalCode;
            State = model.State;
            CountryCode = model.CountryCode;
            PhoneNumber = model.PhoneNumber;

            LinkedInUrl = model.LinkedInUrl;
            FacebookUrl = model.FacebookUrl;
            TwitterUrl = model.TwitterUrl;
            GooglePlusUrl = model.GooglePlusUrl;
            InstagramUrl = model.InstagramUrl;
            WebsiteUrl = model.WebsiteUrl;
            Skills = model.Skills;

            CustomField1 = model.CustomField1;
            CustomField2 = model.CustomField2;
            CustomField3 = model.CustomField3;
            CustomField4 = model.CustomField4;
            CustomField5 = model.CustomField5;
            CustomField6 = model.CustomField6;
            CustomField7 = model.CustomField7;
            CustomField8 = model.CustomField8;
            CustomField9 = model.CustomField9;

            CustomField10 = model.CustomField10;
            CustomField11 = model.CustomField11;
            CustomField12 = model.CustomField12;
            CustomField13 = model.CustomField13;
            CustomField14 = model.CustomField14;
            CustomField15 = model.CustomField15;
            CustomField16 = model.CustomField16;
            CustomField17 = model.CustomField17;
            CustomField18 = model.CustomField18;
            CustomField19 = model.CustomField19;

            CustomField20 = model.CustomField20;
            CustomField21 = model.CustomField21;
            CustomField22 = model.CustomField22;
            CustomField23 = model.CustomField23;
            CustomField24 = model.CustomField24;
            CustomField25 = model.CustomField25;
            CustomField26 = model.CustomField26;
            CustomField27 = model.CustomField27;
            CustomField28 = model.CustomField28;
            CustomField29 = model.CustomField29;

            CustomField30 = model.CustomField30;

        }

        /// <summary>
        /// User id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// User location id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// User language setting
        /// </summary>
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// User firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nickname
        /// </summary>
        public string NickName { get; set; }

        public Gender Gender { get; set; }

        /// <summary>
        /// Short 1 liner of the user
        /// </summary>
        public string ShortAbout { get; set; }

        /// <summary>
        /// User about
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// User city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address line 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Address line 2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 2 letter country code 
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User Linkedin Url
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// User facebook url
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// User twitter url
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// User google+ url
        /// </summary>
        public string GooglePlusUrl { get; set; }

        /// <summary>
        /// User instagram url
        /// </summary>
        public string InstagramUrl { get; set; }


        /// <summary>
        /// User website url
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the skills. (Comma seperated)
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public string Skills { get; set; }

        /// <summary>
        /// Custom properties in user profile
        /// </summary>

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

    public class ApiUserMailNotificationSettings
    {
        #region Community & Collaborate

        /// <summary>
        /// User can recieve a daily digest mail
        /// </summary>
        public bool DailyDigestMails { get; set; }

        /// <summary>
        /// User can recieve a weekly digest mail
        /// </summary>
        public bool WeeklyDigestMails { get; set; }

        /// <summary>
        /// User can recieve mention mails (send immediatly)
        /// </summary>
        public bool MentionMails { get; set; }

        /// <summary>
        /// Contact invite mails
        /// </summary>
        public bool ContactInvites { get; set; }

        /// <summary>
        /// User can recieve workspace invite mails (send immediatly)
        /// </summary>
        public bool WorkspaceInviteMails { get; set; }

        /// <summary>
        /// Usre can recieve mails when assigned to file / note / task
        /// </summary>
        public bool AssignedMails { get; set; }

        #endregion Community & Collaborate

        #region Admin

        /// <summary>
        /// Administrators can disable all admin emails
        /// </summary>
        public bool AdminEmails { get; set; }

        #endregion Admin
    }

    public enum EmailTypes
    {
        DailyDigest,
        NewFeedPost,
        NewFeedPosComment,
        Mention,
        Assigned,
        AdminMails
    }

    public class ApiNotificationSettings
    {
        public bool CanSendNotification(EmailTypes type, ApiWhitelabelPublicUserModel userModel)
        {
            // Disabled users dont recieve emails
            if (userModel.Disabled)
            {
                return false;
            }

            // Pending members dont recieve emails
            if (userModel.IsPending)
            {
                return false;
            }

            // use platform settings
            if (OverwriteDefault == false)
            {
                return true;
            }

            // all mails disabled
            if (DisableAllMails)
            {
                return false;
            }

            if (type == EmailTypes.DailyDigest && DisableDailyDigest)
            {
                return false;
            }

            if (type == EmailTypes.NewFeedPosComment && DisableNewFeedPostComment)
            {
                return false;
            }

            if (type == EmailTypes.NewFeedPost && DisableNewFeedPost)
            {
                return false;
            }

            if (type == EmailTypes.Assigned && DisableAssigned)
            {
                return false;
            }

            if (type == EmailTypes.Mention && DisableMention)
            {
                return false;
            }

            if (type == EmailTypes.AdminMails && DisableAdminMails)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// If set to true use settings in this platform else use platform settings
        /// </summary>
        public bool OverwriteDefault { get; set; }

        /// <summary>
        /// Disable all mails from bisner
        /// </summary>
        public bool DisableAllMails { get; set; }

        /// <summary>
        /// Recieve daily digest (triggered from admin)
        /// </summary>
        public bool DisableDailyDigest { get; set; }

        /// <summary>
        /// Recieve mail on new feed post
        /// </summary>
        public bool DisableNewFeedPost { get; set; }

        /// <summary>
        /// Recieve mail on new feed post comment
        /// </summary>
        public bool DisableNewFeedPostComment { get; set; }

        /// <summary>
        /// Recieve mail on mention
        /// </summary>
        public bool DisableMention { get; set; }

        /// <summary>
        /// Recieve mail if assigned
        /// </summary>
        public bool DisableAssigned { get; set; }

        /// <summary>
        /// Disable all mails user recieves as admin
        /// </summary>
        public bool DisableAdminMails { get; set; }
    }
    
    public class ApiWhitelabelPublicUserModel
    {
        private ApiNotificationSettings _notificationSettings;

        /// <summary>
        /// User notification settings -> Only set and shown to admin users, null for normal users
        /// </summary>
        public ApiNotificationSettings NotificationSettings
        {
            get { return _notificationSettings ?? (_notificationSettings = new ApiNotificationSettings()); }
            set { _notificationSettings = value; }
        }

        /// <summary>
        /// User id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User email -> Only set and shown to admin users null for normal users
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// User location id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// User avatar
        /// </summary>
        public ApiImageModel Avatar { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public ApiImageModel Header { get; set; }

        /// <summary>
        /// User last activity
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// User firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Displayname
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Male of Female
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the skills. (Comma seperated)
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public string Skills { get; set; }

        /// <summary>
        /// User is disabled true = disable / false = enabled
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// User is pending (Not gone trough the welcome flow yet)
        /// </summary>
        public bool IsPending { get; set; }

        /// <summary>
        /// Short 1 liner of the user
        /// </summary>
        public string ShortAbout { get; set; }

        /// <summary>
        /// User about
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// User city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// User Linkedin Url
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// User facebook url
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// User twitter url
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// User google+ url
        /// </summary>
        public string GooglePlusUrl { get; set; }

        /// <summary>
        /// User instagram url
        /// </summary>
        public string InstagramUrl { get; set; }

        /// <summary>
        /// User website url
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// If set to true this user comes from the central database (Not from a whitelabel database)
        /// This means the user is only in collaboration but not in the whitelabel itself
        /// </summary>
        public bool FromCentralDatabase { get; set; }


        public bool FacebookConnected { get; set; }
        public bool GoogleConnected { get; set; }
        public bool LinkedInConnected { get; set; }

        /// <summary>
        /// Custom properties in user profile
        /// </summary>

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

        /// <summary>
        /// User is approved by an admin
        /// </summary>
        public bool IsApproved { get; set; }
    }

    public class ApiWhitelabelPrivateUserModel : ApiWhitelabelPublicUserModel
    {
        private List<Guid> _companyIds;
        private List<Guid> _guestCompanyIds;
        private List<ApiWhitelabelNotificationModel> _notifications;
        private List<ApiWhitelabelContactModel> _contacts;
        private ApiUserMailNotificationSettings _mailNotificationSettings;

        /// <summary>
        /// True of avatar is generated by bisner
        /// </summary>
        public bool HasSystemAvatar { get; set; }

        /// <summary>
        /// Address line 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Address line 2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 2 letter country code 
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public UserRoles UserRole { get; set; }

        /// <summary>
        /// User language setting
        /// </summary>
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// If true user has enabled collaboration sync
        /// </summary>
        public bool CollaborationEnabled { get; set; }

        /// <summary>
        /// If digital membership is enabled this will indicate the user payment plan
        /// </summary>
        public Guid DigitalMembershipActivePaymentPlan { get; set; }

        /// <summary>
        /// Digital membership plan is valid until -> Renewal needed after this
        /// </summary>
        public DateTime? DigitalMembershipValidUntil { get; set; }

        /// <summary>
        /// Facebook connected
        /// </summary>
        public bool FacebookConnected { get; set; }

        /// <summary>
        /// Google connected
        /// </summary>
        public bool GoogleConnected { get; set; }

        /// <summary>
        /// User company Ids
        /// </summary>
        public List<Guid> CompanyIds
        {
            get { return _companyIds ?? (_companyIds = new List<Guid>()); }
            set { _companyIds = value; }
        }

        /// <summary>
        /// User company Ids
        /// </summary>
        public List<Guid> GuestCompanyIds
        {
            get { return _guestCompanyIds ?? (_guestCompanyIds = new List<Guid>()); }
            set { _guestCompanyIds = value; }
        }

        /// <summary>
        /// User notifications
        /// </summary>
        public List<ApiWhitelabelNotificationModel> Notifications
        {
            get { return _notifications ?? (_notifications = new List<ApiWhitelabelNotificationModel>()); }
            set { _notifications = value; }
        }

        /// <summary>
        /// User contacts
        /// </summary>
        public List<ApiWhitelabelContactModel> Contacts
        {
            get { return _contacts ?? (_contacts = new List<ApiWhitelabelContactModel>()); }
            set { _contacts = value; }
        }

        /// <summary>
        /// User mail & notification settings
        /// </summary>
        public ApiUserMailNotificationSettings MailNotificationSettings
        {
            get { return _mailNotificationSettings ?? (_mailNotificationSettings = new ApiUserMailNotificationSettings()); }
            set { _mailNotificationSettings = value; }
        }

        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// True if this user has been invited to the platform by an admin
        /// </summary>
        public bool IsInvited { get; set; }

        /// <summary>
        /// True if this user has registered on the platform
        /// </summary>
        public bool IsRegistered { get; set; }
    }

    public class ApiWhitelabelContactModel
    {
        /// <summary>
        /// Unique id for this contact relation
        /// </summary>
        public Guid UniqueContactId { get; set; }

        /// <summary>
        /// Usermodel of contact
        /// </summary>
        public ApiWhitelabelPublicUserModel User { get; set; }

        /// <summary>
        /// Contact status
        /// </summary>
        public ContactStatus Status { get; set; }

        /// <summary>
        /// Invite datetime
        /// </summary>
        public DateTime InviteDateTime { get; set; }

        /// <summary>
        /// Invite text
        /// </summary>
        public string InviteText { get; set; }
    }
}