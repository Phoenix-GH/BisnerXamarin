namespace Bisner.Constants
{
    /// <summary>
    /// Error codes for api responses, 0 = no errors
    /// </summary>
    public class ApiErrorCodes
    {
        /// <summary>
        /// Error if all other options failed
        /// </summary>
        public const int GeneralError = int.MaxValue;

        public const int InvalidLogin = 1;

        public const int NoAccess = 403;


        /***************************************************************/
        /*                          COMMUNITY                         */
        /***************************************************************/

        /// <summary>
        /// Group name is allready in use when trying to create or edit a group name
        /// </summary>
        public const int WhitelabelGroupNameInUse = 2;

        /// <summary>
        /// Email is allready registered
        /// </summary>
        public const int WhitelabelEmailAllreadyInUse = 3;

        public const int NotFound = 4;

        public const int ChatNotAllowed = 5;

        /// <summary>
        /// URL allready in use
        /// </summary>
        public const int UrlInUse = 6;

        /// <summary>
        /// Email required error
        /// </summary>
        public const int EmailRequired = 7;

        /// <summary>
        /// Password required
        /// </summary>
        public const int PasswordRequired = 8;

        /// <summary>
        /// Invalid user name / password on login
        /// </summary>
        public const int InvalidUserNameOrPassword = 9;

        /// <summary>
        /// Booked room allready in use (reserved)
        /// </summary>
        public const int RoomInUse = 10;
    }
}