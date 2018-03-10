using System;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views
{
    [MvxFromStoryboard]
    public partial class ResetPasswordView : KeyboardListenerViewBase<ResetPasswordViewModel>
    {
        private UITextField EmailInput;
        private UIButton ResetButton;
        private UIView ContainerView;
        private UIImageView Logo;
        private UIView EmailRuler;
        private UIImageView EmailImage;
        private UIActivityIndicatorView Indicator;
        private UIButton BackButton;


        public ResetPasswordView(IntPtr handle) : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ContainerView = new UIView { BackgroundColor = UIColor.Clear };
            using (var logoImage = UIImage.FromBundle("Images/bisner_logo_login.png"))
            {
                Logo = new UIImageView(logoImage);
            }
            EmailRuler = new UIView { BackgroundColor = Appearance.Colors.LoginRulerColor };
            using (var emailImage = UIImage.FromBundle("Icons/icon_login_mail.png").ImageWithColor(Appearance.Colors.LoginRulerColor))
            {
                EmailImage = new UIImageView(emailImage);
            }
            Indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White) { Hidden = true };
            Indicator.StartAnimating();
            BackButton = new UIButton();
            BackButton.SetBackgroundImage(UIImage.FromBundle("Icons/icon_back.png").ImageWithColor(Appearance.Colors.LoginInputTextColor), UIControlState.Normal);

            // Login view does not need a navigation bar
            NavigationController.NavigationBarHidden = true;

            if (!Settings.CustomLogin)
            {
                SetupAppearance1();
            }
            else
            {
                SetupAppearance2();
            }
            EmailInput.ShouldReturn = ShouldReturn;
            ScrollView.AddSubview(ContainerView);
            View.AddSubview(BackButton);
            ContainerView.AddSubviews(EmailInput, ResetButton, Logo, EmailRuler, EmailImage, Indicator);
            SetupConstraints();
            SetupBindings();

            ViewToCenterOnKeyboardShown = ResetButton;
            PositionToScrollView = UITableViewScrollPosition.Bottom;
            ScrollView.ScrollEnabled = false;
        }

        private void SetupConstraints()
        {
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContainerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                ContainerView.WithSameWidth(View),
                BackButton.AtTopOf(View, 15),
                BackButton.AtLeftOf(View, 15)
            );

            ScrollView.AddConstraints(
                ContainerView.AtTopOf(ScrollView),
                ContainerView.AtLeftOf(ScrollView),
                ContainerView.AtRightOf(ScrollView),
                ContainerView.AtBottomOf(ScrollView),
                ContainerView.Height().EqualTo(UIScreen.MainScreen.Bounds.Height)
            );

            ContainerView.AddConstraints(
                Logo.AtTopOf(ContainerView, 40),
                Logo.WithSameCenterX(ContainerView),

                Indicator.Below(Logo, 30),
                Indicator.WithSameCenterX(Logo),

                EmailRuler.Above(ResetButton, 0),
                EmailRuler.WithSameLeft(ResetButton),
                EmailRuler.WithSameRight(ResetButton),
                EmailRuler.Height().EqualTo(0),

                EmailInput.Above(EmailRuler, 15),
                EmailInput.AtRightOf(ContainerView, 15),
                EmailInput.Height().EqualTo(40),
                EmailInput.ToRightOf(EmailImage, 0),

                EmailImage.WithSameCenterY(EmailInput),
                EmailImage.WithSameLeft(EmailRuler),
                EmailImage.Height().EqualTo(0),
                EmailImage.Width().EqualTo(0),

                ResetButton.WithSameCenterX(ContainerView),
                ResetButton.AtBottomOf(ContainerView, 80),
                ResetButton.AtLeftOf(ContainerView, 15),
                ResetButton.AtRightOf(ContainerView, 15),
                ResetButton.Height().EqualTo(40)
            );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ResetPasswordView, ResetPasswordViewModel>();
            set.Bind(EmailInput).To(vm => vm.Email);
            set.Bind(EmailInput).For(p => p.AttributedPlaceholder).To(vm => vm.EmailPlaceHolder).WithConversion("AttributedPlaceholder");
            set.Bind(EmailInput).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(ResetButton).To(vm => vm.ResetCommand);
            set.Bind(ResetButton).For("Title").To(vm => vm.ButtonTitle);
            set.Bind(ResetButton).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(Indicator).For("ActivityHidden").To(vm => vm.IsBusy).WithConversion("Visibility");
            set.Bind(Indicator).For(i => i.IsAnimating).To(vm => vm.IsBusy);
            set.Bind(EmailImage).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(EmailRuler).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(BackButton).For("Visibility").To(vm => vm.IsNotBusy).WithConversion("Visibility");
            set.Bind(BackButton).To(vm => vm.BackCommand);
            set.Apply();
        }

        private void SetupAppearance1()
        {
            EmailInput = new TransparentUITextField { Font = UIFont.SystemFontOfSize(14), KeyboardType = UIKeyboardType.EmailAddress, TextColor = Appearance.Colors.LoginInputTextColor };
            ((TransparentUITextField)EmailInput).SetupApearance();
            ResetButton = new LoginButton { Font = Appearance.Fonts.LatoBlackWithSize(15) };
        }

        private void SetupAppearance2()
        {
            EmailInput = new UITextField() { Font = UIFont.SystemFontOfSize(14), KeyboardType = UIKeyboardType.EmailAddress, TextColor = Appearance.Colors.LoginInputTextColor };
            EmailInput.Layer.BorderWidth = 1;
            EmailInput.Layer.BorderColor = Appearance.Colors.White.ColorWithAlpha(0.6f).CGColor;
            EmailInput.Layer.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f).CGColor;
            EmailInput.LeftViewMode = UITextFieldViewMode.Always;
            EmailInput.LeftView = new UIView(new CGRect(0, 0, 15, 15));

            ResetButton = new UIButton { Font = Appearance.Fonts.LatoBlackWithSize(15) };
            ResetButton.SetTitleColor(Appearance.Colors.LoginButtonText, UIControlState.Normal);
            ResetButton.Layer.BorderWidth = 1;
            ResetButton.Layer.BorderColor = Appearance.Colors.White.ColorWithAlpha(0.6f).CGColor;
            ResetButton.Layer.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f).CGColor;
        }

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
    }
}