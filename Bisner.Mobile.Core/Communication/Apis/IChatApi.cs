using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Refit;

namespace Bisner.Mobile.Core.Communication.Apis
{
    [Headers("Accept: application/json", "Accept-Encoding: gzip, deflate", "Authorization: Bearer")]
    public interface IChatApi
    {
        /// <summary>
        /// Get upto 25 chat messages since the datetime supplied or the latest 25 if no datetime supplied
        /// </summary>
        /// <param name="id">Other userId</param>
        /// <param name="olderThen">Get 25 messages since this point</param>
        /// <returns></returns>
        [Get("/Api/PrivateChat/GetAll")]
        Task<ApiResponse<List<ApiCentralPrivateChatMessageModel>>> GetAll(Guid id, DateTime? olderThen);

        /// <summary>
        /// Get last messages for all contacts
        /// </summary>
        /// <returns></returns>
        [Get("/Api/PrivateChat/GetLastMessages")]
        Task<ApiResponse<List<ApiCentralPrivateChatMessageModel>>> GetLastMessages();

        /// <summary>
        /// Get last messages for all contacts
        /// </summary>
        /// <returns></returns>
        [Get("/Api/PrivateChat/GetUnreadMessages")]
        Task<ApiResponse<List<ApiCentralPrivateChatMessageModel>>> GetUnreadMessages();

        /// <summary>
        /// Get all unread messages for this chat
        /// </summary>
        /// <param name="id">Other userId</param>
        /// <returns></returns>
        [Get("/Api/PrivateChat/GetUnread")]
        Task<ApiResponse<List<ApiCentralPrivateChatMessageModel>>> GetUnread(Guid id);

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="senderMessageId">The sender message identifier.</param>
        /// <returns></returns>
        [Post("/Api/PrivateChat/SendMessageAsync")]
        Task<ApiResponse<ApiCentralPrivateChatMessageModel>> SendMessageAsync(Guid userId, string message, string senderMessageId = null);

        /// <summary>
        /// Marks as read.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Post("/Api/PrivateChat/MarkAsRead")]
        Task<ApiResponse> MarkAsRead(Guid id);
    }
}