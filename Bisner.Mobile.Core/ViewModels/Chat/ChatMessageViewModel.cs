using System;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class ChatMessageViewModel : MvxViewModel
    {
        private string _avatarUrl;
        private string _displayName;
        private DateTime _dateTime;
        private string _message;

        #region Constructor

        public ChatMessageViewModel()
        {

        }

        #endregion Constructor

        #region Properties

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { _avatarUrl = value; RaisePropertyChanged(() => AvatarUrl); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; RaisePropertyChanged(() => DisplayName); }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; RaisePropertyChanged(() => DateTime); }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(() => Message); }
        }

        #endregion Properties
    }
}