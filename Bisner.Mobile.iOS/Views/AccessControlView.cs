using System;
using Bisner.Mobile.Core.ViewModels.AccessControl;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.ItemViews;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    [MvxFromStoryboard]
    public partial class AccessControlView : ViewBase<AccessControlViewModel>
    {
        public AccessControlView (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
        }

        private MvxUIRefreshControl _refreshControl;

        private void SetupTable()
        {
            _refreshControl = new MvxUIRefreshControl();
            var tableViewSource = new GenericTableviewSourceWithHeight<AccessControlItemViewModel>(TableView)
            {
                GetIdentifierFunc = (model, path) => LockItemView.Identifier,
                ModifyCellFunc = (cell, path, item) =>
                {
                    var lockItemView = cell as LockItemView;
                    lockItemView?.SetupStyle();
                },
                GetCellHeightFunc = (view, path, item) => 150,
                GetEstimatedHeightFunc = (view, path, item) => 150,
            };

            var set = this.CreateBindingSet<AccessControlView, AccessControlViewModel>();
            set.Bind(tableViewSource).To(vm => vm.Items);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Apply();

            TableView.RefreshControl = _refreshControl;
            TableView.AllowsSelection = false;
            TableView.BackgroundColor = Appearance.Colors.BackgroundColor;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.RegisterNibForCellReuse(UINib.FromName("LockItemView", NSBundle.MainBundle), LockItemView.Identifier );
            TableView.Source = tableViewSource;
            TableView.ReloadData();
        }

        protected override bool EnableTitleBarLogo => true;
    }
}