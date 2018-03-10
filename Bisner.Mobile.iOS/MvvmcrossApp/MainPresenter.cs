using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Core.ViewModels.PresentationHints;
using Bisner.Mobile.iOS.Controls.SlidingPanels;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views;
using Bisner.Mobile.iOS.Views.Booking;
using Bisner.Mobile.iOS.Views.Chat;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.iOS;
using UIKit;

namespace Bisner.Mobile.iOS.MvvmcrossApp
{
    /// <summary>
    /// Custom presenter to create our views
    /// </summary>
    public class MainPresenter : MvxModalSupportIosViewPresenter, IMainViewPresenterHost
    {
        #region Constructor

        public MainPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        #endregion Constructor

        #region Presenter

        public override void Show(IMvxIosView view)
        {
            // Modal window presentation styles (modal support is in the base class presenter)
            if (view is IMvxModalIosView)
            {
                var controller = view as UIViewController;
                controller.ModalPresentationStyle = UIModalPresentationStyle.Custom;
                controller.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                base.Show(view);
                return;
            }

            //if (view is MainView)
            //{
            //    var navController = new NonTranslucentSlidingPanelsNavigationController(view as UIViewController);

            //    AddPanel(PanelType.LeftPanel, navController, CreatePanelViewController<WhitelabelMenuViewModel>());
            //    AddPanel(PanelType.RightPanel, navController, CreatePanelViewController<WorkspacesViewModel>());

            //    navController.CanSwipeToShowPanel = touch => true;

            //    NavController = navController;
            //    base.Show(navController);
            //}

            if (view is ChatConversationView)
            {
                var visibleController = AppDelegate.MainWindow.RootViewController.VisibleViewController();

                var userView = visibleController as UserView;

                if (userView != null)
                {
                    CloseModalViewController();
                }
            }

            // Booking flow completed, pop all other viewcontrollers
            if (view is RoomIndexView || view is RoomDetailView || view is JobboardView || view is DatePickerView || view is RoomTimeIndexView || view is BookingConfirmedView)
            {
                MainViewPresenter.ShowInTab = false;
            }

            if (MainViewPresenter != null && view != MainViewPresenter && MainViewPresenter.ShowInTab) // && view is ITabbarView)
            {
                // If presenter is set use show on the presenter to show views in the tabs
                MainViewPresenter.Show(view);
                return;
            }

            if (MainViewPresenter == null)
            {
                // If MainViewPresenter is not set use the default show
                var mainViewPresenterHost = Mvx.Resolve<IMainViewPresenterHost>();
                mainViewPresenterHost.EnableSliders = false;
            }

            base.Show(view);

            if (view is BookingConfirmedView && MasterNavigationController.ViewControllers.Length > 3)
            {
                var rootVc = MasterNavigationController.ViewControllers[0];

                foreach (var uiViewController in MasterNavigationController.ViewControllers)
                {
                    if (!uiViewController.Equals(rootVc) && !uiViewController.Equals(MasterNavigationController.VisibleViewController))
                    {
                        // Remove from stack
                        uiViewController.RemoveFromParentViewController();
                    }
                }
            }
        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navController = new UINavigationController(viewController);

            //AddPanel(PanelType.LeftPanel, navController, CreatePanelViewController<WhitelabelMenuViewModel>());
            //AddPanel(PanelType.RightPanel, navController, CreatePanelViewController<WorkspacesViewModel>());

            //EnableSliders = false;
            //navController.CanSwipeToShowPanel = touch =>
            //{
            //    return EnableSliders;
            //};

            //NavController = navController;

            //navController.Delegate = viewController as IUINavigationControllerDelegate;

            return navController;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            CheckCreateChat(hint);

            CheckDetail(hint);

            CheckLogout(hint);

            CheckLogin(hint);

            CheckGroup(hint);

            CheckEvent(hint);

            CheckChangeTab(hint);

            CheckLanguageChanged(hint);

            base.ChangePresentation(hint);
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (toClose is BookingConfirmedViewModel && !MainViewPresenter.ShowInTab)
            {
                Mvx.Resolve<IMainViewPresenter>().Close(toClose);
            }

            base.Close(toClose);
        }

        #endregion Presenter

        #region Hints

