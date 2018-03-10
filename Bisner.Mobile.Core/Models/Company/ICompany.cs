using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.Core.Models.General.User;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Company
{
    public interface ICompany : IItemBase
    {
        /// <summary>
        /// If true collaboration is enabled for this company
        /// </summary>
        bool CollaborationEnabled { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Company description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Year company was founded in
        /// </summary>
        int Founded { get; set; }

        /// <summary>
        /// Small description
        /// </summary>
        string Summary { get; set; }

        /// <summary>
        /// Adress
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// second address line
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Telephone
        /// </summary>
        string Telephone { get; set; }

        /// <summary>
        /// Web url
        /// </summary>
        string WebUrl { get; set; }

        /// <summary>
        /// Facebook url
        /// </summary>
        string FacebookUrl { get; set; }

        /// <summary>
        /// Instagram url
        /// </summary>
        string InstagramUrl { get; set; }

        /// <summary>
        /// Twitter url
        /// </summary>
        string TwitterUrl { get; set; }

        /// <summary>
        /// LinkedIn Url
        /// </summary>
        string LinkedInUrl { get; set; }

        /// <summary>
        /// Company logo
        /// </summary>
        IImage Logo { get; set; }

        /// <summary>
        /// Company header
        /// </summary>
        IImage Header { get; set; }

        /// <summary>
        /// Platform location id
        /// </summary>
        Guid LocationId { get; set; }

        /// <summary>
        /// The name of the location
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// Company users
        /// </summary>
        List<IUser> Users { get; set; }

        /// <summary>
        /// Company users
        /// </summary>
        List<Guid> PendingUsersIds { get; set; }

        /// <summary>
        /// Company users
        /// </summary>
        List<Guid> GuestsIds { get; set; }

        /// <summary>
        /// Company users
        /// </summary>
        List<Guid> PendingGuests { get; set; }

        /// <summary>
        /// Company user ids
        /// </summary>
        List<Guid> UserIds { get; set; }

        /// <summary>
        /// Company user ids
        /// </summary>
        List<Guid> GuestUserIds { get; set; }

        /// <summary>
        /// Company user ids
        /// </summary>
        List<Guid> PendingGuestUserIds { get; set; }

        /// <summary>
        /// Company admin ids
        /// </summary>
        List<Guid> AdminIds { get; set; }

        /// <summary>
        /// Company industry
        /// </summary>
        Guid IndustryId { get; set; }

        /// <summary>
        /// The name of the industry
        /// </summary>
        string Industry { get; set; }

        string LogoUrl { get; }
        MvxCommand ShowCompanyCommand { get; }
    }
}
