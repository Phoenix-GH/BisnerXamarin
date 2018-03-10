using System;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.iOS.Controls;
using Foundation;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using System.Linq;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Service;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using MvvmCross.Platform;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class CommentCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("CommentCell");

        private AvatarImageView _avatar;
        private UILabel _displayName, _timeAgo;
        public HtmlTextView Text;
        private UIView _bottomRuler;
        private UIButton _contextButton;

        private NSLayoutConstraint _bottomRulerLeftConstraint, _bottomRulerRightConstraint;

        public CommentCell(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        private void SetupSubViews()
        {
            _avatar = new AvatarImageView();

            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14.5f), TextColor = iOS.Appearance.Colors.DefaultTextColor, Text = "Display Name" };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11.86f), TextColor = iOS.Appearance.Colors.SubTextColor, Text = "30 minutes ago" };

            //_text = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor, Text = "Comment text", Lines = 0 };
            Text = new HtmlTextView();

            _contextButton = new UIButton();
            _contextButton.TouchUpInside += ContextButtonOnTouchUpInside;
            var contextButtonImage = UIImage.FromBundle("Icons/icon_more.png");
            _contextButton.SetImage(contextButtonImage, UIControlState.Normal);

            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.BarShadowColor };

            ContentView.AddSubviews(_avatar, _displayName, _timeAgo, Text, _bottomRuler, _contextButton);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _contextButton.TouchUpInside -= ContextButtonOnTouchUpInside;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            _bottomRulerLeftConstraint = _bottomRuler.AtLeftOf(ContentView, 14).ToLayoutConstraints().First();
            _bottomRulerRightConstraint = _bottomRuler.AtRightOf(ContentView, 14).ToLayoutConstraints().First();

            ContentView.AddConstraint(_bottomRulerLeftConstraint);
            ContentView.AddConstraint(_bottomRulerRightConstraint);

            ContentView.AddConstraints(
                _avatar.AtTopOf(ContentView, 14),
                _avatar.AtLeftOf(ContentView, 14),
                _avatar.Height().EqualTo(35),
                _avatar.Width().EqualTo(35),

                _displayName.WithSameTop(_avatar).Minus(2),
                _displayName.Height().EqualTo(20),
                _displayName.ToRightOf(_avatar, 13),

                _timeAgo.ToRightOf(_displayName, 7),
                _timeAgo.WithSameBottom(_displayName),
                //_timeAgo.AtLeftOf(ContentView, 14),

                Text.Below(_displayName, 3),
                Text.ToLeftOf(_contextButton, 10),
                Text.ToRightOf(_avatar, 10),

                _contextButton.AtRightOf(ContentView, 10),
                _contextButton.AtTopOf(ContentView, 5),
                _contextButton.Width().EqualTo(30),
                _contextButton.Height().EqualTo(30),

                _bottomRuler.Height().EqualTo(1),
                _bottomRuler.AtBottomOf(ContentView)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<CommentCell, Comment>();
                set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
                set.Bind(_avatar.Tap()).For(tap => tap.Command).To(vm => vm.UserCommand);
                set.Bind(_displayName).To(vm => vm.DisplayName);
                set.Bind(_timeAgo).To(vm => vm.DateTime).WithConversion("TimeAgo");
                //set.Bind(_text).For(p => p.AttributedText).To(x => x.Text).WithConversion("HtmlAttributedText").WithFallback(HtmlAttributedTextValueConverter.FallBackString);
                set.Apply();
            });
        }

        #endregion Constructor

        #region Cell

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();

            Text.AttributedText = new NSAttributedString(string.Empty);
        }

        #endregion Cell

        #region Public

        public void SetRulerFullWidth(bool fullWidth)
        {
            if (fullWidth)
            {
                _bottomRulerLeftConstraint.Constant = 0;
                _bottomRulerRightConstraint.Constant = 0;
            }
            else
            {
                _bottomRulerLeftConstraint.Constant = 14;
                _bottomRulerRightConstraint.Constant = -14;
            }
        }

        #endregion Public

        #region Context menu

        private void ContextButtonOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            var actionSheet = new UIActionSheet(Settings.GetResource(ResKeys.mobile_post_menu_title));
            actionSheet.AddButton(Settings.GetResource(ResKeys.mobile_post_menu_report));
            actionSheet.AddButton(Settings.GetResource(ResKeys.platform_btn_cancel));
            actionSheet.CancelButtonIndex = 1;
            actionSheet.Clicked += async (o, args) =>
            {
                switch (args.ButtonIndex)
                {
                    case 0:
                        await ((Comment)DataContext).ReportComment();
                        break;
                    case 1:
                        // Cancel
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("args.ButtonIndex");
                }
            };
            actionSheet.ShowInView(_contextButton);
        }

        #endregion Context menu
    }
}
