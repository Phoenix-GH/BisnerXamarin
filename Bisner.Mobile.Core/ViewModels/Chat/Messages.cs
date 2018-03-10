using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class ChatConversationAddMessage : MvxMessage
    {
        #region Constructor

        public ChatConversationAddMessage(object sender) : base(sender)
        {
        }

        #endregion Constructor

        public bool Animated { get; set; }
    }

    public class ChatConversationUpdated : MvxMessage
    {
        public ChatConversationUpdated(object sender) : base(sender)
        {
        }
    }

    public class UpdateUnreadChatMessagesMessage : MvxMessage
    {
        public UpdateUnreadChatMessagesMessage(object sender) : base(sender)
        {
        }

        public int NumberUnread { get; set; }
    }
}