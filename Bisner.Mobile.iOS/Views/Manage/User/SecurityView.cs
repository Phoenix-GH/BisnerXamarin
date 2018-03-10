using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.User
{
    partial class SecurityView : KeyboardListenerViewBase<SecurityViewModel>
    {
        #region Constructor

        // Navigation items
        private UIBarButtonItem _updateItem, _updateIndicatorItem;
        private UIActivityIndicatorView _updateIndicator;

        // Scrollview and containers
        private UIView _containerView, _passwordContainer, _passwordTopRuler, _passwordBottomRuler;

        // Password
        private UILabel _passwordHeader, _oldPasswordHeader, _newPasswordHeader, _confirmPasswordHeader;
        private InputTextField _oldPassword, _newPassword, _confirmPassword;

        private NSObject _textFieldChangedNotification;

        public SecurityView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("SECURITYVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupUpdateItem();
            SetupViews();
            SetupConstraints();
            SetupBindings();

            // Set the navigation bar hidden so we don't get a double nav bar
            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = false;
            }

            // Set screen name for analytics
            ScreenName = "SecurityView";
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (IsMovingToParentViewController)
            {
                _textFieldChangedNotification =
                    NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification,
                        TextFieldTextChanged);

                ViewModel.AfterChangedAction = AfterChangedAction;

                ViewModel.StartUpdating += OnStartUpdating;
                ViewModel.StopUpdating += OnStopUpdating;
            }
        }

        private void AfterChangedAction()
        {
            NavigationController.PopViewController(true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                _textFieldChangedNotification.Dispose();

                ViewModel.StartUpdating -= OnStartUpdating;
                ViewModel.StopUpdating -= OnStopUpdating;
            }
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

        #endregion ViewController

        #region Setup

        private void SetupUpdateItem()
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

            // Header
            _passwordHeader = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Security & Privacy" };

            // About you
            _passwordContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _passwordTopRuler = new UIView { BackgroundColor = Appearance.Colors.BackPanelBorderTop };

            _oldPasswordHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Old password" };
            _oldPassword = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), SecureTextEntry = true, ShouldReturn = ShouldReturn };
            _oldPassword.Layer.BorderColor = Appearance.Colors.BarShadowColor.CGColor;

            _newPasswordHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "New password" };
            _newPassword = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), SecureTextEntry = true, ShouldReturn = ShouldReturn };
            _newPassword.Layer.BorderColor = Appearance.Colors.BarShadowColor.CGColor;

            _confirmPasswordHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Confirm password" };
            _confirmPassword = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), SecureTextEntry = true, ShouldReturn = ShouldReturn };
            _confirmPassword.Layer.BorderColor = Appearance.Colors.BarShadowColor.CGColor;

            _passwordBottomRuler = new UIView { BackgroundColor = Appearance.Colors.BackPanelBorderBottom };

            _containerView.AddSubviews(_passwordContainer, _passwordTopRuler, _passwordHeader, _oldPasswordHeader, _oldPassword, _newPasswordHeader, _newPassword, _confirmPasswordHeader, _confirmPassword, _passwordBottomRuler);

            // Add container to scrollview and scrollview to view
            ScrollView.AddSubviews(_containerView);
        }

        private bool ShouldReturn(UITextField textfield)
        {
            textfield.ResignFirstResponder();

            return true;
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
                _containerView.AtRightOf(ScrollView)
            );

            _containerView.AddConstraints(

                _passwordHeader.AtTopOf(_containerView, 8),
                _passwordHeader.AtLeftOf(_containerView, 14),
                _passwordHeader.AtRightOf(_containerView, 14),

                _passwordContainer.Below(_passwordHeader, 8),
                _passwordContainer.AtLeftOf(_containerView),
                _passwordContainer.AtRightOf(_containerView),

                _passwordTopRuler.AtTopOf(_passwordContainer),
                _passwordTopRuler.AtLeftOf(_passwordContainer),
                _passwordTopRuler.AtRightOf(_passwordContainer),
                _passwordTopRuler.Height().EqualTo(1),

                _oldPasswordHeader.Below(_passwordTopRuler, 18),
                _oldPasswordHeader.AtLeftOf(_passwordContainer, 14),
                _oldPasswordHeader.AtRightOf(_passwordContainer, 14),

                _oldPassword.Below(_oldPasswordHeader, 6),
                _oldPassword.AtLeftOf(_passwordContainer, 14),
                _oldPassword.AtRightOf(_passwordContainer, 14),
                _oldPassword.Height().EqualTo(40),

                _newPasswordHeader.Below(_oldPassword, 18),
                _newPasswordHeader.AtLeftOf(_passwordContainer, 14),
                _newPasswordHeader.AtRightOf(_passwordContainer, 14),

                _newPassword.Below(_newPasswordHeader, 6),
                _newPassword.AtLeftOf(_passwordContainer, 14),
                _newPassword.AtRightOf(_passwordContainer, 14),
                _newPassword.Height().EqualTo(40),

                _confirmPasswordHeader.Below(_newPassword, 18),
                _confirmPasswordHeader.AtLeftOf(_passwordContainer, 14),
                _confirmPasswordHeader.AtRightOf(_passwordContainer, 14),

                _confirmPassword.Below(_confirmPasswordHeader, 6),
                _confirmPassword.AtLeftOf(_passwordContainer, 14),
                _confirmPassword.AtRightOf(_passwordContainer, 14),
                _confirmPassword.Height().EqualTo(40),

                _passwordBottomRuler.Below(_confirmPassword, 18),
                _passwordBottomRuler.AtBottomOf(_passwordContainer),
                _passwordBottomRuler.AtLeftOf(_passwordContainer),
                _passwordBottomRuler.AtRightOf(_passwordContainer),
                _passwordBottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<SecurityView, SecurityViewModel>();

            set.Bind(_passwordHeader).To(vm => vm.PasswordHeaderText);
            set.Bind(_updateItem).For("Title").To(vm => vm.UpdateText);
            set.Bind(_updateItem).To(vm => vm.UpdateCommand);
            set.Bind(_oldPasswordHeader).To(vm => vm.OldPasswordText);
            set.Bind(_oldPassword).To(vm => vm.OldPassword);
            set.Bind(_oldPassword).For("ErrorColor").To(vm => vm.OldPasswordEmpty);
            set.Bind(_newPasswordHeader).To(vm => vm.NewPasswordText);
            set.Bind(_newPassword).To(vm => vm.NewPassword);
            set.Bind(_newPassword).For("ErrorColor").To(vm => vm.PasswordsDontMatch);
            set.Bind(_confirmPasswordHeader).To(vm => vm.ConfirmPasswordText);
            set.Bind(_confirmPassword).To(vm => vm.ConfirmPassword);
            set.Bind(_confirmPassword).For("ErrorColor").To(vm => vm.PasswordsDontMatch);
            set.Apply();
        }

        #endregion Setup

        #region Keyboard

        public override bool HandlesKeyboardNotifications => true;
        
        private nfloat _keyboardHeight;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            View.SetNeedsUpdateConstraints();

            _keyboardHeight = keyboardHeight;



            View.LayoutIfNeeded();
        }

        #endregion Keyboard

        #region TextField

        private void TextFieldTextChanged(NSNotification notification)
        {
            var field = (UITextField)notification.Object;

            ScrollView.CenterViewInScroll(field, _keyboardHeight);
        }

        #endregion TextField

        #region Base modifications

        protected override bool EnableTitleBarLogo { get { return true; } }

        protected override bool EnableCustomBackButton { get { return true; } }

        #endregion Base Modifications
    }
}
