using System;
using Bisner.Mobile.Core.ViewModels.Manage;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.Cells
{
    public class ManageItemCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("ManageItemCell");

        private UILabel _text;
        private UIView _ruler;
        private UIImageView _arrowImage;
        private UIView _bottomRuler;

        public ManageItemCell(IntPtr handle) : base(handle)
        {
            SetupViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupViews()
        {
            _text = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(15), TextColor = iOS.Appearance.Colors.DefaultTextColor };
            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            using (
                var arrow =
                    UIImage.FromBundle("Icons/arrow_right_light.png").ImageWithColor(UIColor.FromRGB(197, 197, 197)))
            {
                _arrowImage = new UIImageView(arrow);
            }

            _bottomRuler = new UIView { BackgroundColor = UIColor.Clear };

            ContentView.AddSubviews(_text, _ruler, _arrowImage, _bottomRuler);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _text.AtLeftOf(ContentView, 14),
                _text.WithSameCenterY(ContentView),

                _ruler.AtTopOf(ContentView),
                _ruler.AtRightOf(ContentView),
                _ruler.AtLeftOf(ContentView),
                _ruler.Height().EqualTo(1),

                _arrowImage.AtRightOf(ContentView, 14),
                _arrowImage.Height().EqualTo(15),
                _arrowImage.Width().EqualTo(9),
                _arrowImage.WithSameCenterY(ContentView),

                _text.AtRightOf(_arrowImage, 5),

                _bottomRuler.AtBottomOf(ContentView),
                _bottomRuler.AtLeftOf(ContentView),
                _bottomRuler.AtRightOf(ContentView),
                _bottomRuler.Height().EqualTo(1),

                ContentView.Height().EqualTo(50)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ManageItemCell, ManageItem>();
                set.Bind(_text).To(vm => vm.Text);
                set.Apply();
            });
        }

        #endregion Setup

        #region Appearance

        public void SetBottomRuler(bool enabled)
        {
            _bottomRuler.BackgroundColor = enabled ? iOS.Appearance.Colors.RulerColor : UIColor.Clear;
        }

        #endregion Appearance
    }
}