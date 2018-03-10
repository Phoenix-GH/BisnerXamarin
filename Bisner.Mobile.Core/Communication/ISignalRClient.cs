using System;
using System.Threading;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Microsoft.AspNet.SignalR.Client;

namespace Bisner.Mobile.Core.Communication
{
    public interface ISignalRClient
    {
        Task StartAsync();
        void Stop();
        ConnectionState ConnectionState { get; }
        CancellationTokenSource ReconnectTokenSource { get; }
        DateTime LastDisconnection { get; }

        /// <summary>
        /// Set to true to show user online debug output from signalr, false if you hate spam
        /// </summary>
        bool ShowUserOnline { get; set; }

        event Action<StateChange> ConnectionStateChanged;
        event Action Reconnected;

        #region Events

        event Action<ApiWhitelabelPrivateUserModel> UpdatePersonalProfile;
        event Action<ApiWhitelabelPublicUserModel> UserOnline;
        event Action<Guid> DeleteCompany;
        event Action<ApiWhitelabelCompanyModel> UpdateCompany;
        event Action<Guid> DeleteEvent;
        event Action<ApiWhitelabelEventModel> UpdateEvent;
        event Action<ApiWhitelabelGroupModel> UpdateGroup;
        event Action<Guid> DeleteFeedPost;
        event Action<ApiWhitelabelFeedPostModel> NewFeedPost;
        event Action<ApiCentralPrivateChatMessageModel> NewPrivateMessage;
        event Action<Guid> StartTypingPrivate;
        event Action<Guid> StopTypingPrivate;
        Task StartTypingPrivateAsync(Guid userId);
        Task StopTypingPrivateAsync(Guid userId);
        event Action<Guid> DeleteNotification;
        event Action<ApiWhitelabelNotificationModel, ApiRelatedItemsModel> UpdateNotification;

        #endregion Events

        void ProcessNotificationMessage(Guid conversationId, Guid messageId, Guid userId, string text);
    }
}