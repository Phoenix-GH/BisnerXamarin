namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Admin panel settings
    /// </summary>
    [BisnerRoles]
    public class Admin
    {
        /// <summary>
        /// Can view the admin
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Admin panel", Description = "Can view the admin panel")]
        public const string View = "Admin.View";

        //==================================================================/ General /=============================================================//

        /// <summary>
        /// Admin general settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "General", Description = "Can change platform general setting", RequiresParent = View)]
        public const string General = "Admin.General";

        //==================================================================/ Members /=============================================================//

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Members", Description = "Can view all members", RequiresParent = View)]
        public const string Members = "Admin.Member";

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Add users", Description = "Can add new users", RequiresParent = View)]
        public const string AddUsers = "Admin.AddUsers";

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Edit users", Description = "Can manage users", RequiresParent = View)]
        public const string EditUsers = "Admin.EditUsers";

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Edit users security", Description = "Can change users security group", RequiresParent = View)]
        public const string EditUsersSecurity = "Admin.EditUsers.Security";

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Edit users export", Description = "Can export users to CSV", RequiresParent = View)]
        public const string EditUsersExport = "Admin.EditUsers.Export";


        //==================================================================/ Branding & Customization /=============================================================//

        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Customization", Description = "Can customize the platform", RequiresParent = View)]
        public const string Customization = "Admin.Customization";

        //==================================================================/ Events /=============================================================//

        /// <summary>
        /// Can manage event settings in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Events", Description = "Can change the platform event settings", RequiresParent = View)]
        public const string Events = "Admin.Events";

       //==================================================================/ Suggestions /=============================================================//

        /// <summary>
        /// Can manage suggestions in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Suggestions", Description = "Can access the suggestions in admin", RequiresParent = View)]
        public const string Suggestions = "Admin.Suggestions";

        //==================================================================/ Security groups /=============================================================//

        /// <summary>
        /// Can manage security groups in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "SecurityGroups", Description = "Can access the securitygroups in admin", RequiresParent = View)]
        public const string SecurityGroups = "Admin.SecurityGroups";

        //==================================================================/ Integrations /=============================================================//

        /// <summary>
        /// Can manage integrations in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Integrations", Description = "Can access the integrations in admin", RequiresParent = View)]
        public const string Integrations = "Admin.Integrations";

        //==================================================================/ Booking /=============================================================//

        /// <summary>
        /// Can manage booking in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Booking", Description = "Can access the booking in admin", RequiresParent = View)]
        public const string Booking = "Admin.Booking";

        //==================================================================/ Digital Memberships /=============================================================//

        /// <summary>
        /// Can manage DigitalMembership in the admin panel
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "DigitalMembership", Description = "Can access the DigitalMembership in admin", RequiresParent = View)]
        public const string DigitalMembership = "Admin.DigitalMembership";





        //==================================================================/ OTHER TO BE PLACED /=============================================================//
        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Digest email", Description = "Can send digest email", RequiresParent = View)]
        public const string DigestEmail = "Admin.DigestEmail";




        /// <summary>
        /// Admin group settings
        /// </summary>
        [BisnerDefaultRole(Admin = true, DisplayName = "Booking delete", Description = "Can remove bookings", RequiresParent = View)]
        public const string BookingDelete = "Admin.Booking.Delete";


    }
}