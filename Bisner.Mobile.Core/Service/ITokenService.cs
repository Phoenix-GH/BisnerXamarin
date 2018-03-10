using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface ITokenService
    {
        /// <summary>
        /// Get the token by supplying the username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AuthenticationResult> GetTokenAsync(string username, string password);

        /// <summary>
        /// Gets the token and refreshes it when it has expired
        /// </summary>
        /// <returns></returns>
        Task<string> GetTokenAsync();

        /// <summary>
        /// Refresh the token using the given refreshtoken
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationResult> RefreshTokenAsync();
    }
}