using System;
using Bisner.Mobile.Core.ViewModels.Manage;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Manage.Cells
{
    public class ManageLabelCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("ManageLabelCell");

        private UILabel _text;
        private UIView _ruler;

        public ManageLabelCell(IntPtr handle) : base(handle)
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

            _text = new UILabel { Font = iOS.Appearance.Fonts.LatoWithSize(14), TextColor = iOS.Appearance.Colors.SubTextColor };
            _ruler = new UIView { BackgroundColor = iOS.Appearance.Colors.RulerColor };

            BackgroundColor = iOS.Appearance.Colors.BackgroundColor;

            ContentView.AddSubviews(_text, _ruler);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _text.AtLeftOf(ContentView, 14),
                _text.AtRightOf(ContentView, 14),
                _text.WithSameCenterY(ContentView),

                _ruler.AtTopOf(ContentView),
                _ruler.AtRightOf(ContentView, 14),
                _ruler.AtLeftOf(ContentView, 14),
                _ruler.Height().EqualTo(1),

                ContentView.Height().EqualTo(30)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ManageLabelCell, ManageLabel>();
                set.Bind(_text).To(vm => vm.Text);
                set.Apply();
            });
        }

        #endregion Setup
    }
}