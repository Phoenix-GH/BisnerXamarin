using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using ModernHttpClient;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace Bisner.Mobile.Core.Service
{
    public class UserService : MvxNavigatingObject, IUserService
    {
        #region Constructor

        private const string PersonalModelKey = "personalmodel";
        private const string MembersKey = "members";
        private const string CompaniesKey = "companies";

        private readonly IApiService<IUserApi> _userApiService;
        private readonly IApiService<IMembersApi> _memberApiService;
        private readonly ITokenService _tokenService;

        private readonly IConfiguration _configuration;

        public UserService(IApiService<IUserApi> userApiService, IApiService<IMembersApi> memberApiService, IConfiguration configuration, ITokenService tokenService)
        {
            _userApiService = userApiService;
            _memberApiService = memberApiService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        #endregion Constructor

        #region Private

        public async Task<ApiWhitelabelPrivateUserModel> GetPersonalModelAsync(ApiPriority priority)
        {
            var cacheTimeMinutes = 10;

#if DEBUG
            cacheTimeMinutes = 1;
#endif

            var cachedUserPrivateModel = BlobCache.LocalMachine.GetAndFetchLatest(PersonalModelKey, () => GetPersonalModelRemoteAsync(priority), null, DateTimeOffset.Now.AddMinutes(cacheTimeMinutes));

            var userPrivateModel = await cachedUserPrivateModel.FirstOrDefaultAsync();

            if (userPrivateModel == null)
            {
                userPrivateModel = await cachedUserPrivateModel.LastOrDefaultAsync();
            }

            if (userPrivateModel != null)
            {
                if (Settings.UserRoles.Any(r => userPrivateModel.Roles.All(p => p != r)))
                {
                    ViewDispatcher.ChangePresentation(new LanguageChangedPresentationHint());
                }

                Settings.UserRoles = userPrivateModel.Roles;
            }

            return userPrivateModel;
        }

        private async Task<ApiWhitelabelPrivateUserModel> GetPersonalModelRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var userApi = _userApiService.GetApi(priority);

            var apiResponse = await userApi.GetPersonalModel();

            if (apiResponse != null && apiResponse.Success)
            {
                Settings.UserId = apiResponse.Data.Id;
                Settings.UserRoles = apiResponse.Data.Roles;
            }

            return apiResponse?.Data;
        }

        public async Task<ApiWhitelabelPrivateUserModel> UpdateProfileAsync(ApiUserUpdateModel userUpdateModel)
        {
            var userApi = _userApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<WebException>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await userApi.UpdateProfileAsync(userUpdateModel));

            return response?.Data;
        }

        public async Task<ApiWhitelabelPrivateUserModel> UpdateNotificationSettingsAsync(ApiNotificationSettings notificationSettings)
        {
            var userApi = _userApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<WebException>()
                .WaitAndRetry
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await userApi.UpdateNotificationSettings(notificationSettings));

            return response?.Data;
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
                    await BlobCache.LocalMachine.InsertObject($"company_{apiWhitelabelCompanyModel.Id}",
                        apiWhitelabelCompanyModel, DateTimeOffset.Now.AddHours(1));
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

        public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword, string confirmPassword)
        {
            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await _userApiService.UserInitiated.ChangePassword(oldPassword, newPassword, confirmPassword));

            return response.Success;
        }

        public async Task<ApiWhitelabelPrivateUserModel> ChangeAvatarAsync(Stream stream)
        {
            var response = await ImagePostWorkaroundAsync(stream);

            return response?.Data;
        }

        #endregion Public

        #region Refit workaround

        private async Task<ApiResponse<ApiWhitelabelPrivateUserModel>> ImagePostWorkaroundAsync(Stream image)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(CreateFileContent(image, "image", "image/jpg"));

                var response = await PostAsync("Api/User/ChangeAvatarByFileAsync", content);

                return response;
            }
        }

        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"image\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        private async Task<ApiResponse<ApiWhitelabelPrivateUserModel>> PostAsync(string endPoint, HttpContent content)
        {
            // Build handler
            using (var handler = new NativeMessageHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
            {
                // Build client
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(_configuration.BaseUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenService.GetTokenAsync());

                    // Check response
                    var watch = new Stopwatch();
                    Debug.WriteLine("REQUEST INVOKE");
                    watch.Start();
                    var response = await client.PostAsync(endPoint, content);
                    watch.Stop();
                    Debug.WriteLine("REQUEST FINISHED : Time = {0}", watch.ElapsedMilliseconds);

                    response.EnsureSuccessStatusCode();

                    return await HandleResponse<ApiResponse<ApiWhitelabelPrivateUserModel>>(response);
                }
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : ApiResponse, new()
        {
            // !!!!!!!!!!!!!!!ATTENTION!!!!!!!!!!!!!!!!
            // When getting encoding exceptions, make sure you turn on the required internationalization in the ios project properties

            // Handle response as string
            var responseString = await response.Content.ReadAsStringAsync();

            var toReturn = new T();

            try
            {
                // Deserialize the response to ApiResponse object
                toReturn = JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception)
            {
                var errorString = string.Format("Could not deserialize Json response '{1}' into object of type '{0}'", typeof(T), responseString);
                Debug.WriteLine(errorString);
                toReturn.Success = false;
                toReturn.Message = errorString;
            }

            return toReturn;
        }

        #endregion Refit workaround
    }
}