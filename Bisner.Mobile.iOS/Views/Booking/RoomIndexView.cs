using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.MvvmcrossApp;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class RoomIndexView : ViewBase<RoomIndexViewModel>
    {
        public RoomIndexView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Set Header Slider
            var roomSource = new GenericTableviewSourceWithHeight<RoomIndexItemViewModel>(tvRoom)
            {
                GetCellHeightFunc = (view, path, item) => 225,
                GetEstimatedHeightFunc = (view, path, item) => 225,
                GetIdentifierFunc = (model, path) => RoomIndexItemView.Identifier,
                ModifyCellFunc = (cell, path, item) =>
                {
                    var roomIndexItemView = cell as RoomIndexItemView;
                    roomIndexItemView?.InitStyle();
                },
                AutoDeselect = true,
                RowSelectionFunc = (view, path, item) =>
                {
                    item.BookRoomCommand.Execute(null);
                }
            };

            var refreshControl = new MvxUIRefreshControl();

            // Selection off
            tvRoom.AllowsSelection = false;
            tvRoom.RefreshControl = refreshControl;
            tvRoom.RegisterNibForCellReuse(UINib.FromName("RoomIndexItemView", null), RoomIndexItemView.Identifier);
            tvRoom.Delegate = new UITableViewDelegate();
            tvRoom.Source = roomSource;

            // Add Post button
            var roomTimeIndexButton = new UIBarButtonItem
            {
                Title = "Overview",
            };

            var icoFontAttribute = new UITextAttributes { Font = Appearance.Fonts.LatoBoldWithSize(24), TextColor = Appearance.Colors.BisnerBlue };
            roomTimeIndexButton.SetTitleTextAttributes(icoFontAttribute, UIControlState.Application);
            roomTimeIndexButton.Style = UIBarButtonItemStyle.Done;

            NavigationItem.SetRightBarButtonItems(new[] { roomTimeIndexButton }, true);

            InitStyle();

            var bindingSet = this.CreateBindingSet<RoomIndexView, RoomIndexViewModel>();
            bindingSet.Bind(roomSource).For(s => s.ItemsSource).To(vm => vm.RoomList);
            bindingSet.Bind(roomTimeIndexButton).To(vm => vm.OverViewCommand);
            bindingSet.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            bindingSet.Apply();
        }

        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                Mvx.Resolve<IMainViewPresenter>().ShowInTab = true;
            }
        }

        public void InitStyle()
        {
            tvRoom.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tvRoom.BackgroundColor = Appearance.Colors.BackgroundColor;
        }

        #region Base view properties

        protected override bool EnableCustomBackButton => true;

        protected override bool EnableTitleBarLogo => true;

        #endregion Base view properties
    }
}