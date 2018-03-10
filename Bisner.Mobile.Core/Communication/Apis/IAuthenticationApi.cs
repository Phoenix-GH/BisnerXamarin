using System;
using System.Threading.Tasks;
using Bisner.ApiModels.Integrations;
using Bisner.ApiModels.Whitelabel;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Accept-Encoding: gzip, deflate")]
    public interface IAuthenticationApi
    {
        [Post("/Api/User/Login")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> Login(string username, string password);

        [Post("/Api/User/RequestToken")]
        Task<ApiResponse<string>> RequestToken(string username, string password);

        [Post("/Api/User/RequestJWT")]
        Task<ApiResponse<JwtResponse>> RequestJwt(Guid appId, string username, string password);

        [Post("/Api/User/RefreshJWT")]
        Task<ApiResponse<JwtResponse>> RefreshJwt(Guid appId, string token);

        [Get("/Api/User/SingleSignOn")]
        Task<ApiResponse<string>> SingleSignOn([Header("AppKey")]string appKey, [Header("AppId")]string appId, string username);

        [Post("/Api/User/ConnectSocial")]
        Task<ApiResponse<ApiSocialLoginResult>> ConnectSocial(string provider, string authToken);

        [Post("/Api/User/DisconnectSocial")]
        Task<ApiResponse<ApiSocialLoginResult>> DisconnectSocial(string provider);

        [Post("/Api/User/SocialLogin")]
        Task<ApiResponse<ApiSocialLoginResult>> SocialLogin(string provider, string authToken);

        [Post("/Api/User/ChecEmailAvailable")]
        Task<ApiResponse> CheckEmailAvailable(string email);
    }
}