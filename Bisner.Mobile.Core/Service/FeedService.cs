using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Security;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Bisner.Mobile.Core.Helpers;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace Bisner.Mobile.Core.Service
{
    public class FeedResponseModel
    {
        public List<ApiWhitelabelFeedPostModel> Posts { get; set; }

        public ApiRelatedItemsModel RelatedItems { get; set; }
    }

    public class FeedService : IFeedService
    {
        #region Constructor

        private readonly IApiService<IFeedApi> _feedApiService;
        // TODO : Only needed for the refit workaround with the image post
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public FeedService(IApiService<IFeedApi> feedApiService, IConfiguration configuration, ITokenService tokenService)
        {
            _feedApiService = feedApiService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        #endregion Constructor

        #region Get

        public async Task<FeedResponseModel> GetHomeFeedAsync(ApiPriority priority, DateTime? olderThen)
        {
            FeedResponseModel feedResponseModel;

            if (olderThen == null)
            {
                // First load
                var cachedFeedResponse = BlobCache.LocalMachine.GetOrFetchObject("homefeed",
                    () => GetHomeFeedRemoteAsync(priority), DateTimeOffset.Now.AddMinutes(1));

                feedResponseModel = await cachedFeedResponse.FirstOrDefaultAsync();
            }
            else
            {
                // Load more, always from web
                feedResponseModel = await GetHomeFeedRemoteAsync(priority, olderThen);
            }

            return feedResponseModel;
        }

        private async Task<FeedResponseModel> GetHomeFeedRemoteAsync(ApiPriority priority, DateTime? olderThen = null)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _feedApiService.GetApi(priority);

            var olderThenString = GetOlderThenString(olderThen);

            var response = await api.GetHomeFeed(olderThenString);

            var feedResponse = new FeedResponseModel
            {
                Posts = response.Data,
                RelatedItems = response.AdditionalData,
            };

            return feedResponse;
        }

        public async Task<FeedResponseModel> GetCompanyFeedAsync(ApiPriority priority, Guid companyId,
            DateTime? olderThen)
        {
            FeedResponseModel responseModel;

            if (olderThen == null)
            {
                // First load
                var cachedCompanyFeed = BlobCache.LocalMachine.GetOrFetchObject($"companyfeed-{companyId}",
                    () => GetCompanyFeedRemoteAsync(priority, companyId), DateTimeOffset.Now);

                responseModel = await cachedCompanyFeed.FirstOrDefaultAsync();
            }
            else
            {
                // Load more
                responseModel = await GetCompanyFeedRemoteAsync(priority, companyId, olderThen);
            }

            return responseModel;
        }

        private async Task<FeedResponseModel> GetCompanyFeedRemoteAsync(ApiPriority priority, Guid companyId,
            DateTime? olderThen = null)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _feedApiService.GetApi(priority);

            string olderThenString = GetOlderThenString(olderThen);

            var response = await api.GetCompanyFeed(companyId, olderThenString);

            var feedResponse = new FeedResponseModel
            {
                Posts = response.Data,
                RelatedItems = response.AdditionalData
            };

            return feedResponse;
        }

        public async Task<FeedResponseModel> GetGroupFeedAsync(ApiPriority priority, Guid groupId, DateTime? olderThen)
        {
            FeedResponseModel responseModel;

            if (olderThen == null)
            {
                // First load
                var cachedResponseModel = BlobCache.LocalMachine.GetOrFetchObject($"groupfeed-{groupId}",
                    () => GetGroupFeedRemoteAsync(priority, groupId, null), DateTimeOffset.Now.AddMinutes(5));

                responseModel = await cachedResponseModel.FirstOrDefaultAsync();
            }
            else
            {
                // Load more
                responseModel = await GetGroupFeedRemoteAsync(priority, groupId, olderThen);
            }

            return responseModel;
        }

        private async Task<FeedResponseModel> GetGroupFeedRemoteAsync(ApiPriority priority, Guid groupId,
            DateTime? olderThen)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _feedApiService.GetApi(priority);


            string olderThenString = GetOlderThenString(olderThen);

            var response = await api.GetFeed(groupId, olderThenString);

            var feedResponse = new FeedResponseModel
            {
                Posts = response.Data,
                RelatedItems = response.AdditionalData
            };

            return feedResponse;
        }

        public async Task<FeedResponseModel> GetPostAsync(ApiPriority priority, Guid postId)
        {
            var cachedFeedPost = BlobCache.LocalMachine.GetOrFetchObject($"post-{postId}",
                () => GetPostRemoteAsync(priority, postId), DateTimeOffset.Now);

            var feedPost = await cachedFeedPost.FirstOrDefaultAsync();

            return feedPost;
        }

        private async Task<FeedResponseModel> GetPostRemoteAsync(ApiPriority priority, Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _feedApiService.GetApi(priority);

            var response = await api.GetPost(postId);

            var feedResponseModel = new FeedResponseModel
            {
                Posts = new List<ApiWhitelabelFeedPostModel> { response.Data },
                RelatedItems = response.AdditionalData
            };

            return feedResponseModel;
        }

        #endregion Get

        #region Delete

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        #endregion Delete

        #region Create

        public async Task<bool> CreateTextPostAsync(string inputText, Guid? feedId, List<Guid> mentionedUserIds,
            ParentType parentType = ParentType.Unknown)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(
                    async () => await feedApi.CreateTextPostAsync(inputText, feedId, mentionedUserIds, parentType));

            return response != null && response.Success;
        }

        public async Task<bool> CreateImagePostAsync(string inputText, List<Stream> imageStreamList, Guid? feedId,
            List<Guid> mentionedUserIds, ParentType parentType = ParentType.Unknown)
        {
            //var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            //var imageNumber = 1;
            //var streamPartList = new List<StreamPart>();

            //foreach (var stream in imageStreamList)
            //{
            //    streamPartList.Add(new StreamPart(stream, $"image{imageNumber}", "image/jpg"));
            //    imageNumber++;
            //}

            //var response = await Policy
            //    .Handle<Exception>()
            //    .WaitAndRetryAsync
            //    (
            //        retryCount: 5,
            //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            //    )
            //    .ExecuteAsync(async () => await feedApi.CreateImagePostAsync(inputText, streamPartList, feedId, mentionedUserIds, parentType));

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await ImagePostWorkaroundAsync(inputText, imageStreamList, feedId, mentionedUserIds, parentType));

            //var response = await ImagePostWorkaroundAsync(inputText, imageStreamList, feedId, mentionedUserIds, parentType);

            return response != null && response.Success;
        }

        #region Refit workaround

        private async Task<ApiResponse<ApiWhitelabelFeedPostModel>> ImagePostWorkaroundAsync(string text, List<Stream> images, Guid? parentId, List<Guid> mentionedUserIds, ParentType parentType = ParentType.Unknown)
        {
            using (var content = new MultipartFormDataContent())
            {
                if (text != null)
                {
                    content.Add(new StringContent(text), "text");
                }

                if (parentId != null)
                {
                    var parentIdContent = new StringContent(parentId.ToString());
                    content.Add(parentIdContent, "parentId");
                }

                if (mentionedUserIds != null && mentionedUserIds.Any())
                {
                    var mentionString = mentionedUserIds.Aggregate("", (current, mentionedUserId) => current + (mentionedUserId + ","));
                    content.Add(new StringContent(mentionString), "mentionedUserIds");
                }

                var number = 0;
                foreach (var imageStream in images)
                {
                    content.Add(CreateFileContent(imageStream, "image" + number, "image/jpg"));
                    number++;
                }

                var response = await PostAsync("Api/Feed/CreateImagePostAsync", content);

                return response;
            }
        }

        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        private async Task<ApiResponse<ApiWhitelabelFeedPostModel>> PostAsync(string endPoint, HttpContent content)
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

                    return await HandleResponse<ApiResponse<ApiWhitelabelFeedPostModel>>(response);
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

        #endregion Create

        #region User actions

        public async Task<bool> ReportPostAsync(Guid id)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetry
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await feedApi.ReportPost(id));

            return response != null && response.Success;
        }

        public async Task<bool> CommentAsync(Guid postId, string commentInput, List<Guid> mentionedUsers = null)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await feedApi.CommentAsync(postId, commentInput, mentionedUsers));

            return response != null && response.Success;
        }

        public async Task<bool> FollowPostAsync(Guid id, bool isFollowing)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await feedApi.FollowPostAsync(id, isFollowing));

            return response != null && response.Success;
        }

        public async Task<bool> LikePostAsync(Guid id, bool hasLiked)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await feedApi.LikePostAsync(id, hasLiked));

            return response != null && response.Success;
        }

        public async Task<bool> ReportCommentAsync(Guid id)
        {
            var feedApi = _feedApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await feedApi.ReportComment(Guid.NewGuid(), id));

            return response != null && response.Success;
        }

        #endregion User actions

        #region Helpers

        private static string GetOlderThenString(DateTime? olderThen)
        {
            if (olderThen == null)
            {
                olderThen = DateTime.MaxValue;
            }

            var olderThenString = olderThen.Value.ToString("yyyy-MM-dd HH:mm:ss");
            return olderThenString;
        }

        #endregion Helpers
    }
}