        private void CheckCreateChat(MvxPresentationHint hint)
        {
            var createChatHint = hint as ChatConversationHint;

            if (createChatHint != null)
            {
                Show(new MvxViewModelRequest(typeof(ChatConversationViewModel),
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {"id", createChatHint.SelectedUser.ToString()},
                        { "download", true.ToString() },
                    }), null, null));
            }
        }

        private void CheckDetail(MvxPresentationHint hint)
        {
            var createDetailHint = hint as DetailHint;

            if (createDetailHint != null)
            {
                Show(new MvxViewModelRequest(typeof(DetailsViewModel),
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {"postId", createDetailHint.PostId.ToString()}
                    }), null, null));
            }
        }

        private void CheckLogout(MvxPresentationHint hint)
        {
            if (hint is LogOutPresentationHint)
            {
                var loginController = this.CreateViewControllerFor<LoginViewModel>() as UIViewController;

                if (loginController != null)
                {
                    MainViewPresenter = null;

                    MasterNavigationController = CreateNavigationController(loginController);

                    ChangeRootViewController(MasterNavigationController);
                }
            }
        }

        private void CheckLogin(MvxPresentationHint hint)
        {
            if (hint is LogInPresentationHint)
            {
                var mainViewController = this.CreateViewControllerFor<MainViewModel>() as UIViewController;

                if (mainViewController != null)
                {
                    MasterNavigationController = CreateNavigationController(mainViewController);

                    ChangeRootViewController(MasterNavigationController);
                }
            }
        }

        private void CheckEvent(MvxPresentationHint hint)
        {
            var eventHint = hint as EventHint;

            if (eventHint != null)
            {
                Show(new MvxViewModelRequest(typeof(EventViewModel),
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {"id", eventHint.EventId.ToString()}
                    }), null, null));
            }
        }

        private void CheckGroup(MvxPresentationHint hint)
        {
            var eventHint = hint as GroupHint;

            if (eventHint != null)
            {
                Show(new MvxViewModelRequest(typeof(FeedViewModel),
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {"id", eventHint.GroupId.ToString()},
                        {"feedType", FeedType.Group.ToString() },
                    }), null, null));
            }
        }

        private void CheckChangeTab(MvxPresentationHint hint)
        {
            var tabHint = hint as ChangeTabHint;

            if (tabHint != null)
            {
                Mvx.Resolve<IMainViewPresenter>().SetActiveTab(tabHint.TabIndex);
            }
        }

        private void CheckLanguageChanged(MvxPresentationHint hint)
        {
            var languageChangedPresentationHint = hint as LanguageChangedPresentationHint;

            if (languageChangedPresentationHint != null)
            {
                var mainViewController = this.CreateViewControllerFor<MainViewModel>() as UIViewController;

                MainViewPresenter = null;

                MasterNavigationController = CreateNavigationController(mainViewController);

                ChangeRootViewController(MasterNavigationController);
            }
        }

        #endregion Hints

        #region Controllers

        private int _currentUnread;
        private bool _enableSliders;

        private UIViewController ChangeRootViewController(UIViewController viewController)
        {
            if (Window.RootViewController == null)
            {
                Window.RootViewController = viewController;
                return null;
            }

            var oldRootController = Window.RootViewController;

            var snapShot = Window.SnapshotView(true);

            viewController.View.AddSubview(snapShot);

            Window.RootViewController = viewController;

            UIView.Animate(0.5, () =>
            {
                snapShot.Layer.Opacity = 0;
                snapShot.Layer.Transform = CATransform3D.MakeScale(1.5f, 1.5f, 1.5f);
            },
                () =>
                {
                    snapShot.RemoveFromSuperview();
                });

            return oldRootController;
        }

        #endregion Controllers

        #region Sliding panels

        public SlidingPanelsNavigationViewController NavController { get; private set; }

        /// <summary>
        /// Creates a ViewModel of the desired type (as well as the  ppropriate View) and inserts it into
        /// the correct Panel Container and inserts the panel container into the provided sliding panel
        /// view controller
        /// </summary>
        /// <param name="panelType">Panel type.</param>
        /// <param name="navigationViewController"></param>
        /// <param name="viewToAdd">Mvx controller.</param>
        protected void AddPanel(PanelType panelType, SlidingPanelsNavigationViewController navigationViewController, UIViewController viewToAdd)
        {

            // use the first view to create a view of the desired type
            // We only do this because there's no convenient way to create a view from inside the presenter

            // Insert the view into a new container (of the right type) and insert 
            // that into the sliding panels controller
            switch (panelType)
            {
                case PanelType.LeftPanel:
                    navigationViewController.InsertPanel(new LeftPanelContainer(viewToAdd));
                    break;

                case PanelType.RightPanel:
                    navigationViewController.InsertPanel(new RightPanelContainer(viewToAdd));
                    break;

                case PanelType.BottomPanel:
                    navigationViewController.InsertPanel(new BottomPanelContainer(viewToAdd));
                    break;

                default:
                    throw new ArgumentException("PanelType is invalid");
            }
        }

        /// <summary>
        /// Create a ViewController for the given ViewModel through mvx viewmodel creation
        /// </summary>
        /// <typeparam name="TTargetViewModel"></typeparam>
        /// <returns></returns>
        private UIViewController CreatePanelViewController<TTargetViewModel>() where TTargetViewModel : MvxViewModel
        {
            var parameterBundle = new MvxBundle(null);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null, MvxRequestedBy.UserAction);
            var view = Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);

            return view as UIViewController;
        }

        #endregion Sliding panels

        #region Properties

        /// <summary>
        /// The main view presenter containing the tabs
        /// </summary>
        public IMainViewPresenter MainViewPresenter { get; set; }

        /// <summary>
        /// Indicates if the sliders can be opened
        /// </summary>
        public bool EnableSliders
        {
            get => _enableSliders;
            set
            {
                _enableSliders = value;
                if (NavController != null)
                {
                    NavController.InteractivePopGestureRecognizer.Enabled = !value;
                }
            }
        }

        #endregion Properties
    }
}