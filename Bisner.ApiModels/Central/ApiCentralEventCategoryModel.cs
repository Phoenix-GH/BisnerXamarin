using System;
using Bisner.ApiModels.General;

namespace Bisner.ApiModels.Central
{
    public enum ApiCentralEventCategoryType
    {
        Event = 1,
        Meetup = 2
    }

    public class ApiCentralEventCategoryModel
    {
        /// <summary>
        /// Category id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Seo friendly url
        /// </summary>
        public string Url => Id.ToString("N");

        /// <summary>
        /// Event type
        /// </summary>
        public ApiCentralEventCategoryType Type { get; set; }

        /// <summary>
        /// If true all users can create this type of event
        /// </summary>
        public bool UsersCanCreateEvent { get; set; }

        /// <summary>
        /// Category image
        /// </summary>
        public ApiImageModel Image { get; set; }

        /// <summary>
        /// Category image header
        /// </summary>
        public ApiImageModel Header { get; set; }


        /// <summary>
        /// Category description
        /// </summary>
        public string Description { get; set; }

    }
}