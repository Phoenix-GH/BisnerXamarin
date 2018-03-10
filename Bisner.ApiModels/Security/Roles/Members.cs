namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Event settings
    /// </summary>
    [BisnerRoles]
    public class Members
    {
        /// <summary>
        /// Can access member directory
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Members directory", Description = "Access member directory")]
        public const string View = "Members.View";

        //==================================================================/ Companies /=============================================================//
        /// <summary>
        /// Can view companies
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "CompanyView", Description = "Can view companies")]
        public const string CompanyView = "Members.Company.View";

        /// <summary>
        /// Can create companies
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "CompanyCreate", Description = "Can create a new company")]
        public const string CompanyCreate = "Members.Company.Create";

        /// <summary>
        /// Can update own company
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "CompanyUpdate", Description = "Can update own company")]
        public const string CompanyUpdate = "Members.Company.Update";


        /// <summary>
        /// Can delete own company
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "CompanyDelete", Description = "Can delete own company")]
        public const string CompanyDelete = "Members.Company.Delete";

        /// <summary>
        /// Can manage company team
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "CompanyManageTeam", Description = "Can manage company team")]
        public const string CompanyManageTeam = "Members.Company.ManageTeam";



    }
}