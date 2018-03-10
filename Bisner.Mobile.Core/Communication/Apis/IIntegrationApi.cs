using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Integrations;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IIntegrationApi
    {
        [Get("/Api/Integrations/Nexudus/GetRedirectUrl")]
        Task<ApiResponse<string>> GetNexudusRedirectUrlAsync(string type);

        [Get("/Api/AccessControl/List")]
        Task<ApiResponse<List<ApiAccessControlModel>>> GetLocks();

        [Post("/Api/AccessControl/Open")]
        Task<ApiResponse<bool>> OpenLock(Guid id);

        [Post("/Api/AccessControl/Close")]
        Task<ApiResponse<bool>> CloseLock(Guid id);

        [Post("/Api/Integrations/Roomzilla/GetRedirectUrl")]
        Task<ApiResponse<string>> GetRoomzillaLink();
    }
}