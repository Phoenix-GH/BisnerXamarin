using System;
using MvvmCross.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.Chat
{
    public class ConversationListViewModel : MvxViewModel
    {
        #region Constructor

        private DateTime _lastMessageDateTime;
        private string _lastMessage;
        private string _avatarUrl;
        private string _displayName;
        private bool _fromMe;
        private bool _isRead;

        private int _numberUnread;

        #endregion Constructor

        public Guid Id { get; set; }

        #region Message

        public bool FromMe
        {
            get => _fromMe;
            set { _fromMe = value; RaisePropertyChanged(() => FromMe); RaisePropertyChanged(() => LastMessage); }
        }

        public DateTime LastMessageDateTime
        {
            get => _lastMessageDateTime;
            set { _lastMessageDateTime = value; RaisePropertyChanged(() => LastMessageDateTime); }
        }

        public string LastMessage
        {
            get => _lastMessage;
            set
            {
                var message = FromMe ? $"You : {value}" : value;
                _lastMessage = message;
                RaisePropertyChanged(() => LastMessage);
            }
        }

        public int NumberUnread
        {
            get => _numberUnread;
            set { _numberUnread = value; RaisePropertyChanged(() => NumberUnread); RaisePropertyChanged(() => HasUnreadMessages); }
        }

        public bool HasUnreadMessages => _numberUnread > 0;

        #endregion Message

        #region DisplayUser

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

        public bool IsRead
        {
            get => _isRead;
            set { _isRead = value; RaisePropertyChanged(() => IsRead); }
        }

        #endregion DisplayUser
    }
}