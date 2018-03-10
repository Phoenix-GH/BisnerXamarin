namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Event settings
    /// </summary>
    [BisnerRoles]
    public class Event
    {
        /// <summary>
        /// Can access event page
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Events", Description = "Can view the events")]
        public const string View = "Event.View";

        /// <summary>
        /// Can create/update/delete all events
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Admin", Description = "Can do everything with events")]
        public const string Admin = "Event.Admin";




        /// <summary>
        /// Can create events
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Create", Description = "Can create events")]
        public const string Create = "Event.Create";

        /// <summary>
        /// Can update created events
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Update", Description = "Can update own events")]
        public const string Update = "Event.Update";

        /// <summary>
        /// Can delete created events
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Delete", Description = "Can delete own events")]
        public const string Delete = "Event.Delete";

        /// <summary>
        /// Can publish events
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Publish", Description = "Can publish own events")]
        public const string Publish = "Event.Publish";

        /// <summary>
        /// Can anounce on the feed
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Anounce feed", Description = "Can anounce event on the home feed")]
        public const string AnounceFeed = "Event.AnounceFeed";

        /// <summary>
        /// Can anounce by email
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Anounce email", Description = "Can anounce event by email")]
        public const string AnounceEmail = "Event.AnounceEmail";

        /// <summary>
        /// Can anounce by push message
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Anounce push", Description = "Can anounce event by push message")]
        public const string AnouncePush = "Event.AnouncePush";

        /// <summary>
        /// Can invite members for an event
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Event invite", Description = "Can invite members for an event")]
        public const string EventInvite = "Event.invite";

        /// <summary>
        /// Can invite members for an event
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Event export", Description = "Can export members from an event")]
        public const string EventExport = "Event.export";


        /// <summary>
        /// Can comment on an event
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Event comment", Description = "Can comment on an event")]
        public const string EventComment = "Event.comment";



    }
}