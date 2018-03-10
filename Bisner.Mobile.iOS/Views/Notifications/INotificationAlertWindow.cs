using System;

namespace Bisner.Mobile.iOS.Views.Notifications
{
    public interface INotificationAlertWindow
    {

        /// <summary>
        /// Shows the notification view
        /// </summary>
        void ShowNotification(Guid conversationId, string text);
    }
}