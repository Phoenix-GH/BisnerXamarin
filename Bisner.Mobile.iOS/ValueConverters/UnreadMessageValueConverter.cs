using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class UnreadMessageValueConverter : MvxValueConverter<bool, UIFont>
    {
        public static UIFont FallbackValue = Appearance.Fonts.LatoWithSize(14);

        protected override UIFont Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value)
            {
                return Appearance.Fonts.LatoWithSize(14);
            }

            return Appearance.Fonts.LatoBoldWithSize(14);
        }
    }
}