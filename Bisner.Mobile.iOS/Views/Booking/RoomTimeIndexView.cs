using System;
using Bisner.Mobile.Core.ViewModels.Booking;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Booking
{
    [MvxFromStoryboard]
    public partial class RoomTimeIndexView : ViewBase<RoomTimeIndexViewModel>
    {
        public RoomTimeIndexView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Set Header Slider
            var roomTimeIndexSource = new GenericTableviewSourceWithHeight<RoomTimeIndexItemViewModel>(tvRoomTimeIndex)
            {
                GetIdentifierFunc = (model, path) => RoomTimeIndexItemView.Identifier,
                GetCellHeightFunc = (view, path, item) => 270,
                ModifyCellFunc = (cell, path, item) =>
                {
                    var roomTimeIndexItemView = cell as RoomTimeIndexItemView;
                    roomTimeIndexItemView?.InitStyle();
                }
            };

            var refreshControl = new MvxUIRefreshControl();

            tvRoomTimeIndex.RefreshControl = refreshControl;
            tvRoomTimeIndex.RegisterNibForCellReuse(UINib.FromName("RoomTimeIndexItemView", null), RoomTimeIndexItemView.Identifier);
            tvRoomTimeIndex.Delegate = new UITableViewDelegate();
            tvRoomTimeIndex.Source = roomTimeIndexSource;
            tvRoomTimeIndex.ReloadData();

            var button = new UIBarButtonItem(UIImage.FromBundle("Icons/icon_calendar.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIBarButtonItemStyle.Plain, null, null);
            NavigationItem.SetRightBarButtonItem(button, true);

            var bindingSet = this.CreateBindingSet<RoomTimeIndexView, RoomTimeIndexViewModel>();
            bindingSet.Bind(roomTimeIndexSource).For(b => b.ItemsSource).To(vm => vm.RoomTimeIndexList);
            bindingSet.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            bindingSet.Bind(button).To(vm => vm.ChangeDateCommand);
            bindingSet.Apply();

            InitStyle();
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

        public void InitStyle()
        {
            tvRoomTimeIndex.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tvRoomTimeIndex.AllowsSelection = false;
        }

        #region Base view overrides

        protected override bool EnableCustomBackButton => true;

        protected override bool EnableTitleBarLogo => true;

        #endregion Base view overrides
    }
}