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
    public class CompanyService : ICompanyService
    {
        #region Constructor

        private readonly IApiService<ICompanyApi> _companyApiService;

        public CompanyService(IApiService<ICompanyApi> companyApiService)
        {
            _companyApiService = companyApiService;
        }

        #endregion Constructor

        #region Get

        public async Task<ApiWhitelabelCompanyModel> GetAsync(ApiPriority priority, Guid id)
        {
            var cachedCompany = BlobCache.LocalMachine.GetOrFetchObject($"company-{id}", () => GetRemoteAsync(priority, id), DateTimeOffset.Now.AddHours(1));

            var companyModel = await cachedCompany.FirstOrDefaultAsync();

            return companyModel;
        }

        private async Task<ApiWhitelabelCompanyModel> GetRemoteAsync(ApiPriority priority, Guid id)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var companyApi = _companyApiService.GetApi(priority);

            var apiResponse = await companyApi.Get(id);

            return apiResponse?.Data;
        }

        public async Task<List<ApiWhitelabelCompanyModel>> GetAllAsync(ApiPriority priority)
        {
            var cachedCompanies = BlobCache.LocalMachine.GetOrFetchObject("companies", () => GetAllRemoteAsync(priority), DateTimeOffset.Now.AddHours(1));

            var companyModels = await cachedCompanies.FirstOrDefaultAsync();

            return companyModels;
        }

        private async Task<List<ApiWhitelabelCompanyModel>> GetAllRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var companyApi = _companyApiService.GetApi(priority);

            var apiResponse = await companyApi.GetAll();

            return apiResponse?.Data;
        }

        public async Task<List<ApiWhitelabelCompanyModel>> GetMyCompaniesAsync(ApiPriority priority)
        {
            var cachedCompanies = BlobCache.LocalMachine.GetOrFetchObject("mycompanies", () => GetMyCompaniesRemoteAsync(priority), DateTimeOffset.Now.AddHours(1));

            var companyModels = await cachedCompanies.FirstOrDefaultAsync();

            return companyModels;
        }

        private async Task<List<ApiWhitelabelCompanyModel>> GetMyCompaniesRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var companyApi = _companyApiService.GetApi(priority);

            var apiResponse = await companyApi.GetMyCompanies();

            return apiResponse?.Data;
        }

        #endregion Get
    }
}