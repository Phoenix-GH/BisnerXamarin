using System;
using Bisner.Mobile.Core.Service;
using CoreGraphics;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Notifications
{
    public class NotificationAlertAlertWindow : UIWindow, INotificationAlertWindow
    {
        #region Constructor

        private NotificationAlertViewController _rootView;

        public NotificationAlertAlertWindow(CGRect frame) : base(frame)
        {
            SetupWindow();
        }

        private void SetupWindow()
        {
            RootViewController = _rootView = new NotificationAlertViewController(Mvx.Resolve<IUserService>())
            {
                NotificationShown = () =>
                {

                },
                NotificationHidden = () =>
                {
                    _isVisible = false;
                    Hidden = true;
                }
            };
            //_rootView.View.BackgroundColor= UIColor.Blue;
            Hidden = true;
            WindowLevel = UIWindowLevel.StatusBar + 1;
            UserInteractionEnabled = true;
        }

        #endregion Constructor

        #region Notification

        private bool _isVisible;

        public void ShowNotification(Guid conversationId, string text)
        {
            if (!_isVisible)
            {
                _isVisible = true;
                Hidden = false;

                _rootView.ShowNotification(conversationId, text);
            }
        }

        #endregion Notification
    }
}