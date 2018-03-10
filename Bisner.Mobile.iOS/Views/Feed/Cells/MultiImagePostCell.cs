using System;
using System.Collections.Generic;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    partial class MultiImagePostCell : TextPostCellBase<FeedMultiImagePost>
    {
        #region Constructor

        public static readonly NSString Identifier = new NSString("MultiImagePostCell");

        private UIImageView _postImage01, _postImage02, _postImage03;

        public MultiImagePostCell(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region Overrides.

        protected override List<UIView> AddChildControls()
        {
            var image = UIImage.FromBundle("Images/background_login.png");
            _postImage01 = new UIImageView(image)
            {
                ContentMode = UIViewContentMode.ScaleToFill,
            };
            _postImage01.Layer.BorderColor = iOS.Appearance.Colors.RulerColor.CGColor;
            _postImage01.Layer.BorderWidth = 2.0f;

            _postImage02 = new UIImageView(image)
            {
                ContentMode = UIViewContentMode.ScaleToFill,
            };
            _postImage02.Layer.BorderColor = iOS.Appearance.Colors.RulerColor.CGColor;
            _postImage02.Layer.BorderWidth = 2.0f;

            _postImage03 = new UIImageView(image)
            {
                ContentMode = UIViewContentMode.ScaleToFill,
            };
            _postImage03.Layer.BorderColor = iOS.Appearance.Colors.RulerColor.CGColor;
            _postImage03.Layer.BorderWidth = 2.0f;

            return new List<UIView> { _postImage01, _postImage02, _postImage03 };
        }

        protected override List<FluentLayout> AddContentConstraintsBelowText(UIView contentContainer)
        {
            return new List<FluentLayout>
            {
                _postImage01.AtTopOf(contentContainer, 10),
                _postImage01.WithSameLeft(contentContainer),
                _postImage01.WithSameRight(contentContainer),
                _postImage01.Height().EqualTo().WidthOf(_postImage01).WithMultiplier(0.5f),

                _postImage02.Below(_postImage01, 3),
                _postImage02.WithSameLeft(_postImage01),
                _postImage02.Height().EqualTo().WidthOf(_postImage02).WithMultiplier(0.5f),

                _postImage03.WithSameTop(_postImage02),
                _postImage03.ToRightOf(_postImage02, 3),
                _postImage03.WithSameRight(contentContainer),
                _postImage03.Height().EqualTo().WidthOf(_postImage03).WithMultiplier(0.5f),
                _postImage03.WithSameWidth(_postImage02),

                _postImage02.AtBottomOf(contentContainer, 10)
            };
        }

        protected override void AddBindingsToSet(MvxFluentBindingDescriptionSet<FeedPostCellBase<FeedMultiImagePost>, FeedMultiImagePost> set)
        {
            base.AddBindingsToSet(set);


        }

        #endregion Overrides
    }
}
