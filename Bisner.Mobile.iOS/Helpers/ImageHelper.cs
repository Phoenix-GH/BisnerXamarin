using CoreGraphics;
using Google.Analytics;
using UIKit;

namespace Bisner.Mobile.iOS.Helpers
{
    public class ImageHelper
    {
        public static UIImage GetSinglePixelBackgroundImage(UIColor color)
        {
            var rect = new CGRect(0.0f, 0.0f, 1.0f, 1.0f);
            UIGraphics.BeginImageContext(rect.Size);

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(color.CGColor);
                context.FillRect(rect);

                var image = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return image;
            }
        }
    }
}
