using System;
using Bisner.Mobile.Core.Models.Company;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class DashboardCompanyCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("DashboardCompanyCell");

        private BackPanelView _backPanelView;
        private UIImageView _logo;
        private UILabel _name, _industry, _numberOfCoworkers;
        private UIView _horizontalLine;
        private UICollectionView _usersCollection;

        private MvxCollectionViewSource _source;

        public DashboardCompanyCell(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        private void SetupSubViews()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            _backPanelView = new BackPanelView { BackgroundColor = iOS.Appearance.Colors.White };

            _logo = new MvxImageView();
            _logo.Layer.CornerRadius = 12.0f;
            _logo.Layer.BorderWidth = 1;
            _logo.Layer.BorderColor = iOS.Appearance.Colors.RulerColor.CGColor;
            _logo.ClipsToBounds = true;

            _name = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(16), TextColor = iOS.Appearance.Colors.DefaultTextColor };

            _industry = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(13), TextColor = iOS.Appearance.Colors.DefaultTextColor };

            _horizontalLine = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _numberOfCoworkers = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(12), TextColor = iOS.Appearance.Colors.SubTextColor };

            _usersCollection = new UICollectionView(ContentView.Frame, new UICollectionViewFlowLayout { ItemSize = new CGSize(20, 20), ScrollDirection = UICollectionViewScrollDirection.Horizontal, MinimumInteritemSpacing = 0, MinimumLineSpacing = 2 })
            {
                AllowsSelection = true,
                ScrollEnabled = false,
                BackgroundColor = UIColor.Clear,
            };

            _usersCollection.RegisterClassForCell(typeof(CollectionUserCell), CollectionUserCell.Identifier);
            _usersCollection.Source = _source = new MvxCollectionViewSource(_usersCollection, CollectionUserCell.Identifier);

            ContentView.Add(_backPanelView);
            ContentView.Add(_horizontalLine);
            ContentView.Add(_logo);
            ContentView.Add(_name);
            ContentView.Add(_industry);
            ContentView.Add(_numberOfCoworkers);
            ContentView.Add(_usersCollection);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _backPanelView.AtTopOf(ContentView, 20).WithIdentifier("BackPanelViewAtTopOf"),
                _backPanelView.AtLeftOf(ContentView, 20).WithIdentifier("BackPanelViewAtLeftOf"),
                _backPanelView.AtRightOf(ContentView, 20).WithIdentifier("BackPanelViewAtRightOf"),
                _backPanelView.AtBottomOf(ContentView, 20).WithIdentifier("BackPanelViewAtBottomOf"),

                _logo.AtTopOf(_backPanelView, 20).WithIdentifier("LogoAtTopOfBackPanel"),
                _logo.AtLeftOf(_backPanelView, 20).WithIdentifier("LogoAtLeftOrBackPanel"),
                _logo.Height().EqualTo().WidthOf(_logo).WithIdentifier("LogoHeightEqualToWidthLogo"),
                _logo.Width().EqualTo(80).WithIdentifier("LogoWdthEqualTo"),

                _name.ToRightOf(_logo, 20).WithIdentifier("NameToRightOfLogo"),
                _name.WithSameTop(_logo).Plus(2).WithIdentifier("NameWithSameTopLogo"),
                _name.AtRightOf(_backPanelView, 20).WithIdentifier("NameAtRightOfBackPanel"),

                _industry.Below(_name).WithIdentifier("IndustryBelowName"),
                _industry.WithSameLeft(_name).WithIdentifier("IndustryWithSameLeftName"),
                _industry.WithSameRight(_name).WithIdentifier("IndustrySameRightName"),
                _industry.Height().EqualTo(25).WithIdentifier("IndustryHeightEqualTo"),

                _horizontalLine.Below(_logo, 20).WithIdentifier("HorizontalLineBelowLogo"),
                _horizontalLine.Height().EqualTo(1).WithIdentifier("HorzontalLineHeightEqualTo"),
                _horizontalLine.WithSameLeft(_logo).WithIdentifier("HorizontalLineWithSameLeftLogo"),
                _horizontalLine.WithSameRight(_name).WithIdentifier("HorizontalLineSameRightName"),

                _numberOfCoworkers.Below(_horizontalLine, 5).WithIdentifier("NumberCoworkersBelowHorizontalLine"),
                _numberOfCoworkers.WithSameLeft(_horizontalLine).WithIdentifier("NumberCoworkersSameLeftHorizontalLine"),

                _usersCollection.Below(_horizontalLine, 5).WithIdentifier("UserCollBelowHorizontalLine"),
                _usersCollection.WithSameLeft(_logo).WithIdentifier("UserCollSameLeftLogo"),
                _usersCollection.WithSameRight(_industry).WithIdentifier("UserCollSameRightIndustry"),
                _usersCollection.AtBottomOf(_backPanelView, 5).WithIdentifier("UserCollBottomBackPanel"),
                _usersCollection.Height().EqualTo(30).WithIdentifier("UserCollHeightEqualTo")
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DashboardCompanyCell, ICompany>();
                set.Bind(_name).To(vm => vm.Name);
                set.Bind(_logo.Tap()).For(tap => tap.Command).To(vm => vm.ShowCompanyCommand);
                set.Bind(_logo).For("AvatarImageUrl").To(vm => vm.Logo.Medium).WithConversion("ImageUrl");
                set.Bind(_industry).To(vm => vm.Industry);
                set.Bind(_industry.Tap()).For(tap => tap.Command).To(vm => vm.ShowCompanyCommand);
                set.Bind(ContentView.Tap()).For(tap => tap.Command).To(vm => vm.ShowCompanyCommand);
                set.Bind(_source).To(vm => vm.Users);
                set.Apply();
            });
        }

        #endregion Constructor
    }

    public class CollectionUserCell : MvxCollectionViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("CollectionUserCell");

        private AvatarImageView _avatarImage;

        public CollectionUserCell(IntPtr handle)
            : base(handle)
        {
            SetupControls();
            SetupConstraints();
            SetupBindings();
        }

        private void SetupControls()
        {
            BackgroundView = new UIView { BackgroundColor = iOS.Appearance.Colors.White };

            _avatarImage = new AvatarImageView();

            ContentView.AddSubview(_avatarImage);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                    _avatarImage.AtTopOf(ContentView),
                    _avatarImage.AtLeftOf(ContentView),
                    _avatarImage.AtBottomOf(ContentView),
                    _avatarImage.AtRightOf(ContentView)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<CollectionUserCell, IUser>();
                set.Bind(_avatarImage).For("AvatarImageUrl").To(vm => vm.Avatar.Small).WithConversion("ImageUrl");
                set.Apply();
            });
        }

        #endregion Constructor
    }
}