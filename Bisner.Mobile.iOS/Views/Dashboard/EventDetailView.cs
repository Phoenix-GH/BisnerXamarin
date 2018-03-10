using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Base;
using Bisner.Mobile.Core.Models.Events;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Bisner.Mobile.iOS.Views.Dashboard.Cells;
using Bisner.Mobile.iOS.Views.Feed.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard
{
    public partial class EventDetailView : HideTabBarViewBase<EventViewModel>
    {
        #region Constructor

        private GenericTableViewSource<IItemBase> _source;

        private NSObject _textFieldChangedNotification;

        public EventDetailView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupTable();
            SetupBindings();
            SetupSwipeGesture();

            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            SendRightConstraint.Constant -= Send.Frame.Width + 10;

            _originalSendRightValue = SendRightConstraint.Constant;
            _originalInpuRightValue = InputRightConstraint.Constant;

            _originalInputBoxConstraint = InputContainerBottomConstraint.Constant - TabBarController.TabBar.Frame.Height;

            InputBoxTopBorder.BackgroundColor = Appearance.Colors.RulerColor;

            if (Settings.UserRoles.All(r => r != ApiModels.Security.Roles.Event.EventComment.ToLower()))
            {
                InputBoxHeightConstraint.Constant = 0;
            }

            // Set screen name for analytics
            ScreenName = "EventView id = " + (ViewModel.Event?.Id.ToString() ?? Guid.Empty.ToString());
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _textFieldChangedNotification = NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, TextFieldTextChanged);

            ViewModel.Commented = () => { TableView.ScrollToBottom(true); };

            Input.ShouldReturn = ShouldReturn;
            Send.TouchUpInside += SendOnTouchUpInside;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _textFieldChangedNotification.Dispose();

            ViewModel.Commented = null;

            Input.ShouldReturn = null;
            Send.TouchUpInside -= SendOnTouchUpInside;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("EVENTVIEW RECIEVED MEMORY WARNING!!!");
        }

        #endregion ViewController

        #region Setup

        private void SetupTable()
        {
            _source = new GenericTableviewSourceWithHeight<IItemBase>(TableView)
            {
                GetIdentifierFunc = (item, path) =>
                {
                    if (item is IEvent)
                    {
                        return EventDetailCell.Identifier;
                    }

                    return CommentCell.Identifier;
                },
                ModifyCellFunc = (cell, path, item) =>
                {
                    ModifyCommentCell(cell, item, path);

                    ModifyEventDetailCell(cell, item);
                },
                GetCellHeightFunc = (view, path, item) =>
                {
                    if (item is IComment)
                    {
                        return GetCommentCellHeight(item as IComment);
                    }

                    return GetEventCellHeight(item as IEvent);
                },
                GetEstimatedHeightFunc = (view, path, item) =>
                {
                    if (item is IComment)
                    {
                        return GetCommentCellHeight(item as IComment);
                    }

                    return GetEventCellHeight(item as IEvent);
                }
            };

            TableView.BackgroundColor = Appearance.Colors.BackgroundColor;
            TableView.AllowsSelection = false;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            TableView.RegisterClassForCellReuse(typeof(CommentCell), CommentCell.Identifier);
            TableView.RegisterClassForCellReuse(typeof(EventDetailCell), EventDetailCell.Identifier);

            TableView.Source = _source;
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<EventDetailView, EventViewModel>();
            set.Bind(_source).To(vm => vm.Items);
            set.Bind(Input).To(vm => vm.Input);
            set.Bind(Send).To(vm => vm.CommentCommand);
            set.Bind(Send).For("Title").To(vm => vm.CommentButtonText);
            set.Bind(this).For(v => v.Title).To(vm => vm.Event.Title);
            set.Apply();
        }

        private void SetupSwipeGesture()
        {
            var swipeGesture = new UISwipeGestureRecognizer(OnSwipeDetected)
            {
                Direction = UISwipeGestureRecognizerDirection.Down,
            };

            Input.AddGestureRecognizer(swipeGesture);
        }

        #endregion Setup

        #region Keyboard

        public override bool HandlesKeyboardNotifications => true;

        private nfloat _originalInputBoxConstraint;
        private nfloat _currentKeyboardHeight;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            View.SetNeedsUpdateConstraints();

            var contentOffset = TableView.ContentOffset;

            var keyboardHeightChange = visible ? (nfloat)Math.Abs(keyboardHeight - _currentKeyboardHeight) : (nfloat)Math.Abs(_currentKeyboardHeight - 0);

            if (visible)
            {
                InputContainerBottomConstraint.Constant = keyboardHeight + _originalInputBoxConstraint;

                contentOffset.Y += keyboardHeightChange;
            }
            else
            {
                InputContainerBottomConstraint.Constant = _originalInputBoxConstraint;

                contentOffset.Y -= keyboardHeightChange;
            }

            _currentKeyboardHeight = visible ? keyboardHeight : 0;

            TableView.SetContentOffset(contentOffset, false);

            View.LayoutIfNeeded();
        }

        #endregion Keyboard

        #region TextBox

        private nfloat _originalSendRightValue, _originalInpuRightValue;

        private void TextFieldTextChanged(NSNotification notification)
        {
            var field = (UITextField)notification.Object;

            UIView.Animate(0.1f, () =>
            {
                View.SetNeedsUpdateConstraints();

                if (field.Text.Length > 0)
                {
                    SendRightConstraint.Constant = _originalSendRightValue + Send.Frame.Width + 10;
                    InputRightConstraint.Constant = _originalInpuRightValue + Send.Frame.Width + 10;
                }
                else
                {
                    SendRightConstraint.Constant = _originalSendRightValue;
                    InputRightConstraint.Constant = _originalInpuRightValue;
                }

                View.LayoutIfNeeded();
            });
        }

        #endregion TextBox

        #region Helpers

        private void OnSwipeDetected()
        {
            TableView.ResignFirstResponder();
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
            //ScrollToBottom(true);
        }

        private void BackButtonOnClicked(object sender, EventArgs eventArgs)
        {
            NavigationController.PopViewController(true);
        }

        #endregion Helpers

        #region Base modifications

        protected override NSLayoutConstraint TabBarTopConstraint { get { return InputContainerBottomConstraint; } }

        protected override bool SupportsHideTabBar { get { return true; } }

        protected override bool EnableCustomBackButton { get { return true; } }

        #endregion Base modifications

        #region CommentCell

        private readonly Dictionary<Guid, NSAttributedString> _htmlTexts = new Dictionary<Guid, NSAttributedString>();
        private readonly Dictionary<Guid, nfloat> _textHeights = new Dictionary<Guid, nfloat>();

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
                SetCommentCellText(item as IComment, commentCell);

                // Last cell has different ruler
                if (path.Section == TableView.IndexPathForLastRow.Section && path.Row == TableView.IndexPathForLastRow.Row)
                {
                    commentCell.SetRulerFullWidth(true);
                }
                else
                {
                    commentCell.SetRulerFullWidth(false);
                }
            }
        }

        private void SetCommentCellText(IComment comment, CommentCell commentCell)
        {
            if (!string.IsNullOrWhiteSpace(comment.Text))
            {
                var text = GetText(comment.Id, comment.Text);

                commentCell.Text.AttributedText = text;

                var textSize = commentCell.Text.SizeThatFits(new CGSize(commentCell.Text.Frame.Width, double.MaxValue));

                _textHeights[comment.Id] = textSize.Height;
            }
        }

        private nfloat GetCommentCellHeight(IComment comment)
        {
            nfloat finalHeight = 40;

            var textHeight = _textHeights.ContainsKey(comment.Id) ? _textHeights[comment.Id] : 0;

            return finalHeight + textHeight;
        }

        #endregion CommentCell

        #region EventDetailCell

        private void ModifyEventDetailCell(UITableViewCell cell, IItemBase item)
        {
            var eventDetailCell = cell as EventDetailCell;

            if (eventDetailCell != null)
            {
                var @event = item as IEvent;

                if (@event != null)
                {
                    SetEventCellAboutText(@event, eventDetailCell);

                    eventDetailCell.SetImagesVisible(@event.Images.Any());

                    eventDetailCell.SetAttendeesVisible(@event.Attendees.Any());
                }
            }
        }

        private void SetEventCellAboutText(IEvent @event, EventDetailCell cell)
        {
            if (!string.IsNullOrEmpty(@event.Summary))
            {
                var text = GetText(@event.Id, @event.Summary);

                cell.AboutText.AttributedText = text;

                nfloat textSize = 0;

                if (text != null)
                {
                    textSize = cell.AboutText.SizeThatFits(new CGSize(cell.AboutText.Frame.Width, double.MaxValue)).Height;
                }

                nfloat eventLocationHeight = 0;
                if (@event.Location != null)
                {
                    eventLocationHeight =
                        @event.Location.StringSize(Appearance.Fonts.LatoWithSize(15),
                            new CGSize(cell.EventLocation.Frame.Width, double.MaxValue)).Height;
                }

                _eventDetailHeight = eventLocationHeight + textSize;
            }
        }

        private nfloat _eventDetailHeight = 0;

        private nfloat GetEventCellHeight(IEvent @event)
        {
            // Header                           345
            // Attending                        125 or 0
            // Ruler                            1
            // Spacing                          13
            // Event header                     20
            // Spacing                          14
            // Event date                       20
            // Spacing                          7
            // Event time                       20
            // Spacing                          7
            // Event loc                        ?
            // Spacing                          30
            // Images                           139 or 0
            // Spacing                          5
            // Spacing                          14
            // About header                     20
            // Spacing                          13
            // About text                       ?
            // Spacing                          10

            // Staat uit
            //----------------------------------------
            // Comment button                   15

            var finalHeight = 538;

            if (@event.Images.Any())
            {
                finalHeight += 139;
            }

            if (@event.Attendees.Any())
            {
                finalHeight += 125;
            }

            return finalHeight + _eventDetailHeight;
        }

        #endregion EventDetailCell

        #region AttributedText

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

        #endregion AttributedText
    }
}