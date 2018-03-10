using System;
using Bisner.Mobile.Core.Extensions;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.Chat
{
    public class ChatMessage : ItemBase, IChatMessage
    {
        #region Constructor

        private string _displayName;
        private string _avatarUrl;
        private bool _isRead;
        private bool _sendFail;

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Id of the conversation this message is a part of
        /// </summary>
        public Guid ConversationId { get; set; }

        /// <summary>
        /// Sender message id for message identification
        /// </summary>
        public string SenderMessageId { get; set; }

        /// <summary>
        /// Datetime of message
        /// </summary>
        public DateTime DateTime { get; set; }

        private string _dateTimeText;
        public string DateTimeText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dateTimeText))
                {
                    _dateTimeText = DateTime.ToChatTime();
                }

                return _dateTimeText;
            }
        }

        /// <summary>
        /// Text of message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is read
        /// </summary>
        public bool IsRead
        {
            get => _isRead;
            set { _isRead = value; RaisePropertyChanged(() => IsRead); }
        }

        public Guid UserId { get; set; }

        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public bool SendFail
        {
            get => _sendFail;
            set { _sendFail = value; RaisePropertyChanged(() => SendFail); }
        }

        public void Update(IChatMessage message)
        {
            Id = message.Id;
            IsRead = message.IsRead;
            DateTime = message.DateTime;
            Text = message.Text;
            IsRead = message.IsRead;
            ConversationId = message.ConversationId;
        }

        #endregion Properties
    }
}