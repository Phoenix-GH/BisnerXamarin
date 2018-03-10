using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    public class FeedButton : UIButton
    {
        private int _imageTopSpacing;
        private int _imageBottomSpacing;

        private int _imageTitleSpacing;

        public FeedButton()
        {
            SetupAppearance();
        }

        public FeedButton(IntPtr handle)
            : base(handle)
        {
            SetupAppearance();
        }

        private void SetupAppearance()
        {
            SetTitleColor(iOS.Appearance.Colors.FeedButtonNormalTextColor, UIControlState.Normal);
            Font = iOS.Appearance.Fonts.LatoWithSize(14);
            _imageTitleSpacing = 7;
            ImageEdgeInsets = new UIEdgeInsets(ImageTopSpacing, 0, ImageBottomSpacing, _imageTitleSpacing);
            TitleEdgeInsets = new UIEdgeInsets(0, _imageTitleSpacing, 0, 0);

            LineBreakMode = UILineBreakMode.TailTruncation;
            TitleLabel.Lines = 1;
        }

        public int ImageTopSpacing
        {
            get { return _imageTopSpacing; }
            set
            {
                _imageTopSpacing = value;
                ImageEdgeInsets = new UIEdgeInsets(ImageTopSpacing, 0, ImageBottomSpacing, _imageTitleSpacing);
            }
        }

        public int ImageBottomSpacing
        {
            get { return _imageBottomSpacing; }
            set
            {
                _imageBottomSpacing = value;
                ImageEdgeInsets = new UIEdgeInsets(ImageTopSpacing, 0, ImageBottomSpacing, _imageTitleSpacing);
            }
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                var currentSize = base.IntrinsicContentSize;
                currentSize.Width += ImageEdgeInsets.Right + TitleEdgeInsets.Left;
                return currentSize;
            }
        }
    }
}
