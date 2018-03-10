using System;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    partial class AvatarImageView : UIImageView
    {
        public AvatarImageView(IntPtr handle)
            : base(handle)
        {
            CornerRadius = 4.0f;
        }

        public AvatarImageView(UIImage image)
            : base(image)
        {
            CornerRadius = 4.0f;
        }

        public AvatarImageView()
        {
            CornerRadius = 4.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Layer.CornerRadius = CornerRadius;
            Layer.MasksToBounds = false;
            Layer.BorderWidth = 0;
            ClipsToBounds = true;
            ContentMode = UIViewContentMode.ScaleAspectFill;
        }

        public nfloat CornerRadius { get; set; }
    }
}
