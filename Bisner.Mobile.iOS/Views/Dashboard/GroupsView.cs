using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Dashboard.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    partial class GroupsView : HideTabBarViewBase<GroupsViewModel>
    {
        #region Consturctor

        private GenericTableviewSourceWithHeight<IGroup> _source;
        private MvxUIRefreshControl _refreshControl;

        public GroupsView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "GroupsView";

            View.BackgroundColor = Appearance.Colors.BackgroundColor;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("GROUPSVIEW RECIEVED MEMORY WARNING!!!");
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IGroup>(ItemsTable)
            {
                GetIdentifierFunc = (group, path) => GroupCell.Identifier,
                ModifyCellFunc = (cell, path, group) =>
                {
                    var groupCell = cell as GroupCell;

                    if (groupCell != null)
                    {
                        // Only 1 section
                        groupCell.SetIsFirstCell(true);
                        groupCell.GroupDescription.AttributedText = GetText(group.Id, group.Description);
                    }
                },
                GetCellHeightFunc = (view, path, group) => 220,
                //{
                //    if (path.Row == 0)
                //    {
                //        return 220;
                //    }

                //    return 180;
                //},
                GetEstimatedHeightFunc = (view, path, group) => 220,
                //{
                //    if (path.Row == 0)
                //    {
                //        return 220;
                //    }

                //    return 180;
                //},
                UseAnimations = false,
            };

            _refreshControl = new MvxUIRefreshControl();

            ItemsTable.DelaysContentTouches = false;

            foreach (var subview in ItemsTable.Subviews)
            {
                if (subview is UIScrollView)
                {
                    ((UIScrollView)subview).DelaysContentTouches = false;
                }
            }

            ItemsTable.BackgroundColor = Appearance.Colors.BackgroundColor;
            ItemsTable.AddSubviews(_refreshControl);
            ItemsTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            ItemsTable.RegisterClassForCellReuse(typeof(GroupCell), GroupCell.Identifier);
            ItemsTable.AllowsSelection = false;
            ItemsTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<GroupsView, GroupsViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(_refreshControl).For(c => c.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(_refreshControl).For(c => c.RefreshCommand).To(vm => vm.RefreshCommand);
            set.Apply();
        }

        #endregion Setup

        #region Text cache

        private readonly Dictionary<Guid, NSAttributedString> _texts = new Dictionary<Guid, NSAttributedString>();

        private NSAttributedString GetText(Guid id, string value)
        {
            NSAttributedString text = null;

            if (_texts.ContainsKey(id))
            {
                text = _texts[id];
            }
            else if(!string.IsNullOrWhiteSpace(value))
            {
                var labelString = new NSMutableAttributedString(value);
                var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = 4 };
                var style = UIStringAttributeKey.ParagraphStyle;
                var range = new NSRange(0, labelString.Length);

                labelString.AddAttribute(style, paragraphStyle, range);

                text = labelString;
            }

            return text;
        }


        #endregion text cache

        #region Base modifications

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => true;

        protected override bool SupportsHideTabBar => true;

        protected override NSLayoutConstraint TabBarTopConstraint => TableBottomConstraint;

        #endregion Base modifications
    }
}
