using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface ICompanyService
    {
        Task<ApiWhitelabelCompanyModel> GetAsync(ApiPriority priority, Guid id);
        Task<List<ApiWhitelabelCompanyModel>> GetAllAsync(ApiPriority priority);
        Task<List<ApiWhitelabelCompanyModel>> GetMyCompaniesAsync(ApiPriority priority);
    }
}