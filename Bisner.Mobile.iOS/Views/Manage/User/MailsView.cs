using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.User
{
    partial class MailsView : KeyboardListenerViewBase<MailsViewModel>
    {
        #region Constructor

        // Navigation items
        private UIBarButtonItem _updateItem, _updateIndicatorItem;
        private UIActivityIndicatorView _updateIndicator;

        // Scrollview and containers
        private UIView _containerView, _settingsContainer, _settingsTopRuler, _settingsBottomRuler;

        // Settings
        private UIView _dailyDigestRuler, _weeklyDigestRuler, _contactInvitesRuler, _mentionsRuler, _workspaceRuler, _assignRuler;
        private UILabel _settingsHeader, _disableAllMailsText, _dailyDigestDescription, _disableDailyDigestText, _weeklyDigestDescription, _disableNewFeedPostText, _contactInvitesDescription, _disableNewFeedPostCommentText, _mentionsDescription, _workspaceText, _workspaceDescription, _assignText, _assignDescription;
        private UIButton _disableAllMails, _disableDailyDigest, _disableNewFeedPost, _disableNewFeedPostComment, _workspace, _assign;

        public MailsView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("MAILSVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupUpdateButton();
            SetupViews();
            SetupConstraints();
            SetupBindings();

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            _updateItem.Enabled = false;

            // Set screen name for analytics
            ScreenName = "MailsView";
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ViewModel.EnableUpdate += OnEnableUpdate;
            ViewModel.StartUpdating += OnStartUpdating;
            ViewModel.StopUpdating += OnStopUpdating;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ViewModel.EnableUpdate -= OnEnableUpdate;
            ViewModel.StartUpdating -= OnStartUpdating;
            ViewModel.StopUpdating -= OnStopUpdating;
        }



        private void OnStartUpdating()
        {
            InvokeOnMainThread(() =>
            {
                NavigationItem.SetHidesBackButton(true, true);
                NavigationItem.SetRightBarButtonItems(new[] { _updateIndicatorItem }, true);
            });
        }

        private void OnStopUpdating()
        {
            InvokeOnMainThread(() =>
            {
                NavigationItem.SetHidesBackButton(false, true);
                NavigationItem.SetRightBarButtonItems(new[] { _updateItem }, true);
            });
        }

        private void OnEnableUpdate(bool enabled)
        {
            InvokeOnMainThread(() =>
            {
                _updateItem.Enabled = enabled;
            });
        }

        #endregion ViewController

        #region Setup

        private void SetupUpdateButton()
        {
            // Add Post button
            _updateItem = new UIBarButtonItem
            {
                Title = "Update",
            };

            var icoFontAttribute = new UITextAttributes { Font = Appearance.Fonts.LatoBoldWithSize(24), TextColor = Appearance.Colors.BisnerBlue };
            _updateItem.SetTitleTextAttributes(icoFontAttribute, UIControlState.Application);
            _updateItem.Style = UIBarButtonItemStyle.Done;

            // Post indicator
            _updateIndicator = new UIActivityIndicatorView { Color = Appearance.Colors.BisnerBlue };
            _updateIndicatorItem = new UIBarButtonItem(_updateIndicator);
            _updateIndicator.StartAnimating();

            NavigationItem.SetRightBarButtonItems(new[] { _updateItem }, true);
        }

        private void SetupViews()
        {
            View.BackgroundColor = Appearance.Colors.BackgroundColor;

            _containerView = new UIView { BackgroundColor = Appearance.Colors.BackgroundColor };

            _settingsHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Mails & Notifications" };

            // Notification settings
            _settingsContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _settingsTopRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            // Settings
            _disableAllMailsText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _dailyDigestDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a daily digest", Lines = 0 };
            _disableAllMails = new UIButton();
            _disableAllMails.Layer.CornerRadius = 5.0f;
            _dailyDigestRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _disableDailyDigestText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _weeklyDigestDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a weekly digest", Lines = 0 };
            _disableDailyDigest = new UIButton();
            _disableDailyDigest.Layer.CornerRadius = 5.0f;
            _weeklyDigestRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _disableNewFeedPostText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _contactInvitesDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a mail if someone wants to become a contact", Lines = 0 };
            _disableNewFeedPost = new UIButton();
            _disableNewFeedPost.Layer.CornerRadius = 5.0f;
            _contactInvitesRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _disableNewFeedPostCommentText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _mentionsDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a mail if someone mentions you", Lines = 0 };
            _disableNewFeedPostComment = new UIButton();
            _disableNewFeedPostComment.Layer.CornerRadius = 5.0f;
            _mentionsRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _workspaceText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _workspaceDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a mail if someone infites you to a workspace", Lines = 0 };
            _workspace = new UIButton();
            _workspace.Layer.CornerRadius = 5.0f;
            _workspaceRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _assignText = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };
            _assignDescription = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Recieve a mail if someone assigns a document to you", Lines = 0 };
            _assign = new UIButton();
            _assign.Layer.CornerRadius = 5.0f;
            _assignRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _settingsBottomRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _containerView.AddSubviews(_settingsContainer, _settingsTopRuler, _settingsHeader, _dailyDigestDescription, _disableAllMailsText, _disableAllMails, _disableDailyDigestText, _weeklyDigestDescription, _disableDailyDigest, _disableNewFeedPostText, _contactInvitesDescription, _disableNewFeedPost, _disableNewFeedPostCommentText, _mentionsDescription, _disableNewFeedPostComment, _workspaceText, _workspaceDescription, _workspace, _assignText, _assignDescription, _assign, _settingsBottomRuler, _dailyDigestRuler, _weeklyDigestRuler, _contactInvitesRuler, _mentionsRuler, _workspaceRuler, _assignRuler);

            // Add container to scrollview and scrollview to view
            ScrollView.AddSubviews(_containerView);
        }

        private void SetupConstraints()
        {
            ScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _containerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _containerView.WithSameWidth(View),
                _containerView.Height().EqualTo(UIScreen.MainScreen.Bounds.Height)
            );

            ScrollView.AddConstraints(
                _containerView.AtTopOf(ScrollView),
                _containerView.AtLeftOf(ScrollView),
                _containerView.AtRightOf(ScrollView),
                _containerView.AtBottomOf(ScrollView)
            );

            _containerView.AddConstraints(
                _settingsHeader.AtTopOf(_containerView, 15),
                _settingsHeader.AtLeftOf(_containerView, 14),
                _settingsHeader.AtRightOf(_containerView, 14),

                _settingsContainer.Below(_settingsHeader, 5),
                _settingsContainer.AtLeftOf(_containerView),
                _settingsContainer.AtRightOf(_containerView),

                _settingsTopRuler.AtTopOf(_settingsContainer),
                _settingsTopRuler.AtLeftOf(_settingsContainer),
                _settingsTopRuler.AtRightOf(_settingsContainer),
                _settingsTopRuler.Height().EqualTo(1),

                _disableAllMailsText.Below(_settingsTopRuler, 15),
                _disableAllMailsText.AtLeftOf(_containerView, 14),

                _dailyDigestDescription.Below(_disableAllMailsText),
                _dailyDigestDescription.AtLeftOf(_containerView, 14),
                _dailyDigestDescription.ToLeftOf(_disableAllMails, 5),

                _disableAllMails.AtRightOf(_containerView, 14),
                _disableAllMails.WithSameTop(_disableAllMailsText),
                _disableAllMails.Height().EqualTo(30),
                _disableAllMails.Width().EqualTo(30),

                _dailyDigestRuler.Below(_dailyDigestDescription, 15),
                _dailyDigestRuler.AtLeftOf(_containerView, 14),
                _dailyDigestRuler.AtRightOf(_containerView, 14),
                _dailyDigestRuler.Height().EqualTo(1),

                _disableDailyDigestText.Below(_dailyDigestRuler, 15),
                _disableDailyDigestText.AtLeftOf(_containerView, 14),

                _weeklyDigestDescription.Below(_disableDailyDigestText),
                _weeklyDigestDescription.AtLeftOf(_containerView, 14),
                _weeklyDigestDescription.ToLeftOf(_disableDailyDigest, 5),

                _disableDailyDigest.AtRightOf(_containerView, 14),
                _disableDailyDigest.WithSameTop(_disableDailyDigestText),
                _disableDailyDigest.Height().EqualTo(30),
                _disableDailyDigest.Width().EqualTo(30),

                _weeklyDigestRuler.Below(_weeklyDigestDescription, 15),
                _weeklyDigestRuler.AtLeftOf(_containerView, 14),
                _weeklyDigestRuler.AtRightOf(_containerView, 14),
                _weeklyDigestRuler.Height().EqualTo(1),

                _disableNewFeedPostText.Below(_weeklyDigestRuler, 15),
                _disableNewFeedPostText.AtLeftOf(_containerView, 14),
                _disableNewFeedPostText.ToRightOf(_disableAllMails, 5),

                _contactInvitesDescription.Below(_disableNewFeedPostText),
                _contactInvitesDescription.AtLeftOf(_containerView, 14),
                _contactInvitesDescription.ToLeftOf(_disableNewFeedPost, 5),

                _disableNewFeedPost.AtRightOf(_containerView, 14),
                _disableNewFeedPost.WithSameTop(_disableNewFeedPostText),
                _disableNewFeedPost.Height().EqualTo(30),
                _disableNewFeedPost.Width().EqualTo(30),

                _contactInvitesRuler.Below(_contactInvitesDescription, 15),
                _contactInvitesRuler.AtLeftOf(_containerView, 14),
                _contactInvitesRuler.AtRightOf(_containerView, 14),
                _contactInvitesRuler.Height().EqualTo(1),

                _disableNewFeedPostCommentText.Below(_contactInvitesRuler, 15),
                _disableNewFeedPostCommentText.AtLeftOf(_containerView, 14),
                _disableNewFeedPostCommentText.ToRightOf(_disableAllMails, 5),

                _mentionsDescription.Below(_disableNewFeedPostCommentText),
                _mentionsDescription.AtLeftOf(_containerView, 14),
                _mentionsDescription.ToLeftOf(_disableNewFeedPostComment, 5),

                _disableNewFeedPostComment.AtRightOf(_containerView, 14),
                _disableNewFeedPostComment.WithSameTop(_disableNewFeedPostCommentText),
                _disableNewFeedPostComment.Height().EqualTo(30),
                _disableNewFeedPostComment.Width().EqualTo(30),

                _mentionsRuler.Below(_mentionsDescription, 15),
                _mentionsRuler.AtLeftOf(_containerView, 14),
                _mentionsRuler.AtRightOf(_containerView, 14),
                _mentionsRuler.Height().EqualTo(1),

                _workspaceText.Below(_mentionsRuler, 15),
                _workspaceText.AtLeftOf(_containerView, 14),
                _workspaceText.ToRightOf(_disableAllMails, 5),

                _workspaceDescription.Below(_workspaceText),
                _workspaceDescription.AtLeftOf(_containerView, 14),
                _workspaceDescription.ToLeftOf(_workspace, 5),

                _workspace.AtRightOf(_containerView, 14),
                _workspace.WithSameTop(_workspaceText),
                _workspace.Height().EqualTo(30),
                _workspace.Width().EqualTo(30),

                _workspaceRuler.Below(_workspaceDescription, 15),
                _workspaceRuler.AtLeftOf(_containerView, 14),
                _workspaceRuler.AtRightOf(_containerView, 14),
                _workspaceRuler.Height().EqualTo(1),

                _assignText.Below(_workspaceRuler, 15),
                _assignText.AtLeftOf(_containerView, 14),

                _assignDescription.Below(_assignText),
                _assignDescription.AtLeftOf(_containerView, 14),
                _assignDescription.ToLeftOf(_assign, 5),

                _assign.AtRightOf(_containerView, 14),
                _assign.WithSameTop(_assignText),
                _assign.Height().EqualTo(30),
                _assign.Width().EqualTo(30),

                _assignRuler.Below(_assignDescription, 15),
                _assignRuler.AtLeftOf(_containerView, 14),
                _assignRuler.AtRightOf(_containerView, 14),
                _assignRuler.Height().EqualTo(1),

                _settingsBottomRuler.Below(_assignRuler, 15),
                _settingsBottomRuler.AtBottomOf(_settingsContainer),
                _settingsBottomRuler.AtLeftOf(_settingsContainer),
                _settingsBottomRuler.AtRightOf(_settingsContainer),
                _settingsBottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<MailsView, MailsViewModel>();
            set.Bind(_settingsHeader).To(vm => vm.HeaderText);

            set.Bind(_disableAllMails).For("GreenCheckBox").To(vm => vm.DisableAllMails);
            set.Bind(_disableAllMailsText).To(vm => vm.DisableAllMailsText);

            set.Bind(_disableDailyDigest).For("GreenCheckBox").To(vm => vm.DisableDailyDigest);
            set.Bind(_disableDailyDigestText).To(vm => vm.DisableDailyDigestText);

            set.Bind(_disableNewFeedPost).For("GreenCheckBox").To(vm => vm.DisableNewFeedPost);
            set.Bind(_disableNewFeedPostText).To(vm => vm.DisableNewFeedPostText);

            set.Bind(_disableNewFeedPostComment).For("GreenCheckBox").To(vm => vm.DisableNewFeedPostComment);
            set.Bind(_disableNewFeedPostCommentText).To(vm => vm.DisableNewFeedPostCommentText);

            set.Bind(_workspace).For("GreenCheckBox").To(vm => vm.DisableMention);
            set.Bind(_workspaceText).To(vm => vm.DisableMentionText);

            set.Bind(_assign).For("GreenCheckBox").To(vm => vm.DisableAssigned);
            set.Bind(_assignText).To(vm => vm.DisableAssignedText);

            set.Bind(_updateItem).To(vm => vm.UpdateCommand);

            set.Apply();
        }

        #endregion Setup

        #region Base modifications

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => true;

        #endregion Base modifications
    }
}
