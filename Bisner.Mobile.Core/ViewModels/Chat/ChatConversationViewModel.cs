using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bisner.ApiModels.Central;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Chat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bisner.ApiModels.Whitelabel;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class ChatConversationViewModel : BaseViewModel
    {
        #region Constructor

        private static int _numberAlive;

        ~ChatConversationViewModel()
        {
            _numberAlive--;
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!DESTROYING!!!!!!!!!!!!!!!!!!!!! - ChatConversationViewModel - number alive : {0}", _numberAlive);
        }

        private ObservableCollection<IChatItem> _messages;
        private string _input;
        private MvxCommand _sendCommand;
        private string _avatarUrl;

        private bool _isLoading;
        private bool _isBusy;
        private bool _isTyping;

        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly ISignalRClient _signalRClient;

        private ApiWhitelabelPrivateUserModel _personalModel;
        private List<ApiWhitelabelPublicUserModel> _conversationUsers;

        public ChatConversationViewModel(IPlatformService platformService, IUserService userService, IChatService chatService, ISignalRClient signalRClient) : base(platformService)
        {
            _numberAlive++;
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!CREATING!!!!!!!!!!!!!!!!!!!!! - ChatConversationViewModel - number alive : {0}", _numberAlive);

            _userService = userService;
            _chatService = chatService;
            _signalRClient = signalRClient;

            SendCommand = new MvxCommand(async () => await SendMessage(), CanSendMessage);
            UserCommandEnableChat = false;
            _addMessageHandler = async chatMessageModel => await AddMessage(chatMessageModel);
        }

        #endregion Constructor

        #region Init 

        public async Task Init(Guid id)
        {
            try
            {
                _personalModel = await _userService.GetPersonalModelAsync(ApiPriority.UserInitiated);
                var messageModels = await _chatService.GetConversationAsync(ApiPriority.UserInitiated, id, null);
                var userModels = await _userService.GetAllPublicUsersAsync(ApiPriority.UserInitiated);
                await _chatService.SetHasReadAsync(ApiPriority.UserInitiated, id);

                UpdateCollection(messageModels, userModels);

                // TODO : Only 1 user for now because no group chat
                var user = userModels.FirstOrDefault(u => u.Id == id);

                // TODO : Move unknown user to user service?
                Title = user != null ? user.DisplayName : "Unknown user";
                Id = id;
                AvatarUrl = user?.Avatar?.Small;
                UserId = id;
                _conversationUsers = new List<ApiWhitelabelPublicUserModel> { user };
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion Init

        #region IEventViewModel

        private readonly Action<ApiCentralPrivateChatMessageModel> _addMessageHandler;

        public override void Attach()
        {
            base.Attach();

            _signalRClient.NewPrivateMessage += _addMessageHandler;
            _signalRClient.StartTypingPrivate += StartTyping;
            _signalRClient.StopTypingPrivate += StopTyping;
        }

        public override void Unattach()
        {
            base.Unattach();

            _signalRClient.NewPrivateMessage -= _addMessageHandler;
            _signalRClient.StartTypingPrivate -= StartTyping;
            _signalRClient.StopTypingPrivate -= StopTyping;
        }

        #endregion IEventViewModel

        #region SignalR

        private void StartTyping(Guid userId)
        {
            if (userId == Id)
            {
                IsTyping = true;
            }
        }

        private void StopTyping(Guid userId)
        {
            if (userId == Id)
            {
                IsTyping = false;
            }
        }

        #endregion SignalR

        #region Collection

        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        private bool _isUpdatingConversation;

        public async Task UpdateConversation()
        {
            if (!_isUpdatingConversation)
            {
                _isUpdatingConversation = true;

                try
                {
                    await _chatService.SetHasReadAsync(ApiPriority.UserInitiated, Id);

                    IsLoading = true;

                    var messageModels = await _chatService.GetConversationAsync(ApiPriority.Background, Id, DateTime.MaxValue);

                    // Check for new messages
                    var lastMessageDateTime = Messages.Max(m => m.DateTime);

                    var newMessages = messageModels.Where(m => m.DateTime > lastMessageDateTime).ToList();

                    // Update list
                    UpdateCollection(newMessages, _conversationUsers);

                    Mvx.Resolve<IMvxMessenger>().Publish(new ChatConversationUpdated(this));
                }
                catch (Exception ex)
                {
                    Mvx.Resolve<IExceptionService>().HandleException(ex);
                }
                finally
                {
                    _isUpdatingConversation = false;
                }
            }

            await SendMarkAsRead();
        }

        private void UpdateCollection(List<ApiCentralPrivateChatMessageModel> chatMessageModels, List<ApiWhitelabelPublicUserModel> userModels)
        {
            var groups = BuildGroups(GetChatMessages(chatMessageModels, userModels));

            InvokeOnMainThread(() =>
            {
                Messages = groups;

                RaisePropertyChanged(() => Messages);
                //Mvx.Resolve<IMvxMessenger>().Publish(new ChatConversationAddMessage(this) { Animated = false });
                Mvx.Resolve<IMvxMessenger>().Publish(new ChatConversationUpdated(this));
            });
        }

        private Guid _unreadMessagesLabelId;

        private ObservableCollection<IChatItem> BuildGroups(List<IChatMessage> messages)
        {
            // The final list
            var finalList = new List<IChatItem>();

            // Group all read messages by date
            var groupList = messages.Where(m => m.IsRead).GroupBy(m => m.DateTime.Date).OrderBy(g => g.Key);

            // Add groups to final list
            foreach (var group in groupList)
            {
                // Build label
                var label = new ChatLabel { Id = Guid.NewGuid(), DateTime = group.Key };

                finalList.Add(label);
                finalList.AddRange(group.OrderBy(m => m.DateTime));
            }

            // Add unread messages if there are any
            var unreadMessages = new List<IChatMessage>(messages.Where(m => !m.IsRead));

            if (unreadMessages.Count > 0)
            {
                // Add label and set unread messages label id
                var label = new ChatLabel { Id = _unreadMessagesLabelId = Guid.NewGuid(), Text = "New messages" };

                finalList.Add(label);
                finalList.AddRange(unreadMessages.OrderBy(m => m.DateTime));
            }

            return new ObservableCollection<IChatItem>(finalList);
        }

        private bool UpdateGroups(IEnumerable<IChatMessage> messages)
        {
            var newMessages = messages.Where(x => Messages.All(y => x.Id != y.Id)).OrderBy(m => m.DateTime).ToList();

            Debug.WriteLine("{0} new messages", newMessages.Count);

            if (newMessages.Count > 0)
            {
                foreach (var chatMessage in newMessages)
                {
                    AddMessage(chatMessage);
                }

                return true;
            }

            return false;
        }

        #endregion Collection

        #region Conversation

        public Guid Id { get; private set; }

        public ObservableCollection<IChatItem> Messages
        {
            get => _messages;
            set { _messages = value; RaisePropertyChanged(() => Messages); }
        }

        private async Task AddMessage(ApiCentralPrivateChatMessageModel model)
        {
            // Check if message is from this conversation
            if (model.FromId != Id && model.ToId != Id)
                return;

            // Message is read because screen is open
            model.IsRead = true;
            await SendMarkAsRead();

            // TODO : Fix for multiple users in 1 conversation
            var user = _conversationUsers.FirstOrDefault(u => u.Id == Id);

            AddMessage(GetChatMessage(model, user));
        }

        private void AddMessage(IChatMessage message)
        {
            InvokeOnMainThread(() =>
            {
                // Check label for unread messages
                var unreadLabel = Messages.FirstOrDefault(l => l is IChatLabel && l.Id == _unreadMessagesLabelId);

                if (unreadLabel != null)
                {
                    // Get all that are unread
                    var unreadItems = Messages.Where(m => m is IChatMessage).Cast<IChatMessage>().Where(m => !m.IsRead);

                    var groupedByDate = unreadItems.GroupBy(i => i.DateTime.Date);

                    foreach (var group in groupedByDate)
                    {
                        // Check label and add
                        var groupLabel = Messages.FirstOrDefault(l => l is IChatLabel && l.DateTime.Date == group.Key.Date);

                        if (groupLabel == null)
                        {
                            groupLabel = new ChatLabel { Id = Guid.NewGuid(), DateTime = group.Key.Date };

                            // Find index of first message of this date
                            var indexOfFirstMessage = Messages.IndexOf(Messages.FirstOrDefault(m => m.DateTime.Date == group.Key.Date));

                            Messages.Insert(indexOfFirstMessage, groupLabel);
                        }
                    }

                    Messages.Remove(unreadLabel);
                }

                // Check if label for date has already been added
                var label = Messages.FirstOrDefault(m => m is IChatLabel && m.DateTime.Date == message.DateTime.Date);

                if (label == null)
                {
                    // Add label
                    Messages.Add(new ChatLabel { Id = Guid.NewGuid(), DateTime = message.DateTime.Date });
                }

                var oldMessage = Messages.FirstOrDefault(m => (m.SenderMessageId == message.SenderMessageId && !string.IsNullOrEmpty(m.SenderMessageId)) || m.Id == message.Id && m.Id != Guid.Empty && m is IChatMessage);

                if (oldMessage != null)
                {
                    // Update old message
                    var chatMessage = oldMessage as IChatMessage;
                    chatMessage?.Update(message);
                }
                else
                {
                    // Add message
                    AddMessageToList(message);
                }

                RaisePropertyChanged(() => Messages);
                Mvx.Resolve<IMvxMessenger>().Publish(new ChatConversationAddMessage(this) { Animated = true });
            });
        }

        private void AddMessageToList(IChatMessage message)
        {
            var closestMessage = Messages.Where(c => c.Id != message.Id).OrderBy(t => Math.Abs((t.DateTime - message.DateTime).Ticks)).First();

            var closestMessageIndex = Messages.IndexOf(closestMessage);
            int newIndex;

            if (message.DateTime > closestMessage.DateTime)
            {
                // Younger then
                newIndex = closestMessageIndex;
            }
            else
            {
                // Older then
                newIndex = closestMessageIndex + 1;
            }

            var currentIndex = Messages.IndexOf(message);

            if (currentIndex == -1)
            {
                Messages.Add(message);
            }
            else if (currentIndex != newIndex && currentIndex != newIndex - 1)
            {
                //Messages.RemoveAt(currentIndex);
                //Messages.Insert(newIndex, message);
                Messages.Move(currentIndex, newIndex);
            }
        }

        public bool IsTyping
        {
            get => _isTyping;
            private set { _isTyping = value; RaisePropertyChanged(() => IsTyping); }
        }

        #endregion Conversation

        #region Send

        public MvxCommand SendCommand
        {
            get => _sendCommand;
            private set { _sendCommand = value; RaisePropertyChanged(() => SendCommand); }
        }

        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                SendCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => Input);
            }
        }

        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private async Task SendMessage()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                SendCommand.RaiseCanExecuteChanged();
            }

            IsBusy = true;

            IChatMessage message = new ChatMessage
            {
                DateTime = DateTime.UtcNow,
                UserId = Settings.UserId,
                IsRead = true,
                Text = Input,
                AvatarUrl = _personalModel.Avatar?.Small,
                ConversationId = Id,
                DisplayName = _personalModel?.DisplayName,
                SenderMessageId = Guid.NewGuid().ToString(),
            };

            var text = Input;

            Input = "";

            AddMessage(message);

            try
            {
                await _chatService.SendMessageAsync(Id, text, message.SenderMessageId);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }

            SendCommand.RaiseCanExecuteChanged();

            IsBusy = false;
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(Input) && CrossConnectivity.Current.IsConnected;
        }

        #endregion Send

        #region MarkAsRead

        private async Task SendMarkAsRead()
        {
            try
            {
                await _chatService.SetHasReadAsync(ApiPriority.Background, Id);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleException(ex);
            }
        }

        #endregion MarkAsRead

        #region Resources

        public string SendButtonText => GetResource(ResKeys.mobile_chat_btn_send);

        public string InputPlaceholder => GetResource(ResKeys.mobile_chat_input_placeholder);

        #endregion Resources

        #region ToModel

        private List<IChatMessage> GetChatMessages(List<ApiCentralPrivateChatMessageModel> messageModels, List<ApiWhitelabelPublicUserModel> publicUserModels)
        {
            var messages = new List<IChatMessage>();

            foreach (var messageModel in messageModels)
            {
                // Get the user
                var userModel = publicUserModels.FirstOrDefault(u => u.Id == messageModel.FromId);

                messages.Add(GetChatMessage(messageModel, userModel));
            }

            return messages;
        }

        private IChatMessage GetChatMessage(ApiCentralPrivateChatMessageModel chatMessageModel, ApiWhitelabelPublicUserModel userModel)
        {
            // TODO : Unkown user
            if (chatMessageModel == null)
            {
                return null;
            }

            return new ChatMessage
            {
                Id = chatMessageModel.Id,
                Text = chatMessageModel.Text,
                DateTime = chatMessageModel.DateTime,
                IsRead = chatMessageModel.IsRead,
                SenderMessageId = chatMessageModel.SenderMessageId,
                ConversationId = chatMessageModel.FromId == Settings.UserId ? chatMessageModel.ToId : chatMessageModel.FromId,
                DisplayName = userModel.DisplayName,
                UserId = userModel.Id,
                AvatarUrl = userModel.Avatar?.Small,
                SendFail = false,
            };
        }

        #endregion ToModel
    }
}