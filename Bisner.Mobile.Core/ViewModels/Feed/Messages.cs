using System;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Feed
{
    public class NewCommentMessage : MvxMessage
    {
        #region Constructor

        public NewCommentMessage(object sender) : base(sender)
        {
        }

        #endregion Constructor
    }

    public class GroupJoinedMessage : MvxMessage
    {
        public GroupJoinedMessage(object sender) : base(sender)
        {
        }

        public Guid GroupId { get; set; }

        public bool HasJoined { get; set; }
    }
}
