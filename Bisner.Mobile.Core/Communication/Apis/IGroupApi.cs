using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bisner.ApiModels.Groups;
using Bisner.ApiModels.Whitelabel;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IGroupApi
    {
        [Get("/Api/Group/GetAll")]
        Task<ApiResponse<List<ApiWhitelabelGroupModel>>> GetAll(bool includePrivateHidden = false);

        [Get("/Api/Group/Get")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> Get(Guid id);

        [Post("/Api/Group/CreateAsync")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> CreateAsync(CreateGroupModel model);

        [Post("/Api/Group/UpdateAsync")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> UpdateAsync(GroupEditModel model);

        [Post("/Api/Group/ChangeLogo")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> ChangeLogoAsync(Guid id, [AttachmentName("data")] Stream logo);

        [Post("/Api/Group/ChangeHeaderAsync")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> ChangeHeaderAsync(Guid id, [AttachmentName("data")] Stream logo);

        [Post("/Api/Group/JoinGroupAsync")]
        Task<ApiResponse> JoinGroupAsync(Guid id);

        [Post("/Api/Group/AcceptUserAsync")]
        Task<ApiResponse> AcceptUserAsync(Guid id, Guid userId);

        [Post("/Api/Group/RejectUserAsync")]
        Task<ApiResponse> RejectUserAsync(Guid id, Guid userId);

        [Post("/Api/Group/JoinPendingGroupAsync")]
        Task<ApiResponse> JoinPendingGroupAsync(Guid id);

        [Post("/Api/Group/AdminAdd")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> AdminAdd(Guid id, List<Guid> userIds);

        [Post("/Api/Group/InviteUsersToGroup")]
        Task<ApiResponse> InviteUsersToGroup(Guid id, List<Guid> userIds);

        [Post("/Api/Group/LeaveGroupAsync")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> LeaveGroupAsync(Guid id);

        [Post("/Api/Group/DeleteAsync")]
        Task<ApiResponse> DeleteAsync(Guid id);

        [Get("/Api/Group/GetRandom")]
        Task<ApiResponse<ApiWhitelabelGroupModel>> GetRandom();

        [Get("/Api/Group/GetRandomWelcomeFLow")]
        Task<ApiResponse<List<ApiWhitelabelGroupModel>>> GetRandomWelcomeFLow();
    }
}