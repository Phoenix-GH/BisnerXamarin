using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Dashboard.Cells;
using Bisner.Mobile.iOS.Views.General.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    partial class EventsView : ViewBase<EventsViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IItemBase> _source;
        private MvxUIRefreshControl _refreshControl;

        public EventsView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("EVENTSVIEW RECIEVED MEMORY WARNING!!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "EventsView";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ItemsTable.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ItemsTable.ReloadData();
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IItemBase>(ItemsTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is IEvent)
                    {
                        return EventCell.Identifier;
                    }

                    return HeaderImageCell.Identifier;
                },
                RowSelectionFunc = (view, path, item) =>
                {
                    if (item is IEvent)
                    {
                        ViewModel.EventSelected(item.Id);
                    }
                },
                AutoDeselect = true,
                GetCellHeightFunc = (view, path, arg3) =>
                {
                    return UIScreen.MainScreen.Bounds.Width * 0.5f + 2;
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    return UIScreen.MainScreen.Bounds.Width * 0.5f + 2;
                },
            };

            _refreshControl = new MvxUIRefreshControl();

            ItemsTable.AddSubview(_refreshControl);
            ItemsTable.AllowsSelection = true;
            ItemsTable.RegisterClassForCellReuse(typeof(EventCell), EventCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(HeaderImageCell), HeaderImageCell.Identifier);
            ItemsTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<EventsView, EventsViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsLoading);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            set.Apply();
        }

        #endregion Setup

        #region Base

        protected override bool EnableCustomBackButton { get { return true; } }

        #endregion Base
    }
}
