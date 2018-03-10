using System;
using Bisner.Mobile.Core.Models.General.User;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Chat.Cells
{
    partial class ContactCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("ContactCell");

        private UIView _topRuler, _bottomRuler;
        private AvatarImageView _avatar;
        private UILabel _displayName;

        public ContactCell(IntPtr handle)
            : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _topRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };
            _bottomRuler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            _avatar = new AvatarImageView();

            _displayName = new UILabel { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14), TextColor = iOS.Appearance.Colors.DefaultTextColor };

            ContentView.Add(_topRuler);
            ContentView.Add(_bottomRuler);
            ContentView.Add(_avatar);
            ContentView.Add(_displayName);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _topRuler.AtTopOf(ContentView),
                _topRuler.AtRightOf(ContentView, 0),
                _topRuler.AtLeftOf(ContentView, 0),
                _topRuler.Height().EqualTo(1),

                _bottomRuler.AtBottomOf(ContentView),
                _bottomRuler.AtRightOf(ContentView),
                _bottomRuler.AtLeftOf(ContentView),
                _bottomRuler.Height().EqualTo(1),

                _avatar.AtTopOf(ContentView, 10),
                _avatar.AtLeftOf(ContentView, 10),
                _avatar.Height().EqualTo(50),
                _avatar.Width().EqualTo(50),
                _avatar.AtBottomOf(ContentView, 10),

                _displayName.WithSameCenterY(_avatar),
                _displayName.ToRightOf(_avatar, 10)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ContactCell, IUser>();
                set.Bind(_avatar).For("AvatarImageUrl").To(vm => vm.Avatar.Small).WithConversion("ImageUrl");
                set.Bind(_displayName).To(vm => vm.DisplayName);
                set.Apply();
            });
        }

        #endregion Setup

        #region Overrides

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            BackgroundColor = iOS.Appearance.Colors.White;
        }

        #endregion Overrides
    }
}
