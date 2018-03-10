using System;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.Cells
{
    public class ManageUserCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("ManageUserCell");

        private AvatarImageView _imageView;
        private UILabel _text;
        private UIImageView _arrow;
        private UIView _topRuler;

        public ManageUserCell(IntPtr handle) : base(handle)
        {
            SetupViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupViews()
        {
            // Turn off selection!!!!!
            SelectionStyle = UITableViewCellSelectionStyle.None;

            _imageView = new AvatarImageView();

            _text = new UILabel { TextColor = iOS.Appearance.Colors.DefaultTextColor, Font = iOS.Appearance.Fonts.LatoBoldWithSize(14) };

            using (var arrow = UIImage.FromBundle("Icons/arrow_right_light.png").ImageWithColor(UIColor.FromRGB(197, 197, 197)))
            {
                _arrow = new UIImageView(arrow);
            }

            _topRuler = new UIView
            {
                BackgroundColor = iOS.Appearance.Colors.RulerColor
            };

            ContentView.BackgroundColor = iOS.Appearance.Colors.White;

            ContentView.AddSubviews(_imageView, _text, _arrow, _topRuler);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _imageView.AtLeftOf(ContentView, 14),
                _imageView.AtTopOf(ContentView, 10),
                _imageView.AtBottomOf(ContentView, 10),
                _imageView.Width().EqualTo(30),
                _imageView.Height().EqualTo(30),

                _text.ToRightOf(_imageView, 10),
                _text.WithSameCenterY(_imageView),
                _text.ToLeftOf(_arrow, 10),

                _arrow.WithSameCenterY(ContentView),
                _arrow.AtRightOf(ContentView, 14),
                _arrow.Height().EqualTo(15),
                _arrow.Width().EqualTo(9),

                _topRuler.AtTopOf(ContentView),
                _topRuler.AtLeftOf(ContentView),
                _topRuler.AtRightOf(ContentView),
                _topRuler.Height().EqualTo(1)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ManageUserCell, ManageUser>();
                set.Bind(_imageView).For("ImageUrl").To(vm => vm.ImageUrl).WithConversion("ImageUrl");
                set.Bind(_text).To(vm => vm.Text);
                set.Apply();
            });
        }

        #endregion Setup
    }
}
