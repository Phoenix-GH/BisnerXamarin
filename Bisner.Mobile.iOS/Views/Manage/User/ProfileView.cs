using System;
using System.Diagnostics;
using Bisner.Mobile.Core.ViewModels.Manage.User;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Bisner.Mobile.iOS.Views.Base;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.User
{
    partial class ProfileView : KeyboardListenerViewBase<ProfileViewModel>
    {
        #region Constructor

        // Navigation items
        private UIBarButtonItem _updateItem, _updateIndicatorItem;
        private UIActivityIndicatorView _updateIndicator;

        // Containers
        private UIView _containerView, _aboutYouContainer, _profileContainer, _aboutYouTopRuler, _aboutYouBottomRuler, _profileTopRuler, _profileBottomRuler, _headerContainer, _headerBottomRuler;

        // Header
        private UIImageView _backgroundImage;
        private UIImageView _image;
        private UILabel _text;

        // About you
        private UILabel _aboutYouHeader, _displayNameHeader, _email, _emailHeader, _firstNameHeader, _lastNameHeader;
        private InputTextField _displayName, _firstName, _lastName;

        // Profile
        private UILabel _profileHeader, _shortIntroHeader, _aboutHeader, _cityheader, _linkedinHeader, _facebookHeader, _twitterHeader, _googleHeader, _instagramHeader;
        private InputTextField _shortIntro, _about, _city, _linkedin, _facebook, _twitter, _google, _instagram;

        // Preferences
        //private UILabel _preferencesHeader;//, _locationHeader, _languageHeader;
        //private InputTextField _location, _language;

        private NSObject _textFieldChangedNotification;

        public ProfileView(IntPtr handle) : base(handle)
        {
        }

        #endregion Constructor

        #region ViewController

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            GC.Collect();

            Debug.WriteLine("PROFILEVIEW RECIEVED MEMORY WARNING!!");
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
            ScreenName = "ProfileView";
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (IsMovingToParentViewController)
            {
                _textFieldChangedNotification =
                    NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification,
                        TextFieldTextChanged);

                ViewModel.EnableUpdate += OnEnableUpdate;
                ViewModel.StartUpdating += OnStartUpdating;
                ViewModel.StopUpdating += OnStopUpdating;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                _textFieldChangedNotification.Dispose();

                ViewModel.EnableUpdate -= OnEnableUpdate;
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

            // Header
            _headerContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _headerBottomRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            using (var image = UIImage.FromBundle("Images/contact_background.jpg"))
            {
                _backgroundImage = new UIImageView(image) { ClipsToBounds = true };
            }
            
            _image = new UIImageView { ClipsToBounds = true };
            _image.Layer.MasksToBounds = true;
            _image.Layer.CornerRadius = 15f;
            _image.Layer.BorderColor = Appearance.Colors.White.CGColor;
            _image.Layer.ShadowColor = Appearance.Colors.Green.CGColor;
            _image.Layer.ShadowRadius = 5f;
            _image.Layer.BorderWidth = 4;

            _text = new UILabel { Font = Appearance.Fonts.LatoWithSize(20), TextColor = Appearance.Colors.DefaultTextColor };

            // Basic information
            _aboutYouHeader = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Basic information" };

            _aboutYouContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _aboutYouTopRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _displayNameHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Display name" };
            _displayName = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _firstNameHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "First name" };
            _firstName = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _lastNameHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Last name" };
            _lastName = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _emailHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Email" };
            _email = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor };

            _aboutYouBottomRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            // Profile
            _profileHeader = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "About you" };

            _profileContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            _profileTopRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _shortIntroHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Short introcduction" };
            _shortIntro = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _aboutHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "About" };
            _about = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _cityheader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "City" };
            _city = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _linkedinHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "LinkedIn url" };
            _linkedin = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _facebookHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Facebook url" };
            _facebook = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _twitterHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Twitter url" };
            _twitter = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _googleHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Google+ url" };
            _google = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _instagramHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Instagram url" };
            _instagram = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            _profileBottomRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            // Preferences
            //_preferencesHeader = new UILabel { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.SubTextColor, Text = "Platform settings" };

            //_preferencesContainer = new UIView { BackgroundColor = Appearance.Colors.White };
            //_preferencesTopRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            //_locationHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Select your location" };
            //_location = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            //_languageHeader = new UILabel { Font = Appearance.Fonts.LatoBoldWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, Text = "Select your language" };
            //_language = new InputTextField { Font = Appearance.Fonts.LatoWithSize(14), TextColor = Appearance.Colors.DefaultTextColor, EdgeInsets = new UIEdgeInsets(0, 5, 0, 0), ShouldReturn = ShouldReturn };

            //_preferencesBottomRuler = new UIView { BackgroundColor = Appearance.Colors.RulerColor };

            _containerView.AddSubviews(_headerContainer,
                _aboutYouContainer,
                _backgroundImage,
                _text,
                _image,
                _headerBottomRuler,
                _aboutYouTopRuler,
                _aboutYouHeader,
                _displayNameHeader,
                _displayName,
                _firstName,
                _firstNameHeader,
                _lastNameHeader,
                _lastName,
                _email,
                _emailHeader,
                _aboutYouBottomRuler,
                _profileContainer,
                _profileHeader,
                _profileTopRuler,
                _profileBottomRuler,
                _shortIntro,
                _about,
                _city,
                _linkedin,
                _facebook,
                _twitter,
                _google,
                _instagram,
                _shortIntroHeader,
                _aboutHeader,
                _cityheader,
                _linkedinHeader,
                _facebookHeader,
                _twitterHeader,
                _googleHeader,
                _instagramHeader);

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
                _containerView.WithSameWidth(View)
            );

            ScrollView.AddConstraints(
                _containerView.AtTopOf(ScrollView),
                _containerView.AtLeftOf(ScrollView),
                _containerView.AtRightOf(ScrollView),
                _containerView.AtBottomOf(ScrollView)
            );

            _containerView.AddConstraints(
                // Header
                _headerContainer.AtTopOf(_containerView),
                _headerContainer.AtLeftOf(_containerView),
                _headerContainer.AtRightOf(_containerView),

                _backgroundImage.AtLeftOf(_headerContainer),
                _backgroundImage.AtRightOf(_headerContainer),
                _backgroundImage.AtTopOf(_headerContainer),
                _backgroundImage.Height().EqualTo(200),

                _image.WithSameCenterX(_headerContainer),
                _image.AtTopOf(_headerContainer, 150),
                _image.Width().EqualTo(100),
                _image.Height().EqualTo(100),

                _text.Below(_image, 20),
                _text.WithSameCenterX(_image),

                _headerBottomRuler.Below(_text, 30),
                _headerBottomRuler.AtBottomOf(_headerContainer),
                _headerBottomRuler.AtLeftOf(_headerContainer),
                _headerBottomRuler.AtRightOf(_headerContainer),
                _headerBottomRuler.Height().EqualTo(1),

                // Basic information
                _aboutYouHeader.Below(_headerBottomRuler, 15),
                _aboutYouHeader.AtLeftOf(_containerView, 14),
                _aboutYouHeader.AtRightOf(_containerView, 14),

                _aboutYouContainer.Below(_aboutYouHeader, 5),
                _aboutYouContainer.AtLeftOf(_containerView),
                _aboutYouContainer.AtRightOf(_containerView),

                _aboutYouTopRuler.AtTopOf(_aboutYouContainer),
                _aboutYouTopRuler.AtLeftOf(_aboutYouContainer),
                _aboutYouTopRuler.AtRightOf(_aboutYouContainer),
                _aboutYouTopRuler.Height().EqualTo(1),

                _displayNameHeader.Below(_aboutYouTopRuler, 15),
                _displayNameHeader.AtLeftOf(_aboutYouContainer, 14),
                _displayNameHeader.AtRightOf(_aboutYouContainer, 14),

                _displayName.Below(_displayNameHeader, 15),
                _displayName.AtLeftOf(_aboutYouContainer, 14),
                _displayName.AtRightOf(_aboutYouContainer, 14),
                _displayName.Height().EqualTo(40),

                _firstNameHeader.Below(_displayName, 15),
                _firstNameHeader.AtLeftOf(_aboutYouContainer, 14),
                _firstNameHeader.AtRightOf(_aboutYouContainer, 14),

                _firstName.Below(_firstNameHeader, 15),
                _firstName.AtLeftOf(_aboutYouContainer, 14),
                _firstName.AtRightOf(_aboutYouContainer, 14),
                _firstName.Height().EqualTo(40),

                _lastNameHeader.Below(_firstName, 15),
                _lastNameHeader.AtLeftOf(_aboutYouContainer, 14),
                _lastNameHeader.AtRightOf(_aboutYouContainer, 14),

                _lastName.Below(_lastNameHeader, 15),
                _lastName.AtLeftOf(_aboutYouContainer, 14),
                _lastName.AtRightOf(_aboutYouContainer, 14),
                _lastName.Height().EqualTo(40),

                _emailHeader.Below(_lastName, 15),
                _emailHeader.AtLeftOf(_aboutYouContainer, 14),
                _emailHeader.AtRightOf(_aboutYouContainer, 14),

                _email.Below(_emailHeader, 15),
                _email.AtLeftOf(_aboutYouContainer, 14),
                _email.AtRightOf(_aboutYouContainer, 14),
                _email.Height().EqualTo(40),

                _aboutYouBottomRuler.Below(_email, 15),
                _aboutYouBottomRuler.AtBottomOf(_aboutYouContainer),
                _aboutYouBottomRuler.AtLeftOf(_aboutYouContainer),
                _aboutYouBottomRuler.AtRightOf(_aboutYouContainer),
                _aboutYouBottomRuler.Height().EqualTo(1),

                // Profile
                _profileHeader.Below(_aboutYouBottomRuler, 15),
                _profileHeader.AtRightOf(_profileContainer, 14),
                _profileHeader.AtLeftOf(_profileContainer, 14),

                _profileContainer.Below(_profileHeader, 5),
                _profileContainer.AtLeftOf(_containerView),
                _profileContainer.AtRightOf(_containerView),

                _profileTopRuler.AtTopOf(_profileContainer),
                _profileTopRuler.AtLeftOf(_profileContainer),
                _profileTopRuler.AtRightOf(_profileContainer),
                _profileTopRuler.Height().EqualTo(1),

                _shortIntroHeader.Below(_profileTopRuler, 15),
                _shortIntroHeader.AtLeftOf(_profileContainer, 14),
                _shortIntroHeader.AtRightOf(_profileContainer, 14),

                _shortIntro.Below(_shortIntroHeader, 15),
                _shortIntro.AtRightOf(_profileContainer, 14),
                _shortIntro.AtLeftOf(_profileContainer, 14),
                _shortIntro.Height().EqualTo(40),

                _aboutHeader.Below(_shortIntro, 15),
                _aboutHeader.AtLeftOf(_profileContainer, 14),
                _aboutHeader.AtRightOf(_profileContainer, 14),

                _about.Below(_aboutHeader, 15),
                _about.AtRightOf(_profileContainer, 14),
                _about.AtLeftOf(_profileContainer, 14),
                _about.Height().EqualTo(40),

                _cityheader.Below(_about, 15),
                _cityheader.AtLeftOf(_profileContainer, 14),
                _cityheader.AtRightOf(_profileContainer, 14),

                _city.Below(_cityheader, 15),
                _city.AtRightOf(_profileContainer, 14),
                _city.AtLeftOf(_profileContainer, 14),
                _city.Height().EqualTo(40),

                _linkedinHeader.Below(_city, 15),
                _linkedinHeader.AtLeftOf(_profileContainer, 14),
                _linkedinHeader.AtRightOf(_profileContainer, 14),

                _linkedin.Below(_linkedinHeader, 15),
                _linkedin.AtRightOf(_profileContainer, 14),
                _linkedin.AtLeftOf(_profileContainer, 14),
                _linkedin.Height().EqualTo(40),

                _facebookHeader.Below(_linkedin, 15),
                _facebookHeader.AtLeftOf(_profileContainer, 14),
                _facebookHeader.AtRightOf(_profileContainer, 14),

                _facebook.Below(_facebookHeader, 15),
                _facebook.AtRightOf(_profileContainer, 14),
                _facebook.AtLeftOf(_profileContainer, 14),
                _facebook.Height().EqualTo(40),

                _twitterHeader.Below(_facebook, 15),
                _twitterHeader.AtLeftOf(_profileContainer, 14),
                _twitterHeader.AtRightOf(_profileContainer, 14),

                _twitter.Below(_twitterHeader, 15),
                _twitter.AtRightOf(_profileContainer, 14),
                _twitter.AtLeftOf(_profileContainer, 14),
                _twitter.Height().EqualTo(40),

                _googleHeader.Below(_twitter, 15),
                _googleHeader.AtLeftOf(_profileContainer, 14),
                _googleHeader.AtRightOf(_profileContainer, 14),

                _google.Below(_googleHeader, 15),
                _google.AtRightOf(_profileContainer, 14),
                _google.AtLeftOf(_profileContainer, 14),
                _google.Height().EqualTo(40),

                _instagramHeader.Below(_google, 15),
                _instagramHeader.AtLeftOf(_profileContainer, 14),
                _instagramHeader.AtRightOf(_profileContainer, 14),

                _instagram.Below(_instagramHeader, 15),
                _instagram.AtRightOf(_profileContainer, 14),
                _instagram.AtLeftOf(_profileContainer, 14),
                _instagram.Height().EqualTo(40),

                _profileBottomRuler.Below(_instagram, 15),
                _profileBottomRuler.AtBottomOf(_profileContainer),
                _profileBottomRuler.AtLeftOf(_profileContainer),
                _profileBottomRuler.AtRightOf(_profileContainer),
                _profileBottomRuler.Height().EqualTo(1),


                _profileContainer.AtBottomOf(_containerView, 15)

                // Preferences
                //_preferencesHeader.Below(_profileBottomRuler, 15),
                //_preferencesHeader.AtRightOf(_preferencesContainer, 14),
                //_preferencesHeader.AtLeftOf(_preferencesContainer, 14),

                //_preferencesContainer.Below(_preferencesHeader, 5),
                //_preferencesContainer.AtLeftOf(_containerView),
                //_preferencesContainer.AtRightOf(_containerView),
                //_preferencesContainer.AtBottomOf(_containerView, 15),

                //_preferencesTopRuler.AtTopOf(_preferencesContainer),
                //_preferencesTopRuler.AtLeftOf(_preferencesContainer),
                //_preferencesTopRuler.AtRightOf(_preferencesContainer),
                //_preferencesTopRuler.Height().EqualTo(1),

                //_locationHeader.Below(_preferencesTopRuler, 15),
                //_locationHeader.AtLeftOf(_profileContainer, 14),
                //_locationHeader.AtRightOf(_profileContainer, 14),

                //_location.Below(_locationHeader, 15),
                //_location.AtRightOf(_preferencesContainer, 14),
                //_location.AtLeftOf(_preferencesContainer, 14),
                //_location.Height().EqualTo(40),

                //_languageHeader.Below(_location, 15),
                //_languageHeader.AtLeftOf(_profileContainer, 14),
                //_languageHeader.AtRightOf(_profileContainer, 14),

                //_language.Below(_languageHeader, 15),
                //_language.AtRightOf(_preferencesContainer, 14),
                //_language.AtLeftOf(_preferencesContainer, 14),
                //_language.Height().EqualTo(40),

                //_preferencesBottomRuler.Below(_language, 15),
                //_preferencesBottomRuler.AtBottomOf(_preferencesContainer),
                //_preferencesBottomRuler.AtLeftOf(_preferencesContainer),
                //_preferencesBottomRuler.AtRightOf(_preferencesContainer),
                //_preferencesBottomRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            set.Bind(_image).For("ImageUrl").To(vm => vm.AvatarUrl).WithConversion("ImageUrl");
            set.Bind(_image.Tap()).For(t => t.Command).To(vm => vm.ChangeAvatarCommand);
            set.Bind(_text).To(vm => vm.DisplayName);
            set.Bind(_updateItem).For("Title").To(vm => vm.UpdateButtonText);

            // Profile
            set.Bind(_displayNameHeader).To(vm => vm.DisplayNameText);
            set.Bind(_displayName).To(vm => vm.DisplayName);
            set.Bind(_firstNameHeader).To(vm => vm.FirstnameText);
            set.Bind(_firstName).To(vm => vm.FirstName);
            set.Bind(_lastNameHeader).To(vm => vm.LastnameText);
            set.Bind(_lastName).To(vm => vm.LastName);
            set.Bind(_emailHeader).To(vm => vm.EmailText);
            set.Bind(_email).To(vm => vm.Email);

            // About
            set.Bind(_aboutYouHeader).To(vm => vm.AboutYouText);
            set.Bind(_shortIntroHeader).To(vm => vm.ShortIntroText);
            set.Bind(_shortIntro).To(vm => vm.ShortIntro);
            set.Bind(_aboutHeader).To(vm => vm.AboutTex);
            set.Bind(_about).To(vm => vm.About);
            set.Bind(_cityheader).To(vm => vm.CityText);
            set.Bind(_city).To(vm => vm.City);
            set.Bind(_linkedin).To(vm => vm.LinkedInUrl);
            set.Bind(_facebook).To(vm => vm.FacebookUrl);
            set.Bind(_twitter).To(vm => vm.TwitterUrl);
            set.Bind(_google).To(vm => vm.GoogleUrl);
            set.Bind(_instagram).To(vm => vm.InstagramUrl);

            // Preferences
            //set.Bind(_location).To(vm => vm.SelectedLocation.Name);
            //set.Bind(_language).To(vm => vm.SelectedLanguage);

            set.Bind(_updateItem).To(vm => vm.UpdateCommand);

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

        protected override bool EnableTitleBarLogo => true;

        protected override bool EnableCustomBackButton => true;

        #endregion Base Modifications
    }
}
