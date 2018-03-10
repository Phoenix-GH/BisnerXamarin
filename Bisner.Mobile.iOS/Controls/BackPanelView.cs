using System;
using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    partial class BackPanelView : UIView
    {
        public BackPanelView()
        {
            _borderWidth = 1.0f;
            _cornerRadius = 12.0f;
        }

        public BackPanelView(IntPtr handle)
            : base(handle)
        {
            _borderWidth = 2.0f;
            _cornerRadius = 12.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Layer.CornerRadius = _cornerRadius;
            Layer.BorderColor = iOS.Appearance.Colors.BackPanelBorderTop.CGColor;
            Layer.BorderWidth = _borderWidth;
            Layer.ShadowRadius = 3.0f;
            Layer.ShadowColor = iOS.Appearance.Colors.BackPanelShadow.CGColor;
            Layer.ShadowOffset = new CGSize(0f, 0.5f);
            Layer.ShadowOpacity = 0.3f;
        }

        private nfloat _cornerRadius;
        private nfloat _borderWidth;

        public nfloat CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                Layer.CornerRadius = value;
            }
        }

        public nfloat BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = value;
                Layer.BorderWidth = value;
            }
        }
    }
}
