using System;
using Bisner.Mobile.Core.Models.Base;

namespace Bisner.Mobile.Core.Models.General.Notifications
{
    public interface INotification : IItemBase, IDisplayUser
    {
        Guid RelatedItemId { get; set; }

        string  NotificationType { get; set; }

        DateTime CreationDateTime { get; set; }

        DateTime? IsReadOnDateTime { get; set; }

        bool IsRead { get; set; }

        string Text { get; set; }

        string ExtraText { get; set; }

        void ShowRelatedItem();

        void Update(INotification notification);
    }
}