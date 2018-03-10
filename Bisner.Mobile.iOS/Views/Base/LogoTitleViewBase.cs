using System;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Base
{
    public abstract class LogoTitleViewBase<TViewModel> : MvxViewController<TViewModel> where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        private UIButton _homeButton;
        private UIBarButtonItem _menuButton;

        protected LogoTitleViewBase(IntPtr handle)
            : base(handle)
        {

        }

        #endregion Constructor

        #region Overrides

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetTitleView();
            SetHamburgerIcon();
        }

        #endregion Overrides

        #region Helpers

        private void SetHamburgerIcon()
        {
            // Add hamburger icon to left of navigation bar
            var menuImage = UIImage.FromBundle("Icons/icon_nav_header.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            _menuButton = new UIBarButtonItem
            {
                Image = menuImage,
            };
            _menuButton.Clicked += (sender, args) =>
            {
                var mainPresenter = Mvx.Resolve<IMainViewPresenterHost>();
                mainPresenter.NavController.TogglePanel(PanelType.LeftPanel);
            };

            NavigationItem.SetLeftBarButtonItem(_menuButton, true);
        }

        private void SetTitleView()
        {
            // Add the bisner logo home button
            var image = UIImage.FromBundle("Icons/icon_bisner.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            _homeButton = new UIButton(new CGRect(0, 0, 33, 36));
            _homeButton.SetBackgroundImage(image, UIControlState.Normal);
            _homeButton.TouchUpInside += (sender, args) =>
            {
                var mainPresenter = Mvx.Resolve<IMainViewPresenter>();
                mainPresenter.SetActiveTab(0);
            };

            NavigationItem.TitleView = _homeButton;
        }

        #endregion Helpers
    }
}
