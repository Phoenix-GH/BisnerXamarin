using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Service;
using Microsoft.AspNet.SignalR.Client;
using Bisner.Mobile.Core.Helpers;

namespace Bisner.Mobile.Core.Communication
{
    public class SignalRClient : ISignalRClient
    {
        #region Constructor

        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IExceptionService _exceptionService;

        private HubConnection _connection;
        private IHubProxy _hubProxy;

        private readonly TextWriter _tracer;

        public SignalRClient(IConfiguration configuration, ITokenService tokenService, IExceptionService exceptionService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _exceptionService = exceptionService;

            _tracer = TextWriter.Null;

            //ShowUserOnline = true;
        }

        #endregion Constructor

        #region Start/Stop

        public async Task StartAsync()
        {
            Logger.Log("Starting SignalR");

            await Connect(true);
        }

        public void Stop()
        {
            if (_connection != null && ConnectionState == ConnectionState.Connected)
            {
                _connection.Stop();
            }
        }

        private async Task Connect(bool dismissCurrentConnection = false)
        {
            LastDisconnection = DateTime.UtcNow;

            // Always create a new connection to avoid SignalR close event delays
            if (_connection != null)
            {
                if (!dismissCurrentConnection)
                {
                    return;
                }

                _connection.StateChanged -= OnConnectionStateChangedHandler;
                _connection.Reconnected -= OnReconnectedHandler;
                _connection.ConnectionSlow -= ConnectionSlow;
                _connection.Closed -= ConnectionClosed;
                _connection.Error -= ConnectionError;
                _connection.Received -= ConnectionRecieved;
                _connection.Reconnecting -= ConnectionReconnecting;
                // DON´T call connection.Dispose() or it may block for 20 seconds
                _connection = null;
                _hubProxy = null;
            }

            _connection = new HubConnection(_configuration.BaseUrl)
            {
                TraceWriter = _tracer,
                TraceLevel = TraceLevels.All
            };
            // connection.TransportConnectTimeout = TimeSpan.FromSeconds(5);
            _connection.StateChanged += OnConnectionStateChangedHandler;
            _connection.Reconnected += OnReconnectedHandler;// Create SignalR connectin
            _connection.ConnectionSlow += ConnectionSlow;
            _connection.Closed += ConnectionClosed;
            _connection.Error += ConnectionError;
            _connection.Received += ConnectionRecieved;
            _connection.Reconnecting += ConnectionReconnecting;

            // Add bearer authorization
            var token = await _tokenService.GetTokenAsync();
            _connection.Headers.Add("Authorization", $"Bearer {token}");

            // Dispose current events
            foreach (var currentEventDisposable in _currentEventDisposables)
            {
                currentEventDisposable.Dispose();
            }

            _currentEventDisposables.Clear();

            _hubProxy = _connection.CreateHubProxy("SignalRHub");

            // Wire up events
            var disposables = WireUpEvents(_hubProxy);
            _currentEventDisposables.AddRange(disposables);

            if (_connection.State == ConnectionState.Disconnected)
            {
                try
                {
                    LogMessage("Connecting...");
                    await _connection.Start();
                    Execute(Reconnected, action => action());
                }
                catch (Exception ex)
                {
                    LogMessage($"CONNECTION START ERROR: {ex.Message}");
                }
            }
        }

        private readonly List<IDisposable> _currentEventDisposables = new List<IDisposable>();

        private void OnReconnectedHandler()
        {
            LastDisconnection = DateTime.UtcNow;

            Task.Run(() =>
            {
                Execute(Reconnected, action => action());
            });
        }

        private void OnConnectionStateChangedHandler(StateChange change)
        {
            LogMessage($"State changed from : {change.OldState} to : {change.NewState}, _connection state : {_connection.State}");

            ConnectionState = change.NewState;
            Execute(ConnectionStateChanged, action => action(change));

            switch (change.NewState)
            {
                case ConnectionState.Disconnected:
                    // SignalR doesn´t do anything after disconnected state, so we need to manually reconnect
                    ReconnectTokenSource = new CancellationTokenSource();
                    LastDisconnection = DateTime.UtcNow;
                    Task.Run(async () =>
                    {
                        await Task.Delay(1000, ReconnectTokenSource.Token);
                        await Connect(true);
                    }, ReconnectTokenSource.Token);

                    break;
            }
        }

        #endregion Start/Stop

        #region Properties

