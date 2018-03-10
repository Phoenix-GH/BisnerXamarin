using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;
using Polly;

namespace Bisner.Mobile.Core.Service
{
    public class EventService : IEventService
    {
        #region Constructor

        private readonly IApiService<IEventApi> _eventApiService;

        public EventService(IApiService<IEventApi> eventApiService)
        {
            _eventApiService = eventApiService;
        }


        #endregion Constructor

        #region Get

        public async Task<ApiWhitelabelEventModel> GetAsync(ApiPriority priority, Guid eventId)
        {
            var cachedEventModel = BlobCache.LocalMachine.GetOrFetchObject($"event-{eventId}", () => GetEventRemoteAsync(priority, eventId), DateTimeOffset.Now.AddMinutes(5));

            var eventModel = await cachedEventModel.FirstOrDefaultAsync();

            return eventModel;
        }

        private async Task<ApiWhitelabelEventModel> GetEventRemoteAsync(ApiPriority priority, Guid eventId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _eventApiService.GetApi(priority);

            var eventModel = await api.GetEvent(eventId);

            return eventModel.Data;
        }

        public async Task<List<ApiWhitelabelEventModel>> GetAllAsync(ApiPriority priority, Guid? categoryId = null, bool futureOnly = false)
        {
            var cachedEventModels = BlobCache.LocalMachine.GetOrFetchObject(GetKey(categoryId), () => GetAllRemoteAsync(priority, categoryId), DateTimeOffset.Now.AddSeconds(30));

            var eventModels = await cachedEventModels.FirstOrDefaultAsync();

            return eventModels;
        }

        private string GetKey(Guid? categoryId)
        {
            if (categoryId != null)
                return $"events-{categoryId.Value}";

            return "events";
        }

        private async Task<List<ApiWhitelabelEventModel>> GetAllRemoteAsync(ApiPriority priority, Guid? categoryId = null, bool futureOnly = false)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _eventApiService.GetApi(priority);

            ApiResponse<List<ApiWhitelabelEventModel>> eventModels;

            if (categoryId == null)
            {
                eventModels = await api.GetAll(null, futureOnly);
            }
            else
            {
                eventModels = await api.GetAllForCategory(categoryId.Value, futureOnly);
            }

            return eventModels.Data;
        }

        public async Task<List<ApiWhitelabelEventModel>> GetUpcomingAsync(ApiPriority priority, int number)
        {
            var cachedEventModels = BlobCache.LocalMachine.GetOrFetchObject($"upcomingEvents-{number}", () => GetUpcomingRemoteAsync(priority, number), DateTimeOffset.Now.AddMinutes(5));

            var eventModels = await cachedEventModels.FirstOrDefaultAsync();

            return eventModels;
        }

        private async Task<List<ApiWhitelabelEventModel>> GetUpcomingRemoteAsync(ApiPriority priority, int number)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _eventApiService.GetApi(priority);

            var eventModels = await api.GetUpcomming(number);

            return eventModels.Data;
        }

        #endregion Get

        #region User actions

        public async Task<ApiWhitelabelEventModel> AttendEventAsync(Guid eventId, bool objIsAttending)
        {
            var eventApi = _eventApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await eventApi.AttendAsync(eventId, objIsAttending));

            return response?.Data;
        }

        public async Task<ApiWhitelabelEventModel> CommentEventAsync(Guid eventId, string input, List<Guid> mentionedUsers = null)
        {
            var eventApi = _eventApiService.GetApi(ApiPriority.UserInitiated);

            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetry
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await eventApi.CommentAsync(eventId, input, mentionedUsers));

            return response?.Data;
        }

        #endregion User actions
    }
}