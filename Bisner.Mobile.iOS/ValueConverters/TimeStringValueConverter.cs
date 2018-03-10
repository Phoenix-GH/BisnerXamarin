using System;
using System.Globalization;
using Bisner.Mobile.Core.Extensions;
using MvvmCross.Platform.Converters;

namespace Bisner.Mobile.iOS.ValueConverters
{
    public class TimeStringValueConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToLocalTime().ToSimpleTimeString();
        }
    }
}