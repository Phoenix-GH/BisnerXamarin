using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.Touch.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    public abstract class FeedPostCellBase<TSource> : MvxTableViewCell
        where TSource : IFeedPost
    {
        #region Constructor

        // Controls
        private UIView _backPanel, _topBorder, _bottomBorder, _ruler, _contentContainer;
        private AvatarImage _avatarImage;
        private UILabel _displayName, _timeAgo;
        private UIButton _commentButton, _likeButton;

        private MvxImageViewLoader _imageViewLoader;

        protected FeedPostCellBase(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        private void SetupSubViews()
        {
            // Backpanel
            _backPanel = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _topBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _bottomBorder = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            // Header
            var image = UIImage.FromBundle("Icons/default_avatar.jpg");
            _avatarImage = new AvatarImage(image);
            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14.5f), TextColor = iOS.Appearance.Colors.DefaultTextColor, Text = "Display Name" };
            _timeAgo = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(11.86f), TextColor = iOS.Appearance.Colors.SubTextColor, Text = "30 minutes ago" };

            // Content
            _contentContainer = new UIView { BackgroundColor = UIColor.Clear, };

            // Footer
            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _commentButton = new FeedButton { ImageTopSpacing = 3 };
            _likeButton = new FeedButton { ImageBottomSpacing = 3 };

            // Add views
            ContentView.Add(_backPanel);
            ContentView.Add(_topBorder);
            ContentView.Add(_bottomBorder);
            ContentView.Add(_avatarImage);
            ContentView.Add(_displayName);
            ContentView.Add(_timeAgo);
            ContentView.Add(_contentContainer);
            ContentView.Add(_ruler);
            ContentView.Add(_commentButton);
            ContentView.Add(_likeButton);
            //_imageViewLoader = new MvxImageViewLoader(() => _avatarImage) { DefaultImagePath = "res:Icons/default_avatar.jpg" };

            foreach (var uiView in ControllsToAdd())
            {
                _contentContainer.Add(uiView);
            }
        }

        /// <summary>
        /// Add controls to the content container
        /// </summary>
        /// <returns></returns>
        protected abstract List<UIView> ControllsToAdd();

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _backPanel.AtTopOf(ContentView, 5),
                _backPanel.AtBottomOf(ContentView, 5),
                _backPanel.AtRightOf(ContentView, 0),
                _backPanel.AtLeftOf(ContentView, 0),
                _backPanel.Height().GreaterThanOrEqualTo(40),

                _topBorder.AtTopOf(_backPanel),
                _topBorder.AtLeftOf(_backPanel),
                _topBorder.AtRightOf(_backPanel),
                _topBorder.Height().EqualTo(1),

                _bottomBorder.AtBottomOf(_backPanel),
                _bottomBorder.AtLeftOf(_backPanel),
                _bottomBorder.AtRightOf(_backPanel),
                _bottomBorder.Height().EqualTo(1),

                _avatarImage.Below(_topBorder, 14),
                _avatarImage.AtLeftOf(_backPanel, 14),
                _avatarImage.Height().EqualTo(45),
                _avatarImage.Width().EqualTo(45),

                _displayName.WithSameCenterY(_avatarImage).Plus(-13),
                _displayName.ToRightOf(_avatarImage, 10),
                _displayName.AtRightOf(_backPanel, 13),

                _timeAgo.Below(_displayName, 3),
                _timeAgo.WithSameLeft(_displayName),
                _timeAgo.WithSameRight(_displayName),

                _contentContainer.Below(_avatarImage, 10),
                _contentContainer.WithSameLeft(_avatarImage),
                _contentContainer.WithSameRight(_displayName),
                _contentContainer.Height().GreaterThanOrEqualTo(30),

                _ruler.Below(_contentContainer, 15),
                _ruler.Height().EqualTo(1),
                _ruler.WithSameLeft(_avatarImage),
                _ruler.WithSameRight(_displayName),

                _commentButton.Below(_ruler),
                _commentButton.WithSameLeft(_avatarImage),
                _commentButton.Above(_bottomBorder),
                _commentButton.Height().EqualTo(50),

                _likeButton.WithSameCenterY(_commentButton),
                _likeButton.ToRightOf(_commentButton, 15),
                _likeButton.WithSameHeight(_commentButton)
                );

            // This call does not set the property on the child views of child views so we call this on the content container as well
            _contentContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _contentContainer.AddConstraints(AddContentConstraints(_contentContainer));
        }

        /// <summary>
        /// Add constraints to the content container views
        /// </summary>
        /// <param name="contentContainer"></param>
        /// <returns></returns>
        protected abstract List<FluentLayout> AddContentConstraints(UIView contentContainer);

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<FeedPostCellBase<TSource>, TSource>();
                set.Bind(_displayName).To(item => item.DisplayName);
                set.Bind(_timeAgo).To(item => item.DateTime).WithConversion("TimeAgo");
                //set.Bind(_imageViewLoader).To(item => item.AvatarUrl).WithConversion("AvatarUrl");
                set.Bind(_avatarImage.Tap()).For(tap => tap.Command).To(vm => vm.UserCommand);
                set.Bind(_commentButton).For("Comment").To(vm => vm.HasCommented);
                set.Bind(_commentButton).For("Title").To(item => item.CommentButtonText);
                set.Bind(_commentButton.Tap()).For(tap => tap.Command).To(x => x.CommentCommand);
                set.Bind(_likeButton).For("Like").To(vm => vm.HasLiked);
                set.Bind(_likeButton).For("Title").To(item => item.LikeButtonText);
                set.Bind(_likeButton.Tap()).For(tap => tap.Command).To(x => x.LikeCommand);
                AddBindingsToSet(set);
                set.Apply();
            });
        }

        /// <summary>
        /// Add bindings to the base class set
        /// </summary>
        /// <param name="set"></param>
        protected abstract void AddBindingsToSet(MvxFluentBindingDescriptionSet<FeedPostCellBase<TSource>, TSource> set);

        #endregion Constructor

        #region Overrides

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;
        }

        #endregion Override
    }
}