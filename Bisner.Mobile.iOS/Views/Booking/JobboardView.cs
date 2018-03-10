using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.MvvmcrossApp;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class JobboardView : ViewBase<JobboardViewModel>
    {
        #region Constructor

        public JobboardView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Set Header Slider
            var bookingsSource = new GenericTableviewSourceWithHeight<BookingsItemViewModel>(tvBookings)
            {
                GetCellHeightFunc = (view, path, item) => 120,
                GetIdentifierFunc = (model, path) => BookingsItemView.Identifier,
                AutoDeselect = true,
                ModifyCellFunc = (cell, path, item) =>
                {
                    var bookinsItemView = cell as BookingsItemView;
                    bookinsItemView?.InitStyle();
                },
                GetEstimatedHeightFunc = (view, path, item) => 120,
            };

            var refreshControl = new MvxUIRefreshControl();
            tvBookings.RefreshControl = refreshControl;

            var bindingSet = this.CreateBindingSet<JobboardView, JobboardViewModel>();
            bindingSet.Bind(bookingsSource).For(s => s.ItemsSource).To(vm => vm.BookingsList);
            bindingSet.Bind(bookingsSource).For(s => s.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);
            bindingSet.Bind(btnBookingNew).To(vm => vm.BookingCommand);
            bindingSet.Bind(btnBookingNew).For("Title").To(vm => vm.BookingButtonTitle);
            bindingSet.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            bindingSet.Apply();

            tvBookings.RegisterNibForCellReuse(UINib.FromName("BookingsItemView", null), BookingsItemView.Identifier);
            tvBookings.Delegate = new UITableViewDelegate();
            tvBookings.Source = bookingsSource;
            tvBookings.ReloadData();

            InitStyle();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = false;

            if (IsMovingToParentViewController)
            {
                Mvx.Resolve<IMainViewPresenter>().ShowInTab = false;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                ViewModel.MessageSubscription?.Dispose();
            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        #endregion ViewController

        #region Setup

        public void InitStyle()
        {
            btnBookingNew.Layer.CornerRadius = btnBookingNew.Frame.Height / 2f;
            btnBookingNew.Layer.MasksToBounds = true;
            tvBookings.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tvBookings.BackgroundColor = UIColor.FromRGB(244, 244, 244);
            tvBookings.BackgroundView = null;
            //var navBar = this.NavigationController.NavigationBar;
            //navBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            //navBar.ShadowImage = new UIImage();
            //var navigationSeparator = new UIView(new CGRect(0, navBar.Frame.Height - 1, navBar.Frame.Width, 1));
            //navigationSeparator.BackgroundColor = UIColor.FromRGB(232, 232, 232);
            //navigationSeparator.Opaque = true;
            //this.NavigationController?.NavigationBar.AddSubview(navigationSeparator);

            View.BackgroundColor = Appearance.Colors.White;
        }

        protected override bool EnableCustomBackButton => true;

        protected override bool EnableTitleBarLogo => true;

        #endregion Setup
    }
}