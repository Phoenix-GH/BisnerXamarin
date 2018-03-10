using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.Mobile.Core.Communication;

namespace Bisner.Mobile.Core.Service
{
    public interface IChatService
    {
        Task<List<ApiCentralPrivateChatMessageModel>> GetConversationAsync(ApiPriority priority, Guid conversationId, DateTime? olderThen);
        Task<List<ApiCentralPrivateChatMessageModel>> GetLastMessagesAsync(ApiPriority priority);
        Task<List<ApiCentralPrivateChatMessageModel>> GetUnreadAsync(ApiPriority priority);
        Task<ApiCentralPrivateChatMessageModel> SendMessageAsync(Guid userId, string message, string senderMessageid = null);
        Task<bool> SetHasReadAsync(ApiPriority priority, Guid conversationId);
    }
}