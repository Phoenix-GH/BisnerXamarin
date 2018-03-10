using System;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    public class GroupFeedCell : MvxTableViewCell
    {

        #region Constructor

        public static NSString Identifier = new NSString("GroupFeedCell");

        private UIImageView _header;
        private UIImageView _logo;
        private UILabel _name;
        private UIView _headerContainer, _headerBottomRuler, _logoShadowView;
        private UIActivityIndicatorView _joinIndicator;
        private UIButton _joinButton;

        public GroupFeedCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            ContentView.BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            _header = new UIImageView { ContentMode = UIViewContentMode.ScaleAspectFill, BackgroundColor = UIColor.Black };
            _logo = new AvatarImageView();

            _logoShadowView = new UIView { BackgroundColor = UIColor.White };
            _logoShadowView.Layer.BorderWidth = 2;
            _logoShadowView.Layer.ShadowRadius = 12f;
            _logoShadowView.Layer.ShadowColor = UIColor.Black.CGColor;
            _logoShadowView.Layer.ShadowOffset = new CGSize(0f, 0);
            _logoShadowView.Layer.ShadowOpacity = 0.55f;
            _logoShadowView.Layer.CornerRadius = 16.0f;

            _logo = new UIImageView { ClipsToBounds = true, BackgroundColor = iOS.Appearance.Colors.White };
            _logo.Layer.BorderWidth = 3;
            _logo.Layer.BorderColor = iOS.Appearance.Colors.White.CGColor;
            _logo.Layer.CornerRadius = 16f;
            
            _name = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(25), TextColor = iOS.Appearance.Colors.ChatMessageColor, Lines = 1, TextAlignment = UITextAlignment.Center };
            _headerContainer = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _headerBottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderBottom };

            _joinIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { Color = iOS.Appearance.Colors.BisnerBlue };
            _joinIndicator.StartAnimating();


            //_joinButton = new BlueButton { Font = iOS.Appearance.Fonts.LatoBlackWithSize(18) };
            //_joinButton.Layer.ShadowColor = UIColor.Black.CGColor;
            //_joinButton.Layer.ShadowRadius = 10f;
            //_joinButton.Layer.ShadowOpacity = 0.65f;
            //_joinButton.Layer.ShadowOffset = new CGSize(0, 5);

            _joinButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14), BackgroundColor = iOS.Appearance.Colors.Green };
            _joinButton.Layer.CornerRadius = 20f; // height / 2
            _joinButton.Layer.BorderColor = iOS.Appearance.Colors.Green.CGColor;
            _joinButton.Layer.BorderWidth = 1f;
            _joinButton.SetTitleColor(iOS.Appearance.Colors.White, UIControlState.Normal);

            ContentView.AddSubviews(
                _headerContainer,
                _header,
                _logoShadowView,
                _logo,
                _name,
                _joinIndicator,
                _joinButton,
                _headerBottomRuler
                );
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _headerContainer.AtTopOf(ContentView),
                _headerContainer.AtLeftOf(ContentView),
                _headerContainer.AtRightOf(ContentView),
                _headerContainer.AtBottomOf(ContentView, 5),

                _header.AtTopOf(_headerContainer),
                _header.AtLeftOf(_headerContainer),
                _header.AtRightOf(_headerContainer),
                _header.Height().EqualTo(280),

                _logoShadowView.WithSameCenterY(_logo),
                _logoShadowView.WithSameCenterX(_logo),
                _logoShadowView.Height().EqualTo(110),
                _logoShadowView.Width().EqualTo(110),

                _logo.WithSameBottom(_header).Plus(34),
                _logo.WithSameCenterX(_header),
                _logo.Height().EqualTo(112),
                _logo.Width().EqualTo(112),

                _name.Below(_header, 60),
                _name.AtLeftOf(ContentView, 14),
                _name.AtRightOf(ContentView, 14),
                _name.WithSameCenterX(_headerContainer),

                _joinButton.Below(_name, 20),
                _joinButton.AtLeftOf(ContentView, 50),
                _joinButton.AtRightOf(ContentView, 50),
                _joinButton.Height().EqualTo(45),

                _joinIndicator.WithSameCenterY(_joinButton),
                _joinIndicator.WithSameCenterX(_joinButton),

                //_headerBottomRuler.Below(_header, 140),
                _headerBottomRuler.AtBottomOf(_headerContainer),
                _headerBottomRuler.AtLeftOf(_headerContainer),
                _headerBottomRuler.AtRightOf(_headerContainer),
                _headerBottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<GroupFeedCell, GroupFeedItem>();
                set.Bind(_header).For("ImageUrl").To(vm => vm.HeaderUrl).WithConversion("ImageUrl");
                set.Bind(_logo).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(_name).To(vm => vm.Name);
                set.Bind(_joinButton).To(vm => vm.JoinCommand);
                set.Bind(_joinButton).For("Title").To(vm => vm.JoinButtonText);
                set.Bind(_joinButton).For("Visibility").To(vm => vm.IsNotJoining).WithConversion("Visibility");
                set.Bind(_joinIndicator).For("ActivityHidden").To(vm => vm.IsJoining).WithConversion("Visibility");
                set.Apply();
            });
        }

        #endregion Setup
    }
}