        private List<IDisposable> WireUpEvents(IHubProxy proxy)
        {
            var disposables = new List<IDisposable>
                {
                    // Users and profiles
                    proxy.On<ApiWhitelabelPrivateUserModel>("updatePersonalProfile", OnUpdatePersonalProfile),
                    proxy.On<ApiWhitelabelPublicUserModel>("userOnline", OnUserOnline),

                    // Company
                    proxy.On<ApiWhitelabelCompanyModel>("updateCompany", OnUpdateCompany),
                    proxy.On<Guid>("deleteCompany", OnDeleteCompany),

                    // Event
                    proxy.On<ApiWhitelabelEventModel>("updateEvent", OnUpdateEvent),
                    proxy.On<Guid>("deleteEvent", OnDeleteEvent),

                    // Group
                    proxy.On<ApiWhitelabelGroupModel>("updateGroup", OnUpdateGroup),
                    //proxy.On<Guid>("deleteGroup", OnDeleteGroup),

                    // Feedpost
                    proxy.On<ApiWhitelabelFeedPostModel>("updateFeedpost", OnUpdateFeedPost),
                    proxy.On<Guid>("deleteFeedPost", OnDeleteFeedPost),

                    // Chat
                    proxy.On<ApiCentralPrivateChatMessageModel>("newPrivateMessage", OnNewPrivateMessage),
                    proxy.On<Guid>("startTypingPrivate", OnStartTypingPrivate),
                    proxy.On<Guid>("stopTypingPrivate", OnStopTypingPrivate),

                    // Notifications
                    proxy.On<ApiWhitelabelNotificationModel, ApiRelatedItemsModel>("updateNotification", OnUpdateNotification),
                    proxy.On<Guid>("deleteNotification", OnDeleteNotification),
                    //proxy.On("deleteAllNotifications", OnDeleteAllNotifications),
                    //proxy.On<ApiWhitelabelPrivateUserModel>("deleteAllReadNotifications", OnDeleteAllReadNotifications),
                    //proxy.On<ApiWhitelabelPrivateUserModel>("deleteAllUnReadNotifications", OnDeleteAllUnreadNotifications),

                    // Workspace
                    //proxy.On<ApiWorkspaceModel>("updateWorkspace", OnUpdateWorkspace),
                    //proxy.On<ApiWorkspaceModel>("updateWorkspaceLastVisit", OnUpdateWorkspaceLastVisit),
                    //proxy.On<Guid>("deleteWorkspace", OnDeleteWorkspace),

                    // Note
                    //proxy.On<ApiNoteModel>("updateNote", OnUpdateNote),
                    //proxy.On<Guid>("deleteNote", OnDeleteNote),

                    // Task
                    //proxy.On<ApiTaskModel>("updateTask", OnUpdateTask),
                    //proxy.On<Guid>("deleteTask", OnDeleteTask),

                    // File
                    //proxy.On<List<ApiFileModel>>("updateFiles", OnUpdateFiles),
                    //proxy.On<Guid>("deleteFile", OnDeleteFile),

                    // Suggestions
                    //proxy.On<ApiSuggestionBoxModel>("updateSuggestion", OnUpdateSuggestion),
                    //proxy.On<Guid>("deleteSuggestion", OnDeleteSuggestion),
                    //proxy.On<ApiSuggestionBoxCategoryModel>("updateSuggestionCategory", OnUpdateSuggestionCategory),
                    //proxy.On<Guid>("deleteSuggestionCategory", OnDeleteSuggestionCategory),
                };


            return disposables;
        }

        public ConnectionState ConnectionState { get; private set; }

        public event Action<StateChange> ConnectionStateChanged;

        public event Action Reconnected;

        public CancellationTokenSource ReconnectTokenSource { get; private set; }

        public DateTime LastDisconnection { get; private set; }

        #endregion Properties

        #region SignalR lifetime events

        private void ConnectionSlow()
        {
            LogMessage("Connection Slow");
        }

        private void ConnectionClosed()
        {
            LogMessage("Connection Closed");
        }

        private void ConnectionError(Exception ex)
        {
            LogMessage("Connection Error");
            LogMessage(ex.ToString());
        }

        /// <summary>
        /// Set to true to show user online debug output from signalr, false if you hate spam
        /// </summary>
        public bool ShowUserOnline { get; set; }

        private void ConnectionRecieved(string message)
        {
            if (!ShowUserOnline && !message.Contains("userOnline") || ShowUserOnline)
            {
                LogMessage($"Connection Recieved : {message}");
            }
        }

        private void ConnectionReconnecting()
        {
            LogMessage("Connection Reconnecting");
        }

        #endregion SignalR lifettime events

        #region Helpers

