using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.Service
{
    public class GroupService : IGroupService
    {
        #region Constructor

        private readonly IApiService<IGroupApi> _groupApiService;

        public GroupService(IApiService<IGroupApi> groupApiService)
        {
            _groupApiService = groupApiService;
        }

        #endregion Constructor

        #region Get

        public async Task<ApiWhitelabelGroupModel> GetAsync(ApiPriority priority, Guid id)
        {
            var cachedGroup = BlobCache.LocalMachine.GetOrFetchObject($"group-{id}", () => GetRemoteAsync(priority, id), DateTimeOffset.Now.AddHours(1));

            var groupModel = await cachedGroup.FirstOrDefaultAsync();

            return groupModel;
        }

        private async Task<ApiWhitelabelGroupModel> GetRemoteAsync(ApiPriority priority, Guid id)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _groupApiService.GetApi(priority);

            var apiResponse = await api.Get(id);

            return apiResponse?.Data;
        }

        public async Task<List<ApiWhitelabelGroupModel>> GetAllAsync(ApiPriority priority)
        {
            var cachedGroup = BlobCache.LocalMachine.GetOrFetchObject("groups", () => GetAllRemoteAsync(priority), DateTimeOffset.Now.AddHours(1));

            var groupModel = await cachedGroup.FirstOrDefaultAsync();

            return groupModel;
        }

        public Task JoinGroupAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task LeaveGroupAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private async Task<List<ApiWhitelabelGroupModel>> GetAllRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _groupApiService.GetApi(priority);

            var apiResponse = await api.GetAll();

            return apiResponse?.Data;
        }

        #endregion Get
    }
}
