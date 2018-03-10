using System;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    public class CompanyFeedCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("CompanyFeedCell");

        private UIImageView _header;
        private UIImageView _logo;
        public UILabel Name, Industry;
        private UIView _headerContainer, _headerBottomRuler, _logoShadowView;

        // About
        private UIView _aboutContainer, _aboutTopRuler, _aboutMiddleRuler, _aboutBottomRuler;//, _socialContainer;
        public UILabel AboutTitle, AboutText, ContactTitle, Telephone, SpaceLocation, Website;
        private UIImageView _telephoneImage, _locationImage, _websiteImage;
        //private UIButton _facebook, _twitter, _linkedIn, _instagram;

        public CompanyFeedCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            ContentView.BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            _header = new UIImageView { ContentMode = UIViewContentMode.ScaleAspectFill, BackgroundColor = UIColor.Black, ClipsToBounds = true };
            _logo = new AvatarImageView();

            _logoShadowView = new UIView { BackgroundColor = UIColor.White };
            _logoShadowView.Layer.BorderWidth = 2;
            _logoShadowView.Layer.ShadowRadius = 12f;
            _logoShadowView.Layer.ShadowColor = UIColor.Black.CGColor;
            _logoShadowView.Layer.ShadowOffset = new CGSize(0f, 0);
            _logoShadowView.Layer.ShadowOpacity = 0.55f;
            _logoShadowView.Layer.CornerRadius = 16.0f;

            _logo = new UIImageView { ClipsToBounds = true, BackgroundColor = iOS.Appearance.Colors.White };
            _logo.Layer.BorderWidth = 3;
            _logo.Layer.BorderColor = iOS.Appearance.Colors.White.CGColor;
            _logo.Layer.CornerRadius = 16f;
            
            Name = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(25), TextColor = iOS.Appearance.Colors.ChatMessageColor };
            Industry = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.UserCardSubText };
            _headerContainer = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _headerBottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.BackPanelBorderBottom };

            _aboutContainer = new UIView { BackgroundColor = iOS.Appearance.Colors.White };
            _aboutTopRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _aboutMiddleRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _aboutBottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            AboutTitle = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            ContactTitle = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            AboutText = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor, Lines = 0 };
            Telephone = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            SpaceLocation = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            Website = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };

            using (var telephoneImage = UIImage.FromBundle("Icons/icon_telephone.png"))
            {
                _telephoneImage = new UIImageView(telephoneImage);
            }

            using (var locationImage = UIImage.FromBundle("Icons/icon_location.png"))
            {
                _locationImage = new UIImageView(locationImage);
            }

            using (var websiteImage = UIImage.FromBundle("Icons/icon_website.png"))
            {
                _websiteImage = new UIImageView(websiteImage);
            }

            //_socialContainer = new UIView();

            //using (var facebookImage = UIImage.FromBundle("Icons/icon_facebook.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            //{
            //    _facebook = new UIButton();
            //    _facebook.SetBackgroundImage(facebookImage, UIControlState.Normal);
            //}

            //using (var twitterImage = UIImage.FromBundle("Icons/icon_twitter.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            //{
            //    _twitter = new UIButton();
            //    _twitter.SetBackgroundImage(twitterImage, UIControlState.Normal);
            //}

            //using (var linkedInImage = UIImage.FromBundle("Icons/icon_linkedin.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            //{
            //    _linkedIn = new UIButton();
            //    _linkedIn.SetBackgroundImage(linkedInImage, UIControlState.Normal);
            //}

            //using (var instagramImage = UIImage.FromBundle("Icons/icon_instagram.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal))
            //{
            //    _instagram = new UIButton();
            //    _instagram.SetBackgroundImage(instagramImage, UIControlState.Normal);
            //}

            ContentView.AddSubviews(
                    _headerContainer,
                    _header,
                    _logoShadowView,
                    _logo,
                    Name,
                    Industry,
                    _headerBottomRuler,
                    _aboutContainer,
                    _aboutTopRuler,
                    _aboutMiddleRuler,
                    _aboutBottomRuler,
                    AboutTitle,
                    AboutText,
                    ContactTitle,
                    Telephone,
                    SpaceLocation,
                    Website,
                    _telephoneImage,
                    _locationImage,
                    _websiteImage
                    //_facebook,
                    //_twitter,
                    //_linkedIn,
                    //_instagram,
                    //_socialContainer
                );
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _headerContainer.AtTopOf(ContentView),
                _headerContainer.AtLeftOf(ContentView),
                _headerContainer.AtRightOf(ContentView),

                _header.AtTopOf(_headerContainer),
                _header.AtLeftOf(_headerContainer),
                _header.AtRightOf(_headerContainer),
                _header.Height().EqualTo(280),

                _logoShadowView.WithSameCenterY(_logo),
                _logoShadowView.WithSameCenterX(_logo),
                _logoShadowView.Height().EqualTo(110),
                _logoShadowView.Width().EqualTo(110),

                _logo.WithSameBottom(_header).Plus(34),
                _logo.WithSameCenterX(_header),
                _logo.Height().EqualTo(112),
                _logo.Width().EqualTo(112),

                Name.Below(_logo, 23),
                Name.WithSameCenterX(_headerContainer),

                Industry.WithSameCenterX(_headerContainer),
                Industry.Below(Name, 6),

                _headerBottomRuler.Below(_header, 140),
                _headerBottomRuler.AtBottomOf(_headerContainer),
                _headerBottomRuler.AtLeftOf(_headerContainer),
                _headerBottomRuler.AtRightOf(_headerContainer),
                _headerBottomRuler.Height().EqualTo(1),

                _aboutContainer.Below(_headerContainer, 5),
                _aboutContainer.AtLeftOf(ContentView),
                _aboutContainer.AtRightOf(ContentView),
                _aboutContainer.AtBottomOf(ContentView, 5),

                _aboutTopRuler.AtTopOf(_aboutContainer),
                _aboutTopRuler.AtLeftOf(_aboutContainer),
                _aboutTopRuler.AtRightOf(_aboutContainer),
                _aboutTopRuler.Height().EqualTo(1),

                AboutTitle.Below(_aboutTopRuler, 10),
                AboutTitle.AtLeftOf(_aboutContainer, 14),
                AboutTitle.AtRightOf(_aboutContainer, 14),

                AboutText.Below(AboutTitle, 10),
                AboutText.WithSameLeft(AboutTitle),
                AboutText.WithSameRight(AboutTitle),

                ContactTitle.Below(AboutText, 10),
                ContactTitle.WithSameLeft(AboutText),
                ContactTitle.WithSameRight(AboutText),

                _telephoneImage.Below(ContactTitle, 10),
                _telephoneImage.Height().EqualTo(15),
                _telephoneImage.Width().EqualTo(15),
                _telephoneImage.WithSameLeft(ContactTitle),

                Telephone.ToRightOf(_telephoneImage, 10),
                Telephone.WithSameCenterY(_telephoneImage),
                Telephone.WithSameRight(AboutText),

                _locationImage.Below(_telephoneImage, 10),
                _locationImage.Height().EqualTo(15),
                _locationImage.Width().EqualTo(15),
                _locationImage.WithSameLeft(ContactTitle),

                SpaceLocation.ToRightOf(_locationImage, 10),
                SpaceLocation.WithSameCenterY(_locationImage),
                SpaceLocation.WithSameRight(AboutText),

                _websiteImage.Below(_locationImage, 10),
                _websiteImage.Height().EqualTo(15),
                _websiteImage.Width().EqualTo(15),
                _websiteImage.WithSameLeft(ContactTitle),

                Website.ToRightOf(_websiteImage, 10),
                Website.WithSameCenterY(_websiteImage),
                Website.WithSameRight(AboutText),

                //_facebook.Height().EqualTo(25),
                //_facebook.Width().EqualTo(25),
                //_facebook.AtLeftOf(_socialContainer),

                //_twitter.ToRightOf(_facebook, 5),
                //_twitter.WithSameWidth(_facebook),
                //_twitter.WithSameHeight(_facebook),
                //_twitter.WithSameCenterY(_facebook),

                //_linkedIn.ToRightOf(_twitter, 5),
                //_linkedIn.WithSameWidth(_facebook),
                //_linkedIn.WithSameHeight(_facebook),
                //_linkedIn.WithSameCenterY(_facebook),

                //_instagram.ToRightOf(_linkedIn, 5),
                //_instagram.WithSameWidth(_facebook),
                //_instagram.WithSameHeight(_facebook),
                //_instagram.WithSameCenterY(_facebook),
                //_instagram.AtRightOf(_socialContainer),

                //_socialContainer.WithSameCenterX(ContentView),

                _aboutBottomRuler.Below(Website, 15),
                _aboutBottomRuler.AtBottomOf(_aboutContainer),
                _aboutBottomRuler.AtLeftOf(_aboutContainer),
                _aboutBottomRuler.AtRightOf(_aboutContainer),
                _aboutBottomRuler.Height().EqualTo(1)
            );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<CompanyFeedCell, CompanyFeedItem>();
                set.Bind(_header).For("ImageUrl").To(vm => vm.HeaderUrl).WithConversion("ImageUrl");
                set.Bind(_logo).For("ImageUrl").To(vm => vm.LogoUrl).WithConversion("ImageUrl");
                set.Bind(Name).To(vm => vm.Name);
                set.Bind(Industry).To(vm => vm.Industry);
                set.Bind(AboutTitle).To(vm => vm.AboutTitle);
                set.Bind(AboutText).To(vm => vm.About);
                set.Bind(ContactTitle).To(vm => vm.ContactTitle);
                set.Bind(Telephone).To(vm => vm.Telephone);
                set.Bind(SpaceLocation).To(vm => vm.Location);
                set.Bind(Website).To(vm => vm.Website);
                //set.Bind(_facebook).To(vm => vm.FacebookCommand);
                //set.Bind(_facebook).For("Visibility").To(vm => vm.CanShowFacebook).WithConversion("Visibility");
                //set.Bind(_twitter).To(vm => vm.TwitterCommand);
                //set.Bind(_twitter).For("Visibility").To(vm => vm.CanShowTwitter).WithConversion("Visibility");
                //set.Bind(_linkedIn).To(vm => vm.LinkedInCommand);
                //set.Bind(_linkedIn).For("Visibility").To(vm => vm.CanShowLinkedIn).WithConversion("Visibility");
                //set.Bind(_instagram).To(vm => vm.InstagramCommand);
                //set.Bind(_instagram).For("Visibility").To(vm => vm.CanShowInstagram).WithConversion("Visibility");
                set.Apply();
            });
        }

        #endregion Setup
    }
}