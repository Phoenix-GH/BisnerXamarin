using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.Core.ValueConverters
{
    /// <summary>
    /// Invert boolean value converter
    /// </summary>
    public class BooleanInvertValueConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }

        protected override bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
