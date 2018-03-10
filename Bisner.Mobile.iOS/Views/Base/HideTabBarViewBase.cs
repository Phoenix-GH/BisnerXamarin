using System;
using System.Diagnostics;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Base
{
    public abstract class HideTabBarViewBase<TViewModel> : KeyboardListenerViewBase<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        protected static bool TabBarHidden = false;

        protected HideTabBarViewBase(IntPtr handle)
            : base(handle)
        {

        }

        #endregion Constructor

        #region ViewController

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (TabBarController != null && SupportsHideTabBar)
            {
                SetTabBarHidden(true, TabBarTopConstraint);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (TabBarController != null && SupportsHideTabBar)
            {
                SetTabBarHidden(false, TabBarTopConstraint);
            }
        }

        #endregion ViewController

        #region Tabbar

        protected virtual bool SupportsHideTabBar { get { return false; } }

        protected virtual NSLayoutConstraint TabBarTopConstraint { get { return null; } }

        protected void SetTabBarHidden(bool tabBarHidden, NSLayoutConstraint tabBarTopConstraint = null)
        {
            if (tabBarHidden == TabBarHidden)
                return;

            var offset = tabBarHidden ? TabBarController.TabBar.Frame.Size.Height : -TabBarController.TabBar.Frame.Size.Height;

            UIView.Animate(0.1, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.LayoutSubviews, () =>
            {
                View.SetNeedsUpdateConstraints();

                if (tabBarTopConstraint != null)
                {
                    tabBarTopConstraint.Constant -= offset;
                }

                TabBarController.TabBar.Center = new CGPoint(TabBarController.TabBar.Center.X, TabBarController.TabBar.Center.Y + (tabBarHidden ? offset + 1 : offset - 1));

                View.LayoutIfNeeded();
            }, () =>
            {
                TabBarHidden = tabBarHidden;
            });

            OnAfterTabbarHidden();
        }

        protected virtual void OnAfterTabbarHidden()
        {
            // Do stuff
        }

        #endregion Tabbar
    }
}