using System;
using System.Diagnostics;
using System.Timers;
using Bisner.Mobile.Core.Communication;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Chat;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using SDWebImage;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Notifications
{
    public class NotificationAlertViewController : UIViewController
    {
        #region Constructor

        private readonly IUserService _userService;

        public NotificationAlertViewController(IUserService userService)
        {
            _userService = userService;
            SetupNotificationView();
        }

        #endregion Constructor

        #region ViewController

        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("NOTIFICATIONALERTVIEW RECIEVED MEMORY WARNING!!");
        }

        #endregion ViewController

        #region NotificationView

        private UIView _notificationViewContainer, _bottomSwipHandle;
        private AvatarImageView _avatar;
        private UILabel _displayName, _text;
        private Timer _timer;

        private UITapGestureRecognizer _tap;
        private UISwipeGestureRecognizer _swipe;

        private void SetupNotificationView()
        {
            // Setup subviews
            _notificationViewContainer = new UIView
            {
                BackgroundColor = Appearance.Colors.NatificationPanel,
                Frame = new CGRect(0, -64, View.Frame.Width, 64)
            };

            _avatar = new AvatarImageView();
            _displayName = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = Appearance.Fonts.LatoBoldWithSize(14), Lines = 1 };
            _text = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = Appearance.Fonts.LatoWithSize(11.86f), Lines = 1, LineBreakMode = UILineBreakMode.TailTruncation };
            _bottomSwipHandle = new UIView { BackgroundColor = Appearance.Colors.White };
            _bottomSwipHandle.Layer.CornerRadius = 2.0f;

            // Gesture recognizers
            _tap = new UITapGestureRecognizer(gesture =>
            {
                if (AppDelegate.MainWindow.RootViewController.VisibleViewController().IsChatConversation())
                {
                    var chatConversation = (ChatConversationView)AppDelegate.MainWindow.RootViewController.VisibleViewController();

                    var viewModel = chatConversation.DataContext as ChatConversationViewModel;

                    if (viewModel != null && viewModel.Id != _conversationId)
                    {
                        chatConversation.DismissViewController(false, OpenChatView);
                    }

                    return;
                }

                if (AppDelegate.MainWindow.RootViewController.VisibleViewController() is ImageZoomView)
                {
                    var imageZoom = (ImageZoomView)AppDelegate.MainWindow.RootViewController.VisibleViewController();

                    imageZoom.DismissModalViewController(false);
                }

                if (AppDelegate.MainWindow.RootViewController.VisibleViewController() is UserView)
                {
                    var userView = (UserView)AppDelegate.MainWindow.RootViewController.VisibleViewController();

                    userView.DismissModalViewController(false);
                }

                OpenChatView();
            })
            { NumberOfTapsRequired = 1 };

            _swipe = new UISwipeGestureRecognizer(() =>
            {
                HideNotification();
            })
            { Direction = UISwipeGestureRecognizerDirection.Up };

            // Add gestures to views
            _notificationViewContainer.AddGestureRecognizer(_tap);
            _notificationViewContainer.AddGestureRecognizer(_swipe);
            _avatar.AddGestureRecognizer(_tap);
            _avatar.AddGestureRecognizer(_swipe);
            _displayName.AddGestureRecognizer(_tap);
            _displayName.AddGestureRecognizer(_swipe);
            _text.AddGestureRecognizer(_tap);
            _text.AddGestureRecognizer(_swipe);
            _bottomSwipHandle.AddGestureRecognizer(_swipe);

            // Enable user interaction
            _notificationViewContainer.UserInteractionEnabled = true;
            _avatar.UserInteractionEnabled = true;
            _displayName.UserInteractionEnabled = true;
            _text.UserInteractionEnabled = true;
            _bottomSwipHandle.UserInteractionEnabled = true;
            View.UserInteractionEnabled = true;



            // Add subviews to container
            _notificationViewContainer.AddSubviews(_avatar, _displayName, _text, _bottomSwipHandle);

            // Setup constraints
            _notificationViewContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            _notificationViewContainer.AddConstraints(
                _avatar.AtLeftOf(_notificationViewContainer, 10),
                _avatar.WithSameCenterY(_notificationViewContainer),
                _avatar.Width().EqualTo(40),
                _avatar.Height().EqualTo(40),

                _displayName.ToRightOf(_avatar, 10),
                _displayName.AtRightOf(_notificationViewContainer, 10),
                _displayName.WithSameCenterY(_avatar).Minus(8),

                _text.WithSameCenterY(_avatar).Plus(8),
                _text.WithSameLeft(_displayName),
                _text.WithSameRight(_displayName),

                _bottomSwipHandle.AtBottomOf(_notificationViewContainer, 2),
                _bottomSwipHandle.WithSameCenterX(_notificationViewContainer),
                _bottomSwipHandle.Width().EqualTo(20),
                _bottomSwipHandle.Height().EqualTo(5)
                );

            // Add container to viewdontroller
            View.AddSubviews(_notificationViewContainer);

            // Setup timer for hiding
            _timer = new Timer(3000) { AutoReset = false };
            _timer.Elapsed += (sender, args) =>
            {
                UIApplication.SharedApplication.InvokeOnMainThread(HideNotification);
            };
        }

        private bool _isShowing;
        private Guid _conversationId;

        public void ShowNotification(Guid conversationId, string text)
        {
            if (!_isShowing)
            {
                _conversationId = conversationId;
                _isShowing = true;

                InvokeOnMainThread(async () =>
                {
                    try
                    {
                        // Set conversation id for tap
                        var user = await _userService.GetUserAsync(conversationId, ApiPriority.Background);

                        _displayName.Text = user.DisplayName;
                        _text.Text = text;

                        if (user.Avatar != null)
                        {

                            SDWebImageDownloader.SharedDownloader.DownloadImage(new NSUrl(Settings.BlobUrl + user.Avatar.Small), SDWebImageDownloaderOptions.ProgressiveDownload, null,
                                (image, data, error, finished) =>
                                {
                                    if (image == null) return;

                                    _avatar.Image = image;

                                    var avatarButton = new UIBarButtonItem(_avatar);

                                    NavigationItem.SetRightBarButtonItem(avatarButton, true);
                                });
                        }
                        else
                        {
                                // Default avatar
                                _avatar.Image = UIImage.FromBundle("Icons/default_avatar.jpg");
                        }

                        Show();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });
            }
        }

        private void Show()
        {
            UIView.Animate(0.2, 0.0, UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                SetNotificationContainerY(+64);
            },
                () =>
                {
                    _timer.Start();
                    NotificationShown?.Invoke();
                });
        }

        private void SetNotificationContainerY(int amount)
        {
            var oldFrame = _notificationViewContainer.Frame;

            oldFrame.Y += amount;

            _notificationViewContainer.Frame = oldFrame;
        }

        public void HideNotification()
        {
            if (_isShowing)
            {
                _isShowing = false;

                UIView.Animate(0.2, () =>
                {
                    SetNotificationContainerY(-64);
                },
                () =>
                {
                    NotificationHidden?.Invoke();
                });
            }
        }

        private void OpenChatView()
        {
            MvxPresentationHint presentationHint = new ChatConversationHint { SelectedUser = _conversationId };

            Mvx.Resolve<IMvxViewDispatcher>().ChangePresentation(presentationHint);
        }

        public Action NotificationShown { get; set; }

        public Action NotificationHidden { get; set; }

        #endregion NotificationView

    }
}