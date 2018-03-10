using Bisner.Mobile.Core.ViewModels.Feed;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public class CompanyNotification : NotificationBase
    {
        public override void ShowRelatedItem()
        {
            base.ShowRelatedItem();

            ShowViewModel<FeedViewModel>(new { id = RelatedItemId, feedType = FeedType.Company });
        }
    }
}