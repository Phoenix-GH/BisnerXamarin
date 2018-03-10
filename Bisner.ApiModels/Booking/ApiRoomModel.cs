using System;
using System.Collections.Generic;
using Bisner.ApiModels.General;
using Bisner.ApiModels.Whitelabel;

namespace Bisner.ApiModels.Booking
{
    public class ApiRoomModel
    {
        private List<ApiImageModel> _images;

        /// <summary>
        /// Url for the room ({member}.bisner.com/booking/{locationUrl}/{url}
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Room id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Entity first created
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Entity last updated
        /// </summary>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Amenities comma seperated
        /// </summary>
        public string Amenities { get; set; }

        /// <summary>
        /// Room is published
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// Room location id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Room opening information (Only set if different then parent location)
        /// </summary>
        public OpeningInformation OpeningInformation { get; set; }

        /// <summary>
        /// Room header image
        /// </summary>
        public ApiImageModel Header { get; set; }

        /// <summary>
        /// Room thumbnail image
        /// </summary>
        public ApiImageModel Thumbnail { get; set; }

        /// <summary>
        /// Other images
        /// </summary>
        public List<ApiImageModel> Images
        {
            get { return _images ?? (_images = new List<ApiImageModel>()); }
            set { _images = value; }
        }

        /// <summary>
        /// Room name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Room description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Minimal booking interval (15min / 30min / 1hour / 2 hour / 4 hour)
        /// </summary>
        public RoomTimeUnit TimeUnit { get; set; }

        /// <summary>
        /// Max amount of days the room can be booked into the future (0 = all days)
        /// </summary>
        public int MaxDaysInFuture { get; set; }

        /// <summary>
        /// Room type
        /// </summary>
        public RoomType RoomType { get; set; }


        public int PricePerTimeUnit { get; set; }
        public int PricePerTimeUnitCents { get; set; }

        public int PricePerTimeUnitCommunityMembers { get; set; }
        public int PricePerTimeUnitCommunityMembersCents { get; set; }

        /// <summary>
        /// Vat rate in % (21% / 17.5% etc)
        /// </summary>
        public int VatRate { get; set; }
        public int VatRateCents { get; set; }

        /// <summary>
        /// Amount of square units
        /// </summary>
        public int SquareCount { get; set; }

        /// <summary>
        /// Square type (meters or feet)
        /// </summary>
        public RoomSquareType SquareType { get; set; }

        /// <summary>
        /// Number of guets in a room
        /// </summary>
        public int NumberOfGuests { get; set; }

        public int Order { get; set; }
    }
}