using System;
using System.Linq;
using Bisner.Mobile.iOS.Extensions;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Controls
{
    [Register("OverlayImageView")]
    public class OverlayImageView : UIImageView
    {
        #region UIImageView

        public OverlayImageView()
        {
        }

        public OverlayImageView(IntPtr handle) : base(handle)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!Subviews.Any())
            {
                this.SetOverlay(Transparency);
            }

            foreach (var subview in Subviews)
            {
                var frame = subview.Frame;

                frame.Width = Frame.Width;
                frame.Height = Frame.Height;

                subview.Frame = frame;
            }
        }

        #endregion UIImageView

        #region Properties

        /// <summary>
        /// Value between 0 and 1 to set overlay transparency
        /// </summary>
        public nfloat Transparency { get; set; }

        #endregion Propeties
    }
}