using System;
using Bisner.Mobile.Core.Models.Feed;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class SelectedImageCell : MvxCollectionViewCell
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("SelectedImageCell");

        private AvatarImageView _imageView;

        public SelectedImageCell(IntPtr handle)
            : base(handle)
        {
            SetupControls();
            SetupConstraints();
            SetupBindings();
        }

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
                    _imageView.AtTopOf(ContentView, 10),
                    _imageView.AtLeftOf(ContentView, 10),
                    _imageView.AtBottomOf(ContentView, 10),
                    _imageView.AtRightOf(ContentView, 10),
                    _imageView.Height().EqualTo(80),
                    _imageView.Width().EqualTo(80)
                );
        }

        private void SetupBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SelectedImageCell, SelectedImage>();
                set.Bind(_imageView).For(i => i.Image).To(item => item.Bytes).WithConversion("ByteToImage");
                set.Apply();
            });
        }

        #endregion Constructor
    }
}
