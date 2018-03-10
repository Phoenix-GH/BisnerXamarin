using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Bisner.Mobile.Core.Service;
using System.Linq;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.Core.ViewModels.Notifications;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.MvvmcrossApp;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using ObjCRuntime;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    partial class MainView : MvxTabBarViewController<MainViewModel>, IMainViewPresenter
    {
        #region Constructor

        private MvxSubscriptionToken _unreadNotificationsSubscription;
        private MvxSubscriptionToken _unreadChatMessageSubscription;

        public MainView(IntPtr handle)
            : base(handle)
        {
            // Set this main view as our main view presenter to be able to show viewcontrollers in the tabs
            var mainViewPresenterHost = Mvx.Resolve<IMainViewPresenterHost>();
            mainViewPresenterHost.MainViewPresenter = this;
            ShowInTab = true;

            Mvx.RegisterSingleton<IMainViewPresenter>(() => this);
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("MAINVIEW RECIEVED MEMORY WARNING!!");
        }

        public sealed override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Main";

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            // Check roles and create tabs accordingly
            var userRoles = Settings.UserRoles;

            // Create viewcontroller array
            var viewcontrollerList = new List<UIViewController>();

            // FEED
            if (userRoles.Any(r => r == Home.View.ToLower()))
            {
                var feedTab = CreateTabFor("Feed", "feed_normal", "feed_active", typeof(FeedViewModel));
                // Set for badge numbering 
                FeedTabBarItem = feedTab.TabBarItem;
                viewcontrollerList.Add(feedTab);
            }

            // ACCESS CONTROL
            if (Settings.AccessControlEnabled)
            {
                var accessControlTab = CreateTabFor("AccessControl", "lock_normal", "lock_active",
                    typeof(AccessControlViewModel));
                viewcontrollerList.Add(accessControlTab);
            }

            // DASHBOARD
            var dashboardTab = CreateTabFor("Dashboard", "dashboard_normal", "dashboard_active", typeof(DashboardViewModel));
            viewcontrollerList.Add(dashboardTab);

            // CHAT
            if (userRoles.Any(r => r == ApiModels.Security.Roles.Chat.View.ToLower()))
            {
                var chatTab = CreateTabFor("Chat", "chat_normal", "chat_active", typeof(ChatViewModel));
                // Set for badge numbering 
                ChatTabBarItem = chatTab.TabBarItem;
                viewcontrollerList.Add(chatTab);
            }

            if (!Settings.AccessControlEnabled)
            {
                // NOTIFICATIONS
                var notificationsTab = CreateTabFor("Notifications", "notifications_normal", "notifications_active",
                    typeof(NotificationsViewModel));
                // Set for badge numbering 
                NotificationTabBarItem = notificationsTab.TabBarItem;
                viewcontrollerList.Add(notificationsTab);
            }

            // MORE
            var moreTab = CreateTabFor("More", "more_normal", "more_active", typeof(ManageViewModel));
            viewcontrollerList.Add(moreTab);

            // Set the controllers for the tabs
            ViewControllers = viewcontrollerList.ToArray();
            //CustomizableViewControllers = new UIViewController[] { };
            // Set the first viewcontroller (dashboard) as active
            SelectedViewController = ViewControllers[0];
            //SetupBackButton();

            var frame = new CGRect(0.0, 0.0, View.Bounds.Size.Width, 49);
            var v = new UIView(frame) { BackgroundColor = Appearance.Colors.TabBarColor };
            TabBar.AddSubview(v);
            TabBar.SendSubviewToBack(v);
        }

        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Debug.WriteLine("MainView WillAppear");

            //Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = true;
            }

            var mainViewPresenterHost = Mvx.Resolve<IMainViewPresenterHost>();
            mainViewPresenterHost.EnableSliders = true;

            Task.Factory.StartNew(async () =>
            {
                await ViewModel.Init();
            });

            if (IsMovingToParentViewController)
            {
                ViewControllerSelected += OnViewControllerSelected;

                _unreadNotificationsSubscription =
                    Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<UpdateUnreadNotificationMessage>(m =>
                    {
                        SetUnreadNotifications(m.NumberUnread);
                    });

                _unreadChatMessageSubscription =
                    Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<UpdateUnreadChatMessagesMessage>(m =>
                    {
                        SetUnreadChatMessages(m.NumberUnread);
                    });
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                ViewControllerSelected -= OnViewControllerSelected;

                if (_unreadNotificationsSubscription != null)
                {
                    _unreadNotificationsSubscription.Dispose();
                    _unreadNotificationsSubscription = null;
                }
                if (_unreadChatMessageSubscription != null)
                {
                    _unreadChatMessageSubscription.Dispose();
                    _unreadChatMessageSubscription = null;
                }
            }
        }

        #endregion ViewController

        #region Tabs

        private UIViewController CreateTabFor(string title, string imageName, string selectedImageName, Type viewModel)
        {
            var controller = new NonTransluscentUINavigationController();
            var screen = this.CreateViewControllerFor(new MvxViewModelRequest(viewModel, null, null, null)) as UIViewController;
            SetTitleAndTabBarItem(screen, title, imageName, selectedImageName);
            controller.PushViewController(screen, false);
            return controller;
        }

        private void SetTitleAndTabBarItem(UIViewController screen, string title, string imageName,
            string selectedImageName)
        {
            screen.Title = title;

            UITabBarItem tabbarItem;

            using (var image = UIImage.FromBundle("Icons/Tabs/" + imageName + ".png"))
            {
                using (var image2 = UIImage.FromBundle("Icons/Tabs/" + selectedImageName + ".png"))
                {
                    tabbarItem = new UITabBarItem(title, image, image2)
                    {
                        ImageInsets = new UIEdgeInsets(5, 0, -5, 0)
                    };
                }

                using (
                    var unselectedImage =
                        tabbarItem.SelectedImage.ImageWithColor(Appearance.Colors.UnselectedTabColor)
                            .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
                {
                    tabbarItem.Image = unselectedImage;
                }

                screen.TabBarItem = tabbarItem;
            }
        }

        private void OnViewControllerSelected(object sender, UITabBarSelectionEventArgs uiTabBarSelectionEventArgs)
        {
            if (uiTabBarSelectionEventArgs.ViewController is UINavigationController)
            {
                ((UINavigationController)uiTabBarSelectionEventArgs.ViewController).PopToRootViewController(false);
            }
        }

        #endregion Tabs

        #region IMainViewPresenter

        /// <summary>
        /// Show a viewcontroller in the current active tab
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool Show(IMvxIosView view)
        {
            if (TryShowViewInCurrentTab(view))
                return true;

            return false;
        }

        public bool Close(IMvxViewModel viewModel)
        {
            CloseTopController();

            return true;
        }

        /// <summary>
        /// Feed tab bar item to set badge number
        /// </summary>
        public UITabBarItem FeedTabBarItem { get; private set; }

        /// <summary>
        /// Chat tab bar item to set badge number
        /// </summary>
        public UITabBarItem ChatTabBarItem { get; private set; }

        /// <summary>
        /// Notifications tab bar item to set badge number
        /// </summary>
        public UITabBarItem NotificationTabBarItem { get; private set; }

        /// <summary>
        /// If true new views will be pushed to the master navigation controller
        /// </summary>
        public bool ShowInTab { get; set; }

        public void SetActiveTab(int index)
        {
            SelectedViewController = ViewControllers[index];
        }

        private bool TryShowViewInCurrentTab(IMvxIosView view)
        {
            var navigationController = (UINavigationController)SelectedViewController;
            navigationController.PushViewController((UIViewController)view, true);
            var mainViewPresenterHost = Mvx.Resolve<IMainViewPresenterHost>();
            mainViewPresenterHost.EnableSliders = false;
            return true;
        }

        private bool CloseTopController()
        {
            var navigationController = (UINavigationController)SelectedViewController;
            navigationController.PopViewController(true);
            return true;
        }

        #endregion IMainViewPresenter

        #region Badges

        private void SetUnreadNotifications(int numberUnread)
        {
            if (NotificationTabBarItem != null)
            {
                NotificationTabBarItem.BadgeValue = numberUnread == 0 ? null : numberUnread.ToString();
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = numberUnread;
            }
        }

        private void SetUnreadChatMessages(int numberUnread)
        {
            ChatTabBarItem.BadgeValue = numberUnread == 0 ? null : numberUnread.ToString();
        }

        #endregion Badges
    }
}
