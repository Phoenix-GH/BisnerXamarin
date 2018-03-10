using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Chat;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Controls.Gestures;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Chat.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views.Gestures;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SDWebImage;
using UIKit;
using static Bisner.Mobile.iOS.Appearance;
using ChatConversationViewModel = Bisner.Mobile.Core.ViewModels.Chat.ChatConversationViewModel;

namespace Bisner.Mobile.iOS.Views.Chat
{
    partial class
        ChatConversationView : HideTabBarViewBase<ChatConversationViewModel>
    {
        #region Constructor

        private static int _numberAlive = 0;
        ~ChatConversationView()
        {
            _numberAlive--;
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!DESTROYING!!!!!!!!!!!!!!!!!!!!! - ChatConversationView - number alive : {0}", _numberAlive);
        }

        private GenericTableViewSource<IChatItem> _source;

        private AvatarImageView _avatar;
        private UIView _titleViewContainer;
        private UILabel _titleLabel, _titleSubLabel;

        private NSObject _didAppearNotification, _textFieldChangedNotification;

        private MvxSubscriptionToken _addMessageSub, _updateConversationSub;

        private UIBarButtonItem _backButton;

        public ChatConversationView(IntPtr handle)
            : base(handle)
        {
            _numberAlive++;
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!CREATING!!!!!!!!!!!!!!!!!!!!! - ChatConversationView - number alive : {0}", _numberAlive);
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("CHATCONVERSATIONVIEW RECIEVED MEMORY WARNING!!!!!!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupAppearance();
            //SetupTitleView();
            SetupTable();
            SetupBindings();
            SetupGestures();

            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            _originalInputBoxConstraint = InputBoxBottomConstraint.Constant - TabBarController.TabBar.Frame.Height;

            SendRightConstraint.Constant -= Send.Frame.Width + 10;

            _originalCommentButtonConstraintValue = SendRightConstraint.Constant;
            _originalInputConstraintValue = InputRightConstraint.Constant;

            // Set screen name for analytics
            ScreenName = "ChatConversationView";
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            Send.TouchUpInside -= InputTouchUpInside;

            if (IsMovingFromParentViewController)
            {
                RemoveHandlers();
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Send.TouchUpInside += InputTouchUpInside;

            MessageTable.ReloadData();
            _pendingScroll = true;

            if (IsMovingToParentViewController)
            {
                AddHandlers();
            }
        }

        private bool _pendingScroll;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (_pendingScroll)
            {
                ScrollToBottom(false);
                _pendingScroll = false;
            }
        }

        #endregion ViewController

        #region Setup

        private void SetupAppearance()
        {
            MessageTable.BackgroundColor = Colors.BackgroundColor;
            View.BackgroundColor = UIColor.White;

            ChatInput.BackgroundColor = Colors.BackgroundColor;
            ChatInput.Layer.BorderColor = Colors.BarShadowColor.CGColor;

            _avatar = new AvatarImageView { Frame = new CGRect(0, 0, 30, 30) };

            SDWebImageDownloader.SharedDownloader.DownloadImage(new NSUrl(Settings.BlobUrl + ViewModel.AvatarUrl), SDWebImageDownloaderOptions.ProgressiveDownload, null,
                (image, data, error, finished) =>
                {
                    if (image == null) return;

                    _avatar.Image = image;

                    var avatarButton = new UIBarButtonItem(_avatar);

                    NavigationItem.SetRightBarButtonItem(avatarButton, true);
                });

            // Add back button
            using (
                var backImage =
                    UIImage.FromBundle("Icons/back_arrow.png")
                        .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            {
                _backButton = new UIBarButtonItem(backImage, UIBarButtonItemStyle.Plain, null, null);
            }
            _backButton.Clicked += (sender, args) =>
            {
                NavigationController.PopViewController(true);
            };

            NavigationController.InteractivePopGestureRecognizer.Delegate = new SwipeGestureDelegate();
            NavigationItem.SetLeftBarButtonItem(_backButton, true);

            InputBoxTopRuler.BackgroundColor = Colors.BarShadowColor;
            InputBox.BackgroundColor = Colors.White;
        }

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IChatItem>(MessageTable)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is IChatMessage)
                    {
                        return ChatMessageCell.Identifier;
                    }

                    // If not a message has to be a label cell
                    return ChatLabelCell.Identifier;
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    if (item is IChatLabel)
                    {
                        return 20;
                    }

                    // 10 spacing
                    // Disp name height
                    // 5 spacing
                    // Message height
                    // 10 spacing

                    nfloat finalHeight = 25;

                    if (_textHeights.ContainsKey(item.Id))
                        finalHeight += _textHeights[item.Id];
                    else
                        finalHeight += CalculateTextHeight(item);

                    return finalHeight;
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    if (item is IChatLabel)
                    {
                        return 20;
                    }

                    // 10 spacing
                    // Disp name height
                    // 5 spacing
                    // Message height
                    // 10 spacing

                    nfloat finalHeight = 25;

                    if (_textHeights.ContainsKey(item.Id))
                        finalHeight += _textHeights[item.Id];
                    else
                        finalHeight += CalculateTextHeight(item);

                    return finalHeight;
                },
                UseAnimations = false,
            };

            MessageTable.RegisterClassForCellReuse(typeof(ChatLabelCell), ChatLabelCell.Identifier);

