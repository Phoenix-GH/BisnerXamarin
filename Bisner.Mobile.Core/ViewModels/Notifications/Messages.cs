using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Core.ViewModels.Notifications
{
    public class UpdateUnreadNotificationMessage : MvxMessage
    {
        public UpdateUnreadNotificationMessage(object sender) : base(sender)
        {
        }

        public int NumberUnread { get; set; }
    }
}
