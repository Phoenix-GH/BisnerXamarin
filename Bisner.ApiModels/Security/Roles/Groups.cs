namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Group settings
    /// </summary>
    [BisnerRoles]
    public class Group
    {
        /// <summary>
        /// Can access group page
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Groups", Description = "Access groups")]
        public const string View = "Group.View";



        //==================================================================/ OTHER TO BE PLACED /=============================================================//


        /// <summary>
        /// Can create/update/delete all groups
        /// </summary>
        [BisnerDefaultRole(Admin = true, RequiresParent = View, DisplayName = "Admin", Description = "Can do everything with groups")]
        public const string Admin = "Group.Admin";

        /// <summary>
        /// Can create groups
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Create", Description = "Can create groups")]
        public const string Create = "Group.Create";

        /// <summary>
        /// Can create groups
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Create subgroup", Description = "Can create groups subgroups")]
        public const string CreateSubGroup = "Group.CreateSubGroup";

        /// <summary>
        /// Can update created groups
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Update", Description = "Can update own groups")]
        public const string Update = "Group.Update";

        /// <summary>
        /// Can delete groups
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, RequiresParent = View, DisplayName = "Delete", Description = "Can delete own groups")]
        public const string Delete = "Group.Delete";


    }
}