using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bisner.ApiModels.Central;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Chat;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.Core.ViewModels.Feed;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    /// <summary>
    /// ViewModel for the chat
    /// </summary>
    public class ChatViewModel : BaseViewModel
    {
        #region Constructor

        private ObservableCollection<ConversationListViewModel> _conversations;
        private MvxCommand _createChatCommand;
        private MvxCommand<ConversationListViewModel> _conversationSelectedCommand;

        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatViewModel(IPlatformService platformService, IUserService userService, IChatService chatService) : base(platformService)
        {
            _userService = userService;
            _chatService = chatService;

            CreateChatCommand = new MvxCommand(CreateChat, CanCreateChat);
            ConversationSelectedCommand = new MvxCommand<ConversationListViewModel>(ConversationSelected);

            _newPrivateMessageHandler = async chatMessageModel => await AddOrUpdateConversation(chatMessageModel);
        }

        #endregion Constructor

        #region Init

        public async Task Init()
        {
            try
            {
                var bisnerClient = Mvx.Resolve<ISignalRClient>();

                bisnerClient.NewPrivateMessage += _newPrivateMessageHandler;

                await GetConversationsAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        private readonly Action<ApiCentralPrivateChatMessageModel> _newPrivateMessageHandler;

        #endregion Init

        #region Base

        public override void Unattach()
        {
            base.Unattach();

            var bisnerClient = Mvx.Resolve<ISignalRClient>();

            bisnerClient.NewPrivateMessage -= _newPrivateMessageHandler;
        }

        #endregion Base

        #region Conversations

        private async Task GetConversationsAsync()
        {
            try
            {
                // Get data from services
                var messageModels = await _chatService.GetLastMessagesAsync(ApiPriority.UserInitiated);
                var unreadMessageModels = await _chatService.GetUnreadAsync(ApiPriority.UserInitiated);
                var publicUserModels = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);

                SendUnreadChatMessagesUpdate(unreadMessageModels.Count);

                var ownId = Settings.UserId;

                var conversationViewModels = new List<ConversationListViewModel>();

                foreach (var messageModel in messageModels)
                {
                    // Get the conversation ID
                    var conversationId = messageModel.FromId != ownId ? messageModel.FromId : messageModel.ToId;

                    // Get the user
                    var userModel = publicUserModels.FirstOrDefault(u => u.Id == conversationId);

                    // Check if message is unread
                    var unreadMessage = unreadMessageModels.FirstOrDefault(m => m.Id == messageModel.Id);

                    if (userModel != null)
                    {
                        var conversationViewModel = GetConversation(unreadMessage ?? messageModel, userModel);

                        if (conversationViewModel != null)
                        {
                            // Get all unread messages for the count
                            conversationViewModel.NumberUnread =
                                unreadMessageModels.Count(m => m.FromId == conversationViewModel.Id ||
                                                               m.ToId == conversationViewModel.Id);

                            conversationViewModels.Add(conversationViewModel);
                        }
                    }
                }

                var orderedConversations = conversationViewModels.OrderByDescending(c => c.LastMessageDateTime).ToList();

                Conversations = new ExtendedObservableCollection<ConversationListViewModel>(orderedConversations);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
                Conversations = new ExtendedObservableCollection<ConversationListViewModel>(new List<ConversationListViewModel>());
            }
        }

        private static ConversationListViewModel GetConversation(ApiCentralPrivateChatMessageModel chatMessageModel, ApiWhitelabelPublicUserModel userModel)
        {
            if (chatMessageModel == null)
            {
                return null;
            }

            // Conversation id is always the other user's id
            var conversation = new ConversationListViewModel
            {
                Id = userModel.Id,
                DisplayName = userModel.DisplayName,
                UserId = userModel.Id,
                AvatarUrl = userModel.Avatar?.Small,
                FromMe = userModel.Id == Settings.UserId,
                IsRead = chatMessageModel.IsRead,
                LastMessage = chatMessageModel.Text,
                LastMessageDateTime = chatMessageModel.DateTime,
            };

            return conversation;
        }

        public ObservableCollection<ConversationListViewModel> Conversations
        {
            get => _conversations;
            private set { _conversations = value; RaisePropertyChanged(() => Conversations); }
        }

        public MvxCommand<ConversationListViewModel> ConversationSelectedCommand
        {
            get => _conversationSelectedCommand;
            private set { _conversationSelectedCommand = value; RaisePropertyChanged(() => ConversationSelectedCommand); }
        }

        private bool _isSelecting;

        public void ConversationSelected(ConversationListViewModel chatConversationViewModelChatConversationViewModel)
        {
            if (!_isSelecting)
            {
                _isSelecting = true;

                // Show correct viewmodel
                ShowViewModel<ChatConversationViewModel>(new { id = chatConversationViewModelChatConversationViewModel.UserId });

                // Send update info for number unread messages
                Mvx.Resolve<IMvxMessenger>().Publish(new UpdateUnreadChatMessagesMessage(this));

                // Set conversation number unread to 0
                chatConversationViewModelChatConversationViewModel.NumberUnread = 0;

                _isSelecting = false;
            }
        }

        #endregion Conversations

        #region Conversation

        private async Task AddOrUpdateConversation(ApiCentralPrivateChatMessageModel chatMessageModel)
        {
            // Get the conversation
            var conversation = Conversations.FirstOrDefault(c => c.UserId == chatMessageModel.FromId || c.UserId == chatMessageModel.ToId);

            if (conversation != null)
            {
                UpdateConversation(conversation, chatMessageModel);

                //var closestConversation = Conversations.Where(c => c.Id != conversation.Id)
                //    .OrderBy(t => Math.Abs((t.LastMessageDateTime - conversation.LastMessageDateTime).Ticks))
                //    .FirstOrDefault();

                //// Can happen when there is only 1 conversation
                //if (closestConversation == null)
                //    return;

                //var closestConversationIndex = Conversations.IndexOf(closestConversation);
                //int newIndex;

                //if (conversation.LastMessageDateTime > closestConversation.LastMessageDateTime)
                //{
                //    // Younger then
                //    newIndex = closestConversationIndex;
                //}
                //else
                //{
                //    // Older then
                //    newIndex = closestConversationIndex + 1;
                //}

                //var currentIndex = Conversations.IndexOf(conversation);

                //if (currentIndex != newIndex && currentIndex != newIndex - 1)
                //{
                InvokeOnMainThread(() =>
                {
                    Conversations.Move(Conversations.IndexOf(conversation), 0);
                });
                //}
            }
            else
            {
                // New conversation
                var conversationId = chatMessageModel.FromId == Settings.UserId
                    ? chatMessageModel.ToId
                    : chatMessageModel.FromId;

                var userModel = await _userService.GetUserAsync(conversationId, ApiPriority.Background);

                conversation = GetConversation(chatMessageModel, userModel);

                // Can be null when user is not found or unknown
                if (conversation != null)
                {
                    InvokeOnMainThread(() =>
                    {
                        Conversations.Insert(0, conversation);
                    });
                }
            }
        }

        private void UpdateConversation(ConversationListViewModel conversationListViewModel, ApiCentralPrivateChatMessageModel chatMessageModel)
        {
            conversationListViewModel.FromMe = chatMessageModel.FromId == Settings.UserId;
            conversationListViewModel.LastMessage = chatMessageModel.Text;
            conversationListViewModel.LastMessageDateTime = chatMessageModel.DateTime;
            conversationListViewModel.IsRead = chatMessageModel.IsRead;

            if (!chatMessageModel.IsRead)
            {
                conversationListViewModel.NumberUnread++;
            }
            else
            {
                conversationListViewModel.NumberUnread = 0;
            }
        }

        #endregion Conversation

        #region Create

        public MvxCommand CreateChatCommand
        {
            get => _createChatCommand;
            private set { _createChatCommand = value; RaisePropertyChanged(() => CreateChatCommand); }
        }

        private void CreateChat()
        {
            ShowViewModel<CreateChatViewModel>();
        }

        private bool CanCreateChat()
        {
            return true;
        }

        #endregion Create

        #region Refresh

        private MvxCommand _refreshCommand;
        private bool _isRefreshing;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new MvxCommand(async () => await RefreshAsync()));
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            private set { _isRefreshing = value; RaisePropertyChanged(() => IsRefreshing); }
        }

        private async Task RefreshAsync()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;

                await GetConversationsAsync();

                IsRefreshing = false;
            }
        }

        #endregion Refresh
    }
}
