using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    partial class UserView : ViewBase<UserViewModel>
    {
        #region Constructor

        private UIView _containerView, _avatarShadowView, _nameContainer, _nameBottomBorder, _infoContainer, _infoTopBorder, _infoBottomBorder;
        private UIImageView _header;
        private UIImageView _avatar;
        private UILabel _displayName, _position, _aboutHeader, _about, _skills;
        private UIButton _addContact, _message;
        private UIActivityIndicatorView _activityIndicator;

        public UserView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("USERVIEW RECIEVED MEMORY WARNING!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = Appearance.Colors.BackgroundColor;

            SetupSubViews();
            SetupConstraints();
            SetupBindings();

            // Set screen name for analytics
            ScreenName = "UserView";
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            ResignFirstResponder();
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        #endregion ViewController

        #region Setup

        private void SetupSubViews()
        {
            _containerView = new UIView { BackgroundColor = Appearance.Colors.BackgroundColor };

            using (var image = UIImage.FromBundle("Images/contact_background.jpg"))
            {
                _header = new UIImageView(image) { ContentMode = UIViewContentMode.ScaleToFill, ClipsToBounds = true };
            }

            _avatarShadowView = new UIView { BackgroundColor = UIColor.White.ColorWithAlpha(0.30f) };
            _avatarShadowView.Layer.BorderWidth = 3;
            _avatarShadowView.Layer.ShadowRadius = 10f;
            _avatarShadowView.Layer.ShadowColor = UIColor.Black.CGColor;
            _avatarShadowView.Layer.ShadowOffset = new CGSize(0f, 0);
            _avatarShadowView.Layer.ShadowOpacity = 1f;
            _avatarShadowView.Layer.CornerRadius = 16.0f;

            _avatar = new UIImageView { ClipsToBounds = true };
            _avatar.Layer.BorderWidth = 3;
            _avatar.Layer.BorderColor = Appearance.Colors.White.CGColor;
            _avatar.Layer.CornerRadius = 16f;

            _displayName = new UILabel { Font = Appearance.Fonts.LatoWithSize(25), TextColor = Appearance.Colors.ChatMessageColor };

            _position = new UILabel { Font = Appearance.Fonts.LatoWithSize(18), TextColor = Appearance.Colors.SubTextColor };

            _nameContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _nameBottomBorder = new UIView { BackgroundColor = Appearance.Colors.BackPanelBorderBottom };

            _infoTopBorder = new UIView { BackgroundColor = Appearance.Colors.BackPanelBorderTop };
            _infoBottomBorder = new UIView { BackgroundColor = Appearance.Colors.BackPanelBorderBottom };
            _infoContainer = new UIView { BackgroundColor = Appearance.Colors.White };

            _skills = new UILabel
            {
                Font = Appearance.Fonts.LatoWithSize(14),
                TextColor = Appearance.Colors.ChatMessageColor,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TextAlignment = UITextAlignment.Center,
            };

            _aboutHeader = new UILabel
            {
                Font = Appearance.Fonts.LatoBoldWithSize(14),
                TextColor = Appearance.Colors.DefaultTextColor,
            };

            _about = new UILabel { Font = Appearance.Fonts.LatoWithSize(15), TextColor = Appearance.Colors.ChatMessageColor, Lines = 0 };

            _addContact = new UIButton { BackgroundColor = UIColor.Clear, Font = Appearance.Fonts.LatoBoldWithSize(15) };
            _addContact.SetTitleColor(Appearance.Colors.UserCardSubText, UIControlState.Normal);
            _addContact.Layer.BorderWidth = 1;
            _addContact.Layer.BorderColor = Appearance.Colors.BarShadowColor.CGColor;
            _addContact.Layer.CornerRadius = 8f;

            _message = new UIButton { BackgroundColor = UIColor.Clear, Font = Appearance.Fonts.LatoBoldWithSize(15) };
            _message.SetTitleColor(Appearance.Colors.UserCardSubText, UIControlState.Normal);
            _message.Layer.BorderWidth = 1;
            _message.Layer.BorderColor = Appearance.Colors.BarShadowColor.CGColor;
            _message.Layer.CornerRadius = 8f;

            _activityIndicator = new UIActivityIndicatorView { ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray };
            _activityIndicator.StartAnimating();

            ScrollView.AddSubviews(_containerView);

            _containerView.AddSubviews(_infoContainer, 
                _infoTopBorder, 
                _infoBottomBorder, 
                _nameContainer, 
                _skills, 
                _nameBottomBorder, 
                _header, 
                _avatarShadowView, 
                _avatar, 
                _displayName, 
                _position, 
                _aboutHeader, 
                _about, 
                _message, 
                _activityIndicator);

            _addContact.Hidden = true;
        }

        private void SetupConstraints()
        {
            ScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _containerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _containerView.WithSameWidth(View)
            );

            ScrollView.AddConstraints(
                _containerView.AtTopOf(ScrollView),
                _containerView.AtLeftOf(ScrollView),
                _containerView.AtRightOf(ScrollView),
                _containerView.AtBottomOf(ScrollView)
            );

            var aspectRatio = NSLayoutConstraint.Create(_header, NSLayoutAttribute.Height, NSLayoutRelation.Equal,
                _header, NSLayoutAttribute.Width, 0.5273775216138329f, 0);

            _containerView.AddConstraint(aspectRatio);

            _containerView.AddConstraints(
                _header.AtTopOf(_containerView),
                _header.AtLeftOf(_containerView),
                _header.AtRightOf(_containerView),

                _avatarShadowView.WithSameCenterX(_containerView),
                _avatarShadowView.WithSameBottom(_header).Plus(28),
                _avatarShadowView.Height().EqualTo(108),
                _avatarShadowView.Width().EqualTo(108),

                _avatar.WithSameCenterX(_containerView),
                _avatar.WithSameBottom(_header).Plus(30),
                _avatar.Height().EqualTo(110),
                _avatar.Width().EqualTo(110),

                _displayName.Below(_avatar, 15),
                _displayName.WithSameCenterX(_containerView),

                _position.Below(_displayName, 4),
                _position.WithSameCenterX(_containerView),

                _skills.Below(_position, 14),
                _skills.AtLeftOf(_infoContainer, 14),
                _skills.AtRightOf(_infoContainer, 14),

                _nameContainer.Below(_header),
                _nameContainer.AtLeftOf(_containerView),
                _nameContainer.AtRightOf(_containerView),
                _nameContainer.WithSameBottom(_skills).Plus(20),

                _nameBottomBorder.AtBottomOf(_nameContainer),
                _nameBottomBorder.AtLeftOf(_containerView),
                _nameBottomBorder.AtRightOf(_containerView),
                _nameBottomBorder.Height().EqualTo(1),

                _infoContainer.Below(_nameContainer, 10),
                _infoContainer.AtLeftOf(_containerView),
                _infoContainer.AtRightOf(_containerView),

                _infoTopBorder.AtTopOf(_infoContainer),
                _infoTopBorder.AtLeftOf(_infoContainer),
                _infoTopBorder.AtRightOf(_infoContainer),
                _infoTopBorder.Height().EqualTo(1),

                _aboutHeader.Below(_infoTopBorder, 14),
                _aboutHeader.AtLeftOf(_containerView, 14),
                _aboutHeader.AtRightOf(_containerView, 14),

                _about.Below(_aboutHeader, 13),
                _about.AtLeftOf(_containerView, 14),
                _about.AtRightOf(_containerView, 14),

                //_addContact.Top().GreaterThanOrEqualTo(20).BottomOf(_about),
                //_addContact.AtBottomOf(_backPanel, 10),
                //_addContact.AtLeftOf(_backPanel, 10),
                //_addContact.ToLeftOf(_message, 20),
                //_addContact.Height().EqualTo(40),

                //_message.WithSameWidth(_addContact),
                //_message.AtRightOf(_backPanel, 10),
                //_message.WithSameHeight(_addContact),
                //_message.WithSameTop(_addContact),

                _message.Top().GreaterThanOrEqualTo(20).BottomOf(_about),
                _message.AtBottomOf(_containerView, 10),
                _message.WithSameCenterX(_containerView),
                _message.Width().EqualTo(200),
                _message.Height().EqualTo(40),

                _infoBottomBorder.AtBottomOf(_infoContainer),
                _infoBottomBorder.AtLeftOf(_infoContainer),
                _infoBottomBorder.AtRightOf(_infoContainer),
                _infoBottomBorder.Below(_message, 14)
            );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<UserView, UserViewModel>();
            set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
            set.Bind(_header).For("ImageUrl").To(vm => vm.HeaderUrl).WithConversion("ImageUrl");
            set.Bind(_displayName).To(vm => vm.DisplayName);
            set.Bind(_addContact).For("Visibility").To(vm => vm.ContactButtonAvailable).WithConversion("Visibility");
            set.Bind(_message).For("Title").To(vm => vm.MessageText);
            set.Bind(_message).To(vm => vm.MessageCommand);
            set.Bind(_message).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(_activityIndicator).For("Visibility").To(vm => vm.IsBusy).WithConversion("Visibility");
            set.Bind(_position).To(vm => vm.CompanyName);
            set.Bind(_skills).To(vm => vm.Skills);
            set.Bind(_skills).For("Visibility").To(vm => vm.ShowSkills).WithConversion("Visibility");
            set.Bind(_aboutHeader).To(vm => vm.AboutHeaderText);
            set.Bind(_aboutHeader).For("Visibility").To(vm => vm.ShowAbout).WithConversion("Visibility");
            set.Bind(_about).For(l => l.AttributedText).To(vm => vm.About).WithConversion("HtmlAttributedText");
            set.Bind(View.Tap()).For(tap => tap.Command).To(vm => vm.CloseCommand);
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => true;

        #endregion Setup
    }
}
