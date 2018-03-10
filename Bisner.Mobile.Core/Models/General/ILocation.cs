using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General
{
    public interface ILocation : IItemBase
    {
        /// <summary>
        /// Location image
        /// </summary>
        IImage Image { get; set; }

        /// <summary>
        /// Location image
        /// </summary>
        IImage Header { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// About title
        /// </summary>
        string AboutTitle { get; set; }

        /// <summary>
        /// About text
        /// </summary>
        string AboutText { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Address2
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        string WifiInformation { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        string WifiNetwork { get; set; }

        /// <summary>
        /// Wifi information
        /// </summary>
        string WifiPassword { get; set; }

        /// <summary>
        /// Opening hours
        /// </summary>
        string OpeningHours { get; set; }

        /// <summary>
        /// Telephone
        /// </summary>
        string Telephone { get; set; }

        /// <summary>
        /// Location email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        string Country { get; set; }
    }
}