using System;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class MembersButtonCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("MembersButtonCell");

        private UIButton _companyButton, _memberButton;
        private UIView _bottomRuler;

        public MembersButtonCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            ContentView.BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            _companyButton = new BlueButton();
            _companyButton.Font = iOS.Appearance.Fonts.LatoBoldWithSize(15);
            _companyButton.SetTitle(Settings.GetResource(ResKeys.mobile_members_list_companies), UIControlState.Normal);
            _companyButton.SetTitleColor(iOS.Appearance.Colors.White, UIControlState.Normal);
            _companyButton.Layer.CornerRadius = 5f;
            _companyButton.Layer.BackgroundColor = iOS.Appearance.Colors.BisnerBlue.CGColor;
            _companyButton.Layer.BorderColor = iOS.Appearance.Colors.SubTextColor.CGColor;

            _memberButton = new BlueButton();
            _memberButton.Font = iOS.Appearance.Fonts.LatoBoldWithSize(15);
            _memberButton.SetTitle(Settings.GetResource(ResKeys.mobile_members_list_members), UIControlState.Normal);
            _memberButton.SetTitleColor(iOS.Appearance.Colors.White, UIControlState.Normal);
            _memberButton.Layer.CornerRadius = 5f;
            _memberButton.Layer.BackgroundColor = iOS.Appearance.Colors.BisnerBlue.CGColor;
            _memberButton.Layer.BorderColor = iOS.Appearance.Colors.SubTextColor.CGColor;

            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            ContentView.AddSubviews(_companyButton, _memberButton, _bottomRuler);

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _companyButton.AtLeftOf(ContentView, 14),
                _companyButton.Height().EqualTo(34),
                _companyButton.WithSameCenterY(ContentView),

                _memberButton.WithSameWidth(_companyButton),
                _memberButton.WithSameHeight(_companyButton),
                _memberButton.ToRightOf(_companyButton, 14),
                _memberButton.AtRightOf(ContentView, 14),
                _memberButton.WithSameCenterY(ContentView),

                _bottomRuler.AtBottomOf(ContentView),
                _bottomRuler.AtLeftOf(ContentView),
                _bottomRuler.AtRightOf(ContentView),
                _bottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<MembersButtonCell, MembersButtonItem>();
                set.Bind(_companyButton).To(vm => vm.CompanyCommand);
                set.Bind(_companyButton).For("Members").To(vm => vm.ShowCompanies);
                set.Bind(_companyButton).For("Title").To(item => item.CompanyText);
                set.Bind(_companyButton).For("Visibility").To(vm => vm.CompanyButtonEnabled).WithConversion("Visibility");
                set.Bind(_memberButton).To(vm => vm.MembersCommand);
                set.Bind(_memberButton).For("Members").To(vm => vm.ShowMembers);
                set.Bind(_memberButton).For("Title").To(item => item.MembersText);
                set.Apply();
            });
        }

        #endregion Setup
    }
}