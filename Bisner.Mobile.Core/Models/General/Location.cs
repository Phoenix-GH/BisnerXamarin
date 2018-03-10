using System;

namespace Bisner.Mobile.Core.Models.General
{
    public class Location :  ILocation
    {
        public Guid Id { get; set; }

        public IImage Image { get; set; }

        public IImage Header { get; set; }

        public string Name { get; set; }

        public string AboutTitle { get; set; }

        public string AboutText { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string WifiInformation { get; set; }

        public string WifiNetwork { get; set; }

        public string WifiPassword { get; set; }

        public string OpeningHours { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }
    }
}