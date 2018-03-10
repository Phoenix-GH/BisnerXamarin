using Bisner.Mobile.Core.ViewModels;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public class UserNotification : NotificationBase
    {
        public override void ShowRelatedItem()
        {
            base.ShowRelatedItem();

            ShowViewModel<UserViewModel>(new { userId = RelatedItemId });
        }
    }
}