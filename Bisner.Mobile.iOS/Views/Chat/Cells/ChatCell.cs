using System;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Linq;
using Bisner.Mobile.Core.Models.Chat;
using Bisner.Mobile.iOS.ValueConverters;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;

namespace Bisner.Mobile.iOS.Views.Chat.Cells
{
    partial class ChatCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("ChatCell");

        private UIView _topBorder, _bottomBorder;
        private AvatarImageView _avatar;
        private UILabel _displayName, _timeAgo, _lastMessage, _numberUnread;
        private UIView _unreadContainer;
        
        private NSLayoutConstraint _topRulerLeftConstraint, _topRulerRightConstraint;
        
        public ChatCell(IntPtr handle)
            : base(handle)
        {
            SetupAppearance();
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupAppearance()
        {
            BackgroundColor = iOS.Appearance.Colors.White;

            var selectedBackGroundView =
                new UIView(new CGRect(SelectedBackgroundView.Bounds.X, SelectedBackgroundView.Bounds.Y,
                    SelectedBackgroundView.Bounds.Width, SelectedBackgroundView.Bounds.Height))
                {
                    BackgroundColor = UIColor.FromRGB(250, 250, 250)
                };
            SelectedBackgroundView = selectedBackGroundView;
        }

        private void SetupSubViews()
        {
            _topBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _bottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _avatar = new AvatarImageView();

            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11), TextColor = iOS.Appearance.Colors.SubTextColor, TextAlignment = UITextAlignment.Right };

            _unreadContainer = new UIView { BackgroundColor = iOS.Appearance.Colors.Green };
            _unreadContainer.Layer.CornerRadius = 10.0f;
            _numberUnread = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(11), TextColor = iOS.Appearance.Colors.White };

            _lastMessage = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(14), TextColor = iOS.Appearance.Colors.DefaultTextColor.ColorWithAlpha(0.8f) };
            
            ContentView.AddSubviews(_topBorder, _bottomBorder, _avatar, _displayName, _timeAgo, _unreadContainer, _numberUnread, _lastMessage);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            _topRulerRightConstraint = _topBorder.AtRightOf(ContentView, 14).ToLayoutConstraints().First();
            _topRulerLeftConstraint = _topBorder.AtLeftOf(ContentView, 14).ToLayoutConstraints().First();

            ContentView.AddConstraint(_topRulerLeftConstraint);
            ContentView.AddConstraint(_topRulerRightConstraint);

            ContentView.AddConstraints(
                _topBorder.AtTopOf(ContentView),
                _topBorder.Height().EqualTo(1),

                _avatar.AtLeftOf(ContentView, 14),
                _avatar.WithSameCenterY(ContentView),
                _avatar.Height().EqualTo(45),
                _avatar.Width().EqualTo(45),

                _displayName.WithSameCenterY(_avatar).Plus(-13),
                _displayName.ToRightOf(_avatar, 10),

                _timeAgo.AtRightOf(ContentView, 14),
                _timeAgo.WithSameCenterY(_displayName).Plus(1),
                _timeAgo.ToRightOf(_displayName, 14),

                _unreadContainer.Below(_timeAgo, 5),
                _unreadContainer.AtRightOf(ContentView, 30),
                _unreadContainer.Width().EqualTo(20),
                _unreadContainer.Height().EqualTo(20),

                _numberUnread.WithSameCenterX(_unreadContainer),
                _numberUnread.WithSameCenterY(_unreadContainer),

                _lastMessage.Below(_displayName, 3),
                _lastMessage.WithSameLeft(_displayName),
                _lastMessage.ToLeftOf(_timeAgo),
                
                _bottomBorder.AtBottomOf(ContentView),
                _bottomBorder.AtLeftOf(ContentView),
                _bottomBorder.AtRightOf(ContentView),
                _bottomBorder.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ChatCell, ConversationListViewModel>();
            set.Bind(_displayName).To(vm => vm.DisplayName);
            set.Bind(_timeAgo).To(vm => vm.LastMessageDateTime).WithConversion("TimeAgo");
            set.Bind(_lastMessage).To(vm => vm.LastMessage);
            set.Bind(_lastMessage).For(l => l.Font).To(vm => vm.IsRead).WithConversion("UnreadMessage").WithFallback(UnreadMessageValueConverter.FallbackValue);
            set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
            set.Bind(_numberUnread).To(vm => vm.NumberUnread);
            set.Bind(_numberUnread).For("Visibility").To(vm => vm.HasUnreadMessages).WithConversion("Visibility");
            set.Bind(_unreadContainer).For("Visibility").To(vm => vm.HasUnreadMessages).WithConversion("Visibility");
            set.Apply();
        }

        #endregion Setup

        #region UITableViewCell

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        #endregion UITableViewCell

        #region Modifiers

        public void SetRulerFullWidth(bool fullWidth)
        {
            if (fullWidth)
            {
                _topRulerRightConstraint.Constant = 0;
                _topRulerLeftConstraint.Constant = 0;
            }
            else
            {
                _topRulerRightConstraint.Constant = 14;
                _topRulerLeftConstraint.Constant = 14;
            }
        }

        public void SetBottomRulerVisible(bool visible)
        {
            _bottomBorder.Hidden = !visible;
        }

        #endregion Modifiers
    }

    public class FooterRulerCell : MvxTableViewCell
    {
        #region Constrcutor

        private UIView _ruler;

        public FooterRulerCell(IntPtr handle) : base(handle)
        {
            SetupViews();
            SetupConstraints();
        }

        public static NSString Identifier = new NSString("FoorterRulerCell");

        #endregion Constructor

        #region Setup

        private void SetupViews()
        {
            _ruler = new UIView() { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            ContentView.AddSubviews(_ruler);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _ruler.AtTopOf(ContentView),
                _ruler.AtLeftOf(ContentView),
                _ruler.AtBottomOf(ContentView),
                _ruler.AtRightOf(ContentView),
                _ruler.Height().EqualTo(1)
                );
        }

        #endregion Setup
    }
}
