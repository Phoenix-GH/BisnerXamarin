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
    partial class EventCategoriesView : ViewBase<EventCategoriesViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IItemBase> _source;
        private MvxUIRefreshControl _refreshControl;

        public EventCategoriesView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindgs();

            // Set screen name for analytics
            ScreenName = "EventCategoriesView";
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("EVENTCATEGORIESVIEW RECIEVED MEMORY WARNING!!!");
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IItemBase>(ItemsTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is HeaderImage)
                    {
                        return HeaderImageCell.Identifier;
                    }

                    if (item is Event)
                    {
                        return EventCell.Identifier;
                    }

                    if (item is EventCategoryDouble)
                    {
                        return EventCategoryDoubleCell.Identifier;
                    }

                    if (item is IEventCategory)
                    {
                        return EventCategoryCell.Identifier;
                    }

                    return AllEventsCell.Identifier;
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    return GetCellHeight(item);
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    return GetCellHeight(item);
                }
                // TODO: Vanwege de double image header cell kan row selectie niet want we weten niet op welke plaats er is getapped in de cell, later verbeteren met tap gestures  : http://stackoverflow.com/questions/11070874/how-can-i-distinguish-which-part-of-uitableviewcell-has-been-clicked
                //RowSelectionFunc = (view, path, item) =>
                //{
                //    if (item is DoubleHeaderImage)
                //    {
                //        ViewModel.CategorySelected();

                //    }

                //    ViewModel.CategorySelected(item.Id);
                //    ItemsTable.DeselectRow(path, true);
                //}
            };

            foreach (var subview in ItemsTable.Subviews)
            {
                if (subview is UIScrollView)
                {
                    ((UIScrollView)subview).DelaysContentTouches = false;
                }
            }

            _refreshControl = new MvxUIRefreshControl();
            ItemsTable.AddSubview(_refreshControl);

            ItemsTable.ContentInset = new UIEdgeInsets(0, 0, 5, 0);
            ItemsTable.BackgroundColor = Appearance.Colors.BackgroundColor;
            // The image header is only shown when there is only 1 category in the platform
            ItemsTable.RegisterClassForCellReuse(typeof(HeaderImageCell), HeaderImageCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(EventCell), EventCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(EventCategoryCell), EventCategoryCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(AllEventsCell), AllEventsCell.Identifier);
            ItemsTable.RegisterClassForCellReuse(typeof(EventCategoryDoubleCell), EventCategoryDoubleCell.Identifier);
            ItemsTable.Source = _source;
        }

        private void SetupBindgs()
        {
            var set = this.CreateBindingSet<EventCategoriesView, EventCategoriesViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(_refreshControl).For(c => c.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(_refreshControl).For(c => c.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Apply();
        }

        #endregion Setup

        #region TableCells

        private static nfloat GetCellHeight(IItemBase item)
        {
            nfloat finalHeight = 0;

            if (item is AllEventsItem)
            {
                return 72;
            }

            if (item is EventCategoryDouble)
            {
                var imageWidth = (UIScreen.MainScreen.Bounds.Width - 42) / 2;

                finalHeight += imageWidth * 0.65f;
                finalHeight += 28;
            }

            if (item is EventCategory)
            {
                finalHeight += 50;
            }

            if (item is Event)
            {
                finalHeight += UIScreen.MainScreen.Bounds.Width * 0.5f + 2;
            }

            return finalHeight;
        }

        #endregion TableCells

        #region Base

        protected override bool EnableTitleBarLogo { get { return true; } }

        protected override bool EnableCustomBackButton { get { return true; } }

        #endregion Base
    }
}
