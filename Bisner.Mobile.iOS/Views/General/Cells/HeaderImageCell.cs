using System;
using System.Linq;
using Bisner.Mobile.Core.ViewModels.Dashboard;
using Bisner.Mobile.iOS.Controls;
using Bisner.Mobile.iOS.Extensions;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.General.Cells
{
    public class HeaderImageCell : MvxTableViewCell
    {
        #region Constructor

        public static NSString Identifier = new NSString("HeaderImageCell");

        private OverlayImageView _background;
        private UILabel _title;

        public HeaderImageCell(IntPtr handle) : base(handle)
        {
            SetupSubViews();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupSubViews()
        {
            _background = new OverlayImageView { ContentMode = UIViewContentMode.ScaleAspectFill, ClipsToBounds = true, Transparency = 0.4f };
            _title = new UILabel { Font = iOS.Appearance.Fonts.LatoBlackWithSize(26), TextColor = iOS.Appearance.Colors.White, Lines = 0, TextAlignment = UITextAlignment.Center };

            ContentView.AddSubviews(_background, _title);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                _background.AtTopOf(ContentView),
                _background.AtLeftOf(ContentView),
                _background.AtRightOf(ContentView),
                _background.AtBottomOf(ContentView, 5),

                _title.WithSameCenterY(ContentView),
                _title.AtLeftOf(ContentView, 14),
                _title.AtRightOf(ContentView, 14)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<HeaderImageCell, HeaderImage>();
                set.Bind(_background).For("ImageUrl").To(vm => vm.Image).WithConversion("ImageUrl");
                set.Bind(_title.Tap()).For(tap => tap.Command).To(vm => vm.SelectedCommand);
                set.Bind(ContentView.Tap()).For(tap => tap.Command).To(vm => vm.SelectedCommand);
                set.Bind(_background.Tap()).For(tap => tap.Command).To(vm => vm.SelectedCommand);
                set.Bind(_title).To(vm => vm.TitleText);
                set.Apply();
            });
        }

        #endregion Setup

        #region Properties

        public UIFont Font { get { return _title.Font; } set { _title.Font = value; } }

        #endregion Properties
    }
}