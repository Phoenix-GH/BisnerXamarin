using System;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class MemberCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("MemberCell");

        private UIView _topRuler, _bottomRuler;
        private AvatarImageView _avatar;
        private UILabel _name;

        public MemberCell(IntPtr handle) : base(handle)
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
            _topRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _avatar = new AvatarImageView();

            _name = new UILabel { TextColor = iOS.Appearance.Colors.DefaultTextColor, Font = iOS.Appearance.Fonts.LatoWithSize(17) };
            
            ContentView.AddSubviews(_topRuler, _bottomRuler, _name, _avatar);

            SelectionStyle = UITableViewCellSelectionStyle.Gray;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _avatar.AtLeftOf(ContentView, 14),
                _avatar.WithSameCenterY(ContentView),
                _avatar.Height().EqualTo(40),
                _avatar.Width().EqualTo(40),
                
                _name.ToRightOf(_avatar, 14),
                _name.AtRightOf(ContentView, 14),
                _name.WithSameCenterY(_avatar).Minus(3),

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
                var set = this.CreateBindingSet<MemberCell, IUser>();
                set.Bind(_name).To(vm => vm.DisplayName);
                set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.Avatar.Small).WithConversion("ImageUrl");
                set.Apply();
            });
        }

        #endregion Setup
    }
}