        protected void Execute<T>(T handlers, Action<T> action) where T : class
        {
            Task.Factory.StartNew(() =>
            {
                if (handlers != null)
                {
                    try
                    {
                        action(handlers);
                    }
                    catch (Exception ex)
                    {
                        _exceptionService.HandleException(ex);
                    }
                }
            });
        }

        private void LogMessage(string message)
        {
            Debug.WriteLine($"SIGNALR : {message}");
        }

        #endregion Helpers

        #region Events

        #region User
        
        public event Action<ApiWhitelabelPrivateUserModel> UpdatePersonalProfile;

        private void OnUpdatePersonalProfile(ApiWhitelabelPrivateUserModel model)
        {
            Execute(UpdatePersonalProfile, action => action(model));
        }

        public event Action<ApiWhitelabelPublicUserModel> UserOnline;

        private void OnUserOnline(ApiWhitelabelPublicUserModel publicUserModel)
        {
            Execute(UserOnline, action => action(publicUserModel));
        }

        #endregion User

        #region Company		

        public event Action<Guid> DeleteCompany;

        private void OnDeleteCompany(Guid companyId)
        {
            Execute(DeleteCompany, action => action(companyId));
        }

        public event Action<ApiWhitelabelCompanyModel> UpdateCompany;

        private void OnUpdateCompany(ApiWhitelabelCompanyModel companyModel)
        {
            Execute(UpdateCompany, action => action(companyModel));
        }

        #endregion Company

        #region Event

        public event Action<Guid> DeleteEvent;

        private void OnDeleteEvent(Guid eventId)
        {
            Execute(DeleteEvent, action => action(eventId));
        }

        public event Action<ApiWhitelabelEventModel> UpdateEvent;

        private void OnUpdateEvent(ApiWhitelabelEventModel eventModel)
        {
            Execute(UpdateEvent, action => action(eventModel));
        }

        #endregion Event

        #region Group

        public event Action<ApiWhitelabelGroupModel> UpdateGroup;

        private void OnUpdateGroup(ApiWhitelabelGroupModel groupModel)
        {
            Execute(UpdateGroup, action => action(groupModel));
        }

        #endregion Group

        #region Feed        

        public event Action<Guid> DeleteFeedPost;

        private void OnDeleteFeedPost(Guid feedPostId)
        {
            Execute(DeleteFeedPost, action => action(feedPostId));
        }

        public event Action<ApiWhitelabelFeedPostModel> NewFeedPost;

        private void OnUpdateFeedPost(ApiWhitelabelFeedPostModel feedPostModel)
        {
            Execute(NewFeedPost, action => action(feedPostModel));
        }

        #endregion Feed

        #region Chat        

        public event Action<ApiCentralPrivateChatMessageModel> NewPrivateMessage;

        private void OnNewPrivateMessage(ApiCentralPrivateChatMessageModel privateMessage)
        {
            Execute(NewPrivateMessage, action => action(privateMessage));
        }

        public event Action<Guid> StartTypingPrivate;

        private void OnStartTypingPrivate(Guid userId)
        {
            Execute(StartTypingPrivate, action => action(userId));
        }

        public event Action<Guid> StopTypingPrivate;

        private void OnStopTypingPrivate(Guid userId)
        {
            Execute(StopTypingPrivate, action => action(userId));
        }

        public async Task StartTypingPrivateAsync(Guid userId)
        {
            await _hubProxy.Invoke("StartTypingPrivate", userId);
        }

        public async Task StopTypingPrivateAsync(Guid userId)
        {
            await _hubProxy.Invoke("StopTypingPrivate", userId);
        }

        #endregion Chat

        #region Notifications

        public event Action<Guid> DeleteNotification;

        private void OnDeleteNotification(Guid notificationId)
        {
            Execute(DeleteNotification, action => action(notificationId));
        }

        public event Action<ApiWhitelabelNotificationModel, ApiRelatedItemsModel> UpdateNotification;
        public void ProcessNotificationMessage(Guid conversationId, Guid messageId, Guid userId, string text)
        {
            Debug.WriteLine("PUSH NOTIFICATION RECIEVED BUT IS NOT IMPLEMENTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        private void OnUpdateNotification(ApiWhitelabelNotificationModel notificationModel, ApiRelatedItemsModel relatedItemsModel)
        {
            Execute(UpdateNotification, action => action(notificationModel, relatedItemsModel));
        }

        #endregion Notifications

        #region Workspace

        #endregion Workspace

        #region Note

        #endregion Note

        #region Task

        #endregion Task

        #region File

        #endregion File

        #region Suggestions

        #endregion Suggestions

        #endregion Events
    }
}