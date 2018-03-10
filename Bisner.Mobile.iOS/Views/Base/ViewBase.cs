using System;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.Core.ViewModels.Base;
using Bisner.Mobile.iOS.Controls.Gestures;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;
using Bisner.Mobile.iOS.MvvmcrossApp;
using Bisner.Mobile.iOS.Views.Feed;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Base
{
    public abstract class ViewBase<TViewModel> : MvxViewController<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        #region Constructor

        protected ViewBase(IntPtr intPtr) : base(intPtr)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (EnableMenuButton && EnableCustomBackButton)
                throw new Exception("Unable to add both hamburger icon and custom back button to the left side of the navigation bar, please disable one");

            // When enabled setup logo title view
            if (EnableTitleBarLogo)
            {
                SetupTitleView();
            }

            // When enabled setup back button
            if (EnableCustomBackButton)
            {
                SetupBackButton();
            }

            // When enabled setup hamburger button
            if (EnableMenuButton)
            {
                SetupMenuButton();
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (IsMovingToParentViewController)
            {
                var eventViewModel = ViewModel as IEventViewModel;

                eventViewModel?.Attach();

                // When enabled remove click handler
                if (EnableTitleBarLogo)
                {
                    _titleButton.TouchUpInside += TitleButtonOnTouchUpInside;
                }

                // When enabled add click handler
                if (EnableCustomBackButton)
                {
                    _backButton.Clicked += BackButtonOnClicked;
                }

                // When enabled add handler
                if (EnableMenuButton)
                {
                    _menuButton.Clicked += MenuButtonOnClicked;
                }
            }

            SendScreenMeasure();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                var eventViewModel = ViewModel as IEventViewModel;

                eventViewModel?.Unattach();

                // When enabled remove click handler
                if (EnableTitleBarLogo)
                {
                    if (_titleButton != null)
                        _titleButton.TouchUpInside -= TitleButtonOnTouchUpInside;
                }

                // When enabled remove click handler
                if (EnableCustomBackButton)
                {
                    if (_backButton != null)
                        _backButton.Clicked -= BackButtonOnClicked;
                }

                // When enabled remove handler
                if (EnableMenuButton)
                {
                    if (_menuButton != null)
                        _menuButton.Clicked -= MenuButtonOnClicked;
                }
            }
        }

        #endregion ViewController

        #region TextField

        /// <summary>
        /// Should return default handler to dismiss keyboard
        /// </summary>
        /// <param name="textField"></param>
        /// <returns></returns>
        protected virtual bool TextFieldShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();

            return true;
        }

        #endregion TextField

        #region Navbar Logo

        /// <summary>
        /// The home button
        /// </summary>
        private UIButton _titleButton;

        /// <summary>
        /// Enable or disable the home button on the navbar (logo)
        /// </summary>
        protected virtual bool EnableTitleBarLogo { get { return false; } }

        /// <summary>
        /// Setup the title view (home button)
        /// </summary>
        private void SetupTitleView()
        {
            // Add the bisner logo home button
            using (
                var image =
                    UIImage.FromBundle("Icons/navbar_logo.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _titleButton = new UIButton(new CGRect(0, 0, 33, 36));
                _titleButton.SetBackgroundImage(image, UIControlState.Normal);
            }

            NavigationItem.TitleView = _titleButton;
        }

        /// <summary>
        /// Home button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void TitleButtonOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            var mainPresenter = Mvx.Resolve<IMainViewPresenter>();
            mainPresenter.SetActiveTab(0);

            if (this is FeedView)
            {
                var feedView = this as FeedView;

                feedView.ScrollToTop(true);
            }
        }

        #endregion Navbar Logo

        #region Back button

        /// <summary>
        /// The back button
        /// </summary>
        private UIBarButtonItem _backButton;

        /// <summary>
        /// Override this value and return true to enable custom back button
        /// </summary>
        protected virtual bool EnableCustomBackButton { get { return false; } }

        /// <summary>
        /// Setup the back button
        /// </summary>
        private void SetupBackButton()
        {
            // Add back button
            using (
                var backImage =
                    UIImage.FromBundle("Icons/back_arrow.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _backButton = new UIBarButtonItem(backImage, UIBarButtonItemStyle.Plain, null, null);
            }

            // Custom back button disables the default swipe of iOS viewcontrollers, so we add a new one to the navigation controller
            NavigationController.InteractivePopGestureRecognizer.Delegate = new SwipeGestureDelegate();
            NavigationItem.SetLeftBarButtonItem(_backButton, true);
        }

        /// <summary>
        /// Backbutton clicked handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected void BackButtonOnClicked(object sender, EventArgs eventArgs)
        {
            NavigationController.PopViewController(true);
        }

        #endregion Back button

        #region Menu button

        /// <summary>
        /// The menu button
        /// </summary>
        private UIBarButtonItem _menuButton;

        /// <summary>
        /// Enable or disable the menu button
        /// </summary>
        protected virtual bool EnableMenuButton { get { return false; } }

        /// <summary>
        /// Setup the menu button as left navigation item (hamburger icon)
        /// </summary>
        private void SetupMenuButton()
        {
            // Add hamburger icon to left of navigation bar
            using (var menuImage =
                UIImage.FromBundle("Icons/icon_nav_header.png")
                    .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _menuButton = new UIBarButtonItem
                {
                    Image = menuImage,
                };
            }

            NavigationItem.SetLeftBarButtonItem(_menuButton, true);
        }

        private void MenuButtonOnClicked(object sender, EventArgs eventArgs)
        {
            var mainPresenter = Mvx.Resolve<IMainViewPresenterHost>();
            mainPresenter.NavController.TogglePanel(PanelType.LeftPanel);
        }

        #endregion Menu button

        #region Analytics

        /// <summary>
        /// Screen name for google analytics
        /// </summary>
        protected string ScreenName { get; set; }

        private void SendScreenMeasure()
        {
            Mvx.Resolve<IAnalyticsService>().SendScreen(ScreenName);
        }

        #endregion Analytics
    }
}