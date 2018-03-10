namespace Bisner.ApiModels.Security.Roles
{
    /// <summary>
    /// Event settings
    /// </summary>
    [BisnerRoles]
    public class Chat
    {
        /// <summary>
        /// Can view chat
        /// </summary>
        [BisnerDefaultRole(Admin = true, User = true, DisplayName = "Chat", Description = "Can view chat")]
        public const string View = "Chat.View";
    }
}