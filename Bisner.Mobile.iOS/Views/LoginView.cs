using System;
using System.Diagnostics;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    partial class LoginView : KeyboardListenerViewBase<LoginViewModel>
    {
        #region Constructor

        private UIView _containerView, _emailRuler, _passwordRuler;
        private UIImageView _background, _logo, _passwordImage, _emailImage;
        private UITextField _email, _password;
        private UIButton _signinButton;
        private UIButton _registerButton, _forgotPasswordButton;
        private UIActivityIndicatorView _indicator;

        public LoginView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("LOGINVIEW RECIEVED MEMORY WARNING!!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Login view does not need a navigation bar
            NavigationController.NavigationBarHidden = true;

            if (Settings.CustomLogin)
            {
                SetupSubViews2();
                SetupConstraints2();
            }
            else
            {
                SetupSubViews();
                SetupConstraints();
            }
            SetupBindings();

            ViewToCenterOnKeyboardShown = _signinButton;
            PositionToScrollView = UITableViewScrollPosition.Bottom;
            ScrollView.ScrollEnabled = false;

            // Set screen name for analytics
            ScreenName = "LoginView";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _signinButton.TouchUpInside += ResignFirstResponder;
            _email.ShouldReturn = ShouldReturn;
            _password.ShouldReturn = ShouldReturn;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            _signinButton.TouchUpInside -= ResignFirstResponder;
            _email.ShouldReturn = null;
            _password.ShouldReturn = null;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            ResignFirstResponder();
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        #endregion ViewController

        #region Setup

        private void SetupSubViews()
        {
            using (var image = UIImage.FromBundle("Images/background_login.png"))
            {
                _background = new UIImageView(image) { ContentMode = UIViewContentMode.ScaleAspectFill };
            }

            _containerView = new UIView();

            using (var logoImage = UIImage.FromBundle("Images/bisner_logo_login.png"))
            {
                _logo = new UIImageView(logoImage);
            }

            _indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White) { Hidden = true };
            _indicator.StartAnimating();

            using (var emailImage = UIImage.FromBundle("Icons/icon_login_mail.png").ImageWithColor(Appearance.Colors.LoginRulerColor))
            {
                _emailImage = new UIImageView(emailImage);
            }

            using (var passwordImage = UIImage.FromBundle("Icons/icon_login_lock.png").ImageWithColor(Appearance.Colors.LoginRulerColor))
            {
                _passwordImage = new UIImageView(passwordImage);
            }

            _email = new TransparentUITextField { Font = UIFont.SystemFontOfSize(14), KeyboardType = UIKeyboardType.EmailAddress, TextColor = Appearance.Colors.LoginInputTextColor };
            ((TransparentUITextField)_email).SetupApearance();

            _password = new TransparentUITextField { Font = UIFont.SystemFontOfSize(14), SecureTextEntry = true, TextColor = Appearance.Colors.LoginInputTextColor };
            ((TransparentUITextField)_password).SetupApearance();

            _emailRuler = new UIView { BackgroundColor = Appearance.Colors.LoginRulerColor };
            _passwordRuler = new UIView { BackgroundColor = Appearance.Colors.LoginRulerColor };

            _signinButton = new LoginButton { Font = Appearance.Fonts.LatoBlackWithSize(15) };

            _registerButton = new UIButton { Font = UIFont.SystemFontOfSize(13) };
            _registerButton.SetTitle("No account yet?", UIControlState.Normal);
            _registerButton.SetTitleColor(Appearance.Colors.LoginBottomButtonTextColor, UIControlState.Normal);
            _registerButton.SetTitleColor(Appearance.Colors.White, UIControlState.Highlighted);

            _forgotPasswordButton = new UIButton { Font = UIFont.SystemFontOfSize(13) };
            _forgotPasswordButton.SetTitle("Forgot your password?", UIControlState.Normal);
            _forgotPasswordButton.SetTitleColor(Appearance.Colors.LoginBottomButtonTextColor, UIControlState.Normal);
            _forgotPasswordButton.SetTitleColor(Appearance.Colors.White, UIControlState.Highlighted);

            // TODO: Turned off because we don't ahve registration yet
            _registerButton.Hidden = true;

            View.AddSubviews(_background);
            View.SendSubviewToBack(_background);

            _containerView.AddSubviews(_logo, _indicator, _emailImage, _passwordImage, _email, _password, _emailRuler, _passwordRuler, _signinButton, _registerButton, _forgotPasswordButton);

            ScrollView.AddSubviews(_containerView);
        }

        private void SetupConstraints()
        {
            ScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _containerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _containerView.WithSameWidth(View),

                _background.AtTopOf(View),
                _background.AtLeftOf(View),
                _background.AtRightOf(View),
                _background.AtBottomOf(View)
            );

            ScrollView.AddConstraints(
                _containerView.AtTopOf(ScrollView),
                _containerView.AtLeftOf(ScrollView),
                _containerView.AtRightOf(ScrollView),
                _containerView.AtBottomOf(ScrollView),
                _containerView.Height().EqualTo(UIScreen.MainScreen.Bounds.Height)
            );

            _containerView.AddConstraints(
                _logo.AtTopOf(_containerView, 40),
                _logo.WithSameCenterX(_containerView),

                _indicator.Below(_logo, 30),
                _indicator.WithSameCenterX(_logo),

                _registerButton.AtBottomOf(_containerView, 40),
                _registerButton.AtLeftOf(_containerView, 35),

                _forgotPasswordButton.AtBottomOf(_containerView, 40),
                _forgotPasswordButton.AtRightOf(_containerView, 35),

                _signinButton.Above(_registerButton, 40),
                _signinButton.Height().EqualTo(40),
                _signinButton.WithSameLeft(_registerButton),
                _signinButton.WithSameRight(_forgotPasswordButton),

                _passwordRuler.Above(_signinButton, 30),
                _passwordRuler.WithSameLeft(_registerButton),
                _passwordRuler.WithSameRight(_forgotPasswordButton),
                _passwordRuler.Height().EqualTo(1),

                _password.Above(_passwordRuler, 15),
                _password.WithSameRight(_forgotPasswordButton),
                _password.Height().EqualTo(20),
                _password.ToRightOf(_passwordImage, 15),

                _passwordImage.WithSameCenterY(_password),
                _passwordImage.WithSameLeft(_passwordRuler),
                _passwordImage.Height().EqualTo(20),
                _passwordImage.Width().EqualTo(20),

                _emailRuler.Above(_password, 15),
                _emailRuler.WithSameLeft(_registerButton),
                _emailRuler.WithSameRight(_forgotPasswordButton),
                _emailRuler.Height().EqualTo(1),

                _email.Above(_emailRuler, 15),
                _email.WithSameRight(_forgotPasswordButton),
                _email.Height().EqualTo(20),
                _email.ToRightOf(_emailImage, 15),

                _emailImage.WithSameCenterY(_email),
                _emailImage.WithSameLeft(_emailRuler),
                _emailImage.Height().EqualTo(20),
                _emailImage.Width().EqualTo(20)
                );
        }

        #region CustomAscendas


        private void SetupSubViews2()
        {
            using (var image = UIImage.FromBundle("Images/background_login.png"))
            {
                _background = new UIImageView(image) { ContentMode = UIViewContentMode.ScaleAspectFill };
            }

            _containerView = new UIView();

            using (var logoImage = UIImage.FromBundle("Images/bisner_logo_login.png"))
            {
                _logo = new UIImageView(logoImage);
            }

            _indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White) { Hidden = true };
            _indicator.StartAnimating();

            using (var emailImage = UIImage.FromBundle("Icons/icon_login_mail.png").ImageWithColor(Appearance.Colors.LoginRulerColor))
            {
                _emailImage = new UIImageView(emailImage);
            }

            using (var passwordImage = UIImage.FromBundle("Icons/icon_login_lock.png").ImageWithColor(Appearance.Colors.LoginRulerColor))
            {
                _passwordImage = new UIImageView(passwordImage);
            }

            _email = new UITextField() { Font = UIFont.SystemFontOfSize(14), KeyboardType = UIKeyboardType.EmailAddress, TextColor = Appearance.Colors.LoginInputTextColor };
            _email.Layer.BorderWidth = 1;
            _email.Layer.BorderColor = Appearance.Colors.White.ColorWithAlpha(0.6f).CGColor;
            _email.Layer.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f).CGColor;
            _email.LeftViewMode = UITextFieldViewMode.Always;
            _email.LeftView = new UIView(new CGRect(0, 0, 15, 15));

            _password = new UITextField { Font = UIFont.SystemFontOfSize(14), SecureTextEntry = true, TextColor = Appearance.Colors.LoginInputTextColor };
            _password.Layer.BorderWidth = 1;
            _password.Layer.BorderColor = Appearance.Colors.White.ColorWithAlpha(0.6f).CGColor;
            _password.Layer.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f).CGColor;
            _password.LeftViewMode = UITextFieldViewMode.Always;
            _password.LeftView = new UIView(new CGRect(0, 0, 15, 15));

            _emailRuler = new UIView { BackgroundColor = Appearance.Colors.LoginRulerColor, Hidden = true };
            _passwordRuler = new UIView { BackgroundColor = Appearance.Colors.LoginRulerColor, Hidden = true };

            _signinButton = new UIButton { Font = Appearance.Fonts.LatoBlackWithSize(15) };

            _signinButton.SetTitleColor(iOS.Appearance.Colors.LoginButtonText, UIControlState.Normal);
            _signinButton.Layer.BorderWidth = 1;
            _signinButton.Layer.BorderColor = Appearance.Colors.White.ColorWithAlpha(0.6f).CGColor;
            _signinButton.Layer.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f).CGColor;

            _registerButton = new UIButton { Font = UIFont.BoldSystemFontOfSize(13) };
            _registerButton.SetTitle("Not a member? Join now!", UIControlState.Normal);
            _registerButton.SetTitleColor(UIColor.FromRGB(207, 0, 88), UIControlState.Normal);
            _registerButton.SetTitleColor(Appearance.Colors.White, UIControlState.Highlighted);

            _forgotPasswordButton = new UIButton { Font = UIFont.SystemFontOfSize(13) };
            _forgotPasswordButton.SetBackgroundImage(UIImage.FromBundle("Icons/forgotpassword_icon.png").ImageWithColor(Appearance.Colors.White), UIControlState.Normal);
            //_forgotPasswordButton.SetTitle("Forgot your password?", UIControlState.Normal);
            //_forgotPasswordButton.SetTitleColor(Appearance.Colors.LoginBottomButtonTextColor, UIControlState.Normal);
            //_forgotPasswordButton.SetTitleColor(Appearance.Colors.White, UIControlState.Highlighted);
            
            View.AddSubviews(_background);
            View.SendSubviewToBack(_background);

            _registerButton.TouchUpInside += RegisterButtonOnTouchUpInside;

            _containerView.AddSubviews(_logo, _indicator, _emailImage, _passwordImage, _email, _password, _emailRuler, _passwordRuler, _signinButton, _registerButton, _forgotPasswordButton);

            ScrollView.AddSubviews(_containerView);
        }

        private void RegisterButtonOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("https://thebridge.spaces.nexudus.com/en/signup"));
        }

        private void SetupConstraints2()
        {
            ScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _containerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _containerView.WithSameWidth(View),

                _background.AtTopOf(View),
                _background.AtLeftOf(View),
                _background.AtRightOf(View),
                _background.AtBottomOf(View)
            );

            ScrollView.AddConstraints(
                _containerView.AtTopOf(ScrollView),
                _containerView.AtLeftOf(ScrollView),
                _containerView.AtRightOf(ScrollView),
                _containerView.AtBottomOf(ScrollView),
                _containerView.Height().EqualTo(UIScreen.MainScreen.Bounds.Height)
            );

            _containerView.AddConstraints(
                _logo.AtTopOf(_containerView, 40),
                _logo.WithSameCenterX(_containerView),

                _indicator.Below(_logo, 30),
                _indicator.WithSameCenterX(_logo),

                _registerButton.AtBottomOf(_containerView, 60),
                _registerButton.WithSameCenterX(_containerView),

                _forgotPasswordButton.AtRightOf(_password, 15),
                _forgotPasswordButton.Height().EqualTo(15),
                _forgotPasswordButton.Width().EqualTo(15),
                _forgotPasswordButton.WithSameCenterY(_password),

                _signinButton.Above(_registerButton, 40),
                _signinButton.Height().EqualTo(40),
                _signinButton.WithSameLeft(_password),
                _signinButton.WithSameRight(_password),

                _passwordRuler.Above(_signinButton, 15),
                _passwordRuler.WithSameLeft(_registerButton),
                _passwordRuler.WithSameRight(_forgotPasswordButton),
                _passwordRuler.Height().EqualTo(0),

                _password.Above(_passwordRuler, 0),
                _password.AtRightOf(_containerView, 15),
                _password.Height().EqualTo(40),
                _password.AtLeftOf(_containerView, 15),

                _passwordImage.WithSameCenterY(_password),
                _passwordImage.WithSameLeft(_passwordRuler),
                _passwordImage.Height().EqualTo(0),
                _passwordImage.Width().EqualTo(0),

                _emailRuler.Above(_password, 15),
                _emailRuler.WithSameLeft(_registerButton),
                _emailRuler.WithSameRight(_forgotPasswordButton),
                _emailRuler.Height().EqualTo(0),

                _email.Above(_emailRuler, 0),
                _email.AtRightOf(_containerView, 15),
                _email.Height().EqualTo(40),
                _email.AtLeftOf(_containerView, 15),

                _emailImage.WithSameCenterY(_email),
                _emailImage.WithSameLeft(_emailRuler),
                _emailImage.Height().EqualTo(0),
                _emailImage.Width().EqualTo(0)
            );
        }

        #endregion CustomAscendas

        /// <summary>
        /// Setup mvvmcross bindings to the viewmodel
        /// </summary>
        private void SetupBindings()
        {
            var set = this.CreateBindingSet<LoginView, LoginViewModel>();

            set.Bind(_email).To(vm => vm.Email);
            set.Bind(_email).For(p => p.AttributedPlaceholder).To(vm => vm.EmailPlaceholder).WithConversion("AttributedPlaceholder");
            set.Bind(_email).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_password).To(vm => vm.Password);
            set.Bind(_password).For(p => p.AttributedPlaceholder).To(vm => vm.PasswordPlaceholder).WithConversion("AttributedPlaceholder");
            set.Bind(_password).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_signinButton).To(vm => vm.LoginCommand);
            set.Bind(_signinButton).For("Title").To(vm => vm.SigninButtonTitle);
            set.Bind(_signinButton).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_indicator).For("ActivityHidden").To(vm => vm.IsBusy).WithConversion("Visibility");

            set.Bind(_emailRuler).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_passwordRuler).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_emailImage).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(_passwordImage).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            // TODO: Aanzetten als het is geimplementeer
            //set.Bind(_registerButton).To(vm => vm.RegisterCommand);
            //set.Bind(_registerButton).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Bind(_forgotPasswordButton).To(vm => vm.ForgotPasswordCommand);
            set.Bind(_forgotPasswordButton).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");

            set.Apply();
        }

        #endregion Setup

        #region Private members

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

        #endregion Private members

        #region Keyboard

        public override bool HandlesKeyboardNotifications => true;

        protected override nfloat KeyboardScrollExtraOffset => 15;

        private void ResignFirstResponder(object sender, EventArgs eventArgs)
        {
            var firstResponder = View.FindFirstResponder();

            if (firstResponder != null)
            {
                firstResponder.ResignFirstResponder();
            }
        }

        #endregion Keyboard

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _containerView = null;
                _emailRuler = null;
                _passwordRuler = null;
                _background = null;
                _logo = null;
                _passwordImage = null;
                _emailImage = null;
                _email = null;
                _password = null;
                _signinButton = null;
                _registerButton = null;
                _forgotPasswordButton = null;
                _indicator = null;
            }
        }

        #endregion Dispose
    }
}