            MessageTable.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ChatConversationView, ChatConversationViewModel>();
            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            set.Bind(_source).To(vm => vm.Messages);
            set.Bind(ChatInput).To(vm => vm.Input);
            set.Bind(Send).To(vm => vm.SendCommand);
            set.Bind(_avatar.Tap()).For(tap => tap.Command).To(vm => vm.UserCommand);
            set.Apply();

            MessageTable.ReloadData();

            ScrollToBottom(false);
        }

        private void SetupGestures()
        {
            var swipeGesture = new UISwipeGestureRecognizer(OnSwipeDetected)
            {
                Direction = UISwipeGestureRecognizerDirection.Down,
            };

            ChatInput.AddGestureRecognizer(swipeGesture);
            ChatInput.ShouldReturn = TextFieldShouldReturn;
        }

        #endregion Setup

        #region TextHeight

        private readonly Dictionary<Guid, nfloat> _textHeights = new Dictionary<Guid, nfloat>();

        private nfloat CalculateTextHeight(IChatItem item)
        {
            var chatMessage = item as IChatMessage;

            if (chatMessage != null)
            {
                var displayNameHeight = chatMessage.DisplayName?.StringSize(Fonts.LatoBoldWithSize(14), MessageTable.Frame.Size).Height ?? 0;

                var messageHeight = chatMessage.Text?.StringSize(Fonts.LatoWithSize(15),
                                        new CGSize(MessageTable.Frame.Width - 60, double.MaxValue)).Height ?? 0;

                var finalHeight = messageHeight + displayNameHeight;

                _textHeights[item.Id] = finalHeight;

                return finalHeight;
            }

            return 0;
        }

        #endregion TextHeight

        #region Handlers

        private void AddHandlers()
        {
            // Handlers
            _addMessageSub =
                Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<ChatConversationAddMessage>(message =>
                {
                    ScrollToBottom(message.Animated);
                });

            _updateConversationSub =
                Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<ChatConversationUpdated>(message =>
                {
                    ScrollToBottom(false);
                });

            _didAppearNotification = UIApplication.Notifications.ObserveDidBecomeActive(async (sender, args) =>
            {
                await ViewModel.UpdateConversation();
            });

            _textFieldChangedNotification =
                NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification,
                    TextFieldTextChanged);

            ChatInput.ShouldReturn = TextFieldShouldReturn;
        }

        private void RemoveHandlers()
        {
            _addMessageSub.Dispose();
            _addMessageSub = null;

            _updateConversationSub.Dispose();
            _updateConversationSub = null;

            _didAppearNotification.Dispose();
            _didAppearNotification = null;

            _textFieldChangedNotification.Dispose();
            _textFieldChangedNotification = null;

            ChatInput.ShouldReturn = null;
        }

        #endregion Handlers

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
                    SendRightConstraint.Constant = _originalCommentButtonConstraintValue + Send.Frame.Width + 10;
                    InputRightConstraint.Constant = _originalInputConstraintValue + Send.Frame.Width + 10;
                }
                else
                {
                    SendRightConstraint.Constant = _originalCommentButtonConstraintValue;
                    InputRightConstraint.Constant = _originalInputConstraintValue;
                }

                View.LayoutIfNeeded();
            });
        }

        private void InputTouchUpInside(object sender, EventArgs args)
        {
            var notification = NSNotification.FromName(UITextField.TextFieldTextDidChangeNotification, ChatInput);

            TextFieldTextChanged(notification);
            //ChatInput.Text = string.Empty; // this will not generate change text event
            //ScrollToBottom(true);
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

            var contentOffset = MessageTable.ContentOffset;

            var keyboardHeightChange = visible ? (nfloat)Math.Abs(keyboardHeight - _currentKeyboardHeight) : (nfloat)Math.Abs(_currentKeyboardHeight - 0);

            if (visible)
            {
                InputBoxBottomConstraint.Constant = keyboardHeight + _originalInputBoxConstraint;

                //var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);

                //MessageTable.ContentInset = contentInsets;
                //MessageTable.ScrollIndicatorInsets = contentInsets;

                contentOffset.Y += keyboardHeightChange;
            }
            else
            {
                InputBoxBottomConstraint.Constant = _originalInputBoxConstraint;

                //MessageTable.ContentInset = UIEdgeInsets.Zero;
                //MessageTable.ScrollIndicatorInsets = UIEdgeInsets.Zero;

                contentOffset.Y -= keyboardHeightChange;
            }

            _currentKeyboardHeight = visible ? keyboardHeight : 0;

            //if (MessageTable.ContentOffset.Y >= MessageTable.ContentSize.Height - MessageTable.Frame.Size.Height)
            //{
            //user has scrolled to the bottom
            //    ScrollToBottom(false);
            //}
            //else
            //{
            MessageTable.SetContentOffset(contentOffset, false);
            //}

            View.LayoutIfNeeded();
        }

        #endregion Keyboard

        #region Helpers

        private void ScrollToBottom(bool animated)
        {
            MessageTable.ScrollToBottom(animated);
        }

        private void OnSwipeDetected()
        {
            ChatInput.ResignFirstResponder();
        }

        #endregion Helpers

        #region TabBar

        protected override NSLayoutConstraint TabBarTopConstraint { get { return InputBoxBottomConstraint; } }

        protected override bool SupportsHideTabBar { get { return true; } }

        #endregion TabBar
    }
}

