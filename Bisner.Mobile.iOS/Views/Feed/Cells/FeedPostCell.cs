using System;
using System.Linq;
using System.Threading.Tasks;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    public class FeedPostCell : MvxTableViewCell, IFeedCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("FeedPostCell");

        // Controls
        private UIView _backPanel, _topBorder, _bottomBorder, _ruler;
        private AvatarImageView _avatarImageView;
        private UILabel _displayName, _timeAgo;
        private HtmlTextView _postText;
        private UIImageView _mainImage, _leftSubImage, _rightSubImage;
        private UILabel _title, _date, _time, _location;
        private UIView _dateBackPanel, _infoContainer, _infoContainerPlaceholder;
        private UIButton _commentButton, _likeButton, _contextButton;

        protected FeedPostCell(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings(true);
        }

        public HtmlTextView PostText => _postText;

        #endregion Constructor

        #region Cell

        private FeedPost ViewModel => DataContext as FeedPost;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (HasOverlay)
            {
                if (!_mainImage.Subviews.Any())
                {
                    _mainImage.SetOverlay(0.5f);
                }

                foreach (var subview in _mainImage.Subviews)
                {
                    subview.Hidden = false;

                    var frame = subview.Frame;

                    frame.Width = UIScreen.MainScreen.Bounds.Width;
                    frame.Height = !string.IsNullOrWhiteSpace(ViewModel.MainImageUrl) ? _mainImageHeight : 0;

                    subview.Frame = frame;
                }
            }
            else if (_mainImage.Subviews.Any())
            {
                foreach (var subview in _mainImage.Subviews)
                {
                    subview.Hidden = true;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _contextButton.TouchUpInside -= ContextButtonOnTouchUpInside;
        }

        #endregion Cell

        #region Setup

        private void SetupSubViews()
        {
            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;
            SelectionStyle = UITableViewCellSelectionStyle.None;

            // Backpanel
            _backPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White, AccessibilityIdentifier = "BackPanel" };
            _topBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderTop, AccessibilityIdentifier = "TopBorder" };
            _bottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderBottom, AccessibilityIdentifier = "BottomBorder" };

            // Header
            _avatarImageView = new AvatarImageView { AccessibilityIdentifier = "AvatarImage" };
            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14.5f), TextColor = iOS.Appearance.Colors.DefaultTextColor, Text = "Display Name", AccessibilityIdentifier = "DisplayName" };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11.86f), TextColor = iOS.Appearance.Colors.SubTextColor, Text = "30 minutes ago", AccessibilityIdentifier = "TimeAgo" };
            _contextButton = new UIButton();
            _contextButton.TouchUpInside += ContextButtonOnTouchUpInside;

            var contextButtonImage = UIImage.FromBundle("Icons/icon_more.png");
            _contextButton.SetImage(contextButtonImage, UIControlState.Normal);

            // Text
            _postText = new HtmlTextView(ShouldInteractWithUrl) { AccessibilityIdentifier = "PostText" };

            // Content
            _mainImage = new UIImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, BackgroundColor = iOS.Appearance.Colors.BackgroundColor };

            // Text for events and other items
            _date = new UILabel { TextColor = iOS.Appearance.Colors.DefaultTextColor, Font = iOS.Appearance.Fonts.LatoWithSize(13), TextAlignment = UITextAlignment.Center };
            _dateBackPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _dateBackPanel.Layer.CornerRadius = 4.0f;

            _infoContainer = new UIView();
            _infoContainerPlaceholder = new UIView();

            _title = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoBlackWithSize(24), Lines = 1, LineBreakMode = UILineBreakMode.TailTruncation };
            _time = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(15) };
            _location = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(15), Lines = 1, LineBreakMode = UILineBreakMode.TailTruncation };

            _leftSubImage = new UIImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, BackgroundColor = iOS.Appearance.Colors.BackgroundColor };
            _rightSubImage = new UIImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, BackgroundColor = iOS.Appearance.Colors.BackgroundColor };

            // Footer
            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _commentButton = new FeedButton { ImageTopSpacing = 3 };
            _likeButton = new FeedButton { ImageBottomSpacing = 3 };

            // Add views
            ContentView.AddSubviews(_backPanel, _topBorder, _bottomBorder, _avatarImageView, _displayName, _timeAgo, _postText, _mainImage, _infoContainer, _infoContainerPlaceholder, _title, _dateBackPanel, _date, _time, _location, _leftSubImage, _rightSubImage, _ruler, _commentButton, _likeButton, _contextButton);
        }

        private bool ShouldInteractWithUrl(string url)
        {
            ViewModel.ShowUrlWarning(url);

            return true;
        }

        private NSLayoutConstraint _backPanelBottomConstraint, _mainImageHeightConstraint, _mainImageBottomSpacingConstraint, _subImageHeightConstraint, _subImageSpacingConstraint;

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            // Backpanel bottom constraint for usage in detailsview
            _backPanelBottomConstraint = _backPanel.AtBottomOf(ContentView, 0).WithIdentifier("BackPanelBottomToView").ToLayoutConstraints().First();

            // Main image constraints
            _mainImageHeightConstraint = _mainImage.Height().EqualTo(0).WithIdentifier("MainImageHeightConstraint").ToLayoutConstraints().First();
            _mainImageBottomSpacingConstraint = _mainImage.Above(_leftSubImage, 3).WithIdentifier("MainImageAboveLeftSub").ToLayoutConstraints().First();

            // Sub image constraints
            _subImageSpacingConstraint = _leftSubImage.Above(_ruler, 8).WithIdentifier("LeftSubImageAboveRuler").ToLayoutConstraints().First();
            _subImageHeightConstraint = _leftSubImage.Height().EqualTo(50).WithIdentifier("LeftSubHeight").ToLayoutConstraints().First();

            ContentView.AddConstraint(_backPanelBottomConstraint);
            ContentView.AddConstraint(_mainImageHeightConstraint);
            ContentView.AddConstraint(_mainImageBottomSpacingConstraint);
            ContentView.AddConstraint(_subImageSpacingConstraint);
            ContentView.AddConstraint(_subImageHeightConstraint);

            ContentView.AddConstraints(
                // TOP
                _backPanel.AtTopOf(ContentView, 5).WithIdentifier("BackPanelAtTopContentView"),
                _backPanel.AtRightOf(ContentView, 0).WithIdentifier("BackPanelAtRightOfContentView"),
                _backPanel.AtLeftOf(ContentView, 0).WithIdentifier("BackPanelAtLeftOfContentView"),

                _topBorder.AtTopOf(_backPanel).WithIdentifier("TopBorderAtTopOfBackpanel"),
                _topBorder.AtLeftOf(_backPanel).WithIdentifier("TopBorderAtLeftOfBackpanel"),
                _topBorder.AtRightOf(_backPanel).WithIdentifier("TopBorderAtRightOfBackpanel"),
                _topBorder.Height().EqualTo(1).WithIdentifier("TopBorderHeightEqualTo1"),

                _avatarImageView.Below(_topBorder, 14).WithIdentifier("AvatarImageBelowTopBorder"),
                _avatarImageView.AtLeftOf(_backPanel, 14).WithIdentifier("AvatarImageAtLeftOfBackPanel"),
                _avatarImageView.Height().EqualTo(45).WithIdentifier("AvatarImageHeightEquealTo"),
                _avatarImageView.Width().EqualTo(45).WithIdentifier("AvatarImageWidthEqualTo"),

                _displayName.Bottom().EqualTo().CenterYOf(_avatarImageView).Minus(1.5f).WithIdentifier("DisplayNameBottomEqualtoCenterYOfAvatarImage"),
                _displayName.ToRightOf(_avatarImageView, 10).WithIdentifier("DisplayNameToRightOfAvatarImage"),
                _displayName.ToLeftOf(_contextButton, 13),

                _timeAgo.Below(_displayName, 3).WithIdentifier("TimeAgoBelowDisplayName"),
                _timeAgo.WithSameLeft(_displayName).WithIdentifier("TimeAgoWithSameLeftDisplayName"),
                _timeAgo.WithSameRight(_displayName).WithIdentifier("TimeAgoWithSameRightDisplayName"),

                _postText.Below(_avatarImageView, 8).WithIdentifier("PostTextBelowAvatarImage"),
                _postText.AtLeftOf(_backPanel, 11).WithIdentifier("PostTextAtLeftOfBackPanel"),
                _postText.AtRightOf(_backPanel, 14).WithIdentifier("PostTextAtRightOfBackPanel"),

                _contextButton.AtRightOf(_backPanel, 10),
                _contextButton.AtTopOf(_backPanel, 5),
                _contextButton.Width().EqualTo(30),
                _contextButton.Height().EqualTo(30),

                // CONTENT
                _mainImage.AtLeftOf(_backPanel).WithIdentifier("MainImageAtLeftOfBackPanel"),
                _mainImage.AtRightOf(_backPanel).WithIdentifier("MainImageAtRightOfBackPanel"),

                // INFORMATION
                _dateBackPanel.AtTopOf(_mainImage, 20).WithIdentifier("DateBackPanelAtTopOfContentView"),
                _dateBackPanel.AtRightOf(_mainImage, 20).WithIdentifier("DateBackPanelAtRightOfContentView"),
                _dateBackPanel.Width().EqualTo(90).WithIdentifier("DateBackPanelWidth"),
                _dateBackPanel.Height().EqualTo(25).WithIdentifier("DateBackPanelHeight"),

                _date.WithSameCenterY(_dateBackPanel).WithIdentifier("DateAtRightOfContentView"),
                _date.WithSameCenterX(_dateBackPanel).WithIdentifier("DateAtBottomOfContentView"),

                _infoContainerPlaceholder.AtTopOf(_mainImage),
                _infoContainerPlaceholder.AtLeftOf(_mainImage),
                _infoContainerPlaceholder.AtRightOf(_mainImage),
                _infoContainerPlaceholder.AtBottomOf(_mainImage),

                _infoContainer.AtLeftOf(_infoContainerPlaceholder).WithIdentifier("InfoContainerAtLeftOfBackground"),
                _infoContainer.AtRightOf(_infoContainerPlaceholder).WithIdentifier("InforContainerAtRightOfBackground"),
                _infoContainer.WithSameCenterY(_infoContainerPlaceholder),

                _title.AtLeftOf(_infoContainer, 20).WithIdentifier("NameAtLeftOfBackgorund"),
                _title.AtRightOf(_infoContainer, 20).WithIdentifier("NameAtRightBackground"),
                _title.AtTopOf(_infoContainer, 4),

                _time.Below(_title, 5).WithIdentifier("TimeAboveLocation"),
                _time.AtLeftOf(_infoContainer, 20).WithIdentifier("TimeAtLeftOfBackground"),
                _time.AtRightOf(_infoContainer, 20).WithIdentifier("TimeAtRightOfBackground"),

                _location.Below(_time, 3),
                _location.AtBottomOf(_infoContainer),
                _location.AtLeftOf(_infoContainer, 20),
                _location.AtRightOf(_infoContainer, 20),

                // ---------------------------------------------

                _leftSubImage.AtLeftOf(_backPanel).WithIdentifier("LeftSubImageAtLeftOfBackPanel"),

                _rightSubImage.WithSameTop(_leftSubImage).WithIdentifier("RightSubImageWithSameTopLeftSub"),
                _rightSubImage.WithSameWidth(_leftSubImage).WithIdentifier("RightSubImageWithSameWidthLeftSub"),
                _rightSubImage.AtRightOf(_backPanel).WithIdentifier("RightSubImageAtRightOfBackPanel"),
                _rightSubImage.ToRightOf(_leftSubImage, 3).WithIdentifier("RightSubImageToRightOfLeftSub"),
                _rightSubImage.WithSameHeight(_leftSubImage).WithIdentifier("RightSubImageWithSameHeightLeftSub"),

                // BOTTOM
                _ruler.Above(_commentButton).WithIdentifier("RulerBelowContentContainer"),
                _ruler.Height().EqualTo(1).WithIdentifier("RulerHeightEqualTo1"),
                _ruler.WithSameLeft(_avatarImageView).WithIdentifier("RulerWithSameLeftAvatarImage"),
                _ruler.WithSameRight(_displayName).WithIdentifier("RulerWithSameRightDisplayName"),

                _commentButton.Above(_bottomBorder).WithIdentifier("CommentButtonBelowRuler"),
                _commentButton.WithSameLeft(_avatarImageView).WithIdentifier("CommentWithSameLeftAvatarImage"),
                _commentButton.Height().EqualTo(50).WithIdentifier("CommentButtonHeightEqualTo"),

                _likeButton.WithSameCenterY(_commentButton).WithIdentifier("LikeButtonWithSameCenterYCommentButton"),
                _likeButton.ToRightOf(_commentButton, 15).WithIdentifier("LikeButtonToRightOfCommentButton"),
                _likeButton.WithSameHeight(_commentButton).WithIdentifier("LikeButtonWithSameHeightCommentButton"),

                _bottomBorder.AtBottomOf(_backPanel).WithIdentifier("BottomBorderAtBottomOfBackPanel"),
                _bottomBorder.AtLeftOf(_backPanel).WithIdentifier("BottomBorderAtLeftOfBackPanel"),
                _bottomBorder.AtRightOf(_backPanel).WithIdentifier("BottomBorderAtRightOfBackPanel"),
                _bottomBorder.Height().EqualTo(1).WithIdentifier("BottomBorderHeightEqualTo1")
                );
        }

        private void SetupBindings(bool commentEnabled)
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<FeedPostCell, IFeedPost>();
                // TOP
                set.Bind(_displayName).To(item => item.DisplayName);
                set.Bind(_displayName.Tap()).For(tap => tap.Command).To(vm => vm.UserCommand);
                set.Bind(_timeAgo).To(item => item.DateTime).WithConversion("TimeAgo");
                set.Bind(_avatarImageView).For("AvatarImageUrl").To(item => item.AvatarUrl).WithConversion("ImageUrl");
                set.Bind(_avatarImageView.Tap()).For(tap => tap.Command).To(vm => vm.UserCommand);

                // CONTENT
                set.Bind(_mainImage).For("ImageUrl").To(vm => vm.MainImageUrl).WithConversion("ImageUrl");
                set.Bind(_mainImage.Tap()).For(tap => tap.Command).To(vm => vm.MainImageCommand);

                set.Bind(_leftSubImage).For("ImageUrl").To(vm => vm.LeftSubImageUrl).WithConversion("ImageUrl");
                set.Bind(_leftSubImage.Tap()).For(tap => tap.Command).To(vm => vm.LeftSubImageCommand);
                set.Bind(_rightSubImage).For("ImageUrl").To(vm => vm.RightSubImageUrl).WithConversion("ImageUrl");
                set.Bind(_rightSubImage.Tap()).For(tap => tap.Command).To(vm => vm.RightSubImageCommand);

                // BOTTOM
                set.Bind(_commentButton).For("Comment").To(vm => vm.HasCommented);
                if (commentEnabled)
                {
                    set.Bind(_commentButton.Tap()).For(tap => tap.Command).To(x => x.CommentCommand);
                }
                set.Bind(_commentButton).For("Title").To(item => item.CommentButtonText);
                set.Bind(_likeButton).For("Like").To(vm => vm.HasLiked);
                set.Bind(_likeButton).For("Title").To(item => item.LikeButtonText);
                set.Bind(_likeButton.Tap()).For(tap => tap.Command).To(x => x.LikeCommand);
                set.Bind(_title).To(vm => vm.Title);
                set.Bind(_title.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Bind(_time).To(vm => vm.ItemDateTimeText);
                set.Bind(_time.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Bind(_date).To(vm => vm.ItemDateTime).WithConversion("ShortDate");
                set.Bind(_date.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Bind(_location).To(vm => vm.Location);
                set.Bind(_location.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Bind(_infoContainer.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Bind(_infoContainerPlaceholder.Tap()).For(tap => tap.Command).To(vm => vm.CommentCommand);
                set.Apply();
            });
        }

        #endregion Setup

        #region Context menu

        private void ContextButtonOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            var actionSheet = new UIActionSheet(Settings.GetResource(ResKeys.mobile_post_menu_title));
            actionSheet.AddButton(Settings.GetResource(ResKeys.mobile_post_menu_report));
            actionSheet.AddButton(Settings.GetResource(ResKeys.mobile_post_menu_like));
            actionSheet.AddButton(Settings.GetResource(ResKeys.mobile_post_menu_follow));
            actionSheet.AddButton(Settings.GetResource(ResKeys.platform_btn_cancel));
            actionSheet.CancelButtonIndex = 3;
            actionSheet.Clicked += async (o, args) =>
            {
                switch (args.ButtonIndex)
                {
                    case 0:
                        // Report
                        await ((FeedPost)DataContext).ReportPostAsync();
                        break;
                    case 1:
                        // Like
                        ((FeedPost)DataContext).LikeCommand.Execute();
                        break;
                    case 2:
                        // Follow
                        await ((FeedPost)DataContext).FollowPostAsync();
                        break;
                    case 3:
                        // Cancel
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("args.ButtonIndex");
                }
            };
            actionSheet.ShowInView(_contextButton);
        }

        #endregion Context menu

        #region IFeedCell

        public void SetCommentingEnabled(bool enabled)
        {
            BindingContext.ClearAllBindings();

            SetupBindings(enabled);
        }

        public void SetBottomSpacing(bool bottomSpacing)
        {
            ContentView.SetNeedsUpdateConstraints();

            if (bottomSpacing)
            {
                _bottomBorder.BackgroundColor = iOS.Appearance.Colors.RulerColor;
                _backPanelBottomConstraint.Constant = -5;
            }
            else
            {
                _bottomBorder.BackgroundColor = UIColor.Clear;
                _backPanelBottomConstraint.Constant = 0;
            }

            ContentView.LayoutIfNeeded();
        }

        private readonly nfloat _mainImageHeight = UIScreen.MainScreen.Bounds.Width * 0.6f;
        private readonly nfloat _subImageHeight = (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f;

        public void SetMainImageVisible()
        {
            _mainImageHeightConstraint.Constant = _mainImageHeight;
            _mainImageBottomSpacingConstraint.Constant = 0;
            _subImageSpacingConstraint.Constant = -8;
            _subImageHeightConstraint.Constant = 0;
        }

        public void SetSubImagesVisible()
        {
            _mainImageHeightConstraint.Constant = 0;
            _mainImageBottomSpacingConstraint.Constant = 0;
            _subImageSpacingConstraint.Constant = -8;
            _subImageHeightConstraint.Constant = _subImageHeight;
        }

        public void SetAllImagesVisible()
        {
            _mainImageHeightConstraint.Constant = _mainImageHeight;
            _mainImageBottomSpacingConstraint.Constant = -3;
            _subImageSpacingConstraint.Constant = -8;
            _subImageHeightConstraint.Constant = _subImageHeight;
        }

        public void SetNoImagesVisible()
        {
            _mainImageHeightConstraint.Constant = 0;
            _mainImageBottomSpacingConstraint.Constant = 0;
            _subImageSpacingConstraint.Constant = 0;
            _subImageHeightConstraint.Constant = 0;
        }

        public void SetInformationVisible(bool visible)
        {
            if (visible)
            {
                _title.Hidden = false;
                _date.Hidden = false;
                _dateBackPanel.Hidden = false;
                _location.Hidden = false;
                _time.Hidden = false;
                _infoContainer.Hidden = false;
                _infoContainerPlaceholder.Hidden = false;
                HasOverlay = true;
            }
            else
            {
                _title.Hidden = true;
                _date.Hidden = true;
                _dateBackPanel.Hidden = true;
                _location.Hidden = true;
                _time.Hidden = true;
                _infoContainer.Hidden = true;
                _infoContainerPlaceholder.Hidden = true;
                HasOverlay = false;
            }
        }

        /// <summary>
        /// When set to true adds an transparent dark overlay to the main image
        /// </summary>
        public bool HasOverlay { get; set; }

        #endregion IFeedCell
    }
}