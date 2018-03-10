using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IPlatformService
    {
        Task<ApiCentralPublicPlatformModel> GetPublicPlatformAsync(ApiPriority priority);
        Task<PlatformCustomMenuModel> GetPlatformCustomMenuAsync(ApiPriority apiPriority);
        Task<List<ApiCentralLocationModel>> GetLocationsAsync(ApiPriority priority);
        Task<ApiCentralLocationModel> GetLocationAsync(ApiPriority priority, Guid id);
        Task<List<LanguageModel>> GetLanguagesAsync();
        Task<LanguageModel> GetLanguageAsync(Guid languageId);
        Task<List<ApiCentralEventCategoryModel>> GetAllEventCategoriesAsync(ApiPriority priority);
        Task<ApiCentralEventCategoryModel> GetEventCategoryAsync(ApiPriority priority, Guid id);
        Task<List<ApiIndustryModel>> GetAllIndustriesAsync(ApiPriority priority);
        Task<ApiIndustryModel> GetIndustryAsync(ApiPriority priority, Guid id);
    }
}