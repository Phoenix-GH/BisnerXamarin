using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IUserService
    {
        Task<ApiWhitelabelPrivateUserModel> GetPersonalModelAsync(ApiPriority priority);
        Task<ApiWhitelabelPrivateUserModel> UpdateProfileAsync(ApiUserUpdateModel userUpdateModel);
        Task<ApiWhitelabelPrivateUserModel> UpdateNotificationSettingsAsync(ApiNotificationSettings notificationSettings);
        Task<ApiWhitelabelPublicUserModel> GetUserAsync(Guid userId, ApiPriority priority);
        Task<List<ApiWhitelabelPublicUserModel>> GetAllPublicUsersAsync(ApiPriority priority);
        Task<List<ApiWhitelabelPublicUserModel>> SearchMembersAsync(string input);
        Task<bool> ChangePasswordAsync(string oldPassword, string newPassword, string confirmPassword);
        Task<ApiWhitelabelPrivateUserModel> ChangeAvatarAsync(Stream stream);
    }
}