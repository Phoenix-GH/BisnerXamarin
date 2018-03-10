using System;
using Bisner.Mobile.Core.Models.Chat;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Chat.Cells
{
    partial class ChatMessageCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("ChatMessageCell");

        private AvatarImageView _avatar;
        private UILabel _timeAgo;
        public UILabel Message, DisplayName;
        
        public ChatMessageCell(IntPtr handle)
            : base(handle)
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

            _avatar = new AvatarImageView();

            DisplayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11), TextColor = iOS.Appearance.Colors.SubTextColor };

            Message = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.ChatMessageColor, Lines = 0 };

            ContentView.Add(_avatar);
            ContentView.Add(DisplayName);
            ContentView.Add(_timeAgo);
            ContentView.Add(Message);

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _avatar.AtTopOf(ContentView, 10),
                _avatar.AtLeftOf(ContentView, 10),
                _avatar.Height().EqualTo(30),
                _avatar.Width().EqualTo(30),

                DisplayName.WithSameTop(_avatar),
                DisplayName.ToRightOf(_avatar, 10),

                _timeAgo.WithSameBottom(DisplayName),
                _timeAgo.ToRightOf(DisplayName, 10),

                Message.Below(DisplayName, 5),
                Message.WithSameLeft(DisplayName),
                Message.AtRightOf(ContentView, 10),
                Message.AtBottomOf(ContentView, 10)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ChatMessageCell, IChatMessage>();
            set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
            set.Bind(DisplayName).To(vm => vm.DisplayName);
            set.Bind(_timeAgo).To(vm => vm.DateTimeText);
            set.Bind(Message).To(vm => vm.Text);
            set.Apply();
        }

        #endregion Setup
    }
}
