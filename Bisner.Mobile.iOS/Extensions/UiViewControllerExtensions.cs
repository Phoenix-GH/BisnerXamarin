using System;
using System.Linq;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.iOS.Views.Chat;
using Bisner.Mobile.iOS.Views.Feed;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Extensions
{
    public static class UiViewControllerExtensions
    {
        public static bool IsChatConversation(this UIViewController viewController, Guid? conversationId = null)
        {
            if (viewController != null && viewController is ChatConversationView)
            {
                if (conversationId != null)
                {
                    var chatView = (ChatConversationView)viewController;

                    var viewModel = chatView.DataContext as ChatConversationViewModel;

                    if (viewModel != null && viewModel.Id == conversationId.Value)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool IsDetailsViewController(this UIViewController viewController, Guid postId)
        {
            if (viewController != null && viewController is DetailsView)
            {
                var detailsView = (DetailsView)viewController;

                var viewModel = detailsView.DataContext as DetailsViewModel;

                if (viewModel != null && viewModel.PostId == postId)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public static bool HasViewModelOftype<T>(this UIViewController controller) where T : MvxViewModel
        {
            var mvxView = controller as MvxViewController;

            if (mvxView != null)
            {
                var viewModel = mvxView.DataContext as T;

                if (viewModel != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static UIViewController VisibleViewController(this UIViewController rootViewController)
        {
            if (rootViewController is UINavigationController)
            {
                var navigationController = (UINavigationController)rootViewController;
                var lastViewController = navigationController.ViewControllers.Last();

                return VisibleViewController(lastViewController);
            }

            if (rootViewController is UITabBarController)
            {
                var tabBarController = (UITabBarController)rootViewController;
                var selectedViewController = tabBarController.SelectedViewController;

                return VisibleViewController(selectedViewController);
            }

            if (rootViewController.PresentedViewController == null)
            {
                return rootViewController;
            }

            return VisibleViewController(rootViewController.PresentedViewController);
        }

        public static void CenterViewInScroll(this UIScrollView scrollView, UIView viewToCenter, nfloat keyboardHeight)
        {
            UIView.Animate(0.2f, () =>
            {
                scrollView.Superview.SetNeedsLayout();

                var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);
                scrollView.ContentInset = contentInsets;
                scrollView.ScrollIndicatorInsets = contentInsets;

                // Position of the active field relative isnside the scroll view
                var relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

                var landscape = UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight;
                var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

                // Move the active field to the center of the available space
                var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
                scrollView.ContentOffset = new CGPoint(0, offset);

                scrollView.Superview.LayoutIfNeeded();
            });
        }

        public static void SetOverlay(this UIImageView imageView, nfloat alpha)
        {
            var overlay = new UIView(new CGRect(0, 0, imageView.Frame.Size.Width, imageView.Frame.Size.Height))
            {
                BackgroundColor = UIColor.Black.ColorWithAlpha(alpha),
            };

            imageView.AddSubviews(overlay);
        }
    }
}