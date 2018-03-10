using CoreGraphics;
using UIKit;

namespace Bisner.Mobile.iOS.Extensions
{
    public static class UiImageExtensions
    {
        public static UIImage ImageWithColor(this UIImage image, UIColor color1)
        {
            UIGraphics.BeginImageContextWithOptions(image.Size, false, image.CurrentScale);

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.TranslateCTM(0, image.Size.Height);
                context.ScaleCTM(1.0f, -1.0f);
                context.SetBlendMode(CGBlendMode.Normal);
                var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);
                context.ClipToMask(rect, image.CGImage);
                color1.SetFill();
                context.FillRect(rect);
                var newImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                image.Dispose();
                color1.Dispose();

                return newImage;
            }
        }
    }
}