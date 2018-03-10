using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;

namespace Bisner.Mobile.Core.Models.General
{
    public class Platform : ItemBase, IPlatform
    {
        /// <summary>
        /// Platform created on
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Platform locations
        /// </summary>
        public List<ILocation> Locations { get; set; }

        /// <summary>
        /// Platform event categories
        /// </summary>
        public List<IEventCategory> EventCategories { get; set; }

        /// <summary>
        /// Platform name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Basic or pro
        /// </summary>
        public bool IsPro { get; set; }

        /// <summary>
        /// Allow all users to chat with eachother or only contacts
        /// </summary>
        public bool AllowAllUsersToChat { get; set; }

        /// <summary>
        /// Allow collaboration on the whitelabel
        /// </summary>
        public bool AllowCollaboration { get; set; }

        /// <summary>
        /// Custom logo
        /// </summary>
        public IImage Logo { get; set; }

        /// <summary>
        /// Welcome flow / login page image 1
        /// </summary>
        public IImage WelcomeImage1 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 2
        /// </summary>
        public IImage WelcomeImage2 { get; set; }

        /// <summary>
        /// Welcome flow / login page image 3
        /// </summary>
        public IImage WelcomeImage3 { get; set; }

        /// <summary>
        /// Members index header
        /// </summary>
        public IImage MembersHeader { get; set; }

        /// <summary>
        /// Events index header
        /// </summary>
        public IImage EventsHeader { get; set; }

        /// <summary>
        /// Groups index header
        /// </summary>
        public IImage GroupsHeader { get; set; }

        /// <summary>
        /// Location index header
        /// </summary>
        public IImage LocationHeader { get; set; }


        /// <summary>
        /// Default images
        /// </summary>
        public string UserDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Avatars/default_user_avatar.png"; }
        }
        public string CompanyHeaderDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/community_header_002.png"; }
        }
        public string MembersHeaderDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/community_header_001.png"; }
        }
        public string EventsHeaderDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/group_header_006.png"; }
        }
        public string GroupsHeaderDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/group_header_001.png"; }
        }
        public string LocationHeaderDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/group_header_001.png"; }
        }

        public string EventFeedDefault
        {
            get { return "/Content/Dashboard/Assets/Img/Temp/temp_header_event2.png"; }
        }
    }
}