using System;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Members
{
    public class AttendEventMessage : MvxMessage
    {
        public AttendEventMessage(object sender) : base(sender)
        {
        }

        public Guid EventId { get; set; }
        public bool IsAttending { get; set; }
    }
}
