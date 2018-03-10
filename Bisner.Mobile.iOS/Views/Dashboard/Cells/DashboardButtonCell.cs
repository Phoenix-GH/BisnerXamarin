using System;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class DashboardButtonCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("DashboardButtonsCell");

        private BackPanelView _backPanelView;
        private UIView _horizontalLine, _verticalLine;
        private UIButton _membersButton, _eventsButton, _groupsButton, _infoButton;

        public DashboardButtonCell(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        private void SetupSubViews()
        {
            // Cell properties
            SelectionStyle = UITableViewCellSelectionStyle.None;
            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            // Subviews
            _backPanelView = new BackPanelView { BackgroundColor = iOS.Appearance.Colors.White };
            _horizontalLine = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _verticalLine = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _membersButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };
            _membersButton.SetTitleColor(iOS.Appearance.Colors.DefaultTextColor, UIControlState.Normal);
            using (var memberImage = UIImage.FromBundle("Icons/icon_members.png"))
            {
                _membersButton.SetImage(memberImage, UIControlState.Normal);
            }

            _eventsButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };
            _eventsButton.SetTitleColor(iOS.Appearance.Colors.DefaultTextColor, UIControlState.Normal);
            using (var eventsImage = UIImage.FromBundle("Icons/icon_calendar.png"))
            {
                _eventsButton.SetImage(eventsImage, UIControlState.Normal);
            }

            _groupsButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };
            _groupsButton.SetTitleColor(iOS.Appearance.Colors.DefaultTextColor, UIControlState.Normal);
            using (var groupImage = UIImage.FromBundle("Icons/icon_groups.png"))
            {
                _groupsButton.SetImage(groupImage, UIControlState.Normal);
            }

            _infoButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };
            _infoButton.SetTitleColor(iOS.Appearance.Colors.DefaultTextColor, UIControlState.Normal);
            using (var infoImage = UIImage.FromBundle("Icons/icon_info.png"))
            {
                _infoButton.SetImage(infoImage, UIControlState.Normal);
            }

            ContentView.Add(_backPanelView);
            ContentView.Add(_horizontalLine);
            ContentView.Add(_verticalLine);
            ContentView.Add(_membersButton);
            ContentView.Add(_eventsButton);
            ContentView.Add(_groupsButton);
            ContentView.Add(_infoButton);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                ContentView.Height().EqualTo(300),

                _backPanelView.AtTopOf(ContentView, 20),
                _backPanelView.AtLeftOf(ContentView, 20),
                _backPanelView.AtRightOf(ContentView, 20),
                _backPanelView.AtBottomOf(ContentView, 20),

                _horizontalLine.WithSameCenterY(_backPanelView),
                _horizontalLine.AtLeftOf(_backPanelView, 10),
                _horizontalLine.AtRightOf(_backPanelView, 10),
                _horizontalLine.Height().EqualTo(1),

                _verticalLine.WithSameCenterX(_backPanelView),
                _verticalLine.AtTopOf(_backPanelView, 10),
                _verticalLine.AtBottomOf(_backPanelView, 10),
                _verticalLine.Width().EqualTo(1),

                _membersButton.AtTopOf(_backPanelView),
                _membersButton.AtLeftOf(_backPanelView),
                _membersButton.AtRightOf(_verticalLine),
                _membersButton.Above(_horizontalLine),

                _eventsButton.AtTopOf(_backPanelView),
                _eventsButton.AtRightOf(_backPanelView),
                _eventsButton.AtLeftOf(_verticalLine),
                _eventsButton.Above(_horizontalLine),

                _groupsButton.AtBottomOf(_backPanelView),
                _groupsButton.AtLeftOf(_backPanelView),
                _groupsButton.AtRightOf(_verticalLine),
                _groupsButton.Below(_horizontalLine),

                _infoButton.AtBottomOf(_backPanelView),
                _infoButton.AtRightOf(_backPanelView),
                _infoButton.AtLeftOf(_verticalLine),
                _infoButton.Below(_horizontalLine)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DashboardButtonCell, DashboardButtonItem>();
                set.Bind(_membersButton).To(vm => vm.MembersCommand);
                set.Bind(_membersButton).For("Title").To(vm => vm.MembersText);
                set.Bind(_eventsButton).To(vm => vm.EventsCommand);
                set.Bind(_eventsButton).For("Title").To(vm => vm.EventsText);
                set.Bind(_groupsButton).To(vm => vm.GroupsCommand);
                set.Bind(_groupsButton).For("Title").To(vm => vm.GroupsText);
                set.Bind(_infoButton).To(vm => vm.InfoCommand);
                set.Bind(_infoButton).For("Title").To(vm => vm.MoreText);
                set.Apply();
            });
        }

        #endregion Constructor

        #region Cell

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ContentView.NeedsUpdateConstraints();

            _membersButton.CenterVerticallyWithPadding(10.0f);
            _eventsButton.CenterVerticallyWithPadding(10.0f);
            _groupsButton.CenterVerticallyWithPadding(10.0f);
            _infoButton.CenterVerticallyWithPadding(10.0f);

            ContentView.LayoutIfNeeded();
        }

        #endregion Cell

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _backPanelView.Dispose();
                _backPanelView = null;
                _horizontalLine.Dispose();
                _horizontalLine = null;
                _verticalLine.Dispose();
                _verticalLine = null;
                _membersButton.Dispose();
                _membersButton = null;
                _eventsButton.Dispose();
                _eventsButton = null;
                _groupsButton.Dispose();
                _groupsButton = null;
                _infoButton.Dispose();
                _infoButton = null;
            }
        }
    }
}