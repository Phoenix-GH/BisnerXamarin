using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Whitelabel
{
    public class ApiWhitelabelCreateCompanyModel
    {
        public string Name { get; set; }

        public Guid? IndustryId { get; set; }

        public Guid? CategoryId { get; set; }

        public string Summary { get; }
    }

    public class ApiWhitelabelCompanyModel
    {
        /// <summary>
        /// Company id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// If true collaboration is enabled for this company
        /// </summary>
        public bool CollaborationEnabled { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Company email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Company description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Year company was founded in
        /// </summary>
        public int Founded { get; set; }

        /// <summary>
        /// Small description
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Adress
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// second address line
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Telephone
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Web url
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Facebook url
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// Instagram url
        /// </summary>
        public string InstagramUrl { get; set; }

        /// <summary>
        /// Twitter url
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// LinkedIn Url
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// Company logo
        /// </summary>
        public ApiImageModel Logo { get; set; }

        /// <summary>
        /// Company header
        /// </summary>
        public ApiImageModel Header { get; set; }

        /// <summary>
        /// Platform location id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Company users
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Users { get; set; } = new List<ApiWhitelabelPublicUserModel>();
      
        /// <summary>
        /// Company users
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> PendingUsers { get; set; } = new List<ApiWhitelabelPublicUserModel>();

        /// <summary>
        /// Company users
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> Guests { get; set; } = new List<ApiWhitelabelPublicUserModel>();

        /// <summary>
        /// Company users
        /// </summary>
        public List<ApiWhitelabelPublicUserModel> PendingGuests { get; set; } = new List<ApiWhitelabelPublicUserModel>();
        /// <summary>
        /// Company user ids
        /// </summary>
        public List<Guid> UserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Company user ids
        /// </summary>
        public List<Guid> GuestUserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Company user ids
        /// </summary>
        public List<Guid> PendingGuestUserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Company admin ids
        /// </summary>
        public List<Guid> AdminIds { get; set; } = new List<Guid>();
       

        /// <summary>
        /// Company industry
        /// </summary>
        public Guid IndustryId { get; set; }

        /// <summary>
        /// Sub industry
        /// </summary>
        public Guid SubIndustryId { get; set; }

        /// <summary>
        /// Company category
        /// </summary>
        public Guid CategoryId { get; set; }
    }
}