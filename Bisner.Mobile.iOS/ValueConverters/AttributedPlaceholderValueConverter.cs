using System;
using System.Globalization;
using Foundation;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class AttributedPlaceholderValueConverter : MvxValueConverter<string, NSAttributedString>
    {
        /// <summary>
        /// Fallback value to prevent binding from throwing nullref exceptions
        /// </summary>
        public static readonly NSAttributedString FallBackString = new NSAttributedString(string.Empty, Appearance.Fonts.LatoWithSize(10));

        protected override NSAttributedString Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new NSAttributedString(value, null, Appearance.Colors.LoginInputTextColor);

            return result;
        }
    }
}