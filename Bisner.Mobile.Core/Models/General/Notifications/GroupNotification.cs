using System;
using Bisner.Mobile.Core.ViewModels.Feed;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public class GroupNotification : NotificationBase
    {
        public GroupNotification(Guid groupId)
        {
            Groupid = groupId;
        }
        
        public Guid Groupid { get; set; }

        public override void ShowRelatedItem()
        {
            base.ShowRelatedItem();

            ShowViewModel<FeedViewModel>(new { id = Groupid, feedType = FeedType.Group });
        }
    }
}