using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Bisner.ApiModels.Central;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Communication.Apis;
using Plugin.Connectivity;
using Polly;

namespace Bisner.Mobile.Core.Service
{
    public class ChatService : IChatService
    {
        #region Constructor

        private readonly IApiService<IChatApi> _chatApiService;

        public ChatService(IApiService<IChatApi> chatApiService)
        {
            _chatApiService = chatApiService;
        }

        #endregion Constructor

        #region Get

        public async Task<List<ApiCentralPrivateChatMessageModel>> GetConversationAsync(ApiPriority priority, Guid conversationId, DateTime? olderThen)
        {
            List<ApiCentralPrivateChatMessageModel> conversationMessages;

            if (olderThen == null)
            {
                var cachedConversation = BlobCache.LocalMachine.GetOrFetchObject($"conversation-{conversationId}", () => GetConversationRemoteAsync(priority, conversationId), DateTimeOffset.Now.AddSeconds(1));

                conversationMessages = await cachedConversation.FirstOrDefaultAsync();
            }
            else
            {
                // older then, always remote
                conversationMessages = await GetConversationRemoteAsync(priority, conversationId, olderThen);
            }

            return conversationMessages;
        }

        private async Task<List<ApiCentralPrivateChatMessageModel>> GetConversationRemoteAsync(ApiPriority priority, Guid conversationId, DateTime? olderThen = null)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _chatApiService.GetApi(priority);

            var conversationMessages = await api.GetAll(conversationId, olderThen);

            return conversationMessages.Data;
        }

        public async Task<List<ApiCentralPrivateChatMessageModel>> GetLastMessagesAsync(ApiPriority priority)
        {
            var cachedLastMessages = BlobCache.LocalMachine.GetOrFetchObject("lastmessages", () => GetLastMessagesRemoteAsync(priority), DateTimeOffset.Now.AddSeconds(1));

            var lastMessages = await cachedLastMessages.FirstOrDefaultAsync();

            return lastMessages;
        }

        private async Task<List<ApiCentralPrivateChatMessageModel>> GetLastMessagesRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _chatApiService.GetApi(priority);

            var lastMessages = await api.GetLastMessages();

            return lastMessages.Data;
        }

        public async Task<List<ApiCentralPrivateChatMessageModel>> GetUnreadAsync(ApiPriority priority)
        {
            var cachedUnreadMessages = BlobCache.LocalMachine.GetOrFetchObject("unreadmessages", () => GetUnreadMessagesRemoteAsync(priority), DateTimeOffset.Now.AddSeconds(1));

            var unreadMessages = await cachedUnreadMessages.FirstOrDefaultAsync();

            return unreadMessages;
        }

        private async Task<List<ApiCentralPrivateChatMessageModel>> GetUnreadMessagesRemoteAsync(ApiPriority priority)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var api = _chatApiService.GetApi(priority);

            var lastMessages = await api.GetUnreadMessages();

            return lastMessages.Data;
        }

        public async Task<ApiCentralPrivateChatMessageModel> SendMessageAsync(Guid userId, string message, string senderMessageid = null)
        {
            var response = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync
                (
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await _chatApiService.UserInitiated.SendMessageAsync(userId, message, senderMessageid));
            
            return response.Data;
        }

        public async Task<bool> SetHasReadAsync(ApiPriority priority, Guid conversationId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var api = _chatApiService.GetApi(priority);

            var response = await api.MarkAsRead(conversationId);

            return response.Success;
        }

        #endregion Get
    }
}
