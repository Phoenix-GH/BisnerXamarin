namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Event settings
    /// </summary>
    [BisnerRoles]
    public class Collaborate
    {
        /// <summary>
        /// Can view collaborate
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Collaborate", Description = "Can view collaborate")]
        public const string View = "Collaborate.View";



        //==================================================================/ OTHER TO BE PLACED /=============================================================//



    }
}