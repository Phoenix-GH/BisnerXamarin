using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Helpers;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.Service
{
    public class PlatformService : IPlatformService
    {
        #region Constructor

        private const string PlatformKey = "publicplatform";
        private const string PlatformMenuKey = "platformmenu";

        private readonly IApiService<IPlatformApi> _apiService;

        public PlatformService(IApiService<IPlatformApi> apiService)
        {
            _apiService = apiService;
        }

        #endregion Constructor

        #region Platform

        public async Task<ApiCentralPublicPlatformModel> GetPublicPlatformAsync(ApiPriority priority)
        {
            var cachedPlatform = BlobCache.LocalMachine.GetAndFetchLatest(PlatformKey, () => GetRemotePlatformAsync(priority), null, DateTimeOffset.Now.AddHours(1));

            var platform = await cachedPlatform.FirstOrDefaultAsync();

            if (platform == null)
            {
                platform = await cachedPlatform.LastOrDefaultAsync();
            }

            return platform;
        }

        private async Task<ApiCentralPublicPlatformModel> GetRemotePlatformAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _apiService.GetApi(priority);

            var apiResponse = await api.Get();

            if (apiResponse?.Data != null && apiResponse.Success)
            {
                Settings.AmPmNotation = apiResponse.Data.ShowAmPmNotation;
                Settings.ShowExternalUrlWarning = apiResponse.Data.MemberSettings?.ShowExternalLinkWarning ?? false;
                Settings.TimeFormat = apiResponse.Data.TimeString;
                Settings.DateFormat = apiResponse.Data.DateString;
                Settings.AccessControlEnabled = apiResponse.Data.AccessControlEnabled;
                Settings.NotificationHubPath = apiResponse.Data.NotificationHub;
                Settings.NotificationConnectionString = apiResponse.Data.NotificationListenSharedConnectionString;
                Settings.AmPmNotation = apiResponse.Data.ShowAmPmNotation;
                Settings.SenderId = apiResponse.Data.GoogleSenderId;

                // Always set default language
                var defaultLanguage = apiResponse.Data?.Languages?.FirstOrDefault(l => l.IsDefault);

                if (defaultLanguage != null)
                {
                    Settings.DefaultLanguage = defaultLanguage;

                    // Set the selected language too
                    Settings.SelectedLanguage = Settings.SelectedLanguageId == Guid.Empty ? defaultLanguage : apiResponse.Data.Languages.FirstOrDefault(l => l.Id == Settings.SelectedLanguageId);
                }

                Settings.BlobUrl = apiResponse.Data.CdnBasePath;
                Settings.CompanyHeaderUrl = apiResponse.Data.Images?.CompanyCardHeader?.Medium;
                Settings.EventHeaderUrl = apiResponse.Data.Images?.EventHeader?.Medium;
                Settings.GroupsHeaderUrl = apiResponse.Data.Images?.GroupsHeader?.Medium;
            }

            return apiResponse?.Data;
        }

        public async Task<PlatformCustomMenuModel> GetPlatformCustomMenuAsync(ApiPriority priority)
        {
            var cachedCustomMenu = BlobCache.LocalMachine.GetOrFetchObject(PlatformMenuKey, () => GetPlatformCustomMenuRemoteAsync(priority), DateTimeOffset.Now.AddHours(1));

            var customMenuModel = await cachedCustomMenu.FirstOrDefaultAsync();

            return customMenuModel;
        }

        private async Task<PlatformCustomMenuModel> GetPlatformCustomMenuRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _apiService.GetApi(priority);

            var apiResponse = await api.GetCustomMenu();

            return apiResponse?.Data;
        }

        #endregion Platform

        #region Locations

        public async Task<List<ApiCentralLocationModel>> GetLocationsAsync(ApiPriority priority)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel != null ? platformModel.Locations : new List<ApiCentralLocationModel>();
        }

        public async Task<ApiCentralLocationModel> GetLocationAsync(ApiPriority priority, Guid id)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel?.Locations.FirstOrDefault(p => p.Id == id);
        }

        #endregion Locations

        #region Language and Resources

        public async Task<List<LanguageModel>> GetLanguagesAsync()
        {
            var platformModel = await GetPublicPlatformAsync(ApiPriority.Background);

            return platformModel != null ? platformModel.Languages : new List<LanguageModel>();
        }

        public async Task<LanguageModel> GetLanguageAsync(Guid languageId)
        {
            var platformModel = await GetPublicPlatformAsync(ApiPriority.Background);

            var languageModel = platformModel?.Languages?.FirstOrDefault(l => l.Id == languageId);

            return languageModel;
        }

        #endregion Language and Resources

        #region Event categories

        public async Task<List<ApiCentralEventCategoryModel>> GetAllEventCategoriesAsync(ApiPriority priority)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel?.EventCategories;
        }

        public async Task<ApiCentralEventCategoryModel> GetEventCategoryAsync(ApiPriority priority, Guid id)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel?.EventCategories?.FirstOrDefault(c => c.Id == id);
        }

        #endregion Event categories

        #region Industries

        public async Task<List<ApiIndustryModel>> GetAllIndustriesAsync(ApiPriority priority)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel?.Industries;
        }

        public async Task<ApiIndustryModel> GetIndustryAsync(ApiPriority priority, Guid id)
        {
            var platformModel = await GetPublicPlatformAsync(priority);

            return platformModel?.Industries?.FirstOrDefault(c => c.Id == id);
        }

        #endregion Industries
    }
}
