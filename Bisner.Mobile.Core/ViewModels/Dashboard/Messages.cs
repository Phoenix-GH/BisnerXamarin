using System;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Dashboard
{
    public class JoinGroupMessage : MvxMessage
    {
        public JoinGroupMessage(object sender) : base(sender)
        {
        }

        public Guid GroupId { get; set; }

        public bool IsJoining { get; set; }
    }
}
