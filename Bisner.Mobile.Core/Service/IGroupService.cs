using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IGroupService
    {
        Task<ApiWhitelabelGroupModel> GetAsync(ApiPriority priority, Guid id);
        Task<List<ApiWhitelabelGroupModel>> GetAllAsync(ApiPriority priority);
        Task JoinGroupAsync(Guid id);
        Task LeaveGroupAsync(Guid id);
    }
}