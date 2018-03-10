using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Feed.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;
using static Bisner.Mobile.iOS.Appearance;

namespace Bisner.Mobile.iOS.Views.Feed
{
    partial class DetailsView : MentionViewBase<DetailsViewModel>
    {
        #region Constructor

        private GenericTableviewSourceWithHeight<IItemBase> _source;

        private NSObject _didAppearNotification, _textFieldChangeNotification;

        private MvxSubscriptionToken _newCommentToken;

        public DetailsView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IItemBase>(ItemTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is IFeedPost)
                    {
                        return FeedPostCell.Identifier;
                    }

                    return CommentCell.Identifier;
                },
                ModifyCellFunc = (cell, path, item) =>
                {
                    ModifyPostCell(item, cell);

                    ModifyCommentCell(cell, item, path);
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    if (item is IFeedPost)
                    {
                        return GetFeedPostHeight(item as IFeedPost);
                    }

                    return GetCommentCellHeight(item as IComment);
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    if (item is IFeedPost)
                    {
                        return GetFeedPostHeight(item as IFeedPost);
                    }

                    return GetCommentCellHeight(item as IComment);
                },
                AutoDeselect = true,
                UseAnimations = true,
                AddAnimation = UITableViewRowAnimation.Top,
            };

            ItemTable.RegisterClassForCellReuse(typeof(FeedPostCell), FeedPostCell.Identifier);

            ItemTable.AllowsSelection = false;
            ItemTable.BackgroundColor = Colors.BackgroundColor;
            ItemTable.ContentInset = new UIEdgeInsets(5, 0, 0, 0);
            ItemTable.Source = _source;
        }

        private void SetupAppearance()
        {
            Input.Layer.BorderColor = Colors.BarShadowColor.CGColor;
            Input.BackgroundColor = Colors.BackgroundColor;
            Input.Placeholder = Settings.GetResource(ResKeys.mobile_comment_leave_reply);
            Send.SetTitle(Settings.GetResource(ResKeys.mobile_post_btn_post), UIControlState.Normal);

            InputBoxTopRuler.BackgroundColor = Colors.BarShadowColor;
        }

        private void AddGestureRecognizer()
        {
            var swipeGesture = new UISwipeGestureRecognizer(OnSwipeDetected)
            {
                Direction = UISwipeGestureRecognizerDirection.Down,
            };

            Input.AddGestureRecognizer(swipeGesture);
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<DetailsView, DetailsViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(Input).To(vm => vm.CommentInput);
            set.Bind(Input).For(t => t.Enabled).To(vm => vm.IsNotLoading);
            set.Bind(Send).To(vm => vm.CommentCommand);
            set.Apply();

            ItemTable.ReloadData();
        }

        #endregion Setup

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("DETAILSVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupAppearance();
            AddGestureRecognizer();
            SetupBindings();

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            CommentButtonRightConstraint.Constant -= Send.Frame.Width + 10;

            _originalCommentButtonConstraintValue = CommentButtonRightConstraint.Constant;
            _originalInputConstraintValue = InputRightConstraint.Constant;

            _originalInputBoxConstraint = InputBoxBottomConstraint.Constant - TabBarController.TabBar.Frame.Height;

            if (Settings.UserRoles.All(r => r != Home.Feed.Comment.ToLower()))
            {
                InputBoxHieghtConstraint.Constant = 0;
            }

            // Set screen name for analytics
            ScreenName = "DetailsView postId = " + ViewModel.PostId;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (IsMovingToParentViewController)
            {
                Input.ShouldReturn = ShouldReturn;

                Send.TouchUpInside += SendOnTouchUpInside;

                _didAppearNotification = UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => { ViewModel.Update(); });

                _textFieldChangeNotification = NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, TextFieldTextChanged);

                _newCommentToken = Mvx.Resolve<IMvxMessenger>().Subscribe<NewCommentMessage>(message => { ScrollToBottom(true); });
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                Input.ShouldReturn = null;

                Send.TouchUpInside -= SendOnTouchUpInside;

                _didAppearNotification.Dispose();
                _didAppearNotification = null;

                _textFieldChangeNotification.Dispose();
                _textFieldChangeNotification = null;

                _newCommentToken.Dispose();
                _newCommentToken = null;
            }
        }

        #endregion ViewController

        #region TextField

        private nfloat _originalCommentButtonConstraintValue, _originalInputConstraintValue;

        private void TextFieldTextChanged(NSNotification notification)
        {
            var field = (UITextField)notification.Object;

            UIView.Animate(0.1f, () =>
            {
                View.SetNeedsUpdateConstraints();

                if (field.Text.Length > 0)
                {
                    CommentButtonRightConstraint.Constant = _originalCommentButtonConstraintValue + Send.Frame.Width + 10;
                    InputRightConstraint.Constant = _originalInputConstraintValue + Send.Frame.Width + 10;
                }
                else
                {
                    CommentButtonRightConstraint.Constant = _originalCommentButtonConstraintValue;
                    InputRightConstraint.Constant = _originalInputConstraintValue;
                }

                View.LayoutIfNeeded();
            });
        }

        #endregion TextField

        #region Keyboard

        public override bool HandlesKeyboardNotifications => true;

        private nfloat _originalInputBoxConstraint;
        private nfloat _currentKeyboardHeight;

        // TODO: Base class voor table scrolls
        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            View.SetNeedsUpdateConstraints();

            var contentOffset = ItemTable.ContentOffset;

            var keyboardHeightChange = visible ? (nfloat)Math.Abs(keyboardHeight - _currentKeyboardHeight) : (nfloat)Math.Abs(_currentKeyboardHeight - 0);

            if (visible)
            {
                InputBoxBottomConstraint.Constant = keyboardHeight + _originalInputBoxConstraint;

                contentOffset.Y += keyboardHeightChange;
            }
            else
            {
                InputBoxBottomConstraint.Constant = _originalInputBoxConstraint;

                contentOffset.Y -= keyboardHeightChange;
            }

            _currentKeyboardHeight = visible ? keyboardHeight : 0;

            ItemTable.SetContentOffset(contentOffset, false);

            View.LayoutIfNeeded();
        }

        protected override void OnAfterKeyboardAnimationFinished(bool visible, nfloat keyboardHeight)
        {
            base.OnAfterKeyboardAnimationFinished(visible, keyboardHeight);

            if (visible)
                ScrollToBottom(true);
        }

        #endregion Keyboard

        #region Helpers

        private void ScrollToBottom(bool animated)
        {
            ItemTable.ScrollToBottom(animated);
        }

        private void OnSwipeDetected()
        {
            View.FindFirstResponder().ResignFirstResponder();
        }

        /// <summary>
        /// Textfield handler to resign the keyboard
        /// </summary>
        /// <param name="textField"></param>
        /// <returns></returns>
        private bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();

            return true;
        }

        private void SendOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            var notification = NSNotification.FromName(UITextField.TextFieldTextDidChangeNotification, Input);

            TextFieldTextChanged(notification);
            //ChatInput.Text = string.Empty; // this will not generate change text event
            ScrollToBottom(true);
        }

        #endregion Helpers

        #region Base modifications

        protected override NSLayoutConstraint TabBarTopConstraint => InputBoxBottomConstraint;

        protected override bool SupportsHideTabBar => true;

        protected override bool EnableCustomBackButton => true;

        #endregion Base modifications

        #region CellText

        private readonly Dictionary<Guid, NSAttributedString> _htmlTexts = new Dictionary<Guid, NSAttributedString>();
        private readonly Dictionary<Guid, nfloat> _textHeights = new Dictionary<Guid, nfloat>();

        private void SetCommentCellText(IComment comment, UITableViewCell cell)
        {
            if (!string.IsNullOrWhiteSpace(comment.Text))
            {
                var text = GetText(comment.Id, comment.Text);

                var commentCell = cell as CommentCell;

                if (commentCell != null)
                {
                    commentCell.Text.AttributedText = text;

                    var textSize = commentCell.Text.SizeThatFits(new CGSize(commentCell.Text.Frame.Width, double.MaxValue));

                    _textHeights[comment.Id] = textSize.Height;
                }
            }
        }

        private void SetFeedPostCellText(IFeedPost post, UITableViewCell cell)
        {
            if (!string.IsNullOrWhiteSpace(post.Text))
            {
                var text = GetText(post.Id, post.Text);

                var feedCell = cell as FeedPostCell;

                if (feedCell != null)
                {
                    feedCell.PostText.AttributedText = text;

                    CalculateTextHeight(post, feedCell);
                }
            }
        }

        private NSAttributedString GetText(Guid id, string text)
        {
            NSAttributedString attributedText;

            if (!_htmlTexts.ContainsKey(id))
            {
                attributedText = text.ConvertHtml();

                _htmlTexts[id] = attributedText;
            }
            else
            {
                attributedText = _htmlTexts[id];
            }

            return attributedText;
        }

        private void CalculateTextHeight(IFeedPost post, FeedPostCell feedCell)
        {
            var textSize = feedCell.PostText.SizeThatFits(new CGSize(feedCell.PostText.Frame.Width, double.MaxValue));

            _textHeights[post.Id] = textSize.Height;
        }

        /// <summary>
        /// Modify the cell when it is a post
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cell"></param>
        private void ModifyPostCell(IItemBase item, UITableViewCell cell)
        {
            var post = item as IFeedPost;

            if (post != null)
            {
                SetFeedPostCellText(post, cell);

                var postCell = cell as FeedPostCell;

                if (postCell != null)
                {
                    postCell.SetBottomSpacing(false);
                    postCell.SetCommentingEnabled(false);

                    if (post.NumberOfImages == 1)
                    {
                        postCell.SetMainImageVisible();
                    }
                    else if (post.NumberOfImages == 2)
                    {
                        postCell.SetSubImagesVisible();
                    }
                    else if (post.NumberOfImages > 2)
                    {
                        postCell.SetAllImagesVisible();
                    }
                    else
                    {
                        postCell.SetNoImagesVisible();
                    }

                    postCell.SetInformationVisible(post.Title != null);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="item"></param>
        /// <param name="path"></param>
        private void ModifyCommentCell(UITableViewCell cell, IItemBase item, NSIndexPath path)
        {
            var commentCell = cell as CommentCell;

            if (commentCell != null)
            {
                SetCommentCellText(item as IComment, cell);

                // Last cell has different ruler
                if (path.Section == ItemTable.IndexPathForLastRow.Section && path.Row == ItemTable.IndexPathForLastRow.Row)
                {
                    commentCell.SetRulerFullWidth(true);
                }
                else
                {
                    commentCell.SetRulerFullWidth(false);
                }
            }
        }

        #endregion CellText

        #region CellHeight

        private nfloat GetFeedPostHeight(IFeedPost post)
        {
            nfloat finalHeight = 0;

            // Top border height             1px
            // Top border <-> Avatar        14px
            // Avatar height                45px
            // Avatar <-> text               8px

            // Text                         CALCULATE
            // MainImage                    CALCULATE

            // Text <-> ruler                8px
            // Ruler height                  1px
            // Button height                50px
            // Bottom border height          1px
            // BottomSpacing (OPTIONAL)      5px
            //----------------------------------
            // Total                        133px

            // Calculate text height
            finalHeight += 133 + (_textHeights.ContainsKey(post.Id) ? _textHeights[post.Id] : 0);

            if (post.NumberOfImages == 1)
            {
                // 8 spacing below image
                finalHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
            }
            else if (post.NumberOfImages == 2)
            {
                // 8 spacing below image
                finalHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 8;
            }
            else if (post.NumberOfImages > 2)
            {
                // 8 spacing below image
                finalHeight += UIScreen.MainScreen.Bounds.Width * 0.6f + 8;
                finalHeight += (UIScreen.MainScreen.Bounds.Width - 3) / 2 * 0.6f + 3;
            }

            return finalHeight;
        }

        private nfloat GetCommentCellHeight(IComment comment)
        {
            nfloat finalHeight = 40;

            var textHeight = _textHeights.ContainsKey(comment.Id) ? _textHeights[comment.Id] : 0;

            return finalHeight + textHeight;
        }

        #endregion CellHeight

        #region Mentions

        protected override UIView ViewToPlaceMentions()
        {
            return Input;
        }

        #endregion Mentions
    }
}
