using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.General.Notifications;
using Bisner.Mobile.Core.ViewModels.Notifications;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Notifications.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Notifications
{
    partial class NotificationsView : ViewBase<NotificationsViewModel>
    {
        #region Constructor

        private GenericTableViewSource<INotification> _source;
        private MvxUIRefreshControl _refreshControl;

        public NotificationsView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("NOTIFICATIONSVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableViewSource<INotification>(ItemsTable)
            {
                GetIdentifierFunc = (item, path) => NotificationCell.Identifier,
                RowSelectionFunc = (view, path, arg3) => ViewModel.SelectedCommand.Execute(arg3),
                AutoDeselect = true,
                UseAnimations = true,
                AddAnimation = UITableViewRowAnimation.Top,
                RemoveAnimation = UITableViewRowAnimation.Bottom,
                ReplaceAnimation = UITableViewRowAnimation.Middle,
            };

            _refreshControl = new MvxUIRefreshControl();

            ItemsTable.RegisterClassForCellReuse(typeof(NotificationCell), NotificationCell.Identifier);
            ItemsTable.AddSubview(_refreshControl);
            ItemsTable.BackgroundColor = Appearance.Colors.BackgroundColor;
            ItemsTable.RowHeight = UITableView.AutomaticDimension;
            ItemsTable.EstimatedRowHeight = 44;
            ItemsTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<NotificationsView, NotificationsViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsLoading);
            set.Apply();
            ItemsTable.ReloadData();
        }

        #endregion Setup

        #region Base Modifications

        protected override bool EnableTitleBarLogo { get { return true; } }

        #endregion Base Modifications
    }
}
