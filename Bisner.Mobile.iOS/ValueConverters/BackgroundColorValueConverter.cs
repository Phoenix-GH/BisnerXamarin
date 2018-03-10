using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class BackgroundColorValueConverter : MvxValueConverter<bool, UIColor>
    {
        protected override UIColor Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = parameter as UIColor;

            if (color == null)
            {
                throw new Exception("Unable to cast object to UIColor");
            }

            return value ? UIColor.Clear : color;
        }
    }
}