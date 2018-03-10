using System;
using System.Linq;
using Bisner.Mobile.Core;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.Models.Dashboard;
using Bisner.Mobile.Core.Service;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Dashboard.Cells
{
    public class GroupCell : MvxTableViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("GroupCell");

        private OverlayImageView _background;
        private UILabel _name;
        public UILabel GroupDescription;
        private UIButton _viewButton;
        private UIView _ruler;

        public GroupCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _background = new OverlayImageView { BackgroundColor = UIColor.Black, ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, Transparency = 0.5f };

            _name = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoBlackWithSize(22), Lines = 2, LineBreakMode = UILineBreakMode.TailTruncation };
            GroupDescription = new UILabel { TextColor = iOS.Appearance.Colors.White, Font = iOS.Appearance.Fonts.LatoWithSize(13), Lines = 2 };
            _viewButton = new UIButton { Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };
            _viewButton.SetTitleColor(iOS.Appearance.Colors.Green, UIControlState.Normal);
            _viewButton.Layer.CornerRadius = 20f; // height / 2
            _viewButton.Layer.BorderColor = iOS.Appearance.Colors.Green.CGColor;
            _viewButton.Layer.BorderWidth = 1f;

            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            ContentView.AddSubviews(_background, _name, GroupDescription, _viewButton, _ruler);

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            _buttonCenterConstraint = _viewButton.WithSameCenterX(ContentView).ToLayoutConstraints().First();
            _buttonRightConstraint = _viewButton.AtRightOf(ContentView, 14).ToLayoutConstraints().First();

            ContentView.AddConstraints(
                _background.AtLeftOf(ContentView),
                _background.AtRightOf(ContentView),
                _background.AtTopOf(ContentView),
                _background.AtBottomOf(ContentView),

                _name.AtLeftOf(ContentView, 14),
                _name.AtRightOf(ContentView, 14),
                _name.AtTopOf(ContentView, 30),

                GroupDescription.Below(_name, 14),
                GroupDescription.AtLeftOf(ContentView, 14),
                GroupDescription.AtRightOf(ContentView, 14),

                _viewButton.AtBottomOf(ContentView, 25),
                _viewButton.Width().EqualTo(130),
                _viewButton.Height().EqualTo(40),

                _ruler.AtBottomOf(ContentView),
                _ruler.AtLeftOf(ContentView),
                _ruler.AtRightOf(ContentView),
                _ruler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<GroupCell, IGroup>();
                set.Bind(_name).To(vm => vm.Name);
                // This is set in the view for scroll performance
                //set.Bind(GroupDescription).For(l => l.AttributedText).To(vm => vm.Description).WithConversion("LineHeightAttributed");
                set.Bind(_background).For("ImageUrl").To(vm => vm.Header.Medium).WithConversion("ImageUrl");
                set.Bind(_viewButton).To(vm => vm.ShowGroupCommand);
                set.Bind(_viewButton).For("Title").To(vm => vm.ButtonText);
                set.Apply();
            });
        }

        #endregion Setup

        #region Modifications

        private NSLayoutConstraint _buttonCenterConstraint, _buttonRightConstraint;

        public void SetIsFirstCell(bool isFirst)
        {
            if (isFirst)
            {
                _background.Hidden = false;
                _name.TextColor = iOS.Appearance.Colors.White;
                GroupDescription.TextColor = iOS.Appearance.Colors.White;
                _viewButton.BackgroundColor = iOS.Appearance.Colors.Green;
                _viewButton.SetTitleColor(iOS.Appearance.Colors.White, UIControlState.Normal);
                _buttonRightConstraint.Active = false;
                _buttonCenterConstraint.Active = true;
                //_ruler.Hidden = true;
            }
            else
            {
                _background.Hidden = true;
                _name.TextColor = iOS.Appearance.Colors.BisnerBlue;
                GroupDescription.TextColor = iOS.Appearance.Colors.ChatMessageColor;
                _viewButton.BackgroundColor = UIColor.Clear;
                _viewButton.SetTitleColor(iOS.Appearance.Colors.Green, UIControlState.Normal);
                _buttonCenterConstraint.Active = false;
                _buttonRightConstraint.Active = true;
                _ruler.Hidden = false;
            }
        }

        #endregion Modifications
    }
}