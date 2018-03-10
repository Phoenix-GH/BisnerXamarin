using System;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Central
{
    public class ApiCentralLocationModel
    {
        /// <summary>
        /// Location id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Location image
        /// </summary>
        public ApiImageModel Image { get; set; }

        /// <summary>
        /// Location image
        /// </summary>
        public ApiImageModel Header { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Used in booking
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// About title
        /// </summary>
        public string AboutTitle { get; set; }

        /// <summary>
        /// About text
        /// </summary>
        public string AboutText { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Address2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        public string WifiInformation { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        public string WifiNetwork { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        public string WifiPassword { get; set; }

        /// <summary>
        /// Opening hours
        /// </summary>
        public string OpeningHours { get; set; }

        /// <summary>
        /// Telephone
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Location email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}