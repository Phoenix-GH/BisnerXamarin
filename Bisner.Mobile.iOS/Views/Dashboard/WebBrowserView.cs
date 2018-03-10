
using System;
using System.Linq;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Views.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    [MvxFromStoryboard]
    public partial class WebBrowserView : HideTabBarViewBase<WebBrowserViewModel>
    {
        public WebBrowserView(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //SetSwapItem();
            // Perform any additional setup after loading the view, typically from a nib.

            var set = this.CreateBindingSet<WebBrowserView, WebBrowserViewModel>();
            set.Bind(webView).For("WebviewUrl").To(vm => vm.Url);
            //set.Bind(_swapButton).To(vm => vm.SwapCommand);
            set.Apply();
        }

        private UIBarButtonItem _swapButton;

        private void SetSwapItem()
        {
            if (Settings.UserRoles.All(r => r != Home.Feed.Create.ToLower()))
                return;

            if (_swapButton == null)
            {
                using (
                    var createImage =
                        UIImage.FromBundle("Icons/create_post_btn.png")
                            .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
                {
                    _swapButton = new UIBarButtonItem(createImage, UIBarButtonItemStyle.Plain, null, null);
                }
            }

            NavigationItem.SetRightBarButtonItem(_swapButton, true);
        }

        #endregion

        protected override bool EnableCustomBackButton => true;

        protected override bool EnableTitleBarLogo => true;

        protected override bool SupportsHideTabBar => true;

        protected override NSLayoutConstraint TabBarTopConstraint => WebBottomConstraint;
    }
}