using System;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;

namespace Bisner.Mobile.Core.Service
{
    public class IntegrationService : IIntegrationService
    {
        #region Constructor

        private readonly IApiService<IIntegrationApi> _integrationApiService;

        public IntegrationService(IApiService<IIntegrationApi> integrationApiService)
        {
            _integrationApiService = integrationApiService;
        }

        #endregion Constructor

        #region Nexudus

        public async Task<string> GetNexudusUrlAsync(string type)
        {
            var api = _integrationApiService.GetApi(ApiPriority.UserInitiated);

            var url = await api.GetNexudusRedirectUrlAsync(type);

            return url?.Data;
        }

        public async Task<string> GetRoomzillaUrlAsync()
        {
            var api = _integrationApiService.GetApi(ApiPriority.UserInitiated);

            var url = await api.GetRoomzillaLink();

            return url?.Data;
        }

        #endregion Nexudus
    }
}
