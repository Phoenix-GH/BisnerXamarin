using Bisner.Mobile.Core.ViewModels.Feed;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public class FeedPostNotification : NotificationBase
    {
        public override void ShowRelatedItem()
        {
            base.ShowRelatedItem();

            ShowViewModel<DetailsViewModel>(new { postId = RelatedItemId });
        }
    }
}