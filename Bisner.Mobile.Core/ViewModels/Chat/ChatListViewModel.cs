using System;
using Cirrious.MvvmCross.ViewModels;

namespace Bisner.Mobile.Core.ViewModels.Chat
{
    public class ChatListViewModel : MvxViewModel
    {
        #region Constructor

        private string _title;
        private DateTime _lastMessageDateTime;
        private string _lastMessage;

        #endregion Constructor

        #region Properties

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        public DateTime LastMessageDateTime
        {
            get { return _lastMessageDateTime; }
            set { _lastMessageDateTime = value; RaisePropertyChanged(() => LastMessageDateTime); }
        }

        public string LastMessage
        {
            get { return _lastMessage; }
            set { _lastMessage = value; RaisePropertyChanged(() => LastMessage); }
        }

        #endregion Properties
    }
}