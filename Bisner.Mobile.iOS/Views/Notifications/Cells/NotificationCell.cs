using System;
using Bisner.Mobile.Core.Models.General.Notifications;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.ValueConverters;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Notifications.Cells
{
    public class NotificationCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("NotificationCell");

        private AvatarImageView _avatar;
        private UILabel _displayName, _timeAgo, _text;
        private UILabel _extraText;
        private UIView _bottomRuler, _backgroundColor;

        public NotificationCell(IntPtr handle)
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
            _avatar = new AvatarImageView();

            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14.5f), TextColor = iOS.Appearance.Colors.DefaultTextColor, Text = "Display Name" };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11.86f), TextColor = iOS.Appearance.Colors.SubTextColor, Text = "30 minutes ago" };

            _text = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor, Lines = 0 };

            _extraText = new UILabel { BackgroundColor = UIColor.Clear, Lines = 3, LineBreakMode = UILineBreakMode.TailTruncation, Font = iOS.Appearance.Fonts.LatoWithSize(13), TextColor = iOS.Appearance.Colors.DefaultTextColor };

            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _backgroundColor = new UIView { BackgroundColor = UIColor.Clear };

            ContentView.AddSubviews(_backgroundColor, _avatar, _displayName, _timeAgo, _text, _bottomRuler, _extraText);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _avatar.AtTopOf(ContentView, 14),
                _avatar.AtLeftOf(ContentView, 14),
                _avatar.Height().EqualTo(30),
                _avatar.Width().EqualTo(30),

                _displayName.WithSameTop(_avatar),
                _displayName.ToRightOf(_avatar, 10),

                _timeAgo.ToRightOf(_displayName, 6),
                _timeAgo.WithSameBottom(_displayName),
                //_timeAgo.AtLeftOf(ContentView, 14),

                _text.Below(_displayName, 5),
                _text.WithSameLeft(_displayName),
                _text.AtRightOf(ContentView, 14),

                _extraText.Below(_text, 10),
                _extraText.WithSameLeft(_text),
                _extraText.AtRightOf(ContentView, 14),

                _bottomRuler.Below(_extraText, 10),
                _bottomRuler.WithSameLeft(ContentView),
                _bottomRuler.WithSameRight(ContentView),
                _bottomRuler.Height().EqualTo(1),
                _bottomRuler.AtBottomOf(ContentView),

                _backgroundColor.AtLeftOf(ContentView),
                _backgroundColor.AtTopOf(ContentView),
                _backgroundColor.AtRightOf(ContentView),
                _backgroundColor.AtBottomOf(ContentView)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<NotificationCell, INotification>();
                set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
                set.Bind(_displayName).To(vm => vm.DisplayName);
                set.Bind(_timeAgo).To(vm => vm.CreationDateTime).WithConversion("TimeAgo");
                set.Bind(_backgroundColor).For("NotificationBackground").To(vm => vm.IsRead);
                set.Bind(_text).To(vm => vm.Text);
                set.Bind(_extraText)
                    .For(t => t.AttributedText)
                    .To(vm => vm.ExtraText)
                    .WithConversion("HtmlAttributedText")
                    .WithFallback(HtmlAttributedTextValueConverter.FallBackString);
                //set.Bind(_extraText).To(vm => vm.ExtraText);
                set.Apply();
            });
        }

        #endregion Setup
    }
}
