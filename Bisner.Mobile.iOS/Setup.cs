using System;
using System.Diagnostics;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Controls.Bindings;
using Bisner.Mobile.iOS.Controls.SlidingPanels;
using Bisner.Mobile.iOS.Controls.SlidingPanels.PanelContainers;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;

namespace Bisner.Mobile.iOS
{
    public class Setup : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterSingleton(() => new App());

            return Mvx.Resolve<App>();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxTouchViewsContainer CreateTouchViewsContainer()
        {
            // Return our custom container that creates views from the storyboard
            return new StoryboardContainer();
        }

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            // Create our custom presenter
            var mainPresenter = new MainPresenter(ApplicationDelegate as UIApplicationDelegate, Window);

            // Register in IOC
            Mvx.RegisterSingleton<IMainViewPresenterHost>(mainPresenter);

            return mainPresenter;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<UIButton>("Like", like => new LikeButtonBinding(like));
            registry.RegisterCustomBindingFactory<UIButton>("Comment", like => new CommentButtonBinding(like));

            base.FillTargetFactories(registry);
        }
    }

    /// <summary>
    /// Custom container to create views from our storyboard
    /// </summary>
    public class StoryboardContainer : MvxTouchViewsContainer
    {
        protected override IMvxTouchView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            try
            {
                var view = UIStoryboard.FromName("MainStoryBoard", null).InstantiateViewController(viewType.Name);

                var mvxTouchView = (IMvxTouchView)view;

                return mvxTouchView;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }

    /// <summary>
    /// Custom presenter to create our views
    /// </summary>
    public class MainPresenter : MvxModalSupportTouchViewPresenter, IMainViewPresenterHost
    {
        #region Constructor

        public MainPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        #endregion Constructor

        #region Members

        public override void Show(IMvxTouchView view)
        {
            if (view is IMvxModalTouchView)
            {
                var controller = view as UIViewController;
                controller.ModalPresentationStyle = UIModalPresentationStyle.Custom;
                controller.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                PresentModalViewController(controller, true);

                return;
            }

            // Check if the MainViewPresenter has been set (i.e. user has logged in)
            if (MainViewPresenter != null && view != MainViewPresenter)
            {
                //EnableSliders = true;

                // If presenter is set use show on the presenter to show views in the tabs
                MainViewPresenter.Show(view);
                return;
            }

            // If MainViewPresenter is not set use the default show
            base.Show(view);
        }

        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            NavController = new NonTranslucentSlidingPanelsNavigationController(viewController);

            AddPanel(PanelType.LeftPanel, NavController, CreatePanelViewController<LeftMenuViewModel>());
            //AddPanel(PanelType.RightPanel, navBar, CreatePanelViewController<WorkspacesViewModel>());

            EnableSliders = false;
            NavController.CanSwipeToShowPanel = touch => EnableSliders;

            return NavController;
        }

        #endregion Members

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
            var view = Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);

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
        public bool EnableSliders { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Main view presenter host to get a reference to the main view presenter
    /// </summary>
    public interface IMainViewPresenterHost
    {
        /// <summary>
        /// The main sliding panels navigation controller
        /// </summary>
        SlidingPanelsNavigationViewController NavController { get; }

        /// <summary>
        /// The main view presenter containing the tabs
        /// </summary>
        IMainViewPresenter MainViewPresenter { get; set; }


        //SlidingPanelsNavigationViewController SlidingPanelsController { get; }

        /// <summary>
        /// Indicates if the sliders can be opened
        /// </summary>
        bool EnableSliders { get; set; }
    }

    /// <summary>
    /// Interface for tab bar controller to be able to show views in tabs and set badge numbers
    /// </summary>
    public interface IMainViewPresenter
    {
        /// <summary>
        /// Show a view within the main view presenter
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        bool Show(IMvxTouchView view);

        /// <summary>
        /// Feed tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem FeedTabBarItem { get; }

        /// <summary>
        /// Chat tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem ChatTabBarItem { get; }

        /// <summary>
        /// Notification tab bar item to be able to set badge number
        /// </summary>
        UITabBarItem NotificationTabBarItem { get; }

        /// <summary>
        /// Set the active tab
        /// </summary>
        void SetActiveTab(int index);
    }
}