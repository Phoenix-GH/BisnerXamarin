using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bisner.ApiModels.Integrations;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication.Models;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IUserApi
    {
        #region User

        [Get("/Api/User/GetPersonalModel")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> GetPersonalModel();
        
        [Get("/Api/User/GetLatestOnlineMembers")]
        Task<ApiResponse<List<ApiWhitelabelPublicUserModel>>> GetLatestOnlineMembers();

        [Get("/Api/User/Get")]
        Task<ApiResponse<ApiWhitelabelPublicUserModel, UserCompaniesModel>> Get(Guid id);

        [Post("/Api/User/UpdateProfileAsync")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> UpdateProfileAsync(ApiUserUpdateModel updateModel, bool disableAllMails = false);

        [Get("/Api/User/UpdateNotificationSettings")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> UpdateNotificationSettings(ApiNotificationSettings settings);

        [Post("/Api/User/EnableCollaboration")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> EnableCollaboration();
        
        [Post("/Api/User/ChangeAvatarAsync")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> ChangeAvatarAsync([AliasAs("data")] StreamPart avatar);

        [Post("/Api/User/ChangeAvatarByUrl")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> ChangeAvatarByUrl(string url);

        [Post("/Api/User/ChangeHeaderAsync")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> ChangeHeaderAsync();

        [Post("/Api/User/ChangePassword")]
        Task<ApiResponse<ApiWhitelabelPrivateUserModel>> ChangePassword(string oldPassword, string newPassword, string newPasswordConfirm);
        
        [Post("/Api/User/ConnectSocial")]
        Task<ApiResponse<ApiSocialLoginResult>> ConnectSocial(string provider, string authToken);

        [Post("/Api/User/DisconnectSocial")]
        Task<ApiResponse<ApiSocialLoginResult>> DisconnectSocial(string provider);
        
        [Post("/Api/User/InviteUser")]
        Task<ApiResponse> InviteUser(string email);

        #endregion User
    }
}