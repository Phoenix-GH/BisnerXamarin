using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General.User
{
    public interface IUser : IItemBase
    {
        string DisplayName { get; set; }

        IImage Avatar { get; set; }

        Guid ContactId { get; set; }

        UserStatus Status { get; set; }

        DateTime InviteDateTime { get; set; }

        string InviteText { get; set; }

        /// <summary>
        /// User location id
        /// </summary>
        Guid LocationId { get; set; }

        /// <summary>
        /// User last activity
        /// </summary>
        DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// User firstname
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// User is disabled true = disable / false = enabled
        /// </summary>
        bool Disabled { get; set; }

        /// <summary>
        /// User is pending (Not gone trough the welcome flow yet)
        /// </summary>
        bool IsPending { get; set; }

        /// <summary>
        /// Short 1 liner of the user
        /// </summary>
        string ShortAbout { get; set; }

        /// <summary>
        /// User about
        /// </summary>
        string About { get; set; }

        /// <summary>
        /// User city
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// User Linkedin Url
        /// </summary>
        string LinkedInUrl { get; set; }

        /// <summary>
        /// User facebook url
        /// </summary>
        string FacebookUrl { get; set; }

        /// <summary>
        /// User twitter url
        /// </summary>
        string TwitterUrl { get; set; }

        /// <summary>
        /// User google+ url
        /// </summary>
        string GooglePlusUrl { get; set; }

        /// <summary>
        /// User instagram url
        /// </summary>
        string InstagramUrl { get; set; }

        string CompanyName { get; }
        
        /// <summary>
        /// User email -> Only set and shown to admin users null for normal users
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        IImage Header { get; set; }

        /// <summary>
        /// Gets or sets the skills. (Comma seperated)
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        string Skills { get; set; }

        /// <summary>
        /// If set to true this user comes from the central database (Not from a whitelabel database)
        /// This means the user is only in collaboration but not in the whitelabel itself
        /// </summary>
        bool FromCentralDatabase { get; set; }

        /// <summary>
        /// Custom properties in user profile
        /// </summary>
        string CustomField1 { get; set; }

        string CustomField2 { get; set; }

        string CustomField3 { get; set; }

        string CustomField4 { get; set; }

        string CustomField5 { get; set; }

        string CustomField6 { get; set; }

        string CustomField7 { get; set; }

        string CustomField8 { get; set; }

        string CustomField9 { get; set; }

        string CustomField10 { get; set; }

        string CustomField11 { get; set; }

        string CustomField12 { get; set; }

        string CustomField13 { get; set; }

        string CustomField14 { get; set; }

        string CustomField15 { get; set; }

        string CustomField16 { get; set; }

        string CustomField17 { get; set; }

        string CustomField18 { get; set; }

        string CustomField19 { get; set; }

        string CustomField20 { get; set; }

        string CustomField21 { get; set; }

        string CustomField22 { get; set; }

        string CustomField23 { get; set; }

        string CustomField24 { get; set; }

        string CustomField25 { get; set; }

        string CustomField26 { get; set; }

        string CustomField27 { get; set; }

        string CustomField28 { get; set; }

        string CustomField29 { get; set; }

        string CustomField30 { get; set; }
    }
}