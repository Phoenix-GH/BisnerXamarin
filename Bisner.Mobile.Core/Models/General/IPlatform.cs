using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;

namespace Bisner.Mobile.Core.Models.General
{
    public interface IPlatform : IItemBase
    {
        /// <summary>
        /// Platform created on
        /// </summary>
        DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Platform locations
        /// </summary>
        List<ILocation> Locations { get; set; }
        /// <summary>
        /// Platform event categories
        /// </summary>
        List<IEventCategory> EventCategories { get; set; }

        /// <summary>
        /// Platform name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Basic or pro
        /// </summary>
        bool IsPro { get; set; }

        /// <summary>
        /// Allow all users to chat with eachother or only contacts
        /// </summary>
        bool AllowAllUsersToChat { get; set; }

        /// <summary>
        /// Allow collaboration on the whitelabel
        /// </summary>
        bool AllowCollaboration { get; set; }

        /// <summary>
        /// Custom logo
        /// </summary>
        IImage Logo { get; set; }

        /// <summary>
        /// Welcome flow / login page image 1
        /// </summary>
        IImage WelcomeImage1 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 2
        /// </summary>
        IImage WelcomeImage2 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 3
        /// </summary>
        IImage WelcomeImage3 { get; set; }

        /// <summary>
        /// Members index header
        /// </summary>
        IImage MembersHeader { get; set; }

        /// <summary>
        /// Events index header
        /// </summary>
        IImage EventsHeader { get; set; }

        /// <summary>
        /// Groups index header
        /// </summary>
        IImage GroupsHeader { get; set; }

        /// <summary>
        /// Location index header
        /// </summary>
        IImage LocationHeader { get; set; }

        /// <summary>
        /// Default images
        /// </summary>
        string UserDefault { get; }

        string CompanyHeaderDefault { get; }
        string MembersHeaderDefault { get; }
        string EventsHeaderDefault { get; }
        string GroupsHeaderDefault { get; }
        string LocationHeaderDefault { get; }
        string EventFeedDefault { get; }
    }
}