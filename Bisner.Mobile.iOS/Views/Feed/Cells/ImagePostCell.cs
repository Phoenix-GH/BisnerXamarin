using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class ImagePostCell : TextPostCellBase<FeedImagePost>
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("ImagePostCell");

        private UIImageView _postImage;

        public ImagePostCell(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Overrides.

        protected override List<UIView> AddChildControls()
        {
            var image = UIImage.FromBundle("Images/background_login.png");
            _postImage = new UIImageView(image)
            {
                ContentMode = UIViewContentMode.ScaleToFill,
            };
            _postImage.Layer.BorderColor = iOS.Appearance.Colors.RulerColor.CGColor;
            _postImage.Layer.BorderWidth = 1.0f;

            return new List<UIView> { _postImage };
        }

        protected override List<FluentLayout> AddContentConstraintsBelowText(UIView contentContainer)
        {
            return new List<FluentLayout>
            {
                _postImage.AtTopOf(contentContainer, 10),
                _postImage.WithSameLeft(contentContainer),
                _postImage.WithSameRight(contentContainer),
                _postImage.Height().EqualTo().WidthOf(_postImage).WithMultiplier(0.5f),
                _postImage.AtBottomOf(contentContainer, 10)
            };
        }

        protected override void AddBindingsToSet(MvxFluentBindingDescriptionSet<FeedPostCellBase<FeedImagePost>, FeedImagePost> set)
        {
            base.AddBindingsToSet(set);


        }

        #endregion Overrides
    }
}
