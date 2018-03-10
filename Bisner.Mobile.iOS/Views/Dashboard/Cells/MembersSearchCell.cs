using System;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class MembersSearchCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("MembersSearchCell");

        private InputTextField _searchInput;

        public MembersSearchCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _searchInput = new InputTextField { BackgroundColor = iOS.Appearance.Colors.BackgroundColor };
            _searchInput.Layer.BorderColor = iOS.Appearance.Colors.BarShadowColor.CGColor;

            ContentView.AddSubview(_searchInput);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _searchInput.AtTopOf(ContentView),
                _searchInput.AtLeftOf(ContentView),
                _searchInput.AtRightOf(ContentView),
                _searchInput.AtBottomOf(ContentView)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<MembersSearchCell, MemberSearchItem>();
                set.Bind(_searchInput).For(i => i.Placeholder).To(vm => vm.PlaceholderText);
                set.Bind(_searchInput).To(vm => vm.SearchText);
                set.Apply();
            });
        }

        #endregion Setup
    }
}
