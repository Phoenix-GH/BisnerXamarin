using System;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class AllEventsCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("AllEventsCell");

        private UIView _backPanel, _bottomBorder;
        private UILabel _text;

        public AllEventsCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _backPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _bottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderBottom };

            _text = new UILabel
            {
                TextColor = iOS.Appearance.Colors.BisnerBlue,
                Font = iOS.Appearance.Fonts.LatoBoldWithSize(15)
            };

            ContentView.AddSubviews(_backPanel, _bottomBorder, _text);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _backPanel.AtTopOf(ContentView),
                _backPanel.AtLeftOf(ContentView),
                _backPanel.AtRightOf(ContentView),
                _backPanel.AtBottomOf(ContentView),

                _bottomBorder.Height().EqualTo(1),
                _bottomBorder.AtBottomOf(_backPanel),
                _bottomBorder.AtLeftOf(_backPanel),
                _bottomBorder.AtRightOf(_backPanel),

                _text.WithSameCenterY(_backPanel),
                _text.WithSameCenterX(_backPanel)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<AllEventsCell, AllEventsItem>();
                set.Bind(_text).To(vm => vm.Text);
                set.Bind(_backPanel.Tap()).For(tap => tap.Command).To(vm => vm.ShowCommand);
                set.Bind(_text.Tap()).For(tap => tap.Command).To(vm => vm.ShowCommand);
                set.Apply();
            });
        }

        #endregion Setup
    }
}