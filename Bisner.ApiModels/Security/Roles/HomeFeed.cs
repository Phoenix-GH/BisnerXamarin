namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Roles for the home feed
    /// </summary>
    [BisnerRoles]
    public class Home
    {
        /// <summary>
        /// Can view home
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Home", Description = "Can see the home feed")]
        public const string View = "Home.View";

        [BisnerRoles]
        public class Feed
        {
            ///// <summary>
            ///// Can do everything on the homefeed (Create / Delete / Anounce / Comment)
            ///// </summary>
            //[BisnerDefaultRole(Admin = true, RequiresParent = Home.View, DisplayName = "Admin", Description = "Can do everything on the homepage")]
            //public const string Admin = "Home.Feed.Admin";

            /// <summary>
            /// Can create new posts on the home feed
            /// </summary>
            [BisnerDefaultRole(Admin = true, User = true,  RequiresParent = Home.View, DisplayName = "Feed post", Description = "Can post on the home feed")]
            public const string Create = "Home.Feed.Create";

            /// <summary>
            /// Can comment on the homefeed
            /// </summary>
            [BisnerDefaultRole(Admin = true, User = true, RequiresParent = Home.View, DisplayName = "Feed comment", Description = "Can comment on posts")]
            public const string Comment = "Home.Feed.Comment";

            /// <summary>
            /// Can push messages to mobile
            /// </summary>
            [BisnerDefaultRole(Admin = true, RequiresParent = Home.View, DisplayName = "Anounce push", Description = "Anounce feedpost by push")]
            public const string AnouncePush = "Home.Feed.AnouncePush";

        }

    }
}