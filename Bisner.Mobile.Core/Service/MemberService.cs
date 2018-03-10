using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.Service
{
    public class MemberService
    {
        #region Constructor

        private const string MembersKey = "members";
        private const string CompaniesKey = "companies";

        private readonly IApiService<IUserApi> _userApiService;
        private readonly IApiService<IMembersApi> _memberApiService;

        public MemberService(IApiService<IUserApi> userApiService, IApiService<IMembersApi> memberApiService)
        {
            _userApiService = userApiService;
            _memberApiService = memberApiService;
        }

        #endregion Constructor

        #region Private

        public async Task<ApiWhitelabelPrivateUserModel> GetSelfPrivateAsync(ApiPriority priority)
        {
            var cachedUserPrivateModel = BlobCache.LocalMachine.GetOrFetchObject("personalmodel", () => GetRemoteSelfPrivateAsync(priority), DateTimeOffset.Now.AddHours(1));

            var userPrivateModel = await cachedUserPrivateModel.FirstOrDefaultAsync();

            return userPrivateModel;
        }

        private async Task<ApiWhitelabelPrivateUserModel> GetRemoteSelfPrivateAsync(ApiPriority priority)
        {
            var userApi = _userApiService.GetApi(priority);

            var apiResponse = await userApi.GetPersonalModel();

            return apiResponse?.Data;
        }

        #endregion Private

        #region Public

        public async Task<ApiWhitelabelPublicUserModel> GetUserAsync(Guid userId, ApiPriority priority)
        {
            // 1 hour cache
            var cachedUserModel = BlobCache.LocalMachine.GetOrFetchObject($"user_{userId}", () => GetRemoteUserAsync(userId, priority), DateTimeOffset.Now.AddHours(1));

            var userModel = await cachedUserModel.FirstOrDefaultAsync();

            return userModel;
        }

        private async Task<ApiWhitelabelPublicUserModel> GetRemoteUserAsync(Guid userId, ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _userApiService.GetApi(priority);

            var apiResponse = await api.Get(userId);

            if (apiResponse.Success && apiResponse.AdditionalData?.Companies != null)
            {
                // Add user companies to cache
                foreach (var apiWhitelabelCompanyModel in apiResponse.AdditionalData.Companies)
                {
                    // Add to cache
                    await BlobCache.LocalMachine.InsertObject($"company_{apiWhitelabelCompanyModel.Id}", apiWhitelabelCompanyModel, DateTimeOffset.Now.AddHours(1));
                }
            }

            return apiResponse.Data;
        }

        public async Task<List<ApiWhitelabelPublicUserModel>> GetAllPublicUsersAsync(ApiPriority priority)
        {
            var cachedUserModels = BlobCache.LocalMachine.GetOrFetchObject(MembersKey, () => GetRemoteAllPublicUsersAsync(priority), DateTimeOffset.Now.AddHours(1));

            var userModels = await cachedUserModels.FirstOrDefaultAsync();

            return userModels;
        }

        private async Task<List<ApiWhitelabelPublicUserModel>> GetRemoteAllPublicUsersAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _memberApiService.GetApi(priority);

            var apiResponse = await api.GetAll();

            if (apiResponse?.Data != null && apiResponse.Success)
            {
                // Add companies to cache
                await BlobCache.LocalMachine.InsertObject(CompaniesKey, apiResponse.Data.Companies, DateTimeOffset.Now.AddHours(1));
            }

            return apiResponse?.Data?.Users;
        }

        public async Task<List<ApiWhitelabelPublicUserModel>> SearchMembersAsync(string input)
        {
            input = input.ToLower();

            var userModels = await GetAllPublicUsersAsync(ApiPriority.UserInitiated);

            if (userModels != null)
            {
                // Check displayname and email
                return userModels.Where(u => u.DisplayName.ToLower().Contains(input) || u.Email.ToLower().Contains(input)).ToList();
            }

            return new List<ApiWhitelabelPublicUserModel>();
        }

        #endregion Public
    }
}