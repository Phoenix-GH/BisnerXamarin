namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Booking settings
    /// </summary>
    [BisnerRoles]
    public class Booking
    {
        /// <summary>
        /// Can access booking
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Booking", Description = "Can access booking")]
        public const string View = "Booking.View";


        /// <summary>
        /// Can create/update/delete all bookings
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Admin", Description = "Can do everything with bookings")]
        public const string Admin = "Booking.Admin";

        /// <summary>
        /// Can create bookings
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Create", Description = "Can book a room")]
        public const string Create = "Booking.Create";

        /// <summary>
        /// Can view who booked a room
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "ViewWho", Description = "Can view who booked a room")]
        public const string ViewWho = "Booking.viewwho";



    }
}