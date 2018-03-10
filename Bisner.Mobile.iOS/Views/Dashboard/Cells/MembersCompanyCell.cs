using System;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class MembersCompanyCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("CompanyCell");

        private UIView _bottomRuler;
        private AvatarImageView _logo;
        private UILabel _name;

        public MembersCompanyCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Cell

        private bool _firstTime = true;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_firstTime)
            {
                _firstTime = false;

                LayoutIfNeeded();

                var newBackGroundView =
                    new UIView(new CGRect(SelectedBackgroundView.Bounds.X, SelectedBackgroundView.Bounds.Y,
                        SelectedBackgroundView.Bounds.Width, SelectedBackgroundView.Bounds.Height - 1));
                newBackGroundView.Layer.BackgroundColor = iOS.Appearance.Colors.BackgroundColor.CGColor;
                SelectedBackgroundView = newBackGroundView;
            }
        }

        #endregion Cell

        #region Setup

        private void SetupSubViews()
        {
            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _logo = new AvatarImageView();

            _name = new UILabel { TextColor = iOS.Appearance.Colors.DefaultTextColor, Font = iOS.Appearance.Fonts.LatoWithSize(17) };
            
            ContentView.AddSubviews(_bottomRuler, _name, _logo);

            SelectionStyle = UITableViewCellSelectionStyle.Gray;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _logo.AtLeftOf(ContentView, 14),
                _logo.WithSameCenterY(ContentView),
                _logo.Height().EqualTo(40),
                _logo.Width().EqualTo(40),

                _name.ToRightOf(_logo, 14),
                _name.AtRightOf(ContentView, 14),
                _name.WithSameCenterY(_logo).Minus(3),

                _bottomRuler.AtBottomOf(ContentView),
                _bottomRuler.AtRightOf(ContentView, 14),
                _bottomRuler.AtLeftOf(ContentView, 14),
                _bottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<MembersCompanyCell, ICompany>();
                set.Bind(_name).To(vm => vm.Name);
                set.Bind(_logo).For("ImageUrl").To(vm => vm.LogoUrl).WithConversion("ImageUrl");
                set.Apply();
            });
        }

        #endregion Setup
    }
}