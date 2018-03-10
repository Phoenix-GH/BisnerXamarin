using System;
using Foundation;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class ByteToImageValueConverter : MvxValueConverter<byte[], UIImage>
    {
        protected override UIImage Convert(byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var imageData = NSData.FromArray(value);

            var image = UIImage.LoadFromData(imageData);

            return image;
        }
    }
}