using System;
using Bisner.Mobile.Core.Models.General;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.iOS.Views.Gestures;
using UIKit;

namespace Bisner.Mobile.iOS.Views.General.Cells
{
    public class ImageCollectionCell : MvxCollectionViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("ImageCollectionCell");

        private AvatarImageView _imageView;

        public ImageCollectionCell(IntPtr handle)
            : base(handle)
        {
            SetupControls();
            SetupConstraints();
            SetupBindings();
        }

        #endregion Constructor

        #region Setup

        private void SetupControls()
        {
            BackgroundView = new UIView { BackgroundColor = iOS.Appearance.Colors.White };

            _imageView = new AvatarImageView();

            ContentView.AddSubview(_imageView);
        }

        private void SetupConstraints()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            ContentView.AddConstraints(
                    _imageView.AtTopOf(ContentView),
                    _imageView.AtLeftOf(ContentView),
                    _imageView.AtBottomOf(ContentView),
                    _imageView.AtRightOf(ContentView)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ImageCollectionCell, IImage>();
                set.Bind(_imageView).For("ImageUrl").To(vm => vm.Small).WithConversion("ImageUrl");
                set.Bind(_imageView.Tap()).For(tap => tap.Command).To(vm => vm.ShowCommand);
                set.Apply();
            });
        }

        #endregion Setup

        #region Modifications

        public nfloat CornerRadius
        {
            get { return _imageView.CornerRadius; }
            set { _imageView.CornerRadius = value; }
        }

        #endregion Modifications
    }
}
