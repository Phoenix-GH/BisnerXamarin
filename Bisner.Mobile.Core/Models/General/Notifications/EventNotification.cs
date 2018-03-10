using Bisner.Mobile.Core.ViewModels.Dashboard;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public class EventNotification : NotificationBase
    {
        public override void ShowRelatedItem()
        {
            base.ShowRelatedItem();

            ShowViewModel<EventViewModel>(new { id = RelatedItemId });
        }
    }
}
