//namespace Bisner.ApiModels.Security.Roles
//{
//    /// <summary>
//    /// Company settings
//    /// </summary>
//    [BisnerRoles]
//    public class Company
//    {
//        /// <summary>
//        /// Can view all (public) companies
//        /// </summary>
//        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Companies", Description = "Can view the companies")]
//        public const string View = "Company.View";

//        /// <summary>
//        /// Can create/update/delete all companies
//        /// </summary>
//        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Admin", Description = "Full rights on companies")]
//        public const string Admin = "Company.Admin";

//        /// <summary>
//        /// Can create companies
//        /// </summary>
//        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Create", Description = "Can create companies")]
//        public const string Create = "Company.Create";
        
//        /// <summary>
//        /// Can update created companies
//        /// </summary>
//        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Update", Description = "Can update own companies")]
//        public const string Update = "Company.Update";

//        /// <summary>
//        /// Can delete created companies
//        /// </summary>
//        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Delete", Description = "Can delete own companies")]
//        public const string Delete = "Company.Delete";
//    }
//}